using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using ILSCREEN_UI.Services.MicroServices;
using ILSCREEN_UI.Services.MicroServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ILSCREEN_UI.Controllers;

public class ProcessController : Controller
{
    protected readonly IILScreenBizMicroService _microSV;
    public ProcessController(IILScreenBizMicroService microSV)
    {
        ServerInfoModel.ControllerName = "Process";
        _microSV = microSV;

    }

    public IActionResult MonitorCaseUnsign()
    {
        return View();
    }

    public IActionResult CustomerInquiry()
    {
        return View();
    }

    public IActionResult SignContract()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> GetVendorListMonitorCaseUnsign()
    {
        ResponseModel returnResponse = new ResponseModel();
        try
        {
            var data = await _microSV.GetVendorListMonitorCaseUnsign<ResponseModel>();
            returnResponse = data.Entity;
            returnResponse.data = data.Entity?.data?.ConvertToModel<List<VendorList>>() ?? new List<VendorList>();
            return await Task.FromResult(Ok(returnResponse));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest($"{ex.Message} - {ex.StackTrace}"));
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetMonitorCaseUnsign(string vendor)
    {
        ResponseModel returnResponse = new ResponseModel();
        try
        {
            var data = await _microSV.GetMonitorCaseUnsign<ResponseModel>(vendor);
            returnResponse = data.Entity;
            returnResponse.data = data.Entity?.data?.ConvertToModel<List<MonitorCaseUnsign>>() ?? new List<MonitorCaseUnsign>();
            return await Task.FromResult(Ok(returnResponse));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest($"{ex.Message} - {ex.StackTrace}"));
        }
    }

}
