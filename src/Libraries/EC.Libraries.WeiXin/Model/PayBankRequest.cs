using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 企业付款到银行卡请求
    /// </summary>
    public class PayBankRequest
    {

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }


        /// <summary>
        /// 商户企业付款单号(商户订单号，需保持唯一（只允许数字[0~9]或字母[A~Z]和[a~z]，最短8位，最长32位）)
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public int account_type { get; set; }
        
        /// <summary>
        /// 收款方银行卡号
        /// </summary>
        public string enc_bank_no { get; set; }

        /// <summary>
        /// 收款方用户名
        /// </summary>
        public string enc_true_name { get; set; }

        /// <summary>
        /// 收款方开户行
        /// </summary>
        public string bank_code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string bank_note { get; set; }
        

        /// <summary>
        /// 付款金额
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 付款说明
        /// </summary>
        public string desc { get; set; }

        #region 公共
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// apiKey
        /// </summary>
        public string apiKey { get; set; }


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
        #endregion
    }
}
