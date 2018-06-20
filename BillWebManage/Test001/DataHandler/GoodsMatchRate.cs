using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;
using TaoBaoRequest;
using System.Collections.Generic;
using System.Text;

namespace Test001.DataHandler
{
    [NeedLogin]
    public class GoodsMatchRate : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            string user = Request.QueryString["user"];
            HashObject hash = new HashObject();
            hash["grid"] = GetNeedMatchRateGoods(user);
            hash["user"] = user;
            Context["data"] = hash;
        }

        private DataTable GetNeedMatchRateGoods(string suser)
        {
            DataTable changedTable;
            DataTable table = TaobaoDataHelper.SpliteContentToDataTableByUser(suser, AppUtils.ConnectionString, true);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                List<CommissionRateStruct> list = new List<CommissionRateStruct>();
                Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (DataRow row in table.Rows)
                {
                    GoodsInfo[] ginfos = serializer.Deserialize<GoodsInfo[]>(row["货物信息"].ToString());

                    foreach (GoodsInfo ginfo in ginfos)
                    {
                        CommissionRateStruct crstruct = new CommissionRateStruct();
                        crstruct.Color = ginfo.Color;
                        crstruct.Size = ginfo.Size;
                        crstruct.SourceTitle = ginfo.Title;
                        crstruct.User = row["所属用户"].ToString();
                        if (list.Contains(crstruct))
                        {
                            continue;
                        }
                        list.Add(crstruct);
                    }
                }

                IHashObjectList commList = db.Select("select color,size,sourceTitle, uname, rate from commissionrate");
                foreach (HashObject commItem in commList)
                {
                    CommissionRateStruct crstruct = new CommissionRateStruct();
                    crstruct.Color = commItem.GetValue<string>("color");
                    crstruct.Size = commItem.GetValue<string>("size");
                    crstruct.SourceTitle = commItem.GetValue<string>("sourcetitle");
                    crstruct.User = commItem.GetValue<string>("uname");
                    if (list.Contains(crstruct))
                    {
                        list.Remove(crstruct);
                    }
                }

                changedTable = new DataTable();
                changedTable.Columns.Add("color");
                changedTable.Columns.Add("size");
                changedTable.Columns.Add("sourceTitle");
                changedTable.Columns.Add("uname");
                changedTable.Columns.Add("rate");
                changedTable.Columns.Add("crid");
                foreach (CommissionRateStruct item in list)
                {
                    DataRow row = changedTable.NewRow();
                    row["color"] = item.Color;
                    row["size"] = item.Size;
                    row["sourceTitle"] = item.SourceTitle;
                    row["uname"] = item.User;
                    row["rate"] = item.Rate;
                    changedTable.Rows.Add(row);
                }
                return changedTable;
            }
        }

        [WebMethod]
        public void Save(string user, object[] grid)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                StringBuilder sbuilder = new StringBuilder("insert into commissionrate (crid,color,size,sourceTitle,rate,uname) values");
                foreach (HashObject hash in grid)
                {
                    sbuilder.AppendFormat("({0},'{1}','{2}','{3}',{4},'{5}'),", Cuid.NewCuid(), hash["color"], hash["size"], hash["sourceTitle"], hash["rate"], hash["uname"]);
                }
                db.BatchExecute(sbuilder.ToString().Substring(0, sbuilder.Length - 1));
                DataCatchSave.ClearUserGoodsCache(user);
                Login login = new Login();
                login.ReBillCatch(user);
            }
        }
    }
}
