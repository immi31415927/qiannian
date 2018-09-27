using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.Core.WeiXin
{
    /// <summary>  
    ///AccessToken 的摘要说明  
    /// </summary>
    public class AccessToken
    {
        public AccessToken()
        {
        }
        string _access_token;
        string _expires_in;

        /// <summary>  
        /// 获取到的凭证   
        /// </summary>  
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>  
        /// 凭证有效时间，单位：秒  
        /// </summary>  
        public string expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }

    /// <summary>  
    /// Ticket 的摘要说明  
    /// </summary>  
    public class Ticket
    {
        public Ticket()
        {
            //  
            //TODO: 在此处添加构造函数逻辑  
            //  
        }

        // ReSharper disable once InconsistentNaming
        string _expire_seconds;

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
        public string expire_seconds
        {
            get { return _expire_seconds; }
            set { _expire_seconds = value; }
        }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }

    /// <summary>
    /// 返回参数
    /// </summary>
    public class WeiXinResult
    {
        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsErr { get; set; }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
