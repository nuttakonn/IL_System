using EB_Service.Commons;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.Model;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using UserCenterModel;

namespace ILSystem.App_Code.Commons.APIService
{
    public class ApiService
    {
        Hashtable _header;

        public ApiService()
        {
            _header = new Hashtable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            string accessKey = m_userInfo.AccessKey;
            _header.Add("AccessKey", accessKey);
            _header.Add("Authorization", "Bearer " + ReuseableToken.token ?? "jwt");

        }
        public DataSet ConnectAPI(string UrlAPI, string MethodName, string JsonData)
        {
            DataSet ds = new DataSet();
            try
            {
                #region "Send API"         
                string UrlRequest = UrlAPI + MethodName;// "Report/LoadCLActivityDetailList/";
                var httpWebRequest = HttpWebRequest.Create(UrlRequest);
                httpWebRequest.Headers.Add(_header.Keys.ToString(), _header.Values.ToString());

               //for (int i=0; i< _header.Count;i++)
               // {
               // }

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";//;charset=utf-8
                if (httpWebRequest.Method.Trim().ToUpper() != "GET")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(JsonData);
                    }
                }
                #endregion
                #region "Response API"
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    var responseText = serializer.DeserializeObject(streamReader.ReadToEnd());

                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.Formatting = Formatting.Indented;
                    DataSet ds_Return = JsonConvert.DeserializeObject<DataSet>(responseText.ToString(), st);

                    if ((ds_Return.Tables[0]).TableName == "Result")
                    {
                        string chk_err = ds_Return.Tables[0].Rows[0]["Err_Flag"].ToString();
                        if (chk_err == "Y")
                        {
                            return null;
                        }
                        else
                        {
                            ds = ds_Return;
                        }
                    }
                    else
                    {
                        ds = ds_Return;
                    }

                }
                #endregion          
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return ds;
        }
        public ResponseModel ConnectAPI(string url, string controllerAction, string jsonData, string method = "POST", Hashtable parameterhead = null)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                #region "Send API"         
                string UrlRequest = DeleteFirstSlashLastSlash(url) + "/" + DeleteFirstSlashLastSlash(controllerAction);
                var httpWebRequest = HttpWebRequest.Create(UrlRequest);
                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/json";//;charset=utf-8
                if (parameterhead != null)
                {
                    foreach (DictionaryEntry hash in parameterhead)
                    {
                        httpWebRequest.Headers[hash.Key.ToString()] = hash.Value.ToString();
                    }
                }
                if (httpWebRequest.Method.Trim().ToUpper() != "GET")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonData);
                    }
                }
                #endregion

                #region "Response API"
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = JObject.Parse(streamReader.ReadToEnd()).ToObject<ResponseModel>();
                }
                #endregion

                return response;
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return new ResponseModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
        }
        public string ConnectAPIReturnString(string url, string controllerAction, string jsonData, string method = "POST")
        {
            var response = "";
            try
            {
                #region "Send API"         
                string UrlRequest = DeleteFirstSlashLastSlash(url) + "/" + DeleteFirstSlashLastSlash(controllerAction);
                var httpWebRequest = HttpWebRequest.Create(UrlRequest);
                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/json";//;charset=utf-8
                if (httpWebRequest.Method.Trim().ToUpper() != "GET")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonData);
                    }
                }
                #endregion

                #region "Response API"
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                #endregion

                return response;
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return "";
            }
        }
        private string DeleteFirstSlashLastSlash(string value)
        {
            try
            {
                // delete first slash
                if (value.Substring(0, 1) == "/")
                {
                    value = value.Substring(1, (value.Length - 1));
                }

                // last first slash
                if (value.Substring((value.Length - 1), 1) == "/")
                {
                    value = value.Substring(0, (value.Length - 1));
                }

                return value;
            }
            catch(Exception ex)
            {
                Utility.WriteLog(ex);
                return value;
            }
        }
    }
    public static class ReuseableToken
    {
        private static DateTime timeout { get; set; }

        public static string token
        {
            get
            {
                var datenow = DateTime.Now;
                int timeoutSec = Convert.ToInt32(ConfigurationManager.AppSettings["hisun_expiresMin"]);

                if (datenow > timeout || string.IsNullOrWhiteSpace(_token))
                {
                    _token = BuildToken().Result;
                    timeout = DateTime.Now.AddMinutes(timeoutSec).AddSeconds(-30);
                    return _token;
                }
                return _token;
            }
            set => _token = value;
        }
        public static string tokenGeneral
        {
            get
            {
                var datenow = DateTime.Now;
                int timeoutSec = Convert.ToInt32(ConfigurationManager.AppSettings["hisun_expiresMin"]);

                if (datenow > timeout || string.IsNullOrWhiteSpace(_tokenGeneral))
                {
                    _tokenGeneral = BuildTokenGeneral().Result;
                    //timeout = DateTime.Now.AddMinutes(1);
                    timeout = DateTime.Now.AddMinutes(timeoutSec).AddSeconds(-30);

                    return _tokenGeneral;
                }
                return _tokenGeneral;
            }
            set => _tokenGeneral = value;
        }

        public class RequestTokenModel
        {
            public string userName { set; get; } = "";
            public string password { set; get; } = "";
            public string role { set; get; } = "";
            public string issuer { set; get; } = "";
            public int expiresMinutes { set; get; } = 2;
        }

        public class ResponseTokenModel
        {
            public bool success { get; set; }
            public string message { get; set; }
            public Token data { get; set; }
        }

        public class Token
        {
            public string token { get; set; }
        }


        private static string _token { get; set; }
        private static string _tokenGeneral { get; set; }

        //private static async Task<string> GetToken()
        //{
        //    var url = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var userName = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var password = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var role = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var issuer = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var expiresMinutes = ConfigurationManager.AppSettings["CenterUserTokenURL"].ToString().Trim();
        //    var _tokenUrl = new Uri(url);

        //    var requestBody = new RequestTokenModel
        //    {
        //        userName = "CenterUser",
        //        password = "CenterUser",
        //        role = "user",
        //        issuer = "MiGr@te400",
        //        expiresMinutes = 2
        //    };

        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.PostAsync(_tokenUrl, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));


        //        if (!response.IsSuccessStatusCode)
        //        {
        //            throw new InvalidOperationException("Cannot get token." + response.StatusCode + response.ReasonPhrase ?? "");
        //        }
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        var responseToken = JsonConvert.DeserializeObject<ResponseTokenModel>(responseString);

        //        return responseToken.data.token;
        //    }
        //}

        public static async Task<string> BuildToken()
        {
            string token = "";
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            
            try
            {
                JObject jRequest = new JObject
                {
                       {"username", m_userInfo.Username },
                       {"password", m_userInfo.Password},
                       {"role", ConfigurationManager.AppSettings["hisun_role"].Trim() },
                       {"issuer",  ConfigurationManager.AppSettings["hisun_issuer"].Trim() },
                       {"expiresminutes", ConfigurationManager.AppSettings["BuildToken_expiresMinutes"].Trim()}
                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(ConfigurationManager.AppSettings["BuildToken"].Trim());
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                //Utility.WriteLogResponse("Request to : " + ConfigurationManager.AppSettings["BuildToken"].Trim());
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
                        Token objToken = JsonConvert.DeserializeObject<Token>(response.data.ToString());
                        token = objToken.token;
                        //Utility.WriteLogResponse("Response to : " + ConfigurationManager.AppSettings["BuildToken"].Trim(), JsonConvert.SerializeObject(response.data));
                    }
                }
                #endregion
            }
            catch (Exception ex) { Utility.WriteLog(ex); }
            return await Task.FromResult(token);
        }

        public static async Task<string> BuildTokenGeneral()
        {
            string token = "";
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];

            try
            {
                JObject jRequest = new JObject
                {
                       {"username", m_userInfo.Username },
                       {"password", m_userInfo.Password},
                       {"role", ConfigurationManager.AppSettings["General_role"].Trim() },
                       {"issuer",  ConfigurationManager.AppSettings["General_issuerGen"].Trim() },
                       {"expiresminutes", Int64.Parse(ConfigurationManager.AppSettings["General_expiresMin"].Trim())}
                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(ConfigurationManager.AppSettings["BuildToken"].Trim());
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
                        Token objToken = JsonConvert.DeserializeObject<Token>(response.data.ToString());
                        token = objToken.token;
                    }
                }
                #endregion
            }
            catch (Exception ex) { Utility.WriteLog(ex); }
            return await Task.FromResult(token);
        }

    }
}