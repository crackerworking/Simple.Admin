namespace Simple.Admin.Domain.Entities
{
    public interface IEntityBase
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        long CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// 上次更新用户
        /// </summary>
        long? ModifiedBy { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// 已删除1 正常0
        /// </summary>
        int IsDeleted { get; set; }
    }
}