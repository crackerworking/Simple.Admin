using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Message;
using Mi.Application.Contracts.System.Models.User;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class PersonalController : MiControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        private readonly IMessageService _msgService;

        public PersonalController(IPermissionService permissionService, IUserService userService, IMessageService msgService)
        {
            _permissionService = permissionService;
            _userService = userService;
            _msgService = msgService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<List<PaMenuModel>> GetSiderMenu() => await _permissionService.GetSiderMenuAsync();

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseStructure<UserBaseInfo>> GetUserBaseInfo() => await _userService.GetUserBaseInfoAsync();

        [HttpPost, AuthorizeCode("System:Personal:SetUserBaseInfo")]
        public async Task<ResponseStructure> SetUserBaseInfo([FromBody] UserBaseInfo model) => await _userService.SetUserBaseInfoAsync(model);

        [HttpPost, AuthorizeCode("System:Personal:SetPassword")]
        public async Task<ResponseStructure> SetPassword([FromBody] SetPasswordIn input) => await _userService.SetPasswordAsync(input);

        [HttpGet]
        [AllowAnonymous]
        public async Task<IList<HeaderMsg>> GetHeaderMsg() => await _msgService.GetHeaderMsgAsync();
    }
}