using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Common;
using System.Web;
using System.Text.RegularExpressions;

namespace TaoBaoRequestFCatch
{
    public class DataCatchRequest
    {
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

        internal void GetDetailsData(string cookies, List<HashObject> billDataList, SendDetailState detailSetate = null)
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
        
        public static string GetUser(string cookie)
        {
            string flag = "tracknick=";
            int index = cookie.IndexOf(flag);
            if (index >= 0)
            {
                string subCookies = cookie.Substring(index + flag.Length);
                index = subCookies.IndexOf(";");
                string userName = subCookies.Substring(0, index);

                userName = HttpUtility.UrlDecode(userName);
                userName = UnicodeToString(userName);
                return userName;
            }

            throw new Exception("cookies数据不正确");
        }

        public static string UnicodeToString(string srcText)
        {
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(srcText, (m) => { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
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
