namespace Mi.Domain.Shared.Models
{
    public class QuerySortField
    {
        /// <summary>
        /// 排序方式 true倒序 false倒序
        /// </summary>
        public bool Desc { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string FieldName { get; set; }
    }
}