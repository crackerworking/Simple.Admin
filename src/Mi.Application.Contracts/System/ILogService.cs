using Mi.Application.Contracts.System.Models.Log;

namespace Mi.Application.Contracts.System
{
    public interface ILogService
    {
        Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo);

        Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? uniqueId = default, string? contentType = default, bool succeed = true, string? exception = default);

        Task<ResponseStructure<PagingModel<SysLoginLogFull>>> GetLoginLogListAsync(LoginLogSearch search);

        Task<ResponseStructure<PagingModel<SysLogFull>>> GetLogListAsync(LogSearch search);

        Task<bool> SetExceptionAsync(string uniqueId, string errorMsg);
    }
}