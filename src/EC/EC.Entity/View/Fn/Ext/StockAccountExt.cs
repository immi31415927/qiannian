using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Finance;

namespace EC.Entity.View.Fn.Ext
{
    /// <summary>
    /// 股权账户扩展
    /// </summary>
    public class StockAccountExt : FnStockAccount
    {
        /// <summary>
        /// 会员名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 会员规则编号
        /// </summary>
        public string SerialNumber { get; set; } 
    }
}
