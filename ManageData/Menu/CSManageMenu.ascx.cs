using EB_Service.Commons;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using ILSystem.App_Code.BLL.System.Menu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSManage.ManageData.Menu
{
    public partial class CSManageMenu : System.Web.UI.UserControl
    {
        private UserInfo m_UserInfo = null;
        public UserInfoService userInfoService;

        // private Hashtable htBreadcrumbs = null; 
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            m_UserInfo = userInfoService.GetUserInfo();
            if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
            {
                try
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                }
                catch { }
                Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

            }

            //multiMainMenu.TopMenuGroup.MultiMenuItems. = CreateMainMenu_OnProject();
            //if (!Page.IsPostBack)
            //{
            ASPxSystemMenu.Items.Clear();
            //CreateMenu();
            GetMenu(); //Menu ITaas
            //}
        }

        private void CreateMenu()
        {
            MenuBLL obj = new MenuBLL();
            userInfoService = new UserInfoService();
            obj.UserInformation = userInfoService.GetUserInfo();
            DataSet DS = obj.GetDataSystemMenuFromUserID_PortalID();

            //string strCssHover = "background-color:#C0D6F4;border-bottom-color:#000080;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#000080;border-left-style:solid;border-left-width:1px;border-right-color:#000080;border-right-style:solid;border-right-width:1px;border-top-color:#000080;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;";
            //string strCssNormal = "background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:5px;padding-right:5px;padding-top:1px;";
            //string strCssSubMenu = "background-color:#FFFFE1;border-bottom-color:#002D96;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#002D96;border-left-style:solid;border-left-width:1px;border-right-color:#002D96;border-right-style:solid;border-right-width:1px;border-top-color:#002D96;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;";
            //// Put user code to initialize the page here
            //ASPxWaiveSystemMenu

            //    htBreadcrumbs = new Hashtable(); 


            // Home menu
            DevExpress.Web.ASPxMenu.MenuItem imnu = new DevExpress.Web.ASPxMenu.MenuItem();
            imnu.Text = "Home";

            imnu.NavigateUrl = Request.ApplicationPath + "/DesktopService/Home/Home.aspx";
            ASPxSystemMenu.Items.Add(imnu);

            //      htBreadcrumbs["Home.aspx"] = "Home"; 


            if (DS == null || DS.Tables.Count == 0)
                return;

            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                imnu = new DevExpress.Web.ASPxMenu.MenuItem();
                //imnu.HoverStyle.CssText = strCssHover;
                //imnu.NormalStyle.CssText = strCssNormal;
                //imnu.SubMenu.ExpandEffect.Type = EffectType.GlideTopToBottom;
                //imnu.SubMenu.CollapseEffect.Type = EffectType.GlideTopToBottom;
                //imnu.SubMenu.Style.CssText = strCssSubMenu;
                //imnu.SubMenu.SideImage = "Office2003SideBar2";
                ////imnu.SubMenu.Style.Font.Size = FontUnit.XSmall;  
                //imnu.SubMenu.LeftIconCellWidth = 25;
                ////Peak 05/02/2550 แก้ไข Query เปลี่ยนจาก Lib HM เป็น Lib GN
                ////imnu.Text = dr["T63ALI"].ToString();	
                ////CreateSubMenu(ref imnu, dr["T62MID"].ToString()); 

                string strCaption = dr["T02ALI"].ToString().Trim();
                imnu.Text = dr["T02ALI"].ToString().Trim();
                //string strCaption = dr["T02ALI"].ToString();
                CreateSubMenu(ref imnu, dr["T01MID"].ToString().Trim(), ref strCaption);
                ASPxSystemMenu.Items.Add(imnu);
            }
            imnu = new DevExpress.Web.ASPxMenu.MenuItem();
            //imnu.HoverStyle.CssText = strCssHover;
            //imnu.NormalStyle.CssText = strCssNormal;
            imnu.Text = "Sign Out";
            imnu.NavigateUrl = Request.ApplicationPath + "/DesktopService/Home/Home.aspx?SignOut=true";
            ASPxSystemMenu.Items.Add(imnu);

            //     Session["Hash_BreadCrumbs"] = htBreadcrumbs;

            return;
        }

        private bool CreateSubMenu(ref DevExpress.Web.ASPxMenu.MenuItem mnuParent, string strParentID, ref string strCaption)
        {
            bool bRet = false;
            MenuBLL obj = new MenuBLL();
            userInfoService = new UserInfoService();
            obj.UserInformation = userInfoService.GetUserInfo();
            DataSet DS = obj.GetDataSystemSubMenuByParentID(strParentID);

            if (DS != null || DS.Tables.Count > 0)
            {
                if (DS.Tables[0].Rows.Count > 0)
                    bRet = true;
            }
            //string strCssHover = "background-color:#C0D6F4;border-bottom-color:#000080;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#000080;border-left-style:solid;border-left-width:1px;border-right-color:#000080;border-right-style:solid;border-right-width:1px;border-top-color:#000080;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;";
            //string strCssSubMenu = "background-color:#FFFFE1;border-bottom-color:#002D96;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#002D96;border-left-style:solid;border-left-width:1px;border-right-color:#002D96;border-right-style:solid;border-right-width:1px;border-top-color:#002D96;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;";
            foreach (DataRow dr in DS.Tables[0].Rows)
            {

                DevExpress.Web.ASPxMenu.MenuItem simnu = new DevExpress.Web.ASPxMenu.MenuItem();
                //string strKey = "";
                if (CreateSubMenu(ref simnu, dr["T01MID"].ToString().Trim(), ref strCaption))
                {

                    //simnu.HoverStyle.CssText = strCssHover;
                    //simnu.SubMenu.ExpandEffect.Type = EffectType.GlideTopToBottom;
                    //simnu.SubMenu.CollapseEffect.Type = EffectType.GlideTopToBottom;
                    //simnu.SubMenu.Style.CssText = strCssSubMenu;
                    //simnu.SubMenu.SideImage = "Office2003SideBar2";
                    //simnu.SubMenu.LeftIconCellWidth = 25;
                    ////strCaption = strCaption + "@" + dr["T02ALI"].ToString().Trim();
                }
                else
                {
                    //    strKey = dr["T02SOU"].ToString().Trim().Split('\\')[dr["T02SOU"].ToString().Trim().Split('\\').Length-1];
                    simnu.NavigateUrl = Request.ApplicationPath + dr["T02SOU"].ToString().Trim(); ////+ "?breadcrumb=" + strCaption + "@" + dr["T02ALI"].ToString().Trim();
                    simnu.Target = "myiframe";
                }
                //Peak 05/02/2550 แก้ไข Query เปลี่ยนจาก Lib HM เป็น Lib GN
                //				simnu.Text = dr["T63ALI"].ToString().Trim();


                simnu.Text = dr["T02ALI"].ToString().Trim();
                mnuParent.Items.Add(simnu);

            }
            return bRet;
        }

        //BOT Report
        public static string RootURL
        {
            get
            {
                var _root = System.Web.HttpContext.Current.Request;
                return _root.Url.Scheme + "://" + _root.Url.Authority + _root.ApplicationPath.TrimEnd('/');
            }
        }

        private void GetMenu()
        {
            try
            {
                UserInfo m_userInfo = userInfoService.GetUserInfo();

                string user = m_userInfo.Username;
                string pass = m_userInfo.Password;
                string PortalID = WebConfigurationManager.AppSettings["PortalID_ILHisun"].ToString().Trim(); 
                string strBizInit = "RL";
                try
                { 
                    strBizInit = WebConfigurationManager.AppSettings["BizforInitialLib"].ToString().Trim();
                }
                catch { }
                MenuBLL obj = new MenuBLL();
                UserInfo userInfo = new UserInfo();
                userInfo.Username = user; //Utility.Base64Decode(user);
                userInfo.Password = Utility.Base64Decode(pass);
                userInfo.PortalID = int.Parse(PortalID);

                obj.UserInformation = userInfoService.GetUserInfo();
                //obj.UserInformation = userInfo;

                #region GetMenuAuthen
                var logoutURL = WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim();

                var connect = new Connect();
                string url = WebConfigurationManager.AppSettings["AuthMenuAPIURL"].ToString().Trim();
                string action = "/AuthMenu/GetMenuAuthen";
                var jsonData = "{\"Data\":[{\"AppID\":\"" + PortalID + "\",\"EmpID\":\"" + userInfo.Username + "\"}]}";
                var response = connect.ConnectAPI(url, action, jsonData);
                #endregion

                //Home menu
                DevExpress.Web.ASPxMenu.MenuItem imnu = new DevExpress.Web.ASPxMenu.MenuItem();
                imnu.Text = "Home";
                imnu.NavigateUrl = logoutURL; //Request.ApplicationPath + "/ManageData/Home/Home.aspx";
                ASPxSystemMenu.Items.Add(imnu);

                if (response != null && response.Tables.Count > 0)
                {
                    var dtMenuAll = response.Tables[1];
                    var dtClone = dtMenuAll.Clone();
                    dtClone.Columns["MenuID"].DataType = Type.GetType("System.Int32");
                    foreach (DataRow dr in dtMenuAll.Rows)
                    {
                        dtClone.ImportRow(dr);
                    }
                    dtClone.AcceptChanges();
                    dtClone = dtClone.DefaultView.ToTable();
                    dtClone.DefaultView.Sort = "MenuID ASC";
                    dtMenuAll = dtClone.DefaultView.ToTable();

                    if (dtMenuAll.Rows.Count > 0)
                    {
                        DataRow[] parentMenus = dtMenuAll.Select("MenuParentID=0");
                        JArray arrMenu = new JArray();

                        foreach (var dr in parentMenus)
                        {
                            JObject objParent = new JObject();
                            bool haveChild = false;

                            string id = dr["MenuID"].ToString();
                            string name = dr["MenuAlias"].ToString();
                            string parentId = dr["MenuParentID"].ToString();
                            string link = dr["MenuPath"].ToString();

                            objParent.Add("MenuID", id);
                            objParent.Add("MenuText", name);

                            //Parent
                            imnu = new DevExpress.Web.ASPxMenu.MenuItem();
                            imnu.Text = name;
                            ASPxSystemMenu.Items.Add(imnu);

                            DataRow[] subMenu = dtMenuAll.Select(string.Format("MenuParentID='{0}'", id));

                            if (id == "1" && subMenu.Length == 0)
                            {
                                haveChild = false;
                            }
                            else
                            {
                                haveChild = true;
                            }

                            objParent.Add("IsParent", haveChild);

                            if (haveChild && subMenu.Length > 0 && !id.Equals(parentId))
                            {
                                JArray arrChild = new JArray();

                                foreach (var sub in subMenu)
                                {
                                    var haveSubChild = false;
                                    var subId = sub["MenuID"].ToString();
                                    var subparentId = sub["MenuParentID"].ToString();

                                    DataRow[] subChildMenu = dtMenuAll.Select(string.Format("MenuParentID='{0}'", subId));

                                    if (subChildMenu.Length > 0 && !subId.Equals(subparentId))
                                    {
                                        haveSubChild = true;
                                    }

                                    JObject objChild = new JObject();
                                    objChild.Add("MenuID", sub["MenuID"].ToString());
                                    objChild.Add("MenuText", sub["MenuAlias"].ToString());
                                    objChild.Add("IsParent", haveSubChild);
                                    objChild.Add("MenuLink", sub["MenuPath"].ToString());

                                    //Child
                                    DevExpress.Web.ASPxMenu.MenuItem simnu = new DevExpress.Web.ASPxMenu.MenuItem();
                                    //simnu.NavigateUrl = Request.ApplicationPath + sub["MenuPath"].ToString();
                                    if (sub["MenuPath"].ToString() != "")
                                    {
                                        simnu.NavigateUrl = RootURL + sub["MenuPath"].ToString();
                                    }
                                    simnu.Target = "myiframe";
                                    simnu.Text = sub["MenuAlias"].ToString();
                                    imnu.Items.Add(simnu);

                                    if (haveSubChild)
                                    {
                                        JArray arrSubChild = new JArray();

                                        foreach (var subChild in subChildMenu)
                                        {
                                            JObject objSubChild = new JObject();
                                            objSubChild.Add("MenuID", subChild["MenuID"].ToString());
                                            objSubChild.Add("MenuText", subChild["MenuAlias"].ToString());
                                            objSubChild.Add("IsParent", false);
                                            objSubChild.Add("MenuLink", subChild["MenuPath"].ToString());

                                            arrSubChild.Add(objSubChild);

                                            //SubChild
                                            DevExpress.Web.ASPxMenu.MenuItem scimnu = new DevExpress.Web.ASPxMenu.MenuItem();
                                            //scimnu.NavigateUrl = Request.ApplicationPath + subChild["MenuPath"].ToString();
                                            scimnu.NavigateUrl = RootURL + subChild["MenuPath"].ToString();
                                            scimnu.Target = "myiframe";
                                            scimnu.Text = subChild["MenuAlias"].ToString();
                                            simnu.Items.Add(scimnu);
                                        }

                                        objChild.Add("Child", arrSubChild);
                                    }

                                    arrChild.Add(objChild);
                                }

                                objParent.Add("Child", arrChild);
                            }
                            else
                            {
                                objParent.Add("MenuLink", link);
                            }
                            arrMenu.Add(objParent);
                        }

                    }
                }
                else
                {

                }
                imnu = new DevExpress.Web.ASPxMenu.MenuItem();
                imnu.Text = "Sign Out";

                var acn = HttpContext.Current.Request.Cookies["k"].ToString();
                if (acn == null || acn == "")
                {
                    imnu.NavigateUrl = Request.ApplicationPath + "/DesktopService/Home/Home.aspx?SignOut=true";
                }
                else
                {
                    //imnu.NavigateUrl = logoutURL;
                }

                //imnu.NavigateUrl = logoutURL; //Request.ApplicationPath + "/DesktopService/Home/Home.aspx?SignOut=true";
                ASPxSystemMenu.Items.Add(imnu);
            }
            catch (Exception ex)
            {

            }
        }

        protected void ASPxMenuSignOut_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            userInfoService = new UserInfoService();
            if (e.Item.Text == "Sign Out")
            {
                string accessKey = userInfoService.GetUserInfo().AccessKey;
                if (!string.IsNullOrEmpty(accessKey))
                {
                    try
                    {
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
        }
    }



}