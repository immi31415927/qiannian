using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 会员扩展输入参数
    /// </summary>
    public class CustomerExtRequest : PageCondition
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 经理编号
        /// </summary>
        public int? ReferrerSysNo { get; set; }

        /// <summary>
        /// 层级会员编号（用于查询层级推荐会员）
        /// </summary>
        public int? LevelCustomerSysNo { get; set; }

        /// <summary>
        /// 层级会员编号(包含会员本身)
        /// </summary>
        public int? SelfLevelCustomerSysNo { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 会员编号列表
        /// </summary>
        public List<int> CustomerSysNoList { get; set; }

        /// <summary>
        /// 层级列表
        /// </summary>
        public List<int> LevelList { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 呢称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public int? Bank { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankNumber { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        [Description("奖金")]
        public decimal? WalletAmount { get; set; }
        /// <summary>
        /// 历史钱包
        /// </summary>
        [Description("历史钱包")]
        public decimal? HistoryWalletAmount { get; set; }
        /// <summary>
        /// 升级基金
        /// </summary>
        [Description("升级基金")]
        public decimal? UpgradeFundAmount { get; set; }
        /// <summary>
        /// 充值总金额
        /// </summary>
        [Description("充值总金额")]
        public decimal? PaymentTotalAmount { get; set; }
        /// <summary>
        /// 普通待结算奖金
        /// </summary>
        [Description("普通待结算奖金")]
        public decimal? GeneralBonus { get; set; }
        /// <summary>
        /// 区域待结算奖金
        /// </summary>
        [Description("区域待结算奖金")]
        public decimal? AreaBonus { get; set; }
        /// <summary>
        /// 全国待结算奖金
        /// </summary>
        [Description("全国待结算奖金")]
        public decimal? GlobalBonus { get; set; }

        /// <summary>
        /// 推荐人名称
        /// </summary>
        public string ReferrerName { get; set; }
    }
}
