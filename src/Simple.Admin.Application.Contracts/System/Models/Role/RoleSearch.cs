namespace Simple.Admin.Application.Contracts.System.Models.Role
{
    public class RoleSearch : PagingSearchModel
    {
        public string? RoleName { get; set; }
    }
}