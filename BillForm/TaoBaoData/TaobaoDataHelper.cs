using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Carpa.Web.Ajax;
using Carpa.Web.Script;
using System.Collections;
using TaoBaoRequest;

namespace TaoBaoData
{
    public class TaobaoDataHelper
    {
        private static string[] users = { "ljbbean", "annychenzy", "风灵415743757" };

        private static DataTable MessageTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("订单ID");
            table.Columns.Add("旺旺名称");
            table.Columns.Add("支付金额");
            table.Columns.Add("收货客户");
            table.Columns.Add("联系电话");
            table.Columns.Add("具体地址");
            table.Columns.Add("区域");
            table.Columns.Add("快递公司");
            table.Columns.Add("物流单号");
            table.Columns.Add("发货状态");
            table.Columns.Add("发货状态status");
            table.Columns.Add("支付宝交易号");
            table.Columns.Add("创建时间");
            table.Columns.Add("付款时间");
            table.Columns.Add("发货时间");
            table.Columns.Add("成交时间");
            table.Columns.Add("买家留言");
            table.Columns.Add("卖家留言");
            table.Columns.Add("货物信息");
            table.Columns.Add("所属用户");
            return table;
        }

        public static DataTable GetDetailData()
        {
            string[] dusers = users;

            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            DataTable table = MessageTable();

            foreach (string duser in dusers)
            {
                IHashObjectList list = new HashObjectList();
                using (DbHelper db = new DbHelper(Utils.Connect, true))
                {
                    db.AddParameter("user", duser);
                    list = db.Select("select content from tbilldetail where `user`=@user");
                }
                string[] keys = {
                                    "mainOrder/payInfo/actualFee/value",
                                    "mainOrder/buyer/nick",
                                    "mainOrder/id",
                                    "mainOrder/orderInfo/lines",
                                    "mainOrder/subOrders",
                                    "buyMessage",//买家备注
                                    "operationsGuide"//卖家备注
                            };

                foreach (HashObject hash in list)
                {
                    var hashObject = new HashObject();
                    string content = hash.GetValue<string>("content");
                    try
                    {
                        hashObject = serializer.Deserialize<HashObject>(content);
                    }
                    catch (Exception t)
                    {
                        continue;
                    }

                    DataRow row = table.NewRow();
                    HashObject addressAndLogistics = GeAddressAndLogisticsInfo(hashObject);
                    foreach (string key in addressAndLogistics.Keys)
                    {
                        row[key] = addressAndLogistics[key];
                    }
                    var newHash = hashObject.GetHashValue(keys);
                    row["订单ID"] = newHash[2]["id"];
                    row["旺旺名称"] = newHash[1]["nick"];
                    row["支付金额"] = newHash[0]["value"];
                    row["买家留言"] = newHash.Count >= 6 && newHash[5].ContainsKey("buyMessage") ? newHash[5]["buyMessage"] : "";

                    row["卖家留言"] = GetSaleMessage(GetKeyObject(newHash, "operationsGuide"));
                    ArrayList linesList = (ArrayList)newHash[3]["lines"];
                    IHashObjectList orderInfoList = GetOrderInfoList(serializer, linesList);
                    if (orderInfoList.Count > 1 || orderInfoList.Count == 0)
                    {
                        throw new Exception("订单信息存在多个时间，请重新核实");
                    }
                    IHashObject tempOrderInfoList = orderInfoList[0];
                    row["支付宝交易号"] = tempOrderInfoList.GetValue<string>("支付宝交易号:", "");
                    row["创建时间"] = tempOrderInfoList.GetValue<string>("创建时间:", null);
                    row["付款时间"] = tempOrderInfoList.GetValue<string>("付款时间:", null);
                    var sendDate = tempOrderInfoList.GetValue<string>("发货时间:", null);
                    row["发货时间"] = sendDate;
                    if (sendDate != null)
                    {
                        row["发货状态"] = "已发货";
                        row["发货状态status"] = "1";
                    }

                    var successDate = tempOrderInfoList.GetValue<string>("成交时间:", null);
                    if (successDate != null)
                    {
                        row["发货状态"] = "已收货";
                        row["发货状态status"] = "2";
                    }
                    row["成交时间"] = successDate;
                    ArrayList subOrders = (ArrayList)newHash[4]["subOrders"];
                    List<GoodsInfo> gList = GetSubOrderSkuList(subOrders);
                    row["货物信息"] = serializer.Serialize(gList);
                    row["所属用户"] = duser;
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        
        private static object GetKeyObject(List<TaoBaoRequest.HashObjectExt.KeyValue> list, string key)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ContainsKey(key))
                {
                    return list[i][key];
                }
            }
            return null;
        }

        private static List<GoodsInfo> GetSubOrderSkuList(ArrayList subOrders)
        {
            List<GoodsInfo> gList = new List<GoodsInfo>();

            string[] subOrderKeys = { "itemInfo/skuText", "itemInfo/title", "quantity" };
            foreach (HashObject subOrder in subOrders)
            {
                GoodsInfo info = new GoodsInfo();
                IHashObjectList subOrderSkuList = new HashObjectList();
                var subList = subOrder.GetHashValue(subOrderKeys);
                ArrayList skuText = (ArrayList)subList[0]["skuText"];
                HashObject thash = new HashObject();
                info.Amount = ((HashObject)subList[2]).GetValue<string>("quantity");
                subOrderSkuList.Add(thash);
                foreach (HashObject skuTextItem in skuText)
                {
                    ArrayList skuTextItemContent = skuTextItem.GetValue<ArrayList>("content");

                    IHashObject data = new HashObject();
                    foreach (HashObject skuTextItemContentDetial in skuTextItemContent)
                    {
                        if (!"namevalue".Equals(skuTextItemContentDetial.GetValue<string>("type")))
                        {
                            continue;
                        }
                        HashObject tHash = skuTextItemContentDetial.GetValue<HashObject>("value");
                        if ("颜色分类：".Equals(tHash.GetValue<string>("name")))
                        {
                            info.Color = tHash.GetValue<string>("value");
                        }
                        if ("尺寸：".Equals(tHash.GetValue<string>("name")))
                        {
                            info.Size = tHash.GetValue<string>("value");
                        }
                    }
                    if (data.Keys.Count == 0)
                    {
                        continue;
                    }
                    subOrderSkuList.Add(data);
                }
                if (subOrderSkuList.Count == 0)
                {
                    continue;
                }
                info.Title = subList[1]["title"].ToString();
                gList.Add(info);
            }
            return gList;
        }

        private static HashObject GeAddressAndLogisticsInfo(HashObject hashObject)
        {
            var tabs = (ArrayList)hashObject["tabs"];
            HashObject rt = new HashObject();
            foreach (HashObject tab in tabs)
            {
                if (!"收货和物流信息".Equals(tab.GetValue<string>("title")))
                {
                    continue;
                }
                var content = tab.GetValue<HashObject>("content");
                var address = content.GetValue<string>(content.ContainsKey("newAddress") ? "newAddress" : "address");
                string[] items = address.Split('，');
                rt.Add("收货客户", items[0]);
                rt.Add("联系电话", items[1]);
                StringBuilder saddress = new StringBuilder();
                bool isAddress = false;
                var i = 2;
                for (; i < items.Length - 1; i++)
                {
                    if (items[i].IndexOf('省') >= 0 || items[i].IndexOf('区') >= 0)
                    {
                        isAddress = true;
                    }
                    if (isAddress)
                    {
                        saddress.AppendFormat("{0} ", items[i]);
                    }
                }
                var naddress = saddress.ToString();
                rt.Add("具体地址", naddress);
                var area = naddress.Split(' ');
                rt.Add("区域", string.Format("{0} {1} {2}", area[0], area[1], area[2]));
                rt.Add("快递公司", GetLogisticsInfo(content.GetValue<string>("logisticsName")));//快递公司
                var logisticsInfo = GetLogisticsInfo(content.GetValue<string>("logisticsNum"));
                rt.Add("物流单号", logisticsInfo);//物流单号
                rt.Add("发货状态", string.IsNullOrEmpty(logisticsInfo) ? "未发货" : "已发货");
                rt.Add("发货状态status", string.IsNullOrEmpty(logisticsInfo) ? "0" : "1");
                return rt;
            }
            return null;
        }

        internal static string GetLogisticsInfo(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            string value = obj.ToString();
            if (value.StartsWith("-") || value.StartsWith("—"))
            {
                return "";
            }
            return value;
        }

        private static string GetSaleMessage(object obj)
        {
            if (obj == null || !(obj is ArrayList))
            {
                return "";
            }
            ArrayList list = (ArrayList)obj;
            foreach (HashObject hash in list)
            {
                if (!"div".Equals(hash.GetValue<string>("layout")))
                {
                    continue;
                }

                ArrayList lines = hash.GetValue<ArrayList>("lines");
                foreach (HashObject lhash in lines)
                {
                    if (!"block".Equals(lhash.GetValue<string>("display")))
                    {
                        continue;
                    }
                    ArrayList clist = lhash.GetValue<ArrayList>("content");
                    if (clist.Count >= 2)
                    {
                        HashObject value = clist[1] as HashObject;
                        return value.GetValue<string>("value");
                    }

                    return "";
                }
            }
            return "";
        }

        private static IHashObjectList GetOrderInfoList(JavaScriptSerializer serializer, ArrayList linesList)
        {
            IHashObjectList orderInfoList = new HashObjectList();
            foreach (HashObject thash in linesList)
            {
                var content = thash.GetValue<ArrayList>("content");
                if (content.Count == 0)
                {
                    continue;
                }

                IHashObject orderInfo = new HashObject();
                foreach (HashObject contentItem in content)
                {
                    var contentItemValue = contentItem.GetValue<HashObject>("value");
                    string key = contentItemValue.GetValue<string>("name");
                    if (contentItemValue["value"] is ArrayList)
                    {
                        orderInfo[key] = serializer.Serialize(contentItemValue["value"]);
                        continue;
                    }
                    orderInfo[key] = contentItemValue["value"].ToString();
                }
                orderInfoList.Add(orderInfo);
            }
            return orderInfoList;
        }

    }
}
