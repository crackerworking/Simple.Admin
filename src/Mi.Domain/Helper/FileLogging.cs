using Mi.Domain.Extension;
using Mi.Domain.Shared;
using Mi.Domain.Shared.Options;

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
        /// 获取每天日志
        /// </summary>
        /// <param name="time">时间，只取日期部分</param>
        /// <returns>name:日志文件名，value:绝对路径</returns>
        public static List<Option> GetEveryDayLogs(DateTime time)
        {
            var path = Path.Combine(_BasePath, time.ToString());
            if (!Directory.Exists(path)) return new List<Option>();

            var list = new List<Option>();
            foreach (var full in Directory.GetFiles(path))
            {
                var file = new FileInfo(full);
                list.Add(new Option
                {
                    Name = file.Name,
                    Value = full
                });
            }
            return list;
        }

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
            var log = CreateLogger("exception");
            log.Error(ex, content, values);
        }

        /// <summary>
        /// 创建日志生成器
        /// </summary>
        /// <param name="name">日志文件名</param>
        /// <param name="rollingInterval">周期</param>
        /// <returns></returns>
        private Logger CreateLogger(string name, RollingInterval rollingInterval = RollingInterval.Day)
        {
            var dir = new List<string> { _BasePath };
            if (rollingInterval == RollingInterval.Day)
            {
                dir.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            if (rollingInterval == RollingInterval.Month)
            {
                dir.Add(DateTime.Now.ToString("yyyy-MM"));
            }

            var path = Path.Combine(dir.ToArray());
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var log = new LoggerConfiguration().WriteTo.File(Path.Combine(path, name + ".log"), rollingInterval: rollingInterval).CreateLogger();
            return log;
        }

        /// <summary>
        /// 日志基础目录
        /// </summary>
        private static string _BasePath
        {
            get
            {
                var path = App.Configuration["LogFilePath"];
                if (path.IsNull()) return Path.Combine(App.WebRoot, "logs");
                return path;
            }
        }
    }
}