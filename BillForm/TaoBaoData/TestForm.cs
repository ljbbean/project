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
using Common;
using System.Collections;
using System.IO;

namespace TaoBaoData
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                using (Stream stream = openFileDialog1.OpenFile())
                {
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    textBox1.Text = text;
                }
            }

            string content = textBox1.Text;
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = serializer.Deserialize<HashObject>(content);
            string[] keys = {
                                "mods/itemlist/data/auctions"
                            };
            var list = hash.GetHashValue(keys);
            var auctions = list[0].GetValue<ArrayList>("auctions");
            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("raw_title");
            table.Columns.Add("pic_url");
            table.Columns.Add("detail_url");
            table.Columns.Add("item_loc");
            table.Columns.Add("view_sales", typeof(decimal));
            table.Columns.Add("nick");
            table.Columns.Add("content");

            foreach (HashObject obj in auctions)
            {
                DataRow row = table.NewRow();
                row["title"] = obj.GetValue<string>("title");
                row["raw_title"] = obj.GetValue<string>("raw_title");
                row["pic_url"] = obj.GetValue<string>("pic_url");
                row["detail_url"] = obj.GetValue<string>("detail_url");
                row["item_loc"] = obj.GetValue<string>("item_loc");
                string sales = obj.GetValue<string>("view_sales");
                row["view_sales"] = decimal.Parse( sales.Substring(0, sales.Length - 3));
                row["nick"] = obj.GetValue<string>("nick");
                table.Rows.Add(row);
            }
            dataGridView1.DataSource = table;
        }
    }
}
