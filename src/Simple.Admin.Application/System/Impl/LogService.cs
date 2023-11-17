using AutoMapper;

using Microsoft.AspNetCore.Http;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Log;
using Simple.Admin.Domain.DataAccess;
using Simple.Admin.Domain.Entities.System;
using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.System.Impl
{
    public class LogService : ILogService, IScoped
    {
        private readonly HttpContext httpContext;
        private readonly MiHeader _header;
        private readonly ICurrentUser _miUser;
        private readonly IDapperRepository _dapperRepository;
        private readonly IRepository<SysLog> _logRepo;
        private readonly IRepository<SysLoginLog> _loginLogRepo;
        private readonly IMapper _mapper;

        public LogService(IHttpContextAccessor httpContextAccessor, MiHeader header, ICurrentUser miUser
            , IDapperRepository dapperRepository, IRepository<SysLog> logRepo, IRepository<SysLoginLog> loginLogRepo
            , IMapper mapper)
        {
            httpContext = httpContextAccessor.HttpContext;
            _header = header;
            _miUser = miUser;
            _dapperRepository = dapperRepository;
            _logRepo = logRepo;
            _loginLogRepo = loginLogRepo;
            _mapper = mapper;
        }

        public async Task<MessageModel<PagingModel<SysLoginLogFull>>> GetLoginLogListAsync(LoginLogSearch search)
        {
            var exp = PredicateBuilder.Instance.Create<SysLoginLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.Succeed == 1, x => x.Status == 1)
                .AndIf(search.Succeed == 2, x => x.Status == 0);
            var model = await _loginLogRepo.GetPagedAsync(exp, search.Page, search.Size, new List<QuerySortField>
            {
                new QuerySortField
                {
                    FieldName = nameof(SysLog.CreatedOn),
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

        public async Task<MessageModel<PagingModel<SysLogFull>>> GetLogListAsync(LogSearch search)
        {
            var exp = PredicateBuilder.Instance.Create<SysLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.UserId.HasValue && search.UserId > 0, x => x.UserId == search.UserId.GetValueOrDefault())
                .AndIf(search.Succeed == 1, x => x.Succeed == 1)
                .AndIf(search.Succeed == 2, x => x.Succeed == 0)
                .AndIf(search.CreatedOn != null && search.CreatedOn.Length == 2, x => x.CreatedOn >= search.CreatedOn![0].Date && x.CreatedOn <= search.CreatedOn![1].Date.AddDays(1).AddSeconds(-1));
            var model = await _logRepo.GetPagedAsync(exp, search.Page, search.Size, new List<QuerySortField>
            {
                new QuerySortField
                {
                    FieldName = nameof(SysLog.CreatedOn),
                    Desc = true
                }
            });
            var clonedModel = new PagingModel<SysLogFull>
            {
                Total = model.Total,
                Rows = _mapper.Map<IEnumerable<SysLogFull>>(model.Rows)
            };

            return new MessageModel<PagingModel<SysLogFull>>(clonedModel);
        }

        public async Task<bool> SetExceptionAsync(string uniqueId, string errorMsg)
        {
            if (_miUser.IsDemo) return false;
            var log = await _logRepo.GetAsync(x => x.UniqueId == uniqueId);
            if (log == null) return false;
            log.Exception = errorMsg;
            log.Succeed = 0;
            return await _logRepo.UpdateAsync(log) > 0;
        }

        public async Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? uniqueId = default, string? contentType = null, bool succeed = true, string? exception = null)
        {
            if (_miUser.IsDemo) return false;
            var log = new SysLog();
            log.RequestUrl = url;
            log.RequestParams = param;
            log.ActionFullName = actionFullName;
            log.ContentType = contentType;
            log.Succeed = succeed ? 1 : 0;
            log.Exception = exception;
            log.UserId = _miUser.UserId;
            log.UserName = _miUser.UserName;
            log.UniqueId = uniqueId;
            return await _logRepo.AddAsync(log) > 0;
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