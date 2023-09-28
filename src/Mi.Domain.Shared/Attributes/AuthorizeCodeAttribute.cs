namespace Mi.Domain.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AuthorizeCodeAttribute : Attribute
    {
        public string Code { get; private set; }

        public AuthorizeCodeAttribute(string code)
        {
            this.Code = code;
        }
    }
}