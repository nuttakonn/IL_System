using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace ILSCREEN_UI.Controllers;

public class ReportController : Controller
{
    protected static List<ResultILUsingSpeedTimeReportModel>? _mockResultILUsingSpeedTimeReport { get; set; }
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ReportController(IWebHostEnvironment webHostEnvironment)
    {
        ServerInfoModel.ControllerName = "Account";
        _mockResultILUsingSpeedTimeReport = MockDataILUsingSpeedTimeReport();
        _webHostEnvironment = webHostEnvironment;
    }

    private List<ResultILUsingSpeedTimeReportModel> MockDataILUsingSpeedTimeReport(int rows = 100)
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;

        var result = new List<ResultILUsingSpeedTimeReportModel>();
        ResultILUsingSpeedTimeReportModel row;
        for (int i = 1; i <= rows; i++)
        {
            row = new ResultILUsingSpeedTimeReportModel();
            row.applicationDate = start.AddDays(gen.Next(range)).AddHours(gen.Next(01, 23)).AddMinutes(gen.Next(60)).AddSeconds(gen.Next(60));
            row.name = i.ToString() + " Employee";
            row.vendorID = gen.Next(9999999).ToString();
            row.contractNo = gen.Next(999999999).ToString();
            row.startTime = DateTime.Now.AddMinutes(-10).ToString("hh:mm:ss");
            row.endTime = DateTime.Now.AddMinutes(+256).ToString("hh:mm:ss");
            row.speedTime = DateTime.Now.ToString("mm:ss");
            row.user = "PTEST" + i;
            result.Add(row);
        }

        return result;
    }

    public IActionResult ILUsingSpeedTimeReport()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ILUsingSpeedTimeReportExport()
    {
        var inputDateFrom = HttpContext.Request.Form["InputDateFrom"];
        var inputDateTo = HttpContext.Request.Form["InputDateTo"];
        var selectBranch = HttpContext.Request.Form["SelectBranch"];
        var selectApplicationType = HttpContext.Request.Form["SelectApplicationType"];
        var inputUser = HttpContext.Request.Form["InputUser"];
        var selectDepartment = HttpContext.Request.Form["SelectDepartment"];
        var inputMethod = HttpContext.Request.Form["InputTypeFile"];
        var inputData = new InputILUsingSpeedTimeReportModel { dateFrom = !string.IsNullOrEmpty(inputDateFrom) ? inputDateFrom : DateTime.Now.ToString("dd/MM/yyyy"), dateTo = !string.IsNullOrEmpty(inputDateTo) ? inputDateTo : DateTime.Now.ToString("dd/MM/yyyy"), branch = selectBranch, applicationType = selectApplicationType, user = inputUser, department = selectDepartment };
        var resultData = ILUsingSpeedTimeReportSearching(inputData);

        if (inputMethod.ToString()?.ToUpper() == "PDF")
        {
            return ILUsingSpeedTimeReportPDF(inputData, resultData);
        }
        else
        {
            return ILUsingSpeedTimeReportCSV(resultData);
        }
    }

    private IActionResult ILUsingSpeedTimeReportPDF(InputILUsingSpeedTimeReportModel requestInput, List<ResultILUsingSpeedTimeReportModel>? requestData)
    {
        //var dt = Extensions.ConvertToModel<DataTable>(requestData);

        Microsoft.Reporting.NETCore.LocalReport report = new Microsoft.Reporting.NETCore.LocalReport();
        report.ReportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\ILUsingSpeedTimeReport.rdlc";
        report.SetParameters(new[] { new Microsoft.Reporting.NETCore.ReportParameter("contractDateFrom", requestInput.dateFrom),
                                                new Microsoft.Reporting.NETCore.ReportParameter("contractDateTo", requestInput.dateTo),
                                                new Microsoft.Reporting.NETCore.ReportParameter("username", "TEST01"),
                                                new Microsoft.Reporting.NETCore.ReportParameter("datenow", DateTime.Now.ToString()),
                                                new Microsoft.Reporting.NETCore.ReportParameter("businessName", "IL")});

        report.DataSources.Add(new Microsoft.Reporting.NETCore.ReportDataSource("dsData", requestData));
        byte[] pdf = report.Render("PDF");
        return File(pdf, "application/pdf");
    }

    private IActionResult ILUsingSpeedTimeReportCSV(List<ResultILUsingSpeedTimeReportModel>? requestData)
    {
        var users = Extensions.ConvertToModel<DataTable>(requestData);
        var sb = new StringBuilder();
        sb.AppendLine("No,Application Date,Name,Vendor ID,Contract No,Start Time,End Time,Speed Time,User");

        for (int i = 0; i < users.Rows.Count; i++)
        {
            var applicationDate = users.Rows[i]["applicationDate"].ToString();
            var name = users.Rows[i]["name"].ToString();
            var vendorID = users.Rows[i]["vendorID"].ToString();
            var contractNo = users.Rows[i]["contractNo"].ToString();
            var startTime = users.Rows[i]["startTime"].ToString();
            var endTime = users.Rows[i]["endTime"].ToString();
            var speedTime = users.Rows[i]["endTime"].ToString();
            var user = users.Rows[i]["user"].ToString();
            sb.AppendLine($"{i + 1},{applicationDate},{name},{vendorID},{contractNo},{startTime},{endTime},{speedTime},{user}");
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ILUsingSpeedTimeReport.csv");
    }

    [HttpPost]
    public List<ResultILUsingSpeedTimeReportModel> ILUsingSpeedTimeReportSearching([FromBody] InputILUsingSpeedTimeReportModel input)
    {
        List<ResultILUsingSpeedTimeReportModel> dataWhere = _mockResultILUsingSpeedTimeReport ?? new List<ResultILUsingSpeedTimeReportModel>();

        if (!string.IsNullOrEmpty(input.dateFrom) && !string.IsNullOrEmpty(input.dateTo))
        {
            dataWhere = dataWhere.Where(x => x.applicationDate >= Convert.ToDateTime(input.dateFrom, new System.Globalization.CultureInfo("en-US", false)) && x.applicationDate <= Convert.ToDateTime(input.dateTo, new System.Globalization.CultureInfo("en-US", false))).ToList();
        }

        if (!string.IsNullOrEmpty(input.user))
        {
            //dataWhere = dataWhere.Where(x => (x.user ?? "").ToUpper().Equals(input.user)).ToList();
            dataWhere = dataWhere.Where(x => input.user.ToUpper().Equals(x.user)).ToList();
        }

        return dataWhere.ToList();
    }
}
