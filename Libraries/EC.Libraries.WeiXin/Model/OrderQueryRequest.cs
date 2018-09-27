namespace EC.Libraries.WeiXin.Model
{
    public class OrderQueryRequest
    {
        /// <summary>
        /// 商户账号appid
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 公众账号apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// 商家订单号(必填)
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家订单号(必填)
        /// </summary>
        public string bank_note { get; set; }
        
    }
}
