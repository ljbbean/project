using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCatch
{
    public static class AppUtils
    {
        public static string ConnectionString
        {
            get
            {
                return "server=localhost;database=bill;User ID=root;Password=dev;Charset=utf8; Allow Zero Datetime=True;OldSyntax=true;port=3306;Character Set=utf8;Allow User Variables=True;";
            }
        }
    }
}
