using EB_Service.Commons;
using ESB.WebAppl.ILSystem.Models;
using ILSystem.App_Code.Commons.APIService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;

namespace ESB.WebAppl.ILSystem.commons
{
    public class Connect
    {
        public DataSet ConnectAPI(string UrlAPI, string MethodName, string JsonData)
        {
            DataSet ds = new DataSet();
            try
            {
                #region "Send API"         
                string UrlRequest = UrlAPI + MethodName;// "Report/LoadCLActivityDetailList/";
                var httpWebRequest = HttpWebRequest.Create(UrlRequest);
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
                //cmd.Token = ReuseableToken.tokenGeneral;

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
}