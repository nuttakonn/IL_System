using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model.NOTEAPI
{
    public class NoteModel
    {
    }
    public class DataAPI
    {
        public string api_name { get; set; }
        public DateTime? start_dt { get; set; }
        public DateTime? ent_dt { get; set; }
        //public Span span { get; set; }
    }

    public class Datum
    {
        public string _id { get; set; }
        public string noteNumber { get; set; }
        public string csnNo { get; set; }
        public string contractNo { get; set; }
        public DateTime? noteDate { get; set; }
        public string actionCode { get; set; }
        public string personCode { get; set; }
        public string resultCode { get; set; }
        public string noteDescription { get; set; }
        public DateTime? ppDate { get; set; }
        public DateTime? pdDate { get; set; }
        public DateTime? recallDate { get; set; }
        public decimal? ppAmt { get; set; }
        public decimal? alreadyPaidAmt { get; set; }
        public string ppChannel { get; set; }
        public string callCenter { get; set; }
        public string telType { get; set; }
        public string telNo { get; set; }
        public string callType { get; set; }
        public string contactTo { get; set; }
        public string noteRemark { get; set; }
        public string systemBy { get; set; }
        public string noteFlag { get; set; }
        public string recordStatus { get; set; }
        public string noteBy { get; set; }
        public string noteByName { get; set; }
        public string refNoteNumber { get; set; }
        public DateTime? createDate { get; set; }
        public string createBy { get; set; }
        public DateTime? updateDate { get; set; }
        public string updateBy { get; set; }
    }

    public class Result
    {
        public List<Datum> data { get; set; }
        public int total { get; set; }
    }

    public class GetTimeline
    {
        public List<DataAPI> dataAPI { get; set; }
        public Result result { get; set; }
    }

    public class ResponseGetnote
    {
        public string M38DAT_ { get; set; }
        public string M38TIM { get; set; }
        public string M38ACD { get; set; }
        public string M38RCD { get; set; }
        public string M38USR { get; set; }
        public string M38DES { get; set; }
    }
    public class Span
    {
        public int? ticks { get; set; }
        public int? days { get; set; }
        public int? hours { get; set; }
        public int? milliseconds { get; set; }
        public int? minutes { get; set; }
        public int? seconds { get; set; }
        public int? totalDays { get; set; }
        public int? totalHours { get; set; }
        public int? totalMilliseconds { get; set; }
        public int? totalMinutes { get; set; }
        public int? totalSeconds { get; set; }
    }
}