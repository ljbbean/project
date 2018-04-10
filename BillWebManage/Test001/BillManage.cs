using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Web.Validation.Validators;

namespace Test001
{
    public class BillManage : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            IHashObject data = new HashObject();
            data["date"] = DateTime.Now;
            Context["detail"] = data;
        }

        [WebMethod]
        public void DoSave(IHashObject data)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    int id = Guid.NewGuid().GetHashCode();
                    db.BeginTransaction();
                    object[] list = data.GetValue<object[]>("grid");
                    for (int i = 0; i < list.Length; i++)
                    {
                        IHashObject item = (IHashObject)list[i];
                        item["id"] = Guid.NewGuid().GetHashCode();
                        item["bid"] = id;
                        db.Insert("billdetail", item);
                    }
                    data.Remove("grid");
                    data["id"] = id;
                    db.Insert("bill", data);
                    db.CommitTransaction();
                }
                catch(Exception e)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    throw e;
                }
            }
        }
    }
}