using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;

namespace Test001.FundsManager
{
    [NeedLogin]
    public class BackSectionManager : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }
        [WebMethod]
        public void Save(IHashObject data)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                data["datetime"] = DateTime.Now;
                data["id"] = Cuid.NewCuid();
                data["uid"] = ((Login.User)Session["user"]).Id;

                IHashObject section = db.SelectFirstRow(string.Format("select id from backsectionDetail where month='{0}'", data["month"]));
                if (section != null)
                {
                    throw new Exception(string.Format("{0}月已设置回款金额,如需继续，请联系管理员", data["month"]));
                }
                db.Insert("backsection", data);
            }
        }
    }
}
