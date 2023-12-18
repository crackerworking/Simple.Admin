using Simple.Admin.Application.Contracts.System.Models.Role;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IRoleService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> AddRoleAsync(RolePlus input);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> RemoveRoleAsync(PrimaryKey input);

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<MessageModel<PagingModel<SysRoleFull>>> GetRoleListAsync(RoleSearch search);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> UpdateRoleAsync(RoleEdit input);

        /// <summary>
        /// 单个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageModel<SysRoleFull>> GetRoleAsync(long id);
    }
}