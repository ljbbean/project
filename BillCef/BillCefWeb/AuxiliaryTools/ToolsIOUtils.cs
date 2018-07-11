using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quobject.SocketIoClientDotNet.Client;
using System.Configuration;
using Carpa.Web.Ajax;
using Common;
using Carpa.Web.Script;

namespace AuxiliaryTools
{
    public class ToolsIOUtils
    {
        static Socket socket;
        static ToolsIOUtils()
        {
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

        public static void Init()
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Data data = new Data("tools");
                data.comefrom = "net";
                socket.Emit("login", serializer.Serialize(data));
            });
            socket.On("getGoodMsg", (data) =>
            {
                Console.Write(string.Format("接入查询数据,{0}", data));
                HashObject hash = serializer.Deserialize<HashObject>(data.ToString());
                HashObject hashData = serializer.Deserialize<HashObject>(hash.GetValue<string>("msg"));
                SendMsg msg = new SendMsg("tools");
                msg.comefrom = "net";
                msg.touid = hashData.GetValue<string>("fid");
                try
                {
                    MilitaryInvestigationTool military = new MilitaryInvestigationTool();
                    string condition = hashData.GetValue<string>("url");
                    if (condition.StartsWith("wwid="))
                    {
                        military.GetGoodMsg(condition, (obj) =>
                        {
                            msg.msg = serializer.Serialize(obj);
                            var imsg = serializer.Serialize(msg);
                            Console.Write(string.Format("输出数据,{0}", imsg));
                            ToolsIOUtils.Emit("goodMsg", imsg);
                        });
                        return;
                    }
                    msg.msg = serializer.Serialize(military.GetSingleGoodMsg(condition));
                }
                catch(Exception e1){
                    msg.msg = e1.Message;
                }
                var tmsg = serializer.Serialize(msg);
                Console.Write(string.Format("输出数据,{0}", tmsg));
                ToolsIOUtils.Emit("goodMsg", tmsg);
            });
        }

        public static void Emit(string command, string msg)
        {
            if (socket == null)
            {
                Init();
            }
            socket.Emit(command, msg);
        }
    }
}
