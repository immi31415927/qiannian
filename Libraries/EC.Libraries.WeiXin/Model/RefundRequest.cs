using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 退款请求
    /// </summary>
    public class RefundRequest
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }


        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }

        /// <summary>
        /// 二选一
        /// 商户订单号(商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。)
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal refund_fee { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 二选一
        /// 微信订单号(微信生成的订单号，在支付通知中有返回)
        /// </summary>
        public string transaction_id { get; set; }



        /// <summary>
        /// apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// 是否使用证书
        /// </summary>
        public bool isUseCert { get; set; }

        /// <summary>
        /// 证书
        /// </summary>
        public string cert { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
