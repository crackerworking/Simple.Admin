using Mi.Application.Contracts.System.Models.Log;

namespace Mi.Application.Contracts.System
{
    public interface ILogService
    {
        /// <summary>
        /// 写入登录日志
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="succeed">是否成功</param>
        /// <param name="operationInfo">登录返回信息</param>
        /// <returns></returns>
        Task<bool> WriteLoginLogAsync(string userName, bool succeed, string operationInfo);

        /// <summary>
        /// 写入操作日志
        /// </summary>
        /// <param name="url">webapi地址</param>
        /// <param name="param">参数</param>
        /// <param name="actionFullName"></param>
        /// <param name="uniqueId">唯一值</param>
        /// <param name="contentType">请求参数类型</param>
        /// <param name="succeed">是否成功</param>
        /// <param name="exception">异常信息</param>
        /// <returns></returns>
        Task<bool> WriteLogAsync(string url, string? param, string? actionFullName, string? uniqueId = default, string? contentType = default, bool succeed = true, string? exception = default);

        /// <summary>
        /// 登录日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<SysLoginLogFull>>> GetLoginLogListAsync(LoginLogSearch search);

        /// <summary>
        /// 操作日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<SysLogFull>>> GetLogListAsync(LogSearch search);

        /// <summary>
        /// 设置操作日志异常（更新已有操作日志在<see cref="WriteLogAsync"/>之后）
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        Task<bool> SetExceptionAsync(string uniqueId, string errorMsg);
    }
}