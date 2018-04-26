using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Text;

namespace Test001.Logistics
{
    public class LogisticsInformation : Page
    {
        string[] logistics = { "annengwuliu", "kuaijiesudi" };
        public override void Initialize()
        {
            base.Initialize();
            try
            {
                //使用WebRequest.Create方法建立HttpWebRequest对象
                string logistic = Request.QueryString["n"];

                int index = 0;
                if (string.IsNullOrEmpty(logistic) || !int.TryParse(logistic, out index))
                {
                    do
                    {
                        index = 0;
                        if (GetDetails(logistics[index]))
                        {
                            break;
                        }
                        index++;
                    } while (logistics.Length > index);
                }
                else
                {
                    if(logistics.Length >= index && GetDetails(logistics[index]))
                    {
                        return;
                    }
                    for (int i = 0; i < logistics.Length; i++)
                    {
                        if (GetDetails(logistics[i]))
                        {
                            return;
                        }
                    }
                     
                }
            }
            catch(Exception e)
            {
                Context["grid"] = null;
            }
            
        }

        private bool GetDetails(string logistics)
        {
            string url = string.Format("http://www.kuaidi100.com/query?type={2}&postid={0}&id=1&valicode=&temp={1}", Request.QueryString["code"], Request.QueryString["r"], logistics);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "get";
            webRequest.Accept = "text/html, application/xhtml+xml, */*";
            webRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/7.0)";
            WebResponse response = webRequest.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                string sHtml = new StreamReader(stream, System.Text.Encoding.UTF8).ReadToEnd();
                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                IHashObject list = serializer.Deserialize<IHashObject>(sHtml);
                //对写入数据的RequestStream对象进行异步请求
                Context["grid"] = list.GetValue<object[]>("data");
                return sHtml.IndexOf("参数异常") < 0;
            }
        }

        [WebMethod]
        public void DoSure(int id)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    db.AddParameter("id", id);
                    int i = db.ExecuteNonQuerySQL("update bill set status = 3, goodsstatus=1 where id=@id and status <= 3");
                    if (i == 1)
                    {
                        db.AddParameter("id", id);
                        db.ExecuteNonQuerySQL("update billdetail set goodsstatus = 2 where bid in (select id from bill where id=@id)");
                    }
                    db.CommitTransaction();
                }
                catch (Exception e)
                {
                    db.RollbackTransaction();
                    throw e;
                }
            }
        }
    }
}
