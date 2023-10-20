using Mi.Domain.Extension;
using Mi.Domain.Shared;

using Serilog;
using Serilog.Core;

namespace Mi.Domain.Helper
{
    /// <summary>
    /// 文件日志记录器
    /// </summary>
    public class FileLogging
    {
        private static Lazy<FileLogging> _lazy => new Lazy<FileLogging>(() => new FileLogging());

        public static FileLogging Instance => _lazy.Value;

        /// <summary>
        /// 消息日志，日志级别 <![CDATA[Information]]>
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="values"></param>
        public void Write(string topic, string content, params object[] values)
        {
            var log = CreateLogger(topic);
            log.Information(content, values);
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="content">本次记录内容</param>
        /// <param name="values">日志内容替换</param>
        public void WriteException(Exception ex, string content, params object[] values)
        {
            var log = CreateLogger("exception", RollingInterval.Day);
            log.Error(ex, content, values);
        }

        /// <summary>
        /// 创建日志生成器
        /// </summary>
        /// <param name="name">日志文件名</param>
        /// <param name="rollingInterval">周期</param>
        /// <returns></returns>
        private Logger CreateLogger(string name, RollingInterval rollingInterval = RollingInterval.Infinite)
        {
            var path = App.Configuration["LogFilePath"];
            if (path.IsNull()) path = Path.Combine(App.WebRoot, "logs");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var log = new LoggerConfiguration().WriteTo.File(Path.Combine(path, name + ".log"), rollingInterval: rollingInterval).CreateLogger();
            return log;
        }
    }
}