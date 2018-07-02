using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using WebMain.Common;
using System.Data;
using System.Text;

namespace WebMain
{
    [NeedLogin]
    public class BillList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            SearchArea area = (SearchArea)Enum.Parse(typeof(SearchArea), RequestParams["area"]);
            UserInfo info = (UserInfo)Session["user"];
            Context["grid"] = new list(info.User, area);
            Context["user"] = info.User;
            Context["area"] = (int)area;
        }
        [WebMethod]
        public IHashObjectList GetDetail(int id)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("bid", id);
                return db.Select("select * from billdetail where bid=@bid");
            }
        }

        [Serializable]
        public class list : DbPagerDataSource
        {
            string user;
            SearchArea area;
            public list(string user, SearchArea area)
                : base(AppUtils.ConnectionString)
            {
                this.area = area;
                this.user = user;
            }

            public override void AddSummaryItems(SummaryItemCollection items)
            {
                items.Add("btotal");
                items.Add("ltotal");
                items.Add("preferential");
                base.AddSummaryItems(items);
            }

            protected override DataTable DoQuery(DbHelper db)
            {
                string sql = GetBillSql(db, user, area);
                DataTable table = db.ExecuteSQL(sql);
                return table;
            }

            internal static string GetBillSql(DbHelper db, string user, SearchArea area)
            {
                StringBuilder sbuilder = new StringBuilder(" where 1 = 1 ");

                sbuilder.Append(" and user = @user and date BETWEEN @startdate and @enddate ");

                db.AddParameter("user", user);
                HashObject date = Utils.GetAreaData(area);
                db.AddParameter("startdate", date["startDate"]);
                db.AddParameter("enddate", date["endDate"]);
                string sql = string.Format(@"select id,DATE_FORMAT(date, '%Y-%m-%d') as date,taobaocode,cname,ctel,caddress,carea,csendway,cremark,btotal,ltotal,preferential,scode,sname,status,
                                            case status when 0 then '待发货' 
	                                        when 1 then '已发货' 
	                                        when 2 then '已签收'
	                                        when 3 then '已确认'	
	                                        when 4 then '已核销'	
	                                        when 9 then '已退款' end as process, 
                                            case status when 1 then '运险保证' when 2  then '签售保证' when 3 then '售后保证' else '' end as after,status,cname,
                                            case scode is null or scode = '' when false then CONCAT(sname,'(',scode,')') else '' end as sender from bill {0} order by date desc", sbuilder.ToString());
                return sql;
            }
        }
    }
}
