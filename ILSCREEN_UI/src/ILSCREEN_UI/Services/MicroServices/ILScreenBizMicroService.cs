using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using ILSCREEN_UI.Services.Common;
using ILSCREEN_UI.Services.Log;
using ILSCREEN_UI.Services.MicroServices.Interface;
using Newtonsoft.Json;

namespace ILSCREEN_UI.Services.MicroServices
{
    public partial class ILScreenBizMicroService : IILScreenBizMicroService
    {
        protected readonly ITokenMicroService _tokenService;
        protected readonly LogBusinessService _logService;
        protected readonly IHttpContextAccessor _context;
        public ILScreenBizMicroService(ITokenMicroService tokenService, IHttpContextAccessor context, LogBusinessService logService)
        {
            _tokenService = tokenService;
            _logService = logService;
            _context = context;
        }

        public string GetMicroServiceHost()
        {
            return AppSetting.MicroService("ILSCREEN_BIZ_API");
        }

        private async Task<ApiServiceResponseModel<T>> Send<T>(string url, object parameter, APIServiceMethod method)
        {
            await Task.Delay(1);
            ApiServiceResponseModel<T> serviceResult;
            string correlationId = Guid.NewGuid().ToString();
            string refId = Guid.NewGuid().ToString();
            string accessKey = _context.HttpContext?.Request.Cookies["k"] + "";

            if (_tokenService.UseAuthen())
            {
                bool useTokenJwt = _tokenService.UseAuthen();
                string issuer = AppSetting.Configuration?["Jwt:Issuer"] ?? "";
                string accessToken = await _tokenService.GetToken(correlationId, issuer);
                ApiService apiService = new ApiService(GetMicroServiceHost(), url, parameter, useTokenJwt, accessToken, method, accessKey);

                // Log Request
                string requestUrl = new Uri(new Uri(apiService.BaseAddress), apiService.RequestUri).AbsoluteUri;
                string requestBody = JsonConvert.SerializeObject(parameter);
                string requestMethod = method.ToString();
                new Thread(() => _ = _logService.InsertLogRequest(correlationId, refId, requestMethod, requestUrl, requestBody, "UI")).Start();

                // Call MicroService
                serviceResult = await apiService.CallMicroService<T>(true, correlationId);

                // Log Response
                int responseStatusCode = (int)serviceResult.StatusCode;
                string responseMessage = "";

                if (!serviceResult.IsSuccessStatusCode)
                {
                    responseMessage = $"Service:{serviceResult.RequestUri} | Message:[{(int)serviceResult.StatusCode} {serviceResult.StatusCode}] : {serviceResult.ReturnMessage}";
                }
                else
                {
                    responseMessage = await serviceResult.ResponseMessage.Content.ReadAsStringAsync();
                }

                new Thread(() => _ = _logService.InsertLogResponse(correlationId, refId, responseStatusCode, responseMessage, "UI")).Start();
            }
            else
            {
                ApiService apiService = new ApiService(GetMicroServiceHost(), url, parameter, false, "", method, accessKey);

                // Log Request
                string requestUrl = new Uri(new Uri(apiService.BaseAddress), apiService.RequestUri).AbsoluteUri;
                string requestBody = JsonConvert.SerializeObject(parameter);
                string requestMethod = method.ToString();
                new Thread(() => _ = _logService.InsertLogRequest(correlationId, refId, requestMethod, requestUrl, requestBody, "UI")).Start();

                // Call MicroService
                serviceResult = await apiService.CallMicroService<T>(true, correlationId);

                // Log Response
                int responseStatusCode = (int)serviceResult.StatusCode;
                string responseMessage = "";

                /* remove more error for ui */
                if (!serviceResult.IsSuccessStatusCode)
                {
                    responseMessage = $"Service:{serviceResult.RequestUri} | Message:[{(int)serviceResult.StatusCode} {serviceResult.StatusCode}] : {serviceResult.ReturnMessage}";
                }
                else
                {
                    responseMessage = await serviceResult.ResponseMessage.Content.ReadAsStringAsync();
                }

                new Thread(() => _ = _logService.InsertLogResponse(correlationId, refId, responseStatusCode, responseMessage, "UI")).Start();
            }
            return serviceResult;
        }

        public async Task<ApiServiceResponseModel<T>> GetMonitorCaseUnsign<T>(string vendor)
        {
            string url = $"/api/Process/GetMonitorCaseUnsign?vendor={vendor}";
            return await Send<T>(url, null, APIServiceMethod.GET);
        }
        public async Task<ApiServiceResponseModel<T>> GetVendorListMonitorCaseUnsign<T>()
        {
            string url = $"/api/Process/GetVendorListMonitorCaseUnsign";
            return await Send<T>(url, null, APIServiceMethod.GET);
        }
        public async Task<ApiServiceResponseModel<T>> GetViewVendorMaster<T>(string searchBy, string searchValue)
        {
            string url = $"/api/Account/GetViewVendorMaster?searchBy={searchBy}&searchValue={searchValue}";
            return await Send<T>(url, null, APIServiceMethod.GET);
        }
    }
}
