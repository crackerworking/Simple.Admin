namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserRoleOption : SelectionOption, IRemark
    {
        public string? Remark { get; set; }
    }
}