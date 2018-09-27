using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 刷卡支付
    /// </summary>
    public class BarCodePayResponse : WXResult
    {

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public int total_fee { get; set; }
    }
}
