using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Fiddler;
using TaoBaoRequest;
using System.Net;
using System.IO;
using Carpa.Web.Ajax;

namespace FCatch
{
    public partial class DataCatch : Form
    {
        bool isRuning = false;
        FiddlerCatch fdCatch = new FiddlerCatch();
        public DataCatch()
        {
            InitializeComponent();
        }

        private void buttonCatch_Click(object sender, EventArgs e)
        {
            //GetBill(new HTTPRequestHeaders());
            //return;
            if (isRuning)
            {
                fdCatch.Quit();
                isRuning = false;
                buttonCatch.Text = "开始抓取";
            }
            else
            {
                string url = this.textBoxUrl.Text;
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show("请先设置URL");

                    return;
                }
                Thread thread = new Thread(fdCatch.Start);
                fdCatch.OnGetCatchData += (sessions) =>
                {
                    if (sessions.Count == 0)
                    {
                        return;
                    }
                    Session session = sessions[sessions.Count - 1];
                    this.Invoke(new AsynUpdateUI((sn) =>
                    {
                        if (session.fullUrl.Equals("https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8"))
                        {
                            Thread thread1 = new Thread(GetBill);
                            thread1.Start(session.RequestHeaders);
                        }
                        else
                        {
                            this.textBox1.Text = string.Format("url:{0}\r\n{1}", session.fullUrl, session.RequestHeaders);
                        }
                    }), session);
                };
                thread.Start(url);
                isRuning = true;
                buttonCatch.Text = "停止抓取";
            }
        }

        private void GetBill(object obj)
        {
            try
            {

                HTTPRequestHeaders header = (HTTPRequestHeaders)obj;
                //string url = "http://120.25.122.148/test/Test001/Test001.Login.ajax/BillCatch";
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
                string nData = "{\"cookie\":\"" + header["Cookie"] + "\"}";
                byte[] bytes = Encoding.Default.GetBytes(nData);
                request.ContentLength = bytes.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
                {
                    string rv = reader.ReadToEnd();// string.Format("{0}\r\n{1}", header["Cookie"], reader.ReadToEnd());
                    this.Invoke(new AsynUpdateUI((sn) =>
                    {
                        this.textBox1.Text = sn.ToString();
                    }), rv);
                }
            }
            catch (Exception e)
            {
                this.Invoke(new AsynUpdateUI((sn) =>
                {
                    this.textBox1.Text = sn.ToString();
                }), e.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TranResult result = new TranResult();
            result.ShowDialog();
        }
    }
    delegate void AsynUpdateUI(object session);
}
