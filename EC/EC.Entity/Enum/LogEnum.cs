namespace EC.Entity.Enum
{
    /// <summary>
    /// 日志枚举
    /// </summary>
    public class LogEnum
    {
        /// <summary>
        /// 日志级别:10:Debug,20:Info,30:Warn,40:Error,50:Fata
        /// </summary>
        public enum Level
        {
            Debug = 10,
            Info = 20,
            Warn = 30,
            Error = 40,
            Fata = 50
        }

        /// <summary>
        /// 日志来源:前台(10),后台(20)
        /// </summary>
        public enum Source
        {
            前台 = 10,
            后台 = 20,
            App = 30,
        }
    }
}
