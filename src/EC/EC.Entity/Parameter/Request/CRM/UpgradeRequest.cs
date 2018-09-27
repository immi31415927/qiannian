using System;
using System.Collections.Generic;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 代理升级参数
    /// </summary>
    public class UpgradeRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 选择会员等级
        /// </summary>
        public int SelectGrade { get; set; }

        /// <summary>
        /// 当前会员等级
        /// </summary>
        public int CurrentGrade { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        public List<int> Ids;
    }

}
