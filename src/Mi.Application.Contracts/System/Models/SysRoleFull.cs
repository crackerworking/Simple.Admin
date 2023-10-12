using System.Diagnostics.CodeAnalysis;

namespace Mi.Application.Contracts.System.Models
{
    public class SysRoleFull
    {
        public long Id { get; set; }

        [NotNull]
        public string? RoleName { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}