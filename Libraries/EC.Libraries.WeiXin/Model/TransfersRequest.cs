using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin.Model
{
    //<xml> 
    //<mch_appid>wxe062425f740c30d8</mch_appid> 
    //<mchid>10000098</mchid> 
    //<nonce_str>3PG2J4ILTKCH16CQ2502SI8ZNMTM67VS</nonce_str> 
    //<partner_trade_no>100000982014120919616</partner_trade_no> 
    //<openid>ohO4Gt7wVPxIT1A9GjFaMYMiZY1s</openid> 
    //<check_name>FORCE_CHECK</check_name> 
    //<re_user_name>张三</re_user_name> 
    //<amount>100</amount> 
    //<desc>节日快乐!</desc> 
    //<spbill_create_ip>10.2.3.10</spbill_create_ip> 
    //<sign>C97BDBACF37622775366F38B629F45E3</sign> 
    //</xml>


    /// <summary>
    /// 企业打款参数（https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2）
    /// </summary>
    public class TransfersRequest
    {
        
        /// <summary>
        /// apiKey
        /// </summary>
        public string apiKey { get; set; }
        /// <summary>
        /// 商户账号appid
        /// </summary>
        public string mch_appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mchid { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 商户订单号(商户订单号，需保持唯一性(只能是字母或者数字，不能包含有符号))
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 校验用户姓名选项(NO_CHECK：不校验真实姓名、FORCE_CHECK：强校验真实姓名)
        /// </summary>
        public string check_name { get; set; }

        /// <summary>
        /// 金额(企业付款金额，单位为分)
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 企业付款描述信息
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 是否使用证书
        /// </summary>
        public bool isUseCert { get; set; }

        /// <summary>
        /// 证书
        /// </summary>
        public string cert { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
