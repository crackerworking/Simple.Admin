using Simple.Admin.Application.Contracts.System.Models.Log;

namespace Simple.Admin.Application.Contracts.System
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
        /// 登录日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<MessageModel<PagingModel<SysLoginLogFull>>> GetLoginLogListAsync(LoginLogSearch search);
    }
}