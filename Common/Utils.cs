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
    }
}
