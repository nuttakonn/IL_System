namespace ILSCREEN_UI.Models
{
    public class InputILUsingSpeedTimeReportModel
    {
        public string? dateFrom { get; set; }
        public string? dateTo { get; set; }
        public string? branch { get; set; }
        public string? applicationType { get; set; }
        public string? user { get; set; }
        public string? department { get; set; }
    }

    public class ResultILUsingSpeedTimeReportModel
    {
        public DateTime? applicationDate { get; set; }
        public string? name { get; set; }
        public string? vendorID { get; set; }
        public string? contractNo { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
        public string? speedTime { get; set; }
        public string? user { get; set; }
    }
}
