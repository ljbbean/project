using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Part
{
    /// <summary>
    /// 提供标准获取配置方法接口。
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 获取指定的配置，通过唯一的配置键。
        /// </summary>
        /// <param name="configKey">配置键</param>
        /// <returns>表示配置的对象，这里为统一的String值</returns>
        String GetConfig(string configKey);
    }

    /// <summary>
    /// 提供了针对config文件的配置访问操作。
    /// </summary>
    public class AppSettingProvider : IConfiguration
    {
        /// <summary>
        /// 实现了以config文件的方式获取配置信息。
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public String GetConfig(string configKey)
        {
            return System.Configuration.ConfigurationManager.AppSettings[configKey];
        }
    }

    /// <summary>
    /// 应用设置存取辅助类
    /// </summary>
    public static class AppSettings
    {
        private static IConfiguration _provider;

        /// <summary>
        /// 获取或设置一个用作读取指定的配置的Config对象。
        /// </summary>
        public static IConfiguration Provider
        {
            get { return _provider ?? (_provider = new AppSettingProvider()); }
            set { _provider = value; }
        }

        /// <summary>
        /// 取字符串，值不存在或为空字符串则返回 null
        /// </summary>
        public static string GetString(string keyName)
        {
            string answer = Provider.GetConfig(keyName);
            return !string.IsNullOrEmpty(answer) ? answer : null;
        }

        /// <summary>
        /// 取字符串，值不存在则返回 defaultValue
        /// </summary>
        public static string GetString(string keyName, string defaultValue)
        {
            string result = GetString(keyName);
            return result != null ? result : defaultValue;
        }

        /// <summary>
        /// 取字符串，并解密；值不存在则返回 string.Empty
        /// </summary>
        public static string DecryptGetString(string keyName)
        {
            string result = GetString(keyName);
            return result != null ? StringUtils.Decrypt(result) : string.Empty;
        }

        /// <summary>
        /// 取整数，值不存在则返回 0
        /// </summary>
        public static int GetInt(string keyName)
        {
            return GetInt(keyName, 0);
        }

        /// <summary>
        /// 取整数，值不存在则返回 defaultValue
        /// </summary>
        public static int GetInt(string keyName, int defaultValue)
        {
            string value = GetString(keyName);
            return String.IsNullOrEmpty(value) ? defaultValue : Int32.Parse(value);
        }

        /// <summary>
        /// 取布尔值，值不存在则返回 false
        /// </summary>
        public static bool GetBool(string keyName)
        {
            return GetBool(keyName, false);
        }

        /// <summary>
        /// 取布尔值，值不存在则返回 false
        /// </summary>
        public static bool GetBool(string keyName, bool defaultValue)
        {
            string value = GetString(keyName);
            return String.IsNullOrEmpty(value) ? defaultValue : Boolean.Parse(value); // 空字符串Parse也会出错
        }

        /// <summary>
        /// 取浮点值，值不存在则返回 0.0
        /// </summary>
        public static double GetDouble(string keyName)
        {
            return GetDouble(keyName, 0.0);
        }

        /// <summary>
        /// 取浮点值，值不存在则返回 defaultValue
        /// </summary>
        public static double GetDouble(string keyName, double defaultValue)
        {
            string value = GetString(keyName);
            return String.IsNullOrEmpty(value) ? defaultValue : Double.Parse(value);
        }

        /// <summary>
        /// 取日期时间，值不存在则返回 0001-01-01 00:00:00
        /// </summary>
        public static DateTime GetDateTime(string keyName)
        {
            return GetDateTime(keyName, DateTime.MinValue);
        }

        /// <summary>
        /// 取日期时间，值不存在则返回 defaultValue
        /// </summary>
        public static DateTime GetDateTime(string keyName, DateTime defaultValue)
        {
            string value = GetString(keyName);
            return String.IsNullOrEmpty(value) ? defaultValue : DateTime.Parse(value);
        }
    }
}
