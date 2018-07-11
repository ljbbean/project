using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Web.Validation.Validators;
using WebMain.Common;
using System.Web;
using System.Text;
using WebMain.DataHandler;

namespace WebMain
{
    public class Default : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            string token = Request.QueryString["token"];
            string user = Request.QueryString["user"];
            if (WebLogin(token, user))
            {
                Context["text"] = "³É¹¦µÇÂ¼";

                UserInfo info = (UserInfo)Session["user"];
                Context["user"] = info.User;
                Context["visible"] = true;
            }
            else
            {
                Context["user"] = "";
                Context["text"] = "µÇÂ¼Ê§°Ü£¬µã»÷ÖØÊÔ";
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
    }
}