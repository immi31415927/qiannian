using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Finance;

namespace EC.Entity.View.Fn.Ext
{
    /// <summary>
    /// 交易日志
    /// </summary>
    public class TradeLogExt : FnTradeLog
    {
        /// <summary>
        /// 卖出名称
        /// </summary>
        public string OutName { get; set; }

        /// <summary>
        /// 买入名称
        /// </summary>
        public string InName { get; set; }
    }
}
