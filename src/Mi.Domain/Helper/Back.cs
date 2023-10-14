using Mi.Domain.Shared.Response;

namespace Mi.Domain.Helper
{
    public static class Back
    {
        public static ResponseStructure Success(string? message = default)
        {
            message ??= "success";
            return new ResponseStructure(response_type.Success, message);
        }

        public static ResponseStructure Fail(string? message = default)
        {
            message ??= "fail";
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

        public static ResponseStructure ParameterError(string? message = default)
        {
            message ??= "Parameter error.";
            return new ResponseStructure(response_type.ParameterError, message);
        }
    }
}