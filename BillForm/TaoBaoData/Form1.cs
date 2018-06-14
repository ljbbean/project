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
using Common;

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
            dataGridView1.DataSource = TaobaoDataHelper.GetDetailData(Utils.Connect);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show(DataCatchSave.SaveData().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("未指定请求的cookies");
                return;
            }
            try
            {
                //DataCatchRequest dataCatch = new DataCatchRequest(Utils.Connect);
                //DateTime time = dateTimePicker1.Value;
                //DateTime newTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
                //string list = string.Format("列表插入数据：{0}", dataCatch.GetData(newTime, textBox1.Text));
                //SendDetailState detailState = new SendDetailState(ShowMessage);
                //dataCatch.GetDetailsData(textBox1.Text, detailState);
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
