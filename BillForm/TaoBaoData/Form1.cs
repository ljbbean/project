using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Carpa.Web.Script;
using Test001;
using TaoBaoRequest;
using Carpa.Web.Ajax;
using System.Collections;

namespace TaoBaoData
{
    public partial class Form1 : Form
    {
        private string connect = "server=localhost;database=bill;User ID=root;Password=dev;Charset=utf8; Allow Zero Datetime=True;OldSyntax=true;port=3306;Character Set=utf8";
        private string[] users = { "ljbbean", "annychenzy", "风灵415743757" };
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetDetailData();
        }

        private DataTable GetDetailData()
        {
            string[] dusers = users;

            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            DataTable table = MessageTable();

            foreach (string duser in dusers)
            {
                IHashObjectList list = new HashObjectList();
                using (DbHelper db = new DbHelper(connect, true))
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
                    row["发货时间"] = tempOrderInfoList.GetValue<string>("发货时间:", null);
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

        private object GetKeyObject(List<TaoBaoRequest.HashObjectExt.KeyValue> list, string key)
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

        //operationsGuide/layout/lines/display/content
        private string GetSaleMessage(object obj)
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
                    if(!"block".Equals(lhash.GetValue<string>("display")))
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

        private class GoodsInfo
        {
            public string Title { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public string Amount { get; set; }
            public string Lockers { get; set; }
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
                        if("颜色分类：".Equals(tHash.GetValue<string>("name")))
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
                    if (items[i].IndexOf('省') >= 0 || items[i].IndexOf('区') >=0)
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
                rt.Add("快递公司", GetLogisticsInfo( content.GetValue<string>("logisticsName")));//快递公司
                var logisticsInfo = GetLogisticsInfo(content.GetValue<string>("logisticsNum"));
                rt.Add("物流单号", logisticsInfo);//物流单号
                rt.Add("发货状态", string.IsNullOrEmpty(logisticsInfo) ? "未发货" : "已发货");
                rt.Add("发货状态status", string.IsNullOrEmpty(logisticsInfo) ? "0" : "1");
                return rt;
            }
            return null;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DataTable table = GetDetailData();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            using (DbHelper db = new DbHelper(connect, true))
            {
                string sql = GetTaobaoBillID(table);
                Dictionary<string, string> dic = GetExistBills(db, sql);
                List<string> dicDetail = GetExistBillDetails(db, sql);

                StringBuilder insertBillBuilder = new StringBuilder(@"insert into bill(id, date, taobaocode,cname,ctel,caddress,carea,cremark,
                    ltotal,status, scode, sname, uid, goodsstatus, billfrom, createdate, zfbpaycode,tbcode, senddate, successdate) values");
                StringBuilder insertBillDetailBuilder = new StringBuilder(@"insert into billdetail(id, bid, code, size, amount, color, address,area,total, remark, 
                    ltotal,sourceTitle,goodsstatus,sendway) values");
                foreach (DataRow row in table.Rows)
                {
                    var ddid = row["订单ID"].ToString();
                    string id = Cuid.NewCuid().GetHashCode().ToString();
                    string oldid;
                    if (dic.TryGetValue(ddid, out oldid))
                    {
                        id = oldid;
                    }

                    object sendDate = row["发货时间"];
                    object successDate = row["成交时间"];

                    StringBuilder sformate = new StringBuilder("({0}, '{1}', '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, {9},'{10}', '{11}', {12}, {13}, '{14}','{15}', '{16}', '{17}' ");
                    sformate.Append(IsNullDate(sendDate) ? ",{18}" : ",'{18}'");
                    sformate.Append(IsNullDate(successDate) ? ",{19}" : ",'{19}'");
                    sformate.Append("), ");

                    string remark = GetRemark(row);
                    insertBillBuilder.AppendFormat(sformate.ToString(), id, row["付款时间"], row["旺旺名称"], row["收货客户"], row["联系电话"], row["具体地址"], row["区域"]
                        , remark, row["支付金额"], row["发货状态status"], GetLogisticsInfo(row["物流单号"]), GetLogisticsInfo(row["快递公司"]), GetUser(row["所属用户"]), 1, "抓取"
                        , row["创建时间"], row["支付宝交易号"], ddid, GetDate(sendDate), GetDate(successDate));

                    if (dicDetail.Contains(ddid))
                    {
                        continue;
                    }
                    GoodsInfo[] ginfos = serializer.Deserialize <GoodsInfo[]>(row["货物信息"].ToString());

                    var sendWay = GetLogisticsInfo(row["快递公司"]).IsEmptyObject() ? null : "快递";
                    foreach (GoodsInfo ginfo in ginfos)
                    {
                        string sDetailFormate = "({0}, {1}, '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, '{9}',{10}, '{11}', {12}, '{13}'),";
                        insertBillDetailBuilder.AppendFormat(sDetailFormate, Cuid.NewCuid().GetHashCode(), id, ddid, ginfo.Size, ginfo.Amount, ginfo.Color, row["具体地址"], row["区域"], row["支付金额"], remark, row["支付金额"], ginfo.Title, int.Parse(row["发货状态status"].ToString()) >= 1 ? 2 : 1, sendWay);
                    }
                }
                string insertBill = insertBillBuilder.ToString();
                insertBill = insertBill.Substring(0, insertBill.Length - 2);
                string insertBillDetail = insertBillDetailBuilder.ToString();
                insertBillDetail = insertBillDetail.Substring(0, insertBillDetail.Length - 1);
                try
                {
                    db.BeginTransaction();
                    db.BatchExecute(string.Format("{0} on duplicate key update `createdate`=values(`createdate`),`senddate`=values(`senddate`),`successdate`=values(`successdate`),`zfbpaycode`=values(`zfbpaycode`);", insertBill));
                    db.BatchExecute(insertBillDetail);//直接新增，不修改
                    db.CommitTransaction();
                    MessageBox.Show(string.Format("处理了{0}条数据", table.Rows.Count));
                }
                catch (Exception e1)
                {
                    db.RollbackTransaction();
                    MessageBox.Show(string.Format(e1.Message));
                }
            }
        }

        private static string GetRemark(DataRow row)
        {
            StringBuilder sbuilder = new StringBuilder();
            var mjly = row["买家留言"];
            int index = 0;
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(string.Format("【买家留言：{0}】", mjly));
                index++;
            }

            mjly = row["卖家留言"];
            if (!mjly.IsEmptyObject())
            {
                sbuilder.Append(string.Format("【卖家留言：{0}】", mjly));
            }
            return sbuilder.ToString();
        }

        private int GetUser(object user)
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

        /// <summary>
        /// 格式化物流信息
        /// </summary>
        private static string GetLogisticsInfo(object obj)
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

        //获取订单状态
        private int GetBillStatus(object area, object sendDate, object successDate)
        {
            
            return 0;
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

        /// <summary>
        /// 获取已经存在淘宝单号的订单
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
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

        private string GetDate(object date)
        {
            return IsNullDate(date) ? "null" : date.ToString();
        }

        private bool IsNullDate(object date)
        {
            return date == null || date.ToString().Length == 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataCatch dataCatch = new DataCatch();
            DateTime time = dateTimePicker1.Value;
            DateTime newTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            string list =string.Format("列表插入数据：{0}", dataCatch.GetData(newTime, textBox1.Text));
            SendDetailState detailState = new SendDetailState(ShowMessage);
            dataCatch.GetDetailsData(textBox1.Text, detailState);
        }

        private void ShowMessage(string message)
        {
            Action<String> AsyncUIDelegate = delegate(string n) { label2.Text = n; };
            label2.Invoke(AsyncUIDelegate, new object[] { message });
        }

        private struct CommissionRateStruct
        {
            public string Color { get; set; }
            public string Size { get; set; }
            public string SourceTitle { get; set; }
            public string User { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable table = GetDetailData();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            
            using (DbHelper db = new DbHelper(connect, true))
            {
                //IHashObjectList obj = db.Select("select * from commissionrate");
                List<CommissionRateStruct> list = new List<CommissionRateStruct>();
                Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (DataRow row in table.Rows)
                {
                    GoodsInfo[] ginfos = serializer.Deserialize<GoodsInfo[]>(row["货物信息"].ToString());
                    
                    foreach (GoodsInfo ginfo in ginfos)
                    {
                        CommissionRateStruct crstruct = new CommissionRateStruct();
                        crstruct.Color = ginfo.Color;
                        crstruct.Size = ginfo.Size;
                        crstruct.SourceTitle = ginfo.Title;
                        crstruct.User = row["所属用户"].ToString();
                        if (list.Contains(crstruct))
                        {
                            continue;
                        }
                        list.Add(crstruct);
                    }
                }
                dataGridView1.DataSource = list;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CollectionForm form = new CollectionForm();
            form.ShowDialog();
        }
    }
}
