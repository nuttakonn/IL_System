using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EB_Service.Commons;

public partial class CSManageMasterSiteMain_BlankSite : System.Web.UI.MasterPage
{
    private readonly As400DAL m_da400 = new As400DAL();
    private string dept = "";
    public UserInfoService userInfoService;
    public UserInfo userInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();

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
        //    Utility.WriteLogString("CSManageMasterSiteMain_BlankSite Session_Removed ==> ");
        //    HttpContext.Current.Request.Cookies.Clear();

        //    Session.RemoveAll();
        //    Session.Abandon();
        //    FormsAuthentication.SignOut();
        //    Response.Redirect(FormsAuthentication.LoginUrl);
        //}





        //lbBranch.Text = "[" + ((UserInfo)Session["UserInfo"]).BranchApp + "]";

        //Page.MaintainScrollPositionOnPostBack = true;
        ILDataCenter busobj = new ILDataCenter();
        userInfoService = new UserInfoService();
        UserInfo m_userInfo = userInfoService.GetUserInfo();
        busobj.UserInfomation = m_userInfo;

        DataSet DS = new DataSet();

        DS = busobj.RetriveAsDataSet("select at05dp from syfusdes " +
                                 "left join gnat04 on (usemid=at04em) " +
                                 "left join gnat05 on (at04jd=at05jd) " +
                                 "where uscode = '" + m_userInfo.Username + "' ");


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
        if ((string)e.CommandArgument == "SignOut")
        {
            HttpContext.Current.Request.Cookies.Clear();
            Session["UserInfo"] = null;
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }

    //public TalentToolsbar GetTalentToolsbar
    //{
    //    get { return TalentToolsbar1; }  
    //}



    //public ImageButton GetSaveButton
    //{
    //    get { return TalentToolsbar1.GetSaveButton; }
    //}
    //public ImageButton GetCancelButton
    //{
    //    get { return TalentToolsbar1.GetCancelButton; }
    //}
    //public ImageButton GetDeleteButton
    //{
    //    get { return TalentToolsbar1.GetDeleteButton; }
    //}
    //public ImageButton GetNewButton
    //{
    //    get { return TalentToolsbar1.GetNewButton; }
    //}
    //public ImageButton GetEditButton
    //{
    //    get { return TalentToolsbar1.GetEditButton; }
    //}
    //public ImageButton GetGotoListButton
    //{
    //    get { return TalentToolsbar1.GetGotoListButton; }
    //}
    //public ImageButton GetSearchButton
    //{
    //    get { return TalentToolsbar1.GetSearchButton; }
    //}

    //#region Set Enable Button

    //public bool SetEnableSaveButton
    //{
    // //   get { return TalentToolsbar1.btnSave.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableSaveButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetSaveButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "accept-32x32.png" : PathImg + "accept-32x32_Inactive.png");
    //    }
    //}
    //public bool SetEnableCancelButton
    //{
    // //   get { return TalentToolsbar1.btnCancel.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableCancelButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetCancelButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonRefresh.png" : PathImg + "ButtonRefresh_Inactive.png");
    //    }
    //}
    //public bool SetEnableDeleteButton
    //{
    //    //get { return TalentToolsbar1.btnSave.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableDeleteButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetDeleteButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "delete-32x32.png" : PathImg + "delete-32x32_Inactive.png");
    //    }
    //}
    //public bool SetEnableNewButton
    //{
    //    //get { return TalentToolsbar1.btnNew.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableNewButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetNewButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "add-32x32.png" : PathImg + "add-32x32_Inactive.png");
    //    }
    //}
    //public bool SetEnableEditButton
    //{
    //    //get { return TalentToolsbar1.btnEdit.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableEditButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetEditButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "edit-32x32.png" : PathImg + "edit-32x32_Inactive.png");
    //    }
    //}
    //public bool SetEnableGotoListButton
    //{
    //    //get { return TalentToolsbar1.btnGotoList.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableGotoListButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetGotoListButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonBack.png" : PathImg + "ButtonBack_Inactive.png");
    //    }
    //}
    //public bool SetEnableSearchButton
    //{
    //    //get { return TalentToolsbar1.btnSearch.Enabled; }
    //    set
    //    {
    //        TalentToolsbar1.SetEnableSearchButton = Convert.ToBoolean(value);
    //        TalentToolsbar1.GetSearchButton.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonSearch.png" : PathImg + "ButtonSearch_Inactive.png");
    //    }
    //}

    //#endregion

}