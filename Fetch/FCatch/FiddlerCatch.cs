using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fiddler;
using System.Threading;

namespace FCatch
{
    internal delegate void CatchData(List<Session> data);

    internal class FiddlerCatch
    {
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname = "localhost";
        static int iSecureEndpointPort = 7777;
        static bool bDone = false;
        internal event CatchData OnGetCatchData;
        private List<Session> oAllSessions = new List<Session>();
        private string url = "";


        public static void WriteCommandResponse(string s)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ForegroundColor = oldColor;
        }

        public static void DoQuit()
        {
            WriteCommandResponse("Shutting down...");
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            FiddlerApplication.Shutdown();
            Thread.Sleep(500);
        }
        private static string Ellipsize(string s, int iLen)
        {
            if (s.Length <= iLen) return s;
            return s.Substring(0, iLen - 3) + "...";
        }

        private static void WriteSessionList(List<Session> oAllSessions)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Session list contains...");
            try
            {
                Monitor.Enter(oAllSessions);
                foreach (Session oS in oAllSessions)
                {
                    Console.Write(String.Format("{0} {1} {2}\n{3} {4}\n\n", oS.id, oS.oRequest.headers.HTTPMethod, Ellipsize(oS.fullUrl, 60), oS.responseCode, oS.oResponse.MIMEType));
                }
            }
            finally
            {
                Monitor.Exit(oAllSessions);
            }
            Console.WriteLine();
            Console.ForegroundColor = oldColor;
        }

        private void FBeforeRequest(Session oS)
        {
            oS.bBufferResponse = false;

            if (url == null)
            {
                return;
            }
            int urlIndex = oS.fullUrl.ToLower().IndexOf(url.ToString());
            int pIndex = oS.fullUrl.ToLower().IndexOf("?");

            if (urlIndex < 0 || pIndex >= 0 && urlIndex > pIndex || oS.fullUrl.Equals("http://trade.taobao.com:443") || oS.fullUrl.Equals("https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=Utf8&"))
            {
                return;
            }
            Monitor.Enter(oAllSessions);
            oAllSessions.Add(oS);
            Monitor.Exit(oAllSessions);
            this.OnGetCatchData(oAllSessions);
        }

        public void Start(object url)
        {
            bDone = false;
            FiddlerApplication.SetAppDisplayName("FiddlerCoreDemoApp");
            this.url = url.ToString();
            FiddlerApplication.OnNotification += (object sender, NotificationEventArgs oNEA) => Console.WriteLine("** NotifyUser: " + oNEA.NotifyString); 
            FiddlerApplication.Log.OnLogString += (object sender, LogEventArgs oLEA) =>  Console.WriteLine("** LogString: " + oLEA.LogString);

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            FiddlerApplication.BeforeRequest += FBeforeRequest;
            
            string sSAZInfo = "NoSAZ";

            Console.WriteLine(String.Format("Starting {0} ({1})...", FiddlerApplication.GetVersionString(), sSAZInfo));

            CONFIG.IgnoreServerCertErrors = true;
            
            int iPort = 8877;
            FiddlerApplication.Startup(iPort, true, true);

            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
        }

        public void Quit()
        {
            bDone = true;
            FiddlerApplication.BeforeRequest -= FBeforeRequest;
            DoQuit();
        }
    }
}
