using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 股权账户(数据库表名：Agent_FnStockAccount)
    /// </summary>
    public class FnStockAccount
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 股权数
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 股权待售
        /// </summary>
        public int StockForSale { get; set; }

        /// <summary>
        /// 股权已售
        /// </summary>
        public int StockSold { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
