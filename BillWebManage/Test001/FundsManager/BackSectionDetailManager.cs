using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001.FundsManager
{
    [NeedLogin]
    public class BackSectionDetailManager : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public IHashObject GetTotal(string month)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                return db.SelectFirstRow(string.Format("select total, paytotal from backsection where month='{0}-1'", month));
            }
        }

        [WebMethod]
        public void Save(IHashObject data)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                IHashObject section = db.SelectFirstRow(string.Format("select id, paytotal from backsection where month='{0}-1'", data["tmonth"]));
                if(section == null)
                {
                    throw new Exception(string.Format("{0}月不存在回款金额", data["tmonth"]));
                }
                data["datetime"] = DateTime.Now;
                data["id"] = Cuid.NewCuid();
                data["uid"] = ((Login.User)Session["user"]).Id;
                data.Remove("tmonth");
                section["paytotal"] = section.GetValue<decimal>("paytotal") + data.GetValue<decimal>("total");
                try
                {
                    db.BeginTransaction();
                    db.Insert("backsectionDetail", data);
                    db.Update("backsection", "id", section);//更新金额
                    db.CommitTransaction();
                }
                catch(Exception e)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                }

            }
        }
    }
}
