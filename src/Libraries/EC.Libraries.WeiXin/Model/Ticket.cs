namespace EC.Libraries.WeiXin
{
    /// <summary>  
    ///Ticket 的摘要说明  
    /// </summary>  
    public class Ticket
    {
        /// <summary>  
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。  
        /// </summary>  
        // ReSharper disable once InconsistentNaming
        public string ticket { get; set; }

        /// <summary>  
        /// 凭证有效时间，单位：秒  
        /// </summary>  
        // ReSharper disable once ConvertToAutoProperty
        // ReSharper disable once InconsistentNaming
        public string expire_seconds { get; set; }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }  
}
