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

namespace Test001
{
    internal delegate void CallBackMsg(string msg);

    public class DataCatchSave
    {
        private static Dictionary<string, Dictionary<string, decimal>> userGoodsDictionary = new Dictionary<string, Dictionary<string, decimal>>();
        
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
            return date == null || date.ToString().Length == 0;
        }

        private static string GetRemark(DataRow row)
        {
            StringBuilder sbuilder = new StringBuilder();
            var mjly = row["买家留言"];
            int index = 0;
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(ReplaceHtmlText(string.Format("【买家留言：{0}】", mjly)));
                index++;
            }

            mjly = row["卖家留言"];
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(ReplaceHtmlText(string.Format("【卖家留言：{0}】", mjly)));
            }
            return sbuilder.ToString();
        }

        private static string ReplaceHtmlText(string str)
        {
            return str.Replace("&times;", "*").Replace("&middot;", "。").Replace("&mdash;", "—");
        }
        

        private static Dictionary<string, string> GetExistBills(DbHelper db, string sql)
        {
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

        internal static string SaveData(CallBackMsg callBack, bool onlyAdd = true)
        {
            DataTable table = TaoBaoRequest.TaobaoDataHelper.GetDetailData(AppUtils.ConnectionString, true);
            if(table.Rows.Count == 0)
            {
                return "OK:没有需要分析的数据";
            }
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            using (DbHelper db = new DbHelper(AppUtils.ConnectionString, true))
            {
                try
                {
                    string ids = GetTaobaoBillID(table);
                    Dictionary<string, string> dic = GetExistBills(db, ids);
                    List<string> dicDetail = GetExistBillDetails(db, ids);

                    StringBuilder insertBillBuilder = new StringBuilder(@"insert into bill(id, date, taobaocode,cname,ctel,caddress,carea,cremark,
                        ltotal,status, scode, sname, uid, goodsstatus, billfrom, createdate, zfbpaycode,tbcode, total, btotal, senddate, successdate) values");
                    StringBuilder insertBillDetailBuilder = new StringBuilder(@"insert into billdetail(id, bid, code, size, amount, color, address,area,total, remark, 
                        ltotal,sourceTitle,goodsstatus,sendway, btotal) values");
                    int count = 0;
                    int detailCount = 0;
                    callBack("开始分析数据");
                    callBack("开始构建数据");
                    StringBuilder doedIds = new StringBuilder();//影响到的主数据
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
                        doedIds.AppendFormat("{0},", id);
                        object sendDate = row["发货时间"];
                        object successDate = row["成交时间"];

                        StringBuilder sformate = new StringBuilder("({0}, '{1}', '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, {9},'{10}', '{11}', {12}, {13}, '{14}','{15}', '{16}', '{17}', {18},{19} ");
                        sformate.Append(IsNullDate(sendDate) ? ",{20}" : ",'{20}'");
                        sformate.Append(IsNullDate(successDate) ? ",{21}" : ",'{21}'");
                        sformate.Append("), ");

                        count++;
                        Dictionary<string, decimal> goodsRate = GetGoodsRate(db, user);
                        GoodsInfo[] ginfos = serializer.Deserialize<GoodsInfo[]>(row["货物信息"].ToString());

                        var sendWay = TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]).IsEmptyObject() ? null : "快递";
                        string remark = GetRemark(row);
                        decimal pall = Decimal.Parse(row["拍下总金额"].ToString());
                        decimal total = Decimal.Parse(row["支付金额"].ToString());
                        decimal ltotal = 0;
                        decimal btotal = 0;
                        decimal allPrice = 0;

                        string address = ReplaceHtmlText(row["具体地址"].ToString());
                        if (!dicDetail.Contains(ddid))
                        {
                            detailCount++;
                            for (int j = ginfos.Length - 1; j >= 0; j--)
                            {
                                GoodsInfo ginfo = ginfos[j];
                                string goodKey = GetGoodsKey(ginfo.Color, ginfo.Size, ginfo.Title);
                                if (goodsRate == null || !goodsRate.ContainsKey(goodKey))
                                {
                                    throw new Exception(string.Format("【{3}】   color:{0} size:{1} title:{2}没有设置比例", ginfo.Color, ginfo.Size, ginfo.Title, user));
                                }
                                decimal price = ginfo.PriceInfo / pall * total;
                                if (j == 0)
                                {
                                    price = total - allPrice;
                                }
                                allPrice += price;
                                decimal tbtotal = (price * goodsRate[goodKey]) * (decimal)(0.01);
                                btotal += tbtotal;
                                decimal tltotal = price - tbtotal;
                                string sDetailFormate = "({0}, {1}, '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, '{9}',{10}, '{11}', {12}, '{13}', {14}),";
                                //构建明细数据
                                insertBillDetailBuilder.AppendFormat(sDetailFormate, Cuid.NewCuid().GetHashCode(), id, ddid, ginfo.Size, ginfo.Amount, ginfo.Color, address, row["区域"], price, remark, tltotal, ginfo.Title, int.Parse(row["发货状态status"].ToString()) >= 1 ? 2 : 1, sendWay, tbtotal);
                            }
                            ltotal = total - btotal;
                        }

                        if (IsNullDate(row["付款时间"]))
                        {
                            continue;
                        }
                        //构建主表数据，如果已经存在，直接更改数据
                        insertBillBuilder.AppendFormat(sformate.ToString(), id, row["付款时间"], row["旺旺名称"], row["收货客户"], row["联系电话"], address, row["区域"]
                            , remark, ltotal, row["发货状态status"], TaobaoDataHelper.GetLogisticsInfo(row["物流单号"]), TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]), GetUser(row["所属用户"]), 1, "抓取"
                            , row["创建时间"], row["支付宝交易号"], ddid, total, btotal, GetDate(sendDate), GetDate(successDate));
                    }
                    callBack("构建数据完毕");
                    callBack("开始保存数据");
                    string insertBill = insertBillBuilder.ToString();
                    insertBill = insertBill.Substring(0, insertBill.Length - 2);
                    string insertBillDetail = insertBillDetailBuilder.ToString();
                    insertBillDetail = insertBillDetail.Substring(0, insertBillDetail.Length - 1);
                    if (count != 0)
                    {
                        db.BeginTransaction();
                        db.BatchExecute(string.Format("update tbill set hasupdate=0 where bid in {0}", ids));
                        db.BatchExecute(string.Format("{0} on duplicate key update `createdate`=values(`createdate`),`senddate`=values(`senddate`),`successdate`=values(`successdate`),`zfbpaycode`=values(`zfbpaycode`),`status`=values(`status`),`sname`=values(`sname`),`scode`=values(`scode`);", insertBill));
                        if (detailCount != 0)
                        {
                            db.BatchExecute(insertBillDetail);//直接新增，不修改
                        }
                        //后期退款的单据，金额都为0
                        db.BatchExecute(string.Format("update bill set ltotal = 0, total=0, btotal=0 where status=9 and id in ({0})", doedIds.ToString().Substring(0, doedIds.Length - 1)));
                        db.CommitTransaction();
                    }
                    return string.Format("OK:数据保存成功，分析处理了{0}条数据", count);
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

        private static string GetDate(object date)
        {
            return IsNullDate(date) ? "null" : date.ToString();
        }

        private static int GetUser(object user)
        {
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
            throw new Exception("不存在的用户");
        }
    }
}