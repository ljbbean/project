﻿using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Collections.Generic;

namespace WebMain
{
    public class StatisticsPage : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Context["area"] = RequestParams["area"];

            Context["zfzje"] = 4938.03;//支付总金额
            Context["tkzje"] = 983;//退款总金额
            Context["zfzbs"] = 300;//支付总笔数
            Context["yfhbs"] = 53;//已发货笔数
            Context["wfhbs"] = 40;//未发货笔数
            Context["tkzzje"] = 234.2;//退款中总金额
            Context["tkzzbs"] = 234.2;//退款中总笔数
            Context["qrzje"] = 2344.2;//确认总金额
            Context["qrzbs"] = 234;//确认总笔数
        }
        [WebMethod]
        public object GetData(SearchArea area, SearchKind method)
        {
            HashObject areaData = GetAreaData(area);
            List<string> xAxisData;
            Dictionary<string, Dictionary<string, decimal>> data = GetChatData(areaData.GetValue<string>("startDate"), areaData.GetValue<string>("endDate"), out xAxisData);

            HashObject list = new HashObject();
            HashObject title = new HashObject();
            title.Add("text", "商品销量数量");
            list.Add("title", title);
            HashObject legend = new HashObject();
            legend.Add("data", data.Keys);
            list.Add("legend", legend);
            HashObject xAxis = new HashObject();
            xAxis.Add("data", xAxisData);
            list.Add("xAxis", xAxis);
            HashObject yAxis = new HashObject();
            yAxis.Add("type", "value");
            list.Add("yAxis", yAxis);

            HashObjectList series = new HashObjectList();
            list.Add("series", series);

            foreach (string key in data.Keys)
            {
                Dictionary<string, decimal> chatlist = data[key];

                List<decimal> seriesData = new List<decimal>();
                foreach (string item in xAxisData)
                {
                    if (chatlist.ContainsKey(item))
                    {
                        seriesData.Add(chatlist[item]);
                    }
                    else
                    {
                        seriesData.Add(0);
                    }
                }
                HashObject seriesItem = new HashObject();
                seriesItem.Add("name", key);
                seriesItem.Add("type", "line");
                seriesItem.Add("data", seriesData);
                HashObject normal = new HashObject();
                HashObject label = new HashObject();
                HashObject itemStyle = new HashObject();
                label.Add("show", true);
                normal.Add("label", label);
                itemStyle.Add("normal", normal);
                seriesItem.Add("itemStyle", itemStyle);
                series.Add(seriesItem);
            }

            return list;
        }

        private HashObject GetAreaData(SearchArea area)
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
            }

            return data;
        }

        /// <summary>
        /// 获取图标数据
        /// </summary>
        private Dictionary<string, Dictionary<string, decimal>> GetChatData(string startDate, string endDate, out List<string> xAxis)
        {
            xAxis = new List<string>();
            IHashObjectList data = GetBillAmount(startDate, endDate);
            Dictionary<string, Dictionary<string, decimal>> dictionary = new Dictionary<string, Dictionary<string, decimal>>();
            foreach (HashObject item in data)
            {
                Dictionary<string, decimal> cdataList;
                string sourceTitle = item.GetValue<string>("sourcetitle");
                decimal amount = item.GetValue<decimal>("amount");
                string payDate = item.GetValue<string>("paydate");
                if (!xAxis.Contains(payDate))
                {
                    xAxis.Add(payDate);
                }
                if (!dictionary.TryGetValue(sourceTitle, out cdataList))
                {
                    cdataList = new Dictionary<string, decimal>();
                    dictionary.Add(sourceTitle, cdataList);
                }
                cdataList.Add(payDate, amount);
            }
            return dictionary;
        }

        /// <summary>
        /// 获取订单数量
        /// </summary>
        private IHashObjectList GetBillAmount(string startDate, string endDate)
        {
            using (DbHelper db = Utils.CreateDbHelper())
            {
                db.AddParameter("startDate", startDate);
                db.AddParameter("endDate", endDate);
                return db.Select(@"
                    SELECT COUNT(1) AS amount,sourceTitle,DATE_FORMAT(createdate, '%Y-%m-%d') AS paydate FROM bill b
                     JOIN billdetail bd ON b.id = bd.bid 
                    where createdate BETWEEN @startDate and @endDate GROUP BY sourceTitle ,DATE_FORMAT(createdate, '%Y-%m-%d')");
            }
        }
    }
}