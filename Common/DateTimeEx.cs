using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class DateTimeEx
    {
        public static string GetTimeStamp(this DateTime DateStart)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateStart - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 3);
        }
    }
}
