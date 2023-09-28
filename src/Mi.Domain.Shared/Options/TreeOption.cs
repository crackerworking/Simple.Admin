using Mi.Domain.Shared.Fields;

namespace Mi.Domain.Shared.Options
{
    public class TreeOption : Option, IChildren<IList<TreeOption>>
    {
        public IList<TreeOption>? Children { get; set; }
    }
}