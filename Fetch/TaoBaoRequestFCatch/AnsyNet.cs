﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Script;

namespace TaoBaoRequestFCatch
{
    public class AnsyNet
    {
        public static void AnsyDataCatch(CacthConfig config, NotifyMessage notifyMsg)
        {
            Thread thread = new Thread(AnsyNetDataCatch);
            AnsyNetData adata = new AnsyNetData();
            adata.NotifyMsg = notifyMsg;
            adata.Config = config;
            adata.DataCatch = new DataCatchRequest();
            thread.Start(adata);
        }

        private class AnsyNetData
        {
            public DataCatchRequest DataCatch { get; set; }
            public CacthConfig Config { get; set; }
            public NotifyMessage NotifyMsg { get; set; }
        }

        private static void AnsyNetDataCatch(object data)
        {
            AnsyNetData adata = (AnsyNetData)data;
            DataCatchRequest dataCatch = adata.DataCatch;
            CacthConfig config = adata.Config;
            NotifyMessage notifyMsg = adata.NotifyMsg;
            SendDetailState detailState = new SendDetailState((msg) =>
            {
                if (!(msg is ActionMessage))
                {
                    notifyMsg(config.User, new ActionMessage() { Message = msg.ToString() });
                    return;
                }
                notifyMsg(config.User, (ActionMessage)msg);
            });
            if (config.IsCacthing != null && config.IsCacthing.Value)
            {
                detailState("之前的抓取正在进行");
                return;
            }

            config.IsCacthing = true;

            try
            {
                detailState(string.Format("开始下载【{0}】到当前时间产生的订单", config.StartDate));
                Thread.Sleep(2000);//停留2s
                List<HashMap> listData = dataCatch.GetDataList(config.StartDate, config.Cookies);
                ActionMessage action = new ActionMessage() { Action = ActionType.SendRequestData, Data = listData };
                if (listData.Count != 0)
                {
                    detailState(string.Format("成功下载{0}条主订单信息", listData.Count));
                    dataCatch.GetDetailsData(config.Cookies, listData, detailState);
                    action.Message = string.Format("明细数据下载完成");
                }
                detailState(action);
                config.IsCacthing = false;
                config.StartDate = config.ServerCurrentData;
            }
            catch (Exception t)
            {
                detailState(string.Format("错误消息:{0}", t.Message));
                config.IsCacthing = false;
            }
        }
    }
}
