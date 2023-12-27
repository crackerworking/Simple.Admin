namespace Simple.Admin.Application.Contracts.System.Models.Message
{
    public class HeaderMsg
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public IList<HeaderMsgChild>? Children { get; set; }
    }

    public class HeaderMsgChild
    {
        public long Id { get; set; }
        public string? Avatar { get; set; } = "https://gw.alipayobjects.com/zos/rmsportal/ThXAXghbEsBCCSDihZxY.png";
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Form { get; set; } = "系统";
        public string? Time { get; set; }
    }
}