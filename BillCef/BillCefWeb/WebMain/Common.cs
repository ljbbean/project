using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain
{
    public enum SearchArea
    {
        /// <summary>
        /// 今天
        /// </summary>
        Today,
        /// <summary>
        /// 本周
        /// </summary>
        Week,
        /// <summary>
        /// 本月
        /// </summary>
        Month
    }

    public enum SearchKind
    {
        /// <summary>
        /// 订单总数
        /// </summary>
        BillAmount,
        /// <summary>
        /// 支付金额
        /// </summary>
        Pay,
        /// <summary>
        /// 退款金额
        /// </summary>
        Refound,
        /// <summary>
        /// 数据来源
        /// </summary>
        ComeFrom
    }
}