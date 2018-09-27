using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Bs
{
    /// <summary>
    /// 授权输入参数
    /// </summary>
    public class AuthorizeRequest
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int? UserSysNo { get; set; }
        /// <summary>
        /// 权限状态
        /// </summary>
        public int? RoleStatus { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int? RoleSysNo { get; set; }
        /// <summary>
        /// 资源编号
        /// </summary>
        public int? AuthorizeSysNo { get; set; }
        /// <summary>
        /// 资源类型 菜单（10） 权限（20）
        /// </summary>
        public int? AuthorizeType { get; set; }
    }
}
