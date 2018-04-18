using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaoBaoData
{
    public partial class CollectionForm : Form
    {
        public CollectionForm()
        {
            InitializeComponent();
        }

        private void CollectionForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string condition = "镜子";
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                condition = textBox1.Text;
            }
            try
            {
                Search search = new Search();
                dataGridView1.DataSource = search.GetMainDataMore(condition);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    return;
                }
                Search search = new Search();

                string url = dataGridView1.SelectedRows[0].Cells["detail_url"].Value.ToString();
                if (!url.ToLower().Trim().StartsWith("http"))
                {
                    url = string.Format("https:{0}", url);
                }
                Goods goods = (Goods)search.GetGoodMsg(url);
                dataGridView2.DataSource = goods.Skus;
                textBox1.Text = goods.SendCity;
                textBox2.Text = goods.ConfirmGoodsCount.ToString();
                textBox3.Text = goods.SoldTotalCount.ToString();
                textBox4.Text = goods.Pays;
                textBox5.Text = goods.Service;
                textBox6.Text = goods.Coupon;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }
    }
}
