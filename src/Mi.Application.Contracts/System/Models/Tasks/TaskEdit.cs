using System.ComponentModel.DataAnnotations;

namespace Mi.Application.Contracts.System.Models.Tasks
{
    public class TaskEdit
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 是否启用 1启用 0禁用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 额外参数
        /// </summary>
        public string? ExtraParams { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        [Required]
        public string Cron { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string? Remark { get; set; }
    }
}