using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Test001
{
    public class CacthConfig
    {
        private static Dictionary<string, CacthConfig> catchDic = new Dictionary<string, CacthConfig>();
        internal static Dictionary<string, CacthConfig> CatchDic { get { return catchDic; } }

        private string cookies;
        /// <summary>
        /// 新的cookies， 由于之前的正在进行，这个cookies未做数据拉取
        /// </summary>
        private string newCookies;
        private string user;
        private string errorMessage;

        public CacthConfig()
        {
            DateTime temp = DateTime.Now.AddDays(-2);
            StartDate = new DateTime(temp.Year, temp.Month, temp.Day);
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 执行Cookies
        /// </summary>
        public string Cookies
        {
            get
            {
                return cookies;
            }
            set
            {
                cookies = value;
                DataCatch dataCatch = new DataCatch();
                user = dataCatch.GetUser(value);
            }
        }

        /// <summary>
        /// 新的cookies
        /// </summary>
        public string NewCookies
        {
            get
            {
                return newCookies;
            }
            set
            {
                newCookies = value;
            }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        public string User
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        /// 是否正在抓取
        /// </summary>
        public bool? IsCacthing { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                IsCacthing = false;
            }
        }

        /// <summary>
        /// 10分钟自动下载一次
        /// </summary>
        internal static void DataCatch()
        {
            do
            {
                Thread.Sleep(10 * 1000);
                //Thread.Sleep(60 * 1000 * 10);
                DataCatch dataCatch = new DataCatch();
                foreach (CacthConfig value in catchDic.Values)
                {
                    NetDataCatch(dataCatch, value);
                }
            } while (true);
        }
         
        internal static object NetDataCatch(DataCatch dataCatch, CacthConfig config)
        {
            if (config.IsCacthing != null && config.IsCacthing.Value)
            {
                return "之前的抓取正在进行";
            }

            config.IsCacthing = true;

            try
            {
                string list = string.Format("列表插入数据：{0}", dataCatch.GetData(config.StartDate, config.Cookies));
                SendDetailState detailState = new SendDetailState(ShowMessage);
                dataCatch.GetDetailsData(config.Cookies, detailState);
                config.StartDate = DateTime.Now.AddMinutes(-1);
                return list;
            }
            catch (Exception t)
            {
                config.ErrorMessage = t.Message;
                return t;
            }
        }

        private static void ShowMessage(string message)
        {
            if (message.StartsWith("下载出错"))
            {
                int sindex = message.IndexOf("【");
                int eindex = message.IndexOf("】");
                string user = message.Substring(sindex + 1, eindex - sindex - 1);
                CacthConfig.CatchDic[user].ErrorMessage = message;
                return;
            }
            if (message.EndsWith("(finish)"))
            {
                int sindex = message.IndexOf("【");
                int eindex = message.IndexOf("】");
                string user = message.Substring(sindex + 1, eindex - sindex - 1);
                CacthConfig config = CacthConfig.CatchDic[user];
                try
                {
                    DataCatchSave.SaveData();
                }
                catch (Exception e)
                {
                    config.ErrorMessage = e.Message;
                }
                finally
                {
                    config.IsCacthing = false;
                    config.Cookies = CacthConfig.CatchDic[user].NewCookies;
                }
            }
        }
    }
}