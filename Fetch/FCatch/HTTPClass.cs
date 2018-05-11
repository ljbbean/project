using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.IO;
using System.Threading;

namespace FCatch
{
    public class HTTPClass
    {
        public Response HTTP(string _url, string _type, string _postdata, string _cookie, Encoding _responseEncode)
        {
            try
            {
                TcpClient client = new TcpClient();
                Uri URI = new Uri(_url);
                string host = URI.Host;
                int port = URI.Port;
                client.Connect(host, port);
                //var sslStream = new SslStream(client.GetStream(), true, new RemoteCertificateValidationCallback(
                //    (sender, certificate, chain, sslPolicyErrors) =>
                //    {
                //        return true;
                //    }), null);
                //X509Store store = new X509Store(StoreName.My);
                //sslStream.AuthenticateAsClient(host, store.Certificates, SslProtocols.Tls, false);

                StringBuilder requestHeaders = new StringBuilder();
                requestHeaders.Append(_type + " " + URI.PathAndQuery + " HTTP/1.1\r\n");
                requestHeaders.Append("Content-Type:application/x-www-form-urlencoded; charset=UTF-8\r\n");
                requestHeaders.Append("User-Agent:Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11\r\n");
                requestHeaders.Append("Cookie:" + _cookie + "\r\n");
                requestHeaders.Append("Accept:*/*\r\n");
                requestHeaders.Append("Host:" + host + "\r\n");
                requestHeaders.Append("Content-Length:" + _postdata.Length + "\r\n");
                requestHeaders.Append("Connection:close\r\n\r\n");
                byte[] request = Encoding.UTF8.GetBytes(requestHeaders.ToString() + _postdata);
                string result = string.Empty;
                //if (sslStream.IsAuthenticated)
                //{
                //    sslStream.Write(request, 0, request.Length);
                //    sslStream.Flush();
                //    Thread.Sleep(100);
                //    using (StreamReader reader = new StreamReader(sslStream))
                //    {
                //        result = reader.ReadToEnd();
                //    }
                //    client.Close();
                //}
                //else
                {
                    client.Client.Send(request);
                    byte[] responseByte = new byte[1024000];
                    int len = client.Client.Receive(responseByte);
                    result = Encoding.UTF8.GetString(responseByte, 0, len);
                    client.Close();
                }

                int headerIndex = result.IndexOf("\r\n\r\n");
                string[] headerStr = result.Substring(0, headerIndex).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<object, object> responseHeader = new Dictionary<object, object>();
                for (int i = 0; i < headerStr.Length; i++)
                {
                    string[] temp = headerStr[i].Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length == 2)
                    {
                        if (responseHeader.ContainsKey(temp[0]))
                        {
                            responseHeader[temp[0]] = temp[1];
                        }
                        else
                        {
                            responseHeader.Add(temp[0], temp[1]);
                        }
                    }
                }
                Response response = new Response();
                response.HTTPResponseHeader = responseHeader;
                string[] httpstatus = headerStr[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (httpstatus.Length > 2)
                {

                    response.HTTPStatusCode = httpstatus[1];
                }
                else
                {
                    response.HTTPStatusCode = "400";
                }
                response.HTTPResponseText = _responseEncode.GetString(Encoding.UTF8.GetBytes(result.Substring(headerIndex + 4)));
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
