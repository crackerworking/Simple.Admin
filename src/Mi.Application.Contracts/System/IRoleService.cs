using Mi.Application.Contracts.System.Models.Role;

namespace Mi.Application.Contracts.System
{
    public interface IRoleService
    {
        Task<ResponseStructure> AddRoleAsync(RolePlus input);

        Task<ResponseStructure> RemoveRoleAsync(PrimaryKey input);

        Task<ResponseStructure<PagingModel<SysRoleFull>>> GetRoleListAsync(RoleSearch search);

        Task<ResponseStructure> UpdateRoleAsync(RoleEdit input);

        Task<ResponseStructure<SysRoleFull>> GetRoleAsync(long id);
    }
}