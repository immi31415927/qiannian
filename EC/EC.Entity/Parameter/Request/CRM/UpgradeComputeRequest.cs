namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 升级计算
    /// </summary>
    public class UpgradeComputeRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 选择会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int SelectGrade { get; set; }
    }
}
