namespace Mi.Application.Contracts.System.Models.Function
{
    public class FuncGroupItem
    {
        public long Id { get; set; }
        public string? GroupName { get; set; }

        public IList<Option>? Functions { get; set; }

        public string? Remark { get; set; }
    }
}