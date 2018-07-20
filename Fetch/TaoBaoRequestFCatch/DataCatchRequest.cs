using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Common;
using System.Web;
using System.Text.RegularExpressions;
using Common.Script;

namespace TaoBaoRequestFCatch
{
    public class DataCatchRequest
    {
        public List<HashMap> GetDataList(DateTime date, string cookie)
        {
            BillManage bill = new BillManage();
            List<string> list = bill.GetBillList<string>(date, cookie);

            JsonSerializer serializer = JsonSerializer.CreateInstance();
            string user = GetUser(cookie);

            StringBuilder sbuilder = new StringBuilder();
            DateTime cdate = DateTime.Now;
            List<HashMap> rtList = new List<HashMap>();
            foreach (string str in list)
            {
                List<HashMap> tempList = GetBillList(serializer, str, date, user);
                rtList.AddRange(tempList.ToArray());
            }

            return rtList;
        }

        internal void GetDetailsData(string cookies, List<HashMap> billDataList, SendDetailState detailSetate = null)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                throw new Exception("请输入cookies");
            }
            string user = GetUser(cookies);
            Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
            JsonSerializer serializer = JsonSerializer.CreateInstance();
            for (int i = 0; i < billDataList.Count; i++)
            {
                IHashMap item = billDataList[i];
                string status = item.GetValue<string>("status");
                HashMap tempHash = serializer.Deserialize<HashMap>(item["content"].ToString());
                if (tempHash == null)
                {
                    continue;
                }
                ArrayList array = (ArrayList)((HashMap)tempHash["statusInfo"])["operations"];

                foreach (HashMap aitem in array)
                {
                    if (!"详情".Equals(aitem.GetValue<string>("text")))
                    {
                        continue;
                    }
                    string url = string.Format("https:{0}", aitem.GetValue<string>("url"));
                    string tbid = item.GetValue<string>("tbid");

                    HashMap detail = GetBillDetail(tbid, url, cookies, i + 1, billDataList.Count, detailSetate);
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

        private HashMap GetBillDetail(string tbid, string url, string cookies, int downedCount, int allCount, SendDetailState detailSetate = null)
        {
            Thread.Sleep(1000);//休眠1s
            BillManage bill = new BillManage();

            string user = GetUser(cookies);
            HashMap detail = new HashMap();
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

        private List<HashMap> GetBillList(JsonSerializer serializer, string str, DateTime date, string user)
        {
            string[] keys = { "mainOrders" };
            HashMap hash = (HashMap)serializer.DeserializeObject(str);
            HashMap vhash = hash.GetHashValue(keys)[0];

            List<object> list = new List<object>();
            list.AddRange(vhash["mainOrders"] as object[]);

            List<HashMap> rtlist = new List<HashMap>();
            foreach (HashMap row in list)
            {
                var id = row["id"].ToString();
                string content = serializer.Serialize(row);
                string status = ((HashMap)row["statusInfo"])["text"].ToString();
                HashMap rtData = new HashMap();
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
