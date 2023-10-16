namespace Mi.Domain.Entities.System
{
    public class sys_task
    {
        /// <summary>
        /// ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 任务唯一标识
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string task_name { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string task_desc { get; set; }

        /// <summary>
        /// 是否启用 1启用 0禁用
        /// </summary>
        public int is_enabled { get; set; }

        /// <summary>
        /// 额外参数
        /// </summary>
        public string extra_params { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        public string cron { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? exec_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public long? update_user { get; set; }
    }
}