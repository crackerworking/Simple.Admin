using Mi.Application.Contracts.System.Models.Role;
using Mi.Application.Contracts.System.Models.User;

namespace Mi.Application.Contracts.System
{
    public interface IUserService
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure<string>> AddUserAsync(UserPlus input);

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> RemoveUserAsync(PrimaryKey input);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns>一个随机密码</returns>
        Task<ResponseStructure<string>> UpdatePasswordAsync(PrimaryKey input);

        /// <summary>
        /// 单个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseStructure<SysUserFull>> GetUserAsync(long userId);

        /// <summary>
        /// 允许登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> PassedUserAsync(PrimaryKey input);

        /// <summary>
        /// 用户关联所有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IList<SysRoleFull>> GetRolesAsync(long id);

        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseStructure<UserBaseInfo>> GetUserBaseInfoAsync();

        /// <summary>
        /// 设置用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseStructure> SetUserBaseInfoAsync(UserBaseInfo model);

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> SetPasswordAsync(SetPasswordIn input);

        /// <summary>
        /// 当前登录用户所有权限编码
        /// </summary>
        /// <returns></returns>
        List<string?> GetAuthCode();
    }
}