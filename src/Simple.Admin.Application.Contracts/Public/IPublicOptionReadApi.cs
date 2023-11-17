using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Options;

namespace Simple.Admin.Application.Contracts.Public
{
    public interface IPublicOptionReadApi : IScoped
    {
        /// <summary>
        /// 性别选项
        /// </summary>
        /// <returns></returns>
        Task<List<Option>> GetSexOptionsAsync();
    }
}