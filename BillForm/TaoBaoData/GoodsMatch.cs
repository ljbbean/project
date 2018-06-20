using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Carpa.Web.Ajax;
using Carpa.Web.Script;
using TaoBaoRequest;

namespace TaoBaoData
{
    public partial class GoodsMatch : Form
    {
        private DataTable changedTable;
        private bool isAdd = true;
        public GoodsMatch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable table = TaobaoDataHelper.SpliteContentToDataTableByUser("ljbbean", Utils.Connect, true);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            isAdd = true;
            using (DbHelper db = new DbHelper(Utils.Connect, true))
            {
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

                IHashObjectList commList = db.Select("select color,size,sourceTitle, uname, rate from commissionrate");
                foreach (HashObject commItem in commList)
                {
                    CommissionRateStruct crstruct = new CommissionRateStruct();
                    crstruct.Color = commItem.GetValue<string>("color");
                    crstruct.Size = commItem.GetValue<string>("size");
                    crstruct.SourceTitle = commItem.GetValue<string>("sourcetitle");
                    crstruct.User = commItem.GetValue<string>("uname");
                    if (list.Contains(crstruct))
                    {
                        list.Remove(crstruct);
                    }
                }

                changedTable = new DataTable();
                changedTable.Columns.Add("color");
                changedTable.Columns.Add("size");
                changedTable.Columns.Add("sourceTitle");
                changedTable.Columns.Add("uname");
                changedTable.Columns.Add("rate");
                changedTable.Columns.Add("crid");
                foreach (CommissionRateStruct item in list)
                {
                    DataRow row = changedTable.NewRow();
                    row["color"] = item.Color;
                    row["size"] = item.Size;
                    row["sourceTitle"] = item.SourceTitle;
                    row["uname"] = item.User;
                    row["rate"] = item.Rate;
                    changedTable.Rows.Add(row);
                }
                dataGridView1.DataSource = changedTable;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var data = dataGridView1.DataSource as DataTable;
            if (data.Rows.Count == 0)
            {
                return;
            }
            using (DbHelper db = new DbHelper(Utils.Connect, true))
            {
                if (isAdd)
                {
                    StringBuilder sbuilder = new StringBuilder("insert into commissionrate (crid,color,size,sourceTitle,rate,uname) values");
                    foreach (DataRow row in data.Rows)
                    {
                        sbuilder.AppendFormat("({0},'{1}','{2}','{3}',{4},'{5}'),", Cuid.NewCuid(), row["color"], row["size"], row["sourceTitle"], row["rate"], row["uname"]);
                    }
                    db.BatchExecute(sbuilder.ToString().Substring(0, sbuilder.Length - 1));
                }
                else
                {
                    try
                    {
                        db.BeginTransaction();
                        string sql = "update commissionrate set rate=@rate where crid=@crid";
                        foreach (DataRow row in data.Rows)
                        {
                            db.AddParameter("rate", row["rate"]);
                            db.AddParameter("crid", row["crid"]);
                            db.ExecuteIntSQL(sql);
                        }
                        db.CommitTransaction();
                    }
                    catch (Exception t)
                    {
                        if (db.HasBegunTransaction)
                        {
                            db.RollbackTransaction();
                        }
                    }
                }
            }
            MessageBox.Show("数据操作成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DataTable table = TaobaoDataHelper.GetDetailData(Utils.Connect);
            //JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            //isAdd = false;
            //using (DbHelper db = new DbHelper(Utils.Connect, true))
            //{
            //    DataTable ctable = db.ExecuteSQL("select * from commissionrate");

            //    dataGridView1.DataSource = ctable;
            //}
        }
        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }
    }
}
