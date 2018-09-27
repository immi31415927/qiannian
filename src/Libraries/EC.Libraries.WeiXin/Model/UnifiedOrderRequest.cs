using System.Xml.Serialization;

namespace EC.Libraries.WeiXin
{
    public class UnifiedOrderRequest
    {
        /// <summary>
        /// 公众账号apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// 公众账号idapiKey
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public string body;

        /// <summary>
        /// 订单号
        /// </summary>
        public string order_no { get; set; }

        /// <summary>
        /// 支付金额，单位：分
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 用户的公网ip，不是商户服务器IP
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 用户的公网ip，不是商户服务器IP
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string trade_type;

        /// <summary>
        /// 用户的openId
        /// </summary>
        public string open_id;
        
    }
}
