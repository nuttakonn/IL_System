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

public partial class ManageData_WorkProcess_ProductItem : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    ILDataSubroutine iLDataSubroutine;
    ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    protected string LoanType = "";
    protected string LoanTypeName = "";
    protected string ProductType = "";
    protected string ProductTypeDes = "";
    protected string ProductCode = "";
    protected string ProductDesc = "";
    protected string BrandCode = "";
    protected string BrandName = "";
    protected string ProductModelCode = "";
    protected string ProductModelDesc = "";
    protected string ProductItemCode = "";
    protected string ProductItemDesc = "";
    protected string ProductItemDescAll = "";
    protected string WOERRF = "";
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
    protected string SqlAll = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";
    protected string View = ""; //ใช้เพื่อจุดประสงค์อะไร

    /*********** T44PGM ***********
     * จาก Delphi มีค่าเป็น ILE000011F
     * จาก Code ที่ได้รับมามีค่าเป็น IL_PROITM
     * ยึดจาก Code ที่ได้รับมา T44PGM = IL_PROITM
    */
    protected string ProgramName = "IL_PROITM";

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
        View = Request.QueryString["View"];
        if (View == "1")
        {
            SetEnabled(false);
        }
        else
        {
            SetEnabled(true);
        }

        if (Page.IsPostBack)
        {
            return;
        }
        //SearchData();
    }
    protected void TimerTick(object sender, EventArgs e)
    {
        SearchData();
        Timer1.Enabled = false;
    }

    public static class Globals
    {
        public static String productItemIdShow;
        public static String productItemDesShow;
    }

    #region Bind Data and Search
    protected void btnSearchClick(object sender, EventArgs e)
    {
        pProType.Text = "f";
        pProBrand.Text = "f";
        pProCode.Text = "f";
        pProModel.Text = "f";
        SearchData();
    }

    protected void SearchData()
    {
        string searchBy = ddl_SearchBy.SelectedValue;
        string productItem = txt_ProductItem.Text.Trim().Replace("'", "''");

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetProductItem(searchBy, productItem, 1, 15);

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
    private void SetEnabled(bool type)
    {
        btnAdd.Enabled = type;
        btnClearData.Enabled = type;
        btnAddProductModel.Enabled = type;
        txtProductItemDesc.Enabled = type;
    }

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
        PopupConfirm.HeaderText = CHText;
        PopupConfirm.ShowOnPageLoad = true;
    }

    private void SetClearSearch()
    {
        ddl_SearchBy.SelectedIndex = 0;
        txt_ProductItem.Text = "";
    }

    private void SetDefaultEdit()
    {
        lblLoanType.Text = "01";
        lblLoanTypeDesc.Text = "สินเชื่อเงินผ่อน";
        txtProductType.Text = ProductType;
        txtProductTypeDesc.Text = ProductTypeDes;
        txtProductCode.Text = ProductCode;
        txtProductDesc.Text = ProductDesc;
        txtBrandCode.Text = BrandCode;
        txtBrandName.Text = BrandName;
        txtProductModelCode.Text = ProductModelCode;
        txtProductModelDesc.Text = ProductModelDesc;
        txtProductItemCode.Text = ProductItemCode;
        txtProductItemDesc.Text = ProductItemDesc;
        txtItemDescAll.Text = ProductItemDescAll;
        btnAdd.Text = "Edit";
        lbl_AddEdit.Text = "Edit";
    }

    private void SetClearAddEdit()
    {
        lblLoanType.Text = "";
        lblLoanTypeDesc.Text = "";
        txtProductType.Text = "";
        txtProductTypeDesc.Text = "";
        txtProductCode.Text = "";
        txtProductDesc.Text = "";
        txtBrandCode.Text = "";
        txtBrandName.Text = "";
        txtProductModelCode.Text = "";
        txtProductModelDesc.Text = "";
        txtProductItemCode.Text = "";
        txtItemDescAll.Text = "";
        txtProductItemDesc.Text = "";

        btnAdd.Text = "Add";
        lbl_AddEdit.Text = "Add";
    }
    #endregion

    #region Select Page
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        //GridView1.DataSource = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_grid");
        //GridView1.DataSource = (DataSet)Session["ds_grid"];
        GridView1.DataBind();
    }
    #endregion

    #region Clear Data
    //search
    protected void btnClearClick(object sender, EventArgs e)
    {
        SetClearSearch();
        SearchData();
    }

    //add/edit
    protected void btnClearDataClick(object sender, EventArgs e)
    {
        SetClearAddEdit();
    }
    #endregion

    #region Select Data
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //DataSet ds_grid = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_grid");
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
        //DataSet ds_grid = (DataSet)Session["ds_grid"];
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.NewSelectedIndex];

        LoanType = dr[0].ToString().Trim(); ;
        LoanTypeName = dr[1].ToString().Trim(); ;
        ProductType = dr[2].ToString().Trim(); ;
        ProductTypeDes = dr[3].ToString().Trim();
        BrandCode = dr[4].ToString().Trim();
        BrandName = dr[5].ToString().Trim();
        ProductCode = dr[6].ToString().Trim();
        ProductDesc = dr[7].ToString().Trim();
        ProductModelCode = dr[8].ToString().Trim();
        ProductModelDesc = dr[9].ToString().Trim();
        ProductItemCode = dr[10].ToString().Trim();

        string ItemDesc = dr[11].ToString().Trim();
        string strItemDesc = ProductDesc + " " + BrandName + " " + ProductModelDesc;
        string[] resItemDesc = ItemDesc.Split(new string[] { strItemDesc }, StringSplitOptions.RemoveEmptyEntries);

        ProductItemDescAll = strItemDesc;
        if (resItemDesc.Length > 0)
        {

            ProductItemDesc = resItemDesc[0].Trim();
        }

        SetDefaultEdit();
        txtProductItemDesc.Focus();
        ResetGrid(GridView1, ds_Hiddengrid.Value);
    }

    private void ResetGrid(GridView GridView, string value)
    {
        GridView.PageIndex = 0;
        DataSet ds = new DataSet();
        ds = cookiesStorage.JsonDeserializeObjectHiddenDataSet(value);
        if (!cookiesStorage.check_dataset(ds))
        {
            return;
        }
        GridView.DataSource = ds;
        GridView.DataBind();
    }
    #endregion

    #region Insert and Update Data
    protected void btnAddClick(object sender, EventArgs e)
    {
        LoanType = lblLoanType.Text;
        ProductType = txtProductType.Text.Trim();
        ProductCode = txtProductCode.Text.Trim();
        BrandCode = txtBrandCode.Text.Trim();
        ProductModelCode = txtProductModelCode.Text.Trim();
        ProductItemCode = txtProductItemCode.Text.Trim();
        ProductItemDesc = txtProductItemDesc.Text.ToUpper().Trim();
        ProductItemDescAll = txtItemDescAll.Text.ToUpper().Trim();
        string ItemDescAll = txtItemDescAll.Text.ToUpper().Trim() + " " + ProductItemDesc;

        if (ProductModelCode == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Product Model";
            SetMsg();
            btnAddProductModel.Focus();
            return;
        }

        //if (ProductItemDesc == "")
        //{
        //    MsgHText = "Validate";
        //    Msg = " Please Input Item Desc.";
        //    SetMsg();
        //    txtProductItemDesc.Focus();
        //    return;
        //}

        SqlAll = @"SELECT
                        T44LTY, T44ITM, T44TYP, T44BRD, T44COD, T44MDL, T44PGP, T44DES, T44UDD, T44UDT, T44PGM, T44USR, T44DSP, T44DEL
                    FROM
                        AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                    WHERE
                        CAST(T44LTY as nvarchar) = '" + LoanType + @"'
                    AND CAST(T44TYP as nvarchar) = '" + ProductType + @"'
                    AND CAST(T44BRD as nvarchar) = '" + BrandCode + @"'
                    AND CAST(T44COD as nvarchar) = '" + ProductCode + @"'
                    AND CAST(T44MDL as nvarchar) = '" + ProductModelCode + @"'
                    AND CAST(T44DES as nvarchar) = '" + ItemDescAll + "'";


        if (ProductItemCode != "")
        {
            SqlAll = SqlAll + " AND CAST(T44ITM as nvarchar) = '" + ProductItemCode + "'";
        }

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found this Product Item is Duplicate ";
            MsgHText = "Validate Save";
            SetMsg();
        }
        else
        {
            CHText = "Confirm Save";
            CMsg = "Confirm Save Data ";
            SetConfirmMsg();
        }
    }
    #endregion

    #region Confirm Add/Edit
    protected void btnPopupConfirmOKClick(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        iDB2Command cmd = new iDB2Command();
        iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        DataSet DS = new DataSet();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        string NameButton = "";
        string Curtime = DateTime.Now.ToString("yyyyMMdd", m_DThai);


        LoanType = lblLoanType.Text;
        ProductType = txtProductType.Text.Trim();
        ProductTypeDes = txtProductTypeDesc.Text.ToUpper().Trim();
        ProductCode = txtProductCode.Text.Trim();
        ProductDesc = txtProductDesc.Text.ToUpper().Trim();
        BrandCode = txtBrandCode.Text.Trim();
        BrandName = txtBrandName.Text.ToUpper().Trim();
        ProductModelCode = txtProductModelCode.Text.Trim();
        ProductModelDesc = txtProductModelDesc.Text.ToUpper().Trim();
        ProductItemCode = txtProductItemCode.Text.Trim();
        ProductItemDesc = txtProductItemDesc.Text.ToUpper().Trim();
        ProductItemDescAll = txtItemDescAll.Text.ToUpper().Trim();

        try
        {
            if (ProductItemCode == "") // insert
            {
                SqlAll = @" SELECT
                                T44LTY, T44ITM, T44TYP, T44BRD, T44COD, T44MDL, T44PGP, T44DES, T44UDD, T44UDT, T44PGM, T44USR, T44DSP, T44DEL
                            FROM
                                AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK)
                            WHERE
                               CAST(T44DEL as nvarchar) = 'X'
                           AND CAST(T44LTY as nvarchar) = '" + LoanType + @"'
                           AND CAST(T44TYP as nvarchar) = '" + ProductType + @"'
                           AND CAST(T44BRD as nvarchar) = '" + BrandCode + @"'
                           AND CAST(T44COD as nvarchar) = '" + ProductCode + @"'
                           AND CAST(T44MDL as nvarchar) = '" + ProductModelCode + @"'
                           AND CAST(T44DES as nvarchar) = '" + ProductItemDescAll + "'";

                DataSet DS1 = new DataSet();
                
                ilObj.UserInfomation = m_userInfo;
                DS1 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (cookiesStorage.check_dataset(DS1))
                {
                    DataRow dr = DS1.Tables[0]?.Rows[0];
                    ProductItemCode = dr[0].ToString().Trim();
                    NameButton = "Edit";
                    MsgHText = NameButton;

                    SqlAll = @"UPDATE
                                   AS400DB01.ILOD0001.ILTB44
                               SET
                                   T43DEL = ''
                               WHERE
                                   T44LTY = '" + LoanType + @"'
                               AND T44TYP = " + ProductType + @"
                               AND T44BRD = " + BrandCode + @"
                               AND T44COD = " + ProductCode + @"
                               AND T44MDL = " + ProductModelCode + @"
                               AND T44ITM = " + ProductItemCode;

                    cmd.CommandText = SqlAll;
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resHome13 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (resHome13 == -1)
                    {
                        dataCenter.RollbackMssql();
                        Msg = NameButton + " Product Item  not complete.";
                        SetMsg();

                        return;
                    }
                    else
                    {
                        dataCenter.CommitMssql();
                    }
                }
                else
                {
                    NameButton = "Add";
                    MsgHText = NameButton;

                    string strBizInit = m_userInfo.BizInit.ToString();
                    string strBranchNo = m_userInfo.BranchNo;

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
                        MsgHText = NameButton;

                        //SqlAll = @"INSERT INTO ILTB44
                        //           (
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
                        //           )
                        //           VALUES
                        //           (
                        //                '" + LoanType + @"',
                        //                " + ProductType + @",
                        //                " + BrandCode + @",
                        //                " + ProductCode + @",
                        //                " + ProductModelCode + @",
                        //                " + ProductItemCode + @",
                        //                '" + ProductItemDescAll + " " + ProductItemDesc + @"',
                        //                'N',
                        //                " + Curtime + @",
                        //                " + m_UpdTime.ToString() + @",
                        //                '" + m_userInfo.Username.ToString() + @"',
                        //                '" + ProgramName + @"',
                        //                '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                        //           )";

                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB44(
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
                                                '{LoanType}',
                                                {ProductType},
                                                {BrandCode},
                                                {ProductCode},
                                                {ProductModelCode},
                                                {ProductItemCode},
                                                '{ProductItemDescAll + ProductItemDesc}',
                                                'N',
                                                {Curtime},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                '{ProgramName}',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}'
                                    )";


                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("T44LTY", LoanType);
                        //cmd.Parameters.Add("T44TYP", ProductType);
                        //cmd.Parameters.Add("T44BRD", BrandCode);
                        //cmd.Parameters.Add("T44COD", ProductCode);
                        //cmd.Parameters.Add("T44MDL", ProductModelCode);
                        //cmd.Parameters.Add("T44ITM", ProductItemCode);
                        //cmd.Parameters.Add("T44DES", ProductItemDescAll + ProductItemDesc);
                        //cmd.Parameters.Add("T44PGP", "N");
                        //cmd.Parameters.Add("T44UDD", Curtime);
                        //cmd.Parameters.Add("T44UDT", m_UpdTime.ToString());
                        //cmd.Parameters.Add("T44USR", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("T44PGM", ProgramName);
                        //cmd.Parameters.Add("T44DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        cmd.CommandText = SqlAll;
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        if (resHome11 == -1)
                        {
                            dataCenter.RollbackMssql();
                            Msg = NameButton + " Product Item  not complete.";
                            SetMsg();

                            return;
                        }
                        else
                        {
                            dataCenter.CommitMssql();
                        }
                    }

                }
                dataCenter.CloseConnectSQL();
            }
            else // update
            {
                NameButton = "Edit";
                MsgHText = NameButton;

                //SqlAll = @"UPDATE
                //               ILTB44
                //           SET
                //               T44DEL = '',
                //               T44DES = '" + ProductItemDescAll + " " + ProductItemDesc + @"',
                //               T44UDD = " + Curtime + @",
                //               T44UDT = " + m_UpdTime.ToString() + @",
                //               T44USR = '" + m_userInfo.Username.ToString() + @"',
                //               T44PGM = '" + ProgramName + @"',
                //               T44DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                //           WHERE
                //               T44LTY = '" + LoanType + @"'
                //           AND T44TYP = " + ProductType + @"
                //           AND T44BRD = " + BrandCode + @"
                //           AND T44COD = " + ProductCode + @"
                //           AND T44MDL = " + ProductModelCode + @"
                //           AND T44ITM = " + ProductItemCode;

                SqlAll = $@"Update AS400DB01.ILOD0001.ILTB44 
                                SET T44DEL = '',
                                    T44DES = '{ProductItemDescAll + ProductItemDesc}',                   
                                    T44UDD = {Curtime},
                                    T44UDT = {m_UpdTime.ToString()},
                                    T44USR = '{m_userInfo.Username.ToString()}',
                                    T44PGM = '{ProgramName}',
                                    T44DSP = '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}'
                                WHERE T44LTY = '" + LoanType + @"'
                                    AND T44TYP = " + ProductType + @"
                                    AND T44BRD = " + BrandCode + @"
                                    AND T44COD = " + ProductCode + @"
                                    AND T44MDL = " + ProductModelCode + @"
                                    AND T44ITM = " + ProductItemCode;

                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("T44DEL", string.Empty);
                //cmd.Parameters.AddWithValue("T44DES", ProductItemDescAll + ProductItemDesc);
                //cmd.Parameters.AddWithValue("T44UDD", Curtime);
                //cmd.Parameters.AddWithValue("T44UDT", m_UpdTime.ToString());
                //cmd.Parameters.AddWithValue("T44USR", m_userInfo.Username.ToString());
                //cmd.Parameters.AddWithValue("T44PGM", ProgramName);
                //cmd.Parameters.AddWithValue("T44DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());


                cmd.CommandText = SqlAll;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resHome13 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resHome13 == -1)
                {
                    dataCenter.RollbackMssql();
                    Msg = NameButton + " Product Item : " + ProductItemCode + " - " + ProductItemDescAll + " " + ProductItemDesc + " not complete.";
                    SetMsg();

                    return;
                }
                else
                {
                    dataCenter.CommitMssql();
                }
            }
            if (WOERRF != "Y")
            {
                Msg = NameButton + " Product Item : " + ProductItemCode + " - " + ProductItemDescAll + " " + ProductItemDesc + " complete";
                SetMsgSuccess();
                SearchData();
                SetClearAddEdit();
            }

        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = NameButton + " Product Item : " + ProductItemCode + " - " + ProductItemDescAll + " " + ProductItemDesc + " not complete";
            SetMsg();
            return;
        }
    }

    protected void btnPopupConfirmCancelClick(object sender, EventArgs e)
    {
        btnAddProductModel.Focus();
    }
    #endregion

    #region Delete Data
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (String.IsNullOrEmpty(View)) // View == "")
        {
            DataSet ds_grid = (DataSet)cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
            //DataSet ds_grid = (DataSet)Session["ds_grid"];
            if (!cookiesStorage.check_dataset(ds_grid))
            {
                Msg = "Cannot Delete, please select product item again";
                MsgHText = "Delete product";
                SetMsg();
                return;
            }
            DataRow dr = ds_grid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.RowIndex];
            LoanType = dr[0].ToString().Trim(); ;
            LoanTypeName = dr[1].ToString().Trim(); ;
            ProductType = dr[2].ToString().Trim(); ;
            ProductTypeDes = dr[3].ToString().Trim();
            BrandCode = dr[4].ToString().Trim();
            BrandName = dr[5].ToString().Trim();
            ProductCode = dr[6].ToString().Trim();
            ProductDesc = dr[7].ToString().Trim();
            ProductModelCode = dr[8].ToString().Trim();
            ProductModelDesc = dr[9].ToString().Trim();
            ProductItemCode = dr[10].ToString().Trim();
            ProductItemDesc = dr[11].ToString().Trim();
            Globals.productItemIdShow = dr[10].ToString().Trim();
            Globals.productItemDesShow = dr[11].ToString().Trim();
            if (validateDelete(LoanType))
            {
                m_userInfo = userInfoService.GetUserInfo();
                dataCenter = new DataCenter(m_userInfo);
                /********** Comment for Test **********/
                //string Curtime = "";
                //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
                //ilObj.CloseConnectioDAL();
                string Curtime = DateTime.Now.ToString("yyyyMMdd", m_DThai);
                /**************************************/

                SqlAll = @"UPDATE
                               AS400DB01.ILOD0001.ILTB44
                           SET
                               T44DEL = 'X',
                               T44UDD = " + Curtime + @",
                               T44UDT = " + m_UpdTime.ToString() + @",
                               T44USR = '" + m_userInfo.Username.ToString() + @"',
                               T44DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"',
                               T44PGM = '" + ProgramName + @"'
                           WHERE
                               T44LTY = '" + LoanType + @"'
                           AND T44TYP = " + ProductType + @"
                           AND T44BRD = " + BrandCode + @"
                           AND T44COD = " + ProductCode + @"
                           AND T44MDL = " + ProductModelCode + @"
                           AND T44ITM = " + ProductItemCode;

                txtProductItem_D.Text = ProductItemCode;
                txtSqlAll.Text = SqlAll;
                lblConfimMsg_Delete.Text = "Confirm Delete Data ";
                PopupConfirmDelete.ShowOnPageLoad = true;
            }
        }
    }

    private bool validateDelete(string loanType)
    {
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = @"SELECT
                       T45ITM
                   FROM
                       AS400DB01.ILOD0001.ILTB45 WITH (NOLOCK)
                   WHERE
                       T45DEL = ''
                   AND T45LTY = '" + loanType + @"'
                   AND T45ITM = " + ProductItemCode;

        DataSet DS = new DataSet();
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Cannot Delete,  Found this Item in Group Item";
            MsgHText = "Validate Delete";
            SetMsg();
            return false;
        }

        SqlAll = @"SELECT
                       DISTINCT P1APNO
                   FROM
                       AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                   WHERE
                       P1LTYP = '" + loanType + @"'
                   AND P1ITEM = " + ProductItemCode;

        DS = new DataSet();
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Cannot Delete,  Found this Item in  APPLICATION NO";
            MsgHText = "Validate Delete";
            SetMsg();
            return false;
        }

        SqlAll = @"SELECT
                       DISTINCT C07CMP
                   FROM
                       AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK)
                   WHERE
                       C07LNT= '01'
                   AND C07PIT= " + ProductItemCode;

        DS = new DataSet();
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Cannot Delete,  Found this Item in Campaign";
            MsgHText = "Validate Delete";
            SetMsg();
            return false;
        }

        return true;
    }
    #endregion

    #region Confirm Delete
    protected void btnPopupConfirmDeleteOKClick(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = txtSqlAll.Text;
        cmd.CommandText = SqlAll;
        ProductDesc = txtProductItem_D.Text;

        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                Msg = " Delete Product Item : " + Globals.productItemIdShow + " - " + Globals.productItemDesShow + " Not Complete.";
                SetMsg();
            }
            else
            {
                dataCenter.CommitMssql();
                Msg = " Delete Product Item : " + Globals.productItemIdShow + " - " + Globals.productItemDesShow + " Complete.";
                SetMsgSuccess();
                GridView1.Focus();
                SearchData();
                SetClearAddEdit();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = " Delete Product Item : " + Globals.productItemIdShow + " - " + Globals.productItemDesShow + " Not Complete.";
            SetMsg();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
        }
    }

    protected void btnPopupConfirmDeleteCancelClick(object sender, EventArgs e)
    {
        GridView1.Focus();
    }
    #endregion

    #region Message All
    protected void btnPopupMessageOKClick(object sender, EventArgs e)
    {
        //btnProductType.Focus();
    }
    #endregion

    #region Popup Product Type
    private void LoadDataPopupProductType()
    {
        string sqlWhere = "";
        string searchBy = ddlPopupProductTypeSearchBy.SelectedValue;
        string searchText = txtPopupProductTypeSearchText.Text.Trim();

        SqlAll = @"SELECT
                       T40LTY,
                       T00TNM,
                       T40TYP,
                       T40DES
                   FROM
                       AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                   LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                   ON  T40LTY = T00LTY
                   WHERE
                       T40DEL = '' ";

        if (searchText != "")
        {
            switch (searchBy)
            {
                case "PT":
                    sqlWhere = " AND  CAST(T40TYP as nvarchar) = " + "'"+ searchText + "'";
                    break;
                case "PD":
                    sqlWhere = " AND T40DES LIKE '" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        SqlAll = SqlAll + sqlWhere + " ORDER BY T40TYP ASC ";

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup_type.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView2.DataSource = DS;
            GridView2.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T40LTY");
            dt.Columns.Add("T00TNM");
            dt.Columns.Add("T40TYP");
            dt.Columns.Add("T40DES");
            DS.Tables.Add(dt);
            GridView2.DataSource = DS;
            GridView2.DataBind();
        }
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_type.Value);
        //GridView2.DataSource = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_type");
        //GridView2.DataSource = (DataSet)Session["ds_popup_type"];
        GridView2.DataBind();
    }

    protected void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_type.Value);
        //DataSet ds_grid = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_type");
        //DataSet ds_grid = (DataSet)Session["ds_popup_type"];
        if (!cookiesStorage.check_dataset(ds_grid))
        {
            Msg = "Cannot select, product type  ";
            MsgHText = "Error";
            SetMsg();
            return ;
        }
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];

        if (pProType.Text == "f")
        {
            txtPopupAdvanceSearchProductTypeCode.Text = dr[2].ToString().Trim();
            txtPopupAdvanceSearchProductTypeDesc.Text = dr[3].ToString().Trim();
            btnPopupAdvanceSearchProductType.Focus();
            PopupAddProductType.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
        }
        else
        {
            txtPopupAdvanceSearchProductTypeCode.Text = dr[2].ToString().Trim();
            txtPopupAdvanceSearchProductTypeDesc.Text = dr[3].ToString().Trim();
            PopupAddProductType.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchProductType.Focus();
        }
        ResetGrid(GridView2, ds_popup_type.Value);
        //ResetGrid(GridView2, "ds_popup_type");
    }

    protected void btnPopupProductTypeSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupProductType();
    }

    protected void btnPopupProductTypeClearClick(object sender, EventArgs e)
    {
        ddlPopupProductTypeSearchBy.SelectedIndex = 0;
        txtPopupProductTypeSearchText.Text = "";
        LoadDataPopupProductType();
    }
    #endregion

    #region Popup Product Code
    private void LoadDataPopupProductCode()
    {
        string sqlWhere = "";
        string searchBy = ddlPopupProductCodeSearchBy.SelectedValue;
        string searchText = txtPopupProductCodeSearchText.Text.Trim();

        if (searchText != "")
        {
            switch (searchBy)
            {
                case "PT":
                    sqlWhere = " AND CAST(T41TYP as nvarchar) = " + "'"+ searchText + "'";
                    break;
                case "PC":
                    sqlWhere = " AND CAST(T41COD as nvarchar) = " + "'" + searchText + "'";
                    break;
                case "PD":
                    sqlWhere = " AND T41DES LIKE '" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        if (txtPopupAdvanceSearchProductTypeCode.Text.Trim() != "")
        {
            sqlWhere += " AND TB41.T41TYP =  " + txtPopupAdvanceSearchProductTypeCode.Text.Trim();
        }

        SqlAll = @"SELECT
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

        SqlAll = SqlAll + sqlWhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup_code.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView4.DataSource = DS;
            GridView4.DataBind();
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
            GridView4.DataSource = DS;
            GridView4.DataBind();
        }
    }

    protected void btnProductCodeClick(object sender, EventArgs e)
    {
        PopupAddProductCode.ShowOnPageLoad = true;
        pProCode.Text = "f";
        PopupAddProductCode.Focus();
    }

    protected void btnPopupProductCodeSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupProductCode();
    }

    protected void btnPopupProductCodeClearClick(object sender, EventArgs e)
    {
        ddlPopupProductCodeSearchBy.SelectedIndex = 0;
        txtPopupProductCodeSearchText.Text = "";
        LoadDataPopupProductCode();
    }

    protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView4.PageIndex = e.NewPageIndex;
        //GridView4.DataSource = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_code");
        GridView4.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_code.Value);
        //GridView4.DataSource = (DataSet)Session["ds_popup_code"];
        GridView4.DataBind();
    }

    protected void GridView4_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_code.Value);
        //DataSet ds_grid = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_code");
        //DataSet ds_grid = (DataSet)Session["ds_popup_code"];
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView4.PageIndex * Convert.ToInt16(GridView4.PageSize)) + e.NewSelectedIndex];
        if (pProType.Text == "f")
        {
            txtPopupAdvanceSearchProductCode.Text = dr[4].ToString().Trim();
            txtPopupAdvanceSearchProductDesc.Text = dr[5].ToString().Trim();
            PopupAddProductCode.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchProductCode.Focus();
        }
        else
        {
            txtPopupAdvanceSearchProductCode.Text = dr[4].ToString().Trim();
            txtPopupAdvanceSearchProductDesc.Text = dr[5].ToString().Trim();
            PopupAddProductCode.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchProductCode.Focus();
        }
        ResetGrid(GridView4, ds_popup_code.Value);
        //ResetGrid(GridView4, "ds_popup_code");
    }
    #endregion

    #region Popup Brand
    private void LoadDataPopupBrand()
    {
        string searchBy = ddlPopupBrandSearchBy.SelectedValue;
        string brand = txtPopupBrandSearchText.Text.Trim();

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetBrand(searchBy, brand, 1, 15);
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup_brand.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            if (cookiesStorage.check_dataset(DS))
            {
                GridView3.DataSource = DS;
                GridView3.DataBind();
            }
          
            DataRow dr = DS.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = GridView3.PageIndex * GridView3.PageSize + 1;
            int _CurrentRecEnd = GridView3.PageIndex * GridView3.PageSize + GridView3.Rows.Count;

            lblGridView3.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("T42BRD");
            dt.Columns.Add("T42DES");
            DS.Tables.Add(dt);
            GridView3.DataSource = DS;
            GridView3.DataBind();
        }
        
    }

    protected void btnPopupBrandSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupBrand();
    }

    protected void btnPopupBrandClearClick(object sender, EventArgs e)
    {
        ddlPopupBrandSearchBy.SelectedIndex = 0;
        txtPopupBrandSearchText.Text = "";
        LoadDataPopupBrand();
    }

    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView3.PageIndex = e.NewPageIndex;
        //GridView3.DataSource = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_brand");
        GridView3.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_brand.Value);
        //GridView3.DataSource = (DataSet)Session["ds_popup_brand"];
        GridView3.DataBind();
    }

    protected void GridView3_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_brand.Value);
        //DataSet ds_grid = (DataSet)cookiesStorage.GetCookiesDataSetByKey("ds_popup_brand");
        //DataSet ds_grid = (DataSet)Session["ds_popup_brand"];
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView3.PageIndex * Convert.ToInt16(GridView3.PageSize)) + e.NewSelectedIndex];

        if (pProType.Text == "f")
        {
            txtPopupAdvanceSearchBrandCode.Text = dr[0].ToString().Trim();
            txtPopupAdvanceSearchBrandName.Text = dr[1].ToString().Trim();
            PopupAddBrand.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchBrand.Focus();
        }
        else
        {
            txtPopupAdvanceSearchBrandCode.Text = dr[0].ToString().Trim();
            txtPopupAdvanceSearchBrandName.Text = dr[1].ToString().Trim();
            PopupAddBrand.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchBrand.Focus();
        }
        ResetGrid(GridView3, ds_popup_brand.Value);
        //ResetGrid(GridView3, "ds_popup_brand");
    }
    #endregion

    #region Popup Product Model
    protected void btnAddProductModelClick(object sender, EventArgs e)
    {
        fProModel.Text = "f";
        pProModel.Text = "f";
        pProType.Text = "f";
        pProCode.Text = "f";
        pProBrand.Text = "f";

        tr_ProductItem.Visible = false;
        PopupAdvanceSearch.ShowOnPageLoad = true;
        PopupAdvanceSearch.HeaderText = "Add Product Item";

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDesc.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDesc.Text = "";

        txtPopupAdvanceSearchBrandCode.Text = "";
        txtPopupAdvanceSearchBrandName.Text = "";

        txtPopupAdvanceSearchModelCode.Text = "";
        txtPopupAdvanceSearchModelDesc.Text = "";

        txtPopupAdvanceSearchProductTypeDesc.Focus();
    }

    protected void btnPopupProductModelSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupProductModel();
    }

    private void LoadDataPopupProductModel()
    {
        string searchBy = ddlPopupProductModelSearchBy.SelectedValue;
        string searchText = txtPopupProductModelSearchText.Text.Trim();

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetProductModel(searchBy, searchText, 1, 15);

        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup_model.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView5.DataSource = DS;
            GridView5.DataBind();
            DataRow dr = DS.Tables[0].Rows[0];
            int totalRows = int.Parse(dr["TotalRows"].ToString());
            int _CurrentRecStart = GridView5.PageIndex * GridView5.PageSize + 1;
            int _CurrentRecEnd = GridView5.PageIndex * GridView5.PageSize + GridView5.Rows.Count;

            lblGridView5.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, totalRows);
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
            DS.Tables.Add(dt);
            GridView5.DataSource = DS;
            GridView5.DataBind();
        }
        
        dataCenter.CloseConnectSQL();
    }

    protected void GridView5_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView5.PageIndex = e.NewPageIndex;
        GridView5.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_model.Value);
        GridView5.DataBind();
    }

    protected void GridView5_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_model.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView5.PageIndex * Convert.ToInt16(GridView5.PageSize)) + e.NewSelectedIndex];
        if (fProModel.Text == "f")
        {
            txtProductType.Text = dr[2].ToString().Trim();
            txtProductTypeDesc.Text = dr[3].ToString().Trim();
            txtBrandCode.Text = dr[4].ToString().Trim();
            txtBrandName.Text = dr[5].ToString().Trim();
            txtProductCode.Text = dr[6].ToString().Trim();
            txtProductDesc.Text = dr[7].ToString().Trim();
            txtProductModelCode.Text = dr[8].ToString().Trim();
            //ไม่มีการ Assign ค่า Product Item Code ทำให้ไม่สามารถ Add Product Item (Insert) ได้
            //txtProductItemCode.Text = ""; 
            txtProductModelDesc.Text = dr[9].ToString().Trim();
            txtItemDescAll.Text = txtProductDesc.Text + " " + txtBrandName.Text + " " + txtProductModelDesc.Text;
            lblLoanType.Text = dr[0].ToString().Trim(); ;
            lblLoanTypeDesc.Text = dr[1].ToString().Trim(); ;
            PopupAddProductModel.ShowOnPageLoad = false;

            txtPopupAdvanceSearchProductTypeCode.Text = "";
            txtPopupAdvanceSearchProductTypeDesc.Text = "";

            txtPopupAdvanceSearchProductCode.Text = "";
            txtPopupAdvanceSearchProductDesc.Text = "";

            txtPopupAdvanceSearchBrandCode.Text = "";
            txtPopupAdvanceSearchBrandName.Text = "";

            txtPopupAdvanceSearchModelCode.Text = "";
            txtPopupAdvanceSearchModelDesc.Text = "";

            txtPopupAdvanceSearchProductItemCode.Text = "";
            txtPopupAdvanceSearchProductItemDesc.Text = "";

            fProModel.Text = "";
            txtProductItemDesc.Focus();
        }
        else
        {
            txtPopupAdvanceSearchModelCode.Text = dr[8].ToString().Trim();
            txtPopupAdvanceSearchModelDesc.Text = dr[9].ToString().Trim();
            PopupAddProductModel.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnPopupAdvanceSearchModel.Focus();
            //pProModel.Text = "f";
        }
        ResetGrid(GridView5, ds_popup_model.Value);
        //ResetGrid(GridView5, "ds_popup_model");
    }

    protected void btnPopupProductModelClearClick(object sender, EventArgs e)
    {
        ddlPopupProductModelSearchBy.SelectedIndex = 0;
        txtPopupProductModelSearchText.Text = "";
        LoadDataPopupProductModel();
    }
    #endregion

    #region Advance Seach
    protected void btnAdvanceSearchClick(object sender, EventArgs e)
    {
        pProType.Text = "p";
        pProBrand.Text = "p";
        pProCode.Text = "p";
        pProModel.Text = "p";
        fProModel.Text = "";
        tr_ProductItem.Visible = true;
        PopupAdvanceSearch.ShowOnPageLoad = true;
        PopupAdvanceSearch.HeaderText = "Advance Search";

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDesc.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDesc.Text = "";

        txtPopupAdvanceSearchBrandCode.Text = "";
        txtPopupAdvanceSearchBrandName.Text = "";

        txtPopupAdvanceSearchModelCode.Text = "";
        txtPopupAdvanceSearchModelDesc.Text = "";

        txtPopupAdvanceSearchProductItemCode.Text = "";
        txtPopupAdvanceSearchProductItemDesc.Text = "";

        txtPopupAdvanceSearchProductTypeDesc.Focus();
    }

    protected void btnPopupAdvanceSearchProductTypeClick(object sender, EventArgs e)
    {
        pProBrand.Text = "p";
        ddlPopupProductTypeSearchBy.SelectedIndex = 0;
        txtPopupProductTypeSearchText.Text = "";
        LoadDataPopupProductType();
        PopupAddProductType.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddProductType.Focus();
        GridView2.SelectedIndex = -1;
    }

    protected void btnPopupAdvanceSearchProductCodeClick(object sender, EventArgs e)
    {
        pProCode.Text = "p";
        ddlPopupProductCodeSearchBy.SelectedIndex = 0;
        txtPopupProductCodeSearchText.Text = "";
        LoadDataPopupProductCode();
        PopupAddProductCode.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddProductCode.Focus();
    }

    protected void btnPopupAdvanceSearchBrandClick(object sender, EventArgs e)
    {
        pProBrand.Text = "p";
        ddlPopupBrandSearchBy.SelectedIndex = 0;
        txtPopupBrandSearchText.Text = "";
        LoadDataPopupBrand();
        PopupAddBrand.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddBrand.Focus();
    }

    protected void btnPopupAdvanceSearchModelClick(object sender, EventArgs e)
    {
        pProModel.Text = "p";
        ddlPopupProductModelSearchBy.SelectedIndex = 0;
        txtPopupProductModelSearchText.Text = "";
        LoadDataPopupProductModel();
        PopupAddProductModel.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddProductModel.Focus();
    }

    protected void btnPopupAdvanceSearchClearClick(object sender, EventArgs e)
    {
        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDesc.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDesc.Text = "";

        txtPopupAdvanceSearchBrandCode.Text = "";
        txtPopupAdvanceSearchBrandName.Text = "";

        txtPopupAdvanceSearchModelCode.Text = "";
        txtPopupAdvanceSearchModelDesc.Text = "";

        txtPopupAdvanceSearchProductItemCode.Text = "";
        txtPopupAdvanceSearchProductItemDesc.Text = "";

        btnPopupAdvanceSearch.Focus();
    }

    protected void btnPopupAdvanceSearchClick(object sender, EventArgs e)
    {
        if (fProModel.Text == "")
        {
            LoadDataPopupSearch();
        }
        else
        {
            PopupAddProductModel.Focus();
            LoadDataPopupProductModel();
            PopupAdvanceSearch.ShowOnPageLoad = false;
            PopupAddProductModel.ShowOnPageLoad = true;
        }

        ddlPopupProductTypeSearchBy.SelectedIndex = 0;
        txtPopupProductTypeSearchText.Text = "";

        ddlPopupBrandSearchBy.SelectedIndex = 0;
        txtPopupBrandSearchText.Text = "";

        ddlPopupProductCodeSearchBy.SelectedIndex = 0;
        txtPopupProductCodeSearchText.Text = "";

        ddlPopupProductModelSearchBy.SelectedIndex = 0;
        txtPopupProductModelSearchText.Text = "";
    }

    private void LoadDataPopupSearch()
    {
        PopupAdvanceSearch.ShowOnPageLoad = false;
        string sqlWhere = "";

        SqlAll = @"SELECT
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
                   AND T44LTY = '01'  
                   AND T44PGP = 'N' ";

        if (txtPopupAdvanceSearchProductTypeCode.Text.Trim() != "")
        {
            sqlWhere += " AND CAST(T44TYP as nvarchar) = '" + txtPopupAdvanceSearchProductTypeCode.Text.Trim() + "'";
        }

        if (txtPopupAdvanceSearchProductCode.Text.Trim() != "")
        {
            sqlWhere += " AND CAST(T44COD as nvarchar) = '" + txtPopupAdvanceSearchProductCode.Text.Trim() + "'";
        }

        if (txtPopupAdvanceSearchBrandCode.Text.Trim() != "")
        {
            sqlWhere += " AND CAST(T44BRD as nvarchar) = '" + txtPopupAdvanceSearchBrandCode.Text.Trim() + "'";
        }

        if (txtPopupAdvanceSearchModelCode.Text.Trim() != "")
        {
            sqlWhere += " AND CAST(T44MDL as nvarchar) = '" + txtPopupAdvanceSearchModelCode.Text.Trim() + "'";
        }

        if (txtPopupAdvanceSearchProductItemCode.Text.Trim() != "")
        {
            sqlWhere += " AND CAST(T44ITM as nvarchar) = '" + txtPopupAdvanceSearchProductItemCode.Text.Trim() + "'";
        }

        if (txtPopupAdvanceSearchProductItemDesc.Text.Trim() != "")
        {
            sqlWhere += " AND T44DES LIKE '" + txtPopupAdvanceSearchProductItemDesc.Text.ToUpper().Trim() + "%'";
        }

        SqlAll = SqlAll + sqlWhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            cookiesStorage.SetCookiesDataSetByName("ds_grid", DS);
            //ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            //Session["ds_grid"] = DS;
            GridView1.DataSource = DS;
            GridView1.DataBind();
            dataCenter.CloseConnectSQL();
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
            DS.Tables.Add(dt);
            GridView1.DataSource = DS;
            GridView1.DataBind();
        }
        

        txtPopupAdvanceSearchProductTypeCode.Text = "";
        txtPopupAdvanceSearchProductTypeDesc.Text = "";

        txtPopupAdvanceSearchProductCode.Text = "";
        txtPopupAdvanceSearchProductDesc.Text = "";

        txtPopupAdvanceSearchBrandCode.Text = "";
        txtPopupAdvanceSearchBrandName.Text = "";

        txtPopupAdvanceSearchModelCode.Text = "";
        txtPopupAdvanceSearchModelDesc.Text = "";

        txtPopupAdvanceSearchProductItemCode.Text = "";
        txtPopupAdvanceSearchProductItemDesc.Text = "";

        /*if (GridView1.Rows.Count == 0)
        {
            E_error.Text = "Data not found";            
        }*/

    }
    #endregion

}