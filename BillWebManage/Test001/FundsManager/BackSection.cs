using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;

namespace Test001.FundsManager
{
    [NeedLogin]
    public class BackSection : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            Context["grid"] = new BackSectionDbPagerDataSource();
        }

        [Serializable]
        public class BackSectionDbPagerDataSource : DbPagerDataSource
        {
            public BackSectionDbPagerDataSource() : base(AppUtils.ConnectionString)
            { }

            public override void AddSummaryItems(SummaryItemCollection items)
            {
                items.Add("total");
            }

            protected override DataTable DoQuery(DbHelper db)
            {
                return db.ExecuteSQL("select id,DATE_FORMAT(month, '%Y-%m-%d') as month, total, DATE_FORMAT(datetime, '%Y-%m-%d %h:%i:%s') as datetime ,remark, paytotal  from backsection order by datetime desc");
            }
        }
    }
}
