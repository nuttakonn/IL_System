using EB_Service.DAL;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem.ManageData.WorkProcess.Campaign
{
    public partial class UpdateStatus : System.Web.UI.Page
    {
        protected DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        protected ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        protected ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        protected ILDataCenter ilObj = new ILDataCenter();
        protected DataCenter dataCenter;
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        protected string SqlAll = "";
        protected string Msg = "";
        protected string MsgHText = "";
        protected string checkMode = "";
        public CookiesStorage campaignStorage;
        protected string sqlBetween = "";
        protected string sqlCondition = "";
        protected string sqlChooseCampaign = "";
        protected string sqlRank = "";
        protected string rankVendor = "";
        protected string date97 = "";

        protected int startDateNews = 0;
        protected int endDateNews = 0;
        protected int closingDateNews = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            campaignStorage = new CookiesStorage();
            userInfoService = new UserInfoService();
            m_userInfo = userInfoService.GetUserInfo();
            dataCenter = new DataCenter(m_userInfo);
            if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
            {
                try
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                }
                catch { }
                Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

            }
            if (Page.IsPostBack)
            {

                return;
            }
            if (!IsPostBack)
            {
                DEFAULT_CHB();
                //DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
                DISIBLE_BOX_Note();

                btnMainSearch.Enabled = true;
                btnMainDetail.Enabled = false;
                rdbAllMybranch.Enabled = false;
                rdbAllBranch.Checked = true;

                CLEAR_LIST_CAMPAIGN();
                CALL_PROCEDURES_ILSR97();
                PopupAlertCenter();
            }
            
        }

        #region Functon call procedures ILSR97
        protected void CALL_PROCEDURES_ILSR97()
        {
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = date97;
            if (!String.IsNullOrEmpty(date97) && decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            else
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region  Function show popup alert
        private void PopupAlertCenter()
        {
            PopupAlertApp.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertApp.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupAlert.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupAlert.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductCode.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductCode.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Brand.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Brand.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Vendor.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Vendor.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Model.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Model.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductItem.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductItem.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

        }
        #endregion
        #region Function chang format date
        public bool CHECK_VALIDATE_DATE(string dateTime)
        {
            string[] formats = { "dd/MM/yyyy" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("th-TH"),
                                           DateTimeStyles.None, out parsedDateTime);
        }
        static string FORMAT_DATE(string dateCall)
        {
            DateTime dtime;
            DateTime.TryParseExact(dateCall, "yyyyMMdd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);
            string formattedDateNew = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return formattedDateNew;
        }
        static string CHANGE_FORMAT_DATE(string changeDateCall)
        {
            string[] splitDate = changeDateCall.ToString().Split('/');
            string changeDateNew = splitDate[2] + splitDate[1] + splitDate[0];
            return changeDateNew;
        }
        #endregion
        #region Function control form on change
        private void DEFAULT_CHB()
        {
            btnMainDetail.Enabled = false;

            CLEAR_LIST_CAMPAIGN();
            ENABLE_BOX_CAMPAIGN();
            DISIBLE_BOX_PRODUCT();
            DISIBLE_BOX_VENDOR();
            foreach (ListItem itemCampaign in chbCampaign.Items)
            {
                itemCampaign.Selected = true;
            }
            foreach (ListItem itemProduct in chbProduct.Items)
            {
                itemProduct.Selected = false;
            }
            foreach (ListItem itemProduct in chbVendor.Items)
            {
                itemProduct.Selected = false;
            }
        }
        protected void ONCHANGE_CHB_CAMPAIGN(object sender, EventArgs e)
        {
            btnMainDetail.Enabled = false;
            if (chbCampaign.SelectedIndex == 0)
            {
                CLEAR_LIST_CAMPAIGN();
                ENABLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
                foreach (ListItem itemProduct in chbProduct.Items)
                {
                    itemProduct.Selected = false;
                }
                foreach (ListItem itemVendor in chbVendor.Items)
                {
                    itemVendor.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN();
                DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
            }
        }
        protected void ONCHANGE_CHB_PRODUCT(object sender, EventArgs e)
        {
            btnMainDetail.Enabled = false;
            if (chbProduct.SelectedIndex == 0)
            {
                CLEAR_LIST_CAMPAIGN();
                DISIBLE_BOX_CAMPAIGN();
                ENABLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
                foreach (ListItem itemCampaign in chbCampaign.Items)
                {
                    itemCampaign.Selected = false;
                }
                foreach (ListItem itemVendor in chbVendor.Items)
                {
                    itemVendor.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN();
                DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
            }
        }
        protected void ONCHANGE_CHB_VENDOR(object sender, EventArgs e)
        {
            btnMainDetail.Enabled = false;
            if (chbVendor.SelectedIndex == 0)
            {
                DATA_ILMS16();
                CLEAR_LIST_CAMPAIGN();
                DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                ENABLE_BOX_VENDOR();
                foreach (ListItem itemCampaign in chbCampaign.Items)
                {
                    itemCampaign.Selected = false;
                }
                foreach (ListItem itemProduct in chbProduct.Items)
                {
                    itemProduct.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN();
                DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_PRODUCT();
                DISIBLE_BOX_VENDOR();
            }
        }

        private void DISIBLE_BOX_CAMPAIGN()
        {
            ListItem campaignStatusCampaignNew = campaignStatusCampaign.Items.FindByValue("N");
            campaignStatusCampaignNew.Selected = false;
            campaignID.Enabled = false;
            campaingName.Enabled = false;
            campaignStatusCampaign.Enabled = false;
            startDate.Enabled = false;
            CalendarExtender1.Enabled = false;
            calendarStart.Enabled = false;
            endDate.Enabled = false;
            CalendarExtender2.Enabled = false;
            calendarEnd.Enabled = false;
            closingDate.Enabled = false;
            CalendarExtender3.Enabled = false;
            calendarClosing.Enabled = false;
            serachCampaign.Enabled = false;

            campaignID.Text = "";
            campaingName.Text = "";

            startDate.Text = "";
            endDate.Text = "";
            closingDate.Text = "";

        }
        private void ENABLE_BOX_CAMPAIGN()
        {
            campaignID.Enabled = true;
            campaingName.Enabled = true;
            campaignStatusCampaign.Enabled = true;
            ListItem campaignStatusCampaignAll = campaignStatusCampaign.Items.FindByValue("ALL");
            campaignStatusCampaignAll.Enabled = false;
            ListItem campaignStatusCampaignNew = campaignStatusCampaign.Items.FindByValue("N");
            //campaignStatusCampaignNew.Enabled = false;
            campaignStatusCampaignNew.Selected = true;
            ListItem campaignStatusCampaignApprove = campaignStatusCampaign.Items.FindByValue("A");
            campaignStatusCampaignApprove.Enabled = false;
            ListItem campaignStatusCampaignX = campaignStatusCampaign.Items.FindByValue("X");
            campaignStatusCampaignX.Enabled = false;

            startDate.Enabled = true;
            CalendarExtender1.Enabled = true;
            calendarStart.Enabled = true;
            endDate.Enabled = true;
            CalendarExtender2.Enabled = true;
            calendarEnd.Enabled = true;
            closingDate.Enabled = true;
            CalendarExtender3.Enabled = true;
            calendarClosing.Enabled = true;
            serachCampaign.Enabled = true;
        }
        private void DISIBLE_BOX_PRODUCT()
        {
            productType.Enabled = false;
            serachProductType.Enabled = false;
            typeName.Enabled = false;
            productCode.Enabled = false;
            searchProductCode.Enabled = false;
            codeName.Enabled = false;
            brandTxt.Enabled = false;
            searchBranchTxt.Enabled = false;
            brandName.Enabled = false;
            modelTxt.Enabled = false;
            searchModel.Enabled = false;
            modelName.Enabled = false;
            productItem.Enabled = false;
            productItemDes.Enabled = false;
            searchProductItem.Enabled = false;
            productType.Text = "";
            typeName.Text = "";
            productCode.Text = "";
            codeName.Text = "";
            brandTxt.Text = "";
            brandName.Text = "";
            modelTxt.Text = "";
            modelName.Text = "";
            productItem.Text = "";
            productItemDes.Text = "";
        }
        private void ENABLE_BOX_PRODUCT()
        {


            productType.Enabled = true;
            serachProductType.Enabled = true;
            typeName.Enabled = true;
            productCode.Enabled = true;
            searchProductCode.Enabled = true;
            codeName.Enabled = true;
            brandTxt.Enabled = true;
            searchBranchTxt.Enabled = true;
            brandName.Enabled = true;
            modelTxt.Enabled = true;
            searchModel.Enabled = true;
            modelName.Enabled = true;
            productItem.Enabled = true;
            productItemDes.Enabled = true;
            searchProductItem.Enabled = true;
        }
        private void DISIBLE_BOX_VENDOR()
        {

            ListItem CampaigStatusAll = CampaigStatus.Items.FindByValue("ALL");
            CampaigStatusAll.Selected = true;
            //ListItem CampaigStatusAlls = CampaigStatus.Items.FindByValue("ALL");
            //CampaigStatusAlls.Enabled = false;
            //ListItem CampaigStatusNew = CampaigStatus.Items.FindByValue("N");
            //CampaigStatusNew.Enabled = false;
            //ListItem CampaigStatusX = CampaigStatus.Items.FindByValue("X");
            //CampaigStatusX.Enabled = false;
            //ListItem CampaigStatusA = CampaigStatus.Items.FindByValue("A");
            //CampaigStatusA.Enabled = false;
            vendorCode.Text = "";
            vendorName.Text = "";
            odTxt.Text = "";
            wqTxt.Text = "";
            oldVendorCode.Text = "";
            ddlRank.Items.Clear();
            CampaigStatus.Enabled = false;
            searchVendor.Enabled = false;
            vendorCode.Enabled = false;
            vendorName.Enabled = false;
            odTxt.Enabled = false;
            wqTxt.Enabled = false;
            oldVendorCode.Enabled = false;
            ddlRank.Enabled = false;
            startDateRank.Text = "";
            endDateRank.Text = "";
            startDateRank.Enabled = false;
            endDateRank.Enabled = false;
            calendarStartRank.Enabled = false;
            calendarEndRank.Enabled = false;
        }
        private void ENABLE_BOX_VENDOR()
        {

            ListItem CampaigStatusAll = CampaigStatus.Items.FindByValue("ALL");
            CampaigStatusAll.Selected = true;
            //ListItem CampaigStatusAlls = CampaigStatus.Items.FindByValue("ALL");
            //CampaigStatusAlls.Enabled = true;
            //ListItem CampaigStatusNew = CampaigStatus.Items.FindByValue("N");
            //CampaigStatusNew.Enabled = true;
            //ListItem CampaigStatusX = CampaigStatus.Items.FindByValue("X");
            //CampaigStatusX.Enabled = true;
            //ListItem CampaigStatusA = CampaigStatus.Items.FindByValue("A");
            //CampaigStatusA.Enabled = true;

            vendorCode.Enabled = true;
            vendorName.Enabled = true;
            odTxt.Enabled = true;
            wqTxt.Enabled = true;
            oldVendorCode.Enabled = true;
            CampaigStatus.Enabled = true;
            searchVendor.Enabled = true;
            ddlRank.Enabled = true;
            startDateRank.Enabled = true;
            endDateRank.Enabled = true;
            calendarStartRank.Enabled = true;
            calendarEndRank.Enabled = true;

        }
        private void DISIBLE_BOX_Note()
        {
            searchNote.Enabled = false;
            closeNote.Enabled = false;
        }
        #endregion

        #region Function about select campaign (FROM CAMPAIGN)
        protected void TEXT_CHANGE_CAMPAIGN(object sender, EventArgs e)
        {
            if (campaignID.Text == "")
            {
                campaingName.Text = "";
            }
        }
        protected void CLICK_SEARCH_CAMPAIGN_POPUP(object sender, EventArgs e)
        {
            popupCampaign();
        }
        protected void CLICK_SEARCH_CAMPAIGN(object sender, EventArgs e)
        {
            popupCampaign();
            Popup_Campaign.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_CAMPAIGN(object sender, EventArgs e)
        {
            ddlSearchCampaign.SelectedIndex = 0;
            txtSearchCampaign.Text = "";
            popupCampaign();
        }
        protected void campaign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Campaign.PageIndex = e.NewPageIndex;
            gv_Campaign.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value);
            gv_Campaign.DataBind();
        }
        protected void campaign_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_campaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value);;
            DataRow drCampaign = ds_campaign.Tables[0]?.Rows[(gv_Campaign.PageIndex * Convert.ToInt16(gv_Campaign.PageSize)) + e.NewSelectedIndex];
            campaignID.Text = drCampaign[0].ToString().Trim();
            //campaingName.Text = drCampaign[1].ToString().Trim();

            Popup_Campaign.ShowOnPageLoad = false;

            return;
        }
        private void popupCampaign()
        {
            string sqlwhere = "";
            if (productType.Text != "")
            {
                SqlAll = "SELECT C01CMP, C01CNM FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01CMP LIKE '" + productType.Text.Trim() + "%' AND  C01LTY = '01'";
            }
            else
            {
                SqlAll = "SELECT C01CMP, C01CNM FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01LTY = '01'";
            }


            if (ddlSearchCampaign.SelectedValue == "CCP" && txtSearchCampaign.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(C01CMP as nvarchar) = '" + txtSearchCampaign.Text.Trim() + "' AND C01LTY = '01' ORDER BY C01CMP ";
            }
            else if (ddlSearchCampaign.SelectedValue == "DCP" && txtSearchCampaign.Text.Trim() != "")
            {
                sqlwhere += " AND C01CNM like '" + txtSearchCampaign.Text.ToUpper().Trim() + "%' AND C01LTY = '01' ORDER BY C01CMP ";
            }
            else
            {
                sqlwhere += " ORDER BY C01CMP ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Campaign.DataSource = DS;
                gv_Campaign.DataBind();
            }
            else
            {
                gv_Campaign.DataSource = DS;
                gv_Campaign.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product type (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_TYPE(object sender, EventArgs e)
        {
            if (productType.Text == "")
            {
                typeName.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_TYPE_POPUP(object sender, EventArgs e)
        {
            popupProductType();
        }
        protected void CLICK_SELECT_PRODUCT_TYPE(object sender, EventArgs e)
        {
            popupProductType();
            Popup_ProductType.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCTTYPE(object sender, EventArgs e)
        {
            ddlSearchProducttype.SelectedIndex = 0;
            txtSearchProducttype.Text = "";
            popupProductType();
        }
        protected void productType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_ProductType.PageIndex = e.NewPageIndex;
            gv_ProductType.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            gv_ProductType.DataBind();
        }
        protected void productType_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productType = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            DataRow drProduct = ds_productType.Tables[0]?.Rows[(gv_ProductType.PageIndex * Convert.ToInt16(gv_ProductType.PageSize)) + e.NewSelectedIndex];
            productType.Text = drProduct[0].ToString().Trim();
            typeName.Text = drProduct[1].ToString().Trim();

            Popup_ProductType.ShowOnPageLoad = false;

            return;
        }
        private void popupProductType()
        {
            string sqlwhere = "";
            if (productType.Text != "")
            {
                SqlAll = "SELECT T40TYP, T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40TYP LIKE '" + productType.Text.Trim() + "%'";
            }
            else
            {
                SqlAll = "SELECT T40TYP, T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL = '' ";
            }


            if (ddlSearchProducttype.SelectedValue == "CPT" && txtSearchProducttype.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(T40TYP as nvarchar) = '" + txtSearchProducttype.Text.Trim() + "' AND T40DEL = '' ORDER BY T40TYP ";
            }
            else if (ddlSearchProducttype.SelectedValue == "DPT" && txtSearchProducttype.Text.Trim() != "")
            {
                sqlwhere += " AND T40DES like '" + txtSearchProducttype.Text.ToUpper().Trim() + "%' AND T40DEL = '' ORDER BY T40TYP ";
            }
            else
            {
                sqlwhere += " ORDER BY T40TYP ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvProductType.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_ProductType.DataSource = DS;
                gv_ProductType.DataBind();
            }
            else
            {
                gv_ProductType.DataSource = DS;
                gv_ProductType.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product code (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_CODE(object sender, EventArgs e)
        {
            if (productCode.Text == "")
            {
                codeName.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_CODE(object sender, EventArgs e)
        {
            popupProductCode();
        }
        protected void CLICK_SELECT_PRODUCT_CODE(object sender, EventArgs e)
        {
            popupProductCode();
            Popup_ProductCode.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCTCODE(object sender, EventArgs e)
        {
            ddlSearchProductCode.SelectedIndex = 0;
            txtSearchProductCode.Text = "";
            popupProductCode();
        }
        protected void productCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_ProductCode.PageIndex = e.NewPageIndex;
            gv_ProductCode.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            gv_ProductCode.DataBind();
        }
        protected void productCode_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productCode = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            DataRow drProductCode = ds_productCode.Tables[0]?.Rows[(gv_ProductCode.PageIndex * Convert.ToInt16(gv_ProductCode.PageSize)) + e.NewSelectedIndex];
            productCode.Text = drProductCode[1].ToString().Trim();
            codeName.Text = drProductCode[2].ToString().Trim();
            Popup_ProductCode.ShowOnPageLoad = false;

            return;
        }
        private void popupProductCode()
        {
            string sqlwhere = "";
            if (productType.Text != "")
            {
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode.Text + "%' AND CAST(T41TYP as nvarchar) = '" + productType.Text + "'";
            }
            else
            {
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode.Text + "%'";
            }



            if (ddlSearchProductCode.SelectedValue == "CPC" && txtSearchProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(T41COD as nvarchar) = '" + txtSearchProductCode.Text.Trim() + "' AND T41DEL = '' ORDER BY T41TYP";
            }
            else if (ddlSearchProductCode.SelectedValue == "DPC" && txtSearchProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41DES like '" + txtSearchProductCode.Text.ToUpper().Trim() + "%' AND T41DEL = '' ORDER BY T41TYP";
            }
            else
            {
                sqlwhere += " AND T41DEL = '' ORDER BY T41TYP";
            }

            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvProductCode.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_ProductCode.DataSource = DS;
                gv_ProductCode.DataBind();
            }
            else
            {
                gv_ProductCode.DataSource = DS;
                gv_ProductCode.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select brand (FROM PRODUCT)
        protected void TEXT_CHANGE_BRAND(object sender, EventArgs e)
        {
            if (brandTxt.Text == "")
            {
                brandName.Text = "";
            }
        }
        protected void CLICK_SEARCH_BRAND_POPUP(object sender, EventArgs e)
        {
            popupBrand();
        }
        protected void CLICK_SELECT_BRAND(object sender, EventArgs e)
        {
            popupBrand();
            Popup_Brand.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_BRAND(object sender, EventArgs e)
        {
            ddlSearchBrands.SelectedIndex = 0;
            txtSearchBrands.Text = "";
            popupBrand();
        }
        protected void brand_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Brand.PageIndex = e.NewPageIndex;
            gv_Brand.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            gv_Brand.DataBind();
        }
        protected void brand_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Brand = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            DataRow drBrand = ds_Brand.Tables[0]?.Rows[(gv_Brand.PageIndex * Convert.ToInt16(gv_Brand.PageSize)) + e.NewSelectedIndex];
            brandTxt.Text = drBrand[0].ToString().Trim();
            brandName.Text = drBrand[1].ToString().Trim();

            Popup_Brand.ShowOnPageLoad = false;

            return;
        }
        private void popupBrand()
        {
            string sqlwhere = "";
            SqlAll = "SELECT T42BRD, T42DES FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)";

            if (ddlSearchBrands.SelectedValue == "CB" && txtSearchBrands.Text.Trim() != "")
            {
                sqlwhere += " WHERE CAST(T42BRD as nvarchar) = '" + txtSearchBrands.Text.Trim() + "' AND T42DEL = '' ORDER BY T42BRD ";
            }
            else if (ddlSearchBrands.SelectedValue == "DB" && txtSearchBrands.Text.Trim() != "")
            {
                sqlwhere += " WHERE T42DES like '" + txtSearchBrands.Text.ToUpper().Trim() + "%' AND T42DEL = '' ORDER BY T42BRD ";
            }
            else
            {
                sqlwhere += " WHERE T42DEL = '' ORDER BY T42BRD ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvProductType.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Brand.DataSource = DS;
                gv_Brand.DataBind();
            }
            else
            {
                gv_Brand.DataSource = DS;
                gv_Brand.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select model (FROM PRODUCT)
        protected void TEXT_CHANGE_MODEL(object sender, EventArgs e)
        {
            if (modelTxt.Text == "")
            {
                modelName.Text = "";
            }
        }
        protected void CLICK_SEARCH_MODEL_POPUP(object sender, EventArgs e)
        {
            popupModel();
        }
        protected void CLICK_SELECT_MODEL(object sender, EventArgs e)
        {
            popupModel();
            Popup_Model.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_MODEL(object sender, EventArgs e)
        {
            ddlSearchProducttype.SelectedIndex = 0;
            txtSearchProducttype.Text = "";
            popupModel();
        }
        protected void model_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Model.PageIndex = e.NewPageIndex;
            gv_Model.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel.Value);
            gv_Model.DataBind();
        }
        protected void model_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Model = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel.Value);
            DataRow drModel = ds_Model.Tables[0]?.Rows[(gv_Model.PageIndex * Convert.ToInt16(gv_Model.PageSize)) + e.NewSelectedIndex];
            modelTxt.Text = drModel[0].ToString().Trim();
            modelName.Text = drModel[1].ToString().Trim();

            Popup_Model.ShowOnPageLoad = false;

            return;
        }
        private void popupModel()
        {
            string sqlwhere = "";
            if (productType.Text != "" && productCode.Text != "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T43COD as nvarchar) ='" + productCode.Text.Trim() + "' AND CAST(T43BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43COD as nvarchar) ='" + productCode.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43COD as nvarchar) ='" + productCode.Text.Trim() + "' AND CAST(T43BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T43BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T43COD as nvarchar) ='" + productCode.Text.Trim() + "'";
            }
            else
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%'";
            }


            if (ddlSearchModel.SelectedValue == "CMD" && txtSearchModel.Text.Trim() != "")
            {
                sqlwhere += " AND T43MDL= " + txtSearchModel.Text.Trim() + " AND T43DEL = '' ORDER BY T43MDL ";
            }
            else if (ddlSearchModel.SelectedValue == "DMD" && txtSearchModel.Text.Trim() != "")
            {
                sqlwhere += " AND T43DES like '" + txtSearchModel.Text.ToUpper().Trim() + "%' AND T43DEL = '' ORDER BY T43MDL ";
            }
            else
            {
                sqlwhere += " AND T43DEL = '' ORDER BY T43MDL ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvModel.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Model.DataSource = DS;
                gv_Model.DataBind();
            }
            else
            {
                gv_Model.DataSource = DS;
                gv_Model.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product item (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_ITEM(object sender, EventArgs e)
        {
            if (productItem.Text == "")
            {
                productItemDes.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_ITEM_POPUP(object sender, EventArgs e)
        {
            popupProductItem();
        }
        protected void CLICK_SELECT_PRODUCT_ITEM(object sender, EventArgs e)
        {
            popupProductItem();
            Popup_ProductItem.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCT_ITEM(object sender, EventArgs e)
        {
            ddlSearchProductItem.SelectedIndex = 0;
            txtSearchProductItem.Text = "";
            popupProductItem();
        }
        protected void productItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_productItem.PageIndex = e.NewPageIndex;
            gv_productItem.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem.Value);
            gv_productItem.DataBind();
        }
        protected void productItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_PoductItem = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem.Value);
            DataRow drProductItem = ds_PoductItem.Tables[0]?.Rows[(gv_productItem.PageIndex * Convert.ToInt16(gv_productItem.PageSize)) + e.NewSelectedIndex];
            productItem.Text = drProductItem[0].ToString().Trim();
            productItemDes.Text = drProductItem[1].ToString().Trim();

            Popup_ProductItem.ShowOnPageLoad = false;

            return;
        }
        private void popupProductItem()
        {

            string sqlwhere = "";
            if (productType.Text != "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' AND CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }



            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }



            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType.Text.Trim() + "' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode.Text.Trim() + "' CAST(T44BRD as nvarchar) ='" + brandTxt.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt.Text.Trim() + "'";
            }
            else
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)  WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N'";
            }


            if (ddlSearchProductItem.SelectedValue == "CPI" && txtSearchProductItem.Text.Trim() != "")
            {
                sqlwhere += " AND T44ITM = '" + txtSearchProductItem.Text.Trim() + "' AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
            }
            else if (ddlSearchProductItem.SelectedValue == "DPI" && txtSearchProductItem.Text.Trim() != "")
            {
                sqlwhere += " AND T44DES like '" + txtSearchProductItem.Text.ToUpper().Trim() + "%' AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
            }
            else
            {
                sqlwhere += " AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvProductItem.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_productItem.DataSource = DS;
                gv_productItem.DataBind();
            }
            else
            {
                gv_productItem.DataSource = DS;
                gv_productItem.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select vendor (FROM VENDOR)
        protected void TEXT_CHANGE_VENDOR(object sender, EventArgs e)
        {
            if (vendorCode.Text == "")
            {
                vendorName.Text = "";
                odTxt.Text = "";
                wqTxt.Text = "";
                oldVendorCode.Text = "";
                ddlRank.Items.Clear();
                startDateRank.Text = "";
                endDateRank.Text = "";
            }
            //else if (vendorCode.Text != "" && int.Parse(vendorCode.Text.Length.ToString()) >= 9)
            //{

            //        SqlAll = "SELECT P16RNK,SubStr(P16STD, 7, 2)|| '/' ||SubStr(P16STD, 5, 2)|| '/' ||SubStr(P16STD, 1, 4)|| '@' ||SubStr(P16END, 7, 2)|| '/' ||SubStr(P16END, 5, 2)|| '/' ||SubStr(P16END, 1, 4) AS txtDateRank " +
            //              " FROM ILMS16 WHERE P16VEN ='" + vendorCode.Text + "' ORDER BY  P16STD ASC ";
            //        DataSet ds_RankVendor = new DataSet();
            //        m_userInfo = userInfoService.GetUserInfo();
            //        ilObj.UserInfomation = m_userInfo;
            //        ds_RankVendor = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            //        if (ds_RankVendor.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow drVendorRank = ds_RankVendor.Tables[0].Rows[0];
            //            string[] dateRanks = drVendorRank[1].ToString().Trim().Split('@');
            //            startDateRank.Text = "";
            //            endDateRank.Text = "";
            //            //startDateRank.Text = dateRanks[0];
            //            //endDateRank.Text = dateRanks[1];
            //            //ListItem lst = new ListItem("Add New", "0");

            //            //ddlRank.Items.Insert(ddlRank.Items.Count - 1, lst);
            //            ddlRank.DataSource = ds_RankVendor;
            //            ddlRank.DataTextField = "P16RNK";
            //            ddlRank.DataValueField = "txtDateRank";
            //            ddlRank.DataBind();
            //            ddlRank.Items.Insert(0, "All Rank");
            //            ddlRank.Enabled = true;
            //        }
            //        else
            //        {
            //            ddlRank.Enabled = false;
            //        }

            //        dataCenter.CloseConnectSQL();
            //}
        }
        protected void ONCHANGE_DATE_RANK(object sender, EventArgs e)
        {
            string[] dateRanks = ddlRank.SelectedValue.ToString().Trim().Split('@');
            if (ddlRank.SelectedIndex > 0)
            {
                startDateRank.Text = dateRanks[0];
                endDateRank.Text = dateRanks[1];
            }
            else if (ddlRank.SelectedIndex == 0)
            {
                startDateRank.Text = "";
                endDateRank.Text = "";
            }
        }
        protected void CLICK_SEARCH_VENDOR_POPUP(object sender, EventArgs e)
        {
            popupVendor();
        }
        protected void CLICK_SEARCH_VENDOR(object sender, EventArgs e)
        {
            popupVendor();
            Popup_Vendor.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_VENDOR(object sender, EventArgs e)
        {
            ddlSearchVendor.SelectedIndex = 0;
            txtSearchVendor.Text = "";
            popupVendor();
        }
        protected void gvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendor.PageIndex = e.NewPageIndex;
            gvVendor.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor.Value);
            gvVendor.DataBind();
        }
        protected void gvVendor_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Vendor = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor.Value);
            DataRow drVendor = ds_Vendor.Tables[0]?.Rows[(gvVendor.PageIndex * Convert.ToInt16(gvVendor.PageSize)) + e.NewSelectedIndex];
            vendorCode.Text = drVendor[0].ToString().Trim();
            //vendorName.Text = drVendor[1].ToString().Trim();
            //oldVendorCode.Text = drVendor[2].ToString().Trim();
            //odTxt.Text = drVendor[3].ToString().Trim();
            //wqTxt.Text = drVendor[4].ToString().Trim();

            //SqlAll = "SELECT P16RNK,SubStr(P16STD, 7, 2)|| '/' ||SubStr(P16STD, 5, 2)|| '/' ||SubStr(P16STD, 1, 4)|| '@' ||SubStr(P16END, 7, 2)|| '/' ||SubStr(P16END, 5, 2)|| '/' ||SubStr(P16END, 1, 4) AS txtDateRank " +
            //         " FROM ILMS16 WHERE P16VEN ='" + vendorCode.Text + "' ORDER BY  P16STD ASC ";
            //DataSet ds_RankVendor = new DataSet();
            //m_userInfo = userInfoService.GetUserInfo();
            //
            //ilObj.UserInfomation = m_userInfo;
            //ds_RankVendor = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            //if (ds_RankVendor.Tables[0].Rows.Count > 0)
            //{
            //    DataRow drVendorRank = ds_RankVendor.Tables[0].Rows[0];
            //    string[] dateRanks = drVendorRank[1].ToString().Trim().Split('@');
            //    startDateRank.Text = "";
            //    endDateRank.Text = "";
            //    //startDateRank.Text = dateRanks[0];
            //    //endDateRank.Text = dateRanks[1];
            //    //ListItem lst = new ListItem("Add New", "0");

            //    //ddlRank.Items.Insert(ddlRank.Items.Count - 1, lst);
            //    ddlRank.DataSource = ds_RankVendor;
            //    ddlRank.DataTextField = "P16RNK";
            //    ddlRank.DataValueField = "txtDateRank";
            //    ddlRank.DataBind();
            //    ddlRank.Items.Insert(0, "All Rank");
            //    ddlRank.Enabled = true;
            //}
            //else
            //{
            //    ddlRank.Enabled = false;
            //}

            dataCenter.CloseConnectSQL();
            Popup_Vendor.ShowOnPageLoad = false;

            return;
        }
        private void popupVendor()
        {
            string sqlwhere = "";
            if (vendorCode.Text != "")
            {
                SqlAll = "SELECT P10VEN,P10NAM,P10FIL,P12ODR,P12WOR FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN WHERE P10VEN LIKE'" + vendorCode.Text + "%'";
            }
            else
            {
                SqlAll = "SELECT P10VEN,P10NAM,P10FIL,P12ODR,P12WOR FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN WHERE P10VEN LIKE'" + vendorCode.Text + "%'";
            }


            if (ddlSearchVendor.SelectedValue == "CV" && txtSearchVendor.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(P10VEN AS varchar) = '" + txtSearchVendor.Text.Trim() + "' AND P10DEL = ''  ORDER BY P10NAM, P10VEN";
            }
            else if (ddlSearchVendor.SelectedValue == "DV" && txtSearchVendor.Text.Trim() != "")
            {
                sqlwhere += " AND P10NAM like '" + txtSearchVendor.Text.ToUpper().Trim() + "%' AND P10DEL = ''  ORDER BY P10NAM, P10VEN";
            }
            else
            {
                sqlwhere += " AND P10DEL = '' ORDER BY P10NAM, P10VEN ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvVendor.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gvVendor.DataSource = DS;
                gvVendor.DataBind();
            }
            else
            {
                gvVendor.DataSource = DS;
                gvVendor.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function search main for page
        protected void CLICK_MAIN_SEARCH(object sender, EventArgs e)
        {
            if (chbCampaign.SelectedValue == "" && chbProduct.SelectedValue == "" && chbVendor.SelectedValue == "")
            {
                //lblMsgAlert.Text = "Please select type for searching !";
                //popupAlert.ShowOnPageLoad = true;
                LIST_DATA_NOT_SELECT();
            }
            else if (chbCampaign.SelectedValue == "1" && chbProduct.SelectedValue == "" && chbVendor.SelectedValue == "")
            {
                LIST_DATA_SELECT_CAMPAIGN();
            }
            else if (chbCampaign.SelectedValue == "" && chbProduct.SelectedValue == "1" && chbVendor.SelectedValue == "")
            {
                LIST_DATA_SELECT_PRODUCT();
            }
            else if (chbCampaign.SelectedValue == "" && chbProduct.SelectedValue == "" && chbVendor.SelectedValue == "1")
            {
                LIST_DATA_SELECT_VENDOR();
            }
            else
            {
                gv_listCampaign.DataSource = null;
                gv_listCampaign.DataBind();
            }

            return;
        }

        private void SET_MSG()
        {
            lblMsg.Text = Msg;
            PopupMsg.HeaderText = MsgHText;
            PopupMsg.ShowOnPageLoad = true;
        }


        protected void GV_LIST_CAMPAIGN_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gv_listCampaign, "Select$" + e.Row.RowIndex);
                }

                GridViewRow gv_listCampaign = e.Row;
                gv_listCampaign.Cells[0].Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(gv_listCampaign.Cells[0].Text.ToString().Trim()));
                gv_listCampaign.Cells[1].Text = gv_listCampaign.Cells[1].Text.ToString().PadLeft(3, '0');

                DateTime dtime1;
                DateTime.TryParseExact(gv_listCampaign.Cells[4].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime1);
                string formattedDate1 = dtime1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listCampaign.Cells[4].Text = formattedDate1;

                DateTime dtime2;
                DateTime.TryParseExact(gv_listCampaign.Cells[5].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime2);
                string formattedDate2 = dtime2.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listCampaign.Cells[5].Text = formattedDate2;

                DateTime dtime3;
                DateTime.TryParseExact(gv_listCampaign.Cells[6].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime3);
                string formattedDate3 = dtime3.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listCampaign.Cells[6].Text = formattedDate3;


                if (gv_listCampaign.Cells[7].Text == "E")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fedcda");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#cc0a00");
                    e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#9E9E9E';this.style.color='#000A0F';";
                    e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#fedcda';this.style.color='#cc0a00';";

                }
                else if (gv_listCampaign.Cells[7].Text == "A")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#c9fecc");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#004123");
                    e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#9E9E9E';this.style.color='#000A0F';";
                    e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#c9fecc';this.style.color='#004123';";
                }
                else if (gv_listCampaign.Cells[7].Text == "N")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#65ACE4");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#004123");
                    e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#9E9E9E';this.style.color='#000A0F';";
                    e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#65ACE4';this.style.color='#004123';";
                }

            }
            catch (Exception)
            { }
        }

        protected void GV_LIST_CAMPAIGN_SELECTED_INDEX_CHANGE(object sender, GridViewSelectEventArgs e)
        {
            var txtProductTypeDescription = new TextBox();
            txtProductTypeDescription.Text = "";
            btnMainDetail.Enabled = true;
            DataSet ds_listCampaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value); 
            DataRow drlistCampaign = ds_listCampaign.Tables[0]?.Rows[(gv_listCampaign.PageIndex * Convert.ToInt16(gv_listCampaign.PageSize)) + e.NewSelectedIndex];
            //txtProductTypeDescription.Text = drlistCampaign[0].ToString().Trim();
            lblCopyCampaign.Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(drlistCampaign[0].ToString().Trim()));
            txtTitleCampaign.Text = "Campaign : ";

            DataSet ds_listCampaignSqev = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);

            SqlAll = "SELECT C11NSQ, C11UDT, C11UTM, C11UUS, C11NOT, C11NSQ, C11NOT FROM AS400DB01.ILOD0001.ILCP11 WITH (NOLOCK) WHERE CAST(C11CMP AS nvarchar) = '" + drlistCampaign[0] + "'";
            ds_listCampaign = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            int i = 0;
            int countList = ds_listCampaign.Tables[0].Rows.Count;
            string[] dateList = new string[countList];
            string[] timeList = new string[countList];
            string[] userList = new string[countList];
            string[] dashess = new string[countList];
            string[] detailCampaign1 = new string[countList];
            string[] detailCampaign2 = new string[countList];
            string[] sumDetail = new string[countList];


            foreach (DataRow row in ds_listCampaign.Tables[0].Rows)
            {

                DateTime dtimeListCmapaign;
                DateTime.TryParseExact(row["C11UDT"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtimeListCmapaign);
                string dateListCmapaign = dtimeListCmapaign.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                dateList[i] = dateListCmapaign;
                timeList[i] = String.Format("{0:##:##:##}", Convert.ToInt64(row["C11UTM"].ToString().Trim()));
                userList[i] = row["C11UUS"].ToString().Trim();
                detailCampaign1[i] = "<br><span style=\"margin-top:5px;margin-left:10px;\">" + row["C11NOT"].ToString() + "</span><br>";
                detailCampaign2[i] = "<span style=\"color:#0061C1;margin-top:5px;margin-left:10px;font-weight: bold;\">NOTE TIME:</span> " + dateList[i] + "<span style=\"color:#0061C1;font-weight: bold;\"> NOTE TIME: </span> " + timeList[i] + "<span style=\"color:#0061C1;font-weight: bold;\"> USER: </span>" + userList[i] + "<br>";
                dashess[i] = "<span style=\"margin-top:5px;margin-left:10px;\">------------------------------------------------------------------------------------------------------</span><br>";
                sumDetail[i] = detailCampaign1[i] + detailCampaign2[i] + dashess[i];

                txtProductTypeDescription.Text += sumDetail[i];
                txtProductTypeDescription.ForeColor = Color.Red;


                i++;
            }

            string dataTextList = txtProductTypeDescription.Text.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Javascript", "javascript:dataListNote('" + dataTextList + "');", true);
            Popup_ProductType.ShowOnPageLoad = false;
            return;
        }

        #endregion

        #region Function about not select form
        private void LIST_DATA_NOT_SELECT()
        {
            string date97 = "";
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = date97;
            if (!String.IsNullOrEmpty(date97) && decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            else
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            SqlAll = "SELECT DISTINCT(C01CMP) AS C01CMP, C01BRN, C01CTY, C01CNM, C01SDT, C01EDT, C01CLD, CASE WHEN C01EDT < " + date97 + " THEN 'E' ELSE C01CST END AS C01CST, C01LTY FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) " +
                    "JOIN AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) ON C01CMP=C09CMP WHERE C01CST = 'N' ";

            //SqlAll = "SELECT DISTINCT C01CMP,C01BRN,C01CTY,C01CNM,C01SDT, C01EDT, C01CLD,C01CST,C01LTY " +
            //         " FROM ILCP01 " +
            //         " JOIN ILCP09 ON C01CMP = C09CMP WHERE C01CST IN ('N','X') ";

            //SqlAll = SqlAll + sqlChooseCampaign + sqlCondition;

            DataSet DS = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
           
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_listCampaign.DataSource = DS;
                gv_listCampaign.DataBind();
            }
            else
            {
                lblMsgAlertApp.Text = "Data not found for searching ";
                PopupAlertApp.ShowOnPageLoad = true;
            }

            dataCenter.CloseConnectSQL();

        }
        #endregion

        #region List data from campaign
        private void LIST_DATA_SELECT_CAMPAIGN()
        {
            string date97 = "";
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = date97;
            if (!String.IsNullOrEmpty(date97) && decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            else
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            if (startDate.Text.Trim() != "")
            {
                string[] startSplitDate = startDate.Text.Trim().Split('/');
                string startDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
                startDateNews = int.Parse(startDateNew);
            }

            if (endDate.Text.Trim() != "")
            {
                string[] endtSplitDate = endDate.Text.Trim().Split('/');
                string endDateNew = endtSplitDate[2] + endtSplitDate[1] + endtSplitDate[0];
                endDateNews = int.Parse(endDateNew);
            }

            if (closingDate.Text.Trim() != "")
            {
                string[] closingSplitDate = closingDate.Text.Trim().Split('/');
                string closingDateNew = closingSplitDate[2] + closingSplitDate[1] + closingSplitDate[0];
                closingDateNews = int.Parse(closingDateNew);
            }

            if (campaignStatusCampaign.SelectedValue.ToString() == "ALL")
            {
                sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                sqlChooseCampaign = "  WHERE C01SDT != 0 AND C01EDT != 0";
            }
            else if (campaignStatusCampaign.SelectedValue.ToString() == "N")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                sqlChooseCampaign = " WHERE C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "' AND C01EDT >= " + date97 + "";
            }
            else if (campaignStatusCampaign.SelectedValue.ToString() == "A")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                sqlChooseCampaign = " WHERE C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "' AND C01EDT >= " + date97 + "";
            }
            else if (campaignStatusCampaign.SelectedValue.ToString() == "X")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                sqlChooseCampaign = " WHERE  C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "' OR C01EDT < " + date97 + "";
            }

            //update defect(AC-0044) 04 / 01 / 2564
            if (campaingName.Text.Trim() != "")
            {
                //1
                if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'";
                }
                //2
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT <= " + endDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "";
                }
                //3
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'  AND C01SDT <= " + endDateNews + "  AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' " + sqlBetween + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'  AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01CLD = " + closingDateNews + "";
                }
                //4
                else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' " + sqlBetween + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + "  AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + "";
                }

                //5
                else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                {
                    sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + " AND C01CLD = " + closingDateNews + "";
                }
            }
            //#
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01SDT =  " + startDateNews + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01EDT = " + endDateNews + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = sqlBetween;
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01SDT <= " + endDateNews + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() == ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween;
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() == "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() != "") && (startDate.Text.Trim() == "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + " AND C01CLD = " + closingDateNews + "";
            }
            else if ((campaignID.Text.Trim() == "") && (startDate.Text.Trim() != "") && (endDate.Text.Trim() != "") && (closingDate.Text.Trim() != ""))
            {
                sqlCondition = sqlBetween + " AND C01CLD = " + closingDateNews + "";
            }

            //WHERE C01EDT >= " + date97 +
            SqlAll = "SELECT DISTINCT(C01CMP) AS C01CMP, C01BRN, C01CTY, C01CNM, C01SDT, C01EDT, C01CLD, CASE WHEN C01EDT < " + date97 + " THEN 'E' ELSE C01CST END AS C01CST, C01LTY FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) " +
                " JOIN AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) ON C01CMP=C09CMP ";

            SqlAll = SqlAll + sqlChooseCampaign + sqlCondition + " ORDER BY C01CMP";

            DataSet DS = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_listCampaign.DataSource = DS;
                gv_listCampaign.DataBind();
            }
            else
            {
                lblMsgAlertApp.Text = "Data not found for searching ";
                PopupAlertApp.ShowOnPageLoad = true;
            }

            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region List data from product
        private void LIST_DATA_SELECT_PRODUCT()
        {
            string date97 = "";
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = date97;
            if (!String.IsNullOrEmpty(date97) && decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            else
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            //case sql search 1 unit
            if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition = " WHERE CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }

            //case sql search 2 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }

            //case sql search 3 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }

            //case sql search 4 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }

            //case sql search 5 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP AS nvarchar) LIKE '%" + productType.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44COD AS nvarchar) LIKE '%" + productCode.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44BRD AS nvarchar) LIKE '%" + brandTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44MDL AS nvarchar) LIKE '%" + modelTxt.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%' OR C07PIT = 0";
                sqlCondition += " AND CAST(T44ITM AS nvarchar) LIKE '%" + productItem.Text.Trim() + "%' OR C07PIT = 0 AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%' OR C07PIT = 0)";
            }

            //case sql search 1 unit
            //update defect(AC-0044) 04 / 01 / 2564
            else if ((typeName.Text.Trim() != "") || (codeName.Text.Trim() != "") || (brandName.Text.Trim() != "") || (modelName.Text.Trim() != "") || (productItemDes.Text.Trim() != ""))
            {
                sqlCondition += " WHERE T44DEL = '' ";
                if (typeName.Text.Trim() != "")
                {
                    sqlCondition += " AND UPPER(T40DES) LIKE '%" + typeName.Text.ToUpper().Trim() + "%'";
                }
                if (codeName.Text.Trim() != "")
                {
                    sqlCondition += " AND UPPER(T41DES) LIKE '%" + codeName.Text.ToUpper().Trim() + "%'";
                }
                if (brandName.Text.Trim() != "")
                {
                    sqlCondition += " AND UPPER(T42DES) LIKE '%" + brandName.Text.ToUpper().Trim() + "%'";
                }
                if (modelName.Text.Trim() != "")
                {
                    sqlCondition += " AND UPPER(T43DES) LIKE '%" + modelName.Text.ToUpper().Trim() + "%'";
                }
                if (productItemDes.Text.Trim() != "")
                {
                    sqlCondition += " AND UPPER(T44DES) LIKE '%" + productItemDes.Text.ToUpper().Trim() + "%'";
                }
            }
            //#

            SqlAll = "SELECT C01CMP, C01BRN, C01CTY, C01CNM, C01SDT, C01EDT, C01CLD, CASE WHEN C01EDT < " + date97 + " THEN 'E' ELSE C01CST END AS C01CST, C01LTY FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) " +
               " WHERE C01CMP IN(SELECT DISTINCT C07CMP FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK) " +
               " JOIN AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) ON C07CMP = C09CMP AND C09BRN = '" + campaignStorage.GetCookiesStringByKey("branch") + "'" +
               " LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON T44ITM = C07PIT AND T44DEL = '' " +
               " LEFT JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON T44TYP = T40TYP AND T40DEL = '' " +
               " LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = '' " +
               " LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T44BRD = T42BRD AND T42DEL = '' " +
               " LEFT JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T44TYP = T43TYP AND T44BRD = T43BRD AND " +
               " T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''";

            //SqlAll = SqlAll + sqlCondition + " ";
            //SqlAll = SqlAll + sqlChooseCampaign + sqlCondition;
            //if (String.IsNullOrEmpty(sqlCondition.Trim()))
            //{
            //    SqlAll = SqlAll + sqlCondition + ") ";
            //}
            //else
            //{
            SqlAll = SqlAll + sqlCondition;
            //}

            DataSet DS = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_listCampaign.DataSource = DS;
                gv_listCampaign.DataBind();
            }
            else
            {
                lblMsgAlertApp.Text = "Data not found for searching ";
                PopupAlertApp.ShowOnPageLoad = true;
            }
            dataCenter.CloseConnectSQL();

        }
        #endregion

        #region List data from vendor
        private void DATA_ILMS16()
        {
            SqlAll = "SELECT DISTINCT UPPER(P16RNK) AS P16RNK FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ORDER BY P16RNK";
            DataSet DS_ILMS16 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_ILMS16 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ddlRank.DataSource = DS_ILMS16;
            ddlRank.DataTextField = "P16RNK";
            ddlRank.DataValueField = "P16RNK";
            ddlRank.Items.Insert(0, new ListItem("- -Select- -", ""));
            ddlRank.DataBind();
        }
        private void LIST_DATA_SELECT_VENDOR()
        {
            string date97 = "";
            string startDateRanks = "";
            string endDateRanks = "";
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = date97;
            if (!String.IsNullOrEmpty(date97) && decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            else
            {
                date97 = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            if (startDateRank.Text.Trim() != "")
            {
                startDateRanks = CHANGE_FORMAT_DATE(startDateRank.Text.ToString());
            }
            if (endDateRank.Text.Trim() != "")
            {
                endDateRanks = CHANGE_FORMAT_DATE(endDateRank.Text.ToString());
            }


            if (CampaigStatus.SelectedValue.ToString() == "ALL")
            {
                sqlBetween = " AND P16STD BETWEEN  " + startDateRanks + " AND " + endDateRanks + "";
                sqlChooseCampaign = "  AND C01CST != ''";
            }
            else if (CampaigStatus.SelectedValue.ToString() == "N")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                //sqlBetween = " AND P16STD >=  " + startDateRanks + " AND P16END <= " + endDateRanks + "";
                sqlBetween = " AND P16STD BETWEEN  " + startDateRanks + " AND " + endDateRanks + "";
                sqlChooseCampaign = " AND C01CST = '" + CampaigStatus.SelectedValue.ToString() + "' AND C01EDT >= " + date97 + "";
            }
            else if (CampaigStatus.SelectedValue.ToString() == "A")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                //sqlBetween = " AND P16STD >=  " + startDateRanks + " AND P16END <= " + endDateRanks + "";
                sqlBetween = " AND P16STD BETWEEN  " + startDateRanks + " AND " + endDateRanks + "";
                sqlChooseCampaign = " AND C01CST = '" + CampaigStatus.SelectedValue.ToString() + "' AND C01EDT >= " + date97 + "";
            }
            else if (CampaigStatus.SelectedValue.ToString() == "E")
            {
                //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                //sqlBetween = " AND P16STD >=  " + startDateRanks + " AND P16END <= " + endDateRanks + "";
                sqlBetween = " AND P16STD BETWEEN  " + startDateRanks + " AND " + endDateRanks + "";
                sqlChooseCampaign = " AND  C01CST = '" + CampaigStatus.SelectedValue.ToString() + "' OR C01EDT < " + date97 + "";
            }

            //if (vendorCode.Text.Trim() != "")
            //{
            //    sqlChooseCampaign += " AND C01VDC = " + vendorCode.Text.Trim();      
            //}

            if (vendorCode.Text.Trim() != "")
            {
                sqlCondition += " AND P10VEN = " + vendorCode.Text.Trim();
            }
            if (vendorName.Text.Trim() != "")
            {
                sqlCondition += " AND UPPER(P10NAM) LIKE '%" + vendorName.Text.ToUpper().Trim() + "%'";
            }
            if (oldVendorCode.Text.Trim() != "")
            {
                sqlCondition += " AND UPPER(P10FIL) LIKE '%" + oldVendorCode.Text.ToUpper().Trim() + "%'";
            }
            if (odTxt.Text.Trim() != "")
            {
                sqlCondition += " AND P12ODR = " + odTxt.Text.Trim();
            }
            if (wqTxt.Text.Trim() != "")
            {
                sqlCondition += " AND P12WOR = " + wqTxt.Text.Trim();
            }

            if (startDateRank.Text.Trim() != "" && endDateRank.Text.Trim() != "")
            {
                sqlCondition += " " + sqlBetween;

            }
            else if (startDateRank.Text.Trim() != "")
            {
                sqlCondition += "  AND P16STD = " + startDateRanks;

            }
            else if (endDateRank.Text.Trim() != "")
            {
                sqlCondition += "  AND P16END = " + endDateRanks;

            }

            if (ddlRank.SelectedValue.ToString() != "")
            {
                sqlCondition += " AND P16RNK ='" + ddlRank.SelectedValue.ToString() + "'";
            }

            //" + campaignStorage.GetCookiesStringByKey("branch") + "
            SqlAll = "SELECT C01CMP, C01BRN, C01CTY, C01CNM, C01SDT, C01EDT, C01CLD, CASE WHEN C01EDT < " + date97 + " THEN 'E' ELSE C01CST END AS C01CST, C01LTY FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) " +
                " JOIN AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) ON C01CMP = C09CMP AND C09BRN = '1'" +
                " WHERE C01CMP IN(SELECT DISTINCT(C08CMP) FROM AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK) " +
                " LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON C08VEN = P10VEN " +
                " LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON C08VEN = P12VEN " +
                " LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN  " +
                " WHERE (P16STS = '' or P16STS is null) AND (P12DEL = '' or P12DEL is null) AND (P10DEL = '' or P12DEL is null) " + sqlCondition + " ) " + sqlChooseCampaign + "";
            DataSet DS = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_listCampaign.DataSource = DS;
                gv_listCampaign.DataBind();
            }
            else
            {
                lblMsgAlertApp.Text = "Data not found for searching ";
                PopupAlertApp.ShowOnPageLoad = true;
            }

            dataCenter.CloseConnectSQL();

        }

        protected void SELECT_VENDOR_ONCHANGE(object sender, EventArgs e)
        {
            if (int.Parse(vendorCode.Text.Length.ToString()) >= 9)
            {
                SqlAll = "SELECT P16RNK,SUBSTRING(CAST(P16STD as nvarchar), 7, 2)+ '/' +SUBSTRING(CAST(P16STD as nvarchar), 5, 2)+ '/' +SUBSTRING(CAST(P16STD as nvarchar), 1, 4)+ '@' +SUBSTRING(CAST(P16END as nvarchar), 7, 2)+ '/' +SUBSTRING(CAST(P16END as nvarchar), 5, 2)+ '/' +SUBSTRING(CAST(P16END as nvarchar), 1, 4) AS txtDateRank " +
                      " FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE WHERE FORMAT(P16VEN,'000000000000') ='" + vendorCode.Text + "' ORDER BY  P16STD ASC ";
                DataSet ds_RankVendor = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                ds_RankVendor = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (ds_RankVendor.Tables[0]?.Rows.Count > 0)
                {
                    DataRow drVendorRank = ds_RankVendor.Tables[0]?.Rows[0];
                    string[] dateRanks = drVendorRank[1].ToString().Trim().Split('@');
                    startDateRank.Text = "";
                    endDateRank.Text = "";
                    //startDateRank.Text = dateRanks[0];
                    //endDateRank.Text = dateRanks[1];
                    //ListItem lst = new ListItem("Add New", "0");

                    //ddlRank.Items.Insert(ddlRank.Items.Count - 1, lst);
                    ddlRank.DataSource = ds_RankVendor;
                    ddlRank.DataTextField = "P16RNK";
                    ddlRank.DataValueField = "txtDateRank";
                    ddlRank.DataBind();
                    ddlRank.Items.Insert(0, "All Rank");
                    ddlRank.Enabled = true;
                }
                else
                {
                    ddlRank.Enabled = false;
                }

                dataCenter.CloseConnectSQL();


            }
            return;
        }
        #endregion

        #region clear data list campaign
        private void CLEAR_LIST_CAMPAIGN()
        {
            ViewState["ds_gvListCampaign"] = null;
            DataTable dtListCampaign = new DataTable();
            dtListCampaign.Columns.AddRange(new DataColumn[9] { new DataColumn("C01CMP"),
                new DataColumn("C01BRN"),
                new DataColumn("C01CTY"),
                new DataColumn("C01CNM"),
                new DataColumn("C01SDT"),
                new DataColumn("C01EDT"),
                new DataColumn("C01CLD"),
                new DataColumn("C01CST"),
                new DataColumn("C01LTY") });
            ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtListCampaign);
            ViewState["ds_gvListCampaign"] = dtListCampaign;
            gv_listCampaign.DataSource = dtListCampaign;
            gv_listCampaign.DataBind();
        }
        #endregion


        protected void CLICK_MAIN_DETAIL(object sender, EventArgs e)
        {
            BindDatatoSession();
            Response.Redirect("~/ManageData/WorkProcess/Campaign/ManageCampaign.aspx");
        }

        protected void BindDatatoSession()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STATUS_CAMPAIGN"); //EDIT UPDATE DELETE
            dt.Columns.Add("ID_CAMPAIGN"); //CampaignCode

            DataRow dr = dt.NewRow();
            dr["STATUS_CAMPAIGN"] = "UPDATE_CAMPAIGN";
            dr["ID_CAMPAIGN"] = gv_listCampaign.SelectedRow.Cells[0].Text;
            dt.Rows.Add(dr);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt); 
            campaignStorage.SetCookiesDataSetByName("DS_DATA_CAMPAIGN", ds);
        }

    }
}