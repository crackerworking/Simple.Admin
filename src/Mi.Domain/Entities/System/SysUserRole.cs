namespace Mi.Domain.Entities.System
{
    /// <summary>
    /// 用户-角色关系
    /// </summary>
    [Table("SysUserRole")]
    public class SysUserRole : EntityBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
    }
}