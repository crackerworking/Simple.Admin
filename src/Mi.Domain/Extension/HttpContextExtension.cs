using System.Security.Claims;

using Mi.Domain.Helper;
using Mi.Domain.Shared.Models;

using Microsoft.AspNetCore.Http;

namespace Mi.Domain.Extension
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取UserModel
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static UserModel GetUser(this HttpContext? context)
        {
            if (context == null) return new UserModel();
            return context.Features.Get<UserModel>() ?? new UserModel();
        }

        /// <summary>
        /// 用户UserKey
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserKey(this HttpContext? context)
        {
            var userData = context.GetUserData();
            if (!string.IsNullOrWhiteSpace(userData))
            {
                var arr = StringHelper.GetUserData(userData);
                return StringHelper.UserKey(arr.Item2, arr.Item3);
            }

            return "";
        }

        /// <summary>
        /// 获取UserData
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserData(this HttpContext? context)
        {
            if (context == null) return "";
            return context.User.FindFirst(ClaimTypes.UserData)?.Value ?? "";
        }
    }
}