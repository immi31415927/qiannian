using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 交易日志输入参数
    /// </summary>
    public class TradeLogRequest : PageCondition
    {
        /// <summary>
        /// 来源类型（股权记录（10））
        /// </summary>
        public int? SourceType { get; set; }

        /// <summary>
        /// 卖出操作人类型
        /// </summary>
        public int? OutOperatorType { get; set; }

        /// <summary>
        /// 买入操作人类型
        /// </summary>
        public int? InOperatorType { get; set; }

        #region 扩展条件

        /// <summary>
        /// 卖出名称
        /// </summary>
        public string OutName { get; set; }

        /// <summary>
        /// 买入名称
        /// </summary>
        public string InName { get; set; }

        #endregion
    }
}
