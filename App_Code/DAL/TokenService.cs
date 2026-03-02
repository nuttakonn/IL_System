using System;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using UserCenterModel;
using ESB.WebAppl.ILSystem.Models;

namespace EB_Service.DAL
{
    public class TokenService
    {
        public string BuildToken()
        {
            string token = "";
            try
            {
                JObject jRequest = new JObject
                {
                       {"username", ConfigurationManager.AppSettings["BuildToken_Username"].Trim() },
                       {"password", ConfigurationManager.AppSettings["BuildToken_Password"].Trim()},
                       {"role", ConfigurationManager.AppSettings["BuildToken_role"].Trim() },
                       {"issuer",  ConfigurationManager.AppSettings["BuildToken_issuer"].Trim() },
                       {"expiresminutes", Int64.Parse(ConfigurationManager.AppSettings["BuildToken_expiresMinutes"].Trim())}
                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(ConfigurationManager.AppSettings["BuildToken_Url"].Trim());
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
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
                    }
                }
                #endregion
            }
            catch (Exception ex) { }
            return token;
        }

    }
}