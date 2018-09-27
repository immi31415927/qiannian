using System;
using System.Collections.Concurrent;
using System.Web;
using log4net;
using log4net.Appender;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Error,
        Debug,
        Info
    }

    /// <summary>
    /// 封装log4net组件，根据日志类型写不同的文件
    /// 文件大于2M自动分文件
    /// </summary>
    /// <remarks>2015-12-8 苟治国 创建</remarks>
    public class LogUtil
    {

        static LogUtil()
        {
            FileRootPath = HttpRuntime.AppDomainAppPath + @"\Log\";
        }

        /// <summary>
        /// 日志根目录
        /// </summary>
        public static string FileRootPath
        {
            get;
            set;
        }

        //将日记对象缓存起来
        private static ConcurrentDictionary<string, ILog> LogDic = new ConcurrentDictionary<string, ILog>();

        private static ILog GetLog(string name)
        {
            try
            {
                ILog ilog = null;
                if (LogDic.TryGetValue(name, out ilog))
                {
                    return ilog;
                }
                else
                {
                    return CreatLogger(name);
                }
            }
            catch
            {
                return LogManager.GetLogger("Default");
            }
        }

        /// <summary>
        /// 创建日志Ilog
        /// </summary>
        /// <param name="LogPath"></param>
        /// <returns></returns>
        private static ILog CreatLogger(string LogPath)
        {
            var appender = new log4net.Appender.RollingFileAppender();
            appender.AppendToFile = true;
            appender.File = LogPath;
            //  appender.ImmediateFlush = true;
            // appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender.MaxSizeRollBackups = 500;
            appender.MaximumFileSize = "2MB";
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.StaticLogFileName = true;

            var layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level - %message%newline");
            layout.Header = "------ New session ------" + Environment.NewLine;
            layout.Footer = "------ End session ------" + Environment.NewLine;

            appender.Layout = layout;
            appender.ActivateOptions();

            var repository = log4net.LogManager.CreateRepository("MyRepository" + LogPath);
            log4net.Config.BasicConfigurator.Configure(repository, appender);
            ILog logger = log4net.LogManager.GetLogger(repository.Name, LogPath);
            LogDic.TryAdd(LogPath, logger);
            return logger;

        }

        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="content">信息</param>
        /// <remarks>2015-10-28 苟治国 添加</remarks>
        public static void WriteLog(LogType type, string content)
        {
            string logPath = FileRootPath + DateTime.Now.ToString("yyyyMMdd") + "\\" + type.ToString() + ".txt";
            ILog ilog = GetLog(logPath);
            ilog.Info(content);

        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="content">信息</param>
        /// <param name="exception">异常信息</param>
        /// <remarks>2015-12-8 苟治国 创建</remarks>
        public static void WriteErrLog(string content, Exception exception)
        {
            string logPath = FileRootPath + DateTime.Now.ToString("yyyyMMdd") + "\\" + LogType.Error.ToString() + ".txt";
            ILog ilog = GetLog(logPath);
            ilog.Error(content, exception);
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="content">信息</param>
        /// <param name="exception">异常信息</param>
        /// <param name="folder">文件夹</param>
        /// <remarks>2015-12-18 苟治国 创建</remarks>
        public static void WriteErrLog(string content, Exception exception, string folder)
        {
            string logPath = FileRootPath + folder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + LogType.Error.ToString() + ".txt";
            ILog ilog = GetLog(logPath);
            ilog.Error(content, exception);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">信息</param>
        /// <param name="folder">文件夹</param>
        /// <remarks>2015-12-18 苟治国 创建</remarks>
        public static void WriteInfoLog(string content, string folder)
        {
            string logPath = FileRootPath + folder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + LogType.Info.ToString() + ".txt";
            ILog ilog = GetLog(logPath);
            ilog.Info(content, null);
        }
    }
}
