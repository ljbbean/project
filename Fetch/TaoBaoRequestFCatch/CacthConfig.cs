using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;
using Common;
using Common.Script;

namespace TaoBaoRequestFCatch
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
        private string startDateUrl;
        private readonly long datetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        private DateTime serverCurrentDate;


        public CacthConfig(string netUrl)
        {
            startDateUrl = netUrl;
        }

        public DateTime ServerCurrentData
        {
            get
            {
                return serverCurrentDate;
            }
        }

        private DateTime startDate = new DateTime();
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate 
        {
            get
            {
                if (startDate != new DateTime())
                {
                    return startDate;
                }
                
                string temp = GetNetData(startDateUrl, "{\"user\":\"" + User + "\"}");
                HashMap hash = JsonSerializer.CreateInstance().Deserialize<HashMap>(temp);
                temp = hash.GetValue<string>("startDate");
                serverCurrentDate = new DateTime(hash.GetValue<long>("currentDate") * 10000 + datetimeMinTimeTicks, DateTimeKind.Utc).ToLocalTime();
                if (string.IsNullOrEmpty(temp))
                {
                    DateTime ntemp = DateTime.Now.AddDays(-10);
                    startDate = new DateTime(ntemp.Year, ntemp.Month, ntemp.Day);
                }
                else
                {
                    temp = temp.Trim('\"');
                    long ticks;
                    if (long.TryParse(temp, out ticks))
                    {
                        startDate = new DateTime(ticks * 10000 + datetimeMinTimeTicks, DateTimeKind.Utc);
                        startDate = startDate.ToLocalTime();
                    }
                    else
                    {
                        DateTime ntemp = DateTime.Now.AddDays(-10);
                        startDate = new DateTime(ntemp.Year, ntemp.Month, ntemp.Day);
                    }
                }
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

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
                user = Utils.GetUser(value);
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

        private string GetNetData(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            if (!string.IsNullOrEmpty(param))
            {
                string data = param;
                request.ContentLength = data.Length;
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gbk")))
                {
                    writer.Write(data);
                    writer.Flush();
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}