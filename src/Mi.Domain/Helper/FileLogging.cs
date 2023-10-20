using System.Text;

using Mi.Domain.Extension;
using Mi.Domain.Shared;
using Mi.Domain.Shared.Options;

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
            var path = Path.Combine(_BasePath, time.ToString("yyyy-MM-dd"));
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
        public void Write(string topic, string content)
        {
            Save(topic, content);
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="content">本次记录内容</param>
        /// <param name="values">日志内容替换</param>
        public void WriteException(Exception ex, string content)
        {
            Save("exception", content, ex);
        }

        private void Save(string name, string content, Exception? exception = default)
        {
            lock (this)
            {
                var path = Path.Combine(_BasePath, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var fileName = Path.Combine(path, name + ".log");

                var sb = new StringBuilder();
                sb.AppendFormat("[{0}]: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                sb.AppendLine(content);
                if (exception != null)
                {
                    sb.AppendLine("<!-- exception -->");
                    sb.AppendLine(exception.Message);
                    sb.AppendLine("<!-- exception track -->");
                    sb.AppendLine(exception.StackTrace);
                }
                File.AppendAllText(fileName, sb.ToString());
            }
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