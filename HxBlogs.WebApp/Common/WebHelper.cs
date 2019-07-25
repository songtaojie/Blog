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
        #region 日期处理函数
        public static string GetDispayDate(DateTime? date, bool showTime = false)
        {
            if (!date.HasValue) return "";
            return GetDispayDate(date.Value, showTime);
        }
        public static string GetDispayDate(DateTime date, bool showTime = false)
        {
            TimeSpan ts = DateTime.Now.Subtract(date);
            if (ts.TotalMinutes < 60) return string.Format("{0} 分钟前", (int)Math.Floor(ts.TotalMinutes));
            if (ts.TotalHours <= 24) return string.Format("{0} 小时前", (int)Math.Floor(ts.TotalHours));
            if (ts.TotalDays <= 7) return string.Format("{0} 天前", (int)Math.Floor(ts.TotalDays));
            if (date.Year == DateTime.Now.Year)
            {
                string timeFormat = showTime ? "MM-dd HH:ss" : "MM-dd";
                return date.ToString(timeFormat);
            }
            string format = showTime ? "yyyy-MM-dd HH:ss" : "yyyy-MM-dd";
            return date.ToString(format); ;
        }
        #endregion
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

        #region 图片相关内容
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
        /// 获取图片链接
        /// </summary>
        /// <param name="sHtmlText">html文本</param>
        /// <returns></returns>
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
        /// 随机获取其中的一个imgurl地址
        /// </summary>
        /// <param name="imgUrl">图片地址，中间用,分割</param>
        /// <returns></returns>
        public static string GetRandomImgUrl(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl)) return imgUrl;
            string[] imgUrlArr = imgUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Random random = new Random();
            int index = random.Next(0, imgUrlArr.Length);
            return imgUrlArr[index];
        }
        #endregion

        #region 路径相关方法
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
        /// 获取头像的url
        /// </summary>
        /// <param name="type">0：代表50*50,1代表80*80,2代表160*160</param>
        /// <returns></returns>
        public static string GetAvatarUrl(int type)
        {
            Model.UserInfo userInfo = UserContext.LoginUser;
            string avatarUrl = string.Empty;
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.AvatarUrl))
            {
                avatarUrl = userInfo.AvatarUrl;
                if (type == 1 || type == 2)
                {
                    int index = avatarUrl.LastIndexOf("50x50");
                    string newUrl = avatarUrl.Substring(0, index);
                    string fullPath = string.Empty;
                    if (type == 1)
                    {
                        newUrl = newUrl + "80x80.png";
                        fullPath = HttpContext.Current.Server.MapPath(newUrl);
                    }
                    else if (type == 2)
                    {
                        newUrl = newUrl + "160x160.png";
                        fullPath = HttpContext.Current.Server.MapPath(newUrl);
                    }
                    if (File.Exists(fullPath))
                        avatarUrl = newUrl;
                }

            }
            if (string.IsNullOrEmpty(avatarUrl)) avatarUrl = GetFullUrl("/images/avatar.png");
            return GetFullUrl(avatarUrl);
        }
        #endregion

        #region 设置方法
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
        #endregion

        /// <summary>
        /// 过滤html中的p标签
        /// </summary>
        /// <param name="html">html字符串</param>
        /// <param name="maxSize">返回的字符串最大长度为多少</param>
        /// <param name="onlyText">是否只返回纯文本，还是返回带有标签的</param>
        /// <returns></returns>
        public static string FilterHtmlP(string html, int maxSize, bool onlyText = true)
        {
            if (string.IsNullOrEmpty(html)) return "";
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