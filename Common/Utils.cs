using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Utils
    {
        public static string GetUser(string cookie)
        {
            string flag = "tracknick=";
            int index = cookie.IndexOf(flag);
            if (index >= 0)
            {
                string subCookies = cookie.Substring(index + flag.Length);
                index = subCookies.IndexOf(";");
                return subCookies.Substring(0, index);
            }

            throw new Exception("cookies数据不正确");
        }

        public static string ConvertUnixDate(long tick)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(tick + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow).ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
