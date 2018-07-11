using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Common;
using System.Web;
using System.Collections;

namespace AuxiliaryTools
{
    public class MilitaryInvestigationTool
    {
        string taodaxingUrl = "https://www.taodaxiang.com/shelf/index/get";
        string contentType = "application/x-www-form-urlencoded; charset=UTF-8";

        internal void GetGoodMsg(string name, Action<object> callBack)
        {
            IHashObjectList list = new HashObjectList();
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string value = Net.GetNetData(taodaxingUrl, string.Format("pattern=0&{0}&goodid=&page=1", name), contentType);
            HashObject hash = serializer.Deserialize<HashObject>(value);
            int code = hash.GetValue<int>("code");
            if (code == 1)
            {
                throw new Exception(string.Format("没有查询到网店信息"));
            }
            callBack((object)GetDataList(hash));
            while (code == 0) {
                var post = hash.GetValue<HashObject>("post");
                value = Net.GetNetData(taodaxingUrl, string.Format("pattern={0}&id={1}&page={2}&nt={3}", post["pattern"], post["id"], post["page"], post["nt"]), contentType);
                hash = serializer.Deserialize<HashObject>(value);
                code = hash.GetValue<int>("code");
                if (code != 0)
                {
                    break;
                }
                callBack(GetDataList(hash));
            }
        }

        internal object GetSingleGoodMsg(string id)
        {
            IHashObjectList list = new HashObjectList();
            string value = Net.GetNetData(taodaxingUrl, string.Format("pattern=1&wwid=2&{0}&page=1", id), contentType);
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = serializer.Deserialize<HashObject>(value);
            if (hash.GetValue<int>("code") == 0)
            {
                return GetDataList(hash)[0];
            }
            return "没有找到对应的数据";
        }

        private IHashObjectList GetDataList(HashObject data)
        {
            IHashObjectList list = new HashObjectList();
            foreach (HashObject item in data.GetValue<ArrayList>("data"))
            {
                HashObject copy = new HashObject();
                copy.Add("gid", item["goodid"]);
                copy.Add("title", item["title"]);
                copy.Add("url", item["url"]);
                copy.Add("startDate", Utils.ConvertUnixDate(item.GetValue<long>("start")));
                copy.Add("endDate",  Utils.ConvertUnixDate(item.GetValue<long>("end")));
                long temp = item.GetValue<long>("create");
                copy.Add("create", temp == 0 ? "" : Utils.ConvertUnixDate(temp));
                list.Add(copy);
            }
            return list;
        }
    }
}
