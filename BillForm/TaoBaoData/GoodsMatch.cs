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
        public GoodsMatch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (changedTable != null && changedTable.GetChanges() != null && changedTable.GetChanges().Rows.Count > 0 && MessageBox.Show("已经有修改，是否放弃修改？", "是否放弃", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            //{
            //    return;
            //}
            DataTable table = TaobaoDataHelper.GetDetailData();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

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

                changedTable = new DataTable();
                changedTable.Columns.Add("color");
                changedTable.Columns.Add("size");
                changedTable.Columns.Add("sourceTitle");
                changedTable.Columns.Add("uname");
                changedTable.Columns.Add("rate");
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
                StringBuilder sbuilder = new StringBuilder("insert into commissionrate (crid,color,size,sourceTitle,rate,uname) values");
                foreach (DataRow row in data.Rows)
                {
                    sbuilder.AppendFormat("({0},'{1}','{2}','{3}',{4},'{5}'),", Cuid.NewCuid(), row["color"], row["size"], row["sourceTitle"], row["rate"], row["uname"]);
                }
                db.BatchExecute(sbuilder.ToString().Substring(0, sbuilder.Length - 1));
            }
            MessageBox.Show("数据操作成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable table = TaobaoDataHelper.GetDetailData();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            using (DbHelper db = new DbHelper(Utils.Connect, true))
            {
                DataTable ctable = db.ExecuteSQL("select * from commissionrate");

                dataGridView1.DataSource = changedTable = ctable;
            }
        }
    }
}
