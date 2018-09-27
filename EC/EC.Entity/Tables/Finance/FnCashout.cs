using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 提现申请
    /// </summary>
    [Serializable]
    public partial class FnCashout
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 提现会员
        /// </summary>
        [Description("提现会员")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 真实名称
        /// </summary>
        [Description("真实名称")]
        public string RealName { get; set; }
        /// <summary>
        /// 提现方式：银联（10）
        /// </summary>
        [Description("提现方式：银联（10）,微信（20）")]
        public int CashoutType { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [Description("提现金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        public string Reject { get; set; }
        /// <summary>
        /// 状态：0(驳回)、处理中(10)、 完成 (20)  
        /// </summary>
        [Description("状态：0(驳回)、处理中(10)、 完成 (20)  ")]
        public int Status { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        [Description("提现时间")]
        public DateTime CreatedDate { get; set; }
    }
}
