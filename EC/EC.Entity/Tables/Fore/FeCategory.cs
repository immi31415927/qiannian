using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Fore
{
    /// <summary>
    /// 分类
    /// </summary>
    [Serializable]
    public partial class FeVideoCategory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 父级编号
        /// </summary>
        [Description("父级编号")]
        public int ParentSysNo { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        public string Subject { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [Description("图片地址")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        [Description("拼音")]
        public string Pingyin { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Description("显示顺序")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 状态 启用（1）禁用（0）
        /// </summary>
        [Description("状态 启用（1）禁用（0）")]
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
