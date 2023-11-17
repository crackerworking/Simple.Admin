namespace Simple.Admin.Domain.Shared.Attributes
{
    /// <summary>
    /// 权限编码
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AuthorizeCodeAttribute : Attribute
    {
        public string Code { get; private set; }

        public AuthorizeCodeAttribute(string code)
        {
            Code = code;
        }
    }
}