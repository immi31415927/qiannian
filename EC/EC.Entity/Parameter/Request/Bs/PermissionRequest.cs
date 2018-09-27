using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Bs
{
    /// <summary>
    /// 功能权限输入参数
    /// </summary>
    public class PermissionRequest : PageCondition
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Description("菜单编号")]
        public int? MenuSysNo { get; set; }
    }
}
