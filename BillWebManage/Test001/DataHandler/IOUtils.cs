using Carpa.Logging;
using Carpa.Web.Ajax;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TaoBaoRequest;

namespace Test001.DataHandler
{
    public class IOUtils
    {
        static Socket socket;
        static IOUtils()
        {
            if (ConfigurationManager.ConnectionStrings["socketio"] != null)
            {
                socket = IO.Socket(ConfigurationManager.ConnectionStrings["socketio"].ToString());
            }
            else
            {
                socket = IO.Socket("http://localhost:8080");
            }
        }

        public static void Init()
        {
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                IsConnected = true;
            });
            IsConnected = false;
        }

        public static bool IsConnected { get; private set; }

        public static void Emit(string command, string msg)
        {
            socket.Emit(command, msg);
        }

        public static void DisConnect()
        {
            if (socket == null || socket.IsEmptyObject())
            {
                return;
            }
            socket.Disconnect();
            socket.Close();
            socket = null;
        }
    }
}