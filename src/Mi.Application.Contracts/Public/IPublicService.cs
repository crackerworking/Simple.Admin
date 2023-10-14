using Mi.Domain.Shared.Models.UI;

namespace Mi.Application.Contracts.Public
{
    public interface IPublicService
    {
        Task<PaConfigModel> ReadConfigAsync();

        Task<ResponseStructure> SetUiConfigAsync(SysConfigModel operation);

        Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync();

        ResponseStructure HasPermission(string authCode);

        Task<byte[]> LoginCaptchaAsync(Guid guid);
    }
}