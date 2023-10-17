using Mi.Application.Contracts.System.Models.Tasks;
using Mi.Domain.Shared.Core;

namespace Mi.Application.Contracts.System
{
    public interface ISysTaskService : IScoped
    {
        /// <summary>
        /// 定时任务列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseStructure<List<TaskItem>>> GetListAsync();

        /// <summary>
        /// 更新定时任务配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> UpdateAsync(TaskEdit input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskItem> GetAsync(long id);
    }
}