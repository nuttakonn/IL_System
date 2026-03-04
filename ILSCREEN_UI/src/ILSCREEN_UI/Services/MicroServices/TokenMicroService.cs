using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using ILSCREEN_UI.Services.Common;
using ILSCREEN_UI.Services.Log;
using ILSCREEN_UI.Services.MicroServices.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ILSCREEN_UI.Services.MicroServices
{
    public class TokenMicroService : ITokenMicroService
    {
        protected readonly LogBusinessService _logService;
        protected readonly Jwt _jwtConfig;
        protected readonly IHttpContextAccessor _context;


        public TokenMicroService(LogBusinessService logService, IOptions<MicroServicesModel> msConfig, IOptions<Jwt> jwtConfig, IHttpContextAccessor context)
        {
            _jwtConfig = AppSetting.Configuration?.GetSection("Jwt")?.Get<Jwt>() ?? new Jwt();
            _logService = logService;
            _context = context;
        }

        public string GetMicroServiceHost()
        {
            return AppSetting.MicroService("TOKEN_API");
        }

        public bool UseAuthen()
        {
            return this._jwtConfig.Enable;
        }

        public async Task<ApiServiceResponseModel<T>> GenerateNewToken<T>(string correlationId, string issuer)
        {
            string refId = Guid.NewGuid().ToString();
            string url = "/api/Token/BuildToken";
            var requestToken = new RequestTokenModel { userName = _jwtConfig.Username, password = _jwtConfig.Password, issuer = issuer, expiresMinutes = int.Parse(_jwtConfig.ExpireMinutes ?? "3"), role = _jwtConfig.Role };
            ApiService apiService = new ApiService(GetMicroServiceHost(), url, requestToken, false, string.Empty, APIServiceMethod.POST, "");
            // Log Request
            string requestUrl = new Uri(new Uri(apiService.BaseAddress), apiService.RequestUri).AbsoluteUri;
            string requestBody = JsonConvert.SerializeObject(requestToken);
            string requestMethod = APIServiceMethod.POST.ToString();

            new Thread(() => _ = _logService.InsertLogRequest(correlationId, refId, requestMethod, requestUrl, requestBody, "UI")).Start();

            // Call MicroService
            ApiServiceResponseModel<T> serviceResult = await apiService.CallMicroService<T>(false, refId);

            // Log Response
            int responseStatusCode = (int)serviceResult.StatusCode;
            string responseMessage = "";
            /* remove more error for ui */
            if (!serviceResult.IsSuccessStatusCode)
            {
                responseMessage = $"Service:{serviceResult.RequestUri} | Message:[{(int)serviceResult.StatusCode} {serviceResult.StatusCode.ToString()}] : {serviceResult.ReturnMessage}";
            }
            else
            {
                responseMessage = await serviceResult.ResponseMessage.Content.ReadAsStringAsync();
            }

            new Thread(() => _ = _logService.InsertLogResponse(correlationId, refId, responseStatusCode, responseMessage, "UI")).Start();

            return serviceResult;
        }

        public async Task<string> GetToken(string correlationId, string issuer)
        {
            //TokenModel token = await RequestNewToken(correlationId, issuer);
            //return token.token;
            return await Task.FromResult(RequestNewToken(correlationId, issuer).Result.token);
        }

        private async Task<TokenModel> RequestNewToken(string correlationId, string issuer)
        {
            var serviceResult = await GenerateNewToken<ResponseModel>(correlationId, issuer);
            if (!serviceResult.IsSuccessStatusCode) { throw new ArgumentNullException(serviceResult.ReturnMessage); }
            if (!serviceResult.Entity.success) { throw new ArgumentNullException($"Service : {serviceResult.RequestUri} | Message : {serviceResult.Entity.message}"); }
            return await Task.FromResult(serviceResult.Entity.data != null ? serviceResult.Entity.data.ConvertToModel<TokenModel>() : new TokenModel());
        }
    }
}