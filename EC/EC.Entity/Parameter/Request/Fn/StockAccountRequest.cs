using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Fn
{
    /// <summary>
    /// 股权账号输入参数
    /// </summary>
    public class StockAccountRequest : PageCondition
    {


        #region 扩展条件
         
        /// <summary>
        /// 会员名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 会员规则编号
        /// </summary>
        public string SerialNumber { get; set; }

        #endregion
    }
}
