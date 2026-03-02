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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using UserCenterModel;

namespace ILSystem.App_Code.Commons
{
    public class Connect_NcbAPI
    {
        Hashtable header;
        string str_NcbUrl = WebConfigurationManager.AppSettings["NcbAPIURL"].ToString().Trim();
        string str_NcbIssuer = WebConfigurationManager.AppSettings["Ncb_issuer"].ToString().Trim();
        string str_NcbExpiresMin = WebConfigurationManager.AppSettings["Ncb_expiresMin"].ToString().Trim();
        string str_NcbToken = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();
        UserInfo m_userInfo;
        public Connect_NcbAPI(UserInfo userInfo)
        {
            m_userInfo = userInfo;
        }
        public async Task<string> BuildTokenNcb()
        {
            string token = "";
            header = new Hashtable();
            header.Clear();
            header.Add("AccessKey", m_userInfo.AccessKey);
            try
            {
                JObject jRequest = new JObject
                {
                       {"username", m_userInfo.Username },
                       {"password", m_userInfo.Password},
                       {"role", ConfigurationManager.AppSettings["Ncb_role"].Trim() },
                       {"issuer",  str_NcbIssuer },
                       {"expiresminutes", int.Parse(str_NcbExpiresMin)}
                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(str_NcbToken);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                //Utility.WriteLogResponse("Request to : " + str_NcbToken);
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
                        //Utility.WriteLogResponse("Response from : " + str_NcbToken, JsonConvert.SerializeObject(response.data));
                        TokenModel objToken = JsonConvert.DeserializeObject<TokenModel>(response.data.ToString());
                        token = objToken.token;
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

        public async Task<ResponseModel> CheckNCBGateway(string Biz, string Branch, string AppNo, string IdNo, string CsnNo, string Name, string BirthDate,
                                                        string Premini, string Caller)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var connect = new Connect();
                JObject jObject = new JObject
                {
                    ["biz"] = Biz.Trim(),
                    ["branch"] = Branch.PadLeft(3,'0'),
                    ["appno"] = AppNo.Trim(),
                    ["idno"] = IdNo.Trim(),
                    ["csnno"] = CsnNo.Trim(),
                    ["name"] = Name.Trim(),
                    ["birthdate"] = BirthDate.Trim(),
                    ["premini"] = Premini.Trim(),
                    ["caller"] = Caller.Trim()
                };
                var Bearer = await BuildTokenNcb();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/NcbService/CheckNCBGateway";
                Utility.WriteLogResponse("Request to : " + str_NcbUrl + actionData);
                res = connect.ConnectAPI(str_NcbUrl, actionData, JsonConvert.SerializeObject(jObject), "POST", header);
                Utility.WriteLogResponse("Response from : " + str_NcbUrl + actionData, JsonConvert.SerializeObject(res.data));

            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return await Task.FromResult(res);
            }
            return await Task.FromResult(res);
        }
    }
}