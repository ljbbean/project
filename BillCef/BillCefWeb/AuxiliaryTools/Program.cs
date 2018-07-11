using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace AuxiliaryTools
{
    class Program
    {
        static void Main(string[] args)
        {
            ToolsIOUtils.Init();
            Console.Write("开启商品工具");
            Application.EnableVisualStyles();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Console.Write(string.Format("商品工具已退出, {0}", e));
        }
    }
}