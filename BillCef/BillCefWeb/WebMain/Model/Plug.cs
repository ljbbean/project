using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain.Model
{
    [Serializable]
    public class Plug
    {
        public ulong PId { get; set; }
        public string PName { get; set; }
        public string PVersion { get; set; }
        public string PKind { get; set; }
        public string PLabel1 { get; set; }
        public string PLabel2 { get; set; }
        public string PLabel3 { get; set; }
        public string PLabel { get { return string.Format("{0},{1},{2}", PLabel1, PLabel2, PLabel3); } }
        public string PIcon { get; set; }
        public string PDes { get; set; }
        public string PVideo { get; set; }
        public decimal PCost { get; set; }
        public int PLevel { get; set; }
        public int PView { get; set; }
        public string PDownpath { get; set; }
        public long PTotal { get; set; }
        public string PExt { get { return ".zip"; } }
        public string PPics { get { return string.Format("{0},{1},{2}", PPic1, PPic2, PPic3); } }
        public string PPic1 { get; set; }
        public string PPic2 { get; set; }
        public string PPic3 { get; set; }
        public DateTime PCreatedate { get; set; }
        public DateTime PUpdatedate { get { return DateTime.Now; } }
        public int PShowway { get; set; }
        public string PStorename { get; set; }
        public string PWindowname { get; set; }
    }
}