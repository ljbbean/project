using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoRequestFCatch
{
    /// <summary>
    /// 消息动作
    /// </summary>
    public class ActionMessage
    {
        public ActionMessage()
        {
            Action = ActionType.Common;
        }
        /// <summary>
        /// 发送的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 动作类型
        /// </summary>
        public ActionType Action { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }

    public enum ActionType
    {
        //普通消息
        Common,
        //数据发送请求消息
        SendRequestData,
        //数据确认请求消息
        SureRequestData
    }
}
