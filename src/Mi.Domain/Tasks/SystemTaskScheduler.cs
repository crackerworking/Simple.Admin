using System.Collections.Concurrent;
using System.Reflection;

using Mi.Domain.DataAccess;
using Mi.Domain.Entities.System;
using Mi.Domain.Shared;

using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Mi.Domain.Tasks
{
    public class SystemTaskScheduler
    {
        private static Lazy<SystemTaskScheduler> _lazy => new Lazy<SystemTaskScheduler>(() => new SystemTaskScheduler());

        public static SystemTaskScheduler Instance => _lazy.Value;

        public const string NODE_INSTANCE = nameof(NODE_INSTANCE);
        public const string SYS_TASK_INS = nameof(SYS_TASK_INS);

        private readonly ConcurrentDictionary<string, TaskSchedulerNodeBase> _keyValuePairs = new ConcurrentDictionary<string, TaskSchedulerNodeBase>();

        /// <summary>
        /// 所有任务，包含已禁用的
        /// </summary>
        private List<SysTask> _tasks = new List<SysTask>();

        public void Run()
        {
            Task.Factory.StartNew(StartAsync, TaskCreationOptions.LongRunning);
        }

        private async Task StartAsync()
        {
            await ReadTasksAsync();

            // 1. Create a scheduler Factory ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            // 2. Get and start a scheduler
            IScheduler scheduler = App.Provider.GetRequiredService<IScheduler>();
            await scheduler.Start();
            // 3. Create a job
            var types = GetAllTaskSchedulerNodeBaseTypes();
            var nodeType = typeof(TaskSchedulerNode<>);
            foreach (var type in types)
            {
                var ins = (TaskSchedulerNodeBase?)Activator.CreateInstance(type);
                if (ins == null) continue;

                if (_keyValuePairs.ContainsKey(ins.Name)) throw new Exception(ins.Name + " is existed.");
                _keyValuePairs.TryAdd(ins.Name, ins);

                var obj = _tasks.FirstOrDefault(x => x.TaskName == ins.Name);
                if (obj != null && obj.IsEnabled == 0)
                {
                    continue;
                }

                var jobType = nodeType.MakeGenericType(type);
                IJobDetail job = JobBuilder.Create(jobType).WithIdentity(ins.Name).SetJobData(new JobDataMap
                {
                    { NODE_INSTANCE , ins },
                    { SYS_TASK_INS , obj! },
                }).Build();
                // 4. Create a trigger
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(ins!.Name + "_trigger").WithCronSchedule(obj?.Cron ?? ins.Cron).Build();
                // 5. Schedule the job using the job and trigger
                await scheduler.ScheduleJob(job, trigger);
            }

            await StorageAsync();
        }

        private async Task ReadTasksAsync()
        {
            using var p = App.Provider.CreateScope();
            var repo = p.ServiceProvider.GetRequiredService<IRepository<SysTask>>();
            _tasks = await repo.GetListAsync(x => x.IsDeleted == 0);
        }

        /// <summary>
        /// 定时任务同步到库
        /// </summary>
        private async Task StorageAsync()
        {
            using var p = App.Provider.CreateScope();
            var repo = p.ServiceProvider.GetRequiredService<IRepository<SysTask>>();
            var l1 = new List<SysTask>();
            foreach (var item in _keyValuePairs)
            {
                var obj = _tasks.FirstOrDefault(x => x.TaskName == item.Value.Name);
                if (obj == null)
                {
                    l1.Add(new SysTask
                    {
                        TaskName = item.Key,
                        Cron = item.Value.Cron,
                        IsEnabled = 1
                    });
                }
            }
            await repo.AddRangeAsync(l1);
        }

        private List<TypeInfo> GetAllTaskSchedulerNodeBaseTypes()
        {
            var assemblies = new Assembly[] { Assembly.Load("Mi.Domain"), Assembly.Load("Mi.Application") };
            var type = typeof(TaskSchedulerNodeBase);
            var list = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(type)).ToList();
                list.AddRange(types);
            }
            return list;
        }
    }
}