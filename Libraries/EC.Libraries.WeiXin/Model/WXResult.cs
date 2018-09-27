using System.Collections.Generic;

namespace EC.Libraries.WeiXin
{
    #region 基础结果返回

    /// <summary>
    /// 基础结果返回
    /// </summary>
    /// <remarks>苟治国 创建</remarks>
    public class WXResult
    {
        /// <summary>
        /// 返回状态码(SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断)
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息(返回信息，如非空，为错误原因 签名失败参数格式校验错误)
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
    }

    #endregion

    #region 数据结果返回

    /// <summary>
    /// 数据结果返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WXResult<T> : WXResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    #endregion

    #region 数据结果返回

    /// <summary>
    /// 数据列表结果返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WXResultList<T> : WXResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }
    }

    #endregion
}
