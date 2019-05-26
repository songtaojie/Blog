using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string GetDispayDate(DateTime dateTime, string format = "M", bool showTime = false)
        {
            string result = string.Empty;
            if (dateTime == null) return result;
            DateTime nowTime = DateTime.Now;
            TimeSpan ts = nowTime.Subtract(dateTime);
            if (ts.Days <= 1)
            {
                if (ts.Hours <= 1)
                {
                    result = ts.Minutes + "分钟前";
                }
                else
                {
                    result = ts.Hours + "小时前";
                }
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
            else if (ts.Days > 365 || dateTime.Year != nowTime.Year)
            {
                result = dateTime.ToString("yyyy年MM月dd日");
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
        /// 设置cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expires">过期时间</param>
        public static void SetCookieValue(string cookieName, string value, DateTime? expires = null)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Value = value;
                if (expires.HasValue)
                {
                    cookie.Expires = expires.Value;
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = new HttpCookie(cookieName, value);
                if (expires.HasValue)
                {
                    cookie.Expires = expires.Value;
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 获取cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
                return string.Empty;
            else
                return cookie.Value;
        }
        /// <summary>
        /// 删除cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(string cookieName)
        {
            SetCookieValue(cookieName, "", DateTime.Now.AddDays(-1));
        }
        /// <summary>
        /// 根据图片的全路径判断是否是图片文件,
        /// 如果根据后缀名判断是图片，在判断是否能转换成图片对象，能转换则是图片
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool IsImage(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) && !Path.HasExtension(fullPath)) return false;
            Image img = null;
            try
            {
                string ext = Path.GetExtension(fullPath).ToLower();
                if (!IsImage(ext, true)) return false;
                img = Image.FromFile(fullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (img != null)
                    img.Dispose();
            }
        }
        /// <summary>
        /// 根据扩展名判断是否是图片
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool IsImage(string ext, bool hasPoint)
        {
            string[] extList = new string[] { ".gif", ".jpg", ".jpeg", ".png", ".bmp", ".icon" };
            bool result = false;
            if (hasPoint)
            {
                if (extList.Contains(ext)) result = true;
            }
            else
            {
                if (extList.Contains("." + ext)) result = true;
            }

            return result;
        }
        /// <summary>
        /// 判断当前流是否是图片
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool IsImage(Stream stream)
        {
            Image img = null;
            try
            {
                img = Image.FromStream(stream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (img != null)
                    img.Dispose();
            }
        }
        /// <summary>
        /// 把绝对路径转换成相对路径
        /// </summary>
        /// <param name="imagesurl1"></param>
        /// <returns></returns>
        public static string ToRelativePath(string imagesurl1)
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, "/"); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            return imagesurl2;
        }

        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        /// <summary>
        /// 获取Web.Config中的AppSetting中节点的值
        /// </summary>
        /// <param name="key">AppSetting中节点键</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string key, string defValue = null)
        {
            if (string.IsNullOrEmpty(key)) return defValue;
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 获取Web.Config中的AppSetting中节点的值
        /// </summary>
        /// <param name="key">AppSetting中节点键</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static long GetAppSettingValue(string key, long defValue)
        {
            if (string.IsNullOrEmpty(key)) return defValue;
            string value = ConfigurationManager.AppSettings[key];
            long.TryParse(value, out defValue);
            return defValue;
        }
        /// <summary>
        /// 获取路由的全路径
        /// </summary>
        /// <param name="routeUrl"></param>
        /// <returns></returns>
        public static string GetFullUrl(string routeUrl)
        {
            string rootPath = GetAppSettingValue("RootPath");

            while (!string.IsNullOrEmpty(rootPath) && rootPath.Last() == '/')
            {
                rootPath = rootPath.Remove(rootPath.Length - 1);
            }
            while (!string.IsNullOrEmpty(routeUrl) && routeUrl.First() == '/')
            {
                routeUrl = routeUrl.Substring(1);
            }
            return rootPath + "/" + routeUrl;
        }
        /// <summary>
        /// 过滤html中的p标签
        /// </summary>
        /// <param name="html">html字符串</param>
        /// <param name="maxSize">返回的字符串最大长度为多少</param>
        /// <param name="onlyText">是否只返回纯文本，还是返回带有标签的</param>
        /// <returns></returns>
        public static string FilterHtmlP(string html,int maxSize,bool onlyText = true)
        {
            Regex rReg = new Regex(@"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            var matchs = Regex.Matches(html, @"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            StringBuilder sb = new StringBuilder();
            foreach (Match match in matchs)
            {
                string pContent = match.Value;
                if (onlyText)
                {
                    pContent = Regex.Replace(pContent, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                    pContent = Regex.Replace(pContent, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
                }
                sb.Append(pContent);
            }
            string result = sb.ToString();
            if (string.IsNullOrEmpty(result) || result.Length < maxSize) return result;
            return result.Substring(0, maxSize);
        }
        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            string userIP = "未获取用户IP";

            try
            {
                if (System.Web.HttpContext.Current == null
                 || System.Web.HttpContext.Current.Request == null
                 || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;

        }
    }
}