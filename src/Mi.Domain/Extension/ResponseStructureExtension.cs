using Mi.Domain.Shared.Response;

namespace Mi.Domain.Extension
{
    public static class ResponseStructureExtension
    {
        public static bool IsOk(this ResponseStructure? response)
        {
            return response != null && response.Code == response_type.Success;
        }

        public static ResponseStructure<T> As<T>(this ResponseStructure model, T? result = default)
        {
            return new ResponseStructure<T>(model.Code, model.Message ?? "", result);
        }
    }
}