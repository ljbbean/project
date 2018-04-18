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
using System.Data;

namespace TaoBaoData
{
    public class Goods
    {
        public string SendCity { get; set; }
        public List<SkuMap> Skus { get; set; }
        /// <summary>
        /// 确认销售数量
        /// </summary>
        public decimal ConfirmGoodsCount { get; set; }
        /// <summary>
        /// 销售总数
        /// </summary>
        public decimal SoldTotalCount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string Pays { get; set; }
        /// <summary>
        /// 服务
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public string Coupon { get; set; }
    }

    public class SkuMap
    {
        public string Key { get; set; }
        public string SkuId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public decimal Stock { get; set; }
        public string Title { get; set; }
    }

    public class Search
    {
        public object GetGoodMsg(string url)
        {
            HttpWebRequest request = BillManage.CreateWebRequest(url);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            string rdata = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                rdata = reader.ReadToEnd();
            }
            if (url.IndexOf("detail.tmall.com") > 0 || url.IndexOf("click.simba.taobao.com") > 0)
            {
                string json = rdata.GetSectionString("TShop.Setup(", "})();").Trim().TrimEnd(';').TrimEnd(')').Trim();

                JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
                var jsonData = serializer.Deserialize<HashObject>(json);

                List<SkuMap> list = GetSkumapTmallDetail(jsonData);

                string tianmaoUrl = string.Format("https:{0}", jsonData["initApi"]);
                return GetTianMaoRealPrice(tianmaoUrl, url, list);
            }
            else
            {
                List<SkuMap> list = GetSkumapDetail(rdata);
                string sibUrl = string.Format("https:{0}&callback=onSibRequestSuccess", rdata.GetSectionString("wholeSibUrl      :", "',").TrimStart('\''));
                return GetTaobaoRealPrice(sibUrl, url, list);
            } 
        }

        private List<SkuMap> GetSkumapTmallDetail(HashObject data)
        {
            List<SkuMap> list = new List<SkuMap>();
            string[] keys = {
                                "valItemInfo/skuList",
                                "valItemInfo/skuMap"
                            };
            var skuList = data.GetHashValue(keys);
            var details = skuList.GetDataEx<ArrayList>("skuList");
            var skuMaps = skuList.GetDataEx<HashObject>("skuMap");
            foreach (HashObject hash in details)
            {
                SkuMap skuMap = new SkuMap();
                skuMap.Title = hash.GetValue<string>("names");
                skuMap.SkuId = hash.GetValue<string>("skuId");
                skuMap.Key = string.Format(";{0};", hash.GetValue<string>("pvs"));
                var map = (HashObject)skuMaps[skuMap.Key];
                skuMap.Stock = map.GetValue<decimal>("stock");
                skuMap.OriginalPrice = map.GetValue<decimal>("price");
                list.Add(skuMap);
            }

            return list;
        }

        private List<SkuMap> GetSkumapDetail(string html)
        {
            string subMap = GetSubString(html, "skuMap", ",propertyMemoMap").Trim().TrimStart(':');
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = serializer.Deserialize<HashObject>(subMap);
            List<SkuMap> listSkuMap = new List<SkuMap>();
            Dictionary<string, string> dictionary = new Dictionary<string,string>();
            foreach (string key in hash.Keys)
            {
                SkuMap map = new SkuMap();
                map.Key = key;
                HashObject temp = (HashObject)hash[key];
                map.PromotionPrice = map.OriginalPrice = temp.GetValue<decimal>("price");
                map.SkuId = temp.GetValue<string>("skuId");
                string[] keyItems = key.Trim(';').Split(';');
                StringBuilder sbuilder = new StringBuilder();
                for (var i = 0; i < keyItems.Length; i++)
                {
                    string item = keyItems[i];
                    
                    string itemName;
                    if (!dictionary.TryGetValue(item, out itemName))
                    {
                        itemName = GetItemName(item, html);
                        dictionary.Add(item, itemName);
                    }
                    sbuilder.AppendFormat("{0},", itemName);
                }
                map.Title = sbuilder.ToString().Substring(0, sbuilder.Length - 1);
                listSkuMap.Add(map);
            }
            return listSkuMap;
        }

        private string GetItemName(string id, string html)
        {
            string key = string.Format("data-value=\"{0}\"", id);
            return html.GetSectionString(key, "</a>").GetSectionString("<span>", "</span>");
        }

        public object GetTianMaoRealPrice(string url, string refer, List<SkuMap> skuMaps)
        {
            string html = GetHtml(url, refer).ToString().Trim();
            string[] keys = {
                                "defaultModel/deliveryDO/deliveryAddress",
                                "defaultModel/itemPriceResultDO/priceInfo",
                                "defaultModel/servicePromise/servicePromiseList",
                                "defaultModel/detailPageTipsDO/coupon",
                                "defaultModel/sellCountDO/sellCount"
                            };
            JavaScriptSerializer seralizer = JavaScriptSerializer.CreateInstance();
            var hash = seralizer.Deserialize<HashObject>(html);
            var temp = hash.GetHashValue(keys);
            HashObject priceInfo = temp.GetDataEx<HashObject>("priceInfo");
            foreach (SkuMap map in skuMaps)
            {
                var items= priceInfo.GetValue<HashObject>(map.SkuId);
                var promotionList = items.ContainsKey("promotionList") ? items.GetValue<ArrayList>("promotionList") : items.GetValue<ArrayList>("suggestivePromotionList");
                decimal min = map.OriginalPrice;
                foreach (HashObject item in promotionList)
                {
                    min = Math.Min(min, item.GetValue<decimal>("price"));
                }
                map.PromotionPrice = min;
            }

            Goods goods = new Goods();
            goods.SendCity = temp.GetDataEx<string>("deliveryAddress");
            goods.Skus = skuMaps;
            goods.Service = GetGoodsServiceToTmall(temp.GetDataEx<ArrayList>("servicePromiseList"));
            goods.ConfirmGoodsCount = goods.SoldTotalCount = decimal.Parse( temp.GetDataEx<int>("sellCount").ToString());
            //goods.Pays = GetGoodsPayWay(temp.GetDataEx<ArrayList>("pay"));
            //goods.Coupon = GetGoodsCoupon(temp.GetDataEx<ArrayList>("couponList"));

            return goods;
        }
        public object GetTaobaoRealPrice(string url, string refer, List<SkuMap> skuMaps)
        {
            string html = GetHtml(url, refer).ToString().Trim();
            string flag= "onSibRequestSuccess(";
            string data = html.Substring(flag.Length).TrimEnd(';').TrimEnd(')');
            if (!data.Contains("promoData"))
            {
                return skuMaps;
            }
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = serializer.Deserialize<HashObject>(data);
            string[] keys = {
                                "data/deliveryFee/data/sendCity",
                                "data/tradeContract/pay",
                                "data/tradeContract/service",
                                "data/couponActivity/coupon/couponList",
                                "data/promotion/promoData",
                                "data/dynStock/sku",
                                "data/promotion/soldQuantity/soldTotalCount",
                                "data/promotion/soldQuantity/confirmGoodsCount"
                            };
            var temp = hash.GetHashValue(keys);

            Goods goods = new Goods();
            goods.SendCity = temp.GetDataEx<string>("sendCity");
            goods.Pays = GetGoodsPayWay(temp.GetDataEx<ArrayList>("pay"));
            goods.Service = GetGoodsService(temp.GetDataEx<ArrayList>("service"));
            goods.Skus = skuMaps;
            goods.Coupon = GetGoodsCoupon(temp.GetDataEx<ArrayList>("couponList"));
            goods.SoldTotalCount = temp.GetDataEx<decimal>("soldTotalCount");
            goods.ConfirmGoodsCount = temp.GetDataEx<decimal>("confirmGoodsCount");

            HashObject promoDatas = temp.GetDataEx<HashObject>("promoData");

            if (promoDatas != null)
            {
                foreach (SkuMap map in skuMaps)
                {
                    object keySku;
                    if (promoDatas.TryGetValue(map.Key, out keySku))
                    {
                        ArrayList list = (ArrayList)keySku;
                        decimal minPrice = map.PromotionPrice;
                        foreach (HashObject tempList in list)
                        {
                           minPrice = Math.Min(minPrice, tempList.GetValue<decimal>("price"));
                        }
                        map.PromotionPrice = minPrice;
                    }
                }
            }

            HashObject sku = temp.GetDataEx<HashObject>("sku");
            if (sku != null)
            {
                foreach (SkuMap map in skuMaps)
                {
                    object keySku;
                    if (sku.TryGetValue(map.Key, out keySku))
                    {
                        map.Stock = ((HashObject)keySku).GetValue<decimal>("sellableQuantity");
                    }
                }
            }
            return goods;
        }

        private string GetGoodsPayWay(ArrayList list)
        {
            if (list == null)
            {
                return "";
            }
            StringBuilder sbuiler = new StringBuilder();
            foreach (HashObject item in list)
            {
                sbuiler.AppendFormat("【{0}】", item["title"]);
            }
            return sbuiler.ToString();
        }

        private string GetGoodsServiceToTmall(ArrayList list)
        {
            if (list == null)
            {
                return "";
            }
            StringBuilder sbuiler = new StringBuilder();
            foreach (HashObject item in list)
            {
                sbuiler.AppendFormat("【{0}】", item["displayText"]);
            }
            return sbuiler.ToString();
        }

        private string GetGoodsService(ArrayList list)
        {
            if (list == null)
            {
                return "";
            }
            StringBuilder sbuiler = new StringBuilder();
            foreach (HashObject item in list)
            {
                sbuiler.AppendFormat("【{0}】", item["title"]);
            }
            return sbuiler.ToString();
        }

        private string GetGoodsCoupon(ArrayList list)
        {
            if (list == null)
            {
                return "";
            }
            StringBuilder sbuiler = new StringBuilder();
            foreach (HashObject item in list)
            {
                sbuiler.AppendFormat("【{0}】", item["title"]);
            }
            return sbuiler.ToString();
        }

        private static object GetHtml(string url, string refer)
        {
            HttpWebRequest request = BillManage.CreateWebRequest(url);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Headers.Add("Cookie", "tracknick=ljbbean; ");
            //request.Headers.Add("Cookie", "miid=7255751664550819604; ubn=p; l=AvLyLHx4-vzGJDetYoSX37pTwjbUvfYd; thw=cn; ucn=center; hng=CN%7Czh-CN%7CCNY%7C156; ali_ab=101.204.246.42.1448162813979.9; cna=BuKjDu10wUMCAWXM9i/QG0Gg; enc=bLq2wz6ixeu3j7WHIfHTNsIidUOoX7%2Btu0kukOd25TCmT2By7Njhn6pKb6ulQOHAZQGxEkroK9afIWsWPmA7BQ%3D%3D; uc3=nk2=D8nuvqgSyg%3D%3D&id2=VAMWosWKTl%2Fw&vt3=F8dBz4D42SVrd%2BPXN1A%3D&lg2=U%2BGCWk%2F75gdr5Q%3D%3D; lgc=ljbbean; tracknick=ljbbean; t=1ce270ad9fb29c69c3e5dab389feaf88; _cc_=VT5L2FSpdA%3D%3D; tg=0; mt=ci=-1_0; cookie2=15ae98ea27fc7b10b0f1c1a5cae1e30a; v=0; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; _tb_token_=e373b56b7019e; uc1=cookie14=UoTePTPDEmHCSw%3D%3D; isg=BOPj1WWtvRofJnMsboLVWhCRcidNcHcSUyuosRVAecK-VAJ2naisamGMSiTadM8S");
            request.Referer = refer;//一定要加上这个
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                string rdata = reader.ReadToEnd();

                return rdata;
            }
        }

        public object GetMainData(string url, int maxPage = 2)
        {
            HttpWebRequest request = BillManage.CreateWebRequest(url);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Headers.Add("Cookie", "miid=1728548116093794437; __guid=154677242.2846140567203673000.1501561967580.6582; thw=cn; UM_distinctid=1618940f5d4107-09d02200b9f76d-6b1b1279-13c680-1618940f5d57c; hng=CN%7Czh-CN%7CCNY%7C156; l=AuPj2a11VnuhTPJs4rH1zzUk8y2Nanca; enc=%2F9Y7DzXDWuTfSjPQfji0y5jFSM%2B%2FtbTGiPJfLEF%2Bq1RCwFjIfNXY11SnWr2O2Rcld%2FyFKofeQnxnPH9g1%2BQFYg%3D%3D; _m_h5_tk=bc3b6143cded39d50b3adc5d8b721216_1523585146428; _m_h5_tk_enc=b1332109470ab447f6374140ee0f2ad4; ali_ab=221.237.156.243.1501549367233.5; cna=m/kFEhmaM1cCAd3tnPPc7v81; uc3=nk2=D8nuvqgSyg%3D%3D&id2=VAMWosWKTl%2Fw&vt3=F8dBz4D5GsR5MqMiEgY%3D&lg2=UtASsssmOIJ0bQ%3D%3D; existShop=MTUyMzU4Nzc3NA%3D%3D; lgc=ljbbean; tracknick=ljbbean; v=0; dnk=ljbbean; cookie2=14e6c9f0042e42fe3c2b418ba01f6f39; csg=b1b10b18; mt=np=&ci=13_1; skt=6408697bbf7ebabd; t=49abd0913341db1817811b1dc1654e34; _cc_=Vq8l%2BKCLiw%3D%3D; _tb_token_=ea3b3bee453b3; tg=0; uc1=cookie14=UoTePTPB73hIaQ%3D%3D&lng=zh_CN&cookie16=W5iHLLyFPlMGbLDwA%2BdvAGZqLg%3D%3D&existShop=true&cookie21=Vq8l%2BKCLiv0MyZ1zjQnMQw%3D%3D&tag=8&cookie15=WqG3DMC9VAQiUQ%3D%3D&pas=0; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; monitor_count=2; JSESSIONID=A3AD2F9136C385BFEA399E9B386E00FD; isg=BEJCOhzNrM1-J7M-cjRzOVnyk0gIA0byIYXdkIxbvrVj3-NZdKFIPdVdi9mjj77F");
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                string rdata = reader.ReadToEnd();
                string startFlag = "g_page_config = ";
                string endFlag = "g_srp_loadCss();";
                return GetList(GetSubString(rdata, startFlag, endFlag));
            }
        }

        public object GetMainDataMore(string condition, int maxPage = 3)
        {
            List<DataTable> tableList = new List<DataTable>();
            int startIndex = 0;
            for (int i = 0; i < maxPage; i++)
            {
                DataTable table = GetOnePageData(condition, startIndex);
                tableList.Add(table);
                startIndex = table.Rows.Count - 1;
            }
            DataTable gtable = CreateGoodsTable();
            foreach (DataTable tempTable in tableList)
            {
                foreach (DataRow row in tempTable.Rows)
                {
                    gtable.Rows.Add(row.ItemArray);
                }
            }
            return gtable;
        }

        private DataTable GetOnePageData(string condition, int startIndex)
        {
            string format = "https://s.taobao.com/search?data-key=s&data-value={0}&ajax=true&q={1}&imgfile=&js=1&stats_click=search_radio_all%3A1&ie=utf8&bcoffset=4&p4ppushleft=2%2C48";
            string tempSearchString1 = System.Web.HttpUtility.UrlEncode(condition, Encoding.GetEncoding("utf-8"));
            HttpWebRequest request = BillManage.CreateWebRequest(string.Format(format, startIndex, tempSearchString1));
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                string rdata = reader.ReadToEnd();

                return GetMainData(rdata);
            }
        }

        private DataTable GetMainData(string content)
        {
            JavaScriptSerializer serializer = JavaScriptSerializer.CreateInstance();
            HashObject hash = serializer.Deserialize<HashObject>(content);
            string[] keys = {
                                "mods/itemlist/data/auctions"
                            };
            var list = hash.GetHashValue(keys);
            var auctions = list[0].GetValue<ArrayList>("auctions");
            DataTable table = CreateGoodsTable();

            foreach (HashObject obj in auctions)
            {
                DataRow row = table.NewRow();
                row["title"] = obj.GetValue<string>("title");
                row["raw_title"] = obj.GetValue<string>("raw_title");
                row["pic_url"] = obj.GetValue<string>("pic_url");
                row["detail_url"] = obj.GetValue<string>("detail_url");
                row["item_loc"] = obj.GetValue<string>("item_loc");
                string sales = obj.GetValue<string>("view_sales");
                row["view_sales"] = decimal.Parse(sales.Substring(0, sales.Length - 3));
                row["nick"] = obj.GetValue<string>("nick");
                row["view_price"] = obj.GetValue<string>("view_price");
                row["view_fee"] = obj.GetValue<string>("view_fee");
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable CreateGoodsTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("raw_title");
            table.Columns.Add("pic_url");
            table.Columns.Add("detail_url");
            table.Columns.Add("item_loc");
            table.Columns.Add("view_sales", typeof(decimal));
            table.Columns.Add("nick");
            table.Columns.Add("view_price");
            table.Columns.Add("view_fee");
            return table;
        }

        private static string GetSubString(string html, string startFlag, string endFlag)
        {
            int sindex = html.IndexOf(startFlag);
            int eindex = html.IndexOf(endFlag);
            if (sindex < 0 || eindex < 0 || sindex >= eindex)
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
            if (newHash.Count == 0)
            {
                throw new Exception("没搜索到东西");
            }
            Object[] array = (Object[])newHash[0]["auctions"];
            List<GoodsAddress> list = new List<GoodsAddress>();
            foreach (HashObject item in array)
            {
                GoodsAddress test = new GoodsAddress();
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

        public class GoodsAddress
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
