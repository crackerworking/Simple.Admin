using System.ComponentModel.DataAnnotations;

using Simple.Admin.Domain.Shared.GlobalVars;

namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserPlus
    {
        [RegularExpression(PatternConst.UserName, ErrorMessage = "用户名只支持大小写字母和数字，最短4位，最长12位")]
        public string userName { get; set; }
    }
}