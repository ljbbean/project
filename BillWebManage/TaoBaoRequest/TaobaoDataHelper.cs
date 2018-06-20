using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Carpa.Web.Ajax;
using Carpa.Web.Script;
using System.Collections;
using TaoBaoRequest;
using Common;

namespace TaoBaoRequest
{
    public class TaobaoDataHelper
    {
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
            table.Columns.Add("拍下总金额");
            return table;
        }

        /// <summary>
        /// 拆分明细数据到表
        /// </summary>
        public static DataTable SpliteContentToDataTableByUser(string user, string connectString, bool hasUpdate = false)
        {
            IHashObjectList list = new HashObjectList();

            using (DbHelper db = new DbHelper(connectString, true))
            {
                db.AddParameter("user", user);
                if (hasUpdate)
                {
                    //获取有更新的数据
                    list = db.Select("SELECT tbd.content, (SELECT tb.status FROM tbill tb WHERE tb.tbid = tbd.tbid) AS tstatus FROM tbilldetail tbd JOIN tbill tbl ON tbl.tbid = tbd.tbid WHERE tbl.hasupdate=1 and tbd.`user`=@user");
                }
                else
                {
                    //获取所有数据
                    list = db.Select("select tbd.content, (select tb.status from tbill tb where tb.tbid = tbd.tbid) as tstatus from tbilldetail tbd where tbd.`user`=@user");
                }
            }

            List<CatchDataTemplate> tList = new List<CatchDataTemplate>();

            foreach (HashObject hash in list)
            {
                tList.Add(new CatchDataTemplate() { DetailContent = hash.GetValue<string>("content"), Status = hash.GetValue<string>("tstatus") });
            }

            return SpliteContentToDataTable(user, tList);
        }

        public static DataTable SpliteContentToDataTable(string user, List<CatchDataTemplate> list)
        {
            DataTable table = MessageTable();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string[] keys = {
                                    "mainOrder/payInfo/actualFee/value",
                                    "mainOrder/buyer/nick",
                                    "mainOrder/id",
                                    "mainOrder/orderInfo/lines",
                                    "mainOrder/subOrders",
                                    "buyMessage",//买家备注
                                    "operationsGuide"//卖家备注
                            };
            foreach (CatchDataTemplate template in list)
            {
                var hashObject = new HashObject();
                string content = template.DetailContent;
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
                row["订单ID"] = newHash.GetDataEx<string>("id");
                row["旺旺名称"] = newHash.GetDataEx<string>("nick");
                row["买家留言"] = newHash.GetDataEx<string>("buyMessage");

                row["卖家留言"] = GetSaleMessage(GetKeyObject(newHash, "operationsGuide"));
                ArrayList linesList = newHash.GetDataEx<ArrayList>("lines");
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

                //后期单据退款(各种原因的退款)
                if ("交易关闭".Equals(template.Status))
                {
                    row["支付金额"] = 0;
                    row["发货状态"] = "已关闭";
                    row["发货状态status"] = "9";
                }
                else
                {
                    row["支付金额"] = newHash.GetDataEx<string>("value");//支付总金额
                    newHash.GetDataEx<string>("value");//支付总金额
                }

                ArrayList subOrders = newHash.GetDataEx<ArrayList>("subOrders");
                List<GoodsInfo> gList = GetSubOrderSkuList(subOrders);
                decimal all = 0;
                foreach (GoodsInfo info in gList)
                {
                    all += info.PriceInfo;
                }
                row["拍下总金额"] = all;
                row["货物信息"] = serializer.Serialize(gList);
                row["所属用户"] = user;
                table.Rows.Add(row);
            }

            return table;
        }

        private static object GetKeyObject(List<HashObjectExt.KeyValue> list, string key)
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

            string[] subOrderKeys = { "itemInfo/skuText", "itemInfo/title", "quantity","priceInfo" };
            foreach (HashObject subOrder in subOrders)
            {
                GoodsInfo info = new GoodsInfo();
                IHashObjectList subOrderSkuList = new HashObjectList();
                var subList = subOrder.GetHashValue(subOrderKeys);
                ArrayList skuText = subList.GetDataEx<ArrayList>("skuText");
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
                info.PriceInfo = decimal.Parse(subList.GetDataEx<string>("priceInfo"));
                info.Title = subList.GetDataEx<string>("title");
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

        public static string GetLogisticsInfo(object obj)
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
                    if (!contentItemValue.ContainsKey("value"))
                    {
                        continue;
                    }
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

        private static string SpliteContentUrl(string content)
        {
            if (content == null)
            {
                return "";
            }
            string flag = "\"/trade/memo/update_sell_memo.htm?";
            int index = content.IndexOf(flag);
            if (index < 0)
            {
                return content;
            }
            string tempString = content.Substring(flag.Length + index);
            return content.Substring(0, index + 1) + tempString.Substring(tempString.IndexOf("\""));
        }
        /// <summary>
        /// 获取所有ID构成的sql查询集
        /// </summary>
        private static string GetAllIdString(IList data)
        {
            StringBuilder sbuilder = new StringBuilder("(");
            foreach (HashObject hash in data)
            {
                sbuilder.AppendFormat("'{0}',", hash.GetValue<string>("bid"));
            }
            return sbuilder.ToString().Substring(0, sbuilder.Length - 1) + ")";
        }

        public static void SaveDataToTBill(string user, string connection, IList data)
        {
            DateTime date = DateTime.Now;
            using (DbHelper db = new DbHelper(connection))
            {
                IHashObjectList bidList = db.Select(string.Format("select * from tbill where bid in {0}", GetAllIdString(data)));
                //根据单号获取对应的字典信息
                Dictionary<string, HashObject> bidDictionary = new Dictionary<string, HashObject>();
                foreach (HashObject item in bidList)
                {
                    bidDictionary.Add(item.GetValue<string>("bid"), item);
                }

                //筛选数据，对于已插入的数据做数据对比，当数据没有变化时，不做数据修改,反之则修改数据。没有的数据直接插入
                StringBuilder insertbuilder = new StringBuilder("insert into tbill(tbid,bid,content, cdate, status, `user`, downeddetail, udate, hasUpdate) values");
                StringBuilder insertDetailbuilder = new StringBuilder("insert into tbilldetail(tbdid,tbid,content,user) values");
                string updateSql = "update tbill set content = @content, udate = @udate, status=@status, downeddetail=@downeddetail, hasUpdate = @hasUpdate where tbid=@tbid";
                bool hasInsert = false;
                bool hasDetail = false;
                foreach (HashObject row in data)
                {
                    var id = row.GetValue<string>("bid");
                    string content = ReplaceHtmlText(row.GetValue<string>("content"));
                    string status = row.GetValue<string>("status");
                    HashObject item;
                    ulong tbid = Cuid.NewCuid();
                    if (bidDictionary.TryGetValue(id, out item))
                    {
                        tbid = item.GetValue<ulong>("tbid");
                        if (SpliteContentUrl(item.GetValue<string>("content")).Equals(SpliteContentUrl(content)) && item.GetValue<string>("status").Equals(status))
                        {
                            //数据相同直接返回
                            continue;
                        }
                        db.AddParameter("content", content);
                        db.AddParameter("udate", date);//更新时间
                        db.AddParameter("status", status);
                        db.AddParameter("tbid", tbid);
                        //存在不同的，标记全部更新明细
                        db.AddParameter("downeddetail", 0);
                        db.AddParameter("hasUpdate", 1);
                        db.ExecuteIntSQL(updateSql);//更新已下载数据
                        continue;
                    }
                    else
                    {
                        hasDetail = true;
                        HashObject detail = row.GetValue<HashObject>("detail");
                        string str = ReplaceHtmlText(detail.GetValue<string>("content"));
                        insertDetailbuilder.AppendFormat("({0},{1},'{2}', '{3}'),", Cuid.NewCuid(), tbid, str, detail.GetValue<string>("user"));
                    }
                    hasInsert = true;
                    insertbuilder.AppendFormat("({0},'{1}','{2}', '{3}', '{4}', '{5}', 0, '{6}', 1),", tbid, id, content, date, status, user, date);
                }
                if (!hasInsert)
                {
                    return;
                }
                try
                {
                    db.BeginTransaction();
                    string insertData = insertbuilder.ToString();
                    db.BatchExecute(insertData.Substring(0, insertData.Length - 1));

                    if (hasDetail)
                    {
                        string insertDetailData = insertDetailbuilder.ToString();
                        db.BatchExecute(insertDetailData.Substring(0, insertDetailData.Length - 1));
                    }
                    db.CommitTransaction();
                }
                catch(Exception t)
                {
                    db.RollbackTransaction();
                    throw t;
                }
            }
        }
        
        public static string ReplaceHtmlText(string str)
        {
            return str.Replace("&times;", "*").Replace("&middot;", "。").Replace("&mdash;", "—").Replace("\\\"", "");
        }
    }
}
