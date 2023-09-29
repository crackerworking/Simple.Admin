namespace Mi.Domain.Shared.GlobalVars
{
    public static class CacheConst
    {
        public const string DICT = "all_dict";

        public const string FUNCTION = "all_function";

        public static readonly TimeSpan Week = TimeSpan.FromDays(7);

        public static readonly TimeSpan Year = TimeSpan.FromDays(366);

        public static readonly TimeSpan WxWorkTokenExpire = TimeSpan.FromSeconds(7190);

        public static readonly string CONTROLLER_TYPES = nameof(CONTROLLER_TYPES).ToLower();
    }
}