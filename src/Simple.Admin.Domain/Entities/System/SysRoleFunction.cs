namespace Simple.Admin.Domain.Entities.System
{
    [Table("SysRoleFunction")]
    public class SysRoleFunction : EntityBase
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public long FunctionId { get; set; }
    }
}