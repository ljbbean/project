﻿using System;
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

namespace FCatch
{
    public partial class DataCatchLog : Form
    {
        private Socket socket;
        private string user;
        private string toUser;
        private bool socketConnected = false;
        private string socketUrl = "http://localhost:8080";
        private string analysisUrl = "http://localhost:9613/Test001/Test001.Login.ajax/BillCatch";
        private ulong postDataCuid = 0;
        private object listData = null;

        public DataCatchLog(string user)
        {
            InitializeComponent();
            toUser = user;
            user = string.Format("{0}_{1}", user, this.GetHashCode());
            this.user = user;
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
                    listData = null;
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
                MessageBox.Show("未启动socket");
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            this.Invoke(new AsynUpdateUI((sn) =>
            {
                textBox1.Text = string.Format("{0}\r\n{1}", sn, textBox1.Text);
            }), text);
            
            SendMsgToNode(text);
            if (postDataCuid != 0)
            {
                SendMsgToNode("数据抓取已完成，已发起服务器分析请求");
                SendToAnalysis();
                SocketClose();
                //this.Close();
            }
        }

        private void SendToAnalysis()
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
            HashObject hash = new HashObject();
            hash.Add("user", toUser);
            hash.Add("key", postDataCuid);
            hash.Add("dataList", listData);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string data = serializer.Serialize(hash);
            request.ContentLength = data.Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gbk")))
            {
                int length = 10000;
                int index = 0;
                int dataLength = data.Length;
                while (dataLength > index)
                {
                    writer.Write(data.ToCharArray(index, length));
                    index += length;
                    writer.Flush();
                }
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
        internal bool EmitPostDataRequestMsg(object data)
        {
            if (socket == null)
            {
                SendMessage(string.Format("下载数据为:{0}", JavaScriptSerializer.CreateInstance().Serialize(data)));
                return false;
            }
            listData = data;
            SendMsg msg = new SendMsg(this.user);
            msg.touid = "net_server";
            msg.msg = msg.GetHashCode().ToString();
            socket.Emit("postDataRequest", JavaScriptSerializer.CreateInstance().Serialize(msg));
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listData = new HashObject();
            SendToAnalysis();
            //EmitPostDataRequestMsg("dd");
            //SendMessage("结算抓取  (finish)");
        }
    }
}
