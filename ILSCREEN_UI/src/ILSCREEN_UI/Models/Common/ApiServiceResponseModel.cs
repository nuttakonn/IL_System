using System.Net;

namespace ILSCREEN_UI.Models
{
    public class ApiServiceResponseModel<T>
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalMilliseconds
        {
            get
            {
                return ((EndTime != null && StartTime != null) && (EndTime >= StartTime)) ? (EndTime - StartTime).TotalMilliseconds : 0;
            }
        }
        public double TotalSeconds
        {
            get
            {
                return ((EndTime != null && StartTime != null) && (EndTime >= StartTime)) ? (EndTime - StartTime).TotalSeconds : 0;
            }
        }
        public double TotalMinutes
        {
            get
            {
                return ((EndTime != null && StartTime != null) && (EndTime >= StartTime)) ? (EndTime - StartTime).TotalMinutes : 0;
            }
        }
        public long Ticks
        {
            get
            {
                return ((EndTime != null && StartTime != null) && (EndTime >= StartTime)) ? (EndTime - StartTime).Ticks : 0;
            }
        }
        public Uri RequestUri { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        public StringContent Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReturnMessage { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        public T Entity;

        public ApiServiceResponseModel()
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            IsSuccessStatusCode = false;
        }

        public void StartWatch()
        {
            this.StartTime = DateTime.Now;
        }

        public void StopWatch()
        {
            this.EndTime = DateTime.Now;
        }
    }
}
