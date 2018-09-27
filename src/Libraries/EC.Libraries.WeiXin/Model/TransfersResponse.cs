namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 企业打款输出参数
    /// </summary>
    public class TransfersResponse : WXResult
    {
        /// <summary>
        /// 业务结果
        /// </summary>
        public string mch_appid { get; set; }

        /// <summary>
        /// 业务结果
        /// </summary>
        public string mchid { get; set; }

        /// <summary>
        /// 业务结果
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string payment_no { get; set; }

        /// <summary>
        /// 微信支付成功时间
        /// </summary>
        public string payment_time { get; set; }
    }
}
