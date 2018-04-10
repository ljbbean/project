using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;
using System.Text;

namespace Test001
{
    public class StockBill : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Context["time"] = new DateTime();
            Login.User user = (Login.User)Session["user"];
            Context["list"] = new list(new Login.User() { Id = user.Id, Power = user.Power, Name = user.Name });
        }
        [WebMethod]
        public IHashObjectList GetTemp(object[] list)
        {
            if (list.Length == 0)
            {
                return null;
            }
            StringBuilder sbuilder = new StringBuilder();
            foreach (IHashObject item in list)
            {
                sbuilder.AppendFormat("{0},", item["bid"]);
            }
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                return db.Select(string.Format("select id, ctel from bill where id in ({0})", sbuilder.ToString().Substring(0, sbuilder.Length - 1)));
            }
        }

        [WebMethod]
        public void ToReady(int id)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("@id", id);
                db.ExecuteNonQuerySQL("update billdetail set goodsstatus=1 where id=@id");
            }
        }

        [Serializable]
        public class list : DbPagerDataSource
        {
            Login.User user;
            bool isSample = false;
            public list(Login.User user, bool isSample = false)
                : base(AppUtils.ConnectionString)
            {
                this.user = user;
                this.isSample = isSample;
            }

            protected override DataTable DoQuery(DbHelper db)
            {
                string fileds = isSample ? "id,bid,size,amount,color,address,remark" : "id,size,bid,color,amount,address,sendway,remark, '标记已备货' as process, goodsstatus";
                StringBuilder sql = new StringBuilder(string.Format(@"select {0} from billdetail where 1 = 1 ", fileds));
                string kind = this.queryParams == null?null:this.queryParams.GetValue<string>("kind");
                if (string.IsNullOrEmpty(kind) || kind == "-1")
                {
                    sql.AppendFormat(" and bid in (select id from bill where status in (0, 1)) and goodsstatus <> 2 ");
                }
                else if (kind == "0")
                {
                    sql.AppendFormat(" and goodsstatus = 0 ");
                }
                else if (kind == "1")
                {
                    sql.AppendFormat(" and goodsstatus = 1 ");
                }
                else if (kind == "2")
                {
                    sql.AppendFormat(" and goodsstatus in (0,1) and bid in (select id from bill where csendway like '%送货%')");
                }

                if (user.Power != 99)
                {
                    sql.AppendFormat(" and bid in (select id from bill where uid = {0})", user.Id);
                }
                DataTable table = db.ExecuteSQL(sql.ToString());
                return table;
            }
        }
    }
}
