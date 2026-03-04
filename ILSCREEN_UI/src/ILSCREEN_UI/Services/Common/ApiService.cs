using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ILSCREEN_UI.Services.Common
{
    public class ApiService
    {
        public string BaseAddress { get; set; }
        public string RequestUri { get; set; }
        public object Parameter { get; set; }
        public bool UseTokenJwt { set; get; }
        public string AccessToken { get; set; }
        public string AccessKey { get; set; }
        public APIServiceMethod Method { set; get; }
        public bool HtmlConvert { set; get; }
        public ApiService(string baseAddress, string requestUri, object parameter, bool useTokenJwt, string accessToken, APIServiceMethod method, string accessKey = null)
        {
            this.BaseAddress = baseAddress;
            this.RequestUri = requestUri;
            this.Parameter = parameter;
            this.UseTokenJwt = useTokenJwt;
            this.Method = method;
            this.AccessToken = accessToken;
            this.AccessKey = accessKey;
        }

        public async Task<ApiServiceResponseModel<T>> CallMicroService<T>(bool isWriteLog, string correlationId)
        {
            ApiServiceResponseModel<T> serviceResult = new ApiServiceResponseModel<T>();
            try
            {
                serviceResult.RequestUri = new Uri(BaseAddress + RequestUri);
                serviceResult.Content = new StringContent(JsonConvert.SerializeObject(Parameter), Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    //string timeout = AppSetting.Configuration?["Timeout"] + "";
                    //client.Timeout = TimeSpan.FromSeconds(Int32.Parse(timeout));

                    if (isWriteLog)
                    {
                        client.DefaultRequestHeaders.Add("_CorrelationId_", $"{correlationId}");
                    }
                    if (UseTokenJwt)
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");
                    }
                    if (AccessKey != null)
                    {
                        client.DefaultRequestHeaders.Add("AccessKey", $"{AccessKey}");
                    }
                    serviceResult.StartWatch();
                    switch (Method)
                    {
                        case APIServiceMethod.GET:
                            serviceResult.ResponseMessage = await client.GetAsync(serviceResult.RequestUri);
                            break;
                        case APIServiceMethod.POST:
                            serviceResult.ResponseMessage = await client.PostAsync(serviceResult.RequestUri, serviceResult.Content);
                            break;
                        case APIServiceMethod.PUT:
                            serviceResult.ResponseMessage = await client.PutAsync(serviceResult.RequestUri, serviceResult.Content);
                            break;
                        case APIServiceMethod.DELETE:
                            serviceResult.ResponseMessage = await client.DeleteAsync(serviceResult.RequestUri);
                            break;
                        default:
                            throw new Exception("Api service method is not match with config");
                    }
                    serviceResult.StopWatch();
                    serviceResult.IsSuccessStatusCode = serviceResult.ResponseMessage.IsSuccessStatusCode;
                    serviceResult.StatusCode = serviceResult.ResponseMessage.StatusCode;

                    if (!serviceResult.IsSuccessStatusCode)
                    {
                        /* remove more error for ui */
                        string responseString = await serviceResult.ResponseMessage.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<T>(responseString);
                        var response = obj?.ConvertToModel<ResponseModel>();
                        serviceResult.ReturnMessage = response?.message ?? "";
                    }
                    else
                    {
                        string responseString = await serviceResult.ResponseMessage.Content.ReadAsStringAsync();
                        if (responseString.IndexOf('\"') == 0) responseString = JsonConvert.DeserializeObject<string>(responseString);
                        serviceResult.Entity = JsonConvert.DeserializeObject<T>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StopWatch();
                serviceResult.IsSuccessStatusCode = false;
                serviceResult.StatusCode = HttpStatusCode.BadRequest;
                serviceResult.ReturnMessage = $"{ex.Message} - {ex.InnerException}";
            }
            return serviceResult;
        }

        public async Task<ResponseModel> Call<T>(bool needValue = false)
        {
            if (this.UseTokenJwt && string.IsNullOrEmpty(this.AccessToken))
            {
                throw new ArgumentException("Not found TokenService for generate Token.");
            }

            var respNew = await CallMicroService<T>(false, "").ConfigureAwait(false);
            var response = respNew.Entity != null ? respNew.Entity.ConvertToModel<ResponseModel>() : new ResponseModel();
            if (response == null)
            {
                if (needValue)
                {
                    throw new ArgumentException("Empty value API: " + this.BaseAddress);
                }
                else
                {
                    response = new ResponseModel();
                }
            }
            if (response.data != null)
            {
                if (this.HtmlConvert)
                {
                    return response;
                }
                response.data = ((JContainer)response.data).ToObject<T>();
            }

            return await Task.FromResult(response);
        }
    }
}
