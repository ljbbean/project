using System;
using System.Web;
using Carpa.Web.Common;
using Carpa.Web.Script;
using Carpa.Web.Script.UI;
using System.Threading;

namespace Test001
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Cuid.HiMode = CuidHiMode.Carpa; // Cuid的Hi生成默认使用Carpa机器码（必须指定）
            Carpa.Web.Common.WebUtils.StopMonitorDataDirs();  // 防止bin目录以外的目录如data目录变动导致应用重启
            Settings.ClientHeightContextName = "ClientHeight";
            Settings.ClientWidthContextName = "ClientWidth";
            ThemeManager.RegisterTheme("beefun", typeof(Global).Assembly, "skins/beefun/");
            Page.RegisterLoginPage("Login.gspx", "user");
            Thread thread = new Thread(CacthConfig.DataCatch);
            thread.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Carpa.Web.Common.WebUtils.LogApplicationError();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Carpa.Web.Common.WebUtils.LogApplicationEnd();
        }
    }
}