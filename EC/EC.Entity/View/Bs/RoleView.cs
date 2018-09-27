using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.View.Bs
{
    /// <summary>
    /// 角色页面参数
    /// </summary>
    public class RoleView
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 选中
        /// </summary>
        public bool Selected { get; set; }
    }
}
