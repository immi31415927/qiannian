using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 股权记录输入参数
    /// </summary>
    public class StockRecordRequest : PageCondition
    {
        /// <summary>
        /// 投资类型 （股权挂售（10）平台回购（20）股权申购（30））
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 操作人类型 （会员(10)  管理员(20)）
        /// </summary>
        public int? OperatorType { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int? OperatorSysNo { get; set; }

        /// <summary>
        /// 状态（待处理(10)  部分处理 (20)  已处理(30)）
        /// </summary>
        public List<int> StatusList { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结数时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 前几条数据
        /// </summary>
        public int? TopNum { get; set; } 
    }
}
