using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Video
{
    public class SaveVideoCategoryRequest
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int? Id { get; set; }
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
        /// 链接地址
        /// </summary>
        [Description("链接地址")]
        public string Url { get; set; }
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
