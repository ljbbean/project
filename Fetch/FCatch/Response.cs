using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCatch
{
    public class Response
    {
        string hTTPStatusCode;
        /// 
        /// http状态代码
        /// 
        public string HTTPStatusCode
        {
            get { return hTTPStatusCode; }
            set { hTTPStatusCode = value; }
        }
        Dictionary<object, object> hTTPResponseHeader;
        /// 
        /// Response的header
        /// 
        public Dictionary<object, object> HTTPResponseHeader
        {
            get { return hTTPResponseHeader; }
            set { hTTPResponseHeader = value; }
        }
        string hTTPResponseText;
        /// 
        /// html代码
        /// 
        public string HTTPResponseText
        {
            get { return hTTPResponseText; }
            set { hTTPResponseText = value; }
        }
    }
}
