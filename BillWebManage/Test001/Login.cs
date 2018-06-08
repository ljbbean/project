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
using Test001.DataHandler;
using System.Threading;

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
        
        [WebMethod]
        public void BillCatch(string user)
        {
            string comefrom = string.Format("数据分析_{0}", DateTime.Now.GetHashCode());
            Data data = new Data(user);
            data.comefrom = comefrom;

            IOUtils.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));
            IOUtils.Emit("sendMsg", GetMessage(user, comefrom, DataCatchSave.SaveData((text) =>
            {
                IOUtils.Emit("sendMsg", GetMessage(user, comefrom, text));
            })));

            //string comefrom = string.Format("数据分析_{0}", DateTime.Now.GetHashCode());
            //var socket = IO.Socket("http://localhost:8080");
            //socket.On(Socket.EVENT_CONNECT, () =>
            //{
            //    Data data = new Data(user);
            //    data.comefrom = comefrom;

            //    socket.Emit("login", JavaScriptSerializer.CreateInstance().Serialize(data));
            //    socket.Emit("sendMsg", GetMessage(user, comefrom, DataCatchSave.SaveData((text) =>
            //    {
            //        socket.Emit("sendMsg", GetMessage(user, comefrom, text));
            //    })));
            //    Data edata = new Data(user);
            //    edata.comefrom = comefrom;
            //    socket.Emit("exit", JavaScriptSerializer.CreateInstance().Serialize(edata));
            //    socket.Disconnect();
            //    socket = null;
            //});
            //socket.Emit("sendMsg", GetMessage(user, comefrom, "准备分析"));



            //SendMsg cmsg = new SendMsg("sjfx");
            //cmsg.comefrom = "数据分析";
            //cmsg.msg = "成功接入数据分析接口";
            //cmsg.touid = user;
            //IOUtils.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(cmsg));

            //DataCatchSave.SaveData((text) =>
            //{
            //    SendMsg msg = new SendMsg(user);
            //    msg.comefrom = "数据分析";
            //    msg.touid = user;
            //    msg.msg = text;
            //    IOUtils.Emit("sendMsg", JavaScriptSerializer.CreateInstance().Serialize(msg));
            //});
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
