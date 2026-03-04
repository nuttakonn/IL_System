using ILSCREEN_UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ILSCREEN_UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        ServerInfoModel.ControllerName = "Home";
    }

    public IActionResult Index()
    {
        var xx = AppSetting.MicroServices();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Dashboard1()
    {
        return View();
    }

    public IActionResult Dashboard2()
    {
        return View();
    }

    public IActionResult Dashboard3()
    {
        return View();
    }

    public List<object> GetMenuAuthen()
    {
        var listMenu = new List<object>()
        {
            new { nameMenu = "INTERVIEW", iconMenu = "nav-icon bi bi-box-seam-fill", subMenu = new List<object>() { new { nameMenu = "IL Using Card", urlMenu = "/interview/ILUsingCard" } } },
            new { nameMenu = "PROCESS", iconMenu = "nav-icon bi bi-speedometer", subMenu = new List<object>() { new { nameMenu = "Customer Inquiry", urlMenu = "/process/CustomerInquiry" }, new { nameMenu = "Sign Contract", urlMenu = "/process/SignContract" }, new { nameMenu = "Monitor Case Unsign", urlMenu = "/process/MonitorCaseUnsign" } } },
            new { nameMenu = "ACCOUNT", iconMenu = "nav-icon bi bi-tree-fill", subMenu = new List<object>() { new { nameMenu = "View Vendor Master", urlMenu = "/account/ViewVendorMaster" } } },
            new { nameMenu = "REPORT", iconMenu = "nav-icon bi bi-tree-fill", subMenu = new List<object>() { new { nameMenu = "IL Using Speed Time Report", urlMenu = "/report/ILUsingSpeedTimeReport" } } },
        };

        return listMenu;
        //return await Task.FromResult(listMenu);
    }
}
