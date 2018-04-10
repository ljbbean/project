using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001
{
    public class AffiliateBill : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public void Save(IHashObject data)
        {
             using (DbHelper db = AppUtils.CreateDbHelper())
             {
                 data["id"] = Guid.NewGuid().GetHashCode();
                 data["date"] = DateTime.Now;
                 db.Insert("affiliatedbill", data);
             }
        }
    }
}
