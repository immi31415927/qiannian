using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 更新过期时间
    /// </summary>
    public class UpdateExpiresTimeRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Description("用户账号")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        [Description("用户组")]
        public int GroupSysNo { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Description("过期时间")]
        public int ExpiresTime { get; set; }
    }
}
