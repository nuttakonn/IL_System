using ILSCREEN_UI.Models;

namespace ILSCREEN_UI.Services.MicroServices.Interface
{
    public interface IILScreenBizMicroService
    {
        public Task<ApiServiceResponseModel<T>> GetMonitorCaseUnsign<T>(string vendor);
        public Task<ApiServiceResponseModel<T>> GetVendorListMonitorCaseUnsign<T>();
        public Task<ApiServiceResponseModel<T>> GetViewVendorMaster<T>(string searchBy, string searchValue);

    }
}