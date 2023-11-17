using Simple.Admin.Application.Contracts.System.Models.Permission;
using Simple.Admin.Application.Contracts.System.Models.User;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Models.UI;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IPermissionService
    {
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> SetUserRoleAsync(SetUserRoleIn input);

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MessageModel<IList<UserRoleOption>>> GetUserRolesAsync(long userId);

        /// <summary>
        /// 获取当前用户可查看的侧边菜单
        /// </summary>
        /// <returns></returns>
        Task<List<PaMenuModel>> GetSiderMenuAsync();

        /// <summary>
        /// 获取角色功能
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel<IList<long>>> GetRoleFunctionIdsAsync(PrimaryKey input);

        /// <summary>
        /// 设置角色功能
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> SetRoleFunctionsAsync(SetRoleFunctionsIn input);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> RegisterAsync(RegisterIn input);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> LoginAsync(LoginIn input);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// 查询登录必须数据
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<UserModel> QueryUserModelCacheAsync(string userData);
    }
}