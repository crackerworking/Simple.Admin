using System.Diagnostics.CodeAnalysis;

namespace Mi.Application.Contracts.System.Models
{
    public class SysRoleFull
    {
        [NotNull]
        public string? RoleName { get; set; }

        public string? Remark { get; set; }
    }
}