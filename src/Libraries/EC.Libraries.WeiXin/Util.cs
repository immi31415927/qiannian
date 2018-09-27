using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace EC.Libraries.WeiXin
{
    public class Util
    {
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, Encoding encoding)
        {
            string retStr;

            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用指定编码方式把字符串转化为字节数组．
            try
            {
                inputBye = encoding.GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("utf-8").GetBytes(encypStr);

            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, string charset = "utf-8")
        {
            try
            {
                //使用指定编码
                return GetMD5(encypStr, Encoding.GetEncoding(charset));
            }
            catch (Exception ex)
            {
                //使用UTF-8编码
                return GetMD5("utf-8", Encoding.GetEncoding(charset));
            }
        }

        /// <summary>
        /// 把XML数据转换为SortedDictionary<string, string>集合
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public SortedDictionary<string, string> StringFromXml(string xml)
        {
            SortedDictionary<string, string> dc = new SortedDictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!dc.ContainsKey(name))
                    {
                        dc.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dc;
        }

        /// <summary>
        /// 获取Unix时间
        /// </summary>
        /// <param name="dateTime">要转换的时间</param>
        /// <returns>Unix时间</returns>
        public static string GetUnixTime(DateTime dateTime)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 随机生成Noncestr
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            return GetMD5(Guid.NewGuid().ToString(), "UTF-8");
        }
    }
}
