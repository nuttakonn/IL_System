using EB_Service.Commons;
using ILSystem.App_Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace ILSystem.App_Code.Commons
{
    public class UserInfoService
    {
        public UserInfo GetUserInfo()
        {
            bool isAuthUser = false;
            UserInfo userInfo = new UserInfo();
            if (HttpContext.Current.Session["UserInfo"] != null)
            {
                userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                isAuthUser = true;
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["k"] != null)
                {
                    string accessKey = HttpContext.Current.Request.Cookies["k"].Value;
                    if (!string.IsNullOrEmpty(accessKey))
                    {
                        var api = new APIServices(new APIServiceOptions
                        {
                            url = WebConfigurationManager.AppSettings["SSOTokenServiceAPIURL"].ToString().Trim() + "/Token/GetRedisValueBykey/" + accessKey,
                            method = APIServiceMethod.GET
                        });
                        string responseToken = api.connectAPIReturnString();

                        if (!string.IsNullOrWhiteSpace(responseToken))
                        {
                            string[] userInfos = responseToken.Split('|');

                            userInfo.AccessKey = accessKey;
                            userInfo.Username = Utility.DecryptPassWord(userInfos[0]);
                            userInfo.Password = Utility.DecryptPassWord(userInfos[2]);
                            userInfo.PortalID = int.Parse(WebConfigurationManager.AppSettings["PortalID_ILHisun"].ToString());

                            string strIP = "";
                            try
                            {
                                strIP = (System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
                                strIP = strIP.Length > 10 ? strIP.Substring(0, 10) : strIP;
                                if (string.IsNullOrEmpty(strIP))
                                {
                                    strIP = "SERVER";
                                }
                            }
                            catch
                            {
                                strIP = "SERVER";
                            }
                            userInfo.LocalClient = strIP;
                            HttpContext.Current.Session["UserInfo"] = userInfo;
                            isAuthUser = true;
                        }
                    }
                }

            }
            if (!isAuthUser)
            {
                HttpContext.Current.Request.Cookies.Clear();

                FormsAuthentication.SignOut();
                userInfo = null;
            }
            if (HttpContext.Current.Request.Cookies["branch"] != null)
            {
                string strBranch = HttpContext.Current.Request.Cookies["branch"].Value;
                userInfo.BranchApp = strBranch;
                userInfo.BranchNo = strBranch;
                userInfo.BranchDescEN = strBranch;
            }
            return userInfo;
        }
    }
}
