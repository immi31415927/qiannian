using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 会员扩展批量数据处理输入参数
    /// </summary>
    public class CustomerExtBatchRequest
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// 钱包（新增正数 减少负数）
        /// </summary>
        public decimal? WalletAmount { get; set; }

        /// <summary>
        /// 历史钱包
        /// </summary>
        public decimal? HistoryWalletAmount { get; set; }

        /// <summary>
        /// 升级基金（新增正数 减少负数）
        /// </summary>
        public decimal? UpgradeFundAmount { get; set; }

        /// <summary>
        /// 普通待结算奖金（新增正数 减少负数）
        /// </summary>
        public decimal? GeneralBonus { get; set; }

        /// <summary>
        /// 区域待结算奖金（新增正数 减少负数）
        /// </summary>
        public decimal? AreaBonus { get; set; }

        /// <summary>
        /// 全国待结算奖金（新增正数 减少负数）
        /// </summary>
        public decimal? GlobalBonus { get; set; }
    }
}
