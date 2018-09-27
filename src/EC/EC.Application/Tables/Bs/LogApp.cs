using System;
using System.Threading.Tasks;
using EC.DataAccess.Bs;
using EC.Entity.Enum;
using EC.Entity.Tables.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Libraries.Util;

namespace EC.Application.Tables.Bs
{
    using EC.Libraries.Core;

    /// <summary>
    /// 日志业务层
    /// </summary>
    public class LogApp : Base<LogApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsLog Get(int sysNo)
        {
            var result = Using<IBsLog>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="request"></param>
        public void Info(LogRequest request)
        {
            WriteLog(request.Source, LogEnum.Level.Info, request.Message, request.Exception);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="request"></param>
        public void Warn(LogRequest request)
        {
            WriteLog(request.Source, LogEnum.Level.Warn, request.Message, request.Exception);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="request"></param>
        public void Error(LogRequest request)
        {
            WriteLog(request.Source, LogEnum.Level.Error, request.Message, request.Exception);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="request"></param>
        public void Fata(LogRequest request)
        {
            WriteLog(request.Source, LogEnum.Level.Error, request.Message, request.Exception);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="level">级别</param>
        /// <param name="message">内容</param>
        /// <param name="exception">异常</param>
        public void WriteLog(LogEnum.Source source, LogEnum.Level level, string message, Exception exception)
        {
            var log = new BsLog()
            {
                Source = source.GetHashCode(),
                Level = level.GetHashCode(),
                Message = message,
                Exception = exception == null ? "" : exception + (exception.StackTrace ?? ""),
                Ip = WebUtil.GetUserIp(),
                CreatedDate = DateTime.Now
            };

            Task.Factory.StartNew(() =>
            {
                try
                {
                    Using<IBsLog>().Insert(log);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

        }
    }
}
