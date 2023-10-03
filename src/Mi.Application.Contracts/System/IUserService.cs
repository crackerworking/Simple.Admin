using Mi.Application.Contracts.System.Models.Result;
using Mi.Application.Contracts.System.Models.User;

namespace Mi.Application.Contracts.System
{
    public interface IUserService
    {
        Task<ResponseStructure<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

        Task<ResponseStructure<string>> AddUserAsync(string userName);

        Task<ResponseStructure> RemoveUserAsync(long userId);

        Task<ResponseStructure<string>> UpdatePasswordAsync(long userId);

        Task<ResponseStructure<SysUserFull>> GetUserAsync(long userId);

        Task<ResponseStructure> PassedUserAsync(long id);

        Task<IList<SysRoleFull>> GetRolesAsync(long id);

        Task<ResponseStructure<UserBaseInfo>> GetUserBaseInfoAsync();

        Task<ResponseStructure> SetUserBaseInfoAsync(UserBaseInfo model);

        Task<ResponseStructure> SetPasswordAsync(string password);

        List<string?> GetAuthCode();
    }
}