using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class Net
    {
        public static string GetNetDataPost(string url, string param, string contentType = "application/json; charset=UTF-8", string cookie = null)
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

        public static string GetNetDataGet(string url, string encoding = "gbk", string contentType = "application/json; charset=UTF-8", string cookie = null, string refer = "https://www.taobao.com/")
        {
            string content = "";
            for( int i = 0; i < 5 && string.IsNullOrEmpty(content); i++)//重试5次
            {
                content = GetNetDoataGetData(url, encoding, contentType, cookie, refer);
                Thread.Sleep(500);
            }

            return content;
        }

        private static string GetNetDoataGetData(string url, string encoding, string contentType, string cookie, string refer)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.AutomaticDecompression = DecompressionMethods.GZip;//回传数据被压缩，这里设置自动解压
            request.Accept = "*/*";
            request.ContentType = contentType;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "GET";
            request.Referer = refer;
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        public static byte[] DownFileWebClient(string url)
        {
            List<byte> list = new List<byte>();
            int length = 1024;
            byte[] fileBytes = new byte[length];
            WebClient client = new WebClient();
            using (Stream str = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    while (true)
                    {
                        int readed = str.Read(fileBytes, 0, length);
                        if (readed == 0)
                            break;
                        if(length > readed)
                        {
                            list.AddRange(fileBytes.Take(readed));
                        }
                        else
                        {
                            list.AddRange(fileBytes);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static byte[] DownFileWebClient(string url, int total, Action<double, string> callback)
        {
            byte[] fileBytes = new byte[total];
            int readedSize = 0;
            WebClient client = new WebClient();
            using (Stream str = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(str))
                {
                    while (total > 0)
                    {
                        int readed = str.Read(fileBytes, readedSize, total);
                        if (readed == 0)
                            break;

                        readedSize += readed;
                        total -= readed;
                        if (callback != null)
                        {
                            callback(readedSize, "");
                        }
                    }
                }
            }
            return fileBytes;
        }

        public static void DownFileWebClient(string url, string newDirectory, string realName, int total, Action<double, string> callback)
        {
            string dir = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, newDirectory);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            byte[] fileBytes = DownFileWebClient(url, total, callback);

            string path = string.Format("{0}/{1}", dir, realName);
            using (FileStream fstr = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fstr.Write(fileBytes, 0, total);
                fstr.Flush();
            }
            callback(-1, "");
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

            double length = -1;
            Encoding encode = Encoding.GetEncoding("utf-8");
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                length = response.ContentLength;
                using (StreamWriter writer1 = new StreamWriter(File.Create(string.Format("{0}/{1}", dir, realName)), Encoding.Default))
                {
                    writer1.Write(reader.ReadToEnd());
                    writer1.Flush();
                }
            }
        }
    }
}
