using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Net
    {
        public static string GetNetData(string url, string param, string contentType = "application/json; charset=UTF-8", string cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            request.ContentType = contentType;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }
            if (!string.IsNullOrEmpty(param))
            {
                string data = param;
                request.ContentLength = data.Length;
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gbk")))
                {
                    writer.Write(data);
                    writer.Flush();
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
            {
                return reader.ReadToEnd();
            }
        }

        public static void DownFile(string url, string newDirectory, string realName)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            WebResponse response = request.GetResponse();
            string dir = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, newDirectory);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                using(StreamWriter writer = new StreamWriter(File.Create(string.Format("{0}/{1}", dir, realName))))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                }
            }
        }
    }
}
