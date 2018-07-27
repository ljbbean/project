using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core2.Model
{
    [Table("plugs")]
    public class Plugs
    {
        [Key]
        public ulong PID { get; set; }
        //(下载的文件名称，下载的路径，下载类型，总大小，主程序名称，打开方式，更新时间)
        public string PName { get; set; }
        public string PDownPath { get; set; }
        public string PExt { get; set; }
        /// <summary>
        /// 插件大小
        /// </summary>
        public int PTotal { get; set; }
        public string PWindowName { get; set; }
        public int PShowWay { get; set; }
        public string PUpdateDate { get; set; }
        public string PKind { get; set; }
        public string PVideoweb { get; set; }
        public string PDownpathWeb { get; set; }
        public int PStatus { get; set; }
    }
}
