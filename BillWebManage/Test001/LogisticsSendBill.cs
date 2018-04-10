using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001
{
    public class LogisticsSendBill : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            var list = new StockBill.list(new Login.User() { Id = user.Id, Power = user.Power, Name = user.Name }, true);
            IHashObject item = new HashObject();
            item["kind"] = "2";
            list.SetQueryParams(item);
            Context["list"] = list;
        }

        [WebMethod]
        public void Sure(int id)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    db.AddParameter("id", id);
                    int i = db.ExecuteNonQuerySQL("update bill set status = 2, goodsstatus=1 where id=@id and status < 2");
                    if (i == 1)
                    {
                        db.AddParameter("id", id);
                        db.ExecuteNonQuerySQL("update billdetail set goodsstatus = 2 where bid in (select id from bill where id=@id)");
                    }
                    db.CommitTransaction();
                }
                catch (Exception e)
                {
                    db.RollbackTransaction();
                    throw e;
                }
            }
        }
    }
}
