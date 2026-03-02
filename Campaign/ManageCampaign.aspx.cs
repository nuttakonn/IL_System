using EB_Service.Commons;
using EB_Service.DAL;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.Model.AS400DB01;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ILSystem.App_Code.Model.AS400DB01.AS400DB01Model;

namespace ILSystem.ManageData.WorkProcess.Campaign
{
    public partial class ManageCampaign : System.Web.UI.Page
    {
        private DateTimeFormatInfo _dateThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong _updateDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        private ulong _updateTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        ILDataCenter ilObj = new ILDataCenter();
        iDB2Command cmd = new iDB2Command();
        DataTable dt_AddItemExceptProduct = new DataTable();
        protected DataCenter dataCenter;
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        public ManageCampaign()
        {
        }
        public CookiesStorage campaignStorage;
        private string _mode = string.Empty;
        private string _campaignCode = string.Empty;
        protected string SqlAll = "";
        protected string loanType = "01";

        protected string nameTypeSub;
        protected Int64 codeTypeShareSub;
        protected double subRate;
        protected int dateCheck;
        protected string[] ForAppILCP06;
        protected string[] ShareSubArrayILCP05;
        protected string[] BranchArrayILCP09;
        protected double pricesMin, pricesMax;

        private int _termSeq, CmpSubtyp, CampaingType, productItemCode, countCheck;
        private string CmpVom, tmpVMcode, tmpCampaignCode, tmprunning, lblStatusCampaign;
        public string newIdCampaign;
        //private int _MaximumSeq = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumSeq"].ToString());
        //private int _MaximumTerm = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumSeq"].ToString());
        protected decimal _MaximumBaseRate = Convert.ToDecimal(ConfigurationManager.AppSettings["MaximumBaseRate"].ToString());
        protected decimal _DefaultIntRate = Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultIntRate"].ToString());

        //DB initial config value.
        protected decimal _MinNonXDue;                //NON X DUE
        protected decimal _MaxNonXDue;                //NON X DUE
        protected decimal _MinFreeInstallmentTerm;    //FREE INSTALLMENT TERM
        protected decimal _MaxFreeInstallmentTerm;    //FREE INSTALLMENT TERM
        protected decimal _MinRangeOfInterest;        //RANGE OF INTEREST
        protected decimal _MaxRangeOfInterest;        //RANGE OF INTEREST
        protected decimal _MinTotalTermOfContract;    //TOTAL TERM OF CONTRACT
        protected decimal _MaxTotalTermOfContract;    //TOTAL TERM OF CONTRACTx
        protected decimal _MinInterestRate;       //INTEREST RATE
        protected decimal _MaxInterestRate;       //INTEREST RATE
        protected decimal _MinInstallment;        //INSTALLMENT
        protected decimal _MaxInstallment;        //INSTALLMENT

        protected int startDateNews_2 = 0;
        protected int endDateNews_2 = 0;
        protected int closingDateNews_2 = 0;
        protected string sqlBetween_2 = "";
        protected string sqlCondition_2 = "";
        protected string sqlChooseCampaign_2 = "";
        protected string sqlRank_2 = "";
        protected string Msg_2 = "";
        protected string MsgHText_2 = "";

        protected string Msg = "";
        protected string MsgHText = "";
        protected string sqlBetween = "";
        protected string sqlCondition = "";
        protected string sqlChooseCampaign = "";
        protected string sqlRank = "";
        protected string rankVendor = "";


        protected int startDateNews = 0;
        protected int endDateNews = 0;
        protected int closingDateNews = 0;

        public static class Globals
        {
            public static String newIdCampaign;
            public static String runNumbeILMS99;
            public static String runNumbeP99REC;
            public static String checkTypeSearchItem;
            public static String copyCampaign;
            public static String copyCampaignOld;
            public static String statusCampaigns;
            public static String confirmStatusEdit = null;
            public static String showStatusHeader = null;
            //public static String checkFlag = null;
            public static String productItemID = null;
            public static String productItemSubSeq = null;
        }

        #region DataTable Object
        private DataTable dtSelectProductItem()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TYPE");
            dt.Columns.Add("CODE");
            dt.Columns.Add("NAME");
            return dt;
        }
        private DataTable dtShareSub()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RATES");
            dt.Columns.Add("C05STO");
            dt.Columns.Add("C05CSQ");
            dt.Columns.Add("C05RSQ");
            dt.Columns.Add("C05STM");
            dt.Columns.Add("C05SBT");
            dt.Columns.Add("Easybuy");
            dt.Columns.Add("ESubRate");
            dt.Columns.Add("Maker");
            dt.Columns.Add("MSubRate");
            dt.Columns.Add("Vendor");
            dt.Columns.Add("VSubRate");
            dt.Columns.Add("CampaignStartTerm");
            dt.Columns.Add("CampaignEndTerm");
            dt.Columns.Add("TotalTerm");

            return dt;
        }

        private DataTable dtTermOfCampaign()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("C02TTM");   //TotalTerm
            dt.Columns.Add("C02CSQ");   //SubSeq
            dt.Columns.Add("C02RSQ");   //Seq
            dt.Columns.Add("C02TTR");   //MRange
            dt.Columns.Add("C02FMT");   //Begin
            dt.Columns.Add("C02TOT");   //End
            dt.Columns.Add("C02INR");   //INTRate
            dt.Columns.Add("C02CRR");   //CRRate
            dt.Columns.Add("C02IFR");   //INFRate
            dt.Columns.Add("RATES");    //Rate
            dt.Columns.Add("C02AIR");   //AvgINTRate
            dt.Columns.Add("C02ACR");   //AvgCRRate

            return dt;
        }

        private DataTable dtTermOfCampaignDetail()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SubSeq");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Type");
            dt.Columns.Add("Code");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Price");

            return dt;
        }

        private DataTable dtVendorList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("C08VEN");
            dt.Columns.Add("P10NAM");
            dt.Columns.Add("P16RNK");
            dt.Columns.Add("P12ODR");
            dt.Columns.Add("P12WOR");
            dt.Columns.Add("P16END");

            return dt;
        }
        private DataTable dtILCP05()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("C05CMP");
            dt.Columns.Add("C05CSQ");
            dt.Columns.Add("C05RSQ");
            dt.Columns.Add("C05PAR");
            dt.Columns.Add("C05PCD");
            dt.Columns.Add("C05SBT");
            dt.Columns.Add("C05SIR");
            dt.Columns.Add("C05SCR");
            dt.Columns.Add("C05SFR");
            dt.Columns.Add("C05STR");
            dt.Columns.Add("C05SFM");
            dt.Columns.Add("C05STO");
            dt.Columns.Add("C05SST");
            dt.Columns.Add("C05EST");
            dt.Columns.Add("C05STM");
            dt.Columns.Add("C05UDT");
            dt.Columns.Add("C05UTM");
            dt.Columns.Add("C05UUS");
            dt.Columns.Add("C05UPG");
            dt.Columns.Add("C05UWS");
            dt.Columns.Add("C05RST");

            return dt;
        }
        private DataTable loop_ILCP05()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RATES");
            dt.Columns.Add("C05STO");
            dt.Columns.Add("C05CSQ");
            dt.Columns.Add("C05RSQ");
            dt.Columns.Add("C05STM");
            dt.Columns.Add("C05SBT");
            dt.Columns.Add("EASYBUY");
            dt.Columns.Add("ESUBRATE");
            dt.Columns.Add("MAKER");
            dt.Columns.Add("MSUBRATE");
            dt.Columns.Add("VENDOR");
            dt.Columns.Add("VSUBRATE");
            dt.Columns.Add("CAMPAIGNSTARTTERM");
            dt.Columns.Add("CAMPAIGNENDTERM");
            dt.Columns.Add("C02FMT");
            dt.Columns.Add("C02TOT");
            dt.Columns.Add("C05SSTS");
            dt.Columns.Add("C05ESTS");
            dt.Columns.Add("C05STMS");

            return dt;
        }
        private DataTable dtILCP07()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("C07CMP");
            dt.Columns.Add("C07CSQ");
            dt.Columns.Add("C07LNT");
            dt.Columns.Add("C07PIT");
            dt.Columns.Add("C07FIX");
            dt.Columns.Add("C07PRC");
            dt.Columns.Add("C07MIN");
            dt.Columns.Add("C07MAX");
            dt.Columns.Add("C07DOW");
            dt.Columns.Add("C07UDT");
            dt.Columns.Add("C07UTM");
            dt.Columns.Add("C07UUS");
            dt.Columns.Add("C07UPG");
            dt.Columns.Add("C07UWS");

            return dt;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            userInfoService = new UserInfoService();
            m_userInfo = userInfoService.GetUserInfo();
            campaignStorage = new CookiesStorage();
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

                LoadSessionDefaultParameters();
                SetLoadCampaign();
                if (campaignStorage.GetCookiesBoolByKey("Flag_Load_Campaign"))
                {
                    if ((bool)campaignStorage.GetCookiesBoolByKey("Flag_Load_Campaign"))
                    {
                        LOAD_COPY_CAMPAIGN_S();
                        campaignStorage.SetCookiesBoolByName("Flag_Load_Campaign", false);
                    }
                }
                BinddataSearchgvVendorList();
                DataSet ds_CheckHidder = new DataSet();
                ds_CheckHidder = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistproduct.Value);
                if (campaignStorage.check_dataset(ds_CheckHidder))
                {
                    BindDataToddlSelectItemType4();
                }
                return;
            }
            if (!IsPostBack)
            {
                ClearSession();
                showStatusCampaignDefult();
                ModeManagement();
                SetLoadCampaign();
                imgNewCampaignOn.Visible = false;
                SetDefault();

                TabContentControlSetVisible(false);

                SHOW_RATE_HEADER();

                PopupAlertCenter();
                LoadDBInitialConfigValue();
                LoadIntialConfigValuetoControl();
                BindDataFromMode();
                DDL_BIND_LIST_VENDOR();


                // Vendor
                DEFAULT_CHB_2();
                DISIBLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                DISIBLE_BOX_Note_2();
                btnMainSearch_2.Enabled = true;
                btnMainDetail_2.Enabled = false;
                CLEAR_LIST_CAMPAIGN_2();
                txtTitleCampaign_2.Text = "";
                campaignStorage.ClearCookies("dt_Popup_Vendorlistselected_2");
                dt_Popup_Vendorlistselected_2.Value = string.Empty;
                LoaddataFromgvselectlistvendor_2();
                PopupAlertCenter_2();

                // Product
                DEFAULT_CHB();
                DISIBLE_BOX_CAMPAIGN();
                DISIBLE_BOX_VENDOR();
                DISIBLE_BOX_Note();
                btnMainSearch.Enabled = true;
                btnMainDetail.Enabled = false;
                CLEAR_LIST_CAMPAIGN();
                PopupAlertCenter_1();
                campaignStorage.ClearCookies("dt_Popup_Productlistselected");
                dt_Popup_Productlistselected.Value = string.Empty;
                LoaddataFromgvselectlistProduct();
            }

        }

        protected void BindDataFromMode()
        {
            if (campaignStorage.check_dataset(campaignStorage.GetCookiesDataSetByKey("DS_DATA_CAMPAIGN")))
            {
                if (HdnIsEdit1.Value.ToUpper() == "UPDATE_CAMPAIGN"
                    || HdnIsEdit1.Value.ToUpper() == "EDIT_CAMPAIGN"
                    || HdnIsEdit1.Value.ToUpper() == "DELETE_CAMPAIGN"
                    || HdnIsEdit1.Value.ToUpper() == "VIEW_CAMPAIGN")
                {
                    LOAD_COPY_CAMPAIGN();

                    switch (HdnIsEdit1.Value.ToUpper())
                    {
                        case "CREATE_CAMPAIGN":
                            CreateMode();
                            break;
                        case "EDIT_CAMPAIGN":
                            EditMode();
                            txtCampaignCode.Text = Globals.copyCampaign;
                            break;
                        case "VIEW_CAMPAIGN":
                            ViewMode();
                            txtCampaignCode.Text = Globals.copyCampaign;
                            break;
                        case "UPDATE_CAMPAIGN":
                            UpdateMode();
                            txtCampaignCode.Text = Globals.copyCampaign;
                            break;
                        case "DELETE_CAMPAIGN":
                            DeleteMode();
                            txtCampaignCode.Text = Globals.copyCampaign;
                            break;
                        default:
                            ErrorMode();
                            break;
                    }
                }
                else if (HdnIsEdit1.Value.ToUpper() == "CREATE_CAMPAIGN")
                {
                    CreateMode();
                }
            }
            else
            {
                HdnIsEdit1.Value = "CREATE_CAMPAIGN";
                HdnIsEdit1.Value = "CREATE_CAMPAIGN";
                CreateMode();
            }
        }
        protected void SetLoadCampaign()
        {
            if (campaignStorage.GetCookiesDataTableByKey("DS_DATA_CAMPAIGN")?.Rows.Count > 0 && rdoCreateCampaign.Text != "CC")
            {
                DataTable dtLoad = campaignStorage.GetCookiesDataTableByKey("DS_DATA_CAMPAIGN")?.Copy();
                DataRow drLoad = dtLoad.Rows[0];

                Globals.copyCampaign = drLoad["ID_CAMPAIGN"].ToString();
                HdnIsEdit1.Value = drLoad["STATUS_CAMPAIGN"].ToString();
                lblCampaignMode.Text = "Create Mode";

            }
            else if (rdoCreateCampaign.Text == "CC")
            {
                Globals.copyCampaign = "";
                HdnIsEdit1.Value = "CREATE_CAMPAIGN";
                lblCampaignMode.Text = "Create Mode";
            }

            if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
            {
                lblCampaignMode.Text = "Edit Mode";
            }
            else if (HdnIsEdit1.Value == "VIEW_CAMPAIGN")
            {
                lblCampaignMode.Text = "View Mode";
            }
            else if (HdnIsEdit1.Value == "UPDATE_CAMPAIGN")
            {
                lblCampaignMode.Text = "Update Mode";
            }
            else if (HdnIsEdit1.Value == "DELETE_CAMPAIGN")
            {
                lblCampaignMode.Text = "Delete Mode";
            }
            else
            {
                lblCampaignMode.Text = "Create Mode";
            }

        }
        protected void SetDefault()
        {
            LoadBrandData();
            LoadApplicationType();

            ViewTermOfCampaignBindData();

            SearchCampaign();

            //Tab Fix Installment
            TabFixInstallmentBindData();

            //Tab Share Sub
            LoadApplicationTypeData();
            TabShareSubBindData();

            //Tab Vendor List
            TabVendorListBindData();

            //Tab Branch | Note
            TabBanchAndNoteBindData();

            //ModeManagement();

            string todayDate = DateTime.Now.ToString("dd/MM/yyyy", _dateThai);
            txtStartDate.Text = todayDate;
            txtEndDate.Text = todayDate;
            txtClosingApplicationDate.Text = todayDate;
            txtClosingLayBillDate.Text = todayDate;

            tabDetail.TabPages.FindByName("FixInstallment").Enabled = false;
            //lblSubSeqList.Text = "0";
            //lblSubSeqDetail.Text = "";
            btnStatus.Text = "";
        }

        protected void ModeManagement()
        {
            if (campaignStorage.GetCookiesDataTableByKey("DS_DATA_CAMPAIGN")?.Rows.Count > 0)
            {
                SetLoadCampaign();
                _mode = HdnIsEdit1.Value;
            }
            else
            {
                try
                {
                    _mode = Request.QueryString["mode"].ToString();
                }
                catch (Exception)
                {
                    _mode = "CREATE_CAMPAIGN";
                }

                if (_mode != "CREATE_CAMPAIGN")
                {
                    try
                    {
                        _campaignCode = Request.QueryString["campaigncode"].ToString();
                    }
                    catch (Exception)
                    {
                        ErrorMode();
                    }
                }
            }

            switch (_mode.ToUpper())
            {
                case "CREATE_CAMPAIGN":
                    CreateMode();
                    break;
                case "EDIT_CAMPAIGN":

                    txtCampaignName.Enabled = false;
                    txtProductDetail.Enabled = false;
                    txtSpecialPremium.Enabled = false;
                    txtStartDate.Enabled = false;
                    txtEndDate.Enabled = false;
                    txtXDue.Enabled = false;
                    txtClosingApplicationDate.Enabled = false;
                    txtClosingLayBillDate.Enabled = false;
                    EditMode();
                    break;
                case "VIEW_CAMPAIGN":
                    ViewMode();
                    break;
                case "UPDATE_CAMPAIGN":
                    UpdateMode();
                    break;
                case "DELETE_CAMPAIGN":
                    DeleteMode();
                    break;
                default:
                    ErrorMode();
                    break;
            }
        }

        protected void UpdateMode()
        {
            ASPxRoundPanel1.HeaderText = "Update Campaign";
            //CampaignStatus.Visible = true;
            lblCampaignMode.Text = "Update Mode";
            Mode.Visible = false;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = false;
            txtCampaignCode.Enabled = false;
            buttonTempCreate.Visible = false;
            SaveButton.Visible = false;
            //btnSetTermRange.Enabled = false;
            //btnClearTermRange.Enabled = false;
            pRangeY.Enabled = false;
            pRangeY.Visible = false;
            pAddMonth.Enabled = false;
            pAddRate.Enabled = false;
            //pBaseRate.Disabled = true;
            pCampaignDetailValue.Enabled = false;
            pCampaignDetail.Enabled = false;
            //CampaignMainDetail
            UpdateButton.Visible = true;

            SetTabControlReadOnly();
        }

        protected void DeleteMode()
        {
            ASPxRoundPanel1.HeaderText = "Delete Campaign";
            //CampaignStatus.Visible = true;
            lblCampaignMode.Text = "Delete Mode";
            Mode.Visible = false;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = false;
            txtCampaignCode.Enabled = false;
            buttonTempCreate.Visible = false;
            SaveButton.Visible = false;
            pRangeY.Enabled = false;
            pRangeY.Visible = false;
            pAddMonth.Enabled = false;
            pAddRate.Enabled = false;
            pCampaignDetailValue.Enabled = false;
            pCampaignDetail.Enabled = false;
            DeleteButton.Visible = true;

            SetTabControlReadOnly();
        }

        protected void ViewMode()
        {
            ASPxRoundPanel1.HeaderText = "View Campaign";
            //CampaignStatus.Visible = true;
            lblCampaignMode.Text = "View Mode";
            btnAddMakerCode.Visible = false;
            rSubSeq.Visible = false;
            pButtonCreate.Visible = false; // control btn save or insert data
            pRangeY.Visible = false;
            SaveButton.Visible = false; // control btn save or insert data
            pGridMultiRate.Visible = false; // control rate and sub rate
            Mode.Visible = false;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = false;
            txtCampaignCode.Enabled = false;
            SaveButton.Visible = false;
            btnSetTermRange.Enabled = false;
            btnClearTermRange.Enabled = false;
            btnCreateCampaignClear.Enabled = false;
            txtTermRange.Enabled = false;

            gvViewTermOfCampaignDetail.Enabled = false;
            controlSubdetail.Enabled = false;
            pTab.Enabled = true;
            gvViewTermOfCampaign.Enabled = true;



            //Tab Term | Product
            ContentControl3.Enabled = false;
            //Tab Share Sub.
            ContentControl1.Enabled = false;
            //Tab Share Sub.
            ContentControl4.Enabled = false;
            //Tab Branch | Note
            ContentControl5.Enabled = false;
        }

        protected void CreateMode()
        {
            lblCampaignMode.Text = "Create Mode";
            ASPxRoundPanel1.HeaderText = "Create Campaign";
            //CampaignStatus.Visible = false;
            //lblCampaignMode.Text = "";
            Mode.Visible = true;

            foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
            {
                item.Selected = true;

                if (item.Value == "03")
                {
                    item.Selected = false;
                }
            }
        }

        protected void EditMode()
        {
            //ASPxRoundPanel1.HeaderText = "Edit Campaign";
            //CampaignStatus.Visible = true;
            //lblCampaignMode.Text = "Edit Mode";
            //Mode.Visible = false;
            //SelectCampaignType.Visible = true;
            //ddlCampaignType.Enabled = false;
            //pCampaignDetail.Visible = true;
            //pCampaignDetail.Enabled = true;
            //pCampaignDetailValue.Visible = true;
            //pCampaignDetailValue.Visible = false;
            //txtCampaignCode.Enabled = false;
            //SaveButton.Visible = true;

            //LoadCampaignForEdit();

            ASPxRoundPanel1.HeaderText = "Edit Campaign";
            //CampaignStatus.Visible = true;
            //lblCampaignMode.Text = "Edit Mode";
            //Mode.Visible = false;
            //SelectCampaignType.Visible = true;
            //ddlCampaignType.Enabled = false;
            //pCampaignDetail.Visible = true;
            //pCampaignDetail.Enabled = true;
            //pCampaignDetailValue.Visible = true;
            //pCampaignDetailValue.Enabled = false;
            //txtCampaignCode.Enabled = false;
            //buttonTempCreate.Visible = false;
            //SaveButton.Visible = false;
            //pRangeY.Enabled = false;
            //pRangeY.Visible = false;
            //pAddMonth.Enabled = false;
            EditButton.Visible = true;
            //btnSaveNote.Enabled = true;
            //btnCancelNote.Enabled = true;
            //txtNote.ReadOnly = false;

            //pTab.Enabled = true;

            buttonTempCreate.Visible = false;
            btnAddMakerCode.Visible = false;
            rSubSeq.Visible = false;
            pButtonCreate.Visible = true; // control btn save or insert data
            pRangeY.Visible = false;
            SaveButton.Visible = false; // control btn save or insert data
            pGridMultiRate.Visible = false; // control rate and sub rate
            Mode.Visible = false;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = false;
            txtCampaignCode.Enabled = false;
            SaveButton.Visible = false;
            btnSetTermRange.Enabled = false;
            btnClearTermRange.Enabled = false;
            btnCreateCampaignClear.Enabled = false;
            txtTermRange.Enabled = false;

            gvViewTermOfCampaignDetail.Enabled = false;
            controlSubdetail.Enabled = false;
            pTab.Enabled = true;
            gvViewTermOfCampaign.Enabled = true;
            //Tab Term | Product
            ContentControl3.Enabled = false;
            //Tab Share Sub.
            ContentControl1.Enabled = false;
            //Tab Share Sub.
            ContentControl4.Enabled = false;
            //Tab Branch | Note
            ContentControl5.Enabled = false;

        }

        protected void SuccessMode()
        {
            //ASPxRoundPanel1.HeaderText = "View Campaign";
            //CampaignStatus.Visible = true;
            //lblCampaignMode.Text = "View Mode";
            btnAddMakerCode.Visible = false;
            rSubSeq.Visible = false;
            pButtonCreate.Visible = false; // control btn save or insert data
            pRangeY.Visible = false;
            SaveButton.Visible = false; // control btn save or insert data
            pGridMultiRate.Visible = false; // control rate and sub rate
            Mode.Visible = false;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = false;
            txtCampaignCode.Enabled = false;
            SaveButton.Visible = false;
            btnSetTermRange.Enabled = false;
            btnClearTermRange.Enabled = false;
            btnCreateCampaignClear.Enabled = false;
            txtTermRange.Enabled = false;

            gvViewTermOfCampaignDetail.Enabled = false;
            controlSubdetail.Enabled = false;
            pTab.Enabled = true;



            //Tab Term | Product
            ContentControl3.Enabled = false;
            //Tab Share Sub.
            ContentControl1.Enabled = false;
            //Tab Share Sub.
            ContentControl4.Enabled = false;
            //Tab Branch | Note
            ContentControl5.Enabled = false;
        }

        protected void BTN_CONFIRM_EDIT_CAMPAIGN(object sender, EventArgs e)
        {
            //Globals.confirmStatusEdit = "EDIT_CONFIRM";
            CookiesStorage cookiesStorage = new  CookiesStorage();
            cookiesStorage.SetCookiesStringByName("IsEditMode2", "EDIT_CONFIRM");
            HdnIsEdit2.Value = "EDIT_CONFIRM";

            if (HdnIsEdit1.Value == "EDIT_CAMPAIGN" && HdnIsEdit2.Value == "EDIT_CONFIRM")
            {

                btnClear.Visible = false;
                EditButton.Visible = false;
                SaveButton.Visible = true;
                ddlCampaignType.Enabled = false;
                //if (!String.IsNullOrEmpty(txtMakerCode.Text))
                //{
                //    txtMakerCode.Enabled = true;
                //}
                //else
                //{
                //    txtMakerCode.Enabled = false;
                //}

                //if (!String.IsNullOrEmpty(txtVendorCode.Text))
                //{
                //    txtVendorCode.Enabled = true;
                //}
                //else
                //{
                //    txtVendorCode.Enabled = false;
                //}
                txtCampaignName.Enabled = true;
                txtProductDetail.Enabled = true;
                txtSpecialPremium.Enabled = true;
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
                txtXDue.Enabled = true;
                txtClosingApplicationDate.Enabled = true;
                txtClosingLayBillDate.Enabled = true;

                pCampaignDetail.Enabled = true;
                pCampaignDetailValue.Enabled = true;
                pRangeY.Enabled = true;
                pAddMonth.Enabled = true;
                pAddRate.Enabled = true;

                ContentControl2.Enabled = true;
                ContentControl3.Enabled = true;
                ContentControl4.Enabled = true;
                ContentControl5.Enabled = true;
                //pTab.Enabled = true;
                gvBranch.Enabled = true;

                //ViewCreateCampaign(false);
                SetTermProductDetailActive();
                txtMarketingCode.Enabled = true;
                btnSelectMarketing.Enabled = true;
                txtNote.Enabled = true;
                //txtNote.ReadOnly = false;
                btnSaveNote.Enabled = true;
                btnCancelNote.Enabled = true;
                DataSet ds = new DataSet();
                //ds = campaignStorage.GetCookiesDataSetByKey("ds_vendor_list");
                ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
                if (campaignStorage.check_dataset(ds))
                {

                    //ListItem rdbSelectVendors = rdbSelectVendor.Items.FindByValue("SV");
                    //rdbSelectVendors.Selected = true;
                    rdbSelectVendor.Items[1].Selected = true;
                    btnVendorListSearch.Visible = true;
                    btnVendorListSearch.Enabled = true;
                }
                else
                {
                    //ListItem rdbSelectVendors = rdbSelectVendor.Items.FindByValue("AV");
                    //rdbSelectVendors.Selected = true;
                    rdbSelectVendor.Items[0].Selected = true;
                    btnVendorListSearch.Visible = false;
                }
                rdbSelectVendor.Enabled = true;
                //tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("ShareSub");
                btnEditCampaign.Text = "Edit";
            }
            else
            {

            }
        }
        protected void LoadDBInitialConfigValue()
        {
            string query = @"SELECT T11VCD, T11VDS, T11VMN, T11VMX FROM AS400DB01.ILOD0001.ILTB11 WITH (NOLOCK) ";

            DataSet ds = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            ds = dataCenter.GetDataset<DataTable>(query, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(ds))
            {
                ds_initial_value.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                LoadSessionDefaultParameters();
            }
        }
        protected void LoadIntialConfigValuetoControl()
        {
            //FREE INSTALLMENT TERM
            txtFInstall.Text = _MinFreeInstallmentTerm.ToString();
            txtFInstall.NullText = _MinFreeInstallmentTerm.ToString();
            txtFInstall.MaxValue = _MaxFreeInstallmentTerm;
            txtFInstall.MinValue = _MinFreeInstallmentTerm;

            //NON X DUE
            txtXDue.Text = _MinNonXDue.ToString();
            txtXDue.NullText = _MinNonXDue.ToString();
            txtXDue.MaxValue = _MaxNonXDue;
            txtXDue.MinValue = _MinNonXDue;
        }

        protected void LoadSessionDefaultParameters()
        {
            DataSet ds = new DataSet();
            ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_initial_value.Value);
            if (campaignStorage.check_dataset(ds))
            {
                DataTable dt = ds?.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["T11VCD"].ToString() == "1" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "NON X DUE")
                        {
                            _MinNonXDue = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxNonXDue = Convert.ToDecimal(dr["T11VMX"]);
                        }
                        else if (dr["T11VCD"].ToString() == "2" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "FREE INSTALLMENT TERM")
                        {
                            _MinFreeInstallmentTerm = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxFreeInstallmentTerm = Convert.ToDecimal(dr["T11VMX"]);
                        }
                        else if (dr["T11VCD"].ToString() == "3" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "RANGE OF INTEREST")
                        {
                            _MinRangeOfInterest = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxRangeOfInterest = Convert.ToDecimal(dr["T11VMX"]);
                        }
                        else if (dr["T11VCD"].ToString() == "4" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "TOTAL TERM OF CONTRACT")
                        {
                            _MinTotalTermOfContract = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxTotalTermOfContract = Convert.ToDecimal(dr["T11VMX"]);
                        }
                        else if (dr["T11VCD"].ToString() == "5" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "INTEREST RATE")
                        {
                            _MinInterestRate = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxInterestRate = Convert.ToDecimal(dr["T11VMX"]);
                        }
                        else if (dr["T11VCD"].ToString() == "6" && dr["T11VDS"].ToString().TrimEnd().ToUpper() == "INSTALLMENT")
                        {
                            _MinInstallment = Convert.ToDecimal(dr["T11VMN"]);
                            _MaxInstallment = Convert.ToDecimal(dr["T11VMX"]);
                        }
                    }
                }
            }
        }

        protected void LoadCampaignForEdit()
        {
            txtCampaignCode.Text = _campaignCode;
        }

        protected void ErrorMode()
        {
            //Invalid Parameter
        }

        protected void rbRateSelectedIndexChanged(object sender, EventArgs e)
        {
            pBaseRate.Visible = rdoCreateCampaingTypeVendorCalculateType.SelectedValue == "R" ? true : false;
        }

        protected void rbCreateCampaingTypeVendorCampaignSupportSelectedIndexChanged(object sender, EventArgs e)
        {
            pRangeYSetVisible();
            //if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            //{
            //    pRangeY.Visible = true;
            //    pRangeN.Visible = false;
            //}
            //else
            //{
            //    pRangeY.Visible = false;
            //    pRangeN.Visible = true;
            //}
        }

        //protected void rbRangeSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lbEndTermRange.Text = "End Term Range " + rbRange.SelectedValue.ToString();
        //    //if (rbRange.SelectedValue == "1")
        //    //{
        //    //    lbEndTermRange.Text = "End Term Range 1";
        //    //}
        //    //else if (rbRange.SelectedValue == "2")
        //    //{
        //    //    lbEndTermRange.Text = "End Term Range 2";
        //    //}
        //}

        protected void btnSetTermRangeClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text) && !String.IsNullOrWhiteSpace(txtCreateCampaingTypeVendorBaseRate.Text))
            {
                try
                {
                    pAddMonth.Visible = true;
                    pAddRate.Visible = true;
                    pGridMultiRate.Visible = true;
                    rbCreateCampaingTypeVendorCampaignSupport.Enabled = false;
                    //rbRange.Enabled = false;
                    txtTermRange.Enabled = false;
                    txtEndTermRange.Enabled = false;
                    txtEndTerm.Enabled = false;
                    txtCreateCampaingTypeVendorBaseRate.Enabled = false;
                    txt_addTerm.Enabled = false;

                    InsertTermEvent(false);

                    //if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                    //{
                    ValidateTermMultiRate(Convert.ToInt32(txtTermRange.Text), Convert.ToInt32(txtEndTermRange.Text));

                    if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                    {
                        if (Convert.ToInt32(txtTermRange.Text) > 1)
                        {
                            decimal Rte = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.AsEnumerable()
                                           where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == Convert.ToInt32(txtSubSeq.Text) && int.Parse(dsMultiRate.Field<string>("C05CSQ")) == 1
                                           select decimal.Parse(dsMultiRate.Field<string>("RATES"))).FirstOrDefault();

                            txtRateforRange.Text = Rte.ToString("N2");
                            txtRateforRange.ReadOnly = true;
                        }
                    }
                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return;
                }
            }
        }

        protected void btnClearTermRangeClick(object sender, EventArgs e)
        {
            ClearTermRange();

            txtTermRange.Text = _termSeq.ToString();
            campaignStorage.ClearCookies("ds_multi_rate");
            ds_multi_rate.Value = string.Empty;
            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
            {
                txtSubSeq.Text = _termSeq.ToString();
                pRangeY.Visible = false;
            }
            //pAddMonth.Visible = false;
            //rbCreateCampaingTypeVendorCampaignSupport.Enabled = true;
            ////rbRange.Enabled = true;
            //txtTermRange.Enabled = true;
            //txtEndTermRange.Enabled = true;
            //txtEndTerm.Enabled = true;
            //txtCreateCampaingTypeVendorBaseRate.Enabled = true;
        }
        protected void CLICK_CLOSE_POPUP(object sender, EventArgs e)
        {
            //rdoCreateCampaign.SelectedItem.Selected = false;

        }
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

        #region Select Campaign Type for Create
        protected void rdoCreateCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            string createCampaign = rdoCreateCampaign.SelectedValue;
            switch (createCampaign)
            {
                case "CC":
                    SelectCampaignType.Visible = true;
                    ddlCampaignType.Enabled = true;
                    break;
                case "LC":
                    ddlCampaignType.Enabled = false;
                    SelectCampaignType.Visible = true;
                    popupSearchCampaign.ShowOnPageLoad = true;
                    showStatusCampaignDefult();
                    break;
                default: break;
            }

            TabContentControlSetVisible(false);
        }

        protected void ddlCampaignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campaignType = ddlCampaignType.SelectedValue;
            switch (campaignType)
            {
                case "MAKER":
                    CreateCampaignMakerType();
                    ClearInputDataDetail();
                    break;
                case "VENDOR":
                    CreateCampaignVendorType();
                    ClearInputDataDetail();
                    break;
                case "ESB":
                    CreateCampaignESBType();
                    ClearInputDataDetail();
                    break;
                case "SHARESUB":
                    CreateCampaignShareSubType();
                    ClearInputDataDetail();
                    break;
                default:
                    ValidateSelectCampaignType();
                    ClearInputDataDetail();
                    break;
            }
        }

        protected void ValidateSelectCampaignType()
        {
            pCampaignDetail.Visible = false;
            pCampaignDetailValue.Visible = false;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Enabled = false;

            SelectCampaignType.Visible = true;
        }

        protected void CreateCampaignMakerType()
        {
            Maker.Visible = true;
            Vendor.Visible = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = true;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = true;
            btnCreateCampaignOK.Enabled = false;

            //foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
            //{
            //item.Selected = true;

            //if (item.Value == "03")
            //{
            //    item.Selected = false;
            //}
            //}
            //string copyCampaignString = Globals.copyCampaign;
            //string[] CampaignString; //= String.IsNullOrEmpty(copyCampaignString) ? null : copyCampaignString.ToString().Split('-');
            //if (!String.IsNullOrEmpty(copyCampaignString))
            //{
            //    CampaignString = copyCampaignString.ToString().Split('-');

            //    int countComapaignArray = CampaignString.Length;
            //    if (copyCampaignString != "" && countComapaignArray == 4)
            //    {
            //        LOAD_DATA_TYPESUB_ILCP05();
            //    }
            //}
            //else
            //{
            //Checkbox Maker
            ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = true;
            rSubMaker.Visible = true;

            //Checkbox Vendor
            ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = false;
            rSubVendor.Visible = false;

            //Checkbox ESB
            ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;
            rSubESB.Visible = true;
            //}
        }

        protected void CreateCampaignVendorType()
        {
            Maker.Visible = false;
            Vendor.Visible = true;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = true;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = true;
            btnCreateCampaignOK.Enabled = false;

            foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
            {
                item.Selected = true;

                //if (item.Value == "03")
                //{
                //    item.Selected = false;
                //}
            }
            //string copyCampaignString = Globals.copyCampaign;
            //string[] CampaignString; //= String.IsNullOrEmpty(copyCampaignString) ? null : copyCampaignString.ToString().Split('-');
            //if (!String.IsNullOrEmpty(copyCampaignString))
            //{
            //    CampaignString = copyCampaignString.ToString().Split('-');

            //    int countComapaignArray = CampaignString.Length;
            //    if (copyCampaignString != "" && countComapaignArray == 4)
            //    {
            //        LOAD_DATA_TYPESUB_ILCP05();
            //    }
            //}
            //else
            //{
            //Checkbox Maker        
            ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = false;
            rSubMaker.Visible = false;

            //Checkbox Vendor        
            ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = true;
            rSubVendor.Visible = true;

            //Checkbox ESB
            ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;
            rSubESB.Visible = true;
            //}
        }

        protected void CreateCampaignESBType()
        {
            Maker.Visible = false;
            Vendor.Visible = false;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = true;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = true;
            btnCreateCampaignOK.Enabled = false;

            foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
            {
                item.Selected = true;

                //if (item.Value == "03")
                //{
                //    item.Selected = false;
                //}
            }
            //string copyCampaignString = Globals.copyCampaign;
            //string[] CampaignString; //= String.IsNullOrEmpty(copyCampaignString) ? null : copyCampaignString.ToString().Split('-');
            //if (!String.IsNullOrEmpty(copyCampaignString))
            //{
            //    CampaignString = copyCampaignString.ToString().Split('-');

            //    int countComapaignArray = CampaignString.Length;
            //    if (copyCampaignString != "" && countComapaignArray == 4)
            //    {
            //        LOAD_DATA_TYPESUB_ILCP05();
            //    }
            //}
            //else
            //{
            //Checkbox Maker        
            ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = false;
            rSubMaker.Visible = false;

            //Checkbox Vendor        
            ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = false;
            rSubVendor.Visible = false;

            //Checkbox ESB
            ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;
            rSubESB.Visible = true;
            //}
        }

        protected void CreateCampaignShareSubType()
        {
            Maker.Visible = true;
            Vendor.Visible = true;
            pCampaignDetail.Visible = true;
            pCampaignDetail.Enabled = true;
            pCampaignDetailValue.Visible = true;
            pCampaignDetailValue.Enabled = true;
            btnCreateCampaignOK.Enabled = false;

            foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
            {
                item.Selected = true;

                //if (item.Value == "03")
                //{
                //    item.Selected = false;
                //}
            }
            //string copyCampaignString = Globals.copyCampaign;
            //string[] CampaignString; //= String.IsNullOrEmpty(copyCampaignString) ? null : copyCampaignString.ToString().Split('-');
            //if (!String.IsNullOrEmpty(copyCampaignString))
            //{
            //    CampaignString = copyCampaignString.ToString().Split('-');

            //    int countComapaignArray = CampaignString.Length;
            //    if (copyCampaignString != "" && countComapaignArray == 4)
            //    {
            //        LOAD_DATA_TYPESUB_ILCP05();
            //    }
            //}
            //else
            //{
            //Checkbox Maker        
            ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = true;
            rSubMaker.Visible = true;

            //Checkbox Vendor        
            ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = true;
            rSubVendor.Visible = true;

            //Checkbox ESB
            ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = true;
            ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;
            rSubESB.Visible = true;
            //}
        }
        #endregion

        #region Core Function
        protected DataSet GetApplicationType()
        {
            string sql = "SELECT GN61CD,GN61DT,GN61DE FROM [AS400DB01].[GNOD0000].GNTB61 WITH (NOLOCK) where GN61DL = '' ";

            DataSet ds = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            return ds;
        }

        private void LoadBrandData()
        {
            //try
            //{
            //    ilObj = new ILDataCenter();
            //    DataSet ds = ilObj.getILTB01();
            //    ckbPoupCreateCampaingTypeVendorBranch.Items.Clear();
            //    if (ilObj.check_dataset(ds))
            //    {
            //        ckbPoupCreateCampaingTypeVendorBranch.DataSource = ds?.Tables[0];
            //        ckbPoupCreateCampaingTypeVendorBranch.DataTextField = "T1BNME";
            //        ckbPoupCreateCampaingTypeVendorBranch.DataValueField = "T1BRN";
            //        ckbPoupCreateCampaingTypeVendorBranch.DataBind();
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }
        #endregion

        #region Create Campaign
        #region Create Campaign Type : Maker
        protected void btnAddMakerCodeClick(object sender, EventArgs e)
        {
            hdfAddMaker.Value = "FORM_CREATE_CAMPAIGN";
            LoadMakerData();
            PopupAddMakerCode.ShowOnPageLoad = true;
        }
        protected void CLICK_MAKER_SEARCH(object sender, EventArgs e)
        {
            LoadMakerData();
        }
        protected void CLICK_MAKER_CLEAR(object sender, EventArgs e)
        {
            txtPopupAddMakerCodeSearchText.Text = "";
            //ddlPoupAddMakerCodeSearchBy.Items[0].Selected = true;
            LoadMakerData();
        }
        protected void LoadMakerData()
        {
            string sqlWhere = string.Empty;
            //string sql = @"SELECT
            //                 DISTINCT P11VEN,
            //                 P10NAM,
            //                  T00LTY,
            //                  T00TNM
            //                FROM
            //                 ILMS10
            //                LEFT JOIN ILMS11
            //                ON
            //                 P10VEN = P11VEN
            //                LEFT JOIN ILTB00
            //                ON
            //                 P11LTY = T00LTY
            //                WHERE 
            //                 P11DEL = '' ";
            //string sql = @"SELECT DISTINCT T47MAK, T46TNM, T46ENM, T00LTY, T00TNM
            //               FROM ILTB46
            //               LEFT JOIN ILTB47 ON T46MAK = T47MAK
            //               LEFT JOIN ILTB00 ON T47LTY = T00LTY
            //               WHERE T47DEL = '' ";


            string sql = @"SELECT DISTINCT T46MAK, T46TIC, T46TNM, T46ENM FROM AS400DB01.ILOD0001.ILTB46 WITH (NOLOCK) WHERE T46DEL = '' ";

            string searchText = txtPopupAddMakerCodeSearchText.Text.Trim().Replace("'", "''");
            string searchBy = ddlPoupAddMakerCodeSearchBy.SelectedValue;

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "C":
                        sqlWhere = " AND CAST(T46MAK AS VARCHAR(100)) = '" + searchText + "'";
                        break;
                    case "D":
                        //sqlWhere = " AND T46ENM LIKE '" + searchText.ToUpper() + "%' ";
                        sqlWhere = " AND (T46TNM LIKE '%' + @T46TNM + '%' OR T46ENM LIKE '%' + @T46ENM + '%') ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("T46TNM", searchText);
                        cmd.Parameters.AddWithValue("T46ENM", searchText);
                        break;

                    default: break;
                }
            }

            sql = sql + sqlWhere;
            cmd.CommandText = sql;
            DataSet ds = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            ds = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

            campaignStorage.SetCookiesDataSetByName("ds_popup_maker", ds);
            if (campaignStorage.check_dataset(ds))
            {
                gvMakerCode.DataSource = ds;
                gvMakerCode.DataBind();
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Create Campaign Type : Vendor
        protected void LoadApplicationType()
        {
            DataSet ds = GetApplicationType();

            ckbPoupCreateCampaingTypeVendorApplicationType.DataTextField = "GN61DT";
            ckbPoupCreateCampaingTypeVendorApplicationType.DataValueField = "GN61CD";
            if (campaignStorage.check_dataset(ds))
            {
                ckbPoupCreateCampaingTypeVendorApplicationType.DataSource = ds;
                ckbPoupCreateCampaingTypeVendorApplicationType.DataBind();
            }
        }

        protected void btnAddVendorClick(object sender, EventArgs e)
        {
            hdfAddVendor.Value = "FORM_CREATE_CAMPAIGN";
            LoadVendorData();
            PopupAddVendor.ShowOnPageLoad = true;
        }


        protected void rdoCreateCampaingTypeVendorCalculateTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            pBaseRate.Visible = rdoCreateCampaingTypeVendorCalculateType.SelectedValue == "R" ? true : false;
        }

        protected void btnCreateCampaignOKClick(object sender, EventArgs e)
        {
            pRangeY.Visible = false;
            rSubSeq.Visible = false;
            pTab.Visible = true;


            CampaignTypeVendorBindDataForCreate();

            ViewCreateCampaign(false);
            TabContentControlSetVisible(true);
            TabShareSubSetControlMode(true);
            SetTermProductDetailEnable(false);

            pGridMultiRate.Visible = false;
            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
            {

                btnAddSubSeq.Enabled = false;
            }

            if (ckbCreateCampaignTypeVendorShareSub.SelectedValue == "Vendor")
            {
                TabShareSubVendorControlVisible(true);
            }

            buttonTempCreate.Visible = false;
            SaveButton.Visible = true;
            gvBranch.Enabled = true;
            rdbSelectVendor.Enabled = true;
            txtMarketingCode.ReadOnly = false;
            btnSelectMarketing.Enabled = true;
        }

        protected void CampaignTypeVendorBindDataForCreate()
        {
            BindApplicationTypeForCreate();
            BindBaseRate();
            BindShareSubForCreate();
            BindTypeSubForCreate();

            CampaignTypeVendorBindgvMultiRateTogvPartyRate();

            BindDatagvViewTermOfCampaign();
        }

        protected void BindApplicationTypeForCreate()
        {
            List<string> applicationTypes = new List<string>();

            for (int i = 0; i < ckbPoupCreateCampaingTypeVendorApplicationType.Items.Count; i++)
            {
                if (ckbPoupCreateCampaingTypeVendorApplicationType.Items[i].Selected)
                {
                    applicationTypes.Add(ckbPoupCreateCampaingTypeVendorApplicationType.Items[i].Value);
                }
            }

            foreach (GridViewRow gvRow in gvApplicationType.Rows)
            {
                CheckBox cbSelect = (CheckBox)gvRow.FindControl("cbSelect");
                Label lblApplicationTypeCode = (Label)gvRow.FindControl("lblApplicationTypeCode");
                string applicationType = lblApplicationTypeCode.Text;

                foreach (var applicationTypeSelected in applicationTypes)
                {
                    if (applicationType == applicationTypeSelected)
                    {
                        cbSelect.Checked = true;
                    }
                }
            }
        }

        protected void BindShareSubForCreate()
        {
            ckbShareSub.Items[0].Selected = ckbCreateCampaignTypeVendorShareSub.Items[0].Selected;
            ckbShareSub.Items[1].Selected = ckbCreateCampaignTypeVendorShareSub.Items[1].Selected;
            ckbShareSub.Items[2].Selected = ckbCreateCampaignTypeVendorShareSub.Items[2].Selected;
        }

        protected void BindTypeSubForCreate()
        {
            rdoTypeSub.SelectedValue = rdoCreateCampaingTypeVendorTypeSub.SelectedValue;
        }

        protected void BindBaseRate()
        {
            //Base Rate
            txtBaseRate.Text = txtCreateCampaingTypeVendorBaseRate.Text.Trim();
        }

        protected void CampaignTypeVendorBindMultiRate()
        {
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(ds_multi_rate.Value))
            {
                ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value);
            }
            else
            {
                ds = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_multi_rate");
            }
            gvMultiRate.DataSource = ds;
            gvMultiRate.DataBind();
        }

        protected void CampaignTypeVendorBindPartyRate()
        {
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(ds_party_rate.Value))
            {
                ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_party_rate.Value);
            }
            else
            {
                ds = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_party_rate");
            }

            gvPartyRate.DataSource = ds;
            gvPartyRate.DataBind();
        }

        protected void CampaignTypeVendorBindgvMultiRateTogvPartyRate()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ds_multi_rate.Value))
            {
                dt = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0]?.Copy();
            }
            else
            {
                dt = campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.Copy();
            }
            //ILCP02
            if (dt.Columns["C02FMT"] == null) { dt.Columns.Add("C02FMT"); }
            if (dt.Columns["C02TOT"] == null) { dt.Columns.Add("C02TOT"); }
            if (dt.Columns["C05SSTS"] == null) { dt.Columns.Add("C05SSTS"); }
            if (dt.Columns["C05ESTS"] == null) { dt.Columns.Add("C05ESTS"); }
            if (dt.Columns["C05STMS"] == null) { dt.Columns.Add("C05STMS"); }

            foreach (DataRow dr in dt.Rows)
            {
                dr["C02FMT"] = dr["CampaignStartTerm"];
                dr["C02TOT"] = dr["CampaignEndTerm"];
                dr["C05SSTS"] = dr["CampaignStartTerm"];
                dr["C05ESTS"] = dr["CampaignEndTerm"];
                dr["C05STMS"] = dr["C05STO"];
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            ds_party_rate.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
            campaignStorage.SetCookiesDataSetByName("ds_party_rate", ds);
            gvPartyRate.DataSource = ds;
            gvPartyRate.DataBind();

            gvPartyRateOptionalShareSubEvent();
        }

        protected void gvMultiRateOptionalShareSubEvent()
        {
            int gvIdx = -1;
            //Vendor
            if (ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Vendor");
                gvMultiRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvMultiRate, "V.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = true;

                CampaignTypeVendorBindMultiRate();
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Vendor");
                gvMultiRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvMultiRate, "V.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = false;
            }

            //Maker
            if (ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Maker");
                gvMultiRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvMultiRate, "M.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = true;

                CampaignTypeVendorBindMultiRate();
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Maker");
                gvMultiRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvMultiRate, "M.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = false;
            }

            //ESB
            if (ckbCreateCampaignTypeVendorShareSub.Items[2].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Easybuy");
                gvMultiRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvMultiRate, "E.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = true;

                CampaignTypeVendorBindMultiRate();
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[2].Selected)
            {
                gvIdx = GetColumnIndexByName(gvMultiRate, "Easybuy");
                gvMultiRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvMultiRate, "E.Sub Rate");
                gvMultiRate.Columns[gvIdx].Visible = false;
            }
        }

        protected void gvPartyRateOptionalShareSubEvent()
        {
            int gvIdx = -1;
            //Vendor
            if (ckbShareSub.Items[1].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Vendor");
                gvPartyRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvPartyRate, "V.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = true;

                TabShareSubVendorControlVisible(true);
                CampaignTypeVendorBindPartyRate();
            }
            else if (!ckbShareSub.Items[1].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Vendor");
                gvPartyRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvPartyRate, "V.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = false;

                TabShareSubVendorControlVisible(false);
            }

            //Maker
            if (ckbShareSub.Items[0].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Maker");
                gvPartyRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvPartyRate, "M.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = true;

                TabShareSubVendorControlVisible(false);
                CampaignTypeVendorBindPartyRate();
            }
            else if (!ckbShareSub.Items[0].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Maker");
                gvPartyRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvPartyRate, "M.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = false;

                TabShareSubVendorControlVisible(false);
            }

            //ESB
            if (ckbShareSub.Items[2].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Easybuy");
                gvPartyRate.Columns[gvIdx].Visible = true;
                gvIdx = GetColumnIndexByName(gvPartyRate, "E.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = true;

                CampaignTypeVendorBindPartyRate();
            }
            else if (!ckbShareSub.Items[2].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Easybuy");
                gvPartyRate.Columns[gvIdx].Visible = false;
                gvIdx = GetColumnIndexByName(gvPartyRate, "E.Sub Rate");
                gvPartyRate.Columns[gvIdx].Visible = false;
            }

            //Type Sub
            if (rdoTypeSub.Items[0].Selected || rdoTypeSub.Items[1].Selected)
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Type");
                gvPartyRate.Columns[gvIdx].Visible = true;
            }
            else
            {
                gvIdx = GetColumnIndexByName(gvPartyRate, "Type");
                gvPartyRate.Columns[gvIdx].Visible = false;
            }
        }

        protected void btnCreateCampaignClearClick(object sender, EventArgs e)
        {
            foreach (ListItem shareSubList in ckbShareSub.Items)
            {
                shareSubList.Selected = false;
            }

            txtCampaignCode.Text = null;
            txtMakerCode.Text = null;
            txtVendorCode.Text = null;
            ViewCreateCampaign(true);

            rdoCreateCampaign.Enabled = true;
            rdoCreateCampaign.ClearSelection();
            ddlCampaignType.Enabled = true;
            ddlCampaignType.SelectedIndex = 0;
            pCampaignDetail.Visible = true;
            pCampaignDetailValue.Visible = true;
            pCampaignDetail.Enabled = false;
            pCampaignDetailValue.Enabled = false;
            rSubSeq.Visible = false;
            txtBaseRate.Text = null;

            SelectCampaignType.Visible = false;

            SaveButton.Visible = false;

            ClearCreateCampaign();
            //Globals.checkFlag = null;

            Globals.showStatusHeader = null;
            Globals.copyCampaignOld = null;
            HdnIsEdit1.Value = null;
            Globals.newIdCampaign = null;
            showStatusCampaignDefult();
            //ModeManagement();
            //SetDefault();        
        }

        protected void txtCreateCampaingTypeVendorBaseRate_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text) && String.IsNullOrWhiteSpace(txtCreateCampaingTypeVendorBaseRate.Text))
            {
                pRangeYSetVisible();
            }
            else
            {
                txtCreateCampaingTypeVendorBaseRate.Text = Convert.ToDecimal(ConvertTo2Decimal(txtCreateCampaingTypeVendorBaseRate.Text)) > _MaximumBaseRate ? _MaximumBaseRate.ToString("N2") : ConvertTo2Decimal(txtCreateCampaingTypeVendorBaseRate.Text);

                pRangeYSetVisible();
            }
        }

        protected void txtCreateCampaignRateSeq_TextChanged(object sender, EventArgs e)
        {
            txtCreateCampaignRateSeq.Text = ConvertTo2Decimal(txtCreateCampaignRateSeq.Text);
            //txtCreateCampaingTypeVendorBaseRate.Text = txtCreateCampaignRateSeq.Text;
        }

        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            //var tx = txtStartDate.Text;
            txtEndDate.Text = DateAdd(txtStartDate.Text, 30);
            txtClosingApplicationDate.Text = DateAdd(txtStartDate.Text, 30);
            txtClosingLayBillDate.Text = DateAdd(txtStartDate.Text, 60);
        }

        protected void txtEndDate_TextChanged(object sender, EventArgs e)
        {
            txtClosingApplicationDate.Text = txtEndDate.Text;
            txtClosingLayBillDate.Text = DateAdd(txtEndDate.Text, 30);
        }

        protected void txtClosingApplicationDate_TextChanged(object sender, EventArgs e)
        {
            txtClosingLayBillDate.Text = DateAdd(txtClosingApplicationDate.Text, 30);
        }
        #endregion

        protected void btnSaveClick(object sender, EventArgs e)
        {

        }

        protected void btnClearClick(object sender, EventArgs e)
        {
            if (HdnIsEdit1.Value == "EDIT_CAMPAIGN" && HdnIsEdit2.Value == "EDIT_CONFIRM")
            {
                txtCampaignName.Text = null;
                txtProductDetail.Text = null;
                txtSpecialPremium.Text = null;
                txtStartDate.Text = null;
                txtEndDate.Text = null;
                txtXDue.Text = null;
                txtClosingApplicationDate.Text = null;
                txtClosingLayBillDate.Text = null;
                btnClearRateClick(null, null);

                //---

            }
            else
            {
                ViewCreateCampaign(true);

                rdoCreateCampaign.Enabled = true;
                rdoCreateCampaign.ClearSelection();
                ddlCampaignType.Enabled = true;
                ddlCampaignType.SelectedIndex = 0;
                pCampaignDetail.Visible = true;
                pCampaignDetailValue.Visible = true;
                pCampaignDetail.Enabled = false;
                pCampaignDetailValue.Enabled = false;
                rSubSeq.Visible = false;

                SelectCampaignType.Visible = false;

                SaveButton.Visible = false;
                buttonTempCreate.Visible = true;
                //txtBaseRate.Text = null;
                ClearCreateCampaign();
                btnStatus.Text = null;
                txtCampaignCode.Text = null;
                txtMakerCode.Text = null;
                //Globals.checkFlag = null;

                Globals.showStatusHeader = null;
                Globals.copyCampaignOld = null;
                HdnIsEdit1.Value = null;
                Globals.newIdCampaign = null;
                showStatusCampaignDefult();
                foreach (ListItem shareSubList in ckbShareSub.Items)
                {
                    shareSubList.Selected = false;
                }

            }

            //ModeManagement();
            //SetDefault();
        }

        protected void ClearCreateCampaign()
        {
            //Clear data.
            rdoCreateCampaingTypeVendorCalculateType.ClearSelection();
            rbCreateCampaingTypeVendorCampaignSupport.ClearSelection();
            rbOptionalAddMultiTerm.ClearSelection();
            rbOptionalAddMultiTerm.Enabled = true;
            gvMultiRate.Columns[0].Visible = false;
            txtCreateCampaingTypeVendorBaseRate.Text = null;
            txtCreateCampaignRateSeq.Text = null;
            txtCreateCampaignTotalTermRate.Text = null;
            //txtCreateCampaignTotalTermRateTo.Text = null;
            txtVendorCode.Text = null;
            hdfLoanType.Value = null;
            txtCampaignName.Text = null;
            txtProductDetail.Text = null;
            txtSpecialPremium.Text = null;
            txtTermMultiRange.Text = null;
            txtMarketingCode.Text = null;
            txtBaseRate.Text = null;
            //txtVendorCreditDays.Text = null;
            rdbSelectVendor.Items[0].Selected = true;
            btnVendorListSearch.Visible = false;
            ClearTermRange();

            //Hide Panel.
            pBaseRate.Visible = false;
            pRangeN.Visible = false;
            pRangeY.Visible = false;
            pAddMonth.Visible = false;
            pAddRate.Visible = false;
            pOptionAddMultiTerm.Visible = false;

            TabContentControlSetVisible(false);

            gvVendorList.DataSource = null;
            gvVendorList.DataBind();

            ClearSession();

            ModeManagement();
            SetDefault();
        }

        protected void ViewCreateCampaign(bool enable)
        {
            Mode.Visible = enable;
            SelectCampaignType.Visible = true;
            ddlCampaignType.Enabled = enable;
            pCampaignDetail.Visible = true;
            pCampaignDetailValue.Visible = true;
            //pCampaignDetail.Enabled = false;
            txtCampaignCode.Enabled = enable;
            SaveButton.Visible = enable;

            //Textbox
            foreach (TextBox tb in pCampaignDetail.Controls.OfType<TextBox>())
            {
                tb.Enabled = enable;
            }
            //RadioButtonList
            foreach (RadioButtonList rbl in pCampaignDetail.Controls.OfType<RadioButtonList>())
            {
                rbl.Enabled = enable;
            }
            //CheckBoxList
            foreach (CheckBoxList cbl in pCampaignDetail.Controls.OfType<CheckBoxList>())
            {
                cbl.Enabled = enable;
            }

            //Textbox
            foreach (TextBox tb in pCampaignDetailValue.Controls.OfType<TextBox>())
            {
                tb.Enabled = enable;
            }
            //RadioButtonList
            foreach (RadioButtonList rbl in pCampaignDetailValue.Controls.OfType<RadioButtonList>())
            {
                rbl.Enabled = enable;
            }
            //CheckBoxList
            foreach (CheckBoxList cbl in pCampaignDetailValue.Controls.OfType<CheckBoxList>())
            {
                cbl.Enabled = enable;
            }

            txtCampaignCode.Enabled = false;

            //pBaseRate
            rbCreateCampaingTypeVendorCampaignSupport.Enabled = enable;
            txtCreateCampaingTypeVendorBaseRate.Enabled = enable;

            //pRangeY
            //rbRange.Enabled = enable;
            txtTermRange.Enabled = enable;
            txtEndTermRange.Enabled = enable;
            btnSetTermRange.Enabled = enable;
            btnClearTermRange.Enabled = enable;

            pAddMonth.Enabled = enable;
            pAddRate.Enabled = enable;
            //pRangeN.Enabled = enable;

            txtProductDetail.Enabled = true;
            txtSpecialPremium.Enabled = true;
        }
        #endregion

        #region Tab Share Sub
        //protected void GV_APPLICATION_TYPE_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        //{


        //}
        protected void btnAddSubSeq_Click(object sender, EventArgs e)
        {
            //pRangeY.Visible = true;
            //btnAddSubSeq.Enabled = false;
            SetSubSeqTermVisible(false);
        }

        protected void ckbCreateCampaignTypeVendorShareSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRateOptionalShareSubEvent();
            gvMultiRateOptionalShareSubEvent();
            SetVisibleCampaignMakerVendorCode();
        }

        protected void SetVisibleCampaignMakerVendorCode()
        {
            //Vendor.Visible = false;
            //Maker
            if (ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                Maker.Visible = true;
            }
            else
            {
                Maker.Visible = false;
            }

            //Vendor
            if (ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                Vendor.Visible = true;
            }
            else
            {
                Vendor.Visible = false;
            }
        }

        protected void txtRateOptionalShareSubEvent()
        {
            //rSubVendor rSubMaker rSubESB
            //Vendor
            if (ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                rSubVendor.Visible = true;
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                rSubVendor.Visible = false;
            }

            //Maker
            if (ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                rSubMaker.Visible = true;
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                rSubMaker.Visible = false;
            }

            //ESB
            if (ckbCreateCampaignTypeVendorShareSub.Items[2].Selected)
            {
                rSubESB.Visible = true;
            }
            else if (!ckbCreateCampaignTypeVendorShareSub.Items[2].Selected)
            {
                rSubESB.Visible = false;
            }
        }
        protected void LoadApplicationTypeData()
        {
            DataSet ds = GetApplicationType();
            if (campaignStorage.check_dataset(ds))
            {
                gvApplicationType.DataSource = ds;
                gvApplicationType.DataBind();
            }
        }

        protected void TabShareSubBindData()
        {

            DataTable dt = dtShareSub();

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_party_rate.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
            campaignStorage.SetCookiesDataTableByName("ds_party_rate", dt);
            gvPartyRate.DataSource = ds;
            gvPartyRate.DataBind();
        }

        protected void gvPartyRatePageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPartyRate.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(ds_multi_rate.Value))
            {
                gvPartyRate.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_party_rate.Value);
            }
            else
            {
                gvPartyRate.DataSource = campaignStorage.GetCookiesDataSetByKey("ds_party_rate");
            }
            gvPartyRate.DataBind();
        }


        protected void btnClearRateClick(object sender, EventArgs e)
        {
            //pTab.Enabled = false;
            if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
            {
                //txtVendorCreditDays.Text = null;
                btnCreateCampaignClear.Visible = false;
                //tabDetail.Enabled = false;
                tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("ShareSub");
            }
            SetTabControlReadOnly();
            btnClearTermRangeClick(null, null);

            rbCreateCampaingTypeVendorCampaignSupport.ClearSelection();
            rbOptionalAddMultiTerm.ClearSelection();
            rbOptionalAddMultiTerm.Enabled = true;
            gvMultiRate.Columns[0].Visible = false;
            pOptionAddMultiTerm.Visible = false;
            txtTermMultiRange.Text = null;
            txtCreateCampaingTypeVendorBaseRate.Text = null;
            SaveButton.Visible = false;
            buttonTempCreate.Visible = true;
            pRangeY.Visible = false;
            rSubSeq.Visible = false;

            pRangeYSetVisible();

            pAddMonth.Enabled = true;
            pAddRate.Enabled = true;

            ClearShareSub();
            btnClearTermRange.Enabled = true;
            btnAddVendor.Enabled = true;
            btnAddMakerCode.Enabled = true;
            txtCampaignName.Enabled = true;
            txtStartDate.Enabled = true;
            txtEndDate.Enabled = true;
            txtClosingApplicationDate.Enabled = true;
            txtClosingLayBillDate.Enabled = true;
            ckbPoupCreateCampaingTypeVendorApplicationType.Enabled = true;
            ckbCreateCampaignTypeVendorShareSub.Enabled = true;
            rdoCreateCampaingTypeVendorCalculateType.Enabled = true;
            lblSubSeqList.Text = null;

            campaignStorage.ClearCookies("ds_multi_rate");
            campaignStorage.ClearCookies("ds_party_rate");
            ds_multi_rate.Value = string.Empty;
            ds_party_rate.Value = string.Empty;
            campaignStorage.ClearCookies("ds_vendor_list");
            ds_vendor_list.Value = string.Empty;
            campaignStorage.ClearCookies("ds_term_of_campaign");
            campaignStorage.ClearCookies("ds_term_of_campaign_detail");
            ds_term_of_campaign.Value = string.Empty;
            ds_term_of_campaign_detail.Value = string.Empty;
            gvPartyRate.DataSource = null;
            gvPartyRate.DataBind();

            gvViewTermOfCampaign.DataSource = null;
            gvViewTermOfCampaign.DataBind();

            gvMultiRate.DataSource = null;
            gvMultiRate.DataBind();

            gvVendorList.DataSource = null;
            gvVendorList.DataBind();
            gv_listVendor_2.DataSource = null;
            gv_listVendor_2.DataBind();

            gvViewTermOfCampaignDetail.DataSource = null;
            gvViewTermOfCampaignDetail.DataBind();

            ds_AddItemProduct.Value = string.Empty;
            ds_AddItemExceptProduct.Value = string.Empty;
            ds_itemSeleced.Value = string.Empty;
            ds_gv_selectlistproduct.Value = string.Empty;
            ds_gv_selectlistvendor.Value = string.Empty;
            ds_gridBanch.Value = string.Empty;
            ds_initial_value.Value = string.Empty;
            ds_gridAppType.Value = string.Empty;
            ds_gvListVendor_2.Value = string.Empty;
            ds_gvListVendor.Value = string.Empty;
            dt_Popup_Vendorlistselected_2.Value = string.Empty;
            dt_Popup_Vendorlistselected_1.Value = string.Empty;
            ds_gvCampaign_2.Value = string.Empty;
            ds_gvProductType_2.Value = string.Empty;
            ds_gvProductCode_2.Value = string.Empty;
            ds_gvModel_2.Value = string.Empty;
            ds_gvProductItem_2.Value = string.Empty;
            ds_gvVendor_2.Value = string.Empty;
            ds_gvVendor.Value = string.Empty;
            ds_gvProductType.Value = string.Empty;
            ds_gvProductCode.Value = string.Empty;
            ds_gvModel.Value = string.Empty;
            ds_gvProductItem.Value = string.Empty;
            ds_gvCampaign.Value = string.Empty;
            ds_gvListProduct.Value = string.Empty;
            dt_Popup_Productlistselected.Value = string.Empty;
        }

        protected void TabShareSubSetControlMode(bool enable)
        {
            //CampaignType
            string campaignType = ddlCampaignType.SelectedValue;
            switch (campaignType)
            {
                case "MAKER":
                    ckbShareSub.Items[0].Enabled = true;
                    ckbShareSub.Items[1].Enabled = false;
                    ckbShareSub.Items[2].Enabled = true;
                    break;
                case "VENDOR":
                    ckbShareSub.Items[0].Enabled = false;
                    ckbShareSub.Items[1].Enabled = true;
                    ckbShareSub.Items[2].Enabled = true;
                    break;
                case "ESB":
                    ckbShareSub.Items[0].Enabled = false;
                    ckbShareSub.Items[1].Enabled = false;
                    ckbShareSub.Items[2].Enabled = true;
                    break;
                case "SHARESUB":
                    ckbShareSub.Items[0].Enabled = true;
                    ckbShareSub.Items[1].Enabled = true;
                    ckbShareSub.Items[2].Enabled = true;
                    break;
                default:
                    ckbShareSub.Items[0].Enabled = true;
                    ckbShareSub.Items[1].Enabled = true;
                    ckbShareSub.Items[2].Enabled = true;
                    break;
            }
        }

        protected void TabShareSubVendorControlVisible(bool enable)
        {
            //Vendor Payment
            rdoVendorPayment.Enabled = enable;
            txtVendorCreditDays.Enabled = enable;
            txtVendorCreditDays.ReadOnly = !enable;
        }

        protected void rdoTypeSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPartyRateOptionalShareSubEvent();
        }

        protected void ckbShareSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPartyRateOptionalShareSubEvent();
        }
        #endregion

        #region Tab Term | Product
        protected void CAMPAIGN_DETAIL_ROW_BOUND(object sender, GridViewRowEventArgs e)
        {
            if (HdnIsEdit1.Value == "VIEW_CAMPAIGN")
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Visible = false;
                }
            }
        }
        private void DATA_ILTB71_002()
        {
            SqlAll = "SELECT T71ITM, T71DES FROM AS400DB01.ILOD0001.ILTB71 WITH (NOLOCK) WHERE T71CDE = '002' ORDER BY T71ITM ASC ";
            DataSet DS_SELECT = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_SELECT = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ddlSelectItem.DataSource = DS_SELECT;
            ddlSelectItem.DataTextField = "T71DES";
            ddlSelectItem.DataValueField = "T71ITM";
            ddlSelectItem.DataBind();


        }
        protected void BTN_SEARCH_PRODUCT_CODE(object sender, EventArgs e)
        {
            SEARCH_ITEM_CODE();
        }
        protected void BTN_CLEAR_PRODUCT_CODE(object sender, EventArgs e)
        {
            txtProductCode.Text = "";
            SEARCH_ITEM_CODE();
        }
        private void SEARCH_ITEM_CODE()
        {
            string sqlwhere = "";
            if (ddlItemCode.SelectedValue == "ICT" && txtProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41TYP = '" + txtProductCode.Text.Trim() + "' ";
            }
            if (ddlItemCode.SelectedValue == "ICC" && txtProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41COD = '" + txtProductCode.Text.Trim() + "' ";
            }
            if (ddlItemCode.SelectedValue == "ICN" && txtProductCode.Text.Trim() != "")
            {

                sqlwhere += " AND T41DES LIKE '%' || @T41DES || '%' ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("T41DES", txtProductCode.Text.ToUpper().Trim());
            }

            SqlAll = "SELECT T41TYP,T41COD,T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41DEL=''";

            SqlAll = SqlAll + sqlwhere + " ORDER BY T41TYP,T41COD";
            cmd.CommandText = SqlAll;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;


            gv_searchItemCode.DataSource = DS;
            gv_searchItemCode.DataBind();

            if (gv_searchItemCode.Rows.Count == 0)
            {

                //E_error.Text = "Data not found";
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();

        }
        protected void BTN_SEARCH_PRODUCT_TYPE(object sender, EventArgs e)
        {
            SEARCH_ITEM_TYPE();
        }
        protected void BTN_CLEAR_PRODUCT_TYPE(object sender, EventArgs e)
        {
            txtProductType.Text = "";
            SEARCH_ITEM_TYPE();
        }
        private void SEARCH_ITEM_TYPE()
        {
            string sqlwhere = "";
            if (ddlItemType.SelectedValue == "ITT" && txtProductType.Text.Trim() != "")
            {
                sqlwhere += " AND T40TYP = '" + txtProductType.Text.Trim() + "' ";
            }
            if (ddlItemType.SelectedValue == "ITC" && txtProductType.Text.Trim() != "")
            {
                sqlwhere += " AND t40fix = '" + txtProductType.Text.Trim() + "' ";
            }
            if (ddlItemType.SelectedValue == "ITN" && txtProductType.Text.Trim() != "")
            {
                sqlwhere += " AND t40des LIKE '%' || @t40des || '%' ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("t40des", txtProductType.Text.ToUpper().Trim());
            }

            SqlAll = "SELECT distinct T40TYP,'0' AS t40fix,t40des FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL= '' ";

            SqlAll = SqlAll + sqlwhere + " ORDER BY T40TYP";
            cmd.CommandText = SqlAll;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                gv_searchItemType.DataSource = DS;
                gv_searchItemType.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();

        }
        protected void CONFIRM_SELECT_ITEM(object sender, EventArgs e)
        {
            string productItem = ddlSelectProduct.SelectedValue;
            switch (productItem)
            {
                case "productType1":
                    optionAddItem.Enabled = false;
                    break;
                case "productType2":
                    DataTable DT_ADD_ITEM_CODE = dtSelectProductItem();
                    foreach (GridViewRow row in gv_searchItemCode.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("CheckBoxSelectItemCode");
                        if (chk.Checked)
                        {
                            DT_ADD_ITEM_CODE.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text);
                        }
                    }
                    ds_itemSeleced.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(DT_ADD_ITEM_CODE);
                    ddlSelectItem.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);
                    ddlSelectItem.DataTextField = "NAME";
                    ddlSelectItem.DataValueField = "CODE";
                    ddlSelectItem.DataBind();
                    PopupSearchItemCode.ShowOnPageLoad = false;
                    break;
                case "productType3":

                    DataTable DT_ADD_ITEM_TYPE = dtSelectProductItem();
                    foreach (GridViewRow row in gv_searchItemType.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("CheckBoxSelectItemType");
                        if (chk.Checked)
                        {
                            DT_ADD_ITEM_TYPE.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text);
                        }
                    }
                    ds_itemSeleced.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(DT_ADD_ITEM_TYPE);
                    ddlSelectItem.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value); ;
                    ddlSelectItem.DataTextField = "NAME";
                    ddlSelectItem.DataValueField = "CODE";
                    ddlSelectItem.DataBind();
                    PopupSearchItemType.ShowOnPageLoad = false;
                    break;
                case "productType4":
                    optionAddItem.Enabled = true;
                    break;
                default:

                    break;
            }






        }
        protected void SEARCH_ITEM(object sender, EventArgs e)
        {
            //if (Globals.checkTypeSearchItem == "Code")
            if (ddlSelectProduct.SelectedValue == "productType2")
            {
                txtProductCode.Text = "";
                SEARCH_ITEM_CODE();
                BindDataSearchItemCode();
                PopupSearchItemCode.ShowOnPageLoad = true;

            }
            //else if (Globals.checkTypeSearchItem == "Type")
            else if (ddlSelectProduct.SelectedValue == "productType3")
            {
                txtProductType.Text = "";
                SEARCH_ITEM_TYPE();
                BindDataSearchItemType();
                PopupSearchItemType.ShowOnPageLoad = true;
            }
            //else if (Globals.checkTypeSearchItem == "someProduct")
            if (ddlSelectProduct.SelectedValue == "productType4")
            {
                BindSessionProductItemsFromSubSeq();
                popupSearchProduct.ShowOnPageLoad = true;
                popupSearchProduct.Focus();

                LoaddataFromgvselectlistProduct();
            }

        }
        protected void BindSessionProductItemsFromSubSeq()
        {
            string subseq = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");

            //ds_gv_selectlistproduct
            if (optionAddItem.SelectedValue == "onlySeq")
            {
                if (gvViewTermOfCampaignDetail.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("T42BRD");
                    dt.Columns.Add("T42DES");

                    foreach (GridViewRow gvRow in gvViewTermOfCampaignDetail.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["T42BRD"] = gvRow.Cells[4].Text;
                        dr["T42DES"] = gvRow.Cells[5].Text;

                        dt.Rows.Add(dr);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds_gv_selectlistproduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                }
                else
                {
                    ds_gv_selectlistproduct.Value = string.Empty;
                }
            }
        }
        protected void BindDataSearchItemCode()
        {
            if (gv_searchItemCode.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_itemSeleced.Value))
                {
                    DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (GridViewRow gvRow in gv_searchItemCode.Rows)
                            {
                                CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectItemCode");
                                if (dr["TYPE"].ToString() == gvRow.Cells[1].Text && dr["CODE"].ToString() == gvRow.Cells[2].Text)
                                {
                                    chk.Checked = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void BindDataSearchItemType()
        {
            if (gv_searchItemType.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_itemSeleced.Value))
                {
                    DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (GridViewRow gvRow in gv_searchItemType.Rows)
                            {
                                CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectItemType");
                                if (dr["TYPE"].ToString() == gvRow.Cells[1].Text && dr["CODE"].ToString() == gvRow.Cells[2].Text)
                                {
                                    chk.Checked = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void ddlSelectProduct_onchange(object sender, EventArgs e)
        {
            optionAddItem.ClearSelection();
            string productItem = ddlSelectProduct.SelectedValue;
            switch (productItem)
            {
                case "productType1":
                    optionAddItem.Items.FindByValue("allSeq").Selected = true;
                    optionAddItem.Enabled = false;
                    ddlSelectItem.Enabled = false;
                    serachProductCampaign.Enabled = false;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = false;
                    //ListItem optionType4 = optionAddItem.Items.FindByValue("allSeq");
                    //optionType4.Selected = true;
                    break;
                case "productType2":
                    Globals.checkTypeSearchItem = "Code";
                    optionAddItem.Items.FindByValue("allSeq").Selected = true;
                    optionAddItem.Enabled = false;
                    ddlSelectItem.Enabled = true;
                    serachProductCampaign.Enabled = true;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = false;
                    //ListItem optionType5 = optionAddItem.Items.FindByValue("allSeq");
                    //optionType5.Selected = true;
                    break;
                case "productType3":
                    Globals.checkTypeSearchItem = "Type";
                    optionAddItem.Items.FindByValue("allSeq").Selected = true;
                    optionAddItem.Enabled = false;
                    ddlSelectItem.Enabled = true;
                    serachProductCampaign.Enabled = true;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = false;
                    //ListItem optionType6 = optionAddItem.Items.FindByValue("allSeq");
                    //optionType6.Selected = true;
                    break;
                case "productType4":
                    Globals.checkTypeSearchItem = "someProduct";
                    optionAddItem.Enabled = true;
                    ddlSelectItem.Enabled = true;
                    serachProductCampaign.Enabled = true;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = true;
                    break;
                default:
                    serachProductCampaign.Enabled = false;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = false;
                    break;
            }

            ClearProductItem();
        }
        protected void gvViewTermOfCampaignDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);

                string subseq = e.Values["SubSeq"].ToString();
                string code = e.Values["Code"].ToString();
                string type = e.Values["Type"].ToString();


                if (optionAddItem.SelectedValue == "onlySeq")
                {
                    DataRow dr = dt.AsEnumerable().Where(r => r.Field<string>("SubSeq").ToString() == subseq && r.Field<string>("Type") == type && r.Field<string>("Code") == code).FirstOrDefault();
                    if (dr != null)
                    {
                        dt.Rows.Remove(dr);
                    }
                }
                else if (optionAddItem.SelectedValue == "allSeq")
                {
                    var dr = dt.AsEnumerable().Where(r => r.Field<string>("Type") == type && r.Field<string>("Code") == code).ToArray();
                    if (dr.Length > 0)
                    {
                        foreach (var r in dr)
                        {
                            dt.Rows.Remove(r);
                        }
                    }
                }
                ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);

                //Bind to gridview.
                //if (optionAddItem.SelectedValue == "onlySeq")
                //{
                DataTable dtView = new DataTable();
                var drView = dt.Select("SubSeq = " + subseq);
                if (drView.Length > 0)
                {
                    dtView = drView.CopyToDataTable();
                    gvViewTermOfCampaignDetail.DataSource = dtView;
                    gvViewTermOfCampaignDetail.DataBind();
                }
                else
                {
                    gvViewTermOfCampaignDetail.DataSource = dt;
                    gvViewTermOfCampaignDetail.DataBind();
                }
                //DataTable dtview = dt.Select("SubSeq = " + subseq).CopyToDataTable();


                //}
                //else
                //{
                //    gvViewTermOfCampaignDetail.DataSource = dt;
                //    gvViewTermOfCampaignDetail.DataBind();
                //}

                dt.Dispose();

                if (ddlSelectProduct.SelectedValue == "productType2" || ddlSelectProduct.SelectedValue == "productType3")
                {
                    dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);

                    var dr = dt.AsEnumerable().Where(r => r.Field<string>("Type") == type && r.Field<string>("Code") == code).ToArray();
                    if (dr.Length > 0)
                    {
                        foreach (var r in dr)
                        {
                            dt.Rows.Remove(r);
                        }
                    }
                    ds_AddItemExceptProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                }
                else if (ddlSelectProduct.SelectedValue == "productType4")
                {
                    dt = ((DataSet)campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistproduct.Value))?.Tables[0]?.Copy();

                    var dr = dt.AsEnumerable().Where(r => r.Field<string>("T42BRD") == code).FirstOrDefault();
                    if (dr != null) { dt.Rows.Remove(dr); }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds_gv_selectlistproduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                }
            }
        }
        protected void ADD_PRODUCT_ITEM(object sender, EventArgs e)
        {
            string subseqfromlist = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");
            double txtPriceMins = 0.00;
            double txtPriceMaxs = 0.00;
            string productItem = ddlSelectProduct.SelectedValue;
            if (txtPriceMin.Text != "")
            {
                txtPriceMins = double.Parse(txtPriceMin.Text);
            }
            if (txtPriceMax.Text != "")
            {
                txtPriceMaxs = double.Parse(txtPriceMax.Text);
            }

            if (txtPriceMin.Text == "" && txtPriceMax.Text == "")
            {
                lblMsgAlert.Text = "กรุณาระบุราคาสินค้า !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (txtPriceMin.Text == "" && txtPriceMax.Text != "")
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (txtPriceMin.Text != "" && txtPriceMax.Text == "")
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (txtPriceMins >= txtPriceMaxs && txtPriceMaxs != 0.00)
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else
            {
                switch (productItem)
                {
                    case "productType1":
                        optionAddItem.Enabled = false;

                        DataTable dtAddItemAll = dtTermOfCampaignDetail();
                        foreach (GridViewRow drTermOfCampaign in gvViewTermOfCampaign.Rows)
                        {
                            //TextBox txtSubSeq = (TextBox)drTermOfCampaign.FindControl("tbC02CSQ");
                            int subSeq = int.Parse(drTermOfCampaign.Cells[2].Text);

                            TextBox txtRates = (TextBox)drTermOfCampaign.FindControl("tbRATES");
                            string rates = txtRates.Text.ToString();
                            if (!String.IsNullOrEmpty(txtPriceMin.Text.ToString()))
                            {
                                pricesMin = double.Parse(txtPriceMin.Text.ToString());
                            }
                            if (!String.IsNullOrEmpty(txtPriceMax.Text.ToString()))
                            {
                                pricesMax = double.Parse(txtPriceMax.Text.ToString());
                            }

                            if (txtPriceMin.Text.ToString() != "" && txtPriceMax.Text.ToString() != "")
                            {
                                dtAddItemAll.Rows.Add(subSeq, rates, "0", "0", "ALL PRODUCT ITEM ", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                            }
                            else
                            {
                                dtAddItemAll.Rows.Add(subSeq, rates, "0", "0", "ALL PRODUCT ITEM ", pricesMin.ToString("#,##0.00").Trim());
                            }

                        }
                        ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemAll);

                        if (gvViewTermOfCampaign.Rows.Count > 1)
                        {
                            DataTable dtview = dtAddItemAll.Select("SubSeq = " + subseqfromlist).CopyToDataTable();
                            gvViewTermOfCampaignDetail.DataSource = dtview;
                            gvViewTermOfCampaignDetail.DataBind();
                        }
                        else
                        {
                            gvViewTermOfCampaignDetail.DataSource = dtAddItemAll;
                            gvViewTermOfCampaignDetail.DataBind();
                        }
                        break;

                    case "productType2":
                        DataTable dtAddItemCode = dtTermOfCampaignDetail();
                        DataTable dt_listProductCode = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);
                        if (string.IsNullOrEmpty(ds_itemSeleced.Value))
                        {
                            lblMsgAlert.Text = "กรุณาเลือก Product Code อย่างน้อย 1 รายการ !";
                            PopupAlertRate.ShowOnPageLoad = true;
                            serachProductCampaign.Focus();
                        }
                        else
                        {
                            foreach (GridViewRow drTermOfCampaign in gvViewTermOfCampaign.Rows)
                            {

                                //TextBox txtSubSeq = (TextBox)drTermOfCampaign.FindControl("tbC02CSQ");
                                int subSeq = int.Parse(drTermOfCampaign.Cells[2].Text);

                                TextBox txtRates = (TextBox)drTermOfCampaign.FindControl("tbRATES");
                                string rates = txtRates.Text.ToString();

                                if (!String.IsNullOrEmpty(txtPriceMin.Text.ToString()))
                                {
                                    pricesMin = double.Parse(txtPriceMin.Text.ToString());
                                }
                                if (!String.IsNullOrEmpty(txtPriceMax.Text.ToString()))
                                {
                                    pricesMax = double.Parse(txtPriceMax.Text.ToString());
                                }

                                foreach (DataRow dr_listProduct in dt_listProductCode.Rows)
                                {
                                    if (txtPriceMin.Text.ToString() != "" && txtPriceMax.Text.ToString() != "")
                                    {
                                        dtAddItemCode.Rows.Add(subSeq, rates, dr_listProduct["TYPE"], dr_listProduct["CODE"], "ALL PRODUCT EXCEPT : " + dr_listProduct["NAME"] + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                                    }
                                    else
                                    {
                                        dtAddItemCode.Rows.Add(subSeq, rates, dr_listProduct["TYPE"], dr_listProduct["CODE"], "ALL PRODUCT EXCEPT : " + dr_listProduct["NAME"] + "", pricesMin.ToString("#,##0.00").Trim());
                                    }
                                }
                            }

                            ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);
                            ds_AddItemExceptProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);

                            if (gvViewTermOfCampaign.Rows.Count > 1)
                            {
                                DataTable dtview = dtAddItemCode.Select("SubSeq = " + subseqfromlist).CopyToDataTable();
                                gvViewTermOfCampaignDetail.DataSource = dtview;
                                gvViewTermOfCampaignDetail.DataBind();
                            }
                            else
                            {
                                gvViewTermOfCampaignDetail.DataSource = dtAddItemCode;
                                gvViewTermOfCampaignDetail.DataBind();
                            }
                        }
                        break;

                    case "productType3":

                        DataTable dtAddItemType = dtTermOfCampaignDetail();
                        DataTable dt_listProductType = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);
                        if (string.IsNullOrEmpty(ds_itemSeleced.Value))
                        {
                            lblMsgAlert.Text = "กรุณาเลือก Product Type อย่างน้อย 1 รายการ !";
                            PopupAlertRate.ShowOnPageLoad = true;
                            serachProductCampaign.Focus();
                        }
                        else
                        {
                            foreach (GridViewRow drTermOfCampaign in gvViewTermOfCampaign.Rows)
                            {
                                //TextBox txtSubSeq = (TextBox)drTermOfCampaign.FindControl("tbC02CSQ");
                                int subSeq = int.Parse(drTermOfCampaign.Cells[2].Text);

                                TextBox txtRates = (TextBox)drTermOfCampaign.FindControl("tbRATES");
                                string rates = txtRates.Text.ToString();

                                if (!String.IsNullOrEmpty(txtPriceMin.Text.ToString()))
                                {
                                    pricesMin = double.Parse(txtPriceMin.Text.ToString());
                                }
                                if (!String.IsNullOrEmpty(txtPriceMax.Text.ToString()))
                                {
                                    pricesMax = double.Parse(txtPriceMax.Text.ToString());
                                }
                                foreach (DataRow dr_listProduct in dt_listProductType.Rows)
                                {
                                    if (txtPriceMin.Text.ToString() != "" && txtPriceMax.Text.ToString() != "")
                                    {
                                        dtAddItemType.Rows.Add(subSeq, rates, dr_listProduct["TYPE"], dr_listProduct["CODE"], "ALL PRODUCT EXCEPT : " + dr_listProduct["NAME"] + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                                    }
                                    else
                                    {
                                        dtAddItemType.Rows.Add(subSeq, rates, dr_listProduct["TYPE"], dr_listProduct["CODE"], "ALL PRODUCT EXCEPT : " + dr_listProduct["NAME"] + "", pricesMin.ToString("#,##0.00").Trim());
                                    }
                                }

                            }

                            ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemType);
                            ds_AddItemExceptProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemType);
                            if (gvViewTermOfCampaign.Rows.Count > 1)
                            {
                                DataTable dtview = dtAddItemType.Select("SubSeq = " + subseqfromlist).CopyToDataTable();
                                gvViewTermOfCampaignDetail.DataSource = dtview;
                                gvViewTermOfCampaignDetail.DataBind();
                            }
                            else
                            {
                                gvViewTermOfCampaignDetail.DataSource = dtAddItemType;
                                gvViewTermOfCampaignDetail.DataBind();
                            }
                        }

                        break;

                    case "productType4":
                        optionAddItem.Enabled = true;
                        DataTable dtAddItemProduct = new DataTable();
                        if (!string.IsNullOrEmpty(ds_AddItemProduct.Value))
                        {
                            dtAddItemProduct = (campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value)).Copy();
                        }
                        else
                        {
                            dtAddItemProduct = dtTermOfCampaignDetail();
                        }
                        DataSet ds = new DataSet();
                        ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistproduct.Value);
                        DataTable dt_listProductItems = new DataTable();
                        if (campaignStorage.check_dataset(ds))
                        {
                            dt_listProductItems = ds?.Tables[0]?.Copy();
                        }
                        if (string.IsNullOrEmpty(ds_gv_selectlistproduct.Value))
                        {
                            lblMsgAlert.Text = "กรุณาเลือก Product Item อย่างน้อย 1 รายการ !";
                            PopupAlertRate.ShowOnPageLoad = true;
                            serachProductCampaign.Focus();
                        }
                        else
                        {
                            if (optionAddItem.SelectedValue == "allSeq")
                            {
                                foreach (GridViewRow drTermOfCampaign in gvViewTermOfCampaign.Rows)
                                {
                                    //TextBox txtSubSeq = (TextBox)drTermOfCampaign.FindControl("tbC02CSQ");
                                    int subSeq = int.Parse(drTermOfCampaign.Cells[2].Text);

                                    TextBox txtRates = (TextBox)drTermOfCampaign.FindControl("tbRATES");
                                    string rates = txtRates.Text.ToString();

                                    if (!String.IsNullOrEmpty(txtPriceMin.Text.ToString()))
                                    {
                                        pricesMin = double.Parse(txtPriceMin.Text.ToString());
                                    }
                                    if (!String.IsNullOrEmpty(txtPriceMax.Text.ToString()))
                                    {
                                        pricesMax = double.Parse(txtPriceMax.Text.ToString());
                                    }

                                    foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                                    {
                                        //Check Contains Product Code.
                                        string prdCode = dr_listProduct[0].ToString();
                                        bool chkContain = dtAddItemProduct.AsEnumerable()
                                                          .Any(r => prdCode == r.Field<string>("Code")
                                                                    && subSeq.ToString() == r.Field<string>("SubSeq"));
                                        if (!chkContain)
                                        {
                                            if (txtPriceMin.Text.ToString() != "" && txtPriceMax.Text.ToString() != "")
                                            {
                                                dtAddItemProduct.Rows.Add(subSeq, rates, "0", dr_listProduct["T42BRD"], dr_listProduct["T42DES"], pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                                            }
                                            else
                                            {
                                                dtAddItemProduct.Rows.Add(subSeq, rates, "0", dr_listProduct["T42BRD"], dr_listProduct["T42DES"], pricesMin.ToString("#,##0.00").Trim());
                                            }
                                        }
                                    }

                                }
                                ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemProduct);
                            }
                            else if (optionAddItem.SelectedValue == "onlySeq")
                            {
                                dtAddItemProduct = (campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value)).Copy();

                                //TextBox txtSubSeq = (TextBox)drTermOfCampaign.FindControl("tbC02CSQ");
                                string subSeq = subseqfromlist;

                                //TextBox txtRates = (TextBox)drTermOfCampaign.FindControl("tbC02INR");
                                DataTable dt = (campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value))?.Tables[0];
                                string rates = dt.AsEnumerable().Where(r => r.Field<string>("C02CSQ").ToString() == subseqfromlist).Select(s => s.Field<string>("RATES").ToString()).FirstOrDefault();

                                if (!String.IsNullOrEmpty(txtPriceMin.Text.ToString()))
                                {
                                    pricesMin = double.Parse(txtPriceMin.Text.ToString());
                                }
                                if (!String.IsNullOrEmpty(txtPriceMax.Text.ToString()))
                                {
                                    pricesMax = double.Parse(txtPriceMax.Text.ToString());
                                }

                                foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                                {
                                    //Check Contains Product Code.
                                    string prdCode = dr_listProduct[0].ToString();
                                    //bool chkContain = dtAddItemProduct.AsEnumerable().Any(r => prdCode == r.Field<string>("Code"));
                                    bool chkContain = dtAddItemProduct.AsEnumerable()
                                                          .Any(r => prdCode == r.Field<string>("Code")
                                                                    && subSeq.ToString() == r.Field<string>("SubSeq"));
                                    if (!chkContain)
                                    {
                                        if (txtPriceMin.Text.ToString() != "" && txtPriceMax.Text.ToString() != "")
                                        {
                                            dtAddItemProduct.Rows.Add(subSeq, rates, "0", dr_listProduct["T42BRD"], dr_listProduct["T42DES"], pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                                        }
                                        else
                                        {
                                            dtAddItemProduct.Rows.Add(subSeq, rates, "0", dr_listProduct["T42BRD"], dr_listProduct["T42DES"], pricesMin.ToString("#,##0.00").Trim());
                                        }
                                    }
                                }

                                dtAddItemProduct = dtAddItemProduct.DefaultView.ToTable(true);
                                ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemProduct);
                            }

                            //Bind to gridview.
                            if (gvViewTermOfCampaign.Rows.Count > 1)
                            {
                                DataTable dtview = dtAddItemProduct.Select("SubSeq = " + subseqfromlist).CopyToDataTable();
                                gvViewTermOfCampaignDetail.DataSource = dtview;
                                gvViewTermOfCampaignDetail.DataBind();
                            }
                            else
                            {
                                gvViewTermOfCampaignDetail.DataSource = dtAddItemProduct;
                                gvViewTermOfCampaignDetail.DataBind();
                            }

                            BindDataToddlSelectItem();
                        }

                        break;
                    default:

                        break;
                }
            }

        }
        protected void BindDataToddlSelectItem()
        {
            if (!string.IsNullOrEmpty(ds_AddItemProduct.Value))
            {
                string subseq = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");
                DataTable dtAddItemProduct = (campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value)).Copy();
                if (dtAddItemProduct.Rows.Count > 0)
                {
                    dtAddItemProduct.Columns["Code"].ColumnName = "CODE";
                    dtAddItemProduct.Columns["ItemName"].ColumnName = "NAME";

                    //dtAddItemProduct = dtAddItemProduct.DefaultView.ToTable(true); //Distinct
                    var res = dtAddItemProduct.Select("SubSeq = " + subseq);
                    DataTable dt = new DataTable();

                    if (res.Any())
                    {
                        dt = res.CopyToDataTable();
                        ddlSelectItem.DataSource = dt;
                        ddlSelectItem.DataTextField = "NAME";
                        ddlSelectItem.DataValueField = "CODE";
                        ddlSelectItem.DataBind();
                    }
                    else
                    {
                        dt = dtSelectProductItem();
                        ddlSelectItem.DataSource = dt;
                        ddlSelectItem.DataTextField = "NAME";
                        ddlSelectItem.DataValueField = "CODE";
                        ddlSelectItem.DataBind();
                    }
                }
            }
        }
        protected void BindDataToddlSelectItemType4()
        {
            DataSet ds_check = new DataSet();
            ds_check = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistproduct.Value);
            if (campaignStorage.check_dataset(ds_check))
            {
                string subseq = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");
                DataTable dtAddItemProduct = ds_check?.Tables[0]?.Copy();
                if (campaignStorage.check_dataTable(dtAddItemProduct))
                {
                    dtAddItemProduct.Columns["T42BRD"].ColumnName = "CODE";
                    dtAddItemProduct.Columns["T42DES"].ColumnName = "NAME";
                    ddlSelectItem.DataSource = dtAddItemProduct;
                    ddlSelectItem.DataTextField = "NAME";
                    ddlSelectItem.DataValueField = "CODE";
                    ddlSelectItem.DataBind();
                }
            }
        }

        protected void CLEAR_PRODUCT_ITEM(object sender, EventArgs e)
        {
            ClearProductItem();
        }
        protected void optionAddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearProductItem();
        }
        protected void ClearProductItem()
        {
            DataTable dt = dtTermOfCampaignDetail();
            ds_AddItemProduct.Value = string.Empty;
            gvViewTermOfCampaignDetail.DataSource = null;
            gvViewTermOfCampaignDetail.DataSource = dt;
            gvViewTermOfCampaignDetail.DataBind();
            CLEAR_LIST_CAMPAIGN();
            campaignStorage.ClearMaxLargeCookies("ds_gv_selectlistproduct");
            ds_gv_selectlistproduct.Value = string.Empty;
            dt_Popup_Productlistselected.Value = string.Empty;
            campaignStorage.ClearCookies("ds_itemSeleced");
            ds_itemSeleced.Value = string.Empty;
            ddlSelectItem.Items.Clear();
            ddlSelectItem.ClearSelection();
        }
        protected void BindDatagvViewTermOfCampaignDetail()
        {
            //ds_AddItemProduct        
            string subseq = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");

            if (!string.IsNullOrEmpty(ds_AddItemProduct.Value))
            {
                DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
                if (dt.Rows.Count > 0)
                {
                    var res = dt.Select("SubSeq = " + subseq);

                    if (res.Any())
                    {
                        dt = res.CopyToDataTable();
                        gvViewTermOfCampaignDetail.DataSource = dt;
                        gvViewTermOfCampaignDetail.DataBind();
                    }
                    else
                    {
                        dt = dtTermOfCampaignDetail();
                        gvViewTermOfCampaignDetail.DataSource = dt;
                        gvViewTermOfCampaignDetail.DataBind();
                    }

                    BindDataToddlSelectItem();
                }
            }
        }

        protected void GV_SELECTED_LITSUB_SEQ(object sender, GridViewSelectEventArgs e)
        {
            if (HdnIsEdit1.Value == "EDIT_CAMPAIGN" && HdnIsEdit2.Value == "EDIT_CONFIRM")
            {
                controlSubdetail.Enabled = true;
                ddlSelectProduct.Enabled = true;
                btnAddProductItem.Enabled = true;
                btnClearProductItem.Enabled = true;
                txtPriceMin.Enabled = true;
                txtPriceMax.Enabled = true;
                string productItem = ddlSelectProduct.SelectedValue;
                switch (productItem)
                {
                    case "productType1":
                        optionAddItem.Enabled = false;
                        ddlSelectItem.Enabled = false;
                        serachProductCampaign.Enabled = false;
                        gvViewTermOfCampaignDetail.Enabled = true;
                        break;
                    case "productType2":
                        Globals.checkTypeSearchItem = "Code";
                        optionAddItem.Items.FindByValue("allSeq").Selected = true;
                        optionAddItem.Enabled = false;
                        ddlSelectItem.Enabled = true;
                        serachProductCampaign.Enabled = true;
                        gvViewTermOfCampaignDetail.Enabled = true;
                        break;
                    case "productType3":
                        Globals.checkTypeSearchItem = "Type";
                        optionAddItem.Items.FindByValue("allSeq").Selected = true;
                        optionAddItem.Enabled = false;
                        ddlSelectItem.Enabled = true;
                        serachProductCampaign.Enabled = true;
                        gvViewTermOfCampaignDetail.Enabled = true;
                        break;
                    case "productType4":
                        Globals.checkTypeSearchItem = "someProduct";
                        optionAddItem.Enabled = true;
                        ddlSelectItem.Enabled = true;
                        serachProductCampaign.Enabled = true;
                        gvViewTermOfCampaignDetail.Enabled = true;
                        break;
                    default:
                        serachProductCampaign.Enabled = false;
                        break;
                }
            }
            else if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
            {
                gvViewTermOfCampaignDetail.Enabled = false;
            }
            else if (HdnIsEdit1.Value == "LOAD_CAMPAIGN")
            {
                gvViewTermOfCampaignDetail.Enabled = true;
            }
            else if (HdnIsEdit1.Value == "CREATE_CAMPAIGN")
            {
                gvViewTermOfCampaignDetail.Enabled = true;
            }
            else
            {
                gvViewTermOfCampaignDetail.Enabled = false;
            }

            gvViewTermOfCampaign.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value);
            gvViewTermOfCampaign.DataBind();
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;

                if (!String.IsNullOrEmpty(copyCampaignString))
                {
                    //CampaignString = copyCampaignString.ToString().Split('-');
                    string idCampaignLoad = copyCampaignString.Replace("-", "");//CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];

                    DataTable dt_ILCP02 = (campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value))?.Tables[0]?.Copy();
                    DataRow dr = dt_ILCP02.Rows[(gvViewTermOfCampaign.PageIndex * Convert.ToInt16(gvViewTermOfCampaign.PageSize)) + e.NewSelectedIndex];
                    lblSubSeqList.Text = "Cmp. Sub seq : " + dr["C02CSQ"];
                    SetTermProductDetailEnable(true);
                    BindDatagvViewTermOfCampaignDetail();

                    if (HdnIsEdit1.Value == "UPDATE_CAMPAIGN" || HdnIsEdit1.Value == "DELETE_CAMPAIGN")
                    {
                        SetTermProductDetailReadOnly();
                    }
                }
                else
                {
                    DataTable dt_ILCP02 = (campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value))?.Tables[0]?.Copy();
                    DataRow dr = dt_ILCP02.Rows[(gvViewTermOfCampaign.PageIndex * Convert.ToInt16(gvViewTermOfCampaign.PageSize)) + e.NewSelectedIndex];
                    lblSubSeqList.Text = "Cmp. Sub seq : " + dr["C02CSQ"];
                    SetTermProductDetailEnable(true);
                    BindDatagvViewTermOfCampaignDetail();

                    if (HdnIsEdit1.Value == "UPDATE_CAMPAIGN" || HdnIsEdit1.Value == "DELETE_CAMPAIGN")
                    {
                        SetTermProductDetailReadOnly();
                    }
                }
            }
            catch (Exception ex)
            {
                SetTermProductDetailEnable(false);
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }
        protected void ViewTermOfCampaignBindData()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtTermOfCampaign());

                ds_term_of_campaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                if (campaignStorage.check_dataset(ds))
                {
                    gvViewTermOfCampaign.DataSource = ds;
                    gvViewTermOfCampaign.DataBind();
                }
                gvViewTermOfCampaignDetail.DataSource = dtTermOfCampaignDetail();
                gvViewTermOfCampaignDetail.DataBind();

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }


        protected void gvViewTermOfCampaignSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value);
            DataRow dr = ds?.Tables[0]?.Rows[(gvViewTermOfCampaign.PageIndex * Convert.ToInt16(gvViewTermOfCampaign.PageSize)) + e.NewSelectedIndex];
        }

        protected void gvViewTermOfCampaignDetailPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewTermOfCampaignDetail.PageIndex = e.NewPageIndex;
            gvViewTermOfCampaignDetail.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign_detail.Value);
            gvViewTermOfCampaignDetail.DataBind();
        }

        protected void gvViewTermOfCampaignDetailSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string price = gvViewTermOfCampaignDetail.Rows[e.NewSelectedIndex].Cells[6].Text;
            string subSeq = gvViewTermOfCampaignDetail.Rows[e.NewSelectedIndex].Cells[1].Text;
            string code = gvViewTermOfCampaignDetail.Rows[e.NewSelectedIndex].Cells[4].Text;
            //DataRow dr = dt.Rows[(gvViewTermOfCampaignDetail.PageIndex * Convert.ToInt16(gvViewTermOfCampaignDetail.PageSize)) + e.NewSelectedIndex];
            string[] splitePrice = price.Split('-');//dr["PRICE"].ToString().Split('-');
            int checkSplit = splitePrice.Length;
            if (checkSplit == 2)
            {
                minPriceChange.Text = splitePrice[0];
                maxPriceChange.Text = splitePrice[1];
            }
            else
            {
                minPriceChange.Text = price;//dr["PRICE"].ToString();
                maxPriceChange.Text = "";
            }

            Globals.productItemID = code;//dr["CODE"].ToString();
            Globals.productItemSubSeq = subSeq;//dr["SUBSEQ"].ToString();
            popupChangePrice.ShowOnPageLoad = true;
        }
        protected void CLICK_CONFIRM_SAVE_PRICE(object sender, EventArgs e)
        {
            string subseqfromlist = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");
            double txtPriceMins = 0.00;
            double txtPriceMaxs = 0.00;
            DataTable dtAddItemProduct = dtTermOfCampaignDetail();
            DataTable dt_listProductItems = new DataTable();
            if (!string.IsNullOrEmpty(ds_AddItemProduct.Value))
            {
                dt_listProductItems = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
            }
            if (!string.IsNullOrEmpty(ds_AddItemProduct.Value))
            {
                dtAddItemProduct = (campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value)).Copy();
            }
            if (!String.IsNullOrEmpty(minPriceChange.Text.ToString()))
            {
                txtPriceMins = double.Parse(minPriceChange.Text.ToString());
            }
            if (!String.IsNullOrEmpty(maxPriceChange.Text.ToString()))
            {
                txtPriceMaxs = double.Parse(maxPriceChange.Text.ToString());
            }

            if (minPriceChange.Text == "" && maxPriceChange.Text == "")
            {
                lblMsgAlert.Text = "ท่านไม่ได้ระบุราคาสินค้า !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (minPriceChange.Text == "" && maxPriceChange.Text != "")
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (minPriceChange.Text != "" && maxPriceChange.Text == "")
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (txtPriceMins >= txtPriceMaxs && txtPriceMaxs != 0.00)
            {
                lblMsgAlert.Text = "ท่านระบุราคาสินค้าไม่ถูกต้อง !";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else
            {
                if ((criteriaSubSeq.SelectedValue == "allSeq") && (criteriaProduct.SelectedValue == "allProducts"))
                {
                    foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                    {
                        dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");


                        //if (minPriceChange.Text != "" && maxPriceChange.Text == "")
                        //{
                        //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00");
                        //}
                        //else
                        //{
                        //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");
                        //}

                    }
                }
                else if ((criteriaSubSeq.SelectedValue == "allSeq") && (criteriaProduct.SelectedValue == "thisProducts"))
                {
                    foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                    {
                        if (dr_listProduct["CODE"].ToString() == Globals.productItemID)
                        {
                            dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");

                            //if (minPriceChange.Text != "" && maxPriceChange.Text == "")
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00");
                            //}
                            //else
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");
                            //}
                        }
                    }
                }
                else if ((criteriaSubSeq.SelectedValue == "onlySeq") && (criteriaProduct.SelectedValue == "allProducts"))
                {
                    foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                    {
                        if (dr_listProduct["SUBSEQ"].ToString() == Globals.productItemSubSeq)
                        {
                            dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");

                            //if (minPriceChange.Text != "" && maxPriceChange.Text == "")
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00");
                            //}
                            //else
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");
                            //}
                        }
                    }

                }
                else if ((criteriaSubSeq.SelectedValue == "onlySeq") && (criteriaProduct.SelectedValue == "thisProducts"))
                {
                    foreach (DataRow dr_listProduct in dt_listProductItems.Rows)
                    {
                        if (dr_listProduct["CODE"].ToString() == Globals.productItemID && dr_listProduct["SUBSEQ"].ToString() == Globals.productItemSubSeq)
                        {
                            dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");

                            //if (minPriceChange.Text != "" && maxPriceChange.Text == "")
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00");
                            //}
                            //else
                            //{
                            //    dr_listProduct["PRICE"] = txtPriceMins.ToString("#,##0.00") + " - " + txtPriceMaxs.ToString("#,##0.00");
                            //}
                        }
                    }

                }

                if (gvViewTermOfCampaign.Rows.Count > 1)
                {
                    DataTable dtview = dt_listProductItems.Select("SubSeq = " + subseqfromlist).CopyToDataTable();
                    gvViewTermOfCampaignDetail.DataSource = dtview;
                    gvViewTermOfCampaignDetail.DataBind();
                }
                else
                {
                    gvViewTermOfCampaignDetail.DataSource = dt_listProductItems;
                    gvViewTermOfCampaignDetail.DataBind();
                }
                ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt_listProductItems);

            }

        }
        #endregion

        #region Tab Fix Installment
        protected void TabFixInstallmentBindData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Installment");
            dt.Columns.Add("PrincipleAMT");
            dt.Columns.Add("InstallmentTerm");
            dt.Columns.Add("AdjustmentTerm");
            dt.Columns.Add("CustomerInterestRate");
            dt.Columns.Add("CustomerCreditUsage");
            dt.Columns.Add("InterestRateMaker");
            dt.Columns.Add("TotalRate");

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            campaignStorage.SetCookiesDataTableByName("ds_installment", dt);
            gvInstallment.DataSource = ds;
            gvInstallment.DataBind();

            dt = new DataTable();
            dt.Columns.Add("Code");
            dt.Columns.Add("ItemName");
            ds = new DataSet();
            ds.Tables.Add(dt);
            campaignStorage.SetCookiesDataTableByName("ds_product_list", dt);
            gvProductList.DataSource = ds;
            gvProductList.DataBind();
        }

        protected void gvInstallmentPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInstallment.PageIndex = e.NewPageIndex;
            gvInstallment.DataSource = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_installment");
            gvInstallment.DataBind();
        }

        protected void gvProductListPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductList.PageIndex = e.NewPageIndex;
            DataSet dataSet = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_list");
            if (!campaignStorage.check_dataset(dataSet))
            {
                lblMsgAlertApp.Text = "Error please click again !";
                PopupAlertApp.ShowOnPageLoad = true;
                return;
            }
            gvProductList.DataSource = dataSet;
            gvProductList.DataBind();
        }
        #endregion

        #region Tab Vendor List

        protected void btnVendorListSearch_Click(object sender, EventArgs e)
        {

            popupSearchVendor.ShowOnPageLoad = true;
            popupSearchVendor.Focus();
            LoaddataFromgvselectlistvendor_2();
        }
        protected void popupSearchVendor_WindowCallback(object source, DevExpress.Web.ASPxPopupControl.PopupWindowCallbackArgs e)
        {
            BinddataSearchgvVendorList();
        }

        protected void BinddataSearchgvVendorList()
        {
            DataSet ds_gv = new DataSet();
            DataSet check_ds_vendor_list = new DataSet();
            DataSet check_ds_gv_selectlistvendor = new DataSet();
            //ds_gv = campaignStorage.GetCookMaxLargeCookie("ds_gv_selectlistvendor");
            ds_gv = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistvendor.Value);
            //check_ds_vendor_list = campaignStorage.GetCookiesDataSetByKey("ds_vendor_list");
            check_ds_vendor_list = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
            DataSet ds_hider_vendorlist = new DataSet();
            check_ds_gv_selectlistvendor = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistvendor.Value);
            ds_hider_vendorlist = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
            if (campaignStorage.check_dataset(check_ds_gv_selectlistvendor))
            {
                var countlistVendor = 0;
                var countListSelectVendor = 0;
                if (campaignStorage.check_dataset(check_ds_gv_selectlistvendor))
                {
                    countlistVendor = check_ds_gv_selectlistvendor.Tables[0].Rows.Count;
                }
                if (campaignStorage.check_dataset(ds_gv))
                {
                    countListSelectVendor = ds_gv.Tables[0].Rows.Count;
                }

                if (campaignStorage.check_dataset(check_ds_gv_selectlistvendor) && countlistVendor >= countListSelectVendor)
                {
                    DataTable dt = check_ds_gv_selectlistvendor.Tables[0]?.Copy();

                    if (dt.Columns["P16RNK"] == null) { dt.Columns.Add("P16RNK"); }
                    if (dt.Columns["P12ODR"] == null) { dt.Columns.Add("P12ODR"); }
                    if (dt.Columns["P12WOR"] == null) { dt.Columns.Add("P12WOR"); }
                    if (dt.Columns["P16END"] == null) { dt.Columns.Add("P16END"); }

                    string vendorlist = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        vendorlist += dr["P10VEN"] + ",";
                    }

                    vendorlist = vendorlist.TrimEnd(',');

                    string sql = $@"SELECT DISTINCT P10VEN as C08VEN, P10NAM,P16RNK,P12ODR,P12WOR,P16END FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                                 LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN
                                 LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN AND { _updateDate } BETWEEN P16STD AND P16END 
                                 WHERE  P10VEN IN ({vendorlist})";

                    m_userInfo = userInfoService.GetUserInfo();

                    ilObj.UserInfomation = m_userInfo;
                    dataCenter = new DataCenter(m_userInfo);
                    DataSet ds = new DataSet();
                    ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (campaignStorage.check_dataset(ds))
                    {
                        //campaignStorage.SetCookiesDataSetByName("ds_vendor_list", ds);
                        ds_vendor_list.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                        gvVendorList.DataSource = ds;
                        gvVendorList.DataBind();
                    }
                }
                else
                {
                    DataTable dt = ds_gv.Tables[0]?.Copy();

                    if (dt.Columns["P16RNK"] == null) { dt.Columns.Add("P16RNK"); }
                    if (dt.Columns["P12ODR"] == null) { dt.Columns.Add("P12ODR"); }
                    if (dt.Columns["P12WOR"] == null) { dt.Columns.Add("P12WOR"); }
                    if (dt.Columns["P16END"] == null) { dt.Columns.Add("P16END"); }

                    string vendorlist = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        vendorlist += dr["P10VEN"] + ",";
                    }

                    vendorlist = vendorlist.TrimEnd(',');

                    string sql = $@"SELECT DISTINCT P10VEN as C08VEN, P10NAM,P16RNK,P12ODR,P12WOR,P16END FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                                 LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN
                                 LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN AND { _updateDate } BETWEEN P16STD AND P16END 
                                 WHERE P10VEN IN ({vendorlist})";

                    m_userInfo = userInfoService.GetUserInfo();

                    ilObj.UserInfomation = m_userInfo;
                    dataCenter = new DataCenter(m_userInfo);
                    DataSet ds = new DataSet();
                    ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (campaignStorage.check_dataset(ds))
                    {
                        //campaignStorage.SetCookiesDataSetByName("ds_vendor_list", ds);
                        ds_vendor_list.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                        gvVendorList.DataSource = ds;
                        gvVendorList.DataBind();
                    }
                }

            }
            else if ((campaignStorage.check_dataset(check_ds_vendor_list) || campaignStorage.check_dataset(ds_hider_vendorlist)) && string.IsNullOrEmpty(ds_gv_selectlistvendor.Value))
            {
                DataSet ds = new DataSet();
                if (campaignStorage.check_dataset(ds_hider_vendorlist))
                {
                    //dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_vendor_list.Value);
                    DataTable dt = ds_hider_vendorlist.Tables[0].Copy();
                    dt.Rows.RemoveAt(0);
                    ds.Tables.Add(dt);
                    if(campaignStorage.check_dataset(ds))
                    {
                        gvVendorList.DataSource = ds;
                        gvVendorList.DataBind();
                    }
                }
                else
                {
                    gvVendorList.DataSource = check_ds_vendor_list;
                    gvVendorList.DataBind();
                }
            }
        }

        protected void btnVendorListAdd_Click(object sender, EventArgs e)
        {
            BinddataSearchgvVendorList();
        }
        protected void GV_VENDOR_LIST_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (HdnIsEdit1.Value == "VIEW_CAMPAIGN")
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                }
                else
                {
                    if (gvVendorList.Rows.Count > 0)
                    {
                        GridViewRow gvVendorListRow = e.Row;
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            if (gvVendorListRow.Cells[4].Text.ToString().Trim() == "" || gvVendorListRow.Cells[4].Text.ToString().Trim() == null)
                            {
                                gvVendorListRow.Cells[4].Text = "0";
                            }
                            if (gvVendorListRow.Cells[5].Text.ToString().Trim() == "" || gvVendorListRow.Cells[5].Text.ToString().Trim() == null)
                            {
                                gvVendorListRow.Cells[5].Text = "0";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }
        protected void gvVendorList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        { 
            if (e.RowIndex != -1)
            {
                DataTable dt = new DataTable();
                string code = string.IsNullOrEmpty(e.Values["C08VEN"].ToString()) ? string.Empty : e.Values["C08VEN"].ToString();
                if (string.IsNullOrEmpty(code))
                {
                    lblMsgAlertApp.Text = "cann't find vendor code please click again ";
                    PopupAlertApp.ShowOnPageLoad = true;
                    return;
                }
                if (!string.IsNullOrEmpty(ds_gv_selectlistvendor.Value))
                {
                    dt = (campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistvendor.Value))?.Tables[0]?.Copy();
                }
                else
                {
                    //dt = ((DataSet)campaignStorage.GetCookMaxLargeCookie("ds_gv_selectlistvendor"))?.Tables[0]?.Copy();
                }

                var dtdelete = dt.AsEnumerable().Where(r => decimal.Parse(r.Field<string>("P10VEN")).ToString() != code).ToList();
                if (dtdelete.Count > 0)
                {
                    dt = dtdelete.CopyToDataTable();
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("P10VEN");
                    dt.Columns.Add("P10NAM");
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds_gv_selectlistvendor.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                //campaignStorage.SetCookMaxLargeCookie("ds_gv_selectlistvendor", ds);
                BinddataSearchgvVendorList();
            }
        }
        protected void TabVendorListBindData()
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;

                if (!String.IsNullOrEmpty(copyCampaignString))
                {
                    LOAD_DATA_ILCP08();
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("C08VEN");
                    dt.Columns.Add("P10NAM");
                    dt.Columns.Add("P16RNK");
                    dt.Columns.Add("P12ODR");
                    dt.Columns.Add("P12WOR");
                    dt.Columns.Add("P16END");

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    //campaignStorage.SetCookiesDataTableByName("ds_vendor_list", dt);
                    ds_vendor_list.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                    gvVendorList.DataSource = ds;
                    gvVendorList.DataBind();
                }
            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }

        protected void gvVendorListPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendorList.PageIndex = e.NewPageIndex;
            DataSet ds = new DataSet();
            ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
            if (campaignStorage.check_dataset(ds))
            {
                gvVendorList.DataSource = ds;
                gvVendorList.DataBind();
            }
        }
        #endregion

        #region Tab Banch | Note

        protected void BTN_CONFIRM_SAVE_NOTE(object sender, EventArgs e)
        {
            dataCenter = new DataCenter(m_userInfo);
            bool checkInsertILCP11 = false;
            INSERT_ILCP11(dataCenter, ref checkInsertILCP11);
            LOAD_DATA_ILCP11();
            hd_SaveNote.Value = "1";
            txtNote.Text = null;
        }
        protected void GV_BRANCH_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {


        }
        protected void BTN_SEARCH_MARKETING(object sender, EventArgs e)
        {
            LoadDataMarket();
        }
        protected void BTN_SEARCH_CLEAR_MARKETING(object sender, EventArgs e)
        {
            txt_Marketing.Text = "";
            txtMarketingCode.Text = "";
            LoadDataMarket();
        }
        private void LoadDataMarket()
        {
            try
            {
                string sqlwhere = "";
                if (ddl_SearchBy.SelectedValue == "DMC" && txt_Marketing.Text.Trim() != "")
                {
                    sqlwhere += " AND T74CDE = '" + txt_Marketing.Text.Trim() + "' ";
                }
                if (ddl_SearchBy.SelectedValue == "TMC" && txt_Marketing.Text.Trim() != "")
                {
                    sqlwhere += " AND T74NME LIKE '" + txt_Marketing.Text.ToUpper().Trim() + "%' ";
                }

                SqlAll = "SELECT T74CDE,T74NME FROM  AS400DB01.ILOD0001.ILTB74 WITH (NOLOCK) WHERE T74BRN = " + campaignStorage.GetCookiesStringByKey("branch") + " AND T74STS = '' ";

                SqlAll = SqlAll + sqlwhere + " ";
                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                campaignStorage.SetCookiesDataSetByName("ds_gvMarketingCode", DS);
                gvMarketingCode.SelectedIndex = -1;
                gvMarketingCode.DataSource = DS;
                gvMarketingCode.DataBind();

                if (gvBranch.Rows.Count == 0)
                {
                    dataCenter.CloseConnectSQL();
                    return;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        protected void GV_PAGING_MARKETING_CODE(object sender, GridViewPageEventArgs e)
        {
            gvMarketingCode.PageIndex = e.NewPageIndex;
            gvMarketingCode.DataSource = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvMarketingCode");
            gvMarketingCode.DataBind();
        }

        protected void GV_SELECT_MARKETING_CODE(object sender, GridViewSelectEventArgs e)
        {
            DataSet dsMarketingCode = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvMarketingCode");
            DataRow drMarketingCode = dsMarketingCode?.Tables[0]?.Rows[(gvProductBrand.PageIndex * Convert.ToInt16(gvProductBrand.PageSize)) + e.NewSelectedIndex];
            txtMarketingCode.Text = drMarketingCode[0].ToString().Trim();

            PopupMarketCode.ShowOnPageLoad = false;
        }
        protected void SELECT_MARKETING_CODE(object sender, EventArgs e)
        {
            LoadDataMarket();
            PopupMarketCode.ShowOnPageLoad = true;
        }
        protected void TabBanchAndNoteBindData()
        {
            try
            {
                SqlAll = "SELECT T1BRN,T1BNME FROM AS400DB01.ILOD0001.ILTB01 WITH (NOLOCK) ";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    ds_gridBanch.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                    gvBranch.SelectedIndex = -1;
                    gvBranch.DataSource = DS;
                    gvBranch.DataBind();
                }
                else
                {
                    dataCenter.CloseConnectSQL();
                    return;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }

        protected void btnSaveNoteClick(object sender, EventArgs e)
        {
            popupConfirmNote.ShowOnPageLoad = true;
        }

        protected void btnCancelNoteClick(object sender, EventArgs e)
        {
            txtNote.Text = null;
            LOAD_DATA_ILCP11();
        }

        protected void CheckBoxSelectAllProductTypeSelectChangedShareSub(object sender, EventArgs e)
        {
            CheckBox cbSelectAllShareSub = (CheckBox)gvApplicationType.HeaderRow.FindControl("cbSelectAllShareSub");

            foreach (GridViewRow row in gvApplicationType.Rows)
            {
                CheckBox CheckBoxInsertShareSub = (CheckBox)row.FindControl("CheckBoxInsertShareSub");

                if (cbSelectAllShareSub.Checked == true)
                {
                    CheckBoxInsertShareSub.Checked = true;
                }
                else
                {
                    CheckBoxInsertShareSub.Checked = false;
                }
            }
        }
        protected void CheckBoxSelectAllProductTypeSelectChanged(object sender, EventArgs e)
        {
            CheckBox cbSelectAll = (CheckBox)gvBranch.HeaderRow.FindControl("cbSelectAll");

            foreach (GridViewRow row in gvBranch.Rows)
            {
                CheckBox CheckBoxInsert = (CheckBox)row.FindControl("CheckBoxInsert");

                if (cbSelectAll.Checked == true)
                {
                    CheckBoxInsert.Checked = true;
                }
                else
                {
                    CheckBoxInsert.Checked = false;
                }
            }
        }
        #endregion

        #region Popup Load Data from Other Campaign
        protected void btnSearchVendorClick(object sender, EventArgs e)
        {
            hdfAddVendor.Value = "SEARCH_LOAD_CAMPAIGN";
            LoadVendorData();
            PopupAddVendor.ShowOnPageLoad = true;
        }
        #endregion

        #region Popup Add Maker Code
        #region Mockup Data for Test
        protected void LoadDataPopupAddMakerCode()
        {

        }
        #endregion

        protected void gvMakerCodePageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMakerCode.PageIndex = e.NewPageIndex;
            gvMakerCode.DataSource = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_popup_maker");
            gvMakerCode.DataBind();

        }

        protected void gvMakerCodeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

            DataSet ds_grid = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_popup_maker");
            DataRow dr = ds_grid?.Tables[0]?.Rows[(gvMakerCode.PageIndex * Convert.ToInt16(gvMakerCode.PageSize)) + e.NewSelectedIndex];

            string addMaker = hdfAddMaker.Value;
            switch (addMaker)
            {
                case "FORM_CREATE_CAMPAIGN":
                    txtMakerCode.Text = string.Format("{0:000000000000}", Int64.Parse(dr["T46MAK"].ToString().Trim()));
                    break;
                default: break;
            }

            PopupAddMakerCode.ShowOnPageLoad = false;
            ResetGrid(gvMakerCode, "ds_popup_maker");

            CampaignTypeVendorUpdategvMultiRate("Maker", txtMakerCode.Text);
        }
        #endregion

        #region Popup Add Vendor

        protected void GV_VENDOR_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvVendor = e.Row;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    gvVendor.Cells[2].Text = string.Format("{0:000000000000}", Int64.Parse(gvVendor.Cells[2].Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }

        private void LoadVendorData()
        {
            string sqlWhere = string.Empty;
            string sql = @"SELECT
	                        DISTINCT P11VEN,
	                        P10NAM,
 	                        T00LTY,
 	                        T00TNM
                        FROM
	                        AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                        LEFT JOIN AS400DB01.ILOD0001.ILMS11 WITH (NOLOCK)
                        ON
	                        P10VEN = P11VEN
                        LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                        ON
	                        P11LTY = T00LTY
                        WHERE 
	                        P11DEL = '' ";

            string searchText = txtPopupAddVendorSearchText.Text.Trim().Replace("'", "''");
            string searchBy = ddlPopupAddVendorSearchBy.SelectedValue;

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "VC":
                        decimal searchTextint = decimal.Parse(searchText);
                        sqlWhere = " AND CAST(P10VEN AS VARCHAR) = '" + searchTextint + "'";
                        break;
                    case "VD":
                        sqlWhere = " AND (P10NAM LIKE '%" + searchText.ToUpper() + "%') ";
                        //sqlWhere = " AND (P10TNM LIKE '%' || @P10TNM || '%') ";
                        //cmd.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("P10TNM", searchText);

                        break;
                    default: break;
                }
            }

            sql = sql + sqlWhere;
            cmd.CommandText = sql;
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

            ds_popup_vendor.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (campaignStorage.check_dataset(DS))
            {
                gvVendor.DataSource = DS;
                gvVendor.DataBind();
            }
            dataCenter.CloseConnectSQL();
        }

        protected void gvVendorPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendor.PageIndex = e.NewPageIndex;
            //gvVendor.DataSource = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_popup_vendor");
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_vendor.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gvVendor.DataSource = DS;
                gvVendor.DataBind();
            }
        }

        protected void gvVendorSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //DataSet ds_grid = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_popup_vendor");
            DataSet ds_grid = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_vendor.Value);
            DataRow dr = ds_grid?.Tables[0]?.Rows[(gvVendor.PageIndex * Convert.ToInt16(gvVendor.PageSize)) + e.NewSelectedIndex];

            string addVendor = hdfAddVendor.Value;
            switch (addVendor)
            {
                case "FORM_CREATE_CAMPAIGN":
                    txtVendorCode.Text = string.Format("{0:000000000000}", Int64.Parse(dr["P11VEN"].ToString().Trim()));
                    hdfLoanType.Value = dr["T00LTY"].ToString().Trim();
                    break;
                case "SEARCH_LOAD_CAMPAIGN":
                    txtSearhVendorCode.Text = string.Format("{0:000000000000}", Int64.Parse(dr["P11VEN"].ToString().Trim()));
                    txtSearchVendorName.Text = dr["P10NAM"].ToString().Trim();
                    break;
                default: break;
            }

            PopupAddVendor.ShowOnPageLoad = false;
            ResetGrid(gvVendor, ds_popup_vendor.Value);

            CampaignTypeVendorUpdategvMultiRate("Vendor", txtVendorCode.Text);
        }

        private void ResetGrid(GridView GridView, string ds)
        {
            GridView.PageIndex = 0;
            if (ds == "ds_popup_maker")
            {
                GridView.DataSource = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds");
            }
            else
            {
                GridView.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds);
            }
            GridView.DataBind();
        }

        protected void btnPopupAddVendorSearchClick(object sender, EventArgs e)
        {
            LoadVendorData();
        }

        protected void btnPopupAddVendorClearClick(object sender, EventArgs e)
        {
            ddlPopupAddVendorSearchBy.SelectedIndex = 0;
            txtPopupAddVendorSearchText.Text = "";
            LoadVendorData();
        }
        #endregion

        #region Popup Search Campaign
        protected void SearchCampaign()
        {

        }

        protected void gvSearchCampaignPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gvSearchCampaignSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
        }

        protected void btnSearchCampaign_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearchCampaignClear_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Popup Search Product Type
        protected void btnSearchProductTypeClick(object sender, EventArgs e)
        {
            PopupAddProductType.ShowOnPageLoad = true;
            LoadProductTypeData();
        }

        protected void LoadProductTypeData()
        {
            string sqlWhere = "";
            string loanType = hdfLoanType.Value;
            string searchBy = ddlPopupAddProductTypeSearchBy.SelectedValue;
            string searchText = txtPopupAddProductTypeSearchText.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "PTC":
                        sqlWhere = " AND T40TYP = " + searchText;
                        break;
                    case "PTD":
                        sqlWhere = " AND UPPER(T40DES) LIKE '" + searchText.ToUpper() + "%'";
                        break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(loanType))
            {
                sqlWhere += " AND T40LTY = '" + loanType + "'";
            }

            string sql = @"SELECT
                           T40LTY,
                           T00TNM,
                           T40TYP,
                           T40DES
                       FROM
                           AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                       LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                       ON  T40LTY = T00LTY
                       WHERE
                           T40DEL = ''";

            sql = sql + sqlWhere + " ORDER BY T40TYP ASC ";

            DataSet dsProductType = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductType = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_product_type", dsProductType);
            if (campaignStorage.check_dataset(dsProductType))
            {
                gvProductType.DataSource = dsProductType;
                gvProductType.DataBind();
            }
        }

        protected void gvProductTypePageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductType.PageIndex = e.NewPageIndex;
            DataSet DS = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_type");
            if (campaignStorage.check_dataset(DS))
            {
                gvProductType.DataSource = DS;
                gvProductType.DataBind();
            }
        }

        protected void gvProductTypeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet dsProductType = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_type");
            DataRow drProductType = dsProductType?.Tables[0]?.Rows[(gvProductType.PageIndex * Convert.ToInt16(gvProductType.PageSize)) + e.NewSelectedIndex];

            txtSearchProductTypeCode.Text = drProductType[2].ToString().Trim();
            txtSearchProductTypeName.Text = drProductType[3].ToString().Trim();
            PopupAddProductType.ShowOnPageLoad = false;
        }

        protected void btnPopupAddProductTypeSearchClick(object sender, EventArgs e)
        {
            LoadProductTypeData();
        }

        protected void btnPopupAddProductTypeClearClick(object sender, EventArgs e)
        {
            ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
            txtPopupAddProductTypeSearchText.Text = "";
            LoadProductTypeData();
        }
        #endregion

        #region Popup Search Product Code
        protected void btnSearchProductCodeClick(object sender, EventArgs e)
        {
            PopupAddProductCode.ShowOnPageLoad = true;
            LoadProductCodeData();
        }
        protected void LoadProductCodeData()
        {
            string sqlWhere = "";
            string loanType = hdfLoanType.Value;
            string productTypeCode = txtSearchProductTypeCode.Text;
            string searchBy = ddlPopupAddProductCodeSearchBy.SelectedValue;
            string searchText = txtPopupAddProductCodeSearchText.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "PT":
                        sqlWhere = " AND T41TYP = " + searchText;
                        break;
                    case "PC":
                        sqlWhere = " AND T41COD = " + searchText;
                        break;
                    case "PD":
                        sqlWhere = " AND T41DES LIKE '" + searchText.ToUpper() + "%'";
                        break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(loanType))
            {
                sqlWhere += " AND TB41.T41LTY = '" + loanType + "'";
            }

            if (!string.IsNullOrEmpty(productTypeCode))
            {
                sqlWhere += " AND TB41.T41TYP =  " + productTypeCode;
            }

            string sql = @"SELECT
                           TB41.T41LTY,
                           T00TNM,
                           TB41.T41TYP,
                           TB40.T40DES,
                           TB41.T41COD,
                           TB41.T41DES,
                           TB40.T40LTY  
                       FROM
                           AS400DB01.ILOD0001.ILTB41 AS TB41 WITH (NOLOCK)
                       LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                       ON  T41LTY = T00LTY 
                       LEFT JOIN AS400DB01.ILOD0001.ILTB40 AS TB40 WITH (NOLOCK)
                       ON  TB40.T40TYP = TB41.T41TYP
                       WHERE
                           TB41.T41DEL = ''";

            sql = sql + sqlWhere;

            DataSet dsProductCode = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductCode = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_product_code", dsProductCode);
            if (campaignStorage.check_dataset(dsProductCode))
            {
                gvProductCode.DataSource = dsProductCode;
                gvProductCode.DataBind();
            }
        }

        protected void gvProductCodePageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductCode.PageIndex = e.NewPageIndex;
            DataSet DS = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_code");
            if (campaignStorage.check_dataset(DS))
            {
                gvProductCode.DataSource = DS;
                gvProductCode.DataBind();
            }
        }

        protected void gvProductCodeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet dsProductCode = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_code");
            DataRow drProductCode = dsProductCode?.Tables[0]?.Rows[(gvProductCode.PageIndex * Convert.ToInt16(gvProductCode.PageSize)) + e.NewSelectedIndex];

            txtSearchProductCode.Text = drProductCode[4].ToString().Trim();
            txtSearchProductName.Text = drProductCode[5].ToString().Trim();

            PopupAddProductCode.ShowOnPageLoad = false;
        }

        protected void btnPopupAddProductCodeSearchClick(object sender, EventArgs e)
        {
            LoadProductCodeData();
        }

        protected void btnPopupAddProductCodeClearClick(object sender, EventArgs e)
        {
            ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
            txtPopupAddProductCodeSearchText.Text = "";
            LoadProductCodeData();
        }
        #endregion

        #region Popup Search Product Brand
        protected void btnSearchProductBrandClick(object sender, EventArgs e)
        {
            LoadProductBrandData();
            PopupAddProductBrand.ShowOnPageLoad = true;
        }

        protected void LoadProductBrandData()
        {
            string sqlWhere = "";
            string searchBy = ddlPopupAddProductBrandSearchBy.SelectedValue;
            string searchText = txtPopupAddProductBrandSearchText.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "PBC":
                        sqlWhere = " AND T42BRD = '" + searchText + "'";
                        break;
                    case "PBN":
                        sqlWhere = " AND UPPER(T42DES) LIKE '" + searchText.ToUpper() + "%'";
                        break;
                    default: break;
                }
            }

            string sql = @"SELECT
                           T42BRD,
                           T42DES
                       FROM
                           AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)
                       WHERE
                           T42DEL = ''";

            sql = sql + sqlWhere + " ORDER BY T42BRD ASC ";

            DataSet dsProductBrand = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductBrand = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_product_brand", dsProductBrand);
            if (campaignStorage.check_dataset(dsProductBrand))
            {
                gvProductBrand.DataSource = dsProductBrand;
                gvProductBrand.DataBind();
            }
        }

        protected void gvProductBrandPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductBrand.PageIndex = e.NewPageIndex;
            DataSet DS = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_brand");
            if (campaignStorage.check_dataset(DS))
            {
                gvProductBrand.DataSource = DS;
                gvProductBrand.DataBind();
            }
        }

        protected void gvProductBrandSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet dsProductBrand = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_brand");
            DataRow drProductBrand = dsProductBrand?.Tables[0]?.Rows[(gvProductBrand.PageIndex * Convert.ToInt16(gvProductBrand.PageSize)) + e.NewSelectedIndex];

            txtSearchProductBarandCode.Text = drProductBrand[0].ToString().Trim();
            txtSearchProductBrandName.Text = drProductBrand[1].ToString().Trim();

            PopupAddProductBrand.ShowOnPageLoad = false;
        }

        protected void btnPopupAddProductBrandSearchClick(object sender, EventArgs e)
        {
            LoadProductBrandData();
        }

        protected void btnPopupAddProductBrandClearClick(object sender, EventArgs e)
        {
            ddlPopupAddProductBrandSearchBy.SelectedIndex = 0;
            txtPopupAddProductBrandSearchText.Text = "";
            LoadProductBrandData();
        }
        #endregion

        #region Popup Search Product Model
        protected void btnSearchProductModelClick(object sender, EventArgs e)
        {
            LoadProductModelData();
            PopupAddProductModel.ShowOnPageLoad = true;
        }

        protected void LoadProductModelData()
        {
            string sqlWhere = "";
            string loanType = hdfLoanType.Value;
            string productTypeCode = txtSearchProductTypeCode.Text;
            string productCode = txtSearchProductCode.Text;
            string productBrand = txtSearchProductBarandCode.Text;

            string searchBy = ddlPopupAddProductModelSearchBy.SelectedValue;
            string searchText = txtPopupAddProductModelSearchText.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "PT":
                        sqlWhere = " AND T43TYP = " + searchText;
                        break;
                    case "BC":
                        sqlWhere = " AND T43BRD = " + searchText;
                        break;
                    case "PC":
                        sqlWhere = " AND T43COD = " + searchText;
                        break;
                    case "MC":
                        sqlWhere = " AND T43MDL = " + searchText;
                        break;
                    case "MD":
                        sqlWhere = " AND T43DES LIKE '" + searchText.ToUpper() + "%'";
                        break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(loanType))
            {
                sqlWhere += " AND T43LTY = '" + loanType + "'";
            }

            if (!string.IsNullOrEmpty(productTypeCode))
            {
                sqlWhere += " AND T43TYP = " + productTypeCode;
            }

            if (!string.IsNullOrEmpty(productCode))
            {
                sqlWhere += " AND T43COD = " + productCode;
            }

            if (!string.IsNullOrEmpty(productBrand))
            {
                sqlWhere += " AND T43BRD = " + productBrand;
            }

            string sql = @"SELECT
                           DISTINCT T43LTY,
                           T00TNM,
                           T43TYP,
                           T40DES,
                           T43BRD,
                           T42DES,
                           T43COD,
                           T41DES,
                           T43MDL,
                           T43DES,
                           T43RSV,
                           T43DEL
                       FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                       LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                       ON  T43LTY = T00LTY
                       LEFT JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                       ON  T43LTY = T40LTY
                       AND T43TYP = T40TYP
                       LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK)
                       ON  T43LTY = T41LTY
                       AND T43TYP = T41TYP
                       AND T43COD = T41COD
                       LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)
                       ON  T43BRD = T42BRD 
                       WHERE
                           T43DEL = ''
                       AND T40DEL = '' ";

            sql = sql + sqlWhere;

            DataSet dsProductModel = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsProductModel = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_product_model", dsProductModel);
            if (campaignStorage.check_dataset(dsProductModel))
            {
                gvProductModel.DataSource = dsProductModel;
                gvProductModel.DataBind();
            }
        }

        protected void gvProductModelPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductModel.PageIndex = e.NewPageIndex;
            DataSet DS = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_model");
            if (campaignStorage.check_dataset(DS))
            {
                gvProductModel.DataSource = DS;
                gvProductModel.DataBind();
            }
        }

        protected void gvProductModelSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet dsProductModel = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_product_model");
            DataRow drProductModel = dsProductModel?.Tables[0]?.Rows[(gvProductModel.PageIndex * Convert.ToInt16(gvProductModel.PageSize)) + e.NewSelectedIndex];

            txtSearchProductModelCode.Text = drProductModel[8].ToString().Trim();
            txtSearchProductModelName.Text = drProductModel[9].ToString().Trim();

            PopupAddProductModel.ShowOnPageLoad = false;
        }

        protected void btnPopupAddProductModelSearchClick(object sender, EventArgs e)
        {
            LoadProductModelData();
        }

        protected void btnPopupAddProductModelClearClick(object sender, EventArgs e)
        {
            ddlPopupAddProductModelSearchBy.SelectedIndex = 0;
            txtPopupAddProductModelSearchText.Text = "";
            LoadProductModelData();
        }
        #endregion

        #region Validation_Convert
        private string ConvertTo2Decimal(string input)
        {
            input = String.IsNullOrEmpty(input.Trim()) ? "0" : input.Trim();
            decimal value;

            if (Decimal.TryParse(input, out value))
            {
                return value.ToString("N2");
            }
            else
            {
                return "0.00";
            }
        }

        private string ConvertToInteger(string input)
        {
            input = String.IsNullOrEmpty(input.Trim()) ? "0" : input.Trim().Replace(".", "");
            int value;

            if (Int32.TryParse(input, out value))
            {
                return value.ToString();
            }
            else
            {
                return "0";
            }
        }

        private string DateAdd(string inputDate, int addDays)
        {
            DateTime sumDate = Convert.ToDateTime(inputDate);
            string outputDate = sumDate.AddDays(addDays).ToString("dd/MM/yyyy");

            //string outputDate = sumDate.ToString("dd/MM/yyyy");

            return outputDate;
        }
        #endregion

        #region Get Method
        private DataTable GetDataFromGridview(GridView gvInput, DataTable dtInput)
        {
            DataTable dtOutput = dtInput;

            if (gvInput.Columns.Count == dtInput.Columns.Count)
            {
                foreach (GridViewRow gvRow in gvInput.Rows)
                {
                    DataRow dr = dtOutput.NewRow();
                    for (int i = 0; i < dtOutput.Columns.Count; i++)
                    {
                        dr[i] = Server.HtmlDecode(gvRow.Cells[i].Text.Trim());
                    }

                    dtOutput.Rows.Add(dr);
                }
            }
            else if (gvInput.ID == "gvPartyRate")
            {
                foreach (GridViewRow gvRow in gvInput.Rows)
                {
                    DataRow dr = dtOutput.NewRow();
                    for (int i = 0; i < dtOutput.Columns.Count; i++)
                    {
                        dr[i] = Server.HtmlDecode(gvRow.Cells[i + 1].Text.Trim());
                    }

                    dtOutput.Rows.Add(dr);
                }
            }

            return dtOutput;
        }

        private int GetColumnIndexByName(GridView grid, string name)
        {
            foreach (DataControlField col in grid.Columns)
            {
                if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
                {
                    return grid.Columns.IndexOf(col);
                }
            }

            return -1;
        }
        #endregion

        #region Database Model
        private class ILCP01Model
        {
            public int C01CMP { get; set; }
            public string C01LTY { get; set; }
            public int C01BRN { get; set; }
            public string C01STY { get; set; }
            public string C01SBT { get; set; }
            public string C01CTY { get; set; }
            public string C01RNG { get; set; }
            public string C01CNM { get; set; }
            public int C01VDC { get; set; }
            public int C01MKC { get; set; }
            public string C01PTY { get; set; }
            public int C01SDT { get; set; }
            public int C01EDT { get; set; }
            public int C01CAT { get; set; }
            public int C01CLD { get; set; }
            public int C01NXD { get; set; }
            public int C01FIN { get; set; }
            public double C01SRT { get; set; }
            public int C01TRG { get; set; }
            public string C01CST { get; set; }
            public int C01INV { get; set; }
            public string C01MKT { get; set; }
            public string C01WDT { get; set; }
            public int C01UDT { get; set; }
            public int C01UTM { get; set; }
            public string C01UUS { get; set; }
            public string C01UPG { get; set; }
            public string C01UWS { get; set; }
            public string C01RST { get; set; }
            public string C01VTY { get; set; }
            public int C01VCR { get; set; }
            public string C01PMT { get; set; }
        }
        private class ILCP02Model
        {
            public int C02CMP { get; set; }
            public int C02CSQ { get; set; }
            public int C02RSQ { get; set; }
            public int C02FMT { get; set; }
            public int C02TOT { get; set; }
            public double C02AIR { get; set; }
            public double C02ACR { get; set; }
            public double C02INR { get; set; }
            public double C02CRR { get; set; }
            public double C02IFR { get; set; }
            public double C02INS { get; set; }
            public double C02SPR { get; set; }
            public double C02EPR { get; set; }
            public int C02TTR { get; set; }
            public int C02TTM { get; set; }
            public int C02UDT { get; set; }
            public int C02UTM { get; set; }
            public string C02UUS { get; set; }
            public string C02UPG { get; set; }
            public string C02UWS { get; set; }
            public string C02RST { get; set; }
        }
        private class ILCP04Model
        {
            public int C04CMP { get; set; }
            public int C04PTY { get; set; }
            public int C04PCD { get; set; }
            public int C04PIT { get; set; }
            public int C04UDT { get; set; }
            public int C04UTM { get; set; }
            public string C04UUS { get; set; }
            public string C04UPG { get; set; }
            public string C04UWS { get; set; }
            public string C04RST { get; set; }
        }
        private class ILCP05Model
        {
            public int C05CMP { get; set; }
            public int C05CSQ { get; set; }
            public int C05RSQ { get; set; }
            public string C05PAR { get; set; }
            public int C05PCD { get; set; }
            public string C05SBT { get; set; }
            public double C05SIR { get; set; }
            public double C05SCR { get; set; }
            public double C05SFR { get; set; }
            public double C05STR { get; set; }
            public int C05SFM { get; set; }
            public int C05STO { get; set; }
            public int C05SST { get; set; }
            public int C05EST { get; set; }
            public int C05STM { get; set; }
            public int C05UDT { get; set; }
            public int C05UTM { get; set; }
            public string C05UUS { get; set; }
            public string C05UPG { get; set; }
            public string C05UWS { get; set; }
            public string C05RST { get; set; }
        }
        private class ILCP06Model
        {
            public int C06CMP { get; set; }
            public string C06APT { get; set; }
            public int C06UDT { get; set; }
            public int C06UTM { get; set; }
            public string C06UUS { get; set; }
            public string C06UPG { get; set; }
            public string C06UWS { get; set; }
            public string C0RST { get; set; }
        }
        private class ILCP07Model
        {
            public int C07CMP { get; set; }
            public int C07CSQ { get; set; }
            public string C07LNT { get; set; }
            public int C07PIT { get; set; }
            public string C07FIX { get; set; }
            public double C07PRC { get; set; }
            public double C07MIN { get; set; }
            public double C07MAX { get; set; }
            public double C07DOW { get; set; }
            public int C07UDT { get; set; }
            public int C07UTM { get; set; }
            public string C07UUS { get; set; }
            public string C07UPG { get; set; }
            public string C07UWS { get; set; }
            public string C07RST { get; set; }
        }
        private class ILCP08Model
        {
            public int C08CMP { get; set; }
            public int C08VEN { get; set; }
            public int C08UDT { get; set; }
            public int C08UTM { get; set; }
            public string C08UUS { get; set; }
            public string C08UPG { get; set; }
            public string C08UWS { get; set; }
            public string C08RST { get; set; }
        }
        private class ILCP09Model
        {
            public int C09CMP { get; set; }
            public int C09BRN { get; set; }
            public int C09UDT { get; set; }
            public int C09UTM { get; set; }
            public string C09UUS { get; set; }
            public string C09UPG { get; set; }
            public string C09UWS { get; set; }
            public string C09RST { get; set; }
        }
        private class ILCP11Model
        {
            public string C11CMP { get; set; }
            public int C11NSQ { get; set; }
            public int C11LSQ { get; set; }
            public string C11NOT { get; set; }
            public int C11UDT { get; set; }
            public int C11UTM { get; set; }
            public string C11UUS { get; set; }
            public string C11UPG { get; set; }
            public string C11UWS { get; set; }
            public string C11RST { get; set; }
        }
        private class ILCP99Model
        {
            public int C99CMP { get; set; }
            public int C99BRN { get; set; }
            public string C99CBR { get; set; }
            public string C99EDT { get; set; }
            public string C99UST { get; set; }
            public string C99SPM { get; set; }
            public int C99UDT { get; set; }
            public int C99UTM { get; set; }
            public string C99UUS { get; set; }
            public string C99UPG { get; set; }
            public string C99UWS { get; set; }
            public string C99RST { get; set; }
        }
        private class ILMS99Model
        {
            public int P99BRN { get; set; }
            public string P99LNT { get; set; }
            public string P99REC { get; set; }
            public string P99DES { get; set; }
            public int P99RUN { get; set; }
            public string P99FIL { get; set; }
            public int P99UPD { get; set; }
            public int P99TIM { get; set; }
            public string P99UPG { get; set; }
            public string P99USR { get; set; }
            public string P99DSP { get; set; }
            public string P99DEL { get; set; }
        }
        #endregion

        #region Configuration

        protected void TabContentControlSetVisible(bool enable)
        {

            tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("ShareSub");
            //pTab.Enabled = enable;
            SetTabControlReadOnly();

        }
        protected void ClearSession()
        {
            string nameCookies = "ds_multi_rate,ds_party_rate,ds_term_of_campaign,ds_term_of_campaign_detail,ds_gv_selectlistvendor," +
                                 "ds_gv_selectlistproduct,ds_vendor_list,ds_AddItemProduct,ds_AddItemExceptProduct,ds_TYPESUB_ILCP05,ds_gridAppType" +
                                 "ds_DS_ILCP07,ds_DS_ILCP04_CHECK,ds_DS_ILCP04,ds_gridBranch,Flag_Load_Campaign,ds_gvListVendor";
            campaignStorage.ClearCookies(nameCookies);

            string nameMaxLargeCookies = "ds_gv_selectlistproduct,ds_gv_selectlistvendor";
            campaignStorage.ClearMaxLargeCookies(nameMaxLargeCookies);
            //Globals.confirmStatusEdit = null;
            HdnIsEdit2.Value = "";
        }

        protected void ClearShareSub()
        {
            LoadApplicationTypeData();
            BindBaseRate();

            rdoTypeSub.ClearSelection();
            ckbShareSub.ClearSelection();
        }
        protected void ClearShareSubOption()
        {
            ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = false;
            ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = false;

            BindShareSubForCreate();
        }
        protected void SetTabControlReadOnly()
        {
            rdoVendorPayment.Enabled = false;
            txtVendorCreditDays.ReadOnly = true;
            txtMarketingCode.ReadOnly = true;

            gvViewTermOfCampaign.Enabled = true;
            if (HdnIsEdit1.Value == "LOAD_CAMPAIGN")
            {
                gvVendorList.Enabled = true;
                rdbSelectVendor.Enabled = true;
                btnSelectMarketing.Enabled = true;
                gvBranch.Enabled = true;
            }
            else
            {
                gvVendorList.Enabled = false;
                rdbSelectVendor.Enabled = false;
                btnSelectMarketing.Enabled = false;
                gvBranch.Enabled = false;
            }

            SetTermProductDetailEnable(false);
            SetNoteReadOnly(false);

            try
            {
                if (ckbShareSub.Items[1].Selected)
                {
                    TabShareSubVendorControlVisible(true);
                }
            }
            catch { }
        }
        protected void SetTermProductDetailReadOnly()
        {
            ddlSelectProduct.Enabled = false;
            optionAddItem.Enabled = false;
            ddlSelectItem.Enabled = false;
            //serachProductCampaign.Enabled = false;
            btnAddProductItem.Enabled = false;
            btnClearProductItem.Enabled = false;
            txtPriceMin.ReadOnly = false;
            txtPriceMax.ReadOnly = false;
            gvViewTermOfCampaignDetail.Enabled = false;
        }
        protected void SetTermProductDetailActive()
        {
            ddlSelectProduct.Enabled = true;
            optionAddItem.Enabled = true;
            ddlSelectItem.Enabled = true;
            serachProductCampaign.Enabled = true;
            btnAddProductItem.Enabled = true;
            btnClearProductItem.Enabled = true;
            txtPriceMin.ReadOnly = true;
            txtPriceMax.ReadOnly = true;
            gvViewTermOfCampaignDetail.Enabled = true;
        }
        protected void SetNoteReadOnly(bool enable)
        {
            if (HdnIsEdit1.Value == "CREATE_CAMPAIGN" || HdnIsEdit1.Value == "LOAD_CAMPAIGN")
            {
                txtNote.Enabled = true;
            }
            else
            {
                //txtNote.ReadOnly = !enable;
                txtNote.Enabled = false;
            }
            btnSaveNote.Enabled = enable;
            btnCancelNote.Enabled = enable;
        }
        protected void SetTermProductDetailEnable(bool enable)
        {
            ddlSelectProduct.Enabled = enable;
            if (ddlSelectProduct.SelectedValue == "productType4")
            {
                optionAddItem.Enabled = enable;
            }
            else
            {
                optionAddItem.Enabled = false;
            }
            if (ddlSelectProduct.SelectedValue == "productType1")
            {
                ddlSelectItem.Enabled = false;
            }
            else
            {
                ddlSelectItem.Enabled = enable;
            }
            //serachProductCampaign.Enabled = !enable;
            btnAddProductItem.Enabled = enable;
            btnClearProductItem.Enabled = enable;
            txtPriceMin.ReadOnly = !enable;
            txtPriceMax.ReadOnly = !enable;
            //gvViewTermOfCampaignDetail.Enabled = enable;
        }

        #endregion

        #region Bind Data
        protected void BindDatagvVendorList()
        {

        }

        protected void BindDatagvViewTermOfCampaign()
        {
            if ((HdnIsEdit1.Value == "LOAD_CAMPAIGN")
                || (HdnIsEdit1.Value == "UPDATE_CAMPAIGN") || (HdnIsEdit1.Value == "DELETE_CAMPAIGN")
                || (HdnIsEdit1.Value == "VIEW_CAMPAIGN"))
            {

            }

            else
            {
                //string[] sameColumns = { "TotalTerm", "SubSeq", "Seq" };
                DataTable dtSource = new DataTable();
                if (!string.IsNullOrEmpty(ds_multi_rate.Value))
                {
                    dtSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value).Tables[0]?.Copy();
                }
                else
                {
                    dtSource = campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.Copy();
                }
                bool avgFlg = false;
                decimal avgRte = 0;

                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                {
                    DataTable dtN = dtSource.Copy();
                    //var dt = dtSource.AsEnumerable().GroupBy(x => x.Field<string>("C05CSQ")).Select(x => x.OrderBy(o => int.Parse(o.Field<string>("C05RSQ")))).ToList();
                    var dt = (from t in dtN.AsEnumerable()
                              where int.Parse(t.Field<string>("C05RSQ")) == 1
                              select t).ToList();

                    if (dt.Count > 0)
                    {
                        dtSource.Clear();

                        foreach (DataRow dr in dt)
                        {
                            int totalTerm = Convert.ToInt32(dr["TotalTerm"]);

                            dr["C05STO"] = totalTerm;
                            dr["C05STM"] = totalTerm;
                            dr["CampaignStartTerm"] = 1;
                            dr["CampaignEndTerm"] = totalTerm;

                            dtSource.ImportRow(dr);
                        }
                    }
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    if (dtSource.Rows.Count > 1)
                    {
                        decimal[] calRtePerSeq = new decimal[dtSource.Rows.Count + 1];
                        int totalTerm = dtSource.AsEnumerable().Select(x => int.Parse(x.Field<string>("C05STO"))).FirstOrDefault();

                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            calRtePerSeq[i] = Convert.ToDecimal(dtSource.Rows[i]["C05STM"].ToString()) * Convert.ToDecimal(dtSource.Rows[i]["RATES"].ToString());
                        }

                        avgRte = Math.Ceiling((calRtePerSeq.Sum() / totalTerm) * 100) / 100;
                        avgFlg = true;
                    }
                }

                DataTable dtDest = dtTermOfCampaign();

                foreach (DataRow sRow in dtSource.Rows)
                {
                    DataRow dRow = dtDest.NewRow();
                    dRow["C02TTM"] = sRow["C05STO"];
                    dRow["C02CSQ"] = sRow["C05CSQ"];
                    dRow["C02RSQ"] = sRow["C05RSQ"];
                    dRow["C02TTR"] = sRow["C05STM"];
                    dRow["C02FMT"] = sRow["CampaignStartTerm"];
                    dRow["C02TOT"] = sRow["CampaignEndTerm"];

                    decimal cusRate = Convert.ToDecimal(sRow["RATES"]);
                    decimal intRate = cusRate > _DefaultIntRate ? _DefaultIntRate : cusRate;
                    decimal crRate = cusRate - intRate;

                    dRow["C02INR"] = intRate.ToString("N2");
                    dRow["C02CRR"] = crRate.ToString("N2");

                    dRow["C02IFR"] = "0.00";
                    dRow["RATES"] = cusRate.ToString("N2");

                    dRow["C02AIR"] = avgFlg ? avgRte.ToString("N2") : intRate.ToString("N2");
                    dRow["C02ACR"] = avgFlg ? "0.00" : crRate.ToString("N2");

                    dtDest.Rows.Add(dRow);
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtDest);
                ds_term_of_campaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
                if (campaignStorage.check_dataset(ds))
                {
                    gvViewTermOfCampaign.DataSource = ds;
                    gvViewTermOfCampaign.DataBind();
                }
            }

        }

        protected void BindgvViewTermOfCampaign_Event()
        {
            if (!string.IsNullOrEmpty(ds_term_of_campaign.Value))
            {
                DataSet ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value);
                if (campaignStorage.check_dataset(ds))
                {
                    gvViewTermOfCampaign.DataSource = ds;
                    gvViewTermOfCampaign.DataBind();
                }
            }

        }

        protected void BindDataTermOfCampaignDetail()
        {

        }

        protected void BindDatagvInstallment()
        {

        }

        protected void BindDatagvProductList()
        {

        }
        #endregion

        #region Multi-Rate
        protected void btnInsertTerm_Click(object sender, EventArgs e)
        {
            btnInsertTerm.Enabled = false;
            CampaignTypeVendorBindgvMultiRate();

            InsertTermEvent(true);

            if (_termSeq <= _MaxRangeOfInterest)
            {

                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    _termSeq = (from dsMultiRate in campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0].AsEnumerable()
                                    //_termSeq = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0].AsEnumerable()
                                where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == 1
                                orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                select int.Parse(dsMultiRate.Field<string>("C05RSQ"))).FirstOrDefault();
                    _termSeq++;
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                {
                    //_termSeq = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0].AsEnumerable()
                    _termSeq = (from dsMultiRate in campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0].AsEnumerable()
                                where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == Convert.ToInt32(txtSubSeq.Text)
                                orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                select int.Parse(dsMultiRate.Field<string>("C05RSQ"))).FirstOrDefault();
                    _termSeq++;
                }

                txtTermRange.Text = _termSeq.ToString();
                lbEndTermRange.Text = "Total Term of Range " + txtTermRange.Text;

                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y" && txt_addTerm.Text == txtEndTerm.Text)
                {
                    btnSetTermRange.Enabled = false;
                    txtEndTermRange.Enabled = false;
                    btnCreateCampaignOK.Enabled = true;
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y" && txt_addTerm.Text != txtEndTerm.Text)
                {
                    btnCreateCampaignOK.Enabled = false;
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N" && txt_addTerm.Text == txtEndTerm.Text)
                {
                    //var subseq = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0].AsEnumerable()
                    var subseq = (from dsMultiRate in campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0].AsEnumerable()
                                  orderby int.Parse(dsMultiRate.Field<string>("C05CSQ")) descending
                                  select int.Parse(dsMultiRate.Field<string>("C05CSQ"))).Distinct().FirstOrDefault();

                    txtSubSeq.Text = (subseq + 1).ToString();
                    txtTermRange.Text = "1";
                    lbEndTermRange.Text = "Total Term of Range " + txtTermRange.Text;
                    txtRateforRange.ReadOnly = false;
                    SetSubSeqTermVisible(true);
                }
            }

            pGridMultiRate.Visible = true;
            gvMultiRate.Visible = true;
            gvMultiRateOptionalShareSubEvent();
        }

        private void SetSubSeqTermVisible(bool enable)
        {
            btnAddSubSeq.Enabled = enable;
            btnCreateCampaignOK.Enabled = enable;

            txt_addTerm.Enabled = !enable;
            txt_addTerm.Text = null;
            pRangeY.Visible = !enable;
            pAddMonth.Visible = false;
            pAddRate.Visible = false;
        }

        protected void CampaignTypeVendorBindgvMultiRate()
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(ds_multi_rate.Value))
            {
                dt = dtShareSub();
            }
            else
            {
                dt = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0]?.Copy();
            }
            DataRow dr = dt.NewRow();

            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            {
                dr["RATES"] = Convert.ToDecimal(String.IsNullOrEmpty(txtRateforRange.Text.Trim()) ? "0.00" : txtRateforRange.Text.Trim());
                dr["C05STO"] = Convert.ToInt32(String.IsNullOrEmpty(txt_addTerm.Text.Trim()) ? "0" : txt_addTerm.Text.Trim());
                dr["C05CSQ"] = 1;//Convert.ToInt32(String.IsNullOrEmpty(txtTermRange.Text.Trim()) ? "0" : txtTermRange.Text.Trim());
                dr["C05RSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtTermRange.Text.Trim()) ? "1" : txtTermRange.Text.Trim());
                dr["C05STM"] = Convert.ToDecimal(String.IsNullOrEmpty(txtEndTerm.Text.Trim()) ? "0.00" : txtEndTerm.Text.Trim()) - Convert.ToDecimal(String.IsNullOrEmpty(txtStartTerm.Text.Trim()) ? "0.00" : txtStartTerm.Text.Trim()) + 1;//txtCreateCampaignTotalTermRate.Text.Trim();
                dr["C05SBT"] = rdoCreateCampaingTypeVendorCalculateType.SelectedValue;
                dr["Easybuy"] = "EASY BUY";
                dr["ESubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateESB.Text.Trim()) ? "0.00" : txtSubRateESB.Text.Trim());
                dr["Maker"] = String.IsNullOrEmpty(txtMakerCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtMakerCode.Text.Trim()) ? null : txtMakerCode.Text.Trim();
                dr["MSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateMaker.Text.Trim()) ? "0.00" : txtSubRateMaker.Text.Trim());
                dr["Vendor"] = String.IsNullOrEmpty(txtVendorCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtVendorCode.Text.Trim()) ? null : txtVendorCode.Text.Trim();
                dr["VSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateVendor.Text.Trim()) ? "0.00" : txtSubRateVendor.Text.Trim());
                dr["CampaignStartTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtStartTerm.Text.Trim()) ? "0.00" : txtStartTerm.Text.Trim());
                dr["CampaignEndTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtEndTerm.Text.Trim()) ? "0.00" : txtEndTerm.Text.Trim());
                dr["TotalTerm"] = Convert.ToInt32(String.IsNullOrEmpty(txt_addTerm.Text.Trim()) ? "0" : txt_addTerm.Text.Trim());

                dt.Rows.Add(dr);
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
            {
                if (rbOptionalAddMultiTerm.SelectedValue == "S")
                {
                    dr["RATES"] = Convert.ToDecimal(String.IsNullOrEmpty(txtRateforRange.Text.Trim()) ? "0.00" : txtRateforRange.Text.Trim());
                    dr["C05STO"] = Convert.ToInt32(String.IsNullOrEmpty(txtEndTerm.Text.Trim()) ? "0" : txtEndTerm.Text.Trim());
                    dr["C05CSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtSubSeq.Text.Trim()) ? "1" : txtSubSeq.Text.Trim());
                    dr["C05RSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtTermRange.Text.Trim()) ? "1" : txtTermRange.Text.Trim());
                    dr["C05STM"] = Convert.ToDecimal(String.IsNullOrEmpty(txtEndTerm.Text.Trim()) ? "0.00" : txtEndTerm.Text.Trim()) - Convert.ToDecimal(String.IsNullOrEmpty(txtStartTerm.Text.Trim()) ? "0.00" : txtStartTerm.Text.Trim()) + 1;//txtCreateCampaignTotalTermRate.Text.Trim();
                    dr["C05SBT"] = rdoCreateCampaingTypeVendorCalculateType.SelectedValue;
                    dr["Easybuy"] = "EASY BUY";
                    dr["ESubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateESB.Text.Trim()) ? "0.00" : txtSubRateESB.Text.Trim());
                    dr["Maker"] = String.IsNullOrEmpty(txtMakerCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtMakerCode.Text.Trim()) ? null : txtMakerCode.Text.Trim();
                    dr["MSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateMaker.Text.Trim()) ? "0.00" : txtSubRateMaker.Text.Trim());
                    dr["Vendor"] = String.IsNullOrEmpty(txtVendorCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtVendorCode.Text.Trim()) ? null : txtVendorCode.Text.Trim();
                    dr["VSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateVendor.Text.Trim()) ? "0.00" : txtSubRateVendor.Text.Trim());
                    dr["CampaignStartTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtStartTerm.Text.Trim()) ? "0.00" : txtStartTerm.Text.Trim());
                    dr["CampaignEndTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtEndTerm.Text.Trim()) ? "0.00" : txtEndTerm.Text.Trim());
                    dr["TotalTerm"] = Convert.ToInt32(String.IsNullOrEmpty(txt_addTerm.Text.Trim()) ? "0" : txt_addTerm.Text.Trim());

                    dt.Rows.Add(dr);
                }
                else if (rbOptionalAddMultiTerm.SelectedValue == "M")
                {
                    string[] TermRange = txtTermMultiRange.Text.Split(',');
                    List<int> ListTerms = new List<int>();
                    //Get Multi Range of term.
                    foreach (string s in TermRange)
                    {
                        if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
                        {
                            if (!s.Contains("-"))
                            {
                                ListTerms.Add(Convert.ToInt32(s));
                            }
                            else
                            {
                                string[] termOrdinal = s.Split('-');
                                if (termOrdinal.Length == 2)
                                {
                                    int termStart = Convert.ToInt32(termOrdinal[0].ToString());
                                    int termEnd = Convert.ToInt32(termOrdinal[1].ToString());

                                    for (int i = termStart; i <= termEnd; i++)
                                    {
                                        ListTerms.Add(i);
                                    }
                                }
                            }
                        }
                    }

                    //List Distinct & sort by ascending
                    ListTerms = ListTerms.Distinct().ToList();
                    ListTerms.Sort();

                    int subSeq = 1;
                    foreach (int t in ListTerms)
                    {
                        dr = dt.NewRow();

                        dr["RATES"] = Convert.ToDecimal(String.IsNullOrEmpty(txtRateforRange.Text.Trim()) ? "0.00" : txtRateforRange.Text.Trim());
                        dr["C05STO"] = t;
                        dr["C05CSQ"] = subSeq;
                        dr["C05RSQ"] = 1;
                        dr["C05STM"] = t;
                        dr["C05SBT"] = rdoCreateCampaingTypeVendorCalculateType.SelectedValue;
                        dr["Easybuy"] = "EASY BUY";
                        dr["ESubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateESB.Text.Trim()) ? "0.00" : txtSubRateESB.Text.Trim());
                        dr["Maker"] = String.IsNullOrEmpty(txtMakerCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtMakerCode.Text.Trim()) ? null : txtMakerCode.Text.Trim();
                        dr["MSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateMaker.Text.Trim()) ? "0.00" : txtSubRateMaker.Text.Trim());
                        dr["Vendor"] = String.IsNullOrEmpty(txtVendorCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtVendorCode.Text.Trim()) ? null : txtVendorCode.Text.Trim();
                        dr["VSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtSubRateVendor.Text.Trim()) ? "0.00" : txtSubRateVendor.Text.Trim());
                        dr["CampaignStartTerm"] = 1;
                        dr["CampaignEndTerm"] = t;
                        dr["TotalTerm"] = t;

                        dt.Rows.Add(dr);
                        subSeq++;
                    }
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_multi_rate.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
            campaignStorage.SetCookiesDataSetByName("ds_multi_rate", ds);
            if (campaignStorage.check_dataset(ds))
            {
                gvMultiRate.DataSource = ds;
                gvMultiRate.DataBind();
            }
        }

        protected void CampaignTypeVendorUpdategvMultiRate(string columnName, string value)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = campaignStorage.GetCookiesDataSetByKey("ds_multi_rate");
            if (!campaignStorage.check_dataset(ds))
            {
                dt = dtShareSub();
            }
            else
            {
                dt = campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.Copy();
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr.SetField(columnName, value);
                }
            }

            //Bind data.
            ds.Tables.Add(dt);
            campaignStorage.SetCookiesDataTableByName("ds_multi_rate", dt);
            if (campaignStorage.check_dataset(ds))
            {
                gvMultiRate.DataSource = ds;
                gvMultiRate.DataBind();
            }
        }

        private void ClearTermRange()
        {
            pAddMonth.Visible = false;
            pAddRate.Visible = false;
            pGridMultiRate.Visible = false;
            rbCreateCampaingTypeVendorCampaignSupport.Enabled = true;
            //rbRange.Enabled = true;
            txtTermRange.Enabled = true;
            txtEndTermRange.Enabled = true;
            txtEndTerm.Enabled = true;
            txtCreateCampaingTypeVendorBaseRate.Enabled = true;
            txt_addTerm.Enabled = true;
            btnSetTermRange.Enabled = true;
            btnCreateCampaignOK.Enabled = false;
            btnAddSubSeq.Enabled = true;
            txtRateforRange.ReadOnly = false;

            //Clear data
            txtSubSeq.Text = null;
            txt_addTerm.Text = null;
            txtTermRange.Text = null;
            txtEndTermRange.Text = null;
            txtStartTerm.Text = null;
            txtEndTerm.Text = null;
            txtRateforRange.Text = null;
            txtSubRateVendor.Text = null;
            txtSubRateESB.Text = null;

            //Initail Default
            lbEndTermRange.Text = "Total Term of Range 1";

            //Clear gvMultirate
            DataTable dt = dtShareSub();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            campaignStorage.SetCookiesDataTableByName("ds_multi_rate", dt);
            if (campaignStorage.check_dataset(ds))
            {
                gvMultiRate.DataSource = ds;
                gvMultiRate.DataBind();
            }

            _termSeq = 1;
        }

        private void InsertTermEvent(bool enable)
        {
            rbCreateCampaingTypeVendorCampaignSupport.Enabled = enable;
            //rbRange.Enabled = true;
            txtTermRange.Enabled = enable;
            txtEndTermRange.Enabled = enable;
            //txtEndTerm.Enabled = true;
            txtCreateCampaingTypeVendorBaseRate.Enabled = enable;

            //Diable
            btnSetTermRange.Enabled = enable;
            btnInsertTerm.Enabled = !enable;
            //txt_addTerm.Enabled = !enable;
            txtStartTerm.Enabled = !enable;
            txtEndTerm.Enabled = !enable;
            txtRateforRange.Enabled = !enable;
            txtSubRateMaker.Enabled = !enable;
            txtSubRateVendor.Enabled = !enable;
            txtSubRateESB.Enabled = !enable;

            //Clear value
            if (!enable)
            {
                txtStartTerm.Text = null;
                txtEndTerm.Text = null;
                txtRateforRange.Text = null;
                txtSubRateVendor.Text = null;
                txtSubRateESB.Text = null;
                //txtEndTermRange.Text = null;
                //txtTermRange.Text = null;
            }
            else
            {
                txtEndTermRange.Text = null;
                txtTermRange.Text = null;
            }

            //MultiT Term
            if (rbOptionalAddMultiTerm.SelectedValue == "M")
            {
                pAddMultiTerm.Visible = false;
                rbOptionalAddMultiTerm.Enabled = false;
                txtRateforRange.Enabled = true;
                txtSubRateMaker.Enabled = true;
                txtSubRateVendor.Enabled = true;
                txtSubRateESB.Enabled = true;
                btnInsertTerm.Enabled = true;
            }
        }

        protected void txtSubRateVendor_TextChanged(object sender, EventArgs e)
        {
            //txtSubRateVendor.Text = ConvertTo2Decimal(txtSubRateVendor.Text.Trim());
            txtSubRateVendor.Text = ValidateRateWithBaseRate("VENDOR");
            //decimal baserate = Convert.ToDecimal(txtCreateCampaingTypeVendorBaseRate.Text);
            //decimal rate = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateVendor.Text.Trim()));

            //if (rate <= baserate)
            //{
            //    txtSubRateVendor.Text = rate.ToString("N2");
            //}
            //else
            //{
            //    txtSubRateVendor.Text = baserate.ToString("N2");
            //}
        }

        protected void txtSubRateMaker_TextChanged(object sender, EventArgs e)
        {
            txtSubRateMaker.Text = ValidateRateWithBaseRate("MAKER");
        }

        protected void txtSubRateESB_TextChanged(object sender, EventArgs e)
        {
            //txtSubRateESB.Text = ConvertTo2Decimal(txtSubRateESB.Text.Trim());
            txtSubRateESB.Text = ValidateRateWithBaseRate("ESB");
            //decimal baserate = Convert.ToDecimal(txtCreateCampaingTypeVendorBaseRate.Text);
            //decimal rate = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateESB.Text.Trim()));

            //if (rate <= baserate)
            //{
            //    txtSubRateESB.Text = rate.ToString("N2");
            //}
            //else
            //{
            //    txtSubRateESB.Text = baserate.ToString("N2");
            //}
        }

        protected void txtRateforRange_TextChanged(object sender, EventArgs e)
        {
            txtRateforRange.Text = ValidateRateWithBaseRate("CUSTOMER");
        }

        protected void txtTermRange_TextChanged(object sender, EventArgs e)
        {
            decimal value = Convert.ToInt32(ConvertToInteger(txtTermRange.Text));
            if (value > _MaxRangeOfInterest)
            {
                value = _MaxRangeOfInterest;
            }

            txtTermRange.Text = value.ToString();
            lbEndTermRange.Text = "Total Term of Range " + txtTermRange.Text;
        }

        protected void txtEndTermRange_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEndTermRange.Text))
            {
                string resultTerm = _MinTotalTermOfContract.ToString();
                int inputValue = Convert.ToInt32(ConvertToInteger(txtEndTermRange.Text));
                decimal totalTerm = _MinTotalTermOfContract;//Convert.ToInt32(txt_addTerm.Text);

                //Total Term
                if (String.IsNullOrEmpty(txt_addTerm.Text))
                {
                    if (inputValue <= _MaxTotalTermOfContract && inputValue >= _MinTotalTermOfContract)
                    {
                        totalTerm = inputValue;
                    }
                    else if (inputValue > _MaxTotalTermOfContract && inputValue >= _MinTotalTermOfContract)
                    {
                        totalTerm = _MaxTotalTermOfContract;
                    }
                    else if (inputValue < _MinTotalTermOfContract)
                    {
                        totalTerm = _MinTotalTermOfContract;
                    }

                    txt_addTerm.Text = totalTerm.ToString();
                }
                else
                {
                    totalTerm = Convert.ToInt32(txt_addTerm.Text);
                }

                //Check totalterm max.
                if (totalTerm > _MaxTotalTermOfContract)
                {
                    totalTerm = _MaxTotalTermOfContract;
                }

                //Result
                if (inputValue > totalTerm && inputValue >= _MinTotalTermOfContract)
                {
                    resultTerm = totalTerm.ToString();
                }
                else if (inputValue < _MinTotalTermOfContract)
                {
                    resultTerm = _MinTotalTermOfContract.ToString();
                }
                else
                {
                    resultTerm = inputValue.ToString();
                }

                txtEndTermRange.Text = resultTerm;//Convert.ToInt32(ConvertToInteger(txtEndTermRange.Text)) > _MaximumTerm ? _MaximumTerm.ToString() : ConvertToInteger(txtEndTermRange.Text);//ConvertToInteger(txtEndTermRange.Text);
            }
            else
            {
                return;
            }
        }

        protected void txt_addTerm_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_addTerm.Text))
            {
                string resultTerm = _MinTotalTermOfContract.ToString();
                int inputValue = Convert.ToInt32(ConvertToInteger(txt_addTerm.Text));
                if (inputValue > _MaxTotalTermOfContract && inputValue >= _MinTotalTermOfContract)
                {
                    resultTerm = _MaxTotalTermOfContract.ToString();
                }
                else if (inputValue < _MinTotalTermOfContract)
                {
                    resultTerm = _MinTotalTermOfContract.ToString();
                }
                else
                {
                    resultTerm = inputValue.ToString();
                }

                decimal subTerm = String.IsNullOrEmpty(txtEndTermRange.Text) ? _MinTotalTermOfContract : Convert.ToInt32(txtEndTermRange.Text);
                if (subTerm > Convert.ToInt32(resultTerm))
                {
                    txtEndTermRange.Text = resultTerm;
                }

                txt_addTerm.Text = resultTerm;//Convert.ToInt32(ConvertToInteger(txt_addTerm.Text)) > _MaximumTerm ? _MaximumTerm.ToString() : ConvertToInteger(txt_addTerm.Text);
            }
            else
            {
                return;
            }
        }

        protected void txtStartTerm_TextChanged(object sender, EventArgs e)
        {
            string resultTerm = _MinTotalTermOfContract.ToString();
            int inputValue = Convert.ToInt32(ConvertToInteger(txtStartTerm.Text));
            if (inputValue > _MaxTotalTermOfContract && inputValue >= _MinTotalTermOfContract)
            {
                resultTerm = _MaxTotalTermOfContract.ToString();
            }
            else if (inputValue < _MinTotalTermOfContract)
            {
                resultTerm = _MinTotalTermOfContract.ToString();
            }
            else
            {
                resultTerm = inputValue.ToString();
            }

            txtStartTerm.Text = resultTerm;//Convert.ToInt32(ConvertToInteger(txtStartTerm.Text)) < 1 ? "1" : ConvertToInteger(txtStartTerm.Text);
        }

        protected void txtEndTerm_TextChanged(object sender, EventArgs e)
        {
            txtEndTerm.Text = ConvertToInteger(txtEndTerm.Text);
        }
        #endregion

        #region Create Datatable
        //protected void DT_ILCP02()
        //{
        //    DataTable DT_ILCP02 = new DataTable();
        //    DT_ILCP02.Columns.AddRange(new DataColumn[12] { new DataColumn("C02TTM"),
        //                                                              new DataColumn("C02CSQ"),
        //                                                              new DataColumn("C02RSQ"),
        //                                                              new DataColumn("C02TTR"),
        //                                                              new DataColumn("C02FMT"),
        //                                                              new DataColumn("C02TOT"),
        //                                                              new DataColumn("C02INR"),
        //                                                              new DataColumn("C02CRR"),
        //                                                              new DataColumn("C02IFR"),
        //                                                              new DataColumn("RATES"),
        //                                                              new DataColumn("C02AIR"),
        //                                                              new DataColumn("C02ACR") });
        //    ViewState["ds_term_of_campaign"] = DT_ILCP02;
        //    gvViewTermOfCampaign.DataSource = ViewState["ds_term_of_campaign"];
        //    gvViewTermOfCampaign.DataBind();
        //}
        #endregion

        #region Function about load data for campaign
        private void LOAD_DATA_ILCP01() //load data in panel 'Main'
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];


                SqlAll = @" SELECT C01CMP,C01STY,C01CNM,C01PTY,C01PMT,C01SDT,C01EDT,C01CAD,C01CLD,C01VDC,C01MKC,C01NXD,C01FIN,C01CTY,C01SBT,C01RNG,C01SRT,C01VTY,C01VCR,C01MKT,C01CST 
                            FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE  CAST(C01CMP as nvarchar) = '" + idCampaignLoad + "'";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;



                if (DS?.Tables[0]?.Rows.Count == 0)
                {
                    dataCenter.CloseConnectSQL();
                    return;
                }

                DataRow drILCP01 = DS?.Tables[0].Rows[0];

                Globals.showStatusHeader = drILCP01["C01CST"].ToString().Trim();

                ddlCampaignType.SelectedIndex = int.Parse(drILCP01["C01STY"].ToString().Trim());
                txtCampaignName.Text = drILCP01["C01CNM"].ToString().Trim();
                txtProductDetail.Text = drILCP01["C01PTY"].ToString().Trim();
                txtSpecialPremium.Text = drILCP01["C01PMT"].ToString().Trim();
                txtStartDate.Text = FORMAT_DATE(drILCP01["C01SDT"].ToString().Trim());
                txtEndDate.Text = FORMAT_DATE(drILCP01["C01EDT"].ToString().Trim());
                txtClosingApplicationDate.Text = FORMAT_DATE(drILCP01["C01CAD"].ToString().Trim());
                txtClosingLayBillDate.Text = FORMAT_DATE(drILCP01["C01CLD"].ToString().Trim());
                txtXDue.Text = drILCP01["C01NXD"].ToString().Trim();
                txtFInstall.Text = drILCP01["C01FIN"].ToString().Trim();

                if (int.Parse(drILCP01["C01STY"].ToString()) == 1)
                {

                    CreateCampaignMakerType();
                    txtMakerCode.Text = drILCP01["C01MKC"].ToString().Trim();
                    ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = true;
                    ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = false;
                    ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;

                    ckbShareSub.Items[0].Enabled = true;
                    ckbShareSub.Items[1].Enabled = false;
                    ckbShareSub.Items[2].Enabled = true;
                }
                else if (int.Parse(drILCP01["C01STY"].ToString()) == 2)
                {
                    CreateCampaignVendorType();
                    txtVendorCode.Text = drILCP01["C01VDC"].ToString().Trim();
                    ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = false;
                    ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = true;
                    ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;

                    ckbShareSub.Items[0].Enabled = false;
                    ckbShareSub.Items[1].Enabled = true;
                    ckbShareSub.Items[2].Enabled = true;
                }
                else if (int.Parse(drILCP01["C01STY"].ToString()) == 3)
                {
                    CreateCampaignESBType();
                    ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = false;
                    ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = false;
                    ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;

                    ckbShareSub.Items[0].Enabled = false;
                    ckbShareSub.Items[1].Enabled = false;
                    ckbShareSub.Items[2].Enabled = true;
                }
                else if (int.Parse(drILCP01["C01STY"].ToString()) == 4)
                {
                    CreateCampaignShareSubType();
                    //ckbCreateCampaignTypeVendorShareSub.Items[0].Enabled = true;
                    //ckbCreateCampaignTypeVendorShareSub.Items[1].Enabled = true;
                    //ckbCreateCampaignTypeVendorShareSub.Items[2].Enabled = true;

                    ckbShareSub.Items[0].Enabled = true;
                    ckbShareSub.Items[1].Enabled = true;
                    ckbShareSub.Items[2].Enabled = true;
                    //if ((Int32.Parse(drILCP01["C01MKC"].ToString()) != 0) && (Int32.Parse(drILCP01["C01VDC"].ToString()) != 0))
                    //{

                    //}else if ((Int32.Parse(drILCP01["C01MKC"].ToString()) = 0) && (Int32.Parse(drILCP01["C01VDC"].ToString()) != 0))
                    //{

                    //}
                    //else if ((Int32.Parse(drILCP01["C01MKC"].ToString()) != 0) && (Int32.Parse(drILCP01["C01VDC"].ToString()) != 0))
                    //{

                    //}



                    txtMakerCode.Text = drILCP01["C01MKC"].ToString().Trim();
                    txtVendorCode.Text = drILCP01["C01VDC"].ToString().Trim();

                }

                if (drILCP01["C01NXD"].ToString().Trim() != "0")
                {

                    SelectCampaignType.Visible = true;
                    //ddlCampaignType.SelectedIndex = 2;

                }
                else if (drILCP01["C01MKC"].ToString().Trim() != "0")
                {

                    SelectCampaignType.Visible = true;
                    //ddlCampaignType.SelectedIndex = 1;

                }

                if (drILCP01["C01SBT"].ToString().Trim() == "R")
                {
                    ListItem rdbTypeVendorTypeSub = rdoCreateCampaingTypeVendorTypeSub.Items.FindByValue("R");
                    rdbTypeVendorTypeSub.Selected = true;
                    ListItem rdbTypeVendorTypeSub2 = rdoTypeSub.Items.FindByValue("R");
                    rdbTypeVendorTypeSub2.Selected = true;

                }
                else if (drILCP01["C01SBT"].ToString().Trim() == "I")
                {
                    ListItem rdbTypeVendorTypeSub = rdoCreateCampaingTypeVendorTypeSub.Items.FindByValue("I");
                    rdbTypeVendorTypeSub.Selected = true;
                    ListItem rdbTypeVendorTypeSub2 = rdoTypeSub.Items.FindByValue("I");
                    rdbTypeVendorTypeSub2.Selected = true;
                }

                if (drILCP01["C01CTY"].ToString().Trim() == "R")
                {
                    ListItem rdbCalculateType = rdoCreateCampaingTypeVendorCalculateType.Items.FindByValue("R");
                    rdbCalculateType.Selected = true;
                    pBaseRate.Visible = rdoCreateCampaingTypeVendorCalculateType.SelectedValue == "R" ? true : false;
                }
                else if (drILCP01["C01CTY"].ToString().Trim() == "I")
                {
                    ListItem rdbCalculateType = rdoCreateCampaingTypeVendorCalculateType.Items.FindByValue("I");
                    rdbCalculateType.Selected = true;
                    pBaseRate.Visible = rdoCreateCampaingTypeVendorCalculateType.SelectedValue == "R" ? true : false;
                }

                if (drILCP01["C01RNG"].ToString().Trim() == "Y" && !String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
                {
                    ListItem rdbVendorCampaignSupport = rbCreateCampaingTypeVendorCampaignSupport.Items.FindByValue("Y");
                    rdbVendorCampaignSupport.Selected = true;
                }
                else if (drILCP01["C01RNG"].ToString().Trim() == "N" && !String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
                {
                    ListItem rdbVendorCampaignSupport = rbCreateCampaingTypeVendorCampaignSupport.Items.FindByValue("N");
                    rdbVendorCampaignSupport.Selected = true;
                }
                else if (drILCP01["C01RNG"].ToString().Trim() == "Y" && String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
                {
                    ListItem rdbVendorCampaignSupport = rbCreateCampaingTypeVendorCampaignSupport.Items.FindByValue("Y");
                    rdbVendorCampaignSupport.Selected = true;

                    pRangeY.Visible = true;
                    //lbEndTermRange.Visible = true;
                    //txtEndTermRange.Visible = true;
                    lbStartTerm.Text = "Start Term";
                    txtStartTerm.Enabled = false;
                    txtEndTerm.Enabled = false;
                    txtStartTerm.ReadOnly = true;
                    txtEndTerm.ReadOnly = true;
                    //lbEndTerm.Visible = true;
                    //txtEndTerm.Visible = true;
                    //txt_addTerm.Visible = true;
                    //Label51.Visible = true;
                    //Label52.Visible = true;
                    btnCreateCampaignOK.Enabled = false;

                    ClearTermRange();

                    txtTermRange.Text = _termSeq.ToString();
                }
                else if (drILCP01["C01RNG"].ToString().Trim() == "N" && String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
                {
                    ListItem rdbVendorCampaignSupport = rbCreateCampaingTypeVendorCampaignSupport.Items.FindByValue("N");
                    rdbVendorCampaignSupport.Selected = true;

                    pRangeY.Visible = true;
                    //lbEndTermRange.Visible = false;
                    //txtEndTermRange.Visible = false;
                    lbStartTerm.Text = "Add Term for Rate";
                    txtStartTerm.Enabled = true;
                    txtStartTerm.ReadOnly = false;
                    //lbEndTerm.Visible = false;
                    //txtEndTerm.Visible = false;
                    //txt_addTerm.Visible = false;
                    //Label51.Visible = false;
                    //Label52.Visible = false;
                    btnCreateCampaignOK.Enabled = false;

                    ClearTermRange();

                    txtTermRange.Text = _termSeq.ToString();
                }
                txtCreateCampaingTypeVendorBaseRate.Text = drILCP01["C01SRT"].ToString().Trim();
                txtBaseRate.Text = drILCP01["C01SRT"].ToString().Trim();

                if (drILCP01["C01VTY"].ToString() == "B")
                {
                    rdoVendorPayment.SelectedIndex = 0;
                }
                else if (drILCP01["C01VTY"].ToString() == "A")
                {
                    rdoVendorPayment.SelectedIndex = 1;
                }

                rdoCreateCampaign.Enabled = false;

                txtVendorCreditDays.Text = drILCP01["C01VCR"].ToString();
                txtMarketingCode.Text = drILCP01["C01MKT"].ToString();
                pCampaignDetail.Enabled = true;
                gvPartyRateOptionalShareSubEvent();

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        private void LOAD_DATA_ILCP02() //load data in gridview for all panel (Tab : Term | Product)
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];

                SqlAll = "SELECT C02TTM, C02CSQ, C02RSQ, C02TTR, C02FMT, C02TOT, C02INR, C02CRR, C02IFR, C01SRT as RATES, C02AIR, C02ACR FROM AS400DB01.ILOD0001.ILCP02 WITH (NOLOCK) " +
                    " LEFT JOIN AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) ON C01CMP = C02CMP" +
                    " WHERE CAST(C02CMP as nvarchar) = '" + idCampaignLoad + "'";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                //ViewState["ds_term_of_campaign"] = DS;
                //gvViewTermOfCampaign.DataSource = DS;
                //gvViewTermOfCampaign.DataBind();
                //DT_ILCP02();
                DataTable DT_ILCP02 = dtTermOfCampaign();
                //DataTable DT_ILCP02 = (DataTable)ViewState["ds_term_of_campaign"];

                foreach (DataRow row in DS?.Tables[0]?.Rows)
                {
                    float rateSum = float.Parse(row["C02INR"].ToString()) + float.Parse(row["C02CRR"].ToString());
                    DT_ILCP02.Rows.Add(row["C02TTM"].ToString(),
                                       row["C02CSQ"].ToString(),
                                       row["C02RSQ"].ToString(),
                                       row["C02TTR"].ToString(),
                                       row["C02FMT"].ToString(),
                                       row["C02TOT"].ToString(),
                                       row["C02INR"].ToString(),
                                       row["C02CRR"].ToString(),
                                       row["C02IFR"].ToString(),
                                       rateSum,
                                       row["C02AIR"].ToString(),
                                       row["C02ACR"].ToString());
                }

                //ViewState["ds_term_of_campaign"] = DT_ILCP02;
                DataSet dss = new DataSet();
                dss.Tables.Add(DT_ILCP02);
                ds_term_of_campaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(dss);
                if (campaignStorage.check_dataset(dss))
                {
                    gvViewTermOfCampaign.DataSource = dss;
                    gvViewTermOfCampaign.DataBind();
                }

                if (DS?.Tables[0]?.Rows.Count == 0)
                {
                    dataCenter.CloseConnectSQL();
                    return;
                }

            }
            catch (Exception)
            {

                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_TYPESUB_ILCP05() //load data (Tab : Shar sub)
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];

                SqlAll = "SELECT DISTINCT C05PAR FROM AS400DB01.ILOD0001.ILCP05 WITH (NOLOCK) WHERE CAST(C05CMP as nvarchar) = " + idCampaignLoad + "";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                campaignStorage.SetCookiesDataSetByName("ds_TYPESUB_ILCP05", DS);
                ClearShareSubOption();

                foreach (DataRow row in DS?.Tables[0]?.Rows)
                {
                    if (row["C05PAR"].ToString() == "M")
                    {
                        ckbCreateCampaignTypeVendorShareSub.Items[0].Selected = true;
                        //ListItem ckbShareSubs = ckbShareSub.Items.FindByValue("Maker");
                        //ckbShareSubs.Selected = true;
                        //ListItem ckbCreateCampaignTypeVendorShareSubs = ckbCreateCampaignTypeVendorShareSub.Items.FindByValue("Maker");
                        //ckbCreateCampaignTypeVendorShareSubs.Selected = true;
                    }
                    else if (row["C05PAR"].ToString() == "V")
                    {
                        ckbCreateCampaignTypeVendorShareSub.Items[1].Selected = true;
                        //ListItem ckbShareSubs = ckbShareSub.Items.FindByValue("Vendor");
                        //ckbShareSubs.Selected = true;
                        //ListItem ckbCreateCampaignTypeVendorShareSubs = ckbCreateCampaignTypeVendorShareSub.Items.FindByValue("Vendor");
                        //ckbCreateCampaignTypeVendorShareSubs.Selected = true;
                    }
                    else if (row["C05PAR"].ToString() == "E")
                    {
                        ckbCreateCampaignTypeVendorShareSub.Items[2].Selected = true;
                        //ListItem ckbShareSubs = ckbShareSub.Items.FindByValue("ESB");
                        //ckbShareSubs.Selected = true;
                        //ListItem ckbCreateCampaignTypeVendorShareSubs = ckbCreateCampaignTypeVendorShareSub.Items.FindByValue("ESB");
                        //ckbCreateCampaignTypeVendorShareSubs.Selected = true;
                    }

                }

                BindShareSubForCreate();
            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_ILCP05() //load data in gridview (Tab : Shar sub)
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];


                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DataTable dtILCP05 = dtShareSub();

                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    //SqlAll = "SELECT DISTINCT (P.C02INR+P.C02CRR+P.C02IFR) AS RATES, C.C05STO, C.C05CSQ, C.C05RSQ, C.C05STM, C.C05SBT, " +
                    //         "CASE WHEN E.C05PAR = 'E' THEN 'Easybuy' ELSE '' END AS Easybuy, E.C05SIR AS ESubRate, M.C05PCD AS Maker, M.C05SIR AS MSubRate, V.C05PCD AS Vendor, V.C05SIR AS VSubRate, " +
                    //         "C.C05SST AS CampaignStartTerm, C.C05EST AS CampaignEndTerm, C.C05SST AS C02FMT, C.C05EST AS C02TOT, C.C05SST AS C05SSTs, C.C05EST AS C05ESTs, C.C05STM AS C05STMs " +
                    //         "FROM ILCP05 C LEFT JOIN ILCP02 P ON C.C05CMP = P.C02CMP AND C.C05CSQ = P.C02CSQ AND C.C05RSQ = P.C02RSQ " +
                    //         "LEFT JOIN ILCP05 E ON C.C05CMP = E.C05CMP AND C.C05CSQ = E.C05CSQ AND C.C05RSQ = E.C05RSQ AND E.C05PAR = 'E' " +
                    //         "LEFT JOIN ILCP05 M ON C.C05CMP = M.C05CMP AND C.C05CSQ = M.C05CSQ AND C.C05RSQ = M.C05RSQ AND M.C05PAR = 'M' " +
                    //         "LEFT JOIN ILCP05 V ON C.C05CMP = V.C05CMP AND C.C05CSQ = V.C05CSQ AND C.C05RSQ = V.C05RSQ AND V.C05PAR = 'V' " +
                    //         "WHERE C.C05CMP = " + idCampaignLoad +
                    //         " GROUP BY C.C05STO, C.C05CSQ, C.C05RSQ, C.C05STM, (P.C02INR+P.C02CRR+P.C02IFR), C.C05SBT, E.C05PAR, E.C05SIR, M.C05PCD, M.C05SIR, V.C05PCD, V.C05SIR, C.C05SST, C.C05EST";

                    DataSet DS = new DataSet();

                    SqlAll = @"SELECT DISTINCT  C.C05CSQ, C.C05RSQ, C.C05STM, C.C05SBT, CASE WHEN E.C05PAR = 'E' THEN 'EASY BUY' ELSE '' END AS Easybuy
                            , E.C05SIR AS ESubRate, M.C05PCD AS Maker, M.C05SIR AS MSubRate, V.C05PCD AS Vendor, V.C05SIR AS VSubRate, C.C05SST AS CampaignStartTerm, C.C05EST AS CampaignEndTerm
                            , C.C05SST AS C02FMT, C.C05EST AS C02TOT, C.C05SST AS C05SSTs, C.C05EST AS C05ESTs, C.C05STM AS C05STMs
                            FROM AS400DB01.ILOD0001.ILCP05 C  WITH (NOLOCK)
                            LEFT JOIN AS400DB01.ILOD0001.ILCP02 P WITH (NOLOCK) ON C.C05CMP = P.C02CMP AND C.C05CSQ = P.C02CSQ AND C.C05RSQ = P.C02RSQ 
                            LEFT JOIN AS400DB01.ILOD0001.ILCP05 E WITH (NOLOCK) ON C.C05CMP = E.C05CMP AND C.C05CSQ = E.C05CSQ AND C.C05RSQ = E.C05RSQ AND E.C05PAR = 'E' 
                            LEFT JOIN AS400DB01.ILOD0001.ILCP05 M WITH (NOLOCK) ON C.C05CMP = M.C05CMP AND C.C05CSQ = M.C05CSQ AND C.C05RSQ = M.C05RSQ AND M.C05PAR = 'M' 
                            LEFT JOIN AS400DB01.ILOD0001.ILCP05 V WITH (NOLOCK) ON C.C05CMP = V.C05CMP AND C.C05CSQ = V.C05CSQ AND C.C05RSQ = V.C05RSQ AND V.C05PAR = 'V' 
                            WHERE CAST(C.C05CMP as nvarchar) = '" + idCampaignLoad + "' GROUP BY C.C05CSQ, C.C05RSQ, C.C05STM, C.C05SBT, E.C05PAR, E.C05SIR, M.C05PCD, M.C05SIR, V.C05PCD, V.C05SIR, C.C05SST, C.C05EST";

                    DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    DataTable dt_loop_ILCP05 = loop_ILCP05();
                    DataTable dt = dtTermOfCampaign();
                    DataTable LOOP_DATA_ILCP02 = (campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_term_of_campaign.Value))?.Tables[0]?.Copy();
                    double Rates = 0.00;
                    string strRates = "0.00";
                    int SubSeq = 0;
                    int Seq = 0;
                    int Term = 0;
                    string Types = "";

                    string ESB = "";
                    double eSubrate = 0.00;
                    string Maker = "";
                    double mSubrate = 0.00;
                    string Vendors = "";
                    double vSubrate = 0.00;

                    int startTerm = 0;
                    int endTerm = 0;
                    int fromTerm = 0;
                    int toTerm = 0;
                    int startTerm05 = 0;
                    int endTerm05 = 0;
                    int subTotalTerm = 0;

                    int toTerm02 = 0; // for check process
                    int subTotalTerm05 = 0; // for check process
                    string totalTerm = "";

                    int SubSeqTerm02 = 0;
                    int SeqTerm02 = 0;

                    foreach (DataRow drLoopILCP05 in DS?.Tables[0]?.Rows)
                    {
                        SubSeq = int.Parse(drLoopILCP05["C05CSQ"].ToString());
                        Seq = int.Parse(drLoopILCP05["C05RSQ"].ToString());

                        foreach (DataRow dr_loopILCP02 in LOOP_DATA_ILCP02.Rows)
                        {
                            SubSeqTerm02 = int.Parse(dr_loopILCP02["C02CSQ"].ToString());
                            SeqTerm02 = int.Parse(dr_loopILCP02["C02RSQ"].ToString());

                            if (SubSeq == SubSeqTerm02 && Seq == SeqTerm02)
                            {
                                toTerm02 = int.Parse(dr_loopILCP02["C02TTR"].ToString());
                                totalTerm = dr_loopILCP02["C02TTM"].ToString();
                                subTotalTerm05 = int.Parse(drLoopILCP05["C05STM"].ToString());

                                if (subTotalTerm05 <= toTerm02)
                                {
                                    Rates = double.Parse(dr_loopILCP02["RATES"].ToString());
                                    strRates = Rates.ToString("#,##0.00").Trim();
                                    break;
                                }
                            }
                        }

                        //double RATE = double.Parse(drLoopILCP02["C02INR"].ToString())+ double.Parse(drLoopILCP02["C02INR"].ToString())+double.Parse(drLoopILCP02["C02INR"].ToString());
                        Term = int.Parse(drLoopILCP05["C05STM"].ToString());
                        Types = drLoopILCP05["C05SBT"].ToString();

                        if (!String.IsNullOrEmpty(drLoopILCP05["EASYBUY"].ToString()))
                        {
                            ESB = drLoopILCP05["EASYBUY"].ToString();
                            eSubrate = double.Parse(drLoopILCP05["ESUBRATE"].ToString());
                        }

                        if (!String.IsNullOrEmpty(drLoopILCP05["MAKER"].ToString()))
                        {
                            Maker = drLoopILCP05["MAKER"].ToString();
                            mSubrate = double.Parse(drLoopILCP05["MSUBRATE"].ToString());
                        }

                        if (!String.IsNullOrEmpty(drLoopILCP05["VENDOR"].ToString()))
                        {
                            Vendors = drLoopILCP05["VENDOR"].ToString();
                            vSubrate = double.Parse(drLoopILCP05["VSUBRATE"].ToString());
                        }

                        startTerm = int.Parse(drLoopILCP05["CAMPAIGNSTARTTERM"].ToString());
                        endTerm = int.Parse(drLoopILCP05["CAMPAIGNENDTERM"].ToString());
                        fromTerm = int.Parse(drLoopILCP05["C02FMT"].ToString());
                        toTerm = int.Parse(drLoopILCP05["C02TOT"].ToString());
                        startTerm05 = int.Parse(drLoopILCP05["C05SSTS"].ToString());
                        endTerm05 = int.Parse(drLoopILCP05["C05ESTS"].ToString());
                        subTotalTerm = int.Parse(drLoopILCP05["C05STMS"].ToString());
                        dt_loop_ILCP05.Rows.Add(strRates, totalTerm, SubSeq, Seq, Term, Types, ESB, eSubrate.ToString("#,##0.00"), Maker, mSubrate.ToString("#,##0.00"), Vendors, vSubrate.ToString("#,##0.00"), startTerm, endTerm, fromTerm, toTerm, startTerm05, endTerm05, subTotalTerm);

                    }

                    DataSet dsILCP05_NEW = new DataSet();
                    dsILCP05_NEW.Tables.Add(dt_loop_ILCP05);

                    ds_party_rate.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(dsILCP05_NEW);
                    campaignStorage.SetCookiesDataSetByName("ds_party_rate", dsILCP05_NEW);
                    if (campaignStorage.check_dataset(dsILCP05_NEW))
                    {
                        gvPartyRate.DataSource = dsILCP05_NEW;
                        gvPartyRate.DataBind();
                    }
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                {

                    DataSet dsC05PAR = new DataSet();

                    SqlAll = "SELECT DISTINCT (P.C02INR+P.C02CRR+P.C02IFR) AS RATES, C.C05STO, C.C05CSQ, C.C05RSQ, C.C05STM, C.C05SBT, " +
                             "CASE WHEN E.C05PAR = 'E' THEN 'EASY BUY' ELSE '' END AS Easybuy, E.C05SIR AS ESubRate, M.C05PCD AS Maker, M.C05SIR AS MSubRate, V.C05PCD AS Vendor, V.C05SIR AS VSubRate, " +
                             "C.C05SST AS CampaignStartTerm, C.C05EST AS CampaignEndTerm, C.C05SST AS C02FMT, C.C05EST AS C02TOT, C.C05SST AS C05SSTs, C.C05EST AS C05ESTs, C.C05STM AS C05STMs " +
                             "FROM AS400DB01.ILOD0001.ILCP05 C LEFT JOIN AS400DB01.ILOD0001.ILCP02 P WITH (NOLOCK) ON C.C05CMP = P.C02CMP AND C.C05CSQ = P.C02CSQ " +
                             "LEFT JOIN AS400DB01.ILOD0001.ILCP05 E WITH (NOLOCK) ON C.C05CMP = E.C05CMP AND C.C05CSQ = E.C05CSQ AND C.C05RSQ = E.C05RSQ AND E.C05PAR = 'E' " +
                             "LEFT JOIN AS400DB01.ILOD0001.ILCP05 M WITH (NOLOCK) ON C.C05CMP = M.C05CMP AND C.C05CSQ = M.C05CSQ AND C.C05RSQ = M.C05RSQ AND M.C05PAR = 'M' " +
                             "LEFT JOIN AS400DB01.ILOD0001.ILCP05 V WITH (NOLOCK) ON C.C05CMP = V.C05CMP AND C.C05CSQ = V.C05CSQ AND C.C05RSQ = V.C05RSQ AND V.C05PAR = 'V' " +
                             "WHERE CAST(C.C05CMP as nvarchar) = '" + idCampaignLoad + "' " +
                             " GROUP BY C.C05STO, C.C05CSQ, C.C05RSQ, C.C05STM, (P.C02INR+P.C02CRR+P.C02IFR), C.C05SBT, E.C05PAR, E.C05SIR, M.C05PCD, M.C05SIR, V.C05PCD, V.C05SIR, C.C05SST, C.C05EST";

                    dsC05PAR = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    ds_party_rate.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(dsC05PAR);
                    campaignStorage.SetCookiesDataSetByName("ds_party_rate", dsC05PAR);
                    if (campaignStorage.check_dataset(dsC05PAR))
                    {
                        gvPartyRate.DataSource = dsC05PAR;
                        gvPartyRate.DataBind();
                    }
                }



            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_ILCP06() //load data in panel 'For Application' (Tab : Share Sub.) && load data in panel 'Main'
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = copyCampaignString.Replace("-", "");//CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = "SELECT C06APT FROM AS400DB01.ILOD0001.ILCP06 WITH (NOLOCK) WHERE CAST(C06CMP as nvarchar) = '" + idCampaignLoad + "'";
                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    ds_gridAppType.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                    // in list box (selected application)
                    foreach (ListItem item in ckbPoupCreateCampaingTypeVendorApplicationType.Items)
                    {
                        item.Selected = false;
                    }
                    foreach (DataRow row in DS?.Tables[0]?.Rows)
                    {
                        if (row["C06APT"].ToString() == "01")
                        {
                            ckbPoupCreateCampaingTypeVendorApplicationType.Items[0].Selected = true;

                        }
                        if (row["C06APT"].ToString() == "02")
                        {
                            ckbPoupCreateCampaingTypeVendorApplicationType.Items[1].Selected = true;

                        }
                        if (row["C06APT"].ToString() == "03")
                        {
                            ckbPoupCreateCampaingTypeVendorApplicationType.Items[2].Selected = true;

                        }
                    }

                    foreach (DataRow row in DS?.Tables[0]?.Rows)
                    {

                        foreach (GridViewRow rowAppType in gvApplicationType.Rows)
                        {
                            Label lblAppType = (Label)rowAppType.FindControl("lblApplicationTypeCode");
                            string checkApp = lblAppType.Text.ToString();
                            if (checkApp == row["C06APT"].ToString())
                            {
                                CheckBox chk = (CheckBox)rowAppType.FindControl("cbSelect");
                                chk.Checked = true;
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_ILCP07() //load data in gridview for sub seq detail (Tab : Term | Product)
        {
            string subseqfromlist = lblSubSeqList.Text.Replace("Cmp. Sub seq : ", "");
            Int32 check_ILCP04 = 0;
            Int64 check_ILCP07 = 0;
            string copyCampaignString = Globals.copyCampaign;
            string idCampaignLoad = copyCampaignString.Replace("-", "");

            DataSet DS_ILCP04_CHECK = new DataSet();
            DataSet DS_ILCP07 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = " SELECT C07CSQ, C02INR, (C02INR + C02CRR) AS RATES, C07PIT, C07MIN, C07MAX, T42DES,T44DES FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK) " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILCP02 WITH (NOLOCK) ON C07CMP = C02CMP  AND C07CSQ = C02CSQ " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON C07PIT=T42BRD " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON C07PIT=T44ITM " +
                     " WHERE CAST(C07CMP as nvarchar) = '" + idCampaignLoad + "'";
            DS_ILCP07 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_DS_ILCP07", DS_ILCP07);
            SqlAll = "SELECT C04PTY, C04PCD FROM AS400DB01.ILOD0001.ILCP04 WITH (NOLOCK) WHERE CAST(C04CMP as nvarchar) = " + idCampaignLoad + "";
            DS_ILCP04_CHECK = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_DS_ILCP04.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS_ILCP04_CHECK);
            dataCenter.CloseConnectSQL();
            if (!campaignStorage.check_dataset(DS_ILCP07) || !campaignStorage.check_dataset(DS_ILCP04_CHECK))
            {
                lblMsgAlert.Text = "Can not find data in ILCP07 and ILCP04!";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            DataTable dtILCP07 = DS_ILCP07?.Tables[0]?.Copy();
            DataTable dt_ILCP04_CHECK = DS_ILCP04_CHECK?.Tables[0]?.Copy();


            ddlSelectProduct.ClearSelection();
            if (dt_ILCP04_CHECK.Rows.Count <= 0) //CASE : productType1 || productType4
            {
                foreach (DataRow row in dtILCP07.Rows)
                {
                    check_ILCP07 += Int64.Parse(row["C07PIT"].ToString());
                }
                if (check_ILCP07 == 0)
                {
                    //CASE : "productType1";
                    ddlSelectProduct.Items.FindByValue("productType1").Selected = true;
                    optionAddItem.Enabled = false;

                    DataTable dtAddItemAll = dtTermOfCampaignDetail();
                    foreach (DataRow drTermOfCampaign in dtILCP07.Rows)
                    {

                        string subSeq = drTermOfCampaign["C07CSQ"].ToString();
                        string rates = drTermOfCampaign["RATES"].ToString();

                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MIN"].ToString().Trim()))
                        {
                            pricesMin = double.Parse(drTermOfCampaign["C07MIN"].ToString());
                        }
                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MAX"].ToString().Trim()))
                        {
                            pricesMax = double.Parse(drTermOfCampaign["C07MAX"].ToString());
                        }

                        dtAddItemAll.Rows.Add(subSeq, rates, "0", "0", "ALL PRODUCT ITEM ", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());

                        //if (double.Parse(drTermOfCampaign["C07MIN"].ToString().Trim()) != 0 && double.Parse(drTermOfCampaign["C07MAX"].ToString().Trim()) != 0)
                        //{
                        //    dtAddItemAll.Rows.Add(subSeq, rates, "0", "0", "ALL PRODUCT ITEM ", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                        //}
                        //else
                        //{
                        //    dtAddItemAll.Rows.Add(subSeq, rates, "0", "0", "ALL PRODUCT ITEM ", pricesMax.ToString("#,##0.00").Trim());
                        //}

                    }
                    ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemAll);
                }
                else
                {
                    //CASE : = "productType4";      

                    ddlSelectProduct.Items.FindByValue("productType4").Selected = true;

                    optionAddItem.Enabled = true;
                    gvViewTermOfCampaignDetail.Columns[7].Visible = true;
                    DataTable dtAddItemAll = dtTermOfCampaignDetail();
                    foreach (DataRow drTermOfCampaign in dtILCP07.Rows)
                    {

                        string subSeq = drTermOfCampaign["C07CSQ"].ToString();
                        string rates = drTermOfCampaign["RATES"].ToString();
                        string productCode = drTermOfCampaign["C07PIT"].ToString();
                        string productDes = drTermOfCampaign["T44DES"].ToString();

                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MIN"].ToString().Trim()))
                        {
                            pricesMin = double.Parse(drTermOfCampaign["C07MIN"].ToString());
                        }
                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MAX"].ToString().Trim()))
                        {
                            pricesMax = double.Parse(drTermOfCampaign["C07MAX"].ToString());
                        }

                        dtAddItemAll.Rows.Add(subSeq, rates, "0", productCode, productDes, pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());

                        //if (double.Parse(drTermOfCampaign["C07MIN"].ToString().Trim()) != 0 && double.Parse(drTermOfCampaign["C07MAX"].ToString().Trim()) != 0)
                        //{
                        //    dtAddItemAll.Rows.Add(subSeq, rates, "0", productCode, productDes, pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                        //}
                        //else
                        //{
                        //    dtAddItemAll.Rows.Add(subSeq, rates, "0", productCode, productDes, pricesMax.ToString("#,##0.00").Trim());
                        //}

                    }
                    ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemAll);

                    DataView dv = new DataView(dtAddItemAll.Copy());
                    DataTable dtPopup = dv.ToTable(true, "Code", "ItemName");
                    dtPopup.Columns["Code"].ColumnName = "T42BRD";
                    dtPopup.Columns["ItemName"].ColumnName = "T42DES";

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtPopup);
                    ds_gv_selectlistproduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);

                }
            }
            else  //CASE : productType2 || productType3
            {

                foreach (DataRow row in dt_ILCP04_CHECK.Rows)
                {
                    check_ILCP04 += Int32.Parse(row["C04PCD"].ToString());
                }

                if (check_ILCP04 > 0)
                {
                    //CASE "productType2";
                    DataSet DS_ILCP04 = new DataSet();

                    SqlAll = @"SELECT C02CSQ, C02INR, (C02INR + C02CRR) AS RATES, C04PTY, C04PCD, C07MIN, C07MAX, T41DES FROM AS400DB01.ILOD0001.ILCP04 WITH (NOLOCK)
                             LEFT JOIN AS400DB01.ILOD0001.ILCP02 WITH (NOLOCK) ON C04CMP = C02CMP
                             LEFT JOIN AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK) ON C07CMP = C02CMP  AND C07CSQ = C02CSQ
                             LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON C04PTY = T41TYP AND C04PCD = T41COD
                             WHERE CAST(C04CMP as nvarchar) = '" + idCampaignLoad + "' ORDER BY C02CSQ ASC";

                    DS_ILCP04 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    ds_DS_ILCP04.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS_ILCP04);
                    DataTable dtILCP04 = ((DataSet)DS_ILCP04)?.Tables[0]?.Copy();

                    ddlSelectProduct.Items.FindByValue("productType2").Selected = true;
                    optionAddItem.Enabled = false;
                    DataTable dtAddItemCode = dtTermOfCampaignDetail();
                    DataTable dt_listProductCode = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);

                    foreach (DataRow drTermOfCampaign in dtILCP04.Rows)
                    {
                        string subSeq = drTermOfCampaign["C02CSQ"].ToString();
                        string rates = drTermOfCampaign["RATES"].ToString();
                        string productType = drTermOfCampaign["C04PTY"].ToString();
                        string productCode = drTermOfCampaign["C04PCD"].ToString();
                        string productDes = drTermOfCampaign["T41DES"].ToString();

                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MIN"].ToString().Trim()))
                        {
                            pricesMin = double.Parse(drTermOfCampaign["C07MIN"].ToString());
                        }
                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MAX"].ToString().Trim()))
                        {
                            pricesMax = double.Parse(drTermOfCampaign["C07MAX"].ToString());
                        }

                        dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());

                        //if (double.Parse(drTermOfCampaign["C07MIN"].ToString().Trim()) != 0 && double.Parse(drTermOfCampaign["C07MAX"].ToString().Trim()) != 0)
                        //{
                        //    dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                        //}
                        //else
                        //{
                        //    dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMax.ToString("#,##0.00").Trim());
                        //}

                    }
                    ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);
                    ds_AddItemExceptProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);

                }
                else
                {
                    //CASE "productType3";

                    DataSet DS_ILCP04 = new DataSet();

                    SqlAll = @"SELECT C02CSQ, C02INR, (C02INR + C02CRR) AS RATES, C04PTY, C04PCD, C07MIN, C07MAX, T40DES FROM AS400DB01.ILOD0001.ILCP04 WITH (NOLOCK)
                             LEFT JOIN AS400DB01.ILOD0001.ILCP02 WITH (NOLOCK) ON C04CMP = C02CMP
                             LEFT JOIN AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK) ON C07CMP = C02CMP  AND C07CSQ = C02CSQ
                             LEFT JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON C04PTY = T40TYP 
                             WHERE CAST(C04CMP as nvarchar) = '" + idCampaignLoad + "' ORDER BY C02CSQ ASC";

                    DS_ILCP04 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    ds_DS_ILCP04.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS_ILCP04);
                    DataTable dtILCP04 = ((DataSet)DS_ILCP04)?.Tables[0]?.Copy();

                    ddlSelectProduct.Items.FindByValue("productType3").Selected = true;

                    optionAddItem.Enabled = false;
                    DataTable dtAddItemCode = dtTermOfCampaignDetail();
                    DataTable dt_listProductCode = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_itemSeleced.Value);

                    foreach (DataRow drTermOfCampaign in dtILCP04.Rows)
                    {
                        string subSeq = drTermOfCampaign["C02CSQ"].ToString();
                        string rates = drTermOfCampaign["RATES"].ToString();
                        string productType = drTermOfCampaign["C04PTY"].ToString();
                        string productCode = drTermOfCampaign["C04PCD"].ToString();
                        string productDes = drTermOfCampaign["T40DES"].ToString();

                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MIN"].ToString().Trim()))
                        {
                            pricesMin = double.Parse(drTermOfCampaign["C07MIN"].ToString());
                        }
                        if (!String.IsNullOrEmpty(drTermOfCampaign["C07MAX"].ToString().Trim()))
                        {
                            pricesMax = double.Parse(drTermOfCampaign["C07MAX"].ToString());
                        }

                        dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());

                        //if (double.Parse(drTermOfCampaign["C07MIN"].ToString().Trim()) != 0 && double.Parse(drTermOfCampaign["C07MAX"].ToString().Trim()) != 0)
                        //{
                        //    dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMin.ToString("#,##0.00").Trim() + " - " + pricesMax.ToString("#,##0.00").Trim());
                        //}
                        //else
                        //{
                        //    dtAddItemCode.Rows.Add(subSeq, rates, productType, productCode, "All PRODUCT EXCEPT : " + productDes + "", pricesMax.ToString("#,##0.00").Trim());
                        //}

                    }

                    ds_AddItemProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);
                    ds_AddItemExceptProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtAddItemCode);
                }
            }
        }
        private void LOAD_DATA_ILCP08() //load data in gridview for panel 'Select Vendor' (Tab : Vendor List)
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                string[] CampaignString;
                CampaignString = copyCampaignString.ToString().Split('-');
                string idCampaignLoad = string.Empty;
                if (CampaignString.Length <= 1)
                {
                    idCampaignLoad = CampaignString[0];
                }
                else
                {
                    idCampaignLoad = CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                }
                SqlAll = $@"SELECT C08VEN,P10NAM,P16RNK,P12ODR,P12WOR,P16END FROM AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK) 
                     LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON C08VEN = P10VEN 
                     LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN 
                     LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN 
                     WHERE CAST(C08CMP as nvarchar) = '{idCampaignLoad}' AND P16END > {_updateDate} AND P10DEL=''";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    //campaignStorage.SetCookiesDataSetByName("ds_vendor_list", DS);
                    ds_vendor_list.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                }
                else
                {
                    DataTable dt = new DataTable();
                    //campaignStorage.SetCookiesDataTableByName("ds_vendor_list", dt);
                    ds_vendor_list.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                }
                gvVendorList.DataSource = DS;
                gvVendorList.DataBind();

                //Pop-up vendorlist
                DataTable dtPopup = new DataView(DS?.Tables[0]).ToTable(true, new string[] { "C08VEN", "P10NAM" });
                //dtPopup.Columns["C08VEN"].ColumnName = "P10VEN";
                if (dtPopup.Columns["C08VEN"].GetType() != typeof(string))
                {
                    dtPopup.Columns.Add("P10VEN", typeof(string));
                    foreach (DataRow dr in dtPopup.Rows)
                    {
                        dr["P10VEN"] = dr["C08VEN"].ToString();
                    }

                    dtPopup.Columns.Remove("C08VEN");
                }
                DataSet dsPopup = new DataSet();
                dsPopup.Tables.Add(dtPopup);
                //campaignStorage.SetCookMaxLargeCookie("ds_gv_selectlistvendor", dsPopup);
                ds_gv_selectlistvendor.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(dsPopup);
                btnVendorListSearch.Visible = true;

                if (!campaignStorage.check_dataset(DS))
                {
                    rdbSelectVendor.Items[0].Selected = true;
                    dataCenter.CloseConnectSQL();
                    return;
                }
                else if (campaignStorage.check_dataset(DS))
                {

                    if (HdnIsEdit1.Value == "LOAD_CAMPAIGN")
                    {
                        btnVendorListSearch.Enabled = true;
                        rdbSelectVendor.Items[0].Selected = false;
                        rdbSelectVendor.Items[1].Selected = false;
                    }
                    else
                    {
                        btnVendorListSearch.Enabled = false;

                        rdbSelectVendor.Items[1].Selected = true;
                    }

                    btnVendorListSearch.Visible = true;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_ILCP09()
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                string idCampaignLoad = copyCampaignString.Replace("-", "");

                SqlAll = "SELECT C09BRN FROM AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) WHERE CAST(C09CMP as nvarchar) = '" + idCampaignLoad + "'";

                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                campaignStorage.SetCookiesDataSetByName("ds_gridBranch", DS);
                foreach (DataRow row in DS?.Tables[0]?.Rows)
                {
                    foreach (GridViewRow rowBranch in gvBranch.Rows)
                    {

                        if (int.Parse(rowBranch.Cells[1].Text.ToString()) == int.Parse(row["C09BRN"].ToString()))
                        {
                            CheckBox chk = (CheckBox)rowBranch.FindControl("CheckBoxInsert");
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        private void LOAD_DATA_ILCP11()
        {
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                string idCampaignLoad = copyCampaignString.Replace("-", "");
                var txtProductTypeDescription = new TextBox();
                txtProductTypeDescription.Text = "";
                DataSet ds_listCampaignSqev = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                SqlAll = "SELECT C11NSQ, C11UDT, C11UTM, C11UUS, C11NOT, C11NSQ, C11NOT FROM AS400DB01.ILOD0001.ILCP11 WITH (NOLOCK) WHERE CAST(C11CMP as nvarchar) = '" + idCampaignLoad + "'";
                ds_listCampaignSqev = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                int i = 0;
                int countList = ds_listCampaignSqev.Tables[0].Rows.Count;
                string[] dateList = new string[countList];
                string[] timeList = new string[countList];
                string[] userList = new string[countList];
                string[] dashess = new string[countList];
                string[] detailCampaign1 = new string[countList];
                string[] detailCampaign2 = new string[countList];
                string[] sumDetail = new string[countList];


                foreach (DataRow row in ds_listCampaignSqev?.Tables[0]?.Rows)
                {

                    DateTime dtimeListCmapaign;
                    DateTime.TryParseExact(row["C11UDT"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out dtimeListCmapaign);
                    string dateListCmapaign = dtimeListCmapaign.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    dateList[i] = dateListCmapaign;
                    timeList[i] = String.Format("{0:##:##:##}", Convert.ToInt64(row["C11UTM"].ToString().Trim()));
                    userList[i] = row["C11UUS"].ToString().Trim();
                    detailCampaign1[i] = "<br><span style=\"margin-top:5px;margin-left:10px;\">" + row["C11NOT"].ToString() + "</span><br>";
                    detailCampaign2[i] = "<span style=\"color:#0061C1;margin-top:5px;margin-left:10px;font-weight: normal;\"> NOTE DATE:</span> " + dateList[i] + "<span style=\"color:#0061C1;font-weight: normal;\"> NOTE TIME: </span> " + timeList[i] + "<span style=\"color:#0061C1;font-weight: normal;\"> USER: </span>" + userList[i] + "<br>";
                    dashess[i] = "<span style=\"margin-top:5px;margin-left:10px;\">---------------------------------------------------------------------------------------</span><br>";
                    sumDetail[i] = detailCampaign1[i] + detailCampaign2[i] + dashess[i];

                    txtProductTypeDescription.Text += sumDetail[i];
                    txtProductTypeDescription.ForeColor = Color.Red;


                    i++;
                }

                string dataTextList = txtProductTypeDescription.Text.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Javascript", "javascript:dataListNote('" + dataTextList + "');", true);
                dataCenter.CloseConnectSQL();
                return;

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

        }

        protected void LOAD_COPY_CAMPAIGN()
        {
            string copyCampaignString = Globals.copyCampaign;
            if (!String.IsNullOrEmpty(copyCampaignString))
            {
                if (HdnIsEdit1.Value != "LOAD_CAMPAIGN" || HdnIsEdit1.Value != "EDIT_CAMPAIGN")
                {
                    if (!String.IsNullOrEmpty(copyCampaignString))
                    {
                        LOAD_DATA_ILCP01();
                        LOAD_DATA_ILCP02();
                        LOAD_DATA_ILCP05();
                        LOAD_DATA_TYPESUB_ILCP05();
                        LOAD_DATA_ILCP06();
                        LOAD_DATA_ILCP07();
                        LOAD_DATA_ILCP08();
                        LOAD_DATA_ILCP09();
                        SHOW_STATUS_CAMPAIGN();
                        gvPartyRateOptionalShareSubEvent();
                        if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
                        {
                            LOAD_DATA_ILCP11();

                        }
                    }
                    else
                    {
                        lblMsgAlert.Text = "Can't load the campaign !";
                        PopupAlertRate.ShowOnPageLoad = true;
                    }
                    //}
                }
            }
            return;
        }
        protected void LOAD_COPY_CAMPAIGN_S()
        {
            if (HdnIsEdit1.Value == "LOAD_CAMPAIGN")
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString; //= String.IsNullOrEmpty(copyCampaignString) ? null : copyCampaignString.ToString().Split('-');
                if (!String.IsNullOrEmpty(copyCampaignString))
                {
                    //if (Globals.checkFlag == "" || Globals.checkFlag != "true" || Globals.copyCampaignOld != Globals.copyCampaign)
                    if (Globals.copyCampaignOld != Globals.copyCampaign)
                    {
                        Globals.copyCampaignOld = Globals.copyCampaign;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        //int countComapaignArray = CampaignString.Length;
                        if (!String.IsNullOrEmpty(copyCampaignString))
                        {
                            txtCampaignCode.Text = "#-######-##-####";
                            LOAD_DATA_ILCP01();
                            LOAD_DATA_ILCP02();
                            LOAD_DATA_ILCP05();
                            LOAD_DATA_TYPESUB_ILCP05();
                            LOAD_DATA_ILCP06();
                            LOAD_DATA_ILCP07();
                            LOAD_DATA_ILCP08();
                            LOAD_DATA_ILCP09();
                            SHOW_STATUS_CAMPAIGN();
                            gvPartyRateOptionalShareSubEvent();
                            //Globals.checkFlag = "true";
                            //pTab.Enabled = true;
                            buttonTempCreate.Visible = false;
                            SaveButton.Visible = true;
                            //btnClearRate.Visible = false;
                            pRangeY.Visible = false;
                            TabContentControlSetVisible(true);
                            //rdoCreateCampaign.Items[0].Selected = true; ;
                        }
                        else
                        {
                            lblMsgAlert.Text = "Can't load the campaign !";
                            PopupAlertRate.ShowOnPageLoad = true;
                        }
                    }
                    pTab.Enabled = true;
                }
            }
            else if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
            {
                string copyCampaignString = Globals.copyCampaign;

                if (!String.IsNullOrEmpty(copyCampaignString))
                {
                    //if (Globals.checkFlag == "" || Globals.checkFlag != "true" || Globals.copyCampaignOld != Globals.copyCampaign)
                    if (Globals.copyCampaignOld != Globals.copyCampaign)
                    {
                        Globals.copyCampaignOld = Globals.copyCampaign;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        //int countComapaignArray = CampaignString.Length;
                        if (!String.IsNullOrEmpty(copyCampaignString))
                        {
                            LOAD_DATA_ILCP01();
                            LOAD_DATA_ILCP02();
                            LOAD_DATA_ILCP05();
                            LOAD_DATA_TYPESUB_ILCP05();
                            LOAD_DATA_ILCP06();
                            LOAD_DATA_ILCP09();
                            LOAD_DATA_ILCP11();
                            SHOW_STATUS_CAMPAIGN();
                            gvPartyRateOptionalShareSubEvent();
                            //Globals.checkFlag = "true";
                            //rdoCreateCampaign.Items[0].Selected = true; ;
                        }
                        else
                        {
                            lblMsgAlert.Text = "Can't load the campaign !";
                            PopupAlertRate.ShowOnPageLoad = true;
                        }
                    }
                }
            }

            return;
        }

        protected void BTN_LOAD_CAMPAIGN(object sender, EventArgs e)
        {
            popupLoadCampaign.ShowOnPageLoad = true;
        }
        protected void BTN_NO_LOAD_CAMPAIGN(object sender, EventArgs e)
        {
            foreach (ListItem itemCreateCampaign in rdoCreateCampaign.Items)
            {
                itemCreateCampaign.Selected = false;
            }
        }
        #endregion

        private void PopupAlertCenter()
        {
            PopupSearchItemCode.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupSearchItemCode.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupSearchItemType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupSearchItemType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupAlertRate.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertRate.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupSearchCampaign.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupSearchCampaign.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsgError.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsgError.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupConfirmSaveCampaign.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupConfirmSaveCampaign.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupConfirmNote.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupConfirmNote.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupChangePrice.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupChangePrice.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        }

        protected void gvMultiRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMultiRate.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gvMultiRate.DataSource = DS;
                gvMultiRate.DataBind();
            }
        }

        private void pRangeYSetVisible()
        {
            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y" && !String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
            {
                pOptionAddMultiTerm.Visible = false;
                pAddMultiTerm.Visible = false;
                rSubSeq.Visible = false;
                pRangeY.Visible = true;
                gvMultiRate.Columns[0].Visible = false;
                //lbEndTermRange.Visible = true;
                //txtEndTermRange.Visible = true;
                ////lbStartTerm.Text = "Start Term";
                //txtStartTerm.Enabled = false;
                //txtEndTerm.Enabled = false;
                //txtStartTerm.ReadOnly = true;
                //txtEndTerm.ReadOnly = true;
                //lbEndTerm.Visible = true;
                //txtEndTerm.Visible = true;
                //txt_addTerm.Visible = true;
                //Label51.Visible = true;
                //Label52.Visible = true;
                btnCreateCampaignOK.Enabled = false;
                ClearTermRange();
                txtTermRange.Text = _termSeq.ToString();
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N" && !String.IsNullOrEmpty(txtCreateCampaingTypeVendorBaseRate.Text.Trim()))
            {
                pOptionAddMultiTerm.Visible = true;
                rbOptionalAddMultiTerm.ClearSelection();
                rbOptionalAddMultiTerm.Enabled = true;
                gvMultiRate.Columns[0].Visible = true;
                txtTermMultiRange.Text = null;
                //rSubSeq.Visible = true;
                pRangeY.Visible = false;
                //lbEndTermRange.Visible = true;
                //txtEndTermRange.Visible = true;
                ////lbStartTerm.Text = "Add Term for Rate";
                //txtStartTerm.Enabled = false;
                //txtEndTerm.Enabled = false;
                //txtStartTerm.ReadOnly = true;
                //txtEndTerm.ReadOnly = true;
                //lbEndTerm.Visible = true;
                //txtEndTerm.Visible = true;
                //txt_addTerm.Visible = true;
                //Label51.Visible = true;
                //Label52.Visible = true;
                btnCreateCampaignOK.Enabled = false;

                ClearTermRange();

                txtSubSeq.Text = _termSeq.ToString();
                txtTermRange.Text = _termSeq.ToString();
            }
            else
            {
                pRangeY.Visible = false;
                ClearTermRange();
            }
        }

        #region Edit Grid - Not use
        protected void BindGrid(GridView gvName, DataTable dtBind, string sessionName)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dtBind);
            campaignStorage.SetCookiesDataSetByName(sessionName, ds);
            gvName.DataSource = ds;
            gvName.DataBind();
        }

        protected void gvPartyRate_OnUpdate(object sender, EventArgs e)
        {
            GridViewRow gvRow = (sender as LinkButton).NamingContainer as GridViewRow;
            string ESBSubRate = (gvRow.Cells[GetColumnIndexByName(gvPartyRate, "E.Sub Rate")].Controls[0] as TextBox).Text;
            string VendorSubRate = (gvRow.Cells[GetColumnIndexByName(gvPartyRate, "V.Sub Rate")].Controls[0] as TextBox).Text;

            //DataTable dt = GetDataFromGridview(gvPartyRate, dtShareSub());
            DataTable dt = (DataTable)ViewState["ds_party_rate"];
            dt.Rows[gvRow.RowIndex]["E.Sub Rate"] = ESBSubRate;
            dt.Rows[gvRow.RowIndex]["V.Sub Rate"] = VendorSubRate;

            ViewState["ds_party_rate"] = dt;
            gvPartyRate.DataSource = dt;
            gvPartyRate.DataBind();
            //BindGrid(gvPartyRate, dt, "ds_party_rate");
            gvPartyRate.EditIndex = -1;
        }

        protected void gvPartyRate_OnCancel(object sender, EventArgs e)
        {
            //DataTable dt = GetDataFromGridview(gvPartyRate, dtShareSub());
            gvPartyRate.EditIndex = -1;
            DataTable dt = (DataTable)ViewState["ds_party_rate"];
            gvPartyRate.DataSource = dt;
            gvPartyRate.DataBind();
        }

        protected void gvPartyRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPartyRate.EditIndex = e.NewEditIndex;

            //DataTable dt = GetDataFromGridview(gvPartyRate, dtShareSub());
            DataTable dt = (DataTable)ViewState["ds_party_rate"];
            ViewState["ds_party_rate"] = dt;
            gvPartyRate.DataSource = dt;
            gvPartyRate.DataBind();
            //BindGrid(gvPartyRate, dt, "ds_party_rate");
        }

        protected void gvPartyRate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                //DataTable dt = GetDataFromGridview(gvPartyRate, dtShareSub());
            }
            else if (e.CommandName == "Update")
            {

            }
            else if (e.CommandName == "Cancel")
            {
                gvPartyRate.EditIndex = -1;
                //DataTable dt = (DataTable)ViewState["ds_party_rate"];
                //gvPartyRate.DataSource = dt;
                //gvPartyRate.DataBind();
            }
        }

        protected void gvPartyRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPartyRate.EditIndex = -1;
            DataTable dt = (DataTable)ViewState["ds_party_rate"];
            gvPartyRate.DataSource = dt;
            gvPartyRate.DataBind();
        }

        protected void gvPartyRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = gvPartyRate.DataKeys[e.RowIndex].Value.ToString();
        }
        #endregion

        protected string ValidateRateWithBaseRate(string rateType)
        {
            ///<summary> Validation Input Rate of Customer, Vendor and ESB(EasyBuy) sum rate 3 type not over Base Rate. And return string value in decimal format.
            ///<para>
            ///- rateType have parameters in ("CUSTOMER", "VENDOR", "ESB").
            ///</para>
            ///</summary>
            string resultRte = "0.00";
            decimal BaseRte = Convert.ToDecimal(txtCreateCampaingTypeVendorBaseRate.Text);
            decimal custValue = Convert.ToDecimal(ConvertTo2Decimal(txtRateforRange.Text.Trim()));
            string CampaignType = ddlCampaignType.SelectedValue; //MAKER VENDOR ESB SHARESUB

            bool negative = false;
            decimal BaseValidate = BaseRte;

            //Check null value.
            bool custChkVal = false;
            bool vendChkVal = false;
            bool esbChkVal = false;
            bool makeChkVal = false;

            //Get value from textbox.
            decimal CustRte;
            decimal VendorRte;
            decimal ESBRte;
            decimal MakerRte;

            #region Customer Rate
            //Customer
            if (String.IsNullOrEmpty(txtRateforRange.Text))
            {
                custChkVal = false;
                CustRte = _MinInterestRate;
            }
            else
            {
                custChkVal = true;
                //decimal custValue = Convert.ToDecimal(ConvertTo2Decimal(txtRateforRange.Text.Trim()));

                if (custValue > _MaxInterestRate)
                {
                    if (custValue <= BaseRte && custValue >= _MinInterestRate)
                    {
                        CustRte = custValue;
                    }
                    else
                    {
                        CustRte = BaseRte;
                    }
                }
                else
                {
                    if (custValue <= BaseRte && custValue >= _MinInterestRate)
                    {
                        CustRte = custValue;
                    }
                    else if (custValue <= BaseRte && custValue <= _MinInterestRate)
                    {
                        CustRte = _MinInterestRate;
                    }
                    else
                    {
                        CustRte = BaseRte;
                    }
                }
                //CustRte = Convert.ToDecimal(ConvertTo2Decimal(txtRateforRange.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtRateforRange.Text.Trim()));
            }

            if (CustRte < 0)
            {
                BaseValidate = BaseValidate + (CustRte * -1);
                negative = true;
            }
            #endregion

            #region Check and Get Rate
            //Vendor
            if (String.IsNullOrEmpty(txtSubRateVendor.Text))
            {
                vendChkVal = false;
                VendorRte = 0;
            }
            else
            {
                vendChkVal = true;
                if (negative)
                {
                    VendorRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateVendor.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateVendor.Text.Trim()));
                }
                else
                {
                    VendorRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateVendor.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateVendor.Text.Trim()));
                }

                //vendChkVal = VendorRte == 0 ? false : true;
            }

            //ESB
            if (String.IsNullOrEmpty(txtSubRateESB.Text))
            {
                esbChkVal = false;
                ESBRte = 0;
            }
            else
            {
                esbChkVal = true;
                if (negative)
                {
                    ESBRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateESB.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateESB.Text.Trim()));
                }
                else
                {
                    ESBRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateESB.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateESB.Text.Trim()));
                }

                //esbChkVal = ESBRte == 0 ? false : true;
            }

            //Maker
            if (String.IsNullOrEmpty(txtSubRateMaker.Text))
            {
                makeChkVal = false;
                MakerRte = 0;
            }
            else
            {
                makeChkVal = true;
                if (negative)
                {
                    MakerRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateMaker.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateMaker.Text.Trim()));
                }
                else
                {
                    MakerRte = Convert.ToDecimal(ConvertTo2Decimal(txtSubRateMaker.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtSubRateMaker.Text.Trim()));
                }

                //makeChkVal = MakerRte == 0 ? false : true;
            }
            #endregion

            //Validate from rateType.
            decimal sumRate = CustRte + VendorRte + ESBRte + MakerRte;

            if (CampaignType.ToUpper() == "VENDOR")
            {
                #region Type VENDOR
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                txtSubRateESB.Text = "0.00";
                                txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                txtSubRateESB.Text = "0.00";
                                txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "VENDOR")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = VendorRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseRte - (CustRte + VendorRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = VendorRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "MAKER")
            {
                #region Type MAKER
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                txtSubRateESB.Text = "0.00";
                                txtSubRateMaker.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                txtSubRateESB.Text = "0.00";
                                txtSubRateMaker.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "MAKER")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + MakerRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = MakerRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseRte - (CustRte + MakerRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = MakerRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                //txtSubRateESB.Text = "0.00";
                                //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                                txtRateforRange.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "ESB")
            {
                #region Type ESB
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    //if (negative)
                    //{
                    //    if (custChkVal && !esbChkVal)
                    //    {
                    //        //CustRte = BaseRte - (VendorRte + ESBRte);
                    //        CustRte = sumRate > BaseRte ? (BaseValidate - ESBRte) : CustRte;
                    //        resultRte = CustRte.ToString("N2");

                    //        txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                    //    }
                    //    else if (!custChkVal && esbChkVal)
                    //    {
                    //        CustRte = sumRate > BaseRte ? (BaseValidate - ESBRte) : CustRte;
                    //        resultRte = CustRte.ToString("N2");

                    //        txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                    //    }
                    //    else if (custChkVal && esbChkVal)
                    //    {
                    //        resultRte = CustRte.ToString("N2");

                    //        if ((BaseValidate + (CustRte + ESBRte)) < 0)
                    //        {
                    //            txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                    //        }                       
                    //    }
                    //    else
                    //    {
                    //        resultRte = CustRte.ToString("N2");
                    //    }
                    //}
                    //else
                    //{
                    if (custChkVal && !esbChkVal)
                    {
                        CustRte = sumRate > BaseRte ? (BaseRte - ESBRte) : CustRte;
                        resultRte = CustRte.ToString("N2");

                        txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                    }
                    else if (!custChkVal && esbChkVal)
                    {
                        CustRte = sumRate > BaseRte ? (BaseRte - ESBRte) : CustRte;
                        resultRte = CustRte.ToString("N2");

                        txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                    }
                    else if (custChkVal && esbChkVal)
                    {
                        resultRte = CustRte.ToString("N2");

                        //if ((BaseRte - (CustRte + ESBRte)) < 0)
                        //{
                        //    txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                        //}
                        if (negative)
                        {
                            txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                        }
                    }
                    else
                    {
                        resultRte = CustRte.ToString("N2");
                    }
                    //}
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    //if (negative)
                    //{
                    //    if (custChkVal && !esbChkVal)
                    //    {
                    //        ESBRte = sumRate > BaseRte ? (BaseValidate - CustRte) : ESBRte;
                    //        resultRte = ESBRte.ToString("N2");

                    //        txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                    //    }
                    //    else if (!custChkVal && esbChkVal)
                    //    {
                    //        ESBRte = sumRate > BaseRte ? (BaseValidate - CustRte) : ESBRte;
                    //        resultRte = ESBRte.ToString("N2"); ;

                    //        txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                    //    }
                    //    else if (custChkVal && esbChkVal)
                    //    {
                    //        resultRte = ESBRte.ToString("N2");

                    //        if ((BaseValidate - (CustRte + ESBRte)) < 0)
                    //        {
                    //            txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        resultRte = ESBRte.ToString("N2");
                    //    }
                    //}
                    //else
                    //{
                    if (custChkVal && !esbChkVal)
                    {
                        ESBRte = sumRate > BaseRte ? (BaseRte - CustRte) : ESBRte;
                        resultRte = ESBRte.ToString("N2");

                        txtSubRateESB.Text = (BaseRte - CustRte).ToString("N2");
                    }
                    else if (!custChkVal && esbChkVal)
                    {
                        ESBRte = sumRate > BaseRte ? (BaseRte - CustRte) : ESBRte;
                        resultRte = ESBRte.ToString("N2"); ;

                        txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                    }
                    else if (custChkVal && esbChkVal)
                    {
                        resultRte = ESBRte.ToString("N2");

                        //if ((BaseRte - (CustRte + ESBRte)) < 0)
                        //{
                        //    txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                        //}
                        if (negative)
                        {
                            txtRateforRange.Text = (BaseRte - ESBRte).ToString("N2");
                        }
                    }
                    else
                    {
                        resultRte = ESBRte.ToString("N2");
                    }
                    //}
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "SHARESUB")
            {
                #region Type SHARESUB
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = CustRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = CustRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + VendorRte - MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + MakerRte - VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            //resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtSubRateESB.Text = "0.00";
                                        txtSubRateMaker.Text = "0.00";
                                        txtSubRateVendor.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = CustRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = CustRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "VENDOR")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = VendorRte.ToString("N2");

                            //if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            //{
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}

                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtSubRateVendor.Text = VendorRte.ToString("N2");
                                        txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtSubRateVendor.Text = VendorRte.ToString("N2");
                                        txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = VendorRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = VendorRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = VendorRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + VendorRte)) < 0)
                            //{
                            //    //txtSubRateESB.Text = "0.00";
                            //    //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtSubRateVendor.Text = VendorRte.ToString("N2");
                                        txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtSubRateVendor.Text = VendorRte.ToString("N2");
                                        txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = VendorRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = VendorRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + ESBRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            //resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            //{
                            //    //txtSubRateESB.Text = "0.00";
                            //    //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtSubRateESB.Text = ESBRte.ToString("N2");
                                        txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");                                
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte + VendorRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtSubRateVendor.Text = (Convert.ToDecimal(txtSubRateVendor.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtSubRateESB.Text = ESBRte.ToString("N2");
                                        txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtSubRateVendor.Text = (Convert.ToDecimal(txtSubRateVendor.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = ESBRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = ESBRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            //resultRte = ESBRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + ESBRte)) < 0)
                            //{
                            //    //txtSubRateESB.Text = "0.00";
                            //    //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtSubRateESB.Text = ESBRte.ToString("N2");
                                        txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtSubRateVendor.Text = (Convert.ToDecimal(txtSubRateVendor.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtSubRateESB.Text = ESBRte.ToString("N2");
                                        txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtSubRateVendor.Text = (Convert.ToDecimal(txtSubRateVendor.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = ESBRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = ESBRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "MAKER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = MakerRte.ToString("N2");

                            //if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            //{
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}

                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }

                            resultRte = MakerRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = MakerRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = MakerRte.ToString("N2");

                            //txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + VendorRte)) < 0)
                            //{
                            //    //txtSubRateESB.Text = "0.00";
                            //    //txtSubRateVendor.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtRateforRange.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtSubRateESB.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtSubRateVendor.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtSubRateMaker.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }

                            resultRte = MakerRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = MakerRte.ToString("N2");
                        //}
                    }
                }
                #endregion
            }

            return resultRte;
        }

        protected void ValidateTermMultiRate(int Seq, int Term)
        {
            //if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            //{
            if (Seq == 1)
            {
                txtStartTerm.Text = Seq.ToString();
                txtEndTerm.Text = Term.ToString();
            }
            else
            {
                int TotalTerm = Convert.ToInt32(txt_addTerm.Text);
                //int oldStartTerm = Convert.ToInt32(gvMultiRate.Rows[Seq - 2].Cells[GetColumnIndexByName(gvMultiRate, "CampaignStartTerm")].ToString());
                //int oldEndTerm = Convert.ToInt32(gvMultiRate.Rows[Seq - 2].Cells[GetColumnIndexByName(gvMultiRate, "CampaignEndTerm")].Text);

                //C05CSQ = SubSeq, C05RSQ = Seq
                int oldEndTerm = 1;
                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                {
                    //oldEndTerm = (from dsMultiRate in campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0]?.AsEnumerable()
                    oldEndTerm = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.AsEnumerable()
                                  where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == Convert.ToInt32(txtSubSeq.Text)
                                  orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                  select int.Parse(dsMultiRate.Field<string>("CampaignEndTerm"))).FirstOrDefault();
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    //oldEndTerm = (from dsMultiRate in campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0]?.AsEnumerable()
                    oldEndTerm = (from dsMultiRate in campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.AsEnumerable()
                                  where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == 1
                                  orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                  select int.Parse(dsMultiRate.Field<string>("CampaignEndTerm"))).FirstOrDefault();
                }

                txtStartTerm.Text = (oldEndTerm + 1).ToString();
                int endTerm = oldEndTerm + Term;

                if (endTerm <= TotalTerm)
                {
                    txtEndTerm.Text = endTerm.ToString();
                }
                else
                {
                    txtEndTerm.Text = TotalTerm.ToString();
                    txtEndTermRange.Text = (TotalTerm - oldEndTerm).ToString();
                }
            }
            //}
        }

        #region Tab Vender List
        protected void DDL_BIND_LIST_VENDOR()
        {
            //rdbSelectVendor.DataValueField = "Size_Id";
            //rdbSelectVendor.DataTextField = "Size";
            //rdbSelectVendor.DataSource = size.ToList();
            rdbSelectVendor.DataBind();
        }
        protected void rdbSelectVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbSelectVendor.SelectedValue == "SV")
            {
                gvVendorList.Enabled = true;
                //popupSearchCampaignOld.ShowOnPageLoad = true;
                btnVendorListSearch.Visible = true;

                //DataSet ds = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_vendor_list");
                DataSet ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
                if (campaignStorage.check_dataset(ds))
                {
                    gvVendorList.DataSource = ds;
                    gvVendorList.DataBind();
                }
            }
            else
            {
                btnVendorListSearch.Visible = false;

                //campaignStorage.ClearMaxLargeCookies("ds_gv_selectlistvendor");
                ds_gv_selectlistvendor.Value = string.Empty;
                campaignStorage.ClearCookies("ds_vendor_list");
                gvVendorList.DataSource = campaignStorage.GetCookiesDataSetByKey(ds_vendor_list.Value);
                gvVendorList.DataBind();
            }
        }

        #region Pop-up popupSearchCampaignOld
        protected void ckbSearchFromCampaign_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = ckbSearchFromCampaign.Checked;
            if (chk)
            {
                PopupSearchFromVendorControlSet(!chk);
                PopupSearchFromProductControlSet(!chk);
                PopupSearchFromCampaignControlSet(chk);
            }
            else
            {
                PopupSearchFromVendorControlSet(chk);
                PopupSearchFromProductControlSet(chk);
                PopupSearchFromCampaignControlSet(chk);
            }
        }

        protected void ckbSearchFromProduct_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = ckbSearchFromProduct.Checked;
            if (chk)
            {
                PopupSearchFromVendorControlSet(!chk);
                PopupSearchFromProductControlSet(chk);
                PopupSearchFromCampaignControlSet(!chk);
            }
            else
            {
                PopupSearchFromVendorControlSet(chk);
                PopupSearchFromProductControlSet(chk);
                PopupSearchFromCampaignControlSet(chk);
            }
        }

        protected void ckbSearchFromVendor_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = ckbSearchFromVendor.Checked;
            if (chk)
            {
                PopupSearchFromVendorControlSet(chk);
                PopupSearchFromProductControlSet(!chk);
                PopupSearchFromCampaignControlSet(!chk);
            }
            else
            {
                PopupSearchFromVendorControlSet(chk);
                PopupSearchFromProductControlSet(chk);
                PopupSearchFromCampaignControlSet(chk);
            }
        }

        private void PopupSearchFromVendorControlSet(bool enable)
        {
            btnSearchVendor.Enabled = enable;
            txtSearhVendorCode.Enabled = enable;
            rdoSearchFromVendorCampaignStatus.Enabled = enable;

            ckbSearchFromVendor.Checked = enable;
        }

        private void PopupSearchFromProductControlSet(bool enable)
        {
            txtSearchProductTypeCode.Enabled = enable;
            btnSearchProductType.Enabled = enable;
            txtSearchProductCode.Enabled = enable;
            btnSearchProductCode.Enabled = enable;
            txtSearchProductBarandCode.Enabled = enable;
            btnSearchProductBrand.Enabled = enable;
            txtSearchProductModelCode.Enabled = enable;
            btnSearchProductModel.Enabled = enable;
            txtSearchProductItemCode.Enabled = enable;
            btnSearchProductItem.Enabled = enable;

            ckbSearchFromProduct.Checked = enable;
        }

        private void PopupSearchFromCampaignControlSet(bool enable)
        {
            txtSearchCampaignCode.Enabled = enable;
            btnSearchCampaignCode.Enabled = enable;
            rdoSearchCampaignStatus.Enabled = enable;
            txtSearchStartDate.Enabled = enable;
            CCtxtSearchStartDate.Enabled = enable;
            btncalendarStart.Enabled = enable;
            txtSearchEndDate.Enabled = enable;
            CCtxtSearchEndDate.Enabled = enable;
            btncalendarEnd.Enabled = enable;
            txtSearchClosingLayBillDate.Enabled = enable;
            CCtxtSearchClosingLayBillDate.Enabled = enable;
            btncalendarClosing.Enabled = enable;

            ckbSearchFromCampaign.Checked = enable;
        }

        protected void btnSearchProductItemClick(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnSearchCampaignCodeClick(object sender, ImageClickEventArgs e)
        {

        }
        #endregion
        #endregion

        #region Function about insert data

        #region Function about check validation && create new id campaign
        protected void BTN_CONFIRM_SAVE(object sender, EventArgs e)
        {
            DataSet ds_CheckDate = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = "Select P97CDT from AS400DB01.ILOD0001.ILMS97 WITH (NOLOCK) where P97REC = '01'";
            cmd.CommandText = SqlAll;
            ds_CheckDate = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (!campaignStorage.check_dataset(ds_CheckDate))
            {
                lblMsgAlert.Text = "ไม่สามารถโหลดข้อมูลจาก ILMS97 ได้กรุณาลองใหม่อีกครั้ง ";
                PopupAlertRate.ShowOnPageLoad = true;
                return;
            }
            DataRow drCheckDate = ds_CheckDate?.Tables[0]?.Rows[0];
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = drCheckDate["P97CDT"].ToString();
            if (decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                p97Date = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            dateCheck = int.Parse(p97Date);
            string CalcDate1 = "";
            string CalcDate2 = "";
            string CalcDate3 = "";
            string Error1 = "";
            string Error2 = "";
            string Error3 = "";
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            // start call procedures GNP023 "Start date ต้องไม่เกิน 1 ปี"
            bool call_GNP023_1 = iLDataSubroutine.Call_GNP023(dateCheck.ToString(), "YMD", "B", "1", "Y", "+", ref CalcDate1, ref Error1, m_userInfo.BizInit, m_userInfo.BranchNo);
            bool call_GNP023_2 = iLDataSubroutine.Call_GNP023(dateCheck.ToString(), "YMD", "B", "1", "Y", "-", ref CalcDate2, ref Error2, m_userInfo.BizInit, m_userInfo.BranchNo);
            bool call_GNP023_3 = iLDataSubroutine.Call_GNP023(dateCheck.ToString(), "YMD", "B", "50", "Y", "+", ref CalcDate3, ref Error3, m_userInfo.BizInit, m_userInfo.BranchNo);

            int startDataCampaign = int.Parse(CHANGE_FORMAT_DATE(txtStartDate.Text));
            int endDataCampaign = int.Parse(CHANGE_FORMAT_DATE(txtEndDate.Text));

            foreach (GridViewRow row in gvBranch.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("CheckBoxInsert");
                if (chk.Checked)
                {
                    countCheck++;
                }
            }

            bool isCheckItemProduct = true;
            DataSet ds_CheckVendor = new DataSet();
            //ds_CheckVendor = campaignStorage.GetCookiesDataSetByKey("ds_vendor_list");
            ds_CheckVendor = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_list.Value);
            if (string.IsNullOrEmpty(ds_AddItemProduct.Value))
            {
                isCheckItemProduct = false;
            }
            else
            {
                DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
                if (!campaignStorage.check_dataTable(dt))
                {
                    isCheckItemProduct = false;
                }
            }

            if (countCheck == 0)
            {
                lblMsgAlert.Text = "กรุณาเลือก Branch อย่างน้อย 1 รายการ ";
                PopupAlertRate.ShowOnPageLoad = true;
                tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("BranchNote");
            }
            else if (!isCheckItemProduct)
            {
                lblMsgAlert.Text = " กรุณาเลือกสินค้าใน Sub Seq. Detail อย่างน้อย 1 รายการ";
                PopupAlertRate.ShowOnPageLoad = true;
                tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("TermProduct");
            }
            else if (startDataCampaign > endDataCampaign)
            {
                lblMsgAlert.Text = " กรุณาตรวจสอบวันที่ Start Date และ End Date";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (!string.IsNullOrEmpty(CalcDate1) && startDataCampaign > int.Parse(CalcDate1))
            {
                lblMsgAlert.Text = " Start Date จะต้องไม่มากกว่าวันที่ปัจจุบันเกิน 1 ปี ";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            /* Req.88652 ตัดเงื่อนไขออก
             * else if (startDataCampaign < int.Parse(CalcDate2))
            {
                lblMsgAlert.Text = " Start Date จะต้องไม่กว่าวันที่ปัจจุบันเกิน 1 ปี ";
                PopupAlertRate.ShowOnPageLoad = true;
            }*/
            else if (!string.IsNullOrEmpty(CalcDate3) && endDataCampaign > int.Parse(CalcDate3))
            {
                lblMsgAlert.Text = " End Date จะต้องไม่มากกว่าวันที่ปัจจุบันเกิน 50 ปี ";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (HdnIsEdit1.Value == "EDIT_CAMPAIGN" && startDataCampaign < int.Parse(p97Date) && (endDataCampaign > int.Parse(p97Date) && Convert.ToBoolean(hd_SaveNote.Value.ToString() == "0"))) 
            {

                lblMsgAlert.Text = " กรุณา Add Note ก่อน Save ";
                PopupAlertRate.ShowOnPageLoad = true;
            }
            else if (rdbSelectVendor.SelectedValue == "SV" && !campaignStorage.check_dataset(ds_CheckVendor))
            {
                lblMsgAlert.Text = " กรุณาเลือก vendor อย่างน้อย 1 รายการ ";
                PopupAlertRate.ShowOnPageLoad = true;
                tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("VendorList");
            }

            else
            {
                if (ddlCampaignType.Text == "MAKER")
                {
                    CampaingType = 0;

                }
                else if (ddlCampaignType.Text == "VENDOR")
                {
                    CampaingType = 1;
                }
                else if (ddlCampaignType.Text == "ESB")
                {
                    CampaingType = 2;
                }
                else if (ddlCampaignType.Text == "SHARESUB")
                {
                    CampaingType = 3;
                }

                // Code follow Delphi(.exe) in file name : ILE000001
                int CamptypeRunDefault = 45;
                CamptypeRunDefault = CamptypeRunDefault + CampaingType;
                Globals.runNumbeP99REC = string.Format("{0:000}", CamptypeRunDefault);
                DataSet DS = new DataSet();
                int tranferPlus;
                SqlAll = $@" SELECT SUBSTRING(CAST(P99RUN as varchar),1,2) AS CURRUN
                            FROM AS400DB01.ILOD0001.ILMS99 WITH (NOLOCK) 
                            WHERE P99LNT = '01' AND P99REC = '0{CamptypeRunDefault.ToString()}'";

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                DataRow drRunPrefix = DS?.Tables[0]?.Rows[0];
                if(int.Parse(_updateDate.ToString().Substring(2, 2)) < int.Parse(drRunPrefix["CURRUN"].ToString().Trim()))
                {
                    tranferPlus = int.Parse(_updateDate.ToString().Substring(2, 2) + "0001");
                }
                else
                {
                    SqlAll = " SELECT CASE WHEN SUBSTRING(CAST(P99RUN as varchar),1,2) < " + _updateDate.ToString().Substring(2, 2) + " THEN '" + _updateDate.ToString().Substring(2, 2) + "' + '0000'" +
                         " ELSE CAST(P99RUN as nvarchar) END CMPRUN " +
                         " FROM AS400DB01.ILOD0001.ILMS99 WITH (NOLOCK) " +
                         " WHERE P99LNT = '01' AND P99REC = '0" + CamptypeRunDefault + "'";
                    DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    DataRow drPrefix = DS?.Tables[0]?.Rows[0];
                    tranferPlus = int.Parse(drPrefix["CMPRUN"].ToString().Trim()) + 1;
                }
                SqlAll = $@" SELECT C01CMP
                            FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK)
                            WHERE C01STY = {CampaingType + 1} AND RIGHT(C01CMP, 6) = '{tranferPlus.ToString() }' ";
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    SqlAll = $@"SELECT TOP(1) LastSixDigits FROM
                                (SELECT RIGHT(C01CMP, 6) AS LastSixDigits
                                FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK)
                                WHERE C01STY = {CampaingType + 1}) AS TempTable
								WHERE LastSixDigits LIKE '{_updateDate.ToString().Substring(2, 2)}%'
								ORDER BY LastSixDigits DESC";
                    DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    DataRow drExPrefix = DS?.Tables[0]?.Rows[0];
                    tranferPlus = int.Parse(drExPrefix["LastSixDigits"].ToString().Trim()) + 1;
                }
                tmpCampaignCode = tranferPlus.ToString();
                Globals.runNumbeILMS99 = tranferPlus.ToString();
                dataCenter.CloseConnectSQL();

                if (CampaingType == 0)
                {
                    CmpSubtyp = 1;
                    tmpVMcode = txtMakerCode.Text.ToString().Trim();
                    CmpVom = "0";
                }
                else if (CampaingType == 1)
                {
                    CmpSubtyp = 2;
                    CmpVom = txtVendorCode.Text.ToString().Trim();
                    tmpVMcode = "0";
                }
                else if (CampaingType == 2)
                {
                    CmpSubtyp = 3;
                    tmpCampaignCode = "3999999" + tmpCampaignCode;
                    CmpVom = "0";
                    tmpVMcode = "0";
                }
                else if (CampaingType == 3)
                {
                    CmpSubtyp = 4;
                    tmpCampaignCode = "4000000" + tmpCampaignCode;
                    CmpVom = "0";
                    tmpVMcode = "0";
                }

                if (CampaingType != 3)
                {
                    if (CmpVom != "0")
                    {

                        switch (CmpVom.ToString().Length)
                        {
                            case 12:
                                tmpCampaignCode = CmpSubtyp + CmpVom.ToString().Substring(2, 6) + tmpCampaignCode;
                                break;
                            case 11:
                                tmpCampaignCode = CmpSubtyp + CmpVom.ToString().Substring(1, 6) + tmpCampaignCode;
                                break;
                        }
                    }
                }

                if (CampaingType != 3)
                {
                    if (tmpVMcode != "0")
                    {
                        switch (tmpVMcode.ToString().Length)
                        {
                            case 12:
                                tmpCampaignCode = CmpSubtyp + tmpVMcode.ToString().Substring(2, 6) + tmpCampaignCode;
                                break;
                            case 11:
                                tmpCampaignCode = CmpSubtyp + tmpVMcode.ToString().Substring(1, 6) + tmpCampaignCode;
                                break;
                        }
                    }
                }

                Globals.newIdCampaign = tmpCampaignCode;
                lblNameCampaign.Text = txtCampaignName.Text.ToString().Trim();

                if ((HdnIsEdit1.Value == "UPDATE_CAMPAIGN") || (HdnIsEdit1.Value == "EDIT_CAMPAIGN") || (HdnIsEdit1.Value == "DELETE_CAMPAIGN") || (HdnIsEdit1.Value == "VIEW_CAMPAIGN"))
                {
                    lblCodeCampaign.Text = Globals.copyCampaign;
                }
                else
                {

                    lblCodeCampaign.Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(tmpCampaignCode.ToString().Trim()));

                }

                if (HdnIsEdit1.Value == "CREATE_CAMPAIGN")
                {
                    btnStatus.Text = "INSERT";
                    lblStatusCampaign = "save";
                    lblConfirmSave.Text = "Confirm " + lblStatusCampaign + " campaign.";
                    popupConfirmSaveCampaign.ShowOnPageLoad = true;
                }
                else if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
                {
                    btnStatus.Text = "EDIT";
                    lblStatusCampaign = "edit";
                    lblConfirmSave.Text = "Confirm " + lblStatusCampaign + " campaign.";
                    popupConfirmSaveCampaign.ShowOnPageLoad = true;
                }

                else if (HdnIsEdit1.Value == "UPDATE_CAMPAIGN")
                {
                    lblStatusCampaign = "update";
                    lblConfirmSave.Text = "Confirm " + lblStatusCampaign + " campaign.";
                    popupConfirmSaveCampaign.ShowOnPageLoad = true;
                }
                else if (HdnIsEdit1.Value == "DELETE_CAMPAIGN")
                {
                    lblStatusCampaign = "delete";
                    lblConfirmSave.Text = "Confirm " + lblStatusCampaign + " campaign.";
                    popupConfirmSaveCampaign.ShowOnPageLoad = true;
                }
                else
                {
                    btnStatus.Text = "INSERT";
                    lblStatusCampaign = "save";
                    lblConfirmSave.Text = "Confirm " + lblStatusCampaign + " campaign.";
                    popupConfirmSaveCampaign.ShowOnPageLoad = true;
                }
            }
            //return;
        }
        #endregion

        #region Main Confirm Insert/Edit data all
        protected void BTN_RE_DATA_ALL(object sender, EventArgs e)
        {
            if (HdnIsEdit1.Value == "DELETE_CAMPAIGN")
            {
                Response.Redirect("~/ManageData/WorkProcess/Campaign/DeleteCampaign.aspx");
            }
            else
            {
                string campaignCode = "";
                if (HdnIsEdit1.Value == "CREATE_CAMPAIGN" || HdnIsEdit1.Value == "LOAD_CAMPAIGN")
                {
                    campaignCode = Globals.newIdCampaign;
                }
                else
                {
                    campaignCode = Globals.copyCampaign;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("ID_CAMPAIGN");
                dt.Columns.Add("STATUS_CAMPAIGN");
                dt.Rows.Add(campaignCode, "VIEW_CAMPAIGN");
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                campaignStorage.SetCookiesDataSetByName("DS_DATA_CAMPAIGN", ds);

                Response.Redirect("~/ManageData/WorkProcess/Campaign/ManageCampaign.aspx");

            }
            //Response.Redirect("~/ManageData/WorkProcess/Campaign/CreateCampaign.aspx");
        }
        private SqlParameter CreateTVP(string name, string typeName,DataTable table)
        {
            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Structured,
                TypeName = typeName,
                Value = table ?? new DataTable()
            };
        }
        protected void BTN_CONFIRM_DATA_ALL2(object sender, EventArgs e)
        {
            string copyCampaignString = Globals.copyCampaign;
            dataCenter = new DataCenter(m_userInfo);
            bool isEdit = btnStatus.Text != "INSERT";
            string campaignID = isEdit ? Globals.copyCampaign.Replace("-", "") : Globals.newIdCampaign;
            bool checkInsertILCP04 = false;
            try
            {
                var dtILCP11 = BuildILCP11(campaignID, isEdit);
                var dtILCP01 = BuildILCP01(campaignID);
                var dtILCP02 = BuildILCP02(campaignID);
                var dtILCP04 = BuildILCP04(campaignID);
                var dtILCP05 = BuildILCP05(campaignID);
                var dtILCP06 = BuildILCP06(campaignID);
                var dtILCP07 = BuildILCP07(campaignID);
                var dtILCP08 = BuildILCP08(campaignID);
                var dtILCP09 = BuildILCP09(campaignID);
                var dtILCP99 = BuildILCP99(campaignID);
                var dtILMS99 = BuildILMS99();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@campaignID", campaignID),
                    new SqlParameter("@IsEdit", isEdit),
                    CreateTVP("@ILCP11","ILOD0001.TVP_ILCP11",dtILCP11),
                    CreateTVP("@ILCP01","ILOD0001.TVP_ILCP01",dtILCP01),
                    CreateTVP("@ILCP02","ILOD0001.TVP_ILCP02",dtILCP02),
                    CreateTVP("@ILCP04","ILOD0001.TVP_ILCP04",dtILCP04),
                    CreateTVP("@ILCP05","ILOD0001.TVP_ILCP05",dtILCP05),
                    CreateTVP("@ILCP06","ILOD0001.TVP_ILCP06",dtILCP06),
                    CreateTVP("@ILCP07","ILOD0001.TVP_ILCP07",dtILCP07),
                    CreateTVP("@ILCP08","ILOD0001.TVP_ILCP08",dtILCP08),
                    CreateTVP("@ILCP09","ILOD0001.TVP_ILCP09",dtILCP09),
                    CreateTVP("@ILCP99","ILOD0001.TVP_ILCP99",dtILCP99),
                    CreateTVP("@ILMS99","ILOD0001.TVP_ILMS99",dtILMS99),

                    new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                    new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output }

                };

                var result = dataCenter.ExecuteBulk("AS400DB01.ILOD0001.sp_SaveCampaignMaster", parameters).Result;
                if (result.success)
                {
                    lblMsgCmp.Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(campaignID.ToString().Trim()));
                    lblMsgSuccess.Text = !isEdit ? "Create campaign completed" : "Edit campaign completed";
                    PopupMsgSuccess.HeaderText = "Success";
                    PopupMsgSuccess.ShowOnPageLoad = true;
                    ViewMode();


                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = !isEdit ? "Insert data error : " + ex.Message : "Edit data error : " + ex.Message;
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
            }
        }
        private DataTable BuildILCP01(string campaignID)
        {
            var dt = CreateILCP01();
            Int64 txtVendorCodes = 0;
            Int64 txtMakerCodes = 0;
            int campaingSubSidize = 0;
            int termRange = 0;
            string campaignStatus = "N";
            string waiveDuty = "N";
            switch (ddlCampaignType.Text)
            {
                case "MAKER": campaingSubSidize = 1; break;
                case "VENDOR": campaingSubSidize = 2; break;
                case "ESB": campaingSubSidize = 3; break;
                case "SHARESUB": campaingSubSidize = 4; break;
            }
            txtVendorCodes = !String.IsNullOrEmpty(txtVendorCode.Text.ToString().Trim()) ? Int64.Parse(txtVendorCode.Text.ToString().Trim()) : 0;
            txtMakerCodes = !String.IsNullOrEmpty(txtMakerCode.Text.ToString().Trim()) ? Int64.Parse(txtMakerCode.Text.ToString().Trim()) : 0;
            termRange = rbCreateCampaingTypeVendorCampaignSupport.Text == "N" ? 1 : 2;
            
            dt.Rows.Add(
                campaignID,
                loanType,
                !String.IsNullOrEmpty(campaignStorage.GetCookiesStringByKey("branch")) ? campaignStorage.GetCookiesStringByKey("branch") : m_userInfo.BranchNo,
                campaingSubSidize,
                rdoTypeSub.Text.Trim(),
                rdoCreateCampaingTypeVendorCalculateType.Text.Trim(),
                rbCreateCampaingTypeVendorCampaignSupport.Text.Trim(),
                txtCampaignName.Text.Trim(),
                txtProductDetail.Text.Trim(),
                txtVendorCodes,
                txtMakerCodes,
                CHANGE_FORMAT_DATE(txtStartDate.Text.Trim()),
                CHANGE_FORMAT_DATE(txtEndDate.Text.Trim()),
                CHANGE_FORMAT_DATE(txtClosingApplicationDate.Text.Trim()),
                CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.Trim()),
                txtXDue.Text.Trim(),
                txtFInstall.Text.Trim(),
                txtBaseRate.Text.Trim(),
                termRange,
                campaignStatus,
                CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.Trim()),
                txtMarketingCode.Text.Trim(),
                waiveDuty,
                _updateDate,
                _updateTime,
                m_userInfo.Username,
                "ILE000001F",
                m_userInfo.LocalClient.PadRight(10).Substring(0, 10).Trim(),
                rdoVendorPayment.Text.Trim(),
                int.Parse(txtVendorCreditDays.Text.Trim()),
                txtSpecialPremium.Text.Trim()
            );

            return dt;
        }
        private DataTable BuildILCP02(string campaignID)
        {
            var dt = CreateILCP02();

            foreach (GridViewRow row in gvViewTermOfCampaign.Rows)
            {
                decimal termOfCampaign = Convert.ToDecimal(row.Cells[1].Text);
                decimal subSeq = Convert.ToDecimal(row.Cells[2].Text);
                decimal seq = Convert.ToDecimal(row.Cells[3].Text);
                decimal termOfRange = Convert.ToDecimal(row.Cells[4].Text);
                decimal fromTerm = Convert.ToDecimal(row.Cells[5].Text);
                decimal term = Convert.ToDecimal(row.Cells[6].Text);

                decimal intRate = Convert.ToDecimal(
                    ((TextBox)row.FindControl("tbC02INR")).Text
                );
                decimal cruRate = Convert.ToDecimal(
                    ((TextBox)row.FindControl("tbC02CRR")).Text
                );
                decimal infRate = Convert.ToDecimal(
                    ((TextBox)row.FindControl("tbC02IFR")).Text
                );

                decimal airIntRate = Convert.ToDecimal(row.Cells[11].Text);
                decimal acrCruRate = Convert.ToDecimal(row.Cells[12].Text);

                dt.Rows.Add(
                    campaignID,        // C02CMP
                    subSeq,            // C02CSQ
                    seq,               // C02RSQ
                    fromTerm,          // C02FMT
                    term,              // C02TOT
                    airIntRate,        // C02AIR
                    acrCruRate,        // C02ACR
                    intRate,           // C02INR
                    cruRate,           // C02CRR
                    infRate,           // C02IFR
                    0m,                // C02INS
                    0m,                // C02SPR
                    0m,                // C02EPR
                    termOfRange,       // C02TTR
                    termOfCampaign,    // C02TTM
                    _updateDate,       // C02UDT
                    _updateTime,       // C02UTM
                    m_userInfo.Username,                     // C02UUS
                    "ILE000001F",                            // C02UPG
                    m_userInfo.LocalClient.PadRight(10)
                        .Substring(0, 10).Trim()           // C02UWS
                );
            }

            return dt;
        }
        public DataTable BuildILCP04(string campaignID)
        {
            var dt = CreateILCP04();

            if ((ddlSelectProduct.Text == "productType2") || (ddlSelectProduct.Text == "productType3"))
            {
                int pit = 0;

                DataTable dtItemProduct =
                    campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemExceptProduct.Value);

                dtItemProduct = dtItemProduct
                    .AsEnumerable()
                    .GroupBy(r => new
                    {
                        Type = r["Type"].ToString(),
                        Code = r["Code"].ToString()
                    })
                    .Select(g => g.First())
                    .CopyToDataTable();

                foreach (DataRow row in dtItemProduct.Rows)
                {
                    var dr = dt.NewRow();

                    dr["C04CMP"] = decimal.Parse(campaignID);
                    dr["C04PTY"] = decimal.Parse(row["Type"].ToString());
                    dr["C04PCD"] = decimal.Parse(row["Code"].ToString());
                    dr["C04PIT"] = pit;
                    dr["C04UDT"] = _updateDate;
                    dr["C04UTM"] = _updateTime;
                    dr["C04UUS"] = m_userInfo.Username;
                    dr["C04UPG"] = "ILE000001F";
                    dr["C04UWS"] = m_userInfo.LocalClient
                                        .PadRight(10)
                                        .Substring(0, 10)
                                        .Trim();
                    dr["C04RST"] = "";

                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        public DataTable BuildILCP05(string campaignID)
        {
            var dt = CreateILCP05();

            m_userInfo = userInfoService.GetUserInfo();

            DataSet dsPartyRate;
            if (!string.IsNullOrEmpty(ds_party_rate.Value))
                dsPartyRate = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_party_rate.Value);
            else
                dsPartyRate = campaignStorage.GetCookiesDataSetByKey("ds_party_rate");

            foreach (DataRow row in dsPartyRate.Tables[0].Rows)
            {
                foreach (ListItem typeList in ckbShareSub.Items)
                {
                    if (!typeList.Selected) continue;

                    string par;
                    decimal subRate;
                    decimal pcd;

                    if (typeList.Value == "Maker")
                    {
                        par = "M";
                        subRate = string.IsNullOrEmpty(row["MSUBRATE"].ToString())
                            ? 0m
                            : decimal.Parse(row["MSUBRATE"].ToString());
                        pcd = string.IsNullOrEmpty(row["MAKER"].ToString())
                            ? 0m
                            : decimal.Parse(row["MAKER"].ToString());
                    }
                    else if (typeList.Value == "Vendor")
                    {
                        par = "V";
                        subRate = string.IsNullOrEmpty(row["VSUBRATE"].ToString())
                            ? 0m
                            : decimal.Parse(row["VSUBRATE"].ToString());
                        pcd = string.IsNullOrEmpty(row["VENDOR"].ToString())
                            ? 0m
                            : decimal.Parse(row["VENDOR"].ToString());
                    }
                    else // ESB
                    {
                        par = "E";
                        subRate = string.IsNullOrEmpty(row["ESUBRATE"].ToString())
                            ? 0m
                            : decimal.Parse(row["ESUBRATE"].ToString());
                        pcd = 0m;
                    }

                    var dr = dt.NewRow();

                    dr["C05CMP"] = decimal.Parse(campaignID);
                    dr["C05CSQ"] = decimal.Parse(row["C05CSQ"].ToString());
                    dr["C05RSQ"] = decimal.Parse(row["C05RSQ"].ToString());
                    dr["C05PAR"] = par;
                    dr["C05PCD"] = pcd;
                    dr["C05SBT"] = row["C05SBT"].ToString();
                    dr["C05SIR"] = subRate;
                    dr["C05SCR"] = 0m;
                    dr["C05SFR"] = 0m;
                    dr["C05STR"] = subRate;
                    dr["C05SFM"] = decimal.Parse(row["C02FMT"].ToString());
                    dr["C05STO"] = decimal.Parse(row["C05STO"].ToString());
                    dr["C05SST"] = decimal.Parse(row["C05SSTS"].ToString());
                    dr["C05EST"] = decimal.Parse(row["C05ESTS"].ToString());
                    dr["C05STM"] = decimal.Parse(row["C05STM"].ToString());
                    dr["C05UDT"] = _updateDate;
                    dr["C05UTM"] = _updateTime;
                    dr["C05UUS"] = m_userInfo.Username;
                    dr["C05UPG"] = "ILE000001F";
                    dr["C05UWS"] = m_userInfo.LocalClient
                                        .PadRight(10)
                                        .Substring(0, 10)
                                        .Trim();
                    dr["C05RST"] = "";

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        public DataTable BuildILCP06(string campaignID)
        {
            var dt = CreateILCP06();

            m_userInfo = userInfoService.GetUserInfo();

            foreach (GridViewRow row in gvApplicationType.Rows)
            {
                var chk = (CheckBox)row.FindControl("cbSelect");
                if (!chk.Checked)
                    continue;

                var lbl = (Label)row.FindControl("lblApplicationTypeCode");


                var dr = dt.NewRow();
                dr["C06CMP"] = decimal.Parse(campaignID);
                dr["C06APT"] = lbl.Text.Trim();
                dr["C06UDT"] = _updateDate;
                dr["C06UTM"] = _updateTime;
                dr["C06UUS"] = m_userInfo.Username;
                dr["C06UPG"] = "ILE000001F";
                dr["C06UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C06RST"] = "";   

                dt.Rows.Add(dr);
            }

            return dt;
        }
        public DataTable BuildILCP07(string campaignID)
        {
            var dt = CreateILCP07();

            m_userInfo = userInfoService.GetUserInfo();

            if (ddlSelectProduct.SelectedValue == "productType2"
                || ddlSelectProduct.SelectedValue == "productType3")
            {
                dt_AddItemExceptProduct =
                    campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);

                dt_AddItemExceptProduct =
                    dt_AddItemExceptProduct.AsEnumerable()
                        .GroupBy(r => r["SubSeq"])
                        .Select(g => g.OrderBy(r => r["Type"]).First())
                        .CopyToDataTable();
            }
            else
            {
                dt_AddItemExceptProduct =
                    campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
            }

            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            {
                dt_AddItemExceptProduct =
                    dt_AddItemExceptProduct.AsEnumerable()
                        .GroupBy(r => new { Col1 = r["SubSeq"],Col2 = r["Type"],Col3 = r["Code"] })
                        .Select(g => g.OrderBy(r => r["Type"]).First())
                        .CopyToDataTable();
            }

            foreach (DataRow row in dt_AddItemExceptProduct.Rows)
            {
                double priceMin = 0;
                double priceMax = 0;

                string[] splitPrice = row["Price"].ToString().Split('-');
                if (splitPrice.Length == 2)
                {
                    priceMin = double.Parse(splitPrice[0]);
                    priceMax = double.Parse(splitPrice[1]);
                }
                else
                {
                    priceMin = 0;
                    priceMax = double.Parse(splitPrice[0]);
                }

                decimal productItemCode =
                    (ddlSelectProduct.SelectedValue == "productType2"
                    || ddlSelectProduct.SelectedValue == "productType3")
                    ? 0
                    : decimal.Parse(row["Code"].ToString());

                var dr = dt.NewRow();
                dr["C07CMP"] = decimal.Parse(campaignID);
                dr["C07CSQ"] = decimal.Parse(row["SubSeq"].ToString());
                dr["C07LNT"] = loanType;
                dr["C07PIT"] = productItemCode;
                dr["C07FIX"] = "N";
                dr["C07PRC"] = 0m;
                dr["C07MIN"] = (decimal)priceMin;
                dr["C07MAX"] = (decimal)priceMax;
                dr["C07DOW"] = 0m;
                dr["C07UDT"] = _updateDate;
                dr["C07UTM"] = _updateTime;
                dr["C07UUS"] = m_userInfo.Username;
                dr["C07UPG"] = "ILE000001F";
                dr["C07UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C07RST"] = "";

                dt.Rows.Add(dr);
            }

            return dt;
        }
        public DataTable BuildILCP08(string campaignID)
        {
            var dt = CreateILCP08();

            m_userInfo = userInfoService.GetUserInfo();

            // กรณีไม่มี vendor เลย → insert vendor = 0
            if (gvVendorList.Rows.Count <= 0)
            {
                var dr = dt.NewRow();
                dr["C08CMP"] = decimal.Parse(campaignID);
                dr["C08VEN"] = 0;
                dr["C08UDT"] = _updateDate;
                dr["C08UTM"] = _updateTime;
                dr["C08UUS"] = m_userInfo.Username;
                dr["C08UPG"] = "ILE000001F";
                dr["C08UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C08RST"] = "";

                dt.Rows.Add(dr);
                return dt;
            }

            // มี vendor หลายรายการ
            foreach (GridViewRow row in gvVendorList.Rows)
            {
                long vendorId = long.Parse(row.Cells[1].Text);

                var dr = dt.NewRow();
                dr["C08CMP"] = decimal.Parse(campaignID);
                dr["C08VEN"] = vendorId;
                dr["C08UDT"] = _updateDate;
                dr["C08UTM"] = _updateTime;
                dr["C08UUS"] = m_userInfo.Username;
                dr["C08UPG"] = "ILE000001F";
                dr["C08UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C08RST"] = "";

                dt.Rows.Add(dr);
            }

            return dt;
        }
        public DataTable BuildILCP09(string campaignID)
        {
            var dt = CreateILCP09();

            m_userInfo = userInfoService.GetUserInfo();

            foreach (GridViewRow row in gvBranch.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("CheckBoxInsert");
                if (!chk.Checked)
                    continue;

                int branchCode = int.Parse(row.Cells[1].Text);

                var dr = dt.NewRow();
                dr["C09CMP"] = decimal.Parse(campaignID);
                dr["C09BRN"] = branchCode;
                dr["C09UDT"] = _updateDate;
                dr["C09UTM"] = _updateTime;
                dr["C09UUS"] = m_userInfo.Username;
                dr["C09UPG"] = "ILE000001F";
                dr["C09UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C09RST"] = "";

                dt.Rows.Add(dr);
            }

            return dt;
        }
        private DataTable BuildILCP11(string campaignID, bool isEdit)
        {
            m_userInfo = userInfoService.GetUserInfo();
            var dt = CreateILCP11();
            var noteList = new List<string>();

            /* ===============================
               1. Prepare Note
            ================================ */
            if (!string.IsNullOrWhiteSpace(txtNote.Text))
            {
                if (!isEdit)
                    noteList.Add("Created new campaign");

                noteList.AddRange(SplitStringByLenght(txtNote.Text));
            }
            else if (!isEdit)
            {
                noteList.Add("Created new campaign");
            }

            if (!noteList.Any())
                return dt;

            /* ===============================
               2. NSQ Logic
               - INSERT  : NSQ = 1
               - EDIT    : ให้ SP handle (delete+insert)
            ================================ */
            int nsq = 1;

            /* ===============================
               3. Build Rows
            ================================ */
            for (int i = 0; i < noteList.Count; i++)
            {
                var dr = dt.NewRow();
                dr["C11CMP"] = decimal.Parse(campaignID);
                dr["C11NSQ"] = nsq;
                dr["C11LSQ"] = i + 1;
                dr["C11NOT"] = noteList[i];
                dr["C11UDT"] = _updateDate;
                dr["C11UTM"] = _updateTime;
                dr["C11UUS"] = m_userInfo.Username;
                dr["C11UPG"] = "ILE000001F";
                dr["C11UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();

                dt.Rows.Add(dr);
                
            }

            return dt;
        }
        public DataTable BuildILCP99(string campaignID)
        {
            var dt = CreateILCP99();

            m_userInfo = userInfoService.GetUserInfo();

            foreach (GridViewRow row in gvBranch.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("CheckBoxInsert");
                if (!chk.Checked)
                    continue;

                int branchCode = int.Parse(row.Cells[1].Text);

                string cbr, edt, ust;
                if (branchCode == 1)
                {
                    cbr = "Y";
                    edt = "Y";
                    ust = "Y";
                }
                else
                {
                    cbr = "N";
                    edt = "N";
                    ust = "N";
                }

                var dr = dt.NewRow();
                dr["C99CMP"] = decimal.Parse(campaignID);
                dr["C99BRN"] = branchCode;
                dr["C99CBR"] = cbr;
                dr["C99EDT"] = edt;
                dr["C99UST"] = ust;
                dr["C99SPM"] = m_userInfo.Username;
                dr["C99UDT"] = _updateDate;
                dr["C99UTM"] = _updateTime;
                dr["C99UUS"] = m_userInfo.Username;
                dr["C99UPG"] = "ILE000001F";
                dr["C99UWS"] = m_userInfo.LocalClient
                                    .PadRight(10)
                                    .Substring(0, 10)
                                    .Trim();
                dr["C99RST"] = "";

                dt.Rows.Add(dr);
            }

            return dt;
        }
        public DataTable BuildILMS99()
        {
            var dt = CreateILMS99();

            m_userInfo = userInfoService.GetUserInfo();

            string p99Rec = Globals.runNumbeP99REC;
            decimal p99Run = decimal.Parse(Globals.runNumbeILMS99);

            var dr = dt.NewRow();

            dr["P99LNT"] = "01";
            dr["P99REC"] = p99Rec;
            dr["P99RUN"] = p99Run;
            dr["P99UPD"] = _updateDate;
            dr["P99TIM"] = _updateTime;
            dr["P99UPG"] = "";                     // เดิม set ว่าง
            dr["P99USR"] = m_userInfo.Username;
            dr["P99DSP"] = m_userInfo.LocalClient
                                .PadRight(10)
                                .Substring(0, 10)
                                .Trim();

            dt.Rows.Add(dr);

            return dt;
        }

        protected void BTN_CONFIRM_DATA_ALL(object sender, EventArgs e)
        {
            string copyCampaignString = Globals.copyCampaign;
            bool checkInsertILCP11 = false, checkInsertILCP01 = false, checkInsertILCP02 = false, checkInsertILCP04 = false, checkInsertILCP05 = false, checkInsertILCP06 = false, checkInsertILCP07 = false, checkInsertILCP08 = false, checkInsertILCP09 = false, checkInsertILCP99 = false, checkInsertILMS99 = false;
            bool checkEditILCP01 = false, checkEditILCP02 = false, checkEditILCP04 = false, checkEditILCP05 = false, checkEditILCP06 = false, checkEditILCP07 = false, checkEditILCP08 = false, checkEditILCP09 = false, checkEditILCP99 = false;
            dataCenter = new DataCenter(m_userInfo);
            //if(Convert.ToInt32(_updateDate.ToString()) < Convert.ToInt32(txtEndDate.Text.ToString()))
            //{
            //    lblMsg.Text = "กรุณา Add Note ก่อน Save";
            //    PopupMsgError.HeaderText = "Error";
            //    PopupMsgError.ShowOnPageLoad = true;
            //}
            if (btnStatus.Text == "INSERT")
            {
                try
                {
                    INSERT_ILCP11(dataCenter, ref checkInsertILCP11, true); //success
                    INSERT_ILCP01(dataCenter, ref checkInsertILCP01); //success            
                    INSERT_ILCP02(dataCenter, ref checkInsertILCP02); //success
                    INSERT_ILCP04(dataCenter, ref checkInsertILCP04); //success      
                    INSERT_ILCP05(dataCenter, ref checkInsertILCP05); //success 
                    INSERT_ILCP06(dataCenter, ref checkInsertILCP06); //success
                    INSERT_ILCP07(dataCenter, ref checkInsertILCP07); //success
                    INSERT_ILCP08(dataCenter, ref checkInsertILCP08); //success
                    INSERT_ILCP09(dataCenter, ref checkInsertILCP09); //success
                    INSERT_ILCP99(dataCenter, ref checkInsertILCP99); //success
                    INSERT_ILMS99(dataCenter, ref checkInsertILMS99); //success

                    if ((checkInsertILCP11 == true) && (checkInsertILCP01 == true) && (checkInsertILCP02 == true) && (checkInsertILCP04 == true) && (checkInsertILCP05 == true) && (checkInsertILCP06 == true) && (checkInsertILCP07 == true) && (checkInsertILCP08 == true) && (checkInsertILCP09 == true) && (checkInsertILCP99 == true) && (checkInsertILMS99 == true))
                    {

                        string formatCampaignId = Globals.newIdCampaign.ToString();
                        lblMsgCmp.Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(formatCampaignId.ToString().Trim()));
                        lblMsgSuccess.Text = "Create campaign completed";
                        PopupMsgSuccess.HeaderText = "Success";
                        PopupMsgSuccess.ShowOnPageLoad = true;

                        dataCenter.CommitMssql();
                        dataCenter.CloseConnectSQL();

                        //ViewMode();
                        //SHOW_STATUS_CAMPAIGN();
                    }
                    else
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Insert data error : " + ex.Message;
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                }

            }
            else if (btnEditCampaign.Text == "Edit")
            {
                try
                {

                    EDIT_ILCP01(dataCenter, ref checkEditILCP01);
                    EDIT_ILCP02(dataCenter, ref checkEditILCP02, ref checkInsertILCP02);
                    EDIT_ILCP04(dataCenter, ref checkEditILCP04, ref checkInsertILCP04);
                    EDIT_ILCP05(dataCenter, ref checkEditILCP05, ref checkInsertILCP05);
                    EDIT_ILCP06(dataCenter, ref checkEditILCP06, ref checkInsertILCP06);
                    EDIT_ILCP07(dataCenter, ref checkEditILCP07, ref checkInsertILCP07);
                    EDIT_ILCP08(dataCenter, ref checkEditILCP08, ref checkInsertILCP08);
                    EDIT_ILCP09(dataCenter, ref checkEditILCP09, ref checkInsertILCP09);
                    EDIT_ILCP99(dataCenter, ref checkEditILCP99, ref checkInsertILCP99);

                    if ((checkEditILCP01 == true) &&
                        (checkEditILCP02 == true && checkInsertILCP02 == true) &&
                        (checkEditILCP04 == true && checkInsertILCP04 == true) &&
                        (checkEditILCP05 == true && checkInsertILCP05 == true) &&
                        (checkEditILCP06 == true && checkInsertILCP06 == true) &&
                        (checkEditILCP07 == true && checkInsertILCP07 == true) &&
                        (checkEditILCP08 == true && checkInsertILCP08 == true) &&
                        (checkEditILCP09 == true && checkInsertILCP09 == true) &&
                        (checkEditILCP99 == true && checkInsertILCP99 == true))
                    {
                        string copyCampaignStrings = Globals.copyCampaign;
                        //string[] CampaignString;
                        //CampaignString = copyCampaignStrings.ToString().Split('-');

                        /*lblMsgCmp.Text = copyCampaignString.Replace("-", "");*/ //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                        lblMsgCmp.Text = copyCampaignString;
                        lblMsgSuccess.Text = "Edit campaign completed";
                        PopupMsgSuccess.HeaderText = "Success";
                        PopupMsgSuccess.ShowOnPageLoad = true;

                        dataCenter.CommitMssql();
                        cmd.Parameters.Clear();
                        dataCenter.CloseConnectSQL();
                        ViewMode();
                    }
                    else
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Insert data error : " + ex.Message;
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                }
            }
        }
        #endregion

        #region Insert data ilcp01
        private void INSERT_ILCP01(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP01)// Insert main
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            List<ILCP01Model> ILCP01Table = new List<ILCP01Model>();
            try
            {
                string campaignID = "";
                if (btnStatus.Text == "INSERT")
                {
                    campaignID = Globals.newIdCampaign;
                }
                else
                {
                    string copyCampaignString = Globals.copyCampaign;
                    //string[] CampaignString;
                    //CampaignString = copyCampaignString.ToString().Split('-');

                    campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                }
                Int64 txtVendorCodes = 0;
                Int64 txtMakerCodes = 0;
                int campaingSubSidize = 0;
                string campaignStatus = "N";
                string waiveDuty = "N";
                int termRange = 0;

                if (ddlCampaignType.Text == "MAKER")
                {
                    campaingSubSidize = 1;
                }
                else if (ddlCampaignType.Text == "VENDOR")
                {
                    campaingSubSidize = 2;
                }
                else if (ddlCampaignType.Text == "ESB")
                {
                    campaingSubSidize = 3;
                }
                else if (ddlCampaignType.Text == "SHARESUB")
                {
                    campaingSubSidize = 4;
                }

                if (!String.IsNullOrEmpty(txtVendorCode.Text.ToString().Trim()))
                {
                    txtVendorCodes = Int64.Parse(txtVendorCode.Text.ToString().Trim());
                }
                else
                {
                    txtVendorCodes = 0;
                }

                if (!String.IsNullOrEmpty(txtMakerCode.Text.ToString().Trim()))
                {
                    txtMakerCodes = Int64.Parse(txtMakerCode.Text.ToString().Trim());
                }
                else
                {
                    txtMakerCodes = 0;
                }

                if (rbCreateCampaingTypeVendorCampaignSupport.Text == "N")
                {
                    termRange = 1;
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.Text == "Y")
                {
                    termRange = 2;
                }

                SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP01(
                                                C01CMP,
                                                C01LTY,
                                                C01BRN,
                                                C01STY,
                                                C01SBT,
                                                C01CTY,
                                                C01RNG,
                                                C01CNM,
                                                C01PTY,
                                                C01VDC,
                                                C01MKC,
                                                C01SDT,
                                                C01EDT,
                                                C01CAD,
                                                C01CLD,
                                                C01NXD,
                                                C01FIN,
                                                C01SRT,
                                                C01TRG,
                                                C01CST,
                                                C01INV,
                                                C01MKT,
                                                C01WDT,
                                                C01UDT,
                                                C01UTM,
                                                C01UUS,
                                                C01UPG,
                                                C01UWS,
                                                C01VTY,
                                                C01VCR,
                                                c01pmt)
                                          VALUES(     
                                                {campaignID},
                                                {"'" + loanType + "'"},
                                                {campaignStorage.GetCookiesStringByKey("branch")},
                                                {campaingSubSidize},
                                                {"'" + rdoTypeSub.Text.ToString().Trim() + "'"},
                                                {"'" + rdoCreateCampaingTypeVendorCalculateType.Text.ToString().Trim() + "'"},
                                                {"'" + rbCreateCampaingTypeVendorCampaignSupport.Text.ToString().Trim() + "'"},
                                                {"'" + txtCampaignName.Text.ToString().Trim() + "'"},
                                                {"'" + txtProductDetail.Text.ToString().Trim() + "'"},
                                                {txtVendorCodes},
                                                {txtMakerCodes},
                                                {CHANGE_FORMAT_DATE(txtStartDate.Text.ToString().Trim())},
                                                {CHANGE_FORMAT_DATE(txtEndDate.Text.ToString().Trim())},
                                                {CHANGE_FORMAT_DATE(txtClosingApplicationDate.Text.ToString().Trim())},
                                                {CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim())},
                                                {txtXDue.Text.ToString().Trim()},
                                                {txtFInstall.Text.ToString().Trim()},
                                                {txtBaseRate.Text.ToString().Trim()},
                                                {termRange},
                                                {"'" + campaignStatus + "'"},
                                                {CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim())},
                                                {"'" + txtMarketingCode.Text.ToString().Trim() + "'"},
                                                {"'" + waiveDuty + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"},
                                                {"'" + rdoVendorPayment.Text.ToString().Trim() + "'"},
                                                {int.Parse(txtVendorCreditDays.Text.ToString().Trim())},
                                                {"'" + txtSpecialPremium.Text.ToString().Trim() + "'"})";

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C01CMP", campaignID);
                //cmd.Parameters.Add("C01LTY", loanType);
                //cmd.Parameters.Add("C01BRN", cookiesStorage.GetCookiesStringByKey("branch"));
                //cmd.Parameters.Add("C01STY", campaingSubSidize);
                //cmd.Parameters.Add("C01SBT", rdoTypeSub.Text.ToString().Trim());
                //cmd.Parameters.Add("C01CTY", rdoCreateCampaingTypeVendorCalculateType.Text.ToString().Trim());
                //cmd.Parameters.Add("C01RNG", rbCreateCampaingTypeVendorCampaignSupport.Text.ToString().Trim());
                //cmd.Parameters.Add("C01CNM", txtCampaignName.Text.ToString().Trim());
                //cmd.Parameters.Add("C01PTY", txtProductDetail.Text.ToString().Trim());
                //cmd.Parameters.Add("C01VDC", txtVendorCodes);
                //cmd.Parameters.Add("C01MKC", txtMakerCodes);
                //cmd.Parameters.Add("C01SDT", CHANGE_FORMAT_DATE(txtStartDate.Text.ToString().Trim()));
                //cmd.Parameters.Add("C01EDT", CHANGE_FORMAT_DATE(txtEndDate.Text.ToString().Trim()));
                //cmd.Parameters.Add("C01CAD", CHANGE_FORMAT_DATE(txtClosingApplicationDate.Text.ToString().Trim()));
                //cmd.Parameters.Add("C01CLD", CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim()));
                //cmd.Parameters.Add("C01NXD", txtXDue.Text.ToString().Trim());
                //cmd.Parameters.Add("C01FIN", txtFInstall.Text.ToString().Trim());
                //cmd.Parameters.Add("C01SRT", txtBaseRate.Text.ToString().Trim());
                //cmd.Parameters.Add("C01TRG", termRange);
                //cmd.Parameters.Add("C01CST", campaignStatus);
                //cmd.Parameters.Add("C01INV", CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim()));
                //cmd.Parameters.Add("C01MKT", txtMarketingCode.Text.ToString().Trim());
                //cmd.Parameters.Add("C01WDT", waiveDuty);
                //cmd.Parameters.Add("C01UDT", _updateDate);
                //cmd.Parameters.Add("C01UTM", _updateTime);
                //cmd.Parameters.Add("C01UUS", m_userInfo.Username.ToString());
                //cmd.Parameters.Add("C01UPG", "ILE000001F");
                //cmd.Parameters.Add("C01UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                //cmd.Parameters.Add("C01VTY", rdoVendorPayment.Text.ToString().Trim());
                //cmd.Parameters.Add("C01VCR", int.Parse(txtVendorCreditDays.Text.ToString().Trim()));
                //cmd.Parameters.Add("c01pmt", txtSpecialPremium.Text.ToString().Trim());

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP01 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error insert into : ILCP01 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkInsertILCP01 = false;

                    return;
                }
                else
                {
                    checkInsertILCP01 = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP01!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP01 = false;
                return;
            }
        }
        #endregion
        #region Insert data ilcp02
        private void INSERT_ILCP02(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP02) //(Tab : Term | Product) insert data in gridview for "view Term of Campaign" panel
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string campaignID = "";
                if (btnStatus.Text == "INSERT")
                {
                    campaignID = Globals.newIdCampaign;
                }
                else
                {
                    string copyCampaignString = Globals.copyCampaign;
                    //string[] CampaignString;
                    //CampaignString = copyCampaignString.ToString().Split('-');

                    campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                }
                foreach (GridViewRow rowViewTermOfCampaign in gvViewTermOfCampaign.Rows)
                {
                    //TextBox termOfCampaigns = (TextBox)rowViewTermOfCampaign.FindControl("tbC02TTM");
                    int termOfCampaign = int.Parse(rowViewTermOfCampaign.Cells[1].Text);
                    //TextBox subSeqs = (TextBox)rowViewTermOfCampaign.FindControl("tbC02CSQ");
                    int subSeq = int.Parse(rowViewTermOfCampaign.Cells[2].Text);
                    //TextBox Seqs = (TextBox)rowViewTermOfCampaign.FindControl("tbC02RSQ");
                    int Seq = int.Parse(rowViewTermOfCampaign.Cells[3].Text);
                    //TextBox termOfRanges = (TextBox)rowViewTermOfCampaign.FindControl("tbC02TTR");
                    int termOfRange = int.Parse(rowViewTermOfCampaign.Cells[4].Text);
                    //TextBox fromTerms = (TextBox)rowViewTermOfCampaign.FindControl("tbC02FMT");
                    int fromTerm = int.Parse(rowViewTermOfCampaign.Cells[5].Text);
                    //TextBox terms = (TextBox)rowViewTermOfCampaign.FindControl("tbC02TOT");
                    int term = int.Parse(rowViewTermOfCampaign.Cells[6].Text);
                    TextBox intRates = (TextBox)rowViewTermOfCampaign.FindControl("tbC02INR");
                    float intRate = float.Parse(intRates.Text.ToString());
                    TextBox cruRates = (TextBox)rowViewTermOfCampaign.FindControl("tbC02CRR");
                    float cruRate = float.Parse(cruRates.Text.ToString());
                    TextBox infRates = (TextBox)rowViewTermOfCampaign.FindControl("tbC02IFR");
                    float infRate = float.Parse(infRates.Text.ToString());

                    float installment = 0;
                    float startPrice = 0;
                    float endPrice = 0;

                    //TextBox airIntRates = (TextBox)rowViewTermOfCampaign.FindControl("tbC02AIR");
                    float airIntRate = float.Parse(rowViewTermOfCampaign.Cells[11].Text);
                    //TextBox acrCruRates = (TextBox)rowViewTermOfCampaign.FindControl("tbC02ACR");
                    float acrCruRate = float.Parse(rowViewTermOfCampaign.Cells[12].Text);

                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP02(
                                                C02CMP,
                                                C02CSQ,
                                                C02RSQ,
                                                C02FMT,
                                                C02TOT,
                                                C02AIR,
                                                C02ACR,
                                                C02INR,
                                                C02CRR,
                                                C02IFR,
                                                C02INS,
                                                C02SPR,
                                                C02EPR,
                                                C02TTR,
                                                C02TTM,
                                                C02UDT,
                                                C02UTM,
                                                C02UUS,
                                                C02UPG,
                                                C02UWS)
                                          VALUES(     
                                                {campaignID},
                                                {subSeq},
                                                {Seq},
                                                {fromTerm},
                                                {term},
                                                {airIntRate},
                                                {acrCruRate},
                                                {intRate},
                                                {cruRate},
                                                {infRate},
                                                {installment},
                                                {startPrice},
                                                {endPrice},
                                                {termOfRange},
                                                {termOfCampaign},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"}, 
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("C02CMP", campaignID); //CMP
                    //cmd.Parameters.Add("C02CSQ", subSeq); //CSQ
                    //cmd.Parameters.Add("C02RSQ", Seq); //RSQ
                    //cmd.Parameters.Add("C02FMT", fromTerm); //FMT
                    //cmd.Parameters.Add("C02TOT", term); //TOT
                    //cmd.Parameters.Add("C02AIR", airIntRate); //AIR
                    //cmd.Parameters.Add("C02ACR", acrCruRate); //ACR
                    //cmd.Parameters.Add("C02INR", intRate); //INR
                    //cmd.Parameters.Add("C02CRR", cruRate); //CRR
                    //cmd.Parameters.Add("C02IFR", infRate); //IFR
                    //cmd.Parameters.Add("C02INS", installment); //INS
                    //cmd.Parameters.Add("C02SPR", startPrice); //SPR
                    //cmd.Parameters.Add("C02EPR", endPrice); //EPR
                    //cmd.Parameters.Add("C02TTR", termOfRange); //TTR
                    //cmd.Parameters.Add("C02TTM", termOfCampaign); //TTM
                    //cmd.Parameters.Add("C02UDT", _updateDate);
                    //cmd.Parameters.Add("C02UTM", _updateTime);
                    //cmd.Parameters.Add("C02UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("C02UPG", "ILE000001F");
                    //cmd.Parameters.Add("C02UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                    cmd.CommandText = SqlAll;

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resILCP02 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resILCP02 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        lblMsg.Text = "Error insert into : ILCP02 \n Please check command !";
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                        checkInsertILCP02 = false;
                        return;
                    }
                    else
                    {
                        checkInsertILCP02 = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP02!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP02 = false;
                return;
            }
        }
        #endregion
        #region Insert data ilcp04
        private void INSERT_ILCP04(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP04)
        {
            // (Tab : Term | Product) panel of Campaign Sub Seq.Detail from gridview 
            // "CASE ---> select All product item and Have except item(Code)" 
            // "CASE ---> select All product item and Have except item(Type)"
            if ((ddlSelectProduct.Text == "productType2") || (ddlSelectProduct.Text == "productType3"))
            {
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                try
                {

                    string campaignID = "";
                    if (btnStatus.Text == "INSERT")
                    {
                        campaignID = Globals.newIdCampaign;
                    }
                    else
                    {
                        string copyCampaignString = Globals.copyCampaign;
                        //string[] CampaignString;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                    }
                    int TMPC04ITM = 0;
                    DataTable dtItemProduct = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemExceptProduct.Value);
                    dtItemProduct = dtItemProduct.AsEnumerable().GroupBy(r => new { Col1 = r["Type"], Col2 = r["Code"] })
                                          .Select(g => g.OrderBy(r => r["Type"]).First())
                                          .CopyToDataTable();
                    foreach (DataRow rowItemProduct in dtItemProduct.Rows)
                    {
                        int typeItem = int.Parse(rowItemProduct["Type"].ToString());
                        int codeItem = int.Parse(rowItemProduct["Code"].ToString());

                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP04(
                                                C04CMP,
                                                C04PTY,
                                                C04PCD,
                                                C04PIT,
                                                C04UDT,
                                                C04UTM,
                                                C04UUS,
                                                C04UPG,
                                                C04UWS)
                                          VALUES(     
                                                {campaignID},
                                                {typeItem},
                                                {codeItem},
                                                {TMPC04ITM},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("@C04CMP", campaignID);
                        //cmd.Parameters.Add("@C04PTY", typeItem);
                        //cmd.Parameters.Add("@C04PCD", codeItem);
                        //cmd.Parameters.Add("@C04PIT", TMPC04ITM);
                        //cmd.Parameters.Add("@C04UDT", _updateDate);
                        //cmd.Parameters.Add("@C04UTM", _updateTime);
                        //cmd.Parameters.Add("@C04UUS", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("@C04UPG", "ILE000001F");
                        //cmd.Parameters.Add("@C04UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP04 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP04 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP04 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP04 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP04 = true;
                        }
                    }
                }
                catch (Exception)
                {
                    lblMsg.Text = "Error catch : Check exception : ILCP04!";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    checkInsertILCP04 = false;
                    return;
                }
            }
            else
            {
                checkInsertILCP04 = true;
            }
        }
        #endregion
        #region Insert data ilcp05
        private void INSERT_ILCP05(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP05)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            DataSet dsPartyRate = new DataSet();
            if (!string.IsNullOrEmpty(ds_party_rate.Value))
            {
                dsPartyRate = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_party_rate.Value);
            }
            else
            {
                dsPartyRate = campaignStorage.GetCookiesDataSetByKey("ds_party_rate");
            }
            try
            {
                foreach (DataRow rowPartyRate in dsPartyRate?.Tables[0]?.Rows)
                {
                    foreach (ListItem typeList in ckbShareSub.Items)
                    {
                        if (typeList.Selected)
                        {

                            if (typeList.Value == "Maker")
                            {
                                nameTypeSub = "M";

                                if (String.IsNullOrEmpty(rowPartyRate["MSUBRATE"].ToString()))
                                {
                                    subRate = 0;
                                }
                                else
                                {
                                    subRate = double.Parse(rowPartyRate["MSUBRATE"].ToString());
                                }

                                if (String.IsNullOrEmpty(rowPartyRate["MAKER"].ToString()))
                                {
                                    codeTypeShareSub = 0;
                                }
                                else
                                {
                                    codeTypeShareSub = Int64.Parse(rowPartyRate["MAKER"].ToString());
                                }

                            }


                            if (typeList.Value == "Vendor")
                            {
                                nameTypeSub = "V";
                                if (String.IsNullOrEmpty(rowPartyRate["VSUBRATE"].ToString()))
                                {
                                    subRate = 0;
                                }
                                else
                                {
                                    subRate = double.Parse(rowPartyRate["VSUBRATE"].ToString());
                                }

                                if (String.IsNullOrEmpty(rowPartyRate["VENDOR"].ToString()))
                                {
                                    codeTypeShareSub = 0;
                                }
                                else
                                {
                                    codeTypeShareSub = Int64.Parse(rowPartyRate["VENDOR"].ToString());
                                }

                            }


                            if (typeList.Value == "ESB")
                            {
                                nameTypeSub = "E";
                                if (String.IsNullOrEmpty(rowPartyRate["ESUBRATE"].ToString()))
                                {
                                    subRate = 0;
                                }
                                else
                                {
                                    subRate = double.Parse(rowPartyRate["ESUBRATE"].ToString());
                                }
                                codeTypeShareSub = 0;
                            }


                            string campaignID = "";
                            if (btnStatus.Text == "INSERT")
                            {
                                campaignID = Globals.newIdCampaign;
                            }
                            else
                            {
                                string copyCampaignString = Globals.copyCampaign;
                                //string[] CampaignString;
                                //CampaignString = copyCampaignString.ToString().Split('-');

                                campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                            }
                            int subSeq = int.Parse(rowPartyRate["C05CSQ"].ToString());
                            int seq = int.Parse(rowPartyRate["C05RSQ"].ToString());
                            string subType = rowPartyRate["C05SBT"].ToString();
                            int fromTerm = int.Parse(rowPartyRate["C02FMT"].ToString());
                            int term = int.Parse(rowPartyRate["C05STO"].ToString());
                            int startSubTerm = int.Parse(rowPartyRate["C05SSTS"].ToString());
                            int EndSubTerm = int.Parse(rowPartyRate["C05ESTS"].ToString());
                            int totalTerm = int.Parse(rowPartyRate["C05STM"].ToString());

                            SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP05(
                                                C05CMP,
                                                C05CSQ,
                                                C05RSQ,
                                                C05PAR,
                                                C05PCD,
                                                C05SBT,
                                                C05SIR,
                                                C05SCR,
                                                C05SFR,
                                                C05STR,
                                                C05SFM,
                                                C05STO,
                                                C05SST,
                                                C05EST,
                                                C05STM,
                                                C05UDT,
                                                C05UTM,
                                                C05UUS,
                                                C05UPG,
                                                C05UWS)
                                          VALUES(     
                                                {campaignID},
                                                {subSeq},
                                                {seq},
                                                {"'" + nameTypeSub + "'"},
                                                {"'" + codeTypeShareSub + "'"},
                                                {"'" + subType + "'"},
                                                {subRate},
                                                {"'0.00'"},
                                                {"'0.00'"},
                                                {subRate},
                                                {fromTerm},
                                                {term},
                                                {startSubTerm},
                                                {EndSubTerm},
                                                {totalTerm},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                            //cmd.Parameters.Clear();
                            //cmd.Parameters.Add("C05CMP", campaignID);
                            //cmd.Parameters.Add("C05CSQ", subSeq);
                            //cmd.Parameters.Add("C05RSQ", seq);
                            //cmd.Parameters.Add("C05PAR", nameTypeSub);
                            //cmd.Parameters.Add("C05PCD", codeTypeShareSub);
                            //cmd.Parameters.Add("C05SBT", subType);
                            //cmd.Parameters.Add("C05SIR", subRate);
                            //cmd.Parameters.Add("C05SCR", "0.00");
                            //cmd.Parameters.Add("C05SFR", "0.00");
                            //cmd.Parameters.Add("C05STR", subRate);
                            //cmd.Parameters.Add("C05SFM", fromTerm);
                            //cmd.Parameters.Add("C05STO", term);
                            //cmd.Parameters.Add("C05SST", startSubTerm);
                            //cmd.Parameters.Add("C05EST", EndSubTerm);
                            //cmd.Parameters.Add("C05STM", totalTerm);
                            //cmd.Parameters.Add("C05UDT", _updateDate);
                            //cmd.Parameters.Add("C05UTM", _updateTime);
                            //cmd.Parameters.Add("C05UUS", m_userInfo.Username.ToString());
                            //cmd.Parameters.Add("C05UPG", "ILE000001F");
                            //cmd.Parameters.Add("C05UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                            cmd.CommandText = SqlAll;

                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int resILCP05 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (resILCP05 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();
                                lblMsg.Text = "Error insert into : ILCP05 \n Please check command !";
                                PopupMsgError.HeaderText = "Error";
                                PopupMsgError.ShowOnPageLoad = true;
                                checkInsertILCP05 = false;
                                return;
                            }
                            else
                            {
                                checkInsertILCP05 = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Error catch : Check exception : ILCP05!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP05 = false;
                return;
            }
        }
        #endregion
        #region Insert data ilcp06
        private void INSERT_ILCP06(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP06) // (Tab : Share Sub.) panel for application gridview
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {

                foreach (GridViewRow rowAppType in gvApplicationType.Rows)
                {
                    string campaignID = "";
                    if (btnStatus.Text == "INSERT")
                    {
                        campaignID = Globals.newIdCampaign;
                    }
                    else
                    {
                        string copyCampaignString = Globals.copyCampaign;
                        //string[] CampaignString;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                    }

                    Label lblAppType = (Label)rowAppType.FindControl("lblApplicationTypeCode");
                    CheckBox chkAppType = (CheckBox)rowAppType.FindControl("cbSelect");
                    string applicationTypeSelect = lblAppType.Text.ToString().Trim();
                    if (chkAppType.Checked)
                    {
                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP06(
                                                C06CMP,
                                                C06APT,
                                                C06UDT,
                                                C06UTM,
                                                C06UUS,
                                                C06UPG,
                                                C06UWS)
                                          VALUES(     
                                                {campaignID},
                                                {"'" + applicationTypeSelect + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                { "'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("C06CMP", campaignID);
                        //cmd.Parameters.Add("C06APT", applicationTypeSelect);
                        //cmd.Parameters.Add("C06UDT", _updateDate);
                        //cmd.Parameters.Add("C06UTM", _updateTime);
                        //cmd.Parameters.Add("C06UUS", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("C06UPG", "ILE000001F");
                        //cmd.Parameters.Add("C06UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP06 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP06 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP06 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP06 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP06 = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Error catch : Check exception : ILCP06!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP06 = false;
                return;
            }
        }
        #endregion
        #region Insert data ilcp07
        private void INSERT_ILCP07(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP07)  // (Tab : Term | Product) panel of Campaign Sub Seq.Detail from gridview "CASE -> select some product item"
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string campaignID = "";
                if (btnStatus.Text == "INSERT")
                {
                    campaignID = Globals.newIdCampaign;
                }
                else
                {
                    string copyCampaignString = Globals.copyCampaign;
                    //string[] CampaignString;
                    //CampaignString = copyCampaignString.ToString().Split('-');

                    campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                }

                if (ddlSelectProduct.SelectedValue == "productType2" || ddlSelectProduct.SelectedValue == "productType3")
                {
                    dt_AddItemExceptProduct = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
                    dt_AddItemExceptProduct = dt_AddItemExceptProduct.AsEnumerable().GroupBy(r => new { Col1 = r["Subseq"] })
                                          .Select(g => g.OrderBy(r => r["Type"]).First())
                                          .CopyToDataTable();

                }
                else
                {
                    dt_AddItemExceptProduct = campaignStorage.JsonDeserializeObjectHiddenDataTable(ds_AddItemProduct.Value);
                }

                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    dt_AddItemExceptProduct = dt_AddItemExceptProduct.AsEnumerable().GroupBy(g => new { Col1 = g["SubSeq"], Col2 = g["Type"], Col3 = g["Code"] })
                                                                        .Select(s => s.OrderBy(g => g["Type"]).FirstOrDefault())
                                                                        .CopyToDataTable();
                }

                foreach (DataRow row in dt_AddItemExceptProduct.Rows)
                {
                    string[] splitPrice = row["Price"].ToString().Split('-');
                    if (splitPrice.Length == 2)
                    {
                        pricesMin = Double.Parse(splitPrice[0]);
                        pricesMax = Double.Parse(splitPrice[1]);
                    }
                    else
                    {
                        pricesMin = Double.Parse("0.00");
                        pricesMax = Double.Parse(splitPrice[0]);
                    }

                    if (ddlSelectProduct.SelectedValue == "productType2" || ddlSelectProduct.SelectedValue == "productType3")
                    {
                        productItemCode = 0;
                    }
                    else
                    {
                        productItemCode = int.Parse(row["Code"].ToString());
                    }
                    int subSeq = int.Parse(row["SubSeq"].ToString());
                    string fixPrice = "N";
                    double priceDefault = 0.00;
                    double downPrice = 0.00;
                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP07(
                                                C07CMP,
                                                C07CSQ,
                                                C07LNT,
                                                C07PIT,
                                                C07FIX,
                                                C07PRC,
                                                C07MIN,
                                                C07MAX,
                                                C07DOW,
                                                C07UDT,
                                                C07UTM,
                                                C07UUS,
                                                C07UPG,
                                                C07UWS)
                                          VALUES(     
                                                {campaignID},
                                                {subSeq},
                                                {"'" + loanType + "'"},
                                                {productItemCode},
                                                {"'" + fixPrice + "'"},
                                                {priceDefault},
                                                {pricesMin},
                                                {pricesMax},
                                                {downPrice},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";


                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("C07CMP", campaignID);
                    //cmd.Parameters.Add("C07CSQ", subSeq);
                    //cmd.Parameters.Add("C07LNT", loanType);
                    //cmd.Parameters.Add("C07PIT", productItemCode);
                    //cmd.Parameters.Add("C07FIX", fixPrice);
                    //cmd.Parameters.Add("C07PRC", priceDefault);
                    //cmd.Parameters.Add("C07MIN", pricesMin);
                    //cmd.Parameters.Add("C07MAX", pricesMax);
                    //cmd.Parameters.Add("C07DOW", downPrice);
                    //cmd.Parameters.Add("C07UDT", _updateDate);
                    //cmd.Parameters.Add("C07UTM", _updateTime);
                    //cmd.Parameters.Add("C07UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("C07UPG", "ILE000001F");
                    //cmd.Parameters.Add("C07UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                    cmd.CommandText = SqlAll;

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resILCP07 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resILCP07 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        lblMsg.Text = "Error insert into : ILCP07 \n Please check command !";
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                        checkInsertILCP07 = false;
                        return;
                    }
                    else
                    {
                        checkInsertILCP07 = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP07!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP07 = false;
                return;
            }
        }
        #endregion
        #region Insert data ilcp08
        private void INSERT_ILCP08(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP08)// (Tab : Vendor List) panel of Select Vendor from gridview 
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;

            try
            {
                if (gvVendorList.Rows.Count <= 0)
                {
                    string campaignID = "";
                    if (btnStatus.Text == "INSERT")
                    {
                        campaignID = Globals.newIdCampaign;
                    }
                    else
                    {
                        string copyCampaignString = Globals.copyCampaign;
                        //string[] CampaignString;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                    }

                    Int64 vendorIdSelected = 0;

                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP08(
                                                C08CMP,
                                                C08VEN,
                                                C08UDT,
                                                C08UTM,
                                                C08UUS,
                                                C08UPG,
                                                C08UWS)
                                          VALUES(     
                                                {campaignID},
                                                {vendorIdSelected},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("C08CMP", campaignID);
                    //cmd.Parameters.Add("C08VEN", vendorIdSelected);
                    //cmd.Parameters.Add("C08UDT", _updateDate);
                    //cmd.Parameters.Add("C08UTM", _updateTime);
                    //cmd.Parameters.Add("C08UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("C08UPG", "ILE000001F");
                    //cmd.Parameters.Add("C08UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                    cmd.CommandText = SqlAll;

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resILCP08 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resILCP08 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        lblMsg.Text = "Error insert into : ILCP08 \n Please check command !";
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                        checkInsertILCP08 = false;
                        return;
                    }
                    else
                    {
                        checkInsertILCP08 = true;
                    }


                }
                else
                {
                    string campaignID = "";
                    if (btnStatus.Text == "INSERT")
                    {
                        campaignID = Globals.newIdCampaign;
                    }
                    else
                    {
                        string copyCampaignString = Globals.copyCampaign;
                        //string[] CampaignString;
                        //CampaignString = copyCampaignString.ToString().Split('-');

                        campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                    }

                    foreach (GridViewRow row in gvVendorList.Rows)
                    {
                        Int64 vendorIdSelected = Int64.Parse(row.Cells[1].Text);

                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP08(
                                                C08CMP,
                                                C08VEN,
                                                C08UDT,
                                                C08UTM,
                                                C08UUS,
                                                C08UPG,
                                                C08UWS)
                                          VALUES(     
                                                {campaignID},
                                                {vendorIdSelected},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("C08CMP", campaignID);
                        //cmd.Parameters.Add("C08VEN", vendorIdSelected);
                        //cmd.Parameters.Add("C08UDT", _updateDate);
                        //cmd.Parameters.Add("C08UTM", _updateTime);
                        //cmd.Parameters.Add("C08UUS", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("C08UPG", "ILE000001F");
                        //cmd.Parameters.Add("C08UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP08 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP08 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP08 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP08 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP08 = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP08!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP08 = false;
                return;
            }

            ////dataCenter.CommitMssql();
            ////cmd.Parameters.Clear();
            //dataCenter.CloseConnectSQL();
        }
        #endregion
        #region Insert data ilcp09
        private void INSERT_ILCP09(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP09) // (Tab : Branch | Note) panel branch into gridview
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                foreach (GridViewRow row in gvBranch.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("CheckBoxInsert");
                    if (chk.Checked)
                    {
                        string campaignID = "";
                        if (btnStatus.Text == "INSERT")
                        {
                            campaignID = Globals.newIdCampaign;
                        }
                        else
                        {
                            string copyCampaignString = Globals.copyCampaign;
                            //string[] CampaignString;
                            //CampaignString = copyCampaignString.ToString().Split('-');

                            campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                        }

                        int branchSelect = int.Parse(row.Cells[1].Text);
                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP09(
                                                C09CMP,
                                                C09BRN,
                                                C09UDT,
                                                C09UTM,
                                                C09UUS,
                                                C09UPG,
                                                C09UWS)
                                          VALUES(     
                                                {campaignID},
                                                {branchSelect},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("C09CMP", campaignID);
                        //cmd.Parameters.Add("C09BRN", branchSelect);
                        //cmd.Parameters.Add("C09UDT", _updateDate);
                        //cmd.Parameters.Add("C09UTM", _updateTime);
                        //cmd.Parameters.Add("C09UUS", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("C09UPG", "ILE000001F");
                        //cmd.Parameters.Add("C09UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP09 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP09 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP09 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP09 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP09 = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP09!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP09 = false;
                return;
            }

            ////dataCenter.CommitMssql();
            ////cmd.Parameters.Clear();
            //dataCenter.CloseConnectSQL();
        }
        #endregion
        #region Insert data ilcp10
        private void INSERT_ILCP10()
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                //          
                cmd.Parameters.Add("C01UDT", _updateDate);
                cmd.Parameters.Add("C01UTM", _updateTime);
                cmd.Parameters.Add("C01UUS", m_userInfo.Username.ToString());
                cmd.Parameters.Add("C01UPG", "ILE000001F");
                cmd.Parameters.Add("C01UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                cmd.Parameters.Add("C01VTY", 11111);
                cmd.Parameters.Add("C01VCR", 11111);
                cmd.Parameters.Add("c01pmt", 11111);
            }
            catch (Exception)
            {
                lblMsg.Text = "Error catch : Check exception !";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
        }
        #endregion
        #region Insert data ilcp11 
        private void INSERT_ILCP11(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP11, bool isCreatedNewCampaign = false)  // (Tab : Branch |Note) about Note
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string campaignID = "";
                List<string> noteArray = new List<string>();
                if (btnStatus.Text == "INSERT")
                {
                    campaignID = Globals.newIdCampaign;
                    int checkNote = 0;
                    if (String.IsNullOrEmpty(txtNote.Text.ToString()))
                    {
                        checkNote = 0;
                    }
                    else
                    {
                        checkNote = 1;
                        noteArray.Add("Created new campaign");
                        noteArray.AddRange(SplitStringByLenght(txtNote.Text.ToString()));
                    }

                    if (checkNote == 0)
                    {
                        string txtFixNote = "Created new campaign";

                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP11(
                                                c11cmp,
                                                c11nsq,
                                                c11lsq,
                                                c11not,
                                                c11udt,
                                                c11utm,
                                                c11uus,
                                                c11upg,
                                                c11uws)
                                          VALUES(     
                                                {campaignID},
                                                {1},
                                                {1},
                                                {"'" + txtFixNote + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP11 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP11 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP11 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP11 = true;
                        }
                    }
                    else if (checkNote == 1)
                    {
                        for (int i = 0; i < noteArray.Count; i++)
                        {
                            string txtFixNote = noteArray[i].ToString();

                            SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP11(
                                                c11cmp,
                                                c11nsq,
                                                c11lsq,
                                                c11not,
                                                c11udt,
                                                c11utm,
                                                c11uus,
                                                c11upg,
                                                c11uws)
                                          VALUES(     
                                                {campaignID},
                                                {1},
                                                { i + 1 },
                                                {"'" + txtFixNote + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                { "'ILE000001F'"},
                                                { "'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                            cmd.CommandText = SqlAll;

                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int resILCP11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (resILCP11 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();
                                lblMsg.Text = "Error insert into : ILCP11 \n Please check command !";
                                PopupMsgError.HeaderText = "Error";
                                PopupMsgError.ShowOnPageLoad = true;
                                checkInsertILCP11 = false;
                                return;
                            }
                            else
                            {
                                checkInsertILCP11 = true;
                            }
                        }
                    }

                }
                else if (HdnIsEdit1.Value == "EDIT_CAMPAIGN")
                {
                    campaignID = Globals.copyCampaign.Replace("-", "");

                    //Get lastest version of NOTEs.
                    SqlAll = @"SELECT TOP 1 C11NSQ FROM AS400DB01.ILOD0001.ILCP11 WITH (NOLOCK) WHERE CAST(C11CMP as nvarchar) = '" + campaignID + "' ORDER BY C11NSQ DESC";

                    DataSet ds = new DataSet();
                    ds = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    int lastC11NSQ = 0;
                    if (ds != null)
                    {
                        if (ds?.Tables[0]?.Rows.Count > 0)
                        {
                            lastC11NSQ = Convert.ToInt32(ds?.Tables[0].Rows[0]["C11NSQ"].ToString());
                        }
                    }

                    //Insert to ILCP11
                    noteArray = SplitStringByLenght(txtNote.Text.ToString());
                    if (noteArray != null || !noteArray.Any())
                    {
                        for (int i = 0; i < noteArray.Count; i++)
                        {
                            string txtFixNote = "";
                            txtFixNote = noteArray[i].ToString();

                            SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP11(
                                                c11cmp,
                                                c11nsq,
                                                c11lsq,
                                                c11not,
                                                c11udt,
                                                c11utm,
                                                c11uus,
                                                c11upg,
                                                c11uws)
                                          VALUES(     
                                                {campaignID},
                                                {lastC11NSQ + 1},
                                                { i + 1 },
                                                {"'" + txtFixNote + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                { "'ILE000001F'"},
                                                { "'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                            cmd.CommandText = SqlAll;

                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            int resILCP11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (resILCP11 == -1)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();
                                lblMsg.Text = "Error insert into : ILCP11 \n Please check command !";
                                PopupMsgError.HeaderText = "Error";
                                PopupMsgError.ShowOnPageLoad = true;
                                checkInsertILCP11 = false;
                                return;
                            }
                            else
                            {
                                checkInsertILCP11 = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP11!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP11 = false;
                return;
            }

            if (!isCreatedNewCampaign)
            {
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
            }

            checkInsertILCP11 = true;
        }

        private List<string> SplitStringByLenght(string input)
        {
            List<string> results = new List<string>();
            int count = 0;
            string temp = "";

            foreach (char c in input)
            {
                temp += c;
                count++;
                if (count == 70)
                {
                    results.Add(temp);
                    temp = "";
                    count = 0;
                }
            }

            if (temp != "")
                results.Add(temp);

            return results.ToList();
        }

        #endregion
        #region Insert data ILCP99
        private void INSERT_ILCP99(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILCP99)// (Tab : Branch | Note) panel branch into gridview
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                foreach (GridViewRow row in gvBranch.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("CheckBoxInsert");
                    if (chk.Checked)
                    {
                        string campaignID = "";
                        if (btnStatus.Text == "INSERT")
                        {
                            campaignID = Globals.newIdCampaign;
                        }
                        else
                        {
                            string copyCampaignString = Globals.copyCampaign;
                            //string[] CampaignString;
                            //CampaignString = copyCampaignString.ToString().Split('-');

                            campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                        }

                        int branchSelects = int.Parse(row.Cells[1].Text);
                        string txtC99CBR = "";
                        string txtC99EDT = "";
                        string txtC99UST = "";
                        if (branchSelects == 1)
                        {
                            txtC99CBR = "Y";
                            txtC99EDT = "Y";
                            txtC99UST = "Y";
                        }
                        else
                        {
                            txtC99CBR = "N";
                            txtC99EDT = "N";
                            txtC99UST = "N";
                        }
                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILCP99(
                                                C99CMP,
                                                C99BRN,
                                                C99CBR,
                                                C99EDT,
                                                C99UST,
                                                C99SPM,
                                                C99UDT,
                                                C99UTM,
                                                C99UUS,
                                                C99UPG,
                                                C99UWS)
                                          VALUES(     
                                                {campaignID},
                                                {branchSelects},
                                                {"'" + txtC99CBR + "'"},
                                                {"'" + txtC99EDT + "'"},
                                                {"'" + txtC99UST + "'"},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {_updateDate},
                                                {_updateTime},
                                                {"'" + m_userInfo.Username.ToString() + "'"},
                                                {"'ILE000001F'"},
                                                {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"})";

                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("C99CMP", campaignID);
                        //cmd.Parameters.Add("C99BRN", branchSelects);
                        //cmd.Parameters.Add("C99CBR", txtC99CBR);
                        //cmd.Parameters.Add("C99EDT", txtC99EDT);
                        //cmd.Parameters.Add("C99UST", txtC99UST);
                        //cmd.Parameters.Add("C99SPM", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("C99UDT", _updateDate);
                        //cmd.Parameters.Add("C99UTM", _updateTime);
                        //cmd.Parameters.Add("C99UUS", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("C99UPG", "ILE000001F");
                        //cmd.Parameters.Add("C99UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                        cmd.CommandText = SqlAll;

                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resILCP99 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resILCP99 == -1)
                        {
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            lblMsg.Text = "Error insert into : ILCP99 \n Please check command !";
                            PopupMsgError.HeaderText = "Error";
                            PopupMsgError.ShowOnPageLoad = true;
                            checkInsertILCP99 = false;
                            return;
                        }
                        else
                        {
                            checkInsertILCP99 = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP99!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILCP99 = false;
                return;
            }
        }
        #endregion
        #region Insert data ILMS99
        private void INSERT_ILMS99(EB_Service.DAL.DataCenter dataCenter, ref bool checkInsertILMS99) //insert index run number for 4 type
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string P99REC = Globals.runNumbeP99REC;
                int P99RUN = int.Parse(Globals.runNumbeILMS99);
                SqlAll = $@"UPDATE AS400DB01.ILOD0001.ILMS99 
                                    SET P99RUN = {P99RUN},
                                        P99UPD = {_updateDate},                   
                                        P99TIM = {_updateTime},
                                        P99USR = {"'" + m_userInfo.Username.ToString() + "'"},
                                        P99UPG = {"''"},
                                        P99DSP = {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"}
                                    WHERE P99LNT = '01' AND P99REC = '" + P99REC + "'";

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("P99RUN", P99RUN);
                //cmd.Parameters.Add("P99UPD", _updateDate);
                //cmd.Parameters.Add("P99TIM", _updateTime);
                //cmd.Parameters.Add("P99USR", m_userInfo.Username.ToString());
                //cmd.Parameters.Add("P99UPG", "ILE000001F");
                //cmd.Parameters.Add("P99DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resIMS99 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resIMS99 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error insert into : ILMS99 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkInsertILMS99 = false;
                    return;
                }
                else
                {
                    checkInsertILMS99 = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILMS99!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkInsertILMS99 = false;
                return;
            }
        }
        #endregion

        #region Edit data ilcp01
        private void EDIT_ILCP01(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP01)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];

                Int64 txtVendorCodes = 0;
                Int64 txtMakerCodes = 0;
                int campaingSubSidize = 0;
                //string campaignStatus = "N";
                //string waiveDuty = "N";
                int termRange = 0;

                if (ddlCampaignType.Text == "MAKER")
                {
                    campaingSubSidize = 1;
                }
                else if (ddlCampaignType.Text == "VENDOR")
                {
                    campaingSubSidize = 2;
                }
                else if (ddlCampaignType.Text == "ESB")
                {
                    campaingSubSidize = 3;
                }
                else if (ddlCampaignType.Text == "SHARESUB")
                {
                    campaingSubSidize = 4;
                }

                if (!String.IsNullOrEmpty(txtVendorCode.Text.ToString().Trim()))
                {
                    txtVendorCodes = Int64.Parse(txtVendorCode.Text.ToString().Trim());
                }
                else
                {
                    txtVendorCodes = 0;
                }

                if (!String.IsNullOrEmpty(txtMakerCode.Text.ToString().Trim()))
                {
                    txtMakerCodes = Int64.Parse(txtMakerCode.Text.ToString().Trim());
                }
                else
                {
                    txtMakerCodes = 0;
                }

                if (rbCreateCampaingTypeVendorCampaignSupport.Text == "N")
                {
                    termRange = 1;
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.Text == "Y")
                {
                    termRange = 2;
                }

                //SqlAll = @"UPDATE AS400DB01.ILOD0001.ILCP01 SET C01CMP = @C01CMP,
                //                                C01LTY = @C01LTY,
                //                                C01BRN = @C01BRN,
                //                                C01STY = @C01STY,
                //                                C01SBT = @C01SBT,
                //                                C01CTY = @C01CTY,
                //                                C01RNG = @C01RNG,
                //                                C01CNM = @C01CNM,
                //                                C01PTY = @C01PTY,
                //                                C01VDC = @C01VDC,
                //                                C01MKC = @C01MKC,
                //                                C01SDT = @C01SDT,
                //                                C01EDT = @C01EDT,
                //                                C01CAD = @C01CAD,
                //                                C01CLD = @C01CLD,
                //                                C01NXD = @C01NXD,
                //                                C01FIN = @C01FIN,
                //                                C01SRT = @C01SRT,
                //                                C01TRG = @C01TRG,
                //                                C01CST = @C01CST,
                //                                C01INV = @C01INV,
                //                                C01MKT = @C01MKT,
                //                                C01WDT = @C01WDT,
                //                                C01UDT = @C01UDT,
                //                                C01UTM = @C01UTM,
                //                                C01UUS = @C01UUS,
                //                                C01UPG = @C01UPG,
                //                                C01UWS = @C01UWS,
                //                                C01VTY = @C01VTY,
                //                                C01VCR = @C01VCR,
                //                                c01pmt = @c01pmt  WHERE C01CMP = " + campaignID;
                SqlAll = $@"UPDATE AS400DB01.ILOD0001.ILCP01 
                                        SET C01CMP = {campaignID},
                                            C01LTY = {"'" + loanType + "'"},
                                            C01BRN = {"'" + campaignStorage.GetCookiesStringByKey("branch") + "'"},
                                            C01STY = {campaingSubSidize},
                                            C01SBT = {"'" + rdoTypeSub.Text.ToString().Trim() + "'"},
                                            C01CTY = {"'" + rdoCreateCampaingTypeVendorCalculateType.Text.ToString().Trim() + "'"},
                                            C01RNG = {"'" + rbCreateCampaingTypeVendorCampaignSupport.Text.ToString().Trim() + "'"},
                                            C01CNM = {"'" + txtCampaignName.Text.Trim() + "'"},
                                            C01PTY = {"'" + txtProductDetail.Text.Trim() + "'"},
                                            C01VDC = {txtVendorCodes},
                                            C01MKC = {txtMakerCodes},
                                            C01SDT = {CHANGE_FORMAT_DATE(txtStartDate.Text.ToString().Trim())},
                                            C01EDT = {CHANGE_FORMAT_DATE(txtEndDate.Text.ToString().Trim())},
                                            C01CAD = {CHANGE_FORMAT_DATE(txtClosingApplicationDate.Text.ToString().Trim())},
                                            C01CLD = {CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim())},
                                            C01NXD = {txtXDue.Text.ToString().Trim()},
                                            C01FIN = {txtFInstall.Text.ToString().Trim()},
                                            C01SRT = {txtBaseRate.Text.ToString().Trim()},
                                            C01TRG = {termRange},
                                            C01INV = {CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim())},
                                            C01MKT = {"'" + txtMarketingCode.Text.Trim() + "'"},
                                            C01UDT = {_updateDate},
                                            C01UTM = {_updateTime},
                                            C01UUS = {"'" + m_userInfo.Username.ToString() + "'"},
                                            C01UPG = {"'ILE000001F'"},
                                            C01UWS = {"'" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"},
                                            C01VTY = {"'" + rdoVendorPayment.Text.ToString().Trim() + "'"},
                                            C01VCR = {int.Parse(txtVendorCreditDays.Text.ToString().Trim())},
                                            c01pmt = {"'" + txtSpecialPremium.Text.ToString().Trim() + "'"}  WHERE C01CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("C01CMP", campaignID);
                //cmd.Parameters.AddWithValue("C01LTY", loanType);
                //cmd.Parameters.AddWithValue("C01BRN", cookiesStorage.GetCookiesStringByKey("branch"));
                //cmd.Parameters.AddWithValue("C01STY", campaingSubSidize);
                //cmd.Parameters.AddWithValue("C01SBT", rdoTypeSub.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01CTY", rdoCreateCampaingTypeVendorCalculateType.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01RNG", rbCreateCampaingTypeVendorCampaignSupport.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01CNM", txtCampaignName.Text.Trim());
                //cmd.Parameters.AddWithValue("C01PTY", txtProductDetail.Text.Trim());
                //cmd.Parameters.AddWithValue("C01VDC", txtVendorCodes);
                //cmd.Parameters.AddWithValue("C01MKC", txtMakerCodes);
                //cmd.Parameters.AddWithValue("C01SDT", CHANGE_FORMAT_DATE(txtStartDate.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("C01EDT", CHANGE_FORMAT_DATE(txtEndDate.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("C01CAD", CHANGE_FORMAT_DATE(txtClosingApplicationDate.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("C01CLD", CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("C01NXD", txtXDue.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01FIN", txtFInstall.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01SRT", txtBaseRate.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01TRG", termRange);
                //cmd.Parameters.AddWithValue("C01INV", CHANGE_FORMAT_DATE(txtClosingLayBillDate.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("C01MKT", txtMarketingCode.Text.Trim());
                //cmd.Parameters.AddWithValue("C01UDT", _updateDate);
                //cmd.Parameters.AddWithValue("C01UTM", _updateTime);
                //cmd.Parameters.AddWithValue("C01UUS", m_userInfo.Username.ToString());
                //cmd.Parameters.AddWithValue("C01UPG", "ILE000001F");
                //cmd.Parameters.AddWithValue("C01UWS", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                //cmd.Parameters.AddWithValue("C01VTY", rdoVendorPayment.Text.ToString().Trim());
                //cmd.Parameters.AddWithValue("C01VCR", int.Parse(txtVendorCreditDays.Text.ToString().Trim()));
                //cmd.Parameters.AddWithValue("c01pmt", txtSpecialPremium.Text.ToString().Trim());

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resEditILCP01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resEditILCP01 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error edit : ILCP01 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP01 = false;
                    return;
                }
                else
                {
                    checkEditILCP01 = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : EDIT ILCP01!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP01 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp02
        private void EDIT_ILCP02(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP02, ref bool checkInsertILCP02)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP02 WHERE C02CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C02CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resEditILCP02 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resEditILCP02 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP02 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP02 = false;
                    return;
                }
                else
                {
                    checkEditILCP02 = true;
                }
                INSERT_ILCP02(dataCenter, ref checkInsertILCP02);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP02!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP02 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp04
        private void EDIT_ILCP04(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP04, ref bool checkInsertILCP04)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP04 WHERE C04CMP =" + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C04CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP04 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP04 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP04 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP04 = false;
                    return;
                }
                else
                {
                    checkEditILCP04 = true;
                }
                INSERT_ILCP04(dataCenter, ref checkInsertILCP04);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP04!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP04 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp05
        private void EDIT_ILCP05(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP05, ref bool checkInsertILCP05)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP05 WHERE C05CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C05CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP05 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP05 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP05 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP05 = false;
                    return;
                }
                else
                {
                    checkEditILCP05 = true;
                }
                INSERT_ILCP05(dataCenter, ref checkInsertILCP05);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP05!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP05 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp06
        private void EDIT_ILCP06(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP06, ref bool checkInsertILCP06)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP06 WHERE C06CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C06CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP06 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP06 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP06 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP06 = false;
                    return;
                }
                else
                {
                    checkEditILCP06 = true;
                }
                INSERT_ILCP06(dataCenter, ref checkInsertILCP06);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP06!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP06 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp07
        private void EDIT_ILCP07(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP07, ref bool checkInsertILCP07)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP07 WHERE C07CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C07CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP07 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP07 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP07 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP07 = false;
                    return;
                }
                else
                {
                    checkEditILCP07 = true;
                }
                INSERT_ILCP07(dataCenter, ref checkInsertILCP07);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP07!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP07 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp08
        private void EDIT_ILCP08(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP08, ref bool checkInsertILCP08)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP08 WHERE C08CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C08CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP08 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP08 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP08 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP08 = false;
                    return;
                }
                else
                {
                    checkEditILCP08 = true;
                }
                INSERT_ILCP08(dataCenter, ref checkInsertILCP08);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP08!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP08 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp09
        private void EDIT_ILCP09(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP09, ref bool checkInsertILCP09)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP09 WHERE C09CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C09CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP09 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP09 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP09 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP09 = false;
                    return;
                }
                else
                {
                    checkEditILCP09 = true;
                }
                INSERT_ILCP09(dataCenter, ref checkInsertILCP09);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP09!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP09 = false;
                return;
            }
        }
        #endregion
        #region Edit data ilcp10
        private void EDIT_ILCP10()
        {

        }
        #endregion
        #region Edit data ilcp11
        private void EDIT_ILCP11()
        {

        }
        #endregion
        #region Edit data ilcp99
        private void EDIT_ILCP99(EB_Service.DAL.DataCenter dataCenter, ref bool checkEditILCP99, ref bool checkInsertILCP99)
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            try
            {
                string copyCampaignString = Globals.copyCampaign;
                //string[] CampaignString;
                //CampaignString = copyCampaignString.ToString().Split('-');

                string campaignID = copyCampaignString.Replace("-", ""); //CampaignString[0] + "" + CampaignString[1] + "" + CampaignString[2] + "" + CampaignString[3];
                SqlAll = @"DELETE AS400DB01.ILOD0001.ILCP99 WHERE C99CMP = " + campaignID;

                //cmd.Parameters.Clear();
                //cmd.Parameters.Add("C99CMP", campaignID);

                cmd.CommandText = SqlAll;

                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resILCP09 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resILCP09 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Error Edit : ILCP99 \n Please check command !";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    checkEditILCP99 = false;
                    return;
                }
                else
                {
                    checkEditILCP99 = true;
                }
                INSERT_ILCP99(dataCenter, ref checkInsertILCP99);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP99!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                checkEditILCP99 = false;
                return;
            }
        }
        #endregion

        #endregion

        #region UPDATE Campaign
        protected void btnUpdateClick(object sender, EventArgs e)
        {
            rdoUpdateStatus.ClearSelection();
            popupConfirmUpdateCampaign.ShowOnPageLoad = true;
        }

        protected void btnConfirmUpdateOK_Click(object sender, EventArgs e)
        {
            string flagupdate = rdoUpdateStatus.SelectedValue;
            string campaignCode = txtCampaignCode.Text.Replace("-", "");

            if (!String.IsNullOrEmpty(flagupdate) && !String.IsNullOrEmpty(campaignCode))
            {
                //string sql = "UPDATE AS400DB01.ILOD0001.ILCP01 SET C01CST = '" + flagupdate + "' WHERE C01CMP = " + txtCampaignCode.Text.Replace("-", "");           

                try
                {
                    m_userInfo = userInfoService.GetUserInfo();

                    ilObj.UserInfomation = m_userInfo;
                    dataCenter = new DataCenter(m_userInfo);
                    //old campaign status
                    string sql = @"SELECT C01CST FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01CMP = " + campaignCode;
                    DataSet ds = new DataSet();
                    ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

                    string oldsts = "";
                    if (ds?.Tables[0] != null)
                    {
                        oldsts = ds?.Tables[0]?.Rows[0]["C01CST"].ToString();
                    }

                    string jobname = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

                    //ILCP10
                    sql = $@"INSERT INTO AS400DB01.ILOD0001.ILCP10 (C10LDT,C10LTM,C10LUS,C10LWS,C10CMP,C10STS,C10UDT,C10UTM,C10UUS,C10UPG,C10UWS) 
                              VALUES (
                                        {_updateDate},
                                        {_updateTime},
                                        {"'" + m_userInfo.Username.ToString() + "'"},
                                        {"'" + jobname + "'"},
                                        {campaignCode},
                                        {"'" + oldsts + "'"},
                                        {_updateDate},
                                        {_updateTime},
                                        {"'" + m_userInfo.Username.ToString() + "'"},
                                        {"'ILE000004F'"},
                                        {"'" + jobname + "'"})";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@C10LDT", _updateDate);
                    //cmd.Parameters.Add("@C10LTM", _updateTime);
                    //cmd.Parameters.Add("@C10LUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C10LWS", jobname);
                    //cmd.Parameters.Add("@C10CMP", campaignCode);
                    //cmd.Parameters.Add("@C10STS", oldsts);
                    //cmd.Parameters.Add("@C10UDT", _updateDate);
                    //cmd.Parameters.Add("@C10UTM", _updateTime);
                    //cmd.Parameters.Add("@C10UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C10UPG", "ILE000004F");
                    //cmd.Parameters.Add("@C10UWS", jobname);

                    cmd.CommandText = sql;

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resILCP10 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resILCP10 == -1)
                    {
                        lblMsg.Text = "Error insert into : resILCP10 \n Please check command !";
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        return;
                    }
                    dataCenter.CommitMssql();
                    dataCenter.CloseConnectSQL();
                    //ILCP01
                    sql = $@"UPDATE AS400DB01.ILOD0001.ILCP01 SET C01CST = {"'" + flagupdate + "'"},
                                          C01UDT = {_updateDate},
                                          C01UTM = {_updateTime},
                                          C01UUS = {"'" + m_userInfo.Username.ToString() + "'"},
                                          C01UPG = {"'ILE000004F'"},
                                          C01UWS = {"'" + jobname + "'"}
                        WHERE C01CMP = {campaignCode}";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@C01CST", flagupdate);
                    //cmd.Parameters.Add("@C01UDT", _updateDate);
                    //cmd.Parameters.Add("@C01UTM", _updateTime);
                    //cmd.Parameters.Add("@C01UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C01UPG", "ILE000004F");
                    //cmd.Parameters.Add("@C01UWS", jobname);
                    //cmd.Parameters.Add("@C01CMP", campaignCode);

                    cmd.CommandText = sql;

                    int resILCP01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resILCP01 == -1 || resILCP10 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        if (resILCP10 == -1 && resILCP01 != -1)
                        {
                            lblMsg.Text = "Error insert into : resILCP10 \n Please check command !";
                        }
                        else if (resILCP10 != -1 && resILCP01 == -1)
                        {
                            lblMsg.Text = "Error update : resILCP01 \n Please check command !";
                        }
                        else if (resILCP10 == -1 && resILCP01 == -1)
                        {
                            lblMsg.Text = "Error update : resILCP01 \n Error insert into : resILCP10 \n Please check command !";
                        }
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                    }
                    //else if(resILCP01 != -1 && resILCP10 != -1)
                    //{
                    //    dataCenter.CommitMssql();
                    //    dataCenter.CloseConnectSQL();
                    //}
                    //else
                    //{
                    //    dataCenter.RollbackMssql();
                    //    dataCenter.CloseConnectSQL();
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException != null ? ex.Message + " - " + ex.InnerException.ToString() : ex.Message);
                    lblMsg.Text = "Error catch : Check exception : ILCP01! \n Error catch : Check exception : ILCP10!";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                }

                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();

                lblMsgCmp.Text = campaignCode;
                lblMsgSuccess.Text = "Update campaign completed";
                PopupMsgSuccess.HeaderText = "Success";
                PopupMsgSuccess.ShowOnPageLoad = true;
            }
        }
        #endregion

        #region DELETE Campaign
        protected void btnDeleteClick(object sender, EventArgs e)
        {
            popupConfirmDeleteCampaign.ShowOnPageLoad = true;
        }

        protected void btnConfirmDeleteOK_Click(object sender, EventArgs e)
        {
            //string sql = "UPDATE AS400DB01.ILOD0001.ILCP01 SET C01CST = 'X' WHERE C01CMP = " + txtCampaignCode.Text.Replace("-", "");
            string campaignCode = txtCampaignCode.Text.Replace("-", "");

            if (!String.IsNullOrEmpty(campaignCode))
            {
                try
                {
                    m_userInfo = userInfoService.GetUserInfo();

                    ilObj.UserInfomation = m_userInfo;
                    dataCenter = new DataCenter(m_userInfo);
                    //old campaign status
                    string sql = @"SELECT C01CST FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE CAST(C01CMP as nvarchar) = '" + campaignCode + "'";
                    DataSet ds = new DataSet();
                    ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

                    string oldsts = "";
                    if (ds?.Tables[0] != null)
                    {
                        oldsts = ds?.Tables[0]?.Rows[0]["C01CST"].ToString();
                    }

                    string jobname = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

                    //ILCP10
                    sql = $@"INSERT INTO AS400DB01.ILOD0001.ILCP10 (C10LDT,C10LTM,C10LUS,C10LWS,C10CMP,C10STS,C10UDT,C10UTM,C10UUS,C10UPG,C10UWS) 
                                VALUES (
                                        {_updateDate},
                                        {_updateTime},
                                        {"'" + m_userInfo.Username.ToString() + "'"},
                                        {"'" + jobname + "'"},
                                        {campaignCode},
                                        {"'" + oldsts + "'"},
                                        {_updateDate},
                                        {_updateTime},
                                        {"'" + m_userInfo.Username.ToString() + "'"},
                                        {"'ILE000004F'"},
                                        {"'" + jobname + "'"})";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@C10LDT", _updateDate);
                    //cmd.Parameters.Add("@C10LTM", _updateTime);
                    //cmd.Parameters.Add("@C10LUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C10LWS", jobname);
                    //cmd.Parameters.Add("@C10CMP", campaignCode);
                    //cmd.Parameters.Add("@C10STS", oldsts);
                    //cmd.Parameters.Add("@C10UDT", _updateDate);
                    //cmd.Parameters.Add("@C10UTM", _updateTime);
                    //cmd.Parameters.Add("@C10UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C10UPG", "ILE000004F");
                    //cmd.Parameters.Add("@C10UWS", jobname);

                    cmd.CommandText = sql;

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resILCP10 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                    //ILCP01
                    sql = $@"UPDATE AS400DB01.ILOD0001.ILCP01 
                                      SET C01CST = {"'X'"},
                                          C01UDT = {_updateDate},
                                          C01UTM = {_updateTime},
                                          C01UUS = {"'" + m_userInfo.Username.ToString() + "'"},
                                          C01UPG = {"'ILE000004F'"},
                                          C01UWS = {"'" + jobname + "'"}
                                    WHERE C01CMP = {campaignCode}";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@C01CST", "X");
                    //cmd.Parameters.Add("@C01UDT", _updateDate);
                    //cmd.Parameters.Add("@C01UTM", _updateTime);
                    //cmd.Parameters.Add("@C01UUS", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("@C01UPG", "ILE000004F");
                    //cmd.Parameters.Add("@C01UWS", jobname);
                    //cmd.Parameters.Add("@C01CMP", campaignCode);

                    cmd.CommandText = sql;
                    bool transaction2 = dataCenter.Sqltr == null ? true : false;
                    int resILCP01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction2).Result.afrows;

                    if (resILCP01 == -1 || resILCP10 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        if (resILCP10 == -1 && resILCP01 != -1)
                        {
                            lblMsg.Text = "Error insert into : resILCP10 \n Please check command !";
                        }
                        else if (resILCP10 != -1 && resILCP01 == -1)
                        {
                            lblMsg.Text = "Error update : resILCP01 \n Please check command !";
                        }
                        else if (resILCP10 == -1 && resILCP01 == -1)
                        {
                            lblMsg.Text = "Error update : resILCP01 \n Error insert into : resILCP10 \n Please check command !";
                        }
                        PopupMsgError.HeaderText = "Error";
                        PopupMsgError.ShowOnPageLoad = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException != null ? ex.Message + " - " + ex.InnerException.ToString() : ex.Message);
                    lblMsg.Text = "Error catch : Check exception : ILCP01! \n Error catch : Check exception : ILCP10!";
                    PopupMsgError.HeaderText = "Error";
                    PopupMsgError.ShowOnPageLoad = true;
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                }

                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();

                lblMsgCmp.Text = campaignCode;
                lblMsgSuccess.Text = "Delete campaign completed";
                PopupMsgSuccess.HeaderText = "Success";
                PopupMsgSuccess.ShowOnPageLoad = true;
            }
        }
        #endregion

        #region function show rate (position header for page)
        protected void SHOW_RATE_HEADER()
        {
            try
            {
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                SqlAll = "SELECT MX1APP, MX1INT, MX1CRU, MX1PNT, MX1INF, MX1INA, MX1MAX, MX1STD, MX1END, MX1UPD, MX1UPT, MX1UPU, MX1STS FROM [AS400DB01].[GNOD0000].GNMX01 WITH (NOLOCK) WHERE MX1APP = 'IL' ORDER BY MX1STD";
                cmd.CommandText = SqlAll;
                DataSet DS = new DataSet();
                DS = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
                DataTable dt_Rate = DS?.Tables[0];
                //if(CHANGE_FORMAT_DATE(txtStartDate.Text.ToString().Trim())>)
                //DataRow = dt_Rate 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.Message + " - " + ex.InnerException.ToString() : ex.Message);
                lblMsg.Text = "Error catch : Check exception : ILCP01! \n Error catch : Check exception : ILCP10!";
                PopupMsgError.HeaderText = "Error";
                PopupMsgError.ShowOnPageLoad = true;
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
            }
        }
        #endregion
        protected void txtSubSeq_TextChanged(object sender, EventArgs e)
        {

        }

        protected void SET_CLEAR_ADD(object sender, EventArgs e)
        {

        }

        protected void tabDetail_ActiveTabChanged(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
        {
            if (e.Tab.Name == "BranchNote")
            {
                if (btnStatus.Text == "EDIT")
                {
                    SetNoteReadOnly(true);
                }
                LOAD_DATA_ILCP11();

            }
            else if (e.Tab.Name == "ShareSub" && HdnIsEdit1.Value == "VIEW_CAMPAIGN")
            {
                ContentControl1.Enabled = false;
                ContentControl2.Enabled = false;
                ContentControl3.Enabled = false;
                ContentControl4.Enabled = false;

            }
            else if (e.Tab.Name == "ShareSub" && HdnIsEdit1.Value == "UPDATE_CAMPAIGN")
            {

                showStatusCampaignDefult();
                ModeManagement();
                SetLoadCampaign();
                imgNewCampaignOn.Visible = false;
                SetDefault();

                TabContentControlSetVisible(false);

                SHOW_RATE_HEADER();

                PopupAlertCenter();

                LoadDBInitialConfigValue();
                LoadIntialConfigValuetoControl();

                BindDataFromMode();
            }

            //ContentControl1.Enabled = true;
            //ContentControl2.Enabled = true;
            //ContentControl3.Enabled = true;
            //ContentControl4.Enabled = true; 
            //CampaignTypeVendorBindDataForCreate();
        }

        protected void ClearInputDataDetail()
        {
            rdoCreateCampaingTypeVendorCalculateType.ClearSelection();
            rbCreateCampaingTypeVendorCampaignSupport.ClearSelection();
            txtCreateCampaingTypeVendorBaseRate.Text = null;

            rSubSeq.Visible = false;
            pRangeY.Visible = false;
            pAddMonth.Visible = false;
            pAddRate.Visible = false;
            gvMultiRate.Visible = false;
            campaignStorage.ClearCookies("ds_multi_rate");
            btnClearTermRangeClick(null, null);
        }

        protected void tbRATES_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowIndex = gvr.RowIndex;

            BindGridviewTermProducttoSession(rowIndex);
        }

        protected void tbC02IFR_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowIndex = gvr.RowIndex;

            BindGridviewTermProducttoSession(rowIndex);
        }

        protected void tbC02CRR_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowIndex = gvr.RowIndex;

            BindGridviewTermProducttoSession(rowIndex);
        }

        protected void tbC02INR_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowIndex = gvr.RowIndex;

            BindGridviewTermProducttoSession(rowIndex);
        }

        protected void BindGridviewTermProducttoSession(int rowIndex)
        {
            DataTable dt = dtTermOfCampaign();

            StringBuilder sb = new StringBuilder();
            foreach (GridViewRow gvRow in gvViewTermOfCampaign.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                dr["C02TTM"] = gvRow.Cells[1].Text;
                dr["C02CSQ"] = gvRow.Cells[2].Text;
                dr["C02RSQ"] = gvRow.Cells[3].Text;
                dr["C02TTR"] = gvRow.Cells[4].Text;
                dr["C02FMT"] = gvRow.Cells[5].Text;
                dr["C02TOT"] = gvRow.Cells[6].Text;

                if (hdTermOfCampaignRowIndex.Value == (gvRow.RowIndex + 1).ToString())
                {
                    switch (hdTermOfCampaignRowName.Value.ToString())
                    {
                        case "tbC02INR":
                            if (string.IsNullOrEmpty(((TextBox)gvRow.Cells[7].FindControl("tbC02INR")).Text))
                            {
                                ((TextBox)gvRow.Cells[7].FindControl("tbC02INR")).Text = hdTermOfCampaignRowValue.Value;
                            }
                            break;
                        case "tbC02CRR":
                            if (string.IsNullOrEmpty(((TextBox)gvRow.Cells[8].FindControl("tbC02CRR")).Text))
                            {
                                ((TextBox)gvRow.Cells[8].FindControl("tbC02CRR")).Text = hdTermOfCampaignRowValue.Value;
                            }
                            break;
                        case "tbC02IFR":
                            if (string.IsNullOrEmpty(((TextBox)gvRow.Cells[9].FindControl("tbC02IFR")).Text))
                            {
                                ((TextBox)gvRow.Cells[9].FindControl("tbC02IFR")).Text = hdTermOfCampaignRowValue.Value;
                            }
                            break;
                        case "tbRATES":
                            if (string.IsNullOrEmpty(((TextBox)gvRow.Cells[10].FindControl("tbRATES")).Text))
                            {
                                ((TextBox)gvRow.Cells[10].FindControl("tbRATES")).Text = hdTermOfCampaignRowValue.Value;
                            }
                            break;
                    }
                }

                dr["C02INR"] = ((TextBox)gvRow.Cells[7].FindControl("tbC02INR")).Text;
                dr["C02CRR"] = ((TextBox)gvRow.Cells[8].FindControl("tbC02CRR")).Text;
                dr["C02IFR"] = ((TextBox)gvRow.Cells[9].FindControl("tbC02IFR")).Text;
                dr["RATES"] = ((TextBox)gvRow.Cells[10].FindControl("tbRATES")).Text;
                dr["C02AIR"] = ((TextBox)gvRow.Cells[7].FindControl("tbC02INR")).Text;
                dr["C02ACR"] = ((TextBox)gvRow.Cells[8].FindControl("tbC02CRR")).Text;


                dt.Rows.Add(dr);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_term_of_campaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
            if (campaignStorage.check_dataset(ds))
            {
                gvViewTermOfCampaign.DataSource = ds;
                gvViewTermOfCampaign.DataBind();
            }
            gvViewTermOfCampaign.Rows[rowIndex].Focus();

            if (hdTermOfCampaignRowIndex.Value == (rowIndex + 1).ToString())
            {
                switch (hdTermOfCampaignRowName.Value.ToString())
                {
                    case "tbC02INR":
                        ((TextBox)gvViewTermOfCampaign.Rows[rowIndex].Cells[7].FindControl("tbC02INR")).Focus();
                        break;
                    case "tbC02CRR":
                        ((TextBox)gvViewTermOfCampaign.Rows[rowIndex].Cells[8].FindControl("tbC02CRR")).Focus();
                        break;
                    case "tbC02IFR":
                        ((TextBox)gvViewTermOfCampaign.Rows[rowIndex].Cells[9].FindControl("tbC02IFR")).Focus();
                        break;
                    case "tbRATES":
                        ((TextBox)gvViewTermOfCampaign.Rows[rowIndex].Cells[10].FindControl("tbRATES")).Focus();
                        break;
                }
            }

            hdTermOfCampaignRowValue.Value = "";
            hdTermOfCampaignRowIndex.Value = "";
            hdTermOfCampaignRowName.Value = "";
        }
        #region show status campaign tab header
        private void showStatusCampaignDefult()
        {
            imgNewCampaignOn.Visible = false;
            imgNewCampaignOff.Visible = true;
            imgActiveCampaignOn.Visible = false;
            imgActiveCampaignOff.Visible = true;
            imgEndCampaignOn.Visible = false;
            imgEndCampaignOff.Visible = true;

        }
        private void SHOW_STATUS_CAMPAIGN()
        {
            ulong endDateCheckStatus = ulong.Parse(CHANGE_FORMAT_DATE(txtEndDate.Text));
            if (Globals.showStatusHeader == "N" && Globals.showStatusHeader != "E" && endDateCheckStatus > _updateDate)
            {
                imgNewCampaignOn.Visible = true;
                imgNewCampaignOff.Visible = false;
            }
            else if (Globals.showStatusHeader == "A" && Globals.showStatusHeader != "E")
            {
                imgActiveCampaignOn.Visible = true;
                imgActiveCampaignOff.Visible = false;
            }
            else if (endDateCheckStatus < _updateDate || Globals.showStatusHeader == "E")
            {
                imgEndCampaignOn.Visible = true;
                imgEndCampaignOff.Visible = false;
            }
            else
            {
                showStatusCampaignDefult();
            }

        }
        #endregion


        #region Pop-up ShareSub
        protected void rbOptionalAddMultiTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbOptionalAddMultiTerm.SelectedValue == "S")
            {
                rSubSeq.Visible = true;
                pAddMultiTerm.Visible = false;
                pAddRate.Visible = false;
                gvMultiRate.Columns[0].Visible = false;
            }
            else if (rbOptionalAddMultiTerm.SelectedValue == "M")
            {
                rSubSeq.Visible = false;
                pAddMultiTerm.Visible = true;
                pAddRate.Visible = true;
                //MultiT Term
                txtRateforRange.Enabled = true;
                txtSubRateMaker.Enabled = true;
                txtSubRateVendor.Enabled = true;
                txtSubRateESB.Enabled = true;
                btnInsertTerm.Enabled = true;
                gvMultiRate.Columns[0].Visible = true;
            }
        }

        protected void gvMultiRate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (rbOptionalAddMultiTerm.SelectedValue == "M")
            {
                ClearPopupShareSub();
                InitPopupShareSub();

                string TotalTerm = gvMultiRate.Rows[e.NewSelectedIndex].Cells[2].Text;
                string SubSeq = gvMultiRate.Rows[e.NewSelectedIndex].Cells[3].Text;
                txtPopupShareSubSubSeq.Text = SubSeq;
                txtPopupShareSubTotalTerm.Text = TotalTerm;

                PopupShareSub.ShowOnPageLoad = true;
            }
        }

        protected void txtPopupShareSubCustRate_TextChanged(object sender, EventArgs e)
        {
            txtPopupShareSubCustRate.Text = ValidateShareSubRateWithBaseRate("CUSTOMER");
        }

        protected void txtPopupShareSubVendorRate_TextChanged(object sender, EventArgs e)
        {
            txtPopupShareSubVendorRate.Text = ValidateShareSubRateWithBaseRate("VENDOR");
        }

        protected void txtPopupShareSubMakerRate_TextChanged(object sender, EventArgs e)
        {
            txtPopupShareSubMakerRate.Text = ValidateShareSubRateWithBaseRate("MAKER");
        }

        protected void txtPopupShareSubESBRate_TextChanged(object sender, EventArgs e)
        {
            txtPopupShareSubESBRate.Text = ValidateShareSubRateWithBaseRate("ESB");
        }

        protected void txtPopupShareSubTermRange_TextChanged(object sender, EventArgs e)
        {
            decimal value = Convert.ToInt32(ConvertToInteger(txtPopupShareSubTermRange.Text));
            if (value > _MaxRangeOfInterest)
            {
                value = _MaxRangeOfInterest;
            }
            txtPopupShareSubTermRange.Text = value.ToString();
        }

        protected void txtPopupShareSubTermofRange_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPopupShareSubTermofRange.Text))
            {
                int inputTerm = Convert.ToInt32(txtPopupShareSubTermofRange.Text);
                int maxTerm = Convert.ToInt32(txtPopupShareSubTotalTerm.Text);
                int MinTerm = 1;

                string resultTerm = MinTerm.ToString();
                int inputValue = Convert.ToInt32(ConvertToInteger(txtPopupShareSubTermofRange.Text));
                decimal totalTerm = MinTerm;

                //Total Term
                if (String.IsNullOrEmpty(txtPopupShareSubTotalTerm.Text))
                {
                    if (inputValue <= _MaxTotalTermOfContract && inputValue >= MinTerm)
                    {
                        totalTerm = inputValue;
                    }
                    else if (inputValue > _MaxTotalTermOfContract && inputValue >= MinTerm)
                    {
                        totalTerm = _MaxTotalTermOfContract;
                    }
                    else if (inputValue < MinTerm)
                    {
                        totalTerm = MinTerm;
                    }

                    txt_addTerm.Text = totalTerm.ToString();
                }
                else
                {
                    totalTerm = Convert.ToInt32(txtPopupShareSubTotalTerm.Text);
                }

                //Check totalterm max.
                if (totalTerm > _MaxTotalTermOfContract)
                {
                    totalTerm = _MaxTotalTermOfContract;
                }

                //Result
                if (inputValue > totalTerm && inputValue >= MinTerm)
                {
                    resultTerm = totalTerm.ToString();
                }
                else if (inputValue < MinTerm)
                {
                    resultTerm = MinTerm.ToString();
                }
                else
                {
                    resultTerm = inputValue.ToString();
                }

                txtPopupShareSubTermofRange.Text = resultTerm;

                pPopupShareSubRate.Visible = true;
                pPopupShareSubInputRate.Visible = true;
                pPopupShareSubStartEndTerm.Visible = true;
                txtPopupShareSubTermofRange.Enabled = false;
                ValidatePopupTermMultiRate(Convert.ToInt32(txtPopupShareSubTermRange.Text), Convert.ToInt32(txtPopupShareSubTermofRange.Text));
            }
            else
            {
                return;
            }
        }

        protected void btnPopupShareSubInsert_Click(object sender, EventArgs e)
        {
            BindPopupShareSubtogvPopupShareSub();

            int tSeq = Convert.ToInt32(txtPopupShareSubTermRange.Text);

            //if (tSeq <= _MaxRangeOfInterest)
            //{
            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            {
                tSeq = (from dsMultiRate in ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).AsEnumerable()
                        where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == 1
                        orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                        select int.Parse(dsMultiRate.Field<string>("C05RSQ"))).FirstOrDefault();
                tSeq++;
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
            {
                tSeq = (from dsMultiRate in ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).AsEnumerable()
                        where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == Convert.ToInt32(txtPopupShareSubSubSeq.Text)
                        orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                        select int.Parse(dsMultiRate.Field<string>("C05RSQ"))).FirstOrDefault();
                tSeq++;
            }

            txtPopupShareSubTermRange.Text = tSeq.ToString();

            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y" && txtPopupShareSubTotalTerm.Text == txtPopupShareSubEndTerm.Text)
            {
                pPopupShareSubInputRate.Visible = false;
                pPopupShareSubSubmit.Visible = true;
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y" && txtPopupShareSubTotalTerm.Text != txtPopupShareSubEndTerm.Text)
            {
                pPopupShareSubSubmit.Visible = false;
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N" && txtPopupShareSubTotalTerm.Text == txtPopupShareSubEndTerm.Text)
            {
                var subseq = (from dsMultiRate in ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).AsEnumerable()
                              orderby int.Parse(dsMultiRate.Field<string>("C05CSQ")) descending
                              select int.Parse(dsMultiRate.Field<string>("C05CSQ"))).Distinct().FirstOrDefault();

                txtPopupShareSubCustRate.ReadOnly = false;
                pPopupShareSubSubmit.Visible = true;
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N" && txtPopupShareSubTotalTerm.Text != txtPopupShareSubEndTerm.Text)
            {
                pPopupShareSubSubmit.Visible = false;
                pPopupShareSubStartEndTerm.Visible = false;
                pPopupShareSubRate.Visible = false;
                pPopupShareSubInputRate.Visible = false;
                txtPopupShareSubTermofRange.Enabled = true;
                ClearPopupShareSubRate();
                txtPopupShareSubTermofRange.Text = null;
            }
            //}
        }

        protected void gvPopupShareSub_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnPopupShareSubOK_Click(object sender, EventArgs e)
        {
            int subseq = Convert.ToInt32(txtPopupShareSubSubSeq.Text);

            DataTable dtShareSub = ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).Copy();
            //DataTable dtMulti = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_multi_rate.Value)?.Tables[0]?.Copy();
            DataTable dtMulti = campaignStorage.GetCookiesDataSetByKey("ds_multi_rate")?.Tables[0]?.Copy();

            //Delete current subseq
            DataTable dtResult = dtMulti.AsEnumerable()
                                 .Where(r => int.Parse(r.Field<string>("C05CSQ")) != subseq)
                                 .CopyToDataTable();

            //Merge Sharesub to main
            dtResult.Merge(dtShareSub);

            //Sort by Subseq & Seq
            dtResult = dtResult.AsEnumerable()
                       .OrderBy(r => int.Parse(r.Field<string>("C05CSQ")))
                       .ThenBy(r => int.Parse(r.Field<string>("C05RSQ")))
                       .CopyToDataTable();

            //Bind data
            DataSet ds = new DataSet();
            ds.Tables.Add(dtResult);

            campaignStorage.SetCookiesDataSetByName("ds_multi_rate", ds);
            if (campaignStorage.check_dataset(ds))
            {
                gvMultiRate.DataSource = ds;
                gvMultiRate.DataBind();
            }
        }

        protected void InitPopupShareSub()
        {
            pPopupShareSubRate.Visible = false;
            pPopupShareSubInputRate.Visible = false;
            pPopupShareSubGrid.Visible = false;
            pPopupShareSubSubmit.Visible = false;
            pPopupShareSubStartEndTerm.Visible = false;

            SetPopupShareSubRateVisible();
            txtPopupShareSubTermRange.Text = "1";
            txtPopupShareSubTermofRange.Enabled = true;
        }

        protected void SetPopupShareSubRateVisible()
        {
            //Maker
            if (ckbCreateCampaignTypeVendorShareSub.Items[0].Selected)
            {
                rShareSubMaker.Visible = true;
            }
            else
            {
                rShareSubMaker.Visible = false;
            }

            //Vendor
            if (ckbCreateCampaignTypeVendorShareSub.Items[1].Selected)
            {
                rShareSubVendor.Visible = true;
            }
            else
            {
                rShareSubVendor.Visible = false;
            }

            //ESB
            if (ckbCreateCampaignTypeVendorShareSub.Items[2].Selected)
            {
                rShareSubESB.Visible = true;
            }
            else
            {
                rShareSubESB.Visible = false;
            }
        }

        protected void ClearPopupShareSub()
        {
            ClearPopupShareSubRate();
            txtPopupShareSubTermofRange.Text = null;
            txtPopupShareSubTermRange.Text = null;
            txtPopupShareSubTotalTerm.Text = null;
            txtPopupShareSubSubSeq.Text = null;
            txtPopupShareSubStartTerm.Text = null;
            txtPopupShareSubEndTerm.Text = null;

            campaignStorage.ClearCookies("dt_popup_sharesub");
        }

        protected void ClearPopupShareSubRate()
        {
            txtPopupShareSubESBRate.Text = null;
            txtPopupShareSubMakerRate.Text = null;
            txtPopupShareSubVendorRate.Text = null;
            txtPopupShareSubCustRate.Text = null;
        }

        protected string ValidateShareSubRateWithBaseRate(string rateType)
        {
            ///<summary> Validation Input Rate of Customer, Vendor and ESB(EasyBuy) sum rate 3 type not over Base Rate. And return string value in decimal format.
            ///<para>
            ///- rateType have parameters in ("CUSTOMER", "VENDOR", "ESB").
            ///</para>
            ///</summary>
            string resultRte = "0.00";
            decimal BaseRte = Convert.ToDecimal(txtCreateCampaingTypeVendorBaseRate.Text);
            decimal custValue = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubCustRate.Text.Trim()));
            string CampaignType = ddlCampaignType.SelectedValue; //MAKER VENDOR ESB SHARESUB

            bool negative = false;
            decimal BaseValidate = BaseRte;

            //Check null value.
            bool custChkVal = false;
            bool vendChkVal = false;
            bool esbChkVal = false;
            bool makeChkVal = false;

            //Get value from textbox.
            decimal CustRte;
            decimal VendorRte;
            decimal ESBRte;
            decimal MakerRte;

            #region Customer Rate
            //Customer
            if (String.IsNullOrEmpty(txtPopupShareSubCustRate.Text))
            {
                custChkVal = false;
                CustRte = _MinInterestRate;
            }
            else
            {
                custChkVal = true;
                //decimal custValue = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubCustRate.Text.Trim()));

                if (custValue > _MaxInterestRate)
                {
                    if (custValue <= BaseRte && custValue >= _MinInterestRate)
                    {
                        CustRte = custValue;
                    }
                    else
                    {
                        CustRte = BaseRte;
                    }
                }
                else
                {
                    if (custValue <= BaseRte && custValue >= _MinInterestRate)
                    {
                        CustRte = custValue;
                    }
                    else if (custValue <= BaseRte && custValue <= _MinInterestRate)
                    {
                        CustRte = _MinInterestRate;
                    }
                    else
                    {
                        CustRte = BaseRte;
                    }
                }
                //CustRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubCustRate.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubCustRate.Text.Trim()));
            }

            if (CustRte < 0)
            {
                BaseValidate = BaseValidate + (CustRte * -1);
                negative = true;
            }
            #endregion

            #region Check and Get Rate
            //Vendor
            if (String.IsNullOrEmpty(txtPopupShareSubVendorRate.Text))
            {
                vendChkVal = false;
                VendorRte = 0;
            }
            else
            {
                vendChkVal = true;
                if (negative)
                {
                    VendorRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubVendorRate.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubVendorRate.Text.Trim()));
                }
                else
                {
                    VendorRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubVendorRate.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubVendorRate.Text.Trim()));
                }

                //vendChkVal = VendorRte == 0 ? false : true;
            }

            //ESB
            if (String.IsNullOrEmpty(txtPopupShareSubESBRate.Text))
            {
                esbChkVal = false;
                ESBRte = 0;
            }
            else
            {
                esbChkVal = true;
                if (negative)
                {
                    ESBRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubESBRate.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubESBRate.Text.Trim()));
                }
                else
                {
                    ESBRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubESBRate.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubESBRate.Text.Trim()));
                }

                //esbChkVal = ESBRte == 0 ? false : true;
            }

            //Maker
            if (String.IsNullOrEmpty(txtPopupShareSubMakerRate.Text))
            {
                makeChkVal = false;
                MakerRte = 0;
            }
            else
            {
                makeChkVal = true;
                if (negative)
                {
                    MakerRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubMakerRate.Text.Trim())) > BaseValidate ? BaseValidate : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubMakerRate.Text.Trim()));
                }
                else
                {
                    MakerRte = Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubMakerRate.Text.Trim())) > BaseRte ? BaseRte : Convert.ToDecimal(ConvertTo2Decimal(txtPopupShareSubMakerRate.Text.Trim()));
                }

                //makeChkVal = MakerRte == 0 ? false : true;
            }
            #endregion

            //Validate from rateType.
            decimal sumRate = CustRte + VendorRte + ESBRte + MakerRte;

            if (CampaignType.ToUpper() == "VENDOR")
            {
                #region Type VENDOR
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                txtPopupShareSubESBRate.Text = "0.00";
                                txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                txtPopupShareSubESBRate.Text = "0.00";
                                txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "VENDOR")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = VendorRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseRte - (CustRte + VendorRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = VendorRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "MAKER")
            {
                #region Type MAKER
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseValidate - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                txtPopupShareSubESBRate.Text = "0.00";
                                txtPopupShareSubMakerRate.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (MakerRte + ESBRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                txtPopupShareSubESBRate.Text = "0.00";
                                txtPopupShareSubMakerRate.Text = (BaseRte - CustRte).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = CustRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "MAKER")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + MakerRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = MakerRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            if ((BaseRte - (CustRte + MakerRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = MakerRte.ToString("N2");
                        }
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                    else
                    {
                        if (custChkVal && makeChkVal && !esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                        }
                        else if (!custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                        }
                        else if (custChkVal && makeChkVal && esbChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if ((BaseRte - (CustRte + ESBRte)) < 0)
                            {
                                //txtPopupShareSubESBRate.Text = "0.00";
                                //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                                txtPopupShareSubCustRate.Text = (BaseRte - (MakerRte + ESBRte)).ToString("N2");
                            }
                            else
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            }
                        }
                        else
                        {
                            resultRte = ESBRte.ToString("N2");
                        }
                    }
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "ESB")
            {
                #region Type ESB
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    //if (negative)
                    //{
                    //    if (custChkVal && !esbChkVal)
                    //    {
                    //        //CustRte = BaseRte - (VendorRte + ESBRte);
                    //        CustRte = sumRate > BaseRte ? (BaseValidate - ESBRte) : CustRte;
                    //        resultRte = CustRte.ToString("N2");

                    //        txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                    //    }
                    //    else if (!custChkVal && esbChkVal)
                    //    {
                    //        CustRte = sumRate > BaseRte ? (BaseValidate - ESBRte) : CustRte;
                    //        resultRte = CustRte.ToString("N2");

                    //        txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                    //    }
                    //    else if (custChkVal && esbChkVal)
                    //    {
                    //        resultRte = CustRte.ToString("N2");

                    //        if ((BaseValidate + (CustRte + ESBRte)) < 0)
                    //        {
                    //            txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                    //        }                       
                    //    }
                    //    else
                    //    {
                    //        resultRte = CustRte.ToString("N2");
                    //    }
                    //}
                    //else
                    //{
                    if (custChkVal && !esbChkVal)
                    {
                        CustRte = sumRate > BaseRte ? (BaseRte - ESBRte) : CustRte;
                        resultRte = CustRte.ToString("N2");

                        txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                    }
                    else if (!custChkVal && esbChkVal)
                    {
                        CustRte = sumRate > BaseRte ? (BaseRte - ESBRte) : CustRte;
                        resultRte = CustRte.ToString("N2");

                        txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                    }
                    else if (custChkVal && esbChkVal)
                    {
                        resultRte = CustRte.ToString("N2");

                        //if ((BaseRte - (CustRte + ESBRte)) < 0)
                        //{
                        //    txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                        //}
                        if (negative)
                        {
                            txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                        }
                    }
                    else
                    {
                        resultRte = CustRte.ToString("N2");
                    }
                    //}
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    //if (negative)
                    //{
                    //    if (custChkVal && !esbChkVal)
                    //    {
                    //        ESBRte = sumRate > BaseRte ? (BaseValidate - CustRte) : ESBRte;
                    //        resultRte = ESBRte.ToString("N2");

                    //        txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                    //    }
                    //    else if (!custChkVal && esbChkVal)
                    //    {
                    //        ESBRte = sumRate > BaseRte ? (BaseValidate - CustRte) : ESBRte;
                    //        resultRte = ESBRte.ToString("N2"); ;

                    //        txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                    //    }
                    //    else if (custChkVal && esbChkVal)
                    //    {
                    //        resultRte = ESBRte.ToString("N2");

                    //        if ((BaseValidate - (CustRte + ESBRte)) < 0)
                    //        {
                    //            txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        resultRte = ESBRte.ToString("N2");
                    //    }
                    //}
                    //else
                    //{
                    if (custChkVal && !esbChkVal)
                    {
                        ESBRte = sumRate > BaseRte ? (BaseRte - CustRte) : ESBRte;
                        resultRte = ESBRte.ToString("N2");

                        txtPopupShareSubESBRate.Text = (BaseRte - CustRte).ToString("N2");
                    }
                    else if (!custChkVal && esbChkVal)
                    {
                        ESBRte = sumRate > BaseRte ? (BaseRte - CustRte) : ESBRte;
                        resultRte = ESBRte.ToString("N2"); ;

                        txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                    }
                    else if (custChkVal && esbChkVal)
                    {
                        resultRte = ESBRte.ToString("N2");

                        //if ((BaseRte - (CustRte + ESBRte)) < 0)
                        //{
                        //    txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                        //}
                        if (negative)
                        {
                            txtPopupShareSubCustRate.Text = (BaseRte - ESBRte).ToString("N2");
                        }
                    }
                    else
                    {
                        resultRte = ESBRte.ToString("N2");
                    }
                    //}
                }
                #endregion
            }
            else if (CampaignType.ToUpper() == "SHARESUB")
            {
                #region Type SHARESUB
                if (rateType.ToUpper() == "CUSTOMER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseValidate - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = CustRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = CustRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte - MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            CustRte = sumRate > BaseRte ? BaseRte - (VendorRte + ESBRte + MakerRte) : CustRte;
                            resultRte = CustRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte - VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //CustRte = BaseRte - (VendorRte + ESBRte);
                            //resultRte = CustRte.ToString("N2");

                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (VendorRte + ESBRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (CustRte >= (sumcal * -1))
                                        {
                                            CustRte = CustRte - (sumcal * -1);
                                        }
                                        else if (CustRte > 0 && CustRte < (sumcal * -1))
                                        {
                                            CustRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        txtPopupShareSubESBRate.Text = "0.00";
                                        txtPopupShareSubMakerRate.Text = "0.00";
                                        txtPopupShareSubVendorRate.Text = "0.00";
                                        resultRte = CustRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = CustRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = CustRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "VENDOR")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = VendorRte.ToString("N2");

                            //if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            //{
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}

                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtPopupShareSubVendorRate.Text = VendorRte.ToString("N2");
                                        txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtPopupShareSubVendorRate.Text = VendorRte.ToString("N2");
                                        txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = VendorRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = VendorRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            VendorRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + MakerRte) : VendorRte;
                            resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = VendorRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + VendorRte)) < 0)
                            //{
                            //    //txtPopupShareSubESBRate.Text = "0.00";
                            //    //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtPopupShareSubVendorRate.Text = VendorRte.ToString("N2");
                                        txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + ESBRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (VendorRte >= (sumcal * -1))
                                        {
                                            VendorRte = VendorRte - (sumcal * -1);
                                        }
                                        else if (VendorRte > 0 && VendorRte < (sumcal * -1))
                                        {
                                            VendorRte = 0;
                                        }

                                        txtPopupShareSubVendorRate.Text = VendorRte.ToString("N2");
                                        txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //VendorRte = (BaseRte - (CustRte + ESBRte + MakerRte));
                                        VendorRte = VendorRte > (BaseRte - (CustRte + ESBRte + MakerRte)) ? (BaseRte - (CustRte + ESBRte + MakerRte)) : VendorRte;
                                        resultRte = VendorRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = VendorRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = VendorRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "ESB")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + ESBRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            ESBRte = sumRate > BaseRte ? BaseValidate - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            //resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //if ((BaseValidate - (CustRte + ESBRte)) < 0)
                            //{
                            //    //txtPopupShareSubESBRate.Text = "0.00";
                            //    //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte + VendorRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = ESBRte.ToString("N2");
                                        txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");                                
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte + VendorRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtPopupShareSubVendorRate.Text = (Convert.ToDecimal(txtPopupShareSubVendorRate.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = ESBRte.ToString("N2");
                                        txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtPopupShareSubVendorRate.Text = (Convert.ToDecimal(txtPopupShareSubVendorRate.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = ESBRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = ESBRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = BaseRte - (CustRte + VendorRte);
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2"); ;

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            ESBRte = sumRate > BaseRte ? BaseRte - (CustRte + VendorRte + MakerRte) : ESBRte;
                            resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //ESBRte = CustRte + VendorRte;
                            //resultRte = ESBRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + ESBRte)) < 0)
                            //{
                            //    //txtPopupShareSubESBRate.Text = "0.00";
                            //    //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = ESBRte.ToString("N2");
                                        txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtPopupShareSubVendorRate.Text = (Convert.ToDecimal(txtPopupShareSubVendorRate.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    decimal sumcal = (BaseRte - (CustRte + VendorRte + MakerRte));
                                    if (sumcal < 0)
                                    {
                                        if (ESBRte >= (sumcal * -1))
                                        {
                                            ESBRte = ESBRte - (sumcal * -1);
                                        }
                                        else if (ESBRte > 0 && ESBRte < (sumcal * -1))
                                        {
                                            ESBRte = 0;
                                        }

                                        txtPopupShareSubESBRate.Text = ESBRte.ToString("N2");
                                        txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    }
                                    else
                                    {
                                        //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                        //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                        //ESBRte = ESBRte > (BaseRte - (CustRte + VendorRte + MakerRte)) ? (BaseRte - (CustRte + VendorRte + MakerRte)) : ESBRte;
                                        decimal validESB = (BaseRte - (CustRte + VendorRte + MakerRte));
                                        if (ESBRte > validESB)
                                        {
                                            ESBRte = validESB;
                                        }
                                        else
                                        {
                                            txtPopupShareSubVendorRate.Text = (Convert.ToDecimal(txtPopupShareSubVendorRate.Text) + (validESB - ESBRte)).ToString("N2");
                                        }

                                        resultRte = ESBRte.ToString("N2");
                                    }
                                }
                            }

                            resultRte = ESBRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = ESBRte.ToString("N2");
                        //}
                    }
                }
                else if (rateType.ToUpper() == "MAKER")
                {
                    if (negative)
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseValidate - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = MakerRte.ToString("N2");

                            //if ((BaseValidate - (CustRte + VendorRte)) < 0)
                            //{
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}

                            if ((BaseValidate - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }

                            resultRte = MakerRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = MakerRte.ToString("N2");
                        //}
                    }
                    else
                    {
                        if (custChkVal && vendChkVal && !esbChkVal && makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && !vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                            }
                        }
                        else if (!custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = BaseRte - (CustRte + ESBRte);
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte + MakerRte)).ToString("N2");
                        }
                        else if (custChkVal && vendChkVal && esbChkVal && !makeChkVal)
                        {
                            MakerRte = sumRate > BaseRte ? BaseRte - (CustRte + ESBRte + VendorRte) : MakerRte;
                            resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            if (CampaignType.ToUpper() == "SHARESUB")
                            {
                                txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                            }
                        }
                        else //if (custChkVal && vendChkVal && esbChkVal && makeChkVal)
                        {
                            //VendorRte = CustRte + ESBRte;
                            //resultRte = MakerRte.ToString("N2");

                            //txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //if ((BaseRte - (CustRte + VendorRte)) < 0)
                            //{
                            //    //txtPopupShareSubESBRate.Text = "0.00";
                            //    //txtPopupShareSubVendorRate.Text = (BaseRte - CustRte).ToString("N2");
                            //    txtPopupShareSubCustRate.Text = (BaseRte - (VendorRte + ESBRte)).ToString("N2");
                            //}
                            //else
                            //{
                            //    txtPopupShareSubESBRate.Text = (BaseRte - (CustRte + VendorRte)).ToString("N2");
                            //}
                            if ((BaseRte - (CustRte + ESBRte + MakerRte + VendorRte)) < 0)
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }
                            else
                            {
                                if (CampaignType.ToUpper() == "SHARESUB")
                                {
                                    //txtPopupShareSubVendorRate.Text = (BaseRte - (CustRte + ESBRte + MakerRte)).ToString("N2");
                                    //txtPopupShareSubMakerRate.Text = (BaseRte - (CustRte + ESBRte + VendorRte)).ToString("N2");
                                    //MakerRte = (BaseRte - (CustRte + ESBRte + VendorRte));
                                    MakerRte = MakerRte > (BaseRte - (CustRte + VendorRte + ESBRte)) ? (BaseRte - (CustRte + VendorRte + ESBRte)) : MakerRte;
                                    resultRte = MakerRte.ToString("N2");
                                }
                            }

                            resultRte = MakerRte.ToString("N2");
                        }
                        //else
                        //{
                        //    resultRte = MakerRte.ToString("N2");
                        //}
                    }
                }
                #endregion
            }

            return resultRte;
        }

        protected void BindPopupShareSubtogvPopupShareSub()
        {
            DataTable dt = new DataTable();
            if (campaignStorage.GetCookiesDataSetByKey("dt_popup_sharesub")?.Tables[0]?.Rows.Count <= 0)
            {
                dt = dtShareSub();
            }
            else if (campaignStorage.GetCookiesDataSetByKey("dt_popup_sharesub")?.Tables[0]?.Rows.Count > 0)
            {
                dt = ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).Copy();
            }

            DataRow dr = dt.NewRow();

            if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
            {
                dr["RATES"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubCustRate.Text.Trim()) ? "0.00" : txtPopupShareSubCustRate.Text.Trim());
                dr["C05STO"] = Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubTotalTerm.Text.Trim()) ? "0" : txtPopupShareSubTotalTerm.Text.Trim());
                dr["C05CSQ"] = 1;//Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubTermRange.Text.Trim()) ? "0" : txtPopupShareSubTermRange.Text.Trim());
                dr["C05RSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubTermRange.Text.Trim()) ? "1" : txtPopupShareSubTermRange.Text.Trim());
                dr["C05STM"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubEndTerm.Text.Trim()) ? "0.00" : txtPopupShareSubEndTerm.Text.Trim()) - Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubStartTerm.Text.Trim()) ? "0.00" : txtPopupShareSubStartTerm.Text.Trim()) + 1;//txtCreateCampaignTotalTermRate.Text.Trim();
                dr["C05SBT"] = rdoCreateCampaingTypeVendorCalculateType.SelectedValue;
                dr["Easybuy"] = "EASY BUY";
                dr["ESubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubESBRate.Text.Trim()) ? "0.00" : txtPopupShareSubESBRate.Text.Trim());
                dr["Maker"] = String.IsNullOrEmpty(txtMakerCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtMakerCode.Text.Trim()) ? null : txtMakerCode.Text.Trim();
                dr["MSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubMakerRate.Text.Trim()) ? "0.00" : txtPopupShareSubMakerRate.Text.Trim());
                dr["Vendor"] = String.IsNullOrEmpty(txtVendorCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtVendorCode.Text.Trim()) ? null : txtVendorCode.Text.Trim();
                dr["VSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubVendorRate.Text.Trim()) ? "0.00" : txtPopupShareSubVendorRate.Text.Trim());
                dr["CampaignStartTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubStartTerm.Text.Trim()) ? "0.00" : txtPopupShareSubStartTerm.Text.Trim());
                dr["CampaignEndTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubEndTerm.Text.Trim()) ? "0.00" : txtPopupShareSubEndTerm.Text.Trim());

                dt.Rows.Add(dr);
            }
            else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
            {
                dr["RATES"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubCustRate.Text.Trim()) ? "0.00" : txtPopupShareSubCustRate.Text.Trim());
                dr["C05STO"] = Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubTotalTerm.Text.Trim()) ? "0" : txtPopupShareSubTotalTerm.Text.Trim());
                dr["C05CSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubSubSeq.Text.Trim()) ? "1" : txtPopupShareSubSubSeq.Text.Trim());
                dr["C05RSQ"] = Convert.ToInt32(String.IsNullOrEmpty(txtPopupShareSubTermRange.Text.Trim()) ? "1" : txtPopupShareSubTermRange.Text.Trim());
                dr["C05STM"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubEndTerm.Text.Trim()) ? "0.00" : txtPopupShareSubEndTerm.Text.Trim()) - Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubStartTerm.Text.Trim()) ? "0.00" : txtPopupShareSubStartTerm.Text.Trim()) + 1;//txtCreateCampaignTotalTermRate.Text.Trim();
                dr["C05SBT"] = rdoCreateCampaingTypeVendorCalculateType.SelectedValue;
                dr["Easybuy"] = "EASY BUY";
                dr["ESubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubESBRate.Text.Trim()) ? "0.00" : txtPopupShareSubESBRate.Text.Trim());
                dr["Maker"] = String.IsNullOrEmpty(txtMakerCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtMakerCode.Text.Trim()) ? null : txtMakerCode.Text.Trim();
                dr["MSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubMakerRate.Text.Trim()) ? "0.00" : txtPopupShareSubMakerRate.Text.Trim());
                dr["Vendor"] = String.IsNullOrEmpty(txtVendorCode.Text.Trim()) || String.IsNullOrWhiteSpace(txtVendorCode.Text.Trim()) ? null : txtVendorCode.Text.Trim();
                dr["VSubRate"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubVendorRate.Text.Trim()) ? "0.00" : txtPopupShareSubVendorRate.Text.Trim());
                dr["CampaignStartTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubStartTerm.Text.Trim()) ? "0.00" : txtPopupShareSubStartTerm.Text.Trim());
                dr["CampaignEndTerm"] = Convert.ToDecimal(String.IsNullOrEmpty(txtPopupShareSubEndTerm.Text.Trim()) ? "0.00" : txtPopupShareSubEndTerm.Text.Trim());

                dt.Rows.Add(dr);
            }

            campaignStorage.SetCookiesDataTableByName("dt_popup_sharesub", dt);
            if (campaignStorage.check_dataTable(dt))
            {
                gvPopupShareSub.DataSource = dt;
                gvPopupShareSub.DataBind();
            }
            pPopupShareSubGrid.Visible = true;
        }

        protected void txtPopupShareSubStartTerm_TextChanged(object sender, EventArgs e)
        {
            string resultTerm = _MinTotalTermOfContract.ToString();
            int inputValue = Convert.ToInt32(ConvertToInteger(txtPopupShareSubStartTerm.Text));
            if (inputValue > _MaxTotalTermOfContract && inputValue >= _MinTotalTermOfContract)
            {
                resultTerm = _MaxTotalTermOfContract.ToString();
            }
            else if (inputValue < _MinTotalTermOfContract)
            {
                resultTerm = _MinTotalTermOfContract.ToString();
            }
            else
            {
                resultTerm = inputValue.ToString();
            }

            txtPopupShareSubStartTerm.Text = resultTerm;//Convert.ToInt32(ConvertToInteger(txtStartTerm.Text)) < 1 ? "1" : ConvertToInteger(txtStartTerm.Text);
        }

        protected void txtPopupShareSubEndTerm_TextChanged(object sender, EventArgs e)
        {
            txtPopupShareSubEndTerm.Text = ConvertToInteger(txtPopupShareSubEndTerm.Text);
        }

        protected void ValidatePopupTermMultiRate(int Seq, int Term)
        {
            if (Seq == 1)
            {
                txtPopupShareSubStartTerm.Text = Seq.ToString();
                txtPopupShareSubEndTerm.Text = Term.ToString();
            }
            else
            {
                int TotalTerm = Convert.ToInt32(txtPopupShareSubTotalTerm.Text);
                int oldEndTerm = 1;
                if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "N")
                {
                    oldEndTerm = (from dsMultiRate in ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).AsEnumerable()
                                  where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == Convert.ToInt32(txtPopupShareSubSubSeq.Text)
                                  orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                  select int.Parse(dsMultiRate.Field<string>("CampaignEndTerm"))).FirstOrDefault();
                }
                else if (rbCreateCampaingTypeVendorCampaignSupport.SelectedValue == "Y")
                {
                    oldEndTerm = (from dsMultiRate in ((DataTable)campaignStorage.GetCookiesDataTableByKey("dt_popup_sharesub")).AsEnumerable()
                                  where int.Parse(dsMultiRate.Field<string>("C05CSQ")) == 1
                                  orderby int.Parse(dsMultiRate.Field<string>("C05RSQ")) descending
                                  select int.Parse(dsMultiRate.Field<string>("CampaignEndTerm"))).FirstOrDefault();
                }

                txtPopupShareSubStartTerm.Text = (oldEndTerm + 1).ToString();
                int endTerm = oldEndTerm + Term;

                if (endTerm <= TotalTerm)
                {
                    txtPopupShareSubEndTerm.Text = endTerm.ToString();
                }
                else
                {
                    txtPopupShareSubEndTerm.Text = TotalTerm.ToString();
                    txtPopupShareSubTermofRange.Text = (TotalTerm - oldEndTerm).ToString();
                }
            }
        }

        #region Function control form on change Vendor

        #region  Function show popup alert
        private void PopupAlertCenter_2()
        {
            PopupAlertApp_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupAlertApp_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            popupAlert_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            popupAlert_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductType_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductType_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductCode_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductCode_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Brand_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Brand_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Vendor_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Vendor_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Model_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Model_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductItem_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductItem_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsg_2.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsg_2.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

        }
        #endregion

        private void DEFAULT_CHB_2()
        {
            btnMainDetail_2.Enabled = false;
            DATA_ILMS16_2();
            CLEAR_LIST_CAMPAIGN_2();
            DISIBLE_BOX_CAMPAIGN_2();
            DISIBLE_BOX_PRODUCT_2();
            ENABLE_BOX_VENDOR_2();
            foreach (ListItem itemCampaign in chbCampaign_2.Items)
            {
                itemCampaign.Selected = false;
            }
            foreach (ListItem itemProduct in chbProduct_2.Items)
            {
                itemProduct.Selected = false;
            }
            foreach (ListItem itemProduct in chbVendor_2.Items)
            {
                itemProduct.Selected = true;
            }
        }
        protected void ONCHANGE_CHB_CAMPAIGN_2(object sender, EventArgs e)
        {
            btnMainDetail_2.Enabled = false;
            if (chbCampaign_2.SelectedIndex == 0)
            {
                CLEAR_LIST_CAMPAIGN_2();
                ENABLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                DISIBLE_BOX_VENDOR_2();
                foreach (ListItem itemProduct in chbProduct_2.Items)
                {
                    itemProduct.Selected = false;
                }
                foreach (ListItem itemVendor in chbVendor_2.Items)
                {
                    itemVendor.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN_2();
                DISIBLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                DISIBLE_BOX_VENDOR_2();
            }
        }
        protected void ONCHANGE_CHB_PRODUCT_2(object sender, EventArgs e)
        {
            btnMainDetail_2.Enabled = false;
            if (chbProduct_2.SelectedIndex == 0)
            {
                CLEAR_LIST_CAMPAIGN_2();
                DISIBLE_BOX_CAMPAIGN_2();
                ENABLE_BOX_PRODUCT_2();
                DISIBLE_BOX_VENDOR_2();
                foreach (ListItem itemCampaign in chbCampaign_2.Items)
                {
                    itemCampaign.Selected = false;
                }
                foreach (ListItem itemVendor in chbVendor_2.Items)
                {
                    itemVendor.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN_2();
                DISIBLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                DISIBLE_BOX_VENDOR_2();
            }
        }

        private void CLEAR_LIST_CAMPAIGN_2()
        {
            //ViewState["ds_gvListVendor"] = null;
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

            DataSet ds = new DataSet();
            ds.Tables.Add(dtListCampaign);
            ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
            if (campaignStorage.check_dataTable(dtListCampaign))
            {
                gv_listVendor_2.DataSource = dtListCampaign;
                gv_listVendor_2.DataBind();
            }
        }

        protected void ONCHANGE_CHB_VENDOR_2(object sender, EventArgs e)
        {
            btnMainDetail_2.Enabled = false;
            if (chbVendor_2.SelectedIndex == 0)
            {
                DATA_ILMS16_2();
                CLEAR_LIST_CAMPAIGN_2();
                DISIBLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                ENABLE_BOX_VENDOR_2();
                foreach (ListItem itemCampaign in chbCampaign_2.Items)
                {
                    itemCampaign.Selected = false;
                }
                foreach (ListItem itemProduct in chbProduct_2.Items)
                {
                    itemProduct.Selected = false;
                }
            }
            else
            {
                CLEAR_LIST_CAMPAIGN_2();
                DISIBLE_BOX_CAMPAIGN_2();
                DISIBLE_BOX_PRODUCT_2();
                DISIBLE_BOX_VENDOR_2();
            }
        }

        private void DISIBLE_BOX_CAMPAIGN_2()
        {
            ListItem campaignStatusCampaignNew = campaignStatusCampaign_2.Items.FindByValue("ALL");
            campaignStatusCampaignNew.Selected = true;
            campaignID_2.Enabled = false;
            campaingName_2.Enabled = false;
            campaignStatusCampaign_2.Enabled = false;
            startDate_2.Enabled = false;
            CalendarExtender1_2.Enabled = false;
            calendarStart_2.Enabled = false;
            endDate_2.Enabled = false;
            CalendarExtender2_2.Enabled = false;
            calendarEnd_2.Enabled = false;
            closingDate_2.Enabled = false;
            CalendarExtender3_2.Enabled = false;
            calendarClosing_2.Enabled = false;
            serachCampaign_2.Enabled = false;

            campaignID_2.Text = "";
            campaingName_2.Text = "";

            startDate_2.Text = "";
            endDate_2.Text = "";
            closingDate_2.Text = "";

        }
        private void ENABLE_BOX_CAMPAIGN_2()
        {
            campaignID_2.Enabled = true;
            campaingName_2.Enabled = true;
            campaignStatusCampaign_2.Enabled = true;

            startDate_2.Enabled = true;
            CalendarExtender1_2.Enabled = true;
            calendarStart_2.Enabled = true;
            endDate_2.Enabled = true;
            CalendarExtender2_2.Enabled = true;
            calendarEnd_2.Enabled = true;
            closingDate_2.Enabled = true;
            CalendarExtender3_2.Enabled = true;
            calendarClosing_2.Enabled = true;
            serachCampaign_2.Enabled = true;
        }
        private void DISIBLE_BOX_PRODUCT_2()
        {


            productType_2.Enabled = false;
            serachProductType_2.Enabled = false;
            typeName_2.Enabled = false;
            productCode_2.Enabled = false;
            searchProductCode_2.Enabled = false;
            codeName_2.Enabled = false;
            brandTxt_2.Enabled = false;
            searchBranchTxt_2.Enabled = false;
            brandName_2.Enabled = false;
            modelTxt_2.Enabled = false;
            searchModel_2.Enabled = false;
            modelName_2.Enabled = false;
            productItem_2.Enabled = false;
            productItemDes_2.Enabled = false;
            searchProductItem_2.Enabled = false;
            productType_2.Text = "";
            typeName_2.Text = "";
            productCode_2.Text = "";
            codeName_2.Text = "";
            brandTxt_2.Text = "";
            brandName_2.Text = "";
            modelTxt_2.Text = "";
            modelName_2.Text = "";
            productItem_2.Text = "";
            productItemDes_2.Text = "";
        }
        private void ENABLE_BOX_PRODUCT_2()
        {


            productType_2.Enabled = true;
            serachProductType_2.Enabled = true;
            typeName_2.Enabled = true;
            productCode_2.Enabled = true;
            searchProductCode_2.Enabled = true;
            codeName_2.Enabled = true;
            brandTxt_2.Enabled = true;
            searchBranchTxt_2.Enabled = true;
            brandName_2.Enabled = true;
            modelTxt_2.Enabled = true;
            searchModel_2.Enabled = true;
            modelName_2.Enabled = true;
            productItem_2.Enabled = true;
            productItemDes_2.Enabled = true;
            searchProductItem_2.Enabled = true;
        }
        private void DISIBLE_BOX_VENDOR_2()
        {
            vendorCode_2.Text = "";
            vendorName_2.Text = "";
            odTxt_2.Text = "";
            wqTxt_2.Text = "";
            oldVendorCode_2.Text = "";
            ddlRank_2.Items.Clear();
            searchVendor_2.Enabled = false;
            vendorCode_2.Enabled = false;
            vendorName_2.Enabled = false;
            odTxt_2.Enabled = false;
            wqTxt_2.Enabled = false;
            oldVendorCode_2.Enabled = false;
            ddlRank_2.Enabled = false;
        }
        private void ENABLE_BOX_VENDOR_2()
        {

            vendorCode_2.Enabled = true;
            vendorName_2.Enabled = true;
            odTxt_2.Enabled = true;
            wqTxt_2.Enabled = true;
            oldVendorCode_2.Enabled = true;
            searchVendor_2.Enabled = true;
            ddlRank_2.Enabled = true;

        }
        private void DISIBLE_BOX_Note_2()
        {
        }

        #endregion

        #region List data from vendor
        private void DATA_ILMS16_2()
        {
            SqlAll = "SELECT DISTINCT UPPER(P16RNK) AS P16RNK FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ORDER BY P16RNK";
            DataSet DS_ILMS16 = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS_ILMS16 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS_ILMS16))
            {
                ddlRank_2.DataSource = DS_ILMS16;
                ddlRank_2.DataTextField = "P16RNK";
                ddlRank_2.DataValueField = "P16RNK";
                ddlRank_2.Items.Insert(0, new ListItem("- -Select- -", ""));
                ddlRank_2.DataBind();
            }
        }


        protected void SELECT_VENDOR_ONCHANGE_2(object sender, EventArgs e)
        {
            if (int.Parse(vendorCode_2.Text.Length.ToString()) >= 9)
            {
                SqlAll = " SELECT P16RNK,SUBSTRING(CAST(P16STD AS varchar), 7, 2) + '/' + SUBSTRING(CAST(P16STD AS varchar), 5, 2) + '/' + SUBSTRING(CAST(P16STD AS varchar), 1, 4) + '@' + SUBSTRING(CAST(P16END AS varchar), 7, 2) + '/' + SUBSTRING(CAST(P16END AS varchar), 5, 2) + '/' + SUBSTRING(CAST(P16END AS varchar), 1, 4) AS txtDateRank " +
                      " FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE FORMAT(P16VEN,'000000000000') ='" + vendorCode_2.Text + "' ORDER BY  P16STD ASC ";
                DataSet ds_RankVendor = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                ds_RankVendor = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (ds_RankVendor?.Tables[0]?.Rows.Count > 0)
                {
                    ddlRank_2.DataSource = ds_RankVendor;
                    ddlRank_2.DataTextField = "P16RNK";
                    ddlRank_2.DataValueField = "txtDateRank";
                    ddlRank_2.DataBind();
                    ddlRank_2.Items.Insert(0, "All Rank");
                    ddlRank_2.Enabled = true;
                }
                else
                {
                    ddlRank_2.Enabled = false;
                }

                dataCenter.CloseConnectSQL();


            }
            return;
        }
        #endregion

        #region Function about select campaign (FROM CAMPAIGN)
        protected void TEXT_CHANGE_CAMPAIGN_2(object sender, EventArgs e)
        {
            if (campaignID_2.Text == "")
            {
                campaingName_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_CAMPAIGN_POPUP_2(object sender, EventArgs e)
        {
            popupCampaign_2();
        }
        protected void CLICK_SEARCH_CAMPAIGN_2(object sender, EventArgs e)
        {
            popupCampaign_2();
            Popup_Campaign_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_CAMPAIGN_2(object sender, EventArgs e)
        {
            ddlSearchCampaign_2.SelectedIndex = 0;
            txtSearchCampaign_2.Text = "";
            popupCampaign_2();
        }
        protected void campaign_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_Campaign_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Campaign_2.DataSource = DS;
                gv_Campaign_2.DataBind();
            }
        }
        protected void campaign_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_campaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign_2.Value);
            DataRow drCampaign = ds_campaign?.Tables[0]?.Rows[(gv_Campaign_2.PageIndex * Convert.ToInt16(gv_Campaign_2.PageSize)) + e.NewSelectedIndex];
            campaignID_2.Text = drCampaign[0].ToString().Trim();
            campaingName_2.Text = drCampaign[1].ToString().Trim();

            Popup_Campaign_2.ShowOnPageLoad = false;

            return;
        }
        private void popupCampaign_2()
        {
            string sqlwhere = "";
            if (productType_2.Text != "")
            {
                SqlAll = "SELECT C01CMP, C01CNM FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01CMP LIKE '" + productType_2.Text.Trim() + "%' AND  C01LTY = '01'";
            }
            else
            {
                SqlAll = "SELECT C01CMP, C01CNM FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01LTY = '01'";
            }


            if (ddlSearchCampaign_2.SelectedValue == "CCP" && txtSearchCampaign_2.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(C01CMP as nvarchar) = '" + txtSearchCampaign_2.Text.Trim() + "' AND C01LTY = '01' ORDER BY C01CMP ";
            }
            else if (ddlSearchCampaign_2.SelectedValue == "DCP" && txtSearchCampaign_2.Text.Trim() != "")
            {
                sqlwhere += " AND C01CNM like '" + txtSearchCampaign_2.Text.ToUpper().Trim() + "%' AND C01LTY = '01' ORDER BY C01CMP ";
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
                ds_gvCampaign_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Campaign_2.DataSource = DS;
                gv_Campaign_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product type (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_TYPE_2(object sender, EventArgs e)
        {
            if (productType_2.Text == "")
            {
                typeName_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_TYPE_POPUP_2(object sender, EventArgs e)
        {
            popupProductType_2();
        }
        protected void CLICK_SELECT_PRODUCT_TYPE_2(object sender, EventArgs e)
        {
            popupProductType_2();
            Popup_ProductType_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCTTYPE_2(object sender, EventArgs e)
        {
            ddlSearchProducttype_2.SelectedIndex = 0;
            txtSearchProducttype_2.Text = "";
            popupProductType_2();
        }
        protected void productType_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_ProductType_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_ProductType_2.DataSource = DS;
                gv_ProductType_2.DataBind();
            }
        }
        protected void productType_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productType = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType_2.Value);
            DataRow drProduct = ds_productType?.Tables[0]?.Rows[(gv_ProductType_2.PageIndex * Convert.ToInt16(gv_ProductType_2.PageSize)) + e.NewSelectedIndex];
            productType_2.Text = drProduct[0].ToString().Trim();
            typeName_2.Text = drProduct[1].ToString().Trim();

            Popup_ProductType_2.ShowOnPageLoad = false;

            return;
        }
        private void popupProductType_2()
        {
            string sqlwhere = "";
            if (productType_2.Text != "")
            {
                SqlAll = "SELECT T40TYP, T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40TYP LIKE '" + productType_2.Text.Trim() + "%'";
            }
            else
            {
                SqlAll = "SELECT T40TYP, T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL = '' ";
            }


            if (ddlSearchProducttype_2.SelectedValue == "CPT" && txtSearchProducttype_2.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(T40TYP as nvarchar) = '" + txtSearchProducttype_2.Text.Trim() + "' AND T40DEL = '' ORDER BY T40TYP ";
            }
            else if (ddlSearchProducttype_2.SelectedValue == "DPT" && txtSearchProducttype_2.Text.Trim() != "")
            {
                sqlwhere += " AND T40DES like '" + txtSearchProducttype_2.Text.ToUpper().Trim() + "%' AND T40DEL = '' ORDER BY T40TYP ";
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
                ds_gvProductType_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_ProductType_2.DataSource = DS;
                gv_ProductType_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product code (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_CODE_2(object sender, EventArgs e)
        {
            if (productCode_2.Text == "")
            {
                codeName_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_CODE_2(object sender, EventArgs e)
        {
            popupProductCode_2();
        }
        protected void CLICK_SELECT_PRODUCT_CODE_2(object sender, EventArgs e)
        {
            popupProductCode_2();
            Popup_ProductCode_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCTCODE_2(object sender, EventArgs e)
        {
            ddlSearchProductCode_2.SelectedIndex = 0;
            txtSearchProductCode_2.Text = "";
            popupProductCode_2();
        }
        protected void productCode_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_ProductCode_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_ProductCode_2.DataSource = DS;
                gv_ProductCode_2.DataBind();
            }
        }
        protected void productCode_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productCode = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode_2.Value);
            DataRow drProductCode = ds_productCode?.Tables[0]?.Rows[(gv_ProductCode_2.PageIndex * Convert.ToInt16(gv_ProductCode_2.PageSize)) + e.NewSelectedIndex];
            productCode_2.Text = drProductCode[1].ToString().Trim();
            codeName_2.Text = drProductCode[2].ToString().Trim();
            Popup_ProductCode_2.ShowOnPageLoad = false;

            return;
        }
        private void popupProductCode_2()
        {
            string sqlwhere = "";
            if (productType_2.Text != "")
            {
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode_2.Text + "%' AND T41TYP = " + productType_2.Text + "";
            }
            else
            {
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode_2.Text + "%'";
            }



            if (ddlSearchProductCode_2.SelectedValue == "CPC" && txtSearchProductCode_2.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(T41COD as nvarchar) = '" + txtSearchProductCode_2.Text.Trim() + "' AND T41DEL = '' ORWDER BY T41TYP";
            }
            else if (ddlSearchProductCode_2.SelectedValue == "DPC" && txtSearchProductCode_2.Text.Trim() != "")
            {
                sqlwhere += " AND T41DES like '" + txtSearchProductCode_2.Text.ToUpper().Trim() + "%' AND T41DEL = '' ORDER BY T41TYP";
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
                ds_gvProductCode_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_ProductCode_2.DataSource = DS;
                gv_ProductCode_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select brand (FROM PRODUCT)
        protected void TEXT_CHANGE_BRAND_2(object sender, EventArgs e)
        {
            if (brandTxt_2.Text == "")
            {
                brandName_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_BRAND_POPUP_2(object sender, EventArgs e)
        {
            popupBrand_2();
        }
        protected void CLICK_SELECT_BRAND_2(object sender, EventArgs e)
        {
            popupBrand_2();
            Popup_Brand_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_BRAND_2(object sender, EventArgs e)
        {
            ddlSearchBrands_2.SelectedIndex = 0;
            txtSearchBrands_2.Text = "";
            popupBrand_2();
        }
        protected void brand_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_Brand_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Brand_2.DataSource = DS;
                gv_Brand_2.DataBind();
            }
        }
        protected void brand_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Brand = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType_2.Value);
            DataRow drBrand = ds_Brand?.Tables[0]?.Rows[(gv_Brand_2.PageIndex * Convert.ToInt16(gv_Brand_2.PageSize)) + e.NewSelectedIndex];
            brandTxt_2.Text = drBrand[0].ToString().Trim();
            brandName_2.Text = drBrand[1].ToString().Trim();

            Popup_Brand_2.ShowOnPageLoad = false;

            return;
        }
        private void popupBrand_2()
        {
            string sqlwhere = "";
            SqlAll = "SELECT T42BRD, T42DES FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)";

            if (ddlSearchBrands_2.SelectedValue == "CB" && txtSearchBrands_2.Text.Trim() != "")
            {
                sqlwhere += " WHERE CAST(T42BRD as nvarchar) = " + txtSearchBrands_2.Text.Trim() + " AND T42DEL = '' ORDER BY T42BRD ";
            }
            else if (ddlSearchBrands_2.SelectedValue == "DB" && txtSearchBrands_2.Text.Trim() != "")
            {
                sqlwhere += " WHERE T42DES like '" + txtSearchBrands_2.Text.ToUpper().Trim() + "%' AND T42DEL = '' ORDER BY T42BRD ";
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
                ds_gvProductType_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Brand_2.DataSource = DS;
                gv_Brand_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select model (FROM PRODUCT)
        protected void TEXT_CHANGE_MODEL_2(object sender, EventArgs e)
        {
            if (modelTxt_2.Text == "")
            {
                modelName_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_MODEL_POPUP_2(object sender, EventArgs e)
        {
            popupModel_2();
        }
        protected void CLICK_SELECT_MODEL_2(object sender, EventArgs e)
        {
            popupModel_2();
            Popup_Model_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_MODEL_2(object sender, EventArgs e)
        {
            ddlSearchProducttype_2.SelectedIndex = 0;
            txtSearchProducttype_2.Text = "";
            popupModel_2();
        }
        protected void model_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_Model_2.PageIndex = e.NewPageIndex;
            DataSet DS =  campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Model_2.DataSource = DS;
                gv_Model_2.DataBind();
            }
        }
        protected void model_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Model = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel_2.Value);
            DataRow drModel = ds_Model?.Tables[0]?.Rows[(gv_Model_2.PageIndex * Convert.ToInt16(gv_Model_2.PageSize)) + e.NewSelectedIndex];
            modelTxt_2.Text = drModel[0].ToString().Trim();
            modelName_2.Text = drModel[1].ToString().Trim();

            Popup_Model_2.ShowOnPageLoad = false;

            return;
        }
        private void popupModel_2()
        {
            string sqlwhere = "";
            if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND T43COD =" + productCode_2.Text.Trim() + " AND T43BRD =" + brandTxt_2.Text.Trim() + "";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43COD as nvarchar) ='" + productCode_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text == "" && brandTxt_2.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T43BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WITH (NOLOCK)WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T43BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%' AND CAST(T43TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T43COD as nvarchar) ='" + productCode_2.Text.Trim() + "'";
            }
            else
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt_2.Text.Trim() + "%'";
            }


            if (ddlSearchModel_2.SelectedValue == "CMD" && txtSearchModel_2.Text.Trim() != "")
            {
                sqlwhere += " AND T43MDL= " + txtSearchModel_2.Text.Trim() + " AND T43DEL = '' ORDER BY T43MDL ";
            }
            else if (ddlSearchModel_2.SelectedValue == "DMD" && txtSearchModel_2.Text.Trim() != "")
            {
                sqlwhere += " AND T43DES like '" + txtSearchModel_2.Text.ToUpper().Trim() + "%' AND T43DEL = '' ORDER BY T43MDL ";
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
                ds_gvModel_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Model_2.DataSource = DS;
                gv_Model_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select product item (FROM PRODUCT)
        protected void TEXT_CHANGE_PRODUCT_ITEM_2(object sender, EventArgs e)
        {
            if (productItem_2.Text == "")
            {
                productItemDes_2.Text = "";
            }
        }
        protected void CLICK_SEARCH_PRODUCT_ITEM_POPUP_2(object sender, EventArgs e)
        {
            popupProductItem_2();
        }
        protected void CLICK_SELECT_PRODUCT_ITEM_2(object sender, EventArgs e)
        {
            popupProductItem_2();
            Popup_ProductItem_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_PRODUCT_ITEM_2(object sender, EventArgs e)
        {
            ddlSearchProductItem_2.SelectedIndex = 0;
            txtSearchProductItem_2.Text = "";
            popupProductItem_2();
        }
        protected void productItem_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gv_productItem_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_productItem_2.DataSource = DS;
                gv_productItem_2.DataBind();
            }
        }
        protected void productItem_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_PoductItem = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem_2.Value);
            DataRow drProductItem = ds_PoductItem?.Tables[0]?.Rows[(gv_productItem_2.PageIndex * Convert.ToInt16(gv_productItem_2.PageSize)) + e.NewSelectedIndex];
            productItem_2.Text = drProductItem[0].ToString().Trim();
            productItemDes_2.Text = drProductItem[1].ToString().Trim();

            Popup_ProductItem_2.ShowOnPageLoad = false;

            return;
        }
        private void popupProductItem_2()
        {

            string sqlwhere = "";
            if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text != "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text == "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text == "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text == "" && brandTxt_2.Text != "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text == "" && brandTxt_2.Text == "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }



            else if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text == "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text != "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T44COD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text == "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44TYP as nvarchar) ='" + productType_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text != "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text == "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text == "" && brandTxt_2.Text != "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }



            else if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text != "" && modelTxt_2.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType_2.Text.Trim() + " AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text != "" && brandTxt_2.Text == "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType_2.Text.Trim() + " AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text != "" && productCode_2.Text == "" && brandTxt_2.Text != "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType_2.Text.Trim() + " AND CAST(T44COD as nvarchar) ='" + brandTxt_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else if (productType_2.Text == "" && productCode_2.Text != "" && brandTxt_2.Text != "" && modelTxt_2.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N' AND CAST(T44COD as nvarchar) ='" + productCode_2.Text.Trim() + "' AND CAST(T44BRD as nvarchar) ='" + brandTxt_2.Text.Trim() + "' AND CAST(T44MDL as nvarchar) ='" + modelTxt_2.Text.Trim() + "'";
            }
            else
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem_2.Text.Trim() + "%' AND T44PGP = 'N'";
            }


            if (ddlSearchProductItem_2.SelectedValue == "CPI" && txtSearchProductItem_2.Text.Trim() != "")
            {
                sqlwhere += " AND T44ITM = " + txtSearchProductItem_2.Text.Trim() + " AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
            }
            else if (ddlSearchProductItem_2.SelectedValue == "DPI" && txtSearchProductItem_2.Text.Trim() != "")
            {
                sqlwhere += " AND T44DES like '" + txtSearchProductItem_2.Text.ToUpper().Trim() + "%' AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
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
                ds_gvProductItem_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_productItem_2.DataSource = DS;
                gv_productItem_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function about select vendor (FROM VENDOR)
        protected void TEXT_CHANGE_VENDOR_2(object sender, EventArgs e)
        {
            if (vendorCode_2.Text == "")
            {
                vendorName_2.Text = "";
                odTxt_2.Text = "";
                wqTxt_2.Text = "";
                oldVendorCode_2.Text = "";
                ddlRank_2.Items.Clear();
            }
        }
        protected void ONCHANGE_DATE_RANK_2(object sender, EventArgs e)
        {
            string[] dateRanks = ddlRank_2.SelectedValue.ToString().Trim().Split('@');
            if (ddlRank_2.SelectedIndex > 0)
            {
                //startDateRank.Text = dateRanks[0];
                //endDateRank.Text = dateRanks[1];
            }
            else if (ddlRank_2.SelectedIndex == 0)
            {
                //startDateRank.Text = "";
                //endDateRank.Text = "";
            }
        }
        protected void CLICK_SEARCH_VENDOR_POPUP_2(object sender, EventArgs e)
        {
            popupVendor_2();
        }
        protected void CLICK_SEARCH_VENDOR_2(object sender, EventArgs e)
        {
            popupVendor_2();
            Popup_Vendor_2.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_VENDOR_2(object sender, EventArgs e)
        {
            ddlSearchVendor_2.SelectedIndex = 0;
            txtSearchVendor_2.Text = "";
            popupVendor_2();
        }
        protected void gvVendor_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            gvVendor_2.PageIndex = e.NewPageIndex;
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor_2.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gvVendor_2.DataSource = DS;
                gvVendor_2.DataBind();
            }
        }
        protected void gvVendor_SelectedIndexChanging_2(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Vendor = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor_2.Value);
            DataRow drVendor = ds_Vendor?.Tables[0]?.Rows[(gvVendor_2.PageIndex * Convert.ToInt16(gvVendor_2.PageSize)) + e.NewSelectedIndex];
            vendorCode_2.Text = drVendor[0].ToString().Trim();

            //dataCenter.CloseConnectSQL();
            Popup_Vendor_2.ShowOnPageLoad = false;

            return;
        }
        private void popupVendor_2()
        {
            string sqlwhere = "";
            if (vendorCode_2.Text != "")
            {
                SqlAll = "SELECT P10VEN,P10NAM,P10FIL,P12ODR,P12WOR FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN WHERE P10VEN LIKE'" + vendorCode_2.Text + "%'";
            }
            else
            {
                SqlAll = "SELECT P10VEN,P10NAM,P10FIL,P12ODR,P12WOR FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN WHERE P10VEN LIKE'" + vendorCode_2.Text + "%'";
            }


            if (ddlSearchVendor_2.SelectedValue == "CV" && txtSearchVendor_2.Text.Trim() != "")
            {
                sqlwhere += " AND P10VEN = " + txtSearchVendor_2.Text.Trim() + " AND P10DEL = ''  ORDER BY P10NAM, P10VEN";
            }
            else if (ddlSearchVendor_2.SelectedValue == "DV" && txtSearchVendor_2.Text.Trim() != "")
            {
                sqlwhere += " AND P10NAM like '" + txtSearchVendor_2.Text.ToUpper().Trim() + "%' AND P10DEL = ''  ORDER BY P10NAM, P10VEN";
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
                ds_gvVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gvVendor_2.DataSource = DS;
                gvVendor_2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        protected void CLICK_MAIN_SEARCH_2(object sender, EventArgs e)
        {
            BindDataGridSelected_2();

            if (chbCampaign_2.SelectedValue == "" && chbProduct_2.SelectedValue == "" && chbVendor_2.SelectedValue == "")
            {
                LIST_VENDOR_2();
            }
            else if (chbCampaign_2.SelectedValue == "1" && chbProduct_2.SelectedValue == "" && chbVendor_2.SelectedValue == "")
            {
                LIST_DATA_SELECT_CAMPAIGN_2();
            }
            else if (chbCampaign_2.SelectedValue == "" && chbProduct_2.SelectedValue == "1" && chbVendor_2.SelectedValue == "")
            {
                LIST_DATA_SELECT_PRODUCT_2();
            }
            else if (chbCampaign_2.SelectedValue == "" && chbProduct_2.SelectedValue == "" && chbVendor_2.SelectedValue == "1")
            {
                LIST_DATA_SELECT_VENDOR_2();
            }
            else
            {
                gv_listVendor_2.DataSource = null;
                gv_listVendor_2.DataBind();
            }

            BindCheckboxfromSelected_2();

            btnMainDetail_2.Enabled = true;
            gv_listVendor_2.SetPageIndex(0);
        }

        private void LIST_VENDOR_2()
        {
            try
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
                DataSet DS = new DataSet();

                SqlAll = "SELECT DISTINCT P10VEN,P10NAM,P16RNK,P12ODR,P12WOR,P16END FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) " +
                         "LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN " +
                         "LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN AND " + date97 + " BETWEEN P16STD AND P16END ";

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                if (campaignStorage.check_dataset(DS))
                {
                    gv_listVendor_2.DataSource = DS;
                    gv_listVendor_2.DataBind();
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }

        private void LIST_DATA_SELECT_CAMPAIGN_2()
        {
            #region new
            try
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

                DataSet DS = new DataSet();


                if (startDate_2.Text.Trim() != "")
                {
                    string[] startSplitDate = startDate_2.Text.Trim().Split('/');
                    string startDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
                    startDateNews_2 = int.Parse(startDateNew);
                }

                if (endDate_2.Text.Trim() != "")
                {
                    string[] endtSplitDate = endDate_2.Text.Trim().Split('/');
                    string endDateNew = endtSplitDate[2] + endtSplitDate[1] + endtSplitDate[0];
                    endDateNews_2 = int.Parse(endDateNew);
                }

                if (closingDate_2.Text.Trim() != "")
                {
                    string[] closingSplitDate = closingDate_2.Text.Trim().Split('/');
                    string closingDateNew = closingSplitDate[2] + closingSplitDate[1] + closingSplitDate[0];
                    closingDateNews_2 = int.Parse(closingDateNew);
                }

                if (campaignStatusCampaign_2.SelectedValue.ToString() == "ALL")
                {
                    sqlBetween_2 = " AND C01SDT >=  " + startDateNews_2 + " AND C01SDT <= " + endDateNews_2 + "";
                    sqlChooseCampaign_2 = " AND C01SDT != 0 AND C01EDT != 0";
                }
                else if (campaignStatusCampaign_2.SelectedValue.ToString() == "N")
                {
                    sqlBetween_2 = " AND C01SDT >=  " + startDateNews_2 + " AND C01SDT <= " + endDateNews_2 + "";
                    sqlChooseCampaign_2 = " AND C01CST = '" + campaignStatusCampaign_2.SelectedValue.ToString() + "'";
                }
                else if (campaignStatusCampaign_2.SelectedValue.ToString() == "A")
                {
                    sqlBetween_2 = " AND C01SDT >=  " + startDateNews_2 + " AND C01SDT <= " + endDateNews_2 + "";
                    sqlChooseCampaign_2 = " AND C01CST = '" + campaignStatusCampaign_2.SelectedValue.ToString() + "'";
                }
                else if (campaignStatusCampaign_2.SelectedValue.ToString() == "X")
                {
                    sqlBetween_2 = " AND C01SDT >=  " + startDateNews_2 + " AND C01SDT <= " + endDateNews_2 + "";
                    sqlChooseCampaign_2 = " AND C01CST = '" + campaignStatusCampaign_2.SelectedValue.ToString() + "' OR C01EDT < " + date97 + "";
                }

                //update defect(AC-0044) 04 / 01 / 2564
                if (campaingName_2.Text.Trim() != "")
                {
                    //1
                    if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%'";
                    }
                    //2
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01SDT <= " + endDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + "";
                    }
                    //3
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%'  AND C01SDT <= " + endDateNews_2 + "  AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' " + sqlBetween_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%'  AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT >=  " + startDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01CLD = " + closingDateNews_2 + "";
                    }
                    //4
                    else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' " + sqlBetween_2 + " AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT <= " + endDateNews_2 + "  AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT >=  " + startDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                    }
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + "" + sqlBetween_2 + "";
                    }

                    //5
                    else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != "") && (campaingName_2.Text.Trim() != ""))
                    {
                        sqlCondition_2 = " AND UPPER(C01CNM) LIKE '%" + campaingName_2.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID_2.Text.Trim() + "" + sqlBetween_2 + " AND C01CLD = " + closingDateNews_2 + "";
                    }
                }
                //#

                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + "" + sqlBetween_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01SDT >=  " + startDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01SDT <= " + endDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT >=  " + startDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT <= " + endDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = sqlBetween_2;
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01SDT >=  " + startDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01SDT <= " + endDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() == ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + "" + sqlBetween_2;
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() == "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT >=  " + startDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() != "") && (startDate_2.Text.Trim() == "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = " AND C01CMP = " + campaignID_2.Text.Trim() + " AND C01SDT <= " + endDateNews_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }
                else if ((campaignID_2.Text.Trim() == "") && (startDate_2.Text.Trim() != "") && (endDate_2.Text.Trim() != "") && (closingDate_2.Text.Trim() != ""))
                {
                    sqlCondition_2 = sqlBetween_2 + " AND C01CLD = " + closingDateNews_2 + "";
                }


                SqlAll = "SELECT DISTINCT P10VEN,P10NAM FROM AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK) " +
                         "LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON C08VEN = P10VEN " +
                         "LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN " +
                         "LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN AND " + date97 + " BETWEEN P16STD AND P16END " +
                         "WHERE C08CMP IN (SELECT DISTINCT C01CMP FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01BRN = " + campaignStorage.GetCookiesStringByKey("branch") + "";

                SqlAll = SqlAll + sqlChooseCampaign_2 + sqlCondition_2 + ")";

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                if (campaignStorage.check_dataset(DS))
                {
                    gv_listVendor_2.DataSource = DS;
                    gv_listVendor_2.DataBind();
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            #endregion
        }

        private void LIST_DATA_SELECT_PRODUCT_2()
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
            if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 = " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 = " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 = " WHERE CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 = " WHERE CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 = " WHERE CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 2 unit
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%'";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 3 unit
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 4 unit
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() == ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() == "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() == "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() == "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType_2.Text.Trim() == "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%' ";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 5 unit
            else if ((productType_2.Text.Trim() != "") && (productCode_2.Text.Trim() != "") && (brandTxt_2.Text.Trim() != "") && (modelTxt_2.Text.Trim() != "") && (productItem_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE CHAR(T44TYP) LIKE '%" + productType_2.Text.Trim() + "%' AND UPPER(T40DES) LIKE '" + typeName_2.Text.ToUpper().Trim() + "%'";
                sqlCondition_2 += " AND CHAR(T44COD) LIKE '%" + productCode_2.Text.Trim() + "%' AND UPPER(T41DES) LIKE '" + codeName_2.Text.ToUpper().Trim() + "%'";
                sqlCondition_2 += " AND CHAR(T44BRD) LIKE '%" + brandTxt_2.Text.Trim() + "%' AND UPPER(T42DES) LIKE '" + brandName_2.Text.ToUpper().Trim() + "%'";
                sqlCondition_2 += " AND CHAR(T44MDL) LIKE '%" + modelTxt_2.Text.Trim() + "%' AND UPPER(T43DES) LIKE '" + modelName_2.Text.ToUpper().Trim() + "%'";
                sqlCondition_2 += " AND CHAR(T44ITM) LIKE '%" + productItem_2.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes_2.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 1 unit
            //update defect(AC-0044) 04 / 01 / 2564
            else if ((typeName_2.Text.Trim() != "") || (codeName_2.Text.Trim() != "") || (brandName_2.Text.Trim() != "") || (modelName_2.Text.Trim() != "") || (productItemDes_2.Text.Trim() != ""))
            {
                sqlCondition_2 += " WHERE T44DEL = '' ";
                if (typeName_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(T40DES) LIKE '%" + typeName_2.Text.ToUpper().Trim() + "%'";
                }
                if (codeName_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(T41DES) LIKE '%" + codeName_2.Text.ToUpper().Trim() + "%'";
                }
                if (brandName_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(T42DES) LIKE '%" + brandName_2.Text.ToUpper().Trim() + "%'";
                }
                if (modelName_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(T43DES) LIKE '%" + modelName_2.Text.ToUpper().Trim() + "%'";
                }
                if (productItemDes_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(T44DES) LIKE '%" + productItemDes_2.Text.ToUpper().Trim() + "%'";
                }
            }

            SqlAll = "SELECT DISTINCT P10VEN,P10NAM FROM AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK) " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON C08VEN = P10VEN " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON P10VEN = P12VEN " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN AND " + date97 + " BETWEEN P16STD AND P16END " +
                     " WHERE C08CMP IN (SELECT DISTINCT C07CMP FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK) " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILCP09 WITH (NOLOCK) ON C07CMP = C09CMP AND C09BRN = '" + campaignStorage.GetCookiesStringByKey("branch") + "' " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON C07LNT = T44LTY AND C07PIT = T44ITM " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON T44TYP = T40TYP AND T40DEL = '' " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = '' " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T44BRD = T42BRD AND T42DEL = '' " +
                     " LEFT JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T44TYP = T43TYP AND T44BRD = T43BRD AND " +
                     " T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''";

            if (String.IsNullOrEmpty(sqlCondition_2.Trim()))
            {
                SqlAll = SqlAll + ") ";
            }
            else
            {
                SqlAll = SqlAll + sqlCondition_2 + ") ";
            }

            DataSet DS = new DataSet();

            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (campaignStorage.check_dataset(DS))
            {
                gv_listVendor_2.DataSource = DS;
                gv_listVendor_2.DataBind();
            }

            if (gv_listVendor_2.Rows.Count == 0)
            {
                lblMsgAlertApp_2.Text = "Data not found for searching ";
                PopupAlertApp_2.ShowOnPageLoad = true;
            }

            dataCenter.CloseConnectSQL();

        }

        private void LIST_DATA_SELECT_VENDOR_2()
        {

            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            try
            {
                if (vendorCode_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND P10VEN = " + vendorCode_2.Text.Trim();
                }
                if (vendorName_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(P10NAM) LIKE '%" + vendorName_2.Text.ToUpper().Trim() + "%'";
                }
                if (oldVendorCode_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND UPPER(P10FIL) LIKE '%" + oldVendorCode_2.Text.ToUpper().Trim() + "%'";
                }
                if (odTxt_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND P12ODR = " + odTxt_2.Text.Trim();
                }
                if (wqTxt_2.Text.Trim() != "")
                {
                    sqlCondition_2 += " AND P12WOR = " + wqTxt_2.Text.Trim();
                }
                if (ddlRank_2.SelectedValue.ToString() != "")
                {
                    sqlCondition_2 += " AND P16RNK ='" + ddlRank_2.SelectedValue.ToString() + "'";
                }


                SqlAll = @"SELECT DISTINCT P10VEN,P10NAM FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                            LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON  P10VEN = P12VEN 
                            LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN 
                            WHERE (P16STS is null or P16STS = '') AND P10DEL = '' " + sqlCondition_2 + "";


                DataSet DS = new DataSet();

                ilObj.UserInfomation = m_userInfo;

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                cmd.CommandText = SqlAll;
                ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                if (campaignStorage.check_dataset(DS))
                {
                    gv_listVendor_2.DataSource = DS;
                    gv_listVendor_2.DataBind();
                }

                if (gv_listVendor_2.Rows.Count == 0)
                {
                    lblMsgAlertApp_2.Text = "Data not found for searching ";
                    PopupAlertApp_2.ShowOnPageLoad = true;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg_2 = "The data cannot be searching";
                MsgHText_2 = "Error";
                SET_MSG();


                return;
            }
            dataCenter.CloseConnectSQL();
        }

        private void SET_MSG_2()
        {
            lblMsg.Text = Msg_2;
            PopupMsg_2.HeaderText = MsgHText_2;
            PopupMsg_2.ShowOnPageLoad = true;
        }

        protected void CheckBoxSelectVendor_CheckedChanged_2(object sender, EventArgs e)
        {
        }

        protected void LoaddataFromgvselectlistvendor_2()
        {
            DataSet ds = new DataSet();
            ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistvendor.Value);
            if (campaignStorage.check_dataset(ds))
            {
                DataTable dt = ds.Tables[0].Copy();
                dt_Popup_Vendorlistselected_2.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                if (campaignStorage.check_dataTable(dt))
                {
                    DataSet ds_gv = new DataSet();
                    ds_gv.Tables.Add(dt);
                    ds_gvListVendor_2.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds_gv);
                    gv_listVendor_2.DataSource = dt;
                    gv_listVendor_2.DataBind();
                }
                //P10VEN
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (GridViewRow gvRow in gv_listVendor_2.Rows)
                    {
                        if (gvRow.Cells[1].Text == dr["P10VEN"].ToString())
                        {
                            CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor_2");
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
        }

        protected void SetSelectButton_2()
        {
            foreach (GridViewRow gvRow in gv_listVendor_2.Rows)
            {
                if (gvRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor_2");
                    if (chk.Checked)
                    {
                        btnMainDetail_2.Enabled = true;
                        break;
                    }
                    else
                    {
                        //btnMainDetail.Enabled = false;
                    }
                }
            }
        }

        protected void BindDataGridSelected_2()
        {
            DataTable dtSeleted = new DataTable();
            dtSeleted = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Vendorlistselected_2.Value);
            if (!campaignStorage.check_dataTable(dtSeleted))
            {
                dtSeleted = new DataTable();
                dtSeleted.Columns.Add("P10VEN");
                dtSeleted.Columns.Add("P10NAM");
            }

            foreach (GridViewRow gvRow in gv_listVendor_2.Rows)
            {
                CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor_2");
                if (chk.Checked)
                {
                    DataRow dr = dtSeleted.NewRow();
                    dr["P10VEN"] = gvRow.Cells[1].Text;
                    dr["P10NAM"] = HttpUtility.HtmlDecode(gvRow.Cells[2].Text);// gvRow.Cells[2].Text;

                    dtSeleted.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtSeleted);
            dtSeleted = dv.ToTable(true);
            dt_Popup_Vendorlistselected_2.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtSeleted);
        }
        protected void BindCheckboxfromSelected_2()
        {
            DataTable dt = new DataTable();
            dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Vendorlistselected_2.Value);
            if (campaignStorage.check_dataTable(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string code = dr["P10VEN"].ToString();
                    foreach (GridViewRow gvRow in gv_listVendor_2.Rows)
                    {
                        if (gvRow.Cells[1].Text == code)
                        {
                            CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor_2");
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
        }

        protected void gv_listVendor_PageIndexChanging_2(object sender, GridViewPageEventArgs e)
        {
            BindDataGridSelected();
            gv_listVendor_2.PageIndex = e.NewPageIndex;
            DataSet dsList = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvListVendor_2.Value);
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            ds2 = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistvendor.Value);
            if (campaignStorage.check_dataset(ds2))
            {
                DataTable dt = ds2.Tables[0].Copy();
                ds.Tables.Add(dt);
            }
            if (campaignStorage.check_dataset(dsList))
            {
                gv_listVendor_2.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvListVendor_2.Value);
            }
            gv_listVendor_2.DataBind();
            BindCheckboxfromSelected_2();
        }

        protected void CLICK_MAIN_DETAIL_2(object sender, EventArgs e)
        {
            Bindgv_listVendorToVendorList_2();
            popupSearchVendor.ShowOnPageLoad = false;
            popupSearchVendor.Focus();
            BinddataSearchgvVendorList();
        }

        protected void Bindgv_listVendorToVendorList_2()
        {

            BindDataGridSelected_2();
            DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Vendorlistselected_2.Value);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_gv_selectlistvendor.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
        }
        
        #endregion


        #region  Function show popup alert
        private void PopupAlertCenter_1()
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
            Popup_Search_Product.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Search_Product.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Model.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Model.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_ProductItem.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_ProductItem.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

        }
        #endregion

        #region Function control form on change
        private void DEFAULT_CHB()
        {
            btnMainDetail.Enabled = false;
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
            foreach (ListItem itemProduct in chbProduct.Items)
            {
                itemProduct.Selected = true;
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
            ListItem campaignStatusCampaignNew = campaignStatusCampaign.Items.FindByValue("ALL");
            campaignStatusCampaignNew.Selected = true;
            campaignID.Enabled = false;
            campaingName.Enabled = false;
            campaignStatusCampaign.Enabled = false;
            startDate_1.Enabled = false;
            CalendarExtender1_1.Enabled = false;
            calendarStart_1.Enabled = false;
            endDate_1.Enabled = false;
            CalendarExtender2_1.Enabled = false;
            calendarEnd_1.Enabled = false;
            closingDate_1.Enabled = false;
            CalendarExtender3_1.Enabled = false;
            calendarClosing_1.Enabled = false;
            serachCampaign.Enabled = false;
            campaignID.Text = "";
            campaingName.Text = "";
            startDate_1.Text = "";
            endDate_1.Text = "";
            closingDate_1.Text = "";

        }
        private void ENABLE_BOX_CAMPAIGN()
        {
            campaignID.Enabled = true;
            campaingName.Enabled = true;
            campaignStatusCampaign.Enabled = true;
            startDate_1.Enabled = true;
            CalendarExtender1_1.Enabled = true;
            calendarStart_1.Enabled = true;
            endDate_1.Enabled = true;
            CalendarExtender2_1.Enabled = true;
            calendarEnd_1.Enabled = true;
            closingDate_1.Enabled = true;
            CalendarExtender3_1.Enabled = true;
            calendarClosing_1.Enabled = true;
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

            vendorCode.Text = "";
            vendorName.Text = "";
            odTxt.Text = "";
            wqTxt.Text = "";
            oldVendorCode.Text = "";
            ddlRank.Items.Clear();
            searchVendor.Enabled = false;
            vendorCode.Enabled = false;
            vendorName.Enabled = false;
            odTxt.Enabled = false;
            wqTxt.Enabled = false;
            oldVendorCode.Enabled = false;
            ddlRank.Enabled = false;
        }
        private void ENABLE_BOX_VENDOR()
        {

            vendorCode.Enabled = true;
            vendorName.Enabled = true;
            odTxt.Enabled = true;
            wqTxt.Enabled = true;
            oldVendorCode.Enabled = true;
            searchVendor.Enabled = true;
            ddlRank.Enabled = true;

        }
        private void DISIBLE_BOX_Note()
        {
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
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Campaign.DataSource = DS;
                gv_Campaign.DataBind();
            }
        }
        protected void campaign_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_campaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value); ;
            DataRow drCampaign = ds_campaign?.Tables[0]?.Rows[(gv_Campaign.PageIndex * Convert.ToInt16(gv_Campaign.PageSize)) + e.NewSelectedIndex];
            campaignID.Text = drCampaign[0].ToString().Trim();

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
                sqlwhere += " AND C01CMP = " + txtSearchCampaign.Text.Trim() + " AND C01LTY = '01' ORDER BY C01CMP ";
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
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_ProductType.DataSource = DS;
                gv_ProductType.DataBind();
            }
        }
        protected void productType_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productType = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            DataRow drProduct = ds_productType?.Tables[0]?.Rows[(gv_ProductType.PageIndex * Convert.ToInt16(gv_ProductType.PageSize)) + e.NewSelectedIndex];
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
                sqlwhere += " AND T40TYP = " + txtSearchProducttype.Text.Trim() + " AND T40DEL = '' ORDER BY T40TYP ";
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
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_ProductCode.DataSource = DS;
                gv_ProductCode.DataBind();
            }
        }
        protected void productCode_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_productCode = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductCode.Value);
            DataRow drProductCode = ds_productCode?.Tables[0]?.Rows[(gv_ProductCode.PageIndex * Convert.ToInt16(gv_ProductCode.PageSize)) + e.NewSelectedIndex];
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
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode.Text + "%' AND T41TYP = " + productType.Text + "";
            }
            else
            {
                SqlAll = "SELECT T41TYP, T41COD, T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41COD LIKE'" + productCode.Text + "%'";
            }



            if (ddlSearchProductCode.SelectedValue == "CPC" && txtSearchProductCode.Text.Trim() != "")
            {
                sqlwhere += " AND T41COD = " + txtSearchProductCode.Text.Trim() + " AND T41DEL = '' ORDER BY T41TYP";
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
            DataSet DS =  campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Brand.DataSource = DS;
                gv_Brand.DataBind();
            }
        }
        protected void brand_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Brand = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductType.Value);
            DataRow drBrand = ds_Brand?.Tables[0]?.Rows[(gv_Brand.PageIndex * Convert.ToInt16(gv_Brand.PageSize)) + e.NewSelectedIndex];
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
                sqlwhere += " WHERE T42BRD = " + txtSearchBrands.Text.Trim() + " AND T42DEL = '' ORDER BY T42BRD ";
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
            DataSet DS = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gv_Model.DataSource = DS;
                gv_Model.DataBind();
            }
        }
        protected void model_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Model = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvModel.Value);
            DataRow drModel = ds_Model?.Tables[0]?.Rows[(gv_Model.PageIndex * Convert.ToInt16(gv_Model.PageSize)) + e.NewSelectedIndex];
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
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43TYP =" + productType.Text.Trim() + " AND T43COD =" + productCode.Text.Trim() + " AND T43BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43TYP =" + productType.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43COD =" + productCode.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43COD =" + productCode.Text.Trim() + " AND T43BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43TYP =" + productType.Text.Trim() + " AND T43BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "")
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%' AND T43TYP =" + productType.Text.Trim() + " AND T43COD =" + productCode.Text.Trim() + "";
            }
            else
            {
                SqlAll = "SELECT T43MDL, T43DES FROM AS400DB01.ILOD0001.ILTB43 WHERE T43MDL LIKE '" + modelTxt.Text.Trim() + "%'";
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
            DataSet dataSet = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem.Value);
            if (campaignStorage.check_dataset(dataSet))
            {
                gv_productItem.DataSource = dataSet;
                gv_productItem.DataBind();
            }
        }
        protected void productItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_PoductItem = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvProductItem.Value);
            DataRow drProductItem = ds_PoductItem?.Tables[0]?.Rows[(gv_productItem.PageIndex * Convert.ToInt16(gv_productItem.PageSize)) + e.NewSelectedIndex];
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
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44COD =" + productCode.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44COD =" + productCode.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44MDL =" + modelTxt.Text.Trim() + "";
            }



            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44COD =" + productCode.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44COD =" + productCode.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44COD =" + productCode.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44BRD =" + brandTxt.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }



            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text == "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44COD =" + productCode.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text != "" && brandTxt.Text == "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44COD =" + productCode.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else if (productType.Text != "" && productCode.Text == "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44TYP =" + productType.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else if (productType.Text == "" && productCode.Text != "" && brandTxt.Text != "" && modelTxt.Text != "")
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N' AND T44COD =" + productCode.Text.Trim() + " AND T44BRD =" + brandTxt.Text.Trim() + " AND T44MDL =" + modelTxt.Text.Trim() + "";
            }
            else
            {
                SqlAll = "SELECT T44ITM, T44DES FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE T44ITM LIKE '" + productItem.Text.Trim() + "%' AND T44PGP = 'N'";
            }


            if (ddlSearchProductItem.SelectedValue == "CPI" && txtSearchProductItem.Text.Trim() != "")
            {
                sqlwhere += " AND T44ITM = " + txtSearchProductItem.Text.Trim() + " AND T44PGP = 'N' AND T44DEL = '' ORDER BY T44ITM ";
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
            ds_gvProductItem.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvProductItem.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_productItem.DataSource = DS;
                gv_productItem.DataBind();
            }
            else
            {
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
            }
        }
        protected void ONCHANGE_DATE_RANK(object sender, EventArgs e)
        {
            string[] dateRanks = ddlRank.SelectedValue.ToString().Trim().Split('@');
            if (ddlRank.SelectedIndex > 0)
            {
            }
            else if (ddlRank.SelectedIndex == 0)
            {
            }
        }
        protected void CLICK_SEARCH_VENDOR_POPUP(object sender, EventArgs e)
        {
            popupVendor();
        }
        protected void CLICK_SEARCH_VENDOR(object sender, EventArgs e)
        {
            popupVendor();
            Popup_Search_Product.ShowOnPageLoad = true;

        }
        protected void CLEAR_POPUP_VENDOR(object sender, EventArgs e)
        {
            ddlSearchVendor.SelectedIndex = 0;
            txtSearchVendor.Text = "";
            popupVendor();
        }
        protected void gvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendor_1.PageIndex = e.NewPageIndex;
           DataSet DS =  campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor.Value);
            if (campaignStorage.check_dataset(DS))
            {
                gvVendor_1.DataSource = DS;
                gvVendor_1.DataBind();
            }
        }
        protected void gvVendor_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_Vendor = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvVendor.Value);
            DataRow drVendor = ds_Vendor?.Tables[0]?.Rows[(gvVendor_1.PageIndex * Convert.ToInt16(gvVendor_1.PageSize)) + e.NewSelectedIndex];
            vendorCode.Text = drVendor[0].ToString().Trim();

            dataCenter.CloseConnectSQL();
            Popup_Search_Product.ShowOnPageLoad = false;

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
                sqlwhere += " AND P10VEN = " + txtSearchVendor.Text.Trim() + " AND P10DEL = ''  ORDER BY P10NAM, P10VEN";
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
                gvVendor_1.DataSource = DS;
                gvVendor_1.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region Function search main for page
        protected void BindDataGridSelected()
        {
            DataTable dtSeleted = new DataTable();
            dtSeleted = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Productlistselected.Value);
            if (!campaignStorage.check_dataTable(dtSeleted))
            {
                dtSeleted = new DataTable();
                dtSeleted.Columns.Add("T42BRD");
                dtSeleted.Columns.Add("T42DES");
            }

            foreach (GridViewRow gvRow in gv_listVendor.Rows)
            {
                CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor");
                if (chk.Checked)
                {
                    DataRow dr = dtSeleted.NewRow();
                    dr["T42BRD"] = gvRow.Cells[1].Text;
                    dr["T42DES"] = gvRow.Cells[2].Text;

                    dtSeleted.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtSeleted);
            dtSeleted = dv.ToTable(true);
            if (campaignStorage.check_dataTable(dtSeleted))
            {
                dt_Popup_Productlistselected.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtSeleted);
                gv_listSelected.DataSource = dtSeleted;
                gv_listSelected.DataBind();
            }

        }
        protected void BindCheckboxfromSelected()
        {
            DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Productlistselected.Value);
            if (campaignStorage.check_dataTable(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string code = dr["T42BRD"].ToString();
                    foreach (GridViewRow gvRow in gv_listVendor.Rows)
                    {
                        if (gvRow.Cells[1].Text == code)
                        {
                            CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor");
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
        }
        protected void CLICK_MAIN_SEARCH(object sender, EventArgs e)
        {

            BindDataGridSelected();

            if (chbCampaign.SelectedValue == "" && chbProduct.SelectedValue == "" && chbVendor.SelectedValue == "")
            {
                LIST_VENDOR();
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
                gv_listVendor.DataSource = null;
                gv_listVendor.DataBind();
            }

            //Checkbox from collection.
            BindCheckboxfromSelected();
            btnMainDetail.Enabled = true;
            gv_listVendor.SetPageIndex(0);
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
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gv_listVendor, "Select$" + e.Row.RowIndex);
                }

                GridViewRow gv_listVendor = e.Row;
                gv_listVendor.Cells[0].Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(gv_listVendor.Cells[0].Text.ToString().Trim()));
                gv_listVendor.Cells[1].Text = gv_listVendor.Cells[1].Text.ToString().PadLeft(3, '0');

                DateTime dtime1;
                DateTime.TryParseExact(gv_listVendor.Cells[4].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime1);
                string formattedDate1 = dtime1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listVendor.Cells[4].Text = formattedDate1;

                DateTime dtime2;
                DateTime.TryParseExact(gv_listVendor.Cells[5].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime2);
                string formattedDate2 = dtime2.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listVendor.Cells[5].Text = formattedDate2;

                DateTime dtime3;
                DateTime.TryParseExact(gv_listVendor.Cells[6].Text.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dtime3);
                string formattedDate3 = dtime3.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                gv_listVendor.Cells[6].Text = formattedDate3;



                if (gv_listVendor.Cells[7].Text == "E")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fedcda");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#cc0a00");
                    e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#9E9E9E';this.style.color='#000A0F';";
                    e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#fedcda';this.style.color='#cc0a00';";

                }
                else if (gv_listVendor.Cells[7].Text == "A")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#c9fecc");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#004123");
                    e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#9E9E9E';this.style.color='#000A0F';";
                    e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#c9fecc';this.style.color='#004123';";
                }
                else if (gv_listVendor.Cells[7].Text == "N")
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
            DataSet ds_listCampaign = new DataSet();
            if (!string.IsNullOrEmpty(ds_gvListProduct.Value))
            {
                ds_listCampaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvListProduct.Value);
            }
            else
            {
                ds_listCampaign = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvListProduct");
            }
            DataRow drlistCampaign = ds_listCampaign?.Tables[0]?.Rows[(gv_listVendor.PageIndex * Convert.ToInt16(gv_listVendor.PageSize)) + e.NewSelectedIndex];

            DataSet ds_listCampaignSqev = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            //lblCopyCampaign.Text = String.Format("{0:#-######-##-####}", Convert.ToInt64(drlistCampaign[0].ToString().Trim()));
            //txtTitleCampaign.Text = "Copy Campaign : ";

            SqlAll = "SELECT C11NSQ, C11UDT, C11UTM, C11UUS, C11NOT, C11NSQ, C11NOT FROM AS400DB01.ILOD0001.ILCP11 WITH (NOLOCK) WHERE C11CMP = " + drlistCampaign[0] + "";
            ds_listCampaign = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if(!campaignStorage.check_dataset(ds_listCampaign))
            {
                lblMsgAlertApp.Text = "cann't find data on ILCP11 ";
                PopupAlertApp.ShowOnPageLoad = true;
            }
            int i = 0;
            int countList = ds_listCampaign.Tables[0].Rows.Count;
            string[] dateList = new string[countList];
            string[] timeList = new string[countList];
            string[] userList = new string[countList];
            string[] dashess = new string[countList];
            string[] detailCampaign1 = new string[countList];
            string[] detailCampaign2 = new string[countList];
            string[] sumDetail = new string[countList];


            foreach (DataRow row in ds_listCampaign?.Tables[0]?.Rows)
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

        #region List data from campaign
        private void LIST_DATA_SELECT_CAMPAIGN()
        {
            try
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

                DataSet DS = new DataSet();

                if (startDate_1.Text.Trim() != "")
                {
                    string[] startSplitDate = startDate_1.Text.Trim().Split('/');
                    string startDateNew = startSplitDate[2] + startSplitDate[1] + startSplitDate[0];
                    startDateNews = int.Parse(startDateNew);
                }

                if (endDate_1.Text.Trim() != "")
                {
                    string[] endtSplitDate = endDate_1.Text.Trim().Split('/');
                    string endDateNew = endtSplitDate[2] + endtSplitDate[1] + endtSplitDate[0];
                    endDateNews = int.Parse(endDateNew);
                }

                if (closingDate_1.Text.Trim() != "")
                {
                    string[] closingSplitDate = closingDate_1.Text.Trim().Split('/');
                    string closingDateNew = closingSplitDate[2] + closingSplitDate[1] + closingSplitDate[0];
                    closingDateNews = int.Parse(closingDateNew);
                }

                if (campaignStatusCampaign.SelectedValue.ToString() == "ALL")
                {
                    sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                    sqlChooseCampaign = "  AND C01SDT != 0 AND C01EDT != 0";
                }
                else if (campaignStatusCampaign.SelectedValue.ToString() == "N")
                {
                    //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                    sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                    sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                }
                else if (campaignStatusCampaign.SelectedValue.ToString() == "A")
                {
                    //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                    sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                    sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                }
                else if (campaignStatusCampaign.SelectedValue.ToString() == "X")
                {
                    //sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "'";
                    sqlBetween = " AND C01SDT >=  " + startDateNews + " AND C01SDT <= " + endDateNews + "";
                    sqlChooseCampaign = " AND C01CST = '" + campaignStatusCampaign.SelectedValue.ToString() + "' OR C01EDT < " + date97 + "";
                }

                //update defect(AC-0044) 04 / 01 / 2564
                if (campaingName.Text.Trim() != "")
                {
                    //1
                    if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'";
                    }
                    //2
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT <= " + endDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "";
                    }
                    //3
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'  AND C01SDT <= " + endDateNews + "  AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' " + sqlBetween + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%'  AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01CLD = " + closingDateNews + "";
                    }
                    //4
                    else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' " + sqlBetween + " AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + "  AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                    }
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + "";
                    }

                    //5
                    else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != "") && (campaingName.Text.Trim() != ""))
                    {
                        sqlCondition = " AND UPPER(C01CNM) LIKE '%" + campaingName.Text.ToUpper().Trim() + "%' AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + " AND C01CLD = " + closingDateNews + "";
                    }
                }
                //#

                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01SDT >=  " + startDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01SDT <= " + endDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = sqlBetween;
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01SDT <= " + endDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() == ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + "" + sqlBetween;
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() == "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT >=  " + startDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() != "") && (startDate_1.Text.Trim() == "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = " AND C01CMP = " + campaignID.Text.Trim() + " AND C01SDT <= " + endDateNews + " AND C01CLD = " + closingDateNews + "";
                }
                else if ((campaignID.Text.Trim() == "") && (startDate_1.Text.Trim() != "") && (endDate_1.Text.Trim() != "") && (closingDate_1.Text.Trim() != ""))
                {
                    sqlCondition = sqlBetween + " AND C01CLD = " + closingDateNews + "";
                }


                SqlAll = @"SELECT DISTINCT T44ITM AS T42BRD,T44DES AS T42DES 
                        FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK)
                        JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON C07LNT = T44LTY AND C07PIT = T44ITM AND T44DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON T44TYP = T40TYP AND T40DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T44BRD = T42BRD AND T42DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T44TYP = T43TYP AND T44BRD = T43BRD AND
                        T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''
                        WHERE C07CMP IN(SELECT C01CMP FROM AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) WHERE C01CMP != '0' ";


                SqlAll = SqlAll + sqlChooseCampaign + sqlCondition + ")";


                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                if (campaignStorage.check_dataset(DS))
                {
                    ds_gvListProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                    gv_listVendor.DataSource = DS;
                    gv_listVendor.DataBind();
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
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
                sqlCondition = " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition = " WHERE CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition = " WHERE CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 2 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%'";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 3 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 4 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() == ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() == "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() == "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() == "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%'  AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }
            else if ((productType.Text.Trim() == "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%'  AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%'  AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%'  AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%' ";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
            }

            //case sql search 5 unit
            else if ((productType.Text.Trim() != "") && (productCode.Text.Trim() != "") && (brandTxt.Text.Trim() != "") && (modelTxt.Text.Trim() != "") && (productItem.Text.Trim() != ""))
            {
                sqlCondition += " WHERE CAST(T44TYP as char) LIKE '%" + productType.Text.Trim() + "%' AND UPPER(T40DES) LIKE '" + typeName.Text.ToUpper().Trim() + "%'";
                sqlCondition += " AND CAST(T44COD as char) LIKE '%" + productCode.Text.Trim() + "%' AND UPPER(T41DES) LIKE '" + codeName.Text.ToUpper().Trim() + "%'";
                sqlCondition += " AND CAST(T44BRD as char) LIKE '%" + brandTxt.Text.Trim() + "%' AND UPPER(T42DES) LIKE '" + brandName.Text.ToUpper().Trim() + "%'";
                sqlCondition += " AND CAST(T44MDL as char) LIKE '%" + modelTxt.Text.Trim() + "%' AND UPPER(T43DES) LIKE '" + modelName.Text.ToUpper().Trim() + "%'";
                sqlCondition += " AND CAST(T44ITM as char) LIKE '%" + productItem.Text.Trim() + "%'  AND UPPER(T44DES) LIKE '" + productItemDes.Text.ToUpper().Trim() + "%'";
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

            SqlAll = @"SELECT DISTINCT T44ITM AS T42BRD, T44DES AS T42DES
                   FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) 
                   JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON T44TYP = T40TYP AND T40DEL = ''
                   JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = ''
                   JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T44BRD = T42BRD AND T42DEL = ''
                   JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T44TYP = T43TYP AND T44BRD = T43BRD AND T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''";

            SqlAll = SqlAll + sqlCondition + " ";
            //SqlAll = SqlAll + sqlChooseCampaign + sqlCondition;

            DataSet DS = new DataSet();

            ilObj.UserInfomation = m_userInfo;

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            ds_gvListProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (campaignStorage.check_dataset(DS))
            {
                gv_listVendor.DataSource = DS;
                gv_listVendor.DataBind();
            }
            else
            {
                //gv_listVendor.DataSource = DS;
                //gv_listVendor.DataBind();
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
            if (campaignStorage.check_dataset(DS_ILMS16))
            {
                ddlRank.DataSource = DS_ILMS16;
                ddlRank.DataTextField = "P16RNK";
                ddlRank.DataValueField = "P16RNK";
                ddlRank.Items.Insert(0, new ListItem("- -Select- -", ""));
                ddlRank.DataBind();
            }
        }
        private void LIST_DATA_SELECT_VENDOR()
        {

            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            try
            {
                if (vendorCode.Text.Trim() != "")
                {
                    sqlCondition += " AND C08VEN = " + vendorCode.Text.Trim();
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
                if (ddlRank.SelectedValue.ToString() != "")
                {
                    sqlCondition += " AND P16RNK ='" + ddlRank.SelectedValue.ToString() + "'";
                }


                //SqlAll = @"SELECT DISTINCT C07PIT AS T42BRD, T44DES AS T42DES
                //                FROM ILCP07
                //                INNER JOIN ILTB44 ON C07LNT = T44LTY AND C07PIT = T44ITM AND T44DEL = ''
                //                INNER JOIN ILTB40 ON T44TYP = T40TYP AND T40DEL = ''
                //                INNER JOIN ILTB41 ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = ''
                //                INNER JOIN ILTB42 ON T44BRD = T42BRD AND T42DEL = ''
                //                INNER JOIN ILTB43 ON T44TYP = T43TYP AND T44BRD = T43BRD AND T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''
                //                WHERE C07CMP IN(SELECT DISTINCT C08CMP FROM ILCP08 
                //                LEFT JOIN ILMS10 ON C08VEN = P10VEN 
                //                LEFT JOIN ILMS12 ON C08VEN = P12VEN 
                //                LEFT JOIN ILMS16 ON P10VEN = P16VEN 
                //                WHERE  P16STS = '' AND P12DEL = '' AND P10DEL = '' " + sqlCondition + " )";
                SqlAll = @"SELECT DISTINCT C07PIT AS T42BRD, T44DES AS T42DES
                            FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK)
                            INNER JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON T44ITM = C07PIT AND T44DEL = ''
                            INNER JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) ON T44TYP = T40TYP AND T40DEL = ''
                            INNER JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T44COD = T41COD AND T44TYP = T41TYP AND T41DEL = ''
                            INNER JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T44BRD = T42BRD AND T42DEL = ''
                            INNER JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T44TYP = T43TYP AND T44BRD = T43BRD AND T44COD = T43COD AND T44MDL = T43MDL AND T43DEL = ''
                            WHERE C07CMP IN(SELECT DISTINCT C08CMP FROM AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK)
                            LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON C08VEN = P10VEN 
                            LEFT JOIN AS400DB01.ILOD0001.ILMS12 WITH (NOLOCK) ON C08VEN = P12VEN 
                            LEFT JOIN AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) ON P10VEN = P16VEN 
                            WHERE  P16STS = '' AND P12DEL = '' AND P10DEL = '' " + sqlCondition + " )";


                DataSet DS = new DataSet();

                ilObj.UserInfomation = m_userInfo;

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    //campaignStorage.SetCookiesDataSetByName("ds_gvListProduct", DS);
                    ds_gvListProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                    gv_listVendor.DataSource = DS;
                    gv_listVendor.DataBind();
                }
                if (gv_listVendor.Rows.Count == 0)
                {
                    lblMsgAlertApp.Text = "Data not found for searching ";
                    PopupAlertApp.ShowOnPageLoad = true;
                }

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = "The data cannot be searching";
                MsgHText = "Error";
                SET_MSG();


                return;
            }
            dataCenter.CloseConnectSQL();
        }

        protected void SELECT_VENDOR_ONCHANGE(object sender, EventArgs e)
        {
            if (int.Parse(vendorCode.Text.Length.ToString()) >= 9)
            {
                //SqlAll = "SELECT P16RNK,SubStr(P16STD, 7, 2)|| '/' ||SubStr(P16STD, 5, 2)|| '/' ||SubStr(P16STD, 1, 4)|| '@' ||SubStr(P16END, 7, 2)|| '/' ||SubStr(P16END, 5, 2)|| '/' ||SubStr(P16END, 1, 4) AS txtDateRank " +
                //      " FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE P16VEN ='" + vendorCode.Text + "' ORDER BY  P16STD ASC ";
                SqlAll = "SELECT P16RNK,SUBSTRING(CAST(P16STD as varchar), 7, 2) + '/' +SUBSTRING(CAST(P16STD as varchar), 5, 2)+ '/' +SUBSTRING(CAST(P16STD AS varchar), 1, 4)+ '@' +SUBSTRING(CAST(P16END AS varchar), 7, 2)+ '/' +SUBSTRING(CAST(P16END AS varchar), 5, 2)+ '/' +SUBSTRING(CAST(P16END AS varchar), 1, 4) AS txtDateRank  " +
                    "FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE WHERE FORMAT(P16VEN,'000000000000') ='" + vendorCode.Text + "' ORDER BY  P16STD ASC ";
                DataSet ds_RankVendor = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                ds_RankVendor = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                if (!campaignStorage.check_dataset(ds_RankVendor))
                {
                    return;
                }
                if (ds_RankVendor?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow drVendorRank = ds_RankVendor?.Tables[0].Rows[0];
                    string[] dateRanks = drVendorRank[1].ToString().Trim().Split('@');
                    //startDateRank.Text = "";
                    //endDateRank.Text = "";
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
            //ViewState["ds_gvListProduct"] = null;
            DataTable dtListCampaign = new DataTable();
            dtListCampaign.Columns.AddRange(new DataColumn[9] { new DataColumn("C01CMP"),
                new DataColumn("C01BRN"),
                new DataColumn("C01CTY"),
                new DataColumn("C01CNM"),
                new DataColumn("C01SDT"),
                new DataColumn("C01EDT"),
                new DataColumn("C01CLD"),
                new DataColumn("C01CST"),
                new DataColumn("C01LTY") 
            });

            ds_gvListProduct.Value = campaignStorage.DataTableToJsonObj(dtListCampaign);
            if (campaignStorage.check_dataTable(dtListCampaign))
            {
                gv_listVendor.DataSource = dtListCampaign;
                gv_listVendor.DataBind();
            }

            if (campaignStorage.check_dataTable(dtListCampaign))
            {
                gv_listSelected.DataSource = dtListCampaign;
                gv_listSelected.DataBind();
            }
        }
        private void CLEAR_gv_listVendor()
        {
            //ViewState["ds_gvListProduct"] = null;
            DataTable dtListCampaign = new DataTable();
            dtListCampaign.Columns.AddRange(new DataColumn[9] { new DataColumn("C01CMP"),
                new DataColumn("C01BRN"),
                new DataColumn("C01CTY"),
                new DataColumn("C01CNM"),
                new DataColumn("C01SDT"),
                new DataColumn("C01EDT"),
                new DataColumn("C01CLD"),
                new DataColumn("C01CST"),
                new DataColumn("C01LTY") 
            });

            ds_gvListProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dtListCampaign);
            if (campaignStorage.check_dataTable(dtListCampaign))
            {
                gv_listVendor.DataSource = dtListCampaign;
                gv_listVendor.DataBind();
            }

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


        #endregion

        #region List verdor
        private void LIST_VENDOR()
        {
            try
            {
                DataSet DS = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);

                SqlAll = "SELECT T42BRD,T42DES FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) WHERE T42DEL= ''";

                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (campaignStorage.check_dataset(DS))
                {
                    ds_gvListProduct.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                    gv_listVendor.DataSource = DS;
                    gv_listVendor.DataBind();
                }
            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            dataCenter.CloseConnectSQL();
            return;
        }
        #endregion

        protected void CLICK_MAIN_DETAIL(object sender, EventArgs e)
        {
            Bindgv_listVendorToVendorList();
            popupSearchProduct.ShowOnPageLoad = false;
            popupSearchProduct.Focus();

        }
        protected void Bindgv_listVendorToVendorList()
        {
            BindDataGridSelected();
            DataTable dt = campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Productlistselected.Value);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_gv_selectlistproduct.Value=  campaignStorage.JsonSerializeObjectHiddenDataSet(ds);
        }

        protected void CheckBoxSelectVendor_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void LoaddataFromgvselectlistProduct()
        {
            DataSet ds = new DataSet();
            ds = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gv_selectlistproduct.Value);
            if (campaignStorage.check_dataset(ds))
            {
                DataTable dt = ds?.Tables[0]?.Copy();
                dt_Popup_Productlistselected.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                gv_listSelected.DataSource = dt;
                gv_listSelected.DataBind();

            }
            else
            {
                CLEAR_LIST_CAMPAIGN();
            }
            CLEAR_gv_listVendor();
            
        }

        protected void SetSelectButton()
        {
            foreach (GridViewRow gvRow in gv_listVendor.Rows)
            {
                if (gvRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor");
                    if (chk.Checked)
                    {
                        btnMainDetail.Enabled = true;
                        break;
                    }
                    else
                    {
                        btnMainDetail.Enabled = false;
                    }
                }
            }
        }

        protected void gv_listVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindDataGridSelected();
            gv_listVendor.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(ds_gvListProduct.Value))
            {
                gv_listVendor.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvListProduct.Value);
            }
            gv_listVendor.DataBind();
            BindCheckboxfromSelected();
        }

        protected void gv_listSelected_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_listSelected.PageIndex = e.NewPageIndex;
            DataTable DT  = (DataTable)campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Productlistselected.Value);
            if (campaignStorage.check_dataTable(DT))
            {
                gv_listSelected.DataSource = DT;
                gv_listSelected.DataBind();
            }
        }

        protected void gv_listSelected_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string code = e.Values["T42BRD"].ToString();
                DataTable dt = (DataTable)campaignStorage.JsonDeserializeObjectHiddenDataTable(dt_Popup_Productlistselected.Value);

                var dtdelete = dt.AsEnumerable().Where(r => r.Field<string>("T42BRD") != code).ToList();//.CopyToDataTable();
                if (dtdelete.Count > 0)
                {
                    dt = dtdelete.CopyToDataTable();
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("T42BRD");
                    dt.Columns.Add("T42DES");
                }
                dt_Popup_Productlistselected.Value = campaignStorage.JsonSerializeObjectHiddenDataDataTable(dt);
                if (campaignStorage.check_dataTable(dt))
                {
                    gv_listSelected.DataSource = dt;
                    gv_listSelected.DataBind();
                }

                foreach (GridViewRow gvRow in gv_listVendor.Rows)
                {
                    if (gvRow.Cells[1].Text == code)
                    {
                        CheckBox chk = (CheckBox)gvRow.FindControl("CheckBoxSelectVendor");
                        chk.Checked = false;
                        break;
                    }
                }
            }
        }
        public DataTable CreateILCP01()
        {
            var dt = new DataTable();
            dt.Columns.Add("C01CMP", typeof(decimal));
            dt.Columns.Add("C01LTY", typeof(string));
            dt.Columns.Add("C01BRN", typeof(decimal));
            dt.Columns.Add("C01STY", typeof(string));
            dt.Columns.Add("C01SBT", typeof(string));
            dt.Columns.Add("C01CTY", typeof(string));
            dt.Columns.Add("C01RNG", typeof(string));
            dt.Columns.Add("C01CNM", typeof(string));
            dt.Columns.Add("C01PTY", typeof(string));
            dt.Columns.Add("C01VDC", typeof(decimal));
            dt.Columns.Add("C01MKC", typeof(decimal));
            dt.Columns.Add("C01SDT", typeof(decimal));
            dt.Columns.Add("C01EDT", typeof(decimal));
            dt.Columns.Add("C01CAD", typeof(decimal));
            dt.Columns.Add("C01CLD", typeof(decimal));
            dt.Columns.Add("C01NXD", typeof(decimal));
            dt.Columns.Add("C01FIN", typeof(decimal));
            dt.Columns.Add("C01SRT", typeof(decimal));
            dt.Columns.Add("C01TRG", typeof(decimal));
            dt.Columns.Add("C01CST", typeof(string));
            dt.Columns.Add("C01INV", typeof(decimal));
            dt.Columns.Add("C01MKT", typeof(string));
            dt.Columns.Add("C01WDT", typeof(string));
            dt.Columns.Add("C01UDT", typeof(decimal));
            dt.Columns.Add("C01UTM", typeof(decimal));
            dt.Columns.Add("C01UUS", typeof(string));
            dt.Columns.Add("C01UPG", typeof(string));
            dt.Columns.Add("C01UWS", typeof(string));
            dt.Columns.Add("C01VTY", typeof(string));
            dt.Columns.Add("C01VCR", typeof(decimal));
            dt.Columns.Add("C01PMT", typeof(string));
            return dt;
        }
        public DataTable CreateILCP02()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C02CMP", typeof(decimal));
            dt.Columns.Add("C02CSQ", typeof(decimal));
            dt.Columns.Add("C02RSQ", typeof(decimal));
            dt.Columns.Add("C02FMT", typeof(decimal));
            dt.Columns.Add("C02TOT", typeof(decimal));
            dt.Columns.Add("C02AIR", typeof(decimal));
            dt.Columns.Add("C02ACR", typeof(decimal));
            dt.Columns.Add("C02INR", typeof(decimal));
            dt.Columns.Add("C02CRR", typeof(decimal));
            dt.Columns.Add("C02IFR", typeof(decimal));
            dt.Columns.Add("C02INS", typeof(decimal));
            dt.Columns.Add("C02SPR", typeof(decimal));
            dt.Columns.Add("C02EPR", typeof(decimal));
            dt.Columns.Add("C02TTR", typeof(decimal));
            dt.Columns.Add("C02TTM", typeof(decimal));
            dt.Columns.Add("C02UDT", typeof(decimal));
            dt.Columns.Add("C02UTM", typeof(decimal));
            dt.Columns.Add("C02UUS", typeof(string));
            dt.Columns.Add("C02UPG", typeof(string));
            dt.Columns.Add("C02UWS", typeof(string));

            return dt;
        }
        public DataTable CreateILCP04()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C04CMP", typeof(decimal));
            dt.Columns.Add("C04PTY", typeof(decimal));
            dt.Columns.Add("C04PCD", typeof(decimal));
            dt.Columns.Add("C04PIT", typeof(decimal));
            dt.Columns.Add("C04UDT", typeof(decimal));
            dt.Columns.Add("C04UTM", typeof(decimal));
            dt.Columns.Add("C04UUS", typeof(string));
            dt.Columns.Add("C04UPG", typeof(string));
            dt.Columns.Add("C04UWS", typeof(string));
            dt.Columns.Add("C04RST", typeof(string));

            
            return dt;
        }
        public DataTable CreateILCP05()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C05CMP", typeof(decimal));
            dt.Columns.Add("C05CSQ", typeof(decimal));
            dt.Columns.Add("C05RSQ", typeof(decimal));
            dt.Columns.Add("C05PAR", typeof(string));
            dt.Columns.Add("C05PCD", typeof(decimal));
            dt.Columns.Add("C05SBT", typeof(string));
            dt.Columns.Add("C05SIR", typeof(decimal));
            dt.Columns.Add("C05SCR", typeof(decimal));
            dt.Columns.Add("C05SFR", typeof(decimal));
            dt.Columns.Add("C05STR", typeof(decimal));
            dt.Columns.Add("C05SFM", typeof(decimal));
            dt.Columns.Add("C05STO", typeof(decimal));
            dt.Columns.Add("C05SST", typeof(decimal));
            dt.Columns.Add("C05EST", typeof(decimal));
            dt.Columns.Add("C05STM", typeof(decimal));
            dt.Columns.Add("C05UDT", typeof(decimal));
            dt.Columns.Add("C05UTM", typeof(decimal));
            dt.Columns.Add("C05UUS", typeof(string));
            dt.Columns.Add("C05UPG", typeof(string));
            dt.Columns.Add("C05UWS", typeof(string));
            dt.Columns.Add("C05RST", typeof(string));

            return dt;
        }
        public DataTable CreateILCP06()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C06CMP", typeof(decimal));
            dt.Columns.Add("C06APT", typeof(string));
            dt.Columns.Add("C06UDT", typeof(decimal));
            dt.Columns.Add("C06UTM", typeof(decimal));
            dt.Columns.Add("C06UUS", typeof(string));
            dt.Columns.Add("C06UPG", typeof(string));
            dt.Columns.Add("C06UWS", typeof(string));
            dt.Columns.Add("C06RST", typeof(string));

            return dt;
        }
        public DataTable CreateILCP07()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C07CMP", typeof(decimal));
            dt.Columns.Add("C07CSQ", typeof(decimal));
            dt.Columns.Add("C07LNT", typeof(string));
            dt.Columns.Add("C07PIT", typeof(decimal));
            dt.Columns.Add("C07FIX", typeof(string));
            dt.Columns.Add("C07PRC", typeof(decimal));
            dt.Columns.Add("C07MIN", typeof(decimal));
            dt.Columns.Add("C07MAX", typeof(decimal));
            dt.Columns.Add("C07DOW", typeof(decimal));
            dt.Columns.Add("C07UDT", typeof(decimal));
            dt.Columns.Add("C07UTM", typeof(decimal));
            dt.Columns.Add("C07UUS", typeof(string));
            dt.Columns.Add("C07UPG", typeof(string));
            dt.Columns.Add("C07UWS", typeof(string));
            dt.Columns.Add("C07RST", typeof(string));

            return dt;
        }
        public DataTable CreateILCP08()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C08CMP", typeof(decimal));
            dt.Columns.Add("C08VEN", typeof(decimal));
            dt.Columns.Add("C08UDT", typeof(decimal));
            dt.Columns.Add("C08UTM", typeof(decimal));
            dt.Columns.Add("C08UUS", typeof(string));
            dt.Columns.Add("C08UPG", typeof(string));
            dt.Columns.Add("C08UWS", typeof(string));
            dt.Columns.Add("C08RST", typeof(string));

            return dt;
        }
        public DataTable CreateILCP09()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C09CMP", typeof(decimal));
            dt.Columns.Add("C09BRN", typeof(decimal));
            dt.Columns.Add("C09UDT", typeof(decimal));
            dt.Columns.Add("C09UTM", typeof(decimal));
            dt.Columns.Add("C09UUS", typeof(string));
            dt.Columns.Add("C09UPG", typeof(string));
            dt.Columns.Add("C09UWS", typeof(string));
            dt.Columns.Add("C09RST", typeof(string));

            return dt;
        }
        public DataTable CreateILCP11()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C11CMP", typeof(decimal));
            dt.Columns.Add("C11NSQ", typeof(decimal));
            dt.Columns.Add("C11LSQ", typeof(decimal));
            dt.Columns.Add("C11NOT", typeof(string));
            dt.Columns.Add("C11UDT", typeof(decimal));
            dt.Columns.Add("C11UTM", typeof(decimal));
            dt.Columns.Add("C11UUS", typeof(string));
            dt.Columns.Add("C11UPG", typeof(string));
            dt.Columns.Add("C11UWS", typeof(string));

            return dt;
        }
        public DataTable CreateILCP99()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("C99CMP", typeof(decimal));
            dt.Columns.Add("C99BRN", typeof(decimal));
            dt.Columns.Add("C99CBR", typeof(string));
            dt.Columns.Add("C99EDT", typeof(string));
            dt.Columns.Add("C99UST", typeof(string));
            dt.Columns.Add("C99SPM", typeof(string));
            dt.Columns.Add("C99UDT", typeof(decimal));
            dt.Columns.Add("C99UTM", typeof(decimal));
            dt.Columns.Add("C99UUS", typeof(string));
            dt.Columns.Add("C99UPG", typeof(string));
            dt.Columns.Add("C99UWS", typeof(string));
            dt.Columns.Add("C99RST", typeof(string));

            return dt;
        }
        public DataTable CreateILMS99()
        {
            var dt = new DataTable();

            // เพิ่มคอลัมน์ทั้งหมดใน DataTable
            dt.Columns.Add("P99LNT", typeof(string));
            dt.Columns.Add("P99REC", typeof(string));
            dt.Columns.Add("P99RUN", typeof(decimal));
            dt.Columns.Add("P99UPD", typeof(decimal));
            dt.Columns.Add("P99TIM", typeof(decimal));
            dt.Columns.Add("P99UPG", typeof(string));
            dt.Columns.Add("P99USR", typeof(string));
            dt.Columns.Add("P99DSP", typeof(string));

            return dt;
        }
    }
}