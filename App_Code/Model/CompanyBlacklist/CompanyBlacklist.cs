using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model.CompanyBlacklist
{
    public class CompanyBlacklist
    {
        public class responseCompanyBlacklist
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
            public CompanyStatus data { get; set; }
        }

        public class responseDDCompanyBlacklist
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
            public List<DropDownList> Content { get; set; }
        }
        public class CompanyStatus
        {
            public string CompanyNameTH { get; set; }
            public string CompanyNameEN { get; set; }
            public string CompanyFlag { get; set; }
            public string FlagDescription { get; set; }
            public ExceptionError ExError { get; set; }
        }
        public class ExceptionError
        {
            public string ErrorFlag { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorDescription { get; set; }
        }
        public class DropDownList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}