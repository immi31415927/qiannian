using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Auth
{
    /// <summary>
    /// 角色对象
    /// </summary>
    public class RoleQueryRequeest : PageCondition
    {
        /// <summary>
        /// 启用状态
        /// </summary>
        public int? Status { get; set; }
    }
}
