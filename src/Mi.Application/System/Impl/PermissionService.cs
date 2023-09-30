using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

using Mi.Core.API;
using Mi.Core.Factory;
using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.Entity.System.Enum;
using Mi.IRepository.BASE;
using Mi.IService.Public;
using Mi.IService.System.Models.Result;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Mi.Application.System.Impl
{
    public class PermissionService : IPermissionService, IScoped
    {
        private readonly ResponseStructure _message;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IFunctionService _functionService;
        private readonly CreatorFactory _creatorFactory;
        private readonly CaptchaFactory _captchaFactory;
        private readonly HttpContext _context;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUser _miUser;
        private readonly IUserService _userService;
        private readonly IPublicService _publicService;

        public PermissionService(ResponseStructure message
            , IPermissionRepository permissionRepository
            , IUserRepository userRepository
            , IRoleRepository roleRepository
            , IFunctionService functionService
            , CreatorFactory creatorFactory
            , CaptchaFactory captchaFactory
            , IHttpContextAccessor httpContextAccessor
            , IMemoryCache cache
            , ICurrentUser miUser
            , IUserService userService
            , IPublicService publicService)
        {
            _message = message;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _functionService = functionService;
            _creatorFactory = creatorFactory;
            _captchaFactory = captchaFactory;
            _context = httpContextAccessor.HttpContext!;
            _cache = cache;
            _miUser = miUser;
            _userService = userService;
            _publicService = publicService;
        }

        public async Task<List<PaMenuModel>> GetSiderMenuAsync()
        {
            var topLevels = _functionService.GetFunctionsCache()
                .Where(x => _miUser.FuncIds.Contains(x.Id) && x.Node == EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu).OrderBy(x => x.Sort);
            var list = topLevels.Select(x => new PaMenuModel(x.Id, 0, x.FunctionName, x.Url, x.Icon, GetPaChildren(x.Id).ToList())).ToList();

            return await Task.FromResult(list);
        }

        private IList<PaMenuModel> GetPaChildren(long id)
        {
            var children = _functionService.GetFunctionsCache().Where(x => _miUser.FuncIds.Contains(x.Id) && x.Node != EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new PaMenuModel(x.Id, GetMenuType(x.Children), x.FunctionName, x.Url, x.Icon, GetPaChildren(x.Id).ToList())).ToList();
        }

        /// <summary>
        /// 获取菜单类型 0目录 1菜单
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private int GetMenuType(string? children)
        {
            if (string.IsNullOrEmpty(children)) return 1;
            var arr = children.Split(',');
            if (arr == null) return 1;
            var flag = _functionService.GetFunctionsCache().Any(x => arr.Contains(x.Id.ToString()) && x.FunctionType == EnumFunctionType.Menu);
            return flag ? 0 : 1;
        }

        public async Task<ResponseStructure<IList<UserRoleOption>>> GetUserRolesAsync(long userId)
        {
            var roles = await _roleRepository.GetAllAsync();
            var userRoles = await _permissionRepository.QueryUserRolesAsync(userId);
            var list = roles.Select(x => new UserRoleOption
            {
                Name = x.RoleName,
                Value = x.Id.ToString(),
                Checked = userRoles.Any(m => m.Id == x.Id),
                Remark = x.Remark
            }).ToList();

            return new ResponseStructure<IList<UserRoleOption>>(true, list);
        }

        public async Task<ResponseStructure> SetUserRoleAsync(long userId, List<long> roleIds)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null) return _message.Fail("用户不存在");

            await _permissionRepository.UserRoleRepo.ExecuteAsync("delete from SysUserRole where UserId=@id", new { id = userId });
            var list = new List<SysUserRole>();
            foreach (var roleId in roleIds)
            {
                list.Add(new SysUserRole
                {
                    Id = IdHelper.SnowflakeId(),
                    CreatedBy = userId,
                    CreatedOn = TimeHelper.LocalTime(),
                    UserId = userId,
                    RoleId = roleId
                });
            }
            await _permissionRepository.UserRoleRepo.AddManyAsync(list);

            return _message.Success();
        }

        public async Task<ResponseStructure> RegisterAsync(string userName, string password)
        {
            if (!userName.RegexValidate(PatternConst.UserName)) return _message.Fail("用户名只支持大小写字母和数字，最短4位，最长12位");
            var count = _userRepository.ExecuteScalar<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = userName.ToLower() });
            if (count > 0) return _message.Fail("用户名已存在");

            var user = _creatorFactory.NewEntity<SysUser>();
            user.UserName = userName;
            user.NickName = userName;
            user.Signature = "个性签名";
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            user.Avatar = StringHelper.DefaultAvatar();
            await _userRepository.AddAsync(user);

            var result = _message.Success("注册成功，请等待管理员审核！");
            var ids = await _permissionRepository.GetUserIdInAuthorizationCodesAsync(new string[] { "System:User:Passed" });
            await _publicService.WriteMessageAsync("审核", $"系统有新用户('{userName}')注册，需要您及时审核！", ids);
            return result;
        }

        public async Task<ResponseStructure> LoginAsync(string userName, string password, string verifyCode)
        {
            var mac = StringHelper.GetMacAddress();
            if (!_captchaFactory.Validate(mac, verifyCode)) return _message.Fail("验证码错误");

            var user = _userRepository.Get(x => x.UserName.ToLower() == userName.ToLower());
            if (user.Id <= 0) return _message.Fail("用户名不存在");
            if (user.IsEnabled == 0) return _message.Fail("没有登录权限，请联系管理员");

            var flag = user.Password == EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            if (!flag) return _message.Fail("用户名或密码错误");

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

            return _message.Success("登录成功");
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
                var roleFuncRepo = ServiceManager.Get<IRepositoryBase<SysRoleFullFunction>>();
                var userRoleRepo = ServiceManager.Get<IRepositoryBase<SysUserRole>>();

                var exp = ExpressionCreator.New<SysFunction>();
                if (userModel.IsSuperAdmin)
                {
                    userModel.Roles = AuthorizationConst.SUPER_ADMIN;
                    exp = x => true;
                }
                else
                {
                    var roleIds = (await userRoleRepo.GetAllAsync(x => x.UserId == user.Id)).Select(x => x.RoleId).ToList();
                    userModel.Roles = string.Join(",", _roleRepository.GetAll(x => roleIds.Contains(x.Id)).Select(x => x.RoleName));
                    var funcIds = (await roleFuncRepo.GetAllAsync(x => roleIds.Contains(x.RoleId))).Select(x => x.FunctionId).ToList();
                    exp = x => funcIds.Contains(x.Id);
                }
                userModel.PowerItems = _functionService.GetFunctionsCache().Where(exp.Compile()).Select(x => new PowerItem
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
            var role = _roleRepository.Get(id);
            if (role == null || role.Id <= 0) return _message.Fail("角色不存在");

            var repo = ServiceManager.Get<IRepositoryBase<SysRoleFullFunction>>();
            await repo.ExecuteAsync("delete from SysRoleFullFunction where RoleId=@id", new { id });

            var powers = new List<SysRoleFullFunction>();
            foreach (var item in funcIds)
            {
                var temp = _creatorFactory.NewEntity<SysRoleFullFunction>();
                temp.RoleId = id;
                temp.FunctionId = item;
                powers.Add(temp);
            }

            if (powers.Count > 0) await repo.AddManyAsync(powers);
            //正则移除所有角色功能缓存
            var keys = _cache.GetCacheKeys().Where(x => Regex.IsMatch(x, StringHelper.UserCachePattern()));
            _cache.RemoveAll(keys);

            return _message.Success();
        }

        public async Task LogoutAsync()
        {
            await _context.SignOutAsync();
        }

        public async Task<ResponseStructure<IList<long>>> GetRoleFunctionIdsAsync(long id)
        {
            var roleFuncRepo = ServiceManager.Get<IRepositoryBase<SysRoleFullFunction>>();
            var ids = (await roleFuncRepo.GetAllAsync(x => x.RoleId == id)).Select(x => x.FunctionId).ToList();

            return new ResponseStructure<IList<long>>(ids);
        }
    }
}