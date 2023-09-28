namespace Mi.Domain.User
{
    public interface ICurrentUser
    {
        long UserId { get; }
        string UserName { get; }
        bool IsSuperAdmin { get; }
        IEnumerable<long> FuncIds { get; }

        IEnumerable<string?> AuthCodes { get; }
    }
}