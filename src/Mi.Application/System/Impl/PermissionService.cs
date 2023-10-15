using System.Data;
using System.Security.Claims;

using Mi.Application.Contracts.Public;
using Mi.Application.Contracts.System.Models.Function;
using Mi.Application.Contracts.System.Models.User;
using Mi.Domain.Entities.System.Enum;
using Mi.Domain.Services;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Mi.Application.System.Impl
{
    public class PermissionService : IPermissionService, IScoped
    {
        private readonly IRepository<SysUser> _userRepository;
        private readonly IRepository<SysRole> _roleRepository;
        private readonly IFunctionService _functionService;
        private readonly HttpContext _context;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUser _miUser;
        private readonly IUserService _userService;
        private readonly IPublicService _publicService;
        private readonly IRepository<SysUserRole> _userRoleRepo;
        private readonly IDapperRepository _dapperRepository;
        private readonly IRepository<SysRoleFunction> _roleFunctionRepo;
        private readonly ICaptcha _captcha;

        public PermissionService(IRepository<SysUser> userRepository
            , IRepository<SysRole> roleRepository
            , IFunctionService functionService
            , IHttpContextAccessor httpContextAccessor
            , IMemoryCache cache
            , ICurrentUser miUser
            , IUserService userService
            , IPublicService publicService
            , IRepository<SysUserRole> userRoleRepo
            , IDapperRepository dapperRepository
            , IRepository<SysRoleFunction> roleFunctionRepo
            , ICaptcha captcha)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _functionService = functionService;
            _context = httpContextAccessor.HttpContext!;
            _cache = cache;
            _miUser = miUser;
            _userService = userService;
            _publicService = publicService;
            _userRoleRepo = userRoleRepo;
            _dapperRepository = dapperRepository;
            _roleFunctionRepo = roleFunctionRepo;
            _captcha = captcha;
        }

        public async Task<List<PaMenuModel>> GetSiderMenuAsync()
        {
            var topLevels = (await _functionService.GetFunctionsCacheAsync())
                .Where(x => _miUser.FuncIds.Contains(x.Id) && x.ParentId <=0 && x.FunctionType == (int)EnumFunctionType.Menu).OrderBy(x => x.Sort);
            var list = new List<PaMenuModel>();
            foreach (var x in topLevels)
            {
                var children = (await GetPaChildrenAsync(x.Id)).ToList();
                var temp = new PaMenuModel(x.Id, 0, x.FunctionName, x.Url, x.Icon, children);
                list.Add(temp);
            }

            return list;
        }

        private async Task<IList<PaMenuModel>> GetPaChildrenAsync(long id)
        {
            var children = (await _functionService.GetFunctionsCacheAsync()).Where(x => _miUser.FuncIds.Contains(x.Id) && x.ParentId > 0 && x.FunctionType == (int)EnumFunctionType.Menu && x.ParentId == id).OrderBy(x => x.Sort);
            var list = new List<PaMenuModel>();
            foreach (var x in children)
            {
                var temp = (await GetPaChildrenAsync(x.Id)).ToList();
                var type = await GetMenuTypeAsync(x.Children);
                var menu = new PaMenuModel(x.Id, type, x.FunctionName, x.Url, x.Icon, temp);
                list.Add(menu);
            }
            return list;
        }

        /// <summary>
        /// 获取菜单类型 0目录 1菜单
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private async Task<int> GetMenuTypeAsync(string? children)
        {
            if (string.IsNullOrEmpty(children)) return 1;
            var arr = children.Split(',');
            if (arr == null) return 1;
            var flag = (await _functionService.GetFunctionsCacheAsync()).Any(x => arr.Contains(x.Id.ToString()) && x.FunctionType == (int)EnumFunctionType.Menu);
            return flag ? 0 : 1;
        }

        public async Task<ResponseStructure<IList<UserRoleOption>>> GetUserRolesAsync(long userId)
        {
            var roles = await _roleRepository.GetListAsync();
            var userRoles = await _userRoleRepo.GetListAsync(x => x.UserId == userId);
            var list = roles.Select(x => new UserRoleOption
            {
                Name = x.RoleName,
                Value = x.Id.ToString(),
                Checked = userRoles.Any(m => m.RoleId == x.Id),
                Remark = x.Remark
            }).ToList();

            return new ResponseStructure<IList<UserRoleOption>>(true, list);
        }

        public async Task<ResponseStructure> SetUserRoleAsync(long userId, List<long> roleIds)
        {
            var user = await _userRepository.GetAsync(x => x.Id == userId);
            if (user == null) return Back.Fail("用户不存在");

            await _dapperRepository.ExecuteAsync("delete from SysUserRole where UserId=@id", new { id = userId });
            var list = new List<SysUserRole>();
            foreach (var roleId in roleIds)
            {
                list.Add(new SysUserRole
                {
                    Id = SnowflakeIdHelper.Next(),
                    UserId = userId,
                    RoleId = roleId
                });
            }
            await _userRoleRepo.AddRangeAsync(list);

            return Back.Success();
        }

        public async Task<ResponseStructure> RegisterAsync(string userName, string password)
        {
            if (!userName.RegexValidate(PatternConst.UserName)) return Back.Fail("用户名只支持大小写字母和数字，最短4位，最长12位");
            var count = await _dapperRepository.ExecuteScalarAsync<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = userName.ToLower() });
            if (count > 0) return Back.Fail("用户名已存在");

            var user = new SysUser();
            user.UserName = userName;
            user.NickName = userName;
            user.Signature = "个性签名";
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            user.Avatar = StringHelper.DefaultAvatar();
            await _userRepository.AddAsync(user);

            var result = Back.Success("注册成功，请等待管理员审核！");
            var ids = await _dapperRepository.QueryAsync<long>(@"select ur.UserId from SysFunction s
                inner join SysRoleFunction sr on s.id=sr.FunctionId
                inner join SysUserRole ur on sr.RoleId=ur.RoleId
                where s.IsDeleted=0 and s.AuthorizationCode = 'System:User:Passed'");
            MessageFactory.Instance.WriteMessage("审核", $"系统有新用户('{userName}')注册，需要您及时审核！", ids);
            return result;
        }

        public async Task<ResponseStructure> LoginAsync(Guid guid, string userName, string password, string verifyCode)
        {
            var validateFlag = await _captcha.ValidateAsync(guid.ToString(), verifyCode);
            if (!validateFlag) return Back.Fail("验证码错误");

            var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == userName.ToLower());
            if (user == null) return Back.Fail("用户名不存在");
            if (user.IsEnabled == 0) return Back.Fail("没有登录权限，请联系管理员");

            var flag = user.Password == EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            if (!flag) return Back.Fail("用户名或密码错误");

            var roleNameArray = (await _userService.GetRolesAsync(user.Id)).Select(x => x.RoleName).ToList();
            if (user.IsSuperAdmin == 1)
            {
                roleNameArray.Add(AuthorizationConst.SUPER_ADMIN);
            }
            var roleNames = string.Join(",", roleNameArray);
            var claims = new Claim[]
            {
                new (ClaimTypes.Name,user.UserName),
                new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                new (ClaimTypes.Role,roleNames),
                new (ClaimTypes.UserData,StringHelper.UserDataString(user.Id,user.UserName,roleNames))
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            return Back.Success("登录成功");
        }

        public async Task<UserModel> QueryUserModelCacheAsync(string userData)
        {
            var arr = StringHelper.GetUserData(userData);
            var key = StringHelper.UserKey(arr.Item2, arr.Item3);
            var cacheData = _cache.Get<UserModel>(key);
            if (cacheData != null) return cacheData;
            var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == arr.Item2.ToLower());
            if (user != null)
            {
                var userModel = new UserModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSuperAdmin = user.IsSuperAdmin == 1
                };

                var exp = PredicateBuilder.Instance.Create<SysFunctionFull>();
                if (userModel.IsSuperAdmin)
                {
                    userModel.Roles = AuthorizationConst.SUPER_ADMIN;
                    exp = x => true;
                }
                else
                {
                    var roleIds = (await _userRoleRepo.GetListAsync(x => x.UserId == user.Id)).Select(x => x.RoleId).ToList();
                    var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
                    userModel.Roles = string.Join(",", roles.Select(x => x.RoleName));
                    var funcIds = (await _roleFunctionRepo.GetListAsync(x => roleIds.Contains(x.RoleId))).Select(x => x.FunctionId).ToList();
                    exp = x => funcIds.Contains(x.Id);
                }
                userModel.PowerItems = (await _functionService.GetFunctionsCacheAsync()).Where(exp.Compile()).Select(x => new PowerItem
                {
                    Id = x.Id,
                    Url = x.Url,
                    AuthCode = x.AuthorizationCode
                }).ToList();
                _cache.Set(key, userModel, CacheConst.Week);

                return userModel;
            }

            return new UserModel();
        }

        public async Task<ResponseStructure> SetRoleFunctionsAsync(long id, IList<long> funcIds)
        {
            var role = _roleRepository.GetAsync(x => x.Id == id);
            if (role == null || role.Id <= 0) return Back.Fail("角色不存在");

            await _dapperRepository.ExecuteAsync("delete from SysRoleFunction where RoleId=@id", new { id });

            var powers = new List<SysRoleFunction>();
            foreach (var item in funcIds)
            {
                var temp = new SysRoleFunction();
                temp.RoleId = id;
                temp.FunctionId = item;
                powers.Add(temp);
            }

            if (powers.Count > 0) await _roleFunctionRepo.AddRangeAsync(powers);
            //正则移除所有角色功能缓存
            _cache.RemoveByPattern(StringHelper.UserFunctionCachePattern());

            return Back.Success();
        }

        public async Task LogoutAsync()
        {
            await _context.SignOutAsync();
        }

        public async Task<ResponseStructure<IList<long>>> GetRoleFunctionIdsAsync(long id)
        {
            var ids = (await _roleFunctionRepo.GetListAsync(x => x.RoleId == id)).Select(x => x.FunctionId).ToList();

            return new ResponseStructure<IList<long>>(ids);
        }
    }
}