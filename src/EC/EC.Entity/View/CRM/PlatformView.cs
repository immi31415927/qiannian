using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.View.CRM
{
    /// <summary>
    /// 平台数据
    /// </summary>
    public class PlatformView
    {
        /// <summary>
        /// 注册数
        /// </summary>
        public int RegisterNum { get; set; }

        /// <summary>
        /// Vip会员数
        /// </summary>
        public int VipNum { get; set; }

        /// <summary>
        /// 普通代理数
        /// </summary>
        public int CommonAgentNum { get; set; }

        /// <summary>
        /// 区域代理数
        /// </summary>
        public int AreaAgentNum { get; set; }

        /// <summary>
        /// 全国代理数
        /// </summary>
        public int CountryAgentNum { get; set; }

        /// <summary>
        /// 股东数
        /// </summary>
        public int StockAgentNum { get; set; }

        /// <summary>
        /// 提现总金额
        /// </summary>
        public decimal WithdrawalsTotalAmount { get; set; }

        /// <summary>
        /// 充值总金额
        /// </summary>
        public decimal RechargeTotalAmount { get; set; }

        /// <summary>
        /// 股池资金
        /// </summary>
        public decimal StockTotalAmount { get; set; }

        /// <summary>
        /// 股权池数
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 股权单价
        /// </summary>
        public decimal StockPrice { get; set; }
    }
}
