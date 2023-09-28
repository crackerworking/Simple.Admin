namespace Mi.Domain.Entities
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public long Id { get; set; }

        [DefaultValue(-1)]
        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [DefaultValue(0)]
        public int IsDeleted { get; set; }
    }
}