using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

namespace Mi.Application.Contracts.System
{
    public interface IUIConfigService : IScoped
    {
        /// <summary>
        /// 设置UI配置
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task<ResponseStructure> SetUiConfigAsync(SysConfigModel operation);

        /// <summary>
        /// 读取UI配置
        /// </summary>
        /// <returns></returns>
        Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync();
    }
}