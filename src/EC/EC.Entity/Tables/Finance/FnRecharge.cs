using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 充值日志
    /// </summary>
    [Serializable]
    public partial class FnRecharge
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        [Description("序列号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("获取奖金会员编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 充值类型
        /// </summary>
        [Description("充值类型")]
        public int Type { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        [Description("充值金额")]
        public Decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
