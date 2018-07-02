using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain.Common
{
    [Serializable]
    public class UserInfo
    {
        public string User { get; set; }
        public string Token { get; set; }
        public DateTime LoginDate { get; set; }
    }
}