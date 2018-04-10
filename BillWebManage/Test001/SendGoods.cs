using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001
{
    public class SendGoods : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public void SendGood(int id, string name ,string code)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    db.AddParameter("id", id);
                    db.AddParameter("name", name);
                    db.AddParameter("code", code);
                    db.ExecuteIntSQL("update bill set status = 1,sname=@name,scode=@code where id=@id");
                    db.AddParameter("id", id);
                    db.ExecuteIntSQL("update billdetail set goodsstatus = 2 where bid in (select id from bill where id=@id)");
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
