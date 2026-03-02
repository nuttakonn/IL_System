using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using EB_Service.BLL.Authentication;
using ILSystem.App_Code.Commons;

public partial class DesktopService_Home_Home : System.Web.UI.Page
{
    UserInfoService userInfoService;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        if (userInfoService.GetUserInfo() == null || !Page.User.Identity.IsAuthenticated || Request["SignOut"] == "true")
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Request.Cookies.Clear();
            Response.Redirect(FormsAuthentication.LoginUrl);
            //            Response.Redirect(Request.ApplicationPath + "/Default.aspx");
        }

        
        
    }

   
}
