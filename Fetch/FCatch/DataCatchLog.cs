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
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            socket.Disconnect();
            socket = null;
        }

        public void SetTitle(string title)
        {
            this.Text = title; 
            SendMsgToNode("已接入抓取接口，准备发起抓取请求");
        }

        public void SendMessage(string text)
        {
            textBox1.Text = string.Format("{0}\r\n{1}", text, textBox1.Text);
            SendMsgToNode(text);
            if (text.EndsWith("(finish)"))
            {
                SendMsgToNode("数据抓取已完成，已转移到数据分析操作，请等待");
                this.Close();
            }
        }

        private void SendMsgToNode(string text)
        {
            SendMsg msg = new SendMsg(this.user);
            msg.touid = this.user;
            msg.msg = text;
            socket.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(msg));
        }

        public class Data
        {
            private string _uid;
            public Data(string uid)
            {
                _uid = uid;
            }

            public string uid
            {
                get
                {
                    return string.Format("TBCatch_{0}", this._uid);
                }
            }
        }

        public class SendMsg : Data
        {
            public SendMsg(string uid)
                : base(uid)
            {

            }
            public string touid { get; set; }
            public string msg { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(this.textBox1.Text);
        }
    }
}
