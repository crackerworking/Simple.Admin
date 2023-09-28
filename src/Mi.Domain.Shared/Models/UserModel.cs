namespace Mi.Domain.Shared.Models
{
    public class UserModel
    {
        public bool IsSuperAdmin { get; set; }

        public long UserId { get; set; }
        public string UserName { get; set; }

        public string Roles { get; set; }

        public IList<PowerItem>? PowerItems { get; set; }

        public UserModel()
        {
            UserId = -1;
            UserName = "System";
            Roles = "";
        }
    }

    public class PowerItem
    {
        public long Id { get; set; }

        public string? Url { get; set; }

        public string? AuthCode { get; set; }
    }
}