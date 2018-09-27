using System;
using System.ComponentModel;

namespace EC.Entity.View.CRM
{
    /// <summary>
    /// 级别
    /// </summary>
    public class GradeView
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 升级金额
        /// </summary>
        [Description("升级金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Description("层级")]
        public int Top { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Description("内容")]
        public string Content { get; set; }

        /// <summary>
        /// 过期天数
        /// </summary>
        [Description("过期天数")]
        public int ExpiresDay { get; set; }

    }
}
