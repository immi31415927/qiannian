namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 申请退款输出
    /// </summary>
    public class RefundReponse : WXResult
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }
    }
}
