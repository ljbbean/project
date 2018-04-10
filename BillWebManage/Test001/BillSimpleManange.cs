using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
//using static Test001.Login;
using System.Collections.Generic;

namespace Test001
{
    public class BillSimpleManange : Page
    {
        public override void Initialize()
        {
            base.Initialize();

            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                Test001.Login.User user = ((Test001.Login.User)Session["user"]);
                db.AddParameter("userid", user.Id);
                IHashObjectList userList = db.Select("select color, size, price from btypeconfig where userid=@userid");

                List<string> color = new List<string>();
                List<string> size = new List<string>();
                foreach(IHashObject hash in userList)
                {
                    if (!color.Contains(hash.GetValue<string>("color")))
                    {
                        color.Add(hash.GetValue<string>("color"));
                    }
                    if (!size.Contains(hash.GetValue<string>("size")))
                    {
                        size.Add(hash.GetValue<string>("size"));
                    }
                }
                Context["size"] = size;
                Context["color"] = color;
                Context["sizecolor"] = userList;

                if (this.Request.QueryString["id"] == null)
                {
                    IHashObject data = new HashObject();
                    data["date"] = DateTime.Now;
                    data["amount"] = 1;
                    data["size"] = size.Count > 0 ? size[0] : "";
                    data["color"] = color.Count > 0 ? color[0] : "";
                    data["csendway"] = "送货到小区";
                    data["dobtotal"] = user.Power == 99 ? 1 : 0;
                    data["sizecolor"] = userList;
                    decimal total = userList.Count > 0 ? userList[0].GetValue<decimal>("price") : 0;
                    if(user.Power == 99)
                    {
                        data["btotal"] = 0;
                    }
                    else
                    {
                        data["btotal"] = (total * (decimal)0.05);
                    }
                    data["total"] = total;
                    Context["bill"] = data;
                    Context["enable"] = true;
                    return;
                }

                string id = Request.QueryString["id"].ToString();
                db.AddParameter("id", id);
                IHashObject bill = db.SelectSingleRow(@"select id,DATE_FORMAT(date, '%Y-%m-%d') as date,taobaocode,cname,ctel,caddress,carea,csendway,cremark,btotal,ltotal,preferential, status from bill where id=@id");
                db.AddParameter("id", id);
                IHashObject billDetial = db.SelectSingleRow("select * from billdetail where bid=@id");
                foreach(string key in billDetial.Keys)
                {
                    if (key.ToLower() == "id")
                    {
                        continue;
                    }
                    bill[key] = billDetial[key];
                }
                bill["dobtotal"] = user.Power == 99 ? 1 : 0;
                bill["sizecolor"] = userList;
                Context["bill"] = bill;
                Context["enable"] = true;// bill.GetValue<int>("status") < 1;
            }
        }

        [WebMethod]
        public void DoSave(IHashObject data)
        {
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();
                    int id = Guid.NewGuid().GetHashCode();
                    if (data.ContainsKey("id"))
                    {
                        id = data.GetValue<int>("id");
                        IHashObject detail = GetDetail(data, id);
                        detail.Remove("id");
                        db.Update("billdetail", "bid", detail);
                        IHashObject item = GetBill(data, id);
                        db.Update("bill", "id", item);
                    }
                    else
                    {
                        IHashObject detail = GetDetail(data, id);
                        db.Insert("billdetail", detail);
                        IHashObject item = GetBill(data, id);
                        item["uid"] = ((Test001.Login.User)Session["user"]).Id;
                        db.Insert("bill", item);
                    }
                    db.CommitTransaction();
                }
                catch (Exception e)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    throw e;
                }
            }
        }

        private IHashObject GetBill(IHashObject data, int id)
        {
            IHashObject item = new HashObject();
            item["id"] = id;
            item["date"] = data["date"];
            item["taobaocode"] = data["taobaocode"];
            item["cname"] = data["cname"];
            item["ctel"] = data["ctel"];
            item["caddress"] = data["caddress"];
            item["carea"] = data["carea"];
            item["csendway"] = data["csendway"];
            item["cremark"] = data["cremark"];
            item["btotal"] = data["btotal"];
            item["ltotal"] = data["ltotal"];
            item["preferential"] = data["preferential"];
            return item;
        }

        private IHashObject GetDetail(IHashObject data, int id)
        {
            IHashObject detail = new HashObject();
            detail["id"] = Guid.NewGuid().GetHashCode();
            detail["bid"] = id;
            detail["size"] = data["size"];
            detail["code"] = data["code"];
            detail["area"] = data["carea"];
            detail["address"] = data["caddress"];
            detail["color"] = data["color"];
            detail["amount"] = data["amount"];
            detail["btotal"] = data["btotal"];
            detail["ltotal"] = data["ltotal"];
            detail["total"] = data["total"];
            detail["preferential"] = data["preferential"];
            detail["sendway"] = data["csendway"];
            detail["remark"] = data["cremark"];
            return detail;
        }
    }
}
