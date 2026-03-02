using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem
{
    public partial class Default : System.Web.UI.Page
    {
        UserInfoService userInfoService;
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            if (Request.QueryString["SignOut"] == "true")
            {
                FormsAuthentication.SignOut();
                Session["UserInfo"] = null;
                HttpContext.Current.Request.Cookies.Clear();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
                Response.Redirect(FormsAuthentication.LoginUrl);
            else
                Response.Redirect("DesktopService/Home/Home.aspx");
        }
    }
}