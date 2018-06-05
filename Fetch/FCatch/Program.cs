using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fiddler;

namespace FCatch
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DataCatch());
            }
            catch (Exception e)
            {
                FiddlerApplication.Shutdown();
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            FiddlerApplication.Shutdown();
        }
    }
}
