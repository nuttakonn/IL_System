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

public partial class ManageData_WorkProcess_ProductModel : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
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
    public UserInfoService userInfoService;
    protected string SqlAll = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";
    public CookiesStorage cookiesStorage;

    /*********** T43PGM  ***********
     * จาก Delphi ทีค่าเป็น ILE000009F
     * จาก Code ที่ได้รับมา มีค่าเป็น IL_PROMDL
     * ยึดตาม Code ที่ได้รับมา : T43PGM = IL_PROMDL
    */
    protected string ProgramName = "IL_PROMDL";

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
        //SearchData();
    }
    protected void TimerTick(object sender, EventArgs e)
    {
        SearchData();
        Timer1.Enabled = false;
    }
    public static class Globals
    {
        public static String productModelIdShow;
        public static String productModelDesShow;
    }
    #region Bind Data and Search
    protected void btnSearchClick(object sender, EventArgs e)
    {
        //f : Form
        //p : Popup
        pProType.Text = "f";
        pProBrand.Text = "f";
        pProCode.Text = "f";
        SearchData();
    }

    void SearchData()
    {
        string productModel = txt_ProductModel.Text.Trim().Replace("'", "''");
        string searchBy = ddl_SearchBy.SelectedValue;
        DataSet DS = new DataSet();
      
        m_userInfo = userInfoService.GetUserInfo();
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetProductModel(searchBy, productModel, 1, 15);

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
        btnAddProductModel.Enabled = type;
        btnClearData.Enabled = type;
        btnSearch.Enabled = type;
        btnSelectProductCode.Enabled = type;
        btnSelectProductType.Enabled = type;
        btnSelectBrand.Enabled = type;
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
        txt_ProductModel.Text = "";
    }

    private void SetDefaultEdit()
    {
        btnSelectProductType.Focus();
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
        btnAddProductModel.Text = "Edit";
        lblAddEdit.Text = "Edit";
    }

    private void SetClearAddEdit()
    {
        btnSelectProductType.Focus();
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

        btnAddProductModel.Text = "Add";
        lblAddEdit.Text = "Add";
        btnSelectProductCode.Enabled = true;
        btnSelectProductType.Enabled = true;
        btnSelectBrand.Enabled = true;
    }
    #endregion

    #region Select Page
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
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
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
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

        SetDefaultEdit();

        btnSelectProductCode.Enabled = false;
        btnSelectProductType.Enabled = false;
        btnSelectBrand.Enabled = false;
        txtProductModelDesc.Focus();

        ResetGrid(GridView1, ds_Hiddengrid.Value);
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();
    }
    #endregion

    #region Insert and Update Data
    protected void btnAddProductModelClick(object sender, EventArgs e)
    {
        string sqlWhere = string.Empty;
        LoanType = lblLoanType.Text;
        ProductType = txtProductType.Text.Trim();
        ProductCode = txtProductCode.Text.Trim();
        BrandCode = txtBrandCode.Text.Trim();
        ProductModelCode = txtProductModelCode.Text.Trim();
        ProductModelDesc = txtProductModelDesc.Text.ToUpper().Trim();

        if (ProductType == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Product Type";
            SetMsg();
            btnSelectProductType.Focus();
            return;
        }

        if (ProductCode == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Product Code";
            SetMsg();
            btnSelectProductCode.Focus();
            return;
        }

        if (BrandCode == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Brand";
            SetMsg();
            btnSelectBrand.Focus();
            return;
        }

        if (ProductModelDesc == "")
        {
            MsgHText = "Validate";
            Msg = " Please Input Model Desc.";
            SetMsg();
            txtProductModelDesc.Focus();
            return;
        }

        SqlAll = @"SELECT T43LTY,T43TYP,T43BRD,T43COD
                          ,T43MDL,T43DES,T43RSV,T43UDD
                          ,T43UDT,T43PGM,T43USR,T43DSP,T43DEL
                   FROM
                       AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                   WHERE
                       T43DEL = ''
                   AND T43LTY = '" + LoanType + @"'
                   AND T43TYP = " + ProductType + @"
                   AND T43BRD = " + BrandCode + @"
                   AND T43COD = " + ProductCode + @"
                   AND T43DES = '" + ProductModelDesc + "'";

        if (ProductModelCode != "")
        {
            sqlWhere = " AND T43MDL  = " + ProductModelCode;
        }

        SqlAll = SqlAll + sqlWhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found this Product Model is Duplicate ";
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
    protected void btnConfirmAddEditOKClick(object sender, EventArgs e)
    {
        //iDB2Command cmd = new iDB2Command();
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        string NameButton = "";
        dataCenter = new DataCenter(m_userInfo);
        /*** Comment for Test ***/
        string Curtime = DateTime.Now.ToString("yyyyMMdd");
        //string Curtime = "";
        //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
        //ilObj.CloseConnectioDAL();
        /************************/

        LoanType = lblLoanType.Text;
        ProductType = txtProductType.Text.Trim();
        ProductTypeDes = txtProductTypeDesc.Text.ToUpper().Trim();
        ProductCode = txtProductCode.Text.Trim();
        ProductDesc = txtProductDesc.Text.ToUpper().Trim();
        BrandCode = txtBrandCode.Text.Trim();
        BrandName = txtBrandName.Text.ToUpper().Trim();
        ProductModelCode = txtProductModelCode.Text.Trim();
        ProductModelDesc = txtProductModelDesc.Text.ToUpper().Trim();

        try
        {
            if (ProductModelCode == "") // insert
            {
                SqlAll = @"SELECT
                               T43MDL
                           FROM
                               AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                           WHERE
                               T43DEL = 'X'
                           AND T43LTY = '" + LoanType + @"'
                           AND T43TYP = " + ProductType + @"
                           AND T43BRD = " + BrandCode + @"
                           AND T43COD = " + ProductCode + @"
                           AND T43DES = '" + ProductModelDesc + "'";

                DataSet DS1 = new DataSet();
                
                ilObj.UserInfomation = m_userInfo;
                DS1 = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (cookiesStorage.check_dataset(DS1))
                {
                    DataRow dr = DS1.Tables[0]?.Rows[0];
                    ProductModelCode = dr[0].ToString().Trim();
                    NameButton = "Edit";
                    MsgHText = NameButton;

                    SqlAll = @"UPDATE
                                   AS400DB01.ILOD0001.ILTB43
                               SET
                                   T43DEL = '',
                                   T43UDD = " + Curtime + @",
                                   T43UDT = " + m_UpdTime.ToString() + @",
                                   T43USR = '" + m_userInfo.Username.ToString() + @"',
                                   T43PGM = '" + ProgramName + @"',
                                   T43DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                               WHERE
                                   T43LTY = '" + LoanType + @"'
                               AND T43TYP = " + ProductType + @"
                               AND T43BRD = " + BrandCode + @"
                               AND T43COD = " + ProductCode + @"
                               AND T43MDL = " + ProductModelCode;

                    //cmd.CommandText = SqlAll;
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resHome13 = dataCenter.Execute(SqlAll, CommandType.Text, transaction).Result.afrows;
                    if (resHome13 == -1)
                    {
                        dataCenter.RollbackMssql();
                        Msg = NameButton + " Product Model  not complete.";
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

                    SqlAll = @"SELECT
                                   MAX(T43MDL) as RUNNING
                               FROM
                                   AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
                               WHERE
                                   T43LTY = '" + LoanType + @"'
                               AND T43TYP = " + ProductType + @"
                               AND T43BRD = " + BrandCode + @"
                               AND T43COD = " + ProductCode;
                    DataTable dt = new DataTable();
                    DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                    DataRow dr = dt.NewRow();
                    string yearNew = DateTime.Now.ToString("yy");
                    if (cookiesStorage.check_dataset(DS))
                    {
                        dr = DS.Tables[0]?.Rows[0];
                        if (dr[0].ToString().Trim() != "")
                        {
                            string yearOld = dr[0].ToString().Trim().Substring(0, 2);
                            string runningMax = dr[0].ToString().Trim().Substring(2, 3);
                            string sumRunning = (Convert.ToInt32(runningMax) + 1).ToString();
                            string lengthRunning = sumRunning.Length.ToString();
                            if (yearNew == yearOld)
                            {
                                switch (lengthRunning)
                                {
                                    case "1":
                                        ProductModelCode = "00" + sumRunning;
                                        break;
                                    case "2":
                                        ProductModelCode = "0" + sumRunning;
                                        break;
                                    case "3":
                                        ProductModelCode = sumRunning;
                                        break;
                                    default: break;
                                }
                                ProductModelCode = yearNew + ProductModelCode;
                            }
                            else
                            {
                                ProductModelCode = Convert.ToString(yearNew + "001");
                            }
                        }
                        else
                        {
                            ProductModelCode = Convert.ToString(yearNew + "001");
                        }
                    }
                    else
                    {
                        ProductModelCode = Convert.ToString(yearNew + "001");
                    }
                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB43(
                                                T43LTY,
                                                T43TYP,
                                                T43BRD,
                                                T43COD,
                                                T43MDL,
                                                T43DES,
                                                T43UDD,
                                                T43UDT,
                                                T43USR,
                                                T43PGM,
                                                T43DSP)
                                    VALUES(     
                                                '{LoanType}',
                                                {ProductType},
                                                {BrandCode},
                                                {ProductCode},
                                                {ProductModelCode},
                                                '{ProductModelDesc}',
                                                {Curtime},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                '{ProgramName}',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}'
                                    )";



                    //cmd.CommandText = SqlAll;
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int resHome11 = dataCenter.Execute(SqlAll, CommandType.Text, transaction).Result.afrows;
                    if (resHome11 == -1)
                    {
                        dataCenter.RollbackMssql();
                        Msg = NameButton + " Product Model  not complete.";
                        dataCenter.CloseConnectSQL();
                        SetMsg();
                        return;
                    }

                    dataCenter.CommitMssql();

                    #region ********** INSERT ILTB48 **********
                    /*ทำไมเมื่อ Add New มีการ Insert ลงทั้ง Table ILTB43 และ Table ILTB48 ด้วย
                      แต่เมื่อ Update มีการ Update เฉพาะ Table ILTB43 ไม่มีการ Update Table ILTB48
                      จากที่แกะ Delphi มี Code เฉพาะ Insert เท่านั้น*/

                    SqlAll = @"SELECT
                                   T48LTY,
                                       T48TYP,
                                       T48COD,
                                       T48BRD,
                                       T48RSV,
                                       T48UDD,
                                       T48UDT,
                                       T48USR,
                                       T48PGM,
                                       T48DSP
                               FROM
                                   AS400DB01.ILOD0001.ILTB48 WITH (NOLOCK)
                               WHERE
                                   T48LTY = '" + LoanType + @"'
                               AND T48TYP = " + ProductType + @"
                               AND T48COD = " + ProductCode + @"
                               AND T48BRD = " + BrandCode;
                    
                    ilObj.UserInfomation = m_userInfo;
                    DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    if (!cookiesStorage.check_dataset(DS))
                    {
                        SqlAll = @"INSERT INTO AS400DB01.ILOD0001.ILTB48
                                   (
                                       T48LTY,
                                       T48TYP,
                                       T48COD,
                                       T48BRD,
                                       T48RSV,
                                       T48UDD,
                                       T48UDT,
                                       T48USR,
                                       T48PGM,
                                       T48DSP
                                   )
                                   VALUES
                                   (
                                       '" + LoanType + @"',
                                       " + ProductType + @",
                                       " + ProductCode + @",
                                       " + BrandCode + @",
                                       " + ProductCode + @",
                                       " + Curtime + @",
                                       " + m_UpdTime.ToString() + @",
                                       '" + m_userInfo.Username.ToString() + @"',
                                       '" + ProgramName + @"',
                                       '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                                   )";

                        SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB48(
                                                T48LTY,
                                                T48TYP,
                                                T48COD,
                                                T48BRD,
                                                T48RSV,
                                                T48UDD,
                                                T48UDT,
                                                T48USR,
                                                T48PGM,
                                                T48DSP)
                                    VALUES(     
                                                '{LoanType}',
                                                {ProductType},
                                                {ProductCode},
                                                {BrandCode},
                                                '{ProductCode}',
                                                {Curtime},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                '{ProgramName}',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}'
                                    )";


                        //cmd.Parameters.Clear();
                        //cmd.Parameters.Add("T48LTY", LoanType);
                        //cmd.Parameters.Add("T48TYP", ProductType);
                        //cmd.Parameters.Add("T48COD", ProductCode);
                        //cmd.Parameters.Add("T48BRD", BrandCode);
                        //cmd.Parameters.Add("T48RSV", ProductCode);
                        //cmd.Parameters.Add("T48UDD", Curtime);
                        //cmd.Parameters.Add("T48UDT", m_UpdTime.ToString());
                        //cmd.Parameters.Add("T48USR", m_userInfo.Username.ToString());
                        //cmd.Parameters.Add("T48PGM", ProgramName);
                        //cmd.Parameters.Add("T48DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                        //cmd.CommandText = SqlAll;
                        transaction = dataCenter.Sqltr == null ? true : false;
                        int resHome12 = dataCenter.Execute(SqlAll, CommandType.Text, transaction).Result.afrows;
                        if (resHome12 == -1)
                        {
                            dataCenter.RollbackMssql();
                            Msg = NameButton + " Product Model  not complete.";
                            SetMsg();

                            return;
                        }
                    }
                    dataCenter.CommitMssql();
                    #endregion
                }
            }
            else // update
            {
                NameButton = "Edit";
                MsgHText = NameButton;
                //SqlAll = @"UPDATE
                //               ILTB43
                //           SET
                //               T43DEL = '',
                //               T43DES = '" + ProductModelDesc + @"',
                //               T43UDD = " + Curtime + @",
                //               T43UDT = " + m_UpdTime.ToString() + @",
                //               T43USR = '" + m_userInfo.Username.ToString() + @"',
                //               T43PGM = '" + ProgramName + @"',
                //               T43DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                //           WHERE
                //               T43LTY = '" + LoanType + @"'
                //           AND T43TYP = " + ProductType + @"
                //           AND T43BRD = " + BrandCode + @"
                //           AND T43COD = " + ProductCode + @"
                //           AND T43MDL = " + ProductModelCode;

                SqlAll = $@"Update AS400DB01.ILOD0001.ILTB43 
                                SET T43DEL = '',
                                    T43DES = '{ProductModelDesc}',                   
                                    T43UDD = {Curtime},
                                    T43UDT = {m_UpdTime.ToString()},
                                    T43USR = '{m_userInfo.Username.ToString()}',
                                    T43PGM = '{ProgramName}',
                                    T43DSP = '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}'
                                WHERE T43LTY = '" + LoanType + @"'
                                    AND T43TYP = " + ProductType + @"
                                    AND T43BRD = " + BrandCode + @"
                                    AND T43COD = " + ProductCode + @"
                                    AND T43MDL = " + ProductModelCode;

                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("T43DEL", string.Empty);
                //cmd.Parameters.AddWithValue("T43DES", ProductModelDesc);
                //cmd.Parameters.AddWithValue("T43UDD", Curtime);
                //cmd.Parameters.AddWithValue("T43UDT", m_UpdTime.ToString());
                //cmd.Parameters.AddWithValue("T43USR", m_userInfo.Username.ToString());
                //cmd.Parameters.AddWithValue("T43PGM", ProgramName);
                //cmd.Parameters.AddWithValue("T43DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());

                //cmd.CommandText = SqlAll;
                bool transaction = dataCenter.Sqltr == null ? true : false;
                int resHome12 = dataCenter.Execute(SqlAll, CommandType.Text, transaction).Result.afrows;
                if (resHome12 == -1)
                {
                    dataCenter.RollbackMssql();
                    Msg = NameButton + " Product Model : " + ProductModelCode + " - " + ProductModelDesc + " not complete.";
                    SetMsg();

                    return;
                }
                else
                {
                    dataCenter.CommitMssql();
                }
            }

            Msg = NameButton + " Product Model : " + ProductModelCode + " - " + ProductModelDesc + " complete";
            SetMsgSuccess();
            SearchData();
            SetClearAddEdit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = NameButton + " Product Model : " + ProductModelCode + " - " + ProductModelDesc + " not complete";
            SetMsg();
        }
        finally
        {
            //cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
        }
    }

    protected void btnConfirmAddEditCancelClick(object sender, EventArgs e)
    {
        btnSelectProductType.Focus();
    }
    #endregion

    #region Delete Data
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
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
        Globals.productModelIdShow = dr[8].ToString().Trim();
        Globals.productModelDesShow = dr[9].ToString().Trim();
        if (ValidateDelete())
        {
            m_userInfo = userInfoService.GetUserInfo();
            /*** Comment for Test ***/
            string Curtime = DateTime.Now.ToString("yyyyMMdd");
            //string Curtime = "";
            //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
            //ilObj.CloseConnectioDAL();
            /************************/
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = @"UPDATE
                           AS400DB01.ILOD0001.ILTB43
                       SET
                           T43DEL = 'X',
                           T43UDD = " + Curtime + @", 
                           T43UDT = " + m_UpdTime.ToString() + @", 
                           T43USR = '" + m_userInfo.Username.ToString() + @"',
                           T43DSP = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + @"'
                       WHERE
                           T43LTY = '01'
                       AND T43TYP = " + ProductType + @"
                       AND T43BRD = " + BrandCode + @"
                       AND T43COD = " + ProductCode + @"
                       AND T43MDL = " + ProductModelCode + @"
                       AND T43DEL = ''";

            txtProductModel_D.Text = ProductModelCode;
            txtSqlAll.Text = SqlAll;
            lblConfimMsg_Delete.Text = "Confirm Delete Data ";
            PopupConfirmDelete.ShowOnPageLoad = true;
        }
    }

    private bool ValidateDelete()
    {
        try
        {


            SqlAll = @"SELECT
                       t44.T44COD,
                       t44.T44DES
                   FROM
                       AS400DB01.ILOD0001.ILTB44 t44 WITH (NOLOCK)
                        left join AS400DB01.ILOD0001.ILTB43 t43  WITH (NOLOCK) on 
					   t44.T44LTY = t43.T43LTY and t44.T44MDL = t43.T43MDL
					   and t44.T44BRD = t43.T43BRD and t44.T44TYP = t43.T43TYP
                   WHERE
                       t44.T44LTY = '01'
                   AND t43.T43TYP = " + ProductType + @"
                   AND t43.T43BRD = " + BrandCode + @" 
                   AND t43.T43COD = " + ProductCode + @"
                   AND t43.T43MDL = " + ProductModelCode + @"
                   AND t44.T44DEL = ''";

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();

            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (cookiesStorage.check_dataset(DS))
            {
                Msg = "Cannot delete, Found this Product-Code is uses by Product Item";
                MsgHText = "Validate Delete";
                SetMsg();

                dataCenter.CloseConnectSQL();
                return false;
            }
            else
            {
                if (DS?.Tables[0].Rows.Count == 0)
                {
                    dataCenter.CloseConnectSQL();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Msg = " Delete not complete because "+ ex.Message +"";
            SetMsg();
            return false ;
        }
    }
    #endregion

    #region Confirm Delete
    protected void btnDeleteConfirmOKClick(object sender, EventArgs e)
    {
        //iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        //ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        SqlAll = txtSqlAll.Text;
        //cmd.CommandText = SqlAll;
        ProductDesc = txtProductModel_D.Text;
        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int resHome11 = dataCenter.Execute(SqlAll, CommandType.Text, transaction).Result.afrows;
            if (resHome11 == -1)
            {
                dataCenter.RollbackMssql();
                Msg = " Delete Product Model : " + Globals.productModelIdShow + " - " + Globals.productModelDesShow + " not complete.";
                SetMsg();
            }
            else
            {

                dataCenter.CommitMssql();
                Msg = " Delete Product Model : " + Globals.productModelIdShow + " - " + Globals.productModelDesShow + " complete";
                SetMsgSuccess();
                GridView1.Focus();
                SearchData();
                SetClearAddEdit();
            }
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            Msg = " Delete Product Model : " + Globals.productModelIdShow + " - " + Globals.productModelDesShow + " not complete.";
            SetMsg();
        }
        finally
        {
            dataCenter.CloseConnectSQL();
            //cmd.Parameters.Clear();
        }
    }

    protected void btnDeleteConfirmCancelClick(object sender, EventArgs e)
    {
        GridView1.Focus();
    }
    #endregion

    #region Message all
    protected void btnPopupMessageOKClick(object sender, EventArgs e)
    {
        btnSelectProductType.Focus();
    }
    #endregion

    #region Popup ProductType
    private void LoadDataPopupProductType()
    {
        string sqlWhere = "";

        SqlAll = @"SELECT
                        T40LTY,
                        T00TNM,
                        T40TYP,
                        T40DES
                    FROM
                        AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK)
                    LEFT JOIN AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK)
                    ON  T40LTY=T00LTY
                    WHERE
                        T40DEL = '' ";

        string searchBy = ddlPopupAddProductTypeSearchBy.SelectedValue;
        string searchText = txtPopupAddProductTypeSearchText.Text.Trim();

        if (searchText != "")
        {
            switch (searchBy)
            {
                case "PT":
                    sqlWhere = " AND T40TYP = " + searchText;
                    break;
                case "PD":
                    sqlWhere = " AND T40DES LIKE'" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        SqlAll = SqlAll + sqlWhere + " ORDER BY T40TYP ASC";

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
            dataCenter.CloseConnectSQL();
            return;
        }

        dataCenter.CloseConnectSQL();
    }

    protected void btnSelectProductTypeClick(object sender, EventArgs e)
    {
        hdfAdvanceSearch.Value = "N";
        ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
        txtPopupAddProductTypeSearchText.Text = "";
        LoadDataPopupProductType();
        PopupAddProductType.ShowOnPageLoad = true;
        pProType.Text = "f";
        PopupAddProductType.Focus();
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_type.Value);
        GridView2.DataBind();
    }

    protected void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

        try
        {
            DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_type.Value);
            DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];
            //Console.WriteLine(dr[2]);
            if (pProType.Text == "f")
            {
                //txtPopupAddProductTypeSearchText.Text.Trim() != dr[2].ToString().Trim()
                if (!String.IsNullOrEmpty(dr[2].ToString().Trim()))
                {
                    lblLoanType.Text = dr[0].ToString().Trim();
                    lblLoanTypeDesc.Text = dr[1].ToString().Trim();
                    txtProductType.Text = dr[2].ToString().Trim();
                    txtProductTypeDesc.Text = dr[3].ToString().Trim();
                    txtProductCode.Text = "";
                    txtProductDesc.Text = "";
                    PopupAddProductType.ShowOnPageLoad = false;
                    btnSelectProductType.Focus();
                }

            }
            else
            {
                txtAdvanceSearchProductTypeCode.Text = dr[2].ToString().Trim();
                txtAdvanceSearchProductTypeDesc.Text = dr[3].ToString().Trim();
                PopupAddProductType.ShowOnPageLoad = false;
                PopupAdvanceSearch.ShowOnPageLoad = true;
                btnAdvanceSearchProductType.Focus();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        ResetGrid(GridView2, ds_popup_type.Value);
    }

    protected void btnPopupProductTypeSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupProductType();
    }

    protected void btnPopupProductTypeClearClick(object sender, EventArgs e)
    {
        ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
        txtPopupAddProductTypeSearchText.Text = "";
        LoadDataPopupProductType();
    }
    #endregion    

    #region Popup ProductCode
    private void LoadDataPopupProductCode()
    {
        string sqlWhere = "";
        string searchBy = ddlPopupAddProductCodeSearchBy.SelectedValue;
        string searchText = txtPopupAddProductCodeSearchText.Text.Trim();

        if (searchText != "")
        {
            switch (searchBy)
            {
                case "PT":
                    sqlWhere = " AND T41TYP = " + searchText;
                    break;
                case "PC":
                    sqlWhere += " AND T41COD = " + searchText;
                    break;
                case "PD":
                    sqlWhere += " AND T41DES LIKE '" + searchText.ToUpper() + "%'";
                    break;
                default: break;
            }
        }

        if (pProCode.Text == "p" && !string.IsNullOrEmpty(txtAdvanceSearchProductTypeCode.Text.Trim()))
        {
            sqlWhere += " AND TB41.T41TYP = " + txtAdvanceSearchProductTypeCode.Text.Trim();
        }

        if (pProCode.Text == "f")
        {
            sqlWhere += " AND TB41.T41TYP = " + txtProductType.Text.Trim();
        }

        SqlAll = @"SELECT
                       TB41.T41LTY,
                       TB00.T00TNM,
                       TB41.T41TYP,
                       TB40.T40DES,
                       TB41.T41COD,
                       TB41.T41DES,
                       TB40.T40LTY   
                   FROM
                       AS400DB01.ILOD0001.ILTB41 AS TB41 WITH (NOLOCK) 
                   LEFT JOIN AS400DB01.ILOD0001.ILTB00 AS TB00 WITH (NOLOCK) 
                   ON  TB41.T41LTY = TB00.T00LTY  
                   LEFT JOIN AS400DB01.ILOD0001.ILTB40 AS TB40 WITH (NOLOCK) 
                   ON  TB40.T40TYP = TB41.T41TYP
                   WHERE
                       TB41.T41DEL = '' ";
            
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
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
    }

    protected void btnSelectProductCodeClick(object sender, EventArgs e)
    {
        if (validateProductCodeForm())
        {
            hdfAdvanceSearch.Value = "N";
            ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
            txtPopupAddProductCodeSearchText.Text = "";
            PopupAddProductCode.ShowOnPageLoad = true;
            pProCode.Text = "f";
            PopupAddProductCode.Focus();
            LoadDataPopupProductCode();
        }
    }

    private bool validateProductCodeForm()
    {
        if (txtProductType.Text.Trim() == "")
        {
            MsgHText = "Validate";
            Msg = " Please Select Product Type";
            SetMsg();
            btnSelectProductType.Focus();
            return false;
        }
        return true;
    }

    protected void btnPopupProductCodeSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupProductCode();
    }

    protected void btnPopupProductCodeClearClick(object sender, EventArgs e)
    {
        ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
        txtPopupAddProductCodeSearchText.Text = "";
        LoadDataPopupProductCode();
    }

    protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView4.PageIndex = e.NewPageIndex;
        GridView4.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_code.Value);
        GridView4.DataBind();
    }

    protected void GridView4_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_code.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView4.PageIndex * Convert.ToInt16(GridView4.PageSize)) + e.NewSelectedIndex];
        if (pProType.Text == "f")
        {
            txtProductCode.Text = dr[4].ToString().Trim();
            txtProductDesc.Text = dr[5].ToString().Trim();
            PopupAddProductCode.ShowOnPageLoad = false;
            btnSelectProductCode.Focus();
        }
        else
        {
            txtAdvanceSearchProductCode.Text = dr[4].ToString().Trim();
            txtAdvanceSearchProductDesc.Text = dr[5].ToString().Trim();
            PopupAddProductCode.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnAdvanceSearchProductCode.Focus();
        }
        ResetGrid(GridView4, ds_popup_code.Value);
    }
    #endregion

    #region Popup  Brand
    protected void btnSelectBrandClick(object sender, EventArgs e)
    {
        hdfAdvanceSearch.Value = "N";
        ddlPopupAddBrandSearchBy.SelectedIndex = 0;
        txtPopupAddBrandSearchText.Text = "";
        LoadDataPopupBrand();
        PopupAddBrand.ShowOnPageLoad = true;
        pProBrand.Text = "f";
        PopupAddBrand.Focus();
    }

    private void LoadDataPopupBrand()
    {
        string searchBy = ddlPopupAddBrandSearchBy.SelectedValue;
        string searchText = txtPopupAddBrandSearchText.Text.Trim();
                
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();

        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetBrand(searchBy, searchText, 1, 15);
        
        if (cookiesStorage.check_dataset(DS))
        {
            ds_popup_brand.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView3.DataSource = DS;
            GridView3.DataBind();
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
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
    }

    protected void btnPopupBrandSearchClick(object sender, EventArgs e)
    {
        LoadDataPopupBrand();
    }

    protected void btnPopupBrandClearClick(object sender, EventArgs e)
    {
        ddlPopupAddBrandSearchBy.SelectedIndex = 0;
        txtPopupAddBrandSearchText.Text = "";
        LoadDataPopupBrand();
    }

    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView3.PageIndex = e.NewPageIndex;
        GridView3.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_brand.Value);
        GridView3.DataBind();
    }

    protected void GridView3_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_popup_brand.Value);
        DataRow dr = ds_grid.Tables[0]?.Rows[(GridView3.PageIndex * Convert.ToInt16(GridView3.PageSize)) + e.NewSelectedIndex];

        if (pProType.Text == "f")
        {
            txtBrandCode.Text = dr[0].ToString().Trim();
            txtBrandName.Text = dr[1].ToString().Trim();
            PopupAddBrand.ShowOnPageLoad = false;
            btnSelectBrand.Focus();
        }
        else
        {
            txtAdvanceSearchBrandCode.Text = dr[0].ToString().Trim();
            txtAdvanceSearchBrandName.Text = dr[1].ToString().Trim();
            PopupAddBrand.ShowOnPageLoad = false;
            PopupAdvanceSearch.ShowOnPageLoad = true;
            btnAdvanceSearchBrand.Focus();
        }
        ResetGrid(GridView3, ds_popup_brand.Value);
    }
    #endregion

    #region Advance Seach
    protected void btnAdvanceSearchClick(object sender, EventArgs e)
    {
        PopupAdvanceSearch.ShowOnPageLoad = true;
        pProType.Text = "p";
        pProBrand.Text = "p";
        pProCode.Text = "p";
        hdfAdvanceSearch.Value = "Y";

        txtAdvanceSearchProductTypeCode.Text = "";
        txtAdvanceSearchProductTypeDesc.Text = "";

        txtAdvanceSearchProductCode.Text = "";
        txtAdvanceSearchProductDesc.Text = "";

        txtAdvanceSearchBrandCode.Text = "";
        txtAdvanceSearchBrandName.Text = "";

        txtAdvanceSearchModelCode.Text = "";
        txtAdvanceSearchModelDesc.Text = "";

        PopupAdvanceSearch.Focus();
    }

    protected void btnAdvanceSearchProductTypeClick(object sender, EventArgs e)
    {
        hdfAdvanceSearch.Value = "Y";
        ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
        txtPopupAddProductTypeSearchText.Text = "";
        pProBrand.Text = "p";
        LoadDataPopupProductType();
        PopupAddProductType.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddProductType.Focus();
    }

    protected void btnAdvanceSearchProductCodeClick(object sender, EventArgs e)
    {
        hdfAdvanceSearch.Value = "Y";
        ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
        txtPopupAddProductCodeSearchText.Text = "";
        pProCode.Text = "p";
        LoadDataPopupProductCode();
        PopupAddProductCode.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddProductCode.Focus();
    }

    protected void btnAdvanceSearchBrandClick(object sender, EventArgs e)
    {
        hdfAdvanceSearch.Value = "Y";
        ddlPopupAddBrandSearchBy.SelectedIndex = 0;
        txtPopupAddBrandSearchText.Text = "";
        pProBrand.Text = "p";
        LoadDataPopupBrand();
        PopupAddBrand.ShowOnPageLoad = true;
        PopupAdvanceSearch.ShowOnPageLoad = false;
        PopupAddBrand.Focus();
    }

    protected void btnPopupAdvanceSearchClearClick(object sender, EventArgs e)
    {
        txtAdvanceSearchProductTypeCode.Text = "";
        txtAdvanceSearchProductTypeDesc.Text = "";

        txtAdvanceSearchProductCode.Text = "";
        txtAdvanceSearchProductDesc.Text = "";

        txtAdvanceSearchBrandCode.Text = "";
        txtAdvanceSearchBrandName.Text = "";

        txtAdvanceSearchModelCode.Text = "";
        txtAdvanceSearchModelDesc.Text = "";

        btnPopupAdvanceSearch.Focus();
    }

    protected void btnPopupAdvanceSearchClick(object sender, EventArgs e)
    {
        pProType.Text = "f";
        pProBrand.Text = "f";
        pProCode.Text = "f";
        ddlPopupAddProductTypeSearchBy.SelectedIndex = 0;
        txtPopupAddProductTypeSearchText.Text = "";
        ddlPopupAddBrandSearchBy.SelectedIndex = 0;
        txtPopupAddBrandSearchText.Text = "";
        ddlPopupAddProductCodeSearchBy.SelectedIndex = 0;
        txtPopupAddProductCodeSearchText.Text = "";
        LoadDataPopupAdvanceSearch();
    }

    private void LoadDataPopupAdvanceSearch()
    {
        PopupAdvanceSearch.ShowOnPageLoad = false;
        string sqlWhere = "";

        SqlAll = @"SELECT
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
                   FROM
                       AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK)
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

        if (txtAdvanceSearchProductTypeCode.Text.Trim() != "")
        {
            sqlWhere += " AND T43TYP = " + txtAdvanceSearchProductTypeCode.Text.Trim();
        }
        if (txtAdvanceSearchProductCode.Text.Trim() != "")
        {
            sqlWhere += " AND T43COD = " + txtAdvanceSearchProductCode.Text.Trim();
        }
        if (txtAdvanceSearchBrandCode.Text.Trim() != "")
        {
            sqlWhere += " AND T43BRD = " + txtAdvanceSearchBrandCode.Text.Trim();
        }
        if (txtAdvanceSearchModelCode.Text.Trim() != "")
        {
            sqlWhere += " AND UPPER(T43MDL) LIKE '%" + txtAdvanceSearchModelCode.Text.Trim().ToUpper() + "%'";
        }
        if (txtAdvanceSearchModelDesc.Text.Trim() != "")
        {
            sqlWhere += " AND UPPER(T43DES) LIKE '%" + txtAdvanceSearchModelDesc.Text.ToUpper().Trim() + "%'";
        }

        SqlAll = SqlAll + sqlWhere;

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
            GridView1.DataSource = DS;
            GridView1.DataBind();
            dataCenter.CloseConnectSQL();
            return;
        }

        dataCenter.CloseConnectSQL();

        txtAdvanceSearchProductTypeCode.Text = "";
        txtAdvanceSearchProductTypeDesc.Text = "";

        txtAdvanceSearchProductCode.Text = "";
        txtAdvanceSearchProductDesc.Text = "";

        txtAdvanceSearchBrandCode.Text = "";
        txtAdvanceSearchBrandName.Text = "";

        txtAdvanceSearchModelCode.Text = "";
        txtAdvanceSearchModelDesc.Text = "";
    }
    #endregion

}