using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 更新钱包
    /// </summary>
    public class UpdateWalletAmountRequest
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("会员编号")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        [Description("提现金额")]
        public decimal WalletAmount { get; set; }
        /// <summary>
        /// 提现历史总金额
        /// </summary>
        [Description("提现历史总金额")]
        public decimal WithdrawalsTotalAmount { get; set; }

        /// <summary>
        /// 升级基金
        /// </summary>
        [Description("升级基金")]
        public decimal UpgradeFundAmount { get; set; }

        /// <summary>
        /// 历史钱包
        /// </summary>
        [Description("历史钱包")]
        public decimal HistoryWalletAmount { get; set; }
        
        
    }
}
