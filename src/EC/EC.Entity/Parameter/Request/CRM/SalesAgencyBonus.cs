using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 代理销售奖金
    /// </summary>
    public class SalesAgencyBonus
    {
        /// <summary>
        /// 会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 代理销售奖金上级
    /// </summary>
    public class ParentIds
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        ///  普通会员（10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int Grade { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 经理编号
        /// </summary>
        public int ReferrerSysNo { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 奖金
        /// </summary>
        [Description("奖金")]
        public decimal WalletAmount { get; set; }
        /// <summary>
        /// 历史钱包
        /// </summary>
        [Description("历史钱包")]
        public decimal HistoryWalletAmount { get; set; }
        /// <summary>
        /// 提现总金额（2017-12-5 添加）
        /// </summary>
        [Description("提现总金额")]
        public decimal WithdrawalsTotalAmount { get; set; }
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
        /// <summary>
        /// 普通待结算奖金
        /// </summary>
        [Description("普通待结算奖金")]
        public decimal GeneralBonus { get; set; }
        /// <summary>
        /// 区域待结算奖金
        /// </summary>
        [Description("区域待结算奖金")]
        public decimal AreaBonus { get; set; }
        /// <summary>
        /// 全国待结算奖金
        /// </summary>
        [Description("全国待结算奖金")]
        public decimal GlobalBonus { get; set; }
    }

    /// <summary>
    /// 批量更新父类对象
    /// </summary>
    public class BatchUpgradeParent
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 钱包
        /// </summary>
        public decimal WalletAmount { get; set; }
        /// <summary>
        /// 普通待结算奖金
        /// </summary>
        public decimal GeneralBonus { get; set; }
        /// <summary>
        /// 区域待结算奖金
        /// </summary>
        public decimal AreaBonus { get; set; }
        /// <summary>
        /// 全国待结算奖金
        /// </summary>
        public decimal GlobalBonus { get; set; }

        #region 待结算
        public decimal SettledBonus10 { get; set; }
        public decimal SettledBonus20 { get; set; }
        public decimal SettledBonus30 { get; set; }
        public decimal SettledBonus40 { get; set; }
        public decimal SettledBonus50 { get; set; }
        public decimal SettledBonus60 { get; set; }
        public decimal SettledBonus70 { get; set; }
        public decimal SettledBonus80 { get; set; }
        public decimal SettledBonus90 { get; set; }
        #endregion
    }
}
