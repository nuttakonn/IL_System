using ILSCREEN_UI.Models;

namespace ILSCREEN_UI.Services.MicroServices.Interface
{
    public interface ITokenMicroService
    {
        Task<ApiServiceResponseModel<T>> GenerateNewToken<T>(string correlationId, string issuer);
        Task<string> GetToken(string correlationId, string issuer);
        bool UseAuthen();
    }
}
