using System.Text.Json;

using Mi.Domain.Helper;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Options;
using Mi.Domain.Shared.Response;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Web.Host
{
    public class ValuesController : MiControllerBase
    {
        private readonly IQuickDict _dictionaryApi;

        public ValuesController(IQuickDict dictionaryApi)
        {
            _dictionaryApi = dictionaryApi;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> ExecuteAsync([FromBody] Option option)
        {
            var str = string.Empty;

            switch (option.Name)
            {
                case "字典value":
                    str = await _dictionaryApi.GetAsync(option.Value!);
                    break;

                case "今日日志":
                    var names = FileLogging.GetEveryDayLogs(DateTime.Now).Select(x => x.Name).ToList();
                    str = JsonSerializer.Serialize(names);
                    break;
            }

            return new MessageModel<string>(str);
        }

        [HttpGet]
        public FileResult CreateImageWithVerifyCode()
        {
            var bytes = DrawingHelper.CreateByteByImgVerifyCode("3456", 160, 40);
            return File(bytes, "image/png");
        }
    }
}