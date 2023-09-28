using System.Diagnostics.CodeAnalysis;

namespace Mi.Application.Contracts.System.Models
{
    public class SysLogFull
    {
        public long UserId { get; set; }

        [NotNull]
        public string? UserName { get; set; }

        public string? ActionFullName { get; set; }

        public string? RequestParams { get; set; }

        public string? RequestUrl { get; set; }

        public string? ContentType { get; set; }

        public int Succeed { get; set; } = 0;

        public string? Exception { get; set; }

        public string? UniqueId { get; set; }
    }
}