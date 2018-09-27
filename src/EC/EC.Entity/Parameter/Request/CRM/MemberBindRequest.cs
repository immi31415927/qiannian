using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 绑定账号参数
    /// </summary>
    public class MemberBindRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string Account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("用户密码")]
        public string Password { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
    }
}
