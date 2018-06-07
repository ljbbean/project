using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using TaoBaoRequest;

namespace Test001
{
    public class Login : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public void BillCatch(string user)
        {
            var socket = IO.Socket("http://localhost:8080");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Data data = new Data(user);
                data.comefrom = "数据分析";
                socket.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));
                
                DataCatchSave.SaveData((text) =>
                {
                    SendMsg msg = new SendMsg(user);
                    msg.comefrom = "数据分析";
                    msg.touid = user;
                    msg.msg = text;
                    socket.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(msg));
                    if (text.StartsWith("Exception") || text.StartsWith("OK:"))
                    {
                        socket.Disconnect();
                        socket = null;
                    }
                });
            });
        }

        [WebMethod]
        public void SendUrl(string key, string url)
        {

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
