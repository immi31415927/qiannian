namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// AccessToken
    /// </summary>
    public class AccessTokenResponse:WXResult
    {

        /// <summary>
        /// 错误码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
    }
}
