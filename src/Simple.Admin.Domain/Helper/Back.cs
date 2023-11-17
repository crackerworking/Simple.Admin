using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Domain.Helper
{
    /// <summary>
    /// webapi返回
    /// </summary>
    public static class Back
    {
        public static MessageModel Success(string? message = default)
        {
            message ??= "success";
            return new MessageModel(response_type.Success, message);
        }

        public static MessageModel Fail(string? message = default)
        {
            message ??= "fail";
            return new MessageModel(response_type.Fail, message);
        }

        public static MessageModel NonExist(string? message = default)
        {
            message ??= "data does not exist.";
            return new MessageModel(response_type.NonExist, message);
        }

        public static MessageModel SuccessOrFail(bool flag, string? message = default)
        {
            return flag ? Success(message) : Fail(message);
        }

        public static MessageModel ParameterError(string? message = default)
        {
            message ??= "Parameter error.";
            return new MessageModel(response_type.ParameterError, message);
        }
    }
}