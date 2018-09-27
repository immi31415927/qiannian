namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class PublickeyRequest:WXResult
    {

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }

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
    }
}
