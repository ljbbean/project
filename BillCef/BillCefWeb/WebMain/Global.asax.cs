using System;
using System.Web;
using Carpa.Web.Common;
using WebMain.DataHandler;

namespace WebMain
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
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