using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 短信日志
    /// </summary>
    [Serializable]
    public partial class BsSMS
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        [Description("短信内容")]
        public string Content { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        [Description("优先级")]
        public int Priority { get; set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        [Description("失败次数")]
        public int FailureTimes { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
