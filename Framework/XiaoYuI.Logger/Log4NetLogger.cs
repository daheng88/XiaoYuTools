using System;
using System.Collections.Generic;
using System.Text;

 
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config",Watch = true)]
namespace XiaoYuI.Logger
{

    public sealed class Log4NetLogger
    {

        /// <summary>
        /// 创建一个记录实体
        /// </summary>
        private static log4net.ILog _log = log4net.LogManager.GetLogger("Logger");
        private static Log4NetLogger Current = null;

        public static Log4NetLogger Instance
        {
            get
            {
                if (Current == null)
                {
                    Current = new Log4NetLogger();
                }
                return Current;
            }
        }

        /// <summary>
        /// 记录消息日志
        /// </summary>
        /// <param name="message"></param>
        public  void Info(string message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message"></param>
        public  void Warning(string message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message"></param>
        public  void Error(string message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// 记录指定的一个Exception的日志
        /// </summary>
        /// <param name="exception"></param>
        public  void Exception(Exception exception)
        {
            _log.Error(exception.Message, exception);
        }

        /// <summary>
        /// 记录指定的一个Exception的日志
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public  void Exception(string message, Exception exception)
        {
            _log.Error(message, exception);
        }


    }
}
