using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaoBaoRequest;
using System.IO;
using Carpa.Web.Ajax;

namespace FCatch
{
    public partial class TranResult : Form
    {
        public TranResult()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            textBox1.Text = openFileDialog1.FileName;
            
            using(StreamReader reader = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("gbk")))
            {
                string str = reader.ReadToEnd();
                textBox2.Text = str;
                try
                {
                    textBox3.Text = JavaScriptSerializer.CreateInstance().Serialize(NetDataHandler.ListDataTransformation(str));
                }
                catch (Exception e1)
                {
                    textBox3.Text = e1.Message;
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            textBox4.Text = openFileDialog1.FileName;

            using (StreamReader reader = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("gbk")))
            {
                string str = reader.ReadToEnd();
                textBox2.Text = str;
                try
                {
                    textBox3.Text = JavaScriptSerializer.CreateInstance().Serialize(NetDataHandler.DetailDataTransformation(str));
                }
                catch (Exception e1)
                {
                    textBox3.Text = e1.Message;
                }
            }
        }
    }
}
