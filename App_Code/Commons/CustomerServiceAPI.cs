using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ILSystem.App_Code.Commons
{
    public class CustomerServiceAPI
    {
        private APIService.ApiService _connect;
        private Hashtable _header;
        private string _str_url;
        public CustomerServiceAPI()
        {
            Hashtable header = new Hashtable();

            _str_url = WebConfigurationManager.AppSettings["GeneralDataCenterAPIURL"].ToString().Trim();
            string str_Issuer = WebConfigurationManager.AppSettings["General_issuerGen"].ToString().Trim();
            string str_ExpiresMin = WebConfigurationManager.AppSettings["General_issuerGen"].ToString().Trim();
            string str_Token = WebConfigurationManager.AppSettings["BuildToken_Url"].ToString().Trim();

            _connect = new APIService.ApiService();

        }

        public DataTable GetGeneralTitleName()
        {
            DataSet ds = new DataSet();
            DataTable dt_TitleN = new DataTable();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            string accessKey = m_userInfo.AccessKey;
            try
            {
                //var connect = new Connect();
                //var Barer = BuildTokenGeneral();
                var Barer = "";
                if (HttpContext.Current.Cache["ds_TitleN"] != null)
                {
                    dt_TitleN = (DataTable)HttpContext.Current.Cache["ds_TitleN"];
                }

                if (dt_TitleN.Rows.Count == 0)
                {
                    string actionData = "/GeneralTitle/GetGeneralTitle";
                    var resData = _connect.ConnectAPI(_str_url, actionData, Barer, "GET", _header);
                    var jsonData = JsonConvert.SerializeObject(resData.data);
                    dt_TitleN = (DataTable)JsonConvert.DeserializeObject(jsonData, typeof(DataTable));
                    HttpContext.Current.Cache["ds_TitleN"] = dt_TitleN;
                }
            }
            catch (Exception ex)
            {

            }
            return dt_TitleN;
        }

    }

}