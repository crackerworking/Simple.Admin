using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.Development
{
    [ApiRoute]
    public class CommandController : Controller
    {
        private readonly IDictionaryApi _dictionaryApi;

        public CommandController(IDictionaryApi dictionaryApi)
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