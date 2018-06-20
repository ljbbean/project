using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoRequest
{
    public struct CommissionRateStruct
    {
        public ulong crid { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 原始标题
        /// </summary>
        public string SourceTitle { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 提出比例
        /// </summary>
        public int Rate { get; set; }
        ///// <summary>
        ///// 真实的颜色
        ///// </summary>
        //public string TrueColor { get; set; }
        ///// <summary>
        ///// 真实的尺寸
        ///// </summary>
        //public string TrueSize { get; set; }
        ///// <summary>
        ///// 材质
        ///// </summary>
        //public string Meterial { get; set; }
    }
}
