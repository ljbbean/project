using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Carpa.Web.Script;
using System.Text;
using TaoBaoRequest;
using Carpa.Web.Ajax;
using Common;
using System.Collections;
using WebMain;

namespace WebHandler.DataHandler
{
    internal delegate void CallBackMsg(string msg);

    public class DataCatchSave
    {
        private static Dictionary<string, Dictionary<string, decimal>> userGoodsDictionary = new Dictionary<string, Dictionary<string, decimal>>();
        
        internal static void ClearUserGoodsCache(string uname)
        {
            userGoodsDictionary.Remove(uname);
        }

        private static string GetTaobaoBillID(DataTable table)
        {
            StringBuilder sbuilder = new StringBuilder("(");
            foreach (DataRow row in table.Rows)
            {
                sbuilder.AppendFormat("'{0}',", row["订单ID"]);
            }
            string sql = sbuilder.ToString();
            sql = sql.Substring(0, sql.Length - 1) + ")";
            return sql;
        }

        private static bool IsNullDate(object date)
        {
            return date == null || "null".Equals(date) || date.ToString().Length == 0;
        }

        private static string GetRemark(DataRow row)
        {
            StringBuilder sbuilder = new StringBuilder();
            var mjly = row["买家留言"];
            int index = 0;
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(TaobaoDataHelper.ReplaceHtmlText(string.Format("【买家留言：{0}】", mjly)));
                index++;
            }

            mjly = row["卖家留言"];
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(TaobaoDataHelper.ReplaceHtmlText(string.Format("【卖家留言：{0}】", mjly)));
            }
            return sbuilder.ToString();
        }
        

        private static Dictionary<string, string> GetExistBills(DbHelper db, string sql)
        {
            if (db == null || string.IsNullOrEmpty(sql))
            {
                return new Dictionary<string, string>();
            }
            IHashObjectList oldList = db.Select(string.Format("select id, tbcode FROM bill where tbcode in {0}", sql));
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (HashObject hash in oldList)
            {
                dic[hash.GetValue<string>("tbcode")] = hash.GetValue<string>("id");
            }
            return dic;
        }

        private static List<string> GetExistBillDetails(DbHelper db, string sql)
        {
            if (db == null || string.IsNullOrEmpty(sql))
            {
                return new List<string>();
            }
            IHashObjectList oldList = db.Select(string.Format("select DISTINCT code FROM billdetail where code in {0}", sql));
            List<string> list = new List<string>();
            foreach (HashObject hash in oldList)
            {
                list.Add(hash.GetValue<string>("code"));
            }
            return list;
        }

        private static string GetGoodsKey(string color, string size, string sourceTitle)
        {
            return string.Format("{0}_{1}_{2}", color, size, sourceTitle);
        }

        private static Dictionary<string, decimal> GetGoodsRate(DbHelper db, string uname)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            if (db == null)
            {
                return dictionary;
            }
            if (userGoodsDictionary.TryGetValue(uname, out dictionary))
            {
                return dictionary;
            }
            dictionary = new Dictionary<string, decimal>();
            db.AddParameter("uname", uname);
            IHashObjectList list = db.Select("select color,size,sourcetitle,rate from `commissionrate` where uname=@uname");

            foreach (HashObject item in list)
            {
                decimal temp = 0;
                string key = GetGoodsKey(item.GetValue<string>("color"), item.GetValue<string>("size"), item.GetValue<string>("sourcetitle"));
                if (dictionary.TryGetValue(key, out temp))
                {
                    throw new Exception(string.Format("商品【{0}】已经设置了比例为{1}", key, temp));
                }
                temp = item.GetValue<int>("rate");
                dictionary.Add(key, temp);
            }
            userGoodsDictionary.Add(uname, dictionary);
            return dictionary;
        }

        /// <summary>
        /// 直接分析对象数据，把数据转换为单据数据
        /// </summary>
        internal static HashObject AnalysisData(string suser, IList data, CallBackMsg callBack)
        {
            try
            {
                List<CatchDataTemplate> tlist = new List<CatchDataTemplate>();
                foreach (HashObject item in data)
                {
                    tlist.Add(new CatchDataTemplate() { Status = item.GetValue<string>("status"), DetailContent = item.GetValue<HashObject>("detail").GetValue<string>("content") });
                }
                callBack("开始拆分数据");
                DataTable table = TaobaoDataHelper.SpliteContentToDataTable(suser, tlist);
                callBack("数据拆分完成");
                HashObjectList billList = new HashObjectList();
                HashObjectList detailList = new HashObjectList();

                callBack("开始组装数据");
                BuildBillDataFromTable(suser, false, table, billList, detailList, null, null);
                callBack("数据组装完成");
                HashObject list = new HashObject();
                list.Add("bill", billList);
                list.Add("detail", detailList);
                return list;
            }
            catch(Exception e)
            {
                callBack(string.Format("Exception:{0}", e.Message));
            }
            return null;
        }

        internal static string SaveData(string suser, CallBackMsg callBack, bool onlyAdd = true)
        {
            DataTable table = TaobaoDataHelper.SpliteContentToDataTableByUser(suser, AppUtils.ConnectionString, true);
            if(table.Rows.Count == 0)
            {
                return "OK:没有需要分析的数据";
            }

            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    string ids = GetTaobaoBillID(table);
                    HashObjectList billList = new HashObjectList();
                    HashObjectList detailList = new HashObjectList();

                    callBack("开始分析数据");
                    callBack("开始构建数据");

                    BuildBillDataFromTable(suser, onlyAdd, table, billList, detailList, db, ids, false);

                    StringBuilder insertBillDetailBuilder = new StringBuilder(@"insert into billdetail(id, bid, code, size, amount, color, address,area,total, remark, 
                        ltotal,sourceTitle,goodsstatus,sendway, btotal) values");
                    string sDetailFormate = "({0}, {1}, '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, '{9}',{10}, '{11}', {12}, '{13}', {14}),";
                    foreach (HashObject item in detailList)
                    {
                        //构建明细数据
                        insertBillDetailBuilder.AppendFormat(sDetailFormate, item["id"], item["bid"], item["code"], item["size"], item["amount"], item["color"], item["address"], item["area"], item["total"], item["remark"], item["ltotal"], item["sourceTitle"], item["goodsstatus"], item["sendway"], item["btotal"]);
                    }

                    string insertBillDetail = insertBillDetailBuilder.ToString();
                    insertBillDetail = insertBillDetail.Substring(0, insertBillDetail.Length - 1);


                    StringBuilder doedIds = new StringBuilder();//影响到的主数据
                    StringBuilder insertBillBuilder = new StringBuilder(@"insert into bill(id, date, taobaocode,cname,ctel,caddress,carea,cremark,
                        ltotal,status, scode, sname, uid, goodsstatus, billfrom, createdate, zfbpaycode,tbcode, total, btotal, senddate, successdate, `user`) values");
                    foreach (HashObject item in billList)
                    {
                        StringBuilder sformate = new StringBuilder("({0}, '{1}', '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, {9},'{10}', '{11}', {12}, {13}, '{14}','{15}', '{16}', '{17}', {18},{19} ");
                        sformate.Append(IsNullDate(item["senddate"]) ? ",{20}" : ",'{20}'");
                        sformate.Append(IsNullDate(item["successdate"]) ? ",{21}" : ",'{21}'");
                        sformate.Append(",'{22}'");
                        sformate.Append("), ");
                        doedIds.AppendFormat("{0},", item["id"]);
                        //构建主表数据，如果已经存在，直接更改数据
                        insertBillBuilder.AppendFormat(sformate.ToString(), item["id"], item["date"], item["taobaocode"], item["cname"], item["ctel"], item["caddress"], item["carea"], item["cremark"],
                        item["ltotal"], item["status"], item["scode"], item["sname"], item["uid"], item["goodsstatus"], item["billfrom"], item["createdate"], item["zfbpaycode"], item["tbcode"], item["total"], item["btotal"], item["senddate"], item["successdate"], suser);
                    }
                    string insertBill = insertBillBuilder.ToString();
                    insertBill = insertBill.Substring(0, insertBill.Length - 2);

                    callBack("构建数据完毕");
                    callBack("开始保存数据");
                    if (billList.Count != 0)
                    {
                        db.BeginTransaction();
                        db.BatchExecute(string.Format("update tbill set hasupdate=0 where bid in {0}", ids));
                        db.BatchExecute(string.Format("{0} on duplicate key update `createdate`=values(`createdate`),`senddate`=values(`senddate`),`successdate`=values(`successdate`),`zfbpaycode`=values(`zfbpaycode`),`status`=values(`status`),`sname`=values(`sname`),`scode`=values(`scode`);", insertBill));
                        if (detailList.Count != 0)
                        {
                            db.BatchExecute(insertBillDetail);//直接新增，不修改
                        }
                        //后期退款的单据，金额都为0
                        db.BatchExecute(string.Format("update bill set ltotal = 0, total=0, btotal=0 where status=9 and id in ({0})", doedIds.ToString().Substring(0, doedIds.Length - 1)));
                        db.CommitTransaction();
                    }
                    return string.Format("OK:数据保存成功，分析处理了{0}条数据", billList.Count);
                }
                catch (Exception e1)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    return string.Format("Exception:{0}", e1.Message);
                }
            }
        }

        private static void BuildBillDataFromTable(string suser, bool onlyAdd, DataTable table, HashObjectList billList, HashObjectList detailList, DbHelper db = null, string ids = null, bool sureRate = true)
        {
            Dictionary<string, string> dic = GetExistBills(db, ids);
            List<string> dicDetail = GetExistBillDetails(db, ids);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            foreach (DataRow row in table.Rows)
            {
                var ddid = row["订单ID"].ToString();
                string id = Cuid.NewCuid().GetHashCode().ToString();
                string oldid;
                if (dic.TryGetValue(ddid, out oldid))
                {
                    if (onlyAdd)
                    {
                        continue;
                    }
                    id = oldid;
                }
                string user = row["所属用户"].ToString();
                if (user != suser)
                {
                    throw new Exception("分析用户和数据保存用户不匹配，无法做数据保存");
                }
                object sendDate = row["发货时间"];
                object successDate = row["成交时间"];
                
                Dictionary<string, decimal> goodsRate = GetGoodsRate(db, user);
                GoodsInfo[] ginfos = serializer.Deserialize<GoodsInfo[]>(row["货物信息"].ToString());

                var sendWay = TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]).IsEmptyObject() ? null : "快递";
                string remark = GetRemark(row);
                decimal pall = Decimal.Parse(row["拍下总金额"].ToString());
                decimal total = Decimal.Parse(row["支付金额"].ToString());
                decimal ltotal = 0;
                decimal btotal = 0;
                decimal allPrice = 0;

                string address = TaobaoDataHelper.ReplaceHtmlText(row["具体地址"].ToString());
                if (!dicDetail.Contains(ddid))
                {
                    for (int j = ginfos.Length - 1; j >= 0; j--)
                    {
                        GoodsInfo ginfo = ginfos[j];
                        string goodKey = GetGoodsKey(ginfo.Color, ginfo.Size, ginfo.Title);
                        decimal rate = 0;
                        if (db != null && (goodsRate == null || !goodsRate.ContainsKey(goodKey)))
                        {
                            if (!sureRate)
                            {
                                rate = 0;
                            }
                            else
                            {
                                throw new Exception(string.Format("【{3}】   color:{0} size:{1} title:{2}没有设置比例goodMatchRate", ginfo.Color, ginfo.Size, ginfo.Title, user));
                            }
                        }
                        else
                        {
                            if(db != null)
                            {
                                rate = goodsRate[goodKey];
                            }
                        }
                        decimal price = ginfo.PriceInfo / pall * total;
                        if (j == 0)
                        {
                            price = total - allPrice;
                        }
                        allPrice += price;
                        decimal tbtotal = (price * rate) * (decimal)(0.01);
                        btotal += tbtotal;
                        decimal tltotal = price - tbtotal;
                        HashObject detailHash = new HashObject();
                        detailList.Add(detailHash);
                        detailHash.Add("id", Cuid.NewCuid().GetHashCode());
                        detailHash.Add("bid", id);
                        detailHash.Add("code", ddid);
                        detailHash.Add("size", ginfo.Size);
                        detailHash.Add("amount", ginfo.Amount);
                        detailHash.Add("color", ginfo.Color);
                        detailHash.Add("address", address);
                        detailHash.Add("area", row["区域"]);
                        detailHash.Add("total", price);
                        detailHash.Add("remark", remark);
                        detailHash.Add("ltotal", tltotal);
                        detailHash.Add("sourceTitle", ginfo.Title);
                        detailHash.Add("goodsstatus", int.Parse(row["发货状态status"].ToString()) >= 1 ? 2 : 1);
                        detailHash.Add("sendway", sendWay);
                        detailHash.Add("btotal", tbtotal);
                    }
                    ltotal = total - btotal;
                }

                if (IsNullDate(row["付款时间"]))
                {
                    continue;
                }

                HashObject billHash = new HashObject();
                billHash.Add("id", id);
                billHash.Add("date", row["付款时间"]);
                billHash.Add("taobaocode", row["旺旺名称"]);
                billHash.Add("cname", row["收货客户"]);
                billHash.Add("ctel", row["联系电话"]);
                billHash.Add("caddress", address);
                billHash.Add("carea", row["区域"]);
                billHash.Add("cremark", remark);
                billHash.Add("ltotal", ltotal);
                billHash.Add("status", row["发货状态status"]);
                billHash.Add("scode", TaobaoDataHelper.GetLogisticsInfo(row["物流单号"]));
                billHash.Add("sname", TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]));
                billHash.Add("uid", GetUser(row["所属用户"], db == null));
                billHash.Add("goodsstatus", 1);
                billHash.Add("billfrom", "抓取");
                billHash.Add("createdate", row["创建时间"]);
                billHash.Add("zfbpaycode", row["支付宝交易号"]);
                billHash.Add("tbcode", ddid);
                billHash.Add("total", total);
                billHash.Add("btotal", btotal);
                billHash.Add("senddate", GetDate(sendDate));
                billHash.Add("successdate", GetDate(successDate));
                billList.Add(billHash);
            }
        }

        private static string GetDate(object date)
        {
            return IsNullDate(date) ? "null" : date.ToString();
        }

        private static int GetUser(object user, bool isDemo)
        {
            if (isDemo)
            {
                return 999;
            }
            string tempUser = user.ToString();
            if (tempUser == "ljbbean")
            {
                return 2;
            }
            if (tempUser == "annychenzy")
            {
                return 1;
            }

            if (tempUser == "风灵415743757")
            {
                return 3;
            }
            return -1;
        }
    }
}