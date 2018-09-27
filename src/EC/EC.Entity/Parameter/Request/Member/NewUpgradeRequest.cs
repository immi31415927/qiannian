using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Member
{
    /// <summary>
    /// 升级
    /// </summary>
    public class NewUpgradeRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 升级金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 选择等级
        /// </summary>
        public int UpgradeGrade { get; set; }
    }
}
