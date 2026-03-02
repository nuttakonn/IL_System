using EB_Service.DAL;
using ILSystem.App_Code.Commons;
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

public partial class ManageData_WorkProcess_VendorApplicationType : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    public UserInfoService userInfoService;
    ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    protected string SqlAll = "";
    protected string VendorID = "";
    protected string VendorName = "";
    protected string VendorNameEn = "";
    protected string LoanType = "";
    protected string LoanTypeName = "";
    protected string Branch = "";
    protected string BranchName = "";
    protected string ApplicationType = "";
    protected string tmpExpDate = "";
    protected string ExpireDate = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";
    public CookiesStorage cookiesStorage;
    /*********** D10PGM ***********
      * จาก Delphi คือ ILE000004F
      * จาก Code ที่ได้รับมาคือ IL_VENAPP
      * ยึดค่าตาม Code ที่ได้รับมา : D10PGM = IL_VENAPP
    */
    protected string ProgramName = "IL_VENAPP";

    protected void Page_Load(object sender, EventArgs e)
    {

        userInfoService = new UserInfoService();
        m_userInfo = userInfoService.GetUserInfo();
        dataCenter = new DataCenter(m_userInfo);
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
        //DateTime dt = DateTime.Now;
        //CalendarStartDate.SelectedDate = dt;
        if (Page.IsPostBack)
        {
            return;
        }
        PopupMsgCenter();
        //SearchData();
        //BindDropDownlistApplicationType();
    }
    protected void TimerTick(object sender, EventArgs e)
    {
        SearchData();
        BindDropDownlistApplicationType();
        Timer1.Enabled = false;
    }

    #region Bind Data and Search
    protected void btnSearchClick(object sender, EventArgs e)
    {
        SearchData();
    }

    private void BindDropDownlistApplicationType()
    {
        string sql = @"SELECT
                           GN61CD,
                           GN61DT
                       FROM
                           AS400DB01.GNOD0000.GNTB61 WITH (NOLOCK)
                       WHERE
                           GN61DL = ''
                       ORDER BY
                           GN61CD";

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(sql,CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ddlApplicationType.Items.Clear();
            ddlApplicationType.Items.Add(new ListItem("Select Application Type", ""));
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                string itemText = dr["GN61CD"].ToString().Trim() + " : " + dr["GN61DT"].ToString().Trim();
                string itemValue = dr["GN61CD"].ToString().Trim();
                ddlApplicationType.Items.Add(new ListItem(itemText, itemValue));
            }
            DS.Clear();
        }
    }

    void SearchData()
    {
        iDB2Command cmd = new iDB2Command();
        //string sqlWhere = "";
        string searchBy = ddlSearchBy.SelectedValue;
        string vendorApplication = txtVendorApplicationType.Text.Trim().Replace("'", "''");

        //if (vendorApplication != "")
        //{
        //    switch (ddlSearchBy.SelectedValue)
        //    {
        //        case "VI":
        //            sqlWhere = " AND CAST(D10VEN as nvarchar) = '" + vendorApplication + "'";
        //            break;
        //        case "VN":
        //            sqlWhere = " AND UPPER(P10NAM) LIKE '%" + vendorApplication.ToUpper() + "%' ";
        //            break;
        //        case "VA":
        //            sqlWhere = " AND CAST(D10APT as nvarchar) = '" + vendorApplication + "'";
        //            break;
        //        case "D":
        //            sqlWhere += " AND GN61DT LIKE '%" + vendorApplication.ToUpper().Trim() + "%' ";
        //            break;
        //        default: break;
        //    }
        //}

        //SqlAll = @"SELECT
        //               D10VEN,
        //               P10NAM,
        //               D10BRN,
        //               T1BNME,
        //               T00LTY,
        //               T00TNM,
        //               D10APT,
        //               CONCAT(SUBSTRING(CONVERT(nvarchar,D10EXP),7,2),'/',SUBSTRING(CONVERT(nvarchar,D10EXP),5,2),'/',SUBSTRING(CONVERT(nvarchar,D10EXP),1,4)) AS D10EXP,
        //               GN61DT
        //           FROM
        //               AS400DB01.ILOD0001.ILMD10 WITH (NOLOCK)
        //           LEFT JOIN 
        //               AS400DB01.ILOD0001.ILTB01 WITH (NOLOCK)
        //           ON
        //               (D10BRN = T1BRN) 
        //           LEFT JOIN 
        //               AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
        //           ON
        //               (D10LTY = T00LTY)
        //           LEFT JOIN
        //               AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
        //           ON
        //               (P10VEN = D10VEN) 
        //           LEFT JOIN
        //               AS400DB01.GNOD0000.GNTB61 WITH (NOLOCK)
        //           ON
        //               (GN61CD = D10APT AND GN61DL = '' ) 
        //           WHERE
        //               D10DEL = '' ";

        //SqlAll = SqlAll + sqlWhere + " ORDER BY D10VEN ";
        //cmd.CommandText = SqlAll;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);

        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetVendorApplicationType("S", searchBy, vendorApplication, 1, 15);

        //DS = dataCenter.GetDataset<DataTable>(SqlAll,CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView1.DataSource = DS;
            GridView1.DataBind();
            DataRow dr = DS.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = GridView1.PageIndex * GridView1.PageSize + 1;
            int _CurrentRecEnd = GridView1.PageIndex * GridView1.PageSize + GridView1.Rows.Count;

            lblTitle.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
        }
        else
        {
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
        ResetGrid(GridView1, ds_Hiddengrid.Value);
    }
    #endregion

    #region Default
    private void SetMsg()
    {
        lblMsg.Text = Msg;
        PopupMsg.HeaderText = MsgHText;
        PopupMsg.ShowOnPageLoad = true;
    }
    private void SetMsgSuccess()
    {
        lblMsgSuccess.Text = Msg;
        PopupMsgSuccess.HeaderText = MsgHText;
        PopupMsgSuccess.ShowOnPageLoad = true;
    }

    private void SetConfirmMsg()
    {
        lblConfirmMsgEN.Text = CMsg;
        PopupConfirmAdd.HeaderText = CHText;
        PopupConfirmAdd.ShowOnPageLoad = true;
    }

    private void SetClearSearch()
    {
        ddlSearchBy.SelectedIndex = 0;
        txtVendorApplicationType.Text = "";
    }
    #endregion

    #region Select Page
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.SelectedIndex = -1;
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        GridView1.DataBind();
    }
    #endregion

    #region Clear Data
    protected void btnClearClick(object sender, EventArgs e)
    {
        SetClearSearch();
        SearchData();
        /*GridView1.DataSource = null;
        GridView1.DataBind();*/
    }

    protected void btnClearDataClick(object sender, EventArgs e)
    {
        ClearDataAdd();
    }
    #endregion

    #region Insert and Update Data
    protected void btnAddClick(object sender, EventArgs e)
    {
        VendorID = txtVendorID.Text.Trim();
        LoanType = txtLoanType.Text.Trim();
        Branch = txtBranch.Text.Trim();
        ApplicationType = ddlApplicationType.SelectedValue;
        ExpireDate = txtExpireDate.Text;

        if (VendorID == "")
        {
            Msg = "Please Select Vendor  ";
            MsgHText = "Validate Save";
            SetMsg();
            btnVendor.Focus();
            return;
        }

        if (ApplicationType == "")
        {
            Msg = "Please Select App Type  ";
            MsgHText = "Validate Save";
            SetMsg();
            ddlApplicationType.Focus();
            return;
        }

        if (ExpireDate.Replace("/", "").Trim() == "")
        {
            Msg = "Please Input Expire Date ";
            MsgHText = "Validate Save";
            SetMsg();
            return;
        }
        if (!validateDate(ExpireDate.Trim()))
        {
            Msg = "Format Expire Date Invalid ";
            MsgHText = "Validate Save";
            SetMsg();
            return;
        }

        SqlAll = @"SELECT
                       D10VEN
                   FROM
                       AS400DB01.ILOD0001.ILMD10 WITH (NOLOCK)
                   WHERE
                       D10DEL = ''
                   AND D10LTY = '" + LoanType + @"'
                   AND D10BRN = " + Branch + @"
                   AND D10APT = '" + ApplicationType + @"'
                   AND D10VEN = " + VendorID;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll,CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found this Vendor Application Type is Duplicate";
            MsgHText = "Validate Save";
            SetMsg();
        }
        else
        {
            CHText = "Confirm Save";
            CMsg = "Confirm Save Vendor Application Type Data";
            SetConfirmMsg();
        }
        dataCenter.CloseConnectSQL();
    }
    #endregion

    #region Confirm Add/Edit
    protected void btnConfirmAddOKClick(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        dataCenter = new DataCenter(m_userInfo);
        
        ilObj.UserInfomation = m_userInfo;

        string NameButton = "";
        /*** Comment For Test***/
        string Curtime = DateTime.Now.ToString("yyyyMMdd", m_DThai);

        VendorID = txtVendorID.Text.Trim();
        Branch = txtBranch.Text.Trim();
        LoanType = txtLoanType.Text.Trim();
        ApplicationType = ddlApplicationType.SelectedValue;
        string strExpireDate = txtExpireDate.Text.Trim();
        string[] resExpireDate = strExpireDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

        ExpireDate = resExpireDate[2] + resExpireDate[1] + resExpireDate[0];

        SqlAll = @"SELECT
                       D10VEN
                   FROM
                       AS400DB01.ILOD0001.ILMD10 WITH (NOLOCK)
                   WHERE
                       D10DEL = 'X'
                   AND D10BRN = " + Branch + @"
                   AND D10LTY = '" + LoanType + @"'
                   AND D10APT = '" + ApplicationType + @"'
                   AND D10VEN = " + VendorID;

        DataSet DS = new DataSet();
        
        ilObj.UserInfomation = m_userInfo;
        //if (dataCenter.SqlCon.State != ConnectionState.Open)
        //    dataCenter.OpenConnectSQL();
        //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
        DS = dataCenter.GetDataset<DataTable>(SqlAll,CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(DS))
        {
            DataRow dr = DS.Tables[0]?.Rows[0];
            VendorID = dr[0].ToString().Trim();
            NameButton = "Edit";
            MsgHText = NameButton;

            //SqlAll = @"UPDATE
            //               ILMD10
            //           SET
            //               D10DEL = '',  
            //               D10UPD = " + Curtime + @",
            //               D10TIM = " + m_UpdTime.ToString() + @",
            //               D10USR = '" + m_userInfo.Username.ToString() + @"',
            //               D10PGM = '" + ProgramName + @"',
            //               D10DSP = '" + m_userInfo.LocalClient.ToString() + @"',
            //               D10EXP = " + ExpireDate + @"
            //           WHERE
            //               D10BRN = " + Branch + @"
            //           AND D10LTY = '" + LoanType + @"'
            //           AND D10VEN = " + VendorID + @"
            //           AND D10APT = '" + ApplicationType + "'";

            SqlAll = $@"Update AS400DB01.ILOD0001.ILMD10 
                                SET D10DEL = '',
                                    D10UPD = {Curtime},                   
                                    D10TIM = {m_UpdTime.ToString()},
                                    D10USR = '{m_userInfo.Username.ToString()}',
                                    D10PGM = '{ProgramName}',
                                    D10DSP = '{m_userInfo.LocalClient.ToString()}',
                                    D10EXP = {ExpireDate} 
                                WHERE D10BRN = " + Branch + @"
                                    AND D10LTY = '" + LoanType + @"'
                                    AND D10VEN = " + VendorID + @"
                                    AND D10APT = '" + ApplicationType + "'";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("T40DEL", string.Empty);
            cmd.Parameters.AddWithValue("T40UDD", Curtime);
            cmd.Parameters.AddWithValue("T40UDT", m_UpdTime.ToString());
            cmd.Parameters.AddWithValue("T40USR", m_userInfo.Username.ToString());
            cmd.Parameters.AddWithValue("T40PGM", ProgramName);
            cmd.Parameters.AddWithValue("T40DSP", m_userInfo.LocalClient.ToString());
            cmd.Parameters.AddWithValue("T40DES", ExpireDate);
        }
        else
        {
            NameButton = "Add";
            MsgHText = NameButton;

            //SqlAll = @"INSERT INTO  ILMD10 
            //                    (
            //                        D10BRN,
            //                        D10LTY,
            //                        D10VEN,
            //                        D10APT,
            //                        D10EXP,
            //                        D10UPD,
            //                        D10TIM,
            //                        D10USR,
            //                        D10PGM,
            //                        D10DSP
            //                    ) VALUES (
            //                        " + Branch + @", 
            //                        '" + LoanType + @"',
            //                        " + VendorID + @",
            //                        '" + ApplicationType + "'," +
            //                        ExpireDate + "," +
            //                        Curtime + "," +
            //                        m_UpdTime.ToString() + ",'" +
            //                        m_userInfo.Username.ToString() + "', " +
            //                        "'" + ProgramName + @"'," +
            //                        "'" + m_userInfo.LocalClient.ToString() + "'" +
            //                    ")";

            SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILMD10(
                                                D10BRN,
                                                D10LTY,
                                                D10VEN,
                                                D10APT,
                                                D10EXP,
                                                D10UPD,
                                                D10TIM,
                                                D10USR,
                                                D10PGM,
                                                D10DSP)
                                    VALUES(     
                                                {Branch},
                                                '{LoanType}',
                                                {VendorID},
                                                '{ApplicationType}',
                                                {ExpireDate},
                                                {Curtime},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                '{ProgramName}',
                                                '{m_userInfo.LocalClient.ToString()}'
                                    )";

            cmd.Parameters.Clear();
            cmd.Parameters.Add("D10BRN", Branch);
            cmd.Parameters.Add("D10LTY", LoanType);
            cmd.Parameters.Add("D10VEN", VendorID);
            cmd.Parameters.Add("D10APT", ApplicationType);
            cmd.Parameters.Add("D10EXP", ExpireDate);
            cmd.Parameters.Add("D10UPD", Curtime);
            cmd.Parameters.Add("D10TIM", m_UpdTime.ToString());
            cmd.Parameters.Add("D10USR", m_userInfo.Username.ToString());
            cmd.Parameters.Add("D10PGM", ProgramName);
            cmd.Parameters.Add("D10DSP", m_userInfo.LocalClient.ToString());
        }

        cmd.CommandText = SqlAll;

        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int resHome11 = dataCenter.Execute(SqlAll,CommandType.Text, transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                Msg = NameButton + " Brand not complete.";
                SetMsg();
            }
            else
            {
                dataCenter.CommitMssql();
                Msg = NameButton + " Vendor Application Type complete.";
                SetMsgSuccess();
                SearchData();
                ClearDataAdd();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = NameButton + " Vendor Application Type not complete.";
            SetMsg();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            cmd.Parameters.Clear();
        }
    }

    private void ClearDataAdd()
    {
        //setStartDate.Enabled = true;
        //txtExpireDate.Text = tmpExpDate;
        txtVendorID.Text = "";
        txtVendorName.Text = "";
        txtLoanType.Text = "";
        txtLoanTypeDesc.Text = "";
        txtBranch.Text = "";
        txtBranchName.Text = "";
        ddlApplicationType.SelectedIndex = 0;
        txtExpireDate.Text = "";
    }

    protected void btnConfirmAddCancelClick(object sender, EventArgs e)
    {
        btnVendor.Focus();
    }
    #endregion

    #region Delete Data
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.RowIndex];
        VendorID = dr[0].ToString().Trim();
        LoanType = dr[4].ToString().Trim();
        Branch = dr[2].ToString().Trim();
        ApplicationType = dr[6].ToString().Trim();
        ExpireDate = dr[7].ToString().Trim();
        m_userInfo = userInfoService.GetUserInfo();
        
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        /***Comment For Test***/
        string Curtime = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        //string Curtime = "";
        //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
        //ilObj.CloseConnectioDAL();
        /**********************/

        SqlAll = @"UPDATE
                       AS400DB01.ILOD0001.ILMD10
                   SET
                       D10DEL = 'X',
                       D10UPD = " + Curtime + @",
                       D10TIM = " + m_UpdTime.ToString() + @",  
                       D10USR = '" + m_userInfo.Username.ToString() + @"',
                       D10PGM = '" + ProgramName + @"',  
                       D10DSP =  '" + m_userInfo.LocalClient.ToString() + @"',
                       D10EXP = " + ExpireDate + @"
                   WHERE
                       D10BRN = " + Branch + @"
                   AND D10LTY = '" + LoanType + @"'
                   AND D10VEN = " + VendorID + @"
                   AND D10APT = '" + ApplicationType + "'";

        txtSqlAll.Text = SqlAll;
        txtVendor_D.Text = dr[0].ToString().Trim();
        lblConfimMsgDelete.Text = "Confirm Delete Data ";
        PopupConfirmDelete.ShowOnPageLoad = true;

        GridView1.Focus();
    }
    #endregion

    #region Confirm Delete
    protected void btnConfirmDeleteOKClick(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = txtSqlAll.Text;
        cmd.CommandText = SqlAll;
        //if (dataCenter.SqlCon.State != ConnectionState.Open)
        //    dataCenter.OpenConnectSQL();
        //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int resHome11 = dataCenter.Execute(SqlAll,CommandType.Text, transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                Msg = "Delete Vendor Application Type Not Complete.";
                SetMsg();
            }
            else
            {
                dataCenter.CommitMssql();
                cmd.Parameters.Clear();
                Msg = "Delete Vendor Application Type Complete.";
                SetMsgSuccess();
                GridView1.Focus();
                SearchData();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = "Delete Vendor Application Type Not Complete.";
            SetMsg();
            return;
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }
    }

    protected void btnConfirmDeleteCancelClick(object sender, EventArgs e)
    {
        GridView1.Focus();
    }
    #endregion

    #region Message All
    protected void btnOK_Click(object sender, EventArgs e)
    {
        btnVendor.Focus();
    }
    #endregion

    #region Popup Vendor
    protected void btnVendorClick(object sender, EventArgs e)
    {
        ddlPopupAddVendorSearchBy.SelectedIndex = 0;
        txtVendor.Text = "";
        PopupAddVendor.Text = "";
        LoadDataPopup();
        PopupAddVendor.ShowOnPageLoad = true;
    }

    private void LoadDataPopup()
    {
        //string sqlWhere = "";

        //SqlAll = @"SELECT
	       //             DISTINCT P11VEN,
	       //             P10NAM,
 	      //              T00LTY,
 	      //              T00TNM
        //            FROM
	       //             AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
        //            LEFT JOIN AS400DB01.ILOD0001.ILMS11 WITH (NOLOCK)
        //            ON
	       //             P10VEN = P11VEN
        //            LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
        //            ON
	       //             P11LTY = T00LTY
        //            WHERE 
	       //             P11DEL = '' ";

        string vendor = txtVendor.Text.Trim().Replace("'", "''"); //Replace("'", "''") for prevent SQL Injection
        string searchBy = ddlPopupAddVendorSearchBy.SelectedValue;

        //if (vendor != "")
        //{
        //    switch (searchBy)
        //    {
        //        case "V":
        //            sqlWhere = " AND P10VEN = " + vendor;
        //            break;
        //        case "D":
        //            sqlWhere = " AND UPPER(P10TNM) LIKE '%" + vendor.ToUpper() + "%'";
        //            break;
        //        default: break;
        //    }
        //}

        //SqlAll = SqlAll + sqlWhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);

        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetVendorApplicationType("A", searchBy, vendor, 1, 15);
        //DS = dataCenter.GetDataset<DataTable>(SqlAll,CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView2.DataSource = DS;
            GridView2.DataBind();
            DataRow dr = DS.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = GridView2.PageIndex * GridView2.PageSize + 1;
            int _CurrentRecEnd = GridView2.PageIndex * GridView2.PageSize + GridView2.Rows.Count;

            lblTitle2.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
        }
        else
        {
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        GridView2.DataBind();
    }

    protected void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];
        txtVendorID.Text = dr[0].ToString().Trim();
        txtVendorName.Text = dr[1].ToString().Trim();
        txtLoanType.Text = dr[2].ToString().Trim();
        txtLoanTypeDesc.Text = dr[3].ToString().Trim();
        txtBranch.Text = "000";
        txtBranchName.Text = "All Branch";

        PopupAddVendor.ShowOnPageLoad = false;

        ResetGrid(GridView2, ds_popup.Value);
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();
    }

    protected void btnPopupAddVendorSearchClick(object sender, EventArgs e)
    {
        LoadDataPopup();
    }

    protected void btnPopupAddVendorClearClick(object sender, EventArgs e)
    {
        ddlPopupAddVendorSearchBy.SelectedIndex = 0;
        txtVendor.Text = "";
        PopupAddVendor.Text = "";
        LoadDataPopup();
    }

    private bool validateDate(string date)
    {
        string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "99/99/9999" };
        DateTime expectedDate;

        if (!DateTime.TryParseExact(date, formats, new CultureInfo("th-TH"),
                                        DateTimeStyles.None, out expectedDate))
        {
            return false;
        }
        return true;
    }

    #endregion   

    private void PopupMsgCenter()
    {

        PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmAdd.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmAdd.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupAddVendor.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupAddVendor.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }

}
