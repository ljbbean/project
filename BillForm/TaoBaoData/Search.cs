using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaoBaoRequest;
using System.Net;
using System.IO;
using Carpa.Web.Ajax;
using Carpa.Web.Script;
using System.Collections;

namespace TaoBaoData
{
    public class Search
    {
        public object GetMainData(string url, int maxPage = 2)
        {
            HttpWebRequest request = BillManage.CreateWebRequest(url);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Headers.Add("Cookie", "miid=1728548116093794437; __guid=154677242.2846140567203673000.1501561967580.6582; thw=cn; UM_distinctid=1618940f5d4107-09d02200b9f76d-6b1b1279-13c680-1618940f5d57c; hng=CN%7Czh-CN%7CCNY%7C156; l=AuPj2a11VnuhTPJs4rH1zzUk8y2Nanca; enc=%2F9Y7DzXDWuTfSjPQfji0y5jFSM%2B%2FtbTGiPJfLEF%2Bq1RCwFjIfNXY11SnWr2O2Rcld%2FyFKofeQnxnPH9g1%2BQFYg%3D%3D; _m_h5_tk=bc3b6143cded39d50b3adc5d8b721216_1523585146428; _m_h5_tk_enc=b1332109470ab447f6374140ee0f2ad4; ali_ab=221.237.156.243.1501549367233.5; cna=m/kFEhmaM1cCAd3tnPPc7v81; uc3=nk2=D8nuvqgSyg%3D%3D&id2=VAMWosWKTl%2Fw&vt3=F8dBz4D5GsR5MqMiEgY%3D&lg2=UtASsssmOIJ0bQ%3D%3D; existShop=MTUyMzU4Nzc3NA%3D%3D; lgc=ljbbean; tracknick=ljbbean; v=0; dnk=ljbbean; cookie2=14e6c9f0042e42fe3c2b418ba01f6f39; csg=b1b10b18; mt=np=&ci=13_1; skt=6408697bbf7ebabd; t=49abd0913341db1817811b1dc1654e34; _cc_=Vq8l%2BKCLiw%3D%3D; _tb_token_=ea3b3bee453b3; tg=0; uc1=cookie14=UoTePTPB73hIaQ%3D%3D&lng=zh_CN&cookie16=W5iHLLyFPlMGbLDwA%2BdvAGZqLg%3D%3D&existShop=true&cookie21=Vq8l%2BKCLiv0MyZ1zjQnMQw%3D%3D&tag=8&cookie15=WqG3DMC9VAQiUQ%3D%3D&pas=0; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; monitor_count=2; JSESSIONID=A3AD2F9136C385BFEA399E9B386E00FD; isg=BEJCOhzNrM1-J7M-cjRzOVnyk0gIA0byIYXdkIxbvrVj3-NZdKFIPdVdi9mjj77F");
            request.Method = "POST";

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                string rdata = reader.ReadToEnd();

                return GetList(GetPageConfig(rdata));
            }
        }

        private string GetPageConfig(string html)
        {
            const string startFlag = "g_page_config = ";
            const string endFlag = "g_srp_loadCss();";
            int sindex = html.IndexOf(startFlag);
            int eindex = html.IndexOf(endFlag);
            if (sindex < 0 || eindex <0 || sindex >= eindex)
            {
                throw new Exception("页面配置数据不正确");
            }

            sindex = sindex + startFlag.Length;
            return html.Substring(sindex, eindex - sindex).Trim().TrimEnd(';');
        }

        private object GetList(string json)
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = (HashObject)serializer.DeserializeObject(json);
            string[] keys = {
                                    "mods/itemlist/data/auctions"
                            };
            var newHash = hash.GetHashValue(keys);
            Object[] array = (Object[])newHash[0]["auctions"];
            List<Test> list = new List<Test>();
            foreach (HashObject item in array)
            {
                Test test = new Test();
                test.view_price = item.GetValue<string>("view_price", "");
                test.title = item.GetValue<string>("title", "");
                test.nick = item.GetValue<string>("nick", "");
                test.pic_url = item.GetValue<string>("pic_url", "");
                test.detail_url = item.GetValue<string>("detail_url", "");
                test.view_sales = item.GetValue<string>("view_sales", "");
                list.Add(test);
            }
            return list;
        }

        public class Test
        {
            public string view_price { get; set; }
            public string title { get; set; }
            public string nick { get; set; }
            public string pic_url { get; set; }
            public string detail_url { get; set; }
            public string view_sales { get; set; }
        }
    }
}
