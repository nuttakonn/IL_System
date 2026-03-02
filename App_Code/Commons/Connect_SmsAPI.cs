using EB_Service.Commons;
using ESB.WebAppl.ILSystem.commons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using UserCenterModel;
using ESB.WebAppl.ILSystem.Models;

namespace ILSystem.App_Code.Commons
{
    public class Connect_SmsAPI
    {
        Hashtable header;
        string str_SmsUrl = WebConfigurationManager.AppSettings["SmsAPIURL"].ToString().Trim();
        string str_SmsIssuer = WebConfigurationManager.AppSettings["Sms_issuer"].ToString().Trim();
        string str_SmsExpiresMin = WebConfigurationManager.AppSettings["Sms_expiresMin"].ToString().Trim();
        string str_SmsToken = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();

        public string BuildTokenSms()
        {
            string token = "";
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            header = new Hashtable();
            header.Clear();
            header.Add("AccessKey", m_userInfo.AccessKey);
            try
            {
                JObject jRequest = new JObject
                {
                       {"username", m_userInfo.Username },
                       {"password", m_userInfo.Password},
                       {"role", ConfigurationManager.AppSettings["Sms_role"].ToString().Trim() },
                       {"issuer",  str_SmsIssuer },
                       {"expiresminutes", int.Parse(str_SmsExpiresMin)},
                       {"key", ConfigurationManager.AppSettings["Sms_key"].ToString().Trim()}

                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(str_SmsToken);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                //Utility.WriteLogResponse("Request to : " + str_SmsToken);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jRequest.ToString());
                }
                #endregion

                #region "Response API"
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    ResponseModel response = JObject.Parse(streamReader.ReadToEnd()).ToObject<ResponseModel>();
                    if (response.success)
                    {
                        //Utility.WriteLogResponse("Response from : " + str_SmsToken, JsonConvert.SerializeObject(response.data));
                        TokenModel objToken = JsonConvert.DeserializeObject<TokenModel>(response.data.ToString());
                        token = objToken.token;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return token;
            }
            return "Bearer " + token;
        }

        public ResponseModel SendSMS(string srCData, string mobileNo)
        {
            ResponseModel res = new ResponseModel();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                JObject jObject = new JObject
                {
                    ["templeteSMS"] = srCData.Trim(),
                    [""] = ""
                };
                var Bearer = BuildTokenSms();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/SmsNotification/SendSmsNotification";
                Utility.WriteLogResponse("Request to : " + str_SmsUrl + actionData);
                res = connect.ConnectAPI(str_SmsUrl, actionData, JsonConvert.SerializeObject(jObject), "POST", header);
                Utility.WriteLogResponse("Response from : " + str_SmsUrl + actionData, JsonConvert.SerializeObject(res.data));

            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
    }
}