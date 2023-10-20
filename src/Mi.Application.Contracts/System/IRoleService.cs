using Mi.Application.Contracts.System.Models.Role;

namespace Mi.Application.Contracts.System
{
    public interface IRoleService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> AddRoleAsync(RolePlus input);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> RemoveRoleAsync(PrimaryKey input);

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<SysRoleFull>>> GetRoleListAsync(RoleSearch search);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> UpdateRoleAsync(RoleEdit input);

        /// <summary>
        /// 单个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseStructure<SysRoleFull>> GetRoleAsync(long id);
    }
}