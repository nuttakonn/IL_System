using EB_Service.DAL;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageData_WorkProcess_Brand : System.Web.UI.Page
{
    public DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
    public ILDataCenterOnMasterFile iLDataCenterOnMasterFile;
    protected string SqlAll = "";
    protected string BrandCode = "";
    protected string BrandName = "";
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
        public static String codeIdShow;
        public static String brandDesShow;
    }

    #region Bind Data and search
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Search_Data();
    }
    void Search_Data()
    {        
        string searchBy = ddl_SearchBy.SelectedValue;
        string nameBrand = txt_Brand.Text.Trim().Replace("'", "''");

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        iLDataCenterOnMasterFile = new ILDataCenterOnMasterFile(m_userInfo);
        DS = iLDataCenterOnMasterFile.Sp_GetBrand(searchBy, nameBrand, 1, 15);

        if (cookiesStorage.check_dataset(DS))
        {
            ds_grid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            GridView1.SelectedIndex = -1;
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
            dt.Columns.Add("T42BRD");
            dt.Columns.Add("T42DES");
            DS.Tables.Add(dt);
            GridView1.DataSource = DS;
            GridView1.DataBind();
            dataCenter.CloseConnectSQL();
            return;
        }
        ResetGrid(GridView1, "ds_grid");

    }



    #endregion

    #region Default 
    private void set_Enabled(bool type)
    {
        btnAdd.Enabled = type;
        btnClearData.Enabled = type;
        btn_search.Enabled = type;
        //btn_clear.Enabled = type;

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
        txt_Brand.Text = "";
    }
    private void set_default_edit()
    {
        lblBranCode.Text = BrandCode;
        txtBrandName.Text = BrandName;
        txtBrandName.Focus();
        btnAdd.Text = "Edit";
        lbl_AddEdit.Text = "Edit Brand";
    }
    private void set_clear_add_edit()
    {
        lblBranCode.Text = "";
        txtBrandName.Text = "";
        btnAdd.Text = "Add";
        lbl_AddEdit.Text = "Add Brand";
        txtBrandName.Focus();
    }

    #endregion

    #region select page
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.SelectedIndex = -1;
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_grid.Value);
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
        DataSet ds_grids = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_grid.Value);
        if (cookiesStorage.check_dataset(ds_grids))
        {
            DataRow dr = ds_grids.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.NewSelectedIndex];
            BrandCode = dr[0].ToString().Trim();
            BrandName = dr[1].ToString().Trim();
            set_default_edit();
            ResetGrid(GridView1, "ds_grid");
        }
        
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_grid.Value);
        GridView.DataBind();
    }

    #endregion

    #region delete data
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet ds_grids = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_grid.Value);
        DataRow dr = ds_grids.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.RowIndex];
        BrandCode = dr[0].ToString().Trim();
        BrandName = dr[1].ToString().Trim();
        Globals.codeIdShow = dr[0].ToString().Trim();
        Globals.brandDesShow = dr[1].ToString().Trim();
        if (validateDelete())
        {
            m_userInfo = userInfoService.GetUserInfo();
            //string Curtime = "";
            //ilObj.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref Curtime);
            //ilObj.CloseConnectioDAL();
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = " Update AS400DB01.ILOD0001.ILTB42 SET T42DEL = 'X' "
                  + ", T42UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                  + ", T42UDT = " + m_UpdTime.ToString()
                  + ", T42USR = '" + m_userInfo.Username.ToString() + "'"
                  + ",  T42DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                  + "  WHERE   T42BRD = " + BrandCode;
            txtSqlAll.Text = SqlAll;
            txtBrand_D.Text = dr[1].ToString().Trim();
            lblConfimMsg_Delete.Text = "Confirm Delete Brand Name : " + BrandName;
            PopupConfirmDelete.ShowOnPageLoad = true;
        }

        GridView1.Focus();
    }

    private bool validateDelete()
    {
        SqlAll = " SELECT T44BRD FROM AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) WHERE CAST(T44BRD as nvarchar) = '" + BrandCode + "' AND  T44DEL= ''  AND T44LTY= '01'";
        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

        if (cookiesStorage.check_dataset(DS))
        {
            Msg = "Found this Brand is uses by item ";
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

        BrandName = txtBrandName.Text.ToUpper().Trim();
        BrandCode = lblBranCode.Text.Trim();

        var regex = @"^[a-zA-Z0-9 !-_]*$";
        var match = Regex.Match(BrandName, regex, RegexOptions.IgnoreCase);

        if (BrandName == "")
        {
            Msg = "Please Input Brand Name  ";
            MsgHText = "Validate Save";
            set_Msg();

        }
        //if (!match.Success)
        //{
        //    // does not match
        //    Msg = "Please Input English and Number Only  ";
        //    MsgHText = "Validate Save";
        //    set_Msg();
        //}
        else if (BrandName != "")
        {
            if (BrandCode == "")
            {
                SqlAll = " SELECT T42DEL,T42BRD,T42DES  FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) WHERE T42DEL = '' and CAST(T42DES as nvarchar) = '" + BrandName + "'";
            }
            else
            {
                SqlAll = " SELECT T42DEL,T42BRD,T42DES  FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) WHERE T42DEL = '' and CAST(T42DES as nvarchar) = '" + BrandName + "'" + " and T42BRD  <> " + BrandCode;
            }
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

            if (cookiesStorage.check_dataset(DS))
            {
                Msg = "Found this Brand is Duplicate ";
                MsgHText = "Validate Save";
                set_Msg();

            }
            else
            {
                dataCenter.CloseConnectSQL();
                CHText = "Confirm Save";
                CMsg = "Confirm Save Brand Data";
                set_confirmMsg();
            }
        }
    }

    #endregion

    #region Confirm Add/Edit
    protected void btnConfirmOK_Click(object sender, EventArgs e)
    {
        m_userInfo = userInfoService.GetUserInfo();
        iDB2Command cmd = new iDB2Command();
        
        ilObj.UserInfomation = m_userInfo;
        ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_userInfo);
        string NameButton = "";
        dataCenter = new DataCenter(m_userInfo);
        BrandCode = lblBranCode.Text.Trim();
        BrandName = txtBrandName.Text.ToUpper().Trim();
        string brandCodePopupMsg = "";

        try
        {
            if (BrandCode == "") // insert
            {
                SqlAll = " SELECT T42DEL,T42BRD,T42DES  FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) WHERE T42DEL = 'X' and CAST(T42DES as nvarchar) = '" + BrandName + "'";
                DataSet DS = new DataSet();
                
                ilObj.UserInfomation = m_userInfo;
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                if (cookiesStorage.check_dataset(DS))
                {
                    DataRow dr = DS.Tables[0]?.Rows[0];
                    BrandCode = dr[1].ToString().Trim();
                    NameButton = "Edit";
                    MsgHText = NameButton;
                    brandCodePopupMsg = BrandCode;

                    SqlAll = " Update AS400DB01.ILOD0001.ILTB42 SET T42DEL = ''  "
                                            + ",  T42UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                                            + ",  T42UDT = " + m_UpdTime.ToString()
                                            + ",  T42USR = '" + m_userInfo.Username.ToString() + "'"
                                            + ",  T42PGM ='IL_BRAND'"
                                            + ",  T42DSP =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"
                                            + ",  T42DES = '" + BrandName + "'"
                                            + "   WHERE T42BRD = " + BrandCode;
                }
                else
                {
                    NameButton = "Add";

                    MsgHText = NameButton;

                    string WORUNN = "";
                    string WOLENG = "";
                    string WOERRF = "";
                    string WOEMSG = "";

                    //call procedures Call_ILSR92
                    dataSubroutine.Call_ILSR92("", "042", ref WORUNN, ref WOLENG, ref WOERRF, ref WOEMSG);
                    int brandYear = int.Parse(WORUNN.ToString().Trim().Substring(0, 2));
                    int brandYearNew = int.Parse(WORUNN);
                    int numYears = Convert.ToInt32(DateTime.Now.ToString("yy"));
                    brandCodePopupMsg = brandYearNew.ToString();

                    SqlAll = $@"INSERT INTO AS400DB01.ILOD0001.ILTB42(
                                                T42DEL,
                                                T42UDD,
                                                T42UDT,
                                                T42USR,
                                                T42PGM,
                                                T42DSP,
                                                T42DES,
                                                T42BRD)
                                    VALUES(     
                                                '{string.Empty}',
                                                {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},
                                                {m_UpdTime.ToString()},
                                                '{m_userInfo.Username.ToString()}',
                                                'IL_BRAND',
                                                '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                                '{BrandName}',
                                                {brandYearNew}
                                    )";


                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("T42DEL", string.Empty);
                    //cmd.Parameters.Add("T42UDD", Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd")));
                    //cmd.Parameters.Add("T42UDT", m_UpdTime.ToString());
                    //cmd.Parameters.Add("T42USR", m_userInfo.Username.ToString());
                    //cmd.Parameters.Add("T42PGM", "IL_BRAND");
                    //cmd.Parameters.Add("T42DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                    //cmd.Parameters.Add("T42DES", BrandName);
                    //cmd.Parameters.Add("T42BRD", brandYearNew);
                }
            }
            else // update
            {
                NameButton = "Edit";
                MsgHText = NameButton;
                brandCodePopupMsg = BrandCode;
                //SqlAll = " Update ILTB42 SET T42DEL = ''  "
                //                        + ",  T42UDD = " + Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))
                //                        + ",  T42UDT = " + m_UpdTime.ToString()
                //                        + ",  T42USR = '" + m_userInfo.Username.ToString() + "'"
                //                        + ",  T42PGM ='IL_BRAND'"
                //                        + ",  T42DSP =  '" m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() "'"
                //                        + ",  T42DES = '" + BrandName + "'"
                //                        + "   WHERE T42BRD = " + BrandCode;

                SqlAll = $@"Update AS400DB01.ILOD0001.ILTB42 
                                    SET T42DEL = '{string.Empty}',
                                        T42UDD = {Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"))},                   
                                        T42UDT = {m_UpdTime.ToString()},
                                        T42USR = '{m_userInfo.Username.ToString()}',
                                        T42PGM = 'IL_BRAND',
                                        T42DSP = '{m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim()}',
                                        T42DES = '{BrandName}'
                                    WHERE T42BRD = " + BrandCode;

                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("T42DEL", string.Empty);
                //cmd.Parameters.AddWithValue("T42UDD", Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd")));
                //cmd.Parameters.AddWithValue("T42UDT", m_UpdTime.ToString());
                //cmd.Parameters.AddWithValue("T42USR", m_userInfo.Username.ToString());
                //cmd.Parameters.AddWithValue("T42PGM", "IL_BRAND");
                //cmd.Parameters.AddWithValue("T42DSP", m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim());
                //cmd.Parameters.AddWithValue("T42DES", BrandName);
            }


            cmd.CommandText = SqlAll;

            bool transaction = dataCenter.Sqltr == null ? true : false;
            var resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (resHome11.afrows == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = NameButton + " Brand: " + brandCodePopupMsg + " - " + BrandName + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Brand: " + brandCodePopupMsg + " - " + BrandName + " complete.";
            set_MsgSuccess();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();

            Msg = NameButton + " Brand: " + brandCodePopupMsg + " - " + BrandName + " not complete.";
            set_Msg();
            return;
        }
    }

    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        txtBrandName.Focus();
    }
    #endregion

    #region Confirm Delete
    protected void btnConfirm_OK_Click(object sender, EventArgs e)
    {
        iDB2Command cmd = new iDB2Command();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        SqlAll = txtSqlAll.Text;
        cmd.CommandText = SqlAll;
        BrandName = txtBrand_D.Text;
        dataCenter = new DataCenter(m_userInfo);
        try
        {
            bool transaction = dataCenter.Sqltr == null ? true : false;
            var resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (resHome11.afrows == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = "Delete Brand Name : " + Globals.codeIdShow + " - " + Globals.brandDesShow + " not complete.";
                set_Msg();
                return;
            }
            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            Msg = " Delete Brand Name : " + Globals.codeIdShow + " - " + Globals.brandDesShow + " complete.";
            set_MsgSuccess();
            GridView1.Focus();
            Search_Data();
            set_clear_add_edit();
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            Msg = "Delete Brand Name : " + Globals.codeIdShow + " - " + Globals.brandDesShow + " not complete.";
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
        txtBrandName.Focus();
    }
    #endregion

    private void PopupMsgCenter()
    {

        PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirm.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirm.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
        PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

    }
}