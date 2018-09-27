using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EC.Libraries.WeiXin
{
    public class BarCodePayImpl
    {
        /// <summary>
        /// apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable parameters;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarCodePayImpl()
        {
            parameters = new Hashtable();
        }
        #endregion

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

        public string ParseXML()
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (string k in parameters.Keys)
            {
                var v = (string)parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {
                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <returns></returns>
        public string CreateMd5Sign()
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                sb.Append(k + "=" + v + "&");
            }
            sb.Append("key=" + apiKey);
            string sign = GetMD5(sb.ToString());
            return sign;
        }

        /// <summary>
        /// MD5加密码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /**
        * 提交被扫支付API
        * 收银员使用扫码设备读取微信用户刷卡授权码以后，二维码或条码信息传送至商户收银台，
        * 由商户收银台或者商户后台调用该接口发起支付。
        * @param WxPayData inputObj 提交给被扫支付API的参数
        * @param int timeOut 超时时间
        * @throws Exception
        * @return 成功时返回调用结果，其他抛异常
        */
        public void Micropay()
        {

        }
    }
}
