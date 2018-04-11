using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Carpa.Web.Ajax;
using Carpa.Web.Script;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace TaoBaoRequest
{
    public class BillManage
    {
        public object GetBillList(string cookieStr)
        {
            string data = "auctionType=0&close=0&pageNum={0}&pageSize={1}&queryMore=false&rxAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&action=itemlist%2FSoldQueryAction&prePageNo={2}&buyerNick=&dateBegin=0&dateEnd=0&logisticsService=&orderStatus=&queryOrder=desc&rateStatus=&refund=&sellerNick=&tabCode=latest3Months";
            int startIndex = 1;
            int pageSize = 25;

            object rdata = GetData<SearchOrderInfo>(GetBillList(cookieStr, data, startIndex, pageSize));
            SearchOrderInfo rtData = rdata as SearchOrderInfo;
            if (rtData == null)
            {
                return rdata;
            }
            List<object> list = new List<object>();
            list.AddRange(rtData.MainOrders);

            double max = Math.Ceiling((double)rtData.TotalNumber / pageSize);
            for (int i = startIndex + 1; i <= 1; i++)
            {
                SearchOrderInfo temp = GetData<SearchOrderInfo>(GetBillList(cookieStr, data, i, pageSize).ToString());
                list.AddRange(temp.MainOrders);
            }

            return list;
        }

        private T GetData<T>(string data)
        {
            Type t = typeof(T);
            if(t == typeof(string))
            {
                object obj = data;
                return (T)obj;
            }
            if(t == typeof(SearchOrderInfo))
            {
                object obj = NetDataHandler.ListDataTransformation(data);
                return (T)obj;
            }
            throw new Exception(string.Format("暂不支持这种类型{0}", t));
        }

        public List<T> GetBillList<T>(DateTime startDate, string cookieStr)
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            string dateTimeStr = serializer.Serialize(startDate).Substring(9).TrimEnd(')');
            string data = "auctionType=0&close=0&pageNum={0}&pageSize={1}&queryMore=true&rxAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&action=itemlist%2FSoldQueryAction&dateBegin=";
            data += dateTimeStr + "&prePageNo={2}&buyerNick=&dateEnd=0&logisticsService=&orderStatus=&queryOrder=desc&rateStatus=&refund=&sellerNick=&tabCode=latest3Months";
            //string data = "auctionType=0&close=0&pageNum={0}&pageSize={1}&queryMore=false&rxAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&action=itemlist%2FSoldQueryAction&prePageNo={2}&buyerNick=&dateBegin=0&dateEnd=0&logisticsService=&orderStatus=&queryOrder=desc&rateStatus=&refund=&sellerNick=&tabCode=latest3Months";
            int startIndex = 1;
            int pageSize = 15;
            string sdata = GetBillList(cookieStr, data, startIndex, pageSize);
            SearchOrderInfo rdata = NetDataHandler.ListDataTransformation(sdata);
            
            List<T> list = new List<T>();
            T tempData = GetData<T>(sdata);
            list.Add(tempData);

            double max = Math.Ceiling((double)rdata.TotalNumber / pageSize);
            for (int i = startIndex + 1; i <= max; i++)
            {
                T temp = GetData<T>(GetBillList(cookieStr, data, i, pageSize).ToString());
                list.Add(temp);
            }
            return list;
        }

        private string GetBillList(string cookieStr, string data, int startIndex, int pageSize)
        {
            HttpWebRequest request = CreateWebRequest("https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=Utf8");

            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Referer = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm?spm=a1z38n.10677092.favorite.d28.42bc1debYUYYcK&mytmenu=ymbb";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Headers.Add("Cookie", cookieStr);
            request.Method = "POST";
            string nData = string.Format(data, startIndex, pageSize, startIndex - 1);
            byte[] bytes = Encoding.Default.GetBytes(nData);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                return reader.ReadToEnd();
            }
        }

        private static HttpWebRequest CreateWebRequest(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            return request;
        }

        public object SendLogistics()
        {
            HttpWebRequest request = CreateWebRequest("https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8");
            string cookieStr = "miid=1728548116093794437; thw=cn; __guid=65009961.2618627271943096300.1504832177446.0002; l=ApubrX-VrpOZZDpEmsm9kqsiq/UFHa9y; enc=cmgzWO5rZwG%2FMgMXNTuVBeySo5tucHt11uj7i4CU0MrD%2BJKfMyB6JGGBiftxY8TWsiy5YIyrKnZWXCKL2V4tjQ%3D%3D; UM_distinctid=1618940f5d4107-09d02200b9f76d-6b1b1279-13c680-1618940f5d57c; hng=CN%7Czh-CN%7CCNY%7C156; ali_ab=221.237.156.243.1501549367233.5; cna=m/kFEhmaM1cCAd3tnPPc7v81; _m_h5_tk=445d1917daf6229efc18e56dbfbe552d_1519952673240; _m_h5_tk_enc=5c95a5e2d62c7b2139d593fa4a8bbe5e; v=0; _tb_token_=3bb094e5b3e3; uc3=nk2=D8nuvqgSyg%3D%3D&id2=VAMWosWKTl%2Fw&vt3=F8dBz4TTKM%2BsfeD2ebY%3D&lg2=VFC%2FuZ9ayeYq2g%3D%3D; existShop=MTUxOTk1Mzc5Mg%3D%3D; lgc=ljbbean; tracknick=ljbbean; dnk=ljbbean; cookie2=37dc07376187f4c75f840042362000d7; sg=n9f; csg=2db1ede9; mt=np=&ci=10_1; cookie1=BvGEKR%2FwqveRmEF%2FXGsj8sSzWUYlqTujxEaPT1auLMk%3D; unb=725385819; skt=aa4a8f88d1909521; t=49abd0913341db1817811b1dc1654e34; _cc_=WqG3DMC9EA%3D%3D; tg=0; _l_g_=Ug%3D%3D; _nk_=ljbbean; cookie17=VAMWosWKTl%2Fw; lui=VAMWosWKTl%2Fw; luo=Uw%3D%3D; uc1=cookie14=UoTdcpsLYgaBEA%3D%3D&lng=zh_CN&cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&existShop=true&cookie21=VT5L2FSpcHv%2BujM8lw%3D%3D&tag=8&cookie15=UtASsssmOIJ0bQ%3D%3D&pas=0; monitor_count=4; isg=BPDwJ1hYXmsIJwGIVM6Bpwcowb7uKdRcL4_PRupCgMscpZlPnUojEqvT-a_FNYxb";
            string data = "mailNo=536517930882&companyCode=FAST&orderId=94456067123";
            //string data = "_fmw.r._0.count=510107&_fmw.r._0.c=%E8%94%A1%E4%BA%91%E6%B6%9B&_fmw.r._0.p=%E5%9B%9B%E5%B7%9D%E7%9C%81&_fmw.r._0.ci=%E6%88%90%E9%83%BD%E5%B8%82&_fmw.r._0.d=%E6%AD%A6%E4%BE%AF%E5%8C%BA&=false&_fmw.r._0.to=&_fmw.r._0.i=false&_fmw.r._0.di=510107&_fmw.r._0.coun=&_fmw.r._0.adr=%E6%9C%9B%E6%B1%9F%E8%B7%AF%E8%A1%97%E9%81%93%20%20%20%E8%87%B4%E6%B0%91%E8%B7%AF32%E5%8F%B7%E6%96%B0%E6%A1%A5%E5%B0%8F%E5%8C%BA&_fmw.r._0.dd=%E5%9B%9B%E5%B7%9D%E7%9C%81%E6%88%90%E9%83%BD%E5%B8%82%E6%AD%A6%E4%BE%AF%E5%8C%BA%20%E6%9C%9B%E6%B1%9F%E8%B7%AF%E8%A1%97%E9%81%93%20%20%20%E8%87%B4%E6%B0%91%E8%B7%AF32%E5%8F%B7%E6%96%B0%E6%A1%A5%E5%B0%8F%E5%8C%BA&_fmw.r._0.ad=%E5%9B%9B%E5%B7%9D%E7%9C%81%5E%5E%5E%E6%88%90%E9%83%BD%E5%B8%82%5E%5E%5E%E6%AD%A6%E4%BE%AF%E5%8C%BA%5E%5E%5E%20%E6%9C%9B%E6%B1%9F%E8%B7%AF%E8%A1%97%E9%81%93%20%20%20%E8%87%B4%E6%B0%91%E8%B7%AF32%E5%8F%B7%E6%96%B0%E6%A1%A5%E5%B0%8F%E5%8C%BA&_fmw.r._0.z=000000&_fmw.r._0.ddd=&_fmw.r._0.ph=&_fmw.r._0.b=&_fmw.r._0.t=&_fmw.r._0.mo=18615771571&_fmw.f._0.co=759906322&_fmw.f._0.coun=510114&_fmw.f._0.c=%E6%9D%8E&_fmw.f._0.p=&_fmw.f._0.ci=&_fmw.f._0.cou=&_fmw.f._0.adr=%E6%96%B0%E9%83%BD%E9%A9%AC%E5%AE%B6%E9%95%87&_fmw.f._0.dd=%E5%9B%9B%E5%B7%9D%E7%9C%81%E6%88%90%E9%83%BD%E5%B8%82%E6%96%B0%E9%83%BD%E5%8C%BA%E6%96%B0%E9%83%BD%E9%A9%AC%E5%AE%B6%E9%95%87&_fmw.f._0.ad=%E5%9B%9B%E5%B7%9D%E7%9C%81%5E%5E%5E%E6%88%90%E9%83%BD%E5%B8%82%5E%5E%5E%E6%96%B0%E9%83%BD%E5%8C%BA%5E%5E%5E%E6%96%B0%E9%83%BD%E9%A9%AC%E5%AE%B6%E9%95%87&_fmw.f._0.z=610000&_fmw.f._0.ddd=&_fmw.f._0.ph=&_fmw.f._0.b=&_fmw.f._0.t=&_fmw.f._0.m=18081941228&mailNo=536517930882&companyCode=FAST&orderId=94456067458";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Referer = "https://wuliu.taobao.com/user/consign.htm?trade_id=133122358528673213";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Headers.Add("Cookie", cookieStr);
            request.Method = "POST";
            byte[] bytes = Encoding.Default.GetBytes(data);
            request.ContentLength = bytes.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                string rdata = reader.ReadToEnd();
                return rdata;
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    
        public string GetBillDetailByUrl(string url, string cookieStr)
        {
            try
            {
                HttpWebRequest request = CreateWebRequest(url);
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch, br");
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");

                request.Headers.Add("Cookie", cookieStr);
                request.Method = "get";
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
                {
                    string rt = reader.ReadToEnd();

                    return NetDataHandler.GetDetailDataFromHtml(rt);
                }
            }
            catch(Exception e)
            {
                return string.Format("获取明细数据出错，请求地址为：{0}, 错误信息：{1}", url, e.Message);
            }
        }

        public object GetBillDetail(string bid, string cookieStr)
        {
            HttpWebRequest request = CreateWebRequest(string.Format("https://trade.taobao.com/trade/detail/trade_order_detail.htm?biz_order_id={0}", bid));
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            
            request.Headers.Add("Cookie", cookieStr);
            request.Method = "get";
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                string rdata = reader.ReadToEnd();
                return NetDataHandler.DetailDataTransformation(rdata);
            }
        }
    }
}
