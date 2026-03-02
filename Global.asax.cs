using EB_Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ILSystem
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //RemotingConfiguration.Configure(Server.MapPath("web.config"), true);
            Utility.WriteLogString("Application_Start");
            string strConfigpath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Web.config";
            RemotingConfiguration.Configure(strConfigpath, true);

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["init"] = 0;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string NomCookie = FormsAuthentication.FormsCookieName;
            HttpCookie Cookie = Context.Request.Cookies[NomCookie];
            //Utility.WriteLogString("Application_AuthenticateRequest");
            if (Cookie == null)
            {
                Utility.WriteLogString("Application_AuthenticateRequest Exception Cookie null");
                // pas authentifié
                return;
            }
            FormsAuthenticationTicket Ticket = null;
            try
            {
                Ticket = FormsAuthentication.Decrypt(Cookie.Value);
            }
            catch
            {
                Utility.WriteLogString("Application_AuthenticateRequest Exception Decrypt");
                return;
            }
            if (Ticket == null)
            {
                Utility.WriteLogString("Application_AuthenticateRequest Exception Ticket null");
                return;
            }
            string Role = Ticket.UserData;
            //Utility.WriteLogString(Ticket.UserData.ToString());

            //if(Request.IsAuthenticated)
            //{
            //    string[] roles = {"User"};//UserManagement.GetRoles(HttpContext.Current.User.Identity.Name);
            //    HttpContext.Current.User = new GenericPrincipal(
            //        HttpContext.Current.User.Identity, roles) ;
            //}

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Utility.WriteLogString("Application_Error");
            //Server.ClearError();
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                if (ex.GetBaseException() != null)
                {
                    ex = ex.GetBaseException();
                }

                if (Session != null)  // Exception occurred here.
                {
                    Session["LastException"] = ex;
                }
                Utility.WriteLogString(Server.GetLastError().Message);

                Utility.WriteLogString($"An Application Error Occurred {ex}" );
            }

        }

        protected void Session_End(object sender, EventArgs e)
        {

            Utility.WriteLogString("Session_End");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Utility.WriteLogString("Application_End");


        }
    }
}