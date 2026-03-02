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
using ILSystem.App_Code.Model.NOTEAPI;
using System.Globalization;
using EB_Service.DAL;
using System.Data;

namespace ILSystem.App_Code.Commons
{
    public class Connect_NoteAPI
    {
        Hashtable header;
        string str_NoteUrl = WebConfigurationManager.AppSettings["NoteAPIURL"].ToString().Trim();
        string str_NoteIssuer = WebConfigurationManager.AppSettings["Note_issuer"].ToString().Trim();
        string str_NoteExpiresMin = WebConfigurationManager.AppSettings["Note_expiresMin"].ToString().Trim();
        string str_NoteToken = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();
        DataCenter dataCenter;
        public async Task<string> BuildTokenNote()
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
                       {"role", ConfigurationManager.AppSettings["Note_role"].Trim() },
                       {"issuer",  str_NoteIssuer },
                       {"expiresminutes", int.Parse(str_NoteExpiresMin)}
                };
                #region "Send API"
                var httpWebRequest = WebRequest.Create(str_NoteToken);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                //Utility.WriteLogResponse("Request to : " + str_NoteToken);
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
                        //Utility.WriteLogResponse("Response from : " + str_NoteToken, JsonConvert.SerializeObject(response.data));
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

        public async Task<ResponseModel> AddNote(string CnsNo,string ContractNo, string Action, string Reason, string NoteDesc,string UpDate, string upTime)
        {
            ResponseModel res = new ResponseModel();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            dataCenter = new DataCenter(m_userInfo);
            try
            {
                var connect = new Connect();
                //JObject saveNoteData = new JObject
                //{
                //    ["strBranchNo"] = m_userInfo.BranchNo.ToString().Trim(),
                //    ["username"] = m_userInfo.Username.ToString().Trim(),
                //    ["strActionCode"] = Action.ToString().Trim(),
                //    ["strResultCode"] = Reason.ToString().Trim(),
                //    ["timeStart"] = (int.Parse(UpDate.Trim().Substring(0,4)) - 543).ToString() + "-" + UpDate.Trim().Substring(4,2) + "-" + UpDate.Trim().Substring(6,2) + " " +
                //    upTime.PadLeft(6, '0').Substring(0, 2) + ":" + upTime.PadLeft(6, '0').Substring(2, 2) + ":" + upTime.PadLeft(6, '0').Substring(4, 2),
                //    ["strCSNNo"] = CnsNo.ToString().Trim(),
                //    ["strCardNo"] = ContractNo.ToString().Trim(),
                //    ["strNoteDetail"]= NoteDesc.ToString().Trim()
                //};
                string noteBy = Utility.Base64Encode(m_userInfo.Username.ToString().Trim());
                string createBy = Utility.Base64Encode(m_userInfo.Username.ToString().Trim());
                string sql = $@"SELECT DISTINCT  top 1 USFNME 
                                   FROM [AS400DB01].[SYOD0000].[SYFUSDES] WITH (NOLOCK)
                                   WHERE USEMID = '{m_userInfo.Username.ToString()}' ";
                string NoteByName = "";
                DataSet DS = dataCenter.GetDataset<DataTable>(sql, System.Data.CommandType.Text).Result.data;
                if(DS != null && DS.Tables?[0].Rows?.Count > 0)
                {
                    foreach(DataRow dr in DS.Tables[0]?.Rows)
                    {
                        NoteByName = dr["USFNME"].ToString().Trim();
                    }
                }
                var body = new AddNoteRequestModel
                {
                    CSNNo = CnsNo.ToString().Trim(),
                    ContractNo = ContractNo.ToString().Trim(),
                    ActionCode = Action.ToString().Trim(),
                    //NoteDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")), //P'Mail
                    SystemBy = "ILScreen",
                    ResultCode = Reason.ToString().Trim(),
                    NoteDescription = NoteDesc.ToString().Trim(),
                    NoteRemark = NoteDesc.ToString().Trim(),
                    personCode = string.Empty,
                    NoteByName = NoteByName,
                    NoteBy = noteBy,
                    CreateBy = createBy
                };
                var model = new
                {
                    listTimelines = new List<AddNoteRequestModel> { body },
                    saveNoteData = new bool?(),
                };


                var Bearer = await BuildTokenNote();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/Note/AddNote";
                Utility.WriteLogResponse("Request to : " + str_NoteUrl + actionData);
                res = connect.ConnectAPI(str_NoteUrl, actionData, JsonConvert.SerializeObject(model), "POST", header);
                Utility.WriteLogResponse("Response from : " + str_NoteUrl + actionData, JsonConvert.SerializeObject(res.data));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return await Task.FromResult(res);
            }
            return await Task.FromResult(res);
        }

        public async Task<ResponseModel> GetNote(string CnsNo)
        {
            ResponseModel res = new ResponseModel();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                JObject payload = new JObject
                {
                    ["csnNo"] = CnsNo.ToString().Trim()
                };
                var Bearer = await BuildTokenNote();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/Note/GetTimeline";
                //Utility.WriteLogResponse("Request to : " + str_NoteUrl + actionData);
                res = connect.ConnectAPI(str_NoteUrl, actionData, JsonConvert.SerializeObject(payload), "POST", header);
                //Utility.WriteLogResponse("Response from : " + str_NoteUrl + actionData, JsonConvert.SerializeObject(res.data));
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