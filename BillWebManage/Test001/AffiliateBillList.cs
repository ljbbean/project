using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;

namespace Test001
{
    public class AffiliateBillList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Context["grid"] = new List(this.Request.QueryString["id"].ToString());
        }

        [Serializable]
        private class List : DbPagerDataSource
        {
            private string id;
            public List(string id)
                : base(AppUtils.ConnectionString)
            {
                this.id = id;
            }

            public override void AddSummaryItems(SummaryItemCollection items)
            {
                base.AddSummaryItems(items);
                items.Add("total");
            }

            protected override DataTable DoQuery(DbHelper db)
            {
                db.AddParameter("id", id);
                return db.ExecuteSQL(@"select DATE_FORMAT(date, '%Y-%m-%d %H:%i:%S') as date,code, total, remark, 
                                    case status when 0 then '破损补发' 
                                        when 1 then '发货取消'
                                        when 2 then '破损退款'
                                        when 3 then '缺货补发'
                                        when 4 then '赔偿' end status from affiliatedbill where bid = @id order by date desc"); ;
            }
        }
    }
}
