using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EBWebTemplate.DesktopService.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            TextBox txt = (TextBox)ctlLogin.FindControl("UserName");
            if (txt != null)
                txt.Attributes["onblur"] = "javascript:SetUpperText('" + txt.ClientID + "');";  //this.name



        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool SiteAuthority(string UserName, string Password)
        {
            System.Configuration.Configuration configuration =
                WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            bool bRet = false;

            AuthorizationSection aus = (AuthorizationSection)WebConfigurationManager.GetSection(
            "system.web/authorization");
            foreach (AuthorizationRule aur in aus.Rules)
            {
                if (aur.Users.IndexOf(UserName) >= 0)
                {
                    UserInfo info = new UserInfo();
                    info.Username = UserName;
                    info.Password = Utility.DecryptPassWord(Password);
                    Session["UserInfo"] = info;
                    bRet = true;
                    break;
                }
            }

            //String s = ctlLogin.UserName;
            return bRet;


        }

        protected void Login_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                UserInfo info = new UserInfo();
                if (!bool.Parse(WebConfigurationManager.AppSettings["IsAuthenticationMode"]))
                {

                    info.Username = ctlLogin.UserName;
                    info.Password = Utility.DecryptPassWord(ctlLogin.Password);
                    Session["UserInfo"] = info;
                    e.Authenticated = true;
                    return;
                }
                ArrayList ar = new ArrayList();


                // AS400 Authentication mode
                //if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")
                //{
                //    As400DAL dal = new As400DAL(ctlLogin.UserName.Trim().ToUpper(), ctlLogin.Password.Trim(),
                //        AppConfig.AS400AutLib, EB_Service.DAL.Providor400Types.PriSeries);

                //    if (dal.ConnectAs400() != true)
                //    {
                //        e.Authenticated = false;
                //        dal.CloseConnect();
                //        return;
                //    }

                //    if (!dal.As400Authentication(ref ar))
                //    {
                //        e.Authenticated = false;
                //        dal.CloseConnect();
                //        return;
                //    }
                //    dal.CloseConnect();
                //    IBM.Data.DB2.iSeries.iDB2Numeric iAdmin = new IBM.Data.DB2.iSeries.iDB2Numeric(1);

                //    if (ar.IndexOf(iAdmin) != -1)
                //        info.IsAdmin = true;
                //}

                //else
                //{

                //}
                e.Authenticated = true;

                info.Username = ctlLogin.UserName.Trim();
                info.Password = ctlLogin.Password.Trim();
                info.RolesID = ar;



                try
                {
                    info.LocalClient = (System.Net.Dns.GetHostEntry(Context.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
                    info.LocalClient = info.LocalClient.Length > 10 ? info.LocalClient.Substring(0, 10) : info.LocalClient;

                }
                catch { }
                Session["UserInfo"] = info;
                Session["TestAuth"] = "TestValueDesktop";
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(
                1,  // version
                ctlLogin.UserName.Trim(),      // login
                DateTime.Now,               // date de création
                DateTime.Now.AddDays(1),// expiration au bout de 20 minutes
                false,                      // dans un cookie non persistant
                "SysAuthenAS400",          // on sauvegarde les rôles dans le ticket
                FormsAuthentication.FormsCookiePath);
                // crypte le ticket
                string EncryptedTicket =
                FormsAuthentication.Encrypt(Ticket);
                // création d'un cookie d'authentification contenant le ticket
                HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);

                FormsAuthentication.SetAuthCookie(ctlLogin.UserName.Trim(), true);
                Response.Cookies.Add(Cookie);
                //Response.Cookies.Set(new HttpCookie("Quser", Login.UserName.ToUpper().Trim()));

                //            }
            }
            catch (Exception ex)
            {
                ctlLogin.FailureText = ex.Message;
            }
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {

        }
    }
}