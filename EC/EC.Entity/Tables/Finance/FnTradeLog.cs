using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 交易日志(数据库表名：Agent_FnTradeLog)
    /// </summary>
    public class FnTradeLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 来源类型（股权记录（10））
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 来源编号
        /// </summary>
        public int SourceSysNo { get; set; }

        /// <summary>
        /// 卖出操作人类型 （会员(10)  管理员(20)）
        /// </summary>
        public int OutOperatorType { get; set; }

        /// <summary>
        /// 卖出操作人编号
        /// </summary>
        public int OutOperatorSysNo { get; set; }

        /// <summary>
        /// 买入操作人类型 （会员(10)  管理员(20)）
        /// </summary>
        public int InOperatorType { get; set; }

        /// <summary>
        /// 买入操作人编号
        /// </summary>
        public int InOperatorSysNo { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 当前股权金额
        /// </summary>
        public decimal CurrentStockAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
