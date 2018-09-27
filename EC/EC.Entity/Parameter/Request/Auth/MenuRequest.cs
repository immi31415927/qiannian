using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Auth
{
    /// <summary>
    /// 菜单 
    /// </summary>
    public class MenuRequest
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int? Id { get; set; }
    }
}
