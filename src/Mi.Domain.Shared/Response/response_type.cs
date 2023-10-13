namespace Mi.Domain.Shared.Response
{
    public enum response_type
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 10000,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 10001,

        /// <summary>
        /// 未登录
        /// </summary>
        NonAuth = 10002,

        /// <summary>
        /// 禁止访问
        /// </summary>
        Forbidden = 10003,

        /// <summary>
        /// 找不到，不存在
        /// </summary>
        NonExist = 10004,

        /// <summary>
        /// 程序错误
        /// </summary>
        Error = 10005,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 10006,

        /// <summary>
        /// 请求频繁
        /// </summary>
        FrequentRequests = 10007
    }
}