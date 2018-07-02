using System;
using System.Web;
using Carpa.Web.Common;
using WebMain.DataHandler;
using Carpa.Web.Script;
using Carpa.Web.Script.UI;

namespace WebMain
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Page.RegisterLoginPage("Default.gspx", "user");

            ThemeManager.RegisterTheme("cef", typeof(Global).Assembly, "skins/travel/");
            
            IOUtils.Init();
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        void Application_Error(object sender, EventArgs e)
        {
        }
    }
}