using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMain.chat
{
    public class Bar
    {
        public class Title
        {
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string subtext { get; set; }
        }

        public class Tooltip
        {
            /// <summary>
            /// 
            /// </summary>
            public string trigger { get; set; }
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

        public class MagicType
        {
            public MagicType()
            {
                type = new List<string>();
            }
            /// <summary>
            /// 
            /// </summary>
            public bool show { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> type { get; set; }
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

        public class XAxisItem
        {
            public XAxisItem()
            {
                data = new List<string>();
            }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> data { get; set; }
        }

        public class YAxisItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
        }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 最大值
            /// </summary>
            public string name { get; set; }
        }

        public class MarkPoint
        {
            public MarkPoint()
            {
                data = new List<DataItem>();
            }
            /// <summary>
            /// 
            /// </summary>
            public List<DataItem> data { get; set; }
        }

        public class MarkLine
        {
            public MarkLine()
            {
                data = new List<DataItem>();
            }
            /// <summary>
            /// 
            /// </summary>
            public List<DataItem> data { get; set; }
        }

        public class SeriesItem
        {
            public SeriesItem()
            {
                data = new List<double>();
                markPoint = new MarkPoint();
                markLine = new MarkLine();
            }
            /// <summary>
            /// 蒸发量
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<double> data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MarkPoint markPoint { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MarkLine markLine { get; set; }
        }

        public class Root
        {
            public Root()
            {
                title = new Title();
                tooltip = new Tooltip();
                legend = new Legend();
                toolbox = new Toolbox();
                xAxis = new List<XAxisItem>();
                yAxis = new List<YAxisItem>();
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
            public List<XAxisItem> xAxis { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<YAxisItem> yAxis { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SeriesItem> series { get; set; }
        }
    }
}