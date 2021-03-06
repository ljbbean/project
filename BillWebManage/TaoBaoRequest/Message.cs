﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoRequest
{
    public class Data
    {
        private string _uid;
        public Data(string uid)
        {
            _uid = uid;
            comefrom = "数据抓取";
        }

        public string uid
        {
            get
            {
                return string.Format("{0}_{1}", this.comefrom, this._uid);
            }
        }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string comefrom { get; set; }
    }

    public class SendMsg : Data
    {
        public SendMsg(string uid)
            : base(uid)
        {

        }
        public string touid { get; set; }
        public string msg { get; set; }
    }
}
