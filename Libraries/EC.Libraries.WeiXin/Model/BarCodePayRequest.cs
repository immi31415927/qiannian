using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin
{
    public class BarCodePayRequest
    {

        /// <summary>
        /// 商户账号appid
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 公众账号apiKey
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 支付总金额(必填)
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 商家订单号(必填)
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 授权码(必填：扫描用户的付款二维码)
        /// </summary>
        public string auth_code { get; set; }


    }
}
