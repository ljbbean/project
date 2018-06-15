using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Collections.Generic;

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
        
        public static void SetTempData(string key ,HashObject hash)
        {
            tempDictionary[key] = hash;
        }

        private static Dictionary<string, HashObject> tempDictionary = new Dictionary<string, HashObject>();
        [WebMethod]
        public HashObject GetTempData(string id)
        {
            HashObject value;
            if (tempDictionary.TryGetValue(id, out value))
            {
                tempDictionary.Remove(id);
                return value;
            }
            value = new HashObject();
            value.Add("detail", new HashObjectList());
            value.Add("bill", new HashObjectList());
            return value;
        }
    }
}
