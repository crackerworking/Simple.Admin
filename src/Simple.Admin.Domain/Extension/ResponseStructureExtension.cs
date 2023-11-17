using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Domain.Extension
{
    public static class ResponseStructureExtension
    {
        /// <summary>
        /// 指定Result类型
        /// </summary>
        /// <typeparam name="T">Result类型</typeparam>
        /// <param name="model">MessageModel实例</param>
        /// <param name="result">Result</param>
        /// <returns></returns>
        public static MessageModel<T> As<T>(this MessageModel model, T? result = default)
        {
            return new MessageModel<T>(model.Code, model.Message ?? "", result);
        }
    }
}