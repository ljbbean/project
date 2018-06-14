using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001.DataHandler
{
    public class DataCatchFromTBExample : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            Context["socketurl"] = "http://localhost:8080";
            Context["uid"] = Cuid.NewCuid().ToString();
            
        }

        private string GetName(string name)
        {
            name = name.Trim().ToLower();
            if(name == "ljb")
            {
                return "ljbbean";
            }
            if(name == "cy")
            {
                return "annychenzy";
            }
            return null;
        }
    }
}
