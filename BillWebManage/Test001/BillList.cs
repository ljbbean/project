using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;
using System.Text;
using TaoBaoRequest;

namespace Test001
{
    [NeedLogin]
    public class BillList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Login.User user = (Login.User)Session["user"];
            Context["grid"] = new list(new Login.User() { Id = user.Id, Power = user.Power, Name = user.Name });
            DateTime time = DateTime.Now;
            Context["startDate"] = new DateTime(time.Year, time.Month, 1);
            Context["backSection"] = user.Power == 99;
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                IHashObject data = db.SelectFirstRow("select sum(total) as total, sum(paytotal) as paytotal, max(month) as month from backsection");
                if(data == null)
                {
                    data = new HashObject();
                }
                Context["total"] = data.GetValue<decimal>("total");
                Context["paytotal"] = data.GetValue<decimal>("paytotal");
                Context["nopaytotal"] = data.GetValue<decimal>("total") - data.GetValue<decimal>("paytotal");
                Context["month"] = data.GetValue<DateTime>("month").ToString("yyyy-MM");
            }
        }

        [WebMethod]
        public IHashObjectList GetDetail(int id)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("bid", id);
                return db.Select("select * from billdetail where bid=@bid");
            }
        }

        [WebMethod]
        public int doStatus(int id, int status)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    db.AddParameter("id", id);
                    db.AddParameter("status", status);
                    int i = db.ExecuteNonQuerySQL("update bill set status = @status + 1, goodsstatus=1 where id=@id");
                    if (status >= 1)
                    {
                        db.AddParameter("id", id);
                        db.ExecuteNonQuerySQL("update billdetail set goodsstatus = 2 where bid in (select id from bill where id=@id)");
                    }
                    db.CommitTransaction();
                    return i;
                }
                catch (Exception e)
                {
                    db.RollbackTransaction();
                    throw e;
                }
            }
        }

        [WebMethod]
        public string DeleteBill(int id)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    db.AddParameter("id", id);
                    if (db.ExecuteIntSQL("delete from bill where id=@id;") > 0)
                    {
                        db.ExecuteIntSQL("delete from billdetail where bid=@id;");
                    }
                    else
                    {
                        return "删除失败";
                    }
                    db.CommitTransaction();
                    return "删除成功";
                }
                catch(Exception e)
                {
                    if(db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    throw e;
                }
            }
        }

        [Serializable]
        public class list : DbPagerDataSource
        {
            Login.User user;
            public list(Login.User user) : base(AppUtils.ConnectionString)
            {
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
                StringBuilder sbuilder = new StringBuilder(" where 1 = 1 ");
                if (this.queryParams != null)
                {
                    int status = this.queryParams.GetValue<int>("status", 5);
                    if (status != 5)
                    {
                        sbuilder.AppendFormat(" and status = {0}", status);
                    }
                    DateTime time = DateTime.Now;
                    DateTime startDate = queryParams.GetValue<DateTime>("startDate", new DateTime(time.Year, time.Month, 1));
                    if (startDate != DateTime.MinValue)
                    {
                        sbuilder.AppendFormat(" and date >= '{0}'", string.Format("{0} 00:00:00", startDate.ToShortDateString()));
                    }
                    DateTime endDate = queryParams.GetValue<DateTime>("endDate", DateTime.MinValue);
                    if (endDate != DateTime.MinValue)
                    {
                        sbuilder.AppendFormat(" and date <= '{0}'", string.Format("{0} 23:59:59", endDate.ToShortDateString()));
                    }
                    string scode = queryParams.GetValue<string>("scode", "");
                    if (!string.IsNullOrEmpty(scode))
                    {
                        sbuilder.AppendFormat(" and (scode like '%{0}%' or id in (select bid from affiliatedbill where code like  '%{0}%')) ", scode.Replace("'", "\""));
                    }

                    string code = queryParams.GetValue<string>("code", "");
                    if (!string.IsNullOrEmpty(code))
                    {
                        sbuilder.AppendFormat(" and id in (select bid from billdetail where code like '%{0}%') ", code.Replace("'", "\""));
                    }
                }

                if (user.Power != 99)
                {
                    sbuilder.Append(" and uid = " + user.Id);
                }

                DataTable table = db.ExecuteSQL(string.Format(@"select id,DATE_FORMAT(date, '%Y-%m-%d') as date,taobaocode,cname,ctel,caddress,carea,csendway,cremark,btotal,ltotal,preferential,scode,sname,status,
                                            case status when 0 then '待发货' 
	                                        when 1 then '已发货' 
	                                        when 2 then '已签收'
	                                        when 3 then '已确认'	
	                                        when 4 then '已核销'	
	                                        when 9 then '已退款' end as process, 
                                            case status when 1 then '运险保证' when 2  then '签售保证' when 3 then '售后保证' else '' end as after,status,cname,
                                            case scode is null or scode = '' when false then CONCAT(sname,'(',scode,')') else '' end as sender from bill {0} order by date desc", sbuilder.ToString()));
                return table;
            }
        }
    }
}
