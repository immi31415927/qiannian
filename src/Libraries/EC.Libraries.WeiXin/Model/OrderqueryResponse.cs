namespace EC.Libraries.WeiXin
{
    public class OrderQueryResponse : WXResult
    {
        /// <summary>
        /// 商家订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public int total_fee { get; set; }

    }
}
