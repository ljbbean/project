using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Carpa.Web.Script;

namespace WebMain
{
    public class Utils
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CarpaDbConn"].ToString();
            }
        }
        public static DbHelper CreateDbHelper()
        {
            return new DbHelper(ConnectionString, true);
        }
    }
}