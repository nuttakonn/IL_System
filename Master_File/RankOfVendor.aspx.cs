using EB_Service.DAL;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageData_WorkProcess_RankOfVendor : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    protected ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    protected ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    public UserInfoService userInfoService;
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    protected iDB2Command cmd = new iDB2Command();
    protected DataTable dt = new DataTable();
    protected DataTable dtGridView2 = new DataTable();
    protected string SqlAll = "";
    protected string VendorCode = "";
    protected string VendorName = "";
    protected string rankCode = "";
    protected string tmpVenRank = "";
    protected string tmpStartDate = "";
    protected string CurrentUpdDate = "";
    protected string CurrentUpdTime = "";
    protected string tmpEndDate = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";
    protected string aa = "";
    protected string formatDateCheck = "";
    protected int countCheckBox1 = 0;
    protected int countCheckBox2 = 0;
    protected int startDateCheck;
    protected int dateCheck;
    public CookiesStorage cookiesStorage;


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
        if (!IsPostBack)
        {
            ////DateTime dt = DateTime.Now;
            ////CalendarExtender1.SelectedDate = dt;
            //startDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            startDate.Text = FORMAT_DATE(m_UpdDate.ToString());
            setEndDate.Text = "99/99/9999";
            PopupMsgCenter();

            set_Default();
            //Search_Data();
            //BIND_DATA_GRIDVIEW();
            GridViewCheckbox1.Enabled = false;
            txtSearchVendor.Enabled = false;
            btnClear.Enabled = false;
            searchRank.Enabled = false;
            return;
        }

    }
    #region Function chang format date
    public bool CHECK_VALIDATE_DATE(string dateTime)
    {
        string[] formats = { "dd/MM/yyyy" };
        DateTime parsedDateTime;
        return DateTime.TryParseExact(dateTime, formats, new CultureInfo("th-TH"),
                                       DateTimeStyles.None, out parsedDateTime);
    }
    public static string FORMAT_DATE(string dateCall)
    {
        DateTime dtime;
        DateTime.TryParseExact(dateCall, "yyyyMMdd", CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out dtime);
        string formattedDateNew = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        return formattedDateNew;
    }
    public static string CHANGE_FORMAT_DATE(string changeDateCall)
    {
        string[] splitDate = changeDateCall.ToString().Split('/');
        string changeDateNew = splitDate[2] + splitDate[1] + splitDate[0];
        return changeDateNew;
    }
    #endregion

    protected void TimerTick(object sender, EventArgs e)
    {
        imgdivLoading.Style.Add("visibility", "visible");
        Timer1.Enabled = true;
        Search_Data();
        BindDatatoGridView();
        QueryData();
        BIND_DATA_GRIDVIEW();
        Timer1.Enabled = false;
        imgdivLoading.Style.Add("visibility", "hidden");
        searchRank.Enabled = true;
    }
    private void BIND_DATA_GRIDVIEW()
    {
        DataTable dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value);
        DataTable _clonedTable = new DataTable();
        if (!cookiesStorage.check_dataTable(dt))
        {
            DataTable vendornulldt = dtVendorDefault();
            BindGrid(vendornulldt, GridViewCheckbox1);
            _clonedTable = dt.Clone();
        }
        else
        {
            BindGrid(dt, GridViewCheckbox1);
            _clonedTable = dt.Clone();
        }
        
        if (!cookiesStorage.check_dataTable(_clonedTable)) 
        { 
            ShowNoResultFound(_clonedTable, GridViewCheckbox2);
        }
    }
    #region process checkbox 
    protected void ACTIVE_TAB_CHANGE(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
    {
        if (tabDetail.ActiveTabIndex == 0)
        {
            BIND_DATA_GRIDVIEW();
        }
    }
    protected void CHECK_DISIBLE_CHECKBOX()
    {
        foreach (TableCell headerCell in GridViewCheckbox2?.HeaderRow.Cells)
        {

            CheckBox cbSelectAlls = (headerCell.FindControl("cbSelectAll") as CheckBox);
            cbSelectAlls.Enabled = false;

        }
    }
    protected void CHECK_ENABLE_CHECKBOX()
    {
        foreach (TableCell headerCell in GridViewCheckbox2?.HeaderRow.Cells)
        {

            CheckBox cbSelectAlls = (headerCell.FindControl("cbSelectAll") as CheckBox);
            cbSelectAlls.Enabled = true;

        }
    }
    protected void SELECT_VENDOR_ONCHANGE(object sender, EventArgs e)
    {
        if (int.Parse(txtSearchVendor.Text.Length.ToString()) >= 3)
        {
            //Add Filter
            DataTable dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value).Copy();
            var collection = dt.AsEnumerable().Where(r => r.Field<string>("P10VEN") == txtSearchVendor.Text).Select(y => y);
            dt = collection.Any() ? collection.CopyToDataTable() : dtVendorDefault().Copy();

            BindGrid(dt, GridViewCheckbox1);
            DataTable _clonedTable = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value).Clone();
            //ViewState["ClonedTable"] = _clonedTable;

            if (GridViewCheckbox2.Rows[0].Cells[0].Text == "NO ITEMS FOUND !" || GridViewCheckbox2.Rows.Count == 0)
            {
                ShowNoResultFound(dtVendorDefault(), GridViewCheckbox2);
            }
            //BIND_DATA_GRIDVIEW();
        }
        else if (String.IsNullOrEmpty(txtSearchVendor.Text) || String.IsNullOrWhiteSpace(txtSearchVendor.Text))
        {
            //BIND_DATA_GRIDVIEW();
            BindGrid(cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value), GridViewCheckbox1);

            if (GridViewCheckbox2.Rows[0].Cells[0].Text == "NO ITEMS FOUND !" || GridViewCheckbox2.Rows.Count == 0)
            {
                ShowNoResultFound(dtVendorDefault(), GridViewCheckbox2);
            }
        }
    }

    protected void CLEAR_DATA_LIST(object sender, EventArgs e)
    {
        txtSearchVendor.Text = "";
        DataTable dt = new DataTable();
        dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value);
        if (cookiesStorage.check_dataTable(dt))
        {
            BindGrid(dt, GridViewCheckbox1);
        }
        //QueryData();
        CLEAR_LIST_GRIDVIEW2();
    }

    private DataTable dtVendorDefault()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("P10VEN");
        dt.Columns.Add("P10NAM");
        return dt;
    }
    private DataTable dtSearchDataDefault()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("T12RNK");
        dt.Columns.Add("T12RTE");
        dt.Columns.Add("AT12STD");
        dt.Columns.Add("T12END");
        dt.Columns.Add("T12STD");
        return dt;
    }

    protected DataTable QueryData()
    {

        DataTable dt = new DataTable();

        if (int.Parse(txtSearchVendor.Text.Length.ToString()) >= 3)
        {
            SqlAll = "SELECT FORMAT(P10VEN, '000000000000') As P10VEN, P10NAM FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) where P10DEL = '' AND FORMAT(P10VEN, '000000000000') LIKE '%" + txtSearchVendor.Text.Trim() + "%'";
        }
        else
        {
            SqlAll = "SELECT FORMAT(P10VEN, '000000000000') As P10VEN, P10NAM FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) where P10DEL = ''";
        }

        DataSet DS = new DataSet();

        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(DS))
        {
            dt = DS.Tables[0];
            dt_vendor_detail.Value = cookiesStorage.JsonSerializeObjectHiddenDataDataTable(dt);
        }
        dataCenter.CloseConnectSQL();
        return dt;
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewCheckbox1.HeaderRow.Visible = true;
    }
    protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewCheckbox1.SelectedIndex = -1;
        GridViewCheckbox1.PageIndex = e.NewPageIndex;
        GridViewCheckbox1.DataSource = ViewState["OrigData"];
        GridViewCheckbox1.DataBind();
        DataTable dt = new DataTable();
        DataTable _clonedTable = new DataTable();
        dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value);
        if (!cookiesStorage.check_dataTable(dt))
        {
            _clonedTable = dtVendorDefault().Clone();
        }
        else
        {
            _clonedTable = dt.Clone();
        }
        
    }

    private void BindGrid(DataTable dt, GridView gv)
    {

        if (cookiesStorage.check_dataTable(dt))
        {

            dt = TrimEmptyRow(dt);

            gv.DataSource = SortData(dt);

            gv.DataBind();

        }
        else
        {

            ShowNoResultFound(dt, gv);

        }
        if (gv.ID == "GridViewCheckbox1")
            ViewState["OrigData"] = dt;
        else
        {
            ViewState["MovedData"] = dt;
        }

    }

    private DataTable SortData(DataTable dt)
    {

        DataView dv = dt.DefaultView;

        dv.Sort = "P10VEN ASC";

        dt = dv.ToTable();



        return dt;

    }



    private void MoveRows(DataTable dt, GridView gv)

    {
        string chkName = "CheckBoxInsert";
        if (gv.ID == "GridViewCheckbox1") { chkName = "CheckBoxInsert1"; }

        for (int i = gv.Rows.Count - 1; i >= 0; i--)

        {

            CheckBox cb = (CheckBox)gv.Rows[i].Cells[0].FindControl(chkName);

            if (cb != null)

            {

                if (cb.Checked)

                {

                    AddRow(dt.Rows[i], gv);

                    dt.Rows.Remove(dt.Rows[i]);

                }

            }

        }

        BindGrid(dt, gv);

    }

    private void AddRow(DataRow row, GridView gv)
    {

        DataTable dt = null;

        if (ViewState["MovedData"] == null)

        {

            dt = dtVendorDefault();

            dt.ImportRow(row);

            if (dt.Rows.Count > 1)
            {
                dt.Rows.Remove(dt.Rows[0]);
            }

            ViewState["MovedData"] = dt;

            BindGrid(dt, GridViewCheckbox2);

        }

        else

        {

            if (gv.ID == "GridViewCheckbox1")

            {

                dt = (DataTable)ViewState["MovedData"];

                dt.ImportRow(row);

                ViewState["MovedData"] = dt;

                BindGrid(dt, GridViewCheckbox2);

            }

            else

            {

                dt = (DataTable)ViewState["OrigData"];

                dt.ImportRow(row);

                ViewState["OrigData"] = dt;

                BindGrid(dt, GridViewCheckbox1);

            }

        }

    }
    private DataTable TrimEmptyRow(DataTable dt)

    {

        if (dt.Rows[0][0].ToString() == string.Empty)

        {

            dt.Rows.Remove(dt.Rows[0]);

        }

        return dt;

    }

    private void ShowNoResultFound(DataTable source, GridView gv)
    {
        if (GridViewCheckbox2.Rows.Count != 0)
        {

        }

        source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable

        // Bind the DataTable which contain a blank row to the GridView
        if (cookiesStorage.check_dataTable(source))
        {
            gv.DataSource = source;

            gv.DataBind();
        }
        

        // Get the total number of columns in the GridView to know what the Column Span should be

        int columnsCount = gv.Columns.Count;

        gv.Rows[0].Cells.Clear();// clear all the cells in the row

        gv.Rows[0].Cells.Add(new TableCell()); //add a new blank cell

        gv.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //You can set the styles here

        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;

        gv.Rows[0].Cells[0].Text = "NO ITEMS FOUND !";
        CHECK_DISIBLE_CHECKBOX();
        //deleteListData.Enabled = false;

    }

    protected void BTN_TRANSFER_CLICK(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in GridViewCheckbox1.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("CheckBoxInsert1");
            if (chk.Checked)
            {
                countCheckBox1++;
            }
        }
        if (countCheckBox1 <= 0)
        {
            lblMsgAlertApp.Text = "Please select at least 1 vendor.";
            PopupAlertApp.ShowOnPageLoad = true;
        }
        else
        {
            CHECK_ENABLE_CHECKBOX();
            //deleteListData.Enabled = true;
            DataTable dtOrigData = (DataTable)ViewState["OrigData"];
            MoveRows(SortData(dtOrigData), GridViewCheckbox1);
        }
        return;
    }

    protected void BTN_BACK_CLICK(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow2 in GridViewCheckbox2.Rows)
        {
            CheckBox chk2 = (CheckBox)gvrow2.FindControl("CheckBoxInsert");
            if (chk2.Checked)
            {
                countCheckBox2++;
            }
        }
        if (countCheckBox2 <= 0)
        {
            lblMsgAlertApp.Text = "Please select at least 1 vendor.";
            PopupAlertApp.ShowOnPageLoad = true;

        }
        else
        {
            DataTable dtMovedData = (DataTable)ViewState["MovedData"];
            MoveRows(SortData(dtMovedData), GridViewCheckbox2);
        }


    }

    private void CheckState(bool p)
    {
        foreach (GridViewRow row in GridViewCheckbox2.Rows)
        {
            CheckBox chkcheck = (CheckBox)row.FindControl("CheckBoxInsert");
            chkcheck.Checked = p;
        }
    }
    #endregion

    #region Default & create function

    private void set_Default()
    {
        txtRank.Text = "";
        txtvendorCode.Enabled = false;
        txtvendorName.Enabled = false;
        //setStartDate.Text = "";
        setEndDate.Enabled = false;
        //ContentControlStep1.Enabled = false;


    }
    private void set_Enabled(bool type)
    {
        //btnAdd.Enabled = type;
        //btnClearData.Enabled = type;
        btn_search.Enabled = type;
        //btn_clear.Enabled = type;

    }
    private void set_MsgSuccess()
    {
        if (btnAddData.Text.Trim() == "Add")
        {
            lblMsgSuccess.Text = "Add data success";
            PopupMsgSuccess.HeaderText = "Success";
            PopupMsgSuccess.ShowOnPageLoad = true;
            CLEAR_LIST_GRIDVIEW2();

        }
        else if (btnAddData.Text.Trim() == "Edit")
        {
            lblMsgSuccess.Text = "Edit data success";
            PopupMsgSuccess.HeaderText = "Success";
            PopupMsgSuccess.ShowOnPageLoad = true;
            CLEAR_LIST_GRIDVIEW2();
        }
        else if (confirmVendorDelete.Text.Trim() == "OK")
        {
            lblMsgSuccess.Text = "Delete vendor data success";
            PopupMsgSuccess.HeaderText = "Success";
            PopupMsgSuccess.ShowOnPageLoad = true;
            CLEAR_LIST_GRIDVIEW2();
        }



    }
    private void set_Msg()
    {
        lblMsg.Text = Msg;
        PopupMsg.HeaderText = MsgHText;
        PopupMsg.ShowOnPageLoad = true;
    }
    private void set_confirmMsg()
    {
        //lblConfirmMsgEN.Text = CMsg;
        //PopupConfirm.HeaderText = CHText;
        //PopupConfirm.ShowOnPageLoad = true;
    }
    private void set_clear_search()
    {

        ddl_SearchBy.SelectedIndex = 0;
        txt_Brand.Text = "";
    }
    private void set_default_edit()
    {
        //lblBranCode.Text = BrandCode;
        //txtBrandName.Text = BrandName;
        //txtBrandName.Focus();
        //btnAdd.Text = "Edit";
        //lbl_AddEdit.Text = "Edit Brand";
    }
    protected void set_clear_add(object sender, EventArgs e)
    {
        if (btnAddData.Text.Trim() == "Edit")
        {
            BindDatatoGridView();

            txtRank.Enabled = true;
            txtvendorCode.Text = "";
            txtvendorName.Text = "";
            txtSearchVendor.Text = "";
            startDate.Text = FORMAT_DATE(m_UpdDate.ToString());
            startDate.Enabled = true;
            CalendarExtender1.Enabled = true;
            calendarStart.Enabled = true;
            txtRank.Text = "";
            btnAddData.Text = "Add";
            lbl_RankOfVendor.Text = "Add Rank of Vendor";
        }
        else
        {
            //QueryData();
            txtvendorCode.Enabled = false;
            txtvendorName.Enabled = false;


            txtRank.Enabled = true;
            txtvendorCode.Text = "";
            txtvendorName.Text = "";
            txtSearchVendor.Text = "";
            //setStartDate.Text = "";
            //DateTime dt = DateTime.Now;
            //CalendarStartDate.SelectedDate = dt;
            //startDate.Text = System.DateTime.Now.ToString("dd/mm/yyyy");
            startDate.Text = FORMAT_DATE(m_UpdDate.ToString());
            startDate.Enabled = true;
            CalendarExtender1.Enabled = true;
            calendarStart.Enabled = true;
            txtRank.Text = "";
            btnAddData.Text = "Add";
            lbl_RankOfVendor.Text = "Add Rank of Vendor";

            BIND_DATA_GRIDVIEW();

            GridViewCheckbox1.Enabled = false;
            txtSearchVendor.Enabled = false;
            btnClear.Enabled = false;
            tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("Inserts");
        }
    }

    #endregion

    #region clear data
    //search
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        set_clear_search();
        GridView2.DataSource = null;
        GridView2.DataBind();
    }

    //add/edit
    protected void btnClearData_Click(object sender, EventArgs e)
    {


    }

    #endregion

    #region Bind Data and search
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Search_Data();
        BindDatatoGridView();
    }
    protected void Search_Data()
    {

        string sqlwhere = "";
        if (ddl_SearchBy.SelectedValue == "VC" && txt_Brand.Text.Trim() != "")
        {
            sqlwhere += "AND FORMAT(P16VEN,'000000000000') = '" + txt_Brand.Text.Trim() + "'";
        }
        if (ddl_SearchBy.SelectedValue == "VN" && txt_Brand.Text.Trim() != "")
        {
            sqlwhere += " AND P10NAM LIKE '" + txt_Brand.Text.ToUpper().Trim() + "%' ";
        }

        SqlAll = @"SELECT FORMAT(P16VEN,'000000000000') As P16VEN, P10NAM, P16RNK,CAST(P16STD AS VARCHAR) AS P16STD,CAST(P16END AS VARCHAR) AS P16END
                    FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK)
                    LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) on P10VEN = P16VEN
                    WHERE P16STS = '' ";
        SqlAll = SqlAll + sqlwhere + "  ORDER BY P16VEN, P16STD ";


        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        DateTime dateForm;
        if (cookiesStorage.check_dataset(DS))
        {
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                if (DateTime.TryParseExact(dr["P16STD"].ToString(), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateForm))
                {
                    dr["P16STD"] = dateForm.ToString("dd/MM/yyyy");
                }
                else if (dr["P16STD"].ToString() == "99999999")
                {
                    dr["P16STD"] = "99/99/9999";
                }

                if (DateTime.TryParseExact(dr["P16END"].ToString(), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateForm))
                {
                    dr["P16END"] = dateForm.ToString("dd/MM/yyyy");
                }
                else if (dr["P16END"].ToString() == "99999999")
                {
                    dr["P16END"] = "99/99/9999";
                }
            }

        }
        ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
    }

    protected void BindDatatoGridView()
    {
        DataSet ds = new DataSet();
        ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        if (cookiesStorage.check_dataset(ds))
        {
            GridView2.SelectedIndex = -1;
            GridView2.DataSource = ds;
            GridView2.DataBind();
        }
        else
        {
            dataCenter.CloseConnectSQL();
            return;
        }

        dataCenter.CloseConnectSQL();
        ResetGrid(GridView2, ds_Hiddengrid.Value);
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();
    }



    #endregion

    #region select page GridView2
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.SelectedIndex = -1;
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        GridView2.DataBind();
    }
    #endregion

    #region select page gvRank
    protected void gvRank_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRank.SelectedIndex = -1;
        gvRank.PageIndex = e.NewPageIndex;
        gvRank.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        gvRank.DataBind();
    }
    #endregion

    #region delete data
    protected void DELETE_DATA_LIST_CONFIRM(object sender, EventArgs e)
    {
        PopupConfirmDeleteVendor.ShowOnPageLoad = true;
    }
    protected void DELETE_DATA_LIST(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        bool transaction = dataCenter.Sqltr == null ? true : false;
        if (GridViewCheckbox2.Rows.Count > 0)
        {
            dtGridView2.Columns.AddRange(new DataColumn[1] { new DataColumn("P10VEN", typeof(string)) });
            foreach (GridViewRow row in GridViewCheckbox2.Rows)
            {
                if ((row.FindControl("CheckBoxInsert") as CheckBox).Checked)
                {

                    string P10VEN = row.Cells[1].Text;

                    dtGridView2.Rows.Add(P10VEN);

                }
            }
            for (int i = 0; i < dtGridView2.Rows.Count; i++)
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                SqlAll = "UPDATE AS400DB01.ILOD0001.ILMS10 SET P10DEL = 'X', P10PGM='IL_VENDOR', P10UPD=" + m_UpdDate + ", P10TIM=" + m_UpdTime + ", P10USR='" + m_userInfo.Username.ToString() + "', P10DSP='" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'" +
                    " WHERE P10VEN = " + dtGridView2.Rows[i]["P10VEN"] + "";
                cmd.CommandText = SqlAll;
                int updateILMS10 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                dataCenter.CommitMssql();
                cmd.Parameters.Clear();
            }

            dataCenter.CloseConnectSQL();
            set_MsgSuccess();
        }
        else
        {
            Msg = "Please select checkbox for delete vendor data";
            MsgHText = "Validate Delete";
            set_Msg();
        }
    }

    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.RowIndex];
        VendorCode = dr[0].ToString().Trim();
        VendorName = dr[1].ToString().Trim();
        tmpVenRank = dr[2].ToString().Trim();

        tmpStartDate = CHANGE_FORMAT_DATE(dr[3].ToString().Trim());
        tmpEndDate = CHANGE_FORMAT_DATE(dr[4].ToString().Trim());
        if (validateDelete())
        {
            m_userInfo = userInfoService.GetUserInfo();

            SqlAll = " UPDATE AS400DB01.ILOD0001.ILMS16  SET P16STS = 'X'"
                + ", P16UPD = " + m_UpdDate
                + ", P16UPT = " + m_UpdTime
                + ", P16UPU =  '" + m_userInfo.Username.ToString() + "'"
                + "  WHERE   P16VEN = " + VendorCode
                + "  AND P16STS = ''"
                + "  AND  P16RNK = '" + tmpVenRank + "'"
                + "  AND  P16STD = " + tmpStartDate
                + "  AND  P16END = " + tmpEndDate;

            txtSqlAll.Text = SqlAll;
            lblConfimMsg_Delete.Text = "Confirm Delete Brand Name : " + VendorName;
            PopupConfirmDelete.ShowOnPageLoad = true;
        }
        GridView2.Focus();
    }

    private bool validateDelete()
    {
        //SqlAll = " SELECT T44BRD FROM ILTB44 WHERE T44BRD = " + VendorCode + " AND  T44DEL= ''  AND T44LTY= '01'";
        //DataSet DS = new DataSet();
        //m_userInfo = userInfoService.GetUserInfo();
        //ilObj.UserInfomation = m_userInfo;

        //DS = ilObj.RetriveAsDataSet(SqlAll);


        //if (DS.Tables[0]?.Rows.Count > 0)
        //{
        //    Msg = "Found this Brand is uses by item ";
        //    MsgHText = "Validate Delete";
        //    //set_Msg();
        //    ilObj.CloseConnectioDAL();
        //    return false;
        //}
        //ilObj.CloseConnectioDAL();
        return true;
    }
    #endregion

    #region Confirm Delete
    protected void btnConfirm_OK_Click(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = txtSqlAll.Text;
        cmd.CommandText = SqlAll;
        bool transaction = dataCenter.Sqltr == null ? true : false;
        try
        {
            transaction = dataCenter.Sqltr == null ? true : false;
            int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                Msg = "Delete Vendor Name : " + VendorName + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            Msg = " Delete Vendor Name : " + VendorName + " complete.";
            set_Msg();

            Search_Data();
            BindDatatoGridView();

        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            Msg = "Delete Vendor Name : " + VendorName + " not complete.";
            set_Msg();
            return;
        }

    }


    protected void btnConfirm_Cancel_Click(object sender, EventArgs e)
    {
        GridView2.Focus();
    }
    #endregion

    #region Message all

    #endregion

    private void Search_Data_Popup()
    {
        string sqlwhere = "";
        SqlAll = @" SELECT T12RNK, CAST(SUBSTRING(CAST(T12RTE AS VARCHAR), 1, 3) AS DECIMAL(5, 2)) AS T12RTE, 
             SUBSTRING(CAST(T12STD AS NVARCHAR), 7, 2) + '/' + SUBSTRING(CAST(T12STD AS NVARCHAR), 5, 2) + '/' + SUBSTRING(CAST(T12STD AS NVARCHAR), 1, 4) AS AT12STD, 
             SUBSTRING(CAST(T12END AS NVARCHAR), 7, 2)+ '/' + SUBSTRING(CAST(T12END AS NVARCHAR), 5, 2) + '/' + SUBSTRING(CAST(T12END AS NVARCHAR), 1, 4) AS T12END, 
             T12STD  FROM AS400DB01.ILOD0001.ILTB12 WITH (NOLOCK)
             WHERE T12STS = '' ";

        if (ddl_popup_SearchBy.SelectedValue == "RC" && txt_Detail.Text.Trim() != "")
        {
            sqlwhere += " AND CAST(T12RNK as nvarchar) = '" + txt_Detail.Text.Trim() + "'";
        }
        if (ddl_popup_SearchBy.SelectedValue == "RD" && txt_Detail.Text.Trim() != "")
        {
            sqlwhere += " AND CAST(T12RTE as nvarchar) = '" + txt_Detail.Text.Trim() + "'";
        }


        SqlAll = SqlAll + sqlwhere + " ORDER BY T12RNK, T12STD ";
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            gvRank.DataSource = DS;
            gvRank.DataBind();
        }
        else
        {
            DataTable dt = dtSearchDataDefault();
    
            DataSet dsNull = new DataSet();
            dsNull.Tables.Add(dt);
            gvRank.DataSource = dsNull;
            gvRank.DataBind();
            dataCenter.CloseConnectSQL();
            return;
        }
        BIND_DATA_GRIDVIEW();
        dataCenter.CloseConnectSQL();
        return;
    }
    protected void btAddRank_OnClick(object sender, EventArgs e)
    {
        Search_Data_Popup();
        Popup_AddRank.ShowOnPageLoad = true;
    }

    protected void CheckBoxSelectAllProductTypeChanged(object sender, EventArgs e)
    {
        CheckBox cbSelectAll = (CheckBox)GridViewCheckbox1.HeaderRow.FindControl("cbSelectAll1");

        foreach (GridViewRow row in GridViewCheckbox1.Rows)
        {
            CheckBox CheckBoxInsert = (CheckBox)row.FindControl("CheckBoxInsert1");

            if (cbSelectAll.Checked == true)
            {
                CheckBoxInsert.Checked = true;
            }
            else
            {
                CheckBoxInsert.Checked = false;
            }
        }
        DataTable _clonedTable = cookiesStorage.JsonDeserializeObjectHiddenDataTable(dt_vendor_detail.Value).Clone();
        //ViewState["ClonedTable"] = _clonedTable;
        if (cookiesStorage.check_dataTable(_clonedTable)) 
        { 
            ShowNoResultFound(_clonedTable, GridViewCheckbox2);
        }
    }
    protected void CheckBoxSelectAllProductTypeSelectChanged(object sender, EventArgs e)
    {
        CheckBox cbSelectAll = (CheckBox)GridViewCheckbox2.HeaderRow.FindControl("cbSelectAll");

        foreach (GridViewRow row in GridViewCheckbox2.Rows)
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
    protected void btAdd_Click(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        DataSet ds_CheckDate = new DataSet();

         
        ilObj.UserInfomation = m_userInfo;
        ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = "SELECT DISTINCT(T12RNK) FROM AS400DB01.ILOD0001.ILTB12 WITH (NOLOCK) WHERE T12RNK = '" + txtRank.Text.Trim() + "'";
        cmd.CommandText = SqlAll;
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        SqlAll = "Select P97CDT from AS400DB01.ILOD0001.ILMS97 WITH (NOLOCK) where P97REC = '01'";
        cmd.CommandText = SqlAll;
        ds_CheckDate = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        DataRow drCheckDate = ds_CheckDate.Tables[0]?.Rows[0];
        string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
        var p97Date = drCheckDate["P97CDT"].ToString();
        if (decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
        {
            p97Date = DateTime.Now.ToString("yyyyMMdd",new CultureInfo("th-TH"));
        }
        dateCheck = int.Parse(p97Date);
        formatDateCheck = FORMAT_DATE(p97Date);
        startDateCheck = int.Parse(CHANGE_FORMAT_DATE(startDate.Text.Trim()));

        // start call procedures GNP023 "Start date ต้องไม่เกิน 1 ปี"
        string CalcDate = "";
        string Error = "";
        bool call_GNP023 = dataSubroutine.Call_GNP023(dateCheck.ToString(), "YMD", "B", "1", "Y", "+", ref CalcDate, ref Error, m_userInfo.BizInit, m_userInfo.BranchNo);
        ////ilObj.CloseConnectioDAL();
        // end call procedures GNP023 "Start date ต้องไม่เกิน 1 ปี"

        if (call_GNP023 = false || Error != "")
        {
            txtAlertError.Text = "Process error, please check procedures program 'GNP023'";
            PopupAlertMsgError.ShowOnPageLoad = true;
            BIND_DATA_GRIDVIEW();
        }
        else if (startDateCheck > int.Parse(CalcDate))
        {
            txtAlertError.Text = "Start date must not be more than current date 1 year.";
            PopupAlertMsgError.ShowOnPageLoad = true;
            BIND_DATA_GRIDVIEW();
        }
        else
        {
            if (btnAddData.Text.Trim() == "Add")
            {

                if (txtRank.Text.Trim() == "")
                {
                    txtAlertError.Text = "Please input rank code.";
                    PopupAlertMsgError.ShowOnPageLoad = true;
                    BIND_DATA_GRIDVIEW();
                }
                else if (!cookiesStorage.check_dataset(DS))
                {
                    txtAlertError.Text = "Can't find rank code, please choose rank code again.";
                    PopupAlertMsgError.ShowOnPageLoad = true;
                    BIND_DATA_GRIDVIEW();
                }
                else if (startDate.Text.Trim() == "")
                {
                    txtAlertError.Text = "Please select start date";
                    PopupAlertMsgError.ShowOnPageLoad = true;
                    BIND_DATA_GRIDVIEW();
                }
                else if (startDateCheck < dateCheck)
                {
                    txtAlertError.Text = "Start date must not be less than (" + formatDateCheck + ")";
                    PopupAlertMsgError.ShowOnPageLoad = true;
                    BIND_DATA_GRIDVIEW();
                }
                else
                {

                    dt.Columns.AddRange(new DataColumn[8] { new DataColumn("P16VEN", typeof(string)),
                        new DataColumn("P16RNK", typeof(string)),
                        new DataColumn("P16STD", typeof(int)),
                        new DataColumn("P16END", typeof(int)),
                        new DataColumn("P16UPD", typeof(ulong)),
                        new DataColumn("P16UPT", typeof(ulong)),
                        new DataColumn("P16UPU", typeof(string)),
                        new DataColumn("STD_OLD", typeof(int))
                         });

                    DataSet ds_CheckVendorDateBetween = new DataSet();
                    string strStartDate = CHANGE_FORMAT_DATE(startDate.Text.Trim());
                    bool checkShowPopUp = false;
                    int loopAll = GridViewCheckbox2.Rows.Count;
                    foreach (GridViewRow row in GridViewCheckbox2.Rows)
                    {
                        if ((row.FindControl("CheckBoxInsert") as CheckBox).Checked)
                        {
                            SqlAll = "SELECT MAX(P16STD) AS P16STD FROM AS400DB01.ILOD0001.ILMS16 WITH (NOLOCK) WHERE P16VEN = " + row.Cells[1].Text.Trim() + "  AND P16STS = ''";
                            cmd.CommandText = SqlAll;
                            ds_CheckVendorDateBetween = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                            DataRow dr_CheckVendorDateBetween = ds_CheckVendorDateBetween.Tables[0]?.Rows[0];
                            string checkDateLenght = dr_CheckVendorDateBetween["P16STD"].ToString();
                            int intDateLenght = 0;
                            bool isCheckDateLenght = false;

                            if (!string.IsNullOrEmpty(checkDateLenght))
                            {
                                intDateLenght = int.Parse(checkDateLenght);
                                if (int.Parse(strStartDate) < intDateLenght)
                                {
                                    isCheckDateLenght = true;
                                }
                            }

                            if (isCheckDateLenght)
                            {
                                txtAlertCheckDateBetween.Text = "Please check vendor \n Start date must more than " + FORMAT_DATE(strStartDate) + "\n for vendor : " + row.Cells[1].Text.Trim() + "";
                                PopupAlerCheckDateBetween.ShowOnPageLoad = true;

                                break;
                            }
                            else
                            {
                                string StartDateNew = strStartDate;
                                string P16VEN = row.Cells[1].Text;
                                string P16RNK = txtRank.Text;
                                int P16STD = int.Parse(StartDateNew);
                                int P16END = int.Parse("99999999");
                                int STD_OLD = intDateLenght;
                                ulong P16UPD = m_UpdDate;
                                ulong P16UPT = m_UpdTime;
                                string P16UPU = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();
                                dt.Rows.Add(P16VEN, P16RNK, P16STD, P16END, P16UPD, P16UPT, P16UPU, STD_OLD);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0 && dt.Rows.Count == loopAll)
                    {
                        try
                        {
                            ilObj.UserInfomation = m_userInfo;
                            bool transaction = dataCenter.Sqltr == null ? true : false;
                            DateTime dtime;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (DateTime.TryParseExact(dt.Rows[i]["P16STD"].ToString(), "yyyyMMdd",
                                                     CultureInfo.InvariantCulture,
                                                     DateTimeStyles.None, out dtime))
                                {
                                    Console.WriteLine(dtime);
                                }
                                string formattedDateTime = dtime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                                DateTime newFormatDates = Convert.ToDateTime(formattedDateTime).AddDays(-1);
                                string StartDateUpdate = newFormatDates.ToString("yyyyMMdd");

                                SqlAll = "UPDATE AS400DB01.ILOD0001.ILMS16 SET P16END = " + StartDateUpdate + ""
                                       + ", P16UPD = " + m_UpdDate
                                       + ", P16UPT = " + m_UpdTime
                                       + ", P16UPU = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                       + " WHERE FORMAT(P16VEN, '000000000000') = '" + dt.Rows[i]["P16VEN"] + "' AND P16STS = '' AND P16STD = " + dt.Rows[i]["STD_OLD"] + "";
                                cmd.CommandText = SqlAll;
                                int UPDATEILMS16 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                                if (UPDATEILMS16 == -1)
                                {
                                    dataCenter.RollbackMssql();
                                    dataCenter.CloseConnectSQL();

                                    Msg = "Can't save data , Please check command query : update ILMS16 ! ";
                                    MsgHText = "Error Query";
                                    set_Msg();
                                    checkShowPopUp = false;

                                    return;
                                }
                                else
                                {
                                    checkShowPopUp = true;
                                }

                                SqlAll = "INSERT INTO AS400DB01.ILOD0001.ILMS16(P16VEN, P16RNK, P16STD, P16END, P16UPD, P16UPT, P16UPU) VALUES(" + dt.Rows[i]["P16VEN"] + ",'" + dt.Rows[i]["P16RNK"].ToString() + "', " + dt.Rows[i]["P16STD"] + ", " + dt.Rows[i]["P16END"] + ", " + dt.Rows[i]["P16UPD"] + ", " + dt.Rows[i]["P16UPT"] + ", '" + dt.Rows[i]["P16UPU"] + "')";
                                cmd.CommandText = SqlAll;
                                transaction = dataCenter.Sqltr == null ? true : false;
                                int insertILMS16 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                                if (insertILMS16 == -1)
                                {
                                    dataCenter.RollbackMssql();
                                    dataCenter.CloseConnectSQL();

                                    Msg = "Can't save data , Please check command query : insert ILMS16 ! ";
                                    MsgHText = "Error Query";
                                    set_Msg();
                                    checkShowPopUp = false;

                                    return;
                                }
                                else
                                {
                                    checkShowPopUp = true;
                                }

                                dataCenter.CommitMssql();
                            }

                        }
                        catch (Exception)
                        {

                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();

                            Msg = "The data cannot be saved";
                            MsgHText = "Error";
                            set_Msg();
                            return;
                        }

                        cmd.Parameters.Clear();
                        dataCenter.CloseConnectSQL();

                        if (checkShowPopUp == true)
                        {
                            set_MsgSuccess();
                        }
                        else
                        {
                            Msg = "Please select checkbox for insert data";
                            MsgHText = "Validate Save";
                            set_Msg();
                        }

                    }
                    //else
                    //{
                    //Msg = "Please select checkbox for insert data";
                    //MsgHText = "Validate Save";
                    //set_Msg();

                    //}
                    cmd.Parameters.Clear();
                    dataCenter.CloseConnectSQL();
                }
            }

            else if (btnAddData.Text.Trim() == "Edit")
            {

                string StartDate = startDate.Text.Trim();
                string[] SplitDateStart = StartDate.Split('/');
                string StartDateNew = SplitDateStart[2] + SplitDateStart[1] + SplitDateStart[0];

                string EndDate = setEndDate.Text.Trim();
                string[] SplitDateEnd = EndDate.Split('/');
                string EndDateNew = SplitDateEnd[2] + SplitDateEnd[1] + SplitDateEnd[0];
                try
                {
                    
                    ilObj.UserInfomation = m_userInfo;
                    SqlAll = "UPDATE AS400DB01.ILOD0001.ILMS16 SET P16RNK = '" + txtRank.Text + "'"
                 + ",P16STD = " + StartDateNew
                 + ",P16END = " + EndDateNew
                 + ",P16UPD = " + m_UpdDate
                 + ",P16UPT =  " + m_UpdTime
                 + ",P16UPU =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                 + " WHERE FORMAT(P16VEN, '000000000000')= '" + txtvendorCode.Text + "' AND P16STS = ''  AND P16RNK = '" + hiddenRankCode.Text + "' AND P16STD = " + StartDateNew + " AND P16END = " + EndDateNew + "";

                    cmd.CommandText = SqlAll;
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int EditILMS16 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                    if (EditILMS16 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Can't save data , Please check command query : edit ILMS16 ! ";
                        MsgHText = "Error Query";
                        set_Msg();

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

                    Msg = "The data cannot be edit";
                    MsgHText = "Error";
                    set_Msg();
                    return;
                }
                Search_Data();
                BindDatatoGridView();
                set_MsgSuccess();
            }
        }
    }
    public void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];
        txtvendorCode.Text = dr[0].ToString().Trim();
        txtvendorName.Text = dr[1].ToString().Trim();
        txtRank.Text = dr[2].ToString().Trim();
        hiddenRankCode.Text = dr[2].ToString().Trim();
        //setStartDate.Text = dr[3].ToString().Trim();
        //setEndDate.Text = dr[4].ToString().Trim();
        btnAddData.Text = "Edit";
        lbl_RankOfVendor.Text = "Edit Rank of Vendor";

        startDate.Text = dr[3].ToString().Trim();
        setEndDate.Text = dr[4].ToString().Trim();
        //DateTime dt = Convert.ToDateTime(tmpStartDate);
        //CalendarStartDate.SelectedDate = dt;
        startDate.Enabled = false;
        CalendarExtender1.Enabled = false;
        calendarStart.Enabled = false;
        ResetGrid(GridView2, ds_Hiddengrid.Value);
    }
    protected void btn_popup_search_Click(object sender, EventArgs e)
    {
        Search_Data_Popup();
    }
    protected void btn_popup_clear_Click(object sender, EventArgs e)
    {
        ddl_popup_SearchBy.Text = "";
        txt_Detail.Text = "";
        Search_Data_Popup();
    }
    protected void BTN_OK_VALIDATION(object sender, EventArgs e)
    {
        BIND_DATA_GRIDVIEW();
    }
    protected void gvRank_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_gvRank = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        DataRow dr = ds_gvRank.Tables[0]?.Rows[(gvRank.PageIndex * Convert.ToInt16(gvRank.PageSize)) + e.NewSelectedIndex];

        txtRank.Text = gvRank.Rows[e.NewSelectedIndex].Cells[0].Text.ToString();

        GridViewCheckbox1.Enabled = true;
        txtSearchVendor.Enabled = true;
        btnClear.Enabled = true;
        Popup_AddRank.ShowOnPageLoad = false;
        BIND_DATA_GRIDVIEW();
    }


    protected void btSave_Click(object sender, EventArgs e)
    {
        DataTable dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(ds_grid_rv_New.Value);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblConfimMsg_Delete.Text = "Confirm Save Ranking";
            PopupConfirmSave.ShowOnPageLoad = true;
        }
        //Insert data
    }
    protected void btnConfirm_Add_OK_Click(object sender, EventArgs e)
    {
        PopupConfirmSave.ShowOnPageLoad = false;
    }
    protected void btnConfirm_Add_Cancel_Click(object sender, EventArgs e)
    {
        PopupConfirmSave.ShowOnPageLoad = false;
    }
    private void PopupMsgCenter()
    {
        PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        Popup_AddRank.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        Popup_AddRank.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupAlertApp.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupAlertApp.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDeleteVendor.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDeleteVendor.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupAlertMsgError.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupAlertMsgError.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupAlerCheckDateBetween.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupAlerCheckDateBetween.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
    }
    protected void search_checkbok_gv1(object sender, EventArgs e)
    {
        ContentControlStep2.Focus();
    }

    #region clear data gridview chekbox 2
    private void CLEAR_LIST_GRIDVIEW2()
    {

        ViewState.Remove("MovedData");
        ViewState.Remove("OrigData");
        ViewState.Remove("ClonedTable");
        BIND_DATA_GRIDVIEW();


    }
    #endregion

}