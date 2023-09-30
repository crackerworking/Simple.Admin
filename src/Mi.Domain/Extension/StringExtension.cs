using System.Text.RegularExpressions;

namespace Mi.Domain.Extension
{
    public static class StringExtension
    {
        public static bool RegexValidate(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (string.IsNullOrEmpty(pattern)) return false;
            return new Regex(pattern).IsMatch(str);
        }
    }
}