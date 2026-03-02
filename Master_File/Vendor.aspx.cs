using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EB_Service.Commons;
using ILSystem.App_Code.BLL.Integrate;
using ServiceStack.Script;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Windows.Media;
using static ILSystem.App_Code.Model.CompanyBlacklist.CompanyBlacklist;

public partial class ManageData_WorkProcess_Vendor : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private DateTimeFormatInfo _dateThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong _updatgeDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong _updateTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    public UserInfo m_userInfo;
    private string Mode;
    public UserInfoService userInfoService;
    private string Message;
    private string MessageHeaderText;
    public CookiesStorage cookiesStorage;
    private string ConfirmAddReferenceMessage;
    private string ConfirmAddReferenceHeadText;

    private string ConfirmDeleteReferenceMessage;
    private string ConfirmDeleteReferenceHeadText;

    private string ConfirmAddPersonToContactMessage;
    private string ConfirmAddPersonToContactHeadText;

    private string ConfirmDeletePersonToContactMessage;
    private string ConfirmDeletePersonToContactHeadText;

    private string ConfirmAddNoteMessage;
    private string ConfirmAddNoteHeadText;

    private string ConfirmAddEditMessage;
    private string ConfirmAddEditHeadText;

    private string ConfirmDeleteMessge;
    private string ConfirmDeleteHeadText;

    private string ProgramName = "IL_VENDOR";
    protected string sql = "";
    private string date97 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        m_userInfo = userInfoService.GetUserInfo();
        cookiesStorage = new CookiesStorage();
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
        Mode = lblAddEdit.Text;
        if (Page.IsPostBack)
        {
            return;
        }
        if (!IsPostBack)
        {
            InitialCookiesData();

            SetDefault();
            CALL_PROCEDURES_ILSR97();
        }

    }

    #region Functon call procedures ILSR97
    protected void CALL_PROCEDURES_ILSR97()
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);

        ilObj.UserInfomation = m_userInfo;
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

        //ilObj.CloseConnectioDAL();
    }
    #endregion

    #region Set Default
    protected void SetDefault()
    {
        ddlSearchVendorBy.SelectedIndex = 0;
        txtSearchVendorText.Text = "";
        SetAddEditDefatult();
        LoadVendorData();
        //gvVendor.DataSource = Session["ds_vendor"];
        gvVendor.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor.Value);
        gvVendor.DataBind();

    }

    protected void SetAddEditDefatult()
    {
        btnAddVendorHead.Visible = false;
        lblAddEdit.Text = "Add";
        AddVendor.Visible = true;
        btnAddVendorHead.Visible = false;
        btnAdd.Text = "Add";
        rdoVendorType.ClearSelection();
        SetDefaultForm();
        ResetTab();
        SetDefaultTabAddress();
        SetDefaultTabOther();
        SetDefatultPersonToContact();
        SetDefaultTabNote();
    }

    protected void TimerTick(object sender, EventArgs e)
    {
        LoadVendorData();
        Timer1.Enabled = false;
        imgdivLoading.Style.Add("visibility", "hidden");
    }

    protected void SetDefaultForm()
    {
        txtVendorCode.Text = "";
        txtVendorHeadCode.Text = "";
        txtTitleCode.Text = "";
        txtTitleDescription.Text = "";
        txtThaiName.Text = "";
        txtEnglishName.Text = "";
        txtBranchNameEnglish.Text = "";
        txtVendorRank.Text = "";
        txtVendorGrade.Text = "";
    }

    protected void SetDefaultTabAddress()
    {
        TabAddress_RegistrationAddress_SetDefault();
        TabAddress_LocationAddress_SetDefault();
        TabAddress_AddressForTaxInvoice_SetDefault();
    }

    protected void SetDefaultTabOther()
    {
        //Session["ds_reference"] = null;
        ds_reference.Value = string.Empty;
        TabOther_Information_SetDefault();
        TabOther_Payment_SetDefault();
        LoadReferenceData();
    }

    protected void SetDefatultPersonToContact()
    {
        ds_person_to_contact.Value = string.Empty;
        //cookiesStorage.ClearCookies("ds_person_to_contact");
        TabPersonToContact_SetDefault();
    }

    protected void SetDefaultTabNote()
    {
        //Session["ds_note"] = null;
        ds_note.Value = string.Empty;
        TabNote_SetDefault();
    }

    protected void SetMessage()
    {
        PopupMessage.HeaderText = MessageHeaderText;
        lblMessage.Text = Message;
        PopupMessage.ShowOnPageLoad = true;
    }

    protected void SetMessageSuccess()
    {
        PopupMsgSuccess.HeaderText = MessageHeaderText;
        lblMsgSuccess.Text = Message;
        PopupMsgSuccess.ShowOnPageLoad = true;
    }
    #endregion

    #region Function
    protected string VendorCodeFormatter(string vendorCode)
    {
        vendorCode = vendorCode.Substring(0, 2) + "-" + vendorCode.Substring(2, 6) + "-" + vendorCode.Substring(8, 3) + "-" + vendorCode.Substring(11, 1);

        return vendorCode;
    }

    protected string DateFormatter(string date)
    {
        date = Convert.ToInt32(date).ToString("D8");
        date = date.Substring(6, 2) + "/" + date.Substring(4, 2) + "/" + date.Substring(0, 4);

        return date;
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
    public class ValidationDate
    {
        public bool ValidDate { get; set; }
        public DateTime Date { get; set; }
    }

    protected ValidationDate ValidateDate(string date)
    {
        ValidationDate validationDate = new ValidationDate();
        DateTime dateValue;
        bool validDate = DateTime.TryParseExact(date, "ddMMyyyy", _dateThai, DateTimeStyles.None, out dateValue);

        if (validDate)
        {
            validationDate.Date = dateValue;
            validationDate.ValidDate = true;
        }
        else
        {
            validationDate.ValidDate = false;
        }

        return validationDate;
    }

    protected string GetTitleDescription(string titleCode)
    {
        Connect_GeneralAPI conn_general = new Connect_GeneralAPI();
        string titleDescription = "";
        try
        {
            //string sql = "SELECT GNB2TC, GNB2TD FROM GNMB20L4 WHERE GNB2TC = " + titleCode;

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsTitle = new DataSet();
            DataTable dtTitle = new DataTable();
            ILDataCenterMssql iLDataCenterMssql = new ILDataCenterMssql(m_userInfo);
            //dsTitle = ilObj.RetriveAsDataSet(sql);
            DataSet DS = iLDataCenterMssql.getGeneralCenter("OfficeTitleID");
            if (cookiesStorage.check_dataset(DS))
            {
                dtTitle = DS.Tables[0];
            }
            //dtTitle = conn_general.GetGeneralOfficeTitle();

            dtTitle.Columns["DescriptionTHAI"].ColumnName = "GNB2TD";
            dtTitle.Columns["Code"].ColumnName = "GNB2TC";
            DataTable dtTitle2 = dtTitle.Select("GNB2TC = " + titleCode).CopyToDataTable();

            DataSet dsTitle2 = new DataSet();
            dsTitle2.Tables.Add(dtTitle2);
            DataRow drTitle = dsTitle2.Tables[0].Rows[0];
            titleDescription = drTitle["GNB2TD"].ToString().Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

        return titleDescription;
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        //GridView.DataSource = (DataSet)Session[ds];
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();

    }

    private void ResetTab()
    {
        tabDetail.ActiveTabIndex = 0;
        tabAddress.ActiveTabIndex = 0;
        tabOther.ActiveTabIndex = 0;
    }
    #endregion

    #region Search Vendor
    protected void LoadVendorData()
    {

        iDB2Command cmd = new iDB2Command();
        string curentDate = DateTime.Now.ToString("yyyyMMdd", _dateThai);
        string searchBy = ddlSearchVendorBy.SelectedValue;
        string searchText = txtSearchVendorText.Text.Trim().Replace("'", "''");
        string sqlWhere = "";

        //if (!string.IsNullOrEmpty(searchText))
        //{
        //    switch (searchBy)
        //    {
        //        case "VC":
        //            sqlWhere = " AND FORMAT(P10VEN,'000000000000') LIKE '%" + searchText + "%'";
        //            break;
        //        case "VH":
        //            sqlWhere = " AND G11VHO LIKE '" + searchText + "%'";
        //            break;
        //        case "VN":
        //            sqlWhere = $" AND (P10TNM LIKE '%{searchText}%' OR UPPER(P10NAM) LIKE '%{searchText.ToUpper()}%') ";
        //            break;
        //        default: break;
        //    }
        //}
        //string sql = @"SELECT FORMAT(P10VEN,'000000000000') AS P10VEN, G11VHO, P10TNM FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
        //                LEFT JOIN AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK) ON G11VEN = CAST(FORMAT(P10VEN,'000000000000') AS VARCHAR(12)) 
        //                WHERE P10DEL = ''";

        //sql = sql + sqlWhere + " ORDER BY FORMAT(P10VEN,'000000000000') ASC";

        //cmd.CommandText = sql;

        DataSet dsVendor = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        // dsVendor = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        dsVendor = iLDataCenterOnMasterFile.sp_GetVendorMaster(searchBy, searchText, 1, 15);
        if (!cookiesStorage.check_dataset(dsVendor))
        {
            return;
        }
        ds_vendor.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsVendor);
        ResetGrid(gvVendor, ds_vendor.Value);
        DataRow dr = dsVendor.Tables[0].Rows[0];
        //int totalRows = int.Parse(dr["TotalRows"].ToString());
        int totalRows = dsVendor.Tables[0].Rows.Count > int.Parse(dr["TotalRows"].ToString()) ? dsVendor.Tables[0].Rows.Count : int.Parse(dr["TotalRows"].ToString());
        int _CurrentRecStart = gvVendor.PageIndex * gvVendor.PageSize + 1;
        int _CurrentRecEnd = gvVendor.PageIndex * gvVendor.PageSize + gvVendor.PageSize;
        lblTitle.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd < totalRows ? _CurrentRecEnd : totalRows, totalRows);
        dataCenter.CloseConnectSQL();

    }

    protected void gvVendorPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVendor.PageIndex = e.NewPageIndex;
        //gvVendor.DataSource = (DataSet)Session["ds_vendor"];
        gvVendor.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor.Value);
        DataSet ds = (DataSet)gvVendor.DataSource;
        int totalRows = ds.Tables[0].Rows.Count;
        int _CurrentRecStart = gvVendor.PageIndex * gvVendor.PageSize + 1;
        int _CurrentRecEnd = gvVendor.PageIndex * gvVendor.PageSize + gvVendor.PageSize;
        lblTitle.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd < totalRows ? _CurrentRecEnd : totalRows, totalRows);
        gvVendor.DataBind();
    }

    protected void gvVendorSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        string curentDate = DateTime.Now.ToString("yyyyMMdd", _dateThai);
        string searchBy = ddlSearchVendorBy.SelectedValue;
        string searchText = txtSearchVendorText.Text.Trim().Replace("'", "''");

        SetAddEditDefatult();
        //DataSet dsVendor = (DataSet)Session["ds_vendor"];
        DataTable dt = new DataTable();
        DataRow drVendorNew = dt.NewRow();
        DataSet dsVendor = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor.Value);
        if (cookiesStorage.check_dataset(dsVendor))
        {
            drVendorNew = dsVendor.Tables[0]?.Rows[(gvVendor.PageIndex * Convert.ToInt16(gvVendor.PageSize)) + e.NewSelectedIndex];
        }


        string vendorIdSelect = drVendorNew["P10VEN"].ToString().Trim();
        string vendorIdSelectHead = drVendorNew["G11VHO"].ToString().Trim();
        lblAddEdit.Text = "Edit";
        AddVendor.Visible = false;
        btnAdd.Text = "Edit";

        string sql = @"SELECT
                            FORMAT(A.P10VEN,'000000000000') AS P10VEN1,
                            CASE WHEN A.P10PVN = 0 THEN '0' ELSE FORMAT(A.P10PVN,'000000000000') END AS P10PVN1,
                            P10VEN, P10TIC, P10TNM, P10NAM, P10ADR, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO, P10SOI, P10ROD, P10MOO, P10TMC,
				            P10AMC, P10PVC, P10ZIP, P10AD2, P10VI2, P10BI2, P10BU2, P10RM2, P10FL2, P10SO2, P10RD2, P10MO2, P10TM2, P10AM2, 
				            P10PV2, P10ZI2, P10A31, P10A32, P10A33, P10STS, P10GRD, P10MOU, P10REG, P10TAX, P10TE1, P10TLR, P10EXT, P10TE2, 
				            P10TR2, P10EX2, P10FX1, P10F1T, P10FX2, P10F2T, P10RES, P10POT, P10RDP, P10RE2, P10PO2, P10RP2, P10FJD, P10JDT, 
				            P10EDT, P10PVN, P10BPY, P10TXR, P10PYE, P10PTY, P10BCD, P10BNO, P10BRG, P10DLV, P10HED, P10DTX, P10SFG, P10CLD, 
				            P10RF1, P10CRD, P10MKD, P10TAV, P10LTM, P10SAL, P10CTY, P10DTY, P10CPD, P10BRN, P10DT1, P10DT2, P10FI1, P10FIL, 
				            P10UPD, P10TIM, P10USR, P10PGM, P10DSP, P10DEL, P10ATS, P10FIX, P10SPC,
                            I.P16RNK,
                            G.Code as GT08TC,
                            GG.Code AS GT08TC2,
                            CONCAT(G.Code,' : ',G.DescriptionTHAI) GT08ES,
                            CONCAT(GG.Code,' : ',GG.DescriptionTHAI) GT08ES2,
                            B.ID, B.[Type], B.Code as GNB2TC, B.DescriptionTHAI as GNB2TD, B.DescriptionENG as GNB2ED, B.ShortName as GNB2FL, 
                            B.Sorting as Running, B.RecordStatus, B.[Application] as GNB2US, B.CreateBy, B.CreateDate, B.UpdateBy, B.UpdateDate, B.IsDelete,
                            C.DescriptionTHAI as GN20DT,
                            D.DescriptionTHAI as GN19DT,
                            E.DescriptionTHAI as GN18DT,
                            CC.DescriptionTHAI as GN20DT2,
                            DD.DescriptionTHAI as GN19DT2,
                            EE.DescriptionTHAI as GN18DT2,
                            F.T1BRN,
                            F.T1BNME,
                            H.T1BRN OWNBRN,
                            H.T1BNME OWNBRNNAME
                        FROM
                            AS400DB01.ILOD0001.ILMS10 A WITH (NOLOCK)          
                        LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter G WITH (NOLOCK)
                        ON  (P10BIL = G.ID AND G.Type = 'BuildingTitleID')
                        LEFT JOIN GeneralDB01.generalInfo.GeneralCenter B WITH (NOLOCK)
                        ON  (P10TIC = B.ID AND B.Type = 'OfficeTitleID')
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince C WITH (NOLOCK)
                        ON  P10PVC = C.ID
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur D WITH (NOLOCK)
                        ON  P10AMC = D.ID
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol E WITH (NOLOCK)
                        ON  P10TMC = E.ID
                        LEFT JOIN AS400DB01.ILOD0001.ILTB01 F WITH (NOLOCK)
                        ON  P10BPY = F.T1BRN
                        LEFT JOIN AS400DB01.ILOD0001.ILTB01 H WITH (NOLOCK)
                        ON  P10BRN = H.T1BRN
                        LEFT JOIN AS400DB01.ILOD0001.ILMS16 I WITH (NOLOCK)
                        ON  P10VEN = P16VEN
                        AND " + curentDate + @" BETWEEN P16STD AND P16END
                        AND P16STS = ''
                        LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter GG WITH (NOLOCK)
                        ON  (P10BI2 = GG.Code AND GG.Type = 'BuildingTitleID')
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince CC WITH (NOLOCK)
                        ON  P10PV2 = CC.Code
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince DD WITH (NOLOCK)
                        ON  P10AM2 = DD.Code
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol EE WITH (NOLOCK)
                        ON  P10TM2 = EE.Code
                        LEFT JOIN AS400DB01.BLOD0000.BLMS01 FF WITH (NOLOCK)
                        ON  FORMAT(A.P10VEN,'000000000000') = B1IDNO
                        WHERE P10VEN =" + vendorIdSelect + "";

        cmd.CommandText = sql;

        DataSet dsVendorNew = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dsVendorNew = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
        dataCenter = new DataCenter(m_userInfo);
        if (!cookiesStorage.check_dataset(dsVendorNew))
        {
            return;
        }

        DataRow drVendor = dsVendorNew.Tables[0]?.Rows[0];

        string vendorCode = drVendor["P10VEN1"].ToString().Trim();
        vendorCode = VendorCodeFormatter(vendorCode);

        txtVendorCode.Text = vendorCode;
        txtVendorHeadCode.Text = !string.IsNullOrEmpty(vendorIdSelectHead) ? VendorCodeFormatter(vendorIdSelectHead) : "";
        txtTitleCode.Text = drVendor["P10TIC"].ToString().Trim();
        txtTitleDescription.Text = GetTitleDescription(txtTitleCode.Text);
        txtThaiName.Text = drVendor["P10TNM"].ToString().Trim();
        txtEnglishName.Text = drVendor["P10NAM"].ToString().Trim();
        txtBranchNameEnglish.Text = drVendor["P10FI1"].ToString().Trim();
        txtVendorRank.Text = drVendor["P16RNK"].ToString().Trim();
        txtVendorGrade.Text = drVendor["P10GRD"].ToString().Trim();

        TabAddress_RegistrationAddress_BindData(drVendor);
        TabAddress_LocationAddress_BindData(drVendor);
        TabAddress_AddressForTaxInvoice_BindData(drVendor);
        TabOther_Information_BindData(drVendor);
        TabOther_Payment_BindData(drVendor);

        //cookiesStorage.ClearCookies("ds_person_to_contact");
        ds_person_to_contact.Value = string.Empty;
        TabPersonToContact_BindData();

        TabNote_BindData();

        dataCenter.CloseConnectSQL();

    }

    protected void gvVendorRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ResetTab();
        DataSet dsVendor = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor.Value);
        DataTable dt = new DataTable();
        DataRow drVendor = dt.NewRow();
        if (cookiesStorage.check_dataset(dsVendor))
        {
            drVendor = dsVendor.Tables[0]?.Rows[(gvVendor.PageIndex * Convert.ToInt16(gvVendor.PageSize)) + e.RowIndex];
        }
        string vendorCode = drVendor["P10VEN"].ToString();
        hdfVendorCode.Value = vendorCode;

        ValidateDeleteVendor();
    }

    protected void btnSearchClick(object sender, EventArgs e)
    {
        imgdivLoading.Style.Add("visibility", "visible");
        Timer1.Interval = 2000;
        Timer1.Enabled = true;
        //LoadVendorData();
        //imgdivLoading.Style.Add("visibility", "hidden");
    }

    protected void btnClearClick(object sender, EventArgs e)
    {
        SetDefault();
    }
    #endregion

    #region Poup Add Vendor
    protected void LoadVendorHeadData()
    {
        string curentDate = DateTime.Now.ToString("yyyyMMdd", _dateThai);
        iDB2Command cmd = new iDB2Command();
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;
        string searchBy = ddlPoupAddVendorHeadSearchBy.SelectedValue;
        string searchText = txtPopupAddVendorHeadSearchText.Text.Trim().Replace("'", "''");
        string sqlWhere = "";

        if (!string.IsNullOrEmpty(searchText))
        {
            switch (searchBy)
            {
                case "VC":
                    sqlWhere = $" AND G11VHO LIKE '%{searchText}%'";
                    break;
                case "VN":
                    sqlWhere = $" AND P10TNM LIKE '%{searchText}%'";
                    break;
                default: break;
            }
        }
        string sql = @"SELECT DISTINCT G11VHO,P10TIC,P10TNM,P10NAM,P10FI1,P10GRD,P10MOU,T1BRN,T1BNME FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                        INNER JOIN AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK) ON G11VEN = FORMAT(P10VEN,'000000000000') 
                        LEFT JOIN  AS400DB01.ILOD0001.ILTB01 WITH (NOLOCK)  ON  P10BRN = T1BRN                       
                        WHERE P10DEL = '' AND P10HED = 'Y' ";
        sql = sql + sqlWhere;
        cmd.CommandText = sql;
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsVendorHead = new DataSet();
        dsVendorHead = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
        //Session["ds_vendor_head"] = dsVendorHead;
        //cookiesStorage.SetCookiesDataSetByName("ds_vendor_head", dsVendorHead);
        ds_vendor_head.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsVendorHead);
        if (cookiesStorage.check_dataset(dsVendorHead))
        {
            gvPopupAddVendorHead.DataSource = dsVendorHead;
            gvPopupAddVendorHead.DataBind();
        }
    }

    protected void gvPopupAddVendorHeadPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPopupAddVendorHead.PageIndex = e.NewPageIndex;
        gvPopupAddVendorHead.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_head.Value);
        gvPopupAddVendorHead.DataBind();
    }

    protected void gvPopupAddVendorHeadSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string section = hdfVendorSection.Value;
        DataSet dsVendor = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_vendor_head.Value);
        DataTable dt = new DataTable();
        DataRow drVendor = dt.NewRow();
        if (cookiesStorage.check_dataset(dsVendor))
        {
            drVendor = dsVendor.Tables[0]?.Rows[(gvPopupAddVendorHead.PageIndex * Convert.ToInt16(gvPopupAddVendorHead.PageSize)) + e.NewSelectedIndex];
        }
        if (section == "MAIN")
        {
            string vendorCode = VendorCodeFormatter(drVendor["G11VHO"].ToString().Trim());
            txtVendorHeadCode.Text = vendorCode;
            txtTabOther_Information_VendorHead.Text = vendorCode;
            txtTitleCode.Text = drVendor["P10TIC"].ToString().Trim();
            txtTitleDescription.Text = GetTitleDescription(txtTitleCode.Text);
            txtThaiName.Text = drVendor["P10TNM"].ToString().Trim();
            txtEnglishName.Text = drVendor["P10NAM"].ToString().Trim();
            txtBranchNameEnglish.Text = drVendor["P10FI1"].ToString().Trim();
            //txtVendorRank.Text = drVendor["P16RNK"].ToString().Trim();
            txtVendorGrade.Text = drVendor["P10GRD"].ToString().Trim();

            string MOUDate = drVendor["P10MOU"].ToString().Trim();

            txtTabOther_Information_MOUDate.Text = DateFormatter(MOUDate);
        }

        if (section == "PAYTOVENDOR")
        {
            string vendorCode = VendorCodeFormatter(drVendor["G11VHO"].ToString().Trim());
            //string vendorCode = VendorCodeFormatter(drVendor["P10VEN"].ToString().Trim());
            txtTabOther_Payment_PayToVendor.Text = vendorCode;
            txtTabOther_Payment_BranchCode.Text = drVendor["T1BRN"].ToString().Trim();
            txtTabOther_Payment_BranchPayment.Text = drVendor["T1BNME"].ToString().Trim();
        }

        PopupAddVendorHead.ShowOnPageLoad = false;
    }

    protected void btnAddVendorHeadClick(object sender, ImageClickEventArgs e)
    {
        hdfVendorSection.Value = "MAIN";
        PopupAddVendorHead.ShowOnPageLoad = true;
        LoadVendorHeadData();
    }

    protected void btnPopupAddVendorHeadSearchClick(object sender, EventArgs e)
    {
        LoadVendorHeadData();
    }

    protected void btnPopupAddVendorHeadClearClick(object sender, EventArgs e)
    {
        ddlPoupAddVendorHeadSearchBy.SelectedIndex = 0;
        txtPopupAddVendorHeadSearchText.Text = "";
        LoadVendorHeadData();
    }
    #endregion

    #region Popup Add Title Code
    protected void btnAddTitleClick(object sender, ImageClickEventArgs e)
    {
        PopupAddTitle.ShowOnPageLoad = true;
        LoadTitleData();
    }

    protected void LoadTitleData()
    {
        DataSet dsTitle = new DataSet();
        try
        {
            string searchBy = ddlPopupAddTitleSearchBy.SelectedValue;
            string searchText = txtPopupAddTitleSearchText.Text.Trim().Replace("'", "''");
            string sqlWhere = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "TC":
                        sqlWhere = " AND Code LIKE '" + searchText + "%'";
                        break;
                    case "TD":
                        sqlWhere = " AND UPPER(DescriptionTHAI) LIKE '" + searchText.ToUpper() + "%'";
                        break;
                    default: break;
                }
            }

            string sql = "SELECT Code as GNB2TC, DescriptionTHAI as GNB2TD FROM GeneralDB01.generalInfo.generalcenter WITH (NOLOCK) where type in('OfficeTitleID') " + sqlWhere + " ORDER BY Code ASC";

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsTitle = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            hiddenTitle.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsTitle);
        }
        catch (Exception ex)
        {

        }
        if (cookiesStorage.check_dataset(dsTitle))
        {
            gvPopupAddTitle.DataSource = dsTitle;
            gvPopupAddTitle.DataBind();
        }
    }

    protected void gvPopupAddTitlePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPopupAddTitle.PageIndex = e.NewPageIndex;
        gvPopupAddTitle.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(hiddenTitle.Value);
        gvPopupAddTitle.DataBind();
    }

    protected void gvPopupAddTitleSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsTitle = cookiesStorage.JsonDeserializeObjectHiddenDataSet(hiddenTitle.Value);
        DataTable dt = new DataTable();
        DataRow drTitle = dt.NewRow();
        if (cookiesStorage.check_dataset(dsTitle))
        {
            drTitle = dsTitle.Tables[0]?.Rows[(gvPopupAddTitle.PageIndex * Convert.ToInt16(gvPopupAddTitle.PageSize)) + e.NewSelectedIndex];
        }
        txtTitleCode.Text = drTitle["GNB2TC"].ToString();
        txtTitleDescription.Text = drTitle["GNB2TD"].ToString();

        PopupAddTitle.ShowOnPageLoad = false;
    }

    protected void btnPopupAddTitleSearchClick(object sender, EventArgs e)
    {
        LoadTitleData();
    }

    protected void btnPopupAddTitleClearClick(object sender, EventArgs e)
    {
        ddlPopupAddTitleSearchBy.SelectedIndex = 0;
        txtPopupAddTitleSearchText.Text = "";
        LoadTitleData();
    }
    #endregion

    #region DropDownList Building Type
    protected void LoadBuligTypeData(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        string sql = "SELECT Code as GT08TC, DescriptionTHAI as GT08TD FROM GeneralDB01.generalInfo.generalcenter WITH (NOLOCK) where type in('BuildingTitleID') ";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsBuildingType = new DataSet();
        dsBuildingType = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsBuildingType))
        {
            dropDownList.DataValueField = "GT08TC";
            dropDownList.DataTextField = "GT08TD";
            dropDownList.DataSource = dsBuildingType;
            dropDownList.DataBind();
        }

        dropDownList.Items.Insert(0, new ListItem("Select", ""));
    }
    #endregion

    #region DropDownList Province
    protected void LoadProvinceData(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        dropDownList.Items.Clear();

        string sql = "SELECT Code as GN20CD, DescriptionTHAI as GN20DT FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE IsDelete = ''";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsProvince = new DataSet();
        dsProvince = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProvince))
        {
            dropDownList.DataValueField = "GN20CD";
            dropDownList.DataTextField = "GN20DT";
            dropDownList.DataSource = dsProvince;
            dropDownList.DataBind();
        }

        dropDownList.Items.Insert(0, new ListItem("Select", ""));
    }
    #endregion

    #region DropDownList Amphur
    protected void LoadAmphurData(System.Web.UI.WebControls.DropDownList dropDownList, string provinceCode)
    {
        dropDownList.Items.Clear();


        string sql = @"SELECT DISTINCT aa.Code as GN21AM, aa.DescriptionTHAI as GN19DT  
                        FROM GeneralDB01.GeneralInfo.AddrRelation ar WITH (NOLOCK)
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur aa WITH (NOLOCK)  ON  AumphurID = aa.ID
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince ap WITH (NOLOCK)  ON ProvinceID = ap.ID
                        WHERE ar.IsDelete = ''
                        AND ap.Code = '" + provinceCode + "' ORDER BY aa.DescriptionTHAI ASC";



        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsAmphur = new DataSet();
        dsAmphur = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (ilObj.check_dataset(dsAmphur))
        {
            dropDownList.DataValueField = "GN21AM";
            dropDownList.DataTextField = "GN19DT";
            dropDownList.DataSource = dsAmphur;
            dropDownList.DataBind();
        }


        dropDownList.Items.Insert(0, new ListItem("Select", ""));
    }
    #endregion

    #region DropDownList District
    protected void LoadDistrictData(System.Web.UI.WebControls.DropDownList dropDownList, string amphurCode)
    {
        dropDownList.Items.Clear();
        string sql = @"SELECT
                        GNTB18.Code as GN21TM,
                        GNTB18.DescriptionTHAI as GN18DT,
                        GNTB18.DescriptionENG as GN18DE,
                        GNTB21.ZipCode as GN21ZP,
                        GNTB19.Code as GN21AM,
                        GNTB19.DescriptionTHAI as GN19DT,
                        GNTB19.DescriptionENG as GN19DE,
                        GNTB20.Code as GN21PR,
                        GNTB20.DescriptionTHAI as GN20DT,
                        GNTB20.DescriptionENG as GN20DE,
                        GNTB21.Remark as GN21RE
                    FROM
                        GeneralDB01.GeneralInfo.AddrRelation GNTB21 WITH (NOLOCK)
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol GNTB18 WITH (NOLOCK)
                    ON  GNTB21.TambolID =  GNTB18.ID
                    AND GNTB18.IsDelete = ''
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur GNTB19 WITH (NOLOCK)
                    ON  GNTB21.AumphurID = GNTB19.ID
                    AND GNTB19.IsDelete = ''
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince GNTB20 WITH (NOLOCK)
                    ON  GNTB21.ProvinceID = GNTB20.ID
                    AND GNTB20.IsDelete = ''
                    WHERE
                        GNTB21.IsDelete = ''
                    AND	GNTB19.Code = '" + amphurCode + "'";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsDistrict = new DataSet();
        dsDistrict = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (ilObj.check_dataset(dsDistrict))
        {
            dropDownList.DataValueField = "GN21TM";
            dropDownList.DataTextField = "GN18DT";
            dropDownList.DataSource = dsDistrict;
            dropDownList.DataBind();
        }


        dropDownList.Items.Insert(0, new ListItem("Select", ""));
    }

    protected string GetPostCode(string provinceCode, string amphurCode, string districtCode)
    {
        string postCode = "";
        try
        {
            string sql = @"SELECT
	                            GNTB21.ZipCode as GN21ZP
                            FROM
	                            GeneralDB01.GeneralInfo.AddrRelation GNTB21 WITH (NOLOCK)
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol GNTB18 WITH (NOLOCK)
                                ON  GNTB21.TambolID =  GNTB18.ID
                                AND GNTB18.IsDelete = ''
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur GNTB19 WITH (NOLOCK)
                                ON  GNTB21.AumphurID = GNTB19.ID
                                AND GNTB19.IsDelete = ''
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince GNTB20 WITH (NOLOCK)
                                ON  GNTB21.ProvinceID = GNTB20.ID
                                AND GNTB20.IsDelete = ''
                            WHERE
	                            GNTB20.Code = '" + provinceCode + @"' 
                            AND GNTB19.Code = '" + amphurCode + @"' 
                            AND GNTB18.Code = '" + districtCode + "'";

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsPostCode = new DataSet();
            dsPostCode = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            DataTable dt = new DataTable();
            DataRow drPostCode = dt.NewRow();
            if (cookiesStorage.check_dataset(dsPostCode))
            {
                drPostCode = dsPostCode.Tables[0]?.Rows[0];
                postCode = drPostCode["GN21ZP"].ToString();
            }
        }
        catch (Exception ex)
        {

        }

        return postCode;
    }




    #endregion

    #region Tab Address : Registration Address
    protected void TabAddress_RegistrationAddress_SetDefault()
    {
        txtTabAddress_RegistrationAddress_Address.Text = "";
        txtTabAddress_RegistrationAddress_Moo.Text = "";
        txtTabAddress_RegistrationAddress_Village.Text = "";

        ddlTabAddress_LocationAddress_BuildingType.Items.Clear();
        LoadBuligTypeData(ddlTabAddress_RegistrationAddress_BuildingType);

        txtTabAddress_RegistrationAddress_Building.Text = "";
        txtTabAddress_RegistrationAddress_Room.Text = "";
        txtTabAddress_RegistrationAddress_Floor.Text = "";
        txtTabAddress_RegistrationAddress_Soi.Text = "";
        txtTabAddress_RegistrationAddress_Road.Text = "";

        ddlTabAddress_RegistrationAddress_Province.Items.Clear();
        LoadProvinceData(ddlTabAddress_RegistrationAddress_Province);
        ddlTabAddress_RegistrationAddress_Amphur.Items.Clear();
        ddlTabAddress_RegistrationAddress_District.Items.Clear();
        txtTabAddress_RegistrationAddress_PostCode.Text = "";
    }

    protected void TabAddress_RegistrationAddress_BindData(DataRow drVendor)
    {
        string addressRegistrationProvinceCode = drVendor["P10PVC"].ToString().Trim();
        string addressRegistrationAmphurCode = drVendor["P10AMC"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Address.Text = drVendor["P10ADR"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Moo.Text = drVendor["P10MOO"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Village.Text = drVendor["P10VIL"].ToString().Trim();
        ddlTabAddress_RegistrationAddress_BuildingType.SelectedValue = drVendor["GT08TC"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Building.Text = drVendor["P10BUD"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Room.Text = drVendor["P10ROM"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Floor.Text = drVendor["P10FLO"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Soi.Text = drVendor["P10SOI"].ToString().Trim();
        txtTabAddress_RegistrationAddress_Road.Text = drVendor["P10ROD"].ToString().Trim();
        ddlTabAddress_RegistrationAddress_Province.SelectedValue = addressRegistrationProvinceCode;
        //LoadAmphurData(ddlTabAddress_RegistrationAddress_Amphur, addressRegistrationProvinceCode);
        LoadAmphurDataToCookies(ddlTabAddress_RegistrationAddress_Amphur, addressRegistrationProvinceCode);
        ddlTabAddress_RegistrationAddress_Amphur.SelectedValue = addressRegistrationAmphurCode;
        //LoadDistrictData(ddlTabAddress_RegistrationAddress_District, addressRegistrationAmphurCode);
        LoadDistrictDataToCookies(ddlTabAddress_RegistrationAddress_District, addressRegistrationAmphurCode);
        ddlTabAddress_RegistrationAddress_District.SelectedValue = drVendor["P10TMC"].ToString().Trim();
        txtTabAddress_RegistrationAddress_PostCode.Text = drVendor["P10ZIP"].ToString().Trim();
    }

    protected void ddlTabAddress_RegistrationAddress_Province_SelectedIndexChanged(object sender, EventArgs e)
    {
        string provinceCode = ddlTabAddress_RegistrationAddress_Province.SelectedValue;
        //LoadAmphurData(ddlTabAddress_RegistrationAddress_Amphur, provinceCode);
        LoadAmphurDataToCookies(ddlTabAddress_RegistrationAddress_Amphur, provinceCode);
        ddlTabAddress_RegistrationAddress_District.Items.Clear();
        txtTabAddress_RegistrationAddress_PostCode.Text = "";
    }

    protected void ddlTabAddress_RegistrationAddress_Amphur_SelectedIndexChanged(object sender, EventArgs e)
    {
        string amphurCode = ddlTabAddress_RegistrationAddress_Amphur.SelectedValue;
        //LoadDistrictData(ddlTabAddress_RegistrationAddress_District, amphurCode);
        LoadDistrictDataToCookies(ddlTabAddress_RegistrationAddress_District, amphurCode);
        txtTabAddress_RegistrationAddress_PostCode.Text = "";
    }

    protected void ddlTabAddress_RegistrationAddress_District_SelectedIndexChanged(object sender, EventArgs e)
    {
        string provinceCode = ddlTabAddress_RegistrationAddress_Province.SelectedValue.ToString();
        string amphurCode = ddlTabAddress_RegistrationAddress_Amphur.SelectedValue.ToString();
        string districtCode = ddlTabAddress_RegistrationAddress_District.SelectedValue.ToString();
        string postCode = GetPostCode(provinceCode, amphurCode, districtCode);
        //string postCode = GetPostCodeToCookies(provinceCode, amphurCode, districtCode);
        txtTabAddress_RegistrationAddress_PostCode.Text = postCode;
    }

    protected void btnTabAddress_RegistrationAddress_LoadRegistrationAddressFromVendorHead_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorHeadCode.Text.Trim().Replace("-", "");
        if (!string.IsNullOrEmpty(vendorCode))
        {
            string sql = @"SELECT
                                P10ADR, P10MOO, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO,
                                P10SOI, P10ROD, P10PVC, P10AMC, P10TMC
                            FROM
                                AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                            LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince GNTB20 WITH (NOLOCK)
                            ON  P10PVC = GNTB20.Code
                            LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur GNTB19 WITH (NOLOCK)
                            ON  P10AMC = GNTB19.Code
                            LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol GNTB18 WITH (NOLOCK)
                            ON  P10TMC = GNTB18.Code
                            WHERE
                                P10VEN = " + vendorCode;

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsVendor = new DataSet();
            dsVendor = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            DataTable dt = new DataTable();
            DataRow drVendor = dt.NewRow();
            if (cookiesStorage.check_dataset(dsVendor))
            {
                drVendor = dsVendor.Tables[0]?.Rows[0];
            }

            txtTabAddress_RegistrationAddress_Address.Text = drVendor["P10ADR"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Moo.Text = drVendor["P10MOO"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Village.Text = drVendor["P10VIL"].ToString().Trim();

            string buildingType = drVendor["P10BIL"].ToString().Trim();
            if (string.IsNullOrEmpty(buildingType))
            {
                ddlTabAddress_RegistrationAddress_BuildingType.SelectedIndex = 0;
            }
            else
            {
                ddlTabAddress_RegistrationAddress_BuildingType.SelectedValue = buildingType;
            }

            txtTabAddress_RegistrationAddress_Building.Text = drVendor["P10BUD"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Room.Text = drVendor["P10ROM"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Floor.Text = drVendor["P10FLO"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Soi.Text = drVendor["P10SOI"].ToString().Trim();
            txtTabAddress_RegistrationAddress_Road.Text = drVendor["P10ROD"].ToString().Trim();

            string provinceCode = drVendor["P10PVC"].ToString().Trim();
            string amphurCode = drVendor["P10AMC"].ToString().Trim();
            string districtCode = drVendor["P10TMC"].ToString().Trim();

            if (!string.IsNullOrEmpty(provinceCode))
            {
                ddlTabAddress_RegistrationAddress_Province.SelectedValue = provinceCode;
                //LoadAmphurData(ddlTabAddress_RegistrationAddress_Amphur, provinceCode);
                LoadAmphurDataToCookies(ddlTabAddress_RegistrationAddress_Amphur, provinceCode);

                if (!string.IsNullOrEmpty(amphurCode))
                {
                    ddlTabAddress_RegistrationAddress_Amphur.SelectedValue = amphurCode;
                    LoadDistrictDataToCookies(ddlTabAddress_RegistrationAddress_District, amphurCode);

                    if (!string.IsNullOrEmpty(districtCode))
                    {
                        ddlTabAddress_RegistrationAddress_District.SelectedValue = districtCode;
                        txtTabAddress_RegistrationAddress_PostCode.Text = GetPostCode(provinceCode, amphurCode, districtCode);
                        //txtTabAddress_RegistrationAddress_PostCode.Text = GetPostCodeToCookies(provinceCode, amphurCode, districtCode);
                    }
                    else
                    {
                        ddlTabAddress_RegistrationAddress_District.SelectedIndex = 0;
                        txtTabAddress_RegistrationAddress_PostCode.Text = "";
                    }
                }
                else
                {
                    ddlTabAddress_RegistrationAddress_Amphur.SelectedIndex = 0;
                    ddlTabAddress_RegistrationAddress_District.Items.Clear();
                    txtTabAddress_RegistrationAddress_PostCode.Text = "";
                }
            }
            else
            {
                ddlTabAddress_RegistrationAddress_Province.SelectedIndex = 0;
                ddlTabAddress_RegistrationAddress_Amphur.Items.Clear();
                ddlTabAddress_RegistrationAddress_District.Items.Clear();
                txtTabAddress_RegistrationAddress_PostCode.Text = "";
            }
        }
        else
        {

        }
    }
    #endregion

    #region Tab Address : Location Address
    protected void TabAddress_LocationAddress_SetDefault()
    {
        txtTabAddress_LocationAddress_Address.Text = "";
        txtTabAddress_LocationAddress_Moo.Text = "";
        txtTabAddress_LocationAddress_Village.Text = "";

        ddlTabAddress_LocationAddress_BuildingType.Items.Clear();
        LoadBuligTypeData(ddlTabAddress_LocationAddress_BuildingType);

        txtTabAddress_LocationAddress_Building.Text = "";
        txtTabAddress_LocationAddress_Room.Text = "";
        txtTabAddress_LocationAddress_Floor.Text = "";
        txtTabAddress_LocationAddress_Soi.Text = "";
        txtTabAddress_LocationAddress_Road.Text = "";

        ddlTabAddress_LocationAddress_Province.Items.Clear();
        LoadProvinceData(ddlTabAddress_LocationAddress_Province);
        ddlTabAddress_LocationAddress_Amphur.Items.Clear();
        ddlTabAddress_LocationAddress_District.Items.Clear();
        txtTabAddress_LocationAddress_PostCode.Text = "";
    }

    protected void TabAddress_LocationAddress_BindData(DataRow drVendor)
    {
        string locationAddressProvinceCode = drVendor["P10PV2"].ToString().Trim();
        string locationAddressAmphurCode = drVendor["P10AM2"].ToString().Trim();
        txtTabAddress_LocationAddress_Address.Text = drVendor["P10AD2"].ToString().Trim();
        txtTabAddress_LocationAddress_Moo.Text = drVendor["P10MO2"].ToString().Trim();
        txtTabAddress_LocationAddress_Village.Text = drVendor["P10VI2"].ToString().Trim();
        ddlTabAddress_LocationAddress_BuildingType.SelectedValue = drVendor["GT08TC2"].ToString().Trim();
        txtTabAddress_LocationAddress_Building.Text = drVendor["P10BU2"].ToString().Trim();
        txtTabAddress_LocationAddress_Room.Text = drVendor["P10RM2"].ToString().Trim();
        txtTabAddress_LocationAddress_Floor.Text = drVendor["P10FL2"].ToString().Trim();
        txtTabAddress_LocationAddress_Soi.Text = drVendor["P10SO2"].ToString().Trim();
        txtTabAddress_LocationAddress_Road.Text = drVendor["P10RD2"].ToString().Trim();
        ddlTabAddress_LocationAddress_Province.SelectedValue = locationAddressProvinceCode;
        //LoadAmphurData(ddlTabAddress_LocationAddress_Amphur, locationAddressProvinceCode);
        LoadAmphurDataToCookies(ddlTabAddress_LocationAddress_Amphur, locationAddressProvinceCode);

        ddlTabAddress_LocationAddress_Amphur.SelectedValue = locationAddressAmphurCode;
        //LoadDistrictData(ddlTabAddress_LocationAddress_District, locationAddressAmphurCode);
        LoadDistrictDataToCookies(ddlTabAddress_LocationAddress_District, locationAddressAmphurCode);

        if (ddlTabAddress_LocationAddress_District.Items.FindByValue(drVendor["P10TM2"].ToString().Trim()) != null)
        {
            ddlTabAddress_LocationAddress_District.SelectedValue = drVendor["P10TM2"].ToString().Trim();
        }
        else
        {
            ddlTabAddress_LocationAddress_District.SelectedIndex = 0;
        }

        txtTabAddress_LocationAddress_PostCode.Text = drVendor["P10ZI2"].ToString().Trim();
    }

    protected void ddlTabAddress_LocationAddress_Province_SelectedIndexChanged(object sender, EventArgs e)
    {
        string provinceCode = ddlTabAddress_LocationAddress_Province.SelectedValue;
        //LoadAmphurData(ddlTabAddress_LocationAddress_Amphur, provinceCode);
        LoadAmphurDataToCookies(ddlTabAddress_LocationAddress_Amphur, provinceCode);

        //locationAddressProvinceCode(ddlTabAddress_LocationAddress_Amphur, provinceCode);
        ddlTabAddress_LocationAddress_District.Items.Clear();
        txtTabAddress_LocationAddress_PostCode.Text = "";
    }

    protected void ddlTabAddress_LocationAddress_Amphur_SelectedIndexChanged(object sender, EventArgs e)
    {
        string amphurCode = ddlTabAddress_LocationAddress_Amphur.SelectedValue;
        //LoadDistrictData(ddlTabAddress_LocationAddress_District, amphurCode);
        LoadDistrictDataToCookies(ddlTabAddress_LocationAddress_District, amphurCode);
        txtTabAddress_LocationAddress_PostCode.Text = "";
    }

    protected void ddlTabAddress_LocationAddress_District_SelectedIndexChanged(object sender, EventArgs e)
    {
        string provinceCode = ddlTabAddress_LocationAddress_Province.SelectedValue.ToString();
        string amphurCode = ddlTabAddress_LocationAddress_Amphur.SelectedValue.ToString();
        string districtCode = ddlTabAddress_LocationAddress_District.SelectedValue.ToString();
        string postCode = GetPostCode(provinceCode, amphurCode, districtCode);
        //string postCode = GetPostCodeToCookies(provinceCode, amphurCode, districtCode);
        txtTabAddress_LocationAddress_PostCode.Text = postCode;
    }

    protected void btnTabAddress_LoacationAddress_LoadNewDataFromRegistrationAddress_Click(object sender, EventArgs e)
    {
        txtTabAddress_LocationAddress_Address.Text = txtTabAddress_RegistrationAddress_Address.Text.Trim();
        txtTabAddress_LocationAddress_Moo.Text = txtTabAddress_RegistrationAddress_Moo.Text.Trim();
        txtTabAddress_LocationAddress_Village.Text = txtTabAddress_RegistrationAddress_Village.Text.Trim();
        ddlTabAddress_LocationAddress_BuildingType.SelectedValue = ddlTabAddress_RegistrationAddress_BuildingType.SelectedValue;
        txtTabAddress_LocationAddress_Building.Text = txtTabAddress_RegistrationAddress_Building.Text.Trim();
        txtTabAddress_LocationAddress_Room.Text = txtTabAddress_RegistrationAddress_Room.Text.Trim();
        txtTabAddress_LocationAddress_Floor.Text = txtTabAddress_RegistrationAddress_Floor.Text.Trim();
        txtTabAddress_LocationAddress_Soi.Text = txtTabAddress_RegistrationAddress_Soi.Text.Trim();
        txtTabAddress_LocationAddress_Road.Text = txtTabAddress_RegistrationAddress_Road.Text.Trim();

        string provinceCode = ddlTabAddress_RegistrationAddress_Province.SelectedValue;
        string amphurCode = ddlTabAddress_RegistrationAddress_Amphur.SelectedValue;
        string districtCode = ddlTabAddress_RegistrationAddress_District.SelectedValue;

        ddlTabAddress_LocationAddress_Province.SelectedValue = provinceCode;
        //LoadAmphurData(ddlTabAddress_LocationAddress_Amphur, provinceCode);
        LoadAmphurDataToCookies(ddlTabAddress_LocationAddress_Amphur, provinceCode);

        ddlTabAddress_LocationAddress_Amphur.SelectedValue = amphurCode;
        //LoadDistrictData(ddlTabAddress_LocationAddress_District, amphurCode);
        LoadDistrictDataToCookies(ddlTabAddress_LocationAddress_District, amphurCode);
        ddlTabAddress_LocationAddress_District.SelectedValue = districtCode;
        txtTabAddress_LocationAddress_PostCode.Text = txtTabAddress_RegistrationAddress_PostCode.Text.Trim();
    }
    #endregion

    #region Tab Address : Address For Tax Invoice
    protected void TabAddress_AddressForTaxInvoice_SetDefault()
    {
        txtTabAddress_AddressForTaxInvoice_Address1.Text = "";
        txtTabAddress_AddressForTaxInvoice_Address2.Text = "";
        txtTabAddress_AddressForTaxInvoice_Address3.Text = "";
    }

    protected void TabAddress_AddressForTaxInvoice_BindData(DataRow drVendor)
    {
        txtTabAddress_AddressForTaxInvoice_Address1.Text = drVendor["P10A31"].ToString().Trim();
        txtTabAddress_AddressForTaxInvoice_Address2.Text = drVendor["P10A32"].ToString().Trim();
        txtTabAddress_AddressForTaxInvoice_Address3.Text = drVendor["P10A33"].ToString().Trim();
    }

    protected void btnTabAddress_AddressForTaxInvoice_LoadNewDataFromRegistrationAddress_Click(object sender, EventArgs e)
    {
        string address = txtTabAddress_RegistrationAddress_Address.Text.Trim();
        string moo = txtTabAddress_RegistrationAddress_Moo.Text.Trim();
        string village = txtTabAddress_RegistrationAddress_Village.Text.Trim();
        string buildingType = string.Empty;
        if (ddlTabAddress_RegistrationAddress_BuildingType.SelectedIndex != 0)
        {
            buildingType = ddlTabAddress_RegistrationAddress_BuildingType.SelectedItem.Text.Trim();
        }
        string building = txtTabAddress_RegistrationAddress_Building.Text.Trim();
        string room = txtTabAddress_RegistrationAddress_Room.Text.Trim();
        string floor = txtTabAddress_RegistrationAddress_Floor.Text.Trim();

        string district = "";
        string amphur = "";
        string province = "";

        if (ddlTabAddress_RegistrationAddress_District.SelectedValue != "")
        {
            district = ddlTabAddress_RegistrationAddress_District.SelectedItem.Text.ToString().Trim();
        }

        if (ddlTabAddress_RegistrationAddress_Amphur.SelectedValue != "")
        {
            amphur = ddlTabAddress_RegistrationAddress_Amphur.SelectedItem.Text.ToString().Trim();
        }

        if (ddlTabAddress_RegistrationAddress_Province.SelectedValue != "")
        {
            province = ddlTabAddress_RegistrationAddress_Province.SelectedItem.Text.ToString().Trim();
        }

        string postCode = txtTabAddress_RegistrationAddress_PostCode.Text.Trim();
        string temp = "";

        if (!string.IsNullOrEmpty(address))
        {
            temp = temp + "เลขที่ " + address + " ;";
        }

        if (!string.IsNullOrEmpty(moo))
        {
            temp = temp + "หมู่ที่ " + moo + " ;";
        }

        if (!string.IsNullOrEmpty(buildingType))
        {
            temp = temp + buildingType + building + " ;";
        }

        if (!string.IsNullOrEmpty(room))
        {
            temp = temp + "ห้อง " + room + " ;";
        }

        if (!string.IsNullOrEmpty(floor))
        {
            temp = temp + "ชั้น " + floor + " ;";
        }

        if (!string.IsNullOrEmpty(district))
        {
            temp = temp + "ตำบล " + district + " ;";
        }

        if (!string.IsNullOrEmpty(amphur))
        {
            temp = temp + "อำเภอ " + amphur + " ;";
        }

        if (!string.IsNullOrEmpty(province))
        {
            temp = temp + "จังหวัด " + province + " ;";
        }

        if (!string.IsNullOrEmpty(postCode))
        {
            temp = temp + postCode + " ;";

            if (postCode.Substring(0, 2) == "10")
            {
                temp = temp.Replace("ตำบล", "แขวง");
                temp = temp.Replace("อำเภอ", "เขต");
                temp = temp.Replace("จังหวัด", "");
            }
        }

        string line1 = "";
        string line2 = "";
        string line3 = "";

        string tempString = "";
        while (temp.IndexOf(";") > -1)
        {
            int p = temp.IndexOf(";");
            tempString = temp.Substring(0, p);

            if ((line1 + tempString).Length < 36 && line1.IndexOf("!") == -1)
            {
                line1 = line1 + tempString;
            }
            else
            {
                if (line1.IndexOf("!") == -1)
                {
                    line1 = "!" + line1;
                }

                if ((line2 + tempString).Length < 36 && line2.IndexOf("!") == -1)
                {
                    line2 = line2 + tempString;
                }
                else
                {
                    if (line2.IndexOf("!") == -1)
                    {
                        line2 = "!" + line2;
                    }

                    if ((line3 + tempString).Length < 36)
                    {
                        line3 = line3 + tempString;
                    }
                    else
                    {
                        int pos = 35 - line3.Length;
                        line3 = line3 + tempString.Substring(0, pos);
                    }
                }
            }
            string removeString = temp.Substring(0, temp.IndexOf(";") + 1);
            temp = temp.Replace(removeString, "");
        }

        txtTabAddress_AddressForTaxInvoice_Address1.Text = line1.Replace("!", "");
        txtTabAddress_AddressForTaxInvoice_Address2.Text = line2.Replace("!", "");
        txtTabAddress_AddressForTaxInvoice_Address3.Text = line3.Replace("!", "");
    }
    #endregion

    #region Tab Other : Information
    protected void TabOther_Information_SetDefault()
    {
        string mouDate = DateTime.Now.ToString("dd/MM/yyyy", _dateThai);
        string joinDate = DateTime.Now.ToString("dd/MM/yyyy", _dateThai);
        string expireDate = "99/99/9999";
        string firstDate = joinDate;

        txtTabOther_Information_MOUDate.Text = mouDate;
        txtTabOther_Information_RegisterNoTaxID.Text = "";
        txtTabOther_Information_TaxID.Text = "";

        txtTabOther_Information_JoinDate.Text = joinDate;
        txtTabOther_Information_ExpireDate.Text = expireDate;
        txtTabOther_Information_FirstOpenDate.Text = firstDate;

        /*** Telephone-Fax ***/
        txtTabOther_Information_TelephoneNo1.Text = "";
        txtTabOther_Information_ContactTelRange1.Text = "";
        txtTabOther_Information_Extension1.Text = "";
        ddlTabOther_Information_FaxType1.SelectedIndex = 0;
        txtTabOther_Information_FaxNo1.Text = "";

        txtTabOther_Information_TelephoneNo2.Text = "";
        txtTabOther_Information_ContactTelRange2.Text = "";
        txtTabOther_Information_Extension2.Text = "";
        ddlTabOther_Information_FaxType2.SelectedIndex = 0;
        txtTabOther_Information_FaxNo2.Text = "";

        /*** For Fax Outbound ***/
        txtTabOther_Information_VendorHead.Text = "";
        ddlTabOther_Information_StatusFaxForOper.SelectedIndex = 0;
        ddlTabOther_Information_StatusFaxForISB.SelectedIndex = 0;
        ddlTabOther_Information_FaxForHOForOper.SelectedIndex = 0;
        ddlTabOther_Information_FaxForHOForISB.SelectedIndex = 0;
        ddlTabOther_Information_AutoFax.SelectedIndex = 0;
        ddlTabOther_Information_AutoSignLayBill.SelectedIndex = 0;

        TabOther_Information_Reference_SetDefault();
    }

    protected void TabOther_Information_BindData(DataRow drVendor)
    {
        string MOUDate = drVendor["P10MOU"].ToString().Trim();
        MOUDate = DateFormatter(MOUDate);
        txtTabOther_Information_MOUDate.Text = MOUDate;
        txtTabOther_Information_RegisterNoTaxID.Text = drVendor["P10REG"].ToString().Trim();
        txtTabOther_Information_TaxID.Text = drVendor["P10TAX"].ToString().Trim();

        string joinDate = drVendor["P10JDT"].ToString().Trim();
        if (Convert.ToInt32(joinDate) == 0)
        {
            joinDate = DateTime.Now.ToString("dd/MM/yyyy", _dateThai);
        }
        else
        {
            joinDate = DateFormatter(joinDate);
        }
        txtTabOther_Information_JoinDate.Text = joinDate;

        string expireDate = drVendor["P10EDT"].ToString().Trim();
        expireDate = DateFormatter(expireDate);
        txtTabOther_Information_ExpireDate.Text = expireDate;

        txtTabOther_Information_FirstOpenDate.Text = joinDate;

        txtTabOther_Information_TelephoneNo1.Text = drVendor["P10TE1"].ToString().Trim();
        txtTabOther_Information_ContactTelRange1.Text = drVendor["P10TLR"].ToString().Trim();
        txtTabOther_Information_Extension1.Text = drVendor["P10EXT"].ToString().Trim();
        ddlTabOther_Information_FaxType1.SelectedValue = drVendor["P10F1T"].ToString().Trim();
        string faxNo1 = drVendor["P10FX1"].ToString().Trim();
        if (faxNo1.Length > 3)
        {
            faxNo1 = "0" + faxNo1;
        }
        txtTabOther_Information_FaxNo1.Text = faxNo1;

        txtTabOther_Information_TelephoneNo2.Text = drVendor["P10TE2"].ToString().Trim();
        txtTabOther_Information_ContactTelRange2.Text = drVendor["P10TR2"].ToString().Trim();
        txtTabOther_Information_Extension2.Text = drVendor["P10EX2"].ToString().Trim();
        ddlTabOther_Information_FaxType2.SelectedValue = drVendor["P10F2T"].ToString().Trim();
        string faxNo2 = drVendor["P10FX2"].ToString().Trim();
        if (faxNo2.Length > 3)
        {
            faxNo2 = "0" + faxNo2;
        }
        txtTabOther_Information_FaxNo2.Text = faxNo2;

        txtTabOther_Information_VendorHead.Text = txtVendorHeadCode.Text.Trim();

        ddlTabOther_Information_AutoSignLayBill.SelectedValue = drVendor["P10ATS"].ToString().Trim();

        ForFaxOutboundBindData();

        LoadReferenceData();
    }

    protected void ForFaxOutboundBindData()
    {
        m_userInfo = userInfoService.GetUserInfo();
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        string userBranch = m_userInfo.BranchNo;
        dataCenter = new DataCenter(m_userInfo);
        string sql = @"SELECT G11BRN, G11VEN,G11BUS,G11VHO, G11SFX, G11OHF, G11IFX, G11IHF, G11TFX FROM AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK)
                        WHERE
                             CAST(G11BRN as nvarchar) = '" + userBranch + @"'
                        AND G11BUS = 'IL'
                        AND CAST(G11VEN as nvarchar) = '" + vendorCode + "'";

        ilObj.UserInfomation = m_userInfo;
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();
        if (cookiesStorage.check_dataset(ds))
        {
            dt = ds.Tables[0];
            dr = dt.Rows[0];
            ddlTabOther_Information_StatusFaxForOper.SelectedValue = dr["G11SFX"].ToString().Trim();
            ddlTabOther_Information_StatusFaxForISB.SelectedValue = dr["G11IFX"].ToString().Trim();
            ddlTabOther_Information_FaxForHOForOper.SelectedValue = dr["G11OHF"].ToString().Trim();
            ddlTabOther_Information_FaxForHOForISB.SelectedValue = dr["G11IHF"].ToString().Trim();
            ddlTabOther_Information_AutoFax.SelectedValue = dr["G11TFX"].ToString().Trim();
        }


    }

    #region Referencess
    protected void TabOther_Information_Reference_SetDefault()
    {
        txtTabOther_Information_Reference_IDCard.Text = "";
        ddlTabOther_Information_Reference_Title.SelectedIndex = 0;
        txtTabOther_Information_Reference_Name.Text = "";
        txtTabOther_Information_Reference_Surname.Text = "";
        txtTabOther_Information_Reference_Position.Text = "";
        txtTabOther_Information_Reference_Department.Text = "";
        txtTabOther_Information_Reference_JoinDate.Text = "";
        txtTabOther_Information_Reference_ExpireDate.Text = "";

        btnTabOther_Information_Reference_Add.Text = "Add";

        LoadReferenceTitleData();
    }

    protected void LoadReferenceTitleData()
    {
        DataSet dsTitle = new DataSet();
        try
        {
            string sql = "SELECT DescriptionTHAI as GNB2TD FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'TitleID'";

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            dsTitle = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        }
        catch (Exception ex)
        {

        }


        if (cookiesStorage.check_dataset(dsTitle))
        {
            ddlTabOther_Information_Reference_Title.Items.Clear();
            ddlTabOther_Information_Reference_Title.DataTextField = "GNB2TD";
            ddlTabOther_Information_Reference_Title.DataValueField = "GNB2TD";
            ddlTabOther_Information_Reference_Title.DataSource = dsTitle;
            ddlTabOther_Information_Reference_Title.DataBind();
            ddlTabOther_Information_Reference_Title.Items.Insert(0, new ListItem("Select", ""));
            ddlTabOther_Information_Reference_Title.ClearSelection();
        }

    }

    protected DataSet GetReferenceDataStructure()
    {
        DataSet dsReference = new DataSet();
        DataTable dtReference = new DataTable();
        dtReference.Columns.Add("Seq");
        dtReference.Columns.Add("IDCard");
        dtReference.Columns.Add("Title");
        dtReference.Columns.Add("Name");
        dtReference.Columns.Add("Surname");
        dtReference.Columns.Add("Position");
        dtReference.Columns.Add("Department");
        dtReference.Columns.Add("JoinDate");
        dtReference.Columns.Add("ExpireDate");

        dsReference.Tables.Add(dtReference);

        return dsReference;
    }

    protected void LoadReferenceData()
    {
        //DataSet ds = (DataSet)Session["ds_reference"];
        DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_reference.Value);
        if (!cookiesStorage.check_dataset(ds))
        {
            ds = GetReferenceDataStructure();
        }
        gvTabOther_Information_Reference.DataSource = ds;
        gvTabOther_Information_Reference.DataBind();
    }

    protected void gvTabOther_Information_ReferencePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTabOther_Information_Reference.PageIndex = e.NewPageIndex;
        //gvTabOther_Information_Reference.DataSource = (DataSet)Session["ds_reference"];
        gvTabOther_Information_Reference.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_reference.Value); ;
        gvTabOther_Information_Reference.DataBind();
    }

    protected void gvTabOther_Information_ReferenceSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        int selectRow = (gvTabOther_Information_Reference.PageIndex * Convert.ToInt16(gvTabOther_Information_Reference.PageSize)) + e.NewSelectedIndex;
        hdfReferenceRow.Value = selectRow.ToString();

        DataSet dsReference = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_reference.Value);
        DataTable dt = new DataTable();
        DataRow drReference = dt.NewRow();
        if (cookiesStorage.check_dataset(dsReference))
        {
            drReference = dsReference.Tables[0]?.Rows[selectRow];

            btnTabOther_Information_Reference_Add.Text = "Edit";
            txtTabOther_Information_Reference_IDCard.Text = drReference["IDCard"].ToString().Trim();
            ddlTabOther_Information_Reference_Title.SelectedValue = drReference["Title"].ToString();
            txtTabOther_Information_Reference_Name.Text = drReference["Name"].ToString().Trim();
            txtTabOther_Information_Reference_Surname.Text = drReference["Surname"].ToString().Trim();
            txtTabOther_Information_Reference_Position.Text = drReference["Position"].ToString().Trim();
            txtTabOther_Information_Reference_Department.Text = drReference["Department"].ToString().Trim();
            txtTabOther_Information_Reference_JoinDate.Text = drReference["JoinDate"].ToString().Trim();
            txtTabOther_Information_Reference_ExpireDate.Text = drReference["ExpireDate"].ToString().Trim();
        }


    }

    protected void gvTabOther_Information_ReferenceRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int deleteRow = (gvTabOther_Information_Reference.PageIndex * Convert.ToInt16(gvTabOther_Information_Reference.PageSize)) + e.RowIndex;
        hdfReferenceRow.Value = deleteRow.ToString();

        ConfirmDeleteReferenceHeadText = "Confirm Delete Person Reference";
        ConfirmDeleteReferenceMessage = "คุณต้องการลบ Person Reference?";
        SetMessageConfirmDeleteReference();
    }

    protected void btnTabOther_Information_Reference_Add_Click(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        string idCard = txtTabOther_Information_Reference_IDCard.Text.Trim();
        if (string.IsNullOrEmpty(idCard))
        {
            Message = "กรุณาใส่ ID Card Reference";
            MessageHeaderText = "Validate";
            SetMessage();
            txtTabOther_Information_Reference_IDCard.Focus();

            return;
        }

        if (ddlTabOther_Information_Reference_Title.SelectedIndex == 0)
        {
            Message = "กรุณาเลือก Title Reference";
            MessageHeaderText = "Validate";
            SetMessage();
            ddlTabOther_Information_Reference_Title.Focus();

            return;
        }

        string name = txtTabOther_Information_Reference_Name.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Message = "กรุณาใส่ชื่อ Reference";
            MessageHeaderText = "Validate";
            SetMessage();
            txtTabOther_Information_Reference_Name.Focus();

            return;
        }

        string surName = txtTabOther_Information_Reference_Surname.Text.Trim();
        if (string.IsNullOrEmpty(surName))
        {
            Message = "กรุณาใส่นามสกุล Reference";
            MessageHeaderText = "Validate";
            txtTabOther_Information_Reference_Surname.Focus();
            SetMessage();

            return;
        }

        if (txtTabOther_Information_Reference_JoinDate.Text != "")
        {
            //Call Stored Procedure "GNP0221"
            string joinDateString = txtTabOther_Information_Reference_JoinDate.Text.Replace("/", "").Trim();
            string prmError = "";
            string prmDate = joinDateString;
            GNP0221 gNP0221 = new GNP0221();
            //iLDataSubroutine.Call_GNP0221(prmDate, ref prmError, m_userInfo.BizInit, m_userInfo.BranchNo);
            gNP0221.Call_GNP0221(prmDate, ref prmError, m_userInfo.BizInit, m_userInfo.BranchNo);
            //ilObj.CloseConnectioDAL();

            if ((prmError == "D") || (prmError == "M") || (prmError == "Y"))
            {
                Message = "วันที่ Join Date ไม่ถูกต้อง";
                MessageHeaderText = "Validate";
                txtTabOther_Information_Reference_JoinDate.Focus();
                SetMessage();
                return;
            }

        }
        else
        {
            Message = "กรุณาเลือกวันที่ Join Date";
            MessageHeaderText = "Validate";
            txtTabOther_Information_Reference_JoinDate.Focus();
            SetMessage();
            return;
        }



        if (txtTabOther_Information_Reference_ExpireDate.Text != "")
        {
            //Call Stored Procedure "GNP0221"
            string expireDateString = txtTabOther_Information_Reference_ExpireDate.Text.Replace("/", "").Trim();
            string prmError = "";
            string prmDate = expireDateString;
            GNP0221 gNP0221 = new GNP0221();
            //iLDataSubroutine.Call_GNP0221(prmDate, ref prmError, m_userInfo.BizInit, m_userInfo.BranchNo);
            gNP0221.Call_GNP0221(prmDate, ref prmError, m_userInfo.BizInit, m_userInfo.BranchNo);
            //ilObj.CloseConnectioDAL();

            if ((prmError == "D") || (prmError == "M") || (prmError == "Y"))
            {
                Message = "วันที่ Expire Date ไม่ถูกต้อง";
                MessageHeaderText = "Validate";
                SetMessage();
                txtTabOther_Information_Reference_ExpireDate.Focus();

                return;
            }

            //bool checkExpireDate = ilObj.Call_GNP0221(expireDate, ref error, userInfo.BizInit, userInfo.BranchNo);
            //if(!checkExpireDate)
            //{
            //    //Alert "วันที่ Expire Date ไม่ถูกต้อง"
            //}
        }
        else
        {
            Message = "กรุณาเลือกวันที่ Expire Date";
            MessageHeaderText = "Validate";
            txtTabOther_Information_Reference_ExpireDate.Focus();
            SetMessage();
            return;
        }

        if (txtTabOther_Information_Reference_ExpireDate.Text != "" && txtTabOther_Information_Reference_JoinDate.Text != "")
        {
            int joinDate = int.Parse(CHANGE_FORMAT_DATE(txtTabOther_Information_Reference_JoinDate.Text));
            int expireDate = int.Parse(CHANGE_FORMAT_DATE(txtTabOther_Information_Reference_ExpireDate.Text));
            if (expireDate < joinDate)
            {
                Message = "Join Date > Expire Date";
                MessageHeaderText = "Validate";
                SetMessage();
                txtTabOther_Information_Reference_JoinDate.Focus();

                return;
            }
        }

        string action = btnTabOther_Information_Reference_Add.Text;
        ConfirmAddReferenceHeadText = "Confirm " + action + " Reference Person";
        if (action == "Add")
        {
            ConfirmAddReferenceMessage = "คุณต้องการบันทึกข้อมูล Reference Person?";
        }
        else
        {
            ConfirmAddReferenceMessage = "คุณต้องการแก้ไขข้อมูล Reference Person?";
        }

        SetMessageConfirmAddReference();
    }

    protected void btnTabOther_Information_Reference_Clear_Click(object sender, EventArgs e)
    {
        TabOther_Information_Reference_SetDefault();
    }

    #region Popup Confirm Add/Edit Reference
    protected void SetMessageConfirmAddReference()
    {
        PopupConfirmAddReference.HeaderText = ConfirmAddReferenceHeadText;
        lblPopupConfirmAddReferenceMessage.Text = ConfirmAddReferenceMessage;
        PopupConfirmAddReference.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmAddReferenceOK_Click(object sender, EventArgs e)
    {
        string action = btnTabOther_Information_Reference_Add.Text;
        MessageHeaderText = action + " Reference Person";

        try
        {

            DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(hiddenPopupConfirmAddReference.Value);
            if (!cookiesStorage.check_dataset(ds))
            {
                ds = GetReferenceDataStructure();
            }

            DataTable dt = ds.Tables[0];
            int maxSeq = dt.Rows.Count;

            string joinDate = txtTabOther_Information_Reference_JoinDate.Text.Trim();
            string joinDateValue = joinDate.Replace("/", "").Trim();
            joinDate = string.IsNullOrEmpty(joinDateValue) ? string.Empty : joinDate;

            string expireDate = txtTabOther_Information_Reference_ExpireDate.Text.Trim();
            string expireDateValue = expireDate.Replace("/", "").Trim();
            expireDate = string.IsNullOrEmpty(expireDateValue) ? string.Empty : expireDate;

            if (action == "Add")
            {
                DataRow dr = dt.NewRow();
                dr["Seq"] = (maxSeq + 1).ToString();
                dr["IDCard"] = txtTabOther_Information_Reference_IDCard.Text.Trim();
                dr["Title"] = ddlTabOther_Information_Reference_Title.SelectedValue;
                dr["Name"] = txtTabOther_Information_Reference_Name.Text.Trim();
                dr["Surname"] = txtTabOther_Information_Reference_Surname.Text.Trim();
                dr["Position"] = txtTabOther_Information_Reference_Position.Text.Trim();
                dr["Department"] = txtTabOther_Information_Reference_Department.Text.Trim();
                dr["JoinDate"] = joinDate;
                dr["ExpireDate"] = expireDate;

                dt.Rows.Add(dr);
            }
            else
            {
                int editRow = Convert.ToInt32(hdfReferenceRow.Value);
                DataRow dr = dt.Rows[editRow];
                dr["IDCard"] = txtTabOther_Information_Reference_IDCard.Text.Trim();
                dr["Title"] = ddlTabOther_Information_Reference_Title.SelectedValue;
                dr["Name"] = txtTabOther_Information_Reference_Name.Text.Trim();
                dr["Surname"] = txtTabOther_Information_Reference_Surname.Text.Trim();
                dr["Position"] = txtTabOther_Information_Reference_Position.Text.Trim();
                dr["Department"] = txtTabOther_Information_Reference_Department.Text.Trim();
                dr["JoinDate"] = joinDate;
                dr["ExpireDate"] = expireDate;
            }

            //Session["ds_reference"] = ds;
            ds_reference.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
            TabOther_Information_Reference_SetDefault();
            LoadReferenceData();

            Message = MessageHeaderText + " Complete";
            SetMessageSuccess();
        }
        catch (Exception ex)
        {
            Message = MessageHeaderText + " Not Complete";
            SetMessage();
        }
    }
    #endregion

    #region Popup Confirm Delete Reference
    protected void SetMessageConfirmDeleteReference()
    {
        PopupConfirmDeleteReference.HeaderText = ConfirmDeleteReferenceHeadText;
        lblPopupConfirmDeleteReferenceMessage.Text = ConfirmDeleteReferenceMessage;
        PopupConfirmDeleteReference.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmDeleteReferenceOK_Click(object sender, EventArgs e)
    {
        MessageHeaderText = "Delete Person Reference";
        Message = "Delete Person Reference ";
        try
        {
            int deleteRow = Convert.ToInt32(hdfReferenceRow.Value);
            //DataSet dsReference = (DataSet)Session["ds_reference"];
            DataSet dsReference = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_reference.Value);
            DataTable dtReference = new DataTable();
            DataRow drReference = dtReference.NewRow();
            if (cookiesStorage.check_dataset(dsReference))
            {
                dtReference = dsReference.Tables[0];
                drReference = dtReference.Rows[deleteRow];
                drReference.Delete();
                dtReference.AcceptChanges();
            }
            ds_reference.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsReference);
            TabOther_Information_Reference_SetDefault();
            LoadReferenceData();

            Message = Message + "Complete";
            SetMessageSuccess();
        }
        catch (Exception ex)
        {
            Message = Message + "Not Complete";

            SetMessage();
        }
    }
    #endregion    
    #endregion
    #endregion

    #region Tab Other : Payment
    protected void TabOther_Payment_SetDefault()
    {
        txtTabOther_Payment_PayToVendor.Text = "";
        txtTabOther_Payment_BranchCode.Text = "";
        txtTabOther_Payment_BranchPayment.Text = "";

        ddlTabOther_Payment_PaymentType.SelectedIndex = 0;
        txtTabOther_Payment_BankAccNo.Text = "";
        ddlTabOther_Payment_BankCode.SelectedIndex = 0;

        ddlTabOther_Payment_BankRegion.SelectedIndex = 0;
        ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;
        ddlTabOther_Payment_SpecialLoan.SelectedIndex = 0;

        rdoTabOther_Payment_SignBeforeRecDoc.ClearSelection();
        ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedIndex = 0;
        txtTabOther_Payment_CreditDays.Text = "";

        txtTabOther_Payment_Marketing.Text = "";
        txtTabOther_Payment_TimeAvailable.Text = "";
        txtTabOther_Payment_LeadTime.Text = "";

        txtTabOther_Payment_CLBranch.Text = "";
        txtTabOther_Payment_PayeeName.Text = "";

        PaymentTypeLoadData();
        BankCodeLoadData();
    }

    protected void PaymentTypeLoadData()
    {
        string sql = "SELECT GN40CD,GN40DT FROM AS400DB01.GNOD0000.GNTB40 WITH (NOLOCK)";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(ds))
        {

            ddlTabOther_Payment_PaymentType.Items.Clear();
            ddlTabOther_Payment_PaymentType.DataTextField = "GN40DT";
            ddlTabOther_Payment_PaymentType.DataValueField = "GN40CD";
            ddlTabOther_Payment_PaymentType.DataSource = ds;
            ddlTabOther_Payment_PaymentType.DataBind();
        }
        ddlTabOther_Payment_PaymentType.Items.Insert(0, new ListItem("Select", ""));
        ddlTabOther_Payment_PaymentType.SelectedIndex = 0;
    }

    protected void BankCodeLoadData()
    {
        string sql = "SELECT TRIM(BankCode) AS GNB30A, TRIM(BankNameTHAI) AS GNB30C FROM GeneralDB01.GeneralInfo.Bank WITH (NOLOCK)";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(ds))
        {
            ddlTabOther_Payment_BankCode.Items.Clear();
            ddlTabOther_Payment_BankCode.DataValueField = "GNB30A";
            ddlTabOther_Payment_BankCode.DataTextField = "GNB30C";
            ddlTabOther_Payment_BankCode.DataSource = ds;
            ddlTabOther_Payment_BankCode.DataBind();
        }
        ddlTabOther_Payment_BankCode.Items.Insert(0, new ListItem("Select", ""));
        ddlTabOther_Payment_BankCode.SelectedIndex = 0;
    }

    protected void TabOther_Payment_BindData(DataRow drVendor)
    {
        string payToVendor = drVendor["P10PVN1"].ToString().Trim();
        txtTabOther_Payment_PayToVendor.Text = !string.IsNullOrEmpty(payToVendor) ? payToVendor.Length >= 12 ? VendorCodeFormatter(payToVendor) : payToVendor : "0";
        txtTabOther_Payment_BranchCode.Text = drVendor["T1BRN"].ToString().Trim();
        txtTabOther_Payment_BranchPayment.Text = drVendor["T1BNME"].ToString().Trim();

        ddlTabOther_Payment_PaymentType.SelectedValue = drVendor["P10PTY"].ToString().Trim();
        txtTabOther_Payment_BankAccNo.Text = drVendor["P10BNO"].ToString().Trim();
        ddlTabOther_Payment_BankCode.SelectedValue = drVendor["P10BCD"].ToString().Trim();

        ddlTabOther_Payment_BankRegion.SelectedValue = drVendor["P10BRG"].ToString().Trim();
        ddlTabOther_Payment_DeliveryCTBCHQ.SelectedValue = drVendor["P10DLV"].ToString().Trim();
        ddlTabOther_Payment_SpecialLoan.SelectedValue = drVendor["P10SPC"].ToString().Trim();

        rdoTabOther_Payment_SignBeforeRecDoc.SelectedValue = drVendor["P10SFG"].ToString().Trim();
        ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedValue = drVendor["P10CLD"].ToString().Trim();
        txtTabOther_Payment_CreditDays.Text = drVendor["P10CRD"].ToString().Trim();

        txtTabOther_Payment_Marketing.Text = drVendor["P10MKD"].ToString().Trim();
        txtTabOther_Payment_TimeAvailable.Text = drVendor["P10TAV"].ToString().Trim();
        txtTabOther_Payment_LeadTime.Text = drVendor["P10LTM"].ToString().Trim();

        txtTabOther_Payment_CLBranch.Text = GetCLBranch();
        txtTabOther_Payment_PayeeName.Text = drVendor["P10PYE"].ToString().Trim();

        /*** Temp for Pay to Vendor ***/
        hdfTempBranchCode.Value = drVendor["OWNBRN"].ToString().Trim();
        hdfTempBranchPayment.Value = drVendor["OWNBRNNAME"].ToString().Trim();

        lblReqTabOther_Payment_BankRegion.Visible = false;
        lblReqTabOther_Payment_BankCode.Visible = false;
        lblReqTabOther_Payment_DeliveryCTBCHQ.Visible = false;

        switch (drVendor["P10PTY"].ToString().Trim())
        {
            case "1":
                txtTabOther_Payment_BankAccNo.Enabled = true;
                ddlTabOther_Payment_BankCode.Enabled = false;
                lblReqTabOther_Payment_BankCode.Visible = true;
                ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
                lblReqTabOther_Payment_BankRegion.Visible = true;
                break;
            case "2":
                txtTabOther_Payment_BankAccNo.Enabled = false;
                ddlTabOther_Payment_BankCode.Enabled = false;
                ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = true;
                lblReqTabOther_Payment_DeliveryCTBCHQ.Visible = true;
                break;
            case "3":
                txtTabOther_Payment_BankAccNo.Enabled = true;
                ddlTabOther_Payment_BankCode.Enabled = true;
                lblReqTabOther_Payment_BankCode.Visible = true;
                ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
                lblReqTabOther_Payment_BankRegion.Visible = true;
                break;
            default:
                txtTabOther_Payment_BankAccNo.Enabled = false;
                ddlTabOther_Payment_BankCode.Enabled = false;
                ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
                break;
        }
    }

    protected string GetCLBranch()
    {
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");

        string sql = "SELECT SUBSTRING(CAST(P11FIL as varchar),1,3) AS P11FIL FROM AS400DB01.ILOD0001.ILMS11 WITH (NOLOCK) WHERE P11VEN = " + vendorCode;

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();
        string clBranch = string.Empty;
        if (cookiesStorage.check_dataset(ds))
        {
            dr = ds.Tables[0]?.Rows[0];
            clBranch = dr["P11FIL"].ToString();
        }
        return clBranch;
    }

    protected void btnTabOther_Payment_LoadAllDataFromPaymentVendor_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorHeadCode.Text.Trim().Replace("-", "");

        if (!string.IsNullOrEmpty(vendorCode))
        {
            string sql = @"SELECT
                                P10PTY,
                                P10BNO,
                                P10BCD,
                                P10BRG,
                                P10SFG,
                                P10MKD,
                                P10CLD,
                                P10TAV,
                                P10PYE,
                                P10SPC,
                                P10CRD,
                                P10LTM
                            FROM
                                AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                            WHERE
                                P10VEN = " + vendorCode;

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsVendor = new DataSet();
            dsVendor = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            DataTable dt = new DataTable();
            DataRow drVendor = dt.NewRow();
            if (cookiesStorage.check_dataset(dsVendor))
            {
                drVendor = dsVendor.Tables[0]?.Rows[0];

                ddlTabOther_Payment_PaymentType.SelectedValue = drVendor["P10PTY"].ToString().Trim();
                txtTabOther_Payment_BankAccNo.Text = drVendor["P10BNO"].ToString().Trim();
                ddlTabOther_Payment_BankCode.SelectedValue = drVendor["P10BCD"].ToString().Trim();
                ddlTabOther_Payment_BankRegion.SelectedValue = drVendor["P10BRG"].ToString().Trim();
                rdoTabOther_Payment_SignBeforeRecDoc.SelectedValue = drVendor["P10SFG"].ToString().Trim();
                txtTabOther_Payment_Marketing.Text = drVendor["P10MKD"].ToString().Trim();
                ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedValue = drVendor["P10CLD"].ToString().Trim();
                txtTabOther_Payment_TimeAvailable.Text = drVendor["P10TAV"].ToString().Trim();
                txtTabOther_Payment_PayeeName.Text = drVendor["P10PYE"].ToString().Trim();
                ddlTabOther_Payment_SpecialLoan.SelectedValue = drVendor["P10SPC"].ToString().Trim();
                txtTabOther_Payment_CreditDays.Text = drVendor["P10CRD"].ToString().Trim();
                txtTabOther_Payment_LeadTime.Text = drVendor["P10LTM"].ToString().Trim();
            }


        }
    }

    protected void ddlTabOther_Payment_PaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string paymentType = ddlTabOther_Payment_PaymentType.SelectedValue;

        lblReqTabOther_Payment_BankRegion.Visible = false;
        lblReqTabOther_Payment_BankCode.Visible = false;
        lblReqTabOther_Payment_DeliveryCTBCHQ.Visible = false;

        if (ddlTabOther_Payment_PaymentType.SelectedIndex == 0)
        {
            txtTabOther_Payment_BankAccNo.Enabled = false;
            txtTabOther_Payment_BankAccNo.Text = "";

            ddlTabOther_Payment_BankCode.Enabled = false;
            ddlTabOther_Payment_BankCode.SelectedIndex = 0;

            ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
            ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;
        }

        if (paymentType == "1")
        {
            txtTabOther_Payment_BankAccNo.Enabled = true;

            ddlTabOther_Payment_BankCode.SelectedValue = "BBL";
            ddlTabOther_Payment_BankCode.Enabled = false;
            lblReqTabOther_Payment_BankCode.Visible = true;

            ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
            ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;

            lblReqTabOther_Payment_BankRegion.Visible = true;
        }

        if (paymentType == "2")
        {
            txtTabOther_Payment_BankAccNo.Text = "";
            txtTabOther_Payment_BankAccNo.Enabled = false;

            ddlTabOther_Payment_BankCode.SelectedIndex = 0;
            ddlTabOther_Payment_BankCode.Enabled = false;

            ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = true;
            ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 1;
            lblReqTabOther_Payment_DeliveryCTBCHQ.Visible = true;
        }

        if (paymentType == "3")
        {
            txtTabOther_Payment_BankAccNo.Enabled = true;

            ddlTabOther_Payment_BankCode.Enabled = true;
            lblReqTabOther_Payment_BankCode.Visible = true;

            ddlTabOther_Payment_DeliveryCTBCHQ.Enabled = false;
            ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;

            lblReqTabOther_Payment_BankRegion.Visible = true;
        }
    }

    #region Poup Pay to Vendor
    protected void btnTabOther_Payment_AddPayToVendor_Click(object sender, ImageClickEventArgs e)
    {
        PopupPayToVendor.HeaderText = "Pay to Vendor";
        lblPopupPayToVendorMessage.Text = "Pay to Own Office?";
        PopupPayToVendor.ShowOnPageLoad = true;
    }

    protected void btnPopupPayToVendorYes_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorCode.Text.Trim();

        if (!string.IsNullOrEmpty(vendorCode) && vendorCode != "0")
        {
            txtTabOther_Payment_PayToVendor.Text = vendorCode;
            txtTabOther_Payment_BranchCode.Text = hdfTempBranchCode.Value;
            txtTabOther_Payment_BranchPayment.Text = hdfTempBranchPayment.Value;
        }
        else
        {
            m_userInfo = userInfoService.GetUserInfo();
            string branchCode = m_userInfo.BranchNo;
            string branchPayment = m_userInfo.BranchDescEN;

            txtTabOther_Payment_PayToVendor.Text = "0";
            txtTabOther_Payment_BranchCode.Text = "001";
            txtTabOther_Payment_BranchPayment.Text = "Head Office";
        }
    }

    protected void btnPopupPayToVendorNo_Click(object sender, EventArgs e)
    {
        hdfVendorSection.Value = "PAYTOVENDOR";
        PopupAddVendorHead.ShowOnPageLoad = true;
        LoadVendorHeadData();
    }
    #endregion

    #region Popup Add Marketing
    protected void btnTabOther_Payment_AddMarketing_Click(object sender, ImageClickEventArgs e)
    {
        PopupAddMarketing.ShowOnPageLoad = true;
        MarketingLoadData();
    }

    protected void MarketingLoadData()
    {
        m_userInfo = userInfoService.GetUserInfo();
        string userBranch = m_userInfo.BranchNo;
        string sqlWhere = "";
        string searchBy = ddlPopupAddMarketingSearchBy.SelectedValue;
        string searchText = txtPopupAddMarketingSearchText.Text.Trim().Replace("'", "''").ToUpper();

        if (!string.IsNullOrEmpty(searchText))
        {
            switch (searchBy)
            {
                case "MC":
                    sqlWhere = "AND UPPER(T74CDE) LIKE '%" + searchText + "%'";
                    break;
                case "MN":
                    sqlWhere = "AND UPPER(T74NME) LIKE '%" + searchText + "%'";
                    break;
                default: break;
            }
        }

        string sql = @"SELECT
                            T74CDE,
                            T74NME
                        FROM
                            AS400DB01.ILOD0001.ILTB74 WITH (NOLOCK)
                        WHERE
                            T74BRN = " + userBranch + @"
                        AND T74STS = '' " + sqlWhere;

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        ds_marketing.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
        if (cookiesStorage.check_dataset(ds))
        {
            gvMarketing.DataSource = ds;
            gvMarketing.DataBind();
        }
    }

    protected void gvMarketingPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMarketing.PageIndex = e.NewPageIndex;
        gvMarketing.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_marketing.Value);
        gvMarketing.DataBind();
    }

    protected void gvMarketingSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_marketing.Value);
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();
        if (cookiesStorage.check_dataset(ds))
        {
            dr = ds.Tables[0]?.Rows[(gvMarketing.PageIndex * Convert.ToInt16(gvMarketing.PageSize)) + e.NewSelectedIndex];
            txtTabOther_Payment_Marketing.Text = dr["T74CDE"].ToString().Trim();
        }

        PopupAddMarketing.ShowOnPageLoad = false;
    }

    protected void btnPopupAddMarketingSearch_Click(object sender, EventArgs e)
    {
        MarketingLoadData();
    }

    protected void btnPopupAddMarketingClear_Click(object sender, EventArgs e)
    {
        ddlPopupAddMarketingSearchBy.SelectedIndex = 0;
        txtPopupAddMarketingSearchText.Text = "";
        MarketingLoadData();
    }
    #endregion

    #region Popup Add CL Branch
    protected void btnTabOther_Payment_AddCLBranch_Click(object sender, ImageClickEventArgs e)
    {
        PopupAddCLBranch.ShowOnPageLoad = true;
        CLBranchLoadData();
    }

    protected void CLBranchLoadData()
    {
        string sqlWhere = "";
        string searchBy = ddlPopupAddCLBranchSearchBy.SelectedValue;
        string searchText = txtPopupAddCLBranchSearchText.Text.Trim().Replace("'", "''").ToUpper();

        if (!string.IsNullOrEmpty(searchText))
        {
            switch (searchBy)
            {
                case "BC":
                    sqlWhere = " WHERE FORMAT(T1BRN,'000') LIKE '%" + searchText + "%'";
                    break;
                case "BN":
                    sqlWhere = " WHERE T1BNMT LIKE '%" + searchText + "%'";
                    break;
                default: break;
            }
        }
        string sql = "SELECT SUBSTRING(CAST(T1BRN as varchar),1,3) AS T1BRN, T1BNMT FROM AS400DB01.RLOD0001.RLTB01 WITH (NOLOCK)" + sqlWhere;

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        ds_cl_branch.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
        if (cookiesStorage.check_dataset(ds))
        {
            gvBranch.DataSource = ds;
            gvBranch.DataBind();
        }
    }

    protected void gvBranchPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBranch.PageIndex = e.NewPageIndex;
        gvBranch.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_cl_branch.Value);
        gvBranch.DataBind();
    }

    protected void gvBranchSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_cl_branch.Value);
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();
        if (cookiesStorage.check_dataset(ds))
        {
            dr = ds.Tables[0]?.Rows[(gvBranch.PageIndex * Convert.ToInt16(gvBranch.PageSize)) + e.NewSelectedIndex];
            txtTabOther_Payment_CLBranch.Text = dr["T1BRN"].ToString().Trim();
        }

        PopupAddCLBranch.ShowOnPageLoad = false;
    }

    protected void btnPopupAddCLBranchSearch_Click(object sender, EventArgs e)
    {
        CLBranchLoadData();
    }

    protected void btnPopupAddCLBranchClear_Click(object sender, EventArgs e)
    {
        ddlPopupAddCLBranchSearchBy.SelectedIndex = 0;
        txtPopupAddCLBranchSearchText.Text = "";
        CLBranchLoadData();
    }
    #endregion
    #endregion

    #region Tab Person to Contact
    protected void TabPersonToContact_SetDefault()
    {
        btnTabPersonToContact_Add.Text = "Add";

        txtTabPersonToContact_Seq.Text = "";
        txtTabPersonToContact_ThaiName.Text = "";
        txtTabPersonToContact_EngName.Text = "";
        txtTabPersonToContact_Department.Text = "";
        txtTabPersonToContact_MobilePhone.Text = "";

        /*** Telephone 1 ***/
        txtTabPersonToContact_TelephoneNo1.Text = "";
        txtTabPersonToContact_ContactTelRange1.Text = "";
        txtTabPersonToContact_Extension1.Text = "";
        txtTabPersonToContact_FaxNo1.Text = "";

        /*** Telephone 2 ***/
        txtTabPersonToContact_TelephoneNo2.Text = "";
        txtTabPersonToContact_ContactTelRange2.Text = "";
        txtTabPersonToContact_Extension2.Text = "";
        txtTabPersonToContact_FaxNo2.Text = "";

        /*** Telephone 3 ***/
        txtTabPersonToContact_TelephoneNo3.Text = "";
        txtTabPersonToContact_ContactTelRange3.Text = "";
        txtTabPersonToContact_Extension3.Text = "";
        txtTabPersonToContact_FaxNo3.Text = "";

        LoadPersonToContactData();
    }

    protected void TabPersonToContact_BindData()
    {
        LoadPersonToContactData();
    }

    protected DataSet GetPersonToContactDataStructure()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("T49COD");
        dt.Columns.Add("T49RTY");
        dt.Columns.Add("T49PAR");
        dt.Columns.Add("T49SEQ");
        dt.Columns.Add("T49TNM");
        dt.Columns.Add("T49ENM");
        dt.Columns.Add("T49DEP");
        dt.Columns.Add("T49HMB");
        dt.Columns.Add("T49TE1");
        dt.Columns.Add("T49TR1");
        dt.Columns.Add("T49EX1");
        dt.Columns.Add("T49FX1");
        dt.Columns.Add("T49TE2");
        dt.Columns.Add("T49TR2");
        dt.Columns.Add("T49EX2");
        dt.Columns.Add("T49FX2");
        dt.Columns.Add("T49TE3");
        dt.Columns.Add("T49TR3");
        dt.Columns.Add("T49EX3");
        dt.Columns.Add("T49FX3");
        dt.Columns.Add("T49RNO");
        ds.Tables.Add(dt);

        return ds;
    }

    protected void LoadPersonToContactData()
    {
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        DataSet dsPersonToContact = new DataSet();
        dsPersonToContact = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);

        try
        {
            if (!cookiesStorage.check_dataset(dsPersonToContact))
            {
                string sql = @"SELECT T49PAR, T49RTY, T49COD, T49SEQ, T49RNO, T49TNM, T49ENM, T49DEP, T49TE1, T49TR1, T49EX1, T49FX1, T49TE2, T49TR2, T49EX2, 
                                    T49FX2, T49TE3, T49TR3, T49EX3, T49FX3, T49HMB, T49RSV, T49UDD, T49UDT, T49PGM, T49USR, T49DSP, T49DEL 
                                FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK) WHERE FORMAT(T49COD,'000000000000') = '" + vendorCode + "' AND T49DEL = ''";

                m_userInfo = userInfoService.GetUserInfo();

                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                dsPersonToContact = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            }
        }
        catch (Exception ex)
        {
            dsPersonToContact = GetPersonToContactDataStructure();
        }
        if (dsPersonToContact.Tables.Count == 0)
        {
            dsPersonToContact = GetPersonToContactDataStructure();
        }
        ds_person_to_contact.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsPersonToContact);
        //cookiesStorage.SetCookiesDataSetByName("ds_person_to_contact", dsPersonToContact);
        if (cookiesStorage.check_dataset(dsPersonToContact))
        {
            gvTabPersonToContact.DataSource = dsPersonToContact;
            gvTabPersonToContact.DataBind();
        }
    }

    protected bool ValidatePhoneNumber(string phoneNumber, string phoneType)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber == "0" || phoneNumber == "**")
        {
            return true;
        }

        //P : เบอร์บ้าน
        //M : เบอร์มือถือ
        //F : เบอร์แฟกซ์

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        string prmError = "";
        string prmErrorMsg = "";
        string prmOTYPE = "";
        bool validatePhoneNumber = iLDataSubroutine.Call_GNSR48(phoneNumber, "3", ref prmError, ref prmErrorMsg, m_userInfo.BizInit, m_userInfo.BranchNo, ref prmOTYPE);
        if (!validatePhoneNumber || prmError == "Y")
        {
            Message = prmError;
        }
        else
        {
            if ((phoneType == "F") && (prmOTYPE != "P"))
            {
                Message = phoneNumber + " รูปแบบเบอร์ Fax ผิดพลาด";
                return false;
            }

            if ((phoneType != prmOTYPE) && (phoneType != "F"))
            {
                Message = phoneNumber + " รูปแบบผิดพลาด";
                return false;
            }
        }

        return false;
    }

    protected void btnTabPersonToContact_Add_Click(object sender, EventArgs e)
    {
        string action = btnTabPersonToContact_Add.Text;

        string thaiName = txtTabPersonToContact_ThaiName.Text.Trim();
        string englishName = txtTabPersonToContact_EngName.Text.Trim();

        if (string.IsNullOrEmpty(thaiName) && string.IsNullOrEmpty(englishName))
        {
            MessageHeaderText = "Validate";
            Message = "กรุณาใส่ Thai Name หรือ English Name";
            SetMessage();

            return;
        }

        //Validate Phone Number : Call Stored Procedure

        ConfirmAddPersonToContactHeadText = "Confirm";
        ConfirmAddPersonToContactMessage = "Confirm " + action + " Person to Contact?";
        SetConfirmAddEditPersonToContact();
    }

    protected void btnTabPersonToContact_Clear_Click(object sender, EventArgs e)
    {
        TabPersonToContact_SetDefault();
    }

    protected void gvTabPersonToContactPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTabPersonToContact.PageIndex = e.NewPageIndex;
        gvTabPersonToContact.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
        gvTabPersonToContact.DataBind();
    }

    protected void gvTabPersonToContactSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        int selectRow = (gvTabPersonToContact.PageIndex * Convert.ToInt16(gvTabPersonToContact.PageSize)) + e.NewSelectedIndex;
        hdfPersonToContactRow.Value = selectRow.ToString();
        DataSet dsPersonToContact = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
        DataRow drPersonToContact = dsPersonToContact.Tables[0]?.Rows[selectRow];

        btnTabPersonToContact_Add.Text = "Edit";

        txtTabPersonToContact_Seq.Text = drPersonToContact["T49SEQ"].ToString().Trim();
        txtTabPersonToContact_ThaiName.Text = drPersonToContact["T49TNM"].ToString().Trim();
        txtTabPersonToContact_EngName.Text = drPersonToContact["T49ENM"].ToString().Trim();
        txtTabPersonToContact_Department.Text = drPersonToContact["T49DEP"].ToString().Trim();
        txtTabPersonToContact_MobilePhone.Text = drPersonToContact["T49HMB"].ToString().Trim();

        txtTabPersonToContact_TelephoneNo1.Text = drPersonToContact["T49TE1"].ToString().Trim();
        txtTabPersonToContact_ContactTelRange1.Text = drPersonToContact["T49TR1"].ToString().Trim();
        txtTabPersonToContact_Extension1.Text = drPersonToContact["T49EX1"].ToString().Trim();
        txtTabPersonToContact_FaxNo1.Text = drPersonToContact["T49FX1"].ToString().Trim();

        txtTabPersonToContact_TelephoneNo2.Text = drPersonToContact["T49TE2"].ToString().Trim();
        txtTabPersonToContact_ContactTelRange2.Text = drPersonToContact["T49TR2"].ToString().Trim();
        txtTabPersonToContact_Extension2.Text = drPersonToContact["T49EX2"].ToString().Trim();
        txtTabPersonToContact_FaxNo2.Text = drPersonToContact["T49FX2"].ToString().Trim();

        txtTabPersonToContact_TelephoneNo3.Text = drPersonToContact["T49TE3"].ToString().Trim();
        txtTabPersonToContact_ContactTelRange3.Text = drPersonToContact["T49TR3"].ToString().Trim();
        txtTabPersonToContact_Extension3.Text = drPersonToContact["T49EX3"].ToString().Trim();
        txtTabPersonToContact_FaxNo3.Text = drPersonToContact["T49FX3"].ToString().Trim();
    }

    protected void gvTabPersonToContactRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int selectRow = (gvTabPersonToContact.PageIndex * Convert.ToInt16(gvTabPersonToContact.PageSize)) + e.RowIndex;
        hdfPersonToContactRow.Value = selectRow.ToString();

        ConfirmDeletePersonToContactHeadText = "Confirm Delete";
        ConfirmDeletePersonToContactMessage = "คุณต้องการลบ Person to Contact?";
        SetConfirmDeletePersonToContact();
    }

    #region Popup Confirm Add/Edit Person to Contact
    protected void SetConfirmAddEditPersonToContact()
    {
        PopupTabPersonToContactConfirmAdd.HeaderText = ConfirmAddPersonToContactHeadText;
        lblPopupTabPersonToContactConfirmAddMessage.Text = ConfirmAddPersonToContactMessage;
        PopupTabPersonToContactConfirmAdd.ShowOnPageLoad = true;
    }

    protected void btnPopupTabPersonToContactConfirmAddOK_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        string thaiName = txtTabPersonToContact_ThaiName.Text.Trim();
        string englishName = txtTabPersonToContact_EngName.Text.Trim();
        string department = txtTabPersonToContact_Department.Text.Trim();
        string mobilePhone = txtTabPersonToContact_MobilePhone.Text.Trim();

        string telephoneNo1 = txtTabPersonToContact_TelephoneNo1.Text.Trim();
        string contactTelRange1 = txtTabPersonToContact_ContactTelRange1.Text.Trim();
        string extension1 = txtTabPersonToContact_Extension1.Text.Trim();
        string faxNo1 = txtTabPersonToContact_FaxNo1.Text.Trim();

        string telephoneNo2 = txtTabPersonToContact_TelephoneNo2.Text.Trim();
        string contactTelRange2 = txtTabPersonToContact_ContactTelRange2.Text.Trim();
        string extension2 = txtTabPersonToContact_Extension2.Text.Trim();
        string faxNo2 = txtTabPersonToContact_FaxNo2.Text.Trim();

        string telephoneNo3 = txtTabPersonToContact_TelephoneNo3.Text.Trim();
        string contactTelRange3 = txtTabPersonToContact_ContactTelRange3.Text.Trim();
        string extension3 = txtTabPersonToContact_Extension3.Text.Trim();
        string faxNo3 = txtTabPersonToContact_FaxNo3.Text.Trim();

        DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
        if (!cookiesStorage.check_dataset(ds))
        {
            ds = GetPersonToContactDataStructure();
        }

        DataTable dt = ds.Tables[0];
        string action = btnTabPersonToContact_Add.Text;

        if (action == "Add")
        {
            int seq = dt.Rows.Count + 1;

            DataRow dr = dt.NewRow();
            if (!string.IsNullOrEmpty(vendorCode))
            {
                dr["T49COD"] = Convert.ToDecimal(vendorCode);
            }

            string part = Mode == "Add" ? "V" : "N";

            dr["T49RTY"] = "C";
            dr["T49PAR"] = part;
            dr["T49SEQ"] = seq.ToString();
            dr["T49TNM"] = thaiName;
            dr["T49ENM"] = englishName;
            dr["T49DEP"] = department;
            dr["T49HMB"] = mobilePhone;

            #region Telephone1
            if (!string.IsNullOrEmpty(telephoneNo1))
            {
                dr["T49TE1"] = telephoneNo1;
            }

            if (!string.IsNullOrEmpty(contactTelRange1))
            {
                dr["T49TR1"] = contactTelRange1;
            }

            dr["T49EX1"] = extension1;

            if (!string.IsNullOrEmpty(faxNo1))
            {
                dr["T49FX1"] = faxNo1;
            }
            #endregion

            #region Telephone2
            if (!string.IsNullOrEmpty(telephoneNo2))
            {
                dr["T49TE2"] = telephoneNo2;
            }

            if (!string.IsNullOrEmpty(contactTelRange2))
            {
                dr["T49TR2"] = contactTelRange2;
            }

            dr["T49EX2"] = extension2;

            if (!string.IsNullOrEmpty(faxNo2))
            {
                dr["T49FX2"] = faxNo2;
            }
            #endregion

            #region Telephone3
            if (!string.IsNullOrEmpty(telephoneNo3))
            {
                dr["T49TE3"] = telephoneNo3;
            }

            if (!string.IsNullOrEmpty(contactTelRange3))
            {
                dr["T49TR3"] = contactTelRange3;
            }

            dr["T49EX3"] = extension3;

            if (!string.IsNullOrEmpty(faxNo3))
            {
                dr["T49FX3"] = faxNo3;
            }
            #endregion

            dt.Rows.Add(dr);
        }

        if (action == "Edit")
        {
            int editRow = Convert.ToInt32(hdfPersonToContactRow.Value);
            DataRow dr = dt.Rows[editRow];
            dr["T49TNM"] = thaiName;
            dr["T49ENM"] = englishName;
            dr["T49DEP"] = department;
            dr["T49HMB"] = mobilePhone;

            #region Telephone1
            if (!string.IsNullOrEmpty(telephoneNo1))
            {
                dr["T49TE1"] = telephoneNo1;
            }

            if (!string.IsNullOrEmpty(contactTelRange1))
            {
                dr["T49TR1"] = contactTelRange1;
            }

            dr["T49EX1"] = extension1;

            if (!string.IsNullOrEmpty(faxNo1))
            {
                dr["T49FX1"] = faxNo1;
            }
            #endregion

            #region Telephone2
            if (!string.IsNullOrEmpty(telephoneNo2))
            {
                dr["T49TE2"] = telephoneNo2;
            }

            if (!string.IsNullOrEmpty(contactTelRange2))
            {
                dr["T49TR2"] = contactTelRange2;
            }

            dr["T49EX2"] = extension2;

            if (!string.IsNullOrEmpty(faxNo2))
            {
                dr["T49FX2"] = faxNo2;
            }
            #endregion

            #region Telephone3
            if (!string.IsNullOrEmpty(telephoneNo3))
            {
                dr["T49TE3"] = telephoneNo3;
            }

            if (!string.IsNullOrEmpty(contactTelRange3))
            {
                dr["T49TR3"] = contactTelRange3;
            }

            dr["T49EX3"] = extension3;

            if (!string.IsNullOrEmpty(faxNo3))
            {
                dr["T49FX3"] = faxNo3;
            }
            #endregion
        }

        ds_person_to_contact.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);

        TabPersonToContact_SetDefault();
    }
    #endregion

    #region Popup Confirm Delete Person to Contact
    protected void SetConfirmDeletePersonToContact()
    {
        PopupTabPersonToContactConfirmDelete.HeaderText = ConfirmDeletePersonToContactHeadText;
        lblPopupTabPersonToContactConfirmDeleteMessage.Text = ConfirmDeletePersonToContactMessage;
        PopupTabPersonToContactConfirmDelete.ShowOnPageLoad = true;
    }

    protected void btnPopupTabPersonToContactConfirmDeleteOK_Click(object sender, EventArgs e)
    {
        MessageHeaderText = "Delete";
        Message = "Delete Person to Contact ";
        try
        {
            int deleteRow = Convert.ToInt32(hdfPersonToContactRow.Value);
            DataSet dsPersonToContact = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
            DataTable dtPersonToContact = new DataTable();
            DataRow drPersonToContact = dtPersonToContact.NewRow();
            if (cookiesStorage.check_dataset(dsPersonToContact))
            {
                dtPersonToContact = dsPersonToContact.Tables[0];
                drPersonToContact = dtPersonToContact.Rows[deleteRow];
                drPersonToContact.Delete();
                dtPersonToContact.AcceptChanges();
            }
            ds_person_to_contact.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsPersonToContact);
            TabPersonToContact_SetDefault();
            LoadPersonToContactData();

            Message = Message + "Complete";
            SetMessageSuccess();
        }
        catch (Exception ex)
        {
            Message = Message + "Not Complete";
            SetMessage();
        }
    }
    #endregion
    #endregion

    #region Tab Note
    protected void TabNote_SetDefault()
    {
        txtTabNote_Memo.Text = "";
        LoadNoteData();
    }

    protected void TabNote_BindData()
    {
        LoadNoteData();
    }

    protected void LoadNoteData()
    {
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");

        string sql = @"SELECT
	                        P14NSQ,
	                        P14LSQ,
	                        P14NOT,
	                        P14UDT,
	                        P14UTM,
	                        P14UUS,
	                        SUBSTRING(CAST(P14UDT AS varchar), 7, 2) + '/' + SUBSTRING(CAST(P14UDT AS varchar), 5, 2) + '/' + SUBSTRING(CAST(P14UDT AS varchar), 1, 4) AS NOTE_DATE,
	                        SUBSTRING(CAST(P14UTM AS varchar), 1, 2) + ':' + SUBSTRING(CAST(P14UTM AS varchar), 3, 2) + ':' + SUBSTRING(CAST(P14UTM AS varchar), 5, 2) AS NOTE_TIME
                        FROM
	                         AS400DB01.ILOD0001.ILMS14 WITH (NOLOCK)
                        WHERE
                            FORMAT(P14VEN,'000000000000') = '" + vendorCode + @"'
                        ORDER BY
	                        P14NSQ DESC,
	                        P14LSQ ASC";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        //Session["ds_note"] = ds;
        ds_note.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
        if (cookiesStorage.check_dataset(ds))
        {
            gvNote.DataSource = ds;
            gvNote.DataBind();
        }
    }

    protected void gvNotePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNote.PageIndex = e.NewPageIndex;
        //gvNote.DataSource = (DataSet)Session["ds_note"];
        gvNote.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_note.Value);
        gvNote.DataBind();
    }

    protected void btnTabNoteAdd_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorCode.Text.Trim();
        string noteMemo = txtTabNote_Memo.Text.Trim();

        if (string.IsNullOrEmpty(noteMemo))
        {
            MessageHeaderText = "Validate";
            Message = "กรุณาระบุ Note";
            SetMessage();

            return;
        }

        if (string.IsNullOrEmpty(vendorCode))
        {
            MessageHeaderText = "Validate";
            Message = "กรุณาคลิก Add Vendor ก่อน";
            SetMessage();

            return;
        }

        ConfirmAddNoteHeadText = "Confirm";
        ConfirmAddNoteMessage = "Confirm Add Note?";
        SetConfirmAddNote();
    }

    protected void btnTabNoteClear_Click(object sender, EventArgs e)
    {
        TabNote_SetDefault();
    }

    protected void SetConfirmAddNote()
    {
        PopupTabNoteConfirmAdd.HeaderText = ConfirmAddNoteHeadText;
        lblPopupTabNoteConfirmAdd.Text = ConfirmAddNoteMessage;
        PopupTabNoteConfirmAdd.ShowOnPageLoad = true;
    }

    protected int GetNoteSequence(string vendorCode)
    {
        string sql = "SELECT ISNULL(MAX(P14NSQ), 0) + 1 AS MaxSeq FROM AS400DB01.ILOD0001.ILMS14 WITH (NOLOCK) WHERE P14VEN = " + vendorCode;

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        DataTable dt = new DataTable();
        int maxSeq = 0;
        if (cookiesStorage.check_dataset(ds))
        {
            dt = ds.Tables[0];
            maxSeq = Convert.ToInt32(dt.Rows[0]["MaxSeq"].ToString());
        }
        return maxSeq;
    }

    protected List<SqlCommandText> GenerateAddNoteSQL()
    {
        m_userInfo = userInfoService.GetUserInfo();
        //CALL_PROCEDURES_ILSR97();
        dataCenter = new DataCenter(m_userInfo);
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;
        string currentTime = DateTime.Now.ToString("HHmmss", _dateThai);
        string workStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        int noteSeq = GetNoteSequence(vendorCode);
        string memo = txtTabNote_Memo.Text.Trim();
        int memoLength = memo.Length;
        int memoLine = memoLength / 70;
        int memoRemainer = memoLength % 70;

        if (memoRemainer > 0)
        {
            memoLine++;
        }

        List<string> notes = new List<string>();
        for (int i = 0; i < memoLine; i++)
        {
            int index = i * 70;
            int places = 70;
            if ((i == memoLine - 1) && memoRemainer > 0)
            {
                places = memoRemainer;
            }

            string noteText = memo.Substring(index, places);
            notes.Add(noteText);
        }

        List<SqlCommandText> sqlList = new List<SqlCommandText>();
        int seq = 1;
        foreach (var note in notes)
        {
            string sql = @"INSERT INTO AS400DB01.ILOD0001.ILMS14
                            (
                                P14VEN,
                                P14NSQ,
                                P14LSQ,
                                P14NOT,
                                P14UDT,
                                P14UTM,
                                P14UUS,
                                P14UPG,
                                P14UWS
                            ) VALUES (
                                " + vendorCode + @",
                                " + noteSeq.ToString() + @",
                                " + seq.ToString() + @",
                                '" + note.Replace("'", "''") + @"',
                                " + currentDate + @",
                                " + currentTime + @",
                                '" + m_userInfo.Username.ToString() + @"',
                                '" + ProgramName + @"',
                                '" + workStation + @"'
                            )";

            SqlCommandText sqlILMS14 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Insert Note"
            };
            sqlList.Add(sqlILMS14);
            noteSeq++;
        }

        return sqlList;
    }

    protected void btnPopupTabNoteConfirmAddOK_Click(object sender, EventArgs e)
    {
        MessageHeaderText = "Add Note";
        try
        {
            List<SqlCommandText> sqlAddNote = GenerateAddNoteSQL();
            InsertNote(sqlAddNote);
            TabNote_SetDefault();
        }
        catch (Exception ex)
        {
            Message = "Add Note Not Complete";
            SetMessage();
        }
    }

    protected void InsertNote(List<SqlCommandText> sqlList)
    {
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        iDB2Command cmd = new iDB2Command();
        bool transaction = true;
        //if (dataCenter.SqlCon.State != ConnectionState.Open)
        //    dataCenter.SqlCon.Open();
        //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();

        try
        {
            foreach (SqlCommandText sql in sqlList)
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                cmd.CommandText = sql.QueryString;
                int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                if (resHome11 == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Add Note Not Complete.";
                    SetMessage();
                    return;
                }
            }

            dataCenter.CommitMssql();
            Message = "Add Note Complete.";
            SetMessageSuccess();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Add Note Not Complete.";
            SetMessage();
            return;
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            cmd.Parameters.Clear();
        }
    }
    #endregion

    #region Add/Edit
    protected void rdoVendorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultForm();
        ResetTab();
        SetDefaultTabAddress();
        SetDefaultTabOther();
        SetDefatultPersonToContact();
        SetDefaultTabNote();

        string addType = rdoVendorType.SelectedValue;
        if (addType == "NV")
        {
            btnAddVendorHead.Visible = false;
        }
        else
        {
            btnAddVendorHead.Visible = true;
        }
    }

    protected void btnAddClick(object sender, EventArgs e)
    {
        MessageHeaderText = "Validate";

        #region 

        if (string.IsNullOrEmpty(txtVendorHeadCode.Text) && rdoVendorType.SelectedValue == "NBV")
        {
            Message = "Please Select Vendor (Head)";
            SetMessage();
            return;
        }

        if (string.IsNullOrEmpty(txtTitleCode.Text))
        {
            Message = "Please Select Title Code";
            SetMessage();
            return;
        }

        string vendorType = rdoVendorType.SelectedValue;
        if (string.IsNullOrEmpty(vendorType) && (Mode == "Add"))
        {
            Message = "Please Select Vendor Type";
            SetMessage();
            rdoVendorType.Focus();
            return;
        }

        string vendorHeadCode = txtVendorHeadCode.Text.Trim().Replace("-", "");
        if (string.IsNullOrEmpty(vendorHeadCode))
        {
            vendorHeadCode = "0";
        }

        string thaiName = txtThaiName.Text.Trim();
        if (string.IsNullOrEmpty(thaiName))
        {
            Message = "Please Input Thai Name";
            SetMessage();
            txtThaiName.Focus();
            return;
        }

        string englishName = txtEnglishName.Text.Trim();
        if (string.IsNullOrEmpty(englishName))
        {
            Message = "Please Input English Name";
            SetMessage();
            txtEnglishName.Focus();
            return;
        }
        #endregion

        #region Tab Registration Address
        string registrationAddress = txtTabAddress_RegistrationAddress_Address.Text.Trim();
        if (string.IsNullOrEmpty(registrationAddress))
        {
            Message = "Tab Address >> Registration Address : Please Input Address";
            SetMessage();
            return;
        }

        if (ddlTabAddress_RegistrationAddress_Province.SelectedIndex == 0)
        {
            Message = "Tab Address >> Registration Address : Please Select Province";
            SetMessage();
            return;
        }

        if (ddlTabAddress_RegistrationAddress_Amphur.SelectedIndex == 0)
        {
            Message = "Tab Address >> Registration Address : Please Select Amphur";
            SetMessage();
            return;
        }

        if (ddlTabAddress_LocationAddress_District.SelectedIndex == 0)
        {
            Message = "Tab Address >> Registration Address : Please Select District";
            SetMessage();
            return;
        }

        string registrationPostCode = txtTabAddress_RegistrationAddress_PostCode.Text.Trim();
        if (string.IsNullOrEmpty(registrationPostCode))
        {
            Message = "Tab Address >> Registration Address : Please Input Post Code";
            SetMessage();
            return;
        }
        #endregion

        #region Tab Location Address
        string locationAddress = txtTabAddress_LocationAddress_Address.Text.Trim();
        if (string.IsNullOrEmpty(locationAddress))
        {
            Message = "Tab Address >> Location Address : Please Input Address";
            SetMessage();
            return;
        }

        if (ddlTabAddress_LocationAddress_Province.SelectedIndex == 0)
        {
            Message = "Tab Address >> Location Address : Please Select Province";
            SetMessage();
            return;
        }

        if (ddlTabAddress_LocationAddress_Amphur.SelectedIndex == 0)
        {
            Message = "Tab Address >> Location Address : Please Select Amphur";
            SetMessage();
            return;
        }

        if (ddlTabAddress_LocationAddress_District.SelectedIndex == 0)
        {
            Message = "Tab Address >> Location Address : Please Select District";
            SetMessage();
            return;
        }

        string locationPostCode = txtTabAddress_LocationAddress_PostCode.Text.Trim();
        if (string.IsNullOrEmpty(locationPostCode))
        {
            Message = "Tab Address >> Location Address : Please Input Post Code";
            SetMessage();
            return;
        }
        #endregion

        #region Tab  Other Information        
        ValidationDate validationDate;
        //CALL_PROCEDURES_ILSR97();
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;

        string mouDate = txtTabOther_Information_MOUDate.Text.Replace("/", "").Trim();
        if (string.IsNullOrEmpty(mouDate))
        {
            Message = "Tab Other >> Information : Please Input MOU Date";
            SetMessage();
            return;
        }
        else
        {
            validationDate = new ValidationDate();
            validationDate = ValidateDate(mouDate);
            if (mouDate == "00000000")
            {
                validationDate.ValidDate = true;
            }

            if (!validationDate.ValidDate)
            {
                Message = "Tab Other >> Information : วันที่ MOU Date ไม่ถูกต้อง";
                SetMessage();
                return;
            }
        }

        string tempInformationJoinDate = "";
        string informationJoinDate = txtTabOther_Information_JoinDate.Text.Replace("/", "");
        validationDate = new ValidationDate();
        validationDate = ValidateDate(informationJoinDate);
        if (!validationDate.ValidDate)
        {
            Message = "Tab Other >> Information : วันที่ Join Date ไม่ถูกต้อง";
            SetMessage();
            return;
        }
        else
        {
            tempInformationJoinDate = validationDate.Date.ToString("yyyyMMdd", _dateThai);
        }

        int informationJoinDateInt = Convert.ToInt32(tempInformationJoinDate);
        /* Req.88652 ตัดเงื่อนไขเช็ค Join Date
         * int currentDateInt = Convert.ToInt32(currentDate);
        if (informationJoinDateInt > currentDateInt)
        {
            Message = "Tab Other >> Information : Join date < Current Date Please Check again.";
            SetMessage();
            return;
        }*/

        string tempInformationExpireDate = "";
        string informationExpireDate = txtTabOther_Information_ExpireDate.Text.Replace("/", "");
        validationDate = new ValidationDate();
        if (informationExpireDate == "99999999")
        {
            validationDate.ValidDate = true;
            tempInformationExpireDate = "99999999";
        }
        else
        {
            validationDate = ValidateDate(informationExpireDate);

            if (!validationDate.ValidDate)
            {
                Message = "Tab Other >> Information : วันที่ Expire Date ไม่ถูกต้อง";
                SetMessage();
                return;
            }
            else
            {
                tempInformationExpireDate = validationDate.Date.ToString("yyyyMMdd", _dateThai);
            }
        }

        int locationExpireDateInt = Convert.ToInt32(tempInformationExpireDate);
        if (informationJoinDateInt > locationExpireDateInt)
        {
            Message = "Tab Other >> Information : Join date > Eexpire date Please Check again.";
            SetMessage();
            return;
        }

        //if(ddlTabOther_Information_StatusFaxForOper.SelectedIndex==0)
        //{
        //    Message = "Tab Other >> Information : Please Check Status Fax for Oper !!!";
        //    SetMessage();
        //    return;
        //}

        //if (ddlTabOther_Information_StatusFaxForISB.SelectedIndex == 0)
        //{
        //    Message = "Tab Other >> Information : Please Check Status Fax for ISB !!!";
        //    SetMessage();
        //    return;
        //}

        //if (ddlTabOther_Information_FaxForHOForOper.SelectedIndex == 0)
        //{
        //    Message = "Tab Other >> Information : Please Check Fax for HO Oper !!!";
        //    SetMessage();
        //    return;
        //}

        //if(ddlTabOther_Information_FaxForHOForISB.SelectedIndex == 0)
        //{
        //    Message = "Tab Other >> Information : Please Check Fax for HO ISB !!!";
        //    SetMessage();
        //    return;
        //}

        //if(ddlTabOther_Information_AutoFax.SelectedIndex==0)
        //{
        //    Message = "Tab Other >> Information : Please Check Auto Fax (A --> Auto, M = Manual) !!!";
        //    SetMessage();
        //    return;
        //}

        /*** Call Stored Procedure For Validate Phone Number
        SetupNumericEntry;
        { Procedure }

        Set_Format_All_Tel;

        If EP10TR2.Text = '' then
            tmpP10TR2 := ''
        else
            tmpP10TR2:= EP10TR2.Text;

        if EP10TE2.text = '' then
            tmpP10TE2 := ''
        else
            tmpP10TE2:= EP10TE2.Text;

        if Not Check_All_Phone(ETelNo.Text, 'P') then
             Exit;

        if Not Check_All_Phone(EP10TE2.Text, 'P') then
             Exit;

        if Not Check_All_Phone(EFaxNo1.Text, 'P') then
             Exit;

        if Not Check_All_Phone(EFaxNo2.Text, 'P') then
             Exit;
        */
        #endregion

        #region Tab Other Payment
        if (rdoTabOther_Payment_SignBeforeRecDoc.SelectedIndex == -1)
        {
            Message = "Tab Other >> Payment : Please Select Sign before rec. doc!";
            SetMessage();
            return;
        }

        if (ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedIndex == 0)
        {
            Message = "Tabl Other >> Payment : Please Select Calendary Day or Buisness Day!";
            SetMessage();
            return;
        }

        string paymentBranchCode = txtTabOther_Payment_BranchCode.Text.Trim();
        if (string.IsNullOrEmpty(paymentBranchCode) || paymentBranchCode == "0")
        {
            Message = "Tab Other >> Payment : Please Select Pay to Vendor";
            SetMessage();
            return;
        }

        if (ddlTabOther_Payment_SpecialLoan.SelectedIndex == 0)
        {
            Message = "Tab Other >> Payment : Please Select Special Loan.!";
            SetMessage();
            return;
        }

        string vendorCode = txtVendorCode.Text.Trim();
        string paymentSpecialLoan = ddlTabOther_Payment_SpecialLoan.SelectedValue;
        if (!string.IsNullOrEmpty(vendorCode))
        {
            string sql = @"SELECT	P1BRN, P1APNO, P1LTYP, P1APPT, P1APVS, P1SAPC, P1APDT, P1PBCD, P1PBRN, P1PATY, 
								    P1PANO, P1PAYT, P1VDID, P1MKID, P1CAMP, P1CMSQ, P1ITEM, P1PDGP, P1PRIC, P1QTY, 
								    P1PURC, P1VATR, P1VATA, P1DOWN, P1DISC, P1TERM, P1RANG, P1NDUE, P1LNDR, P1DUTR, 
								    P1INFR, P1INTR, P1CRUR, P1PRAM, P1INTA, P1CRUA, P1INFA, P1DUTY, P1DIFF, P1COAM, 
								    P1FDAM, P1FRTM, P1FRDT, P1FRAM, P1APRJ, P1STDT, P1STTM, P1AVDT, P1AVTM, P1CONT, 
								    P1CNDT, P1FDUE, P1CSNO, P1LOCA, P1CRCD, P1AUTH, P1RESN, P1DCCD, P1DOCR, P1CONC, 
								    P1COOT, P1KUSR, P1KDTE, P1KTIM, P1FAX, P1FILL, P1UPDT, P1UPTM, P1UPUS, P1PROG, 
								    P1WSID, P1RSTS
                            FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                            WHERE
	                            P1VDID = " + vendorCode.Replace("-", "") + @"
                            AND P1APRJ = 'AP'";

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet ds = new DataSet();
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            if (cookiesStorage.check_dataset(ds))
            {
                sql = @"SELECT	P10VEN, P10TIC, P10TNM, P10NAM, P10ADR, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO, P10SOI, 
						        P10ROD, P10MOO, P10TMC, P10AMC, P10PVC, P10ZIP, P10AD2, P10VI2, P10BI2, P10BU2, P10RM2, 
						        P10FL2, P10SO2, P10RD2, P10MO2, P10TM2, P10AM2, P10PV2, P10ZI2, P10A31, P10A32, P10A33, 
						        P10STS, P10GRD, P10MOU, P10REG, P10TAX, P10TE1, P10TLR, P10EXT, P10TE2, P10TR2, P10EX2, 
						        P10FX1, P10F1T, P10FX2, P10F2T, P10RES, P10POT, P10RDP, P10RE2, P10PO2, P10RP2, P10FJD, 
						        P10JDT, P10EDT, P10PVN, P10BPY, P10TXR, P10PYE, P10PTY, P10BCD, P10BNO, P10BRG, P10DLV, 
						        P10HED, P10DTX, P10SFG, P10CLD, P10RF1, P10CRD, P10MKD, P10TAV, P10LTM, P10SAL, P10CTY, 
						        P10DTY, P10CPD, P10BRN, P10DT1, P10DT2, P10FI1, P10FIL, P10UPD, P10TIM, P10USR, P10PGM, 
						        P10DSP, P10DEL, P10ATS, P10FIX, P10SPC
                        FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                        WHERE
	                        P10VEN = " + vendorCode.Replace("-", "") + @"
                        AND	P10DEL = ''";

                ds = new DataSet();
                ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

                if (cookiesStorage.check_dataset(ds))
                {
                    DataRow dr = ds.Tables[0]?.Rows[0];
                    string spcialLoan = dr["P10SPC"].ToString().Trim();

                    if ((spcialLoan == "Y") && (paymentSpecialLoan != "Y") || (spcialLoan != "Y") && (paymentSpecialLoan == "Y"))
                    {
                        Message = "Tabl Other >> Payment : Vendor " + vendorCode + " มี Application ที่ผ่านการ Approve แล้วไม่สามารถเปลี่ยนแปลง Special Loan Type ได้";
                        SetMessage();
                        return;
                    }

                }
            }
        }

        if ((paymentSpecialLoan == "Y") || (paymentSpecialLoan == "F"))
        {
            ddlTabOther_Payment_PaymentType.SelectedIndex = 0;
            txtTabOther_Payment_BankAccNo.Text = "";
            ddlTabOther_Payment_BankCode.SelectedIndex = 0;
            ddlTabOther_Payment_BankRegion.SelectedIndex = 0;
            ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;
            txtTabOther_Payment_CreditDays.Text = "0";
            txtTabOther_Payment_Marketing.Text = "";
        }
        else
        {
            if (ddlTabOther_Payment_PaymentType.SelectedIndex == 0)
            {
                Message = "Tab Other >> Payment : Please select Payment type.!";
                SetMessage();
                return;
            }

            if ((ddlTabOther_Payment_PaymentType.SelectedIndex != 2) && ddlTabOther_Payment_BankRegion.SelectedIndex == 0)
            {
                Message = "Tab Other >> Payment : Please Check Bank Region again??????";
                SetMessage();
                return;
            }

            if ((ddlTabOther_Payment_BankCode.SelectedIndex == 0) && (ddlTabOther_Payment_PaymentType.SelectedIndex != 2))
            {
                Message = "Tabl Other >> Payment : Please select Bank code.!";
                SetMessage();
                return;
            }

            if ((ddlTabOther_Payment_PaymentType.SelectedIndex == 2) && (ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex == 0))
            {
                Message = "Tabl Other >> Payment : Please select Vendor Delivery CTB CHQ.!";
                SetMessage();
                return;
            }
            else
            {
                if ((ddlTabOther_Payment_PaymentType.SelectedIndex != 1))
                {
                    ddlTabOther_Payment_DeliveryCTBCHQ.SelectedIndex = 0;
                }
            }

            string paymentCreditDays = txtTabOther_Payment_CreditDays.Text.Trim();
            if (string.IsNullOrEmpty(paymentCreditDays))
            {
                Message = "Tabl Other >> Payment : Please Input Credit Day.!";
                SetMessage();
                return;
            }

            string paymentMarketing = txtTabOther_Payment_Marketing.Text.Trim();
            if (string.IsNullOrEmpty(paymentMarketing))
            {
                Message = "Tabl Other >> Payment : Please Check Marketing Code.!";
                SetMessage();
                return;
            }
        }

        string paymentCLBranch = txtTabOther_Payment_CLBranch.Text.Trim();
        if (string.IsNullOrEmpty(paymentCLBranch))
        {
            Message = "Tab Other >> Payment : Please Check CL Branch.!";
            SetMessage();
            return;
        }
        #endregion

        if (Mode == "Add")
        {
            ConfirmAddEditHeadText = "Confirm";
            ConfirmAddEditMessage = "Confirm Add " + rdoVendorType.SelectedItem.Text;
            SetConfirmAddEditMessage();
        }
        else
        {
            ConfirmAddEditHeadText = "Confirm";
            ConfirmAddEditMessage = "Confirm Edit Vendor : " + vendorCode;
            SetConfirmAddEditMessage();
        }
    }

    protected void btnClearDataClick(object sender, EventArgs e)
    {
        SetAddEditDefatult();
    }

    protected void AddNewVendor()
    {
        try
        {
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            string addType = rdoVendorType.SelectedValue;
            string locationProvince = ddlTabAddress_LocationAddress_Province.SelectedValue;
            locationProvince = Convert.ToInt32(locationProvince).ToString("D2");
            string paymentPayToVendor = txtTabOther_Payment_PayToVendor.Text.Trim();
            if (paymentPayToVendor.Length == 1)
            {
                paymentPayToVendor = Convert.ToInt32(paymentPayToVendor).ToString("D6");
            }
            else
            {
                //paymentPayToVendor = paymentPayToVendor.Substring(3, 6);
                paymentPayToVendor = paymentPayToVendor.Substring(2, 6);
            }

            string WIPMTY = "V";
            string WIADD = addType == "NV" ? "Y" : "N";
            string WIPROV = locationProvince;
            //edit defect DF-004 : 09/12/2563
            if (addType != "NV")
            {
                string reVendorHeader = txtVendorHeadCode.Text.Trim().Replace("-", "");
                paymentPayToVendor = reVendorHeader.Substring(2, 6);

            }
            //#
            string WIRUNN = paymentPayToVendor;
            string WOCODE = "";
            string WOERR = "";
            string WOEMSG = "";
            string userBizInit = m_userInfo.BizInit;
            string userBranchNo = m_userInfo.BranchNo;
            //W8
            bool canGetVendorCode = iLDataSubroutine.Call_ILSR10(WIPMTY, WIADD, WIPROV, WIRUNN, ref WOCODE, ref WOERR, ref WOEMSG);

            if (canGetVendorCode)
            {
                if (WOERR == "Y")
                {
                    dataCenter.RollbackMssql();
                    MessageHeaderText = "Error";
                    Message = WOEMSG;
                    SetMessage();
                }
                else
                {
                    txtVendorCode.Text = VendorCodeFormatter(WOCODE);

                    if (addType == "NV")
                    {
                        txtTabOther_Information_VendorHead.Text = txtVendorCode.Text;
                    }

                    /*** Tab Other Payment ***/
                    if (txtTabOther_Payment_PayToVendor.Text.Trim().Equals("0"))
                    {
                        txtTabOther_Payment_PayToVendor.Text = txtVendorCode.Text;
                    }

                    GenerateSQLForInsertMasterFileVendor(dataCenter);
                }
            }
            else
            {
                MessageHeaderText = "Error";
                Message = "Error calling Stored Procedure in ILSR10.";
                SetMessage();
                return;
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            MessageHeaderText = "Error";
            Message = "Error catch add new vendor : " + ex.Message;
            SetMessage();
            return;
        }
        finally
        {
            //ilObj.CloseConnectioDAL();
        }
    }

    public class SqlCommandText
    {
        public string QueryString { get; set; }
        public string Message { get; set; }
    }

    protected void GenerateSQLForInsertMasterFileVendor(DataCenter pILObj)
    {
        iDB2Command cmd = new iDB2Command();
        List<SqlCommandText> sqlList = new List<SqlCommandText>();
        m_userInfo = userInfoService.GetUserInfo();
        string currentUser = m_userInfo.Username;
        string workStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();
        string userBranchNo = m_userInfo.BranchNo;
        string userLoanType = "01";
        dataCenter = new DataCenter(m_userInfo);
        //CALL_PROCEDURES_ILSR97();
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;
        string currentTime = DateTime.Now.ToString("HHmmss", _dateThai);

        #region Get Value from Form
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        string vendorHead = txtVendorHeadCode.Text.Trim().Replace("-", "");
        string titleCode = txtTitleCode.Text.Trim();
        string thaiName = txtThaiName.Text.Trim().Replace("'", "''");
        string englishName = txtEnglishName.Text.Trim().Replace("'", "''");
        string branchNameEnglish = txtBranchNameEnglish.Text.Trim().Replace("'", "''");
        string vendorGrade = txtVendorGrade.Text.Trim().Replace("'", "''");
        string vendorRank = txtVendorRank.Text.Trim().Replace("'", "''");

        /*** Tab Registration Address ***/
        string registrationAddress = txtTabAddress_RegistrationAddress_Address.Text.Trim().Replace("'", "''");
        string registrationMoo = txtTabAddress_RegistrationAddress_Moo.Text.Trim().Replace("'", "''");
        string registrationVillage = txtTabAddress_RegistrationAddress_Village.Text.Trim().Replace("'", "''");
        string registrationBuildingType = ddlTabAddress_RegistrationAddress_BuildingType.SelectedValue;
        string registrationBuliding = txtTabAddress_RegistrationAddress_Building.Text.Trim().Replace("'", "''");
        string registrationRoom = txtTabAddress_RegistrationAddress_Room.Text.Trim().Replace("'", "''");
        string registrationFloor = txtTabAddress_RegistrationAddress_Floor.Text.Trim().Replace("'", "''");
        string registrationSoi = txtTabAddress_RegistrationAddress_Soi.Text.Trim().Replace("'", "''");
        string registrationRoad = txtTabAddress_RegistrationAddress_Road.Text.Trim().Replace("'", "''");
        string registrationProvince = ddlTabAddress_RegistrationAddress_Province.SelectedValue;
        string registrationAmphur = ddlTabAddress_RegistrationAddress_Amphur.SelectedValue;
        string registrationDistrict = ddlTabAddress_RegistrationAddress_District.SelectedValue;
        string registrationPostCode = txtTabAddress_RegistrationAddress_PostCode.Text.Trim().Replace("'", "''");

        /*** Tab Location Address ***/
        string locationAddress = txtTabAddress_LocationAddress_Address.Text.Trim().Replace("'", "''");
        string locationMoo = txtTabAddress_LocationAddress_Moo.Text.Trim().Replace("'", "''");
        string locationVillage = txtTabAddress_LocationAddress_Village.Text.Trim().Replace("'", "''");
        string locationBuildingType = ddlTabAddress_LocationAddress_BuildingType.SelectedValue;
        string locationBuilding = txtTabAddress_LocationAddress_Building.Text.Trim().Replace("'", "''");
        string locationRoom = txtTabAddress_LocationAddress_Room.Text.Trim().Replace("'", "''");
        string locationFloor = txtTabAddress_LocationAddress_Floor.Text.Trim().Replace("'", "''");
        string locationSoi = txtTabAddress_LocationAddress_Soi.Text.Trim().Replace("'", "''");
        string locationRoad = txtTabAddress_LocationAddress_Road.Text.Trim().Replace("'", "''");
        string locationProvince = ddlTabAddress_LocationAddress_Province.SelectedValue;
        string locationAmphur = ddlTabAddress_LocationAddress_Amphur.SelectedValue;
        string locationDistrict = ddlTabAddress_LocationAddress_District.SelectedValue;
        string locationPostCode = txtTabAddress_LocationAddress_PostCode.Text.Trim().Replace("'", "''");

        /*** Tab Address For Tax Invoice ***/
        string addressLine1 = txtTabAddress_AddressForTaxInvoice_Address1.Text.Trim().Replace("'", "''");
        string addressLine2 = txtTabAddress_AddressForTaxInvoice_Address2.Text.Trim().Replace("'", "''");
        string addressLine3 = txtTabAddress_AddressForTaxInvoice_Address3.Text.Trim().Replace("'", "''");

        /*** Tab Other : Information ***/
        string mouDate = txtTabOther_Information_MOUDate.Text.Replace("/", "").Replace("'", "''").Trim();
        mouDate = string.IsNullOrEmpty(mouDate) ? "" : mouDate.Substring(4, 4) + mouDate.Substring(2, 2) + mouDate.Substring(0, 2);
        string registerNo = txtTabOther_Information_RegisterNoTaxID.Text.Trim().Replace("'", "''");
        string taxId = txtTabOther_Information_TaxID.Text.Trim().Replace("'", "''");
        string joinDate = txtTabOther_Information_JoinDate.Text.Replace("/", "").Replace("'", "''").Trim();
        joinDate = string.IsNullOrEmpty(joinDate) ? "" : joinDate.Substring(4, 4) + joinDate.Substring(2, 2) + joinDate.Substring(0, 2);
        string expireDate = txtTabOther_Information_ExpireDate.Text.Replace("/", "").Replace("'", "''").Trim();
        expireDate = string.IsNullOrEmpty(expireDate) ? "" : expireDate.Substring(4, 4) + expireDate.Substring(2, 2) + expireDate.Substring(0, 2);
        string firstOpenDate = txtTabOther_Information_FirstOpenDate.Text.Replace("/", "").Replace("'", "''").Trim();
        firstOpenDate = string.IsNullOrEmpty(firstOpenDate) ? "" : firstOpenDate.Substring(4, 4) + firstOpenDate.Substring(2, 2) + firstOpenDate.Substring(0, 2);

        string telephoneNo1 = txtTabOther_Information_TelephoneNo1.Text.Trim().Replace("'", "''");
        string contactTelRange1 = txtTabOther_Information_ContactTelRange1.Text.Trim().Replace("'", "''");
        string extension1 = txtTabOther_Information_Extension1.Text.Trim().Replace("'", "''");
        string faxType1 = ddlTabOther_Information_FaxType1.SelectedValue;
        string faxNo1 = txtTabOther_Information_FaxNo1.Text.Trim().Replace("'", "''");
        faxNo1 = string.IsNullOrEmpty(faxNo1) ? "0" : faxNo1;

        string telephoneNo2 = txtTabOther_Information_TelephoneNo2.Text.Trim().Replace("'", "''");
        string contactTelRange2 = txtTabOther_Information_ContactTelRange2.Text.Trim().Replace("'", "''");
        string extension2 = txtTabOther_Information_Extension2.Text.Trim().Replace("'", "''");
        string faxType2 = ddlTabOther_Information_FaxType2.SelectedValue;
        string faxNo2 = txtTabOther_Information_FaxNo2.Text.Trim().Replace("'", "''");
        faxNo2 = string.IsNullOrEmpty(faxNo2) ? "0" : faxNo2;

        string faxOutBoundVendorHead = txtTabOther_Information_VendorHead.Text.Trim().Replace("'", "''").Replace("-", "");
        string statusFaxForOper = ddlTabOther_Information_StatusFaxForOper.SelectedValue;
        string statusFaxForISB = ddlTabOther_Information_StatusFaxForISB.SelectedValue;
        string faxForHOForOper = ddlTabOther_Information_FaxForHOForOper.SelectedValue;
        string faxForHOForISB = ddlTabOther_Information_FaxForHOForISB.SelectedValue;
        string autoFax = ddlTabOther_Information_AutoFax.SelectedValue;
        string autoSignLayBill = ddlTabOther_Information_AutoSignLayBill.SelectedValue;

        //Loop Reference

        /*** Tab Other Payment ***/
        string payToVendor = txtTabOther_Payment_PayToVendor.Text.Trim().Replace("'", "''").Replace("-", "");
        string branchCode = txtTabOther_Payment_BranchCode.Text.Trim().Replace("'", "''");
        string branchPayment = txtTabOther_Payment_BranchPayment.Text.Trim().Replace("'", "''");
        string paymentType = ddlTabOther_Payment_PaymentType.SelectedValue;
        string bankAccountNo = txtTabOther_Payment_BankAccNo.Text.Trim().Replace("'", "''");
        string bankCode = ddlTabOther_Payment_BankCode.SelectedValue;
        string bankRegion = ddlTabOther_Payment_BankRegion.SelectedValue;
        string vendorDeliveryCTBCHQ = ddlTabOther_Payment_DeliveryCTBCHQ.SelectedValue;
        string specialLoan = ddlTabOther_Payment_SpecialLoan.SelectedIndex == 0 ? "" : ddlTabOther_Payment_SpecialLoan.SelectedValue;
        string signBeforeRecDoc = rdoTabOther_Payment_SignBeforeRecDoc.SelectedValue;
        string calendaryOrBusinessDay = ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedValue;
        string creditDays = txtTabOther_Payment_CreditDays.Text.Trim().Replace("'", "''");
        creditDays = string.IsNullOrEmpty(creditDays) ? "0" : creditDays;
        string marketing = txtTabOther_Payment_Marketing.Text.Trim().Replace("'", "''");
        string timeAvailable = txtTabOther_Payment_TimeAvailable.Text.Trim().Replace("'", "''");
        string leadTime = txtTabOther_Payment_LeadTime.Text.Trim().Replace("'", "''");
        leadTime = string.IsNullOrEmpty(leadTime) ? "0" : leadTime;
        string clBranch = txtTabOther_Payment_CLBranch.Text.Trim().Replace("'", "''");
        string payeeName = txtTabOther_Payment_PayeeName.Text.Trim().Replace("'", "''");
        string commissionType = "1";
        #endregion

        #region Insert Master File Vendor
        string officeFlag = rdoVendorType.SelectedIndex == 0 ? "Y" : "N";

        string sql = $@"INSERT INTO AS400DB01.ILOD0001.ILMS10(
                                           P10VEN,
                                           P10TIC,
                                           P10TNM,
                                           P10NAM,
                                           P10FI1,
                                           P10ADR,
                                           P10MOO,
                                           P10VIL,
                                           P10BIL,
                                           P10BUD,
                                           P10ROM,
                                           P10FLO,
                                           P10SOI,
                                           P10ROD,
                                           P10TMC,
                                           P10AMC,
                                           P10PVC,
                                           P10ZIP,
                                           P10AD2,
                                           P10MO2,
                                           P10VI2,
                                           P10BI2,
                                           P10BU2,
                                           P10RM2,
                                           P10FL2,
                                           P10SO2,
                                           P10RD2,
                                           P10TM2,
                                           P10AM2,
                                           P10PV2,
                                           P10ZI2,
                                           P10A31,
                                           P10A32,
                                           P10A33,
                                           P10GRD,
                                           P10MOU,
                                           P10REG,
                                           P10TAX,
                                           P10TE1,
                                           P10TLR,
                                           P10EXT,
                                           P10TE2,
                                           P10TR2,
                                           P10EX2,
                                           P10FX1,
                                           P10F1T,
                                           P10FX2,
                                           P10F2T,
                                           P10FJD,
                                           P10JDT,
                                           P10EDT,
                                           P10PVN,
                                           P10BPY,
                                           P10PYE,
                                           P10PTY,
                                           P10BCD,
                                           P10BNO,
                                           P10BRG,
                                           P10DLV,
                                           P10HED,
                                           P10SFG,
                                           P10CLD,
                                           P10RF1,
                                           P10CRD,
                                           P10MKD,
                                           P10TAV,
                                           P10LTM,
                                           P10BRN,
                                           P10ATS,
                                           P10SPC,
                                           P10UPD,
                                           P10TIM,
                                           P10USR,
                                           P10PGM,
                                           P10DSP,
                                           P10CTY)
                                    VALUES(     
                                           {vendorCode},
                                           '{titleCode}',
                                           '{thaiName}',
                                           '{englishName}',
                                           '{branchNameEnglish}',
                                           '{registrationAddress}',
                                           '{registrationMoo}',
                                           '{registrationVillage}',
                                           '{registrationBuildingType}',
                                           '{registrationBuliding}',
                                           '{registrationRoom}',
                                           '{registrationFloor}',
                                           '{registrationSoi}',
                                           '{registrationRoad}',
                                           {registrationDistrict},
                                           {registrationAmphur},
                                           {registrationProvince},
                                           {registrationPostCode},
                                           '{locationAddress}',
                                           '{locationMoo}',
                                           '{locationVillage}',
                                           '{locationBuildingType}',
                                           '{locationBuilding}',
                                           '{locationRoom}',
                                           '{locationFloor}',
                                           '{locationSoi}',
                                           '{locationRoad}',
                                           {locationDistrict},
                                           {locationAmphur},
                                           {locationProvince},
                                           {locationPostCode},
                                           '{addressLine1}',
                                           '{addressLine2}',
                                           '{addressLine3}',
                                           '{vendorGrade}',
                                           {mouDate},
                                           '{registerNo}',
                                           '{taxId}',
                                           '{telephoneNo1}',
                                           '{contactTelRange1}',
                                           '{extension1}',
                                           '{telephoneNo2}',
                                           '{contactTelRange2}',
                                           '{extension2}',
                                           {faxNo1},
                                           '{faxType1}',
                                           {faxNo2},
                                           '{faxType2}',
                                           {firstOpenDate},
                                           {joinDate},
                                           {expireDate},
                                           {payToVendor},
                                           {branchCode},
                                           '{payeeName}',
                                           '{paymentType}',
                                           '{bankCode}',
                                           '{bankAccountNo}',
                                           '{bankRegion}',
                                           '{vendorDeliveryCTBCHQ}',
                                           '{officeFlag}',
                                           '{signBeforeRecDoc}',
                                           '{calendaryOrBusinessDay}',
                                           '{string.Empty}',
                                           {creditDays},
                                           '{marketing}',
                                           '{timeAvailable}',
                                           {leadTime},
                                           {m_userInfo.BranchNo},
                                           '{autoSignLayBill}',
                                           '{specialLoan}',
                                           {currentDate},
                                           {currentTime},
                                           '{currentUser}',
                                           '{ProgramName}',
                                           '{workStation}',
                                           '{commissionType}'
                                    )";

        //cmd.Parameters.Clear();
        //cmd.Parameters.Add("@P10VEN", vendorCode);
        //cmd.Parameters.Add("@P10TIC", titleCode);
        //cmd.Parameters.Add("@P10TNM", thaiName);
        //cmd.Parameters.Add("@P10NAM", englishName);
        //cmd.Parameters.Add("@P10FI1", branchNameEnglish);
        //cmd.Parameters.Add("@P10ADR", registrationAddress);
        //cmd.Parameters.Add("@P10MOO", registrationMoo);
        //cmd.Parameters.Add("@P10VIL", registrationVillage);
        //cmd.Parameters.Add("@P10BIL", registrationBuildingType);
        //cmd.Parameters.Add("@P10BUD", registrationBuliding);
        //cmd.Parameters.Add("@P10ROM", registrationRoom);
        //cmd.Parameters.Add("@P10FLO", registrationFloor);
        //cmd.Parameters.Add("@P10SOI", registrationSoi);
        //cmd.Parameters.Add("@P10ROD", registrationRoad);
        //cmd.Parameters.Add("@P10TMC", registrationDistrict);
        //cmd.Parameters.Add("@P10AMC", registrationAmphur);
        //cmd.Parameters.Add("@P10PVC", registrationProvince);
        //cmd.Parameters.Add("@P10ZIP", registrationPostCode);
        //cmd.Parameters.Add("@P10AD2", locationAddress);
        //cmd.Parameters.Add("@P10MO2", locationMoo);
        //cmd.Parameters.Add("@P10VI2", locationVillage);
        //cmd.Parameters.Add("@P10BI2", locationBuildingType);
        //cmd.Parameters.Add("@P10BU2", locationBuilding);
        //cmd.Parameters.Add("@P10RM2", locationRoom);
        //cmd.Parameters.Add("@P10FL2", locationFloor);
        //cmd.Parameters.Add("@P10SO2", locationSoi);
        //cmd.Parameters.Add("@P10RD2", locationRoad);
        //cmd.Parameters.Add("@P10TM2", locationDistrict);
        //cmd.Parameters.Add("@P10AM2", locationAmphur);
        //cmd.Parameters.Add("@P10PV2", locationProvince);
        //cmd.Parameters.Add("@P10ZI2", locationPostCode);
        //cmd.Parameters.Add("@P10A31", addressLine1);
        //cmd.Parameters.Add("@P10A32", addressLine2);
        //cmd.Parameters.Add("@P10A33", addressLine3);
        //cmd.Parameters.Add("@P10GRD", vendorGrade);
        //cmd.Parameters.Add("@P10MOU", mouDate);
        //cmd.Parameters.Add("@P10REG", registerNo);
        //cmd.Parameters.Add("@P10TAX", taxId);
        //cmd.Parameters.Add("@P10TE1", telephoneNo1);
        //cmd.Parameters.Add("@P10TLR", contactTelRange1);
        //cmd.Parameters.Add("@P10EXT", extension1);
        //cmd.Parameters.Add("@P10TE2", telephoneNo2);
        //cmd.Parameters.Add("@P10TR2", contactTelRange2);
        //cmd.Parameters.Add("@P10EX2", extension2);
        //cmd.Parameters.Add("@P10FX1", faxNo1);
        //cmd.Parameters.Add("@P10F1T", faxType1);
        //cmd.Parameters.Add("@P10FX2", faxNo2);
        //cmd.Parameters.Add("@P10F2T", faxType2);
        //cmd.Parameters.Add("@P10FJD", firstOpenDate);
        //cmd.Parameters.Add("@P10JDT", joinDate);
        //cmd.Parameters.Add("@P10EDT", expireDate);
        //cmd.Parameters.Add("@P10PVN", payToVendor);
        //cmd.Parameters.Add("@P10BPY", branchCode);
        //cmd.Parameters.Add("@P10PYE", payeeName);
        //cmd.Parameters.Add("@P10PTY", paymentType);
        //cmd.Parameters.Add("@P10BCD", bankCode);
        //cmd.Parameters.Add("@P10BNO", bankAccountNo);
        //cmd.Parameters.Add("@P10BRG", bankRegion);
        //cmd.Parameters.Add("@P10DLV", vendorDeliveryCTBCHQ);
        //cmd.Parameters.Add("@P10HED", officeFlag);
        //cmd.Parameters.Add("@P10SFG", signBeforeRecDoc);
        //cmd.Parameters.Add("@P10CLD", calendaryOrBusinessDay);
        //cmd.Parameters.Add("@P10RF1", string.Empty);
        //cmd.Parameters.Add("@P10CRD", creditDays);
        //cmd.Parameters.Add("@P10MKD", marketing);
        //cmd.Parameters.Add("@P10TAV", timeAvailable);
        //cmd.Parameters.Add("@P10LTM", leadTime);
        //cmd.Parameters.Add("@P10BRN", userInfo.BranchNo);
        //cmd.Parameters.Add("@P10ATS", autoSignLayBill);
        //cmd.Parameters.Add("@P10SPC", specialLoan);
        //cmd.Parameters.Add("@P10UPD", currentDate);
        //cmd.Parameters.Add("@P10TIM", currentTime);
        //cmd.Parameters.Add("@P10USR", currentUser);
        //cmd.Parameters.Add("@P10PGM", ProgramName);
        //cmd.Parameters.Add("@P10DSP", workStation);
        //cmd.Parameters.Add("@P10CTY", commissionType);

        SqlCommandText sqlILMS10 = new SqlCommandText
        {
            QueryString = sql,
            Message = "Insert Vendor Master File"
        };
        sqlList.Add(sqlILMS10);
        #endregion

        #region Insert Marketing & CL Branch
        sql = $@"INSERT INTO AS400DB01.ILOD0001.ILMS11(
                                                P11BRN,
                                                P11LTY,
                                                P11VEN,
                                                P11MKT,
                                                P11FIL,
                                                P11UPD,
                                                P11TIM,
                                                P11USR,
                                                P11PGM,
                                                P11DSP)
                                    VALUES(     
                                                {userBranchNo},
                                                '{userLoanType}',
                                                {vendorCode},
                                                '{marketing}',
                                                '{clBranch}',
                                                {currentDate},
                                                {currentTime},
                                                '{currentUser}',
                                                '{ProgramName}',
                                                '{workStation}'
                                    )";


        //cmd.Parameters.Clear();
        //cmd.Parameters.Add("@P11BRN", userBranchNo);
        //cmd.Parameters.Add("@P11LTY", userLoanType);
        //cmd.Parameters.Add("@P11VEN", vendorCode);
        //cmd.Parameters.Add("@P11MKT", marketing);
        //cmd.Parameters.Add("@P11FIL", clBranch);
        //cmd.Parameters.Add("@P11UPD", currentDate);
        //cmd.Parameters.Add("@P11TIM", currentTime);
        //cmd.Parameters.Add("@P11USR", currentUser);
        //cmd.Parameters.Add("@P11PGM", ProgramName);
        //cmd.Parameters.Add("@P11DSP", workStation);

        SqlCommandText sqlILMS11 = new SqlCommandText
        {
            QueryString = sql,
            Message = "Insert Marketing & CL Branch"
        };
        sqlList.Add(sqlILMS11);
        #endregion

        #region Backup Vendor Master File
        sql = @"INSERT INTO AS400DB01.ILOD0001.ILMS10HS 
                SELECT
                  " + currentDate + @",
                  " + currentTime + @",
                  '" + currentUser + @"',
                  '" + workStation + @"',
                  'N',
                  P10VEN, P10TIC, P10TNM, P10NAM, P10ADR, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO, P10SOI, P10ROD, P10MOO, P10TMC,
				  P10AMC, P10PVC, P10ZIP, P10AD2, P10VI2, P10BI2, P10BU2, P10RM2, P10FL2, P10SO2, P10RD2, P10MO2, P10TM2, P10AM2, 
				  P10PV2, P10ZI2, P10A31, P10A32, P10A33, P10STS, P10GRD, P10MOU, P10REG, P10TAX, P10TE1, P10TLR, P10EXT, P10TE2, 
				  P10TR2, P10EX2, P10FX1, P10F1T, P10FX2, P10F2T, P10RES, P10POT, P10RDP, P10RE2, P10PO2, P10RP2, P10FJD, P10JDT, 
				  P10EDT, P10PVN, P10BPY, P10TXR, P10PYE, P10PTY, P10BCD, P10BNO, P10BRG, P10DLV, P10HED, P10DTX, P10SFG, P10CLD, 
				  P10RF1, P10CRD, P10MKD, P10TAV, P10LTM, P10SAL, P10CTY, P10DTY, P10CPD, P10BRN, P10DT1, P10DT2, P10FI1, P10FIL, 
				  P10UPD, P10TIM, P10USR, P10PGM, P10DSP, P10DEL, P10ATS, P10FIX, P10SPC 
                FROM
                  AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                WHERE
                  P10VEN = " + vendorCode;
        SqlCommandText sqlILMS10HS = new SqlCommandText
        {
            QueryString = sql,
            Message = "Insert History Vendor Master File"
        };
        sqlList.Add(sqlILMS10HS);
        #endregion

        #region Insert & Update Data to GNTB11
        sql = "SELECT G11BRN, G11VEN, G11BUS, G11VHO, G11SFX, G11OHF, G11IFX, G11IHF, G11TFX FROM AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK) WHERE G11BRN = " + userBranchNo + @" AND G11VEN = '" + vendorCode + @"' AND G11BUS = 'IL' ";
        DataSet dsGNTB11 = new DataSet();
        dsGNTB11 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        bool isInsertGNTB11 = true;

        if (cookiesStorage.check_dataset(dsGNTB11))
        {
            isInsertGNTB11 = false;
        }

        if (isInsertGNTB11)
        {
            sql = @"INSERT INTO AS400DB01.GNOD0000.GNTB11 
                (
                    G11BRN,
                    G11VHO,
                    G11VEN,
                    G11BUS,
                    G11SFX,
                    G11OHF,
                    G11IFX,
                    G11IHF,
                    G11TFX
                ) 
                VALUES 
                (
                    " + userBranchNo + @",
                    '" + faxOutBoundVendorHead + @"',
                    '" + vendorCode + @"',
                    'IL',
                    '" + statusFaxForOper + @"',
                    '" + faxForHOForOper + @"',
                    '" + statusFaxForISB + @"',
                    '" + faxForHOForISB + @"',
                    '" + autoFax + @"'
                )";

            SqlCommandText sqlGNTB11 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Insert Fax Outbound"
            };
            sqlList.Add(sqlGNTB11);
        }
        else
        {
            sql = @"UPDATE
                      AS400DB01.GNOD0000.GNTB11 
                    SET
                      G11VHO = '" + faxOutBoundVendorHead + @"',
                      G11SFX = '" + statusFaxForOper + @"',
                      G11OHF = '" + faxForHOForOper + @"',
                      G11IFX = '" + statusFaxForISB + @"',
                      G11IHF = '" + faxForHOForISB + @"',
                      G11TFX = '" + autoFax + @"'
                    WHERE
                      G11BRN = " + userBranchNo + @" AND
                      G11VEN = '" + vendorCode + @"' AND
                      G11BUS = 'IL'";

            SqlCommandText sqlGNTB11 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Update Fax Outbound"
            };
            sqlList.Add(sqlGNTB11);
        }
        #endregion

        #region Insert Person to Contact (ILTB49)
        DataSet dsPersonToContact = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
        //DataSet dsPersonToContact = cookiesStorage.GetCookiesDataSetByKey("ds_person_to_contact");
        List<string> sqlAddPersonToContact = new List<string>();
        if (cookiesStorage.check_dataset(dsPersonToContact))
        {
            if (dsPersonToContact.Tables[0]?.Rows.Count > 0)
            {
                #region Get Running No
                sql = "SELECT ISNULL(MAX(T49RNO), 0) + 1 AS RUNNO FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK)";
                //ilObj.UserInfomation = m_userInfo;
                DataSet dsRuningNo = new DataSet();
                dsRuningNo = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                DataRow drRunningNo = dsRuningNo.Tables[0]?.Rows[0];
                string runningNo = drRunningNo["RUNNO"].ToString();
                #endregion

                int i = 1;
                DataTable dtPersonToContact = dsPersonToContact.Tables[0];
                foreach (DataRow dr in dtPersonToContact.Rows)
                {
                    string personPart = dr["T49PAR"].ToString().Trim();
                    string personType = dr["T49RTY"].ToString().Trim();
                    string personCode = dr["T49COD"].ToString().Trim();
                    string personSeq = i.ToString();

                    string personThaiName = dr["T49TNM"].ToString().Trim();
                    string personEnglishName = dr["T49ENM"].ToString().Trim();
                    string personDepartment = dr["T49DEP"].ToString().Trim();
                    string personMobilePhone = dr["T49HMB"].ToString().Trim();

                    string personTelephoneNo1 = dr["T49TE1"].ToString().Trim();
                    personTelephoneNo1 = string.IsNullOrEmpty(personTelephoneNo1) ? "0" : personTelephoneNo1;
                    string personContactTelRange1 = dr["T49TR1"].ToString().Trim();
                    personContactTelRange1 = string.IsNullOrEmpty(personContactTelRange1) ? "0" : personContactTelRange1;
                    string personExtension1 = dr["T49EX1"].ToString().Trim();
                    string personFaxNo1 = dr["T49FX1"].ToString().Trim();
                    personFaxNo1 = string.IsNullOrEmpty(personFaxNo1) ? "0" : personFaxNo1;

                    string personTelephoneNo2 = dr["T49TE2"].ToString().Trim();
                    personTelephoneNo2 = string.IsNullOrEmpty(personTelephoneNo2) ? "0" : personTelephoneNo2;
                    string personContactTelRange2 = dr["T49TR2"].ToString().Trim();
                    personContactTelRange2 = string.IsNullOrEmpty(personContactTelRange2) ? "0" : personContactTelRange2;
                    string personExtension2 = dr["T49EX2"].ToString().Trim();
                    string personFaxNo2 = dr["T49FX2"].ToString().Trim();
                    personFaxNo2 = string.IsNullOrEmpty(personFaxNo2) ? "0" : personFaxNo2;

                    string personTelephoneNo3 = dr["T49TE3"].ToString().Trim();
                    personTelephoneNo3 = string.IsNullOrEmpty(personTelephoneNo3) ? "0" : personTelephoneNo3;
                    string personContactTelRange3 = dr["T49TR3"].ToString().Trim();
                    personContactTelRange3 = string.IsNullOrEmpty(personContactTelRange3) ? "0" : personContactTelRange3;
                    string personExtension3 = dr["T49EX3"].ToString().Trim();
                    string personFaxNo3 = dr["T49FX3"].ToString().Trim();
                    personFaxNo3 = string.IsNullOrEmpty(personFaxNo3) ? "0" : personFaxNo3;

                    sql = $@"INSERT INTO AS400DB01.ILOD0001.ILTB49(
                                                T49PAR,
                                                T49RTY,
                                                T49COD,
                                                T49SEQ,
                                                T49RNO,
                                                T49TNM,
                                                T49ENM,
                                                T49DEP,
                                                T49TE1,
                                                T49TR1,
                                                T49EX1,
                                                T49FX1,
                                                T49TE2,
                                                T49TR2,
                                                T49EX2,
                                                T49FX2,
                                                T49TE3,
                                                T49TR3,
                                                T49EX3,
                                                T49FX3,
                                                T49HMB,
                                                T49UDD,
                                                T49UDT,
                                                T49USR,
                                                T49PGM,
                                                T49DSP)
                                    VALUES(     
                                                '{personPart}',
                                                '{personType}',
                                                {vendorCode},
                                                {personSeq},
                                                {runningNo},
                                                '{personThaiName}',
                                                '{personEnglishName}',
                                                '{personDepartment}',
                                                {personTelephoneNo1},
                                                {personContactTelRange1},
                                                '{personExtension1}',
                                                {personFaxNo1},
                                                {personTelephoneNo2},
                                                {personContactTelRange2},
                                                '{personExtension2}',
                                                {personFaxNo2},
                                                {personTelephoneNo3},
                                                {personContactTelRange3},
                                                '{personExtension3}',
                                                {personFaxNo3},
                                                '{personMobilePhone}',
                                                {currentDate},
                                                {currentTime},
                                                '{currentUser}',
                                                '{ProgramName}',
                                                '{workStation}'
                                    )";

                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@T49PAR", personPart);
                    //cmd.Parameters.Add("@T49RTY", personType);
                    //cmd.Parameters.Add("@T49COD", vendorCode);
                    //cmd.Parameters.Add("@T49SEQ", personSeq);
                    //cmd.Parameters.Add("@T49RNO", runningNo);
                    //cmd.Parameters.Add("@T49TNM", personThaiName);
                    //cmd.Parameters.Add("@T49ENM", personEnglishName);
                    //cmd.Parameters.Add("@T49DEP", personDepartment);
                    //cmd.Parameters.Add("@T49TE1", personTelephoneNo1);
                    //cmd.Parameters.Add("@T49TR1", personContactTelRange1);
                    //cmd.Parameters.Add("@T49EX1", personExtension1);
                    //cmd.Parameters.Add("@T49FX1", personFaxNo1);
                    //cmd.Parameters.Add("@T49TE2", personTelephoneNo2);
                    //cmd.Parameters.Add("@T49TR2", personContactTelRange2);
                    //cmd.Parameters.Add("@T49EX2", personExtension2);
                    //cmd.Parameters.Add("@T49FX2", personFaxNo2);
                    //cmd.Parameters.Add("@T49TE3", personTelephoneNo3);
                    //cmd.Parameters.Add("@T49TR3", personContactTelRange3);
                    //cmd.Parameters.Add("@T49EX3", personExtension3);
                    //cmd.Parameters.Add("@T49FX3", personFaxNo3);
                    //cmd.Parameters.Add("@T49HMB", personMobilePhone);
                    //cmd.Parameters.Add("@T49UDD", currentDate);
                    //cmd.Parameters.Add("@T49UDT", currentTime);
                    //cmd.Parameters.Add("@T49USR", currentUser);
                    //cmd.Parameters.Add("@T49PGM", ProgramName);
                    //cmd.Parameters.Add("@T49DSP", workStation);

                    sqlAddPersonToContact.Add(sql);
                    i++;

                    SqlCommandText sqlILTB49 = new SqlCommandText
                    {
                        QueryString = sql,
                        Message = "Insert Person to Contact"
                    };
                    sqlList.Add(sqlILTB49);
                }
            }
        }
        #endregion

        #region Insert Note
        string noteMemo = txtTabNote_Memo.Text.Trim().Replace("'", "''");
        if (!string.IsNullOrEmpty(noteMemo))
        {
            List<SqlCommandText> sqlAddNote = GenerateAddNoteSQL();
            sqlList.AddRange(sqlAddNote);
        }
        #endregion

        #region Update ILMS99 (Running Vendor Code)
        if (officeFlag.Equals("Y"))
        {
            sql = @"UPDATE
                AS400DB01.ILOD0001.ILMS99 
            SET
                P99RUN = P99RUN + 1,
                P99UPD = " + currentDate + @",
                P99TIM = " + currentTime + @"
            WHERE
                P99BRN = 0
                AND P99LNT = '  ' 
                AND P99REC = '010' ";

            SqlCommandText sqlILMS99 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Update Running Vendor Code"
            };
            sqlList.Add(sqlILMS99);
        }
        #endregion

        InsertMasterFileVendor(sqlList, pILObj, cmd);
    }

    protected void InsertMasterFileVendor(List<SqlCommandText> sqlList, DataCenter pILObj, iDB2Command cmd)
    {
        //iDB2Command cmd = new iDB2Command();
        MessageHeaderText = rdoVendorType.SelectedItem.Text;
        //if (dataCenter.SqlCon.State != ConnectionState.Open)
        //    dataCenter.OpenConnectSQL();
        //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
        bool transaction = true;
        try
        {
            foreach (SqlCommandText sql in sqlList)
            {
                transaction = pILObj.Sqltr == null ? true : false;
                cmd.CommandText = sql.QueryString;
                int result = pILObj.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                if (result == -1)
                {
                    pILObj.RollbackMssql();
                    Message = string.Format("Error on {0}.\n Please check command!", sql.Message);
                    SetMessage();
                    return;
                }
            }

            pILObj.CommitMssql();
            Message = "Insert Vendor Code : " + txtVendorCode.Text.Trim() + " \nData Complete.";
            SetMessageSuccess();
            SetDefault();

            Timer1.Interval = 2000;
            Timer1.Enabled = true;
            return;
        }
        catch (Exception ex)
        {
            pILObj.RollbackMssql();
            Message = "Error catch Insert Master Vendor : " + ex.Message;
            SetMessage();
            return;
        }
        finally
        {
            pILObj.CloseConnectSQL();
            cmd.Parameters.Clear();
        }
    }

    protected void EditVendor()
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        string currentUser = m_userInfo.Username;
        string workStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();
        string userBranchNo = m_userInfo.BranchNo;
        string userLoanType = "01";
        dataCenter = new DataCenter(m_userInfo);
        //CALL_PROCEDURES_ILSR97();
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;
        string currentTime = DateTime.Now.ToString("HHmmss", _dateThai);

        List<SqlCommandText> sqlList = new List<SqlCommandText>();

        #region Get Value from Form
        string vendorCode = txtVendorCode.Text.Trim().Replace("-", "");
        string vendorHeadCode = txtVendorHeadCode.Text.Trim().Replace("'", "");
        string titleCode = txtTitleCode.Text.Trim();
        string thaiName = txtThaiName.Text.Trim().Replace("'", "''");
        string englishName = txtEnglishName.Text.Trim().Replace("'", "''");
        string branchNameEnglish = txtBranchNameEnglish.Text.Trim().Replace("'", "''");
        string vendorGrade = txtVendorGrade.Text.Trim().Replace("'", "''");
        string vendorRank = txtVendorRank.Text.Trim().Replace("'", "''");

        /*** Tab Registration Address ***/
        string registrationAddress = txtTabAddress_RegistrationAddress_Address.Text.Trim().Replace("'", "''");
        string registrationMoo = txtTabAddress_RegistrationAddress_Moo.Text.Trim().Replace("'", "''");
        string registrationVillage = txtTabAddress_RegistrationAddress_Village.Text.Trim().Replace("'", "''");
        string registrationBuildingType = ddlTabAddress_RegistrationAddress_BuildingType.SelectedValue;
        string registrationBuliding = txtTabAddress_RegistrationAddress_Building.Text.Trim().Replace("'", "''");
        string registrationRoom = txtTabAddress_RegistrationAddress_Room.Text.Trim().Replace("'", "''");
        string registrationFloor = txtTabAddress_RegistrationAddress_Floor.Text.Trim().Replace("'", "''");
        string registrationSoi = txtTabAddress_RegistrationAddress_Soi.Text.Trim().Replace("'", "''");
        string registrationRoad = txtTabAddress_RegistrationAddress_Road.Text.Trim().Replace("'", "''");
        string registrationProvince = ddlTabAddress_RegistrationAddress_Province.SelectedValue;
        string registrationAmphur = ddlTabAddress_RegistrationAddress_Amphur.SelectedValue;
        string registrationDistrict = ddlTabAddress_RegistrationAddress_District.SelectedValue;
        string registrationPostCode = txtTabAddress_RegistrationAddress_PostCode.Text.Trim().Replace("'", "''");

        /*** Tab Location Address ***/
        string locationAddress = txtTabAddress_LocationAddress_Address.Text.Trim().Replace("'", "''");
        string locationMoo = txtTabAddress_LocationAddress_Moo.Text.Trim().Replace("'", "''");
        string locationVillage = txtTabAddress_LocationAddress_Village.Text.Trim().Replace("'", "''");
        string locationBuildingType = ddlTabAddress_LocationAddress_BuildingType.SelectedValue;
        string locationBuilding = txtTabAddress_LocationAddress_Building.Text.Trim().Replace("'", "''");
        string locationRoom = txtTabAddress_LocationAddress_Room.Text.Trim().Replace("'", "''");
        string locationFloor = txtTabAddress_LocationAddress_Floor.Text.Trim().Replace("'", "''");
        string locationSoi = txtTabAddress_LocationAddress_Soi.Text.Trim().Replace("'", "''");
        string locationRoad = txtTabAddress_LocationAddress_Road.Text.Trim().Replace("'", "''");
        string locationProvince = ddlTabAddress_LocationAddress_Province.SelectedValue;
        string locationAmphur = ddlTabAddress_LocationAddress_Amphur.SelectedValue;
        string locationDistrict = ddlTabAddress_LocationAddress_District.SelectedValue;
        string locationPostCode = txtTabAddress_LocationAddress_PostCode.Text.Trim().Replace("'", "''");

        /*** Tab Address For Tax Invoice ***/
        string addressLine1 = txtTabAddress_AddressForTaxInvoice_Address1.Text.Trim().Replace("'", "''");
        string addressLine2 = txtTabAddress_AddressForTaxInvoice_Address2.Text.Trim().Replace("'", "''");
        string addressLine3 = txtTabAddress_AddressForTaxInvoice_Address3.Text.Trim().Replace("'", "''");

        /*** Tab Other : Information ***/
        string mouDate = txtTabOther_Information_MOUDate.Text.Trim().Replace("'", "''");
        mouDate = mouDate.Substring(6, 4) + mouDate.Substring(3, 2) + mouDate.Substring(0, 2);
        string registerNo = txtTabOther_Information_RegisterNoTaxID.Text.Trim().Replace("'", "''");
        string taxId = txtTabOther_Information_TaxID.Text.Trim().Replace("'", "''");
        string joinDate = txtTabOther_Information_JoinDate.Text.Trim().Replace("'", "''");
        joinDate = joinDate.Substring(6, 4) + joinDate.Substring(3, 2) + joinDate.Substring(0, 2);
        string expireDate = txtTabOther_Information_ExpireDate.Text.Trim().Replace("'", "''");
        expireDate = expireDate.Substring(6, 4) + expireDate.Substring(3, 2) + expireDate.Substring(0, 2);
        string firstOpenDate = txtTabOther_Information_FirstOpenDate.Text.Trim().Replace("'", "''");
        firstOpenDate = firstOpenDate.Substring(6, 4) + firstOpenDate.Substring(3, 2) + firstOpenDate.Substring(0, 2);

        string telephoneNo1 = txtTabOther_Information_TelephoneNo1.Text.Trim().Replace("'", "''");
        string contactTelRange1 = txtTabOther_Information_ContactTelRange1.Text.Trim().Replace("'", "''");
        string extension1 = txtTabOther_Information_Extension1.Text.Trim().Replace("'", "''");
        string faxType1 = ddlTabOther_Information_FaxType1.SelectedValue;
        string faxNo1 = txtTabOther_Information_FaxNo1.Text.Trim().Replace("'", "''");

        string telephoneNo2 = txtTabOther_Information_TelephoneNo2.Text.Trim().Replace("'", "''");
        string contactTelRange2 = txtTabOther_Information_ContactTelRange2.Text.Trim().Replace("'", "''");
        string extension2 = txtTabOther_Information_Extension2.Text.Trim().Replace("'", "''");
        string faxType2 = ddlTabOther_Information_FaxType2.SelectedValue;
        string faxNo2 = txtTabOther_Information_FaxNo2.Text.Trim().Replace("'", "''");

        string faxOutBoundVendorHead = txtTabOther_Information_VendorHead.Text.Trim().Replace("'", "''").Replace("-", "");
        string statusFaxForOper = ddlTabOther_Information_StatusFaxForOper.SelectedValue;
        string statusFaxForISB = ddlTabOther_Information_StatusFaxForISB.SelectedValue;
        string faxForHOForOper = ddlTabOther_Information_FaxForHOForOper.SelectedValue;
        string faxForHOForISB = ddlTabOther_Information_FaxForHOForISB.SelectedValue;
        string autoFax = ddlTabOther_Information_AutoFax.SelectedValue;
        string autoSignLayBill = ddlTabOther_Information_AutoSignLayBill.SelectedValue;

        //Loop Reference

        /*** Tab Other Payment ***/
        string payToVendor = txtTabOther_Payment_PayToVendor.Text.Trim().Replace("'", "''").Replace("-", "");
        string branchCode = txtTabOther_Payment_BranchCode.Text.Trim().Replace("'", "''");
        string branchPayment = txtTabOther_Payment_BranchPayment.Text.Trim().Replace("'", "''");
        string paymentType = ddlTabOther_Payment_PaymentType.SelectedValue;
        string bankAccountNo = txtTabOther_Payment_BankAccNo.Text.Trim().Replace("'", "''");
        string bankCode = ddlTabOther_Payment_BankCode.SelectedValue;
        string bankRegion = ddlTabOther_Payment_BankRegion.SelectedValue;
        string vendorDeliveryCTBCHQ = ddlTabOther_Payment_DeliveryCTBCHQ.SelectedValue;
        string specialLoan = ddlTabOther_Payment_SpecialLoan.SelectedIndex == 0 ? "" : ddlTabOther_Payment_SpecialLoan.SelectedValue;
        string signBeforeRecDoc = rdoTabOther_Payment_SignBeforeRecDoc.SelectedValue;
        string calendaryOrBusinessDay = ddlTabOther_Payment_CarlendaryOrBusinessDay.SelectedValue;
        string creditDays = txtTabOther_Payment_CreditDays.Text.Trim().Replace("'", "''");
        string marketing = txtTabOther_Payment_Marketing.Text.Trim().Replace("'", "''");
        string timeAvailable = txtTabOther_Payment_TimeAvailable.Text.Trim().Replace("'", "''");
        string leadTime = txtTabOther_Payment_LeadTime.Text.Trim().Replace("'", "''");
        string clBranch = txtTabOther_Payment_CLBranch.Text.Trim().Replace("'", "''");
        string payeeName = txtTabOther_Payment_PayeeName.Text.Trim().Replace("'", "''");
        #endregion

        #region Check/Update Master File
        sql = $@"Update AS400DB01.ILOD0001.ILMS10 SET
                  P10TIC = '{titleCode}',
                  P10TNM = '{thaiName}',
                  P10NAM = '{englishName}',
                  P10FI1 = '{branchNameEnglish}',
                  P10ADR = '{registrationAddress}',
                  P10BIL = '{registrationBuildingType}',
                  P10VIL = '{registrationVillage}',
                  P10ROM = '{registrationRoom}',
                  P10BI2 = '{locationBuildingType}',
                  P10VI2 = '{locationVillage}',
                  P10RM2 = '{locationRoom}',
                  P10MOO = '{registrationMoo}',
                  P10BUD = '{registrationBuliding}',
                  P10FLO = '{registrationFloor}',
                  P10SOI = '{registrationSoi}',
                  P10ROD = '{registrationRoad}',
                  P10TMC = {registrationDistrict},
                  P10AMC = {registrationAmphur},
                  P10PVC = {registrationProvince},
                  P10ZIP = {registrationPostCode},
                  P10AD2 = '{locationAddress}',
                  P10MO2 = '{locationMoo}',
                  P10BU2 = '{locationBuilding}',
                  P10FL2 = '{locationFloor}',
                  P10SO2 = '{locationSoi}',
                  P10RD2 = '{locationRoad}',
                  P10TM2 = {locationDistrict},
                  P10AM2 = {locationAmphur},
                  P10PV2 = {locationProvince},
                  P10ZI2 = {locationPostCode},
                  P10A31 = '{addressLine1}',
                  P10A32 = '{addressLine2}',
                  P10A33 = '{addressLine3}',
                  P10GRD = '{vendorGrade}',
                  P10MOU = {mouDate},
                  P10REG = '{registerNo}',
                  P10TAX = '{taxId}',
                  P10TE1 = '{telephoneNo1}',
                  P10TLR = '{contactTelRange1}',
                  P10EXT = '{extension1}',
                  P10TE2 = '{telephoneNo2}',
                  P10TR2 = '{contactTelRange2}',
                  P10EX2 = '{extension2}',
                  P10FX1 = {faxNo1},
                  P10F1T = '{faxType1}',
                  P10FX2 = {faxNo2},
                  P10F2T = '{faxType2}',
                  P10JDT = {joinDate},
                  P10EDT = {expireDate},
                  P10PVN = {payToVendor},
                  P10BPY = {branchCode},
                  P10PYE = '{payeeName}',
                  P10PTY = '{paymentType}',
                  P10BCD = '{bankCode}',
                  P10BNO = '{bankAccountNo}',
                  P10BRG = '{bankRegion}',
                  P10DLV = '{vendorDeliveryCTBCHQ}',
                  P10SFG = '{signBeforeRecDoc}',
                  P10CLD = '{calendaryOrBusinessDay}',
                  P10CRD = {creditDays},
                  P10MKD = '{marketing}',
                  P10TAV = '{timeAvailable}',
                  P10LTM = {leadTime},
                  P10ATS = '{autoSignLayBill}', 
                  P10SPC = '{specialLoan}',
                  P10UPD = {currentDate},
                  P10TIM = {currentTime},
                  P10USR = '{currentUser}',
                  P10PGM = '{ProgramName}',
                  P10DSP = '{workStation}' 
            WHERE P10VEN = " + vendorCode;

        //cmd.Parameters.Clear();
        //cmd.Parameters.AddWithValue("P10TIC", titleCode);
        //cmd.Parameters.AddWithValue("P10TNM", thaiName.Trim());
        //cmd.Parameters.AddWithValue("P10NAM", englishName);
        //cmd.Parameters.AddWithValue("P10FI1", branchNameEnglish);
        //cmd.Parameters.AddWithValue("P10ADR", registrationAddress);
        //cmd.Parameters.AddWithValue("P10BIL", registrationBuildingType);
        //cmd.Parameters.AddWithValue("P10VIL", registrationVillage);
        //cmd.Parameters.AddWithValue("P10ROM", registrationRoom);
        //cmd.Parameters.AddWithValue("P10BI2", locationBuildingType);
        //cmd.Parameters.AddWithValue("P10VI2", locationVillage);
        //cmd.Parameters.AddWithValue("P10RM2", locationRoom);
        //cmd.Parameters.AddWithValue("P10MOO", registrationMoo);
        //cmd.Parameters.AddWithValue("P10BUD", registrationBuliding);
        //cmd.Parameters.AddWithValue("P10FLO", registrationFloor);
        //cmd.Parameters.AddWithValue("P10SOI", registrationSoi);
        //cmd.Parameters.AddWithValue("P10ROD", registrationRoad);
        //cmd.Parameters.AddWithValue("P10TMC", registrationDistrict);
        //cmd.Parameters.AddWithValue("P10AMC", registrationAmphur);
        //cmd.Parameters.AddWithValue("P10PVC", registrationProvince);
        //cmd.Parameters.AddWithValue("P10ZIP", registrationPostCode);
        //cmd.Parameters.AddWithValue("P10AD2", locationAddress);
        //cmd.Parameters.AddWithValue("P10MO2", locationMoo);
        //cmd.Parameters.AddWithValue("P10BU2", locationBuilding);
        //cmd.Parameters.AddWithValue("P10FL2", locationFloor);
        //cmd.Parameters.AddWithValue("P10SO2", locationSoi);
        //cmd.Parameters.AddWithValue("P10RD2", locationRoad);
        //cmd.Parameters.AddWithValue("P10TM2", locationDistrict);
        //cmd.Parameters.AddWithValue("P10AM2", locationAmphur);
        //cmd.Parameters.AddWithValue("P10PV2", locationProvince);
        //cmd.Parameters.AddWithValue("P10ZI2", locationPostCode);
        //cmd.Parameters.AddWithValue("P10A31", addressLine1);
        //cmd.Parameters.AddWithValue("P10A32", addressLine2);
        //cmd.Parameters.AddWithValue("P10A33", addressLine3);
        //cmd.Parameters.AddWithValue("P10GRD", vendorGrade);
        //cmd.Parameters.AddWithValue("P10MOU", mouDate);
        //cmd.Parameters.AddWithValue("P10REG", registerNo);
        //cmd.Parameters.AddWithValue("P10TAX", taxId);
        //cmd.Parameters.AddWithValue("P10TE1", telephoneNo1);
        //cmd.Parameters.AddWithValue("P10TLR", contactTelRange1);
        //cmd.Parameters.AddWithValue("P10EXT", extension1);
        //cmd.Parameters.AddWithValue("P10TE2", telephoneNo2);
        //cmd.Parameters.AddWithValue("P10TR2", contactTelRange2);
        //cmd.Parameters.AddWithValue("P10EX2", extension2);
        //cmd.Parameters.AddWithValue("P10FX1", faxNo1);
        //cmd.Parameters.AddWithValue("P10F1T", faxType1);
        //cmd.Parameters.AddWithValue("P10FX2", faxNo2);
        //cmd.Parameters.AddWithValue("P10F2T", faxType2);
        //cmd.Parameters.AddWithValue("P10JDT", joinDate);
        //cmd.Parameters.AddWithValue("P10EDT", expireDate);
        //cmd.Parameters.AddWithValue("P10PVN", payToVendor);
        //cmd.Parameters.AddWithValue("P10BPY", branchCode);
        //cmd.Parameters.AddWithValue("P10PYE", payeeName);
        //cmd.Parameters.AddWithValue("P10PTY", paymentType);
        //cmd.Parameters.AddWithValue("P10BCD", bankCode);
        //cmd.Parameters.AddWithValue("P10BNO", bankAccountNo);
        //cmd.Parameters.AddWithValue("P10BRG", bankRegion);
        //cmd.Parameters.AddWithValue("P10DLV", vendorDeliveryCTBCHQ);
        //cmd.Parameters.AddWithValue("P10SFG", signBeforeRecDoc);
        //cmd.Parameters.AddWithValue("P10CLD", calendaryOrBusinessDay);
        //cmd.Parameters.AddWithValue("P10CRD", creditDays);
        //cmd.Parameters.AddWithValue("P10MKD", marketing);
        //cmd.Parameters.AddWithValue("P10TAV", timeAvailable);
        //cmd.Parameters.AddWithValue("P10LTM", leadTime);
        //cmd.Parameters.AddWithValue("P10ATS", autoSignLayBill);
        //cmd.Parameters.AddWithValue("P10SPC", specialLoan);
        //cmd.Parameters.AddWithValue("P10UPD", currentDate);
        //cmd.Parameters.AddWithValue("P10TIM", currentTime);
        //cmd.Parameters.AddWithValue("P10USR", currentUser);
        //cmd.Parameters.AddWithValue("P10PGM", ProgramName);
        //cmd.Parameters.AddWithValue("P10DSP", workStation);

        SqlCommandText sqlILMS10 = new SqlCommandText
        {
            QueryString = sql,
            Message = "Update Vendor Master File"
        };
        sqlList.Add(sqlILMS10);
        #endregion

        #region Backup Vendor Master File
        sql = @"INSERT INTO AS400DB01.ILOD0001.ILMS10HS 
                SELECT
                  " + currentDate + @",
                  " + currentTime + @",
                  '" + currentUser + @"',
                  '" + workStation + @"',
                  'E',
                  P10VEN, P10TIC, P10TNM, P10NAM, P10ADR, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO, P10SOI, P10ROD, P10MOO, P10TMC,
				  P10AMC, P10PVC, P10ZIP, P10AD2, P10VI2, P10BI2, P10BU2, P10RM2, P10FL2, P10SO2, P10RD2, P10MO2, P10TM2, P10AM2, 
				  P10PV2, P10ZI2, P10A31, P10A32, P10A33, P10STS, P10GRD, P10MOU, P10REG, P10TAX, P10TE1, P10TLR, P10EXT, P10TE2, 
				  P10TR2, P10EX2, P10FX1, P10F1T, P10FX2, P10F2T, P10RES, P10POT, P10RDP, P10RE2, P10PO2, P10RP2, P10FJD, P10JDT, 
				  P10EDT, P10PVN, P10BPY, P10TXR, P10PYE, P10PTY, P10BCD, P10BNO, P10BRG, P10DLV, P10HED, P10DTX, P10SFG, P10CLD, 
				  P10RF1, P10CRD, P10MKD, P10TAV, P10LTM, P10SAL, P10CTY, P10DTY, P10CPD, P10BRN, P10DT1, P10DT2, P10FI1, P10FIL, 
				  P10UPD, P10TIM, P10USR, P10PGM, P10DSP, P10DEL, P10ATS, P10FIX, P10SPC
                FROM
                  AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                WHERE
                  P10VEN = " + vendorCode;

        SqlCommandText sqlILMS10HS = new SqlCommandText
        {
            QueryString = sql,
            Message = "Insert History Vendor Master File"
        };
        sqlList.Add(sqlILMS10HS);
        #endregion

        #region Check/Update Marketing & CL Branch
        sql = @"UPDATE
                  AS400DB01.ILOD0001.ILMS11 
                SET
                  P11MKT = '" + marketing + @"', 
                  P11FIL = '" + clBranch + @"',
                  P11LTY = '" + userLoanType + @"' 
                WHERE
                  P11VEN = " + vendorCode;

        SqlCommandText sqlILMS11 = new SqlCommandText
        {
            QueryString = sql,
            Message = "Update Marketing & CL Branch"
        };
        sqlList.Add(sqlILMS11);
        #endregion

        #region Update GNTB11
        sql = $@"SELECT 
					G11BRN, G11VEN, G11BUS, G11VHO, G11SFX, G11OHF, G11IFX, G11IHF, G11TFX
                FROM 
					AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK)
                WHERE G11BRN = {userBranchNo} 
                AND G11VEN = '{vendorCode}'
                AND G11BUS = 'IL'";

        ilObj.UserInfomation = m_userInfo;
        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(ds))
        {
            sql = @"UPDATE
                      AS400DB01.GNOD0000.GNTB11 
                    SET
                      G11VHO = '" + faxOutBoundVendorHead + @"',
                      G11SFX = '" + statusFaxForOper + @"',
                      G11OHF = '" + faxForHOForOper + @"',
                      G11IFX = '" + statusFaxForISB + @"',
                      G11IHF = '" + faxForHOForISB + @"',
                      G11TFX = '" + autoFax + @"'
                    WHERE
                      G11BRN = " + userBranchNo + @"
                    AND G11VEN = '" + vendorCode + @"'
                    AND G11BUS = 'IL'";

            SqlCommandText sqlGNTB11 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Update Fax Outbound"
            };
            sqlList.Add(sqlGNTB11);
        }
        else
        {
            sql = @"INSERT INTO AS400DB01.GNOD0000.GNTB11
                    (
                      G11BRN,
                      G11VHO,
                      G11VEN,
                      G11BUS,
                      G11SFX,
                      G11OHF,
                      G11IFX,
                      G11IHF,
                      G11TFX
                    ) VALUES (
                      " + userBranchNo + @",
                      '" + vendorHeadCode + @"',
                      '" + vendorCode + @"',
                      'IL',
                      '" + statusFaxForOper + @"',
                      '" + faxForHOForOper + @"',
                      '" + statusFaxForISB + @"',
                      '" + faxForHOForISB + @"',
                      '" + autoFax + @"'
                    )";

            SqlCommandText sqlGNTB11 = new SqlCommandText
            {
                QueryString = sql,
                Message = "Insert Fax Outbound"
            };
            sqlList.Add(sqlGNTB11);
        }
        #endregion

        #region Update ILTB49 (Person to Contact)
        DataSet dsPersonToContact = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_person_to_contact.Value);
        //DataSet dsPersonToContact = cookiesStorage.GetCookiesDataSetByKey("ds_person_to_contact");
        if (cookiesStorage.check_dataset(dsPersonToContact))
        {
            if (dsPersonToContact.Tables[0]?.Rows.Count > 0)
            {
                #region Get Running No
                string runningNo = "";
                int seq = 0;

                ilObj.UserInfomation = m_userInfo;

                sql = "SELECT MAX(T49RNO) AS RUNNO FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK) WHERE T49COD = " + vendorCode;
                DataSet dsRuningNo = new DataSet();
                dsRuningNo = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                DataTable dtRunningNo = dsRuningNo.Tables[0];
                if (!string.IsNullOrEmpty(dtRunningNo.Rows[0]["RUNNO"].ToString()))  //if (dtRunningNo.Rows.Count > 0)
                {
                    runningNo = dtRunningNo.Rows[0]["RUNNO"].ToString();
                }
                else
                {
                    sql = "SELECT ISNULL(MAX(T49RNO), 0) + 1 AS RUNNO FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK)";
                    dsRuningNo = new DataSet();
                    dsRuningNo = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

                    DataRow drRunningNo = dsRuningNo.Tables[0]?.Rows[0];
                    runningNo = drRunningNo["RUNNO"].ToString();
                }

                sql = "SELECT ISNULL(MAX(T49SEQ), 0) AS SEQ FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK) WHERE T49COD = " + vendorCode;
                DataSet dsSeq = new DataSet();
                dsSeq = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                seq = Convert.ToInt32(dsSeq.Tables[0]?.Rows[0]["SEQ"].ToString());
                #endregion

                List<string> rowSeqForUpdate = new List<string>();
                List<string> rowRunNo = new List<string>();
                List<string> sqlUpdate = new List<string>();
                List<string> sqlInsert = new List<string>();
                DataTable dtPersonToContact = dsPersonToContact.Tables[0];
                foreach (DataRow dr in dtPersonToContact.Rows)
                {
                    #region Get Value from Form
                    string cThaiName = dr["T49TNM"].ToString().Trim();
                    string cEnglishName = dr["T49ENM"].ToString().Trim();
                    string cDepartment = dr["T49DEP"].ToString().Trim();

                    string cTelephoneNo1 = dr["T49TE1"].ToString().Trim();
                    cTelephoneNo1 = string.IsNullOrEmpty(cTelephoneNo1) ? "0" : cTelephoneNo1;
                    string cContactTelRange1 = dr["T49TR1"].ToString().Trim();
                    cContactTelRange1 = string.IsNullOrEmpty(cContactTelRange1) ? "0" : cContactTelRange1;
                    string cExtension1 = dr["T49EX1"].ToString().Trim();
                    string cFaxNo1 = dr["T49FX1"].ToString().Trim();
                    cFaxNo1 = string.IsNullOrEmpty(cFaxNo1) ? "0" : cFaxNo1;

                    string cTelephoneNo2 = dr["T49TE2"].ToString().Trim();
                    cTelephoneNo2 = string.IsNullOrEmpty(cTelephoneNo2) ? "0" : cTelephoneNo2;
                    string cContactTelRange2 = dr["T49TR2"].ToString().Trim();
                    cContactTelRange2 = string.IsNullOrEmpty(cContactTelRange2) ? "0" : cContactTelRange2;
                    string cExtension2 = dr["T49EX2"].ToString().Trim();
                    string cFaxNo2 = dr["T49FX2"].ToString().Trim();
                    cFaxNo2 = string.IsNullOrEmpty(cFaxNo2) ? "0" : cFaxNo2;

                    string cTelephoneNo3 = dr["T49TE3"].ToString().Trim();
                    cTelephoneNo3 = string.IsNullOrEmpty(cTelephoneNo3) ? "0" : cTelephoneNo3;
                    string cContactTelRange3 = dr["T49TR3"].ToString().Trim();
                    cContactTelRange3 = string.IsNullOrEmpty(cContactTelRange3) ? "0" : cContactTelRange3;
                    string cExtension3 = dr["T49EX3"].ToString().Trim();
                    string cFaxNo3 = dr["T49FX3"].ToString().Trim();
                    cFaxNo3 = string.IsNullOrEmpty(cFaxNo3) ? "0" : cFaxNo3;

                    string cMobilePhone = dr["T49HMB"].ToString().Trim();
                    string cRunNo = dr["T49RNO"].ToString().Trim();
                    string cPart = dr["T49PAR"].ToString().Trim();
                    string cSeq = dr["T49SEQ"].ToString().Trim();
                    #endregion

                    if (cPart == "V")
                    {
                        rowSeqForUpdate.Add(cSeq);
                        rowRunNo.Add(cRunNo);

                        sql = @"UPDATE
                                  AS400DB01.ILOD0001.ILTB49
                                SET
                                  --T49SEQ = " + cSeq + @",
                                  T49TNM = '" + cThaiName + @"',
                                  T49ENM = '" + cEnglishName + @"',
                                  T49DEP = '" + cDepartment + @"',
                                  T49TE1 = " + cTelephoneNo1 + @",
                                  T49TR1 = " + cContactTelRange1 + @",
                                  T49EX1 = '" + cExtension1 + @"',
                                  T49FX1 = " + cFaxNo1 + @",
                                  T49TE2 = " + cTelephoneNo2 + @",
                                  T49TR2 = " + cContactTelRange2 + @",
                                  T49EX2 = '" + cExtension2 + @"',
                                  T49FX2 = " + cFaxNo2 + @",
                                  T49TE3 = " + cTelephoneNo3 + @",
                                  T49TR3 = " + cContactTelRange3 + @",
                                  T49EX3 = '" + cExtension3 + @"',
                                  T49FX3 = " + cFaxNo3 + @",
                                  T49HMB = '" + cMobilePhone + @"',
                                  T49UDD = " + currentDate + @",
                                  T49UDT = " + currentTime + @",
                                  T49USR = '" + currentUser + @"',
                                  T49PGM = '" + ProgramName + @"',
                                  T49DSP = '" + workStation + @"'
                                WHERE
                                  T49PAR = 'V'
                                AND T49RTY = 'C'
                                AND T49COD = " + vendorCode + @"
                                AND T49RNO = " + cRunNo + @"
                                AND T49SEQ = " + cSeq;

                        SqlCommandText sqlILTB49 = new SqlCommandText
                        {
                            QueryString = sql,
                            Message = "Update Person to Contact"
                        };
                        sqlList.Add(sqlILTB49);
                    }
                    else
                    {
                        seq++;
                        sql = @"INSERT INTO AS400DB01.ILOD0001.ILTB49
                                (
                                  T49PAR,
                                  T49RTY,
                                  T49COD,
                                  T49SEQ,
                                  T49RNO,
                                  T49TNM,  
                                  T49ENM,
                                  T49DEP,
                                  T49TE1,
                                  T49TR1,
                                  T49EX1,
                                  T49FX1,
                                  T49TE2,
                                  T49TR2,
                                  T49EX2,
                                  T49FX2,
                                  T49TE3,
                                  T49TR3,
                                  T49EX3,
                                  T49FX3,
                                  T49HMB,
                                  T49UDD,
                                  T49UDT,
                                  T49USR,
                                  T49PGM,
                                  T49DSP
                                ) VALUES (
                                  'V',
                                  'C',
                                  " + vendorCode + @",
                                  " + seq + @",
                                  " + runningNo + @",
                                  '" + cThaiName + @"',
                                  '" + cEnglishName + @"',
                                  '" + cDepartment + @"',
                                  " + cTelephoneNo1 + @",
                                  " + cContactTelRange1 + @",
                                  '" + cExtension1 + @"',
                                  " + cFaxNo1 + @",
                                  " + cTelephoneNo2 + @",
                                  " + cContactTelRange2 + @",
                                  '" + cExtension2 + @"',
                                  " + cFaxNo2 + @",
                                  " + cTelephoneNo3 + @",
                                  " + cContactTelRange3 + @",
                                  '" + cExtension3 + @"',
                                  " + cFaxNo3 + @",
                                  '" + cMobilePhone + @"',
                                  " + currentDate + @",
                                  " + currentTime + @",
                                  '" + currentUser + @"',
                                  '" + ProgramName + @"',
                                  '" + workStation + @"'
                                )";

                        SqlCommandText sqlILTB49 = new SqlCommandText
                        {
                            QueryString = sql,
                            Message = "Insert Person to Contact"
                        };
                        sqlList.Add(sqlILTB49);
                    }
                }

                string runNo = "0";
                string rowSeq = "0";
                if (rowSeqForUpdate.Count > 0)
                {
                    rowRunNo = rowRunNo.Distinct().ToList();
                    runNo = string.Join(",", rowRunNo);
                    rowSeq = string.Join(",", rowSeqForUpdate);
                }

                //Delete : SET T49DEL = 'X'
                sql = @"UPDATE
                            AS400DB01.ILOD0001.ILTB49 
                        SET
                            T49DEL = 'X',
                            T49UDD = " + currentDate + @",
                            T49UDT = " + currentTime + @",
                            T49USR = '" + currentUser + @"',
                            T49PGM = '" + ProgramName + @"',
                            T49DSP = '" + workStation + @"'
                        WHERE
                            T49PAR = 'V'
                        AND T49RTY = 'C'
                        AND T49COD = " + vendorCode + @"
                        AND T49RNO NOT IN (" + runNo + @")
                        AND T49SEQ NOT IN (" + rowSeq + @")";

                SqlCommandText sqlILTB49Del = new SqlCommandText
                {
                    QueryString = sql,
                    Message = "Delete Person to Contact"
                };
                sqlList.Add(sqlILTB49Del);
            }
        }
        #endregion

        UpdateVendor(sqlList, dataCenter, cmd);
    }

    protected void UpdateVendor(List<SqlCommandText> sqlList, DataCenter dataCenter, iDB2Command cmd)
    {
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        //iDB2Command cmd = new iDB2Command();
        MessageHeaderText = "Edit Vendor";
        //if (dataCenter.SqlCon.State != ConnectionState.Open)
        //    dataCenter.OpenConnectSQL();
        //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
        bool transaction = true;
        try
        {
            foreach (SqlCommandText sql in sqlList)
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                cmd.CommandText = sql.QueryString;
                int result = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                if (result == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = string.Format("Error on {0}.\n Please check command!", sql.Message);
                    SetMessage();
                    return;
                }
            }

            dataCenter.CommitMssql();
            Message = "Update Data Complete";
            SetMessageSuccess();
            SetDefault();

            Timer1.Interval = 2000;
            Timer1.Enabled = true;
            return;
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Error catch Update Master Vendor : " + ex.Message;
            SetMessage();
            return;
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            cmd.Parameters.Clear();
        }
    }

    #region Popup Confirm Add/Edit
    protected void SetConfirmAddEditMessage()
    {
        PopupConfirmAddEdit.HeaderText = ConfirmAddEditHeadText;
        lblPopupConfirmAddEditMessage.Text = ConfirmAddEditMessage;
        PopupConfirmAddEdit.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmAddEditOK_Click(object sender, EventArgs e)
    {
        imgdivLoading.Style.Add("visibility", "visible"); ;
        if (Mode == "Add")
        {
            AddNewVendor();
        }
        else
        {
            EditVendor();
        }
    }
    #endregion
    #endregion

    #region Delete
    protected void ValidateDeleteVendor()
    {
        string vendorCode = hdfVendorCode.Value;
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string sql = "";
        DataSet ds = new DataSet();

        #region Check Use Vendor on File Contract
        sql = @"SELECT	P1BRN, P1APNO, P1LTYP, P1APPT, P1APVS, P1SAPC, P1APDT, P1PBCD, P1PBRN, P1PATY, 
						P1PANO, P1PAYT, P1VDID, P1MKID, P1CAMP, P1CMSQ, P1ITEM, P1PDGP, P1PRIC, P1QTY, 
						P1PURC, P1VATR, P1VATA, P1DOWN, P1DISC, P1TERM, P1RANG, P1NDUE, P1LNDR, P1DUTR, 
						P1INFR, P1INTR, P1CRUR, P1PRAM, P1INTA, P1CRUA, P1INFA, P1DUTY, P1DIFF, P1COAM, 
						P1FDAM, P1FRTM, P1FRDT, P1FRAM, P1APRJ, P1STDT, P1STTM, P1AVDT, P1AVTM, P1CONT, 
						P1CNDT, P1FDUE, P1CSNO, P1LOCA, P1CRCD, P1AUTH, P1RESN, P1DCCD, P1DOCR, P1CONC, 
						P1COOT, P1KUSR, P1KDTE, P1KTIM, P1FAX, P1FILL, P1UPDT, P1UPTM, P1UPUS, P1PROG, 
						P1WSID, P1RSTS
                FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                WHERE
                    P1VDID = " + vendorCode + @"
                AND P1RSTS is null";

        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(ds))
        {
            MessageHeaderText = "Vlidate";
            Message = "Found Vendor in FILES ILMS01 Record Can not Delete!";
            SetMessage();
            return;
        }
        #endregion

        #region Check Use Vendor on File Campaign
        sql = @"SELECT
                    DISTINCT C01CMP
                FROM
                    AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK)
                WHERE
                    C01VDC = " + vendorCode;

        ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(ds))
        {
            string C01CMP = ds.Tables[0]?.Rows[0]["C01CMP"].ToString();
            MessageHeaderText = "Vlidate";
            Message = "Found this Vendor in ILCP01(CAMPAIGN):" + C01CMP + "...!!!";
            SetMessage();
            return;
        }
        #endregion

        #region Check Use Vendor on File Campaign/Vendor List
        sql = @"SELECT
                    DISTINCT C08CMP 
                FROM
                    AS400DB01.ILOD0001.ILCP08 WITH (NOLOCK)
                WHERE 
                    C08VEN = " + vendorCode;

        if (cookiesStorage.check_dataset(ds))
        {
            string C08CMP = ds.Tables[0]?.Rows[0]["C08CMP"].ToString();
            MessageHeaderText = "Vlidate";
            Message = "Found this Vendor in ILCP08(CAMPAIGN VENDOR):" + C08CMP + "...!!!";
            SetMessage();
            return;
        }
        #endregion

        ConfirmDeleteHeadText = "Confirm";
        ConfirmDeleteMessge = "Confirm Delete : " + hdfVendorCode.Value;
        SetConfirmDeleteMessage();
    }

    protected void DeleteVendor(List<string> sqlList)
    {
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        MessageHeaderText = "Delete Vendor";

        try
        {
            foreach (var sql in sqlList)
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int result = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
                if (result == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Delete Data Not Complete";
                    SetMessage();
                    return;
                }
            }

            dataCenter.CommitMssql();
            Message = "Delete Data Complete";
            SetMessageSuccess();
            SetDefault();

            Timer1.Interval = 2000;
            Timer1.Enabled = true;
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Error on Delete data. Please try again";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }
    }

    #endregion

    #region Popup Confirm Delete
    protected void SetConfirmDeleteMessage()
    {
        PopupConfirmDelete.HeaderText = ConfirmDeleteHeadText;
        lblPopupConfirmDeleteMessage.Text = ConfirmDeleteMessge;
        PopupConfirmDelete.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmDeleteOK_Click(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        string currentUser = m_userInfo.Username;
        string workStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();
        string userBranchNo = m_userInfo.BranchNo;
        //CALL_PROCEDURES_ILSR97();
        string date97 = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentDate = date97;
        string currentTime = DateTime.Now.ToString("HHmmss", _dateThai);
        string sql = "";
        string vendorCode = hdfVendorCode.Value;
        List<string> sqlDelete = new List<string>();
        #region Backup Data before delete
        sql = @"INSERT INTO AS400DB01.ILOD0001.ILMS10HS  
                SELECT
                    " + currentDate + @", 
                    " + currentTime + @", 
                    '" + currentUser + @"', 
                    '" + workStation + @"', 
                    'D', 
                    P10VEN, P10TIC, P10TNM, P10NAM, P10ADR, P10VIL, P10BIL, P10BUD, P10ROM, P10FLO, P10SOI, P10ROD, P10MOO, P10TMC,
				    P10AMC, P10PVC, P10ZIP, P10AD2, P10VI2, P10BI2, P10BU2, P10RM2, P10FL2, P10SO2, P10RD2, P10MO2, P10TM2, P10AM2, 
				    P10PV2, P10ZI2, P10A31, P10A32, P10A33, P10STS, P10GRD, P10MOU, P10REG, P10TAX, P10TE1, P10TLR, P10EXT, P10TE2, 
				    P10TR2, P10EX2, P10FX1, P10F1T, P10FX2, P10F2T, P10RES, P10POT, P10RDP, P10RE2, P10PO2, P10RP2, P10FJD, P10JDT, 
				    P10EDT, P10PVN, P10BPY, P10TXR, P10PYE, P10PTY, P10BCD, P10BNO, P10BRG, P10DLV, P10HED, P10DTX, P10SFG, P10CLD, 
				    P10RF1, P10CRD, P10MKD, P10TAV, P10LTM, P10SAL, P10CTY, P10DTY, P10CPD, P10BRN, P10DT1, P10DT2, P10FI1, P10FIL, 
				    P10UPD, P10TIM, P10USR, P10PGM, P10DSP, P10DEL, P10ATS, P10FIX, P10SPC
                FROM
                    AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                WHERE
                    P10VEN = " + vendorCode;

        sqlDelete.Add(sql);
        #endregion

        #region Delete Vendor
        sql = $@"UPDATE
                    AS400DB01.ILOD0001.ILMS10
                SET
                    P10DEL = 'X',
                    P10UPD = {currentDate},
                    P10TIM = {currentTime},
                    P10USR = '{currentUser}'
                WHERE
                    P10VEN = {vendorCode}";
        sqlDelete.Add(sql);
        #endregion

        DeleteVendor(sqlDelete);
    }
    #endregion


    #region DropDownList Province
    protected void LoadProvinceDataToCookies()
    {
        if (!string.IsNullOrEmpty(HdnVendorProvince.Value))
        {
            DataSet dataSet = new DataSet();
            dataSet = cookiesStorage.JsonDeserializeObjectHiddenDataSet(HdnVendorProvince.Value);
            return;
        }

        string sql = "SELECT Code as GN20CD, DescriptionTHAI as GN20DT FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE IsDelete = ''";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsProvince = new DataSet();
        dsProvince = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        CookiesStorage cookies = new CookiesStorage();

        HdnVendorProvince.Value = cookies.JsonSerializeObjectHiddenDataSet(dsProvince);

    }
    #endregion

    #region DropDownList Amphur
    protected void LoadAmphurDataToCookies(System.Web.UI.WebControls.DropDownList dropDownList, string provinceCode = "")
    {

        if (!string.IsNullOrEmpty(HdnVendorAmphurs.Value))
        {

            DataSet dataSet = new DataSet();
            dataSet = cookiesStorage.JsonDeserializeObjectHiddenDataSet(HdnVendorAmphurs.Value);
            //dataSet.Tables[0].Select("Code")
            if (!string.IsNullOrEmpty(provinceCode))
            {
                DataTable dtFilter = dataSet.Tables[0].Select("Code = '" + provinceCode + "'").CopyToDataTable();
                DataSet dsFilter = new DataSet();
                dsFilter.Tables.Add(dtFilter);

                if (ilObj.check_dataset(dsFilter))
                {
                    dropDownList.DataValueField = "GN21AM";
                    dropDownList.DataTextField = "GN19DT";
                    dropDownList.DataSource = dsFilter;
                    dropDownList.DataBind();
                }


                dropDownList.Items.Insert(0, new ListItem("Select", ""));
            }
            return;

        }
        string sql = @"SELECT DISTINCT aa.Code as GN21AM, aa.DescriptionTHAI as GN19DT , ap.Code 
                        FROM GeneralDB01.GeneralInfo.AddrRelation ar WITH (NOLOCK)
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur aa WITH (NOLOCK)  ON  AumphurID = aa.ID
                        LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince ap WITH (NOLOCK)  ON ProvinceID = ap.ID
                        WHERE ar.IsDelete = '' ORDER BY aa.DescriptionTHAI ASC";
        //AND ap.Code = '" + provinceCode + "' ORDER BY aa.DescriptionTHAI ASC";



        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsAmphur = new DataSet();
        dsAmphur = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        CookiesStorage cookies = new CookiesStorage();

        HdnVendorAmphurs.Value = cookies.JsonSerializeObjectHiddenDataSet(dsAmphur);
        if (ilObj.check_dataset(dsAmphur))
        {
            dropDownList.DataValueField = "GN21AM";
            dropDownList.DataTextField = "GN19DT";
            dropDownList.DataSource = dsAmphur;
            dropDownList.DataBind();
        }


        dropDownList.Items.Insert(0, new ListItem("Select", ""));

    }
    #endregion

    #region DropDownList District
    protected void LoadDistrictDataToCookies(System.Web.UI.WebControls.DropDownList dropDownList, string amphursCode = "")
    {

        if (!string.IsNullOrEmpty(HdnVendorDistrict.Value))
        {

            DataSet dataSet = new DataSet();
            dataSet = cookiesStorage.JsonDeserializeObjectHiddenDataSet(HdnVendorDistrict.Value);
            //dataSet.Tables[0].Select("Code")
            if (!string.IsNullOrEmpty(amphursCode))
            {
                DataTable dtFilter = dataSet.Tables[0].Select("GN21AM = '" + amphursCode + "'").CopyToDataTable();
                DataSet dsFilter = new DataSet();
                dsFilter.Tables.Add(dtFilter);

                if (ilObj.check_dataset(dsFilter))
                {
                    dropDownList.DataValueField = "GN21TM";
                    dropDownList.DataTextField = "GN18DT";
                    dropDownList.DataSource = dsFilter;
                    dropDownList.DataBind();
                }


                dropDownList.Items.Insert(0, new ListItem("Select", ""));
            }
            return;

        }

        string sql = @"SELECT
                        GNTB18.Code as GN21TM,
                        GNTB18.DescriptionTHAI as GN18DT,
                        GNTB18.DescriptionENG as GN18DE,
                        GNTB21.ZipCode as GN21ZP,
                        GNTB19.Code as GN21AM,
                        GNTB19.DescriptionTHAI as GN19DT,
                        GNTB19.DescriptionENG as GN19DE,
                        GNTB20.Code as GN21PR,
                        GNTB20.DescriptionTHAI as GN20DT,
                        GNTB20.DescriptionENG as GN20DE,
                        GNTB21.Remark as GN21RE
                    FROM
                        GeneralDB01.GeneralInfo.AddrRelation GNTB21 WITH (NOLOCK)
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol GNTB18 WITH (NOLOCK)
                    ON  GNTB21.TambolID =  GNTB18.ID
                    AND GNTB18.IsDelete = ''
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur GNTB19 WITH (NOLOCK)
                    ON  GNTB21.AumphurID = GNTB19.ID
                    AND GNTB19.IsDelete = ''
                    LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince GNTB20 WITH (NOLOCK)
                    ON  GNTB21.ProvinceID = GNTB20.ID
                    AND GNTB20.IsDelete = ''
                    WHERE
                        GNTB21.IsDelete = ''";
        //AND	GNTB19.Code = '" + amphurCode + "'";

        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsDistrict = new DataSet();
        dsDistrict = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        CookiesStorage cookies = new CookiesStorage();

        HdnVendorDistrict.Value = cookies.JsonSerializeObjectHiddenDataSet(dsDistrict);
        if (ilObj.check_dataset(dsDistrict))
        {
            dropDownList.DataValueField = "GN21TM";
            dropDownList.DataTextField = "GN18DT";
            dropDownList.DataSource = dsDistrict;
            dropDownList.DataBind();
        }


        dropDownList.Items.Insert(0, new ListItem("Select", ""));
    }

    protected string GetPostCodeToCookies(string provinceCode, string amphurCode, string districtCode)
    {
        string postCode = "";
        try
        {
            if (!string.IsNullOrEmpty(HdnVendorPostCode.Value))
            {

                DataSet dataSet = new DataSet();
                dataSet = cookiesStorage.JsonDeserializeObjectHiddenDataSet(HdnVendorPostCode.Value);
                //dataSet.Tables[0].Select("Code")
                if (!string.IsNullOrEmpty(provinceCode) && !string.IsNullOrEmpty(amphurCode) && !string.IsNullOrEmpty(districtCode))
                {
                    DataTable dtFilter = dataSet.Tables[0].Select("GNTB20Code = '" + provinceCode + "' AND  GNTB19Code = '"+amphurCode+ "' AND GNTB18Code = '"+districtCode+"'").CopyToDataTable();
                    DataSet dsFilter = new DataSet();
                    dsFilter.Tables.Add(dtFilter);

                    DataTable dt2 = new DataTable();
                    DataRow drPostCode2 = dt2.NewRow();
                    if (cookiesStorage.check_dataset(dsFilter))
                    {
                        drPostCode2 = dsFilter.Tables[0]?.Rows[0];
                        postCode = drPostCode2["GN21ZP"].ToString();
                    }
                }
                return postCode;

            }
            string sql = @"SELECT
	                            GNTB21.ZipCode as GN21ZP,
                                GNTB20.Code as GNTB20Code,
                                GNTB19.Code as GNTB19Code,
                                GNTB18.Code as GNTB18Code
                            FROM
	                            GeneralDB01.GeneralInfo.AddrRelation GNTB21 WITH (NOLOCK)
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol GNTB18 WITH (NOLOCK)
                                ON  GNTB21.TambolID =  GNTB18.ID
                                AND GNTB18.IsDelete = ''
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur GNTB19 WITH (NOLOCK)
                                ON  GNTB21.AumphurID = GNTB19.ID
                                AND GNTB19.IsDelete = ''
                                LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince GNTB20 WITH (NOLOCK)
                                ON  GNTB21.ProvinceID = GNTB20.ID
                                AND GNTB20.IsDelete = ''";

            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsPostCode = new DataSet();
            dsPostCode = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            CookiesStorage cookies = new CookiesStorage();

            HdnVendorPostCode.Value = cookies.JsonSerializeObjectHiddenDataSet(dsPostCode);

            DataTable dt = new DataTable();
            DataRow drPostCode = dt.NewRow();
            if (cookiesStorage.check_dataset(dsPostCode))
            {
                drPostCode = dsPostCode.Tables[0]?.Rows[0];
                postCode = drPostCode["GN21ZP"].ToString();
            }
        }
        catch (Exception ex)
        {

        }

        return postCode;
    }
    protected void InitialCookiesData()
    {

        LoadProvinceDataToCookies();
        LoadAmphurDataToCookies(ddlTabAddress_RegistrationAddress_Amphur);
        LoadDistrictDataToCookies(ddlTabAddress_RegistrationAddress_District);
        //GetPostCodeToCookies();
    }




    #endregion

    protected void btnTabOther_Information_LoadAllDataFromInformationVendor_Click(object sender, EventArgs e)
    {
        string vendorCode = txtVendorHeadCode.Text.Trim().Replace("-", "");
        m_userInfo = userInfoService.GetUserInfo();
        string userBranch = m_userInfo.BranchNo;
        if (!string.IsNullOrEmpty(vendorCode))
        {
            string sql = $@"SELECT P16RNK 
                            FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE P16VEN = {vendorCode}";

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsVendor = new DataSet();
            DataTable dt = new DataTable();
            dsVendor = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            DataRow drVendor = dt.NewRow();
            if (cookiesStorage.check_dataset(dsVendor) )
            {
                drVendor = dsVendor.Tables[0]?.Rows[0];
               
                txtVendorRank.Text = drVendor["P16RNK"].ToString().Trim();
            }
            sql = $@"SELECT P10GRD, P10MOU, P10JDT, P10REG, P10EDT, P10TAX, P10JDT, P10ATS
                            FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) WHERE P10VEN = {vendorCode}";
            dsVendor = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            drVendor = dt.NewRow();
            if (cookiesStorage.check_dataset(dsVendor))
            {
                drVendor = dsVendor.Tables[0]?.Rows[0];
                txtVendorGrade.Text = drVendor["P10GRD"].ToString().Trim();
                txtTabOther_Information_MOUDate.Text = DateFormatter(drVendor["P10MOU"].ToString().Trim());
                string joinDate = drVendor["P10JDT"].ToString().Trim();
                if (Convert.ToInt32(joinDate) == 0)
                {
                    joinDate = DateTime.Now.ToString("dd/MM/yyyy", _dateThai);
                }
                else
                {
                    joinDate = DateFormatter(joinDate);
                }
                txtTabOther_Information_JoinDate.Text = joinDate;
                txtTabOther_Information_RegisterNoTaxID.Text = drVendor["P10REG"].ToString().Trim();
                txtTabOther_Information_ExpireDate.Text = DateFormatter(drVendor["P10EDT"].ToString().Trim());
                txtTabOther_Information_TaxID.Text = drVendor["P10TAX"].ToString().Trim();
                txtTabOther_Information_FirstOpenDate.Text = joinDate;
                ddlTabOther_Information_AutoSignLayBill.SelectedValue = drVendor["P10ATS"].ToString().Trim();
            }
            sql = $@"SELECT G11SFX, G11IFX, G11OHF, G11IHF, G11TFX 
                     FROM AS400DB01.GNOD0000.GNTB11 WITH (NOLOCK) 
                     WHERE CAST(G11BRN as nvarchar) = '{userBranch}' AND G11BUS = 'IL' AND G11VEN = '{vendorCode}'";
            dsVendor = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            drVendor = dt.NewRow();
            if (cookiesStorage.check_dataset(dsVendor))
            {
                drVendor = dsVendor.Tables[0]?.Rows[0];
                ddlTabOther_Information_StatusFaxForOper.SelectedValue = drVendor["G11SFX"].ToString().Trim();
                ddlTabOther_Information_StatusFaxForISB.SelectedValue = drVendor["G11IFX"].ToString().Trim();
                ddlTabOther_Information_FaxForHOForOper.SelectedValue = drVendor["G11OHF"].ToString().Trim();
                ddlTabOther_Information_FaxForHOForISB.SelectedValue = drVendor["G11IHF"].ToString().Trim();
                ddlTabOther_Information_AutoFax.SelectedValue = drVendor["G11TFX"].ToString().Trim();

            }
        }
    }
}