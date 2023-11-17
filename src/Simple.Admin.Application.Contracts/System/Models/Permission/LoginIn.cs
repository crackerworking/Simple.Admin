namespace Simple.Admin.Application.Contracts.System.Models.Permission
{
    public class LoginIn
    {
        public Guid guid { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string code { get; set; }
    }
}