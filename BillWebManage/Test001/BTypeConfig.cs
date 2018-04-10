using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Web.Validation.Validators;
using System.Data;
using System.Collections.Generic;
//using Test001.Login;

namespace Test001
{
    public class BTypeConfig : Page
    {
        public override void Initialize()
        {
            base.Initialize(); using (DbHelper db = AppUtils.CreateDbHelper())
            {
                Test001.Login.User user = ((Test001.Login.User)Session["user"]);
                db.AddParameter("userid", user.Id);
                IHashObjectList list = db.Select("select * from btypeconfig where userid=@userid");
                Context["grid"] = list;
                Context["dataTableTree"] = new DbTreeDataSource(GetData(list), "name", "id", "parentId", 0);
            
            }
                
        }
        private static DataTable GetData(IHashObjectList list)
        {
            DataTable table = new DataTable();

            DataColumn dc = new DataColumn();
            dc.ColumnName = "id";
            dc.DataType = typeof(int);
            table.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "parentId";
            dc.DataType = typeof(int);
            table.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "name";
            table.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "remark";
            table.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "num";
            dc.DataType = typeof(int);
            table.Columns.Add(dc);

            table.Rows.Add(new object[] { 0, -1, "商品", "商品", 0 });

            table.Rows.Add(new object[] { 1, 0, "尺码", "尺码", 1 });
            
            table.Rows.Add(new object[] { 2, 0, "颜色", "颜色", 2 });

            List<string> color = new List<string>();
            List<string> size = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];

                var temp = i + 1;
                var coloritem = item["color"].ToString();
                var sizeitem = item["size"].ToString();
                if (!color.Contains(coloritem))
                {
                    color.Add(coloritem);
                    table.Rows.Add(new object[] { 21 * temp, 2, coloritem, coloritem, 11 * temp });
                }

                if (!size.Contains(sizeitem))
                {
                    size.Add(sizeitem);
                    table.Rows.Add(new object[] { 11 * temp, 1, sizeitem, sizeitem, 11 * temp });
                }
            }
            return table;
        }

        [WebMethod]
        public void DoSave(object[] data)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    Test001.Login.User user = ((Test001.Login.User)Session["user"]);
                    db.AddParameter("userid", user.Id);
                    db.ExecuteScalerSQL("delete from btypeconfig where userid=@userid");
                    for (int i = 0; i < data.Length; i++)
                    {
                        IHashObject item = (IHashObject)data[i];
                        db.AddParameter("size", item["size"]);
                        db.AddParameter("color", item["color"]);
                        db.AddParameter("price", item["price"]);
                        db.AddParameter("userid", user.Id);
                        db.ExecuteIntSQL("insert into btypeconfig values(@size, @color, @price, @userid)");
                    }
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