namespace Mi.Domain.Tasks
{
    public class HelloTaskSchedulerNode : TaskSchedulerNodeBase
    {
        public override string Name => "hello";

        public override string Cron => "0/1 * * * * ?";

        public override Task ExcuteAsync(string? extra)
        {
            extra ??= "default";
            Console.WriteLine(extra);
            Console.WriteLine("hello-task");
            return Task.CompletedTask;
        }
    }
}