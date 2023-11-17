using Simple.Admin.Domain.Shared.Fields;
using Simple.Admin.Domain.Shared.Options;

namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserRoleOption : SelectionOption, IRemark
    {
        public string? Remark { get; set; }
    }
}