
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Script;

namespace Common
{
    /// <summary>
    /// 网络数据处理器
    /// </summary>
    public class NetDataHandler
    {
        /// <summary>
        /// 列表数据转换
        /// </summary>
        /// <param name="rdata">网络数据</param>
        public static SearchOrderInfo ListDataTransformation(string rdata)
        {
            SearchOrderInfo info = new SearchOrderInfo();
            if (string.IsNullOrEmpty(rdata))
            {
                info.ErrorMsg = "网络数据为空";
                return info;
            }
            JsonSerializer serializer = JsonSerializer.CreateInstance();
            HashMap hash = null;
            try
            {
                hash = (HashMap)serializer.DeserializeObject(rdata);
            }
            catch (Exception e)
            {
                info.ErrorMsg = string.Format("网络数据转换错误，ErrorMessage:{0}, data:{1}", e.Message, rdata);
                return info;
            }
            string[] keys = { "mainOrders/buyer/nick",
                    "mainOrders/orderInfo/createTime",
                    "mainOrders/orderInfo/id",
                    "mainOrders/payInfo/actualFee",
                    "mainOrders/payInfo/icons",
                    "mainOrders/statusInfo/text",
                    "mainOrders/statusInfo/operations",
                    "mainOrders/subOrders/itemInfo/skuText",
                    "mainOrders/subOrders/itemInfo/title",
                    "mainOrders/subOrders/quantity",
                    "page/currentPage",
                    "page/totalNumber",
                    "page/totalPage"};

            HashMap vhash = hash.GetHashValue(keys)[0];
            info.MainOrders = (IList<object>)vhash["mainOrders"];
            info.CurrentPage = hash.GetHashValue(keys)[1].GetValue<int>("currentPage");
            info.TotalNumber = hash.GetHashValue(keys)[2].GetValue<int>("totalNumber");
            info.TotalPage = hash.GetHashValue(keys)[3].GetValue<int>("totalPage");
            return info;
        }

        /// <summary>
        /// 明细数据获取
        /// </summary>
        /// <param name="rdata">明细数据</param>
        public static object DetailDataTransformation(string rdata)
        {
            JsonSerializer serializer = JsonSerializer.CreateInstance();
            string tempJson = GetDetailDataFromHtml(rdata);
            HashMap data = (HashMap)serializer.DeserializeObject(tempJson);
            string[] keys = { "tabs/content/alingPhone",
                    "tabs/content/logisticsName",
                    "tabs/content/address",
                    "tabs/content/logisticsNum"};

            return data.GetHashValue(keys);
        }

        public static string GetDetailDataFromHtml(string rdata)
        {
            string key = "JSON.parse('";
            string json = rdata.Substring(rdata.IndexOf(key) + key.Length);
            string script = "</script>";
            json = json.Substring(0, json.IndexOf(script));
            json = json.Substring(0, json.LastIndexOf('}') + 1);//去掉尾部
            json = string.Format("\"{0}\"", json);
            JsonSerializer serializer = JsonSerializer.CreateInstance();
            return serializer.DeserializeObject(json).ToString();
        }
    }
}
