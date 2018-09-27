using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 提现申请
    /// </summary>
    public class CashoutRequest
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 钱包
        /// </summary>
        public decimal WalletAmount { get; set; }
        /// <summary>
        /// 提现总金额
        /// </summary>
        [Description("WithdrawalsTotalAmount")]
        public decimal WithdrawalsTotalAmount { get; set; }
        /// <summary>
        /// 升级基金
        /// </summary>
        public decimal UpgradeFundAmount { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 提现方式
        /// </summary>
        public int CashoutType { get; set; }

        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertPath { get; set; }
        

    }
}
