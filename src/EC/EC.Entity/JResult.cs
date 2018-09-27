namespace EC.Entity
{
    #region 基础结果返回

    /// <summary>
    /// 基础结果返回
    /// </summary>
    public class JResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 状态代码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; }
    }

    #endregion

    #region 数据结果返回

    /// <summary>
    /// 数据结果返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JResult<T> : JResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    #endregion

    #region 分页结果返回

    /// <summary>
    /// 分页结果返回
    /// </summary>
    /// <typeparam name="T">分页数据类型</typeparam>
    public class PagerResult<T> : JResult<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }
    }

    #endregion
}
