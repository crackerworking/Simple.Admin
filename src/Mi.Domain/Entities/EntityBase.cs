namespace Mi.Domain.Entities
{
    public class EntityBase : IEntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DefaultValue(-1)]
        public long CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public long? ModifiedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [DefaultValue(0)]
        public int IsDeleted { get; set; }
    }
}