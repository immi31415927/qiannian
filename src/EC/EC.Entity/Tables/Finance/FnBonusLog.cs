using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 奖金记录
    /// </summary>
    [Serializable]
    public partial class FnBonusLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 获取奖金会员编号
        /// </summary>
        [Description("获取奖金会员编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 奖金来源会员编号
        /// </summary>
        [Description("奖金来源会员编号")]
        public int SourceSysNo { get; set; }
        /// <summary>
        /// 来源会员编号
        /// </summary>
        [Description("来源会员编号")]
        public string SourceSerialNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Description("金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 奖金类型：代理奖励（10）、股东奖励（20）
        /// </summary>
        [Description("奖金类型：代理奖励（10）、股东奖励（20）")]
        public int Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 状态：撤销（10）、已撤销（20）
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
