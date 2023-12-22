namespace Simple.Admin.Application.Contracts.System.Models.Permission
{
    public class LoginVo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string[] roles { get; set; }

        /// <summary>
        /// 访问token
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// 过期时间 格式yyyy/MM/dd HH:mm:ss
        /// </summary>
        public string expires { get; set; }
    }
}