using System.Security.Claims;

namespace Simple.Admin.ControllerLibrary.Account
{
    [Authorize]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        [HttpGet]
        [Route("/refreshtoken")]
        public MessageModel Get()
        {
            var str = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (str != null && str.Contains("refresh-token"))
            {
            }
            return new MessageModel(response_type.Fail, "无法获取刷新token");
        }
    }
}