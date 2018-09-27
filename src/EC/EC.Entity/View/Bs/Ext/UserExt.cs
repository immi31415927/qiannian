using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Bs;

namespace EC.Entity.View.Bs.Ext
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    public class UserExt : BsUser
    {
        /// <summary>
        /// 角色选中列表
        /// </summary>
        public string RoleSelectedList { get; set; }
    }
}
