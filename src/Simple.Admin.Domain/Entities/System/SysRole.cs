using Simple.Admin.Domain.Entities;
using Simple.Admin.Domain.Shared.Fields;

namespace Simple.Admin.Domain.Entities.System
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("SysRole")]
    public class SysRole : EntityBase, IRemark
    {
        [NotNull]
        public string? RoleName { get; set; }

        public string? Remark { get; set; }
    }
}