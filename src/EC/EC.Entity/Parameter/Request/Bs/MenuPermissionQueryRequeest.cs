using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Bs
{
    /// <summary>
    /// 菜单权限查询
    /// </summary>
    public class MenuPermissionQueryRequeest : PageCondition
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int MenuSysNo { get; set; }
    }
}
