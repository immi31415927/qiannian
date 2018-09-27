using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.SMS
{
    /// <summary>
    /// 短信查询
    /// </summary>
    public class SMSQueryRequeest : PageCondition
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int? Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        public string EndTime { get; set; }
    }
}
