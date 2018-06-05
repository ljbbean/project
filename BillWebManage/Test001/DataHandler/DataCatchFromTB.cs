using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001.DataHandler
{
    [NeedLogin]
    public class DataCatchFromTB : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            Context["socketurl"] = "http://localhost:8080";
            Context["uid"] = user.Name;
        }
    }
}
