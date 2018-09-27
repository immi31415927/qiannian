using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.Finance
{
    /// <summary>
    /// 股权记录(数据库表名：Agent_FnStockRecord)
    /// </summary>
    public class FnStockRecord
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 投资类型 （股权挂售（10）平台回购（20）股权申购（30））
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 操作人类型 （会员(10)  管理员(20)）
        /// </summary>
        public int OperatorType { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorSysNo { get; set; }

        /// <summary>
        /// 股权数
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 已交易股权数
        /// </summary>
        public int TradedStockNum { get; set; }

        /// <summary>
        /// 当前股权金额
        /// </summary>
        public decimal CurrentStockAmount { get; set; }

        /// <summary>
        /// 状态（待处理(10)  部分处理 (20)  已处理(30)）
        /// </summary>
        public int Status { get; set; }

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
