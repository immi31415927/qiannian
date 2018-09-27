namespace EC.Entity.Parameter.Request.Finance
{
    /// <summary>
    /// 更新支付状态参数
    /// </summary>
    public class UpdatePayStatusRequest
    {

        /// <summary>
        /// 订单来源单号
        /// </summary>
        public string OrderSysNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易凭证
        /// </summary>
        public string VoucherNo { get; set; }
    }
}
