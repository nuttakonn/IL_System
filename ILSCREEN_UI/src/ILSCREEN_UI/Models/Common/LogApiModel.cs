using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILSCREEN_UI.Models
{
    //[Table("GenPortLogApi")]
    public class LogApiModel
    {
        [Key]
        [Column("INF_ID")]
        public int id { get; set; }
        public string correlationId { get; set; }
        public string refId { get; set; }
        public string refType { get; set; }
        public string requestMethod { get; set; }
        public string requestUrl { get; set; }
        public string requestBody { get; set; }
        public int responseStatusCode { get; set; }
        public string responseMessage { get; set; }
        public string ipAddress { get; set; }
        public string createBy { get; set; }
        public DateTime? createDate { get; set; }
    }
}
