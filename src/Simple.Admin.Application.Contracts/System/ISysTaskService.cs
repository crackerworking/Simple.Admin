using Simple.Admin.Application.Contracts.System.Models.Tasks;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.Contracts.System
{
    public interface ISysTaskService : IScoped
    {
        /// <summary>
        /// 定时任务列表
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<List<TaskItem>>> GetListAsync();

        /// <summary>
        /// 更新定时任务配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> UpdateAsync(TaskEdit input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskItem> GetAsync(long id);
    }
}