namespace EC.Libraries.WeiXin.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PublickeyResponse : WXResult
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string pub_key { get; set; }
    }
}
