using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 奖金记录
    /// </summary>
    public class FnBonusLogQueryRequest : PageCondition
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 奖金类型
        /// </summary>
        public int? Type { get; set; }
    }
}
