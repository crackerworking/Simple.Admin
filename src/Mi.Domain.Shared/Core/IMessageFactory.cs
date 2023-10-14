namespace Mi.Domain.Shared.Core
{
    /// <summary>
    /// 消息工厂
    /// </summary>
    public interface IMessageFactory
    {
        /// <summary>
        /// 系统消息写入
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="users">接收用户ID</param>
        /// <returns></returns>
        void WriteMessage(string title, string content, IEnumerable<long> users);
    }
}