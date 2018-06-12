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

namespace Test001.DataHandler
{
    public class IOUtils
    {
        private static List<ulong> PostDataRequestList { get; }

        static Socket socket;
        static IOUtils()
        {
            PostDataRequestList = new List<ulong>();
            if (ConfigurationManager.ConnectionStrings["socketio"] != null)
            {
                socket = IO.Socket(ConfigurationManager.ConnectionStrings["socketio"].ToString());
            }
            else
            {
                socket = IO.Socket("http://localhost:8080");
            }
        }

        /// <summary>
        /// 验证是否是请求值
        /// </summary>
        /// <remarks>
        /// 只做一次请求
        /// </remarks>
        public static bool IsPostDataRequest(ulong key)
        {
            if (PostDataRequestList.Contains(key))
            {
                PostDataRequestList.Remove(key);
                return true;
            }
            return false;
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
                PostDataRequestList.Add(key);
                nmsg.msg = key.ToString();//回传一个唯一标记
                socket.Emit("postDataSure", serializer.Serialize(nmsg));
            });
            IsConnected = false;
        }

        public static bool IsConnected { get; private set; }

        public static void Emit(string command, string msg)
        {
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