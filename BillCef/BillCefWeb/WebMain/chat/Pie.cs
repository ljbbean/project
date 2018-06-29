using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain.chat
{
    public class Pie
    {
        public class Title
        {
            /// <summary>
            /// 某站点用户访问来源
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// 纯属虚构
            /// </summary>
            public string subtext { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string x { get; set; }
        }

        public class Tooltip
        {
            /// <summary>
            /// 
            /// </summary>
            public string trigger { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string formatter { get; set; }
        }

        public class Legend
        {
            public Legend()
            {
                data = new List<string>();
            }
            /// <summary>
            /// 
            /// </summary>
            public string orient { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string x { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> data { get; set; }
        }

        public class Mark
        {
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
        }

        public class DataView
        {
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool readOnly { get; set; }
        }

        public class Funnel
        {
            /// <summary>
            /// 
            /// </summary>
            public string x { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string width { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string funnelAlign { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int max { get; set; }
        }

        public class Option
        {
            public Option()
            {
                funnel = new Funnel();
            }
            /// <summary>
            /// 
            /// </summary>
            public Funnel funnel { get; set; }
        }

        public class MagicType
        {
            public MagicType()
            {
                type = new List<string>();
                option = new Option();
            }
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Option option { get; set; }
        }

        public class Restore
        {
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
        }

        public class SaveAsImage
        {
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
        }

        public class Feature
        {
            public Feature()
            {
                mark = new Mark();
                dataView = new DataView();
                magicType = new MagicType();
                restore = new Restore();
                saveAsImage = new SaveAsImage();
            }
            /// <summary>
            /// 
            /// </summary>
            public Mark mark { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DataView dataView { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MagicType magicType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Restore restore { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public SaveAsImage saveAsImage { get; set; }
        }

        public class Toolbox
        {
            public Toolbox()
            {
                feature = new Feature();
            }
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Feature feature { get; set; }
        }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int value { get; set; }
            /// <summary>
            /// 直接访问
            /// </summary>
            public string name { get; set; }
        }

        public class SeriesItem
        {
            public SeriesItem()
            {
                center = new List<string>();
                data = new List<DataItem>();
            }
            /// <summary>
            /// 访问来源
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string radius { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> center { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<DataItem> data { get; set; }
        }

        public class Root
        {
            public Root()
            {
                title = new Title();
                tooltip = new Tooltip();
                legend = new Legend();
                toolbox = new Toolbox();
                series = new List<SeriesItem>();
            }
            /// <summary>
            /// 
            /// </summary>
            public Title title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Tooltip tooltip { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Legend legend { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Toolbox toolbox { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool calculable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SeriesItem> series { get; set; }
        }
    }
}