namespace Mi.Application.Contracts.System.Models.Permission
{
    public class SetUserRoleIn
    {
        public long userId { get; set; }
        public List<long> roleIds { get; set; }
    }
}