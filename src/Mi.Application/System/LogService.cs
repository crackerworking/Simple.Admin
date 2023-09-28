using Mi.Core.API;
using Mi.Core.Factory;
using Mi.Core.Service;
using Mi.IRepository.BASE;

using Microsoft.AspNetCore.Http;

namespace Mi.Application.System
{
    public class LogService : ILogService, IScoped
    {
        private readonly HttpContext httpContext;
        private readonly CreatorFactory _creator;
        private readonly MiHeader _header;
        private readonly IMiUser _miUser;

        public LogService(IHttpContextAccessor httpContextAccessor, CreatorFactory creator, MiHeader header, IMiUser miUser)
        {
            httpContext = httpContextAccessor.HttpContext;
            _creator = creator;
            _header = header;
            _miUser = miUser;
        }

        public async Task<ResponseStructure<PagingModel<SysLoginLog>>> GetLoginLogListAsync(LoginLogSearch search)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLoginLog>>();
            var exp = ExpressionCreator.New<SysLoginLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.Succeed == 1, x => x.Status == 1)
                .AndIf(search.Succeed == 2, x => x.Status == 0);
            var list = await repo.QueryPageAsync(search.Page, search.Size, x => x.CreatedOn, exp, false);
            return new ResponseStructure<PagingModel<SysLoginLog>>(list);
        }

        public async Task<ResponseStructure<PagingModel<SysLog>>> GetLogListAsync(LogSearch search)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLog>>();
            var exp = ExpressionCreator.New<SysLog>()
                .AndIf(!string.IsNullOrEmpty(search.UserName), x => x.UserName.Contains(search.UserName!))
                .AndIf(search.UserId.HasValue && search.UserId > 0, x => x.UserId == search.UserId.GetValueOrDefault())
                .AndIf(search.Succeed == 1, x => x.Succeed == 1)
                .AndIf(search.Succeed == 2, x => x.Succeed == 0)
                .AndIf(search.CreatedOn != null && search.CreatedOn.Length == 2, x => x.CreatedOn >= search.CreatedOn![0].Date && x.CreatedOn <= search.CreatedOn![1].Date.AddDays(1).AddSeconds(-1));
            var list = await repo.QueryPageAsync(search.Page, search.Size, x => x.CreatedOn, exp, false);
            return new ResponseStructure<PagingModel<SysLog>>(list);
        }

        public async Task<bool> SetExceptionAsync(string uniqueId, string errorMsg)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLog>>();
            var log = await repo.GetAsync(x => x.UniqueId == uniqueId);
            if (log == null) return false;
            log.Exception = errorMsg;
            log.Succeed = 0;
            return await repo.UpdateAsync(log);
        }

        public async Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? uniqueId = default, string? contentType = null, bool succeed = true, string? exception = null)
        {
            var repo = DotNetService.Get<IRepositoryBase<SysLog>>();
            var log = _creator.NewEntity<SysLog>();
            log.RequestUrl = url;
            log.RequestParams = param;
            log.ActionFullName = actionFullName;
            log.ContentType = contentType;
            log.Succeed = succeed ? 1 : 0;
            log.Exception = exception;
            log.UserId = _miUser.UserId;
            log.UserName = _miUser.UserName;
            log.UniqueId = uniqueId;
            return await repo.AddAsync(log);
        }

        public async Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo)
        {
            var model = _creator.NewEntity<SysLoginLog>();
            var repo = DotNetService.Get<IRepositoryBase<SysLoginLog>>();
            model.UserName = userName;
            model.OperationInfo = operationInfo;
            model.Status = succeed ? 1 : 0;
            model.IpAddress = _header.Ip;
            model.RegionInfo = _header.Region;
            model.Browser = _header.Browser;
            model.System = _header.System;
            return await repo.AddAsync(model);
        }
    }
}