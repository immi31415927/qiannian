using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
	/// 码表
	/// </summary>
	[Serializable]
    public class BsCode
	{
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public int Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [Description("代码")]
        public int Code { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [Description("参数")]
        public string Value { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 状态 启用（1）禁用（0）
        /// </summary>
        [Description("状态 启用（1）禁用（0）")]
        public int EnableStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
 	}
}

