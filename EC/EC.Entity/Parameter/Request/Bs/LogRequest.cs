using System;
using System.ComponentModel;
using EC.Entity.Enum;

namespace EC.Entity.Parameter.Request.Bs
{
    /// <summary>
    /// 日志参数
    /// </summary>
    public class LogRequest
    {
        /// <summary>
        /// 来源:前台(10),后台(20)
        /// </summary>
        [Description("来源:前台(10),后台(20)")]
        public LogEnum.Source Source { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string Message { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [Description("异常信息")]
        public Exception Exception { get; set; }

    }
}
