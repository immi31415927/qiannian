using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 提现申请
    /// </summary>
    public class FnCashoutQueryRequest : PageCondition
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

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
