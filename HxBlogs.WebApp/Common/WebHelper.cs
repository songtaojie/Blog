using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp
{
    public class WebHelper
    {
        /// <summary>
        /// 根据时间获取要显示的日期格式形式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <param name="showTime"></param>
        /// <returns></returns>
        public static string GetDispayDate(DateTime dateTime,string format = "M", bool showTime = false)
        {
            string result = string.Empty;
            if (dateTime == null) return result;
            DateTime nowTime = DateTime.Now;
            TimeSpan ts = nowTime.Subtract(dateTime);
            if (ts.Days <= 1)
            {
                result = ts.Hours+"小时前";
            }
            else if (ts.Days <= 2)
            {
                result = "1天前";
            }
            else if (ts.Days <= 3)
            {
                result = "2天前";
            }
            else if (ts.Days <= 4)
            {
                result = "3天前";
            }
            else if (ts.Days <= 5)
            {
                result = "4天前";
            }
            else if (ts.Days <= 6)
            {
                result = "5天前";
            }
            else if (ts.Days <= 7)
            {
                result = "6天前";
            }
            else if (ts.Days <= 8)
            {
                result = "7天前";
            }
            else
            {
                result = dateTime.ToString(format);
            }
            if (showTime)
            {
                result += dateTime.ToString("t");
            }
            return result;
        }
        public static string GetDispayDate(DateTime? dateTime, string format = "M", bool showTime = false)
        {
            string result = string.Empty;
            if (dateTime == null || !dateTime.HasValue) return result;
            return GetDispayDate(dateTime.Value, format, showTime);
        }
        /// <summary>
        /// 根据key获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            return ConfigurationManager.AppSettings[key];
        }
    }
}