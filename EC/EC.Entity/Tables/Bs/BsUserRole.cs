using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Serializable]
    public class BsUserRole
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [Description("用户编号")]
        public int UserSysNo { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        [Description("角色编号")]
        public int RoleSysNo { get; set; }

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
    }
}
