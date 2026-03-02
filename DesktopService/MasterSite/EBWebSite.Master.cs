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

namespace EBWebTemplate.DesktopService
{
    public partial class EBWebSite : System.Web.UI.MasterPage
    {
        public UserInfo userInfo;
        public UserInfoService userInfoService;
        private As400DAL m_da400 = new As400DAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            UserInfo m_userInfo = userInfoService.GetUserInfo();
            if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
            {
                try
                {
                    HttpContext.Current.Request.Cookies.Clear();
                    System.Web.Security.FormsAuthentication.SignOut();
                }
                catch { }
                Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

            }

            //if (Session["UserInfo"] == null || !Request.IsAuthenticated)
            //{
            //    Utility.WriteLogString("EBWebSite Session_Removed ==> ");
            //    Session.RemoveAll();
            //    Session.Abandon();
            //    FormsAuthentication.SignOut();
            //    Response.Redirect(FormsAuthentication.LoginUrl);
            //}

            lblUser.Text = "User: " + m_userInfo.Username; //((UserInfo)Session["UserInfo"]).Username;

            if (Session["EnFullName"] != null)
                lblUser.Text += "<br>" + Session["EnFullName"].ToString().Trim().Replace("|", " ");

            lblClientHost.Text = "[" + m_userInfo.LocalClient + "]";
            //spLabel.Visible = true;
            lblSvrName.Text = Server.MachineName.ToString();
            lblSvrName.Visible = true;

            ILDataCenter busobj = new ILDataCenter();
            userInfoService = new UserInfoService();
            m_userInfo = userInfoService.GetUserInfo();
            busobj.UserInfomation = m_userInfo;

            DataSet DS = new DataSet();

            DS = busobj.RetriveAsDataSet("select at05dp from syfusdes " +
                                     "left join gnat04 on (usemid=at04em) " +
                                     "left join gnat05 on (at04jd=at05jd) " +
                                     "where uscode = '" + m_userInfo.Username + "' ");
            if (DS != null)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    E_department.Text = dr["at05dp"].ToString().Trim();
                }
                DS.Clear();
            }
            m_userInfo.GDepartment = E_department.Text;
        }

        protected void lbtnSignOut_Command(object sender, CommandEventArgs e)
        {
            if ((string)e.CommandArgument == "SignOut")
            {
                Session["UserInfo"] = null;
                FormsAuthentication.SignOut();
                HttpContext.Current.Request.Cookies.Clear();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }
        }
    }
}