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
    }
}
