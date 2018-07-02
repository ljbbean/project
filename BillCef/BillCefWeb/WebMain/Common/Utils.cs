using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carpa.Web.Script;

namespace WebMain.Common
{
    public class Utils
    {
        internal static HashObject GetAreaData(SearchArea area)
        {
            HashObject data = new HashObject();
            switch (area)
            {
                case SearchArea.Today:
                    data.Add("startDate", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                    data.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                    break;
                case SearchArea.Week:
                    int dayofWeek = (int)DateTime.Now.DayOfWeek;
                    data.Add("startDate", DateTime.Now.AddDays(-dayofWeek).ToString("yyyy-MM-dd 00:00:00"));
                    data.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                    break;
                case SearchArea.Month:
                    data.Add("startDate", DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
                    data.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                    break;
                case SearchArea.DaysOf30:
                    data.Add("startDate", DateTime.Now.AddDays(-30).ToString("yyyy-MM-01 00:00:00"));
                    data.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                    break;
            }
            return data;
        }
    }
}