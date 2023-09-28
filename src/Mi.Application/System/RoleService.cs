using Dapper;
using Mi.Core.API;
using Mi.Core.Helper;

namespace Mi.Application.System
{
    public class RoleService : IRoleService, IScoped
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMiUser _miUser;
        private readonly ResponseStructure _message;

        public RoleService(IRoleRepository roleRepository, IMiUser miUser, ResponseStructure message)
        {
            _roleRepository = roleRepository;
            _miUser = miUser;
            _message = message;
        }

        public async Task<ResponseStructure> AddRoleAsync(string name, string? remark)
        {
            var isExist = (await _roleRepository.GetAllAsync(x => x.RoleName.ToLower() == name.ToLower())).Count > 0;
            if (isExist) return _message.Fail("角色名已存在");

            var role = new SysRole
            {
                Id = IdHelper.SnowflakeId(),
                RoleName = name,
                Remark = remark,
                CreatedBy = _miUser.UserId,
                CreatedOn = TimeHelper.LocalTime()
            };
            await _roleRepository.AddAsync(role);

            return _message.Success();
        }

        public async Task<ResponseStructure<SysRole>> GetRoleAsync(long id)
        {
            var role = await _roleRepository.GetAsync(id);

            return new ResponseStructure<SysRole>(true, role);
        }

        public async Task<ResponseStructure<PagingModel<SysRole>>> GetRoleListAsync(RoleSearch search)
        {
            var sql = "select * from SysRole where IsDeleted=0 ";
            var parameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(search.RoleName))
            {
                sql += " and RoleName like @name ";
                parameter.Add("name", "%" + search.RoleName + "%");
            }

            var pageModel = await _roleRepository.QueryPageAsync(search.Page, search.Size, sql, parameter, "CreatedOn desc");

            return new ResponseStructure<PagingModel<SysRole>>(true, "查询成功", pageModel);
        }

        public async Task<ResponseStructure> RemoveRoleAsync(long id)
        {
            var count = await _roleRepository.UsedRoleCountAsync(id);
            if (count > 0) return _message.Fail("角色正在使用，请先移除角色下用户");

            await _roleRepository.UpdateAsync(id, node => node.MarkDeleted()
                .ModifiedUser(_miUser.UserId).ModifiedTime());
            //移除角色下功能
            await _roleRepository.ExecuteAsync("delete from SysRoleFunction where RoleId=@id", new { id });

            return _message.Success();
        }

        public async Task<ResponseStructure> UpdateRoleAsync(long id, string name, string remark)
        {
            var isExist = (await _roleRepository.GetAllAsync(x => x.RoleName.ToLower() == name.ToLower())).Count > 0;
            var role = await _roleRepository.GetAsync(id);
            if (isExist && role.RoleName != name) return _message.Fail("角色名已存在");

            await _roleRepository.UpdateAsync(id, node => node.Set(x => x.RoleName, name)
                .ModifiedTime()
                .Set(x => x.Remark, remark)
                .ModifiedUser(_miUser.UserId));

            return _message.Success();
        }
    }
}