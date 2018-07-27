using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillCefApi.Model
{
    public class PlugDownFileModel
    {
        public ulong ID { get; set; }
       /// (下载的文件名称，下载的路径，下载类型，总大小，主程序名称，打开方式，更新时间)
        public string Name { get; set; }
        public string DownFile { get; set; }
        public string Ext { get; set; }
        /// <summary>
        /// 插件大小
        /// </summary>
        public int Total { get; set; }
        public string MainFileName { get; set; }
        public int OpenWay { get; set; }
        public string UpdateDate { get; set; }
        public string Kind { get; set; }
    }
}
