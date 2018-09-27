using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Enum
{
    /// <summary>
    /// 财务
    /// </summary>
    public class FnEnum
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public enum PayType
        {
            微信支付 = 10,
            奖金金额 = 20,
        }

        /// <summary>
        /// 提现申请方式
        /// </summary>
        public enum CashoutType
        {
            银联 = 10,
            微信 = 20,
        }

        /// <summary>
        /// 提现申请状态
        /// </summary>
        public enum CashoutStatus
        {
            驳回 = 0,
            处理中 = 10,
            完成 = 20
        }

        /// <summary>
        /// 奖金记录类型
        /// </summary>
        public enum BonusLogType
        {
            代理奖励 = 10,
            股东奖励 = 20,
            股东见点奖励 = 30,
            股东升级推荐奖励 = 40,
        }

        /// <summary>
        /// 奖金状态
        /// </summary>
        public enum BonusLogStatus
        {
            撤销 = 10,
            已撤销 = 20
        }

        /// <summary>
        /// 充值类型
        /// </summary>
        public enum RechargeType
        {
            充值 = 10,
            续费 = 20,
            升级 = 30,
        }

        /// <summary>
        /// 续费方式
        /// </summary>
        public enum 续费方式
        {
            在线支付 = 10,
            奖金余额 = 20,
        }

        /// <summary>
        /// 是否撤销
        /// </summary>
        public enum 是否撤销
        {
            是 = 1,
            否 = 0,
        }
    }
}
