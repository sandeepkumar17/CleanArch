using log4net;
using log4net.Core;
using System.Reflection;

namespace CleanArch.Logging
{
    /// <summary>
    /// Wrapper class represents logger implementation for Log4Net.
    /// </summary>
    public sealed class Logger : Application.Interfaces.ILogger
    {
        #region ===[ Private Members ]=============================================================

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private static readonly Lazy<Logger> _loggerInstance = new Lazy<Logger>(() => new Logger());

        private const string ExceptionName = "Exception";
        private const string InnerExceptionName = "Inner Exception";
        private const string ExceptionMessageWithoutInnerException = "{0}{1}: {2}Message: {3}{4}StackTrace: {5}.";
        private const string ExceptionMessageWithInnerException = "{0}{1}{2}";

        #endregion

        #region ===[ Properties ]==================================================================

        /// <summary>
        /// Gets the Logger instance.
        /// </summary>
        public static Logger Instance
        {
            get { return _loggerInstance.Value; }
        }

        #endregion

        #region ===[ ILogger Members ]=============================================================

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Debug level.
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(message);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info level.
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info Warning.
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(message);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            _logger.Error(message);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Debug level including the exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(object message, Exception exception)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(message, exception);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info level including the exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message, exception);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Warn level including the exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            if (_logger.IsWarnEnabled)
                _logger.Info(message, exception);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Error level including the exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Fatal level including the exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        /// <summary>
        /// Log an exception with the log4net.Core.Level.Error level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            _logger.Error(SerializeException(exception, ExceptionName));
        }

        /// <summary>
        /// Log an exception with the log4net.Core.Level.Fatal level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="exception"></param>
        public void Fatal(Exception exception)
        {
            _logger.Fatal(SerializeException(exception, ExceptionName));
        }

        #endregion

        #region ===[ Public Methods ]==============================================================

        /// <summary>
        /// Serialize Exception to get the complete message and stack trace.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string SerializeException(Exception exception)
        {
            return SerializeException(exception, string.Empty);
        }

        #endregion

        #region ===[ Private Methods ]=============================================================

        /// <summary>
        /// Serialize Exception to get the complete message and stack trace.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        private static string SerializeException(Exception ex, string exceptionMessage)
        {
            var mesgAndStackTrace = string.Format(ExceptionMessageWithoutInnerException, Environment.NewLine,
                exceptionMessage, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);

            if (ex.InnerException != null)
            {
                mesgAndStackTrace = string.Format(ExceptionMessageWithInnerException, mesgAndStackTrace,
                    Environment.NewLine,
                    SerializeException(ex.InnerException, InnerExceptionName));
            }

            return mesgAndStackTrace + Environment.NewLine;
        }

        #endregion
    }
}