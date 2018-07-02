using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain
{
    /// <summary>
    /// 搜索时间
    /// </summary>
    public enum SearchArea
    {
        /// <summary>
        /// 今天
        /// </summary>
        Today = 0,
        /// <summary>
        /// 本周
        /// </summary>
        Week = 1,
        /// <summary>
        /// 本月
        /// </summary>
        Month = 2,
        /// <summary>
        /// 近期30天
        /// </summary>
        DaysOf30 = 3
    }
    /// <summary>
    /// 搜索方式
    /// </summary>
    public enum SearchKind
    {
        /// <summary>
        /// 订单总数
        /// </summary>
        BillAmount = 0,
        /// <summary>
        /// 支付金额
        /// </summary>
        Pay = 1,
        /// <summary>
        /// 退款金额
        /// </summary>
        Refound = 2,
        /// <summary>
        /// 数据来源
        /// </summary>
        ComeFrom = 3
    }
}