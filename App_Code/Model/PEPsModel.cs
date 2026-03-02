using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model
{
    public class PEPsModel
    {
        public class ResponseModel
        {
            public int status { get; set; }
            public bool success { get; set; }
            public string message { get; set; }
            public PepModel data { get; set; }
        }

        public class RequestPEPsModel
        {
            public string firstNameTH { get; set; }
            public string lastNameTH { get; set; }
            public string firstNameEN { get; set; }
            public string lastNameEN { get; set; }
        }

        public class PepModel
        {
            public bool isPEPs { get; set; }
            public int UID { get; set; }
        }
    }
}