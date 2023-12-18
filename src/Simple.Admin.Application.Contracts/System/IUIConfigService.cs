using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Models.UI;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IUIConfigService : IScoped
    {
        /// <summary>
        /// 设置UI配置
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task<MessageModel> SetUiConfigAsync(SysConfigModel operation);

        /// <summary>
        /// 读取UI配置
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<SysConfigModel>> GetUiConfigAsync();
    }
}