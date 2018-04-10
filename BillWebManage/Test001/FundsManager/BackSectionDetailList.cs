using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;

namespace Test001.FundsManager
{
    [NeedLogin]
    public class BackSectionDetailList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            string month = this.Request.QueryString["month"];
            Context["grid"] = new BackSectionListDbPagerDataSource(month);
            Context["show"] = string.IsNullOrEmpty(month);
        }

        [Serializable]
        public class BackSectionListDbPagerDataSource : DbPagerDataSource
        {
            private string month;
            public BackSectionListDbPagerDataSource(string month)
                : base(AppUtils.ConnectionString)
            {
                this.month = month;
            }

            public override void AddSummaryItems(SummaryItemCollection items)
            {
                items.Add("total");
            }

            protected override DataTable DoQuery(DbHelper db)
            {
                string str = "select id,DATE_FORMAT(month, '%Y-%m-%d') as month, total, DATE_FORMAT(datetime, '%Y-%m-%d %h:%i:%s') as datetime ,remark  from backsectionDetail {0} order by datetime desc";
                str = string.Format(str, string.IsNullOrEmpty(month) ? "" : string.Format("where month='{0}' ", month));
                return db.ExecuteSQL(str);
            }
        }
    }
}
