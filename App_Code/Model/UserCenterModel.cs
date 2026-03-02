using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserCenterModel
{
    public class RequestUserCenterModel
    {
        public int appID { get; set; }
        public string appType { get; set; }
        public int systemID { get; set; }
    }

    public class ResultGetUserCenterModel
    {
        public int responseCode { get; set; }
        public string responseMSG { get; set; }
        public bool isError { get; set; }
        public object data { get; set; }
        public object error { get; set; }
    }

    public class RequestTokenModel
    {
        public string userName { set; get; }
        public string password { set; get; }
        public string role { set; get; }
        public string issuer { set; get; }
        public int expiresMinutes { set; get; }
    }

    public class ResponseTokenModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public TokenModel data { get; set; }
    }

    public class TokenModel
    {
        public string token { get; set; }
    }
}