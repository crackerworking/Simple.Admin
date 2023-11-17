using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Domain.Exceptions
{
    /// <summary>
    /// 友好异常，异常级别<see cref="response_type.Fail"/>
    /// </summary>
    public class FriendlyException : Exception
    {
        public FriendlyException(string message) : base(message)
        {
            Message = message;
        }

        public new int HResult => (int)response_type.Fail;

        public new string Message { get; }

        public new Exception? InnerException => null;
    }
}