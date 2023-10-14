namespace Mi.Domain.Shared.Models
{
    public class PrimaryKey
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long id { get; set; }
    }

    public class PrimaryKeys
    {
        /// <summary>
        /// 主键ID，数组
        /// </summary>
        public long[] array_id { get; set; }
    }
}