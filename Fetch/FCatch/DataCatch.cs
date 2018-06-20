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
using TaoBaoRequestFCatch;
using System.Net;
using System.IO;
using Carpa.Web.Ajax;
using System.Collections;
using Carpa.Configuration;

namespace FCatch
{
    public partial class DataCatch : Form
    {
        Dictionary<CacthConfig, DataCatchLog> logWindows = new Dictionary<CacthConfig, DataCatchLog>();
        private static object data = new object();
        List<string> listMsg = new List<string>();
        bool isRuning = false;
        FiddlerCatch fdCatch = new FiddlerCatch();
        private string socketUrl = "http://localhost:8080";
        private string analysisUrl = "http://localhost:6369/Test001/Test001.Login.ajax/";
        
        public DataCatch()
        {
            InitializeComponent();
            string tsocketUrl = AppSettings.GetString("socketUrl");
            if (!string.IsNullOrEmpty(tsocketUrl))
            {
                socketUrl = tsocketUrl;
            }
            string tanalysisUrl = AppSettings.GetString("analysisUrl");
            if (!string.IsNullOrEmpty(tanalysisUrl))
            {
                analysisUrl = tanalysisUrl;
            }
        }

        private void buttonCatch_Click(object sender, EventArgs e)
        {
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
                            string keyMsg = GetTOUIDFromWindowTitle(session);
                            string[] item = keyMsg.Split('_');
                            //7829标记下载后需要保存到数据库
                            GetBill(item[0], item.Length == 2 ? !"7829".Equals(item[1]) : false, session.RequestHeaders);
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

        private string GetTOUIDFromWindowTitle(Session session)
        {
            string browerTitle = System.Diagnostics.Process.GetProcessById(session.LocalProcessID).MainWindowTitle;
            int index = browerTitle.IndexOf('-');
            if (index >= 0)
            {
                browerTitle = browerTitle.Substring(0, index).Trim();
            }

            return browerTitle;
        }

        private void GetBill(string touid, bool isDemo, object obj)
        {
            try
            {
                HTTPRequestHeaders header = (HTTPRequestHeaders)obj;
                string cookie = header["Cookie"];
                string user = DataCatchRequest.GetUser(cookie);
                string key = string.Format("{0}&{1}", user, touid);
                CacthConfig config;
                if (!CacthConfig.CatchDic.TryGetValue(key, out config))
                {
                    config = new CacthConfig(string.Format("{0}GetBillBeginValue", analysisUrl)) { Cookies = cookie };
                    CacthConfig.CatchDic.Add(key, config);
                }
                config.NewCookies = cookie;

                DataCatchLog log = GetCatchLog(user, isDemo, config);
                log.SetToUser(touid);
                AnsyNet.AnsyDataCatch(config, (tuser, msg) =>
                {
                    if (!string.IsNullOrEmpty(msg.Message))
                    {
                        UpdateUI(log, msg.Message);
                    }
                    switch (msg.Action)
                    {
                        case ActionType.SendRequestData:
                            log.EmitPostDataRequestMsg((IList)msg.Data);
                            break;
                    }
                    return msg;
                });
            }
            catch (Exception e)
            {
                this.Invoke(new AsynUpdateUI((sn) =>
                {
                    this.textBox1.Text = sn.ToString();
                }), e.Message);
            }
        }

        private void UpdateUI(DataCatchLog log, object msg)
        {
            //异步更新UI
            this.Invoke(new AsynUpdateUI((newMsg) =>
            {
                log.SendMessage(string.Format("{0} {1} {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), newMsg));
            }), msg);
        }

        private DataCatchLog GetCatchLog(string user, bool isDemo, CacthConfig config)
        {
            string tempUrl = string.Format("{0}{1}", analysisUrl, isDemo?"BillCatchDemo":"BillCatch");
            DataCatchLog log;
            if (!logWindows.TryGetValue(config, out log))
            {
                log = new DataCatchLog(user, socketUrl, tempUrl);
                logWindows.Add(config, log);
            }
            if (log.IsDisposed)
            {
                logWindows.Remove(config);
                log = new DataCatchLog(user, socketUrl, tempUrl);
                logWindows.Add(config, log);
            }
            log.Show();
            return log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TranResult result = new TranResult();
            result.ShowDialog();
        }
    }
    delegate void AsynUpdateUI(object session);
}
