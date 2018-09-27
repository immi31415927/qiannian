using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace EC.Libraries.WeiXin
{
    public class OrderNotifyImpl
    {
        /// <summary>
        /// apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// xmlMap
        /// </summary>
        private Hashtable xmlMap;

        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable parameters;

        /// <summary>
        /// 当前上下文
        /// </summary>
        protected HttpContext HttpContext;

        #region 构造函数
        /// <summary>
        /// 获取页面提交的get和post参数
        /// </summary>
        public OrderNotifyImpl()
        {
            parameters = new Hashtable();

            //this.HttpContext = httpContext ?? HttpContext.Current;
            //NameValueCollection collection;
            ////post data
            //if (this.HttpContext.Request.HttpMethod == "POST")
            //{
            //    collection = this.HttpContext.Request.Form;
            //    foreach (string k in collection)
            //    {
            //        string v = (string)collection[k];
            //        this.Add(k, v);
            //    }
            //}
            ////query string
            //collection = this.HttpContext.Request.QueryString;
            //foreach (string k in collection)
            //{
            //    string v = (string)collection[k];
            //    this.Add(k, v);
            //}

            //if (System.Web.HttpContext.Current.Request.InputStream.Length > 0)
            //{

            //    var requestStream = System.Web.HttpContext.Current.Request.InputStream;
            //    byte[] requestByte = new byte[requestStream.Length];
            //    requestStream.Read(requestByte, 0, (int)requestStream.Length);

            //    XmlDocument xmlDoc = new XmlDocument();
            //    xmlDoc.LoadXml(Encoding.UTF8.GetString(requestByte));
            //    XmlNode root = xmlDoc.SelectSingleNode("xml");
            //    XmlNodeList xmlNodeList = root.ChildNodes;

            //    foreach (XmlNode xnf in xmlNodeList)
            //    {
            //        xmlMap.Add(xnf.Name, xnf.InnerText);
            //        this.Add(xnf.Name, xnf.InnerText);
            //    }
            //}
        }
        #endregion

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string Get(string parameter)
        {
            string s = (string)parameters[parameter];
            return (null == s) ? "" : s;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            if (key != null && key != "")
            {
                if (parameters.Contains(value))
                {
                    parameters.Remove(value);
                }
                parameters.Add(key, value);
            }
        }

        /// <summary>
        /// MD5加密码
        /// </summary>
        /// <param name="str"></param>
        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            var retStr = BitConverter.ToString(md5data);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。return boolean
        /// </summary>
        public Boolean IsTenpaySign()
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0 && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }
            sb.Append("key=" + apiKey);

            string sign = GetMD5(sb.ToString()).ToLower();

            return Get("sign").ToLower().Equals(sign);
        }

    }
}
