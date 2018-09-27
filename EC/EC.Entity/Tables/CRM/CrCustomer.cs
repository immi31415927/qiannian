using System;
using System.ComponentModel;

namespace EC.Entity.Tables.CRM
{
    /// <summary>
    /// 会员表
	/// </summary>
    /// <remarks>2017-09-06</remarks>
	[Serializable]
    public class CrCustomer
	{
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Description("账号")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Description("编码")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// 经理编号
        /// </summary>
        [Description("经理编号")]
        public int ReferrerSysNo { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 真实名称
        /// </summary>
        [Description("真实名称")]
        public string RealName { get; set; }
        /// <summary>
        /// 呢称
        /// </summary>
        [Description("呢称")]
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        public string IDNumber { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 团队人数
        /// </summary>
        [Description("团队人数")]
        public int TeamCount { get; set; }
        /// <summary>
        /// 会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        [Description("会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）")]
        public int Grade { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [Description("层级")]
        public int Level { get; set; }
        /// <summary>
        /// 层级推荐会员（该会员所以层级推荐人编号（以逗号隔开 如：1,2,3））
        /// </summary>
        [Description("层级推荐会员（该会员所以层级推荐人编号（以逗号隔开 如：1,2,3））")]
        public string LevelCustomerStr { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        [Description("银行")]
        public int Bank { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [Description("银行卡号")]
        public string BankNumber { get; set; }
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

        #region 待结算
        public decimal SettledBonus10 { get; set; }
        public decimal SettledBonus20 { get; set; }
        public decimal SettledBonus30 { get; set; }
        public decimal SettledBonus40 { get; set; }
        public decimal SettledBonus50 { get; set; }
        public decimal SettledBonus60 { get; set; }
        public decimal SettledBonus70 { get; set; }
        public decimal SettledBonus80 { get; set; }
        #endregion

        /// <summary>
        /// 过期日期
        /// </summary>
        [Description("过期日期")]
        public string ExpiresTime { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        [Description("关注时间")]
        public string FollowDate { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        [Description("注册IP")]
        public string RegisterIP { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Description("注册时间")]
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Description("最后登录IP")]
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Description("最后登陆时间")]
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        [Description("登录次数")]
        public int LoginCount { get; set; }
        /// <summary>
        /// 状态：启用（1）、禁用（0）
        /// </summary>
        [Description("状态：启用（1）、禁用（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
 	}
}

