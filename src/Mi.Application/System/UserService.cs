using System.Text;
using Mi.Core.API;
using Mi.Core.GlobalVar;
using Mi.Core.Helper;
using Mi.Core.Service;
using Mi.Repository.BASE;

using Microsoft.AspNetCore.Http;

namespace Mi.Application.System
{
    public class UserService : IUserService, IScoped
    {
        private readonly IUserRepository _userRepository;
        private readonly ResponseStructure _message;
        private readonly IMiUser _miUser;
        private readonly Repository<SysRole> testRepo;

        public UserService(IUserRepository userRepository, ResponseStructure message
            , IMiUser miUser, Repository<SysRole> testRepo)
        {
            _userRepository = userRepository;
            _message = message;
            _miUser = miUser;
            this.testRepo = testRepo;
        }

        public async Task<ResponseStructure<string>> AddUserAsync(string userName)
        {
            if (!userName.RegexValidate(PatternConst.UserName)) return new ResponseStructure<string>(false, "用户名只支持大小写字母和数字，最短4位，最长12位", null);
            var count = _userRepository.ExecuteScalar<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = userName.ToLower() });
            if (count > 0) return new ResponseStructure<string>(false, "用户名已存在", null);

            var user = new SysUser
            {
                UserName = userName,
                Id = IdHelper.SnowflakeId(),
                PasswordSalt = EncryptionHelper.GetPasswordSalt(),
                Avatar = StringHelper.DefaultAvatar(),
                NickName = userName,
                Signature = "请设置您的个性签名"
            };
            var password = StringHelper.GetRandomString(6);
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);

            var flag = await _userRepository.AddAsync(user);

            return new ResponseStructure<string>(flag, flag ? "操作成功" : "操作失败", password);
        }

        public async Task<IList<SysRole>> GetRolesAsync(long id)
        {
            var roleRepo = DotNetService.Get<Repository<SysRole>>();
            var sql = new StringBuilder("select r.* from SysRole r,SysRoleFunction rf,SysUserRole ur where r.Id=rf.RoleId ");
            sql.Append(" and ur.RoleId=r.Id and r.IsDeleted=0 and ur.UserId=@id group by r.Id");
            return await roleRepo.GetListAsync(sql.ToString(), new { id });
        }

        public async Task<ResponseStructure<SysUser>> GetUserAsync(long userId)
        {
            return new ResponseStructure<SysUser>(true, "查询成功", await _userRepository.GetAsync(userId));
        }

        public async Task<ResponseStructure<UserBaseInfo>> GetUserBaseInfoAsync()
        {
            var user = await _userRepository.GetAsync(_miUser.UserId);
            return new ResponseStructure<UserBaseInfo>(new UserBaseInfo
            {
                Avatar = user.Avatar,
                NickName = user.NickName,
                Sex = user.Sex,
                Signature = user.Signature,
                UserId = _miUser.UserId
            });
        }

        public async Task<ResponseStructure<PagingModel<UserItem>>> GetUserListAsync(UserSearch search)
        {
            var pageModel = await _userRepository.QueryListAsync(search, search.UserName);

            foreach (var item in pageModel.Rows!)
            {
                if (!string.IsNullOrEmpty(item.RoleNameString))
                {
                    item.RoleNames = item.RoleNameString.Split(',');
                }
            }
            return new ResponseStructure<PagingModel<UserItem>>(true, "查询成功", pageModel);
        }

        public async Task<ResponseStructure> PassedUserAsync(long id)
        {
            var flag = await _userRepository.UpdateAsync(id, node => node
                .Set(x => x.IsEnabled, 1)
                .Set(x => x.ModifiedBy, _miUser.UserId)
                .Set(x => x.ModifiedOn, TimeHelper.LocalTime()));

            return flag ? _message.Success() : _message.Fail();
        }

        public async Task<ResponseStructure> RemoveUserAsync(long userId)
        {
            var flag = await _userRepository.UpdateAsync(userId, node => node
                .Set(x => x.IsDeleted, 1)
                .Set(x => x.ModifiedBy, _miUser.UserId)
                .Set(x => x.ModifiedOn, TimeHelper.LocalTime()));

            return flag ? _message.Success() : _message.Fail();
        }

        public async Task<ResponseStructure> SetPasswordAsync(string password)
        {
            var user = await _userRepository.GetAsync(_miUser.UserId);
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            await _userRepository.UpdateAsync(user);
            var context = DotNetService.Get<IHttpContextAccessor>().HttpContext;
            //await context.SignOutAsync();

            return _message.Success("修改成功，下次登录时请使用新密码");
        }

        public async Task<ResponseStructure> SetUserBaseInfoAsync(UserBaseInfo model)
        {
            var user = await _userRepository.GetAsync(_miUser.UserId);
            user.Avatar = model.Avatar;
            user.NickName = model.NickName;
            user.Signature = model.Signature;
            user.Sex = model.Sex;
            user.ModifiedBy = _miUser.UserId;
            user.ModifiedOn = TimeHelper.LocalTime();
            await _userRepository.UpdateAsync(user);
            return _message.Success();
        }

        public async Task<ResponseStructure<string>> UpdatePasswordAsync(long userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user.Id <= 0) return new ResponseStructure<string>(false, "用户不存在", "");

            var password = StringHelper.GetRandomString(6);
            var flag = await _userRepository.UpdateAsync(userId, node => node
                .Set(x => x.Password, EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt))
                .Set(x => x.ModifiedBy, _miUser.UserId)
                .Set(x => x.ModifiedOn, TimeHelper.LocalTime()));

            return new ResponseStructure<string>(flag, flag ? "操作成功" : "操作失败", password);
        }
    }
}