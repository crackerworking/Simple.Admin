using AutoMapper;

using Microsoft.AspNetCore.Http;

using Simple.Admin.Application.Contracts.System.Models.Log;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.System.Impl
{
    public class LogService : ILogService, IScoped
    {
        private readonly HttpContext httpContext;
        private readonly MiHeader _header;
        private readonly ICurrentUser _miUser;
        private readonly IDapperRepository _dapperRepository;
        private readonly IRepository<SysLoginLog> _loginLogRepo;
        private readonly IMapper _mapper;

        public LogService(IHttpContextAccessor httpContextAccessor, MiHeader header, ICurrentUser miUser
            , IDapperRepository dapperRepository, IRepository<SysLoginLog> loginLogRepo
            , IMapper mapper)
        {
            httpContext = httpContextAccessor.HttpContext!;
            _header = header;
            _miUser = miUser;
            _dapperRepository = dapperRepository;
            _loginLogRepo = loginLogRepo;
            _mapper = mapper;
        }

        public async Task<MessageModel<PagingModel<SysLoginLogFull>>> GetLoginLogListAsync(LoginLogSearch search)
        {
            var exp = PredicateBuilder.Instance.Create<SysLoginLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.Succeed == 1, x => x.Status == 1)
                .AndIf(search.Succeed == 2, x => x.Status == 0)
                .AndIf(search.CreatedOn != null && search.CreatedOn.Length == 2, x => x.CreatedOn >= search.CreatedOn![0] && x.CreatedOn <= search.CreatedOn![1]);
            var model = await _loginLogRepo.GetPagedAsync(exp, search.Page, search.Size, new List<QuerySortField>
            {
                new QuerySortField
                {
                    FieldName = nameof(SysLoginLog.CreatedOn),
                    Desc = true
                }
            });
            var clonedModel = new PagingModel<SysLoginLogFull>
            {
                Total = model.Total,
                Rows = _mapper.Map<IEnumerable<SysLoginLogFull>>(model.Rows)
            };

            return new MessageModel<PagingModel<SysLoginLogFull>>(clonedModel);
        }

        public async Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo)
        {
            var model = new SysLoginLog();
            model.UserName = userName;
            model.OperationInfo = operationInfo;
            model.Status = succeed ? 1 : 0;
            model.IpAddress = _header.Ip;
            model.RegionInfo = _header.Region;
            model.Browser = _header.Browser;
            model.System = _header.System;
            return await _loginLogRepo.AddAsync(model) > 0;
        }
    }
}