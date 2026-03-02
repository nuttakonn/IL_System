using EB_Service.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace ILSystem.App_Code.Helper
{
    public class connectAPI
    {
        private static WebClient client = new WebClient();
        //public static SecurityProtocolType SecurityProtocol { get; set; }
        public class connectAPIHelper
        {

        }
        public string getWebApi(string apiPath, string controller, string service, string[] parammeters)
        {
            // Set the header so it knows we are sending JSON
            client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";

            var url = "";

            if (apiPath == "apiCompanyBlacklist")
            {
                var path = WebConfigurationManager.AppSettings[apiPath].ToString();

                if (service == "getCompanyBlacklistDropDownList")
                {
                    url = getCompanyBlacklistDropDownListPath(path, controller, service, parammeters);
                }
                else
                {
                    //var url = path + "/" + controller + "/" + service + "?paramCompanyName=" + parammeters[0];
                    url = path + "/" + controller + "/" + service;
                    if (parammeters != null && parammeters.Count() > 0)
                    {
                        foreach (var param in parammeters)
                        {
                            char end = param[param.Length - 1];
                            if (end == '.')
                            {
                                url = url + "/" + param + " ";
                            }
                            else
                            {
                                url = url + "/" + param;
                            }
                        }
                    }
                }
            }
            else
            {
                url = PathAPI(apiPath, controller, service, parammeters);
            }
            //Utility.WriteLogResponse("Request ", url.ToString());
            // Deserialise the response into a GUID
            var response = client.DownloadString(url);
            //Utility.WriteLogResponse("Response ", JsonConvert.SerializeObject(response).ToString());
            //return JsonConvert.DeserializeObject(response);
            return response;
        }

        public string postWebApi(string apiPath, string controller, string service, object data)
        {
            // Set the header so it knows we are sending JSON
            client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";

            // Serialise the data we are sending in to JSON
            string serialisedData = JsonConvert.SerializeObject(data);

            var path = WebConfigurationManager.AppSettings[apiPath].ToString();

            var url = path + "/" + controller + "/" + service;

            // Make the request
            var response = client.UploadString(url, serialisedData);

            // Deserialise the response into a GUID
            //return JsonConvert.DeserializeObject(response);
            return response;
        }

        private string PathAPI(string apiPath, string controller, string service, string[] parammeters)
        {
            string newPath = "";

            if (apiPath == "apiCompany")
            {
                var path = WebConfigurationManager.AppSettings[apiPath].ToString();
                if (controller == "Company")
                {
                    if (service == "GetCompanyDetailList")
                    {
                        newPath = path + "/" + controller + "/" + service + "?searchCom=" + parammeters[0];
                    }
                }
            }
            return newPath;
        }

        private string getCompanyBlacklistDropDownListPath(string apiPath, string controller, string service, string[] parammeters)
        {
            string Path = "";
            Path = apiPath + "/" + controller + "/" + service;
            if (parammeters != null && parammeters.Count() > 0)
            {
                var i = 0;
                foreach (var param in parammeters)
                {
                    if (i == 0)
                    {
                        Path = Path + "?param=" + param;
                        i++;
                    }
                    else
                    {
                        Path = Path + "&titleCode=" + param;
                    }
                }
            }

            return Path;
        }
    }
}