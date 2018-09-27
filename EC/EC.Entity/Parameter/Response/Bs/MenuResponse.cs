using System;
using System.Collections.Generic;
using System.ComponentModel;
using EC.Entity.Tables.Bs;

namespace EC.Entity.Parameter.Response.Bs
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuResponse
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        [Description("父级编号")]
        public int ParentSysNo { get; set; }

        /// <summary>
        /// 导航显示 显示（10） 不显示（20）
        /// </summary>
        [Description("导航显示 显示（10） 不显示（20）")]
        public int IsNav { get; set; }

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
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Description { get; set; }

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
        /// 描述
        /// </summary>
        [Description("描述")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        [Description("菜单权限")]
        public List<PermissionResponse> PermissionResponse { get; set; }
    }
}
