using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class StringExt
    {
        public static string GetSectionString(this string html, string sflag, string eflag)
        {
            int sindex = html.IndexOf(sflag);
            if (sindex < 0)
            {
                return "";
            }
            sindex = sindex + sflag.Length;
            int eindex = html.Substring(sindex).IndexOf(eflag);
            if (eindex < 0)
            {
                return "";
            }

            return html.Substring(sindex, eindex).Trim();
        }

        public static string CatchSection(this string html, string startKey, string endKey, string msg)
        {
            int index = html.IndexOf(startKey);
            if (index < 0)
            {
                throw new Exception(msg);
            }

            html = html.Substring(index + startKey.Length);

            index = html.IndexOf(endKey);
            if (index < 0)
            {
                throw new Exception(msg);
            }
            html = html.Substring(0, index);
            return html;
        }

        public static string UrlEncode(this string text)
        {
            StringBuilder locator = new StringBuilder();
            foreach (var detail in Encoding.GetEncoding("GBK").GetBytes(text))
            {
                locator.Append(@"%" + Convert.ToString(detail, 16));
            }
            return (locator.ToString());
        }

        /// <summary>
        /// 时间戳=>时间
        /// </summary>
        public static DateTime ToTaobaoTimeStamp(this string text)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(long.Parse(text + "0000")));
        }
    }
}
