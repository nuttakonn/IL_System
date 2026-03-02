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

public partial class ManageData_WorkProcess_ProductGroup : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    ILDataSubroutine iLDataSubroutine;
    public ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    protected string WOERRF = "";
    protected string ProductItemCode = "";
    private string ProgramName = "IL_PROGRP"; //"ILE000010F";
    public UserInfoService userInfoService;
    private string Message;
    private string MessageHeadText;
    public CookiesStorage cookiesStorage;
    private string ConfirmAddMessage;
    private string ConfirmAddHeaderText;

    private string ConfirmDeleteMessage;
    private string ConfirmDeleteHeaderText;

    private string ConfirmAddItemToGroupMessage;
    private string ConfirmAddItemToGroupHeaderText;

    private string ConfirmDeleteItemFromGroupMessage;
    private string ConfirmDeleteItemFromGroupHeaderText;

    private bool AdvanceSearch = false;

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
    public static class Globals
    {
        public static String productGroupIdShow;
        public static String productGroupDesShow;
    }
    #region Set Default
    protected void SetDefault()
    {
        ddlSearchBy.SelectedIndex = 0;
        txtSearchText.Text = "";
        AdvanceSearch = false;
        tbAddItemToGroupProduct.Visible = false;
        LoadProductGroupData();

        lblAddEdit.Text = "Add New";
        btnAddProductModel.Visible = true;

        txtLoanTypeCode.Text = "";
        txtLoanTypeDescription.Text = "";

        txtProductTypeCode.Text = "";
        txtProductTypeDescription.Text = "";

        txtProductBrandCode.Text = "";
        txtProductBrandDescription.Text = "";

        txtProductCode.Text = "";
        txtProductCodeDescription.Text = "";

        txtProductModelCode.Text = "";
        txtProductModelDescription.Text = "";

        txtProductGroupCode.Text = "";
        txtProductItemAll.Text = "";
        txtProductItemDescription.Text = "";
    }
    #endregion

    #region GridView Product Group
    protected void LoadProductGroupData()
    {
        iDB2Command cmd = new iDB2Command();
        string sqlWhere = "";
        string searchBy = ddlSearchBy.SelectedValue;
        string searchText = txtSearchText.Text.Trim().Replace("'", "''");

        if (AdvanceSearch)
        {
            string loanTypeCode = txtPopupAdvanceSearchLoanTypeCode.Text;
            string productTypeCode = txtPopupAdvanceSearchProductTypeCode.Text;
            string productBrandCode = txtPopupAdvanceSearchProductBrandCode.Text;
            string productCode = txtPopupAdvanceSearchProductCode.Text;
            string productModelCode = txtPopupAdvanceSearchProductModelCode.Text;

            if (!string.IsNullOrEmpty(loanTypeCode))
            {
                sqlWhere += " AND T44LTY = '" + loanTypeCode + "'";
            }

            if (!string.IsNullOrEmpty(productTypeCode))
            {
                sqlWhere += " AND CAST(T44TYP as nvarchar) = + '" + productTypeCode + "'";
            }

            if (!string.IsNullOrEmpty(productBrandCode))
            {
                sqlWhere += " AND CAST(T44BRD as nvarchar) = '" + productBrandCode + "'";
            }

            if (!string.IsNullOrEmpty(productCode))
            {
                sqlWhere += " AND CAST(T44COD as nvarchar) = '" + productCode + "'";
            }

            if (!string.IsNullOrEmpty(productModelCode))
            {
                sqlWhere += " AND CAST(T44MDL as nvarchar) = '" + productModelCode + "'";
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchBy)
                {
                    case "LTC": //Loan Type Code
                        sqlWhere = " AND CAST(T44LTY as nvarchar) = '" + searchText + "'";
                        break;
                    case "LTD": //Loan Type Description
                        sqlWhere += $" AND T00TNM LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    case "PTC": //Product Type Code
                        sqlWhere = " AND CAST(T44LTY as nvarchar) = '" + searchText + "'";
                        break;
                    case "PTD": //Product Type Description
                        sqlWhere += $" AND T40DES LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    case "PDC": //Product Code
                        sqlWhere = " AND CAST(T44COD as nvarchar) = '" + searchText + "'";
                        break;
                    case "PDD": //Product Code Description
                        sqlWhere += $" AND T41DES LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    case "PBC": //Product Brand Code
                        sqlWhere = " AND CAST(T44BRD as nvarchar) = '" + searchText + "'";
                        break;
                    case "PBD": //Product Brand Description
                        sqlWhere += $" AND T42DES LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    case "PMC": //Product Model Code
                        sqlWhere = " AND CAST(T44MDL as nvarchar) = '" + searchText + "'";
                        break;
                    case "PMD": //Product Model Description
                        sqlWhere += $" AND T43DES LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    case "PIC": //Product Item Code
                        sqlWhere = " AND CAST(T44MDL as nvarchar) = '" + searchText + "'";
                        break;
                    case "PID": //Product Item Description
                        sqlWhere += $" AND T44DES LIKE '%{searchText.ToUpper().Trim()}%' ";
                        break;
                    default: break;
                }
            }
        }

        string sql = @"SELECT
                            T44LTY,
                            T00TNM,
                            T44TYP,
                            T40DES,
                            T44BRD,
                            T42DES,
                            T44COD,
                            T41DES,
                            T44MDL,
                            T43DES,
                            T44ITM,
                            T44PGP,
                            T44DES 
                        FROM
                            AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                        LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                        ON  T44LTY = T00LTY 
                        LEFT JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                        ON  T44LTY = T40LTY
                        AND T44TYP = T40TYP 
                        LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK)
                        ON  T44LTY = T41LTY
                        AND T44TYP = T41TYP
                        AND T44COD = T41COD 
                        LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)
                        ON  T44BRD = T42BRD 
                        LEFT JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                        ON  T44LTY = T43LTY
                        AND T44TYP = T43TYP
                        AND T44BRD = T43BRD
                        AND T44COD = T43COD
                        AND T44MDL = T43MDL 
                        WHERE
                            T44DEL = ''
                        AND T44PGP = 'Y' ";

        sql = sql + sqlWhere;
        cmd.CommandText = sql;

        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsProductGroup = new DataSet();
        dsProductGroup = dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductGroup))
        {
            ds_product_group.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductGroup);
            gvProductGroup.DataSource = dsProductGroup;
            gvProductGroup.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T44LTY");
            dt.Columns.Add("T00TNM");
            dt.Columns.Add("T44TYP");
            dt.Columns.Add("T40DES");
            dt.Columns.Add("T44BRD");
            dt.Columns.Add("T42DES");
            dt.Columns.Add("T44COD");
            dt.Columns.Add("T41DES");
            dt.Columns.Add("T44MDL");
            dt.Columns.Add("T43DES");
            dt.Columns.Add("T44ITM");
            dt.Columns.Add("T44DES");
            dsProductGroup.Tables.Add(dt);
            gvProductGroup.DataSource = dsProductGroup;
            gvProductGroup.DataBind();
        }
        
    }

    protected void gvProductGroupPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductGroup.PageIndex = e.NewPageIndex;
        gvProductGroup.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_group.Value);
        gvProductGroup.DataBind();
    }
    #endregion

    #region Search
    protected void btnSearchClick(object sender, EventArgs e)
    {
        AdvanceSearch = false;
        LoadProductGroupData();
    }

    protected void btnClearClick(object sender, EventArgs e)
    {
        ddlSearchBy.SelectedIndex = 0;
        txtSearchText.Text = "";
        LoadProductGroupData();
    }

    protected void btnAdvanceSearchClick(object sender, EventArgs e)
    {
        hdfFormSearch.Value = "N";
        SearchProductModel.Visible = true;
        PopupAdvanceSearch.HeaderText = "Advance Search";
        PopupAdvanceSearch.ShowOnPageLoad = true;
        btnPopupAdvanceSearch.Focus();

        txtPopupAdvanceSearchLoanTypeCode.Text = "";
        txtPopupAdvanceSearchLoanTypeDescription.Text = "";

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDescription.Text = "";

        txtPopupAdvanceSearchProductBrandCode.Text = "";
        txtPopupAdvanceSearchProductBrandDescription.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDescription.Text = "";

        txtPopupAdvanceSearchProductModelCode.Text = "";
        txtPopupAdvanceSearchProductModelDescription.Text = "";
    }
    #endregion

    #region Button Add Product Model
    protected void btnAddProductModelClick(object sender, EventArgs e)
    {
        hdfFormSearch.Value = "Y";
        SearchProductModel.Visible = false;
        PopupAdvanceSearch.HeaderText = "Add Product Group";
        PopupAdvanceSearch.ShowOnPageLoad = true;
        btnPopupAdvanceSearch.Focus();

        txtPopupAdvanceSearchLoanTypeCode.Text = "";
        txtPopupAdvanceSearchLoanTypeDescription.Text = "";

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDescription.Text = "";

        txtPopupAdvanceSearchProductBrandCode.Text = "";
        txtPopupAdvanceSearchProductBrandDescription.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDescription.Text = "";
    }
    #endregion

    #region Popup Message
    protected void btnPopupMessageOKClick(object sender, EventArgs e)
    {

    }

    private void SetMessage()
    {
        lblMessage.Text = Message;
        PopupMessage.HeaderText = MessageHeadText;
        PopupMessage.ShowOnPageLoad = true;
    }
    private void SetMessageSuccess()
    {
        lblMessageSuccess.Text = Message;
        PopupMsgSuccess.HeaderText = MessageHeadText;
        PopupMsgSuccess.ShowOnPageLoad = true;
    }
    #endregion

    #region Popup Confirm Add/Edit
    private void SetConfirmMessage()
    {
        lblConfirmAddMessage.Text = ConfirmAddMessage;
        PopupConfirmAdd.HeaderText = ConfirmAddHeaderText;
        PopupConfirmAdd.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmAddOKClick(object sender, EventArgs e)
    {
        string mode = lblAddEdit.Text == "Add New" ? "Add" : lblAddEdit.Text;

        if (mode == "Add")
        {
            AddNewGroup();
        }

        if (mode == "Edit")
        {
            EditProductGroup();
        }
    }

    protected void btnPopupConfirmAddCancelClick(object sender, EventArgs e)
    {

    }
    #endregion

    #region Add
    protected void btnAddClick(object sender, EventArgs e)
    {
        #region Validate Form
        if (string.IsNullOrEmpty(txtProductModelCode.Text))
        {
            MessageHeadText = "Validate Save";
            Message = "Please Select Product Model";
            SetMessage();
            btnAddProductModel.Focus();
            return;
        }

        if (string.IsNullOrEmpty(txtProductItemDescription.Text.Trim()))
        {
            MessageHeadText = "Validate Save";
            Message = "Please Input Product Item Descriptison";
            SetMessage();
            txtProductItemDescription.Focus();
            return;
        }
        #endregion

        ConfirmAddMessage = "Confirm Save Data";
        ConfirmAddHeaderText = "Confirm Save";
        SetConfirmMessage();
    }

    protected void btnClearDataClick(object sender, EventArgs e)
    {
        SetClearAddEdit();
    }

    protected void AddNewGroup()
    {
        m_userInfo = userInfoService.GetUserInfo();
        //iDB2Command cmd = new iDB2Command();
        iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string loanTypeCode = txtLoanTypeCode.Text;
        string productTypeCode = txtProductTypeCode.Text;
        string productBrandCode = txtProductBrandCode.Text;
        string productCode = txtProductCode.Text;
        string productModelCode = txtProductModelCode.Text;
        string productItemDescription = txtProductItemAll.Text.Replace("'", "''") + " " + txtProductItemDescription.Text.Trim().Replace("'", "''");
        string productItemCode = string.Empty;

        string curDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string curTime = DateTime.Now.ToString("HHmmss", m_DThai);
        string curUser = m_userInfo.Username.ToString();
        string curWorkStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

        MessageHeadText = "Add New Group";

        string sql = @"SELECT
                            T44LTY, T44ITM, T44TYP, T44BRD, T44COD, T44MDL, T44PGP, T44DES, T44UDD, T44UDT, T44PGM, T44USR, T44DSP, T44DEL
                        FROM
                            AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                        WHERE
                            CAST(T44LTY as nvarchar) = '" + loanTypeCode + @"'
                        AND CAST(T44TYP as nvarchar) = '" + productTypeCode + @"'
                        AND CAST(T44BRD as nvarchar) = '" + productBrandCode + @"'
                        AND CAST(T44COD as nvarchar) = '" + productCode + @"'
                        AND CAST(T44MDL as nvarchar) = '" + productModelCode + @"'
                        AND CAST(T44DES as nvarchar) = '" + productItemDescription + "'";

        try
        {
            DataSet ds = new DataSet();
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(ds)) //Update
            {
                DataRow dr = ds.Tables[0]?.Rows[0];
                if (dr["T44DEL"].ToString().Trim() == "")
                {
                    Message = "Add New Group is Duplicate";
                    SetMessage();
                    return;
                }

                productItemCode = dr["T44ITM"].ToString();

                sql = @"UPDATE
                        AS400DB01.ILOD0001.ILTB44  
                    SET  
                        T44DEL = '', 
                        T44UDD = " + curDate + @", 
                        T44UDT = " + curTime + @", 
                        T44USR = '" + curUser + @"', 
                        T44PGM = '" + ProgramName + @"', 
                        T44DSP = '" + curWorkStation + @"'
                    WHERE
                        T44LTY = '" + loanTypeCode + @"'
                    AND T44TYP = " + productTypeCode + @" 
                    AND T44BRD = " + productBrandCode + @"
                    AND T44COD = " + productCode + @"
                    AND T44MDL = " + productModelCode + @"
                    AND T44ITM = " + productItemCode;
            }
            else
            {
                /*************** Call Stored Prodeure ***************/

                string WILNTY = "01";
                string WIRECD = "044";
                string WORUNN = "";
                string WOLENG = "";
                //string WOERRF = "";
                string WOEMSG = "";

                //call procedures Call_ILSR92
                iLDataSubroutine.Call_ILSR92(WILNTY, WIRECD, ref WORUNN, ref WOLENG, ref WOERRF, ref WOEMSG);

                if (WOERRF == "Y" || WOEMSG.Trim() != "")
                {
                    msgProcedures.Text = " Check Program AS400 (ILSR92) ! ";
                    PopupMsgProcedures.ShowOnPageLoad = true;
                }
                else
                {
                    ProductItemCode = WORUNN;
                    //sql = @"INSERT INTO ILTB44
                    //    (
                    //                T44LTY,
                    //                T44TYP,
                    //                T44BRD,
                    //                T44COD,
                    //                T44MDL,
                    //                T44ITM,
                    //                T44DES,
                    //                T44PGP,
                    //                T44UDD,
                    //                T44UDT,
                    //                T44USR,
                    //                T44PGM,
                    //                T44DSP
                    //    ) VALUES (
                    //                '" + loanTypeCode + @"',
                    //                " + productTypeCode + @",
                    //                " + productBrandCode + @",
                    //                " + productCode + @",
                    //                " + productModelCode + @",
                    //                " + ProductItemCode + @",
                    //                '" + productItemDescription + @"',
                    //                'Y',
                    //                " + curDate + @",
                    //                " + curTime + @",
                    //                '" + curUser + @"',
                    //                '" + ProgramName + @"',
                    //                '" + curWorkStation + @"'
                    //    )";

                    sql = $@"INSERT INTO AS400DB01.ILOD0001.ILTB44(
                                                T44LTY,
                                                T44TYP,
                                                T44BRD,
                                                T44COD,
                                                T44MDL,
                                                T44ITM,
                                                T44DES,
                                                T44PGP,
                                                T44UDD,
                                                T44UDT,
                                                T44USR,
                                                T44PGM,
                                                T44DSP)
                                    VALUES(     
                                                '{loanTypeCode}',
                                                {productTypeCode},
                                                {productBrandCode},
                                                {productCode},
                                                {productModelCode},
                                                {ProductItemCode},
                                                '{productItemDescription}',
                                                'Y',
                                                {curDate},
                                                {curTime},
                                                '{curUser}',
                                                '{ProgramName}',
                                                '{curWorkStation}'
                                    )";


                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("T44LTY", loanTypeCode);
                    //cmd.Parameters.Add("T44TYP", productTypeCode);
                    //cmd.Parameters.Add("T44BRD", productBrandCode);
                    //cmd.Parameters.Add("T44COD", productCode);
                    //cmd.Parameters.Add("T44MDL", productModelCode);
                    //cmd.Parameters.Add("T44ITM", ProductItemCode);
                    //cmd.Parameters.Add("T44DES", productItemDescription);
                    //cmd.Parameters.Add("T44PGP", "Y");
                    //cmd.Parameters.Add("T44UDD", curDate);
                    //cmd.Parameters.Add("T44UDT", curTime);
                    //cmd.Parameters.Add("T44USR", curUser);
                    //cmd.Parameters.Add("T44PGM", ProgramName);
                    //cmd.Parameters.Add("T44DSP", curWorkStation);

                }

                //cmd.CommandText = sql; ;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int result = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
                if (result == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Add New Group : " + ProductItemCode + " - " + productItemDescription + " Not Complete";
                    SetMessage();
                }
                else
                {
                    dataCenter.CommitMssql();
                    Message = "Add New Group : " + ProductItemCode + " - " + productItemDescription + " Complete";
                    LoadProductGroupData();
                    SetClearAddEdit();
                    SetMessageSuccess();
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Add New Group : " + ProductItemCode + " - " + productItemDescription + " Not Complete";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }

    }
    #endregion

    #region Edit
    protected void SetClearAddEdit()
    {
        lblAddEdit.Text = "Add New";
        btnAdd.Text = "Add";
        btnAddProductModel.Visible = true;
        tbAddItemToGroupProduct.Visible = false;

        txtLoanTypeCode.Text = "";
        txtLoanTypeDescription.Text = "";

        txtProductTypeCode.Text = "";
        txtProductTypeDescription.Text = "";

        txtProductBrandCode.Text = "";
        txtProductBrandDescription.Text = "";

        txtProductCode.Text = "";
        txtProductCodeDescription.Text = "";

        txtProductModelCode.Text = "";
        txtProductModelDescription.Text = "";
        btnAddProductModel.Visible = true;

        txtProductGroupCode.Text = "";
        txtProductItemAll.Text = "";
        txtProductItemDescription.Text = "";
    }

    protected void gvProductGroupSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductGroup = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_group.Value);
        DataRow drProductGroup = dsProductGroup.Tables[0]?.Rows[(gvProductGroup.PageIndex * Convert.ToInt16(gvProductGroup.PageSize)) + e.NewSelectedIndex];

        lblAddEdit.Text = "Edit";
        btnAdd.Text = "Edit";
        btnAddProductModel.Visible = false;
        tbAddItemToGroupProduct.Visible = true;

        txtLoanTypeCode.Text = drProductGroup["T44LTY"].ToString().Trim();
        txtLoanTypeDescription.Text = drProductGroup["T00TNM"].ToString().Trim();

        txtProductTypeCode.Text = drProductGroup["T44TYP"].ToString().Trim();
        txtProductTypeDescription.Text = drProductGroup["T40DES"].ToString().Trim();

        txtProductBrandCode.Text = drProductGroup["T44BRD"].ToString().Trim();
        txtProductBrandDescription.Text = drProductGroup["T42DES"].ToString().Trim();

        txtProductCode.Text = drProductGroup["T44COD"].ToString().Trim();
        txtProductCodeDescription.Text = drProductGroup["T41DES"].ToString().Trim();

        txtProductModelCode.Text = drProductGroup["T44MDL"].ToString().Trim();
        txtProductModelDescription.Text = drProductGroup["T43DES"].ToString().Trim();

        txtProductGroupCode.Text = drProductGroup["T44ITM"].ToString().Trim();
        txtProductItemAll.Text = drProductGroup["T41DES"].ToString().Trim() + " " + drProductGroup["T42DES"].ToString().Trim() + " " + drProductGroup["T43DES"].ToString().Trim();
        txtProductItemDescription.Text = drProductGroup["T44DES"].ToString().Trim();

        LoadProductGroupItemData();
    }

    protected void EditProductGroup()
    {
        MessageHeadText = "Edit Group";
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        try
        {
            string loanTypeCode = txtLoanTypeCode.Text;
            string productTypeCode = txtProductTypeCode.Text;
            string productBrandCode = txtProductBrandCode.Text;
            string productCode = txtProductCode.Text;
            string productModelCode = txtProductModelCode.Text;
            string productGroupCode = txtProductGroupCode.Text;
            //string productGroupDescription = txtProductItemAll.Text.Trim() + " " + txtProductItemDescription.Text.Trim();
            string productGroupDescription = txtProductItemDescription.Text.Trim().Replace("'", "''");

            string curDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
            string curTime = DateTime.Now.ToString("HHmmss", m_DThai);
            string curUser = m_userInfo.Username.ToString();
            string curWorkStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

            string sql = @"SELECT
                                T44LTY, T44ITM, T44TYP, T44BRD, T44COD, T44MDL, T44PGP, T44DES, T44UDD, T44UDT, T44PGM, T44USR, T44DSP, T44DEL
                            FROM
                                AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                            WHERE
                                CAST(T44LTY as nvarchar) = '" + loanTypeCode + @"'
                            AND CAST(T44TYP as nvarchar) = '" + productTypeCode + @"'
                            AND CAST(T44BRD as nvarchar) = '" + productBrandCode + @"'
                            AND CAST(T44COD as nvarchar) = '" + productCode + @"'
                            AND CAST(T44MDL as nvarchar) = '" + productModelCode + @"'
                            AND T44DES = '" + productGroupDescription + "'";

            DataSet ds = new DataSet();
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(ds))
            {
                Message = "Can not Edit Data, Product Item is Duplicate";
                SetMessage();

                return;
            }

            //sql = @"UPDATE
            //        ILTB44
            //    SET
            //        T44DES = '" + productGroupDescription + @"',
            //        T44UDD = " + curDate + @",
            //        T44UDT = " + curTime + @",
            //        T44USR = '" + curUser + @"',
            //        T44PGM = '" + ProgramName + @"',
            //        T44DSP = '" + curWorkStation + @"',
            //        T44DEL = ''
            //    WHERE
            //        T44LTY = '" + loanTypeCode + @"'
            //    AND T44ITM = " + productGroupCode;

            sql = $@"Update AS400DB01.ILOD0001.ILTB44 
                                SET T44DES = '{productGroupDescription}',
                                    T44UDD = {curDate},                   
                                    T44UDT = {curTime},
                                    T44USR = '{curUser}',
                                    T44PGM = '{ProgramName}',
                                    T44DSP = '{curWorkStation}',
                                    T44DEL = ''
                                WHERE T44LTY = '" + loanTypeCode + @"'
                                      AND T44ITM = " + productGroupCode;

            cmd.CommandText = sql;
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int result = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            if (result == -1)
            {
                dataCenter.RollbackMssql();
                Message = "Update Product Group : " + productGroupCode + " - " + productGroupDescription + " Not Complete";
                SetMessage();
            }
            else
            {
                dataCenter.CommitMssql();
                Message = "Update Product Group : " + productGroupCode + " - " + productGroupDescription + " Complete";
                SetClearAddEdit();
                LoadProductGroupData();
                //LoadProductGroupItemData();
                SetMessageSuccess();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Update Data Not Complete";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }
    }

    #endregion

    #region Popup Confirm Delete
    private void SetConfirmDeleteMessage()
    {
        lblConfirmDeleteMessage.Text = ConfirmDeleteMessage;
        PopupConfirmDelete.HeaderText = ConfirmDeleteHeaderText;
        PopupConfirmDelete.ShowOnPageLoad = true;
    }

    protected void gvProductGroupRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet dsProductGroupData = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_group.Value);
        DataRow drProductGroupData = dsProductGroupData.Tables[0]?.Rows[(gvProductGroup.PageIndex * Convert.ToInt16(gvProductGroup.PageSize)) + e.RowIndex];

        hdfLoanTypeCode.Value = drProductGroupData["T44LTY"].ToString().Trim();
        hdfProductGroupCode.Value = drProductGroupData["T44ITM"].ToString().Trim();
        Globals.productGroupIdShow = drProductGroupData["T44ITM"].ToString().Trim();
        Globals.productGroupDesShow = drProductGroupData["T44DES"].ToString().Trim();
        ConfirmDeleteHeaderText = "Confirm Delete";
        ConfirmDeleteMessage = "Confirm Delete Data";
        SetConfirmDeleteMessage();
    }

    protected void btnPopupConfirmDeleteOKClick(object sender, EventArgs e)
    {
        DeleteProductGroup();
    }

    protected void btnPopupConfirmDeleteCancelClick(object sender, EventArgs e)
    {
        PopupConfirmDelete.ShowOnPageLoad = false;
    }
    #endregion

    #region Delete
    protected void DeleteProductGroup()
    {
        MessageHeadText = "Delete Product Group Item";
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string loanTypeCode = hdfLoanTypeCode.Value;
        string productGroupCode = hdfProductGroupCode.Value;

        string curDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
        string curTime = DateTime.Now.ToString("HHmmss", m_DThai);
        string curUser = m_userInfo.Username.ToString();
        string curWorkStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

        List<string> sqlList = new List<string>();

        string sql = "SELECT P1ITEM FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) WHERE P1ITEM = " + productGroupCode;

        DataSet ds = new DataSet();
        ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(ds))
        {
            Message = "Found Group Item is used, Can not Remove";
            SetMessage();
        }
        else
        {
            //Delete Group
            sql = @"UPDATE
                        AS400DB01.ILOD0001.ILTB44
                    SET
                        T44DEL = 'X',
                        T44UDD = " + curDate + @",
                        T44UDT = " + curTime + @",
                        T44USR = '" + curUser + @"',
                        T44PGM = '" + ProgramName + @"',
                        T44DSP = '" + curWorkStation + @"'
                    WHERE
                        T44LTY = '" + loanTypeCode + @"'
                    AND T44ITM = " + productGroupCode;

            sqlList.Add(sql);

            //Delete Item
            sql = @"UPDATE
                        AS400DB01.ILOD0001.ILTB45
                    SET
                        T45DEL = 'X',
                        T45UDD = " + curDate + @",
                        T45UDT = " + curTime + @",
                        T45USR = '" + curUser + @"',
                        T45PGM = '" + ProgramName + @"',
                        T45DSP = '" + curWorkStation + @"'
                    WHERE
                        T45LTY = '" + loanTypeCode + @"'
                    AND T45ITG = " + productGroupCode;

            sqlList.Add(sql);

            ExecuteDelete(sqlList);
        }
    }

    protected void ExecuteDelete(List<string> sql)
    {
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        //iDB2Command cmd = new iDB2Command();
        dataCenter = new DataCenter(m_userInfo);
        try
        {
            foreach (var sqlCommand in sql)
            {
                //cmd.CommandText = sqlCommand;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resHome11 = dataCenter.Execute(sqlCommand, CommandType.Text, transaction).Result.afrows;
                if (resHome11 == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Delete Product Group : " + Globals.productGroupIdShow + " - " + Globals.productGroupDesShow + " Not Complete.";
                    SetMessage();
                    return;
                }
            }

            dataCenter.CommitMssql();
            Message = "Delete Product Group : " + Globals.productGroupIdShow + " - " + Globals.productGroupDesShow + " Complete.";
            SetClearAddEdit();
            LoadProductGroupData();
            SetMessageSuccess();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Delete Product Group : " + Globals.productGroupIdShow + " - " + Globals.productGroupDesShow + " Not Complete.";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            //cmd.Parameters.Clear();
        }
    }
    #endregion

    #region Popup Advance Search
    protected void btnPopupAdvanceSearchClick(object sender, EventArgs e)
    {
        if (hdfFormSearch.Value == "Y")
        {
            LoadProductModelData();
            PopupAddProductModel.ShowOnPageLoad = true;
        }
        else
        {
            AdvanceSearch = true;
            LoadProductGroupData();
        }

        PopupAdvanceSearch.ShowOnPageLoad = false;
    }

    protected void btnPopupAdvanceSearchClearClick(object sender, EventArgs e)
    {
        txtPopupAdvanceSearchLoanTypeCode.Text = "";
        txtPopupAdvanceSearchLoanTypeDescription.Text = "";

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDescription.Text = "";

        txtPopupAdvanceSearchProductBrandCode.Text = "";
        txtPopupAdvanceSearchProductBrandDescription.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDescription.Text = "";

        txtPopupAdvanceSearchProductModelCode.Text = "";
        txtPopupAdvanceSearchProductModelDescription.Text = "";
    }

    protected void btnPopupAdvanceSearchLoanTypeClick(object sender, EventArgs e)
    {
        ddlPopupAddLoanTypeSearchBy.SelectedIndex = 0;
        txtPopupAddLoanTypeSearchText.Text = "";
        LoadLoanTypeData();
        PopupAddLoanType.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
    }

    protected void btnPopupAdvanceSearchProductTypeClick(object sender, EventArgs e)
    {
        ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
        txtPopupAddProductTypeSearchText.Text = "";
        LoadProductTypeData();
        PopupAddProductType.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
    }

    protected void btnPopupAdvanceSearchProductBrandClick(object sender, EventArgs e)
    {
        ddlPopupAddProductBrandSearchBy.SelectedIndex = 0;
        txtPopupAddProductBrandSearchText.Text = "";
        LoadProductBrandData();
        PopupAddProductBrand.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
    }

    protected void btnPopupAdvanceSearchProductCodeClick(object sender, EventArgs e)
    {
        ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
        txtPopupAddProductCodeSearchText.Text = "";
        LoadProductCodeData();
        PopupAddProductCode.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
    }

    protected void btnPopupAdvanceSearchProductModelClick(object sender, EventArgs e)
    {
        ddlPopupAddProductModelSearchBy.SelectedIndex = 0;
        txtPopupAddProductModelSearchText.Text = "";
        LoadProductModelData();
        PopupAddProductModel.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
    }
    #endregion

    #region Popup Add Loan Type
    protected void LoadLoanTypeData()
    {
        string sqlWhere = "";
        string searchBy = ddlPopupAddLoanTypeSearchBy.SelectedValue;
        string searchText = txtPopupAddLoanTypeSearchText.Text.Trim().Replace("'", "''");

        if (!string.IsNullOrEmpty(searchText))
        {
            switch (searchBy)
            {
                case "LC":
                    sqlWhere = "WHERE T00LTY = " + searchText;
                    break;
                case "LN":
                    sqlWhere = "WHERE UPPER(T00TNM) LIKE '%" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        string sql = "SELECT T00LTY, T00TNM FROM AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) " + sqlWhere;

        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsLoanType = new DataSet();
        dsLoanType = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsLoanType))
        {
            ds_loan_type.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsLoanType);
            gvLoanType.DataSource = dsLoanType;
            gvLoanType.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T00LTY");
            dt.Columns.Add("T00TNM");
            dsLoanType.Tables.Add(dt);
            gvLoanType.DataSource = dsLoanType;
            gvLoanType.DataBind();
        }
    }

    protected void gvLoanTypePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLoanType.PageIndex = e.NewPageIndex;
        gvLoanType.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_loan_type.Value);
        gvLoanType.DataBind();
    }

    protected void btnPopupAddLoanTypeSearch_Click(object sender, EventArgs e)
    {
        LoadLoanTypeData();
    }

    protected void btnPopupAddLoanTypeClear_Click(object sender, EventArgs e)
    {
        ddlPopupAddLoanTypeSearchBy.SelectedIndex = 0;
        txtPopupAddLoanTypeSearchText.Text = "";
        LoadLoanTypeData();
    }

    protected void gvLoanTypeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsLoanType = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_loan_type.Value);
        DataRow drLoanType = dsLoanType.Tables[0]?.Rows[(gvLoanType.PageIndex * Convert.ToInt16(gvLoanType.PageSize)) + e.NewSelectedIndex];

        txtPopupAdvanceSearchLoanTypeCode.Text = drLoanType[0].ToString();
        txtPopupAdvanceSearchLoanTypeDescription.Text = drLoanType[1].ToString();

        PopupAddLoanType.ShowOnPageLoad = false;
        PopupAdvanceSearch.ShowOnPageLoad = true;
    }
    #endregion

    #region Popup Add Product Type
    protected void LoadProductTypeData()
    {
        string sqlWhere = "";
        string loanType = txtPopupAdvanceSearchLoanTypeCode.Text;
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
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        dsProductType = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductType))
        {
            ds_product_type.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductType);
            gvProductType.DataSource = dsProductType;
            gvProductType.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T40LTY");
            dt.Columns.Add("T40TYP");
            dt.Columns.Add("T40DES");
            dsProductType.Tables.Add(dt);
            gvProductType.DataSource = dsProductType;
            gvProductType.DataBind();
        }
    }

    protected void gvProductTypePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductType.PageIndex = e.NewPageIndex;
        gvProductType.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_type.Value);
        gvProductType.DataBind();
    }

    protected void gvProductTypeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductType = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_type.Value);
        DataRow drProductType = dsProductType.Tables[0]?.Rows[(gvProductType.PageIndex * Convert.ToInt16(gvProductType.PageSize)) + e.NewSelectedIndex];

        txtPopupAdvanceSearchProductTypeCode.Text = drProductType[1].ToString();
        txtPopupAdvanceSearchProductTypeDescription.Text = drProductType[2].ToString();

        PopupAddProductType.ShowOnPageLoad = false;
        PopupAdvanceSearch.ShowOnPageLoad = true;
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

    #region Popup Add Product Brand
    protected void LoadProductBrandData()
    {
        string searchBy = ddlPopupAddProductBrandSearchBy.SelectedValue;
        string searchText = txtPopupAddProductBrandSearchText.Text.Trim();

        DataSet dsProductBrand = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();

        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        dsProductBrand = iLDataCenterOnMasterFile.Sp_GetBrand(searchBy, searchText, 1, 15);
        if (cookiesStorage.check_dataset(dsProductBrand))
        {
            ds_product_brand.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductBrand);
            gvProductBrand.DataSource = dsProductBrand;
            gvProductBrand.DataBind();
            DataRow dr = dsProductBrand.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = gvProductBrand.PageIndex * gvProductBrand.PageSize + 1;
            int _CurrentRecEnd = gvProductBrand.PageIndex * gvProductBrand.PageSize + gvProductBrand.Rows.Count;

            lblgvProductBrand.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T42BRD");
            dt.Columns.Add("T42DES");
            dsProductBrand.Tables.Add(dt);
            gvProductBrand.DataSource = dsProductBrand;
            gvProductBrand.DataBind();
        }
    }

    protected void gvProductBrandPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductBrand.PageIndex = e.NewPageIndex;
        gvProductBrand.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_brand.Value);
        gvProductBrand.DataBind();
    }

    protected void gvProductBrandSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductBrand = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_brand.Value);
        DataRow drProductBrand = dsProductBrand.Tables[0]?.Rows[(gvProductBrand.PageIndex * Convert.ToInt16(gvProductBrand.PageSize)) + e.NewSelectedIndex];

        txtPopupAdvanceSearchProductBrandCode.Text = drProductBrand[0].ToString();
        txtPopupAdvanceSearchProductBrandDescription.Text = drProductBrand[1].ToString();

        PopupAddProductBrand.ShowOnPageLoad = false;
        PopupAdvanceSearch.ShowOnPageLoad = true;
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

    #region Popup Add Product Code
    protected void LoadProductCodeData()
    {
        string sqlWhere = "";
        string loanType = txtPopupAdvanceSearchLoanTypeCode.Text;
        string productTypeCode = txtPopupAdvanceSearchProductTypeCode.Text;
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
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        dsProductCode = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductCode))
        {
            ds_product_code.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductCode);
            gvProductCode.DataSource = dsProductCode;
            gvProductCode.DataBind();
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
            dsProductCode.Tables.Add(dt);
            gvProductCode.DataSource = dsProductCode;
            gvProductCode.DataBind();
        }
        
    }

    protected void gvProductCodePageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductCode.PageIndex = e.NewPageIndex;
        gvProductCode.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_code.Value);
        gvProductCode.DataBind();
    }

    protected void gvProductCodeSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductCode = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_code.Value);
        DataRow drProductCode = dsProductCode.Tables[0]?.Rows[(gvProductCode.PageIndex * Convert.ToInt16(gvProductCode.PageSize)) + e.NewSelectedIndex];

        txtPopupAdvanceSearchProductCode.Text = drProductCode[4].ToString();
        txtPopupAdvanceSearchProductDescription.Text = drProductCode[5].ToString();

        PopupAddProductCode.ShowOnPageLoad = false;
        PopupAdvanceSearch.ShowOnPageLoad = true;
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

    #region Popup Add Product Model
    protected void LoadProductModelData()
    {
        string sqlWhere = "";
        string loanType = txtPopupAdvanceSearchLoanTypeCode.Text;
        string productTypeCode = txtPopupAdvanceSearchProductTypeCode.Text;
        string productCode = txtPopupAdvanceSearchProductCode.Text;
        string productBrand = txtPopupAdvanceSearchProductBrandCode.Text;

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
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        dsProductModel = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductModel))
        {
            ds_product_model.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductModel);

            gvProductModel.DataSource = dsProductModel;
            gvProductModel.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T43LTY");
            dt.Columns.Add("T00TNM");
            dt.Columns.Add("T43TYP");
            dt.Columns.Add("T40DES");
            dt.Columns.Add("T43BRD");
            dt.Columns.Add("T42DES");
            dt.Columns.Add("T43COD");
            dt.Columns.Add("T41DES");
            dt.Columns.Add("T43MDL");
            dt.Columns.Add("T43DES");
            dt.Columns.Add("T43RSV");
            dt.Columns.Add("T43DEL");
            dsProductModel.Tables.Add(dt);
            gvProductModel.DataSource = dsProductModel;
            gvProductModel.DataBind();
        }
    }

    protected void gvProductModelPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductModel.PageIndex = e.NewPageIndex;
        gvProductModel.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_model.Value);
        gvProductModel.DataBind();
    }

    protected void gvProductModelSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductModel = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_model.Value);
        DataRow drProductModel = dsProductModel.Tables[0]?.Rows[(gvProductModel.PageIndex * Convert.ToInt16(gvProductModel.PageSize)) + e.NewSelectedIndex];

        if (hdfFormSearch.Value == "Y")
        {
            txtLoanTypeCode.Text = drProductModel[0].ToString().Trim();
            txtLoanTypeDescription.Text = drProductModel[1].ToString().Trim();

            txtProductTypeCode.Text = drProductModel[2].ToString().Trim();
            txtProductTypeDescription.Text = drProductModel[3].ToString().Trim();

            txtProductBrandCode.Text = drProductModel[4].ToString().Trim();
            txtProductBrandDescription.Text = drProductModel[5].ToString().Trim();

            txtProductCode.Text = drProductModel[6].ToString().Trim();
            txtProductCodeDescription.Text = drProductModel[7].ToString().Trim();

            txtProductModelCode.Text = drProductModel[8].ToString().Trim();
            txtProductModelDescription.Text = drProductModel[9].ToString().Trim();

            txtProductGroupCode.Text = "0";
            txtProductItemAll.Text = txtProductCodeDescription.Text + " " + txtProductBrandDescription.Text + " " + txtProductModelDescription.Text;

            PopupAdvanceSearch.ShowOnPageLoad = false;
        }
        else
        {
            txtPopupAdvanceSearchProductModelCode.Text = drProductModel[8].ToString();
            txtPopupAdvanceSearchProductModelDescription.Text = drProductModel[9].ToString();
            PopupAdvanceSearch.ShowOnPageLoad = true;
        }

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

    #region Product Group Item
    #region GridView Product Group Item
    protected void LoadProductGroupItemData()
    {
        string productItemCode = txtProductGroupCode.Text.Trim();

        string sql = @"SELECT
                            DISTINCT T45ITG,
                            T45ITM,
                            T44DES
                        FROM
                            AS400DB01.ILOD0001.ILTB45 WITH (NOLOCK)
                        LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                        ON  T44LTY = T45LTY
                        AND T44ITM = T45ITM
                        WHERE
                            T44DEL = ''
                        AND T44PGP = 'N'
                        AND T45DEL = ''
                        AND T45ITG = " + productItemCode;

        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DataSet dsProductGroupItem = new DataSet();
        dsProductGroupItem = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductGroupItem))
        {
            ds_product_group_item.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductGroupItem);
            gvProductGroupItem.DataSource = dsProductGroupItem;
            gvProductGroupItem.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T45ITG");
            dt.Columns.Add("T45ITM");
            dt.Columns.Add("T44DES");
            dsProductGroupItem.Tables.Add(dt);
            gvProductGroupItem.DataSource = dsProductGroupItem;
            gvProductGroupItem.DataBind();
        }
        
    }

    protected void gvProductGroupItemPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductGroupItem.PageIndex = e.NewPageIndex;
        gvProductGroupItem.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_group_item.Value);
        gvProductGroupItem.DataBind();
    }

    protected void gvProductGroupItemRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet dsProductGroupItem = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_group_item.Value);
        DataRow drProductGroupItem = dsProductGroupItem.Tables[0]?.Rows[(gvProductGroupItem.PageIndex * Convert.ToInt16(gvProductGroupItem.PageSize)) + e.RowIndex];
        hdfProductItemCode.Value = drProductGroupItem["T45ITM"].ToString();

        ConfirmDeleteItemFromGroupMessage = "Confirm Delete";
        ConfirmDeleteItemFromGroupHeaderText = "Confirm Delete Data";
        SetConfirmDeleteItemFromMessage();
    }
    #endregion

    #region GridView Product Item
    protected void LoadProductItemGroup()
    {
        string loanTypeCode = txtLoanTypeCode.Text;
        string productTypeCode = txtProductTypeCode.Text;
        string productBrandCode = txtProductBrandCode.Text;
        string productCode = txtProductCode.Text;
        string productModelCode = txtProductModelCode.Text;

        string searchBy = ddlPopupAddProductItemSearchBy.SelectedValue;
        string searchText = txtPopupAddProductItemSearchText.Text.Trim().Replace("'", "''");
        string sqlWhere = "";

        if (!string.IsNullOrEmpty(searchText))
        {
            switch (searchBy)
            {
                case "PIC":
                    sqlWhere = " AND T44ITM = " + searchText;
                    break;
                case "PID":
                    sqlWhere = " AND UPPER(T44DES) LIKE '" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        string sql = @"SELECT
                            DISTINCT T44ITM,
                            T44DES
                        FROM
                            AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                        JOIN AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                        ON  T44TYP = T40TYP
                        AND T40DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK)
                        ON  T44COD = T41COD
                        AND T44TYP = T41TYP
                        AND T41DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)
                        ON  T44BRD = T42BRD
                        AND T42DEL = ''
                        JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                        ON  T44TYP = T43TYP
                        AND T44BRD = T43BRD
                        AND T44COD = T43COD
                        AND T44MDL = T43MDL
                        AND T44PGP = 'N'
                        AND T43DEL = ''
                        AND T44LTY = '" + loanTypeCode + @"'
                        AND T44TYP = " + productTypeCode + @"
                        AND T44BRD = " + productBrandCode + @"
                        AND T44COD = " + productCode + @"
                        AND T44MDL = " + productModelCode;

        sql = sql + sqlWhere;

        DataSet dsProductItem = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        dsProductItem = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(dsProductItem))
        {
            ds_product_item.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(dsProductItem);
            gvAddProductItem.DataSource = dsProductItem;
            gvAddProductItem.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T44ITM");
            dt.Columns.Add("T44DES");
            dsProductItem.Tables.Add(dt);
            gvAddProductItem.DataSource = dsProductItem;
            gvAddProductItem.DataBind();
        }
    }

    protected void gvAddProductItemPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAddProductItem.PageIndex = e.NewPageIndex;
        gvAddProductItem.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_item.Value);
        gvAddProductItem.DataBind();
    }

    protected void gvAddProductItemSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsProductItem = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_product_item.Value);
        DataRow drProductItem = dsProductItem.Tables[0]?.Rows[(gvAddProductItem.PageIndex * Convert.ToInt16(gvAddProductItem.PageSize)) + e.NewSelectedIndex];
        hdfProductItemCode.Value = drProductItem["T44ITM"].ToString();

        ConfirmAddItemToGroupHeaderText = "Confirm Add";
        ConfirmAddItemToGroupMessage = "Confirm Add " + drProductItem["T44DES"].ToString() + " to Group";
        SetConfirmAddItemToGroupMessage();
    }
    #endregion

    #region Add Item to Product Group
    protected void btnAddItemToGroupProductClick(object sender, EventArgs e)
    {
        ddlPopupAddProductItemSearchBy.SelectedIndex = 0;
        txtPopupAddProductItemSearchText.Text = "";
        PopupAddProductItem.ShowOnPageLoad = true;
        LoadProductItemGroup();
    }

    protected void AddItemToGroup()
    {
        //iDB2Command cmd = new iDB2Command();
        MessageHeadText = "Add Item to Group";

        try
        {
            m_userInfo = userInfoService.GetUserInfo();
            
            //ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string loanType = txtLoanTypeCode.Text;
            string productGroupCode = txtProductGroupCode.Text;
            string productItemCode = hdfProductItemCode.Value;
            string curDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
            string curTime = DateTime.Now.ToString("HHmmss", m_DThai);
            string curUser = m_userInfo.Username.ToString();
            string curWorkStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

            string sql = @"SELECT
	                        T45LTY, T45ITG, T45ITM, T45UDD, T45UDT, T45PGM, T45USR, T45DSP, T45DEL
                        FROM
	                        AS400DB01.ILOD0001.ILTB45 WITH (NOLOCK)
                        WHERE
	                        T45LTY = '" + loanType + @"'
                        AND	T45ITG = " + productGroupCode + @"
                        AND	T45ITM = " + productItemCode;

            DataSet ds = new DataSet();
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(ds))
            {
                DataRow dr = ds.Tables[0]?.Rows[0];
                if (dr["T45DEL"].ToString().Trim() == "")
                {
                    Message = "Add Item to Group is Duplicate";
                    SetMessage();
                    return;
                }

                sql = @"UPDATE
                            AS400DB01.ILOD0001.ILTB45 
                        SET
                            T45DEL = '',
                            T45UDD = " + curDate + @",
                            T45UDT = " + curTime + @",
                            T45USR = '" + curUser + @"',
                            T45PGM = '" + ProgramName + @"',
                            T45DSP = '" + curWorkStation + @"'
                        WHERE
                            T45LTY = '" + loanType + @"'
                        AND T45ITG = " + productGroupCode + @"
                        AND T45ITM = " + productItemCode;
            }
            else
            {
                sql = @"INSERT INTO AS400DB01.ILOD0001.ILTB45
                        (   
                            T45LTY,
                            T45ITG,
                            T45ITM,
                            T45UDD,
                            T45UDT,
                            T45USR,
                            T45PGM,
                            T45DSP
                        ) VALUES (
                            '" + loanType + @"',
                            " + productGroupCode + @",
                            " + productItemCode + @",
                            " + curDate + @",
                            " + curTime + @",
                            '" + curUser + @"',
                            '" + ProgramName + @"',
                            '" + curWorkStation + @"'
                        )";
            }

            //cmd.CommandText = sql;
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int result = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
            if (result == -1)
            {
                dataCenter.RollbackMssql();
                Message = "Add Item to Group Not Complete";
                SetMessage();
            }
            else
            {
                dataCenter.CommitMssql();
                Message = "Add Item to Group : " + productGroupCode + " Complete";
                LoadProductGroupItemData();
                SetMessageSuccess();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Add Item to Group Not Complete";
            SetMessage();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }
    }
    #endregion

    #region Popup Confirm Add Item to Group
    protected void SetConfirmAddItemToGroupMessage()
    {
        lblPopupConfirmAddItemToGroupMessage.Text = ConfirmAddItemToGroupMessage;
        PopupConfirmAddItemToGroup.HeaderText = ConfirmAddItemToGroupHeaderText;
        PopupConfirmAddItemToGroup.ShowOnPageLoad = true;
    }

    protected void PopupAddProductItemSearchClick(object sender, EventArgs e)
    {
        LoadProductItemGroup();
    }

    protected void btnPopupAddProductItemClearClick(object sender, EventArgs e)
    {
        ddlPopupAddProductItemSearchBy.SelectedIndex = 0;
        txtPopupAddProductItemSearchText.Text = "";
        LoadProductItemGroup();
    }

    protected void btnPopupConfirmAddItemToGroupOKClick(object sender, EventArgs e)
    {
        PopupAddProductItem.ShowOnPageLoad = false;
        AddItemToGroup();
    }

    protected void btnPopupConfirmAddItemToGroupCancelClick(object sender, EventArgs e)
    {

    }
    #endregion

    #region Delete Item from Group
    protected void DeleteItemFromGroup()
    {
        iDB2Command cmd = new iDB2Command();
        MessageHeadText = "Delete Item";

        try
        {
            m_userInfo = userInfoService.GetUserInfo();
            
            //ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string loanType = txtLoanTypeCode.Text;
            string productGroupCode = txtProductGroupCode.Text;
            string productItemCode = hdfProductItemCode.Value;
            string curDate = DateTime.Now.ToString("yyyyMMdd", m_DThai);
            string curTime = DateTime.Now.ToString("HHmmss", m_DThai);
            string curUser = m_userInfo.Username.ToString();
            string curWorkStation = m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim();

            string sql = "SELECT P1ITEM FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) WHERE P1ITEM = " + productGroupCode;

            DataSet ds = new DataSet();
            ds = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(ds))
            {
                Message = "Item is used, Can not Remove";
                SetMessage();
            }
            else
            {
                sql = @"UPDATE
                            AS400DB01.ILOD0001.ILTB45
                        SET
                            T45DEL = 'X',
                            T45UDD = " + curDate + @",
                            T45UDT = " + curTime + @",
                            T45USR = '" + curUser + @"',
                            T45PGM = '" + ProgramName + @"',
                            T45DSP = '" + curWorkStation + @"'
                        WHERE
                            T45LTY = '" + loanType + @"'
                        AND T45ITG = " + productGroupCode + @"
                        AND T45ITM = " + productItemCode;

                cmd.CommandText = sql;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int result = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (result == -1)
                {
                    dataCenter.RollbackMssql();
                    Message = "Delete Item Not Complete";
                    SetMessage();
                }
                else
                {
                    dataCenter.CommitMssql();
                    Message = "Delete Item Complete";
                    LoadProductGroupItemData();
                    SetMessageSuccess();
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Message = "Error on Delete Data. Please Try Again";
            SetMessage();
        }
        finally
        {
            dataCenter.CommitMssql();
        }
    }
    #endregion

    #region Popup Confirm Delete Item from Group
    private void SetConfirmDeleteItemFromMessage()
    {
        lblPopupConfirmDeleteItemFromGroupMessage.Text = ConfirmDeleteItemFromGroupMessage;
        PopupConfirmDeleteItemFromGroup.HeaderText = ConfirmDeleteItemFromGroupHeaderText;
        PopupConfirmDeleteItemFromGroup.ShowOnPageLoad = true;
    }

    protected void btnPopupConfirmDeleteItemFromGroupOKClick(object sender, EventArgs e)
    {
        DeleteItemFromGroup();
    }

    protected void btnPopupConfirmDeleteItemFromGroupCancelClick(object sender, EventArgs e)
    {

    }
    #endregion
    #endregion

}