namespace Mi.Domain.Extension
{
    public static class CheckerExtension
    {
        // ============== array ==============

        public static bool IsNull<T>(this IEnumerable<T>? arr)
        {
            return arr == null || !arr.Any();
        }

        // ============== string ==============

        public static bool IsNull(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}