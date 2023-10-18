using Mi.Domain.Shared.Options;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mi.Domain.Extension
{
    public static class ModelStateExtension
    {
        /// <summary>
        /// 第一条错误消息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string FirstOrDefaultMsg(this ModelStateDictionary modelState)
        {
            var result = new List<Option>();
            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            if (errorFieldsAndMsgs.Any())
            {
                var e1 = errorFieldsAndMsgs.FirstOrDefault();
                if (e1 != null) return e1.Errors.FirstOrDefault()!.ErrorMessage;
            }
            return string.Empty;
        }
    }
}