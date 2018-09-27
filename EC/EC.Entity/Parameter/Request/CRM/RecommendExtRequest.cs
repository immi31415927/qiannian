using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 关注扩展输入参数
    /// </summary>
    public class RecommendExtRequest : PageCondition
    {
        /// <summary>
        ///推荐人 
        /// </summary>
        public int? ReferrerSysNo { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelNumber { get; set; }
    }
}
