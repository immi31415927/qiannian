
namespace EC.Entity.Parameter.Response.CRM
{
    /// <summary>
    /// 升级计算输出
    /// </summary>
    public class UpgradeComputeResponse
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 扣费金额
        /// </summary>
        public decimal DeductedAmount { get; set; }
        /// <summary>
        /// 在线支付
        /// </summary>
        public decimal OnlinePaymentAmount { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Tips { get; set; }
    }
}
