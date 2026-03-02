using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EBWebTemplate.DesktopService
{
    public partial class CSManage_BlankSite : System.Web.UI.MasterPage
    {
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

        //    System.Text.StringBuilder stb = new System.Text.StringBuilder();

        //    ((Control)Page.FindControl("body")).RenderControl(new HtmlTextWriter(new System.IO.StringWriter(stb)));
        //    string s = stb.ToString();
        //}
        UserInfoService userInfoService;
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            if (userInfoService.GetUserInfo() == null || !Request.IsAuthenticated)
            {
                HttpContext.Current.Request.Cookies.Clear();
                Session.RemoveAll();
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }





            lblUser.Text = "User: " + userInfoService.GetUserInfo().Username.ToString();
            if (Session["EnFullName"] != null)
                lblUser.Text += "<br>" + Session["EnFullName"].ToString().Trim().Replace("|", " ");

            lblClientHost.Text = "[" + userInfoService.GetUserInfo().LocalClient + "]";
            spLabel.Visible = true;
            lblSvrName.Text = Server.MachineName.ToString();
            lblSvrName.Visible = true;

            //Page.MaintainScrollPositionOnPostBack = true;

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
}