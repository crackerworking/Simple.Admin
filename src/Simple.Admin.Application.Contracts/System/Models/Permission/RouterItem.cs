namespace Simple.Admin.Application.Contracts.System.Models.Permission
{
    public class RouterItemMeta
    {
        public string title { get; set; }

        public string? icon { get; set; }

        public int rank { get; set; }

        public string[] roles { get; set; }

        public string[] auths { get; set; }
    }

    public class RouterItem
    {
        public string? path { get; set; }

        public RouterItemMeta meta { get; set; }

        public IList<RouterItem> children { get; set; }
    }
}