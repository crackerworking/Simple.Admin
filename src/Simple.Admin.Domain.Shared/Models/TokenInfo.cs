namespace Simple.Admin.Domain.Shared.Models
{
    public class TokenInfo
    {
        public string token { get; set; }

        public DateTime expire { get; set; }
    }
}