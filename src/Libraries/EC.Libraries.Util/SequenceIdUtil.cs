using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 生成有序Id
    /// </summary>
    public class SequenceIdUtil
    {        
        private static long _lastTimestamp = -1L;
        private static int _sequence = 0;
        private static int MaxSequence = 99999;       
        private static readonly object _lock = new Object();       
        private static readonly DateTime Jan1st1970 = new DateTime  (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly string _path = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, @"App_Data\IdSequence.txt");

        private static string _workId ="1";
        private static string _datef = "yyMMddHH";
        private static int _w = 5;

        static SequenceIdUtil()
        {
            lock (_lock)
            {
                var format = ConfigurationManager.AppSettings["IdFormat"];
                if (!string.IsNullOrEmpty(format))
                {
                    var configs = format.Split(',');
                    if (configs.Length == 3)
                    {
                        _workId = configs[0];
                        _datef = configs[1];
                        _w = int.Parse(configs[2]);
                        MaxSequence =int.Parse("9".PadLeft(_w, '9'));
                    }
                }
                _lastTimestamp = TimeGen();
                _sequence = ReadSequence();
            }
        }

        /// <summary>
        /// 生成序列Id，可在AppSettings下添加IdFormat配置规则
        /// 默认规则（1,yyMMddHH,5）
        /// </summary>
        /// <returns></returns>
        public static long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                //时间错误不能往后调整
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(String.Format("Clock moved backwards.  Refusing to generate id for {0} ", _lastTimestamp - timestamp));
                }
                
                if (_lastTimestamp == timestamp)
                {
                    //当前时间内，则+1
                    _sequence = (_sequence + 1);
                    if (_sequence > MaxSequence)
                    {
                        //当前时刻内计数满了，则等待下一时刻
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }

                _lastTimestamp = timestamp;
                //ID偏移组合生成最终的ID，并返回ID 
                var id =long.Parse(_workId+DateTime.Now.ToString(_datef) +_sequence.ToString().PadLeft(_w,'0'));
                WriteSequence(_sequence);
                return id;
            } 
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        protected static long TimeGen()
        {
            if (_datef.StartsWith("yyMMdd") || _datef.StartsWith("yyyyMMdd"))
            {
                var timesapn = DateTime.UtcNow - Jan1st1970;
                if (_datef.EndsWith("HH"))
                    return (long)(timesapn.TotalHours);
                if (_datef.EndsWith("mm"))
                    return (long)(timesapn.TotalMinutes);
                if (_datef.EndsWith("ss"))
                    return (long)(timesapn.TotalSeconds);
            }
            throw new Exception("时间格式错误，支持yyMMdd[HH|mm|ss]");
        }

        /// <summary>
        /// 等待下一个毫秒的到来 
        /// </summary>
        /// <param name="lastTimestamp">最后一次使用时间戳</param>
        /// <returns></returns>
        protected static long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                Thread.Sleep(1);
                timestamp = TimeGen();
            }
            _sequence = 0;
            return timestamp;
        }

        static void WriteSequence(long id)
        {
            FileStream fs = null;
            if (!Directory.Exists(Path.GetDirectoryName(_path)))
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
            if (File.Exists(_path))
                fs = new FileStream(_path, FileMode.Open);
            else
                fs = new FileStream(_path, FileMode.CreateNew);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(id.ToString());
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        static int ReadSequence()
        {           
            if (!File.Exists(_path))
                return -1;
            StreamReader sr = new StreamReader(_path, Encoding.Default);
            string sId = sr.ReadLine();
            if (string.IsNullOrEmpty(sId))
            {
                sr.Close();
                throw new Exception(string.Format("{0} 文本的自增序列值为null", _path));
            }
            int sequence = int.Parse(sId.Trim());
            sr.Close();
            return sequence;
        }
    }
}
