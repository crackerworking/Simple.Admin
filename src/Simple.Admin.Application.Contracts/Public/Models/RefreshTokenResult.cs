namespace Simple.Admin.Application.Contracts.Public.Models
{
    public class RefreshTokenResult
    {
        /// <summary>
        /// 访问token
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// 格式'xxxx/xx/xx xx:xx:xx'
        /// </summary>
        public string expires { get; set; }
    }
}