﻿using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Common;

namespace TaoBaoRequestFCatch
{
    

    public class DataCatchRequest
    {
        private string connectString;
        public DataCatchRequest(string connectString)
        {
            this.connectString = connectString;
        }
        public DataCatchRequest()
        {
        }

        public void GetDetailsData(string cookies, List<HashObject> billDataList, SendDetailState detailSetate = null)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                throw new Exception("请输入cookies");
            }
            string user = GetUser(cookies);
            Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            for (int i = 0; i < billDataList.Count; i++)
            {
                IHashObject item = billDataList[i];
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
                    if (!"详情".Equals(aitem.GetValue<string>("text")))
                    {
                        continue;
                    }
                    string url = string.Format("https:{0}", aitem.GetValue<string>("url"));
                    string tbid = item.GetValue<string>("tbid");

                    HashObject detail = GetBillDetail(tbid, url, cookies, i + 1, billDataList.Count, detailSetate);
                    //if (detail == null)//下载出错，直接移除
                    //{
                    //    billDataList.RemoveAt(i);
                    //    i--;
                    //}
                    item.Add("detail", detail);
                    break;
                }
            }
        }

        private HashObject GetBillDetail(string tbid, string url, string cookies, int downedCount, int allCount, SendDetailState detailSetate = null)
        {
            Thread.Sleep(1000);//休眠1s
            BillManage bill = new BillManage();

            string user = GetUser(cookies);
            HashObject detail = new HashObject();
            try
            {
                detail.Add("tbid", tbid);
                detail.Add("content", bill.GetBillDetailByUrl(url, cookies));
                detail.Add("user", user);

                if (detailSetate != null)
                {
                    detailSetate(string.Format("【{3}】:总共有{0}条明细，已下载{1}条明细，还剩{2}条明细未下", allCount, downedCount, allCount - downedCount, user));
                }
            }
            catch (Exception e)
            {
                if (detailSetate != null)
                {
                    detailSetate(string.Format("【{1}】订单号【{2}】下载明细时出错：{0}", e.Message, user, tbid));
                }
                return null;
            }
            return detail;
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
            using (DbHelper db = new DbHelper(connectString))
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
                if (dictionary.Count == 0)
                {
                    detailSetate(string.Format("【{0}】:抓取结束(finish)", user));
                    return;
                }
            }
            Thread thread = new Thread(GetBillDetail);
            thread.Start(details);
        }
        
        private void GetBillDetail(object param)
        {
            BillManage bill = new BillManage();
            Details details = (Details)param;
            Dictionary<ulong, string> dictionary = details.Dictionary;

            int index = 0;
            using (DbHelper db = new DbHelper(connectString))
            {
                string user = GetUser(details.Cookies);
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
                            string message = dictionary.Count == index ? string.Format("【{1}】:{0}条明细下载完毕(finish)", dictionary.Count, user) : string.Format("【{3}】:总共有{0}条明细，已下载{1}条明细，还剩{2}条明细未下", dictionary.Count, index, dictionary.Count - index, user);

                            if (dictionary.Count == index)
                            {
                                db.CommitTransaction();
                                details.DetailState(message);
                            }
                            else
                            {
                                details.DetailState(message);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        if (db.HasBegunTransaction)
                        {
                            db.RollbackTransaction();
                        }
                        details.DetailState(string.Format("下载出错【{1}】：{0}", e.Message, user));
                        return;
                    }
                    Thread.Sleep(2000);
                }
                if (db.HasBegunTransaction)
                {
                    db.RollbackTransaction();
                }
            }
        }
        
        public static string GetUser(string cookie)
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
        public List<HashObject> GetDataList(DateTime date, string cookie)
        {
            BillManage bill = new BillManage();
            List<string> list = bill.GetBillList<string>(date, cookie);

            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string user = GetUser(cookie);

            StringBuilder sbuilder = new StringBuilder();
            DateTime cdate = DateTime.Now;
            List<HashObject> rtList = new List<HashObject>();
            foreach (string str in list)
            {
                List<HashObject> tempList = GetBillList(serializer, str, date, user);
                rtList.AddRange(tempList.ToArray());
            }

            return rtList;
        }

        private List<HashObject> GetBillList(JavaScriptSerializer serializer, string str, DateTime date, string user)
        {
            string[] keys = { "mainOrders" };
            HashObject hash = (HashObject)serializer.DeserializeObject(str);
            HashObject vhash = hash.GetHashValue(keys)[0];

            List<object> list = new List<object>();
            list.AddRange(vhash["mainOrders"] as object[]);

            List<HashObject> rtlist = new List<HashObject>();
            foreach (HashObject row in list)
            {
                var id = row["id"].ToString();
                string content = serializer.Serialize(row);
                string status = ((HashObject)row["statusInfo"])["text"].ToString();
                HashObject rtData = new HashObject();
                rtData.Add("bid", id);
                rtData.Add("content", serializer.Serialize(row));
                rtData.Add("cdate", date);
                rtData.Add("status", status);
                rtData.Add("user", user);
                rtlist.Add(rtData);
            }

            return rtlist;
        }

        [WebMethod]
        public object GetData(DateTime date, string cookie)
        {
            BillManage bill = new BillManage();
            List<string> list = bill.GetBillList<string>(date, cookie);

            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string user = GetUser(cookie);
            using (DbHelper db = new DbHelper(connectString))
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
                    return string.Format("网络订单主表数据获取完毕，并更新完毕");
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
                bidBuilder.AppendFormat("'{0}',", row["id"]);
            }
            IHashObjectList bidList = db.Select(string.Format("{0})", bidBuilder.ToString().Substring(0, bidBuilder.Length - 1)));
            //根据单号获取对应的字典信息
            Dictionary<string, HashObject> bidDictionary = new Dictionary<string, HashObject>();
            foreach (HashObject item in bidList)
            {
                bidDictionary.Add(item.GetValue<string>("bid"), item);
            }

            //筛选数据，对于已插入的数据做数据对比，当数据没有变化时，不做数据修改,反之则修改数据。没有的数据直接插入
            StringBuilder insertbuilder = new StringBuilder("insert into tbill(tbid,bid,content, cdate, status, `user`, downeddetail, udate, hasUpdate) values");
            string updateSql = "update tbill set content = @content, udate = @udate, status=@status, downeddetail=@downeddetail, hasUpdate = @hasUpdate where tbid=@tbid";
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
                    db.AddParameter("hasUpdate", 1);
                    db.ExecuteIntSQL(updateSql);//更新已下载数据
                    continue;
                }
                hasInsert = true;
                insertbuilder.AppendFormat("({0},'{1}','{2}', '{3}', '{4}', '{5}', 0, '{6}', 1),", tbid, id, content, date, status, user, date);
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