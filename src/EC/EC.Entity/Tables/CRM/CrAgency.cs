using System;
using System.ComponentModel;

namespace EC.Entity.Tables.CRM
{
    /// <summary>
    /// 代理审核
    /// </summary>
    /// <remarks>2017-09-25</remarks>
    [Serializable]
    public partial class CrAgency
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Description("申请人")]
        public string Applicant { get; set; }
        /// <summary>
        /// 申请人编号
        /// </summary>
        [Description("申请人编号")]
        public int ApplicantSysNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 已审核（1）已拒绝（0）
        /// </summary>
        [Description("状态 已审核（1）已拒绝（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
