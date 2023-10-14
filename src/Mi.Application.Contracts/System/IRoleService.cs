using Mi.Application.Contracts.System.Models.Role;

namespace Mi.Application.Contracts.System
{
    public interface IRoleService
    {
        Task<ResponseStructure> AddRoleAsync(string name, string? remark);

        Task<ResponseStructure> RemoveRoleAsync(long id);

        Task<ResponseStructure<PagingModel<SysRoleFull>>> GetRoleListAsync(RoleSearch search);

        Task<ResponseStructure> UpdateRoleAsync(long id, string name, string remark);

        Task<ResponseStructure<SysRoleFull>> GetRoleAsync(long id);
    }
}