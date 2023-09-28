namespace Mi.Application.Contracts.System.Models.Result
{
    public class DictItem : SysDictFull
    {
        public int ChildCount { get; set; }

        public string? ParentName { get; set; }
    }
}