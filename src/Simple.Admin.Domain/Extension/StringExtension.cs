using System.Text.RegularExpressions;

namespace Simple.Admin.Domain.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool RegexValidate(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(pattern)) return false;
            return new Regex(pattern).IsMatch(str);
        }
    }
}