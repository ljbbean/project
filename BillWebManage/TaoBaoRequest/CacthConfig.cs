using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Carpa.Logging;
using System.Net;
using System.IO;
using System.Text;

namespace TaoBaoRequest
{
    public class CacthConfig
    {
        private static Dictionary<string, CacthConfig> catchDic = new Dictionary<string, CacthConfig>();
        public static Dictionary<string, CacthConfig> CatchDic { get { return catchDic; } }

        private string cookies;
        /// <summary>
        /// 新的cookies， 由于之前的正在进行，这个cookies未做数据拉取
        /// </summary>
        private string newCookies;
        private string user;
        private string errorMessage;

        public CacthConfig()
        {
            DateTime temp = DateTime.Now.AddDays(-3);
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
                user = TaoBaoRequest.DataCatchRequest.GetUser(value);
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
        /// 当前消息
        /// </summary>
        public string CurrentMessage { get; set; }

        /// <summary>
        /// 10分钟自动下载一次
        /// </summary>
        public static void DataCatch(string connectString)
        {
            do
            {
                Thread.Sleep(60 * 1000 * 10);
                DataCatchRequest dataCatch = new DataCatchRequest(connectString);
                foreach (CacthConfig value in catchDic.Values)
                {
                    NetDataCatch(dataCatch, value);
                }
            } while (true);
        }
         
        public static object NetDataCatch(DataCatchRequest dataCatch, CacthConfig config, InvokeMessage notifyMsg = null)
        {
            notifyMsg = notifyMsg == null ? (tuser, msg) => { return msg; } : notifyMsg;
            if (config.IsCacthing != null && config.IsCacthing.Value)
            {
                return notifyMsg(config.User, "之前的抓取正在进行");
            }

            config.IsCacthing = true;

            try
            {
                string list = string.Format("列表插入数据：{0}", dataCatch.GetData(config.StartDate, config.Cookies));
                SendDetailState detailState = new SendDetailState(ShowMessage);
                //dataCatch.GetDetailsData(config.Cookies, detailState);
                config.StartDate = DateTime.Now.AddDays(-1);
                config.CurrentMessage = list;
                return notifyMsg(config.User, list);
            }
            catch (Exception t)
            {
                config.ErrorMessage = t.Message;
                return t;
            }
        }

        private static void ShowMessage(string message)
        {
            int sindex = message.IndexOf("【");
            int eindex = message.IndexOf("】");
            string user = string.Empty;
            if(sindex >= 0 && eindex >= 0 && eindex - sindex - 1 >= 0)
            {
                user = message.Substring(sindex + 1, eindex - sindex - 1);
                CacthConfig.CatchDic[user].CurrentMessage = message;
            }
            if (message.StartsWith("下载出错"))
            {
                CacthConfig.CatchDic[user].ErrorMessage = message;
                return;
            }
            if (message.EndsWith("(finish)"))
            {
                CacthConfig config = CacthConfig.CatchDic[user];
                try
                {
                    //发起网络调用,通知服务器端处理请求
                    //DataCatchSave.SaveData();
                    
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
                    
                    WebResponse response = request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
                    {
                        string rv = reader.ReadToEnd();
                    }
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

    
    public delegate object InvokeMessage(string user, string msg);
}