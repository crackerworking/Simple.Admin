using AutoMapper;

using Dapper;

using Mi.Domain.Shared.Core;

namespace Mi.Application.System.Impl
{
    public class RoleService : IRoleService, IScoped
    {
        private readonly IRepository<SysRole> _roleRepository;
        private readonly ICurrentUser _miUser;
        private readonly IDapperRepository _dapperRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<SysUserRole> _userRoleRepo;

        public RoleService(IRepository<SysRole> roleRepository, ICurrentUser miUser, IDapperRepository dapperRepository
            , IMapper mapper, IRepository<SysUserRole> userRoleRepo)
        {
            _roleRepository = roleRepository;
            _miUser = miUser;
            _dapperRepository = dapperRepository;
            _mapper = mapper;
            _userRoleRepo = userRoleRepo;
        }

        public async Task<ResponseStructure> AddRoleAsync(string name, string? remark)
        {
            var isExist = await _roleRepository.AnyAsync(x => x.RoleName.ToLower() == name.ToLower());
            if (isExist) return ResponseHelper.Fail("角色名已存在");

            var role = new SysRole
            {
                RoleName = name,
                Remark = remark
            };
            await _roleRepository.AddAsync(role);

            return ResponseHelper.Success();
        }

        public async Task<ResponseStructure<SysRoleFull>> GetRoleAsync(long id)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == id);
            var model = _mapper.Map<SysRoleFull>(role);

            return new ResponseStructure<SysRoleFull>(true, model);
        }

        public async Task<ResponseStructure<PagingModel<SysRoleFull>>> GetRoleListAsync(RoleSearch search)
        {
            var sql = "select * from SysRole where IsDeleted=0 ";
            var parameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(search.RoleName))
            {
                sql += " and RoleName like @name ";
                parameter.Add("name", "%" + search.RoleName + "%");
            }

            var pageModel = await _dapperRepository.QueryPagedAsync<SysRoleFull>(sql, search.Page, search.Size, "CreatedOn desc", parameter);

            return new ResponseStructure<PagingModel<SysRoleFull>>(true, "查询成功", pageModel);
        }

        public async Task<ResponseStructure> RemoveRoleAsync(long id)
        {
            var count = await _userRoleRepo.CountAsync(x => x.RoleId == id);
            if (count > 0) return ResponseHelper.Fail("角色正在使用，请先移除角色下用户");

            await _roleRepository.UpdateAsync(id, node => node.
                SetColumn(x => x.IsDeleted, 1));
            //移除角色下功能
            await _dapperRepository.ExecuteAsync("delete from SysRoleFunction where RoleId=@id", new { id });

            return ResponseHelper.Success();
        }

        public async Task<ResponseStructure> UpdateRoleAsync(long id, string name, string remark)
        {
            var isExist = await _roleRepository.AnyAsync(x => x.RoleName.ToLower() == name.ToLower());
            var role = await _roleRepository.GetAsync(x => x.Id == id);
            if (role == null) return ResponseHelper.NonExist();
            if (isExist && role.RoleName != name) return ResponseHelper.Fail("角色名已存在");

            await _roleRepository.UpdateAsync(id, node => node
                .SetColumn(x => x.RoleName, name)
                .SetColumn(x => x.Remark, remark));

            return ResponseHelper.Success();
        }
    }
}