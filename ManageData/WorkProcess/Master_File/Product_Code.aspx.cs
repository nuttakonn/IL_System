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

public partial class ManageData_WorkProcess_Product_Code : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
    public ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    protected string LoadType = "";
    protected string LoadTypeName = "";
    protected string ProductType = "";
    protected string ProductTypeDes = "";
    protected string ProductCode = "";
    protected string ProductDesc = "";
    protected string LoanType = "";
    protected string SqlAll = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";
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
            PopupMsgCenter();
            Search_Data();
        }
    }
    public static class Globals
    {
        public static String productCodeIdShow;
    }
    #region Bind Data and 

    protected void btn_search_Click(object sender, EventArgs e)
    {
        Search_Data();

    }

    void Search_Data()
    {
        //   iDB2Command cmd = new iDB2Command();
        //   E_error.Text = "";
        //   string sqlwhere = "";
        //   SqlAll = @"SELECT 
        //	tb41.T41LTY,T00TNM ,tb41.T41TYP,TB40.T40DES,tb41.T41COD,tb41.T41DES,tb40.T40LTY
        //FROM AS400DB01.ILOD0001.iltb41 as tb41 WITH (NOLOCK)
        //LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) ON T41LTY=T00LTY  
        //LEFT JOIN AS400DB01.ILOD0001.iltb40 as tb40 WITH (NOLOCK) on  tb40.T40TYP = tb41.T41TYP   WHERE tb41.T41DEL = '' ";
        //   if (ddl_SearchBy.SelectedValue == "PT" && txt_Product.Text.Trim() != "")
        //   {
        //       sqlwhere += " AND CAST(T41TYP as nvarchar) = '" + txt_Product.Text.Trim() + "'";
        //   }
        //   if (ddl_SearchBy.SelectedValue == "PTD" && txt_Product.Text.Trim() != "")
        //   {
        //       sqlwhere += " AND CAST(T40DES as nvarchar)  LIKE '" + txt_Product.Text.ToUpper().Trim() + "%' ";
        //   }
        //   if (ddl_SearchBy.SelectedValue == "PC" && txt_Product.Text.Trim() != "")
        //   {
        //       sqlwhere += " AND CAST(T41COD as nvarchar) = '" + txt_Product.Text.Trim() + "'";
        //   }
        //   if (ddl_SearchBy.SelectedValue == "PD" && txt_Product.Text.Trim() != "")
        //   {
        //       sqlwhere += $" AND T41DES LIKE '%{txt_Product.Text.ToUpper().Trim()}%' ";
        //   }
        //   //---------------sql


        //   SqlAll = SqlAll + sqlwhere;
        //   cmd.CommandText = SqlAll;

        string searchBy = ddl_SearchBy.SelectedValue;
        string searchText = txt_Product.Text.Trim();

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetProductCode(searchBy, searchText, 1, 15);
        if (cookiesStorage.check_dataset(DS))
        {
            ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView1.DataSource = DS;
            GridView1.DataBind();
            DataRow dr = DS.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = GridView1.PageIndex * GridView1.PageSize + 1;
            int _CurrentRecEnd = GridView1.PageIndex * GridView1.PageSize + GridView1.Rows.Count;

            lblGridView1.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T41LTY");
            dt.Columns.Add("T00TNM");
            dt.Columns.Add("T41TYP");
            dt.Columns.Add("T40DES");
            dt.Columns.Add("T41COD");
            dt.Columns.Add("T41DES");
            dt.Columns.Add("T40LTY");
            DS.Tables.Add(dt);
            GridView1.DataSource = DS;
            GridView1.DataBind();
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
        ResetGrid(GridView1, ds_Hiddengrid.Value);
    }
    #endregion

    #region Default
    private void set_Enabled(bool type)
    {
        btnAdd.Enabled = type;
        btnClearData.Enabled = type;
        btn_search.Enabled = type;
        btnProductType.Enabled = type;
    }
    private void set_Msg()
    {
        lblMsg.Text = Msg;
        PopupMsg.HeaderText = MsgHText;
        PopupMsg.ShowOnPageLoad = true;
    }
    private void set_MsgSuccess()
    {
        lblMsgSuccess.Text = Msg;
        PopupMsgSuccess.HeaderText = MsgHText;
        PopupMsgSuccess.ShowOnPageLoad = true;
    }
    private void set_confirmMsg()
    {
        lblConfirmMsgEN.Text = CMsg;
        PopupConfirm.HeaderText = CHText;
        PopupConfirm.ShowOnPageLoad = true;
    }
    private void set_clear_search()
    {
        E_error.Text = "";
        ddl_SearchBy.SelectedIndex = 0;
        txt_Product.Text = "";
    }
    private void set_default_edit()
    {
        btnProductType.Focus();
        lblLoanType.Text = "01";
        lblLoanTypeDesc.Text = "สินเชื่อเงินผ่อน";
        txtProductType.Text = ProductType;
        txtProductTypeDesc.Text = ProductTypeDes;
        txtProductCode.Text = ProductCode;
        txtProductDesc.Text = ProductDesc;

        btnAdd.Text = "Edit";
        lbl_AddEdit.Text = "Edit Product Code";
    }
    private void set_clear_add_edit()
    {
        btnProductType.Focus();
        lblLoanType.Text = "";
        lblLoanTypeDesc.Text = "";
        txtProductType.Text = "";
        txtProductTypeDesc.Text = "";
        txtProductCode.Text = "";
        txtProductDesc.Text = "";
        btnAdd.Text = "Add";
        lbl_AddEdit.Text = "Add Product Code";
        btnProductType.Enabled = true;
    }

    #endregion

    #region select page
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        GridView1.DataBind();
    }
    #endregion

    #region clear data
    //search
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        set_clear_search();
        Search_Data();
        /*GridView1.DataSource = null;
        GridView1.DataBind();*/

    }

    //add/edit
    protected void btnClearData_Click(object sender, EventArgs e)
    {
        set_clear_add_edit();

    }

    #endregion

    #region select data
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.NewSelectedIndex];

        LoadType = dr[0].ToString().Trim();
        LoadTypeName = dr[1].ToString().Trim();
        ProductType = dr[2].ToString().Trim();
        ProductTypeDes = dr[3].ToString().Trim();
        ProductCode = dr[4].ToString().Trim();
        ProductDesc = dr[5].ToString().Trim();

        set_default_edit();

        btnProductType.Enabled = false;
        txtProductDesc.Focus();

        ResetGrid(GridView1, ds_Hiddengrid.Value);
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();
    }

    #endregion

    #region delete data
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.RowIndex];
        ProductCode = dr[4].ToString().Trim();
        ProductDesc = dr[5].ToString().Trim();
        ProductType = dr[2].ToString().Trim();
        Globals.productCodeIdShow = dr[4].ToString().Trim();
        if (validateDelete())
        {
            m_userInfo = userInfoService.GetUserInfo();
            //string Curtime = "";
            //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
            //ilObj.CloseConnectioDAL();
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = " Update  AS400DB01.ILOD0001.ILTB41 SET T41DEL = 'X' "
                  + ", T41UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                  + ", T41UDT = " + m_UpdTime.ToString()
                  + ", T41USR = '" + m_userInfo.Username.ToString() + "'"
                  + ", T41DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                  + "  WHERE T41LTY = '01'  AND T41TYP = " + ProductType + " AND T41COD =" + ProductCode;

            txtProduct_D.Text = ProductDesc;
            txtSqlAll.Text = SqlAll;
            lblConfimMsg_Delete.Text = "Confirm Delete Product Code  ";
            PopupConfirmDelete.ShowOnPageLoad = true;
        }
    }

    private bool validateDelete()
    {
        SqlAll = "SELECT T48COD FROM AS400DB01.ILOD0001.ILTB48 WITH (NOLOCK) WHERE T48LTY = '01' AND CAST(T48TYP as nvarchar) = '" + ProductType + "' AND CAST(T48COD as nvarchar) = '" + ProductCode + "' AND T48DEL = '' ";
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found Product code-Brand Uses in Model! Can not Delete ";
            MsgHText = "Validate Delete";
            set_Msg();

            dataCenter.CloseConnectSQL();
            return false;
        }
        dataCenter.CloseConnectSQL();
        return true;
    }

    #endregion

    #region insert and update data
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        ProductCode = txtProductCode.Text.Trim();
        ProductType = txtProductType.Text.Trim();
        ProductDesc = txtProductDesc.Text.ToUpper().Trim();

        if (ProductType == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Product Type";
            set_Msg();
            btnProductType.Focus();

        }
        else if (ProductDesc == "")
        {
            MsgHText = "Validate";
            Msg = " Please Product Description";
            set_Msg();
            txtProductDesc.Focus();

        }
        else
        {
            if (ProductCode == "")
            {
                SqlAll = "SELECT T41LTY, T41TYP, T41COD, T41DES, T41RSV, T41UDD, T41UDT, T41PGM, T41USR, T41DSP, T41DEL FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41DEL = '' AND T41LTY = '01' AND CAST(T41TYP as nvarchar) = '" + ProductType + "' AND T41DES = '" + ProductDesc + "'";
            }
            else
            {
                SqlAll = "SELECT T41LTY, T41TYP, T41COD, T41DES, T41RSV, T41UDD, T41UDT, T41PGM, T41USR, T41DSP, T41DEL FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41DEL = '' AND  T41LTY = '01' AND CAST(T41TYP as nvarchar) = '" + ProductType + "' AND T41DES  = '" + ProductDesc + "' AND CAST(T41COD as nvarchar)  <> '" + ProductCode + "'";
            }

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(DS))
            {
                Msg = "Found this Product Code is Duplicate ";
                MsgHText = "Validate Save";
                set_Msg();
                dataCenter.CloseConnectSQL();
            }
            else
            {

                CHText = "Confirm Save";
                CMsg = "Confirm Save Product Code ";
                set_confirmMsg();
            }
            dataCenter.CloseConnectSQL();
        }
    }


    #endregion

    #region Confirm Add/Edit
    protected void btnConfirmOK_Click(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string NameButton = "";
        string PrdCdePopupMsg = "";
        //string Curtime = "";
        //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
        //ilObj.CloseConnectioDAL();

        ProductType = txtProductType.Text.Trim();
        ProductTypeDes = txtProductTypeDesc.Text.ToUpper().Trim();
        ProductCode = txtProductCode.Text.Trim();
        ProductDesc = txtProductDesc.Text.ToUpper().Trim();

        if (ProductCode == "") // insert
        {
            SqlAll = "SELECT T41COD,T41TYP FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41DEL = 'X' AND T41LTY = '01' AND CAST(T41TYP as nvarchar) = '" + ProductType + "' AND T41DES = '" + ProductDesc + "'";
            DataSet DS1 = new DataSet();
            
            ilObj.UserInfomation = m_userInfo;
            DS1 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(DS1))
            {
                DataRow dr = DS1.Tables[0]?.Rows[0];
                ProductCode = dr[0].ToString().Trim();
                ProductType = dr[1].ToString().Trim();
                NameButton = "Edit";
                MsgHText = NameButton;
                PrdCdePopupMsg = ProductCode;

                SqlAll = " Update AS400DB01.ILOD0001.ILTB41 SET T41DEL = ''  "
                                              + ",  T41UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                                              + ",  T41UDT = " + m_UpdTime.ToString()
                                              + ",  T41USR = '" + m_userInfo.Username.ToString() + "'"
                                              + ",  T41PGM ='IL_PROCODE'"
                                              + ",  T41DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                              + ",  T41DES = '" + ProductDesc + "'"
                                              + ",  T41LTY= '01' "
                                              + "   WHERE T41LTY = '01'  AND T41TYP= " + ProductType + " AND T41COD = " + ProductCode;
            }
            else
            {
                NameButton = "Add";
                MsgHText = NameButton;
                LoanType = lblLoanType.Text.ToString().Trim();
                DataSet DS = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                SqlAll = "SELECT MAX(T41COD) as Running  FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41LTY = '" + LoanType + "' AND CAST(T41TYP as nvarchar) = '" + ProductType + "'";
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                int Running = 1;
                if (cookiesStorage.check_dataset(DS))
                {
                    dr = DS.Tables[0]?.Rows[0];
                    if (DS != null)
                    {
                        if (DS.Tables[0]?.Rows.Count > 0)
                        {
                            string maxtype = dr[0].ToString().Trim();
                            if (maxtype == "")
                            {
                                maxtype = "0";
                            }
                            Running = Convert.ToInt32(maxtype) + Running;
                        }
                    }
                }
                ProductCode = Running.ToString();
                dataCenter.CloseConnectSQL();
                PrdCdePopupMsg = Running.ToString();

                SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB41(
                                                 T41LTY,
                                                 T41TYP,
                                                 T41COD,
                                                 T41DES,
                                                 T41UDD,
                                                 T41UDT,
                                                 T41USR,
                                                 T41PGM,
                                                 T41DSP,
                                                 T41DEL)
                                    VALUES(     
                                                '{LoanType}',
                                                {ProductType},
                                                {Running},
                                                '{ProductDesc}',
                                                {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                'IL_PROCODE',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                                '{string.Empty}'
                                    )";


            }

        }
        else // update
        {
            NameButton = "Edit";
            MsgHText = NameButton;
            PrdCdePopupMsg = ProductCode;
            SqlAll = $@"Update AS400DB01.ILOD0001.ILTB41 
                                SET T41DEL = '{string.Empty}',
                                    T41UDD = {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},                   
                                    T41UDT = {m_UpdTime.ToString()},
                                    T41USR = '{m_userInfo.Username.ToString()}',
                                    T41PGM = 'IL_PROCODE',
                                    T41DSP = '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                    T41DES = '{ProductDesc}',
                                    T41LTY = '01'
                                WHERE T41LTY = '01'  AND T41TYP= " + ProductType + " AND T41COD = " + ProductCode;

        }


        cmd.CommandText = SqlAll;

        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            var resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (resHome11.afrows == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = NameButton + " Product Code : " + PrdCdePopupMsg + " - " + ProductDesc + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Product Code : " + PrdCdePopupMsg + " - " + ProductDesc + " complete.";
            set_MsgSuccess();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Product Code : " + PrdCdePopupMsg + " - " + ProductDesc + " not complete.";
            set_Msg();

            return;
        }
    }

    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        btnProductType.Focus(); set_Enabled(true);
    }
    #endregion

    #region Confirm Delete
    protected void btnConfirm_OK_Click(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = txtSqlAll.Text;
        cmd.CommandText = SqlAll;
        ProductDesc = txtProduct_D.Text;
        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            var resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            //int resHome11 = ilObj.ExecuteNonQuery(cmd);
            //if (resHome11 == -1)
            if (resHome11.afrows == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                Msg = "Delete Product Description  : " + Globals.productCodeIdShow + " - " + ProductDesc + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            Msg = " Delete Product Description : " + Globals.productCodeIdShow + " - " + ProductDesc + " complete.";
            set_MsgSuccess();
            GridView1.Focus();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            Msg = "Delete Product Description : " + Globals.productCodeIdShow + " - " + ProductDesc + " not complete.";
            set_Msg();
            return;
        }

    }
    protected void btnConfirm_Cancel_Click(object sender, EventArgs e)
    {
        GridView1.Focus();

    }
    #endregion

    #region Message all
    protected void btnOK_Click(object sender, EventArgs e)
    {
        btnProductType.Focus();
    }
    #endregion

    #region pop up  ProductType

    private void LoadDataPopup()
    {
        E_popup_error.Text = "";
        string sqlwhere = "";
        SqlAll = @"SELECT T40LTY,T00TNM ,T40TYP,T40DES 
				    FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
				    LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) ON T40LTY=T00LTY
				    WHERE T40DEL = '' ";

        if (ddl_popup_SearchBy.SelectedValue == "PT" && txt_ProductType.Text.Trim() != "")
        {
            sqlwhere += " AND CAST(T40TYP as nvarchar) = '" + txt_ProductType.Text.Trim() + "'";
        }
        if (ddl_popup_SearchBy.SelectedValue == "PD" && txt_ProductType.Text.Trim() != "")
        {
            sqlwhere += " AND T40DES like '" + txt_ProductType.Text.ToUpper().Trim() + "%' ";
        }
        //---------------sql

        SqlAll = SqlAll + sqlwhere + " ORDER BY T40TYP ASC ";
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_Hiddenpopup.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView2.DataSource = DS;
            GridView2.DataBind();
        }
        else
        {
            dataCenter.CloseConnectSQL();
            return;
        }

        dataCenter.CloseConnectSQL();

    }

    protected void btnProductType_Click(object sender, EventArgs e)
    {
        E_popup_error.Text = "";
        ddl_popup_SearchBy.SelectedIndex = 0;
        txt_ProductType.Text = "";
        LoadDataPopup();
        Popup_AddProductType.ShowOnPageLoad = true;
    }


    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddenpopup.Value);
        GridView2.DataBind();
    }
    protected void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddenpopup.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];
        lblLoanType.Text = dr[0].ToString().Trim();
        lblLoanTypeDesc.Text = dr[1].ToString().Trim();
        txtProductType.Text = dr[2].ToString().Trim();
        txtProductTypeDesc.Text = dr[3].ToString().Trim();

        Popup_AddProductType.ShowOnPageLoad = false;

        ResetGrid(GridView2, ds_Hiddenpopup.Value);
    }
    protected void btn_popup_search_Click(object sender, EventArgs e)
    {
        LoadDataPopup();
    }
    protected void btn_popup_clear_Click(object sender, EventArgs e)
    {
        E_popup_error.Text = "";
        ddl_popup_SearchBy.SelectedIndex = 0;
        txt_ProductType.Text = "";
        LoadDataPopup();
    }
    #endregion

    protected void txtProductType_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtProductDesc_TextChanged(object sender, EventArgs e)
    {

    }
    private void PopupMsgCenter()
    {

        PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirm.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirm.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        Popup_AddProductType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        Popup_AddProductType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }

}