using DevExpress.Web.ASPxEditors;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ILSystem.App_Code.Model.AS400DB01.AS400DB01Model;
using ILSystem.App_Code.BLL.Integrate;

public partial class ManageData_WorkProcess_UserControl_UC_Verify_Call : System.Web.UI.UserControl
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    public delegate void ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate(string data);


    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataCust_IN;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTO_IN;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTH_IN;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTM_IN;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTE_IN;

    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataCust_KE;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTO_KE;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTH_KE;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTM_KE;
    public event ManageData_WorkProcess_UserControl_UC_Verify_CallDelegate getDataTE_KE;

    DataCenter dataCenter;
    ILDataCenterMssqlInterview iLDataCenterMssql;
    ILDataSubroutine iLDataSubroutine;
    public UserInfoService userInfoService;

    ILDataCenter ilObj;

    UserInfo userInfo;

    public string CSN
    {
        get
        {
            if (ViewState["CSN"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["CSN"]);
            }
        }
        set
        {
            ViewState["CSN"] = value;
            if (ViewState["CSN"].ToString() != "")
            {
                hid_CSN.Value = (string)(ViewState["CSN"]);
            }
        }
    }
    public string AppNo
    {

        get
        {
            if (ViewState["AppNo"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["AppNo"]);
            }
        }
        set
        {
            ViewState["AppNo"] = value;
            if (ViewState["AppNo"].ToString() != "")
            {
                hid_AppNo.Value = (string)(ViewState["AppNo"]);
            }
        }
    }
    public string BrnNo
    {
        get
        {
            if (ViewState["BrnNo"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["BrnNo"]);
            }
        }
        set
        {
            ViewState["BrnNo"] = value;
            if (ViewState["BrnNo"].ToString() != "")
            {
                hid_brn.Value = (string)(ViewState["BrnNo"]);
            }
        }
    }
    public string Status
    {
        get
        {
            if (ViewState["Status"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["Status"]);
            }
        }
        set
        {
            ViewState["Status"] = value;
            if (ViewState["Status"].ToString() != "")
            {
                hid_status.Value = (string)(ViewState["Status"]);
            }
        }

    }
    //public interface IFilter
    //{
    //    void SetLabel(string text);
    //}


    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            catch { }
            Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

        }
        if (IsPostBack)
        {
            if (!(String.IsNullOrEmpty(txt_mobile_TM_P.Text.Trim())))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "script1", "try{ txt_mobile_TM_P.SetValue('" + txt_mobile_TM_P.Text + "') }catch (err) {}", true);
            }
            return;
        }
        else
        {
        }

    }

    public void changeColor()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds_rltb71 = iLDataCenterMssql.getRLtb71("99", "", "", "Y").Result;

            if (ilObj.check_dataset(ds_rltb71))
            {
                foreach (DataRow dr in ds_rltb71.Tables[0].Rows)
                {
                    if (dr["t71cd1"].ToString().Trim() == "01")
                    {
                        Label2.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "02")
                    {
                        Label3.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "03")
                    {
                        Label12.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "04")
                    {
                        Label7.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "05")
                    {
                        Label124.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "06")
                    {
                        Label127.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "07")
                    {
                        //Label134.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "08")
                    {
                        Label137.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "09")
                    {
                        Label135.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "10")
                    {
                        Label136.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "11")
                    {
                        Label138.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "12")
                    {
                        Label129.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "13")
                    {
                        Label139.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "14")
                    {
                        Label112.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "15")
                    {
                        Label149_.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "16")
                    {
                        //Label150.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "17")
                    {
                        Label42_.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "18")
                    {
                        Label42.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "19")
                    {
                        Label151.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "20")
                    {
                        Label152.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "21")
                    {
                        Label161.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "22")
                    {
                        Label157.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (dr["t71cd1"].ToString().Trim() == "23")
                    {
                        Label160.ForeColor = System.Drawing.Color.Blue;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    public void bindData()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            dataCenter = new DataCenter(userInfo);
            iLDataSubroutine = new ILDataSubroutine(userInfo);

            changeColor();
            Session["ds_perRel"] = null;
            gvTE.DataSource = null;
            gvTE.DataBind();
            loadData();



            //****  check panel  ****//
            if (hid_status.Value == "INTERVIEW")
            {

            }
            else if (hid_status.Value == "KESSAI")
            {
                ddlCompanyType.Enabled = false;
                dd_cust_type.Enabled = false;
                dd_subType.Enabled = false;
                panelTE(false);
                panelTH(false);
                panelTM(false);
                panelTO(false);
                panel_saveCust_kessai();

            }
            changeButton();


        }
        catch (Exception ex)
        {

        }

    }
    private void loadData()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds_cus = iLDataCenterMssql.getCSMS13(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value).Result;
            DataSet ds_11 = iLDataCenterMssql.getCSMS11(hid_CSN.Value).Result;
            string HPhone = "";
            string HPhone_To = "";
            string HPhone_ext = "";
            string OPhone = "";
            string OPhone_To = "";
            string OPhone_ext = "";
            string mobile_ = "";
            string salary_cus = "";

            //***  DATA FROM CSMS13  &&  CSMS11 ***//
            if ((ilObj.check_dataset(ds_cus)) && (ilObj.check_dataset(ds_11)))
            {
                DataRow dr_cus = ds_cus.Tables[0].Rows[0];
                DataRow[] dr_H = ds_11.Tables[0].Select("M11REF = '0' AND M11CDE = 'H' ");
                DataRow[] dr_O = ds_11.Tables[0].Select("M11REF = '0' AND M11CDE = 'O' ");
                salary_cus = decimal.Parse(dr_cus["M13NET"].ToString()).ToString("0");

                foreach (DataRow row in dr_H)
                {
                    HPhone = (row["M11TEL"].ToString().Trim()).PadRight(10, ' ').Substring(0, 9);
                    if (row["M11TEL"].ToString().Trim().Length > 10)
                    {
                        HPhone_To = (row["M11TEL"].ToString().Trim()).Substring(10);
                    }
                    HPhone_ext = row["M11EXT"].ToString().Trim();
                    mobile_ = row["M11MOB"].ToString().Trim();
                }
                foreach (DataRow row in dr_O)
                {
                    OPhone = (row["M11TEL"].ToString().Trim()).PadRight(10, ' ').Substring(0, 9);
                    if (row["M11TEL"].ToString().Trim().Length > 10)
                    {
                        OPhone_To = (row["M11TEL"].ToString().Trim()).Substring(10);
                    }
                    OPhone_ext = row["M11EXT"].ToString().Trim();
                }

            }



            DataSet ds_ = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            if (!(ilObj.check_dataset(ds_)))
            {
                DataSet ds_30 = iLDataCenterMssql.get_csms30(hid_CSN.Value).Result;
                bind_V_CompanyType();
                bind_V_CustomerType();
                if (!(ilObj.check_dataset(ds_30)))
                {
                    dd_cust_type.Value = "N";
                }

                bind_V_SubType();

                bind_V_recieveMoney();  //1.
                txt_salary_TO.Text = salary_cus;  //2.
                bind_V_SSO();  //.3
                bind_V_BOL();  //4.
                //txt_off_TO.Text = OPhone; //5.
                //txt_tel_off2_TO.Text = OPhone_To; // 5.1
                //txt_tel_off_ext_TO.Text = OPhone_ext; //5.2
                txt_brn_tel.Text = ""; //6
                txt_tel2_brn.Text = ""; // 6.1
                txt_tel2_ext.Text = ""; //6.2
                //bind_V_1133(); //7.
                bind_V_checkName_TO(); //8.  dr[""].ToString().Trim()
                txt_res_name_TO.Text = "";
                bind_V_checkAddr_TO(); //9.
                txt_res_addr_TO.Text = "";
                bind_V_statusEmp_TO(); //10.
                bind_V_ResContact_TO();  //11.
                bind_V_Person_TO();  //12.
                txt_person_to.Text = "";
                bind_V_Type_employment(); //13.
                txt_name_support.Text = ""; //14.
                txt_pos_support.Text = ""; //14.1
                txt_dep_support.Text = ""; //14.2                
                //txt_tel_TH.Text = HPhone; //15
                //txt_tel_2_TH.Text = HPhone_To; //15.1
                //txt_tel_ext_TH.Text = HPhone_ext; //15.2
                //bind_V_1133_TH(); //16.
                bind_V_Check_name_TH(); //17.
                bind_V_check_Addr_TH(); //18.
                bind_V_dd_type_tel(); //19.
                bind_V_res_contact_TH(); //20.
                bind_V_Person_TH();
                bind_V_res_contact_TM(); //23
                bind_V_Relation();
                txt_seq.Text = "";
                txt_Fname.Text = "";
                txt_Lname.Text = "";
                txt_tel1_TE.Text = "";
                txt_tel1_to_TE.Text = "";
                txt_tel1_ext_TE.Text = "";
                txt_tel2_TE.Text = "";
                txt_tel2_to_TE.Text = "";
                txt_tel2_ext.Text = "";
                bind_PersonRelation();

                //เบอร์โทรจะโหลดจากข้อมูล CSMS11 ทุกครั้ง
                txt_off_TO.Text = OPhone; //5.
                txt_tel_off2_TO.Text = OPhone_To; // 5.1
                txt_tel_off_ext_TO.Text = OPhone_ext; //5.2
                txt_tel_TH.Text = HPhone; //15
                txt_tel_2_TH.Text = HPhone_To; //15.1
                txt_tel_ext_TH.Text = HPhone_ext; //15.2
                //txt_mobile_TM.Text = mobile_;

                panelTE(false);
                panelTH(false);
                panelTM(false);
                panelTO(false);

                btn_insertPerson.Enabled = false;
                btn_saveTE.Enabled = false;
                btn_saveTH.Enabled = false;
                btn_saveTM.Enabled = false;
                btn_saveTO.Enabled = false;


            }
            else
            {

                // *** Bind  Customer type  ***//
                DataRow dr = ds_.Tables[0].Rows[0];
                bind_V_CompanyType(dr["W5KTDL"].ToString().Trim());
                bind_V_CustomerType(dr["w5csty"].ToString().Trim());
                bind_V_SubType(dr["w5sbcd"].ToString().Trim());
                bind_V_Relation();
                //*** TO ***//
                if (dr["w5fvto"].ToString().Trim() == "Y")
                {
                    bind_V_recieveMoney(dr["w5salt"].ToString().Trim());  //1.
                    txt_salary_TO.Text = decimal.Parse((dr["W5INET"].ToString().Trim())).ToString("0");  //2.
                    bind_V_SSO(dr["W5SSO"].ToString().Trim());  //.3
                    bind_V_BOL(dr["W5BOL"].ToString().Trim());  //4.
                    txt_off_TO.Text = dr["w5hotl"].ToString().Trim().PadRight(14, ' ').Substring(0, 9); //5.
                    txt_tel_off2_TO.Text = dr["w5hotl"].ToString().Trim().PadRight(14, ' ').Substring(10, 4); // 5.1
                    txt_tel_off_ext_TO.Text = dr["w5hoex"].ToString().Trim(); //5.2
                    txt_brn_tel.Text = dr["w5wotl"].ToString().Trim().PadRight(14, ' ').Substring(0, 9); //6
                    txt_tel2_brn.Text = dr["w5wotl"].ToString().Trim().PadRight(14, ' ').Substring(10, 4); // 6.1
                    txt_tel_ext_brn.Text = dr["w5woex"].ToString().Trim(); //6.2
                    txt_tel_off_mobil.Text = dr["w5motl"].ToString().Trim();
                    //bind_V_1133(dr["w5voot"].ToString().Trim()); //7.
                    bind_V_checkName_TO(dr["w5vonm"].ToString().Trim()); //8.  dr[""].ToString().Trim()
                    txt_res_name_TO.Text = dr["w5tonm"].ToString().Trim();
                    bind_V_checkAddr_TO(dr["w5voad"].ToString().Trim()); //9.
                    txt_res_addr_TO.Text = dr["w5toad"].ToString().Trim();
                    bind_V_statusEmp_TO(dr["w5emst"].ToString().Trim()); //10.
                    bind_V_ResContact_TO(dr["w5rvto"].ToString().Trim());  //11.
                    bind_V_Person_TO(dr["w5ipto"].ToString().Trim());  //12.
                    txt_person_to.Text = dr["w5tito"].ToString().Trim();
                    bind_V_Type_employment(dr["w5tyem"].ToString().Trim()); //13.
                    txt_name_support.Text = dr["w5letn"].ToString().Trim(); //14.
                    txt_pos_support.Text = dr["w5letp"].ToString().Trim(); //14.1
                    txt_dep_support.Text = dr["w5letd"].ToString().Trim(); //14.2

                }
                else if (dr["w5fvto"].ToString().Trim() == "")
                {
                    bind_V_recieveMoney();  //1.
                    txt_salary_TO.Text = salary_cus;  //2.
                    bind_V_SSO();  //.3
                    bind_V_BOL();  //4.
                    //txt_off_TO.Text = OPhone; //5.
                    //txt_tel_off2_TO.Text = OPhone_To; // 5.1
                    //txt_tel_off_ext_TO.Text = OPhone_ext; //5.2
                    txt_brn_tel.Text = ""; //6
                    txt_tel2_brn.Text = ""; // 6.1
                    txt_tel2_ext.Text = ""; //6.2
                    //bind_V_1133(); //7.
                    bind_V_checkName_TO(); //8.  
                    txt_res_name_TO.Text = "";
                    bind_V_checkAddr_TO(); //9.
                    txt_res_addr_TO.Text = "";
                    bind_V_statusEmp_TO(); //10.
                    bind_V_ResContact_TO();  //11.
                    bind_V_Person_TO();  //12.
                    txt_person_to.Text = "";
                    bind_V_Type_employment(); //13.
                    txt_name_support.Text = ""; //14.
                    txt_pos_support.Text = ""; //14.1
                    txt_dep_support.Text = ""; //14.2 
                    txt_tel_off_mobil.Text = "";

                    // bind csms11 //
                    txt_off_TO.Text = OPhone; //5.
                    txt_tel_off2_TO.Text = OPhone_To; // 5.1
                    txt_tel_off_ext_TO.Text = OPhone_ext; //5.2


                }
                //*** TH ***//
                if (dr["w5fvth"].ToString().Trim() == "Y")
                {
                    if (dr["W5VRTH"].ToString().Trim() != "Y")
                    {
                        cb_notHave_TH.Checked = false;
                        txt_tel_TH.Text = dr["w5htl"].ToString().Trim().PadRight(14, ' ').Substring(0, 9); //15
                        txt_tel_2_TH.Text = dr["w5htl"].ToString().Trim().PadRight(14, ' ').Substring(10, 4); //15.1
                        txt_tel_ext_TH.Text = dr["w5htex"].ToString().Trim(); //15.2
                        //bind_V_1133_TH(dr["w5vhht"].ToString().Trim()); //16.
                        bind_V_Check_name_TH(dr["w5vhnm"].ToString().Trim()); //17.
                        txt_res_name_TH.Text = dr["w5thnm"].ToString().Trim();
                        bind_V_check_Addr_TH(dr["w5vhad"].ToString().Trim()); //18.
                        txt_res_addr_TH.Text = dr["w5thad"].ToString().Trim();

                        bind_V_dd_type_tel(dr["w5tytl"].ToString().Trim()); //19.
                        bind_V_res_contact_TH(dr["w5rvth"].ToString().Trim()); //20.
                        bind_V_Person_TH(dr["w5ipth"].ToString().Trim());
                        txt_dd_person_TH.Text = dr["w5tith"].ToString().Trim();
                    }
                    else
                    {
                        cb_notHave_TH.Checked = true;
                        txt_tel_TH.Text = "";
                        txt_tel_2_TH.Text = "";
                        txt_tel_ext_TH.Text = "";
                        //bind_V_1133_TH(); //16.
                        bind_V_Check_name_TH(); //17.
                        txt_res_name_TH.Text = "";
                        bind_V_check_Addr_TH(); //18.
                        txt_res_addr_TH.Text = "";

                        bind_V_dd_type_tel(); //19.
                        bind_V_res_contact_TH(); //20.
                        bind_V_Person_TH();
                        txt_dd_person_TH.Text = "";

                        txt_tel_TH.Enabled = false;
                        txt_tel_2_TH.Enabled = false;
                        txt_tel_ext_TH.Enabled = false;
                        //dd_1133_TH.Enabled = false;
                        dd_chkName_TH.Enabled = false;
                        txt_res_name_TH.Enabled = false;
                        dd_chk_addr_TH.Enabled = false;
                        txt_res_addr_TH.Enabled = false;
                        dd_type_tel.Enabled = false;
                        dd_resContact_TH.Enabled = false;
                        dd_person_TH.Enabled = false;
                        txt_dd_person_TH.Enabled = false;

                    }
                }
                else
                {
                    cb_notHave_TH.Checked = false;
                    txt_tel_TH.Text = "";
                    txt_tel_2_TH.Text = "";
                    txt_tel_ext_TH.Text = "";
                    //bind_V_1133_TH(); //16.
                    bind_V_Check_name_TH(); //17.
                    txt_res_name_TH.Text = "";
                    bind_V_check_Addr_TH(); //18.
                    txt_res_addr_TH.Text = "";

                    bind_V_dd_type_tel(); //19.
                    bind_V_res_contact_TH(); //20.
                    bind_V_Person_TH();
                    txt_dd_person_TH.Text = "";


                    // bind csms11 //
                    txt_tel_TH.Text = HPhone; //15
                    txt_tel_2_TH.Text = HPhone_To; //15.1
                    txt_tel_ext_TH.Text = HPhone_ext; //15.2


                }
                ScriptManager.RegisterStartupScript(this, typeof(string), "script6", "try{ txt_mobile_TM_P.SetValue('') }catch (err) {}", true);
                //*** TM ***//
                if (dr["w5fvtm"].ToString().Trim() == "Y")
                {

                    //***  TM ***//
                    if (dr["W5VRTM"].ToString().Trim() != "Y")
                    {
                        cb_check_TM.Checked = false;
                        txt_mobile_TM.Text = dr["W5MBTL"].ToString().Trim();
                        bind_V_res_contact_TM(dr["w5vmtl"].ToString().Trim()); //23

                    }
                    else
                    {
                        cb_check_TM.Checked = true;
                        txt_mobile_TM.Text = "";
                        txt_mobile_TM_P.Text = "";
                        bind_V_res_contact_TM(); //23
                        txt_mobile_TM.Enabled = false;

                        dd_resContact_TM.Enabled = false;
                    }
                }
                else
                {
                    cb_check_TM.Checked = false;
                    txt_mobile_TM.Text = "";
                    txt_mobile_TM_P.Text = "";
                    bind_V_res_contact_TM(); //23

                    // bind csms11 //
                    //txt_mobile_TM.Text = mobile_;

                }
                //***  TE ***//
                if (dr["w5fvte"].ToString().Trim() == "Y")
                {
                    bind_V_Relation();
                    txt_seq.Text = "";
                    txt_seq.Text = "";
                    txt_Fname.Text = "";
                    txt_Lname.Text = "";
                    txt_tel1_TE.Text = "";
                    txt_tel1_to_TE.Text = "";
                    txt_tel1_ext_TE.Text = "";
                    txt_tel2_TE.Text = "";
                    txt_tel2_to_TE.Text = "";
                    txt_tel2_ext.Text = "";

                    bind_PersonRelation();
                }
                else
                {
                    bind_PersonRelation();
                }

                //*****  check เรื่องเบอร์โทรศัพท์  ******//

                if (dr["I_TH"].ToString().Trim() == "")
                {
                    // bind csms11 //
                    txt_tel_TH.Text = HPhone; //15
                    txt_tel_2_TH.Text = HPhone_To; //15.1
                    txt_tel_ext_TH.Text = HPhone_ext; //15.2
                }
                if (dr["I_TM"].ToString().Trim() == "")
                {

                }
                if (dr["I_TO"].ToString().Trim() == "")
                {
                    // bind csms11 //
                    txt_off_TO.Text = OPhone; //5.
                    txt_tel_off2_TO.Text = OPhone_To; // 5.1
                    txt_tel_off_ext_TO.Text = OPhone_ext; //5.2
                }

                panel_saveCust(dr["W5CSTY"].ToString().Trim(), dr["W5SBCD"].ToString().Trim());
            }


        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
            lblMsgEN.Text = ex.Message.ToString();
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
        dataCenter.CloseConnectSQL();
    }


    private void bind_PersonRelation()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds = iLDataCenterMssql.getILWK12(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value).Result;
            if (ilObj.check_dataset(ds))
            {
                gvTE.DataSource = ds.Tables[0];
                gvTE.DataBind();
                DataRow dr = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];

                Session["ds_perRel"] = ds;
                txt_seq.Text = (int.Parse(dr["Seq"].ToString()) + 1).ToString();
            }
            else
            {
                txt_seq.Text = "1";
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void checkStatusForShow()
    {
        if (hid_status.Value == "INTERVIEW")
        {
            dd_recieveMoney.Enabled = true;   //1.
            txt_salary_TO.Enabled = true; ;  //2.
            dd_sso.Enabled = true;  //.3
            dd_BOL.Enabled = true;  //4.
            txt_off_TO.Enabled = true;  //5.
            txt_tel_off2_TO.Enabled = true; // 5.1
            txt_tel_off_ext_TO.Enabled = true; //5.2
            txt_brn_tel.Enabled = true; //6
            txt_tel2_brn.Enabled = true; // 6.1
            txt_tel2_ext.Enabled = true; //6.2
            //dd_1133.Enabled = true; //7.
            dd_chkName.Enabled = true;  //8. 
            txt_res_name_TO.Enabled = true;
            dd_chkAddr.Enabled = true; //9.
            txt_res_addr_TO.Enabled = true;
            dd_statusEmp.Enabled = true; //10.
            dd_res_contact_TO.Enabled = true;  //11.
            dd_person_to.Enabled = true;  //12.
            txt_person_to.Enabled = true;
            dd_empType.Enabled = true; //13.
            txt_name_support.Enabled = true; //14.
            txt_pos_support.Enabled = true; //14.1
            txt_dep_support.Enabled = true; //14.2

        }
        else if (hid_status.Value == "KESSAI")
        {
            dd_recieveMoney.Enabled = false;   //1.
            txt_salary_TO.Enabled = false; ;  //2.
            dd_sso.Enabled = false;  //.3
            dd_BOL.Enabled = false;  //4.
            txt_off_TO.Enabled = false;  //5.
            txt_tel_off2_TO.Enabled = false; // 5.1
            txt_tel_off_ext_TO.Enabled = false; //5.2
            txt_brn_tel.Enabled = false; //6
            txt_tel2_brn.Enabled = false; // 6.1
            txt_tel2_ext.Enabled = false; //6.2
            //dd_1133.Enabled = false; //7.
            dd_chkName.Enabled = false;  //8. 
            txt_res_name_TO.Enabled = false;
            dd_chkAddr.Enabled = false; //9.
            txt_res_addr_TO.Enabled = false;
            dd_statusEmp.Enabled = false; //10.
            dd_res_contact_TO.Enabled = false;  //11.
            dd_person_to.Enabled = false;  //12.
            txt_person_to.Enabled = false;
            dd_empType.Enabled = false; //13.
            txt_name_support.Enabled = false; //14.
            txt_pos_support.Enabled = false; //14.1
            txt_dep_support.Enabled = false; //14.2
            txt_tel_TH.Enabled = false; //15
            txt_tel_2_TH.Enabled = false; //15.1
            txt_tel_ext_TH.Enabled = false; //15.2
            //dd_1133_TH.Enabled = false;
            dd_chkName_TH.Enabled = false;
            //dd_1133_TH.Enabled = false;
            dd_chkName_TH.Enabled = false;
            dd_chk_addr_TH.Enabled = false;
            dd_type_tel.Enabled = false;
            dd_resContact_TH.Enabled = false;
            dd_person_TH.Enabled = false;
            dd_resContact_TM.Enabled = false;


            //bind_V_1133_TH(); //16.
            //bind_V_Check_name_TH(); //17.
            //bind_V_check_Addr_TH(); //18.
            //bind_V_dd_type_tel(); //19.
            //bind_V_res_contact_TH(); //20.
            //bind_V_Person_TH();
            //bind_V_res_contact_TM(); //23
        }

    }



    #region function
    // relation 
    private void bind_V_Relation(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_Relation"] != null)
            {
                ds = (DataSet)Cache["ds_V_Relation"];
            }
            else
            {
                ds = iLDataCenterMssql.getGNTB14().Result;
                Cache["ds_V_Relation"] = ds;
                Cache.Insert("ds_V_Relation", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }



            //DataSet ds = ilObj.getGNTB14();
            dd_relation.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_relation.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_relation.Items.Add(
                        new ListEditItem(dr["gn14cd"].ToString().Trim() + " : " + dr["gn14td"].ToString().Trim(), dr["gn14cd"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_relation.Value = code;
                }
                else
                {
                    dd_relation.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    //  verify call
    private void bind_V_CompanyType(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_CompanyType"] != null)
            {
                ds = (DataSet)Cache["ds_V_CompanyType"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("42", "", "").Result;
                Cache["ds_V_CompanyType"] = ds;
                Cache.Insert("ds_V_CompanyType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //DataSet ds = ilObj.getRLtb71("98", "", "N");
            ddlCompanyType.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                ddlCompanyType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddlCompanyType.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    ddlCompanyType.Value = code;
                }
                else
                {
                    ddlCompanyType.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_CustomerType(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_CustomerType"] != null)
            {
                ds = (DataSet)Cache["ds_V_CustomerType"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("98", "", "N").Result;
                Cache["ds_V_CustomerType"] = ds;
                Cache.Insert("ds_V_CustomerType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //DataSet ds = ilObj.getRLtb71("98", "", "N");
            dd_cust_type.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_cust_type.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_cust_type.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_cust_type.Value = code;
                }
                else
                {
                    dd_cust_type.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_SubType(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            string custType = dd_cust_type.SelectedItem.Value.ToString().Trim();
            DataSet ds = iLDataCenterMssql.getRLtb71("", custType, "Y").Result;
            dd_subType.Items.Clear();
            dd_subType.Value = "";
            dd_subType.Text = "";
            if (ilObj.check_dataset(ds))
            {
                dd_subType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_subType.Items.Add(
                        new ListEditItem(dr["t71cd2"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd2"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_subType.Value = code;
                }
                else
                {
                    dd_subType.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_recieveMoney(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds = new DataSet();
            if (Cache["ds_V_recieveMoney"] != null)
            {
                ds = (DataSet)Cache["ds_V_recieveMoney"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("01", "").Result;
                Cache["ds_V_recieveMoney"] = ds;
                Cache.Insert("ds_V_recieveMoney", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //DataSet ds = ilObj.getRLtb71("01", "");
            dd_recieveMoney.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_recieveMoney.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_recieveMoney.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_recieveMoney.Value = code;
                }
                else
                {
                    dd_recieveMoney.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_SSO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds = new DataSet();
            if (Cache["ds_V_SSO"] != null)
            {
                ds = (DataSet)Cache["ds_V_SSO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("03", "").Result;
                Cache["ds_V_SSO"] = ds;
                Cache.Insert("ds_V_SSO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("03", "");
            dd_sso.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_sso.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_sso.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_sso.Value = code;
                }
                else
                {
                    dd_sso.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_BOL(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_BOL"] != null)
            {
                ds = (DataSet)Cache["ds_V_BOL"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("04", "").Result;
                Cache["ds_V_BOL"] = ds;
                Cache.Insert("ds_V_BOL", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //DataSet ds = ilObj.getRLtb71("04", "");
            dd_BOL.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_BOL.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_BOL.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_BOL.Value = code;
                }
                else
                {
                    dd_BOL.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    //private void bind_V_1133(string code = "")
    //{
    //    try
    //    {
    //        ilObj = new ILDataCenter();
    //        DataSet ds = ilObj.getRLtb71("07", "");
    //        dd_1133.Items.Clear();
    //        if (ilObj.check_dataset(ds))
    //        {
    //            dd_1133.Items.Add("--- Select ---", "");
    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                dd_1133.Items.Add(
    //                    new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
    //            }

    //            if (code != "")
    //            {
    //                dd_1133.Value = code;
    //            }
    //            else
    //            {
    //                dd_1133.SelectedIndex = -1;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //private void bind_V_1133_TH(string code = "")
    //{
    //    try
    //    {
    //        ilObj = new ILDataCenter();
    //        DataSet ds = ilObj.getRLtb71("16", "");
    //        dd_1133_TH.Items.Clear();
    //        if (ilObj.check_dataset(ds))
    //        {
    //            dd_1133_TH.Items.Add("--- Select ---", "");
    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                dd_1133_TH.Items.Add(
    //                    new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
    //            }

    //            if (code != "")
    //            {
    //                dd_1133_TH.Value = code;
    //            }
    //            else
    //            {
    //                dd_1133_TH.SelectedIndex = -1;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    private void bind_V_checkName_TO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_checkName_TO"] != null)
            {
                ds = (DataSet)Cache["ds_V_checkName_TO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("08", "").Result;
                Cache["ds_V_checkName_TO"] = ds;
                Cache.Insert("ds_V_checkName_TO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("08", "");
            dd_chkName.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_chkName.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_chkName.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_chkName.Value = code;
                }
                else
                {
                    dd_chkName.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_checkAddr_TO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_checkAddr_TO"] != null)
            {
                ds = (DataSet)Cache["ds_V_checkAddr_TO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("09", "").Result;
                Cache["ds_V_checkAddr_TO"] = ds;
                Cache.Insert("ds_V_checkAddr_TO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("09", "");
            dd_chkAddr.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_chkAddr.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_chkAddr.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_chkAddr.Value = code;
                }
                else
                {
                    dd_chkAddr.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void bind_V_statusEmp_TO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_statusEmp_TO"] != null)
            {
                ds = (DataSet)Cache["ds_V_statusEmp_TO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("10", "").Result;
                Cache["ds_V_statusEmp_TO"] = ds;
                Cache.Insert("ds_V_statusEmp_TO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("10", "");
            dd_statusEmp.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_statusEmp.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_statusEmp.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_statusEmp.Value = code;
                }
                else
                {
                    dd_statusEmp.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_ResContact_TO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_ResContact_TO"] != null)
            {
                ds = (DataSet)Cache["ds_V_ResContact_TO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("11", "").Result;
                Cache["ds_V_ResContact_TO"] = ds;
                Cache.Insert("ds_V_ResContact_TO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("11", "");
            dd_res_contact_TO.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_res_contact_TO.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_res_contact_TO.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_res_contact_TO.Value = code;
                }
                else
                {
                    dd_res_contact_TO.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_Person_TO(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_Person_TO"] != null)
            {
                ds = (DataSet)Cache["ds_V_Person_TO"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("12", "").Result;
                Cache["ds_V_Person_TO"] = ds;
                Cache.Insert("ds_V_Person_TO", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("12", "");
            dd_person_to.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_person_to.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_person_to.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_person_to.Value = code;
                }
                else
                {
                    dd_person_to.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_Type_employment(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_Type_employment"] != null)
            {
                ds = (DataSet)Cache["ds_V_Type_employment"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("13", "").Result;
                Cache["ds_V_Type_employment"] = ds;
                Cache.Insert("ds_V_Type_employment", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("13", "");

            dd_empType.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_empType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_empType.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_empType.Value = code;
                }
                else
                {
                    dd_empType.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_Check_name_TH(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_Check_name_TH"] != null)
            {
                ds = (DataSet)Cache["ds_V_Check_name_TH"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("17", "").Result;
                Cache["ds_V_Check_name_TH"] = ds;
                Cache.Insert("ds_V_Check_name_TH", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("17", "");
            dd_chkName_TH.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_chkName_TH.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_chkName_TH.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_chkName_TH.Value = code;
                }
                else
                {
                    dd_chkName_TH.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_check_Addr_TH(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_check_Addr_TH"] != null)
            {
                ds = (DataSet)Cache["ds_V_check_Addr_TH"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("18", "").Result;
                Cache["ds_V_check_Addr_TH"] = ds;
                Cache.Insert("ds_V_check_Addr_TH", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("18", "");
            dd_chk_addr_TH.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_chk_addr_TH.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_chk_addr_TH.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_chk_addr_TH.Value = code;
                }
                else
                {
                    dd_chk_addr_TH.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_dd_type_tel(string code = "")
    {
        try
        {

            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_dd_type_tel"] != null)
            {
                ds = (DataSet)Cache["ds_V_dd_type_tel"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("19", "").Result;
                Cache["ds_V_dd_type_tel"] = ds;
                Cache.Insert("ds_V_dd_type_tel", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("19", "");
            dd_type_tel.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_type_tel.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_type_tel.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }
                if (code != "")
                {
                    dd_type_tel.Value = code;
                }
                else
                {
                    dd_type_tel.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_res_contact_TH(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_res_contact_TH"] != null)
            {
                ds = (DataSet)Cache["ds_V_res_contact_TH"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("20", "").Result;
                Cache["ds_V_res_contact_TH"] = ds;
                Cache.Insert("ds_V_res_contact_TH", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("20", "");
            dd_resContact_TH.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_resContact_TH.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_resContact_TH.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_resContact_TH.Value = code;
                }
                else
                {
                    dd_resContact_TH.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_Person_TH(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_Person_TH"] != null)
            {
                ds = (DataSet)Cache["ds_V_Person_TH"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("21", "").Result;
                Cache["ds_V_Person_TH"] = ds;
                Cache.Insert("ds_V_Person_TH", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("21", "");
            dd_person_TH.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_person_TH.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_person_TH.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_person_TH.Value = code;
                }
                else
                {
                    dd_person_TH.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void bind_V_res_contact_TM(string code = "")
    {
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_V_res_contact_TM"] != null)
            {
                ds = (DataSet)Cache["ds_V_res_contact_TM"];
            }
            else
            {
                ds = iLDataCenterMssql.getRLtb71("23", "").Result;
                Cache["ds_V_res_contact_TM"] = ds;
                Cache.Insert("ds_V_res_contact_TM", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            //ilObj = new ILDataCenter();
            //DataSet ds = ilObj.getRLtb71("23", "");
            dd_resContact_TM.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_resContact_TM.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_resContact_TM.Items.Add(
                        new ListEditItem(dr["t71cd1"].ToString().Trim() + " : " + dr["t71dst"].ToString().Trim(), dr["t71cd1"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_resContact_TM.Value = code;
                }
                else
                {
                    dd_resContact_TM.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }

    private void changeButton()
    {
        if (hid_status.Value == "INTERVIEW")
        {
            btn_saveCust.Text = "Save Customer";
            btn_saveTH.Text = "Save TH";
            btn_saveTO.Text = "Save TO";
            btn_saveTE.Text = "Save TE";
            btn_saveTM.Text = "Save TM";

        }
        else if (hid_status.Value == "KESSAI")
        {
            btn_saveCust.Text = "Confirm Customer";
            btn_saveTH.Text = "Confirm TH";
            btn_saveTO.Text = "Confirm TO";
            btn_saveTE.Text = "Confirm TE";
            btn_saveTM.Text = "Confirm TM";
            btn_insertPerson.Enabled = false;
        }
    }


    private void insertCustomer()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        dataCenter = new DataCenter(userInfo);
        ilObj.UserInfomation = userInfo;
        try
        {

            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            

            string w5csno = hid_CSN.Value;
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;
            string w5csty = dd_cust_type.SelectedItem.Value.ToString().Substring(0, 1);
            string w5sbcd = dd_subType.Value.ToString();//dd_subType.SelectedItem.Value.ToString().Substring(0, 2);
            string w5cupd = AppDate;
            string w5cupt = m_UpdTime.ToString();
            string w5cusr = userInfo.Username.ToString();
            string w5cwrk = userInfo.LocalClient.ToString();
            string w5inet = txt_salary_TO.Text.Trim() == "" ? "0" : txt_salary_TO.Text.Replace(",", "");
            string w5crtd = AppDate;
            string w5crtt = m_UpdTime.ToString();
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            #region 82983  BOT Report 
            string w5ktdl = ddlCompanyType.Value.ToString();
            string w5kcrd = "";
            string w5kupd = AppDate;
            string w5kupt = m_UpdTime.ToString();
            string w5kusr = userInfo.Username.ToString();
            string w5kwrk = userInfo.LocalClient.ToString();
            string w5kacd = "";
            string w5kaud = AppDate;
            string w5kaut = m_UpdTime.ToString();
            string w5kaus = userInfo.Username.ToString(); 
            string w5kauw = userInfo.LocalClient.ToString();
            #endregion
            iDB2Command cmd = new iDB2Command();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";

                cmd.Parameters.Clear();
                //cmd.CommandText = ilObj.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                //int res13 = ilObj.ExecuteNonQuery(cmd);
                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                //if (dataCenter.SqlCon.State == ConnectionState.Closed)
                //    dataCenter.OpenConnectSQL();
                //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text, dataCenter.Sqltr?.Connection == null ? true : false).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }

            cmd.Parameters.Clear();
            //cmd.CommandText = ilObj.INSERT_ILWK05(w5csno, w5brn, w5apno, w5csty, w5sbcd,
            //                     w5cupd, w5cupt, w5cusr, w5cwrk, w5inet,
            //                     w5crtd, w5crtt, w5updt, w5uptm, w5user,
            //                     w5prgm, w5wrks, w5ktdl, w5kcrd, w5kupd, w5kupt, w5kusr, w5kwrk, w5kacd, w5kaud, w5kaut, w5kaus, w5kauw);
            //int res05 = ilObj.ExecuteNonQuery(cmd);

            string cmd_ILWK05 = iLDataCenterMssql.INSERT_ILWK05(w5csno, w5brn, w5apno, w5csty, w5sbcd,
                                 w5cupd, w5cupt, w5cusr, w5cwrk, w5inet,
                                 w5crtd, w5crtt, w5updt, w5uptm, w5user,
                                 w5prgm, w5wrks, w5ktdl, w5kcrd, w5kupd, w5kupt, w5kusr, w5kwrk, w5kacd, w5kaud, w5kaut, w5kaus, w5kauw);
            bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
            var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
            int res05 = res_ILWK05.afrows;
            if (res05 == -1)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save not complete";
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }


            dataCenter.CommitMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save completed";
            if (getDataCust_IN != null)
            {
                getDataCust_IN(w5user);
            }

            //panel_saveCust();
            loadData();
            //*******************//


            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
        }
    }
    private void updateCustomer()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        lblMsgEN.Text = "";
        lblMsgTH.Text = "";
        ilObj = new ILDataCenter();
        dataCenter = new DataCenter(userInfo);
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        ilObj.UserInfomation = userInfo;

        try
        {

            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            
            string w5csno = hid_CSN.Value;
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;
            string w5csty = dd_cust_type.SelectedItem.Value.ToString();
            string w5sbcd = dd_subType.Value == null ? "" : dd_subType.Value.ToString();
            string w5cupd = AppDate;
            string w5cupt = m_UpdTime.ToString();
            string w5cusr = userInfo.Username.ToString();
            string w5cwrk = userInfo.LocalClient.ToString();
            string w5crtd = AppDate;
            string w5crtt = m_UpdTime.ToString();
            string w5caud = "0";
            string w5caut = "0";
            string w5caus = "";
            string w5cauw = "";
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            #region 82983  BOT Report 
            string w5ktdl = ddlCompanyType.SelectedItem.Value.ToString();
            string w5kcrd = "";
            string w5kupd = AppDate;
            string w5kupt = m_UpdTime.ToString();
            string w5kusr = userInfo.Username.ToString();
            string w5kwrk = userInfo.LocalClient.ToString();
            string w5kacd = "";
            string w5kaud = AppDate;
            string w5kaut = m_UpdTime.ToString();
            string w5kaus = userInfo.Username.ToString(); ;
            string w5kauw = userInfo.LocalClient.ToString();
            #endregion
            iDB2Command cmd = new iDB2Command();
            //if (dataCenter.SqlCon.State == ConnectionState.Closed)
            //    dataCenter.OpenConnectSQL();
            //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";
                cmd.Parameters.Clear();
                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text, transaction).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }
            cmd.Parameters.Clear();
            if (hid_status.Value == "INTERVIEW")
            {
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_Cust_INT(w5csty, w5sbcd, w5cupd, w5cupt,
                                                               w5cusr, w5cwrk, w5crtd, w5crtt,
                                                               w5updt, w5uptm, w5user, w5prgm,
                                                               w5wrks, w5brn, w5apno, w5ktdl, w5kcrd, w5kupd, w5kupt, w5kusr, w5kwrk, w5kacd, w5kaud, w5kaut, w5kaus, w5kauw);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res05 = res_ILWK05.afrows;
                if (res05 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataCust_IN != null)
                {
                    getDataCust_IN(w5user);
                }

                //panel_saveCust();
                loadData();
                //*******************//
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5caud = AppDate;
                w5caut = m_UpdTime.ToString();
                w5caus = userInfo.Username.ToString();
                w5cauw = userInfo.LocalClient.ToString();

                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_Cust_KESSAI(w5caud, w5caut, w5caus, w5cauw, w5updt, w5uptm, w5user, w5prgm,
                                                           w5wrks, w5brn, w5apno, w5ktdl, w5kcrd, w5kupd, w5kupt, w5kusr, w5kwrk, w5kacd, w5kaud, w5kaut, w5kaus, w5kauw);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res05 = res_ILWK05.afrows;
                if (res05 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }

                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataCust_KE != null)
                {
                    getDataCust_KE(w5user);
                }



                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }



        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }
    //****  new version for Open channal  get from rltb70 ****//
    private void panel_saveCust_kessai()
    {
        try
        {
            string custType = dd_cust_type.Value.ToString();
            string cust_sub = "";
            if(dd_subType.Value != null)
            {
                custType = dd_subType.Value.ToString();
            }
            else
            {
                custType = "";
            }
           

            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds = iLDataCenterMssql.getRLtb70(custType, cust_sub).Result;
            if (ilObj.check_dataset(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                // Open TO
                if (dr["T70VTO"].ToString().Trim() == "Y")
                {
                    panelTO(true);
                    btn_saveTO.Enabled = true;
                }
                else
                {
                    panelTO(false);
                    btn_saveTO.Enabled = false;
                }
                // Open TH
                if (dr["T70VTH"].ToString().Trim() == "Y")
                {
                    panelTH(true);
                    btn_saveTH.Enabled = true;
                }
                else
                {
                    panelTH(false);
                    btn_saveTH.Enabled = false;
                }
                // Open TM
                if (dr["T70VTM"].ToString().Trim() == "Y")
                {
                    panelTM(true);
                    btn_saveTM.Enabled = true;
                }
                else
                {
                    panelTM(false);
                    btn_saveTM.Enabled = false;
                }
                // Open TE
                if (dr["T70VTE"].ToString().Trim() == "Y")
                {
                    panelTE(true);
                    btn_insertPerson.Enabled = false;
                    btn_saveTE.Enabled = true;
                }
                else
                {
                    panelTE(false);
                    btn_insertPerson.Enabled = false;
                    btn_saveTE.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    private void panel_saveCust(string cust, string cust_sub)
    {
        //*** open panel ***//
        try
        {
            ilObj = new ILDataCenter();
            dataCenter = new DataCenter(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds = iLDataCenterMssql.getRLtb70(cust, cust_sub).Result;
            if (ilObj.check_dataset(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                // Open TO
                if (dr["T70VTO"].ToString().Trim() == "Y")
                {
                    panelTO(true);
                    btn_saveTO.Enabled = true;
                }
                else
                {
                    panelTO(false);
                    btn_saveTO.Enabled = false;
                }
                // Open TH
                if (dr["T70VTH"].ToString().Trim() == "Y")
                {
                    panelTH(true);
                    btn_saveTH.Enabled = true;
                }
                else
                {
                    panelTH(false);
                    btn_saveTH.Enabled = false;
                }
                // Open TM
                if (dr["T70VTM"].ToString().Trim() == "Y")
                {
                    panelTM(true);
                    btn_saveTM.Enabled = true;
                }
                else
                {
                    panelTM(false);
                    btn_saveTM.Enabled = false;
                }
                // Open TE
                if (dr["T70VTE"].ToString().Trim() == "Y")
                {
                    panelTE(true);
                    btn_insertPerson.Enabled = true;
                    btn_saveTE.Enabled = true;
                }
                else
                {
                    panelTE(false);
                    btn_insertPerson.Enabled = false;
                    btn_saveTE.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            dataCenter.CloseConnectSQL();
        }
        dataCenter.CloseConnectSQL();
    }
    //****  Old version for Open channal ****//

    //private void panel_saveCust_kessai() 
    //{
    //    try
    //    {
    //        if (dd_cust_type.Value.ToString() == "N")
    //        {

    //            btn_saveTE.Enabled = true;
    //            btn_insertPerson.Enabled = false;
    //            btn_saveTH.Enabled = true;
    //            btn_saveTM.Enabled = true;
    //            btn_saveTO.Enabled = true;
    //        }
    //        else if (dd_cust_type.Value.ToString() == "S" || dd_cust_type.Value.ToString() == "Z")
    //        {

    //            btn_insertPerson.Enabled = false;
    //            btn_saveTE.Enabled = false;
    //            btn_saveTH.Enabled = false;
    //            btn_saveTM.Enabled = false;
    //            btn_saveTO.Enabled = false;

    //        }
    //        else if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value.ToString() == "01" || dd_subType.Value.ToString() == "02" || dd_subType.Value.ToString() == "06"))
    //        {

    //            btn_insertPerson.Enabled = false;
    //            btn_saveTE.Enabled = true;
    //            btn_saveTH.Enabled = true;
    //            btn_saveTM.Enabled = true;
    //            btn_saveTO.Enabled = true;
    //        }
    //        else if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value.ToString() == "03" || dd_subType.Value.ToString() == "04" || dd_subType.Value.ToString() == "05"))
    //        {
    //            if (dd_subType.Value.ToString() == "03")
    //            {

    //                btn_insertPerson.Enabled = false;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = false;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = false;
    //            }
    //            else if (dd_subType.Value.ToString() == "04")
    //            {
    //                btn_insertPerson.Enabled = false;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = false;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = true;
    //            }
    //            else if (dd_subType.Value.ToString() == "05")
    //            {
    //                btn_insertPerson.Enabled = false;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = true;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //private void panel_saveCust(string cust,string cust_sub) 
    //{
    //    //*** open panel ***//
    //    try
    //    {
    //        if (cust == "")
    //        {
    //            panelTE(false);
    //            panelTH(false);
    //            panelTM(false);
    //            panelTO(false);
    //            btn_insertPerson.Enabled = false;
    //            btn_saveTE.Enabled = false;
    //            btn_saveTH.Enabled = false;
    //            btn_saveTM.Enabled = false;
    //            btn_saveTO.Enabled = false;
    //        }

    //        else if (cust == "N")
    //        //else if (dd_cust_type.Value.ToString() == "N")
    //        {
    //            panelTE(true);
    //            panelTH(true);
    //            panelTM(true);
    //            panelTO(true);

    //            btn_saveTE.Enabled = true;
    //            btn_insertPerson.Enabled = true;
    //            btn_saveTH.Enabled = true;
    //            btn_saveTM.Enabled = true;
    //            btn_saveTO.Enabled = true;
    //        }
    //        else if (cust == "S" || cust == "Z")
    //        //else if (dd_cust_type.Value.ToString() == "S" || dd_cust_type.Value.ToString() == "Z")
    //        {
    //            panelTE(false);
    //            panelTH(false);
    //            panelTM(false);
    //            panelTO(false);
    //            btn_insertPerson.Enabled = false;
    //            btn_saveTE.Enabled = false;
    //            btn_saveTH.Enabled = false;
    //            btn_saveTM.Enabled = false;
    //            btn_saveTO.Enabled = false;

    //        }
    //        else if (cust == "O" && (cust_sub == "01" || dd_subType.Value.ToString() == "02" || cust_sub == "06"))
    //        //else if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value.ToString() == "01" || dd_subType.Value.ToString() == "02" || dd_subType.Value.ToString() == "06"))
    //        {
    //            panelTE(true);
    //            panelTH(true);
    //            panelTM(true);
    //            panelTO(true);
    //            btn_insertPerson.Enabled = true;
    //            btn_saveTE.Enabled = true;
    //            btn_saveTH.Enabled = true;
    //            btn_saveTM.Enabled = true;
    //            btn_saveTO.Enabled = true;
    //        }
    //        else if (cust == "O" && (cust_sub == "03" || cust_sub == "04" || cust_sub == "05"))
    //        //else if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value.ToString() == "03" || dd_subType.Value.ToString() == "04" || dd_subType.Value.ToString() == "05"))
    //        {
    //            if (cust_sub == "03")
    //            {
    //                panelTO(false);
    //                panelTH(false);
    //                panelTE(true);
    //                panelTM(true);
    //                btn_insertPerson.Enabled = true;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = false;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = false;
    //            }
    //            else if (cust_sub == "04")
    //            {
    //                panelTH(false);
    //                panelTO(true);
    //                panelTE(true);
    //                panelTM(true);

    //                btn_insertPerson.Enabled = true;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = false;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = true;
    //            }
    //            else if (cust_sub == "05")
    //            {
    //                panelTH(true);
    //                panelTO(false);
    //                panelTE(true);
    //                panelTM(true);

    //                btn_insertPerson.Enabled = true;
    //                btn_saveTE.Enabled = true;
    //                btn_saveTH.Enabled = true;
    //                btn_saveTM.Enabled = true;
    //                btn_saveTO.Enabled = false;
    //            }
    //        }
    //    }catch(Exception ex)
    //    {

    //    }
    //}

    private void saveTO()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        dataCenter = new DataCenter(userInfo);
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        ilObj.UserInfomation = userInfo;
        try
        {
            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            
            string w5vrto = "O";
            string w5fvto = "Y";
            string w5salt = dd_recieveMoney.SelectedItem.Value.ToString();
            string w5inet = txt_salary_TO.Text.Trim().Replace(",", "");
            string w5sso = dd_sso.SelectedItem.Value.ToString();
            string w5bol = dd_BOL.SelectedItem.Value.ToString();
            string w5hotl = ilObj.getTel(txt_off_TO.Text.Trim(), txt_tel_off2_TO.Text.Trim()); //txt_off_TO.Text.Trim()+"-"+txt_tel_off2_TO.Text.Trim();  
            string w5hoex = txt_tel_off_ext_TO.Text.Trim();
            string w5wotl = ilObj.getTel(txt_brn_tel.Text.Trim(), txt_tel2_brn.Text.Trim());//txt_brn_tel.Text.Trim()+"-"+txt_tel2_brn.Text.Trim();  
            string w5woex = txt_tel_ext_brn.Text.Trim();
            string w5motl = txt_tel_off_mobil.Text.Trim();
            string w5voot = "";//dd_1133.SelectedItem.Value.ToString(); 
            string w5vonm = dd_chkName.SelectedItem.Value.ToString();
            string w5tonm = txt_res_name_TO.Text.Trim();
            string w5voad = dd_chkAddr.SelectedItem.Value.ToString();
            string w5toad = txt_res_addr_TO.Text.Trim();
            string w5emst = dd_statusEmp.SelectedItem.Value.ToString();
            string w5rvto = dd_res_contact_TO.SelectedItem.Value.ToString();
            string w5ipto = dd_person_to.SelectedItem.Value.ToString();
            string w5tito = txt_person_to.Text.Trim();
            string w5tyem = dd_empType.SelectedItem.Value.ToString();
            string w5letn = txt_name_support.Text.Trim();
            string w5letp = txt_pos_support.Text.Trim();
            string w5letd = txt_dep_support.Text.Trim();
            string w5oupd = AppDate;
            string w5oupt = m_UpdTime.ToString();
            string w5ousr = userInfo.Username.ToString();
            string w5owrk = userInfo.LocalClient.ToString();
            string w5oaud = "0";
            string w5oaut = "0";
            string w5oaus = "";
            string w5oauw = "";
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;
            iDB2Command cmd = new iDB2Command();

            //if (dataCenter.SqlCon.State == ConnectionState.Closed)
            //    dataCenter.OpenConnectSQL();
            //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";

                cmd.Parameters.Clear();
                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text, transaction).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }
            cmd.Parameters.Clear();
            int res_udp = -1;
            if (hid_status.Value == "INTERVIEW")
            {


                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TO_INT(w5vrto, w5fvto, w5salt, w5inet,
                                  w5sso, w5bol, w5hotl, w5hoex,
                                  w5wotl, w5woex, w5motl, w5voot,
                                  w5vonm, w5tonm, w5voad, w5toad,
                                  w5emst, w5rvto, w5ipto, w5tito,
                                  w5tyem, w5letn, w5letp, w5letd,
                                  w5oupd, w5oupt, w5ousr, w5owrk,
                                  w5oaud, w5oaut, w5oaus, w5oauw,
                                  w5updt, w5uptm, w5user, w5prgm,
                                  w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05,CommandType.Text, transaction).Result;
                res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTO_IN != null)
                {
                    getDataTO_IN(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;

            }
            else if (hid_status.Value == "KESSAI")
            {
                w5oaud = AppDate;
                w5oaut = m_UpdTime.ToString();
                w5oaus = userInfo.Username.ToString();
                w5oauw = userInfo.LocalClient.ToString();
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TO_KESSAI(w5vrto, w5oaud, w5oaut,
                                                w5oaus, w5oauw, w5updt,
                                                w5uptm, w5user, w5prgm,
                                                w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTO_KE != null)
                {
                    getDataTO_KE(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;

            }

        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }
    private void SaveTH()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        dataCenter = new DataCenter(userInfo);
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        ilObj.UserInfomation = userInfo;
        try
        {
            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            

            string w5vrto = "H";
            string w5fvth = "Y";
            string w5vrth = cb_notHave_TH.Checked == true ? "Y" : "";
            string w5htl = ilObj.getTel(txt_tel_TH.Text.Trim(), txt_tel_2_TH.Text.Trim());//txt_tel_TH.Text.Trim()+"-"+txt_tel_2_TH.Text.Trim(); 
            string w5htex = txt_tel_ext_TH.Text.Trim();
            string w5vhht = "";//dd_1133_TH.SelectedItem.Value.ToString(); 
            string w5vhnm = dd_chkName_TH.SelectedItem.Value.ToString();
            string w5thnm = txt_res_name_TH.Text.Trim();
            string w5vhad = dd_chk_addr_TH.SelectedItem.Value.ToString();
            string w5thad = txt_res_addr_TH.Text.Trim();
            string w5tytl = dd_type_tel.SelectedItem.Value.ToString();
            string w5rvth = dd_resContact_TH.SelectedItem.Value.ToString();
            string w5ipth = dd_person_TH.SelectedItem.Value.ToString();
            string w5tith = txt_dd_person_TH.Text.Trim();
            string w5hupd = AppDate;
            string w5hupt = m_UpdTime.ToString();
            string w5husr = userInfo.Username.ToString();
            string w5hwrk = userInfo.LocalClient.ToString();
            string w5haud = "0";
            string w5haut = "0";
            string w5haus = "";
            string w5hauw = "";
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;

            if (cb_notHave_TH.Checked == true)
            {
                w5htl = "";
                w5htex = "";
                w5vhht = "";
                w5vhnm = "";
                w5thnm = "";
                w5vhad = "";
                w5thad = "";
                w5tytl = "";
                w5rvth = "";
                w5ipth = "";
                w5tith = "";
            }

            iDB2Command cmd = new iDB2Command();
            //if (dataCenter.SqlCon.State == ConnectionState.Closed)
            //    dataCenter.OpenConnectSQL();
            //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";

                cmd.Parameters.Clear();

                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text, transaction).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }



            cmd.Parameters.Clear();
            int res_udp = -1;
            if (hid_status.Value == "INTERVIEW")
            {
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TH_INT(w5vrto, w5fvth, w5vrth, w5htl,
                                          w5htex, w5vhht, w5vhnm, w5thnm,
                                          w5vhad, w5thad, w5tytl, w5rvth,
                                          w5ipth, w5tith, w5hupd, w5hupt,
                                          w5husr, w5hwrk, w5haud, w5haut,
                                          w5haus, w5hauw, w5updt, w5uptm,
                                          w5user, w5prgm, w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTH_IN != null)
                {
                    getDataTH_IN(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;

            }
            else if (hid_status.Value == "KESSAI")
            {
                w5haud = AppDate;
                w5haut = m_UpdTime.ToString();
                w5haus = userInfo.Username.ToString();
                w5hauw = userInfo.LocalClient.ToString();
                // ***  check ก่อนว่า Interview ทำการบันทึกข้อมูลหรือยัง ***//
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TH_KESSAI(w5vrto, w5haud, w5haut, w5haus,
                                                                w5hauw, w5updt, w5uptm, w5user,
                                                                w5prgm, w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTH_KE != null)
                {
                    getDataTH_KE(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;

            }


        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }

    private void SaveTM()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        dataCenter = new DataCenter(userInfo);
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        ilObj.UserInfomation = userInfo;
        try
        {
            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            

            string w5vrto = "M";
            string w5fvtm = "Y";
            string w5vrtm = cb_check_TM.Checked == true ? "Y" : "";
            string w5mbtl = txt_mobile_TM.Text.Trim();
            string w5vmtl = dd_resContact_TM.SelectedItem.Value.ToString();
            string w5mupd = AppDate;
            string w5mupt = m_UpdTime.ToString();
            string w5musr = userInfo.Username.ToString();
            string w5mwrk = userInfo.LocalClient.ToString();
            string w5maud = "0";
            string w5maut = "0";
            string w5maus = "";
            string w5mauw = "";
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;
            iDB2Command cmd = new iDB2Command();
            //if (dataCenter.SqlCon.State == ConnectionState.Closed)
            //    dataCenter.OpenConnectSQL();
            //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";

                cmd.Parameters.Clear();
                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text, transaction).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }
            if (cb_check_TM.Checked == true)
            {
                w5mbtl = "";
                w5vmtl = "";
            }



            cmd.Parameters.Clear();
            if (hid_status.Value == "INTERVIEW")
            {
                //cmd.CommandText = ilObj.UPDATE_ILWK05_TM_INT(w5vrto, w5fvtm, w5vrtm, w5mbtl, w5vmtl,
                //                                                w5mupd, w5mupt, w5musr, w5mwrk, w5maud,
                //                                                w5maut, w5maus, w5mauw, w5updt, w5uptm,
                //                                                w5user, w5prgm, w5wrks, w5brn, w5apno);//"";
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TM_INT(w5vrto, w5fvtm, w5vrtm, w5mbtl, w5vmtl,
                                                                w5mupd, w5mupt, w5musr, w5mwrk, w5maud,
                                                                w5maut, w5maus, w5mauw, w5updt, w5uptm,
                                                                w5user, w5prgm, w5wrks, w5brn, w5apno);//"";
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTM_IN != null)
                {
                    getDataTM_IN(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5maud = AppDate;
                w5maut = m_UpdTime.ToString();
                w5maus = userInfo.Username.ToString();
                w5mauw = userInfo.LocalClient.ToString();
                // ***  check ก่อนว่า Interview ทำการบันทึกข้อมูลหรือยัง ***//

                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TM_KESSAI(w5vrto, w5maud, w5maut, w5maus,
                                                                w5mauw, w5updt, w5uptm, w5user,
                                                                w5prgm, w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTM_KE != null)
                {
                    getDataTM_KE(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;

            }


        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }


    private void SaveTE()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ilObj = new ILDataCenter();
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        dataCenter = new DataCenter(userInfo);
        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
        ilObj.UserInfomation = userInfo;
        try
        {
            string AppDate = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            

            string w5vrto = "E";
            string w5fvte = "Y";
            string w5eupd = AppDate;
            string w5eupt = m_UpdTime.ToString();
            string w5eusr = userInfo.Username.ToString();
            string w5ewrk = userInfo.LocalClient.ToString();
            string w5eaud = "0";
            string w5eaut = "0";
            string w5eaus = "";
            string w5eauw = "";
            string w5updt = AppDate;
            string w5uptm = m_UpdTime.ToString();
            string w5user = userInfo.Username.ToString();
            string w5prgm = "ILVERIFY";
            string w5wrks = userInfo.LocalClient.ToString();
            string w5brn = hid_brn.Value;
            string w5apno = hid_AppNo.Value;

            iDB2Command cmd = new iDB2Command();
            //if (dataCenter.SqlCon.State == ConnectionState.Closed)
            //    dataCenter.OpenConnectSQL();
            //dataCenter.Sqltr?.Connection = dataCenter.SqlCon.BeginTransaction();
            if (hid_status.Value == "INTERVIEW")
            {
                w5prgm = "INTERVIEW";
                cmd.Parameters.Clear();
                string cmd_MS13CRU = iLDataCenterMssql.UPDATE_MS13CRU(hid_brn.Value, hid_AppNo.Value);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_MS13CRU = dataCenter.Execute(cmd_MS13CRU, CommandType.Text ,transaction).Result;
                int res13 = res_MS13CRU.afrows;
                if (res13 == -1)
                {
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }


            }
            else if (hid_status.Value == "KESSAI")
            {
                w5prgm = "KESSAI";
            }

            cmd.Parameters.Clear();
            if (hid_status.Value == "INTERVIEW")
            {
                
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TE_INT(w5vrto, w5fvte, w5eupd, w5eupt, w5eusr,
                                                             w5ewrk, w5eaud, w5eaut, w5eaus, w5eauw,
                                                             w5updt, w5uptm, w5user, w5prgm, w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res_05 = res_ILWK05.afrows;
                if (res_05 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }

                
                cmd.Parameters.Clear();
                string cmd_ILWK12 = iLDataCenterMssql.DELETE_ILWK12(hid_brn.Value, hid_AppNo.Value);
                transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK12 = dataCenter.Execute(cmd_ILWK12, CommandType.Text, transaction).Result;
                int res_del12 = res_ILWK12.afrows;
                if (res_del12 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }

                for (int i = 0; i < gvTE.Rows.Count; i++)
                {
                    cmd.Parameters.Clear();

                    Label lb_Seq = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_Seq");
                    Label lb_relation = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_relation");
                    Label lb_name = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_name");
                    Label lb_surname = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_surname");
                    Label lb_tel_1 = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_tel_1");
                    Label lb_To = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_To");
                    Label lb_ext_1 = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_ext_1");
                    Label lb_tel_2 = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_tel_2");
                    Label lb_To2 = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_To2");
                    Label lb_ext_2 = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_ext_2");
                    Label lb_Mobile = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_Mobile");
                    Label lb_Verify = (Label)gvTE.Rows[i].Cells[0].FindControl("lb_Verify");

                    

                    cmd_ILWK12 = iLDataCenterMssql.INSERT_ILWK12(hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, lb_Seq.Text,
                                                          lb_relation.Text, "115", lb_name.Text, lb_surname.Text,
                                                          ilObj.getTel(lb_tel_1.Text, lb_To.Text),//lb_tel_1.Text +"-"+ lb_To.Text, 
                                                          lb_ext_1.Text,
                                                          ilObj.getTel(lb_tel_2.Text, lb_To2.Text),//lb_tel_2.Text +"-"+ lb_To2.Text,
                                                          lb_ext_2.Text,
                                                          lb_Mobile.Text, lb_Verify.Text,
                                                          AppDate, m_UpdTime.ToString(), userInfo.Username.ToString(), w5prgm, userInfo.LocalClient.ToString());
                    transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                    res_ILWK12 = dataCenter.Execute(cmd_ILWK12, CommandType.Text, transaction).Result;
                    int res_12 = res_ILWK12.afrows;

                    if (res_12 == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        
                        
                        lblMsgEN.Text = "Save not complete";
                        PopupMsg_ver.ShowOnPageLoad = true;
                        return;
                    }
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTE_IN != null)
                {
                    getDataTE_IN(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
            else if (hid_status.Value == "KESSAI")
            {
                w5eaud = AppDate;
                w5eaut = m_UpdTime.ToString();
                w5eaus = userInfo.Username.ToString();
                w5eauw = userInfo.LocalClient.ToString();
                // ***  check ก่อนว่า Interview ทำการบันทึกข้อมูลหรือยัง ***//
                string cmd_ILWK05 = iLDataCenterMssql.UPDATE_ILWK05_TE_KESSAI(w5vrto, w5eaud, w5eaut, w5eaus,
                                                                 w5eauw, w5updt, w5uptm, w5user,
                                                                 w5prgm, w5wrks, w5brn, w5apno);
                bool transaction = dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ILWK05 = dataCenter.Execute(cmd_ILWK05, CommandType.Text, transaction).Result;
                int res_udp = res_ILWK05.afrows;
                if (res_udp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    
                    
                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_ver.ShowOnPageLoad = true;
                    return;
                }
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed";
                if (getDataTE_KE != null)
                {
                    getDataTE_KE(w5user);
                }
                PopupMsg_ver.ShowOnPageLoad = true;
                return;

            }


        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }

    private void Save_cust()
    {
        try
        {
            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            if (hid_status.Value == "INTERVIEW")
            {
                //**  check data in ilwk05 **//
                DataSet ds_05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
                if (!ilObj.check_dataset(ds_05))
                {
                    insertCustomer();
                }
                else
                {
                    updateCustomer();
                }
            }
            else if (hid_status.Value == "KESSAI")
            {
                updateCustomer();
            }


        }
        catch (Exception ex)
        {
        }
    }
    private bool validateTO(ref string err)
    {
        bool res = true;
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds_wk05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            if (!ilObj.check_dataset(ds_wk05))
            {
                err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
                res = false;
            }


            DataSet ds = iLDataCenterMssql.getRLtb71("99", "", "", "Y").Result;
            if (ilObj.check_dataset(ds))
            {
                //DataRow dr = ds.Tables[0].Rows[0];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["t71cd1"].ToString().Trim() == "01")
                    {
                        if (dd_recieveMoney.Value == null)
                        {
                            err += "กรุณาระบุ 01. ลักษณะการรับเงินเดือน" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "02")
                    {
                        if (txt_salary_TO.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 02. รายได้ต่อเดือน" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "03")
                    {
                        if (dd_sso.ToString() == "")
                        {
                            err += "กรุณาระบุ 03. ผลการเช็คประกันสังคม (SSO)" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "04")
                    {
                        if (dd_BOL.Value == null)
                        {
                            err += "กรุณาระบุ 04. ผลการเช็ค BOL" + "\r\n" + "\r\n";
                            res = false;
                        }
                    }
                    //if (dr["t71cd1"].ToString().Trim() == "07")
                    //{
                    //    if (dd_1133.Value ==  null)
                    //    {
                    //        err += "กรุณาระบุ 07. วิธีการตรวจสอบ 1133/Internet" + "\r\n";
                    //        res = false;
                    //    }
                    //}
                    if (dr["t71cd1"].ToString().Trim() == "08")
                    {
                        if (dd_chkName.Value == null)
                        {
                            err += "กรุณาระบุ 08. ผลการตรวจสอบ (ชื่อ)" + "\r\n";
                            res = false;
                        }
                        if (dd_chkName.Value.ToString() == "02" && txt_res_name_TO.Text.Trim() == "")
                        {
                            err += "กรุณาระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                            res = false;
                        }
                        if (dd_chkName.Value.ToString() != "02" && txt_res_name_TO.Text.Trim() != "")
                        {
                            err += "ไม่ต้องระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "09")
                    {
                        if (dd_chkAddr.Value == null)
                        {
                            err += "กรุณาระบุ 09. ผลการตรวจสอบ (ที่อยู่)" + "\r\n";
                            res = false;
                        }
                        if (dd_chkAddr.Value.ToString() == "02" && txt_res_addr_TO.Text.Trim() == "")
                        {
                            err += "กรุณาระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                            res = false;
                        }
                        if (dd_chkAddr.Value.ToString() != "02" && txt_res_addr_TO.Text.Trim() != "")
                        {
                            err += "ไม่ต้องระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "10")
                    {
                        if (dd_statusEmp.Value == null)
                        {
                            err += "กรุณาระบุ 10. สถานภาพการเป็นพนักงาน" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "11")
                    {
                        if (dd_res_contact_TO.Value == null)
                        {
                            err += "กรุณาระบุ 11. ผลการติดต่อ TO" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "12")
                    {
                        if (dd_person_to.Value == null || txt_person_to.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 12. ผู้ให้ข้อมูล (TO)" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "13")
                    {
                        if (dd_empType.Value == null)
                        {
                            err += "กรุณาระบุ 13. ประเภทการจ้างงาน" + "\r\n";

                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "14")
                    {
                        if (txt_name_support.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 14. ผู้มีอำนาจลงนามหนังสือรับรอง" + "\r\n";
                            res = false;
                        }
                    }
                }
            }

            if ((txt_off_TO.Text.Trim() == "") && (txt_brn_tel.Text.Trim() == "") && (txt_tel_off_mobil.Text.Trim() == ""))
            {
                err += "กรุณาระบุ 05 หรือ 06 หรือ 35 เบอร์ที่ติดต่อที่ทำงานของลูกค้าอย่างน้อย 1 เบอร์" + "\r\n";
                res = false;
            }

            if (dd_chkName.Value != null)
            {

                if (dd_chkName.Value.ToString() == "02" && txt_res_name_TO.Text.Trim() == "")
                {
                    err += "กรุณาระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                    res = false;
                }
                if (dd_chkName.Value.ToString() != "02" && txt_res_name_TO.Text.Trim() != "")
                {
                    err += "ไม่ต้องระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                    res = false;
                }
            }
            if (dd_chkAddr.Value != null)
            {
                if (dd_chkAddr.Value.ToString() == "02" && txt_res_addr_TO.Text.Trim() == "")
                {
                    err += "กรุณาระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                    res = false;
                }
                if (dd_chkAddr.Value.ToString() != "02" && txt_res_addr_TO.Text.Trim() != "")
                {
                    err += "ไม่ต้องระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                    res = false;
                }
            }

            ilObj.UserInfomation = userInfo;
            iLDataSubroutine = new ILDataSubroutine(userInfo);
            string prmError = "";
            string prmErrorMsg = "";
            string prmOTYPE = "";

            if (txt_off_TO.Text.Trim() != "")
            {
                bool res48_off = iLDataSubroutine.Call_GNSR48(txt_off_TO.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_off || prmError == "Y")
                {
                    err += (prmErrorMsg + " เบอร์ติดต่อที่ทำงาน(สนญ.)" + "\r\n");
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์ติดต่อที่ทำงาน(สนญ.) รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }

            }
            else
            {
                bool chk_ext = ilObj.checkTelExt(txt_off_TO.Text.Trim(), txt_tel_off_ext_TO.Text.Trim());
                if (!chk_ext)
                {
                    err += (" เบอร์ติดต่อที่ทำงาน(สนญ.) Ext. ไม่ถูกต้อง" + "\r\n");
                    res = false;
                }
            }

            prmError = "";
            prmErrorMsg = "";

            if (txt_brn_tel.Text.Trim() != "")
            {
                bool res48_brn = iLDataSubroutine.Call_GNSR48(txt_brn_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_brn || prmError == "Y")
                {
                    err += (prmErrorMsg + " เบอร์ติดต่อสาขาที่ประจำ" + "\r\n");
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์ติดต่อสาขาที่ประจำ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            else
            {
                bool chk_ext = ilObj.checkTelExt(txt_brn_tel.Text.Trim(), txt_tel_ext_brn.Text.Trim());
                if (!chk_ext)
                {
                    err += " เบอร์ติดต่อสาขาที่ประจำ Ext. ไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }

            prmError = "";
            prmErrorMsg = "";
            if (txt_tel_off_mobil.Text.Trim() != "")
            {
                bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_tel_off_mobil.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_mobile || prmError == "Y")
                {
                    err += (prmErrorMsg + " เบอร์มือถือ " + "\r\n");
                    res = false;
                }
                if (prmOTYPE != "M")
                {
                    err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            if (txt_off_TO.Text.Trim() != "" && txt_tel_off2_TO.Text.Trim() != "")
            {
                if (!checkTelTo(txt_off_TO.Text.Trim(), txt_tel_off2_TO.Text.Trim()))
                {
                    err += ("เบอร์ถึง 5. เบอร์ติดต่อที่ทำงาน(สนญ.) ต้องไม่มากกว่า " + txt_tel_off2_TO.Text.Trim() + "\r\n");
                    res = false;
                }
            }
            if (txt_brn_tel.Text.Trim() != "" && txt_tel2_brn.Text.Trim() != "")
            {
                if (!checkTelTo(txt_brn_tel.Text.Trim(), txt_tel2_brn.Text.Trim()))
                {
                    err += ("เบอร์ถึง 6. เบอร์ติดต่อสาขาที่ประจำ ต้องไม่มากกว่า " + txt_brn_tel.Text.Trim() + "\r\n");
                    res = false;
                }
            }

            return res;

        }
        catch (Exception ex)
        {
            err += " Validate error ";
            

            return false;
        }
    }

    //private async Task<ResValidate> validateTO2()
    //{
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {
    //        ilObj = new ILDataCenter();
    //        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
    //        iLDataSubroutine = new ILDataSubroutine(userInfo);
    //        DataSet ds_wk05 = await iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value);
    //        if (!ilObj.check_dataset(ds_wk05))
    //        {
    //            err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
    //            res = false;
    //        }


    //        DataSet ds = await iLDataCenterMssql.getRLtb71("99", "", "", "Y");
    //        if (ilObj.check_dataset(ds))
    //        {
    //            //DataRow dr = ds.Tables[0].Rows[0];
    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                if (dr["t71cd1"].ToString().Trim() == "01")
    //                {
    //                    if (dd_recieveMoney.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 01. ลักษณะการรับเงินเดือน" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "02")
    //                {
    //                    if (txt_salary_TO.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 02. รายได้ต่อเดือน" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "03")
    //                {
    //                    if (dd_sso.ToString() == "")
    //                    {
    //                        err += "กรุณาระบุ 03. ผลการเช็คประกันสังคม (SSO)" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "04")
    //                {
    //                    if (dd_BOL.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 04. ผลการเช็ค BOL" + "\r\n" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                //if (dr["t71cd1"].ToString().Trim() == "07")
    //                //{
    //                //    if (dd_1133.Value ==  null)
    //                //    {
    //                //        err += "กรุณาระบุ 07. วิธีการตรวจสอบ 1133/Internet" + "\r\n";
    //                //        res = false;
    //                //    }
    //                //}
    //                if (dr["t71cd1"].ToString().Trim() == "08")
    //                {
    //                    if (dd_chkName.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 08. ผลการตรวจสอบ (ชื่อ)" + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkName.Value.ToString() == "02" && txt_res_name_TO.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkName.Value.ToString() != "02" && txt_res_name_TO.Text.Trim() != "")
    //                    {
    //                        err += "ไม่ต้องระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "09")
    //                {
    //                    if (dd_chkAddr.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 09. ผลการตรวจสอบ (ที่อยู่)" + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkAddr.Value.ToString() == "02" && txt_res_addr_TO.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkAddr.Value.ToString() != "02" && txt_res_addr_TO.Text.Trim() != "")
    //                    {
    //                        err += "ไม่ต้องระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "10")
    //                {
    //                    if (dd_statusEmp.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 10. สถานภาพการเป็นพนักงาน" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "11")
    //                {
    //                    if (dd_res_contact_TO.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 11. ผลการติดต่อ TO" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "12")
    //                {
    //                    if (dd_person_to.Value == null || txt_person_to.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 12. ผู้ให้ข้อมูล (TO)" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "13")
    //                {
    //                    if (dd_empType.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 13. ประเภทการจ้างงาน" + "\r\n";

    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "14")
    //                {
    //                    if (txt_name_support.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 14. ผู้มีอำนาจลงนามหนังสือรับรอง" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //            }
    //        }

    //        if ((txt_off_TO.Text.Trim() == "") && (txt_brn_tel.Text.Trim() == "") && (txt_tel_off_mobil.Text.Trim() == ""))
    //        {
    //            err += "กรุณาระบุ 05 หรือ 06 หรือ 35 เบอร์ที่ติดต่อที่ทำงานของลูกค้าอย่างน้อย 1 เบอร์" + "\r\n";
    //            res = false;
    //        }

    //        if (dd_chkName.Value != null)
    //        {

    //            if (dd_chkName.Value.ToString() == "02" && txt_res_name_TO.Text.Trim() == "")
    //            {
    //                err += "กรุณาระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                res = false;
    //            }
    //            if (dd_chkName.Value.ToString() != "02" && txt_res_name_TO.Text.Trim() != "")
    //            {
    //                err += "ไม่ต้องระบุข้อมูล 08.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                res = false;
    //            }
    //        }
    //        if (dd_chkAddr.Value != null)
    //        {
    //            if (dd_chkAddr.Value.ToString() == "02" && txt_res_addr_TO.Text.Trim() == "")
    //            {
    //                err += "กรุณาระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                res = false;
    //            }
    //            if (dd_chkAddr.Value.ToString() != "02" && txt_res_addr_TO.Text.Trim() != "")
    //            {
    //                err += "ไม่ต้องระบุข้อมูล 09.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                res = false;
    //            }
    //        }

    //        ilObj.UserInfomation = userInfo;
    //        string prmError = "";
    //        string prmErrorMsg = "";
    //        string prmOTYPE = "";

    //        if (txt_off_TO.Text.Trim() != "")
    //        {
    //            bool res48_off = iLDataSubroutine.Call_GNSR48(txt_off_TO.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_off || prmError == "Y")
    //            {
    //                err += (prmErrorMsg + " เบอร์ติดต่อที่ทำงาน(สนญ.)" + "\r\n");
    //                res = false;
    //            }
    //            if (prmOTYPE != "P")
    //            {
    //                err += "เบอร์ติดต่อที่ทำงาน(สนญ.) รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }

    //        }
    //        else
    //        {
    //            bool chk_ext = ilObj.checkTelExt(txt_off_TO.Text.Trim(), txt_tel_off_ext_TO.Text.Trim());
    //            if (!chk_ext)
    //            {
    //                err += (" เบอร์ติดต่อที่ทำงาน(สนญ.) Ext. ไม่ถูกต้อง" + "\r\n");
    //                res = false;
    //            }
    //        }

    //        prmError = "";
    //        prmErrorMsg = "";

    //        if (txt_brn_tel.Text.Trim() != "")
    //        {
    //            bool res48_brn = iLDataSubroutine.Call_GNSR48(txt_brn_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_brn || prmError == "Y")
    //            {
    //                err += (prmErrorMsg + " เบอร์ติดต่อสาขาที่ประจำ" + "\r\n");
    //                res = false;
    //            }
    //            if (prmOTYPE != "P")
    //            {
    //                err += "เบอร์ติดต่อสาขาที่ประจำ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        else
    //        {
    //            bool chk_ext = ilObj.checkTelExt(txt_brn_tel.Text.Trim(), txt_tel_ext_brn.Text.Trim());
    //            if (!chk_ext)
    //            {
    //                err += " เบอร์ติดต่อสาขาที่ประจำ Ext. ไม่ถูกต้อง" + "\r\n";
    //                res = false;
    //            }
    //        }

    //        prmError = "";
    //        prmErrorMsg = "";
    //        if (txt_tel_off_mobil.Text.Trim() != "")
    //        {
    //            bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_tel_off_mobil.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_mobile || prmError == "Y")
    //            {
    //                err += (prmErrorMsg + " เบอร์มือถือ " + "\r\n");
    //                res = false;
    //            }
    //            if (prmOTYPE != "M")
    //            {
    //                err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        if (txt_off_TO.Text.Trim() != "" && txt_tel_off2_TO.Text.Trim() != "")
    //        {
    //            if (!checkTelTo(txt_off_TO.Text.Trim(), txt_tel_off2_TO.Text.Trim()))
    //            {
    //                err += ("เบอร์ถึง 5. เบอร์ติดต่อที่ทำงาน(สนญ.) ต้องไม่มากกว่า " + txt_tel_off2_TO.Text.Trim() + "\r\n");
    //                res = false;
    //            }
    //        }
    //        if (txt_brn_tel.Text.Trim() != "" && txt_tel2_brn.Text.Trim() != "")
    //        {
    //            if (!checkTelTo(txt_brn_tel.Text.Trim(), txt_tel2_brn.Text.Trim()))
    //            {
    //                err += ("เบอร์ถึง 6. เบอร์ติดต่อสาขาที่ประจำ ต้องไม่มากกว่า " + txt_brn_tel.Text.Trim() + "\r\n");
    //                res = false;
    //            }
    //        }
    //        result.Status = res;
    //        result.ErrorMsg = err;
    //        return result;

    //    }
    //    catch (Exception ex)
    //    {
    //        err += " Validate error ";
            
    //        result.ErrorMsg = err;
    //        return result;
    //    }
    //}

    private bool validateTH(ref string err)
    {
        bool res = true;
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds_wk05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            if (!ilObj.check_dataset(ds_wk05))
            {
                err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
                res = false;
            }


            DataSet ds = iLDataCenterMssql.getRLtb71("99", "", "", "Y").Result;
            if (ilObj.check_dataset(ds))
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["t71cd1"].ToString().Trim() == "15")
                    {
                        if (txt_tel_TH.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 15. เบอร์ติดต่อที่อยู่ปัจจุบัน" + "\r\n";
                            res = false;
                        }
                    }
                    //if (dr["t71cd1"].ToString().Trim() == "16")
                    //{
                    //    if (dd_1133_TH.Value == null)
                    //    {
                    //        err += "กรุณาระบุ 16.วิธีการตรวจสอบ 1133/Internet" + "\r\n";
                    //        res = false;
                    //    }
                    //}
                    if (dr["t71cd1"].ToString().Trim() == "17")
                    {
                        if (dd_chkName_TH.Value == null)
                        {
                            err += "กรุณาระบุ 17. ผลการตรวจสอบ (ชื่อ)" + "\r\n";
                            res = false;
                        }
                        if (dd_chkName_TH.Value.ToString() == "02" && txt_res_name_TH.Text.Trim() == "")
                        {
                            err += "กรุณาระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                            res = false;
                        }
                        if (dd_chkName_TH.Value.ToString() != "02" && txt_res_name_TH.Text.Trim() != "")
                        {
                            err += "ไม่ต้องระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                            res = false;
                        }

                    }


                    if (dr["t71cd1"].ToString().Trim() == "18")
                    {
                        if (dd_chk_addr_TH.Value == null)
                        {
                            err += "กรุณาระบุ 18. ผลการตรวจสอบ (ที่อยู่)" + "\r\n";
                            res = false;
                        }
                        if (dd_chk_addr_TH.Value.ToString() == "02" && txt_res_addr_TH.Text.Trim() == "")
                        {
                            err += "กรุณาระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                            res = false;
                        }
                        if (dd_chk_addr_TH.Value.ToString() != "02" && txt_res_addr_TH.Text.Trim() != "")
                        {
                            err += "ไม่ต้องระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "19")
                    {
                        if (dd_type_tel.Value == null)
                        {
                            err += "กรุณาระบุ 19. ลักษณะเบอร์โทร" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "20")
                    {
                        if (dd_resContact_TH.Value == null)
                        {
                            err += "กรุณาระบุ 20. ผลการติดต่อ TH" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "21")
                    {
                        if (dd_person_TH.Value == null || txt_dd_person_TH.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
                            res = false;
                        }
                    }

                    //if (dd_person_TH.Value != null && txt_dd_person_TH.Text.Trim() == "")
                    //{
                    //    err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
                    //    res = false;
                    //}

                }
                if (dd_person_TH.Value != null && txt_dd_person_TH.Text.Trim() == "")
                {
                    err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
                    res = false;
                }
                if (dd_chkName_TH.Value != null)
                {
                    if (dd_chkName_TH.Value.ToString() == "02" && txt_res_name_TH.Text.Trim() == "")
                    {
                        err += "กรุณาระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                        res = false;
                    }
                    if (dd_chkName_TH.Value.ToString() != "02" && txt_res_name_TH.Text.Trim() != "")
                    {
                        err += "ไม่ต้องระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
                        res = false;
                    }
                }

                if (dd_chk_addr_TH.Value != null)
                {
                    if (dd_chk_addr_TH.Value.ToString() == "02" && txt_res_addr_TH.Text.Trim() == "")
                    {
                        err += "กรุณาระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                        res = false;
                    }
                    if (dd_chk_addr_TH.Value.ToString() != "02" && txt_res_addr_TH.Text.Trim() != "")
                    {
                        err += "ไม่ต้องระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
                        res = false;
                    }
                }

                ilObj.UserInfomation = userInfo;
                iLDataSubroutine = new ILDataSubroutine(userInfo);
                string prmError = "";
                string prmErrorMsg = "";
                string prmOTYPE = "";
                bool res48_TH = iLDataSubroutine.Call_GNSR48(txt_tel_TH.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (prmOTYPE != "P")
                {
                    err += "เบอร์ติดต่อที่อยู่ปัจจุบัน รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }

                if (!res48_TH || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n";
                    res = false;
                }

                else
                {
                    bool chk_ext = ilObj.checkTelExt(txt_tel_TH.Text.Trim(), txt_tel_ext_TH.Text.Trim());
                    if (!chk_ext)
                    {
                        err += " เบอร์ติดต่อที่อยู่ปัจจุบัน Ext. ไม่ถูกต้อง" + "\r\n";
                        res = false;
                    }
                }


                if (txt_tel_TH.Text.Trim() != "" && txt_tel_2_TH.Text.Trim() != "")
                {
                    if (!checkTelTo(txt_tel_TH.Text.Trim(), txt_tel_2_TH.Text.Trim()))
                    {
                        err += "15. เบอร์ติดต่อที่อยู่ปัจจุบัน ต้องไม่มากกว่า " + txt_tel_TH.Text.Trim() + "\r\n";
                        res = false;
                    }
                }



                
                return res;
            }
            err += "Validate error";
            
            return false;

        }
        catch (Exception ex)
        {
            err += "Validate error";
            return false;
        }
    }

    //private ResValidate validateTH2()
    //{
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {
    //        ilObj = new ILDataCenter();
    //        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
    //        iLDataSubroutine = new ILDataSubroutine(userInfo);
    //        DataSet ds_wk05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
    //        if (!ilObj.check_dataset(ds_wk05))
    //        {
    //            err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
    //            res = false;
    //        }


    //        DataSet ds = iLDataCenterMssql.getRLtb71("99", "", "", "Y").Result;
    //        if (ilObj.check_dataset(ds))
    //        {

    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                if (dr["t71cd1"].ToString().Trim() == "15")
    //                {
    //                    if (txt_tel_TH.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 15. เบอร์ติดต่อที่อยู่ปัจจุบัน" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                //if (dr["t71cd1"].ToString().Trim() == "16")
    //                //{
    //                //    if (dd_1133_TH.Value == null)
    //                //    {
    //                //        err += "กรุณาระบุ 16.วิธีการตรวจสอบ 1133/Internet" + "\r\n";
    //                //        res = false;
    //                //    }
    //                //}
    //                if (dr["t71cd1"].ToString().Trim() == "17")
    //                {
    //                    if (dd_chkName_TH.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 17. ผลการตรวจสอบ (ชื่อ)" + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkName_TH.Value.ToString() == "02" && txt_res_name_TH.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chkName_TH.Value.ToString() != "02" && txt_res_name_TH.Text.Trim() != "")
    //                    {
    //                        err += "ไม่ต้องระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                        res = false;
    //                    }

    //                }


    //                if (dr["t71cd1"].ToString().Trim() == "18")
    //                {
    //                    if (dd_chk_addr_TH.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 18. ผลการตรวจสอบ (ที่อยู่)" + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chk_addr_TH.Value.ToString() == "02" && txt_res_addr_TH.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                        res = false;
    //                    }
    //                    if (dd_chk_addr_TH.Value.ToString() != "02" && txt_res_addr_TH.Text.Trim() != "")
    //                    {
    //                        err += "ไม่ต้องระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "19")
    //                {
    //                    if (dd_type_tel.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 19. ลักษณะเบอร์โทร" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "20")
    //                {
    //                    if (dd_resContact_TH.Value == null)
    //                    {
    //                        err += "กรุณาระบุ 20. ผลการติดต่อ TH" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "21")
    //                {
    //                    if (dd_person_TH.Value == null || txt_dd_person_TH.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
    //                        res = false;
    //                    }
    //                }

    //                //if (dd_person_TH.Value != null && txt_dd_person_TH.Text.Trim() == "")
    //                //{
    //                //    err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
    //                //    res = false;
    //                //}

    //            }
    //            if (dd_person_TH.Value != null && txt_dd_person_TH.Text.Trim() == "")
    //            {
    //                err += "กรุณาระบุ 21. ผู้ให้ข้อมูล (TH)" + "\r\n";
    //                res = false;
    //            }
    //            if (dd_chkName_TH.Value != null)
    //            {
    //                if (dd_chkName_TH.Value.ToString() == "02" && txt_res_name_TH.Text.Trim() == "")
    //                {
    //                    err += "กรุณาระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                    res = false;
    //                }
    //                if (dd_chkName_TH.Value.ToString() != "02" && txt_res_name_TH.Text.Trim() != "")
    //                {
    //                    err += "ไม่ต้องระบุข้อมูล 17.ผลการตรวจสอบ (ชื่อ) " + "\r\n";
    //                    res = false;
    //                }
    //            }

    //            if (dd_chk_addr_TH.Value != null)
    //            {
    //                if (dd_chk_addr_TH.Value.ToString() == "02" && txt_res_addr_TH.Text.Trim() == "")
    //                {
    //                    err += "กรุณาระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                    res = false;
    //                }
    //                if (dd_chk_addr_TH.Value.ToString() != "02" && txt_res_addr_TH.Text.Trim() != "")
    //                {
    //                    err += "ไม่ต้องระบุข้อมูล 18.ผลการตรวจสอบ (ที่อยู่) " + "\r\n";
    //                    res = false;
    //                }
    //            }

    //            ilObj.UserInfomation = userInfo;
    //            string prmError = "";
    //            string prmErrorMsg = "";
    //            string prmOTYPE = "";
    //            bool res48_TH = iLDataSubroutine.Call_GNSR48(txt_tel_TH.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (prmOTYPE != "P")
    //            {
    //                err += "เบอร์ติดต่อที่อยู่ปัจจุบัน รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }

    //            if (!res48_TH || prmError == "Y")
    //            {
    //                err += prmErrorMsg + "\r\n";
    //                res = false;
    //            }

    //            else
    //            {
    //                bool chk_ext = ilObj.checkTelExt(txt_tel_TH.Text.Trim(), txt_tel_ext_TH.Text.Trim());
    //                if (!chk_ext)
    //                {
    //                    err += " เบอร์ติดต่อที่อยู่ปัจจุบัน Ext. ไม่ถูกต้อง" + "\r\n";
    //                    res = false;
    //                }
    //            }


    //            if (txt_tel_TH.Text.Trim() != "" && txt_tel_2_TH.Text.Trim() != "")
    //            {
    //                if (!checkTelTo(txt_tel_TH.Text.Trim(), txt_tel_2_TH.Text.Trim()))
    //                {
    //                    err += "15. เบอร์ติดต่อที่อยู่ปัจจุบัน ต้องไม่มากกว่า " + txt_tel_TH.Text.Trim() + "\r\n";
    //                    res = false;
    //                }
    //            }



                
    //            result.ErrorMsg = err;
    //            result.Status = res;
    //            return result;
    //        }
    //        err += "Validate error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
            
    //        return result;

    //    }
    //    catch (Exception ex)
    //    {
    //        err += "Validate error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //}

    private bool validateTM(ref string err)
    {
        bool res = true;
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            iLDataSubroutine = new ILDataSubroutine(userInfo);
            DataSet ds_wk05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            if (!ilObj.check_dataset(ds_wk05))
            {
                err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
                res = false;
            }

            if (hid_status.Value == "INTERVIEW")
            {
                if (cb_check_TM.Checked == false)
                {
                    if ((txt_mobile_TM_P.Text.Trim() == "" || txt_mobile_TM.Text.Trim() == ""))
                    {
                        err += "กรุณาระบุ 22. เบอร์มือถือ" + "\r\n";
                        res = false;
                    }
                }

                if (txt_mobile_TM_P.Text.Trim() != txt_mobile_TM.Text.Trim())
                {
                    err += " 22. เบอร์มือถือ  ไม่ตรงกัน กรุณาระบุอีกครั้ง " + "\r\n";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "script2", "txt_mobile_TM_P.SetValue('')", true);
                    txt_mobile_TM.Text = "";
                    res = false;
                }
            }

            DataSet ds = iLDataCenterMssql.getRLtb71("99", "", "", "Y").Result;
            if (ilObj.check_dataset(ds))
            {
                //DataRow dr = ds.Tables[0].Rows[0];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["t71cd1"].ToString().Trim() == "22")
                    {


                        if (txt_mobile_TM.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 22. เบอร์มือถือ" + "\r\n";
                            res = false;
                        }
                    }
                    if (dr["t71cd1"].ToString().Trim() == "23")
                    {
                        if (txt_salary_TO.Text.Trim() == "")
                        {
                            err += "กรุณาระบุ 23. ผลการติดต่อ TM" + "\r\n";
                            res = false;
                        }
                    }
                }
                if (txt_mobile_TM.Text.Trim() != "")
                {
                    ilObj.UserInfomation = userInfo;
                    string prmError = "";
                    string prmErrorMsg = "";
                    string prmOTYPE = "";
                    bool res48_TM = iLDataSubroutine.Call_GNSR48(txt_mobile_TM.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                    
                    if (!res48_TM || prmError == "Y")
                    {
                        err += prmErrorMsg + "\r\n";
                        res = false;
                    }
                    if (prmOTYPE != "M")
                    {
                        err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                        res = false;
                    }
                }


                return res;
            }
            err = "Validate error";
            
            return false;


        }
        catch (Exception ex)
        {
            err += "Validate error";
            return false;
        }
    }

    //private async Task<ResValidate> validateTM2()
    //{
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {
    //        ilObj = new ILDataCenter();
    //        iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
    //        iLDataSubroutine = new ILDataSubroutine(userInfo);
    //        DataSet ds_wk05 = await iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value);
    //        if (!ilObj.check_dataset(ds_wk05))
    //        {
    //            err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
    //            res = false;
    //        }

    //        if (hid_status.Value == "INTERVIEW")
    //        {
    //            if (cb_check_TM.Checked == false)
    //            {
    //                if ((txt_mobile_TM_P.Text.Trim() == "" || txt_mobile_TM.Text.Trim() == ""))
    //                {
    //                    err += "กรุณาระบุ 22. เบอร์มือถือ" + "\r\n";
    //                    res = false;
    //                }
    //            }

    //            if (txt_mobile_TM_P.Text.Trim() != txt_mobile_TM.Text.Trim())
    //            {
    //                err += " 22. เบอร์มือถือ  ไม่ตรงกัน กรุณาระบุอีกครั้ง " + "\r\n";
    //                ScriptManager.RegisterStartupScript(this, typeof(string), "script2", "txt_mobile_TM_P.SetValue('')", true);
    //                txt_mobile_TM.Text = "";
    //                res = false;
    //            }
    //        }

    //        DataSet ds = await iLDataCenterMssql.getRLtb71("99", "", "", "Y");
    //        if (ilObj.check_dataset(ds))
    //        {
    //            //DataRow dr = ds.Tables[0].Rows[0];
    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                if (dr["t71cd1"].ToString().Trim() == "22")
    //                {


    //                    if (txt_mobile_TM.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 22. เบอร์มือถือ" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //                if (dr["t71cd1"].ToString().Trim() == "23")
    //                {
    //                    if (txt_salary_TO.Text.Trim() == "")
    //                    {
    //                        err += "กรุณาระบุ 23. ผลการติดต่อ TM" + "\r\n";
    //                        res = false;
    //                    }
    //                }
    //            }
    //            if (txt_mobile_TM.Text.Trim() != "")
    //            {
    //                ilObj.UserInfomation = userInfo;
    //                string prmError = "";
    //                string prmErrorMsg = "";
    //                string prmOTYPE = "";
    //                bool res48_TM = iLDataSubroutine.Call_GNSR48(txt_mobile_TM.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                    
    //                if (!res48_TM || prmError == "Y")
    //                {
    //                    err += prmErrorMsg + "\r\n";
    //                    res = false;
    //                }
    //                if (prmOTYPE != "M")
    //                {
    //                    err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                    res = false;
    //                }
    //            }

    //            result.ErrorMsg = err;
    //            result.Status = res;
    //            return result;
    //        }
    //        err = "Validate error";
            
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;


    //    }
    //    catch (Exception ex)
    //    {
    //        err += "Validate error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //}

    private bool validateTE(ref string err)
    {
        bool res = true;
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfo);
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            DataSet ds_wk05 = iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            if (!ilObj.check_dataset(ds_wk05))
            {
                err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
                res = false;
            }
            if (hid_status.Value == "KESSAI")
            {
                DataRow dr = ds_wk05.Tables[0].Rows[0];
                if (dr["w5eusr"].ToString().Trim() == "")
                {
                    err += " Interview ยังไม่ได้ Save Data ในส่วนของ TE ไม่สามารถ Confirm ได้ " + "\r\n";
                    res = false;
                }
            }

            if (gvTE.Rows.Count == 0)
            {
                err += " กรุณาระบุบุคคลอ้างอิงก่อน " + "\r\n";
                res = false;
            }


            for (int rowNum = 0; rowNum < gvTE.Rows.Count; rowNum++)
            {
                Label seq = (Label)gvTE.Rows[rowNum].FindControl("lb_Seq");
                Label lb_rel = (Label)gvTE.Rows[rowNum].FindControl("lb_relation");
                Label lb_name = (Label)gvTE.Rows[rowNum].FindControl("lb_name");
                Label lb_surname = (Label)gvTE.Rows[rowNum].FindControl("lb_surname");
                Label lb_tel_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_1");
                Label lb_To = (Label)gvTE.Rows[rowNum].FindControl("lb_To");
                Label lb_ext_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_1");
                Label lb_tel_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_2");
                Label lb_To2 = (Label)gvTE.Rows[rowNum].FindControl("lb_To2");
                Label lb_ext_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_2");
                Label lb_Mobile = (Label)gvTE.Rows[rowNum].FindControl("lb_Mobile");
                Label lb_Verify = (Label)gvTE.Rows[rowNum].FindControl("lb_Verify");

                if (lb_rel.Text.Trim() == "" || lb_name.Text.Trim() == "" || lb_name.Text.Trim() == "" || lb_surname.Text.Trim() == "" ||
                    (lb_tel_1.Text.Trim() == "" && lb_Mobile.Text.Trim() == "") || lb_Verify.Text.Trim() == "")
                {
                    err += "กรุณาใส่ ข้อมูล Verify TE Flag ให้ครบถ้วน" + "\r\n";
                    res = false;
                }
            }

            
            return res;

        }
        catch (Exception ex)
        {
            err += "Validate error";
            return false;
        }
    }

    //private async Task<ResValidate> validateTE2()
    //{
    //    iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {
    //        DataSet ds_wk05 = await iLDataCenterMssql.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value);
    //        if (!ilObj.check_dataset(ds_wk05))
    //        {
    //            err += " กรุณาระบุ Customer type ก่อน " + "\r\n";
    //            res = false;
    //        }
    //        if (hid_status.Value == "KESSAI")
    //        {
    //            DataRow dr = ds_wk05.Tables[0].Rows[0];
    //            if (dr["w5eusr"].ToString().Trim() == "")
    //            {
    //                err += " Interview ยังไม่ได้ Save Data ในส่วนของ TE ไม่สามารถ Confirm ได้ " + "\r\n";
    //                res = false;
    //            }
    //        }

    //        if (gvTE.Rows.Count == 0)
    //        {
    //            err += " กรุณาระบุบุคคลอ้างอิงก่อน " + "\r\n";
    //            res = false;
    //        }


    //        for (int rowNum = 0; rowNum < gvTE.Rows.Count; rowNum++)
    //        {
    //            Label seq = (Label)gvTE.Rows[rowNum].FindControl("lb_Seq");
    //            Label lb_rel = (Label)gvTE.Rows[rowNum].FindControl("lb_relation");
    //            Label lb_name = (Label)gvTE.Rows[rowNum].FindControl("lb_name");
    //            Label lb_surname = (Label)gvTE.Rows[rowNum].FindControl("lb_surname");
    //            Label lb_tel_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_1");
    //            Label lb_To = (Label)gvTE.Rows[rowNum].FindControl("lb_To");
    //            Label lb_ext_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_1");
    //            Label lb_tel_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_2");
    //            Label lb_To2 = (Label)gvTE.Rows[rowNum].FindControl("lb_To2");
    //            Label lb_ext_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_2");
    //            Label lb_Mobile = (Label)gvTE.Rows[rowNum].FindControl("lb_Mobile");
    //            Label lb_Verify = (Label)gvTE.Rows[rowNum].FindControl("lb_Verify");

    //            if (lb_rel.Text.Trim() == "" || lb_name.Text.Trim() == "" || lb_name.Text.Trim() == "" || lb_surname.Text.Trim() == "" ||
    //                (lb_tel_1.Text.Trim() == "" && lb_Mobile.Text.Trim() == "") || lb_Verify.Text.Trim() == "")
    //            {
    //                err += "กรุณาใส่ ข้อมูล Verify TE Flag ให้ครบถ้วน" + "\r\n";
    //                res = false;
    //            }
    //        }

            
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;

    //    }
    //    catch (Exception ex)
    //    {
    //        err += "Validate error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //}
    private bool validateRelation(ref string err)
    {
        bool res = true;
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfo);
            iLDataSubroutine = new ILDataSubroutine(userInfo);
            if (txt_seq.Text.Trim() == "")
            {
                err += "Seq ไม่ถูกต้อง" + "\r\n";
                res = false;
            }
            if (dd_relation.Value == null)
            {
                err += "กรุณาระบุ Relation " + "\r\n";
                res = false;
            }
            if (dd_relation.Value.ToString().Trim() == "")
            {
                err += "กรุณาระบุ Relation " + "\r\n";
                res = false;
            }
            if (txt_Fname.Text.Trim() == "" || txt_Lname.Text.Trim() == "")
            {
                err += "กรุณาระบุ ชื่อและนามสกุล" + "\r\n";
                res = false;
            }
            ilObj = new ILDataCenter();
            ilObj.UserInfomation = userInfo;
            GNSRNM gNSRNM = new GNSRNM();
            string refErr = "";
            string ErrorMsg = "";

            //bool call_GNSRNM_name = iLDataSubroutine.Call_GNSRNM(txt_Fname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
            bool call_GNSRNM_name = gNSRNM.Call_GNSRNM(txt_Fname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);

            if (!call_GNSRNM_name || refErr.ToString().Trim() == "Y")
            {
                err += "กรุณาใส่ชื่อบุคคลอ้างอิงเป็นภาษาไทย" + "\r\n";
                
                res = false;
            }
            refErr = "";
            ErrorMsg = "";
            //Check SurName Thai
            //bool call_GNSRNM_sur = iLDataSubroutine.Call_GNSRNM(txt_Lname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
            bool call_GNSRNM_sur = gNSRNM.Call_GNSRNM(txt_Lname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);

            if (!call_GNSRNM_sur || refErr.ToString().Trim() == "Y")
            {
                err += "กรุณาใส่นามสกุลบุคคลอ้างอิงเป็นภาษาไทย" + "\r\n";
                
                res = false;
            }


            if (dd_verTE.Value == null)
            {
                err += "กรุณาระบุ Verify flag" + "\r\n";
                res = false;
            }

            if (txt_mobile_TE.Text.Trim() == "" && txt_tel1_TE.Text.Trim() == "")
            {
                err += "กรุณาระบุ Mobile phone หรือ เบอร์โทรที่ 1" + "\r\n";
                res = false;
            }
            if (txt_tel1_TE.Text.Trim() != "" && txt_tel1_to_TE.Text.Trim() != "")
            {
                if (!checkTelTo(txt_tel1_TE.Text.Trim(), txt_tel1_to_TE.Text.Trim()))
                {
                    err += ("เบอร์ติดต่อที่ 1 ต้องไม่มากกว่า " + txt_tel1_TE.Text.Trim() + "\r\n");
                    res = false;
                }
            }
            if (txt_tel2_TE.Text.Trim() != "" && txt_tel2_to_TE.Text.Trim() != "")
            {
                if (!checkTelTo(txt_tel2_TE.Text.Trim(), txt_tel2_to_TE.Text.Trim()))
                {
                    err += "เบอร์ติดต่อที่ 2 ต้องไม่มากกว่า " + txt_tel2_TE.Text.Trim() + "\r\n";
                    res = false;
                }
            }

            // ***  check Mobile ***//

            string prmError = "";
            string prmErrorMsg = "";
            string prmOTYPE = "";
            if (txt_mobile_TE.Text.Trim() != "")
            {
                bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_mobile_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_mobile || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n";
                    res = false;
                }
                if (prmOTYPE != "M")
                {
                    err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            if (txt_tel1_TE.Text.Trim() != "")
            {
                prmError = "";
                prmErrorMsg = "";

                bool res48_tel1 = iLDataSubroutine.Call_GNSR48(txt_tel1_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_tel1 || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n"; ;
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์ Tel1. รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            else
            {
                bool chk_ext = ilObj.checkTelExt(txt_tel1_TE.Text.Trim(), txt_tel1_ext_TE.Text.Trim());
                if (!chk_ext)
                {
                    err += " เบอร์ Tel.1 Ext. ไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            prmError = "";
            prmErrorMsg = "";

            if (txt_tel2_TE.Text.Trim() != "")
            {
                bool res48_tel2 = iLDataSubroutine.Call_GNSR48(txt_tel2_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_tel2 || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n"; ;
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์ Tel2. รูปแบบไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            else
            {
                bool chk_ext = ilObj.checkTelExt(txt_tel2_TE.Text.Trim(), txt_tel2_ext.Text.Trim());
                if (!chk_ext)
                {
                    err += " เบอร์ Tel.2 Ext. ไม่ถูกต้อง" + "\r\n"; ;
                    res = false;
                }
            }
            return res;
        }
        catch (Exception ex)
        {
            err += "Validate  Error";
            return false;
        }
    }

    //private async Task<ResValidate> validateRelation2()
    //{
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {
    //        if (txt_seq.Text.Trim() == "")
    //        {
    //            err += "Seq ไม่ถูกต้อง" + "\r\n";
    //            res = false;
    //        }
    //        if (dd_relation.Value == null)
    //        {
    //            err += "กรุณาระบุ Relation " + "\r\n";
    //            res = false;
    //        }
    //        if (dd_relation.Value.ToString().Trim() == "")
    //        {
    //            err += "กรุณาระบุ Relation " + "\r\n";
    //            res = false;
    //        }
    //        if (txt_Fname.Text.Trim() == "" || txt_Lname.Text.Trim() == "")
    //        {
    //            err += "กรุณาระบุ ชื่อและนามสกุล" + "\r\n";
    //            res = false;
    //        }
    //        ilObj = new ILDataCenter();
    //        iLDataSubroutine = new ILDataSubroutine(userInfo);
    //        ilObj.UserInfomation = userInfo;
    //        string refErr = "";
    //        string ErrorMsg = "";

    //        bool call_GNSRNM_name = iLDataSubroutine.Call_GNSRNM(txt_Fname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
            
    //        if (!call_GNSRNM_name || refErr.ToString().Trim() == "Y")
    //        {
    //            err += "กรุณาใส่ชื่อบุคคลอ้างอิงเป็นภาษาไทย" + "\r\n";
                
    //            res = false;
    //        }
    //        refErr = "";
    //        ErrorMsg = "";
    //        //Check SurName Thai
    //        bool call_GNSRNM_sur = iLDataSubroutine.Call_GNSRNM(txt_Lname.Text.Trim(), "ก", "T", ref refErr, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
            
    //        if (!call_GNSRNM_sur || refErr.ToString().Trim() == "Y")
    //        {
    //            err += "กรุณาใส่นามสกุลบุคคลอ้างอิงเป็นภาษาไทย" + "\r\n";
                
    //            res = false;
    //        }


    //        if (dd_verTE.Value == null)
    //        {
    //            err += "กรุณาระบุ Verify flag" + "\r\n";
    //            res = false;
    //        }

    //        if (txt_mobile_TE.Text.Trim() == "" && txt_tel1_TE.Text.Trim() == "")
    //        {
    //            err += "กรุณาระบุ Mobile phone หรือ เบอร์โทรที่ 1" + "\r\n";
    //            res = false;
    //        }
    //        if (txt_tel1_TE.Text.Trim() != "" && txt_tel1_to_TE.Text.Trim() != "")
    //        {
    //            if (!checkTelTo(txt_tel1_TE.Text.Trim(), txt_tel1_to_TE.Text.Trim()))
    //            {
    //                err += ("เบอร์ติดต่อที่ 1 ต้องไม่มากกว่า " + txt_tel1_TE.Text.Trim() + "\r\n");
    //                res = false;
    //            }
    //        }
    //        if (txt_tel2_TE.Text.Trim() != "" && txt_tel2_to_TE.Text.Trim() != "")
    //        {
    //            if (!checkTelTo(txt_tel2_TE.Text.Trim(), txt_tel2_to_TE.Text.Trim()))
    //            {
    //                err += "เบอร์ติดต่อที่ 2 ต้องไม่มากกว่า " + txt_tel2_TE.Text.Trim() + "\r\n";
    //                res = false;
    //            }
    //        }

    //        // ***  check Mobile ***//

    //        string prmError = "";
    //        string prmErrorMsg = "";
    //        string prmOTYPE = "";
    //        if (txt_mobile_TE.Text.Trim() != "")
    //        {
    //            bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_mobile_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_mobile || prmError == "Y")
    //            {
    //                err += prmErrorMsg + "\r\n";
    //                res = false;
    //            }
    //            if (prmOTYPE != "M")
    //            {
    //                err += "เบอร์มือถือ รูปแบบไม่ถูกต้อง" + "\r\n";
    //                res = false;
    //            }
    //        }
    //        if (txt_tel1_TE.Text.Trim() != "")
    //        {
    //            prmError = "";
    //            prmErrorMsg = "";

    //            bool res48_tel1 = iLDataSubroutine.Call_GNSR48(txt_tel1_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_tel1 || prmError == "Y")
    //            {
    //                err += prmErrorMsg + "\r\n"; ;
    //                res = false;
    //            }
    //            if (prmOTYPE != "P")
    //            {
    //                err += "เบอร์ Tel1. รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        else
    //        {
    //            bool chk_ext = ilObj.checkTelExt(txt_tel1_TE.Text.Trim(), txt_tel1_ext_TE.Text.Trim());
    //            if (!chk_ext)
    //            {
    //                err += " เบอร์ Tel.1 Ext. ไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        prmError = "";
    //        prmErrorMsg = "";

    //        if (txt_tel2_TE.Text.Trim() != "")
    //        {
    //            bool res48_tel2 = iLDataSubroutine.Call_GNSR48(txt_tel2_TE.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
    //            if (!res48_tel2 || prmError == "Y")
    //            {
    //                err += prmErrorMsg + "\r\n"; ;
    //                res = false;
    //            }
    //            if (prmOTYPE != "P")
    //            {
    //                err += "เบอร์ Tel2. รูปแบบไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        else
    //        {
    //            bool chk_ext = ilObj.checkTelExt(txt_tel2_TE.Text.Trim(), txt_tel2_ext.Text.Trim());
    //            if (!chk_ext)
    //            {
    //                err += " เบอร์ Tel.2 Ext. ไม่ถูกต้อง" + "\r\n"; ;
    //                res = false;
    //            }
    //        }
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        err += "Validate  Error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //}

    private bool validate_customerType(ref string err)
    {
        bool res = true;
        try
        {

            if (ddlCompanyType.Value == null || string.IsNullOrEmpty(ddlCompanyType.Value.ToString()))
            {
                err += "กรุณาระบุ Company type" + "\r\n";
                res = false;
            }

            if (dd_cust_type.Value == null || string.IsNullOrEmpty(dd_cust_type.Value.ToString()))
            {
                err += "กรุณาระบุ Customer type" + "\r\n";
                res = false;
            }
            if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value == null || string.IsNullOrEmpty(dd_subType.Value.ToString())))
            {
                err += "กรุณาระบุ Customer sub type" + "\r\n";
                res = false;
            }

            return res;
        }
        catch (Exception ex)
        {
            err += "Validate error";
            return false;
        }
    }
    //private async Task<ResValidate> validate_customerType2()
    //{
    //    ResValidate result = new ResValidate();
    //    bool res = true;
    //    string err = "";
    //    try
    //    {

    //        if ( ddlCompanyType.Value == null || string.IsNullOrEmpty(ddlCompanyType.Value.ToString()) )
    //        {
    //            err += "กรุณาระบุ Company type" + "\r\n";
    //            res = false;
    //        }

    //        if (dd_cust_type.Value == null || string.IsNullOrEmpty(dd_cust_type.Value.ToString()))
    //        {
    //            err += "กรุณาระบุ Customer type" + "\r\n";
    //            res = false;
    //        }
    //        if (dd_cust_type.Value.ToString() == "O" && (dd_subType.Value == null ||  string.IsNullOrEmpty(dd_subType.Value.ToString())))
    //        {
    //            err += "กรุณาระบุ Customer sub type" + "\r\n";
    //            res = false;
    //        }
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        err += "Validate error";
    //        result.ErrorMsg = err;
    //        result.Status = res;
    //        return result;
    //    }
    //}

    private void panelTO(bool enable)
    {

        dd_recieveMoney.Enabled = enable;
        txt_salary_TO.Enabled = enable;
        dd_sso.Enabled = enable;
        dd_BOL.Enabled = enable;
        txt_off_TO.Enabled = enable;
        txt_tel_off2_TO.Enabled = enable;
        txt_tel_off_ext_TO.Enabled = enable;
        txt_brn_tel.Enabled = enable;
        txt_tel2_brn.Enabled = enable;
        txt_tel_ext_brn.Enabled = enable;
        txt_tel_off_mobil.Enabled = enable;
        //dd_1133.Enabled = enable;
        dd_chkName.Enabled = enable;
        if (enable)
        {
            txt_res_name_TO.ReadOnly = false;
            txt_res_addr_TO.ReadOnly = false;
            txt_name_support.ReadOnly = false;
            txt_pos_support.ReadOnly = false;
            txt_dep_support.ReadOnly = false;
        }
        else
        {
            txt_res_name_TO.ReadOnly = true;
            txt_res_addr_TO.ReadOnly = true;
            txt_name_support.ReadOnly = true;
            txt_pos_support.ReadOnly = true;
            txt_dep_support.ReadOnly = true;
        }

        dd_chkAddr.Enabled = enable;

        dd_statusEmp.Enabled = enable;
        dd_res_contact_TO.Enabled = enable;
        dd_person_to.Enabled = enable;
        txt_person_to.Enabled = enable;
        dd_empType.Enabled = enable;



        // btn_saveTO.Enabled = enable;

    }
    private void panelTH(bool enable)
    {
        cb_notHave_TH.Enabled = enable;
        txt_tel_TH.Enabled = enable;
        txt_tel_2_TH.Enabled = enable;
        txt_tel_ext_TH.Enabled = enable;
        //dd_1133_TH.Enabled = enable;
        dd_chkName_TH.Enabled = enable;
        txt_res_name_TH.Enabled = enable;
        dd_chk_addr_TH.Enabled = enable;
        txt_res_addr_TH.Enabled = enable;
        dd_type_tel.Enabled = enable;
        dd_resContact_TH.Enabled = enable;
        dd_person_TH.Enabled = enable;
        txt_dd_person_TH.Enabled = enable;
        // btn_saveTH.Enabled = enable;

    }
    private void panelTM(bool enable)
    {

        cb_check_TM.Enabled = enable;
        txt_mobile_TM_P.Enabled = enable;
        txt_mobile_TM.Enabled = enable;
        dd_resContact_TM.Enabled = enable;
        // btn_saveTM.Enabled = enable;
    }
    private void panelTE(bool enable)
    {

        dd_relation.Enabled = enable;
        txt_Fname.Enabled = enable;
        txt_Lname.Enabled = enable;
        txt_tel1_TE.Enabled = enable;
        txt_tel1_to_TE.Enabled = enable;
        txt_tel1_ext_TE.Enabled = enable;
        txt_tel2_to_TE.Enabled = enable;
        txt_tel2_TE.Enabled = enable;
        txt_tel2_ext.Enabled = enable;
        txt_mobile_TE.Enabled = enable;
        dd_verTE.Enabled = enable;
        btn_insertPerson.Enabled = enable;
        gvTE.Enabled = enable;
        // btn_saveTE.Enabled = enable;
    }

    #endregion

    protected void dd_cust_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        bind_V_SubType();

    }


    protected void btn_saveCust_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            // verify data //
            string err = "";
            if (!validate_customerType(ref err))
            {
                //***  Err ***//
                lblMsgEN.Text = err;

                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
            if (hid_status.Value == "INTERVIEW")
            {

                lblConfirmMsgEN.Text = "Do you want to save customer type";
                hid_oper.Value = "S_cust";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }
            else if (hid_status.Value == "KESSAI")
            {
                lblConfirmMsgEN.Text = "Do you want to confirm customer type";
                hid_oper.Value = "S_cust";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }


        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_saveTH_Click(object sender, EventArgs e)
    {
        lblMsgEN.Text = "";
        lblMsgTH.Text = "";
        lblConfirmMsgEN.Text = "";
        lblConfirmMsgTH.Text = "";
        if (!cb_notHave_TH.Checked)
        {
            string err = "";
            if (!validateTH(ref err))
            {
                //***  Err ***//
                lblMsgEN.Text = err;
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
        }
        if (hid_status.Value == "INTERVIEW")
        {
            lblConfirmMsgEN.Text = "Do you want to save TH";
            hid_oper.Value = "S_TH";
            PopupConfirmSave_ver.ShowOnPageLoad = true;
            return;
        }
        else if (hid_status.Value == "KESSAI")
        {
            lblConfirmMsgEN.Text = "Do you want to confirm TH";
            hid_oper.Value = "S_TH";
            PopupConfirmSave_ver.ShowOnPageLoad = true;
            return;
        }

    }
    protected void btn_saveTM_Click(object sender, EventArgs e)
    {
        lblMsgEN.Text = "";
        lblMsgTH.Text = "";
        lblConfirmMsgEN.Text = "";
        lblConfirmMsgTH.Text = "";
        if (!cb_check_TM.Checked)
        {
            string err = "";
            if (!validateTM(ref err))
            {
                //***  Err ***//
                lblMsgEN.Text = err;
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
        }
        if (hid_status.Value == "INTERVIEW")
        {
            lblConfirmMsgEN.Text = "Do you want to save TM";
            hid_oper.Value = "S_TM";
            PopupConfirmSave_ver.ShowOnPageLoad = true;
            return;
        }
        else if (hid_status.Value == "KESSAI")
        {
            lblConfirmMsgEN.Text = "Do you want to confirm TM";
            hid_oper.Value = "S_TM";
            PopupConfirmSave_ver.ShowOnPageLoad = true;
            return;
        }
    }
    protected void btn_saveTO_Click(object sender, EventArgs e)
    {
        //saveTO();
        try
        {
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            ilObj = new ILDataCenter();
            // verify data //
            string err = "";
            if (!validateTO(ref err))
            {
                //***  Err ***//
                lblMsgEN.Text = err;
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }

            if (hid_status.Value == "INTERVIEW")
            {
                lblConfirmMsgEN.Text = "Do you want to save TO";
                hid_oper.Value = "S_TO";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }
            else if (hid_status.Value == "KESSAI")
            {
                lblConfirmMsgEN.Text = "Do you want to confirm TO";
                hid_oper.Value = "S_TO";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }


        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_saveTE_Click(object sender, EventArgs e)
    {
        //SaveTE();

        try
        {
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            ilObj = new ILDataCenter();
            // verify data //
            string err = "";
            if (!validateTE(ref err))
            {
                //***  Err ***//
                lblMsgEN.Text = err;
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }
            if (hid_status.Value == "INTERVIEW")
            {
                lblConfirmMsgEN.Text = "Do you want to save TE";
                hid_oper.Value = "S_TE";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }
            else if (hid_status.Value == "KESSAI")
            {
                lblConfirmMsgEN.Text = "Do you want to confirm TE";
                hid_oper.Value = "S_TE";
                PopupConfirmSave_ver.ShowOnPageLoad = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_insertPerson_Click(object sender, EventArgs e)
    {
        try
        {
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";

            string err = "";
            if (!validateRelation(ref err))
            {
                lblMsgEN.Text = err;
                PopupMsg_ver.ShowOnPageLoad = true;
                return;
            }

            string[] rel_ = dd_relation.SelectedItem.Text.ToString().Split(':');

            int seq = int.Parse(txt_seq.Text);
            string rel = dd_relation.SelectedItem.Value.ToString();
            string rel_des = rel_[1];
            string fName = txt_Fname.Text.Trim();
            string lName = txt_Lname.Text.Trim();
            string tel_1 = txt_tel1_TE.Text.Trim();
            string tel_1_to = txt_tel1_to_TE.Text.Trim();
            string tel_1_ext = txt_tel1_ext_TE.Text.Trim();
            string tel_2 = txt_tel2_TE.Text.Trim();
            string tel_2_to = txt_tel2_to_TE.Text.Trim();
            string tel_2_ext = txt_tel2_ext.Text.Trim();
            string mobile = txt_mobile_TE.Text.Trim();
            string verify = dd_verTE.SelectedItem.Value.ToString();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (Session["ds_perRel"] != null)
            {
                ds = (DataSet)Session["ds_perRel"];
                if (hid_rowNumSel.Value != "")
                {
                    ds.Tables[0].Rows[int.Parse(hid_rowNumSel.Value)].Delete();
                    hid_rowNumSel.Value = "";
                }
                dt = ds.Tables[0];
                ds.Tables.Remove(dt);
                dt.Rows.Add(seq, rel, fName, lName, tel_1, tel_1_to, tel_1_ext, tel_2, tel_2_to, tel_2_ext, mobile, verify, rel_des);
                DataView dv = dt.DefaultView;
                dv.Sort = "Seq ASC ";
                DataTable dt_sort = dv.ToTable();
                ds.Tables.Add(dt_sort);

                txt_seq.Text = (int.Parse(dt_sort.Rows[dt_sort.Rows.Count - 1]["Seq"].ToString()) + 1).ToString();
                //ds.Tables.Add(dt);
            }
            else
            {

                dt.Columns.Add("Seq", typeof(int));
                dt.Columns.Add("Relation", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("SurName", typeof(string));
                dt.Columns.Add("Tel_1", typeof(string));
                dt.Columns.Add("To_1", typeof(string));
                dt.Columns.Add("Ext_1", typeof(string));
                dt.Columns.Add("Tel_2", typeof(string));
                dt.Columns.Add("To_2", typeof(string));
                dt.Columns.Add("Ext_2", typeof(string));
                dt.Columns.Add("Mobile", typeof(string));
                dt.Columns.Add("Ver", typeof(string));
                dt.Columns.Add("Rel_DES", typeof(string));



                dt.Rows.Add(seq, rel, fName, lName, tel_1, tel_1_to, tel_1_ext, tel_2, tel_2_to, tel_2_ext, mobile, verify, rel_des);
                DataView dv = dt.DefaultView;
                dv.Sort = "Seq ASC ";
                DataTable dt_sort = dv.ToTable();
                ds.Tables.Add(dt_sort);

                Session["ds_perRel"] = ds;

                txt_seq.Text = (int.Parse(dt_sort.Rows[dt_sort.Rows.Count - 1]["Seq"].ToString()) + 1).ToString();
            }
            gvTE.DataSource = ds;
            gvTE.DataBind();
            btn_insertPerson.Text = "Insert";

            dd_relation.Value = "";
            txt_Fname.Text = "";
            txt_Lname.Text = "";
            txt_tel1_TE.Text = "";
            txt_tel1_to_TE.Text = "";
            txt_tel1_ext_TE.Text = "";

            txt_tel2_TE.Text = "";
            txt_tel2_to_TE.Text = "";
            txt_tel2_ext.Text = "";
            txt_mobile_TE.Text = "";
            dd_verTE.Value = "";
            btn_saveTE.Enabled = true;

        }
        catch (Exception ex)
        {
            btn_saveTE.Enabled = true;
            lblMsgEN.Text = "insert/edit not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }

    }
    private void del_gvTE()
    {
        lblMsgEN.Text = "";
        lblMsgTH.Text = "";
        try
        {
            int rowNum = int.Parse(hid_rowNum.Value);
            DataSet ds = (DataSet)Session["ds_perRel"];
            //ds.Tables[0].Rows[rowNum].Delete();

            //*******  re- seq *******//
            DataTable dt_ = new DataTable();
            dt_.Columns.Add("Seq", typeof(string));
            dt_.Columns.Add("Relation", typeof(string));
            dt_.Columns.Add("Name", typeof(string));
            dt_.Columns.Add("SurName", typeof(string));
            dt_.Columns.Add("Tel_1", typeof(string));
            dt_.Columns.Add("To_1", typeof(string));
            dt_.Columns.Add("Ext_1", typeof(string));
            dt_.Columns.Add("Tel_2", typeof(string));
            dt_.Columns.Add("To_2", typeof(string));
            dt_.Columns.Add("Ext_2", typeof(string));
            dt_.Columns.Add("Mobile", typeof(string));
            dt_.Columns.Add("Ver", typeof(string));
            dt_.Columns.Add("Rel_DES", typeof(string));

            int countR = 1;
            int countRow = 0;
            //foreach (DataRow row in gvTE.Rows) // Loop over the rows.
            for (int i = 0; i < gvTE.Rows.Count; i++)
            {
                if (countRow != rowNum)
                {
                    Label lb_relation = (Label)gvTE.Rows[i].Cells[3].FindControl("lb_relation");
                    Label lb_name = (Label)gvTE.Rows[i].Cells[4].FindControl("lb_name");
                    Label lb_surname = (Label)gvTE.Rows[i].Cells[5].FindControl("lb_surname");
                    Label lb_tel_1 = (Label)gvTE.Rows[i].Cells[6].FindControl("lb_tel_1");
                    Label lb_To = (Label)gvTE.Rows[i].Cells[7].FindControl("lb_To");
                    Label lb_ext_1 = (Label)gvTE.Rows[i].Cells[8].FindControl("lb_ext_1");
                    Label lb_tel_2 = (Label)gvTE.Rows[i].Cells[9].FindControl("lb_tel_2");
                    Label lb_To2 = (Label)gvTE.Rows[i].Cells[10].FindControl("lb_To2");
                    Label llb_ext_2 = (Label)gvTE.Rows[i].Cells[11].FindControl("lb_ext_2");
                    Label lb_Mobile = (Label)gvTE.Rows[i].Cells[12].FindControl("lb_Mobile");
                    Label lb_Verify = (Label)gvTE.Rows[i].Cells[13].FindControl("lb_Verify");
                    Label lb_rel_desc = (Label)gvTE.Rows[i].Cells[3].FindControl("lb_rel_desc");
                    dt_.Rows.Add(countR.ToString(),
                                 lb_relation.Text,
                                 lb_name.Text,
                                 lb_surname.Text,
                                 lb_tel_1.Text,
                                 lb_To.Text,
                                 lb_ext_1.Text,
                                 lb_tel_2.Text,
                                 lb_To2.Text,
                                 llb_ext_2.Text,
                                 lb_Mobile.Text,
                                 lb_Verify.Text,
                                 lb_rel_desc.Text);

                    countR += 1;
                }
                countRow += 1;
            }
            //foreach (DataRow row in ds.Tables[0].Rows) // Loop over the rows.
            //{
            //    if (countRow != rowNum)
            //    {
            //        dt_.Rows.Add(countR.ToString(),
            //                     row[1].ToString(),
            //                     row[2].ToString(),
            //                     row[3].ToString(),
            //                     row[4].ToString(),
            //                     row[5].ToString(),
            //                     row[6].ToString(),
            //                     row[7].ToString(),
            //                     row[8].ToString(),
            //                     row[9].ToString(),
            //                     row[10].ToString(),
            //                     row[11].ToString());

            //        countR += 1;
            //    }
            //    countRow += 1;
            //}

            DataSet ds_2 = new DataSet();
            if (dt_.Rows.Count > 0)
            {
                ds_2.Tables.Add(dt_);

                Session["ds_perRel"] = ds_2;

                DataTable dt = ds_2.Tables[0];
                DataView dv = dt.DefaultView;
                dv.Sort = "Seq ASC ";

                DataTable dt_sort = dv.ToTable();
                txt_seq.Text = (int.Parse(dt_sort.Rows[dt_sort.Rows.Count - 1]["Seq"].ToString()) + 1).ToString();
            }
            else
            {
                ds_2.Tables.Add(dt_);
                Session["ds_perRel"] = null;
                txt_seq.Text = "1";
            }
            gvTE.DataSource = ds_2;
            gvTE.DataBind();

            lblMsgEN.Text = "Delete completed";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;

        }
        catch (Exception ex)
        {
            lblMsgEN.Text = "Delete not complete";
            PopupMsg_ver.ShowOnPageLoad = true;
            return;
        }
    }

    protected void gvTE_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            int dataItem = int.Parse(e.CommandArgument.ToString());
            int rowNum = dataItem;
            hid_rowNum.Value = dataItem.ToString();

            if (e.CommandName.ToString() == "Del")
            {
                hid_oper.Value = "Del_P";
                lblConfirmMsgEN.Text = "Do you want to delete data ?";
                PopupConfirmSave_ver.ShowOnPageLoad = true;

            }
            else if (e.CommandName.ToString() == "Sel")
            {
                Label seq = (Label)gvTE.Rows[rowNum].FindControl("lb_Seq");
                Label lb_rel = (Label)gvTE.Rows[rowNum].FindControl("lb_relation");
                Label lb_name = (Label)gvTE.Rows[rowNum].FindControl("lb_name");
                Label lb_surname = (Label)gvTE.Rows[rowNum].FindControl("lb_surname");
                Label lb_tel_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_1");
                Label lb_To = (Label)gvTE.Rows[rowNum].FindControl("lb_To");
                Label lb_ext_1 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_1");
                Label lb_tel_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_tel_2");
                Label lb_To2 = (Label)gvTE.Rows[rowNum].FindControl("lb_To2");
                Label lb_ext_2 = (Label)gvTE.Rows[rowNum].FindControl("lb_ext_2");
                Label lb_Mobile = (Label)gvTE.Rows[rowNum].FindControl("lb_Mobile");
                Label lb_Verify = (Label)gvTE.Rows[rowNum].FindControl("lb_Verify");

                txt_seq.Text = seq.Text;
                dd_relation.Value = lb_rel.Text;
                txt_Fname.Text = lb_name.Text;
                txt_Lname.Text = lb_surname.Text;
                txt_tel1_TE.Text = lb_tel_1.Text;
                txt_tel1_to_TE.Text = lb_To.Text;
                txt_tel1_ext_TE.Text = lb_ext_1.Text;
                txt_tel2_TE.Text = lb_tel_2.Text;
                txt_tel2_to_TE.Text = lb_To2.Text;
                txt_tel2_ext.Text = lb_ext_2.Text;
                txt_mobile_TE.Text = lb_Mobile.Text;
                dd_verTE.Value = lb_Verify.Text;
                btn_insertPerson.Text = "Edit";
                hid_rowNumSel.Value = rowNum.ToString();

                btn_saveTE.Enabled = false;


            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnConfirmSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hid_oper.Value == "S_cust")
            {
                Save_cust();
            }
            else if (hid_oper.Value == "S_TO")
            {
                saveTO();
            }
            else if (hid_oper.Value == "S_TH")
            {
                SaveTH();
            }
            else if (hid_oper.Value == "S_TM")
            {
                SaveTM();
            }
            else if (hid_oper.Value == "S_TE")
            {
                SaveTE();
            }
            else if (hid_oper.Value == "Del_P")
            {
                del_gvTE();
            }
            else if (hid_oper.Value == "S_comp")
            {

            }
            hid_oper.Value = "";
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {

    }
    protected void cb_notHave_TH_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_tel_TH.Text = "";
            txt_tel_2_TH.Text = "";
            txt_tel_ext_TH.Text = "";
            //dd_1133_TH.Value = "";
            dd_chkName_TH.Value = "";
            txt_res_name_TH.Text = "";
            dd_chk_addr_TH.Value = "";
            txt_res_addr_TH.Text = "";
            dd_type_tel.Value = "";
            dd_resContact_TH.Value = "";
            dd_person_TH.Value = "";
            txt_dd_person_TH.Text = "";

            if (cb_notHave_TH.Checked)
            {
                txt_tel_TH.Enabled = false;
                txt_tel_2_TH.Enabled = false;
                txt_tel_ext_TH.Enabled = false;
                // dd_1133_TH.Enabled = false;
                dd_chkName_TH.Enabled = false;
                txt_res_name_TH.Enabled = false;
                dd_chk_addr_TH.Enabled = false;
                txt_res_addr_TH.Enabled = false;
                dd_type_tel.Enabled = false;
                dd_resContact_TH.Enabled = false;
                dd_person_TH.Enabled = false;
                txt_dd_person_TH.Enabled = false;


            }
            else
            {
                txt_tel_TH.Enabled = true;
                txt_tel_2_TH.Enabled = true;
                txt_tel_ext_TH.Enabled = true;
                // dd_1133_TH.Enabled = true;
                dd_chkName_TH.Enabled = true;
                txt_res_name_TH.Enabled = true;
                dd_chk_addr_TH.Enabled = true;
                txt_res_addr_TH.Enabled = true;
                dd_type_tel.Enabled = true;
                dd_resContact_TH.Enabled = true;
                dd_person_TH.Enabled = true;
                txt_dd_person_TH.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_check_TM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_mobile_TM.Text = "";
            //txt_mobile_TM_P.Text = "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "script5", "try{ txt_mobile_TM_P.SetValue('') }catch (err) {}", true);
            dd_resContact_TM.Value = "";

            if (cb_check_TM.Checked)
            {
                txt_mobile_TM_P.Enabled = false;
                txt_mobile_TM.Enabled = false;
                dd_resContact_TM.Enabled = false;

            }
            else
            {

                txt_mobile_TM_P.Enabled = true;
                txt_mobile_TM.Enabled = true;
                dd_resContact_TM.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }

    }
    private bool checkTelTo(string tel, string tel_to)
    {
        try
        {

            int to_ = int.Parse(tel.Substring((tel.Length - tel_to.Length), tel_to.Length));
            if (int.Parse(tel_to) <= to_)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        catch (Exception ex)
        {
            return false;
        }

    }


    protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    {
        try
        {
            Cache.Remove("ds_V_Relation");
            Cache.Remove("ds_V_CompanyType");
            Cache.Remove("ds_V_CustomerType");
            Cache.Remove("ds_V_recieveMoney");
            Cache.Remove("ds_V_SSO");
            Cache.Remove("ds_V_BOL");
            Cache.Remove("ds_V_checkName_TO");
            Cache.Remove("ds_V_checkAddr_TO");
            Cache.Remove("ds_V_statusEmp_TO");
            Cache.Remove("ds_V_ResContact_TO");
            Cache.Remove("ds_V_Person_TO");
            Cache.Remove("ds_V_Type_employment");
            Cache.Remove("ds_V_Check_name_TH");
            Cache.Remove("ds_V_check_Addr_TH");
            Cache.Remove("ds_V_dd_type_tel");
            Cache.Remove("ds_V_res_contact_TM");
            Cache.Remove("ds_V_Person_TH");
            Cache.Remove("ds_V_res_contact_TH");

            loadData();

        }
        catch (Exception ex)
        {
        }
    }

}