using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Carpa.Web.Script;
using System.Configuration;

namespace Test001
{
    public class AppUtils
    {
        /// <summary>
        /// 统计字典
        /// </summary>
        internal static Dictionary<int, Dictionary<string, List<string>>> statisticsDictionary = new Dictionary<int, Dictionary<string, List<string>>>();
        private static ISessionGateway currentSession = null;
        public static ISessionGateway CurrentSession
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    return new SessionGateway(context.Session);
                }
                else if (currentSession != null)
                {
                    return currentSession;
                }
                else
                {
                    throw new InvalidOperationException("找不到会话");
                }
            }
            set
            {
                currentSession = value;
            }
        }

        internal static Dictionary<int, Dictionary<string, List<string>>> StatisticsDictionary 
        {
            get { return statisticsDictionary; }
            set { statisticsDictionary = value; }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["dbconnstr"].ToString();
                string connectionString = string.Empty;
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["CarpaDbConn"].ToString();
                }
                catch (Exception e)
                {
                    connectionString = "server=localhost;database=bill;User ID=root;Password=dev;Charset=utf8; Allow Zero Datetime=True;OldSyntax=true;port=3306;Character Set=utf8;Allow User Variables=True;";
                }
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("没有登录，连接账套失败！");
                }
                return connectionString;
            }
            set
            {
                CurrentSession["connectionString"] = value;
            }
        }

        /// <summary>
        /// 创建DbHelper
        /// </summary>
        /// <returns></returns>
        public static DbHelper CreateDbHelper()
        {
            return new DbHelper(ConnectionString, true);
        }

        /// <summary>
        /// 创建DbHelper
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbHelper CreateDbHelper(string connectionString)
        {
            return new DbHelper(connectionString, true);
        }

        public static void CheckError(bool isError, string errorText)
        {
            if (isError)
                throw new Exception(errorText);
        }

        public static DataTable GetArea(DbHelper dbHelper, int parentId)
        {
            var areas = dbHelper.ExecuteSQL("select * from area");
            DataRow[] rows = areas.Select(string.Format("parid={0}", parentId), "id");
            DataTable result = areas.Clone();
            foreach (DataRow row in rows)
            {
                result.ImportRow(row);
            }

            return result;
        }

        public static int GetAreaParId(DbHelper dbHelper, int areaId,ref string areaName)
        {
            var areas = dbHelper.ExecuteFirstRowSQL(string.Format("select * from area where id={0}", areaId));

            areaName = areas["Name"].ToString();
            return Convert.ToInt32(areas["Parid"]);
        }

        public static void ShowDateDropDownIndex(ref int yearindex, ref int monthindex)
        {
            ShowDateDropDownIndex(DateTime.Now,ref yearindex,ref monthindex);
        }

        public static void ShowDateDropDownIndex( DateTime sday,ref int yearindex, ref int monthindex)
        {
            string syear = sday.ToString("yyyy");

            yearindex = int.Parse(syear) - 2016;
            string smonth = sday.ToString("MM");
            monthindex = int.Parse(smonth) - 1;

        }
    }
}