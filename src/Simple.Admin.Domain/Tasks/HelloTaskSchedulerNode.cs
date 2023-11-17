namespace Simple.Admin.Domain.Tasks
{
    public class HelloTaskSchedulerNode : TaskSchedulerNodeBase
    {
        public override string Name => "hello";

        public override string Cron => "0/50 * * * * ?";

        public override Task ExcuteAsync(string? extra)
        {
            extra ??= "default";
            Console.WriteLine("{0}: {1} -- {2}", nameof(HelloTaskSchedulerNode), extra, DateTime.Now);
            return Task.CompletedTask;
        }
    }
}