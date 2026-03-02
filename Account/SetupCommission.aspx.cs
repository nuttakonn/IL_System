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

namespace ILSystem.ManageData.WorkProcess.Account
{
    public partial class SetupCommission : System.Web.UI.Page
    {
        protected DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        protected ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        protected ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        protected ILDataCenter ilObj = new ILDataCenter();
        public DataCenter dataCenter;
        public UserInfoService userInfoService;
        protected string SqlAll = "";
        protected string vendorIDs = null;
        protected string commissionTypes;
        protected string dueDays;
        protected string cancelSerach = null;
        protected string productId = null;
        protected string productDes = null;
        public CookiesStorage cookiesStorage;
        protected string Msg = "";
        protected string MsgHText = "";
        protected string checkMode = "";

        protected int dateTranfer = 0;
        protected int checkPopupTwo = 0;
        protected double checkRate = 0.00;

        protected int k = 0;
        public UserInfo m_userInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            m_userInfo = userInfoService.GetUserInfo();
            cookiesStorage = new CookiesStorage();
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
            dataCenter = new DataCenter(m_userInfo);
            if (!IsPostBack)
            {
                DATA_COMISSION_TYPE2();
                DATA_ILTB71_001();
                DATA_ILTB71_002();
                DATA_ILTB71_003();
                DATA_ILTB71_004();
                DATA_ILTB71_005();
                DATA_ILTB71_006();
                DATA_ILTB71_007();
                DATA_ILTB71_008();
                DATA_ILTB71_009();
                DATA_ILTB71_010();
                DATA_ILTB71_011();
                DATA_ILTB71_012();


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"), new DataColumn("M02SAM"), new DataColumn("M02EAM"), new DataColumn("M02CMR") });
                ViewState["gridAddRow"] = dt;


                DataTable dtApp = new DataTable();
                dtApp.Columns.AddRange(new DataColumn[3] { new DataColumn("IdApp"), new DataColumn("GN61CD"), new DataColumn("GN61DT") });
                ViewState["gridAddRow_applicationType"] = dtApp;
                gvAddRowsApplication.DataBind();

                DataTable dtProduct = new DataTable();
                dtProduct.Columns.AddRange(new DataColumn[5] { new DataColumn("IdProduct"), new DataColumn("T40TYP"), new DataColumn("T40DES"), new DataColumn("T71ITM"), new DataColumn("T71DES") });
                ViewState["gridAddRow_productType"] = dtProduct;
                gvAddRowsProductType.DataBind();

                DataTable dtProductCode = new DataTable();
                dtProductCode.Columns.AddRange(new DataColumn[3] { new DataColumn("T41TYP"), new DataColumn("T41COD"), new DataColumn("T41DES") });
                ViewState["gridAddRow_productCode"] = dtProductCode;
                gvAddRowsProductCode.DataBind();

                checkStepStatus.Text = "activeStep1";
                step1_box1.Enabled = false;
                step1_box2.Enabled = false;
                step1_box3.Enabled = false;
                step2_box1.Enabled = false;
                step2_box2.Enabled = false;
                //vendorDescription.Enabled = false; 
                createPayment.Enabled = false;
                paymentVendor.Enabled = false;
                paymentDesciption.Enabled = false;
                referanceNo.Enabled = false;
                commissionType.Enabled = false;
                dueDay.Enabled = false;

                btnEdit.Enabled = false;
                btnRefresh.Visible = false;
                btnSave.Visible = false;

                //start step2
                budgetPercentage.Enabled = false;
                startDateStep2.Enabled = false;
                endDateStep2.Enabled = false;
                budgetBranch.Enabled = false;
                budgetType.Enabled = false;
                seqNo.Enabled = false;
                CalendarExtender3.Enabled = false;
                CalendarExtender4.Enabled = false;
                //end step2

                ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
                DEFAULT_STEP1();
                DISIBLE_ALL();
                btnEdit.Enabled = false;
                checkModeStatus.Text = "";
                PopupAlertCenter();
            }
        }


        #region  show popup alert

        private void SET_MSG_SUCCESS()
        {
            checkMode = checkModeStatus.Text;
            if (checkMode == "saveMain")
            {
                lblMsgSuccess.Text = "Save data completed";
                PopupMsgSuccess.HeaderText = "Success";
                PopupMsgSuccess.ShowOnPageLoad = true;
            }
            else if (checkMode == "editMain")
            {
                lblMsgSuccess.Text = "Edit data completed";
                PopupMsgSuccess.HeaderText = "Success";
                PopupMsgSuccess.ShowOnPageLoad = true;

            }

        }
        private void SET_MSG()
        {
            lblMsg.Text = Msg;
            PopupMsg.HeaderText = MsgHText;
            PopupMsg.ShowOnPageLoad = true;
        }

        private void PopupAlertCenter()
        {

            Popup_SelectVendor.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_SelectVendor.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupSearchAlert.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupSearchAlert.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupAlertRate.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertRate.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ApplicationType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ApplicationType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductCode.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductCode.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupAlertApp.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertApp.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupAlertProduct.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertProduct.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupAlertClearStep2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertClearStep2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

        }
        #endregion

        #region step1 & step2 : default and set value
        private void SetDate()
        {

            DateTime dtime;
            DateTime.TryParseExact(m_UpdDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);

            string formattedDateTime = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            startDate.Text = formattedDateTime;
            endDate.Text = formattedDateTime;

        }
        private void DEFAULT_STEP1()
        {

            CLEAR_GRIDVIEW1();
            ListDateVendor();
            vendorID.Enabled = true;
            vendorDescription.Enabled = true;
            btnAddRank.Enabled = true;
            btnEdit.Enabled = true;
            CalendarExtender1.Enabled = false;
            CalendarExtender2.Enabled = false;
            listboxBBM.SelectedIndex = 0;
            listboxBTY.SelectedIndex = 0;
            listboxRTY.SelectedIndex = 0;
            listboxIIR.SelectedIndex = 0;
            listboxCBO.SelectedIndex = 0;
            step1_box1.Enabled = true;
            step1_box2.Enabled = false;
            step1_box3.Enabled = false;
            gvAddRows.DataSource = null;
            gvAddRows.DataBind();
            txtStartAmount.Text = "0.00";
            txtEndAmount.Text = "0.00";
            txtRate.Text = "0.00";
            setCommision.Text = "";
            checkPopupTwo = 0;
            ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;

        }
        private void DEFAULT_STEP2()
        {
            //DateTime dtime;
            //DateTime.TryParseExact(m_UpdDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
            //                          DateTimeStyles.None, out dtime);
            //string formattedDateTime = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            CLEAR_GRIDVIEW2();
            ListSeqNo();
            gvSeqNo.Enabled = true;
            step2_box1.Enabled = false;
            step2_box2.Enabled = false;
            btnAddRank.Enabled = true;
            btnEdit.Enabled = true;
            seqNo.Text = "";
            startDateStep2.Text = startDate.Text;
            endDateStep2.Text = endDate.Text;
            CalendarExtender3.Enabled = false;
            CalendarExtender4.Enabled = false;
            //budgetBranch.SelectedIndex = 1;
            //budgetType.SelectedIndex = 1;
            budgetContract.SelectedIndex = 0;
            txtAmount.Text = "0.00";
            budgetCriteriaType.SelectedIndex = 0;
            budgetAmount.Text = "0.00";

            budgetPercentage.Text = "0.00";
            budgetTypeComparative.SelectedIndex = 0;

            budgetAppType.SelectedIndex = 0;
            budgetProductType.SelectedIndex = 0;

            month1.Text = "0.00";
            month2.Text = "0.00";
            month3.Text = "0.00";
            month4.Text = "0.00";
            month5.Text = "0.00";
            month6.Text = "0.00";
            month7.Text = "0.00";
            month8.Text = "0.00";
            month9.Text = "0.00";
            month10.Text = "0.00";
            month11.Text = "0.00";
            month12.Text = "0.00";
            setCommision.Text = "";
            step2_applicationType.Enabled = false;
            ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            checkPopupTwo = 0;
        }
        private void CLEAR_GRIDVIEW1()
        {
            ViewState["gridAddRow"] = null;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"), new DataColumn("M02SAM"), new DataColumn("M02EAM"), new DataColumn("M02CMR") });
            ViewState["gridAddRow"] = dt;
            gvAddRows.DataSource = null;
            gvAddRows.DataBind();
        }
        private void CLEAR_GRIDVIEW2()
        {
            readApplicationType.Text = "";
            readProductType.Text = "";
            productTypeSelect.Text = "";
            readProductCode.Text = "";
            //ddlProductType.SelectedIndex = 0;
            ViewState["gridAddRow_applicationType"] = null;
            DataTable dtApp = new DataTable();
            dtApp.Columns.AddRange(new DataColumn[3] { new DataColumn("IdApp"), new DataColumn("GN61CD"), new DataColumn("GN61DT") });
            ViewState["gridAddRow_applicationType"] = dtApp;
            gvAddRowsApplication.DataBind();

            ViewState["gridAddRow_productType"] = null;
            DataTable dtProduct = new DataTable();
            dtProduct.Columns.AddRange(new DataColumn[5] { new DataColumn("IdProduct"), new DataColumn("T40TYP"), new DataColumn("T40DES"), new DataColumn("T71ITM"), new DataColumn("T71DES") });
            ViewState["gridAddRow_productType"] = dtProduct;
            gvAddRowsProductType.DataBind();

            ViewState["gridAddRow_productCode"] = null;
            DataTable dtProductCode = new DataTable();
            dtProductCode.Columns.AddRange(new DataColumn[3] { new DataColumn("T41TYP"), new DataColumn("T41COD"), new DataColumn("T41DES") });
            ViewState["gridAddRow_productCode"] = dtProductCode;
            gvAddRowsProductCode.DataBind();
        }

        private void DISIBLE_ALL()
        {
            btnMainInsert.Enabled = false;
            btnMainSave.Enabled = false;
            btnMainCancel.Enabled = false;
            btnMainDelete.Enabled = false;
            btnMainEdit.Enabled = false;
        }

        #endregion

        #region step1 & step2 : select vendor onchange text box
        protected void SELECT_CAMPAIGN_TYPE_ONCHANGE(object sender, EventArgs e)
        {
            if (commissionType.SelectedIndex == 0)
            {
                DATA_COMISSION_TYPE2();
                dueDay.Enabled = false;
            }
            else if (commissionType.SelectedIndex == 1)
            {
                //DATA_ILTB71_001();
                DATA_COMISSION_TYPE();
                dueDay.Enabled = true;
            }
        }
        protected void SELECT_VENDOR_ONCHANGE_DES(object sender, EventArgs e)
        {
            if (int.Parse(vendorDescription.Text.Length.ToString()) >= 3)
            {
                SetDate();
                CLEAR_GRIDVIEW1();
                DISIBLE_ALL();

                iDB2Command cmd = new iDB2Command();
                string sqlwhere = "";

                if (ddl_popup_SearchBy.SelectedValue == "CV" && txt_Detail.Text.Trim() != "")
                {
                    sqlwhere += " AND P10VEN LIKE '%" + txt_Detail.Text.Trim() + "%'";
                }
                else if (ddl_popup_SearchBy.SelectedValue == "DV" && txt_Detail.Text.Trim() != "")
                {
                    sqlwhere += " AND (P10TNM LIKE '%" + txt_Detail.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + txt_Detail.Text.ToUpper().Trim() + "%')";
                }
                else
                {

                    if (vendorID.Text.Trim() != "" && vendorDescription.Text.Trim() != "")
                    {
                        sqlwhere += " AND P10VEN LIKE '%" + vendorID.Text.Trim() + "%'  AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR P10NAM LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                    }
                    else if (vendorID.Text.Trim() != "")
                    {
                        sqlwhere += " AND P10VEN LIKE '%" + vendorID.Text.Trim() + "%'";
                    }
                    else if (vendorDescription.Text.Trim() != "")
                    {
                        sqlwhere += " AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                    }
                }

                DataSet dsOnchange = new DataSet();
                userInfoService = new UserInfoService();
                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);

                SqlAll = @" SELECT DISTINCT P10VEN, P10NAM, P10CTY, P10CPD, P10PTY,
							    CASE WHEN P10VEN = P10PVN THEN P10VEN ELSE P10PVN END AS P10PVN, 
							    CASE WHEN P10PVN = 0 THEN '' ELSE P10NAM END AS PAY_VEND_NAME
                            FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) 
                            WHERE P10DEL = ''";

                SqlAll = SqlAll + sqlwhere + " ORDER BY P10VEN ASC";
                cmd.CommandText = SqlAll;
                dsOnchange = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

                ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsOnchange);

                dsOnchange = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

                if (!cookiesStorage.check_dataset(dsOnchange))
                {
                    //DEFAULT_STEP1();
                    vendorID.Text = "";
                    vendorDescription.Text = "";
                    paymentVendor.Text = "";
                    paymentDesciption.Text = "";
                    createPayment.Text = "";
                    referanceNo.Text = "";
                    lblMsgAlert.Text = "Vendor ID number was not found !";
                    PopupAlertRate.ShowOnPageLoad = true;
                }
                else if (dsOnchange.Tables[0]?.Rows.Count > 1)
                {
                    LoadDataPopup();
                    Popup_SelectVendor.ShowOnPageLoad = true;
                }
                else if (dsOnchange.Tables[0]?.Rows.Count == 1)
                {
                    SetDate();
                    DATA_ILTB71_001();

                    CLEAR_GRIDVIEW1();
                    DISIBLE_ALL();
                    DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
                    DataRow dr = ds_grid.Tables[0]?.Rows[0];

                    if (dr[2].ToString().Trim() != "")
                    {
                        if (int.Parse(dr[2].ToString().Trim()) == 1)
                        {
                            DATA_COMISSION_TYPE2();
                        }
                        else if (int.Parse(dr[2].ToString().Trim()) == 2)
                        {
                            DATA_COMISSION_TYPE();
                        }
                    }

                    checkStepStatus.Text = "activeStep1";
                    vendorID.Text = dr[0].ToString().Trim();
                    vendorDescription.Text = dr[1].ToString().Trim();
                    paymentVendor.Text = dr[5].ToString().Trim();
                    paymentDesciption.Text = dr[6].ToString().Trim();

                    if (dr[4].ToString().Trim() == "1")
                    {
                        createPayment.Text = "Medie Clearing";
                    }
                    else if (dr[4].ToString().Trim() == "2")
                    {
                        createPayment.Text = "Cheque";
                    }
                    else if (dr[4].ToString().Trim() == "3")
                    {
                        createPayment.Text = "Medie Clearing";
                    }


                    dueDays = dr[3].ToString().Trim();
                    hisCommissionType.Text = dr[2].ToString().Trim();
                    hisDueDay.Text = dr[3].ToString().Trim();

                    if (dr[2].ToString().Trim() != "")
                    {
                        commissionType.SelectedIndex = int.Parse(dr[2].ToString().Trim()) - 1;
                    }
                    dueDay.SelectedIndex = int.Parse(dr[3].ToString().Trim()) - 1;
                    referanceNo.Text = "";
                    btnEdit.Enabled = true;
                    btnRefresh.Enabled = true;
                    CalendarExtender1.Enabled = false;
                    CalendarExtender2.Enabled = false;
                    btnSave.Enabled = true;
                    btnMainInsert.Enabled = true;
                    ContentControlStep1.Enabled = true;


                    Popup_SelectVendor.ShowOnPageLoad = false;
                    ResetGrid(psVendor, ds_popup.Value);

                    ListDateVendor();
                    ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step1");
                    ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
                    highLightRow.Text = "";
                }
            }
            return;
        }
        protected void SELECT_VENDOR_ONCHANGE(object sender, EventArgs e)
        {
            if (int.Parse(vendorID.Text.Length.ToString()) >= 3)
            {
                SetDate();
                CLEAR_GRIDVIEW1();
                DISIBLE_ALL();

                iDB2Command cmd = new iDB2Command();
                string sqlwhere = "";

                if (ddl_popup_SearchBy.SelectedValue == "CV" && txt_Detail.Text.Trim() != "")
                {
                    sqlwhere += " AND P10VEN LIKE '%" + txt_Detail.Text.Trim() + "%'";
                }
                else if (ddl_popup_SearchBy.SelectedValue == "DV" && txt_Detail.Text.Trim() != "")
                {
                    sqlwhere += " AND (P10TNM LIKE '%" + txt_Detail.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + txt_Detail.Text.ToUpper().Trim() + "%')";
                }
                else
                {

                    if (vendorID.Text.Trim() != "" && vendorDescription.Text.Trim() != "")
                    {
                        sqlwhere += " AND P10VEN LIKE '%" + vendorID.Text.Trim() + "%'  AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR P10NAM LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                    }
                    else if (vendorID.Text.Trim() != "")
                    {
                        sqlwhere += " AND P10VEN LIKE '%" + vendorID.Text.Trim() + "%'";
                    }
                    else if (vendorDescription.Text.Trim() != "")
                    {
                        sqlwhere += " AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                    }
                }

                DataSet dsOnchange = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);

                SqlAll = @" SELECT DISTINCT P10VEN, P10NAM, P10CTY, P10CPD, P10PTY,
							    CASE WHEN P10VEN = P10PVN THEN P10VEN ELSE P10PVN END AS P10PVN, 
							    CASE WHEN P10PVN = 0 THEN '' ELSE P10NAM END AS PAY_VEND_NAME
                            FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                            WHERE P10DEL = ''";

                SqlAll = SqlAll + sqlwhere + " ORDER BY P10VEN ASC";
                cmd.CommandText = SqlAll;
                dsOnchange = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
                ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsOnchange);
                dsOnchange = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

                if (!cookiesStorage.check_dataset(dsOnchange))
                {
                    //DEFAULT_STEP1();
                    vendorID.Text = "";
                    vendorDescription.Text = "";
                    paymentVendor.Text = "";
                    paymentDesciption.Text = "";
                    createPayment.Text = "";
                    referanceNo.Text = "";
                    lblMsgAlert.Text = "Vendor ID number was not found !";
                    PopupAlertRate.ShowOnPageLoad = true;
                }
                else if (dsOnchange.Tables[0]?.Rows.Count > 1)
                {
                    LoadDataPopup();
                    Popup_SelectVendor.ShowOnPageLoad = true;
                }
                else if (dsOnchange.Tables[0]?.Rows.Count == 1)
                {
                    SetDate();
                    DATA_ILTB71_001();

                    CLEAR_GRIDVIEW1();
                    DISIBLE_ALL();
                    DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
                    DataRow dr = ds_grid.Tables[0]?.Rows[0];

                    if (dr[2].ToString().Trim() != "")
                    {
                        if (int.Parse(dr[2].ToString().Trim()) == 1)
                        {
                            DATA_COMISSION_TYPE2();
                        }
                        else if (int.Parse(dr[2].ToString().Trim()) == 2)
                        {
                            DATA_COMISSION_TYPE();
                        }
                    }

                    checkStepStatus.Text = "activeStep1";
                    vendorID.Text = dr[0].ToString().Trim();
                    vendorDescription.Text = dr[1].ToString().Trim();
                    paymentVendor.Text = dr[5].ToString().Trim();
                    paymentDesciption.Text = dr[6].ToString().Trim();

                    if (dr[4].ToString().Trim() == "1")
                    {
                        createPayment.Text = "Medie Clearing";
                    }
                    else if (dr[4].ToString().Trim() == "2")
                    {
                        createPayment.Text = "Cheque";
                    }
                    else if (dr[4].ToString().Trim() == "3")
                    {
                        createPayment.Text = "Medie Clearing";
                    }


                    dueDays = dr[3].ToString().Trim();
                    hisCommissionType.Text = dr[2].ToString().Trim();
                    hisDueDay.Text = dr[3].ToString().Trim();

                    if (dr[2].ToString().Trim() != "")
                    {
                        commissionType.SelectedIndex = int.Parse(dr[2].ToString().Trim()) - 1;
                    }
                    dueDay.SelectedIndex = int.Parse(dr[3].ToString().Trim()) - 1;
                    referanceNo.Text = "";
                    btnEdit.Enabled = true;
                    btnRefresh.Enabled = true;
                    CalendarExtender1.Enabled = false;
                    CalendarExtender2.Enabled = false;
                    btnSave.Enabled = true;
                    btnMainInsert.Enabled = true;
                    ContentControlStep1.Enabled = true;


                    Popup_SelectVendor.ShowOnPageLoad = false;
                    ResetGrid(psVendor, ds_popup.Value);

                    ListDateVendor();
                    ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step1");
                    ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
                    highLightRow.Text = "";
                }
            }
            return;
        }
        #endregion

        #region step1 & step2 : tab control status
        protected void ACTIVE_TAB_CHANGE(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
        {
            if (checkStepStatus.Text == "activeStep2" && checkModeStatus.Text == "saveMain")
            {
                lblClearStep2.Text = "If you return to Step1, the information in Step2 will be cleared.";
                PopupAlertClearStep2.ShowOnPageLoad = true;
            }
            else
            {
                if (ContentControlStep1.ActiveTabIndex == 0)
                {

                    checkStepStatus.Text = "activeStep1";
                    //referanceNo.Text = "";
                    //ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
                    checkModeStatus.Text = "";
                    //DEFAULT_STEP1();
                    btnMainEdit.Enabled = true;
                    btnMainDelete.Enabled = true;
                    btnMainCancel.Enabled = true;
                    btnMainInsert.Enabled = false;
                }
                else if (ContentControlStep1.ActiveTabIndex == 1)
                {
                    DEFAULT_STEP2();
                    calendarStartStep2.Enabled = false;
                    calendarEndStep2.Enabled = false;
                    step2_box1.Enabled = false;
                    step2_box2.Enabled = false;

                    checkStepStatus.Text = "activeStep2";

                    ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step2");
                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
                    checkModeStatus.Text = "";
                    //DEFAULT_STEP2();
                    DISIBLE_ALL();
                    btnMainInsert.Enabled = true;
                    int checkListboxBTY = int.Parse(listboxBTY.Text.Trim());
                    int checkListboxBBM = int.Parse(listboxBBM.Text.Trim());
                    budgetBranch.SelectedIndex = checkListboxBBM - 1;
                    budgetType.SelectedIndex = checkListboxBTY - 1;
                }


            }
            return;
        }
        protected void ACTIVE_TAB2_CHANGE(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
        {
            if (ContentControlStep2.ActiveTabIndex == 0)
            {

            }
            else if (ContentControlStep2.ActiveTabIndex == 1)
            {

            }
        }
        #endregion

        #region step1 & step2 : event button click
        protected void CLICK_SELECT_VENDOR(object sender, EventArgs e)
        {

            //if (vendorID.Text == "" && vendorDescription.Text == "")
            //{
            //    PopupSearchAlert.ShowOnPageLoad = true;

            //}
            //else
            //{
            //    LoadDataPopup();
            //    Popup_SelectVendor.ShowOnPageLoad = true;


            //}
            LoadDataPopup();
            Popup_SelectVendor.ShowOnPageLoad = true;
        }

        protected void BTN_CONFIRM_SEARCH_CLICK(object sender, EventArgs e)
        {

            DEFAULT_STEP1();
            PopupSearchAlert.ShowOnPageLoad = false;
            vendorID.Focus();

        }
        protected void BTN_CONFIRM_SEARCH_CANCEL_CLICK(object sender, EventArgs e)
        {
            vendorID.Text = "";
            vendorDescription.Text = "";
            ddl_popup_SearchBy.SelectedIndex = 0;
            txt_Detail.Text = "";

            cancelSerach = popUpCancel.Text;
            PopupSearchAlert.ShowOnPageLoad = false;
            LoadDataPopup();
            Popup_SelectVendor.ShowOnPageLoad = true;

            vendorIDs = null;
        }

        protected void BTN_EDIT_COMISSION(object sender, EventArgs e)
        {
            setCommision.Text = "edit";
            if (commissionType.SelectedIndex == 0)
            {
                dueDay.Enabled = false;
            }
            else if (commissionType.SelectedIndex == 1)
            {
                dueDay.Enabled = true;
            }
            commissionType.Enabled = true;
            btnEdit.Enabled = false;
            vendorID.Enabled = false;
            vendorDescription.Enabled = false;
            btnAddRank.Enabled = false;
            btnSave.Visible = true;
            btnRefresh.Visible = true;

        }
        protected void CLICK_RE_COMMISSION(object sender, EventArgs e)
        {

            if (hisCommissionType.Text.ToString().Trim() == "1")
            {
                commissionType.SelectedIndex = int.Parse(hisCommissionType.Text.ToString()) - 1;
                DATA_COMISSION_TYPE2();
            }
            else if (hisCommissionType.Text.ToString().Trim() == "2")
            {
                commissionType.SelectedIndex = int.Parse(hisCommissionType.Text.ToString()) - 1;
                DATA_COMISSION_TYPE();
                dueDay.SelectedIndex = int.Parse(hisDueDay.Text.ToString()) - 1;
            }
            btnEdit.Enabled = true;
            btnSave.Visible = false;
            btnRefresh.Visible = false;
            btnAddRank.Enabled = true;
            commissionType.Enabled = false;
            dueDay.Enabled = false;
            vendorID.Enabled = true;
            vendorDescription.Enabled = true;
        }

        protected void SET_CLEAR_ADD(object sender, EventArgs e)
        {
            if (checkStepStatus.Text == "activeStep1")
            {
                int checkListboxBTY = int.Parse(listboxBTY.Text.Trim());
                int checkListboxBBM = int.Parse(listboxBBM.Text.Trim());
                if (checkListboxBTY == 1)
                {
                    referanceNo.Text = "";

                    DEFAULT_STEP1();
                    btnMainInsert.Enabled = true;
                    DISIBLE_ALL();
                    ListDateVendor();
                    btnAddRank.Enabled = true;
                    btnEdit.Enabled = true;
                    btnMainInsert.Enabled = true;
                    step1_box2.Enabled = false;
                    step1_box3.Enabled = false;
                    txtStartAmount.Text = "0.00";
                    txtEndAmount.Text = "0.00";
                    txtRate.Text = "0.00";
                    checkModeStatus.Text = "";
                    hisCommissionType.Text = commissionType.Text;
                    hisDueDay.Text = dueDay.Text;
                    SetDate();

                }
                else if (((checkListboxBTY == 2) && (setCommision.Text != "edit")) || ((checkListboxBTY == 3) && (setCommision.Text != "edit")))
                {
                    startDateStep2.Text = startDate.Text;
                    endDateStep2.Text = endDate.Text;
                    DEFAULT_STEP1();
                    DEFAULT_STEP2();
                    DISIBLE_ALL();
                    btnMainInsert.Enabled = true;
                    btnAddRank.Enabled = true;
                    btnEdit.Enabled = true;
                    checkStepStatus.Text = "activeStep2";
                    ContentControlStep1.TabPages.FindByName("Step2").Enabled = true;
                    ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step2");
                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;


                    budgetBranch.SelectedIndex = checkListboxBBM - 1;
                    budgetType.SelectedIndex = checkListboxBTY - 1;
                    hisCommissionType.Text = commissionType.Text;
                    hisDueDay.Text = dueDay.Text;
                }
                else
                {
                    referanceNo.Text = "";

                    DEFAULT_STEP1();
                    btnMainInsert.Enabled = true;
                    DISIBLE_ALL();
                    ListDateVendor();
                    btnAddRank.Enabled = true;
                    btnEdit.Enabled = true;
                    btnMainInsert.Enabled = true;
                    step1_box2.Enabled = false;
                    step1_box3.Enabled = false;
                    txtStartAmount.Text = "0.00";
                    txtEndAmount.Text = "0.00";
                    txtRate.Text = "0.00";
                    checkModeStatus.Text = "";
                    hisCommissionType.Text = commissionType.Text;
                    hisDueDay.Text = dueDay.Text;
                }

                return;

            }
            else if (checkStepStatus.Text == "activeStep2")
            {

                ListSeqNo();
                DEFAULT_STEP2();
                DISIBLE_ALL();
                readApplicationType.Text = "";
                readProductType.Text = "";
                readProductTypeHidden.Text = "";
                ddlProductType.SelectedIndex = 1;

                productTypeSelect.Text = "";
                readProductCode.Text = "";
                productTypeSend.Text = "";
                productDesSend.Text = "";
                gvAddRowsApplication.DataSource = null;
                gvAddRowsApplication.DataBind();
                gvAddRowsProductType.DataSource = null;
                gvAddRowsProductType.DataBind();
                gvAddRowsProductCode.DataSource = null;
                gvAddRowsProductCode.DataBind();
                btnMainInsert.Enabled = true;
                checkStepStatus.Text = "activeStep2";
                ContentControlStep1.TabPages.FindByName("Step2").Enabled = true;
                ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step2");


            }

            return;

        }
        protected void TXTRATE_ONCHANGE_NAN(object sender, EventArgs e)
        {
            if (txtRate.Text.Trim() == "NaN")
            {
                txtRate.Text = "0.00";
            }
        }

        protected void CLICK_ALERT_RATE(object sender, EventArgs e)
        {

            txtRate.Focus();
            btnMainCancel.Enabled = true;
            return;
        }

        protected void CLICK_MAIN_EDIT(object sender, EventArgs e)
        {
            if (checkStepStatus.Text == "activeStep1")
            {
                DISIBLE_ALL();
                checkModeStatus.Text = "editMain";
                if (listboxRTY.SelectedIndex == 0)
                {
                    txtEndAmount.Enabled = false;
                }
                else
                {
                    txtEndAmount.Enabled = true;
                }


                btnEdit.Enabled = false;
                btnAddRank.Enabled = false;
                vendorID.Enabled = false;
                vendorDescription.Enabled = false;
                btnMainSave.Enabled = true;
                btnMainCancel.Enabled = true;
                step1_box1.Enabled = false;
                step1_box2.Enabled = true;
                step1_box3.Enabled = true;
                CalendarExtender1.Enabled = true;
                CalendarExtender2.Enabled = true;
                string ads = endDate.Text.ToString();
                string[] endSplitDate = endDate.Text.Trim().Split('/');
                string endDateEdit = endSplitDate[2] + endSplitDate[1] + endSplitDate[0];

                DataSet dsDateNew = new DataSet();

                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                SqlAll = "SELECT MAX(M02EDT) FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE cast(M02VEN as nvarchar) = '" + vendorID.Text + "'";
                dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (cookiesStorage.check_dataset(dsDateNew))
                {
                    DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];

                    DateTime dtime;
                    DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out dtime);
                    ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
                }
                
            }
            else if (checkStepStatus.Text == "activeStep2")
            {
                DISIBLE_ALL();
                checkModeStatus.Text = "editMain";
                btnEdit.Enabled = false;
                btnAddRank.Enabled = false;
                step2_box1.Enabled = true;
                btnMainSave.Enabled = true;
                btnMainCancel.Enabled = true;
                gvSeqNo.Enabled = false;

                if (budgetCriteriaType.SelectedItem.Value.Trim() == "1")
                {
                    budgetAmount.Enabled = true;
                    budgetPercentage.Enabled = false;
                    step2_box2.Enabled = false;
                }
                else if (budgetCriteriaType.SelectedItem.Value.Trim() == "2")
                {
                    budgetAmount.Enabled = false;
                    budgetPercentage.Enabled = true;
                    step2_box2.Enabled = false;
                }
                else if (budgetCriteriaType.SelectedItem.Value.Trim() == "3")
                {
                    budgetAmount.Enabled = false;
                    budgetPercentage.Enabled = false;
                    step2_box2.Enabled = true;

                }

                if (budgetAppType.SelectedItem.Value.Trim() == "0")
                {
                    gvAddRowsApplication.Enabled = false;
                    step2_applicationType.Enabled = false;
                    readApplicationType.Enabled = false;
                    applicationTypes.Enabled = false;
                }
                else
                {
                    gvAddRowsApplication.Enabled = true;
                    step2_applicationType.Enabled = true;
                    readApplicationType.Enabled = true;
                    applicationTypes.Enabled = true;

                }

                if (budgetProductType.SelectedItem.Value.Trim() == "0")
                {
                    gvAddRowsProductType.Enabled = false;
                    step2_productType.Enabled = false;
                    readProductType.Enabled = false;
                    productTypesSearch.Enabled = false;
                    ddlProductType.Enabled = false;
                    productTypeSelect.Enabled = false;
                    readProductCode.Enabled = false;
                    productCodes.Enabled = false;
                }
                else
                {
                    gvAddRowsProductType.Enabled = true;
                    step2_productType.Enabled = true;
                    readProductType.Enabled = true;
                    productTypesSearch.Enabled = true;
                    ddlProductType.Enabled = true;

                    gvAddRowsProductCode.Enabled = true;
                    step2_productCode.Enabled = true;
                    readProductCode.Enabled = true;
                    productCodes.Enabled = true;
                    productTypeSelect.Enabled = true;
                }
                ContentControlStep1.TabPages.FindByName("Step1").Enabled = false;
            }
            return;
        }
        protected void CLICK_CLEAR_ALL(object sender, EventArgs e)
        {
            if (checkStepStatus.Text == "activeStep1")
            {
                CLEAR_GRIDVIEW1();
                highLightRow.Text = "";
                referanceNo.Text = "";
                checkModeStatus.Text = "";
                DEFAULT_STEP1();
                DISIBLE_ALL();
                SetDate();
                btnEdit.Enabled = true;
                ContentControlStep1.Enabled = true;
                btnMainInsert.Enabled = true;
                step1_box1.Enabled = true;
                step1_box2.Enabled = false;
                step1_box3.Enabled = false;
                txtStartAmount.Text = "0.00";
                txtEndAmount.Text = "0.00";
                txtRate.Text = "0.00";
            }
            else if (checkStepStatus.Text == "activeStep2")
            {
                checkModeStatus.Text = "";
                DEFAULT_STEP2();
                DISIBLE_ALL();
                btnMainInsert.Enabled = true;
                ContentControlStep1.TabPages.FindByName("Step1").Enabled = true;

            }
            return;

        }
        protected void CLICK_YES_CONFIRM_BACKSTEP(object sender, EventArgs e)
        {
            checkStepStatus.Text = "activeStep1";
            //referanceNo.Text = "";
            ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
            //checkModeStatus.Text = "";
            //DEFAULT_STEP1();
            CLEAR_GRIDVIEW1();
            CLEAR_GRIDVIEW2();
            highLightRow.Text = "";
            referanceNo.Text = "";
            checkModeStatus.Text = "";
            DEFAULT_STEP1();
            //DEFAULT_STEP2();
            DISIBLE_ALL();
            SetDate();
            btnEdit.Enabled = true;
            ContentControlStep1.Enabled = true;
            btnMainInsert.Enabled = true;
            step1_box1.Enabled = true;
            step1_box2.Enabled = false;
            step1_box3.Enabled = false;
            txtStartAmount.Text = "0.00";
            txtEndAmount.Text = "0.00";
            txtRate.Text = "0.00";

        }
        protected void CLICK_NO_CONFIRM_BACKSTEP(object sender, EventArgs e)
        {
            checkStepStatus.Text = "activeStep2";
            ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step2");
            ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            if ((budgetAppType.SelectedItem.Value.Trim() == "0") && (budgetProductType.SelectedItem.Value.Trim() == "0"))
            {
                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            }
            else
            {
                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;
            }

        }
        protected void CLICK_CONFIRM_ALERT(object sender, EventArgs e)
        {
            if (checkStepStatus.Text == "activeStep2")
            {
                checkModeStatus.Text = "";
            }
            return;
        }

        protected void BTN_SAVE_COMISSION(object sender, EventArgs e)
        {

            iDB2Command cmd = new iDB2Command();
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = "UPDATE AS400DB01.ILOD0001.ILMS10 SET P10CTY = '" + commissionType.Text.ToString() + "', P10CPD = '" + dueDay.Text.ToString() + "' WHERE P10VEN = '" + vendorID.Text.ToString() + "'";

            cmd.CommandText = SqlAll;

            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resHome11 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "Can't save data , Please check command query ! ";
                    MsgHText = "Error Query";
                    SET_MSG();

                    return;
                }

                dataCenter.CommitMssql();
                cmd.Parameters.Clear();
                dataCenter.CommitMssql();

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            lblMsgSuccess.Text = "Update commission type completed";
            PopupMsgSuccess.HeaderText = "Success";
            PopupMsgSuccess.ShowOnPageLoad = true;

            commissionType.Enabled = false;
            dueDay.Enabled = false;
            vendorID.Enabled = true;
            vendorDescription.Enabled = true;
            btnAddRank.Enabled = true;
            btnSave.Visible = false;
            btnRefresh.Visible = false;

        }


        protected void CLICK_MAIN_INSERT(object sender, EventArgs e)
        {
            checkModeStatus.Text = "saveMain";
            if (checkStepStatus.Text == "activeStep1")
            {
                //SetDate();
                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                // ไม่ได้เรียกใช้ store เพราะหา Call_ILSR99 ไม่เจอ
                SqlAll = "SELECT P99RUN+1 FROM AS400DB01.ILOD0001.ILMS99 WITH (NOLOCK) WHERE P99REC = '600'";

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                DataRow drs = DS.Tables[0]?.Rows[0];
                referanceNo.Text = drs[0].ToString();

                if (gvListDateVendor.Rows.Count > 0)
                {
                    DataSet dsDateNew = new DataSet();
                    SqlAll = "SELECT MAX(M02EDT) AS M02EDT FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE M02VEN = " + vendorID.Text + "";
                    dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];
                    string checkYearsFail = drDateNew[0].ToString().Trim().Substring(0, 4);
                    if (checkYearsFail != "9999")
                    {
                        DateTime dtime;
                        DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                                  DateTimeStyles.None, out dtime);

                        string formatDateCheck = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime dCheck = Convert.ToDateTime(formatDateCheck);
                        DateTime newDate = dCheck.AddDays(Convert.ToInt32(1));

                        startDate.Text = newDate.ToShortDateString();
                        endDate.Text = newDate.ToShortDateString();
                    }
                    else
                    {
                        SetDate();
                    }
                }
                else
                {
                    SetDate();
                }

                txtEndAmount.Text = "9,999,999,999,999.99";
                txtEndAmount.Enabled = false;
                btnEdit.Enabled = false;
                btnAddRank.Enabled = false;
                vendorID.Enabled = false;
                vendorDescription.Enabled = false;
                step1_box1.Enabled = false;
                step1_box2.Enabled = true;
                step1_box3.Enabled = true;
                btnMainInsert.Enabled = false;
                btnMainSave.Enabled = true;
                btnMainCancel.Enabled = true;
                CalendarExtender1.Enabled = true;
                CalendarExtender2.Enabled = true;
            }
            else if (checkStepStatus.Text == "activeStep2")
            {
                SET_DISABLE_ALL_3ITEM();
                if ((budgetType.SelectedIndex == 1) && (gvSeqNo.Rows.Count >= 1))
                {
                    lblMsgAlert.Text = "In case budget Type = 'Fix Budget' Seq No. must have only one event";
                    PopupAlertRate.ShowOnPageLoad = true;


                }
                else
                {
                    DataSet dsSeqNo = new DataSet();
                    m_userInfo = userInfoService.GetUserInfo();
                    
                    ilObj.UserInfomation = m_userInfo;
                    dataCenter = new DataCenter(m_userInfo);
                    int checkReferanceNo = int.Parse(referanceNo.Text.ToString()) - 1;
                    SqlAll = "SELECT (CASE WHEN MAX(M03SEQ) IS NOT NULL then MAX(M03SEQ)+1 else 1 end) AS M03SEQ  FROM AS400DB01.ILOD0001.ILCM03 WITH (NOLOCK) WHERE cast(M03REF as nvarchar) = '" + referanceNo.Text + "' AND cast(M03VEN as nvarchar) = '" + vendorID.Text + "'";

                    dsSeqNo = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    if (cookiesStorage.check_dataset(dsSeqNo))
                    {
                        DataRow drSeqNo = dsSeqNo.Tables[0]?.Rows[0];
                        seqNo.Text = drSeqNo[0].ToString();
                        btnEdit.Enabled = false;
                        btnAddRank.Enabled = false;
                        vendorID.Enabled = false;
                        vendorDescription.Enabled = false;
                        btnMainInsert.Enabled = false;
                        btnMainSave.Enabled = true;
                        btnMainCancel.Enabled = true;
                        step2_box1.Enabled = true;
                        budgetAmount.Enabled = true;
                        gvSeqNo.Enabled = false;
                    }
                    
                }
            }
        }
        #endregion

        #region step2 : onchange budget application
        protected void ONCHANGE_BUDGET_APP(object sender, EventArgs e)
        {
            if ((budgetAppType.SelectedItem.Value.Trim() == "0") && (budgetProductType.SelectedItem.Value.Trim() == "0"))
            {
                ViewState["gridAddRow_applicationType"] = null;
                DataTable dtApp = new DataTable();
                dtApp.Columns.AddRange(new DataColumn[3] { new DataColumn("IdApp"), new DataColumn("GN61CD"), new DataColumn("GN61DT") });
                ViewState["gridAddRow_applicationType"] = dtApp;
                gvAddRowsApplication.DataBind();
                readApplicationType.Text = "";

                step2_applicationType.Enabled = false;
                applicationTypes.Enabled = false;
                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            }
            else
            {
                if (budgetAppType.SelectedItem.Value.Trim() == "0")
                {
                    ViewState["gridAddRow_applicationType"] = null;
                    DataTable dtApp = new DataTable();
                    dtApp.Columns.AddRange(new DataColumn[3] { new DataColumn("IdApp"), new DataColumn("GN61CD"), new DataColumn("GN61DT") });
                    ViewState["gridAddRow_applicationType"] = dtApp;
                    gvAddRowsApplication.DataBind();

                    readApplicationType.Text = "";

                    step2_applicationType.Enabled = false;
                    applicationTypes.Enabled = false;
                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;
                }
                else
                {
                    gvAddRowsApplication.Enabled = true;
                    step2_applicationType.Enabled = true;
                    applicationTypes.Enabled = true;
                    readApplicationType.Enabled = true;
                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;
                }

            }
        }
        #endregion

        #region step2 : onchange budget product
        protected void ONCHANGE_BUDGET_PRODUCT(object sender, EventArgs e)
        {
            if ((budgetAppType.SelectedItem.Value.Trim() == "0") && (budgetProductType.SelectedItem.Value.Trim() == "0"))
            {
                ViewState["gridAddRow_productType"] = null;
                DataTable dtProduct = new DataTable();
                dtProduct.Columns.AddRange(new DataColumn[5] { new DataColumn("IdProduct"), new DataColumn("T40TYP"), new DataColumn("T40DES"), new DataColumn("T71ITM"), new DataColumn("T71DES") });
                ViewState["gridAddRow_productType"] = dtProduct;
                gvAddRowsProductType.DataBind();

                ViewState["gridAddRow_productCode"] = null;
                DataTable dtProductCode = new DataTable();
                dtProductCode.Columns.AddRange(new DataColumn[3] { new DataColumn("T41TYP"), new DataColumn("T41COD"), new DataColumn("T41DES") });
                ViewState["gridAddRow_productCode"] = dtProductCode;
                gvAddRowsProductCode.DataBind();

                ddlProductType.SelectedIndex = 0;
                readProductType.Text = "";

                productTypeSelect.Text = "";
                readProductCode.Text = "";
                productTypeSelect.Enabled = false;
                readProductCode.Enabled = false;
                step2_productType.Enabled = false;
                productTypesSearch.Enabled = false;
                addProductType.Enabled = false;
                readProductType.Enabled = false;
                ddlProductType.Enabled = false;
                productCodes.Enabled = false;

                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            }
            else
            {
                if (budgetProductType.SelectedItem.Value.Trim() == "0")
                {
                    ViewState["gridAddRow_productType"] = null;
                    DataTable dtProduct = new DataTable();
                    dtProduct.Columns.AddRange(new DataColumn[5] { new DataColumn("IdProduct"), new DataColumn("T40TYP"), new DataColumn("T40DES"), new DataColumn("T71ITM"), new DataColumn("T71DES") });
                    ViewState["gridAddRow_productType"] = dtProduct;
                    gvAddRowsProductType.DataBind();

                    ViewState["gridAddRow_productCode"] = null;
                    DataTable dtProductCode = new DataTable();
                    dtProductCode.Columns.AddRange(new DataColumn[3] { new DataColumn("T41TYP"), new DataColumn("T41COD"), new DataColumn("T41DES") });
                    ViewState["gridAddRow_productCode"] = dtProductCode;
                    gvAddRowsProductCode.DataBind();

                    ddlProductType.SelectedIndex = 0;
                    readProductType.Text = "";

                    productTypeSelect.Text = "";
                    readProductCode.Text = "";
                    productTypeSelect.Enabled = false;
                    readProductCode.Enabled = false;
                    step2_productType.Enabled = false;
                    productTypesSearch.Enabled = false;
                    addProductType.Enabled = false;
                    readProductType.Enabled = false;
                    ddlProductType.Enabled = false;
                    productCodes.Enabled = false;



                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;
                }
                else
                {
                    gvAddRowsProductType.Enabled = true;
                    gvAddRowsProductCode.Enabled = true;
                    step2_productType.Enabled = true;
                    productTypesSearch.Enabled = true;
                    readProductType.Enabled = true;
                    ddlProductType.Enabled = true;
                    //addProductType.Enabled = true;
                    ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;
                }
            }

        }
        #endregion

        #region step2 : onchange budget criteria type

        protected void ONCHANGE_BUDGET_CRITERIA(object sender, EventArgs e)
        {
            DropDownList budgetCriteriaType = sender as DropDownList;
            budgetAmount.Enabled = false;
            //checkMode = checkModeStatus.Text;
            if (checkStepStatus.Text == "activeStep1")
            {
                lblMsgAlert.Text = "Rate should not exceed 99%";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (checkStepStatus.Text == "activeStep2")
            {
                if (budgetCriteriaType.SelectedItem.Value.Trim() == "1")
                {
                    month1.Text = "0.00";
                    month2.Text = "0.00";
                    month3.Text = "0.00";
                    month4.Text = "0.00";
                    month5.Text = "0.00";
                    month6.Text = "0.00";
                    month7.Text = "0.00";
                    month8.Text = "0.00";
                    month9.Text = "0.00";
                    month10.Text = "0.00";
                    month11.Text = "0.00";
                    month12.Text = "0.00";
                    budgetAmount.Enabled = true;
                    budgetPercentage.Enabled = false;
                    budgetPercentage.Text = "0.00";
                    step2_box2.Enabled = false;
                    budgetAmount.Focus();

                }
                else if (budgetCriteriaType.SelectedItem.Value.Trim() == "2")
                {
                    month1.Text = "0.00";
                    month2.Text = "0.00";
                    month3.Text = "0.00";
                    month4.Text = "0.00";
                    month5.Text = "0.00";
                    month6.Text = "0.00";
                    month7.Text = "0.00";
                    month8.Text = "0.00";
                    month9.Text = "0.00";
                    month10.Text = "0.00";
                    month11.Text = "0.00";
                    month12.Text = "0.00";
                    budgetAmount.Enabled = false;
                    budgetAmount.Text = "0.00";
                    budgetPercentage.Enabled = true;
                    step2_box2.Enabled = false;
                    budgetPercentage.Focus();
                }
                else if (budgetCriteriaType.SelectedItem.Value.Trim() == "3")
                {
                    budgetAmount.Enabled = false;
                    budgetPercentage.Enabled = false;
                    budgetAmount.Text = "0.00";
                    budgetPercentage.Text = "0.00";
                    step2_box2.Enabled = true;
                    month1.Focus();

                }
            }
        }
        #endregion

        #region step2 : popup application type
        private void POPUP_APPLICATION_TYPE()
        {
            string sqlwhere = "";
            SqlAll = "SELECT GN61CD, GN61DT FROM AS400DB01.GNOD0000.GNTB61 WITH (NOLOCK)";

            if (ddlSearchApptype.SelectedValue == "CAT" && txtSearchAppType.Text.Trim() != "")
            {
                sqlwhere += " WHERE GN61CD = " + txtSearchAppType.Text.Trim() + "";
            }

            SqlAll = SqlAll + sqlwhere + " ORDER BY GN61CD ";

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_gvApplicationType.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (cookiesStorage.check_dataset(DS))
            {
                gv_applicationType.DataSource = DS;
                gv_applicationType.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region step2 : popup product type
        private void POPUP_PRODUCT_TYPE()
        {
            string sqlwhere = "";
            SqlAll = "SELECT T40TYP, T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)";

            if (ddlSearchProducttype.SelectedValue == "CPT" && txtSearchProducttype.Text.Trim() != "")
            {
                sqlwhere += " WHERE T40TYP = " + txtSearchProducttype.Text.Trim() + "";
            }
            if (ddlSearchProducttype.SelectedValue == "DPT" && txtSearchProducttype.Text.Trim() != "")
            {
                sqlwhere += " WHERE T40DES like '" + txtSearchProducttype.Text.ToUpper().Trim() + "%' ";
            }

            SqlAll = SqlAll + sqlwhere + " ORDER BY T40TYP ";

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_gvProductType.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (!cookiesStorage.check_dataset(DS))
            {
                
                dataCenter.CloseConnectSQL();
                return;
            }
            gv_ProductType.DataSource = DS;
            gv_ProductType.DataBind();
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region step2 : popup product code
        private void POPUP_PRODUCT_CODE()
        {
            string sqlwhere = "";
            SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE cast(T41TYP as nvarchar) = '" + productTypeSend.Text + "'";

            if (ddlSearchProductCode.SelectedValue == "CPC" && txtSearchProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41COD = " + txtSearchProductCode.Text.Trim() + "";
            }
            if (ddlSearchProductCode.SelectedValue == "DPC" && txtSearchProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41DES like '" + txtSearchProductCode.Text.ToUpper().Trim() + "%' ";
            }

            SqlAll = SqlAll + sqlwhere + " ORDER BY T41COD ";

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_gvProductCode.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (!cookiesStorage.check_dataset(DS))
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            gv_ProductCode.DataSource = DS;
            gv_ProductCode.DataBind();


            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region step2 : popup alert confirm select item 
        protected void BTN_OK_CHECK_ITEM_APP(object sender, EventArgs e)
        {
            if (checkModeStatus.Text == "saveMain")
            {
                if (budgetProductType.SelectedIndex == 1 && gvAddRowsProductType.Rows.Count <= 0)
                {
                    lblMsgAlertProduct.Text = lblMsgAlertProduct.Text = "Budget product type = 'Use Check', Do you want to insert product type ? ";
                    PopupAlertProduct.ShowOnPageLoad = true;

                }
                else
                {
                    SAVE_DATA_ILCM03_ILCM07();
                }
            }
            else if (checkModeStatus.Text == "editMain")
            {
                if (budgetProductType.SelectedIndex == 1 && gvAddRowsProductType.Rows.Count <= 0)
                {
                    lblMsgAlertProduct.Text = lblMsgAlertProduct.Text = "Budget product type = 'Use Check', Do you want to insert product type ? ";
                    PopupAlertProduct.ShowOnPageLoad = true;

                }
                else
                {
                    EDIT_DATA_ILCM03_ILCM07();
                }
            }



        }
        protected void BTN_OK_CHECK_ITEM_PRODUCT(object sender, EventArgs e)
        {
            if (checkModeStatus.Text == "saveMain")
            {
                SAVE_DATA_ILCM03_ILCM07();
            }
            else if (checkModeStatus.Text == "editMain")
            {
                EDIT_DATA_ILCM03_ILCM07();
            }

        }
        protected void BTN_CANCEL_ITEM_APP(object sender, EventArgs e)
        {
            readApplicationType.Focus();
            ContentControlStep2.ActiveTabPage = ContentControlStep2.TabPages.FindByName("ApplicationAndProduct");
        }
        protected void BTN_CANCEL_ITEM_PRODUCT(object sender, EventArgs e)
        {
            readProductType.Focus();
            ContentControlStep2.ActiveTabPage = ContentControlStep2.TabPages.FindByName("ApplicationAndProduct");
        }
        #endregion

        #region step2 : select application type
        protected void CLICK_SELECT_APPLICATION_TYPE(object sender, EventArgs e)
        {
            POPUP_APPLICATION_TYPE();
            Popup_ApplicationType.ShowOnPageLoad = true;

        }
        #endregion

        #region step2 : select product type
        protected void CLICK_SELECT_PRODUCT_TYPE(object sender, EventArgs e)
        {
            POPUP_PRODUCT_TYPE();
            Popup_ProductType.ShowOnPageLoad = true;

        }
        #endregion

        #region step2 : select product code
        protected void CLICK_SELECT_PRODUCT_CODE(object sender, EventArgs e)
        {
            POPUP_PRODUCT_CODE();
            Popup_ProductCode.ShowOnPageLoad = true;

        }
        #endregion

        #region step2 : selection index product type

        protected void PRODUCT_SELECTED_INDEX_CAHNGE(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productType = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            DataRow drProduct = ds_productType.Tables[0]?.Rows[(gv_ProductType.PageIndex * Convert.ToInt16(gv_ProductType.PageSize)) + e.NewSelectedIndex];
            readProductType.Text = drProduct[0].ToString().Trim() + " : " + drProduct[1].ToString().Trim();
            readProductTypeHidden.Text = drProduct[1].ToString().Trim();

            addProductType.Enabled = true;
            Popup_ProductType.ShowOnPageLoad = false;

            return;
        }
        protected void DATA_PRODUCT_SELECTED_INDEX_CAHNGE(object sender, GridViewSelectEventArgs e)
        {

            DataTable dtProductType = (DataTable)ViewState["gridAddRow_productType"];
            DataRow drProduct = dtProductType.Rows[(gv_applicationType.PageIndex * Convert.ToInt16(gv_applicationType.PageSize)) + e.NewSelectedIndex];

            productTypeSend.Text = drProduct[1].ToString();
            productDesSend.Text = drProduct[4].ToString();
            gvAddRowsProductCode.Enabled = true;
            if (drProduct[3].ToString() == "1")
            {
                step2_productCode.Enabled = false;
                productCodes.Enabled = false;
            }
            else
            {
                step2_productCode.Enabled = true;
                productCodes.Enabled = true;
                readProductCode.Enabled = true;
                productTypeSelect.Enabled = true;
                productTypeSelect.Text = drProduct[1].ToString();
            }

        }
        protected void CLICK_ADD_PRODUCT_TYPE(object sender, EventArgs e)
        {
            if (readProductType.Text == "")
            {
                lblMsgAlert.Text = "Please select product type";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else
            {
                DataSet ds_productType = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
                DataRow drProduct = ds_productType.Tables[0]?.Rows[0];

                DataTable dtProduct = (DataTable)ViewState["gridAddRow_productType"];

                int inDex = dtProduct.Rows.Count;
                string[] productType = readProductType.Text.Split(':');
                string pId = productType[0].ToString().Trim();
                string pDes = readProductTypeHidden.Text.Trim();
                string codes = ddlProductType.SelectedItem.Value;
                string description = ddlProductType.SelectedItem.Text;

                if (dtProduct.Rows.Count <= 0)
                {
                    inDex = 1;
                    dtProduct.Rows.Add(inDex.ToString(), pId.ToString().Trim(), pDes.ToString().Trim(), codes.ToString().Trim(), description.ToString().Trim());
                }
                else
                {
                    inDex = dtProduct.Rows.Count + 1;
                    int checkDuplicate = 0;
                    foreach (DataRow row in dtProduct.Rows)

                    {

                        if (row["T40TYP"].ToString() == pId)
                        {
                            checkDuplicate = checkDuplicate + 1;
                        }

                    }

                    if (checkDuplicate > 0)
                    {
                        lblMsgAlert.Text = "Application Type : Data Duplicated";
                        PopupAlertRate.ShowOnPageLoad = true;
                    }
                    else
                    {
                        dtProduct.Rows.Add(inDex.ToString(), pId.ToString().Trim(), pDes.ToString().Trim(), codes.ToString().Trim(), description.ToString().Trim());

                    }
                }


                ViewState["gridAddRow_productType"] = dtProduct;

                gvAddRowsProductType.DataSource = (DataTable)ViewState["gridAddRow_productType"];
                gvAddRowsProductType.DataBind();
                //textId.Text = string.Empty;
                //Popup_ProductCode.ShowOnPageLoad = false;
                return;
            }
        }
        protected void CLICK_DELETE_ROW_PRODUCT_TYPE(object sender, GridViewDeleteEventArgs e)
        {
            ddlProductType.SelectedIndex = 0;
            productTypeSelect.Text = "";
            readProductType.Text = "";
            readProductCode.Text = "";
            productCodes.Enabled = false;
            addProductType.Enabled = false;
            if (ViewState["gridAddRow_productType"] != null)
            {
                DataTable dt = (DataTable)ViewState["gridAddRow_productType"];
                DataRow drCurrentRow = null;


                int rowIndex = Convert.ToInt32(e.RowIndex);
                string indexProductType = Convert.ToInt32(dt.Rows[e.RowIndex]["T40TYP"]).ToString();
                if (dt.Rows.Count >= 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["gridAddRow_productType"] = dt;
                    gvAddRowsProductType.DataSource = dt;
                    gvAddRowsProductType.DataBind();

                    for (int i = 0; i < gvAddRowsProductType.Rows.Count - 1; i++)
                    {
                        gvAddRowsProductType.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    //SetOldData();
                }
                DataTable dts = (DataTable)ViewState["gridAddRow_productCode"];



                for (int i = dts.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow drCurrentRows = dts.Rows[i];

                    if (drCurrentRows["T41TYP"].ToString() == indexProductType)
                    {

                        drCurrentRows.Delete();

                    }

                }
                ViewState["gridAddRow_productCode"] = dts;
                gvAddRowsProductCode.DataSource = dts;
                gvAddRowsProductCode.DataBind();

            }
            return;

        }
        protected void PRODUCT_TYPE_PAGEINDEX_CAHNGE(object sender, GridViewPageEventArgs e)
        {
            gv_ProductType.PageIndex = e.NewPageIndex;
            gv_ProductType.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            gv_ProductType.DataBind();
        }

        #endregion

        #region step2 : selection index product code

        protected void CODE_SELECTED_INDEX_CAHNGE(object sender, GridViewSelectEventArgs e)
        {

            DataSet ds_productCode = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            DataRow drProductCode = ds_productCode.Tables[0]?.Rows[(gv_ProductCode.PageIndex * Convert.ToInt16(gv_ProductCode.PageSize)) + e.NewSelectedIndex];

            DataTable dtProductCode = (DataTable)ViewState["gridAddRow_productCode"];
            readProductCode.Text = drProductCode[1].ToString().Trim() + " : " + drProductCode[2].ToString().Trim();


            int checkDuplicate = 0;
            foreach (DataRow row in dtProductCode.Rows)
            {


                if (row["T41COD"].ToString() == drProductCode[1].ToString() && row["T41TYP"].ToString() == drProductCode[0].ToString())
                {
                    checkDuplicate = checkDuplicate + 1;
                }

            }

            if (checkDuplicate > 0)
            {
                lblMsgAlert.Text = "Product Code : Data Duplicated";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else
            {
                dtProductCode.Rows.Add(drProductCode[0].ToString().Trim(), drProductCode[1].ToString().Trim(), drProductCode[2].ToString().Trim());

            }

            ViewState["gridAddRow_productCode"] = dtProductCode;
            gvAddRowsProductCode.DataSource = (DataTable)ViewState["gridAddRow_productCode"];
            gvAddRowsProductCode.DataBind();
            textId.Text = string.Empty;
            Popup_ProductCode.ShowOnPageLoad = false;
            return;


        }
        protected void CLICK_DELETE_ROW_PRODUCT_CODE(object sender, GridViewDeleteEventArgs e)
        {
            productCodes.Enabled = false;
            productTypeSelect.Text = "";
            readProductCode.Text = "";
            if (ViewState["gridAddRow_productCode"] != null)
            {
                DataTable dt = (DataTable)ViewState["gridAddRow_productCode"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count >= 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["gridAddRow_productCode"] = dt;
                    gvAddRowsProductCode.DataSource = dt;
                    gvAddRowsProductCode.DataBind();

                    for (int i = 0; i < gvAddRowsProductCode.Rows.Count - 1; i++)
                    {
                        gvAddRowsProductCode.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    //SetOldData();
                }
            }

        }
        protected void PRODUCT_CODE_PAGEINDEX_CAHNGE(object sender, GridViewPageEventArgs e)
        {
            gv_ProductCode.PageIndex = e.NewPageIndex;
            gv_ProductCode.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            gv_ProductCode.DataBind();
        }
        #endregion

        #region step2 : selection index appplication type

        protected void APPLICATION_TYPE_SELECTED_INDEX_CAHNGE(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_applicationType = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvApplicationType.Value);
            DataRow drApp = ds_applicationType.Tables[0]?.Rows[(gv_applicationType.PageIndex * Convert.ToInt16(gv_applicationType.PageSize)) + e.NewSelectedIndex];

            DataTable dtApp = (DataTable)ViewState["gridAddRow_applicationType"];

            readApplicationType.Text = drApp[0].ToString().Trim() + " : " + drApp[1].ToString().Trim();

            int inDex = dtApp.Rows.Count;
            string checkedDuplicate = drApp[0].ToString().Trim();
            if (dtApp.Rows.Count <= 0)
            {
                inDex = 1;
                dtApp.Rows.Add(inDex.ToString(), drApp[0].ToString().Trim(), drApp[1].ToString().Trim());
            }
            else
            {
                inDex = dtApp.Rows.Count + 1;
                int checkDuplicate = 0;
                foreach (DataRow row in dtApp.Rows)
                {
                    if (row["GN61CD"].ToString() == drApp[0].ToString())
                    {
                        checkDuplicate = checkDuplicate + 1;
                    }

                }

                if (checkDuplicate > 0)
                {
                    lblMsgAlert.Text = "Application Type : Data Duplicated";
                    PopupAlertRate.ShowOnPageLoad = true;
                }
                else
                {
                    dtApp.Rows.Add(inDex.ToString(), drApp[0].ToString().Trim(), drApp[1].ToString().Trim());

                }
            }
            ViewState["gridAddRow_applicationType"] = dtApp;
            gvAddRowsApplication.DataSource = (DataTable)ViewState["gridAddRow_applicationType"];
            gvAddRowsApplication.DataBind();
            //textId.Text = string.Empty;
            Popup_ApplicationType.ShowOnPageLoad = false;
            return;
        }
        protected void CLICK_DELETE_ROW_APPLICATION_TYPE(object sender, GridViewDeleteEventArgs e)
        {

            readApplicationType.Text = "";
            if (ViewState["gridAddRow_applicationType"] != null)
            {
                DataTable dt = (DataTable)ViewState["gridAddRow_applicationType"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count >= 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["gridAddRow_applicationType"] = dt;
                    gvAddRowsApplication.DataSource = dt;
                    gvAddRowsApplication.DataBind();

                    for (int i = 0; i < gvAddRowsApplication.Rows.Count - 1; i++)
                    {
                        gvAddRowsApplication.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    //SetOldData();
                }
            }

        }
        protected void APPLICATION_TYPE_SELECTED_PAGEINDEX_CAHNGE(object sender, GridViewPageEventArgs e)
        {
            gv_applicationType.PageIndex = e.NewPageIndex;
            gv_applicationType.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvApplicationType.Value);
            gv_applicationType.DataBind();
        }
        #endregion

        #region step2 : selection index list seqNo.
        protected void GV_SEQNO_SELECTED_INDEX_CAHNGE(object sender, GridViewSelectEventArgs e)
        {
            //SetDate();
            ContentControlStep2.ActiveTabPage = ContentControlStep2.TabPages.FindByName("BudgetContract");
            gvAddRowsApplication.DataSource = null;
            gvAddRowsApplication.DataBind();
            gvAddRowsProductType.DataSource = null;
            gvAddRowsProductType.DataBind();
            gvAddRowsProductCode.DataSource = null;
            gvAddRowsProductCode.DataBind();

            DataSet ds_grid_gvSeqNo = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvSeqNo.Value);
            DataRow dr = ds_grid_gvSeqNo.Tables[0]?.Rows[(psVendor.PageIndex * Convert.ToInt16(psVendor.PageSize)) + e.NewSelectedIndex];

            btnMainInsert.Enabled = false;
            btnMainEdit.Enabled = true;
            btnMainDelete.Enabled = true;
            btnMainCancel.Enabled = true;

            seqNo.Text = dr[1].ToString().Trim();

            budgetContract.SelectedIndex = int.Parse(dr[2].ToString());
            double amounts = double.Parse(dr[3].ToString().Trim());
            txtAmount.Text = amounts.ToString("#,##0.00").Trim();
            budgetCriteriaType.SelectedIndex = int.Parse(dr[4].ToString()) - 1;
            budgetTypeComparative.SelectedIndex = int.Parse(dr[5].ToString()) - 1;
            budgetAppType.SelectedIndex = int.Parse(dr[6].ToString());
            budgetProductType.SelectedIndex = int.Parse(dr[7].ToString());
            double BAM = double.Parse(dr[8].ToString().Trim());
            budgetAmount.Text = BAM.ToString("#,##0.00").Trim();
            double BPC = double.Parse(dr[9].ToString().Trim());
            budgetPercentage.Text = BPC.ToString("#,##0.00").Trim();

            if (budgetCriteriaType.Text == "1")
            {
                budgetAmount.Enabled = true;
                budgetPercentage.Enabled = false;
            }
            else if (budgetCriteriaType.Text == "2")
            {
                budgetAmount.Enabled = false;
                budgetPercentage.Enabled = true;
            }
            else
            {
                budgetAmount.Enabled = false;
                budgetPercentage.Enabled = false;
            }

            if ((budgetAppType.SelectedItem.Value.Trim() == "0") && (budgetProductType.SelectedItem.Value.Trim() == "0"))
            {
                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = false;
            }
            else
            {
                ContentControlStep2.TabPages.FindByName("ApplicationAndProduct").Enabled = true;

            }

            SET_DISABLE_ALL_3ITEM();


            SqlAll = "SELECT M07BAM FROM AS400DB01.ILOD0001.ILCM07 WITH (NOLOCK) WHERE CAST(M07REF as nvarchar) ='" + referanceNo.Text + "' AND CAST(M07VEN as nvarchar) ='" + vendorID.Text + "' AND CAST(M07SEQ as nvarchar) ='" + seqNo.Text + "'";

            DataSet DS2 = new DataSet();

            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS2 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (budgetCriteriaType.SelectedItem.Value.Trim() == "3")
            {
                DataTable table = new DataTable();
                if (cookiesStorage.check_dataset(DS2))
                {
                    table = DS2.Tables[0];
                }
                DataRow[] rows = table.Select();
                month1.Text = rows[0]["M07BAM"].ToString();
                month2.Text = rows[1]["M07BAM"].ToString();
                month3.Text = rows[2]["M07BAM"].ToString();
                month4.Text = rows[3]["M07BAM"].ToString();
                month5.Text = rows[4]["M07BAM"].ToString();
                month6.Text = rows[5]["M07BAM"].ToString();
                month7.Text = rows[6]["M07BAM"].ToString();
                month8.Text = rows[7]["M07BAM"].ToString();
                month9.Text = rows[8]["M07BAM"].ToString();
                month10.Text = rows[9]["M07BAM"].ToString();
                month11.Text = rows[10]["M07BAM"].ToString();
                month12.Text = rows[11]["M07BAM"].ToString();
            }


            // application type for grid add rows into table 
            SqlAll = @" SELECT M04APT,GN61DT 
                        FROM AS400DB01.ILOD0001.ILCM04 WITH (NOLOCK)
                        INNER JOIN  AS400DB01.GNOD0000.GNTB61 WITH (NOLOCK) ON M04APT = GN61CD
                        WHERE CAST(M04REF as nvarchar) = '" + referanceNo.Text + "' AND CAST(M04SEQ as nvarchar) = '" + seqNo.Text + "' AND CAST(M04VEN as nvarchar) = '" + vendorID.Text + "'";

            DataSet dsAppType = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsAppType = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (cookiesStorage.check_dataset(dsAppType))
            {
                DataTable dtAppType = new DataTable();
                dtAppType.Columns.AddRange(new DataColumn[3] { new DataColumn("IdApp"), new DataColumn("GN61CD"), new DataColumn("GN61DT") });
                int i = 0;
                foreach (DataRow drsAppType in dsAppType.Tables[0]?.Rows)
                {
                    i++;

                    dtAppType.Rows.Add(i.ToString(), drsAppType[0].ToString(), drsAppType[1].ToString());
                }

                ViewState["gridAddRow_applicationType"] = dtAppType;
                gvAddRowsApplication.DataSource = dtAppType;
                gvAddRowsApplication.DataBind();
                step2_applicationType.Enabled = true;

            }
            else
            {
                step2_applicationType.Enabled = false;
            }


            // product type for grid add rows into table 
            SqlAll = @"SELECT M05PRT, T40DES, M05PFG, T71DES 
						FROM AS400DB01.ILOD0001.ILCM05 WITH (NOLOCK)
						INNER JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON(M05PRT = T40TYP)
						INNER JOIN AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) ON(M05PFG = T71ITM)
						WHERE CAST(M05REF as nvarchar) = '" + referanceNo.Text + "' AND CAST(M05SEQ as nvarchar) ='" + seqNo.Text + "' AND CAST(M05VEN as nvarchar) ='" + vendorID.Text + "' AND T71CDE = '012'";

            DataSet dsProductType = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductType = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(dsProductType))
            {
                DataTable dtProductType = new DataTable();
                dtProductType.Columns.AddRange(new DataColumn[5] { new DataColumn("IdProduct"), new DataColumn("T40TYP"), new DataColumn("T40DES"), new DataColumn("T71ITM"), new DataColumn("T71DES") });
                int j = 0;
                foreach (DataRow drsProductType in dsProductType.Tables[0]?.Rows)
                {
                    j++;

                    dtProductType.Rows.Add(j.ToString(), drsProductType[0].ToString(), drsProductType[1].ToString(), drsProductType[2].ToString(), drsProductType[3].ToString());
                }

                ViewState["gridAddRow_productType"] = dtProductType;
                gvAddRowsProductType.DataSource = dtProductType;
                gvAddRowsProductType.DataBind();
                step2_productType.Enabled = true;
            }
            else
            {
                step2_productType.Enabled = false;
            }




            // product code for grid add rows into table 
            SqlAll = @"SELECT M06PRT,M06PCD,T41DES 
						FROM AS400DB01.ILOD0001.ILCM06 WITH (NOLOCK)
						RIGHT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON (M06PCD = T41COD) AND (M06PRT = T41TYP) 
						WHERE CAST(M06REF as nvarchar) = '" + referanceNo.Text + "' AND CAST(M06SEQ as nvarchar) ='" + seqNo.Text + " 'AND CAST(M06VEN as nvarchar) ='" + vendorID.Text + "'";

            DataSet dsProductCode = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductCode = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(dsProductCode))
            {
                DataTable dtProductCode = new DataTable();
                dtProductCode.Columns.AddRange(new DataColumn[3] { new DataColumn("T41TYP"), new DataColumn("T41COD"), new DataColumn("T41DES") });

                foreach (DataRow drsProductCode in dsProductCode.Tables[0]?.Rows)
                {

                    dtProductCode.Rows.Add(drsProductCode[0].ToString(), drsProductCode[1].ToString(), drsProductCode[2].ToString());
                }

                ViewState["gridAddRow_productCode"] = dtProductCode;
                gvAddRowsProductCode.DataSource = dtProductCode;
                gvAddRowsProductCode.DataBind();
                step2_productCode.Enabled = true;
            }
            else
            {
                step2_productCode.Enabled = false;
            }
            dataCenter.CloseConnectSQL();
            return;
        }
        #endregion

        #region step2 : List seqNo.
        private void ListSeqNo()
        {

            DataSet DS = new DataSet();

            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = @"SELECT M03REF, M03SEQ, M03BCS, M03BCA, M03BCT, M03BCO, M03BAT, M03BPT, M03BAM, M03BPC 
						FROM AS400DB01.ILOD0001.ILCM03 WITH (NOLOCK)
						WHERE CAST(M03REF as nvarchar) = '" + referanceNo.Text.Trim() + "' AND CAST(M03VEN as nvarchar) = '" + vendorID.Text.Trim() + "'";

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (cookiesStorage.check_dataset(DS))
            {
                ds_gvSeqNo.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
                gvSeqNo.DataSource = DS;
                gvSeqNo.DataBind();
            }
            else
            {

                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
            ResetGrid(gvSeqNo, ds_gvSeqNo.Value);

        }

        #endregion

        #region step2 : data
        private void DATA_ILTB71_012()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '012' ORDER BY T71ITM ASC ";
            DataSet DS_012 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_012 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ddlProductType.DataSource = DS_012;
            ddlProductType.DataTextField = "T71DES";
            ddlProductType.DataValueField = "T71ITM";

            ddlProductType.DataBind();



        }
        private void DATA_ILTB71_001()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '001' ORDER BY T71ITM ASC ";
            DataSet DS_001 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_001 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            commissionType.DataSource = DS_001;
            commissionType.DataTextField = "T71DES";
            commissionType.DataValueField = "T71ITM";
            commissionType.DataBind();
        }
        private void DATA_ILTB71_002()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '002' ORDER BY T71ITM ASC ";
            DataSet DS_002 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_002 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetBranch.DataSource = DS_002;
            budgetBranch.DataTextField = "T71DES";
            budgetBranch.DataValueField = "T71ITM";
            budgetBranch.DataBind();

            listboxBBM.DataSource = DS_002;
            listboxBBM.DataTextField = "T71DES";
            listboxBBM.DataValueField = "T71ITM";
            listboxBBM.DataBind();

        }
        private void DATA_ILTB71_003()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '003' ORDER BY T71ITM ASC ";
            DataSet DS_003 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_003 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetType.DataSource = DS_003;
            budgetType.DataTextField = "T71DES";
            budgetType.DataValueField = "T71ITM";
            budgetType.DataBind();

            listboxBTY.DataSource = DS_003;
            listboxBTY.DataTextField = "T71DES";
            listboxBTY.DataValueField = "T71ITM";
            listboxBTY.DataBind();


        }
        private void DATA_ILTB71_004()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '004' ORDER BY T71ITM ASC ";
            DataSet DS_004 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_004 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            listboxRTY.DataSource = DS_004;
            listboxRTY.DataTextField = "T71DES";
            listboxRTY.DataValueField = "T71ITM";
            listboxRTY.DataBind();
        }
        private void DATA_ILTB71_005()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '005' ORDER BY T71ITM ASC ";
            DataSet DS_005 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_005 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            listboxIIR.DataSource = DS_005;
            listboxIIR.DataTextField = "T71DES";
            listboxIIR.DataValueField = "T71ITM";
            listboxIIR.DataBind();
        }
        private void DATA_ILTB71_006()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '006' ORDER BY T71ITM ASC ";
            DataSet DS_006 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_006 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            listboxCBO.DataSource = DS_006;
            listboxCBO.DataTextField = "T71DES";
            listboxCBO.DataValueField = "T71ITM";
            listboxCBO.DataBind();
        }
        private void DATA_ILTB71_007()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '007' ORDER BY T71ITM ASC ";
            DataSet DS_007 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_007 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetContract.DataSource = DS_007;
            budgetContract.DataTextField = "T71DES";
            budgetContract.DataValueField = "T71ITM";
            budgetContract.DataBind();
        }
        private void DATA_ILTB71_008()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '008' ORDER BY T71ITM ASC ";
            DataSet DS_008 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_008 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetCriteriaType.DataSource = DS_008;
            budgetCriteriaType.DataTextField = "T71DES";
            budgetCriteriaType.DataValueField = "T71ITM";
            budgetCriteriaType.DataBind();
        }
        private void DATA_ILTB71_009()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '009' ORDER BY T71ITM ASC ";
            DataSet DS_009 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_009 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetTypeComparative.DataSource = DS_009;
            budgetTypeComparative.DataTextField = "T71DES";
            budgetTypeComparative.DataValueField = "T71ITM";
            budgetTypeComparative.DataBind();
        }
        private void DATA_ILTB71_010()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '010' ORDER BY T71ITM ASC ";
            DataSet DS_010 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_010 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetAppType.DataSource = DS_010;
            budgetAppType.DataTextField = "T71DES";
            budgetAppType.DataValueField = "T71ITM";
            budgetAppType.DataBind();
        }
        private void DATA_ILTB71_011()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '011' ORDER BY T71ITM ASC ";
            DataSet DS_011 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_011 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            budgetProductType.DataSource = DS_011;
            budgetProductType.DataTextField = "T71DES";
            budgetProductType.DataValueField = "T71ITM";
            budgetProductType.DataBind();
        }
        private void DATA_COMISSION_TYPE()
        {
            DataTable dtDueMonth = new DataTable();
            dtDueMonth.Columns.AddRange(new DataColumn[1] { new DataColumn("DAY") });
            ViewState["dataComissionType"] = dtDueMonth;

            for (int i = 1; i <= 28; i++)
            {
                dtDueMonth.Rows.Add(i.ToString());
            }

            dueDay.DataSource = ViewState["dataComissionType"];
            dueDay.DataTextField = "DAY";
            dueDay.DataValueField = "DAY";
            dueDay.DataBind();

        }
        private void DATA_COMISSION_TYPE2()
        {
            DataTable dtDueMonth = new DataTable();
            dtDueMonth.Columns.AddRange(new DataColumn[1] { new DataColumn("DAY") });
            ViewState["dataComissionType"] = dtDueMonth;
            dtDueMonth.Rows.Add(0);
            dueDay.DataSource = ViewState["dataComissionType"];
            dueDay.DataTextField = "DAY";
            dueDay.DataValueField = "DAY";
            dueDay.DataBind();

        }


        #endregion

        #region step1 : popup searching vendor
        private void LoadDataPopup()
        {
            iDB2Command cmd = new iDB2Command();
            string sqlwhere = "";


            if (ddl_popup_SearchBy.SelectedValue == "CV" && txt_Detail.Text.Trim() != "")
            {
                sqlwhere += " AND FORMAT(P10VEN,'000000000000') LIKE '%" + txt_Detail.Text.Trim() + "%'";
            }
            else if (ddl_popup_SearchBy.SelectedValue == "DV" && txt_Detail.Text.Trim() != "")
            {
                sqlwhere += " AND (P10TNM LIKE '%" + txt_Detail.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + txt_Detail.Text.ToUpper().Trim() + "%')";;
            }
            else
            {
                if (vendorID.Text.Trim() != "" && vendorDescription.Text.Trim() != "")
                {
                    sqlwhere += " AND FORMAT(P10VEN,'000000000000') LIKE '%" + vendorID.Text.Trim() + "%'  AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR P10NAM LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                }
                else if (vendorID.Text.Trim() != "")
                {
                    sqlwhere += " AND FORMAT(P10VEN,'000000000000') LIKE '%" + vendorID.Text.Trim() + "%'";
                }
                else if (vendorDescription.Text.Trim() != "")
                {
                    sqlwhere += " AND (P10TNM LIKE '%" + vendorDescription.Text.Trim() + "%' OR UPPER(P10NAM) LIKE '%" + vendorDescription.Text.ToUpper().Trim() + "%') ";
                }
            }
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = @"SELECT DISTINCT FORMAT(P10VEN,'000000000000') AS P10VEN, P10NAM, P10CTY, P10CPD, P10PTY, 
						CASE WHEN P10VEN = P10PVN THEN P10VEN ELSE P10PVN END AS P10PVN, 
						CASE WHEN P10PVN = 0 THEN '' ELSE P10NAM END AS PAY_VEND_NAME 
						FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
						WHERE P10DEL = ''";

            SqlAll = SqlAll + sqlwhere + " ORDER BY P10VEN ASC";
            cmd.CommandText = SqlAll;
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (cookiesStorage.check_dataset(DS))
            {
                psVendor.DataSource = DS;
                psVendor.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }


        #region step1 : selection index list date of vendor 

        protected void GV_ONBOUND_ROW_LISTVENDOR(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            foreach (TableCell tc in e.Row.Cells)
            {
                tc.BorderStyle = BorderStyle.None;
            }
            if (highLightRow.Text.ToString() != "")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex == int.Parse(highLightRow.Text.ToString()))
                    {
                        e.Row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                        e.Row.ToolTip = string.Empty;
                    }
                }
            }

        }

        protected void GV_VENDOR_SELECTED_INDEX_CHANGE(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gvListDateVendor.Value);
            DataRow dr = ds_grid.Tables[0]?.Rows[(gvListDateVendor.PageIndex * Convert.ToInt16(gvListDateVendor.PageSize)) + e.NewSelectedIndex];
            referanceNo.Text = dr[2].ToString().Trim();
            startDate.Text = dr[0].ToString().Trim();
            endDate.Text = dr[1].ToString().Trim();
            highLightRow.Text = e.NewSelectedIndex.ToString();
            foreach (GridViewRow row in gvListDateVendor.Rows)
            {
                if (row.RowIndex == int.Parse(highLightRow.Text))
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }


            listboxBBM.SelectedIndex = int.Parse(dr[3].ToString()) - 1;
            listboxBTY.SelectedIndex = int.Parse(dr[4].ToString()) - 1;
            listboxRTY.SelectedIndex = int.Parse(dr[5].ToString()) - 1;
            listboxIIR.SelectedIndex = int.Parse(dr[6].ToString()) - 1;
            listboxCBO.SelectedIndex = int.Parse(dr[7].ToString()) - 1;


            startDateStep2.Text = dr[0].ToString().Trim();
            endDateStep2.Text = dr[1].ToString().Trim();

            btnMainEdit.Enabled = true;
            btnMainDelete.Enabled = true;
            btnMainCancel.Enabled = true;
            btnMainInsert.Enabled = false;

            step1_box1.Enabled = true;
            step1_box3.Enabled = false;
            txtStartAmount.Enabled = false;

            if (dr[4].ToString().Trim() == "1")
            {
                ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
            }
            else
            {
                ContentControlStep1.TabPages.FindByName("Step2").Enabled = true;
            }

            if (listboxRTY.Text.ToString().Trim() == "1")
            {
                txtStartAmount.Text = "0.00";
                txtEndAmount.Text = "9,999,999,999,999.99";
                txtRate.Text = "0.00";
            }
            else
            {
                txtStartAmount.Text = "0.00";
                txtEndAmount.Text = "0.00";
                txtRate.Text = "0.00";
            }

            SqlAll = "SELECT  M02SAM, M02EAM, M02CMR FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE CAST(M02REF as nvarchar) = '" + referanceNo.Text + "'";


            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (referanceNo.Text != "")
            {
                ListSeqNo();
            }
            DataTable dtDb = new DataTable();
            dtDb.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"), new DataColumn("M02SAM"), new DataColumn("M02EAM"), new DataColumn("M02CMR") });
            int i = 0;
            if (cookiesStorage.check_dataset(DS))
            {
                foreach (DataRow drs in DS.Tables[0]?.Rows)
                {
                    i++;

                    dtDb.Rows.Add(i.ToString(), drs[0].ToString(), drs[1].ToString(), drs[2].ToString());
                }
            }
            

            ViewState["gridAddRow"] = dtDb;
            gvAddRows.DataSource = dtDb;
            gvAddRows.DataBind();
            dataCenter.CloseConnectSQL();
            return;


        }
        protected void PS_VENDOR_SELECTED_INDEX_CHANGE(object sender, GridViewSelectEventArgs e)
        {
            SetDate();
            DATA_ILTB71_001();

            CLEAR_GRIDVIEW1();
            DISIBLE_ALL();
            DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
            DataRow dr = ds_grid.Tables[0]?.Rows[(psVendor.PageIndex * Convert.ToInt16(psVendor.PageSize)) + e.NewSelectedIndex];

            if (dr[2].ToString().Trim() != "")
            {
                if (int.Parse(dr[2].ToString().Trim()) == 1)
                {
                    DATA_COMISSION_TYPE2();
                }
                else if (int.Parse(dr[2].ToString().Trim()) == 2)
                {
                    DATA_COMISSION_TYPE();
                }
            }

            checkStepStatus.Text = "activeStep1";
            vendorID.Text = dr[0].ToString().Trim();
            vendorDescription.Text = dr[1].ToString().Trim();
            paymentVendor.Text = dr[5].ToString().Trim();
            paymentDesciption.Text = dr[6].ToString().Trim();

            if (dr[4].ToString().Trim() == "1")
            {
                createPayment.Text = "Medie Clearing";
            }
            else if (dr[4].ToString().Trim() == "2")
            {
                createPayment.Text = "Cheque";
            }
            else if (dr[4].ToString().Trim() == "3")
            {
                createPayment.Text = "Medie Clearing";
            }

            dueDays = dr[3].ToString().Trim();
            hisCommissionType.Text = dr[2].ToString().Trim();
            hisDueDay.Text = dr[3].ToString().Trim();

            if (dr[2].ToString().Trim() != "")
            {
                commissionType.SelectedIndex = int.Parse(dr[2].ToString().Trim()) - 1;
            }
            dueDay.SelectedIndex = int.Parse(dr[3].ToString().Trim()) - 1;
            referanceNo.Text = "";
            btnEdit.Enabled = true;
            btnRefresh.Enabled = true;
            CalendarExtender1.Enabled = false;
            CalendarExtender2.Enabled = false;
            btnSave.Enabled = true;
            btnMainInsert.Enabled = true;
            ContentControlStep1.Enabled = true;


            Popup_SelectVendor.ShowOnPageLoad = false;
            ResetGrid(psVendor, ds_popup.Value);

            ListDateVendor();
            ContentControlStep1.ActiveTabPage = ContentControlStep1.TabPages.FindByName("Step1");
            ContentControlStep1.TabPages.FindByName("Step2").Enabled = false;
            highLightRow.Text = "";
            return;

        }
        protected void PS_VENDOR_SELECTED_PAGEINDEX_CHANGE(object sender, GridViewPageEventArgs e)
        {
            psVendor.PageIndex = e.NewPageIndex;
            psVendor.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
            psVendor.DataBind();
        }
        #endregion

        private void ResetGrid(GridView GridView, string ds)
        {
            GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
            GridView.DataBind();
        }
        protected void CLICK_SEARCH_VENDOR(object sender, EventArgs e)
        {
            LoadDataPopup();
        }
        protected void CLICK_SEARCH_APP_TYPE(object sender, EventArgs e)
        {
            POPUP_APPLICATION_TYPE();
        }
        protected void CLICK_SEARCH_PRODUCT_TYPE(object sender, EventArgs e)
        {
            POPUP_PRODUCT_TYPE();
        }
        protected void CLICK_SEARCH_PRODUCT_CODE(object sender, EventArgs e)
        {
            POPUP_PRODUCT_CODE();
        }

        protected void CLEAR_POPUP_VENDOR(object sender, EventArgs e)
        {
            vendorID.Text = "";
            vendorDescription.Text = "";
            paymentVendor.Text = "";
            paymentDesciption.Text = "";
            createPayment.Text = "";
            referanceNo.Text = "";
            ddl_popup_SearchBy.SelectedIndex = 0;
            txt_Detail.Text = "";
            LoadDataPopup();
            //DEFAULT_STEP1();

        }
        protected void CLEAR_POPUP_APPTYPE(object sender, EventArgs e)
        {

            ddlSearchApptype.SelectedIndex = 0;
            txtSearchAppType.Text = "";
            POPUP_APPLICATION_TYPE();
        }
        protected void CLEAR_POPUP_PRODUCTTYPE(object sender, EventArgs e)
        {

            ddlSearchProducttype.SelectedIndex = 0;
            txtSearchProducttype.Text = "";
            POPUP_PRODUCT_TYPE();
        }
        protected void CLEAR_POPUP_PRODUCTCODE(object sender, EventArgs e)
        {

            ddlSearchProductCode.SelectedIndex = 0;
            txtSearchProductCode.Text = "";
            POPUP_PRODUCT_CODE();
        }
        #endregion

        #region step1 : onchange rate type
        protected void ONCHANGE_LISTBOX(object sender, EventArgs e)
        {
            DropDownList listboxRTY = sender as DropDownList;
            checkMode = checkModeStatus.Text;
            checkMode = "saveMain";
            if (checkMode == "saveMain" || checkMode == "editMain")
            {
                if (listboxRTY.SelectedItem.Value.Trim() == "1")
                {
                    CLEAR_GRIDVIEW1();
                    btnMainSave.Enabled = true;
                    btnMainCancel.Enabled = true;
                    txtEndAmount.Enabled = false;
                    txtEndAmount.Text = "9,999,999,999,999.99";
                    ;
                }
                else if (listboxRTY.SelectedItem.Value.Trim() == "2")
                {
                    CLEAR_GRIDVIEW1();
                    btnMainSave.Enabled = true;
                    btnMainCancel.Enabled = true;
                    txtEndAmount.Enabled = true;
                    txtEndAmount.Text = "0.00";

                }
            }
        }
        #endregion

        #region step1 : List Date of Vendor
        private void ListDateVendor()
        {
            SqlAll = @" SELECT SUBSTRING(CAST(M01SDT as varchar),7,2) + '/' +  SUBSTRING(CAST(M01SDT AS varchar), 5, 2) + '/' + SUBSTRING(CAST(M01SDT AS varchar), 1, 4) AS M01SDT,
						SUBSTRING(CAST(M01EDT AS varchar), 7, 2) + '/' + SUBSTRING(CAST(M01EDT AS varchar), 5, 2) + '/' + SUBSTRING(CAST(M01EDT AS varchar), 1, 4) AS M01EDT, M01REF ,M01BBM ,M01BTY ,M01RTY ,M01I0P ,M01CBO
						FROM AS400DB01.ILOD0001.ILCM01 WITH (NOLOCK) WHERE FORMAT(m01ven,'000000000000') = '" + vendorID.Text.Trim() + "'";
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_gvListDateVendor.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (cookiesStorage.check_dataset(DS))
            {
                gvListDateVendor.DataSource = DS;
                gvListDateVendor.DataBind();
                dataCenter.CloseConnectSQL();
                ResetGrid(gvListDateVendor, ds_gvListDateVendor.Value);
                btnMainInsert.Enabled = true;
                step1_box1.Enabled = true;
            }
            else
            {
                gvListDateVendor.DataSource = DS;
                gvListDateVendor.DataBind();
                dataCenter.CloseConnectSQL();
                return;
            }
            
        }
        #endregion

        #region step1 : function add row for table

        protected void BindGrid()
        {
            gvAddRows.DataSource = (DataTable)ViewState["gridAddRow"];
            gvAddRows.DataBind();
        }

        protected void CLICK_ADD_ROW(object sender, EventArgs e)
        {

            btnMainCancel.Enabled = true;
            btnMainSave.Enabled = true;
            step1_box2.Enabled = true;
            step1_box3.Enabled = true;
            if (txtRate.Text == "")
            {
                checkRate = double.Parse("0.00");
            }
            else
            {
                checkRate = double.Parse(txtRate.Text);
            }

            if (txtRate.Text == "" || txtRate.Text == "0.00")
            {
                lblMsgAlert.Text = "Please input rate %";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (checkRate > 99)
            {
                lblMsgAlert.Text = "Rate should not exceed 99%";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else
            {

                DataTable dt = (DataTable)ViewState["gridAddRow"];

                int inDex = dt.Rows.Count;

                if (dt.Rows.Count <= 0)
                {
                    inDex = 1;
                    txtStartAmount.Text = "0.00";
                }
                else
                {
                    inDex = dt.Rows.Count + 1;
                }

                if (listboxRTY.SelectedItem.Value.Trim() == "1")
                {
                    int checkRateType = dt.Rows.Count;
                    if (checkRateType < 1)
                    {
                        dt.Rows.Add(inDex.ToString(), txtStartAmount.Text.Trim(), txtEndAmount.Text.Trim(), txtRate.Text.Trim());
                    }
                    else
                    {
                        txtStartAmount.Text = "0.00";
                        txtEndAmount.Text = "9,999,999,999,999.99";
                        lblMsgAlert.Text = "In case rate type = 'Fix Rate' commission rate must have only one value";
                        PopupAlertRate.ShowOnPageLoad = true;
                    }
                }
                else if (listboxRTY.SelectedItem.Value.Trim() == "2")
                {
                    dt.Rows.Add(inDex.ToString(), txtStartAmount.Text.Trim(), txtEndAmount.Text.Trim(), txtRate.Text.Trim());
                    double eDtae = double.Parse(txtEndAmount.Text.Trim()) + 0.01;
                    txtEndAmount.Text = eDtae.ToString("#,##0.00");

                    double sDtae = double.Parse(txtEndAmount.Text.Trim());
                    txtStartAmount.Text = sDtae.ToString("#,##0.00");
                }

                ViewState["gridAddRow"] = dt;
                this.BindGrid();
                textId.Text = string.Empty;
                //txtStartAmount.Text = string.Empty;
                //txtEndAmount.Text = string.Empty;
                //txtRate.Text = string.Empty;
            }
        }
        protected void CLICK_DELETE_ROW(object sender, EventArgs e)
        {
            btnMainCancel.Enabled = true;
            btnMainSave.Enabled = true;
            step1_box2.Enabled = true;
            step1_box3.Enabled = true;

            DataTable dt = ViewState["gridAddRow"] as DataTable;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                if (dr["Id"].ToString() == dt.Rows.Count.ToString())
                {
                    if (listboxRTY.SelectedItem.Value.Trim() == "1")
                    {
                        txtStartAmount.Text = "0.00";
                        txtEndAmount.Text = "9,999,999,999,999.99";
                    }
                    else if (listboxRTY.SelectedItem.Value.Trim() == "2")
                    {
                        if (dt.Rows.Count > 1)
                        {
                            double sDtae = double.Parse(txtStartAmount.Text.Trim()) - 0.01;
                            txtStartAmount.Text = sDtae.ToString("#,##0.00");
                            double eDtae = double.Parse(txtEndAmount.Text.Trim()) - 0.01;
                            txtEndAmount.Text = eDtae.ToString("#,##0.00");
                        }
                        else
                        {
                            txtStartAmount.Text = "0.00";
                            txtEndAmount.Text = "0.00";

                        }
                    }



                    dr.Delete();
                    BindGrid();

                    return;
                }

            }

        }
        #endregion

        #region step1 & step2 : function insert & edit 
        protected void CLICK_MAIN_SAVE(object sender, EventArgs e)
        {

            iDB2Command cmd = new iDB2Command();
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string[] startSplitDate = startDate.Text.Split('/');
            string StartDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
            string[] EndSplitDate = endDate.Text.Split('/');
            string EndDateNew = EndSplitDate[2] + EndSplitDate[1] + EndSplitDate[0];
            //SqlAll = "SELECT P99RUN+1 FROM ILMS99 WHERE P99REC = '600'";

            //DS = ilObj.RetriveAsDataSet(SqlAll);
            //    DataRow drs = DS.Tables[0].Rows[0];
            //    string keys = drs[0].ToString().Trim();
            //    int referanceNoNew = int.Parse(keys);
            // ไม่ได้เรียกใช้ store เพราะหา Call_ILSR99 ไม่เจอ

            if (checkStepStatus.Text == "activeStep1")
            {
                if (checkModeStatus.Text == "saveMain")
                {
                    if (gvListDateVendor.Rows.Count > 0)
                    {
                        DataSet dsDateNew = new DataSet();
                        SqlAll = "SELECT MAX(M02EDT) FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE CAST(M02VEN AS nvarchar) = '" + vendorID.Text + "'";
                        dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                        DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];

                        DateTime dtime;
                        DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                                  DateTimeStyles.None, out dtime);

                        string formatDateCheck = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime dCheck = Convert.ToDateTime(formatDateCheck);
                        DateTime newDate = dCheck.AddDays(Convert.ToInt32(1));
                        string[] splitDateCheck = newDate.ToShortDateString().Split('/');
                        int dateTranfer = int.Parse(splitDateCheck[2] + splitDateCheck[1] + splitDateCheck[0]);

                    }
                    else
                    {

                        dateTranfer = int.Parse(m_UpdDate.ToString());
                    }

                    if (Int64.Parse(StartDateNew) > Int64.Parse(EndDateNew))
                    {
                        lblMsgAlert.Text = "You made a mistake date input, please check date.";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }
                    else if ((startDate.Text.Trim().Length != 10) || (endDate.Text.Trim().Length != 10))
                    {
                        lblMsgAlert.Text = "You made a mistake date input, please check format date.";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }

                    //else if (int.Parse(StartDateNew) < dateTranfer)
                    //{
                    //    lblMsgAlert.Text = "You made a mistake date input, please check date.";
                    //    PopupAlertRate.ShowOnPageLoad = true;
                    //}
                    else if (gvAddRows.Rows.Count <= 0)
                    {
                        lblMsgAlert.Text = "You have not selected rate %";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }
                    else
                    {


                        SqlAll = "insert into AS400DB01.ILOD0001.ilcm01(m01ref, m01ven, m01sdt, m01edt, m01bbm, m01bty, m01rty, m01i0p, m01cbo, m01udt, m01utm, m01uus, m01upg, m01uws, m01del)"
                                      + "VALUES('" + referanceNo.Text + "'"
                                      + "," + vendorID.Text
                                      + "," + StartDateNew
                                      + "," + EndDateNew
                                      + "," + listboxBBM.Text
                                      + "," + listboxBTY.Text
                                      + "," + listboxRTY.Text
                                      + "," + listboxIIR.Text
                                      + "," + listboxCBO.Text
                                      + "," + m_UpdDate
                                      + "," + m_UpdTime
                                      + ",'" + m_userInfo.Username.ToString() + "'"
                                      + ",'ILE002014'"
                                      + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                      + ",' ')";

                        cmd.CommandText = SqlAll;

                        try
                        {
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int insertILCM01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (insertILCM01 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "Can't save data , Please check command query : insert ilcm01 ! ";
                                MsgHText = "Error Query";
                                SET_MSG();

                                return;
                            }

                        }
                        catch (Exception)
                        {

                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be saved";
                            MsgHText = "Error";
                            SET_MSG();
                            return;
                        }

                        foreach (GridViewRow row in gvAddRows.Rows)
                        {
                            double sAmount = double.Parse(row.Cells[1].Text);
                            double eAmount = double.Parse(row.Cells[2].Text);
                            double rates = double.Parse(row.Cells[3].Text);
                            SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM02 (M02REF, M02VEN, M02SDT, M02EDT, M02SAM, M02EAM, M02CMR, M02UDT, M02UTM, M02UUS, M02UPG, M02UWS, M02DEL)"
                                   + "VALUES('" + referanceNo.Text + "'"
                                   + "," + vendorID.Text
                                   + "," + StartDateNew
                                   + "," + EndDateNew
                                   + "," + sAmount
                                   + "," + eAmount
                                   + "," + rates
                                   + "," + m_UpdDate
                                   + "," + m_UpdTime
                                   + ",'" + m_userInfo.Username.ToString() + "'"
                                   + ",'ILE002014'"
                                   + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                   + ",' ')";

                            cmd.CommandText = SqlAll;
                            try
                            {
                                bool transaction = dataCenter.Sqltr == null ? true : false;
                                int insertILCM02 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                                if (insertILCM02 == -1)
                                {
                                    dataCenter.RollbackMssql();
                                    dataCenter.CloseConnectSQL();

                                    Msg = "Can't save data , Please check command query : insert ilcm02 ! ";
                                    MsgHText = "Error Query";
                                    SET_MSG();

                                    return;
                                }

                            }
                            catch (Exception)
                            {

                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "The data cannot be saved";
                                MsgHText = "Error";
                                SET_MSG();
                                return;
                            }



                        }

                        SqlAll = "UPDATE AS400DB01.ILOD0001.ILMS99 SET P99RUN = " + referanceNo.Text + ",P99UPD = " + m_UpdDate + ",P99TIM = " + m_UpdTime + ",P99UPG = 'ILSR92' , P99USR = '" + m_userInfo.Username.ToString() + "', P99DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "' WHERE P99REC = '600'";
                        cmd.CommandText = SqlAll;

                        try
                        {
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int updateIndex = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (updateIndex == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "Can't save data , Please check command query : update ilms99 ! ";
                                MsgHText = "Error Query";
                                SET_MSG();

                                return;
                            }

                        }
                        catch (Exception)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be saved";
                            MsgHText = "Error";
                            SET_MSG();
                            return;
                        }
                        dataCenter.CommitMssql();
                        cmd.Parameters.Clear();
                        dataCenter.CloseConnectSQL();
                        SET_MSG_SUCCESS();
                    }

                }
                else if (checkModeStatus.Text == "editMain")
                {
                    if (gvListDateVendor.Rows.Count > 0)
                    {
                        DataSet dsDateNew = new DataSet();
                        SqlAll = "SELECT MAX(M02EDT) FROM AS400DB01.ILOD0001.ILCM02 WHERE M02VEN = " + vendorID.Text + "";
                        dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                        DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];

                        DateTime dtime;
                        DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                                  DateTimeStyles.None, out dtime);

                        string formatDateCheck = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime dCheck = Convert.ToDateTime(formatDateCheck);
                        DateTime newDate = dCheck.AddDays(Convert.ToInt32(1));
                        string[] splitDateCheck = newDate.ToShortDateString().Split('/');
                        int dateTranfer = int.Parse(splitDateCheck[2] + splitDateCheck[1] + splitDateCheck[0]);

                    }
                    else
                    {

                        dateTranfer = int.Parse(m_UpdDate.ToString());
                    }

                    if (Int64.Parse(StartDateNew) > Int64.Parse(EndDateNew))
                    {
                        lblMsgAlert.Text = "You made a mistake date input, please check date.";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }
                    else if ((startDate.Text.Trim().Length != 10) || (endDate.Text.Trim().Length != 10))
                    {
                        lblMsgAlert.Text = "You made a mistake date input, please check format date.";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }

                    //else if (int.Parse(StartDateNew) < dateTranfer)
                    //{
                    //    lblMsgAlert.Text = "You made a mistake date input, please check date.";
                    //    PopupAlertRate.ShowOnPageLoad = true;
                    //}
                    else if (gvAddRows.Rows.Count <= 0)
                    {
                        lblMsgAlert.Text = "You have not selected rate %";
                        PopupAlertRate.ShowOnPageLoad = true;

                    }
                    else
                    {
                        SqlAll = "UPDATE AS400DB01.ILOD0001.ILCM01 SET M01SDT = " + StartDateNew
                                                   + ", M01EDT = " + EndDateNew
                                                   + ", M01BBM = " + listboxBBM.Text
                                                   + ", M01BTY = " + listboxBTY.Text
                                                   + ", M01RTY = " + listboxRTY.Text
                                                   + ", M01I0P = " + listboxIIR.Text
                                                   + ", M01CBO = " + listboxCBO.Text
                                                   + ", M01UDT = " + m_UpdDate
                                                   + ", M01UTM = " + m_UpdTime
                                                   + ", M01UUS = '" + m_userInfo.Username.ToString() + "'"
                                                   + ", M01UPG = 'ILE002014'"
                                                   + ", M01UWS = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                                   + " WHERE M01REF = " + referanceNo.Text + " AND M01VEN =" + vendorID.Text + "";

                        cmd.CommandText = SqlAll;
                        try
                        {
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int editILCM01S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (editILCM01S1 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "Can't edit data , Please check command query : update ilcm01 ! ";
                                MsgHText = "Error Query";
                                SET_MSG();

                                return;
                            }

                        }
                        catch (Exception)
                        {

                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be update";
                            MsgHText = "Error";
                            SET_MSG();
                            return;
                        }

                        SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM02 WHERE M02REF = " + referanceNo.Text + " AND M02VEN = " + vendorID.Text + "";

                        cmd.CommandText = SqlAll;
                        try
                        {
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int deleteILCM02S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (deleteILCM02S1 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "Can't edit data , Please check command query : update ilcm02 ! ";
                                MsgHText = "Error Query";
                                SET_MSG();

                                return;
                            }

                        }
                        catch (Exception)
                        {

                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be update";
                            MsgHText = "Error";
                            SET_MSG();
                            return;
                        }
                        foreach (GridViewRow row in gvAddRows.Rows)
                        {
                            double sAmount = double.Parse(row.Cells[1].Text);
                            double eAmount = double.Parse(row.Cells[2].Text);
                            double rates = double.Parse(row.Cells[3].Text);
                            SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM02 (M02REF, M02VEN, M02SDT, M02EDT, M02SAM, M02EAM, M02CMR, M02UDT, M02UTM, M02UUS, M02UPG, M02UWS, M02DEL)"
                                   + "VALUES('" + referanceNo.Text + "'"
                                   + "," + vendorID.Text
                                   + "," + StartDateNew
                                   + "," + EndDateNew
                                   + "," + sAmount
                                   + "," + eAmount
                                   + "," + rates
                                   + "," + m_UpdDate
                                   + "," + m_UpdTime
                                   + ",'" + m_userInfo.Username.ToString() + "'"
                                   + ",'ILE002014'"
                                   + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                   + ",' ')";

                            cmd.CommandText = SqlAll;
                            try
                            {
                                bool transaction = dataCenter.Sqltr == null ? true : false;
                                int insertILCM02S1s = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                                if (insertILCM02S1s == -1)
                                {
                                    dataCenter.RollbackMssql();
                                    dataCenter.CloseConnectSQL();

                                    Msg = "Can't save data , Please check command query : insert ilcm02 ! ";
                                    MsgHText = "Error Query";
                                    SET_MSG();

                                    return;
                                }
                            }
                            catch (Exception)
                            {

                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "The data cannot be saved";
                                MsgHText = "Error";
                                SET_MSG();
                                return;
                            }

                        }
                        SqlAll = "UPDATE AS400DB01.ILOD0001.ILCM03 SET M03SDT = " + StartDateNew + ", M03EDT = " + EndDateNew + " WHERE M03REF = " + referanceNo.Text + " AND M03VEN =" + vendorID.Text + "";

                        cmd.CommandText = SqlAll;
                        try
                        {
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int editILCM01S3S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (editILCM01S3S1 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();

                                Msg = "Can't edit data , Please check command query : update ilcm03(Step1) ! ";
                                MsgHText = "Error Query";
                                SET_MSG();

                                return;
                            }

                        }
                        catch (Exception)
                        {

                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be update";
                            MsgHText = "Error";
                            SET_MSG();
                            return;
                        }
                        highLightRow.Text = "";
                        dataCenter.CommitMssql();
                        cmd.Parameters.Clear();
                        dataCenter.CloseConnectSQL();
                        SET_MSG_SUCCESS();

                    }
                }

            }
            else if (checkStepStatus.Text == "activeStep2")
            {
                if (checkModeStatus.Text == "saveMain")
                {
                    double checkRatePercentage = double.Parse(budgetPercentage.Text);
                    if ((checkRatePercentage > 99 && budgetPercentage.Text != "0.00") || (checkRatePercentage > 99 && budgetPercentage.Text != ""))
                    {
                        lblMsgAlert.Text = "Rate should not exceed 99%";
                        PopupAlertRate.ShowOnPageLoad = true;
                    }
                    else
                    {
                        if (budgetAppType.SelectedIndex == 1 && gvAddRowsApplication.Rows.Count <= 0)
                        {
                            lblMsgAlertApp.Text = "Budget app type = 'Use Check', Do you want to insert application type ? ";
                            PopupAlertApp.ShowOnPageLoad = true;

                        }
                        else if (budgetProductType.SelectedIndex == 1 && gvAddRowsProductType.Rows.Count <= 0)
                        {
                            lblMsgAlertProduct.Text = "Budget product type = 'Use Check', Do you want to insert product type ? ";
                            PopupAlertProduct.ShowOnPageLoad = true;

                        }
                        else
                        {
                            SAVE_DATA_ILCM03_ILCM07();
                        }
                    }
                }
                else if (checkModeStatus.Text == "editMain")
                {
                    double checkRatePercentage = double.Parse(budgetPercentage.Text);
                    if ((checkRatePercentage > 99 && budgetPercentage.Text != "0.00") || (checkRatePercentage > 99 && budgetPercentage.Text != ""))
                    {
                        lblMsgAlert.Text = "Rate should not exceed 99%";
                        PopupAlertRate.ShowOnPageLoad = true;
                    }
                    else
                    {
                        if (budgetAppType.SelectedIndex == 1 && gvAddRowsApplication.Rows.Count <= 0)
                        {
                            lblMsgAlertApp.Text = "Budget app type = 'Use Check', Do you want to insert application type ? ";
                            PopupAlertApp.ShowOnPageLoad = true;

                        }
                        else if (budgetProductType.SelectedIndex == 1 && gvAddRowsProductType.Rows.Count <= 0)
                        {
                            lblMsgAlertProduct.Text = "Budget product type = 'Use Check', Do you want to insert product type ? ";
                            PopupAlertProduct.ShowOnPageLoad = true;

                        }
                        else
                        {
                            EDIT_DATA_ILCM03_ILCM07();
                        }

                    }
                }
            }
        }

        #endregion


        #region Step2 : SAVE DATA ILCM03-ILCM07
        protected void SAVE_DATA_ILCM03_ILCM07()
        {
            iDB2Command cmd = new iDB2Command();
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string[] startSplitDate = startDate.Text.Split('/');
            string StartDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
            string[] EndSplitDate = endDate.Text.Split('/');
            string EndDateNew = EndSplitDate[2] + EndSplitDate[1] + EndSplitDate[0];

            DataSet dsDateNew = new DataSet();
            SqlAll = "SELECT MAX(M02EDT) FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE CAST(M02VEN as nvarchar) = '" + vendorID.Text + "'";
            dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];

            DateTime dtime;
            DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);

            string formatDateCheck = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dCheck = Convert.ToDateTime(formatDateCheck);
            DateTime newDate = dCheck.AddDays(Convert.ToInt32(1));
            string[] splitDateCheck = newDate.ToShortDateString().Split('/');
            int dateTranfer = int.Parse(splitDateCheck[2] + splitDateCheck[1] + splitDateCheck[0]);

            SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM03(M03REF, M03SEQ, M03BRN, M03VEN, M03SDT, M03EDT, M03BCS, M03BCA, M03BCT, M03BCO, M03BAT, M03BPT, M03BAM, M03BPC, M03UDT, M03UTM, M03UUS, M03UPG, M03UWS, M03DEL)"
                              + "VALUES('" + referanceNo.Text + "'"
                              + "," + seqNo.Text
                              + "," + cookiesStorage.GetCookiesStringByKey("branch")
                              + "," + vendorID.Text
                              + "," + StartDateNew
                              + "," + EndDateNew
                              + "," + budgetContract.Text
                              + "," + double.Parse(txtAmount.Text)
                              + "," + budgetCriteriaType.Text
                              + "," + budgetTypeComparative.Text
                              + "," + budgetAppType.Text
                              + "," + budgetProductType.Text
                              + "," + double.Parse(budgetAmount.Text)
                              + "," + budgetPercentage.Text
                              + "," + m_UpdDate
                              + "," + m_UpdTime
                              + ",'" + m_userInfo.Username.ToString() + "'"
                              + ",'ILE002014'"
                              + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                              + ",' ')";
            cmd.CommandText = SqlAll;
            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int insertILCM03 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (insertILCM03 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "Can't save data , Please check command query : insert ilcm03 ! ";
                    MsgHText = "Error Query";
                    SET_MSG();

                    return;
                }

            }
            catch (Exception)
            {

                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = "The data cannot be saved";
                MsgHText = "Error";
                SET_MSG();
                return;
            }

            List<double> months = new List<double> { };
            months.Add(double.Parse(month1.Text));
            months.Add(double.Parse(month2.Text));
            months.Add(double.Parse(month3.Text));
            months.Add(double.Parse(month4.Text));
            months.Add(double.Parse(month5.Text));
            months.Add(double.Parse(month6.Text));
            months.Add(double.Parse(month7.Text));
            months.Add(double.Parse(month8.Text));
            months.Add(double.Parse(month9.Text));
            months.Add(double.Parse(month10.Text));
            months.Add(double.Parse(month11.Text));
            months.Add(double.Parse(month12.Text));

            double[] monthsList = months.ToArray();

            for (int j = 1; j < 13; j++)
            {
                double monthsAmount = monthsList[j - 1];

                SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM07(M07REF, M07SEQ, M07BRN, M07VEN, M07MON, M07BAM, M07UDT, M07UTM, M07UUS, M07UPG, M07UWS, M07DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + j
                        + "," + monthsAmount
                        + "," + m_UpdDate
                        + "," + m_UpdTime
                        + ",'" + m_userInfo.Username.ToString() + "'"
                        + ",'ILE002014'"
                        + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                        + ",' ')";
                cmd.CommandText = SqlAll;
                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int insertILCM07 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (insertILCM07 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't save data , Please check command query : insert ilcm07 ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                }
                catch (Exception)
                {

                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be saved";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
            }
            if (gvAddRowsApplication.Rows.Count > 0)
            {
                foreach (GridViewRow rowApplication in gvAddRowsApplication.Rows)

                {
                    string numAppTypes = rowApplication.Cells[1].Text;

                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM04(M04REF, M04SEQ, M04BRN, M04VEN, M04APT, M04UDT, M04UTM, M04UUS, M04UPG, M04UWS, M04DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + ",'" + numAppTypes
                    + "'," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM04 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM04 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm04 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            if (gvAddRowsProductType.Rows.Count > 0)
            {
                foreach (GridViewRow rowProductType in gvAddRowsProductType.Rows)

                {
                    string numProductTypes = rowProductType.Cells[1].Text;
                    string numSelectCode = rowProductType.Cells[3].Text;
                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM05 (M05REF, M05SEQ, M05BRN, M05VEN, M05PRT, M05PFG, M05UDT, M05UTM, M05UUS, M05UPG, M05UWS, M05DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + numProductTypes
                    + "," + numSelectCode
                    + "," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM05 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM05 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm05 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            if (gvAddRowsProductCode.Rows.Count > 0)
            {
                foreach (GridViewRow rowProductCode in gvAddRowsProductCode.Rows)

                {
                    string numberProductTypes = rowProductCode.Cells[0].Text;
                    string numberProductCode = rowProductCode.Cells[1].Text;
                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM06(M06REF, M06SEQ, M06BRN, M06VEN, M06PRT, M06PCD, M06UDT, M06UTM, M06UUS, M06UPG, M06UWS, M06DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + numberProductTypes
                    + "," + numberProductCode
                    + "," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM06 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM06 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm06 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            SET_MSG_SUCCESS();

        }

        #endregion

        #region Step2 : EDIT DATA ILCM03-ILCM07
        protected void EDIT_DATA_ILCM03_ILCM07()
        {
            iDB2Command cmd = new iDB2Command();
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string[] startSplitDate = startDate.Text.Split('/');
            string StartDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
            string[] EndSplitDate = endDate.Text.Split('/');
            string EndDateNew = EndSplitDate[2] + EndSplitDate[1] + EndSplitDate[0];

            DataSet dsDateNew = new DataSet();
            SqlAll = "SELECT MAX(M02EDT) FROM AS400DB01.ILOD0001.ILCM02 WITH (NOLOCK) WHERE CAST(M02VEN as nvarchar) = '" + vendorID.Text + "'";
            dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];

            DateTime dtime;
            DateTime.TryParseExact(drDateNew[0].ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);

            string formatDateCheck = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dCheck = Convert.ToDateTime(formatDateCheck);
            DateTime newDate = dCheck.AddDays(Convert.ToInt32(1));
            string[] splitDateCheck = newDate.ToShortDateString().Split('/');
            int dateTranfer = int.Parse(splitDateCheck[2] + splitDateCheck[1] + splitDateCheck[0]);
            SqlAll = "UPDATE AS400DB01.ILOD0001.ILCM03 SET M03BRN = " + cookiesStorage.GetCookiesStringByKey("branch")
                                               + ", M03BCS = " + budgetContract.Text
                                               + ", M03BCA = " + double.Parse(txtAmount.Text)
                                               + ", M03BCT = " + budgetCriteriaType.Text
                                               + ", M03BCO = " + budgetTypeComparative.Text
                                               + ", M03BAT = " + budgetAppType.Text
                                               + ", M03BPT = " + budgetProductType.Text
                                               + ", M03BAM = " + double.Parse(budgetAmount.Text)
                                               + ", M03BPC = " + budgetPercentage.Text
                                               + ", M03UDT = " + m_UpdDate
                                               + ", M03UTM = " + m_UpdTime
                                               + ", M03UUS = '" + m_userInfo.Username.ToString() + "'"
                                               + ", M03UPG = 'ILE002014'"
                                               + ", M03UWS = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                               + " WHERE M03REF = " + referanceNo.Text + " AND M03VEN =" + vendorID.Text + " AND M03SEQ =" + seqNo.Text + "";

            cmd.CommandText = SqlAll;
            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int editILCM03S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (editILCM03S2 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "Can't save data , Please check command query : update ilcm03(Step2) ! ";
                    MsgHText = "Error Query";
                    SET_MSG();

                    return;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = "The data cannot be update : ilcm03(Step2)";
                MsgHText = "Error";
                SET_MSG();
                return;
            }
            if (gvAddRowsApplication.Rows.Count > 0)
            {
                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm04 WHERE m04ref = " + referanceNo.Text + " AND m04seq = " + seqNo.Text + " AND m04ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM04S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM04S2 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm04(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }


                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm04(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
            }
            if (gvAddRowsProductType.Rows.Count > 0)
            {
                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm05 WHERE m05ref = " + referanceNo.Text + " AND m05seq = " + seqNo.Text + " AND m05ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM05S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM05S2 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm05(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }


                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm05(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
            }
            if (gvAddRowsProductCode.Rows.Count > 0)
            {
                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm06 WHERE m06ref = " + referanceNo.Text + " AND m06seq = " + seqNo.Text + " AND m06ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM06S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM06S2 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm06(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }


                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm06(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
            }

            SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm07 WHERE m07ref = " + referanceNo.Text + " AND m07seq = " + seqNo.Text + " AND m07ven = " + vendorID.Text + "";
            cmd.CommandText = SqlAll;

            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int deleteILCM07S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (deleteILCM07S2 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "Can't delete data , Please check command query delete : ilcm07(Step2) ! ";
                    MsgHText = "Error Query";
                    SET_MSG();

                    return;
                }


            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = "The data cannot be deleted : ilcm07(Step2)";
                MsgHText = "Error";
                SET_MSG();
                return;
            }

            List<double> months = new List<double> { };
            months.Add(double.Parse(month1.Text));
            months.Add(double.Parse(month2.Text));
            months.Add(double.Parse(month3.Text));
            months.Add(double.Parse(month4.Text));
            months.Add(double.Parse(month5.Text));
            months.Add(double.Parse(month6.Text));
            months.Add(double.Parse(month7.Text));
            months.Add(double.Parse(month8.Text));
            months.Add(double.Parse(month9.Text));
            months.Add(double.Parse(month10.Text));
            months.Add(double.Parse(month11.Text));
            months.Add(double.Parse(month12.Text));

            double[] monthsList = months.ToArray();

            for (int j = 1; j < 13; j++)
            {
                double monthsAmount = monthsList[j - 1];

                SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM07(M07REF, M07SEQ, M07BRN, M07VEN, M07MON, M07BAM, M07UDT, M07UTM, M07UUS, M07UPG, M07UWS, M07DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + j
                        + "," + monthsAmount
                        + "," + m_UpdDate
                        + "," + m_UpdTime
                        + ",'" + m_userInfo.Username.ToString() + "'"
                        + ",'ILE002014'"
                        + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                        + ",' ')";
                cmd.CommandText = SqlAll;
                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int insertILCM07S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (insertILCM07S2 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't save data , Please check command query : insert ilcm07(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                }
                catch (Exception)
                {

                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be update : ilcm07(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
            }
            if (gvAddRowsApplication.Rows.Count > 0)
            {
                foreach (GridViewRow rowApplication in gvAddRowsApplication.Rows)

                {
                    string numAppTypes = rowApplication.Cells[1].Text;

                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM04(M04REF, M04SEQ, M04BRN, M04VEN, M04APT, M04UDT, M04UTM, M04UUS, M04UPG, M04UWS, M04DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + ",'" + numAppTypes
                    + "'," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM04 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM04 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm04 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            if (gvAddRowsProductType.Rows.Count > 0)
            {
                foreach (GridViewRow rowProductType in gvAddRowsProductType.Rows)

                {
                    string numProductTypes = rowProductType.Cells[1].Text;
                    string numSelectCode = rowProductType.Cells[3].Text;
                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM05 (M05REF, M05SEQ, M05BRN, M05VEN, M05PRT, M05PFG, M05UDT, M05UTM, M05UUS, M05UPG, M05UWS, M05DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + numProductTypes
                    + "," + numSelectCode
                    + "," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM05 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM05 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm05 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            if (gvAddRowsProductCode.Rows.Count > 0)
            {
                foreach (GridViewRow rowProductCode in gvAddRowsProductCode.Rows)

                {
                    string numberProductTypes = rowProductCode.Cells[0].Text;
                    string numberProductCode = rowProductCode.Cells[1].Text;
                    SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILCM06(M06REF, M06SEQ, M06BRN, M06VEN, M06PRT, M06PCD, M06UDT, M06UTM, M06UUS, M06UPG, M06UWS, M06DEL)"
                    + "VALUES('" + referanceNo.Text + "'"
                    + "," + seqNo.Text
                    + "," + cookiesStorage.GetCookiesStringByKey("branch")
                    + "," + vendorID.Text
                    + "," + numberProductTypes
                    + "," + numberProductCode
                    + "," + m_UpdDate
                    + "," + m_UpdTime
                    + ",'" + m_userInfo.Username.ToString() + "'"
                    + ",'ILE002014'"
                    + ",'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                    + ",' ')";
                    cmd.CommandText = SqlAll;
                    try
                    {
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int insertILCM06 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (insertILCM06 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "Can't save data , Please check command query : insert ilcm06 ! ";
                            MsgHText = "Error Query";
                            SET_MSG();

                            return;
                        }

                    }
                    catch (Exception)
                    {

                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "The data cannot be saved";
                        MsgHText = "Error";
                        SET_MSG();
                        return;
                    }
                }
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            SET_MSG_SUCCESS();


        }

        #endregion

        #region step1 : function delete

        protected void CANCEL_MAIN_DELETE(object sender, EventArgs e)
        {
            return;
        }
        protected void CLICK_MAIN_DELETE(object sender, EventArgs e)
        {
            PopupConfirmDelete.ShowOnPageLoad = true;
        }
        protected void CONFIRM_MAIN_DELETE(object sender, EventArgs e)
        {
            iDB2Command cmd = new iDB2Command();
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            DISIBLE_ALL();
            btnMainInsert.Enabled = true;
            dataCenter = new DataCenter(m_userInfo);
            string[] startSplitDate = startDate.Text.Split('/');
            string StartDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
            string[] EndSplitDate = endDate.Text.Split('/');
            string EndDateNew = EndSplitDate[2] + EndSplitDate[1] + EndSplitDate[0];

            if (checkStepStatus.Text == "activeStep1")
            {

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm01 WHERE m01ref = " + referanceNo.Text + " AND m01ven = " + vendorID.Text + " AND m01sdt = " + StartDateNew + " AND m01edt = " + EndDateNew + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM01S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM01S1 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm01(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm01(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm02 WHERE m02ref = " + referanceNo.Text + " AND m02ven = " + vendorID.Text + " AND m02sdt = " + StartDateNew + " AND m02edt = " + EndDateNew + "";
                cmd.CommandText = SqlAll;


                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM02S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM02S1 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm02(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();

                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm02(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM03 WHERE M03REF = " + referanceNo.Text + " AND M03VEN =  " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM03S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM03S1 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm03(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm03(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM04 WHERE M04REF = " + referanceNo.Text + "";
                cmd.CommandText = SqlAll;


                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM04S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM04S1 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm04(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                    dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();

                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm04(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM05 WHERE M05REF = " + referanceNo.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM05S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM05S1 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm05(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                    dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm05(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM06 WHERE M06REF = " + referanceNo.Text + "";
                cmd.CommandText = SqlAll;


                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM06S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM06S1 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm06(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();

                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm06(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ILCM07 WHERE M07REF = " + referanceNo.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM07S1 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM07S1 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm07(Step1) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm07(Step1)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
                DISIBLE_ALL();
                ListDateVendor();
                CLEAR_GRIDVIEW1();
                btnAddRank.Enabled = true;
                referanceNo.Text = "";
                checkModeStatus.Text = "";
                DEFAULT_STEP1();
                btnEdit.Enabled = true;
                ContentControlStep1.Enabled = true;
                btnMainInsert.Enabled = true;
                step1_box1.Enabled = true;
                step1_box2.Enabled = false;
                step1_box3.Enabled = false;
                txtStartAmount.Text = "0.00";
                txtEndAmount.Text = "0.00";
                txtRate.Text = "0.00";
                PopupConfirmDelete.ShowOnPageLoad = false;
                SetDate();
            }
            else if (checkStepStatus.Text == "activeStep2")
            {

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm03 WHERE m03ref = " + referanceNo.Text + " AND m03seq = " + seqNo.Text + " AND m03ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM03S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM03S2 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm03(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm03(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm04 WHERE m04ref = " + referanceNo.Text + " AND m04seq = " + seqNo.Text + " AND m04ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM04S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM04S2 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm04(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm04(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm05 WHERE m05ref = " + referanceNo.Text + " AND m05seq = " + seqNo.Text + " AND m05ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM05S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM05S2 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm05(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm05(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm06 WHERE m06ref = " + referanceNo.Text + " AND m06seq = " + seqNo.Text + " AND m06ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM06S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM06S2 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm06(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm06(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }

                SqlAll = "DELETE FROM AS400DB01.ILOD0001.ilcm07 WHERE m07ref = " + referanceNo.Text + " AND m07seq = " + seqNo.Text + " AND m07ven = " + vendorID.Text + "";
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int deleteILCM07S2 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (deleteILCM07S2 == -1)
                    {
                         dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't delete data , Please check command query delete : ilcm07(Step2) ! ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                     dataCenter.CommitMssql();
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
                catch (Exception)
                {
                     dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "The data cannot be deleted : ilcm07(Step2)";
                    MsgHText = "Error";
                    SET_MSG();
                    return;
                }
                ListSeqNo();
                DEFAULT_STEP1();
                return;
            }

        }
        #endregion
        #region Step2 : set unable 3 item
        private void SET_ENABLE_ALL_3ITEM()
        {
            gvAddRowsApplication.Enabled = true;
            step2_applicationType.Enabled = true;
            readApplicationType.Enabled = true;
            applicationTypes.Enabled = true;

            gvAddRowsProductType.Enabled = true;
            step2_productType.Enabled = true;
            readProductType.Enabled = true;
            productTypesSearch.Enabled = true;
            ddlProductType.Enabled = true;

            //gvAddRowsProductCode.Enabled = true;
            //step2_productCode.Enabled = true;
            //readProductCode.Enabled = true;
            //productCodes.Enabled = true;
            //productTypeSelect.Enabled = true;
        }
        #endregion

        #region Step2 : set disable 3 item
        private void SET_DISABLE_ALL_3ITEM()
        {
            gvAddRowsApplication.Enabled = false;
            step2_applicationType.Enabled = false;
            readApplicationType.Enabled = false;
            applicationTypes.Enabled = false;

            gvAddRowsProductType.Enabled = false;
            step2_productType.Enabled = false;
            readProductType.Enabled = false;
            productTypesSearch.Enabled = false;
            ddlProductType.Enabled = false;
            addProductType.Enabled = false;

            gvAddRowsProductCode.Enabled = false;
            step2_productCode.Enabled = false;
            readProductCode.Enabled = false;
            productCodes.Enabled = false;
            productTypeSelect.Enabled = false;
        }
        #endregion
    }
}