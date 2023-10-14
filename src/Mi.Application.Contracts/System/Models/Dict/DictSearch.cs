namespace Mi.Application.Contracts.System.Models.Dict
{
    public class DictSearch : PagingSearchModel, IRemark
    {
        /// <summary>
        /// Name/Key
        /// </summary>
        public string? Vague { get; set; }

        public string? Remark { get; set; }

        public long? ParentId { get; set; }
    }
}