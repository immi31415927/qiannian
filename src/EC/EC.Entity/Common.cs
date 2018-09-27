using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity
{
    #region 等级
    /// <summary>
    /// 等级
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 升级层数
        /// </summary>
        public int Top { get; set; }
    }
    #endregion

    #region 代理销售
    /// <summary>
    /// 代理销售
    /// </summary>
    public class AgencySale
    {
        /// <summary>
        /// 会员等级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
    #endregion
}
