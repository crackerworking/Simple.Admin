using Mi.Domain.Shared.Fields;

namespace Mi.Domain.Shared.Options
{
    public class TreeOption : Option, IChildren<IList<TreeOption>>
    {
        /// <summary>
        /// 子集
        /// </summary>
        public IList<TreeOption>? Children { get; set; }
    }
}