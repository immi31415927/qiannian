using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Finance
{
    /// <summary>
    /// 保存充值日志
    /// </summary>
    public class SaveRechargeRequest
    {
        /// <summary>
        /// 充值金额
        /// </summary>
        [Description("充值金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 充值类型：续费（10）、充值（20）、升级（20）
        /// </summary>
        [Description("充值类型：续费（10）、充值（20）、升级（20）")]
        public int Type { get; set; }

        /// <summary>
        /// 充值类型：充值（10）、续费（20）
        /// </summary>
        [Description("充值类型：充值（10）、续费（20）")]
        public int RechargeType { get; set; }

        /// <summary>
        /// 续费方式：在线支付（10）、奖金余额（20）
        /// </summary>
        [Description("续费方式：在线支付（10）、奖金余额（20）")]
        public int PayWay { get; set; }

        /// <summary>
        /// 选择会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int SelectGrade { get; set; }
    }
}
