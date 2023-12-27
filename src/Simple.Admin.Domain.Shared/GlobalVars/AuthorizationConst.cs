namespace Simple.Admin.Domain.Shared.GlobalVars
{
    public static class AuthorizationConst
    {
        /// <summary>
        /// 登录策略名
        /// </summary>
        public const string BASE_LOGIN = "base_login";

        /// <summary>
        /// 超级管理员角色名(系统内置)
        /// </summary>
        public const string ADMIN = "admin";

        /// <summary>
        /// 刷新token claimType
        /// </summary>
        public const string REFRESH_TYPE = "refresh_flag";

        /// <summary>
        /// 刷新token标识
        /// </summary>
        public const string REFRESH_TOKEN = "refresh_token";
    }
}