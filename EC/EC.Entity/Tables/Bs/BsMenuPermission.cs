using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Serializable]
    public class BsMenuPermission
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int PermissionSysNo { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int MenuSysNo { get; set; }
    }
}
