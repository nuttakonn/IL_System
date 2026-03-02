using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model
{
    public class ResponseServiceModel
    {
 

    }
    public class ResponseModel
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public object data { get; set; }
        public object error { get; set; }
    }
}