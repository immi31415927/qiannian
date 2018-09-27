using System;
using System.Linq;
using System.Web;
using EC.Libraries.Util;

namespace EC.Libraries.Util
{
    /// <summary>
    /// Cookie工具类
    /// </summary>
    public class CookieUtil
    {
        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string key, string value, DateTime expires)
        {
            var cookie = new HttpCookie(key) {Value = HttpUtility.UrlEncode(value), Expires = expires};
            if (HttpContext.Current.Response.Cookies.AllKeys.Any(o => o == key))
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        public static void SetCookie(string key, string value)
        {
            SetCookie(key, value, DateTime.Now.AddHours(24));
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns>cookie字符串</returns>
        public static string Get(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            return cookie == null ? string.Empty : HttpUtility.UrlDecode(cookie.Value);
        }

        /// <summary>
        /// 返回指定名称的Cookie实例
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns>Cookie对象</returns>
        public static HttpCookie GetCookie(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            return cookie;
        }

        /// <summary>
        /// 返回将指定名称的Cookie的值反序列化的对象
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="key">Cookie名称</param>
        /// <returns>Cookie字符串反序列化后后对象</returns>
        public static T Get<T>(string key) where T : class
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null) return null;
            return !string.IsNullOrWhiteSpace(cookie.Value) ? HttpUtility.UrlDecode(cookie.Value).ToObject<T>() : null;
        }

        /// <summary>
        /// 将指定的Cookie对象写入Response
        /// </summary>
        /// <param name="cookie">Cookie对象</param>
        public static void Set(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 删除指定Cookie
        /// </summary>
        /// <param name="key">Cookie Key</param>
        public static void Remove(string key)
        {
            SetCookie(key, "", DateTime.Now.AddDays(-1));
        }
    }
}
