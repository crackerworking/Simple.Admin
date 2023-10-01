using Mi.Domain.Shared.Models.UI;

namespace Mi.Application.Contracts.Public
{
    public interface IPublicService
    {
        Task<PaConfigModel> ReadConfigAsync();

        Task<ResponseStructure> SetUiConfigAsync(SysConfigModel operation);

        Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync();

        Task<bool> WriteMessageAsync(string title, string content, IList<long> receiveUsers);

        ResponseStructure HasPermission(string authCode);

        Task<byte[]> LoginCaptchaAsync();
    }
}