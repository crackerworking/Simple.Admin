using Mi.Application.Contracts.System.Models.Role;
using Mi.Application.Contracts.System.Models.User;

namespace Mi.Application.Contracts.System
{
    public interface IUserService
    {
        Task<ResponseStructure<PagingModel<UserItem>>> GetUserListAsync(UserSearch search);

        Task<ResponseStructure<string>> AddUserAsync(UserPlus input);

        Task<ResponseStructure> RemoveUserAsync(PrimaryKey input);

        Task<ResponseStructure<string>> UpdatePasswordAsync(PrimaryKey input);

        Task<ResponseStructure<SysUserFull>> GetUserAsync(long userId);

        Task<ResponseStructure> PassedUserAsync(PrimaryKey input);

        Task<IList<SysRoleFull>> GetRolesAsync(long id);

        Task<ResponseStructure<UserBaseInfo>> GetUserBaseInfoAsync();

        Task<ResponseStructure> SetUserBaseInfoAsync(UserBaseInfo model);

        Task<ResponseStructure> SetPasswordAsync(SetPasswordIn input);

        List<string?> GetAuthCode();
    }
}