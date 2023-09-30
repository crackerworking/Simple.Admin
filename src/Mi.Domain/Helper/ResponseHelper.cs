using Mi.Domain.Shared.Response;

namespace Mi.Domain.Helper
{
    public static class ResponseHelper
    {
        public static ResponseStructure Success(string? message = default)
        {
            message ??= "successful";
            return new ResponseStructure(response_type.Success, message);
        }

        public static ResponseStructure Fail(string? message = default)
        {
            message ??= "failed";
            return new ResponseStructure(response_type.Fail, message);
        }

        public static ResponseStructure NonExist(string? message = default)
        {
            message ??= "data does not exist.";
            return new ResponseStructure(response_type.NonExist, message);
        }

        public static ResponseStructure SuccessOrFail(bool flag, string? message = default)
        {
            return flag ? Success(message) : Fail(message);
        }

        public static bool IsSucceed(this ResponseStructure? response)
        {
            return response != null && response.Code == response_type.Success;
        }

        public static ResponseStructure<T> As<T>(this ResponseStructure model, T? result = default)
        {
            return new ResponseStructure<T>(model.Code, model.Message ?? "", result);
        }

        public static ResponseStructure ParameterError(string? message = default)
        {
            message ??= "Parameter error.";
            return new ResponseStructure(response_type.ParameterError, message);
        }
    }
}