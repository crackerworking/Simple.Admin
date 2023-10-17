using Mi.Domain.DataAccess;
using Mi.Domain.Entities.System;
using Mi.Domain.Shared;

using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Mi.Domain.Tasks
{
    public class TaskSchedulerNode<T> : IJob where T : TaskSchedulerNodeBase, new()
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var model = (T?)context.MergedJobDataMap[SystemJobScheduler.NODE_INSTANCE];
                var task = (SysTask?)context.MergedJobDataMap[SystemJobScheduler.SYS_TASK_INS];
                var extra = task?.ExtraParams;

                if (model != null)
                {
                    model.ExcuteAsync(extra);
                    // 执行时间
                    Task.Factory.StartNew(() =>
                    {
                        if (task == null) return;
                        Thread.Sleep(10000);
                        using var p = App.Provider.CreateScope();
                        var repo = p.ServiceProvider.GetRequiredService<IRepository<SysTask>>();
                        task.ModifiedOn = DateTime.Now;
                        task.ExecutedTime = task.ModifiedOn;
                        repo.UpdateAsync(task).ConfigureAwait(true).GetAwaiter();
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Task.CompletedTask;
        }
    }
}