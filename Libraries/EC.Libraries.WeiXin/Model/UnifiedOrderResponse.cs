namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// 统一下单返回参数
    /// </summary>
    public class UnifiedOrderResponse : WXResult
    {

        /// <summary>
        /// 交易类型(调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，,H5支付固定传MWEB)
        /// </summary>
        public bool trade_type { get; set; }

        /// <summary>
        /// 预支付交易会话标识(微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时,针对H5支付此参数无特殊用途)
        /// </summary>
        public string prepay_id { get; set; }

        /// <summary>
        /// (支付跳转链接)mweb_url为拉起微信支付收银台的中间页面，可通过访问该url来拉起微信客户端，完成支付,mweb_url的有效期为5分钟。
        /// </summary>
        public string mweb_url { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }
}
