using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using Simple.Admin.Domain.Exceptions;

namespace Simple.Admin.Domain.Helper
{
    public class StringHelper
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRandomString(int len)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new(GetNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (!reValue.Contains(s1, StringComparison.CurrentCulture)) reValue += s1;
            }
            return reValue;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// mac地址
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FriendlyException"></exception>
        public static string GetMacAddress()
        {
            try
            {
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface network in networks)
                {
                    if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        return network.GetPhysicalAddress().ToString();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new FriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 单用户功能缓存key
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static string UserKey(string userName, string roleName) => $"{userName}_function_{roleName}";

        /// <summary>
        /// 用户Claim.UserData字符串
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static string UserDataString(long id, string userName, string roleName) => $"{id}_{userName}_{roleName.Replace(',', '&')}";

        /// <summary>
        /// 用户Claim.UserData
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static (long, string, string) GetUserData(string str)
        {
            var strArray = str.Split('_');
            return (long.Parse(strArray[0]), strArray[1], strArray[2]);
        }

        /// <summary>
        /// 用户功能缓存key正则匹配
        /// </summary>
        /// <returns></returns>
        public static string UserFunctionCachePattern() => "\\.*(_function_){1}\\.*";

        /// <summary>
        /// 默认头像地址
        /// </summary>
        /// <returns></returns>
        public static string DefaultAvatar()
        {
            return "/admin/images/avatar.jpg";
        }
    }
}