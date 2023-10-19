using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.Development
{
    [Authorize]
    public class CommandController : MiControllerBase
    {
        private readonly IQuickDict _dictionaryApi;

        public CommandController(IQuickDict dictionaryApi)
        {
            _dictionaryApi = dictionaryApi;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseStructure<string>> PostAsync([FromBody] Option option)
        {
            var str = string.Empty;

            switch (option.Name)
            {
                case "字典value":
                    str = await _dictionaryApi.GetAsync(option.Value!);
                    break;
            }

            return new ResponseStructure<string>(str);
        }
    }
}