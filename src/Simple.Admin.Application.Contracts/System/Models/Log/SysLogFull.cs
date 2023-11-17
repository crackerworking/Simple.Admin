using System.Diagnostics.CodeAnalysis;

namespace Simple.Admin.Application.Contracts.System.Models.Log
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
        public DateTime CreatedOn { get; set; }
    }
}