namespace Simple.Admin.Domain.Entities.System
{
    [Table("SysMessage")]
    public class SysMessage : EntityBase
    {
        /// <summary>
        /// 1已读 0未读
        /// </summary>
        [NotNull]
        [DefaultValue(0)]
        public int Readed { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [NotNull]
        public string? Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [NotNull]
        public string? Content { get; set; }

        /// <summary>
        /// 接收用户
        /// </summary>
        public long ReceiveUser { get; set; }
    }
}