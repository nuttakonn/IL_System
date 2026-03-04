using ILSCREEN_UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ILSCREEN_UI.Controllers;

public class InterviewController : Controller
{
    public InterviewController()
    {
        ServerInfoModel.ControllerName = "Interview";
    }

    public IActionResult ILUsingCard()
    {
        return View();
    }
}
