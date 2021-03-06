using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Web.Validation.Validators;
using WebMain.Common;
using System.Web;
using System.Text;
using WebMain.DataHandler;
using WebHandler.DataHandler;
using System.IO;

namespace WebMain
{
    public class Default : Page
    {
        public override void Initialize()
        {
            base.Initialize();

            string token = Request.QueryString["token"];
            string user = Request.QueryString["user"];
            //using (Stream stream = Request.InputStream)
            //{
            //    if (stream.Length != 0)
            //    {
            //        using (StreamReader reader = new StreamReader(Request.InputStream))
            //        {
            //            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            //            string text = reader.ReadToEnd();
            //            HashObject hash = serializer.Deserialize<HashObject>(text);
            //            user = hash.GetValue<string>("user");
            //            token = hash.GetValue<string>("token");
            //        }
            //    }
            //}
            if (WebLogin(token, user))
            {
                Context["text"] = "�ɹ���¼";

                UserInfo info = (UserInfo)Session["user"];
                Context["user"] = info.User;
                Context["visible"] = true;
            }
            else
            {
                Context["user"] = "";
                Context["text"] = "��¼ʧ�ܣ��������";
                Context["visible"] = false;
            }
        }

        private bool WebLogin(string token, string user)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                string sql = "select count(1) as count from  logintoken where `user`=@user and `token`=@token";
                db.AddParameter("token", token);
                db.AddParameter("user", user);
                if (db.SelectSingleRow(sql).GetValue<int>("count") > 0)
                {
                    UserInfo data = new UserInfo();
                    data.User = user;
                    data.LoginDate = DateTime.Now;
                    Session["user"] = data;
                    return true;
                }
            }
            return false;
        }

        [WebMethod]
        public void Test()
        {
            //DataCatchSave.SaveData("���˵Ļ���", (text) =>
            //{
            //    //IOUtils.Emit("sendMsg", GetMessage(user, comefrom, text));
            //});
        }
    }
}