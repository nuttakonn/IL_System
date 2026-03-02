using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CSManage_Home_Home : BasePage//System.Web.UI.Page
{
    private readonly RedisHelper _redis;
    public CSManage_Home_Home()
    {
        _redis = new RedisHelper();
    }

    //ILDataCenter ilObj;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfoService userInfoService = new UserInfoService();
        UserInfo m_userInfo = userInfoService.GetUserInfo();

        //Response.Write(@"<script>console.log('HOME page_load - " + DateTime.Now.ToString() + @"');</script>");
        if (Request["PortalID"] != null)
        {
            UserInfo usInfo = new UserInfo();
            if (m_userInfo != null)
            {
                usInfo = m_userInfo;

            }

            usInfo.PortalID = Convert.ToInt32(Request["PortalID"]);

            if (Request["UserName"] != null)
                usInfo.Username = ((string)Request["UserName"]).ToUpper();
            if (Request["Password"] != null)
                usInfo.Password = HttpUtility.HtmlDecode(Utility.DecryptPassWord((string)Request["Password"]));
            //-----------  
            //-- 61053 : Check Authorize Copy Data [2015-03-27]
            if (Request["AuthenCopy"] != null)
                usInfo.AuthenCopy = (string)Request["AuthenCopy"];


            ArrayList ar = new ArrayList();

            usInfo.RolesID = ar;



            try
            {
                usInfo.LocalClient = (System.Net.Dns.GetHostEntry(Context.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
                /*
                if (usInfo.LocalClient.Length > 10)
                {
                    usInfo.LocalClient = usInfo.LocalClient.Substring(0, 10);
                }
                */
                usInfo.LocalClient = usInfo.LocalClient.Length > 10 ? usInfo.LocalClient.Substring(0, 10) : usInfo.LocalClient;
            }
            catch { }
            //---------

            //Session["UserInfo"] = usInfo;
            Session["UserInfo"] = usInfo;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(
                  1,  // version
                  usInfo.Username.Trim(),      // login
                  DateTime.Now,               // date de creation
                  DateTime.Now.AddDays(1),// expiration au bout de 20 minutes
                  false,                      // dans un cookie non persistant
                  "SysAuthenAS400",          // on sauvegarde les roles dans le ticket
                  FormsAuthentication.FormsCookiePath);
            //// crypte le ticket
            //string EncryptedTicket =
            //FormsAuthentication.Encrypt(Ticket);
            //// création d'un cookie d'authentification contenant le ticket
            //HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);

            //FormsAuthentication.SetAuthCookie(usInfo.Username.Trim(), true);
            //Response.Cookies.Add(Cookie);


            string a = Session.SessionID;
            Session.Timeout = 3600;
            string EncryptedTicket = FormsAuthentication.Encrypt(Ticket);
            HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);
            Cookie.Expires = DateTime.Now.AddMinutes(720);

            Utility.WriteLogString(JsonConvert.SerializeObject(Ticket).ToString());
            FormsAuthentication.SetAuthCookie(usInfo.Username.Trim(), true);
            Response.Cookies.Add(Cookie);

            HttpCookie CookieUserInfo = new HttpCookie("k", usInfo.AccessKey);
            CookieUserInfo.Expires = DateTime.Now.AddMinutes(720);
            Response.Cookies.Add(CookieUserInfo);
            Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");
            //Response.Redirect("Home.aspx");
            //Response.Redirect("../WorkProcess/SelectBranch/IL_branch.aspx");
            //if (!IsPostBack) 
            //{
            //    bind_branch();
            //    PopupBranch.ShowOnPageLoad = true;
            //}
        }

        //BOT Report
        if (Request["acn"] != null)
        {
            UserInfo _UserInfo = new UserInfo();
            if (m_userInfo != null)
            {
                _UserInfo = m_userInfo;

            }
            string acn = Request["acn"].ToString().Trim();
            //string acn = WebConfigurationManager.AppSettings["LocalSessionkey"].ToString().Trim();

            string decode = HttpUtility.UrlDecode(acn);
            string[] decode_split = decode.Split('.');
            string accessKey = decode_split[0];

            #region Redis
            if (!string.IsNullOrEmpty(accessKey))
            {
                var empid = Utility.Base64Decode(accessKey).Split('_')[0];

                #region GetRedisValue
                var connect = new Connect();
                string url = WebConfigurationManager.AppSettings["SSOTokenServiceAPIURL"].ToString().Trim();
                string action = "/Token/GetRedisValueBykey/" + accessKey;
                string response = connect.ConnectAPIReturnString(url, action, "", "GET");
                HttpCookie httpCookie = new HttpCookie("AccessKey");
                httpCookie["acn"] = accessKey;
                httpCookie.Expires = DateTime.Now.AddHours(3);

                Response.Cookies.Add(httpCookie);
                var userInfo = response.Split('|');
                #endregion

                _UserInfo.AccessKey = accessKey;
                _UserInfo.Username = Utility.DecryptPassWord(empid);
                _UserInfo.Password = Utility.DecryptPassWord(userInfo[2]);
                _UserInfo.PortalID = int.Parse(WebConfigurationManager.AppSettings["PortalID_ILHisun"].ToString());
                ArrayList ar = new ArrayList();

                // AS400 Authentication mode
                //if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")
                //{
                //    As400DAL dal = new As400DAL(_UserInfo.Username.Trim().ToUpper(), _UserInfo.Password.Trim(),
                //        AppConfig.GNLib, EB_Service.DAL.Providor400Types.PriSeries);
                //    dal.ConnectAs400();
                //    if (!dal.As400Authentication(ref ar))
                //    {

                //        dal.CloseConnect();
                //        FormsAuthentication.SignOut();
                //        Response.Redirect(FormsAuthentication.LoginUrl);
                //        //return;
                //    }
                //    dal.CloseConnect();
                //    IBM.Data.DB2.iSeries.iDB2Numeric iAdmin = new IBM.Data.DB2.iSeries.iDB2Numeric(1);

                //    if (ar.IndexOf(iAdmin) != -1)
                //        _UserInfo.IsAdmin = true;
                //}
                //else
                //{

                //}

                _UserInfo.RolesID = ar;

                try
                {
                    _UserInfo.LocalClient = (System.Net.Dns.GetHostEntry(Context.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
                }
                catch { }

                Session["UserInfo"] = _UserInfo;
                Session["TestAuth"] = "TestValue";
                Utility.WriteLogString($"DEBUG => { Session["TestAuth"].ToString()}");
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(
                      1,  // version
                      _UserInfo.Username.Trim(),      // login
                      DateTime.Now,               // date de creation
                      DateTime.Now.AddMinutes(3600),// expiration au bout de 20 minutes
                      false,                      // dans un cookie non persistant
                      "SysAuthenAS400",          // on sauvegarde les roles dans le ticket
                      FormsAuthentication.FormsCookiePath);
                string a = Session.SessionID;
                Session.Timeout = 3600;
                string EncryptedTicket = FormsAuthentication.Encrypt(Ticket);
                HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);
                Cookie.Expires = DateTime.Now.AddMinutes(720);

                Utility.WriteLogString(JsonConvert.SerializeObject(Ticket).ToString());
                FormsAuthentication.SetAuthCookie(_UserInfo.Username.Trim(), true);
                Response.Cookies.Add(Cookie);

                HttpCookie CookieUserInfo = new HttpCookie("k", accessKey);
                CookieUserInfo.Expires = DateTime.Now.AddMinutes(720);
                Response.Cookies.Add(CookieUserInfo);
                //Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");
                Response.Redirect("../Home/Home.aspx");
                //var isExist = _redis.IsKeyExists(accessKey);
                //if (isExist)
                //{
                //    var empid = Utility.Base64Decode(accessKey).Split('_')[0];

                //    #region GetRedisValue
                //    var connect = new Connect();
                //    string url = WebConfigurationManager.AppSettings["SSOTokenServiceAPIURL"].ToString().Trim();
                //    string action = "/Token/GetRedisValueBykey/" + accessKey;
                //    string response = connect.ConnectAPIReturnString(url, action, "", "GET");
                //    var userInfo = response.Split('|');
                //    #endregion

                //    _UserInfo.AccessKey = accessKey;
                //    _UserInfo.Username = Utility.DecryptPassWord(empid);
                //    _UserInfo.Password = Utility.DecryptPassWord(userInfo[2]);
                //    _UserInfo.PortalID = 27;
                //    ArrayList ar = new ArrayList();

                //    // AS400 Authentication mode
                //    //if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")
                //    //{
                //    //    As400DAL dal = new As400DAL(_UserInfo.Username.Trim().ToUpper(), _UserInfo.Password.Trim(),
                //    //        AppConfig.GNLib, EB_Service.DAL.Providor400Types.PriSeries);
                //    //    dal.ConnectAs400();
                //    //    if (!dal.As400Authentication(ref ar))
                //    //    {

                //    //        dal.CloseConnect();
                //    //        FormsAuthentication.SignOut();
                //    //        Response.Redirect(FormsAuthentication.LoginUrl);
                //    //        //return;
                //    //    }
                //    //    dal.CloseConnect();
                //    //    IBM.Data.DB2.iSeries.iDB2Numeric iAdmin = new IBM.Data.DB2.iSeries.iDB2Numeric(1);

                //    //    if (ar.IndexOf(iAdmin) != -1)
                //    //        _UserInfo.IsAdmin = true;
                //    //}
                //    //else
                //    //{

                //    //}

                //    _UserInfo.RolesID = ar;

                //    try
                //    {
                //        _UserInfo.LocalClient = (System.Net.Dns.GetHostEntry(Context.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
                //    }
                //    catch { }

                //    Session["UserInfo"] = _UserInfo;
                //    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(
                //          1,  // version
                //          _UserInfo.Username.Trim(),      // login
                //          DateTime.Now,               // date de creation
                //          DateTime.Now.AddDays(1),// expiration au bout de 20 minutes
                //          false,                      // dans un cookie non persistant
                //          "SysAuthenAS400",          // on sauvegarde les roles dans le ticket
                //          FormsAuthentication.FormsCookiePath);
                //    string EncryptedTicket =
                //    FormsAuthentication.Encrypt(Ticket);
                //    HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);

                //    FormsAuthentication.SetAuthCookie(_UserInfo.Username.Trim(), true);
                //    Response.Cookies.Add(Cookie);

                //    //Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");
                //    Response.Redirect("../Home/Home.aspx");
                //}
                //else
                //{
                //    Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
                //}
            }
            else
            {
                Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
            }
            #endregion
        }

        if (m_userInfo == null || !Page.User.Identity.IsAuthenticated || Request["SignOut"] == "true")
        {

            FormsAuthentication.SignOut();
            HttpContext.Current.Request.Cookies.Clear();
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        //if (!IsPostBack)
        //{
        //    BreadCrumbs1.Level = 1;
        //    BreadCrumbs1.TailName = "Home";
        //}
        //Response.Write(@"<script>console.log('HOME page_load - " + DateTime.Now.ToString() + @"');</script>");
    }

}
