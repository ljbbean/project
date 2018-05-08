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
    public delegate void SendDetailState(string message);

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
            public string User;
            public Dictionary<ulong, string> Dictionary;
            public SendDetailState DetailState;
        }

        [WebMethod]
        public void GetDetailsData(string cookies, SendDetailState detailSetate = null)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                throw new Exception("请输入cookies");
            }
            string user = GetUser(cookies);
            Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select * from tbill where `user`= @user and downeddetail = 0";
                db.AddParameter("user", user);
                IHashObjectList list = db.Select(sql);
                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                foreach (HashObject item in list)
                {
                    string status = item.GetValue<string>("status");
                    if ("等待买家付款".Equals(status))
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
            details.User = user;
            details.Dictionary = dictionary;
            details.DetailState = detailSetate;
            if (detailSetate != null)
            {
                detailSetate(string.Format("总共有{0}条明细需要下载", dictionary.Count));
            }
            Thread thread = new Thread(GetBillDetail);
            thread.Start(details);
        }
        
        private void GetBillDetail(object param)
        {
            TaoBaoRequest.BillManage bill = new TaoBaoRequest.BillManage();
            Details details = (Details)param;
            Dictionary<ulong, string> dictionary = details.Dictionary;

            int index = 0;
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.BeginTransaction();
                foreach (ulong key in dictionary.Keys)
                {
                    try
                    {
                        index++;
                        string data = bill.GetBillDetailByUrl(dictionary[key], details.Cookies);
                        db.AddParameter("tbid", key);
                        db.ExecuteIntSQL("delete from tbilldetail where tbid=@tbid");//先删除明细，再添加
                        HashObject hash = new HashObject();
                        hash.Add("tbdid", Cuid.NewCuid());
                        hash.Add("tbid", key);
                        hash.Add("content", data);
                        hash.Add("user", details.User);
                        db.Insert("tbilldetail", hash);
                        db.AddParameter("tbid", key);
                        db.ExecuteIntSQL("update tbill set downeddetail=1 where tbid=@tbid");
                        if (details.DetailState != null)
                        {
                            string message = dictionary.Count == index ? string.Format("{0}条明细下载完毕", dictionary.Count) : string.Format("总共有{0}条明细，已下载{1}条明细，还剩{2}条明细未下", dictionary.Count, index, dictionary.Count - index);
                            details.DetailState(message);
                        }
                    }
                    catch(Exception e)
                    {
                        if (db.HasBegunTransaction)
                        {
                            db.RollbackTransaction();
                        }
                        details.DetailState(string.Format("下载出错：{0}", e.Message));
                        return;
                    }
                    Thread.Sleep(2000);
                }

                db.CommitTransaction();
            }
        }
        
        private string GetUser(string cookie)
        {
            string flag = "tracknick=";
            int index = cookie.IndexOf(flag);
            if (index >= 0)
            {
                string subCookies = cookie.Substring(index + flag.Length);
                index = subCookies.IndexOf(";");
                return subCookies.Substring(0, index);
            }

            throw new Exception("cookies数据不正确");
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

                    foreach (string data in list)
                    {
                        InsertDataToMysql(serializer, data, db, cdate, user);
                    }
                    
                    db.CommitTransaction();
                    return string.Format("数据插入成功");
                }
                catch (Exception e)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    return e;
                }
            }
        }

        private void InsertDataToMysql(JavaScriptSerializer serializer, string str, DbHelper db, DateTime date, string user)
        {
            string[] keys = { "mainOrders" };
            HashObject hash = (HashObject)serializer.DeserializeObject(str);
            HashObject vhash = hash.GetHashValue(keys)[0];

            List<object> list = new List<object>();
            list.AddRange(vhash["mainOrders"] as object[]);

            if (list.Count == 0)
            {
                return ;
            }

            //根据单号获取当前以及存储的数据
            StringBuilder bidBuilder = new StringBuilder("select tbid, content, bid, cdate, status, downeddetail from tbill where bid in (");
            foreach (HashObject row in list)
            {
                bidBuilder.AppendFormat("{0},", row["id"]);
            }
            IHashObjectList bidList = db.Select(string.Format("{0})", bidBuilder.ToString().Substring(0, bidBuilder.Length - 1)));
            //根据单号获取对应的字典信息
            Dictionary<string, HashObject> bidDictionary = new Dictionary<string, HashObject>();
            foreach (HashObject item in bidList)
            {
                bidDictionary.Add(item.GetValue<string>("bid"), item);
            }

            //筛选数据，对于已插入的数据做数据对比，当数据没有变化时，不做数据修改,反之则修改数据。没有的数据直接插入
            StringBuilder insertbuilder = new StringBuilder("insert into tbill(tbid,bid,content, cdate, status, `user`, downeddetail, udate) values");
            string updateSql = "update tbill set content = @content, udate = @udate, status=@status, downeddetail=@downeddetail where tbid=@tbid";
            bool hasInsert = false;
            foreach (HashObject row in list)
            {
                var id = row["id"].ToString();
                string content = serializer.Serialize(row);
                string status = ((HashObject)row["statusInfo"])["text"].ToString();
                HashObject item;
                ulong tbid = Cuid.NewCuid();
                if (bidDictionary.TryGetValue(id, out item))
                {
                    tbid = item.GetValue<ulong>("tbid");
                    if (SpliteContentUrl(item.GetValue<string>("content")).Equals(SpliteContentUrl(content)) && item.GetValue<string>("status").Equals(status))
                    {
                        //数据相同直接返回
                        continue;
                    }
                    db.AddParameter("content", content);
                    db.AddParameter("udate", date);//更新时间
                    db.AddParameter("status", status);
                    db.AddParameter("tbid", tbid);
                    //存在不同的，标记全部更新明细
                    db.AddParameter("downeddetail", 0);
                    db.ExecuteIntSQL(updateSql);//更新已下载数据
                    continue;
                }
                hasInsert = true;
                insertbuilder.AppendFormat("({0},'{1}','{2}', '{3}', '{4}', '{5}', 0, '{6}'),", tbid, id, content, date, status, user, date);
            }
            if (!hasInsert)
            {
                return;
            }
            string insertData = insertbuilder.ToString();
            db.BatchExecute(insertData.Substring(0, insertData.Length - 1));
        }

        private static string SpliteContentUrl(string content)
        {
            if (content == null)
            {
                return "";
            }
            string flag = "\"/trade/memo/update_sell_memo.htm?";
            int index = content.IndexOf(flag);
            if (index < 0)
            {
                return content;
            }
            string tempString = content.Substring(flag.Length + index);
            return content.Substring(0, index + 1) + tempString.Substring(tempString.IndexOf("\""));
        }
    }
}
