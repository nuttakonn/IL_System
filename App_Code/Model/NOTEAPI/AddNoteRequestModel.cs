using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model.NOTEAPI
{
    public class AddNoteRequestModel
    {
        [JsonProperty("csnNo")]
        public string CSNNo { get; set; }

        [JsonProperty("contractNo")]
        public string ContractNo { get; set; }

        [JsonProperty("actionCode")]
        public string ActionCode { get; set; }
        [JsonProperty("NoteDate ")]
        public string NoteDate { get; set; }

        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("noteDescription")]
        public string NoteDescription { get; set; }

        [JsonProperty("noteRemark")]
        public string NoteRemark { get; set; }
        [JsonProperty("SystemBy")]
        public string SystemBy { get; set; }
        [JsonProperty("personCode")]
        public string personCode { get; set; }
        [JsonProperty("noteBy")]
        public string NoteBy { get; set; }

        [JsonProperty("createBy")]
        public string CreateBy { get; set; }
        [JsonProperty("noteByName")]
        public string NoteByName { get; set; }
    }
    public class AddNoteResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }
}