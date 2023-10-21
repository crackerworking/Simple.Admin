using System.Text;

using AutoMapper;
using Mi.Application.Contracts.System.Models.Role;
using Mi.Application.Contracts.System.Models.User;
using Mi.Domain.Shared.Core;

namespace Mi.Application.System.Impl
{
    public class UserService : IUserService, IScoped
    {
        private readonly IDapperRepository _dapperRepo;
        private readonly ICurrentUser _miUser;
        private readonly IRepository<SysUser> _userRepo;
        private readonly IMapper _mapper;

        public UserService(IDapperRepository dapperRepository
            , ICurrentUser miUser, IRepository<SysUser> userRepo
            , IMapper mapper)
        {
            _dapperRepo = dapperRepository;
            _miUser = miUser;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<MessageModel<string>> AddUserAsync(UserPlus input)
        {
            if (!input.userName.RegexValidate(PatternConst.UserName)) return new MessageModel<string>(false, "用户名只支持大小写字母和数字，最短4位，最长12位", null);
            var count = await _dapperRepo.ExecuteScalarAsync<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = input.userName.ToLower() });
            if (count > 0) return new MessageModel<string>(false, "用户名已存在", null);

            var user = new SysUser
            {
                UserName = input.userName,
                Id = SnowflakeIdHelper.Next(),
                PasswordSalt = EncryptionHelper.GetPasswordSalt(),
                Avatar = StringHelper.DefaultAvatar(),
                NickName = input.userName,
                Signature = "请设置您的个性签名"
            };
            var password = StringHelper.GetRandomString(6);
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);

            var flag = await _userRepo.AddAsync(user);

            return new MessageModel<string>(flag > 0, password);
        }

        public List<string?> GetAuthCode()
        {
            return _miUser.AuthCodes!.ToList();
        }

        public async Task<IList<SysRoleFull>> GetRolesAsync(long id)
        {
            var sql = new StringBuilder("select r.* from SysRole r,SysRoleFunction rf,SysUserRole ur where r.Id=rf.RoleId ");
            sql.Append(" and ur.RoleId=r.Id and r.IsDeleted=0 and ur.UserId=@id group by r.Id");
            return await _dapperRepo.QueryAsync<SysRoleFull>(sql.ToString(), new { id });
        }

        public async Task<MessageModel<SysUserFull>> GetUserAsync(long userId)
        {
            var user = await _userRepo.GetAsync(x => x.Id == userId);
            var model = _mapper.Map<SysUserFull>(user);

            return new MessageModel<SysUserFull>(true, "查询成功", model);
        }

        public async Task<MessageModel<UserBaseInfo>> GetUserBaseInfoAsync()
        {
            var user = await _userRepo.GetAsync(x => x.Id == _miUser.UserId);
            if (user == null) return Back.NonExist().As<UserBaseInfo>();
            return new MessageModel<UserBaseInfo>(new UserBaseInfo
            {
                Avatar = user.Avatar,
                NickName = user.NickName,
                Sex = user.Sex,
                Signature = user.Signature,
                UserId = _miUser.UserId
            });
        }

        public async Task<MessageModel<PagingModel<UserItem>>> GetUserListAsync(UserSearch search)
        {
            var sql = @"select u.*,GROUP_CONCAT(r.RoleName) as RoleNameString from SysUser u 
                        left join SysUserRole ur on u.Id=ur.UserId
                        left join SysRole r on ur.RoleId=r.Id
                        where u.IsDeleted=0 ";
            if (!string.IsNullOrWhiteSpace(search.UserName))
            {
                sql += " and u.UserName like @UserName ";
            }
            sql += " GROUP BY u.Id ";
            var pageModel = await _dapperRepo.QueryPagedAsync<UserItem>(sql, search.Page, search.Size, "CreatedOn asc",
                new { UserName = "%" + search.UserName + "%" });

            foreach (var item in pageModel.Rows!)
            {
                if (!string.IsNullOrEmpty(item.RoleNameString))
                {
                    item.RoleNames = item.RoleNameString.Split(',');
                }
            }
            return new MessageModel<PagingModel<UserItem>>(true, "查询成功", pageModel);
        }

        public async Task<MessageModel> PassedUserAsync(PrimaryKey input)
        {
            var rows = await _userRepo.UpdateAsync(input.id, updatable => updatable
                .SetColumn(x => x.IsEnabled, 1)
                .SetColumn(x => x.ModifiedBy, _miUser.UserId)
                .SetColumn(x => x.ModifiedOn, DateTime.Now));

            return rows > 0 ? Back.Success() : Back.Fail();
        }

        public async Task<MessageModel> RemoveUserAsync(PrimaryKey input)
        {
            var flag = await _userRepo.UpdateAsync(input.id, updatable => updatable
                .SetColumn(x => x.IsDeleted, 1)
                .SetColumn(x => x.ModifiedBy, _miUser.UserId)
                .SetColumn(x => x.ModifiedOn, DateTime.Now));

            return Back.SuccessOrFail(flag > 0);
        }

        public async Task<MessageModel> SetPasswordAsync(SetPasswordIn input)
        {
            var user = await _userRepo.GetAsync(x => x.Id == _miUser.UserId);
            if (user == null) return Back.NonExist();
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(input.password, user.PasswordSalt);
            await _userRepo.UpdateAsync(user);

            return Back.Success("修改成功，下次登录时请使用新密码");
        }

        public async Task<MessageModel> SetUserBaseInfoAsync(UserBaseInfo model)
        {
            var user = await _userRepo.GetAsync(x => x.Id == _miUser.UserId);
            if (user == null) return Back.NonExist();
            user.Avatar = model.Avatar;
            user.NickName = model.NickName;
            user.Signature = model.Signature;
            user.Sex = model.Sex;
            user.ModifiedBy = _miUser.UserId;
            user.ModifiedOn = DateTime.Now;
            await _userRepo.UpdateAsync(user);
            return Back.Success();
        }

        public async Task<MessageModel<string>> UpdatePasswordAsync(PrimaryKey input)
        {
            var user = await _userRepo.GetAsync(x => x.Id == input.id);
            if (user == null || user.Id <= 0) return new MessageModel<string>(false, "用户不存在", "");

            var password = StringHelper.GetRandomString(6);
            var flag = await _userRepo.UpdateAsync(input.id, updatable => updatable
                .SetColumn(x => x.Password, EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt))
                .SetColumn(x => x.ModifiedBy, _miUser.UserId)
                .SetColumn(x => x.ModifiedOn, DateTime.Now));

            return new MessageModel<string>(flag > 0, password);
        }
    }
}