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
using TaoBaoRequest;
using System.Net;
using System.IO;

namespace FCatch
{
    public partial class DataCatchLog : Form
    {
        Socket socket;
        string user;
        public DataCatchLog(string user)
        {
            InitializeComponent();
            this.user = user;
            
            socket = IO.Socket("http://localhost:8080");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Data data = new Data(user);
                socket.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));
                SendMsgToNode("已接入抓取接口，准备发起抓取请求");
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
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            textBox1.Text = string.Format("{0}\r\n{1}", text, textBox1.Text);
            SendMsgToNode(text);
            if (text.EndsWith("(finish)"))
            {
                SendMsgToNode("数据抓取已完成，已转移到数据分析操作");
                SocketClose();
                SendToAnalysis();
                this.Close();
            }
        }

        private void SendToAnalysis()
        {
            string url = "http://localhost:9613/Test001/Test001.Login.ajax/BillCatch";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            string data = "{\"user\":\"" + user + "\"}";
            request.ContentLength = data.Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gbk")))
            {
                writer.Write(data);
                writer.Flush();
            }
        }

        private void SendMsgToNode(string text)
        {
            if (socket == null)
            {
                return;
            }
            SendMsg msg = new SendMsg(this.user);
            msg.touid = this.user;
            msg.msg = text;
            socket.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(msg));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(this.textBox1.Text);
        }
    }
}
