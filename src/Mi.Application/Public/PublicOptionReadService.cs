using Mi.Application.Contracts.Public;
using Mi.Domain.Shared.Core;

namespace Mi.Application.Public
{
    internal class PublicOptionReadService : IPublicOptionReadApi
    {
        private readonly IDictionaryApi _dictionaryApi;

        public PublicOptionReadService(IDictionaryApi dictionaryApi)
        {
            _dictionaryApi = dictionaryApi;
        }

        public Task<List<Option>> GetSexOptionsAsync()
        {
            return _dictionaryApi.GetOptionsAsync("Sex");
        }
    }
}