namespace Mi.Domain.Tasks
{
    public class HelloTaskSchedulerNode : TaskSchedulerNodeBase
    {
        public override string Name => "hello";

        public override string Cron => "0/1 * * * * ?";

        public override Task ExcuteAsync(string? extra)
        {
            extra ??= "default";
            Console.WriteLine("[{0,10}]--[{1,15}]", extra, DateTime.Now);
            Console.WriteLine("[{0,10}]--[{1,15}]", "hello-task", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}