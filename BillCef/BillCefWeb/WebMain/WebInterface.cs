using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Logging;
using System.Collections;
using Common;
using WebMain.DataHandler;
using WebHandler.DataHandler;
using TaoBaoRequest;
using WebMain.Common;

namespace WebMain
{
    public class WebInterface:Page
    {
        /// <summary>
        /// 记录websocket用户，确保信息通畅
        /// </summary>
        private static Dictionary<string, int> wsUserDictionary = new Dictionary<string, int>();

        /// <summary>
        /// 保存未一次性传入的数据
        /// </summary>
        private static Dictionary<ulong, IList> backDataList = new Dictionary<ulong, IList>();

        public static List<string> SureUpdateIds = new List<string>();

        [WebMethod]
        public HashObject MinVersion(string client)
        {
            IHashObjectList files = new HashObjectList();
            //HashObject file = new HashObject();
            //file.Add("down", "log4net.dll1");
            //file.Add("real", "log4net.dll");
            //files.Add(file);
            //file = new HashObject();
            //file.Add("down", "update.bat");
            //file.Add("real", "update.bat");
            //files.Add(file);
            HashObject data = new HashObject();
            data.Add("version", "2.0.0.34");
            data.Add("files", files);
            data.Add("updateFile", "update.bat");
            SureUpdateIds.Add(Guid.NewGuid().ToString());
            return data;
        }
        /// <summary>
        /// 获取单据抓取开始值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebMethod]
        public HashObject GetBillBeginValue(string user)
        {
            try
            {
                HashObject data = new HashObject();
                data.Add("startDate", GetStartDate(user));
                data.Add("currentDate", MilliTimeStamp(DateTime.Now.AddDays(-1)));
                return data;
            }
            catch (Exception e)
            {
                Log.Error(string.Format("user:{0}   message:{1}", user, e.Message));
                throw e;
            }
        }

        private string GetStartDate(string user)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("uid", GetId(db, user));

                var list = db.Select("SELECT MAX(DATE) as ndate FROM bill WHERE billfrom IS NOT NULL AND uid = @uid ORDER BY DATE DESC");
                if (list == null || list.Count == 0)
                {
                    return MilliTimeStamp(DateTime.Now.AddDays(-2)).ToString();
                }

                DateTime dateTime = list[0].GetValue<DateTime>("ndate");
                if (dateTime == new DateTime())
                {
                    return MilliTimeStamp(DateTime.Now.AddDays(-2)).ToString();
                }
                dateTime = dateTime.AddDays(-2);//往后推2天
                return MilliTimeStamp(dateTime).ToString();
            }
        }

        public long MilliTimeStamp(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = TheDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
        }

        private long GetId(DbHelper db, string user)
        {
            user = user.ToLower();
            if (user == "ljbbean")
            {
                db.AddParameter("name", "ljb");
            }
            else if (user == "annychenzy")
            {
                db.AddParameter("name", "cy");
            }
            else
            {
                return -1;
            }


            IHashObject hash = db.SelectFirstRow("select id from `user` where name =@name");

            return hash == null ? -1 : hash.GetValue<long>("id");
        }

        [WebMethod]
        public string BillCatch(string user, ulong key, IList dataList)
        {
            if (!IOUtils.IsPostDataRequest(key, (ulong)dataList.Count))
            {
                throw new Exception("抓取数据未被验证，非法请求");
            }
            IList list = GetAllList(key, dataList);
            if (IOUtils.HasKey(key))
            {
                dataList.Clear();
                return "";//还有未传完的数据
            }
            if (wsUserDictionary.ContainsKey(user))
            {
                wsUserDictionary.Remove(user);
            }
            int id = DateTime.Now.GetHashCode();
            wsUserDictionary.Add(user, id);
            DataCatchSave.ClearUserGoodsCache(user);
            backDataList.Remove(key);//清除备份，做一次性数据处理
            string comefrom = string.Format("数据分析_{0}", id);
            Data data = new Data(user);
            data.comefrom = comefrom;

            IOUtils.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));

            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, "准备保存下载数据"));
            TaobaoDataHelper.SaveDataToTBill(user, AppUtils.ConnectionString, list);
            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, "下载数据保存成功"));

            string mssage =  GetMessage(user, comefrom, DataCatchSave.SaveData(user, (text) =>
            {
                IOUtils.Emit("sendMsg", GetMessage(user, comefrom, text));
            }));
            IOUtils.Emit("sendMsg",mssage);
            return mssage;
        }

        internal void ReBillCatch(string user)
        {
            int id = 0;
            if (!wsUserDictionary.TryGetValue(user, out id))
            {
                id = DateTime.Now.GetHashCode();
            }
            string comefrom = string.Format("数据分析_{0}", id);
            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, "成功保存商品配置信息，系统将继续分析下载数据"));
            DataCatchSave.SaveData(user, (text) =>
            {
                IOUtils.Emit("sendMsg", GetMessage(user, comefrom, text));
            });
        }

        private string GetMessage(string user, string comefrom, string msg)
        {
            SendMsg cmsg = new SendMsg(user);
            cmsg.comefrom = comefrom;
            cmsg.msg = msg;
            cmsg.touid = user;
            return JavaScriptSerializer.CreateInstance().Serialize(cmsg);
        }

        private static IList GetAllList(ulong key, IList dataList)
        {
            IList list;
            if (!backDataList.TryGetValue(key, out list))
            {
                list = new List<object>();
                backDataList.Add(key, list);
            }

            foreach (var item in dataList)
            {
                list.Add(item);
            }
            return list;
        }

        [WebMethod]
        public object GetDownData(string token)
        {
            //UserInfo info = (UserInfo)Session["user"];
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select `condition`, user from downtoken where  `token`=@token";
                db.AddParameter("token", token);
                IHashObject data = db.SelectSingleRow(sql);//只用一次
                db.AddParameter("token", token);
                db.AddParameter("ntoken", Guid.NewGuid());
                db.ExecuteIntSQL("update downtoken set `token`=@ntoken where token=@token");
                string user = data.GetValue<string>("user");
                //if (!info.User.Equals(user))
                //{
                //    throw new Exception("非法请求");
                //}
                HashObject condition = serializer.Deserialize<HashObject>(data.GetValue<string>("condition"));

                string dataSql = WebMain.BillList.list.GetBillSql(db, user, (SearchArea)Enum.Parse(typeof(SearchArea), condition.GetValue<string>("area")));
                HashObject rt = new HashObject();
                rt.Add("data", db.Select(dataSql));
                rt.Add("captions", new string[]{ "订单日期", "姓名", "联系电话", "淘宝账号", "联系地址", "发货信息", "订单状态", "订单回款" });
                rt.Add("fields", new string[] { "date", "cname", "ctel", "taobaocode", "caddress", "sender", "process", "ltotal" });
                return rt;
            }
        }
    }
}