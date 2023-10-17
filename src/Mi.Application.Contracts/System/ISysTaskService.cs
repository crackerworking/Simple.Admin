using Mi.Application.Contracts.System.Models.Tasks;
using Mi.Domain.Shared.Core;

namespace Mi.Application.Contracts.System
{
    public interface ISysTaskService : IScoped
    {
        Task<ResponseStructure<List<TaskItem>>> GetListAsync();

        Task<ResponseStructure> UpdateAsync(TaskEdit input);
    }
}