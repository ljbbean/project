using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carpa.Configuration;

namespace FCatch
{
    public static class AppUtils
    {
        private static string ip;
        private static string password;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(ip))
                {
                    ip = AppSettings.GetString("ip");
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = "localhost";
                    }
                }
                return string.Format("server={0};database=billtest;User ID=root;Password={1};Charset=utf8; Allow Zero Datetime=True;OldSyntax=true;port=3306;Character Set=utf8;Allow User Variables=True;", ip, password);
            }
        }

        public static string Pwd
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
    }
}
