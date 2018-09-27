namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// 支付结果通知
    /// </summary>
    public class OrderNotifyResponse
    {
        /// <summary>
        /// 返回状态码 SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看 result_code来判断
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息，如非空为错误原因 1：签名失败 2：参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }

        #region 以下字段在return_code为SUCCESS的时候有返回

        /// <summary>
        /// 微信公众账号id
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        #endregion

        #region 以下字段在return_code和result_code都为SUCCESS的时候有返回

        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 现金券金额
        /// </summary>
        public string coupon_fee { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string time_end { get; set; }

        #endregion
    }
}
