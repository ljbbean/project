﻿using Carpa.Logging;
using Carpa.Web.Ajax;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Common;
using Carpa.Web.Script;

namespace WebMain.DataHandler
{
    /// <summary>
    /// 计数器
    /// </summary>
    internal struct Counter
    {
        /// <summary>
        /// 总的数量
        /// </summary>
        public ulong All { get; set; }
        /// <summary>
        /// 已处理数量
        /// </summary>
        public ulong Doned { get; set; }
    }
    public class IOUtils
    {
        private static Dictionary<ulong, Counter> PostDataRequestList { get; set; }
        private static object obj = new object();

        static Socket socket;
        static IOUtils()
        {
            PostDataRequestList = new Dictionary<ulong, Counter>();
            SocketIOUrl = "http://localhost:8080";
            if (ConfigurationManager.ConnectionStrings["socketio"] != null)
            {
                SocketIOUrl = ConfigurationManager.ConnectionStrings["socketio"].ToString();
                socket = IO.Socket(SocketIOUrl);
            }
            else
            {
                socket = IO.Socket(SocketIOUrl);
            }
        }

        public static string SocketIOUrl { get; private set; }

        /// <summary>
        /// 验证是否是请求值
        /// </summary>
        /// <remarks>
        /// 只做一次请求
        /// </remarks>
        public static bool IsPostDataRequest(ulong key, ulong currentCount)
        {
            Counter counter;
            if (PostDataRequestList.TryGetValue(key, out counter))
            {
                lock (obj)
                {
                    ulong all = counter.All;
                    ulong newDoned = currentCount + counter.Doned;
                    if (all == newDoned)
                    {
                        PostDataRequestList.Remove(key);
                    } else  if (all < newDoned)
                    {
                        throw new Exception("请求令牌数不对，非法请求");
                    } else
                    {
                        counter.Doned = newDoned;
                        PostDataRequestList[key] = counter;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool HasKey(ulong key)
        {
            return PostDataRequestList.ContainsKey(key);
        }

        public static void Init()
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                IsConnected = true;
                Data data = new Data("server");
                data.comefrom = "net";
                socket.Emit("login", serializer.Serialize(data));
            });
            socket.On("postDataRequest", (data) =>
            {
                HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                SendMsg nmsg = new SendMsg("server");
                nmsg.comefrom = "net";
                nmsg.touid = hash.GetValue<string>("fuid");
                ulong key = Cuid.NewCuid();
                Counter counter = new Counter();
                counter.All = hash.GetValue<ulong>("msg");
                counter.Doned = 0;
                PostDataRequestList.Add(key, counter);
                nmsg.msg = key.ToString();//回传一个唯一标记
                socket.Emit("postDataSure", serializer.Serialize(nmsg));
            });
            socket.On("consultationEnabledUpdate", (data) =>//咨询是否允许更新
            {
                HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                SendMsg nmsg = new SendMsg("server");
                nmsg.comefrom = "net";
                nmsg.touid = hash.GetValue<string>("fuid");

                string key = hash.GetValue<string>("msg");
                nmsg.msg = WebInterface.SureUpdateIds.Contains(key) ? "1" : "0";
                WebInterface.SureUpdateIds.Remove(key);
                socket.Emit("sureEnabledUpdate", serializer.Serialize(nmsg));//确认可以做什么操作
            });
            socket.On("getLoginToken", (data) =>//客户端登录
            {
                HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                SendMsg nmsg = new SendMsg("server");
                nmsg.comefrom = "net";
                nmsg.touid = hash.GetValue<string>("fuid");
                try
                {

                    string key = Guid.NewGuid().ToString().Replace("-", "");
                    nmsg.msg = key;
                    using (DbHelper db = AppUtils.CreateDbHelper())
                    {
                        string sql = "insert into logintoken (`user`, `token`,updatedate) values(@user, @token,@updatedate) on duplicate key update `token` = values(`token`),`updatedate` = values(`updatedate`);";
                        db.AddParameter("user", hash.GetValue<string>("msg"));
                        db.AddParameter("token", key);
                        db.AddParameter("updatedate", DateTime.Now);
                        db.ExecuteIntSQL(sql);
                    }
                    socket.Emit("sendLoginToken", serializer.Serialize(nmsg));//确认可以做什么操作
                }
                catch (Exception e)
                {
                    nmsg.msg = string.Format("Exception:{0}", e.Message);
                    socket.Emit("sendLoginToken", serializer.Serialize(nmsg));//确认可以做什么操作
                }
            });
            socket.On("getDownDataToken", (data) =>
            {
                HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                SendMsg nmsg = new SendMsg("server");
                nmsg.comefrom = "net";
                nmsg.touid = hash.GetValue<string>("fuid");

                try
                {
                    string key = Guid.NewGuid().ToString().Replace("-", "");
                    nmsg.msg = key;
                    string conditionMsg = hash.GetValue<string>("msg");
                    HashObject condition = serializer.Deserialize<HashObject>(conditionMsg);
                    using (DbHelper db = AppUtils.CreateDbHelper())
                    {
                        string sql = "insert into downtoken (`user`, `token`,getDate, `condition`) values(@user, @token,@getDate, @condition) on duplicate key update `token` = values(`token`),`getDate` = values(`getDate`),`condition` = values(`condition`);";
                        db.AddParameter("user", condition.GetValue<string>("user"));
                        db.AddParameter("token", key);
                        db.AddParameter("getDate", DateTime.Now);
                        db.AddParameter("condition", conditionMsg);
                        db.ExecuteIntSQL(sql);
                    }
                    socket.Emit("sendDownDataToken", serializer.Serialize(nmsg));//确认可以做什么操作
                }
                catch (Exception e)
                {
                    nmsg.msg = string.Format("Exception:{0}", e.Message);
                    socket.Emit("sendLoginToken", serializer.Serialize(nmsg));//确认可以做什么操作
                }
            });
            IsConnected = false;
        }

        public static bool IsConnected { get; private set; }

        public static void Emit(string command, string msg)
        {
            if (socket == null)
            {
                Init();
            }
            socket.Emit(command, msg);
        }

        public static void DisConnect()
        {
            if (socket == null || socket.IsEmptyObject())
            {
                return;
            }
            socket.Disconnect();
            socket.Close();
            socket = null;
        }
    }
}