using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class PasswordRequest
    {
        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("用户密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("新密码")]
        public string Password { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("确认密码")]
        public string ConfirmPassword { get; set; }
    }
}
