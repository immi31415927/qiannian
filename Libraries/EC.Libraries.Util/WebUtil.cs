using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EC.Libraries.Util
{
    /// <summary>
    /// Web工具
    /// </summary>
    public class WebUtil
    {
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string path)
        {
            if (path.ToLower().StartsWith("http://"))
            {
                return path;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else //非web程序引用
            {
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\"))
                {
                    path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        /// <summary>
        /// 检查是否为本地地址正则
        /// </summary>
        private static Regex checkUrlRegex = null;

        /// <summary>
        /// 检查地址是否为本地地址（包含相对路径和绝对路径，例如：xxx://开头的都不是本地地址）
        /// </summary>
        /// <param name="url"></param>
        /// <returns>bool</returns>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if (url.StartsWith("/"))
                return true;

            //先判断前缀
            if (
                url.StartsWith("http://", true, null)
                || url.StartsWith("https://", true, null)
                || url.StartsWith("ftp://", true, null)
                )
            {
                return false;
            }
            if (checkUrlRegex == null)
                checkUrlRegex = new Regex(@"^\w+://.*$", RegexOptions.IgnoreCase);

            return !checkUrlRegex.IsMatch(url);
        }

        /// <summary>
        /// 生成指定长度随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns>指定长度的随机数</returns>
        public static string Number(int length, bool sleep)
        {
            if (sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成8位长度随机数字、首字母不能为零
        /// </summary>
        /// <returns>指定长度的随机数</returns>
        public static string Rand()
        {
            if (true)
                System.Threading.Thread.Sleep(2);

            string result = "";

            var random = new Random();

            for (int i = 0; i < 7; i++)
            {
                result += random.Next(10).ToString();
            }
            return new Random().Next(1, 9) + result;
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(int length, bool sleep)
        {
            var pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            if (sleep)
                System.Threading.Thread.Sleep(3);
            
            string result = "";

            var random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, pattern.Length);
                result += pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 隐藏手机号中间4位
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>格式:135****0534</returns>
        public static string HideMobilePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";
            string pattern = @"(1[3,5,8]\d)(\d{4})(\d{4})";
            Regex reg = new Regex(pattern);
            return reg.Replace(phone, "$1****$3");
        }

        /// <summary>
        /// 将时间转换为Unix格式的刻度
        /// </summary>
        /// <param name="time"></param>
        /// <returns>刻度</returns>
        public static long ConvertDateTimeUnix(System.DateTime time)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = time;
            TimeSpan t = (dtNow - dtStart);
            return (long)t.TotalSeconds * 1000;
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (int)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static DateTime IntToDateTime(int timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timestamp);
        }

        /// <summary>
        /// 获取当前页面URL参数
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-1-21 苟治国 添加注释</remarks>
        public static string GetUrl()
        {
            if (System.Web.HttpContext.Current.Request.Url != null)
                return System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            else
                return "";
        }

        /// <summary>
        /// 获取当前页面URL来源
        /// </summary>
        /// <returns></returns>
        public static string GetSource()
        {
            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                return System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
            else
                return "";
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>参数字符串</returns>
        public static string buildParamStr(Dictionary<String, String> param)
        {
            String paramStr = String.Empty;
            foreach (var key in param.Keys.ToList())
            {
                if (param.Keys.ToList().IndexOf(key) == 0)
                {
                    paramStr += (key + "=" + param[key]);
                }
                else
                {
                    paramStr += ("&" + key + "=" + param[key]);
                }
            }
            return paramStr;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parames">请求参数</param>
        /// <param name="encoding">编码方式</param>
        public static String Get(String url, Dictionary<String, String> parames, Encoding encoding = null)
        {
            var paramStr = buildParamStr(parames);

            var request = WebRequest.Create(url + "?" + paramStr) as HttpWebRequest;
            request.Method = "GET";
            var result = request.GetResponse() as HttpWebResponse;
            encoding = encoding ?? Encoding.Default;
            return GetResponseAsString(result, encoding);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parames">请求参数</param>
        public static String Post(String url, Dictionary<String, String> parames)
        {
            string paramStr = buildParamStr(parames);

            HttpWebRequest request = WebRequest.Create(url + "?" + paramStr) as HttpWebRequest;
            request.Method = "POST";
            request.Timeout = 30000;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = paramStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(paramStr);
            writer.Flush();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string retString = reader.ReadToEnd();
            return retString; 
        }

        ///<summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static String GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null)
                {
                    reader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
        }

        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            try
            {
                string ip;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                else
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
                
                return ip;
            }
            catch 
            {
                return "8.8.8.8";
            }
        }

        /// <summary>     
        /// 去除HTML标记     
        /// </summary>     
        /// <param name="strHtml">包括HTML的源码 </param>     
        /// <returns>已经去除后的文字</returns>  
        public static string StripHTML(string strHtml)
        {
            string[] aryReg =
                {
                    @"<script[^>]*?>.*?</script>",
                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                    @"([ ])[\s]+",
                    @"&(quot|#34);",
                    @"&(amp|#38);",
                    @"&(lt|#60);",
                    @"&(gt|#62);",
                    @"&(nbsp|#160);",
                    @"&(iexcl|#161);",
                    @"&(cent|#162);",
                    @"&(pound|#163);",
                    @"&(copy|#169);",
                    @"&#(\d+);",
                    @"-->",
                    @"<!--.* "
                };
            string[] aryRep =
                {
                    "",
                    "",
                    "",
                    "\"",
                    "&",
                    "<",
                    ">",
                    " ",
                    "\xa1", //chr(161),    
                    "\xa2", //chr(162),    
                    "\xa3", //chr(163),     
                    "\xa9", //chr(169),        
                    "",
                    " ",
                    ""
                };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace(" ", "");
            return strOutput;
        }

        /// <summary>
        /// 判断访问是否来至于手机等设备
        /// </summary>
        /// <param name="AgentStr">UserAgent参数</param>
        /// <returns>是否来至手机访问</returns>
        /// <remarks>2014-09-23 苟治国 创建</remarks> 
        public static bool IsComeFromMobile(string AgentStr)
        {
            var AgentRegex = new System.Text.RegularExpressions.Regex(@"iemobile|iphone|ipod|android|nokia|sonyericsson|blackberry|samsung|sec\-|windows ce|motorola|mot\-|up.b|midp\-", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(AgentStr) || AgentRegex.IsMatch(AgentStr))
                return true;
            return false;
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>参数字符串</returns>
        public static string buildParamStr(string reqeustUrl, Dictionary<String, String> param)
        {
            String paramStr = String.Empty;
            foreach (var key in param.Keys.ToList())
            {
                if (param.Keys.ToList().IndexOf(key) == 0)
                {
                    paramStr += (key + "=" + param[key]);
                }
                else
                {
                    paramStr += ("&" + key + "=" + param[key]);
                }
            }
            return reqeustUrl + paramStr;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1(string str)
        {
            byte[] strRes = Encoding.UTF8.GetBytes(str);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }
    }
}
