using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;
using Carpa.Web.Ajax;
using Common;
using System.Net;
using System.IO;
using Carpa.Web.Script;
using System.Collections;
using System.Threading;

namespace FCatch
{
    public partial class DataCatchLog : Form
    {
        private Socket socket;
        private string user;
        private string toUser;
        private string sUser;
        private bool socketConnected = false;
        private ulong postDataCuid = 0;
        private IList listData = null;
        private string socketUrl;
        private string analysisUrl;

        public DataCatchLog(string user, string socketUrl, string analysisUrl)
        {
            this.socketUrl = socketUrl;
            this.analysisUrl = analysisUrl;
            sUser = user;
            InitializeComponent();
            toUser = user;
            user = string.Format("{0}_{1}", user, this.GetHashCode());
            this.user = user;
            InitSocket(user);
        }

        internal void SetToUser(string touid)
        {
            toUser = touid;
        }

        private void InitSocket(string user)
        {
            socket = IO.Socket(socketUrl);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                if (socket == null)
                {
                    socket = IO.Socket(socketUrl);
                }
                socketConnected = true;
                Data data = new Data(user);
                data.needAsk = false;
                socket.Emit("login", serializer.Serialize(data));
                SendMsgToNode("已接入抓取接口，准备发起抓取请求");
            });
            socket.On("postDataSure", (data) =>
            {
                try
                {
                    HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                    postDataCuid = hash.GetValue<ulong>("msg");
                    SendMessage("服务器验证通过，准备接收数据");
                }
                catch (Exception e)
                {
                    postDataCuid = 0;
                    SendMessage(e.Message);
                }
                finally
                {
                    postDataCuid = 0;
                    //listData = null;
                }
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            SocketClose();
        }

        private void SocketClose()
        {
            if (socket != null)
            {
                socket.Disconnect();
                socket = null;
            }
        }

        public void SetTitle(string title)
        {
            this.Text = title; 
        }

        public void SendMessage(string text)
        {
            if (!socketConnected)
            {
                InitSocket(sUser);
                SendMsgToNode(text);
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (!this.IsDisposed)
            {
                this.Invoke(new AsynUpdateUI((sn) =>
                {
                    if (!this.IsDisposed)
                    {
                        textBox1.Text = string.Format("{0}\r\n{1}", sn, textBox1.Text);
                    }
                }), text);
            }
            
            SendMsgToNode(text);
            if (postDataCuid != 0)
            {
                Send();
                Thread.Sleep(1000);
                SocketClose();
                FormClose();
            }
        }

        private void Send()
        {
            int length = 100;//一次性请求发送100个订单
            if (listData.Count == 0)
            {
                SendMsgToNode("此时间段没有订单信息");
                return;
            }
            else
            {
                SendMsgToNode("数据抓取已完成，已发起服务器分析请求");
            }
            for (int i = 0; i < listData.Count; i = i + length)
            {
                List<object> newData = new List<object>();
                for (int j = 0; j < length && i + j < listData.Count; j++)
                {
                    newData.Add(listData[i + j]);
                }
                SendToAnalysis(newData);
            }
        }

        private void FormClose()
        {
            this.Invoke(new AsynUpdateUI((sn) =>
            {
                this.Close();
            }), "");
        }

        private void SendToAnalysis(object billData)
        {
            string url = analysisUrl;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            request.Headers.Add("X-JSONFormat", "true");
            HashObject hash = new HashObject();
            hash.Add("user", toUser);
            hash.Add("key", postDataCuid);
            hash.Add("dataList", billData);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            string data = serializer.Serialize(hash, JavaScriptSerializer.SerializationFormat.JSON);
            byte[] bs = Encoding.UTF8.GetBytes(data);
            request.ContentLength = bs.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Flush();
            }
        }

        private void SendMsgToNode(string text)
        {
            if (socket == null)
            {
                return;
            }
            SendMsg msg = new SendMsg(this.user);
            msg.touid = toUser;
            msg.msg = text;
            socket.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(msg));
        }

        /// <summary>
        /// 发送确认消息
        /// </summary>
        internal bool EmitPostDataRequestMsg(IList data)
        {
            if (data.Count == 0)
            {
                SendMsgToNode("此时间段没有订单信息");
                FormClose();
                return true;
            }
            if (socket == null)
            {
                SendMessage(string.Format("下载数据为:{0}", JavaScriptSerializer.CreateInstance().Serialize(data)));
                return false;
            }
            listData = data;
            SendMsg msg = new SendMsg(this.user);
            msg.touid = "net_server";
            msg.msg = ((IList)data).Count.ToString();
            socket.Emit("postDataRequest", JavaScriptSerializer.CreateInstance().Serialize(msg));
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Send();
        }
    }
}
