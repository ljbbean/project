using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace Test001
{

    public class Login : Page
    {
        const string cookieStr = "miid=1728548116093794437; thw=cn; __guid=125552463.4542729777356338700.1504500034900.3682; l=ApubrX-VrpOZZDpEmsm9kqsiq/UFHa9y; enc=cmgzWO5rZwG%2FMgMXNTuVBeySo5tucHt11uj7i4CU0MrD%2BJKfMyB6JGGBiftxY8TWsiy5YIyrKnZWXCKL2V4tjQ%3D%3D; UM_distinctid=1618940f5d4107-09d02200b9f76d-6b1b1279-13c680-1618940f5d57c; hng=CN%7Czh-CN%7CCNY%7C156; cna=m/kFEhmaM1cCAd3tnPPc7v81; ali_ab=221.237.156.243.1501549367233.5; _m_h5_tk=41d55534101b12a1b459fe2b899c4f03_1520989822025; _m_h5_tk_enc=cdf18912e56ff7e27ed6f93f045e7882; v=0; _tb_token_=e3ad09f8e587b; uc3=nk2=D8nuvqgSyg%3D%3D&id2=VAMWosWKTl%2Fw&vt3=F8dBz4W8NiGRDz6hMQk%3D&lg2=UIHiLt3xD8xYTw%3D%3D; existShop=MTUyMTAwNjI5NQ%3D%3D; lgc=ljbbean; tracknick=ljbbean; dnk=ljbbean; cookie2=3580c7238c56f07cd07e661b60c5a40a; sg=n9f; csg=d7951896; mt=np=&ci=11_1; cookie1=BvGEKR%2FwqveRmEF%2FXGsj8sSzWUYlqTujxEaPT1auLMk%3D; unb=725385819; skt=d1f60fe3123cb7db; t=49abd0913341db1817811b1dc1654e34; _cc_=WqG3DMC9EA%3D%3D; tg=0; _l_g_=Ug%3D%3D; _nk_=ljbbean; cookie17=VAMWosWKTl%2Fw; monitor_count=13; uc1=cookie14=UoTeP7jH70wDgg%3D%3D&lng=zh_CN&cookie16=URm48syIJ1yk0MX2J7mAAEhTuw%3D%3D&existShop=true&cookie21=Vq8l%2BKCLj6Hk37282g%3D%3D&tag=8&cookie15=W5iHLLyFOGW7aA%3D%3D&pas=0; apushe955aefbe937056cde8154446b2b0642=%7B%22ts%22%3A1521009220151%2C%22parentId%22%3A1521008400801%7D; isg=BPr6CP5adN3i2_tm-pw7YUHaSyCwu35aKb0V6ATw7g1Y95Ax7DoKlFdhQ4Er5_Yd";
           
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public object Test(string cookie)
        {
            try
            {
                TaoBaoRequest.BillManage bill = new TaoBaoRequest.BillManage();
                return bill.GetBillList(cookie);
            }
            catch(Exception e)
            {
                return e;
            }
        }

        [WebMethod]
        public object GetBillDetail(string id)
        {
            TaoBaoRequest.BillManage bill = new TaoBaoRequest.BillManage();
            return bill.GetBillDetail(id, cookieStr);
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
