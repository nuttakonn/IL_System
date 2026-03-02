using ILSystem.App_Code.Commons;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Web.Configuration;
using System;
using ESB.WebAppl.ILSystem.commons;
using System.Web;
using EB_Service.Commons;
using static ServiceStack.Diagnostics.Events;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Net;
using UserCenterModel;
using ESB.WebAppl.ILSystem.Models;
using ILSystem.App_Code.Commons.APIService;

namespace ESB.WebAppl.ILSystem.commons
{
    public class Connect_GeneralAPI
    {

        Hashtable header;
        string str_urlGeneral = WebConfigurationManager.AppSettings["GeneralDataCenterAPIURL"].ToString().Trim();
        string str_Issuer = WebConfigurationManager.AppSettings["General_issuerGen"].ToString().Trim();
        string str_ExpiresMin = WebConfigurationManager.AppSettings["General_expiresMin"].ToString().Trim();
        string str_Token = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();

        public Connect_GeneralAPI()
        {
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            string accessKey = m_userInfo.AccessKey;
            var Bearer = ReuseableToken.tokenGeneral;
            header = new Hashtable();
            header.Clear();
            header.Add("Authorization", "Bearer " + Bearer);
            header.Add("AccessKey", m_userInfo.AccessKey);
        }
        private void GetParameterGN()
        {
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                if (m_userInfo != null)
                {
                    string accessKey = m_userInfo.AccessKey;
                    header = new Hashtable();
                    header.Add("AccessKey", accessKey);
                    var requestModelGeneral = new UserCenterModel.RequestTokenModel
                    {
                        userName = m_userInfo.Username,
                        password = m_userInfo.Password,
                        role = "",
                        issuer = str_Issuer,
                        expiresMinutes = Convert.ToInt16(str_ExpiresMin)
                    };
                    string jsonRequest = JsonConvert.SerializeObject(requestModelGeneral);
                    var connect = new Connect();
                    var Barer = connect.ConnectAPI(str_Token, "", jsonRequest);
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
        }

        public string BuildTokenGeneral()
        {
            string token = "";
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            //header = new Hashtable();
            string accessKey = m_userInfo.AccessKey;
            //header.Add("AccessKey", accessKey);
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
                //DataSet  ds = Connect_GeneralAPI("",'', jRequest)
                #region "Send API
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
                        TokenModel objToken = JsonConvert.DeserializeObject<TokenModel>(response.data.ToString());
                        token = objToken.token;
                        //Utility.WriteLogResponse("Response from : " + ConfigurationManager.AppSettings["BuildToken"].Trim(), JsonConvert.SerializeObject(response.data));

                    }
                }
                #endregion
            }
            catch (Exception ex) { Utility.WriteLog(ex); }
            return "Bearer " + token;
        }


        public DataTable GetGeneralTitleName()
        {
            DataSet ds = new DataSet();
            DataTable dt_TitleN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralTitle/GetGeneralTitle";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_TitleN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TitleN;
        }

        public DataTable GetGeneralOfficeTitle()
        {
            DataSet ds = new DataSet();
            DataTable dt_OfficeTitleN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];

            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralOfficeTitle/GetGeneralOfficeTitle";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_OfficeTitleN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_OfficeTitleN;
        }

        public DataTable GetGeneralBranchCode()
        {
            DataSet ds = new DataSet();
            DataTable dt_BranchCodeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_BranchCodeN"] != null)
                {
                    dt_BranchCodeN = (DataTable)HttpContext.Current.Cache["ds_BranchCodeN"];
                }

                if (dt_BranchCodeN.Rows.Count == 0)
                {
                    string actionData = "/BankHS/GetBankBranchHS";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_BranchCodeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_BranchCodeN"] = dt_BranchCodeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_BranchCodeN;
        }

        public DataTable GetGeneralBankList()
        {
            DataSet ds = new DataSet();
            DataTable dt_Bank = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/Bank/GetBankList";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_Bank = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                HttpContext.Current.Cache["ds_Bank"] = dt_Bank;
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_Bank;
        }

        public DataTable GetGeneralBuilding()
        {
            DataSet ds = new DataSet();
            DataTable dt_BuildingN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_BuildingN"] != null)
                {
                    dt_BuildingN = (DataTable)HttpContext.Current.Cache["ds_BuildingN"];
                }

                if (dt_BuildingN.Rows.Count == 0)
                {
                    string actionData = "/GeneralBuilding/GetGeneralBuilding";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_BuildingN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_BuildingN"] = dt_BuildingN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_BuildingN;
        }

        public DataTable GetGeneralAccountType()
        {
            DataSet ds = new DataSet();
            DataTable dt_AccountTypeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_AccountTypeN"] != null)
                {
                    dt_AccountTypeN = (DataTable)HttpContext.Current.Cache["ds_AccountTypeN"];
                }

                if (dt_AccountTypeN.Rows.Count == 0)
                {
                    string actionData = "/BankHS/GetAccountType";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_AccountTypeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_AccountTypeN"] = dt_AccountTypeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_AccountTypeN;
        }

        public DataTable GetGeneralReceiveSalary()
        {
            DataSet ds = new DataSet();
            DataTable dt_ReceiveSalaryN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ReceiveSalaryN"] != null)
                {
                    dt_ReceiveSalaryN = (DataTable)HttpContext.Current.Cache["ds_ReceiveSalaryN"];
                }

                if (dt_ReceiveSalaryN.Rows.Count == 0)
                {
                    string actionData = "/GeneralCompanyBusinessHS/GetTypeReceiveSalary";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ReceiveSalaryN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ReceiveSalaryN"] = dt_ReceiveSalaryN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ReceiveSalaryN;
        }

        public DataTable GetGeneralTambol()
        {
            DataSet ds = new DataSet();
            DataTable dt_TambolN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralAddrTambol/GeneralAddrTambol";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_TambolN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TambolN;
        }

        public DataTable GetGeneralAmphurByID(string strAmphur)
        {
            DataSet ds = new DataSet();
            DataTable dt_AmphurN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_AmphurN"] != null)
                {
                    dt_AmphurN = (DataTable)HttpContext.Current.Cache["ds_AmphurN"];
                }

                if (dt_AmphurN.Rows.Count == 0)
                {
                    string actionData = "/AddrAumphur/GetAddrAumphur?id=" + strAmphur;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_AmphurN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_AmphurN"] = dt_AmphurN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_AmphurN;
        }
        public DataTable GetGeneralProvinceByID(string strProvince)
        {
            DataSet ds = new DataSet();
            DataTable dt_ProvinceN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ProvinceN"] != null)
                {
                    dt_ProvinceN = (DataTable)HttpContext.Current.Cache["ds_ProvinceN"];
                }

                if (dt_ProvinceN.Rows.Count == 0)
                {
                    string actionData = "/AddrProvince/GetAddrProvince?id=" + strProvince;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ProvinceN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ProvinceN"] = dt_ProvinceN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ProvinceN;
        }
        public DataTable GetGeneralProvince()
        {
            DataSet ds = new DataSet();
            DataTable dt_ProvinceAllN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ProvinceAllN"] != null)
                {
                    dt_ProvinceAllN = (DataTable)HttpContext.Current.Cache["ds_ProvinceAllN"];
                }

                if (dt_ProvinceAllN.Rows.Count == 0)
                {
                    string actionData = "/AddrProvince/GetProvinceByCode";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ProvinceAllN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ProvinceAllN"] = dt_ProvinceAllN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ProvinceAllN;
        }
        public DataTable GetAmphurByProvince(string strProvince)
        {
            DataSet ds = new DataSet();
            DataTable dt_AmphurByProvince = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_AmphurByProvince"] != null)
                {
                    dt_AmphurByProvince = (DataTable)HttpContext.Current.Cache["ds_AmphurByProvince"];
                }

                if (dt_AmphurByProvince.Rows.Count == 0)
                {
                    string actionData = "/AddrAumphur/GetAmphurByProvinceID?id=" + strProvince;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_AmphurByProvince = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_AmphurByProvince"] = dt_AmphurByProvince;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_AmphurByProvince;
        }
        public DataTable GetTambolByAmphur(string strAmphur)
        {
            DataSet ds = new DataSet();
            DataTable dt_TambolByAmphur = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_TambolByAmphur"] != null)
                {
                    dt_TambolByAmphur = (DataTable)HttpContext.Current.Cache["ds_TambolByAmphur"];
                }

                if (dt_TambolByAmphur.Rows.Count == 0)
                {
                    string actionData = "/GeneralAddrTambol/GetTambolByAmphurCode?id=" + strAmphur;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_TambolByAmphur = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TambolByAmphur"] = dt_TambolByAmphur;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TambolByAmphur;
        }

        public DataTable GetGeneralBusinessType()
        {
            DataSet ds = new DataSet();
            DataTable dt_BusinessTypeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_BusinessTypeN"] != null)
                {
                    dt_BusinessTypeN = (DataTable)HttpContext.Current.Cache["ds_BusinessTypeN"];
                }

                if (dt_BusinessTypeN.Rows.Count == 0)
                {
                    string actionData = "/GeneralCompanyBusinessHS​/GetBusinessType";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_BusinessTypeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_BusinessTypeN"] = dt_BusinessTypeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_BusinessTypeN;
        }

        public DataTable GetGeneralActionCode()
        {
            DataSet ds = new DataSet();
            DataTable dt_ActionCodeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ActionCodeN"] != null)
                {
                    dt_ActionCodeN = (DataTable)HttpContext.Current.Cache["ds_ActionCodeN"];
                }

                if (dt_ActionCodeN.Rows.Count == 0)
                {
                    string actionData = "/ProductHS/GetActionDetail";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ActionCodeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ActionCodeN"] = dt_ActionCodeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ActionCodeN;
        }

        public DataTable GetGeneralReasonCode()
        {
            DataSet ds = new DataSet();
            DataTable dt_ReasonCodeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ReasonCodeN"] != null)
                {
                    dt_ReasonCodeN = (DataTable)HttpContext.Current.Cache["ds_ReasonCodeN"];
                }

                if (dt_ReasonCodeN.Rows.Count == 0)
                {
                    string actionData = "/ProductHS/GetReasonDetail";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ReasonCodeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ReasonCodeN"] = dt_ReasonCodeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ReasonCodeN;
        }

        public DataTable GetGeneralPaySlip()
        {
            DataSet ds = new DataSet();
            DataTable dt_PaySlipN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_PaySlipN"] != null)
                {
                    dt_PaySlipN = (DataTable)HttpContext.Current.Cache["ds_PaySlipN"];
                }

                if (dt_PaySlipN.Rows.Count == 0)
                {
                    string actionData = "/GeneralCompanyBusinessHS/GetPaySlip";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_PaySlipN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_PaySlipN"] = dt_PaySlipN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_PaySlipN;
        }

        public DataTable GetGeneralOccupation()
        {
            DataSet ds = new DataSet();
            DataTable dt_OccupationN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralOccupation/GetGeneralOccupation";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_OccupationN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                //if (HttpContext.Current.Cache["ds_OccupationN"] != null)
                //{
                //    dt_OccupationN = (DataTable)HttpContext.Current.Cache["ds_OccupationN"];
                //}

                //if (dt_OccupationN.Rows.Count == 0)
                //{
                //    string actionData = "/GeneralOccupation/GetGeneralOccupation";
                //    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                //    var jsonData = JsonConvert.SerializeObject(resData.data);
                //    dt_OccupationN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                //    HttpContext.Current.Cache["ds_OccupationN"] = dt_OccupationN;
                //}
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_OccupationN;
        }
        public DataTable GetGeneralPosition()
        {
            DataSet ds = new DataSet();
            DataTable dt_PositionN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralPosition/GetGeneralPosition";
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                dt_PositionN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_PositionN;
        }

        public DataTable GetGeneralTypeCusAddress()
        {
            DataSet ds = new DataSet();
            DataTable dt_TypeCusAddressN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_TypeCusAddressN"] != null)
                {
                    dt_TypeCusAddressN = (DataTable)HttpContext.Current.Cache["ds_TypeCusAddressN"];
                }

                if (dt_TypeCusAddressN.Rows.Count == 0)
                {
                    string actionData = "/GeneralAddressHS/GetTypeCustomerAddress";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_TypeCusAddressN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TypeCusAddressN"] = dt_TypeCusAddressN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TypeCusAddressN;
        }

        public DataTable GetGeneralMaritalStatus()
        {
            DataSet ds = new DataSet();
            DataTable dt_MaritalStatusN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralMaritalStatus/GetGeneralMaritalStatus";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_MaritalStatusN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_MaritalStatusN;
        }

        public DataTable GetGeneralPaymentType()
        {
            DataSet ds = new DataSet();
            DataTable dt_PaymentTypeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_PaymentTypeN"] != null)
                {
                    dt_PaymentTypeN = (DataTable)HttpContext.Current.Cache["ds_PaymentTypeN"];
                }

                if (dt_PaymentTypeN.Rows.Count == 0)
                {
                    string actionData = "/PaymentHS/GetPaymentType";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_PaymentTypeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_PaymentTypeN"] = dt_PaymentTypeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_PaymentTypeN;
        }
        public DataTable GetGeneralApplyType()
        {
            DataSet ds = new DataSet();
            DataTable dt_ApplyTypeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ApplyTypeN"] != null)
                {
                    dt_ApplyTypeN = (DataTable)HttpContext.Current.Cache["ds_ApplyTypeN"];
                }

                if (dt_ApplyTypeN.Rows.Count == 0)
                {
                    string actionData = "/Apply/GetApplyType";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ApplyTypeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ApplyTypeN"] = dt_ApplyTypeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ApplyTypeN;
        }

        public DataTable GetGeneralTypeHomeTelephone()
        {
            DataSet ds = new DataSet();
            DataTable dt_TypeHomeTelN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_TypeHomeTelN"] != null)
                {
                    dt_TypeHomeTelN = (DataTable)HttpContext.Current.Cache["ds_TypeHomeTelN"];
                }

                if (dt_TypeHomeTelN.Rows.Count == 0)
                {
                    string actionData = "/CustomerHS/GetTypeHomeTelephone";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_TypeHomeTelN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TypeHomeTelN"] = dt_TypeHomeTelN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TypeHomeTelN;
        }

        public DataTable GetGeneralEmployment()
        {
            DataSet ds = new DataSet();
            DataTable dt_EmploymentN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralEmployment/GetGeneralEmployment";
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                dt_EmploymentN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_EmploymentN;
        }
        public DataTable GetGeneralTypeContract()
        {
            DataSet ds = new DataSet();
            DataTable dt_TypeContractN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_TypeContractN"] != null)
                {
                    dt_TypeContractN = (DataTable)HttpContext.Current.Cache["ds_TypeContractN"];
                }

                if (dt_TypeContractN.Rows.Count == 0)
                {
                    string actionData = "/CustomerHS/GetTypeContract";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_TypeContractN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TypeContractN"] = dt_TypeContractN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TypeContractN;
        }

        public DataTable GetGeneralTypePayDate()
        {
            DataSet ds = new DataSet();
            DataTable dt_TypePayDateN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_TypePayDateN"] != null)
                {
                    dt_TypePayDateN = (DataTable)HttpContext.Current.Cache["ds_TypePayDateN"];
                }

                if (dt_TypePayDateN.Rows.Count == 0)
                {
                    string actionData = "/GeneralCompanyBusinessHS/GetTypePayDate";
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    dt_TypePayDateN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TypePayDateN"] = dt_TypePayDateN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_TypePayDateN;
        }

        public DataTable GetGeneralApplyChannel()
        {
            DataSet ds = new DataSet();
            DataTable dt_ApplyChannelN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ApplyChannelN"] != null)
                {
                    dt_ApplyChannelN = (DataTable)HttpContext.Current.Cache["ds_ApplyChannelN"];
                }

                if (dt_ApplyChannelN.Rows.Count == 0)
                {
                    string actionData = "/Channel/GetApplyChannel";
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ApplyChannelN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ApplyChannelN"] = dt_ApplyChannelN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ApplyChannelN;
        }

        public DataTable GetGeneralSubApplyChannel(string strChannel)
        {
            DataSet ds = new DataSet();
            DataTable dt_SubApplyChannelN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_SubApplyChannelN"] != null)
                {
                    dt_SubApplyChannelN = (DataTable)HttpContext.Current.Cache["ds_SubApplyChannelN"];
                }

                if (dt_SubApplyChannelN.Rows.Count == 0)
                {
                    string actionData = "/Channel/GetContractSubChannel?id=" + strChannel;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_SubApplyChannelN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_SubApplyChannelN"] = dt_SubApplyChannelN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_SubApplyChannelN;
        }

        public DataTable GetGeneralProductType(string strProductType)
        {
            DataSet ds = new DataSet();
            DataTable dt_ProductTypeN = new DataTable();
            try
            {
                var connect = new Connect();
                if (HttpContext.Current.Cache["ds_ProductTypeN"] != null)
                {
                    dt_ProductTypeN = (DataTable)HttpContext.Current.Cache["ds_ProductTypeN"];
                }

                if (dt_ProductTypeN.Rows.Count == 0)
                {
                    string actionData = "/ProductHS/GetProductType?desc=" + strProductType;
                    //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                    var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                    dt_ProductTypeN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_ProductTypeN"] = dt_ProductTypeN;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ProductTypeN;
        }

        public DataTable getResidentType()
        {
            DataSet ds = new DataSet();
            DataTable dt_ResidentType = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralResidentalStatus/GetGeneralResidentalStatus";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_ResidentType = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_ResidentType;
        }

        public DataTable getSalaryTime()
        {
            DataSet ds = new DataSet();
            DataTable dt_SalaryTime = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                var connect = new Connect();
                var Bearer = BuildTokenGeneral();
                header.Clear();
                header.Add("Authorization", Bearer);
                header.Add("AccessKey", m_userInfo.AccessKey);
                string actionData = "/GeneralSalaryTime/GetGeneralSalaryTime";
                //Utility.WriteLogResponse("Request to : " + str_urlGeneral + actionData);
                var resData = connect.ConnectAPI(str_urlGeneral, actionData, "", "GET", header);
                var jsonData = JsonConvert.SerializeObject(resData.data);
                //Utility.WriteLogResponse("Response from : " + str_urlGeneral + actionData, jsonData);
                dt_SalaryTime = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return dt_SalaryTime;
        }

    }
}