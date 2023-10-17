namespace Mi.Domain.Tasks
{
    public abstract class TaskSchedulerNodeBase
    {
        public abstract string Name { get; }
        public abstract string Cron { get; }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="extra"></param>
        /// <returns></returns>
        public abstract Task ExcuteAsync(string? extra);
    }
}