using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Serializable]
    public class BsLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 来源:前台(10),后台(20)
        /// </summary>
        [Description("来源:前台(10),后台(20)")]
        public int Source { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        [Description("日志级别:10:Debug,20:Info,30:Warn,40:Error,50:Fata")]
        public int Level { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string Message { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [Description("异常信息")]
        public string Exception { get; set; }
        /// <summary>
        /// 操作人IP
        /// </summary>
        [Description("操作人IP")]
        public string Ip { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
