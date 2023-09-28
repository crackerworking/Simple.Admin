using System.Diagnostics.CodeAnalysis;

namespace Mi.Application.Contracts.System.Models
{
    public class FuncGroupOperation
    {
        public long GroupId { get; set; }

        /// <summary>
        /// 组名，同名为一组
        /// </summary>
        [NotNull]
        public string? GroupName { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [NotNull]
        public IList<long>? FunctionIds { get; set; }

        public string? Remark { get; set; }
    }
}