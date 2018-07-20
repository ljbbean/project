using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;

namespace WebMain
{
    public class PlugList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            Context["SFList"] = new TestSFDataSource();
        }

        [WebMethod]
        public object GetItems()
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select picon as img, pname as name, pdes as des, pvideo as video, pview as view, pversion as version, pdownpath, ppics, DATE_FORMAT(pupdatedate,'%Y-%m-%d') as pupdatedate, DATE_FORMAT(pcreatedate,'%Y-%m-%d') as pcreatedate, plabel as label from plugs";
                return db.ExecuteSQL(sql);
            }
        }

        [Serializable]
        public class TestSFDataSource : DbPagerDataSource
        {
            public TestSFDataSource() :
                base(AppUtils.ConnectionString)
            {

            }
            protected override DataTable DoQuery(DbHelper db)
            {
                string sql = "select * from plugs";
                return db.ExecuteSQL(sql);
            }
        }
    }
}
