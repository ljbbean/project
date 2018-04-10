using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using TaoBaoRequest;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace Test001
{
    public class DataCatch : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            DateTime time = DateTime.Now;
            Context["startDate"] = new DateTime(time.Year, time.Month, 1);
        }

        private void ChangeData()
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                IHashObjectList list = db.Select("select * from tbill");
                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
                foreach(HashObject item in list)
                {
                    HashObject tempHash = serializer.Deserialize<HashObject>(item["content"].ToString());
                    if (tempHash == null)
                    {
                        continue;
                    }
                    dictionary.Add(item.GetValue<ulong>("tbid"), ((HashObject)tempHash["statusInfo"])["text"].ToString());
                }

                StringBuilder sbuilder = new StringBuilder("insert into tbill (tbid, `status`) values");
                foreach(ulong key in dictionary.Keys)
                {
                    sbuilder.AppendFormat("({0}, '{1}'), ", key, dictionary[key]);
                }
                string temp = sbuilder.ToString();
                temp = temp.Substring(0, temp.Length - 2);

                db.BatchExecute(string.Format("{0} on duplicate key update `status`=values(`status`);", temp));
            }
        }

        struct Details
        {
            public string Cookies;
            public Dictionary<ulong, string> Dictionary;
        }

        [WebMethod]
        public void GetDetailsData(string cookies)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                throw new Exception("请输入cookies");
            }
            string user = GetUser(cookies);
            Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select * from tbill where `user`= @user";
                db.AddParameter("user", user);
                IHashObjectList list = db.Select(sql);
                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                foreach (HashObject item in list)
                {
                    if ("交易关闭".Equals(item.GetValue<string>("status")) || "等待买家付款".Equals(item.GetValue<string>("status")))
                    {
                        continue;
                    }
                    HashObject tempHash = serializer.Deserialize<HashObject>(item["content"].ToString());
                    if (tempHash == null)
                    {
                        continue;
                    }
                    ArrayList array = (ArrayList)((HashObject)tempHash["statusInfo"])["operations"];

                    foreach (HashObject aitem in array)
                    {
                        if ("详情".Equals(aitem.GetValue<string>("text")))
                        {
                            dictionary.Add(item.GetValue<ulong>("tbid"), string.Format("https:{0}", aitem.GetValue<string>("url")));
                            break;
                        }
                    }
                }
            }
            Details details = new Details();
            details.Cookies = cookies;
            details.Dictionary = dictionary;
            Thread thread = new Thread(GetBillDetail);
            thread.Start(details);
        }
        
        private void GetBillDetail(object param)
        {
            TaoBaoRequest.BillManage bill = new TaoBaoRequest.BillManage();
            Details details = (Details)param;
            Dictionary<ulong, string> dictionary = details.Dictionary;
            
            foreach (ulong key in dictionary.Keys)
            {
                try
                {
                    using (DbHelper db = AppUtils.CreateDbHelper())
                    {
                        string data = bill.GetBillDetailByUrl(dictionary[key], details.Cookies);
                        HashObject hash = new HashObject();
                        hash.Add("tbdid", Cuid.NewCuid());
                        hash.Add("tbid", key);
                        hash.Add("content", data);
                        db.Insert("tbilldetail", hash);
                    }
                }
                catch(Exception e)
                {

                }
                Thread.Sleep(2000);
            }
        }
        
        private string GetUser(string cookie)
        {
            string flag = "tracknick=";
            int index = cookie.IndexOf(flag);
            if (index >= 0)
            {
                string temp = cookie.Substring(index);
                return temp.Substring(0, temp.IndexOf(';')).Substring(flag.Length);
            }

            throw new Exception("未找到用户");
        }

        [WebMethod]
        public object GetData(DateTime date, string cookie)
        {
            TaoBaoRequest.BillManage bill = new TaoBaoRequest.BillManage();
            List<string> list = bill.GetBillList<string>(date, cookie);

            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string user = GetUser(cookie);
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    StringBuilder sbuilder = new StringBuilder();
                    DateTime cdate = DateTime.Now;
                    List<string> ids = new List<string>();
                    
                    foreach (string data in list)
                    {
                        ids.AddRange(InsertDataToMysql(serializer, data, db, cdate, user));
                    }
                    
                    db.CommitTransaction();
                    return string.Format("数据插入成功,插入条数:{0}", ids.Count);
                }
                catch (Exception e)
                {
                    db.RollbackTransaction();
                    return e;
                }
            }
        }

        private List<string> InsertDataToMysql(JavaScriptSerializer serializer, string str, DbHelper db, DateTime date, string user)
        {
            string[] keys = { "mainOrders" };
            HashObject hash = (HashObject)serializer.DeserializeObject(str);
            HashObject vhash = hash.GetHashValue(keys)[0];

            List<object> list = new List<object>();
            list.AddRange(vhash["mainOrders"] as object[]);

            if (list.Count == 0)
            {
                return new List<string>();
            }
            List<string> ids = new List<string>();
            StringBuilder sbuilder = new StringBuilder("insert into tbill(tbid,bid,content, cdate, status, `user`) values");
            foreach (HashObject row in list)
            {
                ids.Add(row["id"].ToString());
                sbuilder.AppendFormat("({0},'{1}','{2}', '{3}', '{4}', '{5}'),", Cuid.NewCuid(), row["id"], serializer.Serialize(row), date, ((HashObject)row["statusInfo"])["text"].ToString(), user);
            }
            string insertData = sbuilder.ToString();
            db.BatchExecute(insertData.Substring(0, insertData.Length - 1));
            return ids;
        }
    }
}
