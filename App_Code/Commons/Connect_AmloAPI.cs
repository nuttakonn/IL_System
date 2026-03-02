using EB_Service.Commons;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using ESB.WebAppl.ILSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using UserCenterModel;

namespace ILSystem.App_Code.Commons
{
    public class Connect_AmloAPI
    {
        Hashtable header;
        string str_AmloUrl = WebConfigurationManager.AppSettings["AmloAPIURL"].ToString().Trim();
        string str_AmloIssuer = WebConfigurationManager.AppSettings["Amlo_issuer"].ToString().Trim();
        string str_AmloExpiresMin = WebConfigurationManager.AppSettings["Amlo_expiresMin"].ToString().Trim();
        string str_TokenUrl = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();

        public async Task<string> BuildTokenAmlo()
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
                       {"role", ConfigurationManager.AppSettings["Amlo_role"].Trim() },
                       {"issuer",  str_AmloIssuer },
                       {"expiresminutes", int.Parse(str_AmloExpiresMin)}
                };
                //DataSet  ds = Connect_GeneralAPI("",'', jRequest)
                #region "Send API"
                var httpWebRequest = WebRequest.Create(str_TokenUrl);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                //Utility.WriteLogResponse("Request to :" + str_TokenUrl);
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
                        TokenModel objToken = JsonConvert.DeserializeObject<TokenModel>(response.data.ToString());
                        token = objToken.token;
                        //Utility.WriteLogResponse("Resposne from :" + str_TokenUrl, JsonConvert.SerializeObject(response.data));
                    }
                }
                #endregion
            }
            catch (Exception ex) 
            {
                Utility.WriteLog(ex);
                return await Task.FromResult(token);
            }
            return await Task.FromResult("Bearer " + token);
        }

        public async Task<ResponseModel> CheckAmloAPI(string Idcard, string Name, string Surname,string Birthdate)
        {
            ResponseModel res = new ResponseModel();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                JObject jObject = new JObject
                {
                    ["idNo"] = Idcard.Trim(),
                    ["name"] = Name.Trim(),
                    ["surname"] = Surname.Trim(),
                    ["birthday"] = Birthdate.Trim(),
                    ["processCode"] = "APL"
                };
                var Bearer = await BuildTokenAmlo();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/AmloProcess/CheckAmlo";
                //Utility.WriteLogResponse("Request to : " + str_AmloUrl + actionData);
                res = connect.ConnectAPI(str_AmloUrl, actionData, JsonConvert.SerializeObject(jObject), "POST", header);
                //Utility.WriteLogResponse("Response from : " + str_AmloUrl + actionData, JsonConvert.SerializeObject(res.data));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return await Task.FromResult(res);
            }
            return await Task.FromResult(res);
        }

        public class AmloRespone
        {
            public string amloGroup { get; set; }
            public string amloFlagCode { get; set; }
            public string matchType { get; set; }
            public string errorFlag { get; set; }
        }
    }
}