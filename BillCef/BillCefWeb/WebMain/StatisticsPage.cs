using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Collections.Generic;
using WebMain.chat;
using WebMain.Common;

namespace WebMain
{
    [NeedLogin]
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
            Context["ddlyVisible"] = false;
        }
        [WebMethod]
        public object GetData(SearchArea area, SearchKind method)
        {
            HashObject areaData = Utils.GetAreaData(area);
            List<string> xAxisData;
            Dictionary<string, Dictionary<string, decimal>> data = GetChatData(areaData.GetValue<string>("startDate"), areaData.GetValue<string>("endDate"), out xAxisData);

            if (method == SearchKind.ComeFrom)
            {
                return GetPieData(data, xAxisData);
            }
            switch (area)
            {
                case SearchArea.Today:
                    return GetBarData(data, xAxisData);
                case SearchArea.Month:
                case SearchArea.Week:
                case SearchArea.DaysOf30:
                    return GetLineData(data, xAxisData);
            }
            return GetLineData(data, xAxisData);
        }

        private object GetLineData(Dictionary<string, Dictionary<string, decimal>> data, List<string> xAxisData)
        {
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

        private object GetBarData(Dictionary<string, Dictionary<string, decimal>> data, List<string> xAxisData)
        {
            Bar.Root root = new Bar.Root();

            root.title.text = "";
            root.title.subtext = "";

            root.tooltip.trigger = "axis";

            root.legend = new Bar.Legend();
            root.legend.data = xAxisData;

            root.toolbox.show = false;
            root.toolbox.feature.mark.show = true;
            root.toolbox.feature.dataView = new Bar.DataView() { show = true, readOnly = true };
            root.toolbox.feature.magicType = new Bar.MagicType() { show = true };
            root.toolbox.feature.magicType.type.AddRange(new string[] { "line", "bar" });
            root.toolbox.feature.restore.show = true;
            root.toolbox.feature.saveAsImage.show = true;
            root.calculable = true;
            Bar.XAxisItem xAxisItem = new Bar.XAxisItem();
            xAxisItem.type = "category";
            xAxisItem.data = new List<string>() { "单品销量"};
            root.xAxis.Add(xAxisItem);
            Bar.YAxisItem yAxisItem = new Bar.YAxisItem();
            yAxisItem.type = "value";
            root.yAxis.Add(yAxisItem);
            foreach (string key in data.Keys)
            {
                var tempValue = data[key];
                if (tempValue.Count == 0)
                {
                    continue;
                }
                Bar.SeriesItem seriesItem = new Bar.SeriesItem();
                seriesItem.name = key;
                seriesItem.type = "bar";
                decimal[] array = new decimal[tempValue.Values.Count];
                tempValue.Values.CopyTo(array, 0);
                seriesItem.data.Add((double)array[0]);
                root.series.Add(seriesItem);
            }
            return root;
        }

        private object GetPieData(Dictionary<string, Dictionary<string, decimal>> data, List<string> xAxisData)
        {
            Pie.Root root = new Pie.Root();
            root.title = new Pie.Title() { text = "某站点用户访问来源", subtext="纯属虚构", x="center" };
            root.tooltip = new Pie.Tooltip() { trigger = "item", formatter = "{a} <br/>{b} : {c} ({d}%)" };
            root.legend = new Pie.Legend() { orient = "vertical", x = "left" };
            root.legend.data.AddRange(new string[] { "直接访问", "邮件营销", "联盟广告", "视频广告", "搜索引擎" });
            Pie.Option option = new Pie.Option() { funnel = new Pie.Funnel() { x = "25%", width="50%", funnelAlign="left", max=1548 } };
            Pie.MagicType magicType = new Pie.MagicType();
            magicType.show = true;
            magicType.type = new List<string>() { "pie", "funnel" };
            magicType.option = option;
            Pie.Feature feature = new Pie.Feature() { mark = new Pie.Mark() { show = true }, dataView = new Pie.DataView() { readOnly = false, show = true }, magicType = magicType, restore = new Pie.Restore() { show = true }, saveAsImage = new Pie.SaveAsImage() { show = false } };
            root.toolbox = new Pie.Toolbox() { show = true, feature = feature };

            root.calculable = true;

            Pie.SeriesItem seriesItem = new Pie.SeriesItem();
            seriesItem.name = "访问来源";
            seriesItem.type = "pie";
            seriesItem.radius = "55%";
            seriesItem.center = new List<string>() { "50%", "60%" };
            seriesItem.data.Add(new Pie.DataItem() { value = 3323, name = "直接访问1" });
            seriesItem.data.Add(new Pie.DataItem() { value = 3312, name = "直接访问2" });
            seriesItem.data.Add(new Pie.DataItem() { value = 331, name = "直接访问3" });
            seriesItem.data.Add(new Pie.DataItem() { value = 323, name = "直接访问4" });
            seriesItem.data.Add(new Pie.DataItem() { value = 2233, name = "直接访问5" });
            root.series.Add(seriesItem);
            return root;
        }

        /// <summary>
        /// 获取图标数据
        /// </summary>
        /// <remarks>
        /// key:sourcetitle  key:paydate   value:amount
        /// </remarks>
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
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("startDate", startDate);
                db.AddParameter("endDate", endDate);
                UserInfo info = (UserInfo)Session["user"];
                db.AddParameter("user", info.User);

                return db.Select(@"
                    SELECT COUNT(1) AS amount,sourceTitle,DATE_FORMAT(createdate, '%Y-%m-%d') AS paydate FROM bill b
                     JOIN billdetail bd ON b.id = bd.bid 
                    where `user`=@user and createdate BETWEEN @startDate and @endDate GROUP BY sourceTitle ,DATE_FORMAT(createdate, '%Y-%m-%d')");
            }
        }
    }
}
