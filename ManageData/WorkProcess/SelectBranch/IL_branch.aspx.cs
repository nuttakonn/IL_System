using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageData_WorkProcess_SelectBranch_IL_branch : System.Web.UI.Page
{

    ILDataCenter ilObj;
    ILDataCenterMssql iLDataCenterMssql;
    UserInfo userInfo;
    public UserInfoService userInfoService;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            catch { }
            Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

        }
        if (IsPostBack) { return; }
        bind_branch();
        PopupBranch.ShowOnPageLoad = true;

    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        iLDataCenterMssql = new ILDataCenterMssql(userInfo);
        userInfo.BranchApp = dd_branch.Value.ToString().Trim();
        HttpCookie CookiesBranch = new HttpCookie("branch", dd_branch.Value.ToString().Trim());
        CookiesBranch.Expires = DateTime.Now.AddMinutes(720);
        Response.Cookies.Add(CookiesBranch);
        Label lblSvr = (Label)Master.FindControl("lb_branch");
        DataSet ds = iLDataCenterMssql.getILTB01(userInfo.BranchApp.ToString());
        if (ilObj.check_dataset(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblSvr.Text = "Branch App: " + userInfo.BranchApp.ToString() + " [" + dr["T1BNME"].ToString().Trim() + "]";
        }
        else
        {
            lblSvr.Text = "Branch App: " + userInfo.BranchApp.ToString();
        }
        //lblSvr.Text = "Branch App: " + m_userInfo.BranchApp;
    }
    private void bind_branch()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();

            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssql(userInfo);
            DataSet ds = new DataSet();

            if (Cache["dsBranch"] != null)
            {
                ds = (DataSet)Cache["dsBranch"];
            }
            else
            {
                ds = iLDataCenterMssql.getILTB01();
                Cache["dsBranch"] = ds;
                Cache.Insert("dsBranch", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //DataSet ds = ilObj.getILTB01();
            dd_branch.Items.Clear();
            if (ilObj.check_dataset(ds))
            {

                //dd_branch.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["T1BRN"].ToString().Trim() != "301")
                    {
                        dd_branch.Items.Add(
                            new ListEditItem(dr["T1BRN"].ToString().Trim() + " : " + dr["T1BNME"].ToString().Trim(), dr["T1BRN"].ToString().Trim()));
                    }
                }

                dd_branch.SelectedIndex = 0;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    {
        try
        {
            Cache.Remove("dsBranch");
            bind_branch();
        }
        catch (Exception ex)
        {

        }
    }


}