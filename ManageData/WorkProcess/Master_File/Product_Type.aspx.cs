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

public partial class ManageData_WorkProcess_Product_Type : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    protected string indexLoanType = "";
    protected string LoadType = "";
    protected string LoadTypeName = "";
    protected string ProductType = "";
    protected string ProductTypeDescription = "";
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
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
        txtProductType.Enabled = false;
        txtLoanType.Enabled = false;
        txtLoanTypeName.Enabled = false;
    }
    public static class Globals
    {
        public static String productTypeIdShow;
    }

    #region Bind Data and Search
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Search_Data();
    }
    void Search_Data()
    {
        //iDB2Command cmd = new iDB2Command();
        E_error.Text = "";
        string sqlwhere = "";
        SqlAll = @" SELECT T40LTY,T00TNM ,T40TYP,T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) 
                    LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) ON T40LTY=T00LTY  
                    WHERE T40DEL = '' ";
        if (ddl_SearchBy.SelectedValue == "PT" && txt_ProductType.Text.Trim() != "")
        {
            sqlwhere += " AND CAST(T40TYP as nvarchar) = '" + txt_ProductType.Text.Trim() + "'";
        }
        if (ddl_SearchBy.SelectedValue == "PD" && txt_ProductType.Text.Trim() != "")
        {
            sqlwhere += $" AND T40DES LIKE '%{txt_ProductType.Text.ToUpper().Trim()}%' ";
        }

        SqlAll = SqlAll + sqlwhere + " ORDER BY T40LTY ASC ";

        //cmd.CommandText = SqlAll;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView1.DataSource = DS;
            GridView1.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T40LTY");
            dt.Columns.Add("T00TNM");
            dt.Columns.Add("T40TYP");
            dt.Columns.Add("T40DES");
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
        txt_ProductType.Text = "";
        txt_LoanType.Text = "";
    }
    private void set_default_edit()
    {
        txtLoanType.Text = LoadType;
        txtLoanTypeName.Text = LoadTypeName;
        txtProductType.Text = ProductType;
        txtProductTypeDescription.Text = ProductTypeDescription;
        txtProductTypeDescription.Focus();
        btnAdd.Text = "Edit";
        lbl_AddEdit.Text = "Edit Product type";
    }
    private void set_clear_add_edit()
    {
        txtLoanType.Text = "";
        txtLoanTypeName.Text = "";
        txtProductType.Text = "";
        txtProductTypeDescription.Text = "";

        btnAdd.Text = "Add";
        lbl_AddEdit.Text = "Add Product type";
        txtProductTypeDescription.Focus();

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

        btnProductType.Enabled = false;
        LoadType = dr[0].ToString().Trim(); ;
        LoadTypeName = dr[1].ToString().Trim(); ;
        ProductType = dr[2].ToString().Trim(); ;
        ProductTypeDescription = dr[3].ToString().Trim(); ;
        txtProductTypeDescription.Focus();

        set_default_edit();
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
        indexLoanType = dr[0].ToString().Trim();
        ProductType = dr[2].ToString().Trim();
        ProductTypeDescription = dr[3].ToString().Trim();
        Globals.productTypeIdShow = dr[2].ToString().Trim();
        if (validateDelete())
        {
            m_userInfo = userInfoService.GetUserInfo();
            //string Curtime = "";
            //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
            //ilObj.CloseConnectioDAL();
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = " Update  AS400DB01.ILOD0001.ILTB40  SET T40DEL = 'X' "
                  + ", T40UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                  + ", T40UDT = " + m_UpdTime.ToString()
                  + ", T40USR = '" + m_userInfo.Username.ToString() + "'"
                  + ", T40DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                  + "  WHERE   T40TYP = " + ProductType;

            txtProductType_D.Text = ProductTypeDescription;
            txtSqlAll.Text = SqlAll;
            lblConfimMsg_Delete.Text = "Confirm Delete Product Type Description : " + ProductTypeDescription;
            PopupConfirmDelete.ShowOnPageLoad = true;
        }
        GridView1.Focus();
    }

    private bool validateDelete()
    {

        SqlAll = " SELECT T41TYP FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41TYP= " + ProductType + " AND  T41DEL= ''  AND T41LTY= '" + indexLoanType + "'";
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;


        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found this Product-type is uses by item ";
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
        ProductTypeDescription = txtProductTypeDescription.Text.ToUpper().Trim();
        //byte[] bytes = Encoding.Default.GetBytes(ProductTypeDescription);
        //ProductTypeDescription = Encoding.ASCII.GetString(bytes);
        ProductType = txtProductType.Text.Trim();
        string LoanTypes = txtLoanType.Text.Trim();
        if (ProductTypeDescription == "" && LoanTypes == "")
        {

            Msg = "Please Select Loan Type and Input Product Type Description";
            MsgHText = "Validate Save";
            set_Msg();
        }
        else if (ProductTypeDescription == "")
        {

            Msg = "Please Input Product Type Description  ";
            MsgHText = "Validate Save";
            set_Msg();
        }
        else if (LoanTypes == "")
        {

            Msg = "Please Select Loan Type ";
            MsgHText = "Validate Save";
            set_Msg();
        }
        else
        {

            if (ProductType == "")
            {
                SqlAll = " SELECT T40TYP FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL = '' AND T40DES = '" + ProductTypeDescription + "'";
            }
            else
            {
                SqlAll = " SELECT T40TYP FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL = '' AND T40DES = '" + ProductTypeDescription + "'" + " AND T40TYP  <> " + ProductType;
            }

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;


            if (cookiesStorage.check_dataset(DS))
            {
                Msg = "Found this Product Type is Duplicate ";
                MsgHText = "Validate Save";
                set_Msg();
                dataCenter.CloseConnectSQL();            
            }
            else
            {
                CHText = "Confirm Save";
                CMsg = "Confirm Save Product Type ";
                set_confirmMsg();
            }
        }
    }

    #endregion

    #region Confirm Add/Edit
    protected void btnConfirmOK_Click(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        //iDB2Command cmd = new iDB2Command();
        ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_userInfo);
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string NameButton = "";
        string PrdTypPopupMsg = "";

        try
        {

            ProductTypeDescription = txtProductTypeDescription.Text.ToUpper().Trim();
            ProductType = txtProductType.Text.Trim();

            if (ProductType == "") // insert
            {
                SqlAll = " SELECT T40TYP FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) WHERE T40DEL = 'X' AND T40DES = '" + ProductTypeDescription + "'";
                DataSet DS = new DataSet();
                
                ilObj.UserInfomation = m_userInfo;
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (cookiesStorage.check_dataset(DS))
                {
                    DataRow dr = DS.Tables[0]?.Rows[0];
                    ProductType = dr[0].ToString().Trim();
                    NameButton = "Edit";
                    MsgHText = NameButton;
                    PrdTypPopupMsg = ProductType;

                    SqlAll = " Update AS400DB01.ILOD0001.ILTB40 SET T40DEL = ''  "
                                                  + ",  T40UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                                                  + ",  T40UDT = " + m_UpdTime.ToString()
                                                  + ",  T40USR = '" + m_userInfo.Username.ToString() + "'"
                                                  + ",  T40PGM ='IL_PROTYPE'"
                                                  + ",  T40DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                                  + ",  T40DES = '" + ProductTypeDescription + "'"
                                                  + ",  T40LTY= '01' "
                                                  + "   WHERE T40TYP = " + ProductType;
                }
                else
                {
                    NameButton = "Add";

                    string WORUNN = "";
                    string WOLENG = "";
                    string WOERRF = "";
                    string WOEMSG = "";
                    //call procedures Call_ILSR92
                    dataSubroutine.Call_ILSR92("01", "040", ref WORUNN, ref WOLENG, ref WOERRF, ref WOEMSG);

                    string strBizInit = m_userInfo.BizInit.ToString();
                    string strBranchNo = m_userInfo.BranchNo;
                    string LoanTypeKey = txtLoanType.Text;
                    string ProductTypes = WORUNN;

                    MsgHText = NameButton;
                    PrdTypPopupMsg = ProductTypes;

                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB40(
                                                T40DEL,
                                                T40UDD,
                                                T40UDT,
                                                T40USR,
                                                T40PGM,
                                                T40DSP,
                                                T40DES,
                                                T40LTY,
                                                T40TYP)
                                    VALUES(     
                                                '{string.Empty}',
                                                {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},
                                                {m_UpdTime.ToString()},
                                                '{ m_userInfo.Username.ToString()}',
                                                'IL_PROTYPE',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                                '{ProductTypeDescription}',
                                                '{LoanTypeKey}',
                                                {ProductTypes}
                                    )";


                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("t40del", string.Empty);
                    //cmd.Parameters.Add("t40udd", Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd")));
                    //cmd.Parameters.Add("t40udt", m_UpdTime.ToString());
                    //cmd.Parameters.Add("t40usr", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("t40pgm", "IL_PROTYPE");
                    //cmd.Parameters.Add("t40dsp", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                    //cmd.Parameters.Add("t40des", ProductTypeDescription);
                    //cmd.Parameters.Add("t40lty", LoanTypeKey);
                    //cmd.Parameters.Add("t40typ", ProductTypes);
                }

            }
            else // update
            {
                NameButton = "Edit";
                MsgHText = NameButton;
                string LoanTypeKey = txtLoanType.Text.Trim();
                PrdTypPopupMsg = ProductType;

                SqlAll = $@"Update AS400DB01.ILOD0001.ILTB40 
                                SET T40DEL = '{string.Empty}',
                                    T40UDD = {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},                   
                                    T40UDT = {m_UpdTime.ToString()},
                                    T40USR = '{m_userInfo.Username.ToString()}',
                                    T40PGM = 'IL_PROTYPE',
                                    T40DSP = '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                    T40DES = '{ProductTypeDescription}'
                                WHERE T40LTY = '{LoanTypeKey}' AND T40TYP = {ProductType}";

                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("T40DEL", string.Empty);
                //cmd.Parameters.AddWithValue("T40UDD", Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd")));
                //cmd.Parameters.AddWithValue("T40UDT", m_UpdTime.ToString());
                //cmd.Parameters.AddWithValue("T40USR", m_userInfo.Username.ToString());
                //cmd.Parameters.AddWithValue("T40PGM", "IL_PROTYPE");
                //cmd.Parameters.AddWithValue("T40DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                //cmd.Parameters.AddWithValue("T40DES", ProductTypeDescription);
                //cmd.Parameters.AddWithValue("T40LTY", LoanTypeKey);
                //cmd.Parameters.AddWithValue("T40TYP", ProductType);
            }



            //cmd.CommandText = SqlAll;
            bool transaction = dataCenter.Sqltr == null ? true : false;

            int resHome11 = dataCenter.Execute(SqlAll, CommandType.Text,transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = NameButton + " Product Type:" + PrdTypPopupMsg + " - " + ProductTypeDescription + " not complete.";
                set_Msg();


                return;
            }

            dataCenter.CommitMssql();
            //cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Product Type : " + PrdTypPopupMsg + " - " + ProductTypeDescription + " complete.";
            set_MsgSuccess();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Product Type : " + PrdTypPopupMsg + " - " + ProductTypeDescription + " not complete.";
            set_Msg();
            return;
        }
    }

    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        txtProductTypeDescription.Focus();
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
        ProductTypeDescription = txtProductType_D.Text;
        bool transaction = dataCenter.Sqltr == null ? true : false;
        try
        {
            int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text,transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                Msg = "Delete Product Type Description : " + Globals.productTypeIdShow + " - " + ProductTypeDescription + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            Msg = "Delete Product Type Description : " + Globals.productTypeIdShow + " - " + ProductTypeDescription + " complete.";
            set_MsgSuccess();
            GridView1.Focus();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            Msg = "Delete Product Type Description : " + Globals.productTypeIdShow + " - " + ProductTypeDescription + " not complete.";
            set_Msg();
            return;
        }

    }
    protected void btnConfirm_Cancel_Click(object sender, EventArgs e)
    {
        GridView1.Focus();
    }
    #endregion

    #region pop up  LoanType

    private void LoadDataPopup()
    {
        E_popup_error.Text = "";
        string sqlwhere = "";
        SqlAll = " SELECT T00LTY,T00TNM FROM AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) ";

        if (ddl_popup_SearchBy.SelectedValue == "LT" && txt_LoanType.Text.Trim() != "")
        {
            sqlwhere += " WHERE T00LTY = " + txt_LoanType.Text.Trim() + "";
        }
        if (ddl_popup_SearchBy.SelectedValue == "LD" && txt_LoanType.Text.Trim() != "")
        {
            sqlwhere += " WHERE T00TNM like '%" + txt_LoanType.Text.ToUpper().Trim() + "%' ";
        }
        //---------------sql


        SqlAll = SqlAll + sqlwhere + " ORDER BY T00LTY ASC ";
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
        txt_LoanType.Text = "";
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
        txtLoanType.Text = dr[0].ToString().Trim();
        txtLoanTypeName.Text = dr[1].ToString().Trim();
        txtProductType.Text = "";
        //txtProductTypeDesc.Text = dr[3].ToString().Trim();

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
        txt_LoanType.Text = "";
        LoadDataPopup();
    }
    #endregion

    #region Message all
    protected void btnOK_Click(object sender, EventArgs e)
    {

    }
    private void PopupMsgCenter()
    {

        PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirm.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirm.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        Popup_AddProductType.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        Popup_AddProductType.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }
    #endregion

}