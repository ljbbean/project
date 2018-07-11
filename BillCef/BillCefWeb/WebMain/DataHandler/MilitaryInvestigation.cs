using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Common;
using System.Web;
using System.Collections;
using WebMain.Common;

namespace WebMain.DataHandler
{
    [NeedLogin]
    public class MilitaryInvestigation : Page
    {
        string taodaxingUrl = "https://www.taodaxiang.com/shelf/index/get";
        string contentType = "application/x-www-form-urlencoded; charset=UTF-8";
        public override void Initialize()
        {
            base.Initialize();
            UserInfo info = (UserInfo)Session["user"];
            Context["user"] = info.User;
            Context["socketurl"] = IOUtils.SocketIOUrl;
        }

        [WebMethod]
        public void GetGoodMsg(string name, string uid)
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            HashObject data = new HashObject();
            data["fid"] = uid;
            data["url"] = string.Format("wwid={0}", HttpUtility.UrlEncode(name));
            SendMsg msg = new SendMsg("server");
            msg.comefrom = "net";
            msg.msg = serializer.Serialize(data);
            IOUtils.Emit("getGoodMsg", serializer.Serialize(msg));


            //JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            //IHashObjectList list = new HashObjectList();
            //string value = Net.GetNetData(taodaxingUrl, string.Format("pattern=0&wwid={0}&goodid=&page=1", HttpUtility.UrlEncode(name)), contentType);
            //HashObject hash = serializer.Deserialize<HashObject>(value);
            //int code = hash.GetValue<int>("code");
            //if (code == 1)
            //{
            //    throw new Exception(string.Format("没有查询到【{0}】的网店信息", name));
            //}
            //CopyDataToList(hash, list);
            //while (code == 0) {
            //    var post = hash.GetValue<HashObject>("post");
            //    value = Net.GetNetData(taodaxingUrl, string.Format("pattern={0}&id={1}&page={2}&nt={3}", post["pattern"], post["id"], post["page"], post["nt"]), contentType);
            //    hash = serializer.Deserialize<HashObject>(value);
            //    code = hash.GetValue<int>("code");
            //    if (code != 0)
            //    {
            //        break;
            //    }
            //    CopyDataToList(hash, list);
            //}
            //return list;
        }

        [WebMethod]
        public void GetSingleGoodMsg(string id, string uid)
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();

            HashObject data = new HashObject();
            data["fid"] = uid;
            data["url"] = string.Format("goodid={0}", id);
            SendMsg msg = new SendMsg("server");
            msg.comefrom = "net";
            msg.msg = serializer.Serialize(data);
            IOUtils.Emit("getGoodMsg", serializer.Serialize(msg));

            //IHashObjectList list = new HashObjectList();
            //string value = Net.GetNetData(taodaxingUrl, string.Format("pattern=1&wwid=2&goodid={0}&page=1", id), contentType);
            //HashObject hash = serializer.Deserialize<HashObject>(value);
            //if (hash.GetValue<int>("code") == 0)
            //{
            //    CopyDataToList(hash, list);
            //    return list[0];
            //}
            //return value;
        }

        private void CopyDataToList(HashObject data, IHashObjectList list)
        {
            foreach (HashObject item in data.GetValue<ArrayList>("data"))
            {
                HashObject copy = new HashObject();
                copy.Add("gid", item["goodid"]);
                copy.Add("title", item["title"]);
                copy.Add("url", item["url"]);
                copy.Add("startDate", string.Format("{0}000", item["start"]));
                copy.Add("endDate", string.Format("{0}000", item["end"]));
                copy.Add("create", string.Format("{0}000", item["create"]));
                list.Add(copy);
            }
        }
    }
}
