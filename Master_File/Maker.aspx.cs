using DevExpress.Web.ASPxEditors;
using EB_Service.DAL;
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

public partial class ManageData_WorkProcess_Maker : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
    protected ILDataCenter ilObj = new ILDataCenter();
    protected DataCenter dataCenter;
    public UserInfo m_userInfo;
    ILDataCenterMssql CallMasterEnt;
    ILDataSubroutine iLDataSubroutine;
    public UserInfoService userInfoService;
    public CookiesStorage cookiesStorage;
    //------------------------All
    protected string _txt_MakerCode = "";
    protected string _dd_off_title = "";
    protected string _txt_ThaiName = "";
    protected string _txt_EngName = "";
    //-------------------------Regis Address
    protected string _txt_reAddress = "";
    protected string _txt_reMoo = "";
    protected string _txt_reVilage = "";
    protected string _dd_BuildingType1 = "";
    protected string _txt_reBuilding = "";
    protected string _txt_reRoom = "";
    protected string _txt_reFloor = "";
    protected string _txt_reSoi = "";
    protected string _txt_reRoad = "";
    protected string _C_address_I = "";
    protected string _D_tambol1 = "";
    protected string _D_amphur1 = "";
    protected string _D_province1 = "";
    protected string _D_zipcode1 = "";

    //-------------------------Local Address

    protected string _txt_loAddress = "";
    protected string _txt_loMoo = "";
    protected string _txt_loVilage = "";
    protected string _dd_BuildingType2 = "";
    protected string _txt_loBuilding = "";
    protected string _txt_loRoom = "";
    protected string _txt_loFloor = "";
    protected string _txt_loSoi = "";
    protected string _txt_loRoad = "";
    protected string _C_address_I_2 = "";
    protected string _D_tambol2 = "";
    protected string _D_amphur2 = "";
    protected string _D_province2 = "";
    protected string _D_zipcode2 = "";

    //-------------------------tab Other
    protected string _txt_RegisterNo_TaxID = "";
    protected string _txt_TaxID = "";
    protected string _txt_TelNo = "";
    protected string _txt_ContactTel = "";
    protected string _dd_FaxNo1Type = "";
    protected string _txt_FaxNo1 = "";
    protected string _dd_FaxNo2Type = "";
    protected string _txt_FaxNo2 = "";
    protected string _txt_Ref_Person1 = "";
    protected string _txt_Ref_Position1 = "";
    protected string _txt_Ref_Dept1 = "";
    protected string _txt_Ref_Person2 = "";
    protected string _txt_Ref_Position2 = "";
    protected string _txt_Ref_Dept2 = "";
    protected string _txt_IssueInvoiceDays = "";

    //-------------------------Whom to contact
    protected string _txt_Seq = "";
    protected string _txt_thaiName1 = "";
    protected string _txt_EngName1 = "";
    protected string _txt_Department = "";
    protected string _txt_MobilePhone = "";
    protected string _txt_Tel1 = "";
    protected string _txt_ContactTelRange1 = "";
    protected string _txt_Extension1 = "";
    protected string _t_FaxNo1 = "";
    protected string _txt_Tel2 = "";
    protected string _txt_ContactTelRange2 = "";
    protected string _txt_Extension2 = "";
    protected string _t_FaxNo2 = "";
    protected string _txt_Tel3 = "";
    protected string _txt_ContactTelRange3 = "";
    protected string _txt_Extension3 = "";
    protected string _txt_FaxNo3 = "";

    protected string SqlAll = "";
    protected string CMsg = "";
    protected string CHText = "";
    protected string Msg = "";
    protected string MsgHText = "";

    protected string View = "2";

    protected void Page_Load(object sender, EventArgs e)
    {
        View = Request.QueryString["View"];
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
            set_Enabled(View);
            bind_buildingType("", dd_BuildingType1);
            bind_buildingType("", dd_BuildingType2);
            bind_OfficeTitle("");
        }
        if (Page.IsPostBack)
        {
            return;
        }
        
    }


    #region Default
    private void bind_buildingType(string code, ASPxComboBox dd_BuildingType)
    {
        try
        {
            ilObj = new ILDataCenter();
            SqlAll = "SELECT Code as GT08TC, DescriptionTHAI as GT08TD, DescriptionENG as GT08ED  FROM  GeneralDB01.GeneralInfo.GeneralCenter as GNTB08 WITH (NOLOCK) WHERE Type = 'BuildingTitleID' AND IsDelete = '" + code + "'";

            DataSet ds = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            ds = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            dd_BuildingType.Items.Clear();

            if (cookiesStorage.check_dataset(ds))
            {
                dd_BuildingType.Items.Add("ไม่ระบุ", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_BuildingType.Items.Add(
                        new ListEditItem(dr["GT08TC"].ToString().Trim() + " : " + dr["GT08TD"].ToString().Trim(), dr["GT08TC"].ToString().Trim())
                                              );

                    if (code != "")
                    {
                        dd_BuildingType.Value = code;
                    }
                }
            }
            dataCenter.CloseConnectSQL();
        }

        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
    }

    private void bind_OfficeTitle(string code = "")
    {
        try
        {
            m_userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            CallMasterEnt = new ILDataCenterMssql(m_userInfo);
            dataCenter = new DataCenter(m_userInfo);
            DataSet ds = iLDataSubroutine.getGNMB20("2");
            dd_off_title.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_off_title.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_off_title.Items.Add(
                        new ListEditItem(dr["gnb2tc"].ToString().Trim() + " : " + dr["gnb2td"].ToString().Trim(), dr["gnb2tc"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_off_title.Value = code;
                }
            }
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
    }

    private void ResetGrid(GridView GridView, string ds)
    {
        GridView.PageIndex = 0;
        GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
        GridView.DataBind();
    }
    private void set_Enabled(string type)
    {
        if (type == "1") //view
        {
        }
        else if (type == "2") // new
        {
        }
        else if (type == "3") // edit
        {
        }

        //  txtAddress.Enabled = type;
    }
    #endregion

    #region Message
    protected void btnOK_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region pop up search maker
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Popup_AddMaker.ShowOnPageLoad = true;
        LoadMaker();
    }
    private void LoadMaker()
    {
        E_popup_error.Text = "";

        string sqlwhere = "";


        SqlAll = @"SELECT
					    ILTB46.T46MAK ,ILTB46.T46TIC, GNMB20.DescriptionTHAI as GNB2TD ,ILTB46.T46TNM ,ILTB46.T46ENM 
					    ,SUBSTRING(CAST(ILTB46.T46MAK as varchar),1,2) +'-'+ SUBSTRING(CAST(ILTB46.T46MAK as varchar),3,6) +'-'+ SUBSTRING(CAST(ILTB46.T46MAK as varchar),9,3)+'-'+SUBSTRING(CAST(ILTB46.T46MAK as varchar),12,1) as STRT46MAK
					    ,T46ADR,T46MOO ,T46VIL ,T46BIL ,T46BUD,T46ROM ,T46FLO,T46SOI,T46ROD,T46TMC,A.DescriptionTHAI as GN18DT,T46AMC,B.DescriptionTHAI as GN19DT,T46PVC,T46ZIP
					    ,T46AD2,T46MO2,T46VI2,T46BI2,T46BU2,T46RO2,T46FL2,T46SO2,T46RD2,T46TM2,AA.DescriptionTHAI as GN18DT2 ,T46AM2,BB.DescriptionTHAI as GN19DT2,T46PV2,T46ZI2
					    ,T46REG,T46TAX,T46TE1,T46TLR,T46FX1,T46F1T,T46FX2,T46F2T,T46HOM
					    ,T46HTL,T46HMB,T46CRD,T46ODD,T46RES,T46POT
					    ,T46RDP,T46RE2,T46PO2,T46RP2,T46TRT,T46RSV
					    ,T46UDD,T46UDT,T46PGM,T46USR,T46DSP,T46DEL
					    ,D.Code + ' : ' + TRIM(D.DescriptionTHAI) + D.DescriptionENG as GT08ES, DD.Code + ' : ' + DD.DescriptionTHAI as GT08ES2
					    ,A.DescriptionENG as A1,B.DescriptionENG as B1,C.DescriptionENG as C1, AA.DescriptionTHAI as A2,BB.DescriptionTHAI as B2,CC.DescriptionTHAI as C2
				    FROM AS400DB01.ILOD0001.ILTB46 ILTB46 WITH (NOLOCK)
					    LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter GNMB20 WITH (NOLOCK) ON (T46TIC = GNMB20.Code and GNMB20.[Type] = 'OfficeTitleID')
					    LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter D WITH (NOLOCK) ON (T46BIL = D.Code and d.[Type] = 'BuildingTitleID')
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol A WITH (NOLOCK) ON T46TMC = A.Code
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur B WITH (NOLOCK)ON T46AMC = B.Code
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince C WITH (NOLOCK) ON T46PVC = C.Code
					    LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter DD WITH (NOLOCK) ON (T46BI2 = DD.Code and DD.[Type] = 'BuildingTitleID')
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol AA WITH (NOLOCK) ON T46TM2 = AA.Code
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur BB WITH (NOLOCK) ON T46AM2 = BB.Code
					    LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince CC WITH (NOLOCK) ON T46PV2 = CC.Code
				    WHERE T46DEL ='' ";

        if (ddl_popup_SearchBy.SelectedValue == "C" && txt_SearchMaker.Text.Trim() != "")
        {
            //spilt string
            string value = txt_SearchMaker.Text.Trim();
            char[] delimiters = new char[] { '-' };
            string[] parts = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string SearchMaker = "";
            for (int i = 0; i < parts.Length; i++)
            {
                SearchMaker = SearchMaker + parts[i];
            }

            sqlwhere += " AND CAST(T46MAK as nvarchar) = '" + SearchMaker.Trim() + "'";
        }
        if (ddl_popup_SearchBy.SelectedValue == "T" && txt_SearchMaker.Text.Trim() != "")
        {
            sqlwhere += " AND GNB2TD LIKE N'" + txt_SearchMaker.Text.Trim() + "%'";
        }
        if (ddl_popup_SearchBy.SelectedValue == "MN" && txt_SearchMaker.Text.Trim() != "")
        {
            sqlwhere += " AND T46TNM  LIKE '" + txt_SearchMaker.Text.Trim() + "%'";
        }
        if (ddl_popup_SearchBy.SelectedValue == "MEn" && txt_SearchMaker.Text.Trim() != "")
        {
            sqlwhere += " AND T46ENM LIKE '" + txt_SearchMaker.Text.ToUpper().Trim() + "%'";
        }

        // ---------------sql

        SqlAll = SqlAll + sqlwhere;

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        hidden_makergrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
        if (cookiesStorage.check_dataset(DS))
        {
            GridView1.DataSource = DS;
            GridView1.DataBind();
        }
        else
        {
            E_popup_error.Text = "Data not found";
            dataCenter.CloseConnectSQL();
            return;
        }
        dataCenter.CloseConnectSQL();
        ResetGrid(GridView1, hidden_makergrid.Value);
    }
    protected void btn_popup_search_Click(object sender, EventArgs e)
    {
        LoadMaker();
    }
    protected void btn_popup_clear_Click(object sender, EventArgs e)
    {
        Clear_Popup_Maker();
    }
    private void Clear_Popup_Maker()
    {
        ddl_popup_SearchBy.SelectedIndex = 0;
        txt_SearchMaker.Text = "";
        LoadMaker();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.SelectedIndex = -1;
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(hidden_makergrid.Value);
        GridView1.DataBind();
    }
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataSet dsp_makergrid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(hidden_makergrid.Value);
        DataRow dr = dsp_makergrid.Tables[0]?.Rows[(GridView1.PageIndex * Convert.ToInt16(GridView1.PageSize)) + e.NewSelectedIndex];
        //set value
        //-----------Main
        MakerCode.Text = dr[0].ToString().Trim();
        txt_MakerCode.Text = dr[5].ToString().Trim();
        bind_OfficeTitle(dr[1].ToString().Trim());
        txt_ThaiName.Text = dr[3].ToString().Trim();
        txt_EngName.Text = dr[4].ToString().Trim();
        //-----------tab Regis Address
        txt_reAddress.Text = dr[6].ToString().Trim();
        txt_reMoo.Text = dr[7].ToString().Trim();
        txt_reVilage.Text = dr[8].ToString().Trim();
        bind_buildingType(dr[9].ToString().Trim(), dd_BuildingType1);
        txt_reBuilding.Text = dr[10].ToString().Trim();
        txt_reRoom.Text = dr[11].ToString().Trim();
        txt_reFloor.Text = dr[12].ToString().Trim();
        txt_reSoi.Text = dr[13].ToString().Trim();
        txt_reRoad.Text = dr[14].ToString().Trim();
        // C_address_I.Text = dr[15].ToString().Trim();
        D_tambol1.Text = dr[16].ToString().Trim();
        D_amphur1.Text = dr[18].ToString().Trim();
        //D_province1.Text = dr[17].ToString().Trim();
        //D_zipcode1.Text = dr[18].ToString().Trim();
        Get_D_Tombol1();
        Get_D_Amphur1();
        //----------------lOCAL Address
        txt_loAddress.Text = dr[21].ToString().Trim();
        txt_loMoo.Text = dr[22].ToString().Trim();
        txt_loVilage.Text = dr[23].ToString().Trim();
        //dd_BuildingType2.Text = dr[24].ToString().Trim();
        bind_buildingType(dr[24].ToString().Trim(), dd_BuildingType2);
        txt_loBuilding.Text = dr[25].ToString().Trim();
        txt_loRoom.Text = dr[26].ToString().Trim();
        txt_loFloor.Text = dr[27].ToString().Trim();
        txt_loSoi.Text = dr[28].ToString().Trim();
        txt_loRoad.Text = dr[29].ToString().Trim();
        // C_address_I_2.Text = dr[18].ToString().Trim();
        D_tambol2.Text = dr[31].ToString().Trim();
        D_amphur2.Text = dr[33].ToString().Trim();
        Get_D_Tombol2();
        Get_D_Amphur2();
        // D_province2.Text = dr[18].ToString().Trim();
        // D_zipcode2.Text = dr[18].ToString().Trim();
        //-----------------------tab other
        txt_RegisterNo_TaxID.Text = dr[36].ToString().Trim();
        txt_TaxID.Text = dr[37].ToString().Trim();
        txt_TelNo.Text = dr[38].ToString().Trim();
        txt_ContactTel.Text = dr[39].ToString().Trim();
        t_FaxNo1.Text = "0" + dr[40].ToString().Trim();
        dd_FaxNo1Type.SelectedValue = dr[41].ToString().Trim();
        t_FaxNo2.Text = dr[42].ToString().Trim();
        dd_FaxNo2Type.SelectedValue = dr[43].ToString().Trim();

        txt_Ref_Person1.Text = dr[49].ToString().Trim();
        txt_Ref_Position1.Text = dr[50].ToString().Trim();
        txt_Ref_Dept1.Text = dr[51].ToString().Trim();
        txt_Ref_Person2.Text = dr[52].ToString().Trim();
        txt_Ref_Position2.Text = dr[53].ToString().Trim();
        txt_Ref_Dept2.Text = dr[54].ToString().Trim();
        txt_IssueInvoiceDays.Text = dr[47].ToString().Trim();
        //-----------------------tab whom contact
        Get_Whom_Contact();
        //-----------------------tab note
        Get_Note();
        ResetGrid(GridView1, hidden_makergrid.Value);
        Popup_AddMaker.ShowOnPageLoad = false;
        Clear_Popup_Maker();
        if (View != "1")
        {
            set_Enabled("3");//edit
        }
    }

    #endregion

    #region confirm Save,Action Save ,Validate Save
    protected void btnConfirmOK_Click(object sender, EventArgs e)
    {

    }
    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
        }
    }

    private bool ValidateForm()
    {
        return true;
    }
    #endregion

    #region     Action Delete
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnConfirm_OK_Click(object sender, EventArgs e)
    {

    }
    protected void btnConfirm_Cancel_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region Action Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        View = "2"; //new
    }
    #endregion

    #region Address Regis
    protected void C_address_I_TextChanged(object sender, EventArgs e)
    {
        Get_C_Address_I1();

    }
    private void Get_C_Address_I1()
    {
        L_count.Text = "";
        m_userInfo = userInfoService.GetUserInfo();
        if (C_address_I.Text.Trim() == "")
        {
            return;
        }

        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = new DataSet();

        if (C_address_I.Text.Trim().Length > 20)
        {
            DS = CallMasterEnt.getTambol(C_address_I.Value.ToString().Substring(0, 5), "").Result;

            if (DS != null && DS.Tables[0]?.Rows.Count > 0)
            {
                D_tambol1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());

                }
                DS.Clear();
            }
            DS = CallMasterEnt.getAmphur("", "", C_address_I.Value.ToString().Substring(6, 4)).Result;

            if (DS != null)
            {
                D_amphur1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                }
                DS.Clear();
            }


            DS = CallMasterEnt.getProvince("", "", C_address_I.Value.ToString().Substring(11, 3)).Result;
            if (DS != null)
            {
                D_province1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                }
                DS.Clear();
            }
            D_zipcode1.Items.Clear();
            D_zipcode1.Items.Add(C_address_I.Value.ToString().Substring(15, 5), C_address_I.Value.ToString().Substring(15, 5));

            D_tambol1.SelectedIndex = 0;
            D_amphur1.SelectedIndex = 0;
            D_province1.SelectedIndex = 0;
            D_zipcode1.SelectedIndex = 0;
            C_address_I.Enabled = false;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        CallMasterEnt._dataCenter.CloseConnectSQL();



        DS = CallMasterEnt.getAddress(C_address_I.Text.Trim()).Result;//ilObj.RetriveAsDataSet(cmd);

        int count = 0;
        if (DS != null)
        {
            C_address_I.Items.Clear();
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                C_address_I.Items.Add(dr["Address"].ToString().Trim(), dr["Code"].ToString().Trim());
                count += 1;
            }
            DS.Clear();
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        CallMasterEnt._dataCenter.CloseConnectSQL();
        L_count.Text = count.ToString() + " Matching";
    }
    protected void D_tambol1_TextChanged(object sender, EventArgs e)
    {
        Get_D_Tombol1();
    }
    private void Get_D_Tombol1()
    {
        if (D_tambol1.Text.Trim() == "")
        {
            return;
        }
        m_userInfo = userInfoService.GetUserInfo();
        iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        //ilObj = new ILDataCenter();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = iLDataSubroutine.getTambol("", D_tambol1.Text.Trim());
        string Mem_tambol = D_tambol1.Text.Trim();

        int count_tambol = 0;
        D_tambol1.Items.Clear();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                count_tambol += 1;
            }
            DS.Clear();
        }
        D_tambol1.SelectedIndex = 0;

        if (count_tambol == 0)
        {
            D_amphur1.Items.Clear();
            D_province1.Items.Clear();
            D_zipcode1.Items.Clear();
            D_amphur1.Text = "";
            D_province1.Text = "";
            D_zipcode1.Text = "";

            D_tambol1.Text = Mem_tambol;
            D_tambol1.Focus();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }


        /***************************************  Find Amphur  ********************************************/

        DS = CallMasterEnt.getAmphur("", D_tambol1.Text.Trim()).Result; //ilObj.RetriveAsDataSet(cmd);

        int count_amphur = 0;
        D_amphur1.Items.Clear();
        if (DS != null)
        {
            D_amphur1.Items.Add("---Select---", "");
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                count_amphur += 1;
            }
            DS.Clear();
        }
        D_amphur1.SelectedIndex = 0;

        if (count_amphur == 0)
        {
            D_tambol1.Items.Clear();
            D_province1.Items.Clear();
            D_zipcode1.Items.Clear();
            D_amphur1.Focus();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        if (count_amphur > 1)
        {
            CallMasterEnt._dataCenter.CloseConnectSQL();
            D_province1.Items.Clear();
            D_province1.Text = "";
            D_zipcode1.Items.Clear();
            D_zipcode1.Text = "";
            D_amphur1.Text = "";
            D_amphur1.Focus();
            D_amphur1.SelectedIndex = -1;
            return;
        }
        else if (count_amphur == 1)
        {
            D_amphur1.SelectedIndex = 1;
        }

        /***************************************  Find Province  ********************************************/
        D_province1.Items.Clear();
        D_zipcode1.Items.Clear();
        if ((count_tambol == 1) & (count_amphur == 1))
        {
            DS = CallMasterEnt.getProvince(D_tambol1.Text.Trim(), D_amphur1.Text.Trim()).Result;//ilObj.RetriveAsDataSet(cmd);

            int count_province = 0;
            if (DS != null)
            {
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                    D_zipcode1.Items.Add(dr["gn21zp"].ToString().Trim());
                    count_province += 1;
                }
                DS.Clear();
            }
            D_province1.SelectedIndex = 0;
            D_zipcode1.SelectedIndex = 0;
        }

        if (count_amphur == 1)
        {
            D_tambol1.Enabled = false;
            D_amphur1.Enabled = false;
            D_province1.Enabled = false;
            D_zipcode1.Enabled = false;
        }
        else
        {
            D_tambol1.Enabled = true;
            D_amphur1.Enabled = true;
            D_province1.Enabled = true;
            D_zipcode1.Enabled = true;
        }

        CallMasterEnt._dataCenter.CloseConnectSQL();
    }
    protected void D_amphur1_TextChanged(object sender, EventArgs e)
    {
        Get_D_Amphur1();
    }
    private void Get_D_Amphur1()
    {
        if (D_amphur1.Text.Trim() == "")
        {
            return;
        }

        //ilObj = new ILDataCenter();
        m_userInfo = userInfoService.GetUserInfo();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = CallMasterEnt.getAmphur(D_amphur1.Text.Trim(), "").Result;//new DataSet();    
        string Mem_amphur = D_amphur1.Text.Trim();

        int count_amphur = 0;
        D_amphur1.Items.Clear();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                count_amphur += 1;
            }
            DS.Clear();
        }
        D_amphur1.SelectedIndex = 0;

        if (count_amphur == 0)
        {
            D_amphur1.Items.Clear();
            D_province1.Items.Clear();
            D_zipcode1.Items.Clear();
            D_amphur1.Text = "";
            D_province1.Text = "";
            D_zipcode1.Text = "";

            D_amphur1.Text = Mem_amphur;
            D_amphur1.Focus();
            return;
        }

        if (D_tambol1.Value.ToString().Trim() == "")
        {
            DS = CallMasterEnt.getProvince("", D_amphur1.Text.Trim()).Result;

        }
        else
        {
            DS = CallMasterEnt.getProvince(D_tambol1.Text.Trim(), D_amphur1.Text.Trim()).Result;

        }

        string Mem_tambol = D_tambol1.Text.Trim();
        int count_tambol = 0;
        if (DS != null && DS.Tables[0]?.Rows.Count > 0)
        {
            D_tambol1.Items.Clear();
            D_province1.Items.Clear();
            D_zipcode1.Items.Clear();
            D_tambol1.Text = "";
            D_province1.Text = "";
            D_zipcode1.Text = "";
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                D_zipcode1.Items.Add(dr["gn21zp"].ToString().Trim());
                count_tambol += 1;
            }
            DS.Clear();
        }

        if (count_tambol == 0)
        {
            DS = CallMasterEnt.getProvince("", D_amphur1.Text.Trim()).Result;

            if (DS != null && DS.Tables[0]?.Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                }
                DS.Clear();
            }
            //List tambol

            D_tambol1.Text = Mem_tambol;
            D_tambol1.Focus();
            //m_da400.CloseConnect();
            return;
        }
        if (count_tambol == 1)
        {
            D_tambol1.SelectedIndex = 0;
            D_province1.SelectedIndex = 0;
            D_zipcode1.SelectedIndex = 0;
            D_tambol1.Enabled = false;
            D_amphur1.Enabled = false;
            D_province1.Enabled = false;
            D_zipcode1.Enabled = false;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        if (count_tambol > 1)
        {
            D_tambol1.Focus();
            D_tambol1.Enabled = true;
            D_amphur1.Enabled = true;
            D_province1.Enabled = true;
            D_zipcode1.Enabled = true;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
    }
    #endregion

    #region Address Local
    protected void C_address_I_2_TextChanged(object sender, EventArgs e)
    {
        Get_C_Address_I2();

    }

    private void Get_C_Address_I2()
    {
        L_count2.Text = "";

        if (C_address_I_2.Text.Trim() == "")
        {
            return;
        }

        //ilObj = new ILDataCenter();
        m_userInfo = userInfoService.GetUserInfo();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = new DataSet();

        if (C_address_I_2.Text.Trim().Length > 20)
        {
            DS = CallMasterEnt.getTambol(C_address_I_2.Value.ToString().Substring(0, 5), "").Result;

            if (DS != null)
            {
                D_tambol2.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_tambol2.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());

                }
                DS.Clear();
            }
            DS = CallMasterEnt.getAmphur("", "", C_address_I_2.Value.ToString().Substring(6, 4)).Result;

            if (DS != null)
            {
                D_amphur2.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_amphur2.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                }
                DS.Clear();
            }


            DS = CallMasterEnt.getProvince("", "", C_address_I_2.Value.ToString().Substring(11, 3)).Result;
            if (DS != null)
            {
                D_province2.Items.Clear();
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_province2.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                }
                DS.Clear();
            }
            D_zipcode2.Items.Clear();
            D_zipcode2.Items.Add(C_address_I_2.Value.ToString().Substring(15, 5), C_address_I_2.Value.ToString().Substring(15, 5));

            D_tambol2.SelectedIndex = 0;
            D_amphur2.SelectedIndex = 0;
            D_province2.SelectedIndex = 0;
            D_zipcode2.SelectedIndex = 0;
            C_address_I_2.Enabled = false;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        CallMasterEnt._dataCenter.CloseConnectSQL();



        DS = CallMasterEnt.getAddress(C_address_I_2.Text.Trim()).Result;

        int count = 0;
        if (DS != null)
        {
            C_address_I_2.Items.Clear();
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                C_address_I_2.Items.Add(dr["Address"].ToString().Trim(), dr["Code"].ToString().Trim());
                count += 1;
            }
            DS.Clear();
        }
        L_count2.Text = count.ToString() + " Matching";
    }

    protected void D_tambol2_TextChanged(object sender, EventArgs e)
    {
        Get_D_Tombol2();

    }

    private void Get_D_Tombol2()
    {

        if (D_tambol2.Text.Trim() == "")
        {
            return;
        }

        //ilObj = new ILDataCenter();
        m_userInfo = userInfoService.GetUserInfo();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = CallMasterEnt.getTambol("", D_tambol2.Text.Trim()).Result;
        string Mem_tambol = D_tambol2.Text.Trim();

        int count_tambol = 0;
        D_tambol2.Items.Clear();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_tambol2.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                count_tambol += 1;
            }
            DS.Clear();
        }
        D_tambol2.SelectedIndex = 0;

        if (count_tambol == 0)
        {
            D_amphur2.Items.Clear();
            D_province2.Items.Clear();
            D_zipcode2.Items.Clear();
            D_amphur2.Text = "";
            D_province2.Text = "";
            D_zipcode2.Text = "";

            D_tambol1.Text = Mem_tambol;
            D_tambol1.Focus();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }


        /***************************************  Find Amphur  ********************************************/

        DS = CallMasterEnt.getAmphur("", D_tambol2.Text.Trim()).Result;

        int count_amphur = 0;
        D_amphur2.Items.Clear();
        if (DS != null)
        {
            D_amphur2.Items.Add("---Select---", "");
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_amphur2.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                count_amphur += 1;
            }
            DS.Clear();
        }
        D_amphur2.SelectedIndex = 0;

        if (count_amphur == 0)
        {
            D_tambol2.Items.Clear();
            D_province2.Items.Clear();
            D_zipcode2.Items.Clear();
            D_amphur2.Focus();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        if (count_amphur > 1)
        {
            CallMasterEnt._dataCenter.CloseConnectSQL();
            D_province2.Items.Clear();
            D_province2.Text = "";
            D_zipcode2.Items.Clear();
            D_zipcode2.Text = "";
            D_amphur2.Text = "";
            D_amphur2.Focus();
            D_amphur2.SelectedIndex = -1;
            return;
        }
        else if (count_amphur == 1)
        {
            D_amphur2.SelectedIndex = 1;
        }

        /***************************************  Find Province  ********************************************/
        D_province2.Items.Clear();
        D_zipcode2.Items.Clear();
        if ((count_tambol == 1) & (count_amphur == 1))
        {
            DS = CallMasterEnt.getProvince(D_tambol2.Text.Trim(), D_amphur2.Text.Trim()).Result;//ilObj.RetriveAsDataSet(cmd);

            int count_province = 0;
            if (DS != null)
            {
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_province2.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                    D_zipcode2.Items.Add(dr["gn21zp"].ToString().Trim());
                    count_province += 1;
                }
                DS.Clear();
            }
            D_province2.SelectedIndex = 0;
            D_zipcode2.SelectedIndex = 0;
        }

        if (count_amphur == 1)
        {
            D_tambol2.Enabled = false;
            D_amphur2.Enabled = false;
            D_province2.Enabled = false;
            D_zipcode2.Enabled = false;
        }
        else
        {
            D_tambol2.Enabled = true;
            D_amphur2.Enabled = true;
            D_province2.Enabled = true;
            D_zipcode2.Enabled = true;
        }

        CallMasterEnt._dataCenter.CloseConnectSQL();
    }
    protected void D_amphur2_TextChanged(object sender, EventArgs e)
    {
        Get_D_Amphur2();

    }

    private void Get_D_Amphur2()
    {
        if (D_amphur2.Text.Trim() == "")
        {
            return;
        }

        //ilObj = new ILDataCenter();
        m_userInfo = userInfoService.GetUserInfo();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        dataCenter = new DataCenter(m_userInfo);
        DataSet DS = CallMasterEnt.getAmphur(D_amphur2.Text.Trim(), "").Result;//new DataSet();    
        string Mem_amphur = D_amphur2.Text.Trim();

        int count_amphur = 0;
        D_amphur2.Items.Clear();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_amphur2.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                count_amphur += 1;
            }
            DS.Clear();
        }
        D_amphur2.SelectedIndex = 0;

        if (count_amphur == 0)
        {
            D_amphur2.Items.Clear();
            D_province2.Items.Clear();
            D_zipcode2.Items.Clear();
            D_amphur2.Text = "";
            D_province2.Text = "";
            D_zipcode2.Text = "";

            D_amphur2.Text = Mem_amphur;
            D_amphur2.Focus();
            return;
        }

        if (D_tambol2.Value.ToString().Trim() == "")
        {
            DS = CallMasterEnt.getProvince("", D_amphur2.Text.Trim()).Result;

        }
        else
        {
            DS = CallMasterEnt.getProvince(D_tambol2.Text.Trim(), D_amphur2.Text.Trim()).Result;

        }

        string Mem_tambol = D_tambol2.Text.Trim();
        int count_tambol = 0;
        if (DS != null)
        {
            D_tambol2.Items.Clear();
            D_province2.Items.Clear();
            D_zipcode2.Items.Clear();
            D_tambol2.Text = "";
            D_province2.Text = "";
            D_zipcode2.Text = "";
            foreach (DataRow dr in DS.Tables[0]?.Rows)
            {
                D_tambol2.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                D_province2.Items.Add(dr["gn20dt"].ToString().Trim(), dr["gn20cd"].ToString().Trim());
                D_zipcode2.Items.Add(dr["gn21zp"].ToString().Trim());
                count_tambol += 1;
            }
            DS.Clear();
        }

        if (count_tambol == 0)
        {
            DS = CallMasterEnt.getProvince("", D_amphur2.Text.Trim()).Result;

            if (DS != null)
            {
                foreach (DataRow dr in DS.Tables[0]?.Rows)
                {
                    D_tambol2.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());
                }
                DS.Clear();
            }
            //List tambol

            D_tambol2.Text = Mem_tambol;
            D_tambol2.Focus();
            //m_da400.CloseConnect();
            return;
        }
        if (count_tambol == 1)
        {
            D_tambol2.SelectedIndex = 0;
            D_province2.SelectedIndex = 0;
            D_zipcode2.SelectedIndex = 0;
            D_tambol2.Enabled = false;
            D_amphur2.Enabled = false;
            D_province2.Enabled = false;
            D_zipcode2.Enabled = false;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        if (count_tambol > 1)
        {
            D_tambol2.Focus();
            D_tambol2.Enabled = true;
            D_amphur2.Enabled = true;
            D_province2.Enabled = true;
            D_zipcode2.Enabled = true;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
    }
    #endregion

    #region tab whom to contact
    private void Get_Whom_Contact()
    {
        SqlAll = @"Select 
					 T49PAR,T49RTY,T49COD,T49SEQ,T49RNO,T49TNM,T49ENM,T49DEP,T49TE1,T49TR1,T49EX1,T49FX1,T49TE2,T49TR2,
					 T49EX2,T49FX2,T49TE3,T49TR3,T49EX3,T49FX3,T49HMB,T49RSV,T49UDD,T49UDT,T49PGM,T49USR,T49DSP,T49DEL
					FROM AS400DB01.ILOD0001.ILTB49 WITH (NOLOCK)
					WHERE T49PAR = '" + MakerCode.Text + "'";

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        ds_hiddenWhom.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
        if (cookiesStorage.check_dataset(DS))
        {
            gWhom_Contact.DataSource = DS;
            gWhom_Contact.DataBind();
        }
        else
        {
            E_popup_error.Text = "Data not found";
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }
        CallMasterEnt._dataCenter.CloseConnectSQL();
        ResetGrid(gWhom_Contact, ds_hiddenWhom.Value);
    }
    protected void gWhom_Contact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gWhom_Contact_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gWhom_Contact_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    #endregion

    #region tab note
    private void Get_Note()
    {
        SqlAll = @" Select 
                        P15MAK,P15NSQ,P15LSQ,P15NOT,P15UDT,P15UTM,P15UUS,P15UPG,P15UWS,P15RST 
                    FROM  AS400DB01.ILOD0001.ilms15 WITH (NOLOCK)   
                    WHERE P15MAK = '" + MakerCode.Text + "'";

        DataSet DS = new DataSet();
        m_userInfo = userInfoService.GetUserInfo();
        
        ilObj.UserInfomation = m_userInfo;
        dataCenter = new DataCenter(m_userInfo);
        DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
        if (cookiesStorage.check_dataset(DS))
        {
            ds_Note.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
            gvNote.DataSource = DS;
            gvNote.DataBind();
        }
        else
        {
            E_popup_error.Text = "Data not found";
            dataCenter.CloseConnectSQL();
            return;
        }

        dataCenter.CloseConnectSQL();
        ResetGrid(gvNote, ds_Note.Value);
    }
    protected void gvNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gvNote_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvNote_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    #endregion

}