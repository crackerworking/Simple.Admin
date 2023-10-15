namespace Mi.Domain.Shared.Core
{
    public interface ICurrentUser
    {
        long UserId { get; }
        string UserName { get; }
        bool IsSuperAdmin { get; }
        bool IsDemo { get; }
        IEnumerable<long> FuncIds { get; }

        IEnumerable<string?> AuthCodes { get; }
    }
}