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

public partial class ManageData_WorkProcess_VendorProduct : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
    protected string sqlAll = "";
    protected string vendorId = "";
    protected string vendorName = "";
    protected string loanType = "";
    protected string loanTypeName = "";

    protected string Message = "";
    protected string MessageHeadText = "";

    protected string ConfirmSaveMessage = "";
    protected string ConfirmSaveMessageHeadText = "";

    protected string programName = "IL_VENPRO";

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
        if (Page.IsPostBack)
        {
            return;
        }
        if (!IsPostBack)
        {
            SetDefault();
        }
    }

    #region Default
    protected void SetDefault()
    {
        ds_product_type_select.Value = string.Empty;
        ds_product_type.Value = string.Empty;

        txtVendorID.Text = "";
        txtVendorName.Text = "";
        txtLoanType.Text = "";
        txtLoanTypeDesc.Text = "";

        gvProductType.DataSource = null;
        gvProductType.DataBind();

        gvProductTypeSelected.DataSource = null;
        gvProductTypeSelected.DataBind();

        btnAdd.Enabled = false;
        btnDelete.Enabled = false;
    }

    protected void ProductTypeBindData(string btnAction = "")
    {
        string productTypeSelect = "";
        List<string> productTypeSelectList = new List<string>();

        DataSet dsProductTypeSelect = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_type_select.Value);
        if (dsProductTypeSelect.Tables.Count <= 0 && btnAction.ToUpper() != "DELETE")
        {
            dsProductTypeSelect = LoadProductTypeSelect();
            ds_product_type_select.Value = cookiesStorage.SetHisdenValueByDataSet(dsProductTypeSelect);
        }

        if (cookiesStorage.check_dataset(dsProductTypeSelect))
        {
            ds_product_type_select.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductTypeSelect);
            gvProductTypeSelected.DataSource = dsProductTypeSelect;
            gvProductType.DataBind();

            DataTable dt = dsProductTypeSelect.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                productTypeSelectList.Add(dr["P13TYP"].ToString().Trim());
            }
            productTypeSelect = string.Join(", ", productTypeSelectList);

            btnDelete.Enabled = true;
        }
        else
        {
            btnDelete.Enabled = false;
        }

        if (dsProductTypeSelect != null && dsProductTypeSelect.Tables.Count > 0 && dsProductTypeSelect.Tables[0]?.Rows?.Count > 0)
        {
            DataTable dt = dsProductTypeSelect.Tables[0];

            var sortedRows = from row in dt.AsEnumerable()
                             orderby int.Parse(row["P13TYP"].ToString())
                             select row;

            DataTable sortedTable = sortedRows.CopyToDataTable();

            gvProductTypeSelected.DataSource = sortedTable;
            gvProductTypeSelected.DataBind();
        }
        else
        {
            gvProductTypeSelected.DataSource = null;
            gvProductTypeSelected.DataBind();

        }

        DataSet dsProductType = LoadProductType(productTypeSelect);
        if (cookiesStorage.check_dataset(dsProductType))
        {
            ds_product_type.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductType);
            gvProductType.DataSource = dsProductType;
            gvProductType.DataBind();
            btnAdd.Enabled = true;
        }
        else
        {
            btnAdd.Enabled = false;
        }
    }

    private DataSet LoadProductType(string productTypeSelect)
    {
        vendorId = txtVendorID.Text;
        DataSet ds = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string sqlWhere = "";
        if (!string.IsNullOrEmpty(productTypeSelect))
        {
            sqlWhere = "AND T40TYP NOT IN ( " + productTypeSelect.Trim() + " )";
        }

        sqlAll = @"SELECT
	                   T40TYP,
	                   T40DES
                   FROM
	                   AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                   WHERE
	                   T40DEL = ''
	                   " + sqlWhere + @"
                   ORDER BY
	                   T40TYP";
        ds = dataCenter.GetDataset<DataTable>(sqlAll, CommandType.Text).Result.data;

        return ds;
    }

    private DataSet LoadProductTypeSelect()
    {
        vendorId = txtVendorID.Text;
        DataSet ds = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        sqlAll = @"SELECT
	                   P13TYP,
	                   T40DES
                   FROM
	                   AS400DB01.ILOD0001.ILMS13 WITH (NOLOCK)
                   INNER JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                   ON	P13TYP = T40TYP
                   WHERE
                        P13DEL <> 'X'
	               AND  P13VEN = " + vendorId;

        ds = dataCenter.GetDataset<DataTable>(sqlAll, CommandType.Text).Result.data;

        return ds;
    }

    private DataSet LoadProductTypeSelectByHeader()
    {
        vendorId = txtVendorID.Text;
        DataSet ds = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        sqlAll = @"SELECT
	                   P13TYP,
	                   T40DES
                   FROM
	                  [AS400DB01].[ILOD0001].[ILMS10] WITH (NOLOCK)
				   INNER JOIN   AS400DB01.ILOD0001.ILMS13 WITH (NOLOCK)
				   ON P10PVN = P13VEN 				   
                   INNER JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                   ON	P13TYP = T40TYP

                   WHERE 
                        P13DEL <> 'X'
	               AND  P10VEN = " + vendorId;

        ds = dataCenter.GetDataset<DataTable>(sqlAll, CommandType.Text).Result.data;

        return ds;
    }
    #endregion

    #region Popup Vendor
    protected void btnSelectVendorClick(object sender, EventArgs e)
    {
        ddl_popup_SearchBy.SelectedIndex = 0;
        txt_Vendor.Text = "";
        PopupAddVendor.Text = "";
        LoadDataPopup();
        PopupAddVendor.ShowOnPageLoad = true;
    }

    private void LoadDataPopup()
    {
        string sqlwhere = "";

        sqlAll = @"SELECT
	                    DISTINCT P11VEN
	                    ,P10NAM
	                    ,T00LTY
	                    ,T00TNM
                    FROM
	                    AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                    LEFT JOIN
	                    AS400DB01.ILOD0001.ILMS11 WITH (NOLOCK)
                    ON
	                    P10VEN = P11VEN
                    LEFT JOIN
	                    AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                    ON
	                    P11LTY = T00LTY
                    WHERE
	                    P11DEL = ''  ";

        if (ddl_popup_SearchBy.SelectedValue == "V" && txt_Vendor.Text.Trim() != "")
        {
            sqlwhere += " AND CAST(P10VEN as nvarchar) = '" + txt_Vendor.Text.Trim() + "' or FORMAT(P10VEN,'000000000000') = '" + txt_Vendor.Text.Trim() + "'";
        }

        if (ddl_popup_SearchBy.SelectedValue == "D" && txt_Vendor.Text.Trim() != "")
        {
            sqlwhere += " AND P10TNM LIKE '" + txt_Vendor.Text.ToUpper().Trim() + "%' ";
        }

        sqlAll = sqlAll + sqlwhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(sqlAll, CommandType.Text).Result.data;
        if (!cookiesStorage.check_dataset(DS))
        {
            dataCenter.CloseConnectSQL();
            return;
        }
        ds_popup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
        GridView1.DataSource = DS;
        GridView1.DataBind();
        dataCenter.CloseConnectSQL();
        ResetGrid(GridView1, ds_popup.Value);
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        GridView1.DataBind();
    }

    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup.Value);
        if (cookiesStorage.check_dataset(ds_grid))
        {
            DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.NewSelectedIndex];
            txtVendorID.Text = dr[0].ToString().Trim();
            txtVendorName.Text = dr[1].ToString().Trim();
            txtLoanType.Text = dr[2].ToString().Trim();
            txtLoanTypeDesc.Text = dr[3].ToString().Trim();
            ds_product_type_select.Value = string.Empty;
            ds_product_type.Value = string.Empty;
            ProductTypeBindData();
            PopupAddVendor.ShowOnPageLoad = false;
            ResetGrid(GridView1, ds_popup.Value);
        }
        
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
        ddl_popup_SearchBy.SelectedIndex = 0;
        txt_Vendor.Text = "";
        PopupAddVendor.Text = "";
        LoadDataPopup();
    }
    #endregion

    #region Popup Message
    private void SetMessage()
    {
        lblMsg.Text = Message;
        PopupMsg.HeaderText = MessageHeadText;
        PopupMsg.ShowOnPageLoad = true;
    }
    private void SetMessageSuccess()
    {
        lblMsgSuccess.Text = Message;
        PopupMsgSuccess.HeaderText = MessageHeadText;
        PopupMsgSuccess.ShowOnPageLoad = true;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        btnVendor.Focus();
    }
    #endregion

    #region Add/Delete Temp
    protected void CheckBoxSelectAllProductTypeChanged(object sender, EventArgs e)
    {
        CheckBox cbSelectAll = (CheckBox)gvProductType.HeaderRow.FindControl("cbSelectAll");

        foreach (GridViewRow row in gvProductType.Rows)
        {
            CheckBox cbSelext = (CheckBox)row.FindControl("cbSelect");

            if (cbSelectAll.Checked == true)
            {
                cbSelext.Checked = true;
            }
            else
            {
                cbSelext.Checked = false;
            }
        }
    }

    protected void CheckBoxSelectAllProductTypeSelectChanged(object sender, EventArgs e)
    {
        CheckBox cbSelectAll = (CheckBox)gvProductTypeSelected.HeaderRow.FindControl("cbSelectAll");

        foreach (GridViewRow row in gvProductTypeSelected.Rows)
        {
            CheckBox cbSelext = (CheckBox)row.FindControl("cbSelect");

            if (cbSelectAll.Checked == true)
            {
                cbSelext.Checked = true;
            }
            else
            {
                cbSelext.Checked = false;
            }
        }
    }

    protected void btnAddClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (!string.IsNullOrEmpty(ds_product_type_select.Value) && ds_product_type_select.Value.Contains("Table"))
        {
            dt = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_type_select.Value).Tables?[0];
        }
        else
        {
            dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(ds_product_type_select.Value);

        }
        if (dt == null || dt.Rows?.Count == 0)
        {
            dt.Columns.Add("P13TYP");
            dt.Columns.Add("T40DES");
        }
        int countSelected = 0;
        if(dt.Rows?.Count > 0 && String.IsNullOrEmpty(dt.Rows[0]["P13TYP"].ToString()) && String.IsNullOrEmpty(dt.Rows[0]["T40DES"].ToString()))
        {
           dt.Rows.RemoveAt(0);
        }
        foreach (GridViewRow gvRow in gvProductType.Rows)
        {
            CheckBox cbSelect = (CheckBox)gvRow.FindControl("cbSelect");
            if (cbSelect != null && cbSelect.Checked)
            {
                string productTypeCode = gvProductType.DataKeys[gvRow.RowIndex].Value.ToString();
                string productTypeDescription = gvRow.Cells[2].Text.Trim();
                DataRow dr = dt.NewRow();
                dr["P13TYP"] = productTypeCode;
                dr["T40DES"] = productTypeDescription;
                dt.Rows.Add(dr);
                countSelected++;
            }
        }

        if (countSelected > 0)
        {
            dt.DefaultView.Sort = "P13TYP";
            dt = dt.DefaultView.ToTable();
            dt.AcceptChanges();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds_product_type_select.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
            ProductTypeBindData();
        }
        else
        {
            MessageHeadText = "Validsate";
            Message = "Please Select Product Type to Add";
            SetMessage();
        }
    }

    protected void btnDeleteClick(object sender, EventArgs e)
    {
        DataSet ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_type_select.Value);
        DataTable dt = ds.Tables[0];
        List<string> productTypeCodeList = new List<string>();

        int countSelected = 0;
        foreach (GridViewRow gvRow in gvProductTypeSelected.Rows)
        {
            CheckBox cbSelect = (CheckBox)gvRow.FindControl("cbSelect");
            if (cbSelect != null && cbSelect.Checked)
            {
                string productTypeCode = gvProductTypeSelected.DataKeys[gvRow.RowIndex].Value.ToString();
                productTypeCodeList.Add(productTypeCode);
                countSelected++;
            }
        }

        if (countSelected > 0)
        {
            foreach (string productTypeCode in productTypeCodeList)
            {
                DataRow[] drProcuctType = dt.Select("P13TYP = '" + productTypeCode + "'");
                foreach (DataRow dr in drProcuctType)
                {
                    if (dr["P13TYP"].ToString().Trim().ToUpper().Contains(productTypeCode.ToUpper()))
                    {
                        dt.Rows.Remove(dr);
                    }
                }
            }

            dt.AcceptChanges();
            ds_product_type_select.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
            ProductTypeBindData("DELETE");
        }
        else
        {
            MessageHeadText = "Validsate";
            Message = "Please Select Product Type to Delete";
            SetMessage();
        }
    }
    #endregion

    #region Save/Clear
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string vendorID = txtVendorID.Text.Trim();
        if (string.IsNullOrEmpty(vendorID))
        {
            Message = "Please Select Vendor";
            MessageHeadText = "Validate";
            SetMessage();

            return;
        }

        ConfirmSaveMessageHeadText = "Confirm";
        ConfirmSaveMessage = "Comfirm to Save Vendor Product?";
        SetConfirmSaveMessage();
    }

    protected void SaveVendorProduct()
    {
        List<string> productTypeCodeSelectList = new List<string>();
        foreach (GridViewRow gvRow in gvProductTypeSelected.Rows)
        {
            string productTypeCode = gvProductTypeSelected.DataKeys[gvRow.RowIndex].Value.ToString();
            productTypeCodeSelectList.Add(productTypeCode);
        }

        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        string currentUser = m_userInfo.Username.ToString();
        string userWorkStation = string.Empty;
        if (m_userInfo.LocalClient == "VIEW-LOCALHOST")
        {
            userWorkStation = "LOCALHOST";
        }
        else
        {
            userWorkStation = m_userInfo.LocalClient;
        }
         
        vendorId = txtVendorID.Text.Trim();
        loanType = txtLoanType.Text.Trim();
        dataCenter = new DataCenter(m_userInfo);
        /*** Comment for Test ***/
        string currentDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string currentTime = DateTime.Now.ToString("HHmmss", m_DThai);
        //string Curtime = "";
        //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
        //ilObj.CloseConnectioDAL();
        /************************/

        List<string> sqlList = new List<string>();
        string sql = "";
        string productTypeCodeSelect = string.Join(",", productTypeCodeSelectList);

        #region Delete
        sql = @"UPDATE
                    AS400DB01.ILOD0001.ILMS13
                SET 
                    P13DEL = 'X',
                    P13UPD = " + currentDate + @",
                    P13TIM = " + currentTime + @",
                    P13USR = '" + currentUser + @"',
                    P13PGM = '" + programName + @"',
                    P13DSP = '" + userWorkStation + @"'
                WHERE
                    P13LTY = '" + loanType + @"' 
                AND P13VEN = " + vendorId;

        if (!string.IsNullOrEmpty(productTypeCodeSelect))
        {
            sql += " AND P13TYP NOT IN(" + productTypeCodeSelect + ")";
        }
        sqlList.Add(sql);
        #endregion

        #region Add/Edit
        if (productTypeCodeSelectList.Count > 0)
        {
            //Select for Check Exist Data
            sql = @"SELECT
                    P13RRNO, P13VEN, P13LTY, P13TYP, P13FIL, P13UPD, P13TIM, P13USR, P13PGM, P13DSP, P13DEL
                FROM
                    AS400DB01.ILOD0001.ILMS13 WITH (NOLOCK)
                WHERE
                    P13LTY = '" + loanType + @"'
                AND P13VEN = " + vendorId + @"
                AND P13TYP IN (" + productTypeCodeSelect + ")";

            DataSet ds = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;
            ds = dataCenter.GetDataset<DataTable>(sql,CommandType.Text).Result.data;
            DataTable dt = new DataTable();
            if (cookiesStorage.check_dataset(ds))
            {
                dt = ds.Tables[0];
            }

            List<string> productTypeCodeUpdateList = new List<string>();
            List<string> productTypeCodeInsertList = new List<string>();

            if (dt.Rows.Count > 0)
            {
                foreach (string productTypeCode in productTypeCodeSelectList)
                {
                    decimal code = Convert.ToDecimal(productTypeCode);
                    var existData = dt.AsEnumerable().FirstOrDefault(x => x.Field<decimal>("P13TYP") == code);

                    if (existData != null)
                    {
                        productTypeCodeUpdateList.Add(productTypeCode);
                    }
                    else
                    {
                        productTypeCodeInsertList.Add(productTypeCode);
                    }
                }
            }
            else
            {
                productTypeCodeInsertList.Add(productTypeCodeSelect);
            }

            if (productTypeCodeUpdateList.Count > 0)
            {
                string productTypeCodeUpdate = string.Join(",", productTypeCodeUpdateList);

                sql = @"UPDATE
                            AS400DB01.ILOD0001.ILMS13
                        SET
                            P13DEL = '',
                            P13UPD = " + currentDate + @",
                            P13TIM = " + currentTime + @",
                            P13USR = '" + currentUser + @"',
                            P13PGM = '" + programName + @"',
                             P13DSP = '" + userWorkStation + @"'
                        WHERE
                            P13LTY = '" + loanType + @"'
                        AND P13VEN = " + vendorId + @"
                        AND P13TYP in (" + productTypeCodeUpdate + ")";

                sqlList.Add(sql);
            }

            if (productTypeCodeInsertList.Count > 0)
            {
                string productTypeCodeInsert = string.Join(",", productTypeCodeInsertList);

                sql = @"INSERT INTO AS400DB01.ILOD0001.ILMS13
                        (
                           P13TYP,
                           P13LTY,
                           P13VEN,
                           P13UPD,
                           P13TIM,
                           P13USR,
                           P13PGM,
                           P13DSP
                        )             
                           SELECT
                               T40TYP,
                               '" + loanType + @"',
                               " + vendorId + @",
                               " + currentDate + @",
                               " + currentTime + @",
                               '" + currentUser + @"',
                               '" + programName + @"',
                               '" + userWorkStation + @"'
                           FROM
                               AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                           WHERE
                               T40TYP IN (" + productTypeCodeInsert + ")";

                sqlList.Add(sql);
            }
        }
        #endregion

        SaveDB(sqlList);
    }

    private void SaveDB(List<string> sql)
    {
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iDB2Command cmd = new iDB2Command();
        dataCenter = new DataCenter(m_userInfo);

        try
        {
            foreach (var sqlCommand in sql)
            {
                cmd.CommandText = sqlCommand;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int result = dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
                if (result == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Save Product Type Not Complete.";
                    SetMessage();
                    return;
                }
                dataCenter.CommitMssql();
            }

            Message = "Save Product Type Complete.";
            SetMessageSuccess();

            ds_product_type_select.Value = string.Empty;
            ds_product_type.Value = string.Empty;
            ProductTypeBindData();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Save Product Type Not Complete";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            cmd.Parameters.Clear();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        SetDefault();
    }

    protected void btnLoadHeader_Click(object sender, EventArgs e)
    {
        string productTypeSelect = "";
        List<string> productTypeSelectList = new List<string>();

        DataSet dsProductTypeSelect = LoadProductTypeSelectByHeader();

        if (cookiesStorage.check_dataset(dsProductTypeSelect))
        {
            ds_product_type_select.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductTypeSelect);
            gvProductTypeSelected.DataSource = dsProductTypeSelect;
            gvProductType.DataBind();

            DataTable dt = dsProductTypeSelect.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                productTypeSelectList.Add(dr["P13TYP"].ToString().Trim());
            }
            productTypeSelect = string.Join(", ", productTypeSelectList);

            btnDelete.Enabled = true;
        }
        else
        {
            btnDelete.Enabled = false;
        }

        if (dsProductTypeSelect.Tables.Count != 0)
        {
            gvProductTypeSelected.DataSource = dsProductTypeSelect;
            gvProductTypeSelected.DataBind();
            DataSet dsProductType = LoadProductType(productTypeSelect);
            if (cookiesStorage.check_dataset(dsProductType))
            {
                ds_product_type.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductType);
                gvProductType.DataSource = dsProductType;
                gvProductType.DataBind();
                btnAdd.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
            }
        }
                
    }
    #endregion

    #region Popup Confirm Save
    protected void SetConfirmSaveMessage()
    {
        PopupConfirmSave.HeaderText = ConfirmSaveMessageHeadText;
        lblPopupConfirmSaveMessage.Text = ConfirmSaveMessage;
        PopupConfirmSave.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmSaveOK_Click(object sender, EventArgs e)
    {
        SaveVendorProduct();
    }
    #endregion
    private void PopupMsgCenter()
    {
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }

}