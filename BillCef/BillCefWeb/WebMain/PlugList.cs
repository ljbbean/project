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
            Context["grid"] = new TestSFDataSource();
        }

        [WebMethod]
        public object GetItems()
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select pid as id, picon as img, pname as name, pdes as des, pvideo as video, pview as view, pversion as version, pdownpath, ppics, DATE_FORMAT(pupdatedate,'%Y-%m-%d %H:%i:%s') as pupdatedate, DATE_FORMAT(pcreatedate,'%Y-%m-%d') as pcreatedate, plabel as label from plugs";
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
                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                string sql = "select * from plugs";
                DataTable table = db.ExecuteSQL(sql);
                table.Columns.Add("info");
                table.Columns.Add("ppic1");
                table.Columns.Add("ppic2");
                table.Columns.Add("ppic3");
                foreach (DataRow row in table.Rows)
                {
                    IHashObjectList list = new HashObjectList();
                    IHashObject nr = new HashObject();
                    nr["001"] = string.Format("<a href='{0}' target='_blank'>下载地址</a>&nbsp;&nbsp;", row["pdownpath"]);
                    list.Add(nr);
                    nr = new HashObject();
                    nr["002"] = string.Format("<a href='{0}' target='_blank'>视频地址</a>", row["pvideo"]);
                    list.Add(nr);
                    row["info"] = serializer.Serialize(list);

                    string ppics = row["ppics"].ToString();
                    string[] picArray = ppics.Split(',');
                    row["ppic1"] = picArray[0];
                    row["ppic2"] = picArray[1];
                    row["ppic3"] = picArray[2];
                }
                return table;
            }
        }
    }
}
