using Simple.Admin.Domain.Shared.Fields;

namespace Simple.Admin.Domain.Shared.Options
{
    public class TreeOption : Option, IChildren<IList<TreeOption>>
    {
        /// <summary>
        /// 子集
        /// </summary>
        public IList<TreeOption>? Children { get; set; }
    }
}