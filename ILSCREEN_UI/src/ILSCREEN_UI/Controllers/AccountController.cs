using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using ILSCREEN_UI.Services.MicroServices;
using ILSCREEN_UI.Services.MicroServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ILSCREEN_UI.Controllers;

public class AccountController : Controller
{
    protected readonly IILScreenBizMicroService _microSV;
    public AccountController(ILScreenBizMicroService microSV)
    {
        ServerInfoModel.ControllerName = "Account";
        _microSV = microSV;
    }

    public IActionResult ViewVendorMaster()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetViewVendorMaster(string searchBy, string searchValue)
    {
        ResponseModel returnResponse = new ResponseModel();
        try
        {
            var data = await _microSV.GetViewVendorMaster<ResponseModel>(searchBy, searchValue);
            if (data?.Entity != null || data?.Entity.data != null)
            {
                returnResponse = data.Entity;
                returnResponse.data = data.Entity?.data?.ConvertToModel<List<ViewVendorMasterModel>>() ?? new List<ViewVendorMasterModel>();
            }
            return await Task.FromResult(Ok(returnResponse));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest($"{ex.Message} - {ex.StackTrace}"));
        }
    }
}
