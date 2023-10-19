using Mi.Application.Contracts.System.Models.Permission;
using Mi.Application.Contracts.System.Models.User;
using Mi.Domain.Shared.Models.UI;

namespace Mi.Application.Contracts.System
{
    public interface IPermissionService
    {
        Task<ResponseStructure> SetUserRoleAsync(SetUserRoleIn input);

        Task<ResponseStructure<IList<UserRoleOption>>> GetUserRolesAsync(long userId);

        /// <summary>
        /// 获取当前用户可查看的侧边菜单
        /// </summary>
        /// <returns></returns>
        Task<List<PaMenuModel>> GetSiderMenuAsync();

        Task<ResponseStructure<IList<long>>> GetRoleFunctionIdsAsync(PrimaryKey input);

        Task<ResponseStructure> SetRoleFunctionsAsync(SetRoleFunctionsIn input);

        Task<ResponseStructure> RegisterAsync(RegisterIn input);

        Task<ResponseStructure> LoginAsync(LoginIn input);

        Task LogoutAsync();

        Task<UserModel> QueryUserModelCacheAsync(string userData);
    }
}