using System.Security.Cryptography;
using System.Text;

namespace Simple.Admin.Domain.Helper
{
    /// <summary>
    /// 加密
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// 获取根据盐码加密的密码
        /// </summary>
        /// <param name="password">原密码</param>
        /// <param name="salt">盐码</param>
        /// <returns></returns>
        public static string GenEncodingPassword(string password
            , string salt)
        {
            var bs = Encoding.UTF8.GetBytes(password + salt);
            var hs = MD5.HashData(bs);
            var stringBuilder = new StringBuilder();
            foreach (var item in hs) stringBuilder.Append(item.ToString("x2"));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取新的密码盐码
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordSalt()
        {
            var salt = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            byte[] bytes = encodeType.GetBytes(source);
            string encode;
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            string decode;
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}