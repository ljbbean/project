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
        List<TaoBaoData.Search.GoodsAddress> list = new List<TaoBaoData.Search.GoodsAddress>();

        public CollectionForm()
        {
            InitializeComponent();
        }

        private void CollectionForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string condition = "显瘦镜";
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                condition = textBox1.Text;
            }
            try
            {
                Search search = new Search();
                var data = search.GetMainDataMore("test");
                return;
                string tempSearchString1 = System.Web.HttpUtility.UrlEncode(condition, Encoding.GetEncoding("utf-8"));
                list = (List<TaoBaoData.Search.GoodsAddress>) search.GetMainData(string.Format("https://s.taobao.com/search?q={0}&imgfile=&js=1&stats_click=search_radio_all%3A1&ie=utf8", tempSearchString1));
                dataGridView1.DataSource = list;
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
                string url = list[dataGridView1.SelectedRows[0].Index].detail_url;
                if (!url.ToLower().Trim().StartsWith("http"))
                {
                    url = string.Format("https:{0}", url);
                }
                dataGridView2.DataSource = search.GetGoodMsg(url);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }
    }
}
