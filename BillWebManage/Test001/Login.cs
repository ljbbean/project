﻿using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using Common;
using Test001.DataHandler;
using System.Threading;
using System.Collections;
using TaoBaoRequest;

namespace Test001
{
    public class Login : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        private string GetMessage(string user, string comefrom, string msg)
        {
            SendMsg cmsg = new SendMsg(user);
            cmsg.comefrom = comefrom;
            cmsg.msg = msg;
            cmsg.touid = user;
            return JavaScriptSerializer.CreateInstance().Serialize(cmsg);
        }
        
        /// <summary>
        /// 获取单据抓取开始值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetBillBeginValue(string user)
        {
            using(DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("uid", GetId(db, user));
                var list = db.Select("SELECT MAX(DATE) as ndate FROM bill WHERE billfrom IS NOT NULL AND uid = @uid ORDER BY DATE DESC");
                if(list.Count == 0)
                {
                    return MilliTimeStamp(DateTime.Now.AddDays(-1)).ToString();
                }

                DateTime dateTime = list[0].GetValue<DateTime>("ndate");
                if(dateTime == new DateTime())
                {
                    return "";
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
            if(user== "annychenzy")
            {
                db.AddParameter("name", "cy");
            }

            IHashObjectList list = db.Select("select id from `user` where name =@name");

            return list.Count == 0 ? -1 : list[0].GetValue<long>("id");
        }

        /// <summary>
        /// 保存未一次性传入的数据
        /// </summary>
        private static Dictionary<ulong, IList> backDataList = new Dictionary<ulong, IList>();
        [WebMethod]
        public void BillCatch(string user, ulong key, IList dataList)
        {
            if (!IOUtils.IsPostDataRequest(key, (ulong)dataList.Count))
            {
                throw new Exception("抓取数据未被验证，非法请求");
            }
            IList list = GetAllList(key, dataList);
            if (IOUtils.HasKey(key))
            {
                dataList.Clear();
                return;//还有未传完的数据
            }

            backDataList.Remove(key);//清除备份，做一次性数据处理
            string comefrom = string.Format("数据分析_{0}", DateTime.Now.GetHashCode());
            Data data = new Data(user);
            data.comefrom = comefrom;

            IOUtils.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));

            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, "准备保存下载数据"));
            TaobaoDataHelper.SaveDataToTBill(user, AppUtils.ConnectionString, list);
            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, "下载数据保存成功"));

            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, DataCatchSave.SaveData(user, (text) =>
            {
                IOUtils.Emit("sendMsg", GetMessage(user, comefrom, text));
            })));
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
        public string UserLogin(int width, int height, string name, string pwd)
        {
            Session[Settings.ClientWidthContextName] = width ;
            Session[Settings.ClientHeightContextName] = height ;
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("name", name);
                try
                {
                    IHashObject list = db.SelectSingleRow("select id,password,passwordsalt, power from user where name=@name");

                    if (Utils.ValidatePasswordHashed(list.GetValue<string>("password"), list.GetValue<string>("passwordsalt"), pwd))
                    {
                        User user = new User() { Name = name, Id = list.GetValue<int>("id"), Power = list.GetValue<int>("power") };
                        Session["user"] = user;
                    }
                    else
                    {
                        throw new Exception("用户名或密码错误，请重试");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("用户名或密码错误，请重试");
                }
            }
            return "";
        }
        [Serializable]
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int Power { get; set; }
        }
    }
}
