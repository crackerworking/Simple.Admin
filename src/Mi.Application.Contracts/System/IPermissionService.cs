using Mi.Application.Contracts.System.Models.Result;
using Mi.Domain.Shared.Models.UI;

namespace Mi.Application.Contracts.System
{
    public interface IPermissionService
    {
        Task<ResponseStructure> SetUserRoleAsync(long userId, List<long> roleIds);

        Task<ResponseStructure<IList<UserRoleOption>>> GetUserRolesAsync(long userId);

        /// <summary>
        /// 获取当前用户可查看的侧边菜单
        /// </summary>
        /// <returns></returns>
        Task<List<PaMenuModel>> GetSiderMenuAsync();

        Task<ResponseStructure<IList<long>>> GetRoleFunctionIdsAsync(long id);

        Task<ResponseStructure> SetRoleFunctionsAsync(long id, IList<long> funcIds);

        Task<ResponseStructure> RegisterAsync(string userName, string password);

        Task<ResponseStructure> LoginAsync(Guid guid, string userName, string password, string verifyCode);

        Task LogoutAsync();

        Task<UserModel> QueryUserModelCacheAsync(string userData);
    }
}