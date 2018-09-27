using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    public class UpdatePayWalletAmountRequest
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("会员编号")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 升级基金
        /// </summary>
        [Description("升级基金")]
        public decimal UpgradeFundAmount { get; set; }

        /// <summary>
        /// 充值总金额
        /// </summary>
        [Description("充值总金额")]
        public decimal RechargeTotalAmount { get; set; }

    }
}
