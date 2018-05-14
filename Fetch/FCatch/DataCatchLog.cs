using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FCatch
{
    public partial class DataCatchLog : Form
    {
        public DataCatchLog()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            this.Text = title;
        }

        public void SendMessage(string text)
        {
            textBox1.Text =string.Format("{0}\r\n{1}",text, textBox1.Text);
        }
    }
}
