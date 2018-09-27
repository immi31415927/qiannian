using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 授权表
    /// </summary>
    [Serializable]
    public class BsAuthorize
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        [Description("角色编号")]
        public int RoleSysNo { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [Description("资源编号")]
        public int AuthorizeSysNo { get; set; }

        /// <summary>
        /// 资源类型 菜单（10） 权限（20）
        /// </summary>
        [Description("资源类型 菜单（10） 权限（20）")]
        public int AuthorizeType { get; set; }
    }
}
