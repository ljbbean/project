using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoData
{
    internal class Utils
    {
        private static string connect = "server=localhost;database=bill;User ID=root;Password=dev;Charset=utf8; Allow Zero Datetime=True;OldSyntax=true;port=3306;Character Set=utf8";
        public static string Connect { get { return connect; } }
    }
}
