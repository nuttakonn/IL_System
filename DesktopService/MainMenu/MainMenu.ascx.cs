using EB_Service.BLL.Authentication;
using EB_Service.Commons;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EBWebTemplate.DesktopService.MainMenu
{
    public partial class MainMenu : System.Web.UI.UserControl
    {
        public UserInfoService userInfoService;
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            if (userInfoService.GetUserInfo() == null || !Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Request.Cookies.Clear();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }
            UserInfo m_userInfo = userInfoService.GetUserInfo();
            // AS400 Authentication mode
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0") {
                //dviewMainMenu.DataSource = Authen_MainSystemBLL.Get_MainSystemByUserName(userInfo.Username, userInfo.Password).Tables[0].DefaultView;

            }

            else
            {
                return;
            }

            dviewMainMenu.DataBind();
        }

        public void SystemID_OnCommand(Object source, CommandEventArgs e)
        {
            string sPassword = HttpUtility.UrlEncode(Utility.EncryptPassWord(((UserInfo)Session["UserInfo"]).Password));
            //-- 61053 : Check Authorize Copy Data [2015-03-27]
            string strFlgCopy = "Y";
                //Authen_MainSystemBLL.Get_Authorize_CopyData(((UserInfo)Session["UserInfo"]).Username, ((UserInfo)Session["UserInfo"]).Password);
            //


            ((UserInfo)Session["UserInfo"]).PortalID = Convert.ToInt32(e.CommandName);
            string cmdArg = (e.CommandArgument.ToString().ToLower().Trim().StartsWith("http") ? e.CommandArgument.ToString().Trim() :
                Request.ApplicationPath + e.CommandArgument.ToString().Trim());
            string strLinkPortal = String.Format("{0}?PortalID={1}&UserName={2}&Password={3}&AuthenCopy={4}", cmdArg, e.CommandName.Trim(),
                ((UserInfo)Session["UserInfo"]).Username, sPassword, strFlgCopy);



            Response.Redirect(Server.UrlPathEncode(strLinkPortal));

            //Response.Redirect("../../PilotProject/Home/Home.aspx");
        }
        public void ImgSystemID_OnCommand(Object source, CommandEventArgs e)
        {
            userInfoService = new UserInfoService();
            UserInfo userInfo = userInfoService.GetUserInfo();
            string sPassword = HttpUtility.UrlEncode(Utility.EncryptPassWord(((UserInfo)Session["UserInfo"]).Password));
            //-- 61053 : Check Authorize Copy Data [2015-03-27]
            string strFlgCopy = "Y";
            //Authen_MainSystemBLL.Get_Authorize_CopyData(((UserInfo)Session["UserInfo"]).Username, ((UserInfo)Session["UserInfo"]).Password);
            //

            ((UserInfo)Session["UserInfo"]).PortalID = Convert.ToInt32(e.CommandName);
            string cmdArg = (e.CommandArgument.ToString().ToLower().Trim().StartsWith("http") ? e.CommandArgument.ToString().Trim() :
                Request.ApplicationPath + e.CommandArgument.ToString().Trim());
            string strLinkPortal = String.Format("{0}?PortalID={1}&UserName={2}&Password={3}&AuthenCopy={4}", cmdArg, e.CommandName.Trim(),
                ((UserInfo)Session["UserInfo"]).Username, sPassword, strFlgCopy);



            Response.Redirect(Server.UrlPathEncode(strLinkPortal));

            //Response.Redirect("../../PilotProject/Home/Home.aspx");
        }

        protected void dviewMainMenu_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton imgbtn = new ImageButton();
                imgbtn.ID = "imgbtn" + Convert.ToString(((DataRowView)e.Item.DataItem).Row["SystemID"]);
                imgbtn.Width = Unit.Pixel(73);
                imgbtn.Width = Unit.Pixel(82);

                //imgbtn.ImageUrl = ((DataRowView)e.Item.DataItem).Row.IsNull("ImgUrl") ? ""
                //    : (string)((DataRowView)e.Item.DataItem).Row["ImgUrl"];
                imgbtn.ImageUrl = "../../Images/logo2.gif";
                imgbtn.CommandName = Convert.ToString(((DataRowView)e.Item.DataItem).Row["SystemID"]);
                imgbtn.CommandArgument = ((DataRowView)e.Item.DataItem).Row.IsNull("URL") ? ""
                    : (string)((DataRowView)e.Item.DataItem).Row["URL"];
                imgbtn.Command += new CommandEventHandler(ImgSystemID_OnCommand);
                e.Item.Controls.AddAt(0, imgbtn);


                LinkButton lnkbtn = new LinkButton();

                lnkbtn.ID = "lnkbtn" + Convert.ToString(((DataRowView)e.Item.DataItem).Row["SystemID"]);
                lnkbtn.Text = ((DataRowView)e.Item.DataItem).Row.IsNull("PortalName") ? ""
                    : (string)((DataRowView)e.Item.DataItem).Row["PortalName"];
                lnkbtn.CommandName = Convert.ToString(((DataRowView)e.Item.DataItem).Row["SystemID"]);
                lnkbtn.CommandArgument = ((DataRowView)e.Item.DataItem).Row.IsNull("URL") ? ""
                    : (string)((DataRowView)e.Item.DataItem).Row["URL"];
                lnkbtn.Command += new CommandEventHandler(ImgSystemID_OnCommand);
                e.Item.Controls.AddAt(1, lnkbtn);

            }
        }
    }
}