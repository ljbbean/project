using Carpa.Logging;
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