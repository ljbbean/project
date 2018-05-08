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
        private static Dictionary<string, Dictionary<string, decimal>> userGoodsDictionary = new Dictionary<string, Dictionary<string, decimal>>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowHeadersWidth = 50;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = TaobaoDataHelper.GetDetailData();
        }


        

        //operationsGuide/layout/lines/display/content
        

        
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            DataTable table = TaobaoDataHelper.GetDetailData();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            using (DbHelper db = new DbHelper(Utils.Connect, true))
            {
                try
                {
                    string sql = GetTaobaoBillID(table);
                    Dictionary<string, string> dic = GetExistBills(db, sql);
                    List<string> dicDetail = GetExistBillDetails(db, sql);

                    StringBuilder insertBillBuilder = new StringBuilder(@"insert into bill(id, date, taobaocode,cname,ctel,caddress,carea,cremark,
                        ltotal,status, scode, sname, uid, goodsstatus, billfrom, createdate, zfbpaycode,tbcode, total, btotal, senddate, successdate) values");
                    StringBuilder insertBillDetailBuilder = new StringBuilder(@"insert into billdetail(id, bid, code, size, amount, color, address,area,total, remark, 
                        ltotal,sourceTitle,goodsstatus,sendway, btotal) values");
                    int count = 0;
                    int detailCount = 0;
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

                        StringBuilder sformate = new StringBuilder("({0}, '{1}', '{2}', '{3}', '{4}','{5}', '{6}', '{7}', {8}, {9},'{10}', '{11}', {12}, {13}, '{14}','{15}', '{16}', '{17}', {18},{19} ");
                        sformate.Append(IsNullDate(sendDate) ? ",{20}" : ",'{20}'");
                        sformate.Append(IsNullDate(successDate) ? ",{21}" : ",'{21}'");
                        sformate.Append("), ");

                        count++;
                        Dictionary<string, decimal> goodsRate = GetGoodsRate(db, row["所属用户"].ToString());
                        GoodsInfo[] ginfos = serializer.Deserialize <GoodsInfo[]>(row["货物信息"].ToString());

                        var sendWay = TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]).IsEmptyObject() ? null : "快递";
                        string remark = GetRemark(row);
                        decimal pall = Decimal.Parse(row["拍下总金额"].ToString());
                        decimal total = Decimal.Parse(row["支付金额"].ToString());
                        decimal ltotal = 0;
                        decimal btotal = 0;
                        decimal allPrice = 0;

                        if (!dicDetail.Contains(ddid))
                        {
                            detailCount++;
                            for (int j = ginfos.Length - 1; j >= 0; j--)
                            {
                                GoodsInfo ginfo = ginfos[j];
                                string goodKey = GetGoodsKey(ginfo.Color, ginfo.Size, ginfo.Title);
                                if (goodsRate == null || !goodsRate.ContainsKey(goodKey))
                                {
                                    throw new Exception(string.Format("color:{0} size:{1} title:{2}没有设置比例", ginfo.Color, ginfo.Size, ginfo.Title));
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
                                insertBillDetailBuilder.AppendFormat(sDetailFormate, Cuid.NewCuid().GetHashCode(), id, ddid, ginfo.Size, ginfo.Amount, ginfo.Color, row["具体地址"], row["区域"], price, remark, tltotal, ginfo.Title, int.Parse(row["发货状态status"].ToString()) >= 1 ? 2 : 1, sendWay, tbtotal);
                            }
                            ltotal = total - btotal;
                        }

                        //构建主表数据，如果已经存在，直接更改数据
                        insertBillBuilder.AppendFormat(sformate.ToString(), id, row["付款时间"], row["旺旺名称"], row["收货客户"], row["联系电话"], row["具体地址"], row["区域"]
                            , remark, ltotal, row["发货状态status"], TaobaoDataHelper.GetLogisticsInfo(row["物流单号"]), TaobaoDataHelper.GetLogisticsInfo(row["快递公司"]), GetUser(row["所属用户"]), 1, "抓取"
                            , row["创建时间"], row["支付宝交易号"], ddid, total, btotal, GetDate(sendDate), GetDate(successDate));

                    }
                    string insertBill = insertBillBuilder.ToString();
                    insertBill = insertBill.Substring(0, insertBill.Length - 2);
                    string insertBillDetail = insertBillDetailBuilder.ToString();
                    insertBillDetail = insertBillDetail.Substring(0, insertBillDetail.Length - 1);
                    if (count != 0)
                    {
                        db.BeginTransaction();
                        db.BatchExecute(string.Format("{0} on duplicate key update `createdate`=values(`createdate`),`senddate`=values(`senddate`),`successdate`=values(`successdate`),`zfbpaycode`=values(`zfbpaycode`),`status`=values(`status`);", insertBill));
                        if (detailCount != 0)
                        {
                            db.BatchExecute(insertBillDetail);//直接新增，不修改
                        }
                        db.CommitTransaction();
                    }
                    MessageBox.Show(string.Format("处理了{0}条数据", count));
                }
                catch (Exception e1)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    MessageBox.Show(string.Format(e1.Message));
                }
            }
        }

        private static Dictionary<string, decimal> GetGoodsRate(DbHelper db,string uname)
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

        private static string GetGoodsKey(string color, string size, string sourceTitle)
        {
            return string.Format("{0}_{1}_{2}", color, size, sourceTitle);
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
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("未指定请求的cookies");
            }
            try
            {
                DataCatch dataCatch = new DataCatch();
                DateTime time = dateTimePicker1.Value;
                DateTime newTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
                string list = string.Format("列表插入数据：{0}", dataCatch.GetData(newTime, textBox1.Text));
                SendDetailState detailState = new SendDetailState(ShowMessage);
                dataCatch.GetDetailsData(textBox1.Text, detailState);
            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message);
            }
        }

        private void ShowMessage(string message)
        {
            Action<String> AsyncUIDelegate = delegate(string n) { label2.Text = n; };
            label2.Invoke(AsyncUIDelegate, new object[] { message });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GoodsMatch match = new GoodsMatch();
            match.ShowDialog();
            userGoodsDictionary.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CollectionForm form = new CollectionForm();
            form.ShowDialog();
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);  
        }
    }
}
