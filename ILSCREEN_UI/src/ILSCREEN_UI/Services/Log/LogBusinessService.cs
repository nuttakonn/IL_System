using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using ILSCREEN_UI.Services.Common;
using System.Net;

namespace ILSCREEN_UI.Services.Log
{
    public class LogBusinessService
    {
        protected bool _isEnable;
        public LogBusinessService()
        {
            _isEnable = bool.Parse(AppSetting.Configuration?["Logging:LogEnable"] ?? "false");
        }
        public string GetMicroServiceHost()
        {
            return AppSetting.MicroService("ILSCREEN_LOG_API");
        }

        public async Task InsertItem(LogApiModel entity, RefType requestORresponse)
        {
            try
            {
                if (!_isEnable) return;
                string url = string.Empty;
                ResponseModel response;
                if (requestORresponse == RefType.REQUEST) url = "/api/LogAPI/AddLogRequest";
                else if (requestORresponse == RefType.RESPONSE) url = "/api/LogAPI/AddLogResponse";

                ApiService apiService = new ApiService(GetMicroServiceHost(), url, entity, false, string.Empty, APIServiceMethod.POST);
                response = await apiService.Call<object>();
                if (!response.success)
                {
                    throw new ArgumentNullException($"ไม่สามารถเชื่อต่อ API " + "ILSCREEN_LOG_API" + " ได้ กรุณาติดต่อผู้ดูแลระบบ");
                }
            }
            catch
            {
                //
            }
        }

        public async Task InsertLogRequest(string correlationId, string refId, string requestMethod, string requestUrl, string requestBody, string createBy)
        {
            var logRequest = new LogApiModel()
            {
                correlationId = correlationId,
                refId = refId,
                refType = RefType.REQUEST.ToString(),
                requestMethod = requestMethod,
                requestUrl = requestUrl,
                requestBody = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ? requestBody.PBKDF2Encrypt() : requestBody,
                ipAddress = Dns.GetHostName(),
                createBy = createBy,
                createDate = DateTime.Now
            };

            await InsertItem(logRequest, RefType.REQUEST);
        }

        public async Task InsertLogResponse(string correlationId, string refId, int responseStatusCode, string responseMessage, string createBy)
        {
            var logResponse = new LogApiModel()
            {
                correlationId = correlationId,
                refId = refId,
                refType = RefType.RESPONSE.ToString(),
                responseStatusCode = responseStatusCode,
                responseMessage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ? responseMessage.PBKDF2Encrypt() : responseMessage,
                ipAddress = Dns.GetHostName(),
                createBy = createBy,
                createDate = DateTime.Now
            };

            await InsertItem(logResponse, RefType.RESPONSE);
        }
    }
}