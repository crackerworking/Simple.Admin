namespace Mi.Application.Contracts.System.Models.Dict
{
    public class DictItem : SysDictFull
    {
        public int ChildCount { get; set; }

        public string? ParentName { get; set; }
    }
}