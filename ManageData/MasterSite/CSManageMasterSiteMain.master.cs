using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using ILSystem.App_Code.BLL.DataCenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EB_Service.Commons;

public partial class CSManageMasterSiteMain : System.Web.UI.MasterPage
{
    private As400DAL m_da400 = new As400DAL();
    private string dept = "";
    private DataCenter dataCenter;
    public UserInfo userInfo;
    public UserInfoService userInfoService;
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
        //    Utility.WriteLogString("CSManageMasterSiteMain Session_Removed ==> ");

        //    Session.RemoveAll();
        //    Session.Abandon();
        //    FormsAuthentication.SignOut();
        //    Response.Redirect(FormsAuthentication.LoginUrl);
        //}



        lblUser.Text = "User: " + userInfo.Username;
        if (Session["EnFullName"] != null)
            lblUser.Text += "<br>" + Session["EnFullName"].ToString().Trim().Replace("|", " ");

        lblClientHost.Text = "[" + userInfo.LocalClient + "]";
        //spLabel.Visible = true;
        lblSvrName.Text = Server.MachineName.ToString();
        lblSvrName.Visible = true;

        //lbBranch.Text = "[" + ((UserInfo)Session["UserInfo"]).BranchApp + "]";

        //Page.MaintainScrollPositionOnPostBack = true;
        ILDataCenter busobj = new ILDataCenter();
        //DataCenter dataCenter = new DataCenter();
        userInfoService = new UserInfoService();
        UserInfo m_userInfo = userInfoService.GetUserInfo();
        dataCenter = new DataCenter(m_userInfo);
        busobj.UserInfomation = m_userInfo;

        DataSet DS = new DataSet();
         //var resCustomer = dataCenter.Get<CustomerResponse>(cmdCustomer, CommandType.Text).Result;


        string sql = $@"select  AT05DP from AS400DB01.SYOD0000.SYFUSDES WITH (NOLOCK)
left join as400db01.GNOD0000.GNAT04  WITH (NOLOCK) on usemid = AT04EM
left join as400db01.GNOD0000.GNAT05   WITH (NOLOCK) on AT04JD = AT05JD
where USCODE = '{ m_userInfo.Username  }'";
        DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

        //DS = busobj.RetriveAsDataSet("select at05dp from syfusdes " +
        //                         "left join gnat04 on (usemid=at04em) " +
        //                         "left join gnat05 on (at04jd=at05jd) " +
        //                         "where uscode = '" + m_userInfo.Username + "' ");
        CookiesStorage cookiesStorage = new CookiesStorage();
        if (cookiesStorage.check_dataset(DS))
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                dept = dr["at05dp"].ToString().Trim();
                lblSvrName.Text = dr["at05dp"].ToString().Trim();
            }
            DS.Clear();
        }
        dataCenter.CloseConnectSQL();


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
            FormsAuthentication.SignOut();
            var acn = HttpContext.Current.Request.Cookies["k"].ToString();
            if (acn == null || acn == "")
            {
                HttpContext.Current.Request.Cookies.Clear();
                Session["UserInfo"] = null;
                Response.Redirect(FormsAuthentication.LoginUrl);
            }
            else
            {
                SignOutSSO();
            }


        }
    }

    public void SignOutSSO()
    {

        string accessKey = HttpContext.Current.Request.Cookies["k"].ToString();
        if (!string.IsNullOrEmpty(accessKey))
        {
            try
            {
                HttpContext.Current.Request.Cookies.Clear();

                var connect = new Connect();
                string url = WebConfigurationManager.AppSettings["SSOAPI"].ToString().Trim();
                string action = "/Authentication/SignOutSSO";
                JObject jsonData = new JObject();
                jsonData.Add("key", accessKey);
                var response = connect.ConnectAPI(url, action, JsonConvert.SerializeObject(jsonData), "POST");

                if (response.success)
                {
                    Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
                }
                else
                {
                    Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
                }
            }
            catch (Exception)
            {
                Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
            }
        }
        else
        {
            Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
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