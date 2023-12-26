using System.Data;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using Simple.Admin.Application.Contracts.System.Models.Function;
using Simple.Admin.Application.Contracts.System.Models.Permission;
using Simple.Admin.Application.Contracts.System.Models.User;
using Simple.Admin.Domain.Entities.System.Enum;
using Simple.Admin.Domain.Services;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.System.Impl
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
        private readonly IRepository<SysUserRole> _userRoleRepo;
        private readonly IDapperRepository _dapperRepository;
        private readonly IRepository<SysRoleFunction> _roleFunctionRepo;
        private readonly ITransactionContext _transactionContext;

        public PermissionService(IRepository<SysUser> userRepository
            , IRepository<SysRole> roleRepository
            , IFunctionService functionService
            , IHttpContextAccessor httpContextAccessor
            , IMemoryCache cache
            , ICurrentUser miUser
            , IUserService userService
            , IRepository<SysUserRole> userRoleRepo
            , IDapperRepository dapperRepository
            , IRepository<SysRoleFunction> roleFunctionRepo
            , ITransactionContext transactionContext)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _functionService = functionService;
            _context = httpContextAccessor.HttpContext!;
            _cache = cache;
            _miUser = miUser;
            _userService = userService;
            _userRoleRepo = userRoleRepo;
            _dapperRepository = dapperRepository;
            _roleFunctionRepo = roleFunctionRepo;
            _transactionContext = transactionContext;
        }

        public async Task<MessageModel<List<RouterItem>>> GetSiderMenuAsync()
        {
            var topLevels = (await _functionService.GetFunctionsCacheAsync()).Where(x => x.ParentId <= 0 && x.FunctionType == (int)function_type.Menu).OrderBy(x => x.Sort);
            var functionIds = topLevels.Select(x => x.Id).ToList();
            var roles = await _roleRepository.GetListAsync(x => x.IsDeleted == 0);
            var roleFunction = await _roleFunctionRepo.GetListAsync(x => functionIds.Contains(x.FunctionId));
            var btnFunctions = (await _functionService.GetFunctionsCacheAsync()).Where(x => x.FunctionType == (int)function_type.Button);
            var list = new List<RouterItem>();
            foreach (var x in topLevels)
            {
                var roleIds = roleFunction.Where(m => m.FunctionId == x.Id).Select(m => m.RoleId);
                var roleNames = roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.RoleName);
                roleNames = roleNames.Append(AuthorizationConst.ADMIN).Distinct();
                var children = (await GetChildrenAsync(x.Id)).ToList();
                var temp = new RouterItem
                {
                    name = x.Name ?? "",
                    path = x.Url,
                    meta = new RouterItemMeta
                    {
                        icon = x.Icon,
                        rank = x.Sort,
                        title = x.Title,
                        roles = roleNames.ToArray(),
                        auths = btnFunctions.Where(b => b.ParentId == x.Id && _miUser.FuncIds.Contains(b.Id)).Select(b => b.AuthorizationCode ?? "").ToArray(),
                        frameSrc = x.FrameSrc
                    },
                    children = (await GetChildrenAsync(x.Id)).OrderBy(x => x.meta.rank).ToList()
                };
                list.Add(temp);
            }

            return new MessageModel<List<RouterItem>>(list);
        }

        private async Task<IList<RouterItem>> GetChildrenAsync(long id)
        {
            var children = (await _functionService.GetFunctionsCacheAsync()).Where(x => x.ParentId > 0 && x.FunctionType == (int)function_type.Menu && x.ParentId == id).OrderBy(x => x.Sort);
            var functionIds = children.Select(x => x.Id).ToList();
            var roles = await _roleRepository.GetListAsync(x => x.IsDeleted == 0);
            var roleFunction = await _roleFunctionRepo.GetListAsync(x => functionIds.Contains(x.FunctionId));
            var btnFunctions = (await _functionService.GetFunctionsCacheAsync()).Where(x => x.FunctionType == (int)function_type.Button);
            var list = new List<RouterItem>();
            foreach (var x in children)
            {
                var roleIds = roleFunction.Where(m => m.FunctionId == x.Id).Select(m => m.RoleId);
                var roleNames = roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.RoleName);
                roleNames = roleNames.Append(AuthorizationConst.ADMIN).Distinct();
                var temp = new RouterItem
                {
                    name = x.Name ?? "",
                    path = x.Url,
                    meta = new RouterItemMeta
                    {
                        icon = x.Icon,
                        rank = x.Sort,
                        title = x.Title,
                        roles = roleNames.ToArray(),
                        auths = btnFunctions.Where(b => b.ParentId == x.Id && _miUser.FuncIds.Contains(b.Id)).Select(b => b.AuthorizationCode ?? "").ToArray(),
                        frameSrc = x.FrameSrc
                    },
                    children = (await GetChildrenAsync(x.Id)).OrderBy(x => x.meta.rank).ToList()
                };
                list.Add(temp);
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
            var flag = (await _functionService.GetFunctionsCacheAsync()).Any(x => arr.Contains(x.Id.ToString()) && x.FunctionType == (int)function_type.Menu);
            return flag ? 0 : 1;
        }

        public async Task<MessageModel<IList<UserRoleOption>>> GetUserRolesAsync(long userId)
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

            return new MessageModel<IList<UserRoleOption>>(true, list);
        }

        public async Task<MessageModel> SetUserRoleAsync(SetUserRoleIn input)
        {
            var user = await _userRepository.GetAsync(x => x.Id == input.userId);
            if (user == null) return Back.Fail("用户不存在");

            await _dapperRepository.ExecuteAsync("delete from SysUserRole where UserId=@id", new { id = input.userId });
            var list = new List<SysUserRole>();
            foreach (var roleId in input.roleIds)
            {
                list.Add(new SysUserRole
                {
                    Id = SnowflakeIdHelper.Next(),
                    UserId = input.userId,
                    RoleId = roleId
                });
            }
            await _userRoleRepo.AddRangeAsync(list);

            return Back.Success();
        }

        public async Task<MessageModel> RegisterAsync(RegisterIn input)
        {
            if (!input.userName.RegexValidate(PatternConst.UserName)) return Back.Fail("用户名只支持大小写字母和数字，最短4位，最长12位");
            var count = await _dapperRepository.ExecuteScalarAsync<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = input.userName.ToLower() });
            if (count > 0) return Back.Fail("用户名已存在");

            var user = new SysUser
            {
                UserName = input.userName,
                NickName = input.userName,
                Signature = "",
                PasswordSalt = EncryptionHelper.GetPasswordSalt()
            };
            user.Password = EncryptionHelper.GenEncodingPassword(input.password, user.PasswordSalt);
            user.Avatar = StringHelper.DefaultAvatar();
            await _userRepository.AddAsync(user);

            var result = Back.Success("注册成功，请等待管理员审核！");
            var ids = await _dapperRepository.QueryAsync<long>(@"select ur.UserId from SysFunction s
                inner join SysRoleFunction sr on s.id=sr.FunctionId
                inner join SysUserRole ur on sr.RoleId=ur.RoleId
                where s.IsDeleted=0 and s.AuthorizationCode = 'System:User:Passed'");
            MessageFactory.Instance.WriteMessage("审核", $"系统有新用户('{input.userName}')注册，需要您及时审核！", ids);
            return result;
        }

        public async Task<MessageModel> LoginAsync(LoginIn input)
        {
            var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == input.username.ToLower());
            if (user == null) return Back.Fail("用户名不存在");
            if (user.IsEnabled == 0) return Back.Fail("没有登录权限，请联系管理员");

            var flag = user.Password == EncryptionHelper.GenEncodingPassword(input.password, user.PasswordSalt);
            if (!flag) return Back.Fail("用户名或密码错误");

            var roleNameArray = (await _userService.GetRolesAsync(user.Id)).Select(x => x.RoleName).ToList();
            if (user.IsSuperAdmin == 1)
            {
                roleNameArray.Add(AuthorizationConst.ADMIN);
            }
            var roleNames = string.Join(",", roleNameArray);
            var claims = new Claim[]
            {
                new (ClaimTypes.Name,user.UserName),
                new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                new (ClaimTypes.Role,roleNames),
                new (ClaimTypes.UserData,StringHelper.UserDataString(user.Id,user.UserName,roleNames))
            };
            var expireTime = DateTime.Now.AddMinutes(Convert.ToInt32(App.Configuration.GetSection("JWT")["Expires"]));
            string token = TokenHelper.GenerateToken(claims, expireTime);
            claims[3] = new Claim(ClaimTypes.Role, roleNames + "," + "refresh-token");
            string refreshToken = TokenHelper.GenerateToken(claims, expireTime.AddHours(1));
            var vo = new LoginVo
            {
                username = user.UserName,
                roles = [.. roleNameArray],
                accessToken = token,
                expires = expireTime.ToString("yyyy/MM/dd HH:mm:ss"),
                refreshToken = refreshToken
            };

            return Back.Success("登录成功").As(vo);
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
                    userModel.Roles = AuthorizationConst.ADMIN;
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

        public async Task<MessageModel> SetRoleFunctionsAsync(SetRoleFunctionsIn input)
        {
            var role = _roleRepository.GetAsync(x => x.Id == input.id);
            if (role == null || role.Id <= 0) return Back.Fail("角色不存在");

            try
            {
                _transactionContext.Begin();
                await _dapperRepository.ExecuteAsync("delete from SysRoleFunction where RoleId=@id", new { input.id });

                var powers = new List<SysRoleFunction>();
                foreach (var item in input.funcIds)
                {
                    var temp = new SysRoleFunction
                    {
                        RoleId = input.id,
                        FunctionId = item,
                        Id = SnowflakeIdHelper.Next()
                    };
                    powers.Add(temp);
                }

                if (powers.Count > 0) await _roleFunctionRepo.AddRangeAsync(powers);

                _transactionContext.Commit();
                //正则移除所有角色功能缓存
                _cache.RemoveByPattern(StringHelper.UserFunctionCachePattern());
            }
            catch (Exception)
            {
                _transactionContext.Rollback();
                throw;
            }

            return Back.Success();
        }

        public async Task<MessageModel<IList<long>>> GetRoleFunctionIdsAsync(PrimaryKey input)
        {
            var ids = (await _roleFunctionRepo.GetListAsync(x => x.RoleId == input.id)).Select(x => x.FunctionId).ToList();

            return new MessageModel<IList<long>>(ids);
        }
    }
}