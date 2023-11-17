using Simple.Admin.Application.Contracts.Public;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Options;

namespace Simple.Admin.Application.Public
{
    internal class PublicOptionReadService : IPublicOptionReadApi
    {
        private readonly IQuickDict _dictionaryApi;

        public PublicOptionReadService(IQuickDict dictionaryApi)
        {
            _dictionaryApi = dictionaryApi;
        }

        public Task<List<Option>> GetSexOptionsAsync()
        {
            return _dictionaryApi.GetOptionsAsync("Sex");
        }
    }
}