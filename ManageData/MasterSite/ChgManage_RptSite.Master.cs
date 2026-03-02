using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChgManage_RptSite : System.Web.UI.MasterPage
{
    private string PathImg = ConfigurationManager.AppSettings["PathImgButton"].ToString().Trim();

    public UserInfoService userInfoService;
    public UserInfo m_userInfo;

    protected void Page_Load(object sender, EventArgs e)
    {

        userInfoService = new UserInfoService();
        m_userInfo = userInfoService.GetUserInfo();

        //dataCenter = new DataCenter(userInfoService.GetUserInfo());
        if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            catch { }
            Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

        }

        //if (Session["UserInfo"] == null || !Request.IsAuthenticated)
        //{
        //    HttpContext.Current.Request.Cookies.Clear();

        //    FormsAuthentication.SignOut();
        //    Response.Redirect(FormsAuthentication.LoginUrl);
        //}

        if (((UserInfo)Session["UserInfo"]).AuthenCopy.ToString().Trim() == "N")
        {
            //Block Control 
            System.Text.StringBuilder sbCtlr = new System.Text.StringBuilder();
            sbCtlr.Append(@"<script language='javascript'>");
            //Ctrl+C,Ctrl+X
            sbCtlr.Append(@"document.attachEvent('onkeydown', my_onkeydown_handler);");
            sbCtlr.Append(@"function my_onkeydown_handler()");
            sbCtlr.Append(@"{if(event.ctrlKey)");
            sbCtlr.Append(@"{if(event.keyCode==67)");
            sbCtlr.Append(@"{event.returnValue = false;");
            sbCtlr.Append(@"window.status = 'Disabled Ctrl+C';");
            sbCtlr.Append(@"alert('Not have authorize to copy data.');}");
            sbCtlr.Append(@"else if(event.keyCode==88)");
            sbCtlr.Append(@"{event.returnValue = false;");
            sbCtlr.Append(@"window.status = 'Disabled Ctrl+X';");
            sbCtlr.Append(@"alert('Not have authorize to copy data.');}}}");
            //Right Click
            sbCtlr.Append(@"var isNS = (navigator.appName == 'Netscape') ? 1 : 0;");
            sbCtlr.Append(@"if(navigator.appName == 'Netscape') document.captureEvents(Event.MOUSEDOWN||Event.MOUSEUP);");
            sbCtlr.Append(@"function mischandler()");
            sbCtlr.Append(@"{ return false; }");
            sbCtlr.Append(@"function mousehandler(e)");
            sbCtlr.Append(@"{var myevent = (isNS) ? e : event;");
            sbCtlr.Append(@"var eventbutton = (isNS) ? myevent.which : myevent.button;");
            sbCtlr.Append(@"if((eventbutton==2)||(eventbutton==3))");
            sbCtlr.Append(@"{alert('Not have authorize to copy data.');");
            sbCtlr.Append(@"window.status = 'Disabled Rigth Click.';");
            sbCtlr.Append(@"return false;}}");
            sbCtlr.Append(@"document.oncontextmenu = mischandler;");
            sbCtlr.Append(@"document.onmousedown = mousehandler;");
            sbCtlr.Append(@"document.onmouseup = mousehandler;");
            sbCtlr.Append(@"</script>");

            if (!Page.ClientScript.IsStartupScriptRegistered("JSScript"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "JSScript", sbCtlr.ToString());
            }

        }




        if (!IsPostBack)
        {
            BreadCrumbs1.Level = 1;
            BreadCrumbs1.TailName = "Home";
        }





    }


    protected void lbtnSignOut_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument == "SignOut")
        {
            HttpContext.Current.Request.Cookies.Clear();
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }



}