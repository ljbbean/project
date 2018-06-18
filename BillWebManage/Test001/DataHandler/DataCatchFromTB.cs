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
            Context["socketurl"] = IOUtils.SocketIOUrl;
            Context["uid"] = GetName(user.Name);
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
