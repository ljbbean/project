﻿using System;
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
        Dictionary<CacthConfig, DataCatchLog> logWindows = new Dictionary<CacthConfig, DataCatchLog>();
        private static object data = new object();
        List<string> listMsg = new List<string>();
        bool isRuning = false;
        FiddlerCatch fdCatch = new FiddlerCatch();
        
        public DataCatch()
        {
            InitializeComponent();
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
                            GetBill(session.RequestHeaders);
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
                string cookie = header["Cookie"];
                string user = DataCatchRequest.GetUser(cookie);

                CacthConfig config;
                if (!CacthConfig.CatchDic.TryGetValue(user, out config))
                {
                    config = new CacthConfig() { Cookies = cookie };
                    CacthConfig.CatchDic.Add(user, config);
                }
                config.NewCookies = cookie;

                DataCatchLog log = GetCatchLog(user, config);
                CacthConfig.AnsyDataCatch(config, (tuser, msg) =>
                {
                    this.Invoke(new AsynUpdateUI((sn) =>
                    {
                        log.SendMessage(string.Format("{0} {1} {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), sn));
                    }), msg);

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

        private DataCatchLog GetCatchLog(string user, CacthConfig config)
        {
            DataCatchLog log;
            if (!logWindows.TryGetValue(config, out log))
            {
                log = new DataCatchLog(user);
                logWindows.Add(config, log);
            }
            if (log.IsDisposed)
            {
                logWindows.Remove(config);
                log = new DataCatchLog(user);
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
