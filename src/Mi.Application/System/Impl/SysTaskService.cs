using AutoMapper;

using Mi.Application.Contracts.System.Models.Tasks;
using Mi.Domain.Tasks;

using Quartz;
using Quartz.Impl.Triggers;

namespace Mi.Application.System.Impl
{
    internal class SysTaskService : ISysTaskService
    {
        private readonly IRepository<SysTask> _repo;
        private readonly IMapper _mapper;
        private readonly IScheduler _scheduler;

        public SysTaskService(IRepository<SysTask> repo, IMapper mapper, IScheduler scheduler)
        {
            _repo = repo;
            _mapper = mapper;
            _scheduler = scheduler;
        }

        public async Task<TaskItem> GetAsync(long id)
        {
            var raw = await _repo.GetAsync(x=>x.Id == id);
            var model = _mapper.Map<TaskItem>(raw);

            return model;
        }

        public async Task<ResponseStructure<List<TaskItem>>> GetListAsync()
        {
            var raw = await _repo.GetListAsync();
            var list = _mapper.Map<List<TaskItem>>(raw);

            return new ResponseStructure<List<TaskItem>>(list);
        }

        public async Task<ResponseStructure> UpdateAsync(TaskEdit input)
        {
            var model = await _repo.GetAsync(x => x.Id == input.Id);
            if (model == null) return Back.NonExist();

            model.Remark = input.Remark;
            model.Cron = input.Cron;
            model.IsEnabled = input.IsEnabled;
            model.ExtraParams = input.ExtraParams;

            var flag = await _repo.UpdateAsync(model) > 0;
            if (flag)
            {
                var job = await _scheduler.GetJobDetail(new JobKey(model.TaskName));
                if (job != null)
                {
                    if (model.IsEnabled == 0)
                    {
                        await _scheduler.PauseJob(job.Key);
                    }
                    else if (model.IsEnabled == 1)
                    {
                        var triggerKey = new TriggerKey(model.TaskName + "_trigger");
                        var trigger = await _scheduler.GetTrigger(triggerKey);
                        var ct = (CronTriggerImpl?)trigger;
                        if (ct != null)
                        {
                            job.JobDataMap[SystemTaskScheduler.SYS_TASK_INS] = model;
                            ct.CronExpressionString = model.Cron;
                            await _scheduler.UnscheduleJob(triggerKey);
                            await _scheduler.ScheduleJob(job, ct);
                        }
                    }
                }
            }

            return Back.SuccessOrFail(flag);
        }
    }
}