using Simple.Admin.Application.Contracts.System.Models.Role;
using Simple.Admin.Application.Contracts.System.Models.User;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IUserService
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel<string>> AddUserAsync(UserPlus input);

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> RemoveUserAsync(PrimaryKey input);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns>一个随机密码</returns>
        Task<MessageModel<string>> UpdatePasswordAsync(PrimaryKey input);

        /// <summary>
        /// 单个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MessageModel<SysUserFull>> GetUserAsync(long userId);

        /// <summary>
        /// 允许登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> PassedUserAsync(PrimaryKey input);

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
        Task<MessageModel<UserBaseInfo>> GetUserBaseInfoAsync();

        /// <summary>
        /// 设置用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<MessageModel> SetUserBaseInfoAsync(UserBaseInfo model);

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> SetPasswordAsync(SetPasswordIn input);

        /// <summary>
        /// 当前登录用户所有权限编码
        /// </summary>
        /// <returns></returns>
        List<string?> GetAuthCode();
    }
}