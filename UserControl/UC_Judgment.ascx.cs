using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBM.Data.DB2.iSeries;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using System.Globalization;
using System.Collections;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;

using ESB.WebAppl.ILSystem.commons;
using EB_Service.Commons;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Model.CompanyBlacklist;
using ILSystem.App_Code.Helper;
using static ILSystem.App_Code.BLL.DataCenter.ILDataSubroutine;
using ILSystem.App_Code.BLL.Integrate;

public partial class ManageData_WorkProcess_UserControl_UC_Judgment : System.Web.UI.UserControl
{
    public delegate void ManageData_WorkProcess_UserControl_UC_Judgment_CallDelegate(string data);
    public event ManageData_WorkProcess_UserControl_UC_Judgment_CallDelegate getDataInterview;
    public event ManageData_WorkProcess_UserControl_UC_Judgment_CallDelegate getDataKESSAI;
    public event ManageData_WorkProcess_UserControl_UC_Judgment_CallDelegate changeTabToProduct;

    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    private string[] telno = {"","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","","",
                              "","","","","","","","","",""};

    ILDataCenter CallHisun;
    ILDataCenterMssql CallHisunMaster;
    ILDataCenterMssqlInterview CallHisunCustomer;
    ILDataSubroutine iLDataSubroutine;
    public UserInfoService userInfoService;
    //Connect_GeneralAPI conn_general;
    private readonly connectAPI _connectAPI;
    UserInfo userInfo;
    public ManageData_WorkProcess_UserControl_UC_Judgment()
    {
        _connectAPI = new connectAPI();
       
    }
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
    public string appDate
    {
        get
        {
            if (ViewState["appDate"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["appDate"]);
            }
        }
        set
        {
            ViewState["appDate"] = value;
            if (ViewState["appDate"].ToString() != "")
            {
                hid_AppDate.Value = (string)(ViewState["appDate"]);
            }
        }

    }
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

        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        CallHisunCustomer = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        CallHisun = new ILDataCenter();
        CallHisun.UserInfomation = userInfoService.GetUserInfo();
        if (string.IsNullOrEmpty(hidDate97.Value))
        {
            string Date_97 = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);

            hidDate97.Value = Date_97;

        }
        //  var conn_general = new Connect_GeneralAPI();

        if (IsPostBack)
        {
            return;
        }
        if (hid_AppNo.Value.Trim() != "" || hid_brn.Value.Trim() != "")
        {
            bindData();
        }

    }
    #region function

    // load ข้อมูลของลูกค้า //
    public  void bindData()
    {
        try
        {
            //hid_CSN.Value = "66036658";
            if (hid_CSN.Value.Trim() == "")
            {
                string err = "Please check CSN ";
                lblMsgEN.Text = err;
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunCustomer = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());

            txt_birthDate_j.Text = "";

            //********************************************//
            //**  check csms00 **//
            DataSet ds_00 =  CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            //var xx = iLDataSubroutine.CallSubroutine();
            DataSet ds_11 =  CallHisunCustomer.getCSMS11(hid_CSN.Value).Result;
            if (ds_11 == null)
            {
                string err = "Error";
                lblMsgEN.Text = err;
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            

            //*****    calculate age *****//
            DataRow dr_00 = ds_00.Tables[0]?.Rows[0];
            //DataRow dr_11 = ds_11.Tables[0].Rows[0];



            //*****   check type home ***//
            CallHisun.UserInfomation = userInfoService.GetUserInfo();

            string TelType = "";
            string Zip = "";
            string O_TLDS = "";
            string Error = "";
            if (ds_11.Tables[0]?.Rows.Count > 0)
            {
                DataRow dr_11 = ds_11.Tables[0]?.Rows[0];
                bool resTelHome = iLDataSubroutine.Call_GNSR16(hid_CSN.Value, dr_11["m11tel"].ToString().Trim(), dr_11["M11EXT"].ToString().Trim(), ref TelType, ref Zip, ref O_TLDS, ref Error, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //bool resTelHome = CallHisun.Call_GNSR16(hid_CSN.Value, dr_11["m11tel"].ToString().Trim(), dr_11["M11EXT"].ToString().Trim(), ref TelType, ref Zip, ref O_TLDS, ref Error, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

                checkTelType(!string.IsNullOrEmpty(dr_11["m11tel"].ToString().Trim()), !string.IsNullOrEmpty(dr_11["M11EXT"].ToString().Trim()));
                //if (resTelHome)
                //{
                //    dd_homeTelType.Items.Clear();
                //    dd_homeTelType.Items.Add(TelType + ":" + O_TLDS, TelType);
                //    dd_homeTelType.SelectedIndex = 1;
                //}
                //else
                //{
                //    dd_homeTelType.Items.Clear();
                //}
            }
            else
            {
                dd_homeTelType.Items.Clear();
            }
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            

            //***************************//
            hid_idno.Value = dr_00["m00idn"].ToString().Trim();
            DataRow[] dr_H = ds_11.Tables[0]?.Select("M11REF = 0 AND M11CDE = 'H' ");
            DataRow[] dr_O = ds_11.Tables[0]?.Select("M11REF = 0 AND M11CDE = 'O' ");
            //DataRow[] dr_I = ds_11.Tables[0].Select("M11REF = '' AND M11CDE = 'I' ");

            bind_J_OfficeTitle(dr_00["M00OFT"].ToString().Trim());
            //txt_off_name.Text = dr_00["M00OFC"].ToString().Trim();
            //bind_DDl_OfficeName(dr_00["M00OFC"].ToString().Trim());
            bind_DDl_OfficeName();
            var offnamefull = dr_00["M00OFC"].ToString();
            if (offnamefull != null)
            {
                if (offnamefull.Trim().Length < 49)
                {
                    checkcharactor(offnamefull != null ? offnamefull.ToString().Trim() : "");
                }
                else
                {
                    checkcharactor(offnamefull != null ? offnamefull.ToString() : "");
                }
            }
            dd_off_name.Value = dr_00["M00OFC"].ToString().Trim();


            string tamO = "";
            string ampO = "";
            string provO = "";
            string postcode_O = "";

            string tamH = "";
            string ampH = "";
            string provH = "";
            string postcode_H = "";

            string tamI = "";
            string ampI = "";
            string provI = "";
            string postcode_I = "";

            foreach (DataRow row in dr_H)
            {
                if (row["M11TEL"].ToString().Trim().IndexOf('*') != -1)
                {
                    txt_h_tel.Text = "";
                    txt_h_tel_to.Text = "";
                    txt_h_ext.Text = "";
                }
                else
                {
                    txt_h_tel.Text = (row["M11TEL"].ToString().Trim()).PadRight(10, ' ').Substring(0, 9);
                    if (row["M11TEL"].ToString().Trim().Length > 10)
                    {
                        txt_h_tel_to.Text = (row["M11TEL"].ToString().Trim()).Substring(10);
                    }
                    txt_h_ext.Text = row["M11EXT"].ToString().Trim();

                }
                txt_mobile.Text = row["M11MOB"].ToString().Trim();
                tamH = row["M11TAM"].ToString().Trim();
                ampH = row["M11AMP"].ToString().Trim();
                provH = row["M11PRV"].ToString().Trim();
                postcode_H = row["M11ZIP"].ToString().Trim();

            }
            foreach (DataRow row in dr_O)
            {
                if (row["M11TEL"].ToString().Trim().IndexOf('*') != -1)
                {
                    txt_off_phone.Text = "";
                    txt_off_tel_to.Text = "";
                    txt_off_tel_ext.Text = "";
                }
                else
                {
                    txt_off_phone.Text = (row["M11TEL"].ToString().Trim()).PadRight(10, ' ').Substring(0, 9);
                    if (row["M11TEL"].ToString().Trim().Length > 10)
                    {
                        txt_off_tel_to.Text = (row["M11TEL"].ToString().Trim()).Substring(10);
                    }
                    txt_off_tel_ext.Text = row["M11EXT"].ToString().Trim();
                }
                tamO = row["M11TAM"].ToString().Trim();
                ampO = row["M11AMP"].ToString().Trim();
                provO = row["M11PRV"].ToString().Trim();
                postcode_O = row["M11ZIP"].ToString().Trim();


            }


            int ageForShow = CallHisunMaster.computeAge(dr_00["M00BDT"].ToString().Trim().PadLeft(8, '0'));
            txt_age.Text = ageForShow.ToString();

            //********************************************//
            // กรณีเป็นลูกค้าเก่า ที่ Judgment KEY-IN STEP 1 จะทำการ load ข้อมูลมาให้อัตโนมัติ

            
            //DataSet ds = CallHisun.getCSMS13(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value);
            DataSet ds_13 = CallHisunCustomer.getCSMS13(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value).Result; // check data from csms13 by appNo and Branch
            if (!CallHisun.check_dataset(ds_13))
            {
                bind_J_ApplyType(); 
                bind_J_ApplyVia();
                bind_J_ApplyChannel();
                bind_J_Marital();
                bind_J_SubMaritalStatus(dd_marital_j.SelectedItem.Value.ToString());
                bind_J_ResidentType();
                bind_J_BusinessType();
                bind_J_Occupation();
                bind_J_Position();
                bind_J_SalaryType();
                bind_J_TypeCust();
                bind_J_IncomeStatement();
                bind_J_OfficeTitle();
                bind_J_EmployeeType();
                bind_J_Date_of_Income();
                bind_J_contract();
                bind_DDl_OfficeName();//

                //bind_J_Comercial();

                //BOT Report
                bind_J_SubCompanyBusiness("", "");

            
                bind_J_SubOccupation("", "");
            }
            else
            {
                DataRow dr_13 = ds_13.Tables[0]?.Rows[0];

                bind_J_ApplyType(dr_13["m13apt"].ToString().Trim());
                bind_J_ApplyVia(dr_13["m13apv"].ToString().Trim());
                bind_J_ApplyChannel(dr_13["M13CHA"].ToString().Trim());
                bind_J_ApplySubChannel(dr_13["M13CHA"].ToString().Trim(), dr_13["M13SCH"].ToString().Trim());
                bind_J_Marital(dr_13["M13MRT"].ToString().Trim());
                bind_J_SubMaritalStatus(dr_13["M13MRT"].ToString().Trim(), dr_13["M13SMT"].ToString().Trim());
                bind_J_ResidentType(dr_13["M13RES"].ToString().Trim());
                bind_J_BusinessType(dr_13["M13BUT"].ToString().Trim());
                bind_J_Occupation(dr_13["M13OCC"].ToString().Trim());
                bind_J_Comercial(dr_13["M13OCC"].ToString().Trim(), dr_13["M13FIL"].ToString().PadRight(50, ' ').Substring(21, 1));
                bind_J_Position(dr_13["M13POS"].ToString().Trim());
                bind_J_SalaryType(dr_13["M13SLT"].ToString().Trim());
                bind_J_TypeCust(dr_13["M13SST"].ToString().Trim());

                bind_J_IncomeStatement(dr_13["M13DOC"].ToString().Trim());
                hid_incomeDoc.Value = dr_13["M13DOC"].ToString().Trim();

                bind_J_EmployeeType(dr_13["m13emp"].ToString().Trim());
                bind_J_Date_of_Income(dr_13["m13sld"].ToString().Trim());
                bind_J_contract(dr_13["m13rtl"].ToString().Trim());

                //BOT Report
                bind_J_SubCompanyBusiness(dr_13["M13BUT"].ToString().Trim(), dr_13["M13CSN"].ToString().Trim());
                bind_J_SubOccupation(dr_13["M13OCC"].ToString().Trim(), dr_13["M13CSN"].ToString().Trim());


                hid_birthDate.Value = dr_00["M00BDT"].ToString().Trim();
                hid_provH.Value = dr_13["m13hpv"].ToString().Trim();


                tamI = dr_13["M13ITM"].ToString().Trim();
                ampI = dr_13["M13IAM"].ToString().Trim();
                provI = dr_13["M13IPV"].ToString().Trim();
                postcode_I = dr_13["M13IZP"].ToString().Trim();


                hid_tamI.Value = tamI == "0" ? "" : tamI;
                hid_ampI.Value = ampI == "0" ? "" : ampI;
                hid_provI.Value = provI == "0" ? "" : provI;
                txt_postcode_idcard_j.Text = postcode_I == "0" ? "" : postcode_I;

                try
                {
                    string birthdate = hid_birthDate.Value.PadLeft(8, '0').Substring(0, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(3, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(6, 4);
                    //CALL_GNP014
                    string oerrCHK = "";
                    string oNamed = "";

                    CallHisun.UserInfomation = userInfoService.GetUserInfo();  //
                    bool resGNP014 = iLDataSubroutine.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    //bool resGNP014 = CallHisun.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    if (resGNP014 == false || oerrCHK.Trim() == "Y")
                    {
                        lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่ (GNP014)" + "\r\n";
                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                        
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        txt_birthDate_j.Text = "";
                        txt_birthDate_j.Focus();
                        return;
                    }
                    lb_day.Text = oNamed.Trim();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    

                }
                catch (Exception ex)
                {
                    Utility.WriteLog(ex);
                    lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + ex.ToString() + "\r\n";
                    lb_day.Text = "";
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }




                if (dr_13["M13SEX"].ToString().Trim() == "")
                {
                    hid_sex.Value = dr_00["M00SEX"].ToString().Trim();
                    rb_sex.Value = dr_00["M00SEX"].ToString().Trim();
                }
                else
                {
                    hid_sex.Value = dr_13["M13SEX"].ToString().Trim();
                    rb_sex.Value = dr_13["M13SEX"].ToString().Trim();
                }
                txt_child.Text = dr_13["M13CHL"].ToString().Trim();
                txt_fPerson.Text = dr_13["M13CON"].ToString().Trim();
                txt_yearResident.Text = dr_13["M13LYR"].ToString().Trim();
                txt_monthResident.Text = dr_13["M13LMT"].ToString().Trim();
                txt_empNo.Text = dr_13["m13off"].ToString().Trim();
                txt_s_year.Text = dr_13["M13WKY"].ToString().Trim();
                txt_s_month.Text = dr_13["M13WKM"].ToString().Trim();
                txt_salary.Text = decimal.Parse(dr_13["m13net"].ToString().Trim()).ToString("0");
                hid_salary.Value = decimal.Parse(dr_13["m13net"].ToString().Trim()).ToString("0");
                txt_salary_adj.Text = decimal.Parse(dr_13["M13SAJ"].ToString().Trim()).ToString("0");
                txt_mobile_j.Text = dr_13["m13mtl"].ToString().Trim();
                txt_postcode_j.Text = dr_13["m13hzp"].ToString().Trim() == "0" ? "" : dr_13["m13hzp"].ToString().Trim();
                txt_postcode_off_j.Text = dr_13["m13ozp"].ToString().Trim() == "0" ? "" : dr_13["m13ozp"].ToString().Trim();
                txt_postcode_idcard_j.Text = dr_13["M13IZP"].ToString().Trim() == "0" ? "" : dr_13["M13IZP"].ToString().Trim();
                txt_amount_j.Text = decimal.Parse(dr_13["m13lna"].ToString().Trim()).ToString("0");
                txt_solvency.Text = decimal.Parse(dr_13["m13pbl"].ToString().Trim()).ToString("0");

                //pact 20170822
                txt_seftDec.Text = dr_13["m13sfd"].ToString().Trim();

                //**  bind CSSR07  **//
                string salary = "0";
                string date_sal = "";
                string time_sal = "";
                bool resSal = iLDataSubroutine.Call_CSSR07(dr_00["M00IDN"].ToString(), "99999999", ref salary, ref date_sal, ref time_sal, userInfo.BizInit, userInfo.BranchNo);
                //bool resSal = CallHisun.Call_CSSR07(dr_00["M00IDN"].ToString(), "99999999", ref salary, ref date_sal, ref time_sal, userInfo.BizInit, userInfo.BranchNo);
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
                if (resSal)
                {
                    hid_sal_old.Value = salary;
                    hid_sal_date.Value = date_sal;
                    hid_sal_time.Value = time_sal;
                }

                if (hid_status.Value == "INTERVIEW")
                {
                    // ***  change data --> ilwk05 
                    DataSet ds_ilwk05 = CallHisunCustomer.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
                    if (CallHisun.check_dataset(ds_ilwk05))
                    {
                        DataRow dr_05 = ds_ilwk05.Tables[0]?.Rows[0];
                        txt_salary.Text = decimal.Parse(dr_05["W5INET"].ToString().Trim()).ToString("0");
                        bind_J_SalaryType(dr_05["W5SALT"].ToString().Trim());
                        bind_J_EmployeeType(dr_05["w5tyem"].ToString().Trim());


                    }
                }

            }
            //***  check status ***//
            if (hid_status.Value == "STEP1")
            {
                cb_OpenPanel.Visible = false;
                panel_ver_home_office.Enabled = true;
                dd_off_title.Enabled = true;
                //txt_off_name.Enabled = true;
                dd_off_name.Enabled = true; // ปรับ textbox เป็น combobox เพื่อโชว์ ชื่อบริษัทที่ติด blacklist
                postcodeid.Visible = true;
                txt_postcode_idcard_j.Visible = true;
                btn_post_I.Visible = true;

                //****   แก้ไขวันที่  11/11/2557 เรื่อง ให้แสดงข้อมูลจาก CSMS00 ในกรณีที่เป็น Step 1
                DataRow dr_13 = ds_13.Tables[0]?.Rows[0];

                //bind_J_ApplyType(dr["m13apt"].ToString().Trim());
                //bind_J_ApplyVia(dr["m13apv"].ToString().Trim());
                //bind_J_ApplyChannel(dr["M13CHA"].ToString().Trim());
                //bind_J_ApplySubChannel(dr["M13CHA"].ToString().Trim(), dr["M13SCH"].ToString().Trim());
                bind_J_Marital(dr_00["M00MST"].ToString().Trim());
                bind_J_SubMaritalStatus(dr_00["M00MST"].ToString().Trim());
                bind_J_ResidentType(dr_00["M00RST"].ToString().Trim());
                bind_J_BusinessType(dr_00["M00BUS"].ToString().Trim());
                bind_J_Occupation(dr_00["M00OCC"].ToString().Trim());
                bind_J_Comercial(dr_00["M00OCC"].ToString().Trim());
                bind_J_Position(dr_00["M00POS"].ToString().Trim());
                bind_J_SalaryType(dr_00["M00SAL"].ToString().Trim());
                bind_J_TypeCust();
                bind_J_IncomeStatement();
                //hid_incomeDoc.Value = dr["M13DOC"].ToString().Trim();

                bind_J_EmployeeType(dr_00["M00EPT"].ToString().Trim());
                bind_J_Date_of_Income();
                bind_J_contract();

                //BOT Report
                bind_J_SubCompanyBusiness("", "");
                bind_J_SubOccupation("", "");

                hid_birthDate.Value = dr_00["M00BDT"].ToString().Trim();

                hid_tamO.Value = tamO == "0" ? "" : tamO;
                hid_ampO.Value = ampO == "0" ? "" : ampO;
                hid_provO.Value = provO == "0" ? "" : provO;
                txt_postcode_off_j.Text = postcode_O == "0" ? "" : postcode_O;

                hid_tambolH.Value = tamH == "0" ? "" : tamH;
                hid_ampH.Value = ampH == "0" ? "" : ampH;
                hid_provH.Value = provH == "0" ? "" : provH;
                txt_postcode_j.Text = postcode_H == "0" ? "" : postcode_H;



                //hid_provH.Value = dr["m13hpv"].ToString().Trim();
                try
                {
                    //string birthdate = hid_birthDate.Value.PadLeft(8, '0').Substring(6, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(4, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(0, 4);
                    //CALL_GNP014
                    string birthdate = hid_birthDate.Value.PadLeft(8, '0').Substring(0, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(3, 2) + hid_birthDate.Value.PadLeft(8, '0').Substring(6, 4);
                    string oerrCHK = "";
                    string oNamed = "";

                    CallHisun.UserInfomation = userInfoService.GetUserInfo();
                    bool resGNP014 = iLDataSubroutine.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    //bool resGNP014 = CallHisun.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    if (resGNP014 == false || oerrCHK.Trim() == "Y")
                    {
                        lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + "\r\n";
                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                        
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        txt_birthDate_j.Text = "";
                        txt_birthDate_j.Focus();
                        return;
                    }
                    lb_day.Text = oNamed.Trim();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    

                }
                catch (Exception ex)
                {
                    Utility.WriteLog(ex);
                    lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + "\r\n";
                    lb_day.Text = "";
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }




                if (dr_13["M13SEX"].ToString().Trim() == "")
                {
                    hid_sex.Value = dr_00["M00SEX"].ToString().Trim();
                    rb_sex.Value = dr_00["M00SEX"].ToString().Trim();
                }
                else
                {
                    hid_sex.Value = dr_13["M13SEX"].ToString().Trim();
                    rb_sex.Value = dr_13["M13SEX"].ToString().Trim();
                }
                txt_child.Text = dr_00["M00TOC"].ToString().Trim();
                txt_fPerson.Text = dr_00["M00TOF"].ToString().Trim();
                txt_yearResident.Text = dr_00["M00RYR"].ToString().Trim();
                txt_monthResident.Text = dr_00["M00RMO"].ToString().Trim();
                txt_empNo.Text = dr_00["M00TOO"].ToString().Trim();
                txt_s_year.Text = dr_00["M00WTY"].ToString().Trim();
                txt_s_month.Text = dr_00["M00WTM"].ToString().Trim();
                txt_salary.Text = decimal.Parse(dr_00["M00SAL"].ToString().Trim() != "" ? dr_00["M00SAL"].ToString().Trim() : "0" ).ToString("0");
                hid_salary.Value = decimal.Parse(dr_00["M00SAL"].ToString().Trim() != "" ? dr_00["M00SAL"].ToString().Trim() : "0").ToString("0");
                txt_salary_adj.Text = decimal.Parse(dr_13["M13SAJ"].ToString().Trim()).ToString("0");
                txt_mobile_j.Text = dr_13["m13mtl"].ToString().Trim();
                //txt_postcode_j.Text = dr["m13hzp"].ToString().Trim() == "0" ? "" : dr["m13hzp"].ToString().Trim();
                //txt_postcode_off_j.Text = dr["m13ozp"].ToString().Trim() == "0" ? "" : dr["m13ozp"].ToString().Trim();
                txt_amount_j.Text = decimal.Parse(dr_13["m13lna"].ToString().Trim()).ToString("0");
                txt_solvency.Text = decimal.Parse(dr_13["m13pbl"].ToString().Trim()).ToString("0");





                // End Step 1 //
            }
            else
            {
                cb_OpenPanel.Visible = true;
                cb_OpenPanel.Checked = false;
                panel_ver_home_office.Enabled = false;
                //dd_off_title.Enabled = false;  // แก้ไขให้ INTERVIEW และ KESSAI สามารถแก้ไขชื่อ บริษัทได้ วันที่ 17/03/2558  REQ:60538
                //txt_off_name.Enabled = false;

            }

            if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
            {
                txt_mobile.Enabled = false;
                txt_mobile_j.Enabled = false;
                txt_salary.Text = "";
                dd_statement_j.SelectedIndex = -1;
            }

            //***   enabled = false **  1.ลักษณะการรับเงินเดือน    2.ประเภทการจ้างงาน
            if (hid_status.Value == "INTERVIEW" || hid_status.Value == "KESSAI")
            {
                dd_incomeType_j.Enabled = false;
                dd_empType_j.Enabled = false;
            }


            bindAutoSalaryAdj();
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }



    private void SaveJudgment()
    {   /*กรณี KEY-IN STEP 1 โปรแกรมจะ AUTO REJECT เรื่อง เงินเดือน,สามจังหวัด,บริษัทติด Blacklist 
         *กรณี KEY-IN STEP 1  TCL = 0 โปรแกรมจะส่งผลไปให้ KESSAI Judgment อีกครั้ง
         *กรณี ที่ KESSAI jugdment ข้อมูลที่ TCL = 0 มาจาก KEY-IN STEP 1 สามารถที่จะใส่เงินเดือนไม่เหมือนกับ INTERVIEW ได้ หาก JUDGMENT แล้ว TCL != 0 โปรแกรมจะ Return ไปให้กับ INTErview อีกครั้ง หาก tcl = 0 --> จะ AUTO REJECT อัตโนมัติ
         *กรณี ที่ INTERVIEW และ KESSAI JUGDMENT ใน CASE ปกติ หาก ลูกค้าเข้าเงื่อนเรื่อง เงินเดือน,สามจังหวัด,บริษัทติด Blacklist  โปรแกรมจะไม่ Auto reject  INTERVIEW จะต้องไปกด reject เองที่หน้า TCL
         */


        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            lblMsgEN.Text = "";
            string progName = "ILJUDG";

            if (hid_status.Value == "STEP1")
            {
                progName = "KEYINSTEP1";
            }
            else if (hid_status.Value == "INTERVIEW")
            {
                progName = "INTERVIEW";
            }
            else if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
            {
                progName = "KESSAI";
            }


            CallHisun = new ILDataCenter();

            //***  check before insert or update ***//
            DataSet ds_csms00 = CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            if (!CallHisun.check_dataset(ds_csms00))
            {
                lblMsgEN.Text = "Save not complete";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            DataSet ds_csms13 = CallHisunCustomer.getCSMS13(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value).Result;
            DataSet ds_ilms01 = CallHisunCustomer.get_ilms01(hid_brn.Value, hid_AppNo.Value).Result;
            if (!CallHisun.check_dataset(ds_ilms01))
            {
                lblMsgEN.Text = "Save not complete";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }

            DataSet ds_rltb10 = CallHisunCustomer.get_RLTB10().Result;

            CallHisun.UserInfomation = userInfoService.GetUserInfo();
            iDB2Command cmd = new iDB2Command();
            DataRow dr_00 = ds_csms00?.Tables[0]?.Rows[0];
            DataRow dr_ilms01 = ds_ilms01?.Tables[0]?.Rows[0];
            string Date_97 = hidDate97.Value;
            //iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);
            //CallHisun.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            

            string Error = "";


            string[] birthDate = (txt_birthDate_j.Text.Trim().PadRight(8, '0')).Split('/');
            string m13app = "IL";
            string m13csn = hid_CSN.Value;
            string m13brn = hid_brn.Value;
            string m13apn = hid_AppNo.Value;
            string m13apt = dd_applyType_j.Value.ToString();
            string m13apv = dd_apply_via_j.Value.ToString();
            string m13cha = dd_apply_channel_j.Value.ToString();
            string m13sch = dd_subChannel_j.Value.ToString();
            string m13bdt = birthDate[2] + birthDate[1] + birthDate[0];
            string m13sex = rb_sex.SelectedItem.Value.ToString();
            string m13mrt = dd_marital_j.SelectedItem.Value.ToString();
            string m13smt = dd_subMarital_j.Value.ToString();
            string m13but = dd_busType_j.Value.ToString();
            string m13occ = dd_occup_j.Value.ToString();
            string m13pos = dd_position_j.Value.ToString();
            string m13off = txt_empNo.Text.Trim().Replace(",", "");
            string m13res = dd_resident_j.Value.ToString();
            string m13con = txt_fPerson.Text.Trim();
            string m13ttl = dd_homeTelType.Value.ToString() == "" ? "0" : (int.Parse(dd_homeTelType.Value.ToString()).ToString());
            string m13htl = CallHisunCustomer.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()); //"";  //เบอร์บ้าน Edit 21/10/2557 :ใส่เบอร์บ้านเพิ่ม
            string m13hex = txt_h_ext.Text.Trim();  //เบอร์บ้าน
            string m13mtl = txt_mobile_j.Text.Trim();  //ไม่มีใน Step 1    
            string m13wky = txt_s_year.Text.Trim();
            string m13wkm = txt_s_month.Text.Trim();
            string m13slt = dd_incomeType_j.Value == null ? "" : dd_incomeType_j.Value.ToString();
            string m13sld = dd_dateOfIncome.Value == null ? "" : dd_dateOfIncome.Value.ToString();
            string m13net = txt_salary.Text.Trim().ToString().Replace(",", "");
            string m13cmp = dr_ilms01["P1CAMP"].ToString().Trim(); //  edit 21/10/2557 
            string m13csq = dr_ilms01["P1CMSQ"].ToString().Trim(); // edit 21/10/2557
            string m13trm = dr_ilms01["P1TERM"].ToString().Trim(); // edit 21/10/2557
            string m13tcl = "0"; // 
            string m13tca = "0"; //ไม่มี
            string m13gol = "0"; //ไม่มี
            string m13chl = txt_child.Text.Trim();
            string m13hzp = txt_postcode_j.Text.Trim();
            string m13htm = hid_tambolH.Value.ToString().Trim() == "" ? " m13htm " : hid_tambolH.Value.ToString().Trim();//"0"; //ตำบล  
            string m13ham = hid_ampH.Value.ToString().Trim() == "" ? " m13ham " : hid_ampH.Value.ToString().Trim();  //"0"; //อำเภอ
            string m13hpv = hid_provH.Value.ToString().Trim() == "" ? " m13hpv " : hid_provH.Value.ToString().Trim();//"0"; //จังหวัด
            string m13ozp = txt_postcode_off_j.Text.Trim();
            string m13otm = hid_tamO.Value.ToString().Trim() == "" ? " m13otm " : hid_tamO.Value.ToString().Trim();//"0"; //ตำบล
            string m13oam = hid_ampO.Value.ToString().Trim() == "" ? " m13oam " : hid_ampO.Value.ToString().Trim();//"0"; //อำเภอ 
            string m13opv = hid_provO.Value.ToString().Trim() == "" ? " m13opv " : hid_provO.Value.ToString().Trim();//"0"; //จังหวัด
            string m13lyr = txt_yearResident.Text.Trim();
            string m13lmt = txt_monthResident.Text.Trim();
            string m13fdt = hid_AppDate.Value; //Appdate 
            string m13mob = txt_mobile_j.Text.Trim() == "" ? "" : "1";   //w_mobileF
            string m13emp = dd_empType_j.Value.ToString().Trim();
            string m13lna = txt_amount_j.Text.Trim().Replace(",", "");
            string m13pbl = txt_solvency.Text.Trim().Replace(",", "");
            string m13fil = ""; // G_PD +dd_comerc.Value.ToString(); 
            string m131id = "0";//rank
            string m13gno = "0"; //group
            string m13acl = "0"; //G_ACL_13
            string m13bot = "0"; //EdtBOTLoan.Value
            string m13cbl = "0"; //EdtCrBal.Value
            string m13_rk = "0"; // rank
            string m13_gn = "0"; // group
            string m13_ac = "0"; //G_ACL_13
            string m13cru = "'" + userInfo.Username.ToString() + "'";
            string m13cud = Date_97;
            string m13aus = "''";
            string m13aud = "0"; // ไม่มี
            string m13rtl = dd_contact_judg.Value == null ? "" : dd_contact_judg.Value.ToString();  //dd_contact_judg.SelectedItem.Value.ToString();
            string m13sad = hid_AppDate.Value;
            string m13sat = m_UpdTime.ToString();
            string m13osl = hid_sal_old.Value.ToString().Trim() == "" ? "0" : hid_sal_old.Value.ToString().Trim();
            string m13osd = hid_sal_date.Value.ToString().Trim() == "" ? "0" : hid_sal_date.Value.ToString().Trim();
            string m13ost = hid_sal_time.Value.ToString().Trim() == "" ? "0" : hid_sal_time.Value.ToString();
            string M13SST = dd_typeCust_j.SelectedItem.Value.ToString();
            string M13SAJ = txt_salary_adj.Text.Trim() == "" ? "0" : txt_salary_adj.Text.Trim().Replace(",", "");
            string M13CAL = dd_statement_j.Value.ToString();
            string M13DOC = dd_statement_j.Value.ToString();//dd_incomeType_j.SelectedItem.Value.ToString();
            string m13upg = progName;
            string m13udt = Date_97;
            string m13utm = m_UpdTime.ToString();
            string m13usr = userInfo.Username.ToString(); 
            string m13wks = userInfo.LocalClient.ToString();
            string m13otl = CallHisunCustomer.getTel(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim());
            string M13OEX = txt_off_tel_ext.Text.Trim();
            string vendor01 = dr_ilms01["P1VDID"].ToString().Trim().PadLeft(12, '0');
            //pact
            string m13izp = txt_postcode_idcard_j.Text.Trim() == "" ? " m13izp " : txt_postcode_idcard_j.Text.ToString().Trim();//"0"; //ตำบล
            string m13itm = hid_tamI.Value.ToString().Trim() == "" ? " m13itm " : hid_tamI.Value.ToString().Trim();//"0"; //ตำบล
            string m13iam = hid_ampI.Value.ToString().Trim() == "" ? " m13iam " : hid_ampI.Value.ToString().Trim();//"0"; //อำเภอ 
            string m13ipv = hid_provI.Value.ToString().Trim() == "" ? " m13ipv " : hid_provI.Value.ToString().Trim();//"0"; //จังหวัด
            string m13rkf = "";
            string m13sfd = txt_seftDec.Text.Trim() == "-" ? " m13sfd " : txt_seftDec.Text.Trim().PadLeft(2, '0');//"0";

            //BOT Report ================================================
            JObject jsonData = new JObject();
            jsonData.Add("CustomerTypeID", '1');
            jsonData.Add("BirthDate", birthDate[2] + birthDate[1] + birthDate[0]);
            jsonData.Add("CISNumber", hid_CSN.Value);
            jsonData.Add("TitleID", dr_00["m00ttl"].ToString().Trim());
            jsonData.Add("NameInENG", "");
            jsonData.Add("SurnameInENG", "");
            jsonData.Add("NickName", "");
            jsonData.Add("NameInTHAI", dr_00["m00tnm"].ToString().Trim());
            jsonData.Add("SurnameInTHAI", dr_00["m00tsn"].ToString().Trim());
            jsonData.Add("SexID", dr_00["m00sex"].ToString().Trim());
            jsonData.Add("MaritalStatusID", "");
            jsonData.Add("CardTypeID", dr_00["m00idt"].ToString().Trim());
            jsonData.Add("IDCard", dr_00["m00idn"].ToString().Trim());
            jsonData.Add("IDCardIssued", "");
            jsonData.Add("IDCardExpiredDate", dr_00["m00eid"].ToString().Trim());
            jsonData.Add("EmailAddress", dr_00["m00eml"].ToString().Trim());
            jsonData.Add("ResidentalStatusID", "");
            jsonData.Add("ResidentalYear", "");
            jsonData.Add("ResidentalMonth", "");
            jsonData.Add("NoOfChildren", "");
            jsonData.Add("NoOfFamily", "");
            jsonData.Add("ContactTime", "");
            jsonData.Add("RecordStatus", "");
            jsonData.Add("Application", "ILSystem");
            jsonData.Add("CreateBy", "");
            jsonData.Add("CreateDate", "");
            jsonData.Add("UpdateBy", userInfo.Username.ToString());
            jsonData.Add("UpdateDate", DateTime.Now);
            jsonData.Add("IsDelete", "");
            //===========================================================

            //Ning - BOT-Reg
            string sP1NCBF = dr_ilms01["P1FILL"].ToString().Substring(20, 1);
            string sBOTMINSAL = "0";
            if (ds_rltb10 != null && ds_rltb10.Tables.Count > 0)
            {
                sBOTMINSAL = ds_rltb10.Tables[0]?.Rows[0]["T10CD2"].ToString().Trim();
            }

            DataSet ds_res = new DataSet();


            string resident_y = ((int.Parse(m13lyr) * 12) + int.Parse(m13lmt)).ToString();
            string workyear = ((int.Parse(m13wky) * 12) + int.Parse(m13wkm)).ToString();

            CallHisunCustomer.calTCL(dr_00["m00idn"].ToString().Trim(), hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, hid_AppDate.Value, Date_97, m13bdt, m13sex, txt_age.Text.Trim(), m13mrt, m13ttl, m13mtl, m13res,
                        resident_y, workyear, m13occ, m13but, m13slt, m13off, m13net, m13con, m13chl, m13pos, m13emp, m13brn, "0", m13apv, m13hzp, M13SST, M13SAJ,
                        "0", vendor01.ToString(), m13gol, m13cha, ref ds_res, ref Error, m13ozp);

            DataRow dr_res = ds_res.Tables[0]?.Rows.Count > 0 ? ds_res.Tables[0].Rows[0] : null;
            
            bool Have_CSMS03 = false;  //SL42 New Model and New Model ZR   
            //if (CallHisunCustomer._dataCenter.SqlCon.State != ConnectionState.Open)
            //    CallHisunCustomer._dataCenter.SqlCon.Open();
            //CallHisunCustomer._dataCenter.Sqltr?.Connection = CallHisunCustomer._dataCenter.SqlCon.BeginTransaction();
            if (CallHisun.check_dataset(ds_res))
            {
                Have_CSMS03 = (bool)dr_res["G_Have_CSMS03"];  //SL42 New Model and New Model ZR  
                m13tcl = dr_res["EdtTCL"].ToString().Trim();
                m13tca = dr_res["EdtCrAvi"].ToString().Trim();


                m13gol = dr_res["EdtTCL"].ToString().Trim();
                //assign ค่า อย่าลืมลบ
               // m13tcl = "50000.00";
                if (dr_res["G_Have_TCL"].ToString().Trim() != "Y")
                {
                    m13gol = "0";
                }

                DataSet ds_07_1 = CallHisunCustomer.getCSMS07(CallHisunCustomer, hid_AppNo.Value, userInfo.BranchApp, hid_CSN.Value).Result;
                if (CallHisun.check_dataset(ds_07_1))
                {
                    DataRow dr = ds_07_1.Tables[0]?.Rows[0];
                    string C07STS = "";
                    if (txt_salary.Text.Trim() == "0" && dr["c07sts"].ToString().Trim() == "")
                    {
                        C07STS = "X";
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = " update AS400DB01.CSOD0001.csms07 set c07sts='" + C07STS + "'" +
                                       " where c07csn =" + hid_CSN.Value + " and " +
                                       " c07app = 'IL' and c07apn= " + hid_AppNo.Value + " and c07brn=" + userInfo.BranchApp;
                    //*** insert into csms13HS ***//
                    //cmd.CommandText = upda_07;
                    //int res_07_1 = CallHisun.ExecuteNonQuery(cmd);
                    bool transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res_07_1 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                    if (res_07_1.afrows == -1)
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();

                        Utility.WriteLogString(res_07_1.message.ToString(), cmd.CommandText.ToString());
                        lblMsgEN.Text = "Save not complete";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }
                }
                string comerc = "";
                if (dd_comerc.Value != null)
                {
                    comerc = dd_comerc.Value.ToString();
                }
                //string comerc =  dd_comerc.Value.ToString()  == "" ? " " : dd_comerc.Value.ToString();
                if (comerc == "")
                {
                    comerc = " ";
                }

                m13fil = "CONCAT(Substring(m13fil,1,4)," + "'" + dr_res["G_PD"].ToString() + "'" + ",Substring(m13fil,20,2)," + "'" + comerc + "'" + ",Substring(m13fil,23,len(m13fil))) ";//dr_res["G_PD"].ToString().Trim() + "  " + dd_comerc.Value.ToString()  == "" ? " " : dd_comerc.Value.ToString();
                m131id = dr_res["E_rank"].ToString().Trim() == "" ? "0" : dr_res["E_rank"].ToString().Trim();//rank
                m13gno = dr_res["E_group"].ToString().Trim() == "" ? "0" : dr_res["E_group"].ToString().Trim(); //group
                m13acl = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim(); //G_ACL_13
                m13bot = dr_res["EdtBOTLoan"].ToString().Trim() == "" ? "0" : dr_res["EdtBOTLoan"].ToString().Trim(); //EdtBOTLoan.Value
                m13cbl = dr_res["EdtCrBal"].ToString().Trim() == "" ? "0" : dr_res["EdtCrBal"].ToString().Trim(); //EdtCrBal.Value
                m13_rk = dr_res["E_rank"].ToString().Trim() == "" ? "0" : dr_res["E_rank"].ToString().Trim(); // rank
                m13_gn = dr_res["E_group"].ToString().Trim() == "" ? "0" : dr_res["E_group"].ToString().Trim(); // group
                m13_ac = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim(); //G_ACL_13
                hid_resCal.Value = decimal.Parse(!string.IsNullOrEmpty(m13tcl) ? m13tcl : "0" ).ToString("0");

                
            }

            m13ttl = m13ttl == "0" ? "" : (int.Parse(dd_homeTelType.Value.ToString()).ToString()).PadLeft(2, '0');


            string s_M13FIL = "";//SL42 New Model and New Model ZR  
            if (CallHisun.check_dataset(ds_csms13))
            {
                DataRow dr_csms13 = ds_csms13.Tables[0]?.Rows[0];
                string s_M13FIL_Length = dr_csms13["M13FIL"].ToString();
                if (s_M13FIL_Length.Trim().Length > 18)
                {
                    s_M13FIL = s_M13FIL_Length.ToString().Substring(18, 1);
                }
            }

            cmd.Parameters.Clear();



            //******************************//

            string autoRejMsg = "";
            string res_sms = "";
            string showAppNo = "";
            if (!CallHisun.check_dataset(ds_csms13))
            {
                //*** insert into csms13 ***//
                cmd.CommandText = CallHisunCustomer.INSERT_CSMS13_JUDG(m13app, m13csn, m13brn, m13apn, m13apt, m13apv, m13cha, m13sch, m13bdt, m13sex, m13mrt, m13smt, m13but, m13occ, m13pos,
                                                             m13off, m13res, m13con, m13ttl, m13htl, m13hex, m13mtl, m13wky, m13wkm, m13slt, m13sld, m13net, m13cmp, m13csq, m13trm,
                                                             m13tcl, m13tca, m13gol, m13chl, m13hzp, m13htm, m13ham, m13hpv, m13ozp, m13otm, m13oam, m13opv, m13lyr, m13lmt, m13fdt,
                                                             m13mob, m13emp, m13lna, m13pbl, m13fil, m131id, m13gno, m13acl, m13bot, m13cbl, m13_rk, m13_gn, m13_ac, m13cru, m13cud,
                                                             m13aus, m13aud, m13rtl, m13sad, m13sat, m13osl, m13osd, m13ost, M13SST, M13SAJ, M13CAL, M13DOC, m13upg, m13udt, m13utm,
                                                             m13usr, m13wks, m13otl, M13OEX, m13cmp, m13csq, m13trm, m13izp, m13itm, m13iam, m13ipv);
                //cmd.Parameters.Add("@m13hex", m13hex);
                //cmd.Parameters.Add("@M13OEX", M13OEX);
                bool transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                var res_13 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                //int res_13 = CallHisun.ExecuteNonQuery(cmd);
                if (res_13.afrows == -1)
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    Utility.WriteLogString(res_13.message.ToString(), cmd.CommandText.ToString());
                    lblMsgEN.Text = "Save not complete ";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }


                if (hid_G_aprj.Value != "" && hid_G_loca.Value != "" && hid_G_reason.Value != "" && (hid_status.Value == "STEP1")) /*เพิ่มเติม ให้ Auto reject เฉพาะที่ Key-in step 1 17/03/2558 */
                {
                    string step = "";
                    if (hid_status.Value == "STEP1")
                    {
                        step = "1";
                    }
                    else if (hid_status.Value == "INTERVIEW")
                    {
                        step = "2";
                    }
                    else if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
                    {
                        step = "3";
                    }
                    if (hid_G_aprj.Value == "RJ")
                    {
                        if (hid_G_reason.Value == "QL25")
                        {
                            //autoRejMsg = "["+hid_G_reason.Value+"] "+" Office name :"+txt_off_name.Text.Trim()+" is blacklist status ."+ "\r\n"+" System will Auto Reject. ";
                            autoRejMsg = "[" + hid_G_reason.Value + "] " + " Office name :" + dd_off_name.Text.Trim() + " is blacklist status ." + "\r\n" + " System will Auto Reject. ";
                        }
                        else if (hid_G_reason.Value == "HL1")
                        {
                            autoRejMsg = "[" + hid_G_reason.Value + "]" + " Customer was found in 3 provinces . " + "\r\n" + " System will Auto Reject. ";
                        }
                        else if (hid_G_reason.Value == "EL11")
                        {
                            autoRejMsg = "[" + hid_G_reason.Value + "]" + " Salary less then regulation [" + hid_G_reason.Value + "]" + "\r\n" + "  System will Auto Reject. ";
                        }

                    }
                    // ***  check reject ***//
                    bool saveReject = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, hid_G_reason.Value, hid_G_aprj.Value, hid_G_loca.Value, step, progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                    if (!saveReject)
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();

                        lblMsgEN.Text = "Save not complete";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }


                    //*** call  send SMS sub routine ***//
                    if (txt_mobile.Text.Trim() != "")
                    {
                        string poerrc = "";
                        string poerrm = "";
                        //bool sms = true;
                        ILSRSMS iLSRSMS = new ILSRSMS();
                        bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                      hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                        //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                        //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                        //                              , userInfo.BizInit, userInfo.BranchNo);
                        if (!sms || poerrc == "Y")
                        {
                            lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                        res_sms = " [ส่ง SMS สำเร็จ] ";
                    }
                    else
                    {
                        res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                    }
                    //********************************

                }
                else
                {
                    if (hid_status.Value.ToUpper() == "STEP1")
                    { // step 1 TCL = 0 ให้ส่งไป KESSAI .
                        bool saveStep;
                        string s_key1_M13FLI = "";
                        s_key1_M13FLI = dr_res["G_PD"].ToString();
                        if (s_key1_M13FLI.Trim().Length > 14)
                        {
                            s_M13FIL = s_key1_M13FLI.ToString().Substring(14, 1);
                        }
                        if ((decimal.Parse(m13tcl).ToString("0") == "0")
                            || (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToDouble(m13net) < 30000 && Convert.ToInt32(decimal.Parse(m13tcl).ToString("0")) >= 0)
                            || (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false)))
                        {
                            autoRejMsg = " TCL = 0. This case go to KESSAI.";
                            saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "'MI'");
                        }
                        else
                        {
                            saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "'MI'");
                        }

                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                    }
                    else if (hid_status.Value == "TCL0")
                    {
                        bool saveStep_TCL0;
                        if ((decimal.Parse(m13tcl).ToString("0") == "0")
                            || (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToDouble(m13net) < 30000 && Convert.ToInt32(decimal.Parse(m13tcl).ToString("0")) >= 0)
                            || (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToInt32(m131id) == 1 && Convert.ToDouble(m13net) < 10000)
                            || (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false)))
                        {
                            // IL10 TCL ≥ 1/9/2560 และ NCB Flag = T และ Salary < 30000 และ TCL > 0 
                            // IL10 TCL ≥ 1/9/2560 และ NCB Flag = T และ Salary < 30000 และ TCL = 0 
                            // IL10 Rank 1 และ Salary < 10000 และ 1st TCL ≥ 1/9/2560 และ NCB Flag = T 
                            // SL19 TCL = 0  
                            // SL42 Condition Rank 1 และ Salary < 10,000 และ (New Model Have_CSMS03 OR New model ZR s_M13FIL == "Z")
                            if (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToDouble(m13net) < 30000 && Convert.ToInt32(decimal.Parse(m13tcl).ToString("0")) >= 0)
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "IL10", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);

                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                            }
                            else if (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToInt32(m131id) == 1 && Convert.ToDouble(m13net) < 10000)
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "IL10", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                            }
                            else if (decimal.Parse(m13tcl).ToString("0") == "0")
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "SL19", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);

                                if (!saveStep_TCL0)
                                {

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                                //********************************
                            }
                            else if (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false))
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "SL42", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);

                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }

                            }
                        }
                        else
                        {
                            autoRejMsg = " This case return to INTERVIEW.";
                            saveStep_TCL0 = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                            if (!saveStep_TCL0)
                            {
                                CallHisunCustomer._dataCenter.RollbackMssql();
                                CallHisunCustomer._dataCenter.CloseConnectSQL();

                                lblMsgEN.Text = "Save not complete";
                                PopupMsg_judgment.ShowOnPageLoad = true;
                                return;
                            }

                        }
                    }
                    else if (hid_status.Value == "INTERVIEW")
                    {

                        bool saveStep;
                        saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }

                    }
                    else if (hid_status.Value == "KESSAI")
                    {


                        bool saveStep;
                        saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                    }
                }

                CallHisunCustomer._dataCenter.CommitMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
                
                lblMsgEN.Text = "Save completed" + autoRejMsg + res_sms;

                PopupMsg_judgment.ShowOnPageLoad = true;
                //  Success //
                return;
            }
            else
            {
                cmd.Parameters.Clear();
                

                if (hid_status.Value == "STEP1")
                {
                    m13cru = "''";
                    m13cud = "0";
                    m13aus = "''";
                    m13aud = "0";
                }
                //pact check 3pv
                string m13hpv_3pv = "";
                string m13opv_3pv = "";
                string m13ipv_3pv = "";
                if (m13hpv.Trim() == "m13hpv")
                {
                    m13hpv_3pv = "0";
                }
                else
                {
                    m13hpv_3pv = m13hpv;
                }
                if (m13opv.Trim() == "m13opv")
                {
                    m13opv_3pv = "0";
                }
                else
                {
                    m13opv_3pv = m13opv;
                }
                if (m13ipv.Trim() == "m13ipv")
                {
                    m13ipv_3pv = "0";
                }
                else
                {
                    m13ipv_3pv = m13ipv;
                }


                DataSet DS = new DataSet();
                cmd.CommandText = "SELECT G06CDE FROM AS400DB01.GNOD0000.GNTB106 WITH (NOLOCK) WHERE G06CDE in (" + m13hpv_3pv + "," + m13opv_3pv + "," + m13ipv_3pv + ")";
                //DS = CallHisun.RetriveAsDataSet("SELECT G06CDE FROM GNTB106 WHERE G06CDE in (" + m13hpv_3pv + "," + m13opv_3pv + "," + m13ipv_3pv + ")");
                DS = CallHisunCustomer._dataCenter.GetDataset<DataSet>(cmd.CommandText, CommandType.Text).Result.data;
                if (DS != null)
                {
                    if (DS.Tables[0]?.Rows.Count > 0)
                    {
                        m13rkf = "3PV";
                    }
                }

                if (hid_status.Value == "INTERVIEW")
                {
                    m13cru = "'" + userInfo.Username + "'";
                    m13cud = Date_97;
                    m13aus = "''";
                    m13aud = "0";
                }
                if (hid_status.Value == "KESSAI")
                {
                    m13aus = "'" + userInfo.Username + "'";
                    m13aud = Date_97;
                    m13cru = "m13cru";
                    m13cud = "m13cud";
                }


                //*** Update csms13 ***//
                cmd.Parameters.Clear();
                cmd.CommandText = CallHisunCustomer.UPDATE_CSMS13(m13apt, m13apv, m13cha, m13sch, m13bdt, m13sex,
                                                       m13mrt, m13smt, m13but, m13occ, m13pos, m13off,
                                                       m13res, m13con, m13ttl, m13htl, m13hex, m13mtl,
                                                       m13wky, m13wkm, m13slt, m13sld, m13net, m13cmp,
                                                       m13csq, m13trm, m13tcl, m13tca, m13gol, m13chl,
                                                       m13hzp, m13htm, m13ham, m13hpv, m13ozp, m13otm,
                                                       m13oam, m13opv, m13lyr, m13lmt, m13fdt, m13mob,
                                                       m13emp, m13lna, m13pbl, m13fil, m131id, m13gno,
                                                       m13acl, m13bot, m13cbl, m13_rk, m13_gn, m13_ac,
                                                       m13rtl, m13sad, m13sat, m13osl, m13osd, m13ost,
                                                       M13SST, M13SAJ, M13CAL, M13DOC, m13upg, m13udt,
                                                       m13utm, m13usr, m13wks, m13brn, m13app, m13apn,
                                                       m13aus, m13aud, m13cru, m13cud, m13otl, M13OEX);
                bool transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false; 
                var res_update_13 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (res_update_13.afrows == -1)
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    Utility.WriteLogString(res_update_13.message.ToString(), cmd.CommandText.ToString());
                    lblMsgEN.Text = "Save not complete ";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }
                //if (hid_status.Value == "STEP1")
                //{
                cmd.Parameters.Clear();
                //cmd.CommandText = CallHisun.UPDATE_CSMS13POSTID(m13brn, m13app, m13apn, m13izp, m13itm, m13iam, m13ipv, m13rkf);
                //int res_update_13POSTID = CallHisun.ExecuteNonQuery(cmd);
                cmd.CommandText = CallHisunCustomer.UPDATE_CSMS13POSTID(m13brn, m13app, m13apn, m13izp, m13itm, m13iam, m13ipv, m13rkf);
                transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                var res_update_13POSTID = CallHisunCustomer._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (res_update_13POSTID.afrows == -1)
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    Utility.WriteLogString(res_update_13POSTID.message.ToString(), cmd.CommandText.ToString());
                    lblMsgEN.Text = "Save not complete ";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }
                //}

                if (hid_status.Value == "STEP1" || hid_status.Value == "INTERVIEW" || hid_status.Value == "KESSAI")
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = CallHisunCustomer.UPDATE_CSMS13SEFTDEC(m13brn, m13app, m13apn, m13sfd);
                    transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res_update_13SEFT = CallHisunCustomer._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result;
                    if (res_update_13SEFT.afrows == -1)
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();

                        Utility.WriteLogString(res_update_13SEFT.message.ToString(), cmd.CommandText.ToString());
                        lblMsgEN.Text = "Save not complete ";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }
                }


                if (hid_G_aprj.Value != "" && hid_G_loca.Value != "" && hid_G_reason.Value != "" && (hid_status.Value == "STEP1")) /*เพิ่มเติม ให้ Auto reject เฉพาะที่ Key-in step 1 17/03/2558 */
                {
                    string step = "";
                    if (hid_status.Value == "STEP1")
                    {
                        step = "1";
                    }
                    else if (hid_status.Value == "INTERVIEW")
                    {
                        step = "2";
                    }
                    else if (hid_status.Value == "KESSAI")
                    {
                        step = "3";
                    }

                    if (hid_G_reason.Value == "QL25")
                    {
                        //autoRejMsg = "[" + hid_G_reason.Value + "]"+" Office name :" + txt_off_name.Text.Trim() + " is blacklist status. " + "\r\n" + "  System will Auto Reject. ";
                        autoRejMsg = "[" + hid_G_reason.Value + "]" + " Office name :" + dd_off_name.Text.Trim() + " is blacklist status. " + "\r\n" + "  System will Auto Reject. ";
                    }
                    else if (hid_G_reason.Value == "HL1")
                    {
                        autoRejMsg = "[" + hid_G_reason.Value + "]" + " Customer was found in 3 provinces " + "\r\n" + "  System will Auto Reject. ";
                    }
                    else if (hid_G_reason.Value == "EL11")
                    {
                        autoRejMsg = "[" + hid_G_reason.Value + "]" + " Salary less then regulation " + "\r\n" + "  System will Auto Reject. ";
                    }
                    bool saveReject = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, hid_G_reason.Value, hid_G_aprj.Value, hid_G_loca.Value, step, progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                    if (!saveReject)
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();

                        lblMsgEN.Text = "Save not complete";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }

                    //*** call  send SMS sub routine ***//
                    if (txt_mobile.Text.Trim() != "")
                    {
                        string poerrc = "";
                        string poerrm = "";
                        //bool sms = true;
                        ILSRSMS iLSRSMS = new ILSRSMS();
                        bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                      hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                        //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                        //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                        //                              , userInfo.BizInit, userInfo.BranchNo);
                        if (!sms || poerrc == "Y")
                        {
                            lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();
                            
                            

                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                        res_sms = " [ส่ง SMS สำเร็จ] ";
                    }
                    else
                    {
                        res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                    }
                    //********************************
                }
                else
                {

                    if (hid_status.Value.ToUpper() == "STEP1")
                    {
                        bool saveStep;
                        string s_key1_M13FLI = "";
                        s_key1_M13FLI = dr_res["G_PD"].ToString();
                        if (s_key1_M13FLI.Trim().Length > 14)
                        {
                            s_M13FIL = s_key1_M13FLI.ToString().Substring(14, 1);
                        }
                        if ((decimal.Parse(!string.IsNullOrEmpty(m13tcl) ? m13tcl : "0").ToString("0") == "0")
                            || (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToDouble(m13net) < 30000 && Convert.ToInt32(decimal.Parse(m13tcl).ToString("0")) >= 0)
                            || (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false)))
                        {
                            autoRejMsg = " TCL = 0. This case go to KESSAI.";
                            saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "'MI'");
                        }
                        else
                        {
                            saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "'MI'");
                        }

                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                    }
                    else if (hid_status.Value == "TCL0")
                    {
                        bool saveStep_TCL0;
                        if (decimal.Parse(m13tcl).ToString("0") == "0" || (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL))
                            || (decimal.Parse(m13tcl).ToString("0") == "0")
                            || (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false)))
                        {

                            // IL10 TCL ≥ 1/9/2560 และ NCB Flag = T และ Salary < 30000 และ TCL > 0 
                            // IL10 TCL ≥ 1/9/2560 และ NCB Flag = T และ Salary < 30000 และ TCL = 0 
                            // IL10 Rank 1 และ Salary < 10000 และ 1st TCL ≥ 1/9/2560 และ NCB Flag = T 
                            // SL19 TCL = 0  
                            // SL42 Condition Rank 1 และ Salary < 10,000 และ (New Model Have_CSMS03 OR New model ZR s_M13FIL == "Z")

                            if (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToDouble(m13net) < 30000 && Convert.ToInt32(decimal.Parse(m13tcl).ToString("0")) >= 0)
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "IL10", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                            }
                            else if (sP1NCBF == "T" && Convert.ToDouble(m13net) < Convert.ToDouble(sBOTMINSAL) && Convert.ToInt32(m131id) == 1 && Convert.ToDouble(m13net) < 10000)
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "IL10", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                            }
                            else if (decimal.Parse(m13tcl).ToString("0") == "0")
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "SL19", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                                //********************************
                            }
                            else if (Convert.ToDouble(m13net) < 10000 && Convert.ToInt32(m131id) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false))
                            {
                                autoRejMsg = " TCL = 0. Auto reject.";
                                saveStep_TCL0 = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "SL42", "RJ", "210", "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);

                                if (!saveStep_TCL0)
                                {
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                                    
                                    
                                    lblMsgEN.Text = "Save not complete";
                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                //*** call  send SMS sub routine ***//
                                if (txt_mobile.Text.Trim() != "")
                                {
                                    string poerrc = "";
                                    string poerrm = "";
                                    //bool sms = true;
                                    ILSRSMS iLSRSMS = new ILSRSMS();
                                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                                  hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                    //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                    //                              hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm
                                    //                              , userInfo.BizInit, userInfo.BranchNo);
                                    if (!sms || poerrc == "Y")
                                    {
                                        lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                        CallHisunCustomer._dataCenter.RollbackMssql();
                                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                                        
                                        

                                        PopupMsg_judgment.ShowOnPageLoad = true;
                                        return;
                                    }
                                    res_sms = " [ส่ง SMS สำเร็จ] ";
                                }
                                else
                                {
                                    res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                                }
                            }
                        }
                        else
                        {
                            autoRejMsg = " This case return to INTERVIEW.";
                            saveStep_TCL0 = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                            if (!saveStep_TCL0)
                            {
                                CallHisunCustomer._dataCenter.RollbackMssql();
                                CallHisunCustomer._dataCenter.CloseConnectSQL();
                                
                                
                                lblMsgEN.Text = "Save not complete";
                                PopupMsg_judgment.ShowOnPageLoad = true;
                                return;
                            }
                        }


                    }
                    else if (hid_status.Value == "INTERVIEW")
                    {
                        bool saveStep;
                        saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "2", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();
                            
                            
                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }

                    }
                    else if (hid_status.Value == "KESSAI")
                    {
                        bool saveStep;
                        saveStep = CallHisunCustomer.SaveStepCustomer(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "3", progName, userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, "p1aprj");
                        if (!saveStep)
                        {
                            CallHisunCustomer._dataCenter.RollbackMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            lblMsgEN.Text = "Save not complete";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                    }
                }
                string G_Have_TCL = "1";
                if (dr_res["G_Have_TCL"].ToString().Trim() == "Y")
                //if (G_Have_TCL == "1")
                {
                    DataSet ds_csms07 = CallHisunCustomer.getCSMS07(CallHisunCustomer, hid_AppNo.Value, userInfo.BranchApp, hid_CSN.Value).Result;

                    string sql07 = "";

                    string PDGNO = "";
                    PDGNO = " " + dr_res["G_PD1"].ToString().Trim() + dr_res["G_GNO"].ToString().Trim();
                    if (!CallHisun.check_dataset(ds_csms07))
                    {
                        sql07 = " insert into  AS400DB01.CSOD0001.csms07(c07csn,c07rnk,c07tim,c07acl,c07ark,c07atm, " +
                                            " c07ac1,c07rrk,c07rtm,c07tcl,c07ftc,c07cun,c07tcn,c07flg,c07app,c07brn," +
                                            " c07apn,c07sad,c07sat,c07net,c07odg,c07fil,c07udt,c07utm, " +
                                            " c07usr,c07dsp,c07pgm) values( "
                                            + hid_CSN.Value + ","
                                            + dr_res["G_Orank"].ToString().Trim() + ","
                                            + dr_res["G_Otimes"].ToString().Trim() + ","
                                            + dr_res["G_ACL"].ToString().Trim() + ","
                                            + dr_res["G_Arank"].ToString().Trim() + ","
                                            + dr_res["G_Atimes"].ToString().Trim() + ","
                                            + dr_res["G_AACL"].ToString().Trim() + ","
                                            + dr_res["G_Rrank"].ToString().Trim() + ","
                                            + dr_res["G_Rtimes"].ToString().Trim() + ","
                                            + dr_res["G_TCL"].ToString().Trim() + ","
                                            + dr_res["G_Final_TCL"].ToString().Trim() + ","
                                            + dr_res["G_CSP"].ToString().Trim() + ","
                                            + dr_res["G_Total_CSP"].ToString().Trim() + ","
                                            + "'" + dr_res["G_Up_Down_Flag"].ToString().Trim() + "',"
                                            + "'IL',"
                                            + userInfo.BranchApp + ","
                                            + hid_AppNo.Value + ","
                                            + hid_AppDate.Value + ","
                                            + m_UpdTime + ","
                                            + dr_res["G_Net_Income"].ToString().Trim() + ","
                                            + dr_res["G_GRACE_Period"].ToString().Trim() + ","
                                            + "'" + PDGNO.Replace(".", "") + "',"      //+ "'" + "   " + "',"//+StringReplace(PDGNO,'.','',[rfReplaceAll])
                                            + Date_97 + ","
                                            + m_UpdTime + ","
                                            + "'" + userInfo.Username + "',"
                                            + "'" + userInfo.LocalClient + "',"
                                            + "'" + hid_status.Value + "')";
                    }
                    else
                    {
                        sql07 = " update  AS400DB01.CSOD0001.csms07 set " +
                                              " c07csn = " + hid_CSN.Value + "," +
                                              " c07rnk = " + dr_res["G_Orank"].ToString().Trim() + "," +
                                              " c07tim = " + dr_res["G_Otimes"].ToString().Trim() + "," +
                                              " c07acl=" + dr_res["G_ACL"].ToString().Trim() + "," +
                                              " c07ark=" + dr_res["G_Arank"].ToString().Trim() + "," +
                                              " c07atm=" + dr_res["G_Atimes"].ToString().Trim() + "," +
                                              " c07ac1=" + dr_res["G_AACL"].ToString().Trim() + "," +
                                              " c07rrk=" + dr_res["G_Rrank"].ToString().Trim() + "," +
                                              " c07rtm=" + dr_res["G_Rtimes"].ToString().Trim() + "," +
                                              " c07tcl=" + dr_res["G_TCL"].ToString().Trim() + "," +
                                              " c07ftc=" + dr_res["G_Final_TCL"].ToString().Trim() + "," +
                                              " c07cun=" + dr_res["G_CSP"].ToString().Trim() + "," +
                                              " c07tcn=" + dr_res["G_Total_CSP"].ToString().Trim() + "," +
                                              " c07flg='" + dr_res["G_Up_Down_Flag"].ToString().Trim() + "'," +
                                              " c07app= 'IL' ," +
                                              " c07brn=" + userInfo.BranchApp + "," +
                                              " c07apn=" + hid_AppNo.Value + "," +
                                              " c07sad=" + hid_AppDate.Value + "," +
                                              " c07sat= " + m_UpdTime + "," +
                                              " c07net= " + dr_res["G_Net_Income"].ToString().Trim() + "," +
                                              " c07odg= " + dr_res["G_GRACE_Period"].ToString().Trim() + "," +
                                              " c07fil= CONCAT(SUBSTRING(c07fil,1,1),'" + dr_res["G_PD1"].ToString().Trim().Replace(".", "") + dr_res["G_GNO"].ToString().Trim() + "',SUBSTRING(c07fil,15,10)) ," +
                                              " c07udt= " + Date_97 + "," +
                                              " c07utm= " + m_UpdTime + "," +
                                              " c07usr='" + userInfo.Username + "'," +
                                              " c07dsp= '" + userInfo.LocalClient + "'," +
                                              " c07pgm='" + hid_status.Value + "' " +
                                              " where c07csn=" + hid_CSN.Value + " and c07app= 'IL' and c07apn=" + hid_AppNo.Value + " " +
                                              " and c07brn=" + userInfo.BranchApp;
                    }

                    cmd.Parameters.Clear();
                    cmd.CommandText = sql07;
                    transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res_07 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result;
                    if (res_07.afrows == -1)
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();

                        Utility.WriteLogString(res_07.message.ToString(), cmd.CommandText.ToString());
                        lblMsgEN.Text = "Save not complete ";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }

                }

                CallHisunCustomer._dataCenter.CommitMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
                

                if (hid_status.Value == "INTERVIEW")
                {
                    // ** clear  filer position 28 29 **//

                    if (getDataInterview != null)
                    {
                        getDataInterview(m13cru.Replace("'", ""));
                    }
                }
                else if (hid_status.Value == "KESSAI")
                {
                    if (getDataKESSAI != null)
                    {
                        getDataKESSAI(m13aus.Replace("'", ""));
                    }
                }

                if (hid_status.Value == "STEP1")
                {
                    showAppNo = "App No. : " + hid_AppNo.Value;
                }







                lblMsgEN.Text = "Save completed." + "\r\n\r\n" + autoRejMsg + "\r\n\r\n" + res_sms + "\r\n  " + showAppNo;
                hid_validate.Value = "SAVE";
                PopupMsg_judgment.ShowOnPageLoad = true;
                //  Success //
                return;


            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            CallHisunCustomer._dataCenter.RollbackMssql();
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            lblMsgEN.Text = "Error!! Save not complete ";
            PopupMsg_judgment.ShowOnPageLoad = true;
            return;

        }
    }

    void InsertTel(string TelText, string TelAdr, string TelType, string TelExt, string refType, string refSEQ, int i_addr, string curdate, ILDataCenter busobj1)
    {
        int i = 0;
        if (TelText != "0")
        {
            CalCTel(TelText);
            for (i = 1; i <= 10; i++)
            {
                if ((i == 1) || (telno[i] != ""))
                {
                    InsertCSMS12(telno[i], TelAdr, TelType, TelExt, refType, refSEQ, i, i_addr, curdate, busobj1);
                }
            }
        }
    }

    void InsertCSMS12(string Teleno, string AddCode, string TelType, string TeleExt, string refType, string refSEQ, int iSeq, int cnt_addr, string curdate, ILDataCenter busobj2)
    {
        if (E_sub_succuss.Text == "")
        {
            //waiting reduce user connection
            ILDataCenter busobj_csms12 = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            busobj_csms12.UserInfomation = userInfo;
            DataSet DS = new DataSet();
            iDB2Command cmd1 = new iDB2Command();

            int w_icnt = 0, affectedRows = 0;
            E_sub_succuss.Text = "";

            if ((cnt_addr == 1) & (iSeq == 1))
            {
                DS = busobj_csms12.RetriveAsDataSet("select * from csms12 " +
                                             "where m12csn = " + hid_CSN.Value.Trim() + " and " +
                                             "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                             "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ");
                if (DS != null)
                {
                    if (DS.Tables[0]?.Rows.Count > 0)
                    {
                        cmd1.Parameters.Clear();
                        cmd1.CommandText = "delete from CSMS12 " +
                                            "where m12csn = " + hid_CSN.Value.Trim() + " and " +
                                            "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                            "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ";
                        affectedRows = -1;
                        try
                        {
                            affectedRows = busobj2.ExecuteNonQuery(cmd1);
                        }
                        catch (Exception ex)
                        {
                            Utility.WriteLog(ex);
                            E_sub_succuss.Text = "Error" ;
                            //L_message.Text = "Error on delete CSMS12";
                            //Control Rollback จาก E_sub_succuss.Text                        
                        }
                        if (affectedRows < 0)
                        {
                            E_sub_succuss.Text = "Error";
                            //L_message.Text = "Error on delete CSMS12";
                            //Control Rollback จาก E_sub_succuss.Text
                        }
                    }
                }
            }
            w_icnt = 0;
            DS = busobj_csms12.RetriveAsDataSet("select Max(m12seq) as max_seq from csms12 " +
                                         "where m12csn = " + hid_CSN.Value.Trim() + " and " +
                                         "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                         "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ");
            if (DS != null)
            {
                if (DS.Tables[0]?.Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0]?.Rows)
                    {
                        if (dr["max_seq"].ToString().Trim() == "")
                        {
                            w_icnt = 1;
                        }
                        else
                        {
                            w_icnt = Convert.ToInt16(dr["max_seq"].ToString().Trim()) + 1;
                        }
                    }
                    DS.Clear();
                }
            }
            if ((Teleno != "**") & (Teleno != ""))
            {
                cmd1.Parameters.Clear();
                cmd1.CommandText = "insert into CSMS12 " +
                                  "(M12CSN,m12ref,m12rsq,M12ACD,M12TTY,M12SEQ,M12TEL,M12EXT, " +
                                  "M12UDT,M12UTM,M12UUS,M12UPG,M12UWS,m12sts) " +
                                  "Values(" + hid_CSN.Value.Trim() + ", " +
                                  "'" + refType + "', " +
                                  "" + refSEQ + ", " +
                                  "'" + AddCode + "', " +
                                  "'" + TelType + "', " +
                                  "" + w_icnt.ToString() + ", " +
                                  "'" + Teleno + "', " +
                                  "'" + TeleExt + "', " +
                                  "" + curdate + ", " +
                                  "" + m_UpdTime + ", " +
                                  "'" + userInfo.Username + "', " +
                                  "'CHGDATA', " +
                                  "'" + userInfo.LocalClient + "', " +
                                  "'') ";
                affectedRows = -1;
                try
                {
                    affectedRows = busobj2.ExecuteNonQuery(cmd1);
                }
                catch (Exception ex)
                {
                    Utility.WriteLog(ex);
                    E_sub_succuss.Text = "Error";
                    //L_message.Text = "Error on Insert CSMS12";
                    //Control Rollback จาก E_sub_succuss.Text
                }
                if (affectedRows < 0)
                {
                    E_sub_succuss.Text = "Error";
                    //L_message.Text = "Error on Insert CSMS12";
                    //Control Rollback จาก E_sub_succuss.Text
                }
            }
            busobj_csms12.CloseConnectioDAL();
        }
    }
    public string CalCTel(string strPhone)
    {
        string aa = "", bb = "", strtext = "", maxtel = "";
        int i = 0, j = 0, jj = 0, jjj = 0, maxtelno = 0, TelDiff = 0;

        for (i = 1; i <= 10; i++)
        {
            telno[i] = "";
        }
        aa = strPhone;
        strtext = "";
        j = 1;
        for (i = 1; i < aa.Length + 1; i++)
        {
            bb = aa.Substring(i - 1, 1);
            if (bb == "-")
            {
                telno[j] = strtext;
                break;
            }
            else
            {
                if (bb == ",")
                {
                    telno[j] = strtext;
                    j = j + 1;
                    bb = "";
                    strtext = "";
                }
            }
            strtext = strtext + bb;
        }

        telno[j] = strtext;

        if (bb == "-")
        {
            for (jjj = i + 1; jjj <= aa.Length; jjj++)
            {
                maxtel = maxtel + aa.Substring(jjj - 1, 1);
            }

            TelDiff = Convert.ToInt32(aa.Substring(0, 9 - maxtel.Length) + maxtel) - Convert.ToInt32(telno[j]);
            maxtelno = Convert.ToInt32(telno[j]) + TelDiff;

            for (jj = j + 1; jj <= 10; jj++)
            {
                maxtel = "0" + (Convert.ToInt32(telno[jj - 1]) + 1).ToString();
                if (Convert.ToInt32(maxtel) > maxtelno)
                {
                    break;
                }
                else
                {
                    telno[jj] = "0" + (Convert.ToInt32(telno[jj - 1]) + 1).ToString();
                }
            }
        }
        return "";
    }
    private void checkAutoReject(ref string G_aprj, ref string G_loca, ref string G_reason, ref string G_Msg, ref string G_err)
    {
        try
        {
            CallHisun = new ILDataCenter();



            // check 3 blacklist provinces -- Cancel Check 3PV
            /* 
            if (txt_postcode_off_j.Text.Trim() != "" && hid_offP.Value.Trim() != "")
            {
                DataSet ds_GNTB106 = CallHisun.get_GNTB106(hid_offP.Value);

                if (CallHisun.check_dataset(ds_GNTB106))
                {
                    G_aprj = "RJ";
                    G_loca = "210";
                    G_reason = "HL1";
                    return;
                }
                else if (ds_GNTB106 == null)
                {
                    G_err = "Y";
                    lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลจังหวัดได้ กรุณาทำรายการใหม่อีกครั้ง";
                    PopupMsg_judgment.ShowOnPageLoad = true;

                    return;

                }
            }
             */
            // check salary 
            DataSet ds_CSTB05 = CallHisunCustomer.get_CSTB05().Result;
            if (!CallHisun.check_dataset(ds_CSTB05))
            {
                G_err = "Y";
                lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลเงินเดือนได้ กรุณาทำรายการใหม่อีกครั้ง";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            DataRow dr_salary = ds_CSTB05.Tables[0]?.Rows[0];
            int salary = int.Parse(txt_salary.Text.Trim().Replace(",", ""));
            if (salary <= int.Parse(dr_salary["t05inc"].ToString()))
            {
                G_aprj = "RJ";
                G_loca = "210";
                G_reason = "EL11";
                return;
            }

            DataSet ds_00 =  CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            if (CallHisun.check_dataset(ds_00))
            {
                DataRow dr = ds_00.Tables[0]?.Rows[0];
                //if (dr["M00OFC"].ToString().Trim() != txt_off_name.Text.Trim()) 
                var off_name = "";
                if (dd_off_name.Text.Trim().Length > 50)
                {
                    off_name = dd_off_name.Text.Substring(0, 50).Trim();
                }
                else
                {
                    off_name = dd_off_name.Text.Trim();
                }
                if (dr["M00OFC"].ToString().Trim() != off_name)
                {
                    G_err = "Y";
                    lblMsgEN.Text = "ชื่อบริษัทในระบบและหน้าจอไม่ตรงกัน หากชื่อบริษัทที่หน้าจอถูกต้องแล้ว กรุณากดปุ่ม Save Office & Tel";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }

                #region newChangeCheckBlacklist_O4203016
                try
                {
                    string companyName = dd_off_name.Text;
                    string title = dd_off_title.Value.ToString();
                    
                    var a = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlackList", new string[] { companyName, title });
                    CompanyBlacklist.responseCompanyBlacklist companyBlacklist = JsonConvert.DeserializeObject<CompanyBlacklist.responseCompanyBlacklist>(a);

                    if (companyBlacklist.Success == false)
                    {
                        G_err = "Y";
                        lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลบริษัทได้ กรุณาทำรายการใหม่อีกครั้ง" + "\n" + companyBlacklist.Message;
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }
                    if (companyBlacklist.data != null)
                    {
                        #region before20220901
                        //if (companyBlacklist.data.CompanyFlag == "T")
                        //{
                        //    G_aprj = "RJ";
                        //    G_loca = "210";
                        //    G_reason = "QL25";
                        //    G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a temporary blacklist company.";//O4203016 comment
                        //    return;
                        //}
                        //else if (companyBlacklist.data.CompanyFlag == "P")
                        //{
                        //    G_aprj = "RJ";
                        //    G_loca = "210";
                        //    G_reason = "QL25";
                        //    G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";//O4203016 comment
                        //    return;
                        //}
                        #endregion
                        if (companyBlacklist.data.CompanyFlag == "T")
                        {
                            #region before20220901
                            //G_aprj = "RJ";
                            //G_loca = "210";
                            //G_reason = "QL25";
                            //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a temporary blacklist company.";//O4203016 comment
                            //return;
                            #endregion
                            string WPERR = "";
                            string WPHSTS = "";
                            string WPMSG = "";
                            userInfoService = new UserInfoService();
                            userInfo = userInfoService.GetUserInfo();
                            bool res_cs = iLDataSubroutine.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                            //bool res_cs = CallHisun.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                            CallHisunCustomer._dataCenter.CloseConnectSQL();
                            
                            if (!res_cs || WPERR == "Y")
                            {
                                G_err = "Y";
                                lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง";
                                PopupMsg_judgment.ShowOnPageLoad = true;
                                return;
                            }

                            if (WPHSTS == "N")
                            {
                                #region update20220913
                                //G_aprj = "RJ";
                                //G_loca = "210";
                                //G_reason = "QL25";
                                ////G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a temporary blacklist company.";
                                //G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a temporary blacklist company.";
                                //return;
                                #endregion
                                G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a temporary blacklist company(T).";
                                hid_oper_judg.Value = "SAVE";
                                return;
                            }
                            else
                            {
                                //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a temporary blacklist company.";
                                G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a temporary blacklist company(T).";
                                hid_oper_judg.Value = "SAVE";
                                return;
                            }
                        }
                        else if (companyBlacklist.data.CompanyFlag == "P")
                        {
                            #region before20220901
                            //G_aprj = "RJ";
                            //G_loca = "210";
                            //G_reason = "QL25";
                            //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";//O4203016 comment
                            //return;
                            #endregion
                            string WPERR = "";
                            string WPHSTS = "";
                            string WPMSG = "";
                            userInfoService = new UserInfoService();
                            userInfo = userInfoService.GetUserInfo();
                            bool res_cs = iLDataSubroutine.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                            //bool res_cs = CallHisun.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                            CallHisunCustomer._dataCenter.CloseConnectSQL();
                            
                            if (!res_cs || WPERR == "Y")
                            {
                                G_err = "Y";
                                lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง";
                                PopupMsg_judgment.ShowOnPageLoad = true;
                                return;
                            }

                            if (WPHSTS == "N")
                            {
                                G_aprj = "RJ";
                                G_loca = "210";
                                G_reason = "QL25";
                                //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";
                                G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a blacklist company(P).";
                                return;
                            }
                            else
                            {
                                //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";
                                G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a blacklist company(P).";
                                hid_oper_judg.Value = "SAVE";
                                return;
                            }
                        }
                        else if (companyBlacklist.data.CompanyFlag == "S")//O4203016 flag 'S' => จับตามองพิเศษ
                        {
                            //G_Msg = "\n Office name : " + txt_off_name.Text.Trim() + " is a pending blacklist company.";
                            G_Msg = "\n Office name : " + dd_off_name.Text.Trim() + " is a pending blacklist company(S).";
                            hid_oper_judg.Value = "SAVE";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteLog(ex);
                    lblMsgEN.Text = ex.Message;
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }
                #endregion

                #region old CheckFlagBlacklist

                //string WPNME = dr["M00OFC"].ToString().Trim();
                //string WPPER = "";
                //string WPERR = "";
                //string WPOLR = "";
                //CallHisun.UserInfomation = userInfo;
                //bool res_BL = CallHisun.Call_GNSRCBST(WPNME, ref WPPER, ref WPERR, ref WPOLR, userInfo.BizInit.ToString(), userInfo.BranchNo);
                //
                //if (!res_BL || WPERR == "Y")
                //{
                //    G_err = "Y";
                //    lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลบริษัทได้ กรุณาทำรายการใหม่อีกครั้ง";
                //    PopupMsg_judgment.ShowOnPageLoad = true;
                //    return;
                //}
                //if (WPPER == "T")
                //{
                //    G_Msg = "Office name : " + txt_off_name.Text.Trim() + " is a pending blacklist company ";
                //    return;
                //}
                //else if (WPPER == "P")
                //{
                //    WPERR = "";
                //    string WPHSTS = "";
                //    string WPMSG = "";
                //    bool res_cs = CallHisun.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                //    
                //    if (!res_cs || WPERR == "Y")
                //    {
                //        G_err = "Y";
                //        lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง";
                //        PopupMsg_judgment.ShowOnPageLoad = true;
                //        return;
                //    }

                //    if (WPHSTS == "N")
                //    {
                //        G_aprj = "RJ";
                //        G_loca = "210";
                //        G_reason = "QL25";
                //        return;
                //    }
                //    else
                //    {
                //        G_Msg = "Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";
                //        hid_oper_judg.Value = "SAVE";
                //        return;
                //    }
                //}
                #endregion
            }
            else
            {
                G_err = "Y";
                lblMsgEN.Text = "ยังไม่มีข้อมูลลูกค้าอยู่ในระบบ กรุณาทำการ Save Office & Tel ก่อน";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }



        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }


    #endregion


    #region function bind master data
    // ***  Judgment form ***//
    private void bind_J_ApplyType(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfo);
            DataSet ds = new DataSet();
            if (Cache["ds_J_ApplyType"] != null)
            {
                ds = (DataSet)Cache["ds_J_ApplyType"];
            }
            else
            {
                ds = CallHisunMaster.getApplyType();
                Cache["ds_J_ApplyType"] = ds;
                Cache.Insert("ds_J_ApplyType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            dd_applyType_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_applyType_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_applyType_j.Items.Add(
                        new ListEditItem(dr["gn61cd"].ToString().Trim() + " : " + dr["gn61dt"].ToString().Trim(), dr["gn61cd"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_applyType_j.Value = code;
                }

            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_ApplyVia(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfo);

            DataSet ds = new DataSet();
            if (Cache["ds_J_ApplyVia"] != null)
            {
                ds = (DataSet)Cache["ds_J_ApplyVia"];
            }
            else
            {
                ds = CallHisunMaster.getApplyVia();
                Cache["ds_J_ApplyVia"] = ds;
                Cache.Insert("ds_J_ApplyVia", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            dd_apply_via_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_apply_via_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_apply_via_j.Items.Add(
                        new ListEditItem(dr["gn16cd"].ToString().Trim() + " : " + dr["gn16dt"].ToString().Trim(), dr["gn16cd"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_apply_via_j.Value = code;
                }
            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_ApplyChannel(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

            DataSet ds = new DataSet();

            if (Cache["ds_J_ApplyChannel"] != null)
            {
                ds = (DataSet)Cache["ds_J_ApplyChannel"];
            }
            else
            {
                ds = CallHisunMaster.getApplyChannel();
                Cache["ds_J_ApplyChannel"] = ds;
                Cache.Insert("ds_J_ApplyChannel", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            dd_apply_channel_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_apply_channel_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_apply_channel_j.Items.Add(
                        new ListEditItem(dr["gs16cd"].ToString().Trim() + " : " + dr["gs16dt"].ToString().Trim(), dr["gs16cd"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_apply_channel_j.Value = code;
                }
            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_ApplySubChannel(string channel, string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

            DataSet ds = CallHisunMaster.getApplySubChannel(channel);
            dd_subChannel_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_subChannel_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_subChannel_j.Items.Add(
                        new ListEditItem(dr["gs17sc"].ToString().Trim() + " : " + dr["gs17dt"].ToString().Trim(), dr["gs17sc"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_subChannel_j.Value = code;
                }
            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_Marital(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ILDataCenterMssql CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
           // conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            if (Cache["ds_MaritalStatusN"] != null)
            {
                ds = (DataSet)Cache["ds_MaritalStatusN"];
            }
            else
            {
                //var resultMarital = conn_general.GetGeneralMaritalStatus();
                //ds.Clear();
                // ds.Tables.Add(conn_general.GetGeneralMaritalStatus());
                ds = CallHisunMaster.getGeneralCenter("MaritalStatusID");
                Cache["ds_MaritalStatusN"] = ds;
                Cache.Insert("ds_MaritalStatusN", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getMaritalStatus("");

            dd_marital_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_marital_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_marital_j.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_marital_j.Value = code;
                //}

                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_marital_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_marital_j.SelectedIndex = dd_marital_j.Items.IndexOf(dd_marital_j.Items.FindByText(values));

                }

            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_SubMaritalStatus(string maritalSts, string subMaritalcode = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
          //  conn_general = new Connect_GeneralAPI();

            //DataSet ds = CallHisun.getSubMaritalStatus(maritalSts, "");
            DataSet ds = CallHisunMaster.getSubMaritalStatus("", "");
            dd_subMarital_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_subMarital_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_subMarital_j.Items.Add(
                        new ListEditItem(dr["S44SMT"].ToString().Trim() + " : " + dr["S44TDS"].ToString().Trim(), dr["S44SMT"].ToString().Trim()));
                }

                //if (maritalSts != "" && subMaritalcode != "")
                //{
                //    dd_subMarital_j.Value = subMaritalcode;
                //}
                string values = "";

                if (maritalSts != "" && subMaritalcode != "")
                {
                    foreach (var selected in dd_subMarital_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == subMaritalcode.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_subMarital_j.SelectedIndex = dd_subMarital_j.Items.IndexOf(dd_subMarital_j.Items.FindByText(values));

                }
                else
                {
                    dd_subMarital_j.SelectedIndex = -1;
                }
            }

        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_ResidentType(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            //  conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            if (Cache["ds_ResidentType"] != null)
            {
                ds = (DataSet)Cache["ds_ResidentType"];
            }
            else
            {
                //var resultResidentType = conn_general.getResidentType();
                //ds.Clear();
                //ds.Tables.Add(conn_general.getResidentType());
                //ds = CallHisun.getResidentType("");
                ds = CallHisunMaster.getGeneralCenter("ResidentalStatusID");
                Cache["ds_ResidentType"] = ds;
                Cache.Insert("ds_ResidentType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getResidentType("");
            dd_resident_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_resident_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_resident_j.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_resident_j.Value = code;
                //}

                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_resident_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_resident_j.SelectedIndex = dd_resident_j.Items.IndexOf(dd_resident_j.Items.FindByText(values));

                }
            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_BusinessType(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            if (Cache["ds_J_BusinessType"] != null)
            {
                ds = (DataSet)Cache["ds_J_BusinessType"];
            }
            else
            {
                ds = CallHisunMaster.getBusinessType("");

                //ds = CallHisun.getBusinessType("");
                Cache["ds_J_BusinessType"] = ds;
                Cache.Insert("ds_J_BusinessType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getBusinessType("");
            dd_busType_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_busType_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_busType_j.Items.Add(
                        new ListEditItem(dr["GN22CD"].ToString().Trim() + " : " + dr["GN22DT"].ToString().Trim(), dr["GN22CD"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_busType_j.Value = code;
                //}

                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_busType_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_busType_j.SelectedIndex = dd_busType_j.Items.IndexOf(dd_busType_j.Items.FindByText(values));

                }

            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }

    private void bind_J_Occupation(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            // conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (Cache["ds_J_Occupation"] != null)
            {
                ds = (DataSet)Cache["ds_J_Occupation"];
            }
            else
            {
                //var resultOccupation = conn_general.GetGeneralOccupation();
                //ds.Clear();
                ds = CallHisunMaster.getGeneralCenter("OccupationID");
              //  ds.Tables.Add(conn_general.GetGeneralOccupation());
                Cache["ds_J_Occupation"] = ds;
                Cache.Insert("ds_J_Occupation", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getOccupation("");
            dd_occup_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_occup_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_occup_j.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_occup_j.Value = code;
                //}

                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_occup_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_occup_j.SelectedIndex = dd_occup_j.Items.IndexOf(dd_occup_j.Items.FindByText(values));

                }
            }
        }
        catch (Exception ex)
        {
            //lblMsgEN.Text = ex.Message.ToString();
            Utility.WriteLog(ex);
        }
    }
     
    private void bind_J_Comercial(string occ_code, string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getCommercialRegister("");
            DataSet ds = CallHisunMaster.getCommercialRegister(occ_code);
            dd_comerc.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_comerc.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_comerc.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["desc"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                //if (code.Trim() != "")
                //{
                //    dd_comerc.Value = code;
                //    //dd_comerc.Enabled = false;
                //}
                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_comerc.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_comerc.SelectedIndex = dd_comerc.Items.IndexOf(dd_comerc.Items.FindByText(values));

                }
                else
                {
                    dd_comerc.SelectedIndex = -1;
                    //dd_comerc.Enabled = true;
                }
                dd_comerc.Enabled = true;

            }
            else
            {
                dd_comerc.SelectedIndex = -1;
                dd_comerc.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    private void bind_J_Position(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getPosition("");

            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
          //  conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            if (Cache["ds_J_Position"] != null)
            {
                ds = (DataSet)Cache["ds_J_Position"];
            }
            else
            {
                //var resultPosition = conn_general.GetGeneralPosition();
                //ds.Clear();
                //ds.Tables.Add(conn_general.GetGeneralPosition());
                ds = CallHisunMaster.getGeneralCenter("PositionID");
                Cache["ds_J_Position"] = ds;
                Cache.Insert("ds_J_Position", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_position_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_position_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_position_j.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                //if (code != "")
                //{
                //    dd_position_j.Value = code;
                //}
                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_position_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_position_j.SelectedIndex = dd_position_j.Items.IndexOf(dd_position_j.Items.FindByText(values));

                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_EmployeeType(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getEmployeeType("");

            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            if (Cache["ds_J_EmployeeType"] != null)
            {
                ds = (DataSet)Cache["ds_J_EmployeeType"];
            }
            else
            {
                //ds = CallHisun.getEmployeeType("");
                ds = CallHisunMaster.getEmployeeType("");
                Cache["ds_J_EmployeeType"] = ds;
                Cache.Insert("ds_J_EmployeeType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_empType_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_empType_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_empType_j.Items.Add(
                        new ListEditItem(dr["GN68CD"].ToString().Trim() + " : " + dr["GN68DT"].ToString().Trim(), dr["GN68CD"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_empType_j.Value = code;
                //}
                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_empType_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_empType_j.SelectedIndex = dd_empType_j.Items.IndexOf(dd_empType_j.Items.FindByText(values));

                }
                else
                {
                    dd_empType_j.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    private void bind_J_SalaryType(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getSalaryType("");
            bool checkCode = false;
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            if (Cache["ds_J_SalaryType"] != null)
            {
                ds = (DataSet)Cache["ds_J_SalaryType"];
            }
            else
            {
                //ds = CallHisun.getSalaryType("");
                ds = CallHisunMaster.getSalaryType("");
                Cache["ds_J_SalaryType"] = ds;
                Cache.Insert("ds_J_SalaryType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_incomeType_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_incomeType_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_incomeType_j.Items.Add(
                        new ListEditItem(dr["GN17CD"].ToString().Trim() + " : " + dr["GN17DT"].ToString().Trim(), dr["GN17CD"].ToString().Trim()));
                    if (dr["GN17CD"].ToString().Trim() == code)
                    {
                        checkCode = true;
                    }
                }

                if (code != "" && checkCode)
                {
                    dd_incomeType_j.Value = code;
                }
                else
                {
                    dd_incomeType_j.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_TypeCust(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();


            ds = CallHisunCustomer.getGNTS18().Result;


            dd_typeCust_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_typeCust_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_typeCust_j.Items.Add(
                        new ListEditItem(dr["gs18sc"].ToString().Trim() + " : " + dr["GS18DT"].ToString().Trim(), dr["gs18sc"].ToString().Trim()));
                }

                //if (code != "")
                //{
                //    dd_typeCust_j.Value = code;
                //}
                string values = "";

                if (code != "")
                {
                    foreach (var selected in dd_typeCust_j.Items)
                    {
                        var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();

                        }
                    }
                    dd_typeCust_j.SelectedIndex = dd_typeCust_j.Items.IndexOf(dd_typeCust_j.Items.FindByText(values));

                }
                else
                {
                    dd_typeCust_j.Enabled = false;
                    dd_typeCust_j.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_IncomeStatement(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getSlipDocType();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            if (Cache["ds_J_IncomeStatement"] != null)
            {
                ds = (DataSet)Cache["ds_J_IncomeStatement"];
            }
            else
            {
                ds = CallHisunMaster.getslipdoc();
                //ds = CallHisun.getSlipDocType();
                Cache["ds_J_IncomeStatement"] = ds;
                Cache.Insert("ds_J_IncomeStatement", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_statement_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_statement_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_statement_j.Items.Add(
                        new ListEditItem(dr["GS26CD"].ToString().Trim() + " : " + dr["GS26DT"].ToString().Trim() + "[" + dr["gs25de"].ToString().Trim() + "]", dr["GS26CD"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_statement_j.Value = code;
                }
                else
                {
                    dd_statement_j.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_TypeOfEmployee(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getEmployeeType("");
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            DataSet ds = new DataSet();
            if (Cache["ds_J_TypeOfEmployee"] != null)
            {
                ds = (DataSet)Cache["ds_J_TypeOfEmployee"];
            }
            else
            {
                ds = CallHisunMaster.getEmployeeType("");
                //ds = CallHisun.getEmployeeType("");
                Cache["ds_J_TypeOfEmployee"] = ds;
                Cache.Insert("ds_J_TypeOfEmployee", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }



            dd_empType_j.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_empType_j.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_empType_j.Items.Add(
                        new ListEditItem(dr["GN68CD"].ToString().Trim() + " : " + dr["GN68DT"].ToString().Trim(), dr["GN68CD"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_empType_j.Value = code;
                }
                else
                {
                    dd_empType_j.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_OfficeTitle(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getGNMB20("2");
            //  conn_general = new Connect_GeneralAPI();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (Cache["ds_J_OfficeTitle"] != null)
            {
                ds = (DataSet)Cache["ds_J_OfficeTitle"];
            }
            else
            {
                //var resultOfficeTitle = conn_general.GetGeneralOfficeTitle();
                //resultOfficeTitle.TableName = "OfficeTitle";
                //ds.Tables.Clear();
                //if (ds.Tables.Count > 0)
                //    ds.Tables.Remove(resultOfficeTitle);

                //ds.Tables.Add(resultOfficeTitle);
                //ds.Tables.Add(conn_general.GetGeneralOfficeTitle());
                ds = CallHisunMaster.getGeneralCenter("OfficeTitleID");
                Cache["ds_J_OfficeTitle"] = ds;
                Cache.Insert("ds_J_OfficeTitle", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            dd_off_title.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_off_title.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_off_title.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                string values = "";

                if (code != "")
                {
                   foreach(var selected in dd_off_title.Items)
                    {
                       var split = selected.ToString().Split(':');
                        string codeCondition = split[0];
                        if (codeCondition.ToString().Trim() == code.ToString().Trim())
                        {
                            values = selected.ToString();
                            
                        }
                    }
                    dd_off_title.SelectedIndex = dd_off_title.Items.IndexOf(dd_off_title.Items.FindByText(values));

                }
                else
                {
                    dd_off_title.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_Date_of_Income(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getGNTB71();
            //  conn_general = new Connect_GeneralAPI();
            CallHisun = new ILDataCenter();
            DataSet ds = new DataSet();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            if (Cache["ds_J_Date_of_Income"] != null)
            {
                ds = (DataSet)Cache["ds_J_Date_of_Income"];
            }
            else
            {
                //ds = CallHisun.getGNTB71();
                //var resultSalaryTime = conn_general.getSalaryTime();
                //ds.Clear();
                // ds.Tables.Add(conn_general.getSalaryTime());
                ds = CallHisunMaster.getGeneralCenter("SalaryTimeID");
                Cache["ds_J_Date_of_Income"] = ds;
                Cache.Insert("ds_J_Date_of_Income", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_dateOfIncome.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_dateOfIncome.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_dateOfIncome.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_dateOfIncome.Value = code;
                }
                else
                {
                    dd_dateOfIncome.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void bind_J_contract(string code = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            //CallHisun = new ILDataCenter();
            //DataSet ds = CallHisun.getGNTB70();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            DataSet ds = new DataSet();
            if (Cache["ds_J_contract"] != null)
            {
                ds = (DataSet)Cache["ds_J_contract"];
            }
            else
            {
                //ds = CallHisun.getGNTB70();
                ds = CallHisunMaster.getContract();
                Cache["ds_J_contract"] = ds;
                Cache.Insert("ds_J_contract", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }



            dd_contact_judg.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_contact_judg.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_contact_judg.Items.Add(
                        new ListEditItem(dr["gn70cd"].ToString().Trim() + " : " + dr["gn70dt"].ToString().Trim(), dr["gn70cd"].ToString().Trim()));
                }

                if (code != "")
                {
                    dd_contact_judg.Value = code;
                }
                else
                {
                    dd_contact_judg.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }




    #endregion
    #region validate 
    private bool checkBeforeCal_time_1(ref string err)
    {
        bool res = true;

        try
        {
            CallHisun = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun.UserInfomation = userInfoService.GetUserInfo();

            try
            {
                if (txt_birthDate_j.Text.Length == 10)
                {
                    string Age = "";
                    string Error = "";
                    string birthdate = txt_birthDate_j.Text.Replace("/", "");
                    //Mock ไม่ได้
                    bool resGNP0371 = iLDataSubroutine.CALL_GNP0371(birthdate, "", "DMY", "B", "", "IL", "", userInfo.BizInit, userInfo.BranchNo, ref Age, ref Error);
                    //bool resGNP0371 = CallHisun.CALL_GNP0371(birthdate, "", "DMY", "B", "", "IL", "", userInfo.BizInit, userInfo.BranchNo, ref Age, ref Error);
                    if (resGNP0371 == false || Error.Trim() == "Y")
                    {
                        err += "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + "\r\n";
                        
                        //PopupMsg_judgment.ShowOnPageLoad = true;
                        txt_birthDate_j.Text = "";
                        txt_birthDate_j.Focus();
                        res = false;
                    }
                    //CALL_GNP014
                    string oerrCHK = "";
                    string oNamed = "";

                    //Mock ไม่ได้
                    bool resGNP014 = iLDataSubroutine.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    //bool resGNP014 = CallHisun.CALL_GNP014(birthdate.Substring(0, 2) + birthdate.Substring(2, 2) + birthdate.Substring(6, 2), "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                    if (resGNP014 == false || oerrCHK.Trim() == "Y")
                    {
                        err += "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + "\r\n";
                        
                        //PopupMsg_judgment.ShowOnPageLoad = true;
                        txt_birthDate_j.Text = "";
                        txt_birthDate_j.Focus();
                        res = false;
                    }

                    txt_age.Text = int.Parse(Age.Trim()).ToString();
                    lb_day.Text = oNamed.Trim();
                    dd_marital_j.Focus();
                    
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่" + "\r\n";
                
                PopupMsg_judgment.ShowOnPageLoad = true;
                txt_birthDate_j.Text = "";
                txt_birthDate_j.Focus();
            }


            if (dd_off_title.Value.ToString() == "")
            {
                err += "กรุณาระบุ คำนำหน้าบริษัท" + "\r\n";
                res = false;
            }
            //if (txt_off_name.Text.Trim() == "")
            if (dd_off_name.Text.Trim() == "")
            {
                err += "กรุณาระบุ ชื่อบริษัท" + "\r\n";
                res = false;
            }
            if (txt_off_phone.Text.Trim() == "")
            {
                err += "กรุณาระบุ เบอร์โทรศัพท์บริษัท" + "\r\n"; 
                res = false;
            }
            //if (txt_h_tel.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ เบอร์โทรศัพท์บ้าน";
            //    return false;
            //}
            //if (txt_mobile.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ เบอร์มือถือ";
            //    return false;
            //}

            //***  check telephone number ***//
            string prmError = "";
            string prmErrorMsg = "";
            string prmOTYPE = "";
            bool res48_off = iLDataSubroutine.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            //bool res48_off = CallHisun.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            
            if (!res48_off || prmError == "Y")
            {
                err += prmErrorMsg + "\r\n";
                res = false;
            }
            if (prmOTYPE != "P")
            {
                err += "เบอร์ออฟฟิศ รูปแบบไม่ถูกต้อง" + "\r\n";
                res = false;
            }

            prmError = "";
            prmErrorMsg = "";

            if (txt_h_tel.Text.Trim() != "")
            {
                bool res48_h = iLDataSubroutine.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_h = CallHisun.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_h || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n"; 
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์บ้าน รูปแบบไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            else
            {
                bool chk_ext = CallHisun.checkTelExt(txt_h_tel.Text.Trim(), txt_h_ext.Text.Trim());
                if (!chk_ext)
                {
                    err += " เบอร์บ้าน Ext. ไม่ถูกต้อง" + "\r\n"; 
                    res = false;
                }
            }

            prmError = "";
            prmErrorMsg = "";

            if (txt_mobile.Text.Trim() != "")
            {
                bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_mobile = CallHisun.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_mobile || prmError == "Y")
                {
                    err += prmErrorMsg + "\r\n";
                    res = false;
                }
                if (prmOTYPE != "M")
                {
                    err += "เบอร์ Mobile รูปแบบไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            //********** Check Telephone number TO ************//


            if (txt_off_phone.Text.Trim() != "" && txt_off_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()))
                {
                    err += "กรุณาระบุ เบอร์ติดต่อ(Office)ให้ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            if (txt_h_tel.Text.Trim() != "" && txt_h_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()))
                {
                    err += "กรุณาระบุ เบอร์ติดต่อ(บ้าน)ให้ถูกต้อง" + "\r\n";
                    res = false;
                }
            }

            DataSet ds_11 = CallHisunCustomer.getCSMS11(hid_CSN.Value).Result;
            if (CallHisun.check_dataset(ds_11))
            {
                //*** check Home ***//
                DataRow[] res_H = ds_11.Tables[0]?.Select("M11CDE = 'H' ");
                //*** check Office ***//
                DataRow[] res_O = ds_11.Tables[0]?.Select("M11CDE = 'O' ");

                if (res_H != null && res_H.Count() > 0)
                {
                    if (res_H[0]["M11MOB"].ToString().Trim() != txt_mobile_j.Text.Trim())
                    {
                        err += "กรุณาระบุ เบอร์มือถือต้องตรงกัน หากเบอร์มือถือที่หน้าจอตรงกันแล้วต้องทำการ Save Office & Tel เสียก่อน" + "\r\n"; 
                        res = false;
                    }
                    if (res_H[0]["M11TEL"].ToString().Trim() != CallHisun.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()))
                    {
                        err += "กรุณาระบุ เบอร์บ้านต้องตรงกับรหัสไปรษณีย์ หากเบอร์บ้านที่หน้าจอตรงกับรหัสไปรษณีย์แล้วแล้วต้องทำการ Save Office & Tel เสียก่อน" + "\r\n"; 
                        res = false;
                    }
                    if ((res_H[0]["M11TEL"].ToString().Trim() != "") & (hid_provH.Value.Trim() != ""))
                    {
                        var checkProvince = CallHisunCustomer.CheckProvinceNA(hid_CSN.Value).Result;
                        if (!CallHisun.check_dataset(checkProvince))
                        {
                            bool res49_h = iLDataSubroutine.Call_GNSR49(res_H[0]["M11TEL"].ToString().Trim(), hid_provH.Value.PadLeft(3, '0'), ref prmError, userInfo.BizInit, userInfo.BranchNo);
                            //bool res49_h = CallHisun.Call_GNSR49(res_H[0]["M11TEL"].ToString().Trim(), hid_provH.Value, ref prmError, userInfo.BizInit, userInfo.BranchNo);

                            if (prmError == "Y")
                            {
                                err += "เบอร์โทรศัพท์บ้านไม่สัมพันธ์กับรหัสจังหวัด" + "\r\n";
                                res = false;
                            }
                        }
                    }

                }


            }


            if (txt_mobile.Text.Trim() != txt_mobile_j.Text.Trim())
            {
                err += "กรุณาระบุ เบอร์มือถือต้องตรงกัน " + "\r\n";
                res = false;
            }

            //if ((txt_h_tel.Text.Trim() != "") & (hid_provH.Value.Trim() != ""))
            //{
            //    bool res49_h = CallHisun.Call_GNSR49(txt_h_tel.Text.Trim(), hid_provH.Value, ref prmError, userInfo.BizInit, userInfo.BranchNo);
            //    
            //    if (prmError == "Y")
            //    {
            //        err += "เบอร์โทรศัพท์บ้านไม่สัมพันธ์กับรหัสจังหวัด" + "\r\n";
            //        res = false;
            //    }
            //}
            //************************************************//

            if (dd_applyType_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 1. Apply type" + "\r\n";
                res = false;
            }
            if (dd_apply_via_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 2. Apply via" + "\r\n";
                res = false;
            }
            if (dd_apply_channel_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 3. Apply channel" + "\r\n";
                res = false;
            }
            //if (dd_subChannel_j.Value.ToString()  == "") 
            //{
            //    err = "กรุณาระบุ 4. sub apply channel";
            //    return false;
            //}
            if (txt_age.Text.Trim() == "")
            {
                err += "กรุณาระบุ 5. อายุ" + "\r\n";
                res = false;
            }
            if (txt_birthDate_j.Text.Trim() == "")
            {
                err += "กรุณาระบุ 6. วันเดือนปีเกิด" + "\r\n";
                res = false;
            }
            //string[] birthDateC = txt_birthDate_j.Text.Split('/');
            //string birthDate1 = birthDateC[2] + birthDateC[1] + birthDateC[0];
            string birthDate1 = txt_birthDate_j.Text.Trim();
            string realBirth = hid_birthDate.Value.Trim();
            if(!realBirth.Contains(birthDate1))
            //if (birthDate1 != realBirth)
            {
                err += "ว.ด.ป.เกิดไม่ตรง Int." + "\r\n";
                //err += " Birth Date ไม่ตรงกับในระบบ ไม่สามารถ Approve ได้ กรุณาแก้ไข" + "\r\n"; 
                res = false;
            }


            if (dd_marital_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 7. สถานภาพ" + "\r\n";
                res = false;
            }
            if (dd_subMarital_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ สถานภาพย่อย" + "\r\n";
                res = false;
            }
            string subMarital = dd_subMarital_j.Value.ToString().Trim();
            string marital = dd_marital_j.Value.ToString().Trim();
            string errSub = "";
            if (!CallHisun.checkSubMarital(subMarital, int.Parse(txt_child.Text), int.Parse(txt_age.Text), ref errSub, marital))
            {
                err += errSub + "\r\n";
                res = false;
            }
            if (rb_sex.Value.ToString() == "")
            {
                err += "กรุณาระบุ 9. เพศ" + "\r\n";
                res = false;
            }
            DataSet ds_00 = CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            if (!CallHisun.check_dataset(ds_00))
            {
                err += "ไม่สามารถตรวจสอบข้อมูลได้ กรุณาลองใหม่อีกครั้ง" + "\r\n";
                res = false;
            }

            DataRow dr_00 = ds_00.Tables[0]?.Rows[0];
            DataSet ds_title = new DataSet();
            string condition = "id = " + dr_00["M00TTL"].ToString().Trim();
            //ds_title.Tables.Add(conn_general.GetGeneralTitleName());
            ds_title = CallHisunMaster.getGeneralCenter("TitleID");
            if (CallHisun.check_dataset(ds_title))
            {
                DataRow[] dr_title = ds_title.Tables[0]?.Select(condition);
                if (dr_title[0]["ShortName"].ToString().Trim() != "")
                {
                    if (hid_sex.Value != "")
                    {
                        if (rb_sex.Value.ToString() != hid_sex.Value)
                        {
                            err += " ข้อมูล 09. เพศ ไม่สัมพันธ์กับข้อมูลในระบบ" + "\r\n";
                            res = false;
                        }
                    }
                }
            }
            else
            {
                if (hid_sex.Value != "")
                {
                    if (rb_sex.Value.ToString() != hid_sex.Value)
                    {
                        err += " ข้อมูล 09. เพศ ไม่สัมพันธ์กับข้อมูลในระบบ" + "\r\n";
                        res = false;
                    }
                }
            }

            if ((txt_yearResident.Text.Trim() == "" || txt_yearResident.Text.Trim() == "0") && (txt_monthResident.Text.Trim() == "" || txt_monthResident.Text.Trim() == "0"))
            {
                err += "กรุณาระบุ 10. ปีที่อยู่อาศัย" + "\r\n";
                res = false;
            }
            if (txt_yearResident.Text.Trim() == "")
            {
                err += "กรุณาระบุ 10. ปีที่อยู่อาศัย" + "\r\n";
                res = false;
            }
            if (dd_resident_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 10. ลักษณะที่อยู่อาศัย" + "\r\n";
                res = false;
            }

            if (int.Parse(txt_monthResident.Text.Trim()) > 11)
            {
                err += "จำนวนเดือนที่อยู่อาศัยต้องไม่เกิน 11 เดือน" + "\r\n";
                res = false;
            }

            if (txt_fPerson.Text.Trim() == "" || txt_fPerson.Text.Trim() == "0")
            {
                err += "กรุณาระบุ 11. จำนวนคนในครอบครัว" + "\r\n";
                res = false;
            }



            //**  check days of resident **//
            //string birthDate = ;//txt_birthDate_j.Text.Trim();
            int age = CallHisunMaster.computeAge(birthDate1);

            //** time of resident **//


            if ((int.Parse(txt_yearResident.Text.Trim()) * 12) + int.Parse(txt_monthResident.Text.Trim()) > (age * 12))
            {
                err += "ระยะเวลาที่อยู่อาศัยต้องไม่มากกว่าอายุของตัวเอง" + "\r\n";
                res = false;
            }

            if (dd_busType_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ  13. ประเภทธุรกิจ" + "\r\n";
                res = false;
            }
            if (dd_occup_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ  14. อาชีพ" + "\r\n";
                res = false;
            }

            if (dd_occup_j.Value.ToString() == "011" || dd_occup_j.Value.ToString() == "012")
            {
                if (dd_comerc.SelectedItem.Value.Equals(""))
                {
                    err += "กรณีลูกค้าเป็นเจ้าของธุรกิจ จะต้องระบุว่านำใบทะเบียนการค้ามาหรือไม่" + "\r\n";
                    res = false;
                }
            }
            else
            {
                if (!(dd_comerc.Value == null))
                {
                    err += " ต้องไม่ระบุนำใบทะเบียนการค้า " + "\r\n";
                    res = false;
                }
            }
            if (dd_position_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ  15. ตำแหน่ง" + "\r\n";
                res = false;
            }

            if (txt_empNo.Text.Trim() == "" || txt_empNo.Text == "0")
            {
                err += "กรุณาระบุ 16.จำนวนพนักงาน" + "\r\n";
                res = false;
            }
            if (txt_s_year.Text.Trim() == "")
            {
                err += " กรุณาระบุ 17. อายุงาน" + "\r\n";
                res = false;
            }

            if (int.Parse(txt_s_month.Text.Trim()) > 11)
            {
                err += " กรุณาใส่เดือนของอายุงานให้ถูกต้อง " + "\r\n";
                res = false;
            }

            if (txt_s_year.Text.Trim() == "0" && txt_s_month.Text.Trim() == "0")
            {
                err += " กรุณาระบุอายุงานให้ถูกต้อง" + "\r\n";
                res = false;
            }
            int ageService = 15;
            if (int.Parse(txt_age.Text.Trim()) - int.Parse(txt_s_year.Text.Trim()) < ageService)
            {
                err += " ลูกค้าทำงานก่อนอายุ 15 ปี กรุณาตรวจสอบอายุงานอีกครั้ง" + "\r\n";
                res = false;
            }



            if (dd_incomeType_j.Value != null && dd_incomeType_j.Value.ToString() == "01")
            {
                if (dd_occup_j.Value.ToString() != "011" && dd_occup_j.Value.ToString() != "012")
                {

                    if (dd_typeCust_j.SelectedItem.Value.Equals(""))
                    {
                        err += "กรุณาระบุ ประเภทลูกค้า " + "\r\n";
                        res = false;
                    }
                }
            }
            else
            {
                if (dd_occup_j.Value.ToString() == "011" && dd_occup_j.Value.ToString() == "012")
                {
                    if (!dd_typeCust_j.SelectedItem.Value.Equals(""))
                    {
                        err += "ไม่ต้องระบุ ประเภทลูกค้า " + "\r\n";
                        res = false;
                    }
                }
            }
            if (dd_empType_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 18. ประเภทการจ้างงาน" + "\r\n";
                res = false;
            }

            if (dd_busType_j.Value.ToString() == "17" || dd_occup_j.Value.ToString() == "060" || dd_position_j.Value.ToString() == "006" || dd_empType_j.Value.ToString() == "06")
            {
                if (!(dd_busType_j.Value.ToString() == "17" && dd_occup_j.Value.ToString() == "060" && dd_position_j.Value.ToString() == "006" && dd_empType_j.Value.ToString() == "06"))
                {
                    err += "ประเภทธุรกิจ, อาชีพ, ตำแหน่ง, ประเภทการจ้างงาน กรณีที่เลือกหัวข้อใดหัวข้อหนึ่งเป็น 'ว่างงาน' ทุกหัวข้อที่เหลือต้องเลือกเป็น 'ว่างงาน' ทั้งหมด" + "\r\n";
                    res = false;
                }
            }

            if (dd_statement_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ  เอกสารแสดงรายได้" + "\r\n";
                res = false;
            }
            if (hid_status.Value == "KESSAI")
            {
                if (hid_incomeDoc.Value.Trim() != dd_statement_j.Value.ToString().Trim())
                {
                    err += "เอกสารแสดงรายได้ไม่ตรง Int." + "\r\n";
                    //err += "เอกสารแสดงรายได้ ไม่ตรงกับข้อมูลที่มีในระบบ" + "\r\n";
                    res = false;
                }
            }



            if (dd_incomeType_j.Value != null && dd_incomeType_j.Value.ToString() == "")
            {
                err += "กรุณาระบุ 19. ลักษณะการรับเงินเดือน" + "\r\n";
                res = false;
            }
            if (txt_salary.Text.Trim() == "0" || txt_salary.Text.Trim() == "")
            {
                err += "กรุณาระบุ 20. รายได้ต่อเดือน" + "\r\n";
                res = false;
            }
            if (txt_salary_adj.Text.Trim() == "")
            {
                err += "Salary Adjust ต้องไม่เป็นค่าว่าง ถ้าไม่มีกรุณาระบุ 0" + "\r\n";
                res = false;
            }
            if (hid_status.Value == "KESSAI")
            {
                if (hid_salary.Value.Trim().Replace(",", "") != txt_salary.Text.Trim().Replace(",", ""))
                {
                    err += "รายได้ไม่ตรง Int." + "\r\n";
                    //err += "Salary ไม่ตรงกับข้อมูลที่มีอยู่ในระบบ" + "\r\n";
                    res = false;
                }
            }



            //if (txt_mobile_j.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ 21. เบอร์โทรศัพท์มือถือ ";
            //    return false;
            //}
            if (txt_postcode_j.Text.Trim() == "")
            {
                err += "กรุณาระบุ 23. รหัสไปรณีย์ (Home)" + "\r\n";
                res = false;
            }
            if (txt_postcode_off_j.Text.Trim() == "")
            {
                err += "กรุณาระบุ 24. รหัสไปรณีย์ (Office)" + "\r\n";
                res = false;
            }

            if (txt_amount_j.Text.Trim() == "")
            {
                err += "กรุณาระบุ 25. วงเงินที่ต้องการ" + "\r\n";
                res = false;
            }
            //Pact
            if (hid_status.Value == "STEP1")
            {
                if (txt_postcode_idcard_j.Text.Trim() == "")
                {
                    err += "กรุณาระบุ รหัสไปรณีย์ (ID)" + "\r\n";
                    res = false;
                }
            }


            //if (dd_dateOfIncome.Value.ToString()  == "") 
            //{
            //    err = "กรุณาระบุ 27. วันที่เงินเดือนออก";
            //    return false;
            //}

            return res;
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            
            err += " Cannot validate data , Please try again.";
            return false;
        }
    }
    private bool checkBeforeCal_time_1_back(ref string err)
    {
        
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun.UserInfomation = userInfoService.GetUserInfo();

            if (dd_off_title.Value.ToString() == "")
            {
                err = "กรุณาระบุ คำนำหน้าบริษัท";
                return false;
            }
            //if (txt_off_name.Text.Trim() == "") 
            if (dd_off_name.Text.Trim() == "")
            {
                err = "กรุณาระบุ ชื่อบริษัท";
                return false;
            }
            if (txt_off_phone.Text.Trim() == "")
            {
                err = "กรุณาระบุ เบอร์โทรศัพท์บริษัท";
                return false;
            }
            //if (txt_h_tel.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ เบอร์โทรศัพท์บ้าน";
            //    return false;
            //}
            //if (txt_mobile.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ เบอร์มือถือ";
            //    return false;
            //}

            //***  check telephone number ***//
            string prmError = "";
            string prmErrorMsg = "";
            string prmOTYPE = "";
            bool res48_off = iLDataSubroutine.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            //bool res48_off = CallHisun.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            
            if (!res48_off || prmError == "Y")
            {
                err = prmErrorMsg + "\r\n";
                return false;
            }
            if (prmOTYPE != "P")
            {
                err += "เบอร์ Office รูปแบบไม่ถูกต้อง" + "\r\n";
                return false;
            }

            prmError = "";
            prmErrorMsg = "";
            prmOTYPE = "";

            if (txt_h_tel.Text.Trim() != "")
            {
                bool res48_h = iLDataSubroutine.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_h = CallHisun.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_h || prmError == "Y")
                {
                    err = prmErrorMsg + "\r\n";
                    return false;
                }

                if (prmOTYPE != "P")
                {
                    err += "เบอร์บ้าน รูปแบบไม่ถูกต้อง" + "\r\n";
                    return false;
                }


            }
            else
            {
                bool chk_ext = CallHisun.checkTelExt(txt_h_tel.Text.Trim(), txt_h_ext.Text.Trim());
                if (!chk_ext)
                {
                    err = " เบอร์บ้าน Ext. ไม่ถูกต้อง";
                    return false;
                }
            }

            prmError = "";
            prmErrorMsg = "";
            prmOTYPE = "";
            if (txt_mobile.Text.Trim() != "")
            {
                bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_mobile = CallHisun.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                
                if (!res48_mobile || prmError == "Y")
                {
                    err = prmErrorMsg + "\r\n";
                    return false;
                }
                if (prmOTYPE != "M")
                {
                    err += "เบอร์ Moblile รูปแบบไม่ถูกต้อง" + "\r\n";
                    return false;
                }
            }
            //********** Check Telephone number TO ************//


            if (txt_off_phone.Text.Trim() != "" && txt_off_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()))
                {
                    err = "กรุณาระบุ เบอร์ติดต่อ(Office)ให้ถูกต้อง";
                    return false;
                }
            }
            if (txt_h_tel.Text.Trim() != "" && txt_h_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()))
                {
                    err = "กรุณาระบุ เบอร์ติดต่อ(บ้าน)ให้ถูกต้อง";
                    return false;
                }
            }

            DataSet ds_11 = CallHisunCustomer.getCSMS11(hid_CSN.Value).Result;
            if (CallHisun.check_dataset(ds_11))
            {
                //*** check Home ***//
                DataRow[] res_H = ds_11.Tables[0]?.Select("M11CDE = 'H' ");
                //*** check Office ***//
                DataRow[] res_O = ds_11.Tables[0]?.Select("M11CDE = 'O' ");

                if (res_H != null && res_H.Count() > 0)
                {
                    if (res_H[0]["M11MOB"].ToString().Trim() != txt_mobile_j.Text.Trim())
                    {
                        err = "กรุณาระบุ เบอร์มือถือต้องตรงกัน หากเบอร์มือถือที่หน้าจอตรงกันแล้วต้องทำการ Save Office & Tel เสียก่อน";
                        return false;
                    }
                }


            }


            if (txt_mobile.Text.Trim() != txt_mobile_j.Text.Trim())
            {
                err = "กรุณาระบุ เบอร์มือถือต้องตรงกัน ";
                return false;
            }
            //************************************************//

            if (dd_applyType_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 1. Apply type";
                return false;
            }
            if (dd_apply_via_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 2. Apply via";
                return false;
            }
            if (dd_apply_channel_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 3. Apply channel";
                return false;
            }
            //if (dd_subChannel_j.Value.ToString()  == "") 
            //{
            //    err = "กรุณาระบุ 4. sub apply channel";
            //    return false;
            //}
            if (txt_age.Text.Trim() == "")
            {
                err = "กรุณาระบุ 5. อายุ";
                return false;
            }
            if (txt_birthDate_j.Text.Trim() == "")
            {
                err = "กรุณาระบุ 6. วันเดือนปีเกิด";
                return false;
            }
            //string[] birthDateC = txt_birthDate_j.Text.Split('/');
            //string birthDate1 = birthDateC[2] + birthDateC[1] + birthDateC[0];
            string birthDate1 = txt_birthDate_j.Text.Trim();
            string realBirth = hid_birthDate.Value.Trim();
            if (!realBirth.Contains(birthDate1))
            {
                err = " Birth Date ไม่ตรงกับในระบบ ไม่สามารถ Approve ได้ กรุณาแก้ไข";
                return false;
            }


            if (dd_marital_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 7. สถานภาพ";
                return false;
            }
            if (dd_subMarital_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ สถานภาพย่อย";
                return false;
            }
            string subMarital = dd_subMarital_j.Value.ToString().Trim();
            string marital = dd_marital_j.Value.ToString().Trim();
            if (!CallHisun.checkSubMarital(subMarital, int.Parse(txt_child.Text), int.Parse(txt_age.Text), ref err, marital))
            {

                return false;
            }
            if (rb_sex.Value.ToString() == "")
            {
                err = "กรุณาระบุ 9. เพศ";
                return false;
            }
            DataSet ds_00 = CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            if (!CallHisun.check_dataset(ds_00))
            {
                err = "ไม่สามารถตรวจสอบข้อมูลได้ กรุณาลองใหม่อีกครั้ง";
                return false;
            }

            DataRow dr_00 = ds_00.Tables[0]?.Rows[0];
            DataSet ds_title = new DataSet();
            string cmdCondition = " Code = " + dr_00["M00TTL"].ToString().Trim();
            string condition = "Code = " + dr_00["M00TTL"].ToString().Trim();
            //ds_title.Tables.Add(conn_general.GetGeneralTitleName());
            ds_title = CallHisunMaster.getGeneralCenter("TitleID");
            if (CallHisun.check_dataset(ds_title))
            {
                DataRow[] dr_title = ds_title.Tables[0].Select(condition);
                if (dr_title[0]["ShortName"].ToString().Trim() != "")
                {
                    if (hid_sex.Value != "")
                    {
                        if (rb_sex.Value.ToString() != hid_sex.Value)
                        {
                            err = " ข้อมูล 09. เพศ ไม่สัมพันธ์กับข้อมูลในระบบ";
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (hid_sex.Value != "")
                {
                    if (rb_sex.Value.ToString() != hid_sex.Value)
                    {
                        err = " ข้อมูล 09. เพศ ไม่สัมพันธ์กับข้อมูลในระบบ";
                        return false;
                    }
                }
            }
            if (dd_resident_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 10. ประเภทที่อยู่อาศัย";
                return false;
            }
            if (txt_fPerson.Text.Trim() == "" || txt_fPerson.Text.Trim() == "0")
            {
                err = "กรุณาระบุ 11. จำนวนคนในครอบครัว";
                return false;
            }

            if ((txt_yearResident.Text.Trim() == "" || txt_yearResident.Text.Trim() == "0") && (txt_monthResident.Text.Trim() == "" || txt_monthResident.Text.Trim() == "0"))
            {
                err = "กรุณาระบุ 12. ปีที่อยู่อาศัย";
                return false;
            }
            if (txt_yearResident.Text.Trim() == "")
            {
                err = "กรุณาระบุ 12. ปีที่อยู่อาศัย";
                return false;
            }

            if (int.Parse(txt_monthResident.Text.Trim()) > 11)
            {
                err = "จำนวนเดือนที่อยู่อาศัยต้องไม่เกิน 11 เดือน";
                return false;
            }

            //**  check days of resident **//
            //string birthDate = ;//txt_birthDate_j.Text.Trim();
            int age = CallHisunMaster.computeAge(birthDate1);

            //** time of resident **//


            if ((int.Parse(txt_yearResident.Text.Trim()) * 12) + int.Parse(txt_monthResident.Text.Trim()) > (age * 12))
            {
                err = "ระยะเวลาที่อยู่อาศัยต้องไม่มากกว่าอายุของตัวเอง";
                return false;
            }

            if (dd_busType_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ  13. ประเภทธุรกิจ";
                return false;
            }
            if (dd_occup_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ  14. อาชีพ";
                return false;
            }

            if (dd_occup_j.Value.ToString() == "011" || dd_occup_j.Value.ToString() == "012")
            {
                if (dd_comerc.SelectedItem.Value.Equals(""))
                {
                    err = "กรณีลูกค้าเป็นเจ้าของธุรกิจ จะต้องระบุว่านำใบทะเบียนการค้ามาหรือไม่";
                    return false;
                }
            }
            else
            {
                if (!dd_comerc.SelectedItem.Value.Equals(""))
                {
                    err = " ต้องไม่ระบุนำใบทะเบียนการค้า ";
                    return false;
                }
            }
            if (dd_position_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ  15. ตำแหน่ง";
                return false;
            }
            if (dd_busType_j.Value.ToString() == "17" || dd_occup_j.Value.ToString() == "060" || dd_position_j.Value.ToString() == "006")
            {
                if (dd_busType_j.Value.ToString() != "17" && dd_occup_j.Value.ToString() != "060" && dd_position_j.Value.ToString() != "006")
                {
                    err = "ประเภทธุรกิจ, อาชีพ, ตำแหน่ง, ประเภทการจ้างงาน กรณีที่เลือกหัวข้อใดหัวข้อหนึ่งเป็น 'ว่างงาน' ทุกหัวข้อที่เหลือต้องเลือกเป็น 'ว่างงาน' ทั้งหมด";
                    return false;
                }
            }
            if (txt_empNo.Text.Trim() == "" || txt_empNo.Text == "0")
            {
                err = "กรุณาระบุ 16.จำนวนพนักงาน";
                return false;
            }
            if (txt_s_year.Text.Trim() == "")
            {
                err = " กรุณาระบุ 17. อายุงาน";
                return false;
            }

            if (int.Parse(txt_s_month.Text.Trim()) > 11)
            {
                err = " กรุณาใส่เดือนของอายุงานให้ถูกต้อง ";
                return false;
            }

            if (txt_s_year.Text.Trim() == "0" && txt_s_month.Text.Trim() == "0")
            {
                err = " กรุณาระบุอายุงานให้ถูกต้อง";
                return false;
            }
            int ageService = 15;
            if (int.Parse(txt_age.Text.Trim()) - int.Parse(txt_s_year.Text.Trim()) < ageService)
            {
                err = " ลูกค้าทำงานก่อนอายุ 15 ปี กรุณาตรวจสอบอายุงานอีกครั้ง";
                return false;
            }

            if (dd_incomeType_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 18. ลักษณะการรับเงินเดือน";
                return false;
            }

            if (dd_incomeType_j.Value.ToString() == "01")
            {
                if (dd_occup_j.Value.ToString() != "011" && dd_occup_j.Value.ToString() != "012")
                {

                    if (dd_typeCust_j.SelectedItem.Value.Equals(""))
                    {
                        err = "กรุณาระบุ ประเภทลูกค้า ";
                        return false;
                    }
                }
            }
            else
            {
                if (dd_occup_j.Value.ToString() == "011" && dd_occup_j.Value.ToString() == "012")
                {
                    if (!dd_typeCust_j.SelectedItem.Value.Equals(""))
                    {
                        err = "ไม่ต้องระบุ ประเภทลูกค้า ";
                        return false;
                    }
                }
            }
            if (dd_statement_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 18. เอกสารแสดงรายได้";
                return false;
            }
            if (hid_status.Value == "KESSAI")
            {
                if (hid_incomeDoc.Value.Trim() != dd_statement_j.Value.ToString().Trim())
                {
                    err = "เอกสารแสดงรายได้ ไม่ตรงกับข้อมูลที่มีในระบบ";
                    return false;
                }
            }


            if (txt_salary.Text.Trim() == "0" || txt_salary.Text.Trim() == "")
            {
                err = "กรุณาระบุ 19. รายได้ต่อเดือน";
                return false;
            }
            if (txt_salary_adj.Text.Trim() == "")
            {
                err = "Salary Adjust ต้องไม่เป็นค่าว่าง ถ้าไม่มีกรุณาระบุ 0";
                return false;
            }
            if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
            {
                if (hid_salary.Value.Trim().Replace(",", "") != txt_salary.Text.Trim().Replace(",", ""))
                {
                    err = "Salary ไม่ตรงกับข้อมูลที่มีอยู่ในระบบ";
                    return false;
                }
            }



            //if (txt_mobile_j.Text.Trim() == "") 
            //{
            //    err = "กรุณาระบุ 21. เบอร์โทรศัพท์มือถือ ";
            //    return false;
            //}
            if (txt_postcode_j.Text.Trim() == "")
            {
                err = "กรุณาระบุ 22. รหัสไปรณีย์ (Home)";
                return false;
            }
            if (txt_postcode_off_j.Text.Trim() == "")
            {
                err = "กรุณาระบุ 23. รหัสไปรณีย์ (Office)";
                return false;
            }
            if (dd_empType_j.Value.ToString() == "")
            {
                err = "กรุณาระบุ 24. ประเภทการจ้างงาน";
                return false;
            }
            if (txt_amount_j.Text.Trim() == "")
            {
                return false;
            }

            if (txt_seftDec.Text == "")
            {
                err = "กรุณาระบุตรวจสอบข้อมูล self declare..!!";
                return false;
            }

            //if (dd_dateOfIncome.Value.ToString()  == "") 
            //{
            //    err = "กรุณาระบุ 27. วันที่เงินเดือนออก";
            //    return false;
            //}

            return true;
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            err = ex.Message.ToString();

            return false;
        }
    }
    private bool checkTelNumber()
    {
        string err = "";
        try
        {
            CallHisun = new ILDataCenter();
            DataSet ds_05 = CallHisunCustomer.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
            DataSet ds_11 = CallHisunCustomer.getCSMS11(hid_CSN.Value).Result;
            if (CallHisun.check_dataset(ds_05) && CallHisun.check_dataset(ds_11))
            {   //*** dataRow ***// 
                DataRow dr = ds_05.Tables[0]?.Rows[0];
                //*** check Home ***//
                DataRow[] res_H = ds_11.Tables[0]?.Select("M11CDE = 'H' ");
                //*** check Office ***//
                DataRow[] res_O = ds_11.Tables[0]?.Select("M11CDE = 'O' ");
                if (res_H != null || res_H.Count() > 0)
                {
                    if (hid_val_1.Value != "N")
                    {
                        if (
                            (dr["W5CSTY"].ToString().Trim() == "S" || dr["W5CSTY"].ToString().Trim() == "Z") ||
                            (dr["W5CSTY"].ToString().Trim() == "O" && (dr["W5SBCD"].ToString().Trim() == "03" || dr["W5SBCD"].ToString().Trim() == "04"))
                           )
                        {
                            return true;
                        }
                        if (res_H[0]["M11TEL"].ToString().Trim() != "" && dr["w5htl"].ToString().Trim() != "")
                        {
                            if (
                               ((dr["w5htl"].ToString().Trim() != res_H[0]["M11TEL"].ToString().Trim()) ||
                               (dr["w5htex"].ToString().Trim() != res_H[0]["M11EXT"].ToString().Trim()))
                               )
                            {

                                err = " ข้อมูล 15. เบอร์ติดต่อที่อยู่ปัจจุบัน ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                                hid_quest.Value = "1";
                                hid_val_1.Value = "";
                                lb_msg_2.Text = err;
                                Popup_confirm_2.ShowOnPageLoad = true;
                                return false;

                            }
                        }
                        else if (((res_H[0]["M11TEL"].ToString().Trim() == "" && dr["w5htl"].ToString().Trim() != "") ||
                                 (res_H[0]["M11TEL"].ToString().Trim() != "" && dr["w5htl"].ToString().Trim() == ""))
                                )
                        {

                            err = " ข้อมูล 15. เบอร์ติดต่อที่อยู่ปัจจุบัน ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                            hid_quest.Value = "1";
                            hid_val_1.Value = "";
                            lb_msg_2.Text = err;
                            Popup_confirm_2.ShowOnPageLoad = true;
                            return false;

                        }
                    }
                    if (hid_val_2.Value != "N")
                    {
                        if (dr["W5CSTY"].ToString().Trim() == "S" || dr["W5CSTY"].ToString().Trim() == "Z")
                        {
                            return true;
                        }

                        if (dr["w5mbtl"].ToString().Trim() != "" && res_H[0]["M11MOB"].ToString().Trim() != "")
                        {
                            if (dr["w5mbtl"].ToString().Trim().Substring(0, 9) != res_H[0]["M11MOB"].ToString().Trim().Substring(0, 9))
                            {


                                err = " ข้อมูล 22. เบอร์มือถือ ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                                hid_quest.Value = "2";
                                hid_val_2.Value = "";
                                lb_msg_2.Text = err;
                                Popup_confirm_2.ShowOnPageLoad = true;
                                return false;
                            }
                        }
                        else if ((dr["w5mbtl"].ToString().Trim() == "" && res_H[0]["M11MOB"].ToString().Trim() != "") ||
                                  (dr["w5mbtl"].ToString().Trim() != "" && res_H[0]["M11MOB"].ToString().Trim() == "")
                                )
                        {
                            err = " ข้อมูล 22. เบอร์มือถือ ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                            hid_quest.Value = "2";
                            hid_val_2.Value = "";
                            lb_msg_2.Text = err;
                            Popup_confirm_2.ShowOnPageLoad = true;
                            return false;
                        }

                    }

                }
                if (res_O != null || res_O.Count() > 0)
                {
                    if (hid_val_3.Value != "N")
                    {
                        if (
                                (dr["W5CSTY"].ToString().Trim() == "S" || dr["W5CSTY"].ToString().Trim() == "Z") ||
                                (dr["W5CSTY"].ToString().Trim() == "O" && (dr["W5SBCD"].ToString().Trim() == "03" || dr["W5SBCD"].ToString().Trim() == "05")))
                        {
                            return true;
                        }

                        if (dr["w5hotl"].ToString().Trim() != "" && res_O[0]["M11TEL"].ToString().Trim() != "")
                        {


                            if (
                                (dr["w5hotl"].ToString().Trim() != res_O[0]["M11TEL"].ToString().Trim()) ||
                                (dr["w5hoex"].ToString().Trim() != res_O[0]["M11EXT"].ToString().Trim())
                                )
                            {
                                err = " ข้อมูล 05. เบอร์ติดต่อที่ทำงาน (สนญ.) ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                                hid_quest.Value = "3";
                                hid_val_3.Value = "";
                                lb_msg_2.Text = err;
                                Popup_confirm_2.ShowOnPageLoad = true;
                                return false;

                            }
                        }
                        else if (
                                ((dr["w5hotl"].ToString().Trim() == "" && res_O[0]["M11TEL"].ToString().Trim() != "") ||
                                (dr["w5hotl"].ToString().Trim() != "" && res_O[0]["M11TEL"].ToString().Trim() == ""))
                                )
                        {


                            err = " ข้อมูล 05. เบอร์ติดต่อที่ทำงาน (สนญ.) ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                            hid_quest.Value = "3";
                            hid_val_3.Value = "";
                            lb_msg_2.Text = err;
                            Popup_confirm_2.ShowOnPageLoad = true;
                            return false;
                        }
                    }


                    //if (hid_val_4.Value != "N")
                    //{
                    //    if ((dr["W5CSTY"].ToString().Trim() == "S" || dr["W5CSTY"].ToString().Trim() == "Z") ||
                    //        (dr["W5CSTY"].ToString().Trim() == "O" && 
                    //        (dr["W5SBCD"].ToString().Trim() == "03" || dr["W5SBCD"].ToString().Trim() == "05"))
                    //       ) 
                    //    {
                    //        return true;
                    //    }

                    //    if (dr["w5wotl"].ToString().Trim() != "" && res_O[0]["M11TL2"].ToString().Trim() != "")
                    //    {
                    //        if (
                    //            ((dr["w5wotl"].ToString().Trim() != res_O[0]["M11TL2"].ToString().Trim()) ||
                    //            (dr["w5woex"].ToString().Trim() != res_O[0]["M11EX2"].ToString().Trim())) 
                    //           )
                    //        {
                    //            err = " ข้อมูล 06. เบอร์ติดต่อสาขาที่ประจำ ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                    //            hid_quest.Value = "4";
                    //            hid_val_4.Value = "";
                    //            lb_msg_2.Text = err;
                    //            Popup_confirm_2.ShowOnPageLoad = true;
                    //            return false;

                    //        }
                    //    }
                    //    else if
                    //        (   dr["w5wotl"].ToString().Trim() == "" && res_O[0]["M11TL2"].ToString().Trim() != "" ||
                    //            dr["w5wotl"].ToString().Trim() != "" && res_O[0]["M11TL2"].ToString().Trim() == ""
                    //        )
                    //    {
                    //        err = " ข้อมูล 06. เบอร์ติดต่อสาขาที่ประจำ ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                    //        hid_quest.Value = "4";
                    //        hid_val_4.Value = "";
                    //        lb_msg_2.Text = err;
                    //        Popup_confirm_2.ShowOnPageLoad = true;
                    //        return false;
                    //    }
                    //}

                    //if (hid_val_5.Value != "N")
                    //{
                    //    if((dr["W5CSTY"].ToString().Trim() == "S" || dr["W5CSTY"].ToString().Trim() == "Z") ||
                    //       (dr["W5CSTY"].ToString().Trim() == "O" && 
                    //       (dr["W5SBCD"].ToString().Trim() == "03" || dr["W5SBCD"].ToString().Trim() == "05"))
                    //      )
                    //    {
                    //        return true;
                    //    }
                    //    if ((dr["w5motl"].ToString().Trim() != "" && res_O[0]["M11MOB"].ToString().Trim() != ""))
                    //    {
                    //        if (dr["w5motl"].ToString().Trim().Substring(0, 9) != res_O[0]["M11MOB"].ToString().Trim())
                    //        {
                    //            err = " ข้อมูล 35. เบอร์ที่ทำงาน (Mobile) ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                    //            hid_quest.Value = "5";
                    //            hid_val_5.Value = "";
                    //            lb_msg_2.Text = err;
                    //            Popup_confirm_2.ShowOnPageLoad = true;
                    //            return false;
                    //        }
                    //    }else if
                    //        (
                    //            (dr["w5motl"].ToString().Trim() == "" && res_O[0]["M11MOB"].ToString().Trim() != "") ||
                    //            (dr["w5motl"].ToString().Trim() != "" && res_O[0]["M11MOB"].ToString().Trim() == "")
                    //        )
                    //    {
                    //        err = " ข้อมูล 35. เบอร์ที่ทำงาน (Mobile) ไม่ตรงกับข้อมูลในระบบ คุณต้องการแก้ไขหรือไม่? ";
                    //        hid_quest.Value = "5";
                    //        hid_val_5.Value = "";
                    //        lb_msg_2.Text = err;
                    //        Popup_confirm_2.ShowOnPageLoad = true;
                    //        return false;
                    //    }
                    //}

                }
                return true;

            }
            else
            {
                return true;
            }

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            err = "Check telephone error " + "\r\n";
            lblMsgEN.Text = err;
            PopupMsg_judgment.ShowOnPageLoad = true;
            return false;
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
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            return false;
        }

    }

    private void checkTelType(bool tel, bool ext)
    {
        CallHisun = new ILDataCenter();

        try
        {
            DataSet ds_tel = CallHisunCustomer.getGNTB67(tel, ext).Result;
            if (CallHisun.check_dataset(ds_tel))
            {
                DataRow dr = ds_tel.Tables[0]?.Rows[0];
                dd_homeTelType.Items.Clear();
                dd_homeTelType.Items.Add(dr["GN67CD"].ToString().Trim() + ":" + dr["GN67DE"].ToString().Trim(), dr["GN67CD"].ToString().Trim());
                dd_homeTelType.SelectedIndex = 1;
            }
            else
            {
                dd_homeTelType.Items.Clear();
            }

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            dd_homeTelType.Items.Clear();
        }
    }


    #endregion


    private void check_judgment()
    {
        try
        {
            CallHisun = new ILDataCenter();
            string err = "";

            checkAutoSalaryAdj();
            //if (!checkAutoSalaryAdj()) 
            //{
            //    lblMsgEN.Text = "ต้องไม่ระบุประเภทลูกค้า";
            //    hid_validate.Value = "VER";
            //    PopupMsg_judgment.ShowOnPageLoad = true;
            //    return;
            //}


            //check verify call page
            if (hid_status.Value.ToUpper() != "STEP1" && hid_status.Value.ToUpper() != "TCL0")
            {
                DataSet ds_wk05 = CallHisunCustomer.getCustVerifyCall(hid_brn.Value, hid_AppNo.Value).Result;
                if (CallHisun.check_dataset(ds_wk05))//if (ds_wk05 != null)
                {
                    DataRow dr = ds_wk05.Tables[0]?.Rows[0];

                    bool res_ver = true;
                    string errMsg = "";
                    if (hid_status.Value == "INTERVIEW")
                    { //***** interview ********//

                        if (dr["W5CSTY"].ToString().Trim() == "N" ||
                            dr["W5SBCD"].ToString().Trim() == "01" || dr["W5SBCD"].ToString().Trim() == "02" || dr["W5SBCD"].ToString().Trim() == "06"
                            )
                        {
                            if (dr["w5ousr"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please verify TO" + "\r\n";

                                //lblMsgEN.Text = "Please verify TO";
                                //hid_validate.Value = "VER";
                                //PopupMsg_judgment.ShowOnPageLoad = true;
                                //return;
                            }
                            if (dr["w5husr"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please verify TH" + "\r\n";

                                //lblMsgEN.Text = "Please verify TH";
                                //hid_validate.Value = "VER";
                                //PopupMsg_judgment.ShowOnPageLoad = true;
                                //return;
                            }
                            if (dr["w5musr"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please verify TM" + "\r\n";

                                //lblMsgEN.Text = "Please verify TM";
                                //hid_validate.Value = "VER";
                                //PopupMsg_judgment.ShowOnPageLoad = true;
                                //return;
                            }

                        }
                        else if (dr["W5CSTY"].ToString().Trim() == "O")
                        {
                            if (dr["W5SBCD"].ToString().Trim() == "03")
                            {
                                if (dr["w5musr"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please verify TM" + "\r\n";

                                    //lblMsgEN.Text = "Please verify TM";
                                    //hid_validate.Value = "VER";
                                    //PopupMsg_judgment.ShowOnPageLoad = true;
                                    //return;
                                }

                            }
                            else if (dr["W5SBCD"].ToString().Trim() == "04")
                            {
                                if (dr["w5ousr"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please verify TO" + "\r\n";

                                    //lblMsgEN.Text = "Please verify TO";
                                    //hid_validate.Value = "VER";
                                    //PopupMsg_judgment.ShowOnPageLoad = true;
                                    //return;
                                }

                                if (dr["w5musr"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please verify TM" + "\r\n";

                                    //lblMsgEN.Text = "Please verify TM";
                                    //hid_validate.Value = "VER";
                                    //PopupMsg_judgment.ShowOnPageLoad = true;
                                    //return;
                                }

                            }
                            else if (dr["W5SBCD"].ToString().Trim() == "05")
                            {
                                if (dr["w5husr"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please verify TH" + "\r\n";

                                    //lblMsgEN.Text = "Please verify TH";
                                    //hid_validate.Value = "VER";
                                    //PopupMsg_judgment.ShowOnPageLoad = true;
                                    //return;
                                }
                                if (dr["w5musr"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please verify TM" + "\r\n";

                                    //lblMsgEN.Text = "Please verify TM";
                                    //hid_validate.Value = "VER";
                                    //PopupMsg_judgment.ShowOnPageLoad = true;
                                    //return;
                                }

                            }
                        }

                    }  //*************** Kessai ********************//
                    else if (hid_status.Value == "KESSAI")
                    {
                        if (dr["W5CAUS"].ToString().Trim() == "")
                        {
                            res_ver = false;
                            errMsg += "Please Confirm Customer type" + "\r\n";


                        }
                        //****  Customer = N   , Sub = 01,02,06 ********//
                        if (dr["W5CSTY"].ToString().Trim() == "N" ||
                            dr["W5SBCD"].ToString().Trim() == "01" || dr["W5SBCD"].ToString().Trim() == "02" || dr["W5SBCD"].ToString().Trim() == "06"
                            )
                        {
                            if (dr["w5oaus"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please Confirm TO" + "\r\n";


                            }
                            if (dr["w5haus"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please Confirm TH" + "\r\n";


                            }
                            if (dr["w5maus"].ToString().Trim() == "")
                            {
                                res_ver = false;
                                errMsg += "Please Confirm TM" + "\r\n";


                            }
                            if ((dr["w5eusr"].ToString().Trim() != "") && (dr["w5eaus"].ToString().Trim() == ""))
                            {
                                res_ver = false;
                                errMsg += "Please Confirm TE" + "\r\n";


                            }
                        }
                        //****  Customer = O   , Sub = 03,04,05 ********//
                        else if (dr["W5CSTY"].ToString().Trim() == "O")
                        {
                            if (dr["W5SBCD"].ToString().Trim() == "03")
                            {

                                if (dr["w5maus"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TM" + "\r\n";


                                }
                                //if (dr["w5eaus"].ToString().Trim() == "")
                                //{
                                //    res_ver = false;
                                //    errMsg += "Please Confirm TE" + "\r\n";


                                //}
                            }
                            else if (dr["W5SBCD"].ToString().Trim() == "04")
                            {
                                if (dr["w5oaus"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TO" + "\r\n";

                                }

                                if (dr["w5maus"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TM" + "\r\n";


                                }
                                if ((dr["w5eusr"].ToString().Trim() != "") && (dr["w5eaus"].ToString().Trim() == ""))
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TE" + "\r\n";


                                }
                            }
                            else if (dr["W5SBCD"].ToString().Trim() == "05")
                            {
                                if (dr["w5haus"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TH" + "\r\n";


                                }
                                if (dr["w5maus"].ToString().Trim() == "")
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TM" + "\r\n";


                                }
                                if ((dr["w5eusr"].ToString().Trim() != "") && (dr["w5eaus"].ToString().Trim() == ""))
                                {
                                    res_ver = false;
                                    errMsg += "Please Confirm TE" + "\r\n";


                                }
                            }
                        }
                    }

                    if (!res_ver)
                    {
                        lblMsgEN.Text = errMsg;
                        hid_validate.Value = "VER";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }


                }
                else
                {
                    lblMsgEN.Text = "Please Verify Data before judgment";
                    hid_validate.Value = "VER";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }

            }
            if (!checkBeforeCal_time_1(ref err))
            {
                lblMsgEN.Text = err;
                hid_validate.Value = "";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            //checkAutoSalaryAdj();

            bool chk_number = checkTelNumber();
            if (!chk_number)
            {
                return;
            }

            string G_aprj = "";
            string G_loca = "";
            string G_reason = "";
            string G_Msg = "";
            string G_err = "";

            checkAutoReject(ref G_aprj, ref G_loca, ref G_reason, ref G_Msg, ref G_err);
            hid_G_aprj.Value = G_aprj;
            hid_G_loca.Value = G_loca;
            hid_G_reason.Value = G_reason;

            if (G_err != "Y")
            {
                if (hid_status.Value == "TCL0")
                {
                    string msg_forSave = "";
                    if (hid_incomeDoc.Value.Trim() != dd_statement_j.Value.ToString().Trim())
                    {

                        msg_forSave += "เอกสารแสดงรายได้ไม่ตรง Int." + "\r\n";

                    }

                    if (hid_salary.Value.Trim().Replace(",", "") != txt_salary.Text.Trim().Replace(",", ""))
                    {
                        msg_forSave += "รายได้ไม่ตรง Int." + "\r\n";

                    }
                    lblConfirmMsgEN.Text = msg_forSave + " Do you want to save ? " + G_Msg;
                }
                else
                {
                    lblConfirmMsgEN.Text = " Do you want to save ? " + G_Msg;
                }
                hid_oper_judg.Value = "SAVE";
                PopupConfirmSave_judg.ShowOnPageLoad = true;
                return;
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }

    }


    protected void btn_judgment_Click(object sender, EventArgs e)
    {


        hid_val_1.Value = "";
        hid_val_2.Value = "";
        hid_val_3.Value = "";
        //hid_val_4.Value = "";
        //hid_val_5.Value = "";
        hid_val_6.Value = "";
        hid_quest.Value = "";
        check_judgment();


    }
    protected void dd_apply_channel_j_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bind_J_ApplySubChannel(dd_apply_channel_j.SelectedItem.Value.ToString());

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    protected void dd_marital_j_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bind_J_SubMaritalStatus(dd_marital_j.SelectedItem.Value.ToString());
            dd_subMarital_j.Value = "";
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    protected void dd_occup_j_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dd_comerc.Value = "";
            bind_J_Comercial(dd_occup_j.SelectedItem.Value.ToString().Trim());
            checkAutoSalaryAdj();

            //BOT Report
            if (dd_occup_j.SelectedItem.Value.ToString().Trim() == "051" || dd_occup_j.SelectedItem.Value.ToString().Trim() == "052" || dd_occup_j.SelectedItem.Value.ToString().Trim() == "054" || dd_occup_j.SelectedItem.Value.ToString().Trim() == "055")
            {
                dd_suboccup.Value = "13";
                dd_suboccup.Enabled = false;
            }
            else
            {
                dd_suboccup.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }


    protected void btnConfirmSave_Click(object sender, EventArgs e)
    {
        if (hid_oper_judg.Value == "SAVE")
        {
            SaveJudgment();

            //BOT Report
            SaveCustomerSubWorked();
        }
        if (hid_oper_judg.Value == "SAVE_O")
        {
            SaveInformation();
        }

        hid_oper_judg.Value = "";
    }
    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        hid_oper_judg.Value = "";
    }
    protected void D_tambol1_TextChanged(object sender, EventArgs e)
    {
        if (D_tambol1.Text.Trim() == "")
        {
            return;
        }
        try
        {
            CallHisun = new ILDataCenter();
            DataSet DS = new DataSet();
            //if (Cache["ds_Tambol"] != null)
            //{
            //    DS = (DataSet)Cache["ds_Tambol"];
            //}
            //else
            //{
            DS = CallHisunMaster.getTambol("", D_tambol1.Text.Trim()).Result;
            //    Cache["ds_Tambol"] = DS;
            //    Cache.Insert("ds_Tambol", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
            //}

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
                CallHisunMaster._dataCenter.CloseConnectSQL();

                return;
            }


            /***************************************  Find Amphur  ********************************************/
            //if (Cache["ds_Amphur"] != null)
            //{
            //    DS = (DataSet)Cache["ds_Amphur"];
            //}
            //else
            //{
            DS = CallHisunMaster.getAmphur("", D_tambol1.Text.Trim()).Result;
            //    Cache["ds_Amphur"] = DS;
            //    Cache.Insert("ds_Amphur", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
            //}

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
                CallHisunMaster._dataCenter.CloseConnectSQL();

                return;
            }
            if (count_amphur > 1)
            {
                CallHisunMaster._dataCenter.CloseConnectSQL();

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
                //if (Cache["ds_Province"] != null)
                //{
                //    DS = (DataSet)Cache["ds_Province"];
                //}
                //else
                //{
                DS = CallHisunMaster.getProvince(D_tambol1.Text.Trim(), D_amphur1.Text.Trim()).Result;
                //    Cache["ds_Province"] = DS;
                //    Cache.Insert("ds_Province", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
                //}

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
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    protected void D_amphur1_TextChanged(object sender, EventArgs e)
    {
        if (D_amphur1.Text.Trim() == "")
        {
            return;
        }
        try
        {
            CallHisun = new ILDataCenter();
            DataSet DS = new DataSet();
            //if (Cache["ds_Amphur"] != null)
            //{
            //    DS = (DataSet)Cache["ds_Amphur"];
            //}
            //else
            //{
            DS = CallHisunMaster.getAmphur(D_amphur1.Text.Trim(), "").Result;
            //    Cache["ds_Amphur"] = DS;
            //    Cache.Insert("ds_Amphur", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
            //} 
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
                //if (Cache["ds_Province"] != null)
                //{
                //    DS = (DataSet)Cache["ds_Province"];
                //}
                //else
                //{
                DS = CallHisunMaster.getProvince("", D_amphur1.Text.Trim()).Result;
                //    Cache["ds_Province"] = DS;
                //    Cache.Insert("ds_Province", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
                //}
                //DS = CallHisun.getProvince("", D_amphur1.Text.Trim());

            }
            else
            {
                //if (Cache["ds_Province"] != null)
                //{
                //    DS = (DataSet)Cache["ds_Province"];
                //}
                //else
                //{
                DS = CallHisunMaster.getProvince(D_tambol1.Text.Trim(), D_amphur1.Text.Trim()).Result;
                //    Cache["ds_Province"] = DS;
                //    Cache.Insert("ds_Province", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
                //}
                //DS = CallHisun.getProvince(D_tambol1.Text.Trim(), D_amphur1.Text.Trim());

            }

            string Mem_tambol = D_tambol1.Text.Trim();
            int count_tambol = 0;
            if (DS != null)
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
                DS = CallHisunMaster.getProvince("", D_amphur1.Text.Trim()).Result;

                if (DS != null)
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
                CallHisunMaster._dataCenter.CloseConnectSQL();

                return;
            }
            if (count_tambol > 1)
            {
                D_tambol1.Focus();
                D_tambol1.Enabled = true;
                D_amphur1.Enabled = true;
                D_province1.Enabled = true;
                D_zipcode1.Enabled = true;
                CallHisunMaster._dataCenter.CloseConnectSQL();

                return;
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }

    }

    protected void C_address_I_TextChanged(object sender, EventArgs e)
    {
        L_count.Text = "";

        if (C_address_I.Text.Trim() == "")
        {
            return;
        }

        try
        {
            CallHisun = new ILDataCenter();


            DataSet DS = new DataSet();

            if (C_address_I.Text.Trim().Length > 20)
            {
                DS = CallHisunMaster.getTambol(int.Parse(C_address_I.Value.ToString().Substring(0, 5)).ToString(), "").Result;//CallHisun.getAddress(C_address_I.Value.ToString().Substring(0, 5)+" ");

                if (DS != null)
                {
                    D_tambol1.Items.Clear();
                    foreach (DataRow dr in DS.Tables[0]?.Rows)
                    {
                        D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());

                    }
                    DS.Clear();
                }
                DS = CallHisunMaster.getAmphur("", "", C_address_I.Value.ToString().Substring(6, 4)).Result;

                if (DS != null)
                {
                    D_amphur1.Items.Clear();
                    foreach (DataRow dr in DS.Tables[0]?.Rows)
                    {
                        D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                    }
                    DS.Clear();
                }


                DS = CallHisunMaster.getProvince("", "", C_address_I.Value.ToString().Substring(11, 3)).Result;
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
                CallHisunMaster._dataCenter.CloseConnectSQL();

                return;
            }
            CallHisunMaster._dataCenter.CloseConnectSQL();




            DS = CallHisunMaster.getAddress(C_address_I.Text.Trim()).Result;//CallHisun.RetriveAsDataSet(cmd);

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
                CallHisunMaster._dataCenter.CloseConnectSQL();
            }
            L_count.Text = count.ToString() + " Matching";
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    protected void btn_post_H_Click(object sender, EventArgs e)
    {
        C_address_I.Enabled = true;
        D_tambol1.Enabled = true;
        D_amphur1.Enabled = true;
        D_province1.Enabled = true;
        D_zipcode1.Enabled = true;
        C_address_I.Items.Clear();
        C_address_I.Text = "";
        D_tambol1.Items.Clear();
        D_tambol1.Value = "";
        D_amphur1.Items.Clear();
        D_amphur1.Value = "";
        D_province1.Items.Clear();
        D_province1.Value = "";
        D_zipcode1.Items.Clear();
        D_zipcode1.Value = "";
        hid_Type.Value = "H";
        Popup_Province.ShowOnPageLoad = true;

    }
    protected void btn_post_O_Click(object sender, EventArgs e)
    {
        C_address_I.Enabled = true;
        D_tambol1.Enabled = true;
        D_amphur1.Enabled = true;
        D_province1.Enabled = true;
        D_zipcode1.Enabled = true;
        C_address_I.Items.Clear();
        C_address_I.Text = "";
        D_tambol1.Items.Clear();
        D_tambol1.Value = "";
        D_amphur1.Items.Clear();
        D_amphur1.Value = "";
        D_province1.Items.Clear();
        D_province1.Value = "";
        D_zipcode1.Items.Clear();
        D_zipcode1.Value = "";
        hid_Type.Value = "O";
        Popup_Province.ShowOnPageLoad = true;

    }
    protected void btn_post_I_Click(object sender, EventArgs e)
    {
        C_address_I.Enabled = true;
        D_tambol1.Enabled = true;
        D_amphur1.Enabled = true;
        D_province1.Enabled = true;
        D_zipcode1.Enabled = true;
        C_address_I.Items.Clear();
        C_address_I.Text = "";
        D_tambol1.Items.Clear();
        D_tambol1.Value = "";
        D_amphur1.Items.Clear();
        D_amphur1.Value = "";
        D_province1.Items.Clear();
        D_province1.Value = "";
        D_zipcode1.Items.Clear();
        D_zipcode1.Value = "";
        hid_Type.Value = "I";
        Popup_Province.ShowOnPageLoad = true;

    }
    protected void btn_sel_prov_Click(object sender, EventArgs e)
    {
        if (hid_Type.Value == "O")
        {
            txt_postcode_off_j.Text = D_zipcode1.Value.ToString();
            hid_offP.Value = D_province1.Value.ToString();
            hid_tamO.Value = D_tambol1.Value.ToString();
            hid_ampO.Value = D_amphur1.Value.ToString();
            hid_provO.Value = D_province1.Value.ToString();

        }
        else if (hid_Type.Value == "H")
        {
            txt_postcode_j.Text = D_zipcode1.Value.ToString();
            hid_homeP.Value = D_province1.Value.ToString();
            hid_tambolH.Value = D_tambol1.Value.ToString();
            hid_ampH.Value = D_amphur1.Value.ToString();
            hid_provH.Value = D_province1.Value.ToString();
        }
        else if (hid_Type.Value == "I")
        {
            txt_postcode_idcard_j.Text = D_zipcode1.Value.ToString();
            hid_idcardP.Value = D_province1.Value.ToString();
            hid_tamI.Value = D_tambol1.Value.ToString();
            hid_ampI.Value = D_amphur1.Value.ToString();
            hid_provI.Value = D_province1.Value.ToString();
        }

    }
    public void bindAutoSalaryAdj()
    {
        try
        {
            CallHisun = new ILDataCenter();
            if(dd_occup_j.Value == null)
            {
                txt_salary_adj.Text = "0";
            }
            else
            {
                if (!dd_occup_j.Value.Equals("011") && !dd_occup_j.Value.Equals("012") && !dd_occup_j.Value.Equals("") && !dd_occup_j.Value.Equals(null))
                {
                    if (dd_incomeType_j.Value != null && dd_incomeType_j.Value.Equals("01") &&  hid_salary.Value != "")
                    {
                        dd_typeCust_j.Enabled = true;
                        DataSet ds_adj = CallHisunCustomer.getGNTS18(dd_typeCust_j.Value.ToString()).Result;
                        if (CallHisun.check_dataset(ds_adj))
                        {
                            DataRow dr_sal = ds_adj.Tables[0]?.Rows[0];
                            if (float.Parse(txt_salary.Text.Trim().Replace(",", "")) < float.Parse(dr_sal["gs18aj"].ToString()))
                            {
                                txt_salary_adj.Text = txt_salary.Text;
                            }
                            else
                            {
                                txt_salary_adj.Text = float.Parse(dr_sal["gs18aj"].ToString()).ToString("0");
                            }
                        }
                    }
                    else
                    {
                        dd_typeCust_j.SelectedIndex = -1;
                        dd_typeCust_j.Enabled = false;
                        txt_salary_adj.Text = "0";
                    }

                }
                else
                {

                    txt_salary_adj.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
        CallHisunCustomer._dataCenter.CloseConnectSQL();
    }

    public bool checkAutoSalaryAdj_return()
    {
        try
        {
            //  case ปกติ

            CallHisun = new ILDataCenter();
            if (dd_incomeType_j.SelectedItem != null)
            {
                if (dd_incomeType_j.SelectedItem.Value.Equals("01"))
                {
                    if (dd_occup_j.SelectedItem != null)
                    {
                        if (!dd_occup_j.SelectedItem.Value.Equals("011") && !dd_occup_j.SelectedItem.Value.Equals("012") && !dd_occup_j.SelectedItem.Value.Equals(""))
                        {
                            //dd_typeCust_j.Enabled = true;
                            if (txt_salary.Text.Trim() != "")
                            {
                                if (!dd_typeCust_j.SelectedItem.Value.Equals(""))
                                {
                                    DataSet ds_adj = CallHisunCustomer.getGNTS18(dd_typeCust_j.SelectedItem.Value.ToString()).Result;
                                    if (CallHisun.check_dataset(ds_adj))
                                    {
                                        DataRow dr_sal = ds_adj.Tables[0]?.Rows[0];
                                        if (float.Parse(txt_salary.Text.Trim().Replace(",", "")) < float.Parse(dr_sal["gs18aj"].ToString()))
                                        {
                                            txt_salary_adj.Text = txt_salary.Text;
                                        }
                                        else
                                        {
                                            txt_salary_adj.Text = float.Parse(dr_sal["gs18aj"].ToString()).ToString("0");
                                        }
                                    }
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                                }
                            }
                            else
                            {

                                txt_salary_adj.Text = "0";

                                dd_typeCust_j.Value = "";
                                return true;
                                //dd_typeCust_j.Enabled = false;
                            }
                        }
                        else
                        {
                            txt_salary_adj.Text = "0";
                            return false;
                            //dd_typeCust_j.Value = "";
                            //dd_typeCust_j.Enabled = false;
                        }
                    }
                }
                else
                {
                    txt_salary_adj.Text = "0";
                    return false;
                    //dd_typeCust_j.Value = "";
                    //dd_typeCust_j.Enabled = false;
                }

            }
            else
            {
                txt_salary_adj.Text = "0";

                dd_typeCust_j.Value = "";
                return true;
                //dd_typeCust_j.Enabled = false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            txt_salary_adj.Text = "0";
            dd_typeCust_j.Value = "";
            return true;

            //dd_typeCust_j.Enabled = false;
        }

    }
    public void checkAutoSalaryAdj()
    {
        try
        {
            //  case ปกติ
            CallHisun = new ILDataCenter();
            if (dd_incomeType_j.SelectedItem != null)
            {
                if (dd_incomeType_j.SelectedItem.Value.Equals("01"))
                {
                    if (dd_occup_j.SelectedItem != null)
                    {
                        if (!dd_occup_j.SelectedItem.Value.Equals("011") && !dd_occup_j.SelectedItem.Value.Equals("012") && !dd_occup_j.SelectedItem.Value.Equals(""))
                        {
                            dd_typeCust_j.Enabled = true;
                            if (txt_salary.Text.Trim() != "")
                            {
                                if (!dd_typeCust_j.SelectedItem.Value.Equals(""))
                                {
                                    DataSet ds_adj = CallHisunCustomer.getGNTS18(dd_typeCust_j.SelectedItem.Value.ToString()).Result;
                                    if (CallHisun.check_dataset(ds_adj))
                                    {
                                        DataRow dr_sal = ds_adj.Tables[0]?.Rows[0];
                                        if (float.Parse(txt_salary.Text.Trim().Replace(",", "")) < float.Parse(dr_sal["gs18aj"].ToString()))
                                        {
                                            txt_salary_adj.Text = txt_salary.Text;
                                        }
                                        else
                                        {
                                            txt_salary_adj.Text = float.Parse(dr_sal["gs18aj"].ToString()).ToString("0");
                                        }
                                    }
                                }
                            }
                            else
                            {

                                txt_salary_adj.Text = "0";

                                dd_typeCust_j.Value = "";
                                dd_typeCust_j.Enabled = false;
                            }
                        }
                        else
                        {
                            txt_salary_adj.Text = "0";
                            dd_typeCust_j.Value = "";
                            dd_typeCust_j.Enabled = false;
                        }
                    }
                }
                else
                {
                    txt_salary_adj.Text = "0";
                    dd_typeCust_j.Value = "";
                    dd_typeCust_j.Enabled = false;
                }

            }
            else
            {
                txt_salary_adj.Text = "0";
                dd_typeCust_j.Value = "";
                dd_typeCust_j.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            txt_salary_adj.Text = "0";
            dd_typeCust_j.Value = "";
            dd_typeCust_j.Enabled = false;
        }
        CallHisunCustomer._dataCenter.CloseConnectSQL();
    }
    protected void btnClosePopupMsg_Click(object sender, EventArgs e)
    {
        if (hid_validate.Value == "SAVE")
        {
            if (hid_status.Value == "STEP1")
            {
                Response.Redirect("IL_KEY_IN_Step1.aspx", false);
            }

            else
            {

                if ((hid_status.Value == "TCL0") || (hid_G_aprj.Value != "" && hid_G_loca.Value != "" && hid_G_reason.Value != ""))
                {
                    //if (hid_status.Value == "INTERVIEW")
                    //{
                    //    changeTabToProduct("N_INTERVIEW");
                    //}
                    //else if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
                    //{
                    //    changeTabToProduct("N_KESSAI");
                    //}

                    if (hid_status.Value == "TCL0")
                    {
                        changeTabToProduct("N_KESSAI");
                    }
                    else if (hid_status.Value == "KESSAI" || hid_status.Value == "INTERVIEW")
                    {
                        changeTabToProduct("PASS");
                    }
                }
                else
                {

                    changeTabToProduct("PASS");

                }
            }
        }
        else if (hid_validate.Value == "VER")
        {
            hid_validate.Value = "";
        }

    }
    protected void btn_ok_2_Click(object sender, EventArgs e)
    {
        if (hid_quest.Value == "1")
        {
            hid_val_1.Value = "";
        }
        else if (hid_quest.Value == "2")
        {
            hid_val_2.Value = "";
        }
        else if (hid_quest.Value == "3")
        {
            hid_val_3.Value = "";
        }
        //else if (hid_quest.Value == "4")
        //{
        //    hid_val_4.Value = "";
        //}
        //else if (hid_quest.Value == "5")
        //{
        //    hid_val_5.Value = "";
        //}
        //btn_judgment_Click(null, null);

    }
    protected void btn_clear_prov_Click(object sender, EventArgs e)
    {
        C_address_I.Enabled = true;
        C_address_I.Text = "";
        D_tambol1.Items.Clear();
        D_tambol1.Value = "";
        D_amphur1.Items.Clear();
        D_amphur1.Value = "";
        D_province1.Items.Clear();
        D_province1.Value = "";
        D_zipcode1.Items.Clear();
        D_zipcode1.Value = "";

        D_tambol1.Enabled = true;
        D_amphur1.Enabled = true;
        D_province1.Enabled = true;
        D_zipcode1.Enabled = true;
    }
    protected void btn_cancel_2_Click(object sender, EventArgs e)
    {
        if (hid_quest.Value == "1")
        {
            hid_val_1.Value = "N";
        }
        else if (hid_quest.Value == "2")
        {
            hid_val_2.Value = "N";
        }
        else if (hid_quest.Value == "3")
        {
            hid_val_3.Value = "N";
        }
        //else if (hid_quest.Value == "4")
        //{
        //    hid_val_4.Value = "N";
        //}
        //else if (hid_quest.Value == "5")
        //{
        //    hid_val_5.Value = "N";
        //}
        check_judgment();
        //btn_judgment_Click(null, null);
    }
    protected void dd_incomeType_j_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkAutoSalaryAdj();
    }
    protected void dd_typeCust_j_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkAutoSalaryAdj();
    }
    protected void txt_salary_TextChanged(object sender, EventArgs e)
    {
        checkAutoSalaryAdj();
    }
    protected void txt_h_tel_TextChanged(object sender, EventArgs e)
    {
        validateTelHome();
    }

    protected void txt_h_tel_to_TextChanged(object sender, EventArgs e)
    {
        validateTelHome();
    }
    protected void txt_h_ext_TextChanged(object sender, EventArgs e)
    {
        validateTelHome();
    }

    private void validateTelHome()
    {
        try
        {
            CallHisun = new ILDataCenter();
            string tel = CallHisunCustomer.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim());
            if (tel.Length >= 9)
            {
                if (txt_h_ext.Text.Trim() != "")
                {
                    checkTelType(true, true);
                }
                else
                {
                    checkTelType(true, false);
                }
            }
            else if (tel == "")
            {
                checkTelType(false, false);
            }

        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private bool validate_information(ref string err)
    {
        bool res = true;
        try
        {
            CallHisun = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun.UserInfomation = userInfoService.GetUserInfo();


            if (dd_off_title.Value == null)
            {
                err += "กรุณาระบุ คำนำหน้าบริษัท" + "\r\n";
                res = false;
            }
            //if (txt_off_name.Text.Trim() == "")
            if (dd_off_name.Text.Trim() == "")
            {
                err += "กรุณาระบุ ชื่อบริษัท" + "\r\n";
                res = false;
            }
            if (txt_off_phone.Text.Trim() == "")
            {
                err += "กรุณาระบุ เบอร์โทรศัพท์บริษัท" + "\r\n";
                res = false;
            }


            //***  check telephone number ***//
            string prmError = "";
            string prmErrorMsg = "";
            string prmOTYPE = "";
            bool res48_off = iLDataSubroutine.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            //bool res48_off = CallHisun.Call_GNSR48(txt_off_phone.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            
            if (!res48_off || prmError == "Y")
            {
                err += "Office Telephone: " + prmErrorMsg + "\r\n";
                res = false;
            }
            if (prmOTYPE != "P")
            {
                err += "เบอร์ Office รูปแบบไม่ถูกต้อง" + "\r\n";
                res = false;
            }

            prmError = "";
            prmErrorMsg = "";
            prmOTYPE = "";

            if (txt_h_tel.Text.Trim() != "")
            {
                bool res48_h = iLDataSubroutine.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_h = CallHisun.Call_GNSR48(txt_h_tel.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
                if (!res48_h || prmError == "Y")
                {
                    err += "Home Telephone: " + prmErrorMsg + "\r\n";
                    res = false;
                }
                if (prmOTYPE != "P")
                {
                    err += "เบอร์บ้าน รูปแบบไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            else
            {
                bool chk_ext = CallHisun.checkTelExt(txt_h_tel.Text.Trim(), txt_h_ext.Text.Trim());
                if (!chk_ext)
                {
                    err += " เบอร์บ้าน Ext. ไม่ถูกต้อง" + "\r\n";
                    res = false;
                }
            }

            DataSet ds_11 = CallHisunCustomer.getCSMS11(hid_CSN.Value).Result;
            if (CallHisun.check_dataset(ds_11))
            {
                //*** check Home ***//
                DataRow[] res_H = ds_11.Tables[0]?.Select("M11CDE = 'H' ");
                //*** check Office ***//
                DataRow[] res_O = ds_11.Tables[0]?.Select("M11CDE = 'O' ");

                if (res_H != null && res_H.Count() > 0)
                {

                    //if ((res_H[0]["M11TEL"].ToString().Trim() != CallHisun.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim())) && (hid_provH.Value.Trim() != ""))
                    //{
                    //    err += "กรุณาระบุ เบอร์บ้านต้องตรงกับรหัสไปรษณีย์ หากเบอร์บ้านที่หน้าจอตรงกับรหัสไปรษณีย์แล้วแล้วต้องทำการ Save Office & Tel เสียก่อน" + "\r\n";
                    //    res = false;
                    //}

                    if ((txt_h_tel.Text.Trim() != "") && (hid_provH.Value.Trim() != ""))
                    {
                        var checkProvince = CallHisunCustomer.CheckProvinceNA(hid_CSN.Value).Result;
                        if (!CallHisun.check_dataset(checkProvince))
                        {
                            bool res49_h = iLDataSubroutine.Call_GNSR49(CallHisunCustomer.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.PadLeft(3, '0').Trim()), hid_provH.Value.PadLeft(3, '0'), ref prmError, userInfo.BizInit, userInfo.BranchNo);
                            //bool res49_h = CallHisun.Call_GNSR49(CallHisun.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()), hid_provH.Value, ref prmError, userInfo.BizInit, userInfo.BranchNo);
                            CallHisunCustomer._dataCenter.CloseConnectSQL();

                            if (prmError == "Y")
                            {
                                err += "เบอร์โทรศัพท์บ้านไม่สัมพันธ์กับรหัสจังหวัด" + "\r\n";
                                res = false;
                            }
                        }
                    }

                }


            }

            prmError = "";
            prmErrorMsg = "";
            if (txt_mobile.Text.Trim() != "")
            {
                bool res48_mobile = iLDataSubroutine.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                //bool res48_mobile = CallHisun.Call_GNSR48(txt_mobile.Text.Trim(), "3", ref prmError, ref prmErrorMsg, userInfo.BizInit, userInfo.BranchNo, ref prmOTYPE);
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
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
            //********** Check Telephone number TO ************//


            if (txt_off_phone.Text.Trim() != "" && txt_off_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()))
                {
                    err += "กรุณาระบุ เบอร์ติดต่อ(Office)ให้ถูกต้อง" + "\r\n";
                    res = false;
                }
            }
            if (txt_h_tel.Text.Trim() != "" && txt_h_tel_to.Text.Trim() != "")
            {
                if (!checkTelTo(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()))
                {
                    err += "กรุณาระบุ เบอร์ติดต่อ(บ้าน)ให้ถูกต้อง" + "\r\n";
                    res = false;
                }
            }

            return res;
            //return true;
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            err += " Validate error " + "\r\n" ;
            return res;
        }
    }

    protected void btn_save_Info_Click(object sender, EventArgs e)
    {
        try
        {
            string err = "";
            if (!validate_information(ref err))
            {
                lblMsgEN.Text = err;
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }

            #region newChangeCheckBlacklist
            string autoRejMsg = "";
            string res_sms = "";
            string G_Msg = "";
            string G_err = "";
            //string companyName = txt_off_name.Text.Trim();
            string companyName = dd_off_name.Value.ToString().Trim();
            string title = dd_off_title.Value.ToString();

            CallHisun = new ILDataCenter();

            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun.UserInfomation = userInfoService.GetUserInfo();
            iDB2Command cmd = new iDB2Command();
            string Date_97 = hidDate97.Value;
            //iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);
            //CallHisun.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            

            var a = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlackList", new string[] { companyName, title });
            CompanyBlacklist.responseCompanyBlacklist companyBlacklist = JsonConvert.DeserializeObject<CompanyBlacklist.responseCompanyBlacklist>(a);

            if (companyBlacklist.Success == false)
            {
                G_err = "Y";
                lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลบริษัทได้ กรุณาทำรายการใหม่อีกครั้ง" + companyBlacklist.Message;
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }

            if (companyBlacklist.data != null)
            {
                if (companyBlacklist.data.CompanyFlag == "T") //TEMPORARY COMPANY BLACKLIST
                {
                    //G_Msg = "Office name : " + txt_off_name.Text.Trim() + " is a temporary blacklist company ";
                    G_Msg = "Office name : " + dd_off_name.Text.Trim() + " is a temporary blacklist company(T).";
                }
                else if (companyBlacklist.data.CompanyFlag == "P") //PERMANENT COMPANY BLACKLIST
                {
                    string WPERR = "";
                    string WPHSTS = "";
                    string WPMSG = "";
                    bool res_cs = iLDataSubroutine.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                    //bool res_cs = CallHisun.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    
                    if (!res_cs || WPERR == "Y")
                    {
                        G_err = "Y";
                        lblMsgEN.Text = "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง";
                        PopupMsg_judgment.ShowOnPageLoad = true;
                        return;
                    }

                    if (WPHSTS == "N") // ลูกค้าใหม่ AutoReject
                    {
                        if (hid_status.Value == "STEP1")
                        {
                            //autoRejMsg = "[QL25] Office name :" + txt_off_name.Text.Trim() + " is blacklist status ." + "\r\n" + " System will Auto Reject. ";
                            autoRejMsg = "[QL25] Office name :" + dd_off_name.Text.Trim() + " is blacklist status(P)." + "\r\n" + " System will Auto Reject. ";

                            #region save reject
                            // ***  Save reject ***//
                            bool saveReject = CallHisunCustomer.SaveRejectStatus(CallHisunCustomer._dataCenter, Date_97, hid_AppNo.Value, hid_AppDate.Value, "QL25", "RJ", "210", "1", "KEYINSTEP1", userInfo.Username, userInfo.LocalClient, userInfo.BranchApp, hid_CSN.Value, hid_idno.Value);
                            if (!saveReject)
                            {
                                CallHisunCustomer._dataCenter.RollbackMssql();
                                CallHisunCustomer._dataCenter.CloseConnectSQL();

                                lblMsgEN.Text = "Save Reject not complete";
                                PopupMsg_judgment.ShowOnPageLoad = true;
                                return;
                            }
                            #endregion

                            #region SMS
                            //*** call  send SMS sub routine ***//
                            if (txt_mobile.Text.Trim() != "")
                            {
                                string poerrc = "";
                                string poerrm = "";
                                //bool sms = true;
                                ILSRSMS iLSRSMS = new ILSRSMS();
                                bool sms = iLSRSMS.Call_ILSRSMS(CallHisunCustomer._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'), hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm);
                                //bool sms = CallHisun.Call_ILSRSMS("C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'), hid_AppNo.Value.PadLeft(11, '0'), txt_mobile.Text.Trim(), ref poerrc, ref poerrm, userInfo.BizInit, userInfo.BranchNo);
                                if (!sms || poerrc == "Y")
                                {
                                    lblMsgEN.Text = "Save not complete. Cannot send SMS " + poerrm;
                                    CallHisunCustomer._dataCenter.RollbackMssql();
                                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                                    
                                    

                                    PopupMsg_judgment.ShowOnPageLoad = true;
                                    return;
                                }
                                res_sms = " [ส่ง SMS สำเร็จ] ";
                            }
                            else
                            {
                                res_sms = " [ไม่สามารถส่ง SMS ได้เนื่องจากไม่พบเบอร์มือถือ] ";
                            }
                            #endregion
                            CallHisunCustomer._dataCenter.CommitMssql();
                            CallHisunCustomer._dataCenter.CloseConnectSQL();
                            
                            

                            lblMsgEN.Text = autoRejMsg + " " + res_sms;
                            hid_validate.Value = "SAVE";
                            PopupMsg_judgment.ShowOnPageLoad = true;
                            return;
                        }
                        else // != step1
                        {
                            G_Msg = "Office name : " + dd_off_name.Text.Trim() + " is a blacklist company(P).";
                        }
                    }
                    else // ลูกค้าเก่า message แจ้งเตือน
                    {
                        //G_Msg = "Office name : " + txt_off_name.Text.Trim() + " is a blacklist company.";
                        G_Msg = "Office name : " + dd_off_name.Text.Trim() + " is a blacklist company(P).";
                    }
                }
                else if (companyBlacklist.data.CompanyFlag == "S")//O4203016 flag 'S' => จับตามองพิเศษ
                {
                    //G_Msg = "Office name : " + txt_off_name.Text.Trim() + " is a pending blacklist company ";
                    G_Msg = "Office name : " + dd_off_name.Text.Trim() + " is a pending blacklist company(S).";
                }
            }
            #endregion

            lblConfirmMsgEN.Text = "Do you want to save office & Tel ?  \n" + G_Msg;
            hid_oper_judg.Value = "SAVE_O";
            PopupConfirmSave_judg.ShowOnPageLoad = true;
            return;


        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void SaveInformation()
    {
        try
        {
            lblMsgEN.Text = "";
            string progName = "ILJUDG";

            if (hid_status.Value == "STEP1")
            {
                progName = "KEYINSTEP1";
            }
            else if (hid_status.Value == "INTERVIEW")
            {
                progName = "INTERVIEW";
            }
            else if (hid_status.Value == "KESSAI" || hid_status.Value == "TCL0")
            {
                progName = "KESSAI";
            }


            CallHisun = new ILDataCenter();

            //***  check before insert or update ***//
            DataSet ds_csms00 = CallHisunCustomer.getCSMS00(hid_CSN.Value).Result;
            if (!CallHisun.check_dataset(ds_csms00))
            {
                lblMsgEN.Text = "Save not complete";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            DataSet ds_csms13 = CallHisunCustomer.getCSMS13(hid_AppNo.Value, hid_brn.Value, hid_CSN.Value).Result;
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun.UserInfomation = userInfoService.GetUserInfo();

            DataRow dr_00 = ds_csms00.Tables[0]?.Rows[0];
            string Date_97 = hidDate97.Value;
            //iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);
            //CallHisun.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref Date_97);            

            string Error = "";
            string m13upg = progName;
            string m13udt = Date_97;
            string m13utm = m_UpdTime.ToString();
            string m13usr = userInfo.Username.ToString(); 
            string m13wks = userInfo.LocalClient.ToString();

            DataSet ds_res = new DataSet();

            //*** Update CSMS00 ***//
            iDB2Command cmd = new iDB2Command();
            //cmd.Parameters.Clear();
            //cmd.CommandText = CallHisun.INSERT_CSMS00HS(hid_CSN.Value);
            //int res_udp_HS_00 = CallHisun.ExecuteNonQuery(cmd);
            //if (res_udp_HS_00 == -1)
            //{
            //    CallHisunCustomer._dataCenter.RollbackMssql();
            //    CallHisunCustomer._dataCenter.CloseConnectSQL();
            //    
            //    
            //    lblMsgEN.Text = "Save not complete";
            //    PopupMsg_judgment.ShowOnPageLoad = true;
            //    return;
            //}
            string M00OFT = dd_off_title.Value.ToString();
            //string M00OFC = txt_off_name.Text.Trim(); **Substring
            string M00OFC = dd_off_name.Text.Trim().Length > 50 ? dd_off_name.Text.Trim().Substring(0, 50) : dd_off_name.Text.Trim();

            //if (CallHisunCustomer._dataCenter.SqlCon.State != ConnectionState.Open)
            //    CallHisunCustomer._dataCenter.SqlCon.Open();
            //CallHisunCustomer._dataCenter.Sqltr?.Connection = CallHisunCustomer._dataCenter.SqlCon.BeginTransaction();

            cmd.Parameters.Clear();
            cmd.CommandText = CallHisunCustomer.UPDATE_CustomerWorked(hid_CSN.Value, Date_97, m_UpdTime.ToString(), userInfo.Username.ToString(), progName, userInfo.LocalClient.ToString(), M00OFT, M00OFC);
            bool transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
            var res_udp_00 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result;
            if (res_udp_00.afrows == -1)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLogString(res_udp_00.message.ToString(), cmd.CommandText.ToString());
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            if (res_udp_00.afrows == 0)
            {
                cmd.CommandText = CallHisunCustomer.INSERT_CustomerWorked(hid_CSN.Value, Date_97, m_UpdTime.ToString(), userInfo.Username.ToString(), progName, userInfo.LocalClient.ToString(), M00OFT, M00OFC);
                transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
                var res_ins_00 = CallHisunCustomer._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (res_ins_00.afrows == -1)
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    Utility.WriteLogString(res_ins_00.message.ToString(), cmd.CommandText.ToString());
                    lblMsgEN.Text = "Save not complete ";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }
            }
            //***  update csms11 ***//
            //cmd.Parameters.Clear();
            //cmd.CommandText = CallHisun.INSERT_CSMS11HS(hid_CSN.Value);
            //int res_11HS = CallHisun.ExecuteNonQuery(cmd);
            //if (res_11HS == -1)
            //{
            //    CallHisunCustomer._dataCenter.RollbackMssql();
            //    CallHisunCustomer._dataCenter.CloseConnectSQL();
            //    
            //    
            //    lblMsgEN.Text = "Save not complete";
            //    PopupMsg_judgment.ShowOnPageLoad = true;
            //    return;
            //}

            //  update csms 11  //
            cmd.Parameters.Clear();
            // ***  HOME
            string sqlCSMS11_H = CallHisunCustomer.UPDATE_CSMS11(CallHisun, userInfo.Username, userInfo.LocalClient, hid_CSN.Value, "H",
                                                     CallHisunCustomer.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()),
                                                     txt_h_ext.Text.Trim(), txt_mobile.Text.Trim(), Date_97, m13upg);
            transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
            var resHome11 = CallHisunCustomer._dataCenter.Execute(sqlCSMS11_H,CommandType.Text, transaction).Result;
            if (resHome11.afrows == -1)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLogString(resHome11.message.ToString(), sqlCSMS11_H);
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            cmd.Parameters.Clear();
            string Err_tel = "";
            try
            {
                CSSR16 cSSR16 = new CSSR16(userInfo);
                cSSR16.checkTelType(CallHisunCustomer._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "H", "P", CallHisun.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()), txt_h_ext.Text.Trim(),
                                                          ref Err_tel);
                //bool call_CSSR16_H = CallHisun.CALL_CSSR16("C", "M", hid_CSN.Value.Trim(), "0", "H", "P", CallHisun.getTel(txt_h_tel.Text.Trim(), txt_h_tel_to.Text.Trim()), txt_h_ext.Text.Trim(),
                //                                          ref Err_tel, userInfo.BizInit, userInfo.BranchNo);
                //if (!call_CSSR16_H && Err_tel.Trim() != "")

                if (Err_tel.Trim() != "")
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }
                //InsertTel(CallHisun.getTel(txt_h_tel.Text.Trim(),txt_h_tel_to.Text.Trim()), "H", "P", txt_h_ext.Text.Trim(), "", "0", 1,Date_97, CallHisun);
            }
            catch (Exception ex)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLog(ex);
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            Err_tel = "";
            try
            {
                CSSR16 cSSR16 = new CSSR16(userInfo);
                //bool call_CSSR16_M = true;
                cSSR16.checkTelType(CallHisunCustomer._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "H", "M", txt_mobile.Text.Trim(), "",
                                                          ref Err_tel);
                //bool call_CSSR16_M = CallHisun.CALL_CSSR16("C", "M", hid_CSN.Value.Trim(), "0", "H", "M", txt_mobile.Text.Trim(), "",
                //                                          ref Err_tel, userInfo.BizInit, userInfo.BranchNo);
                //if (!call_CSSR16_M && Err_tel.Trim() != "")
                if (Err_tel.Trim() != "")
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }

                //InsertTel(txt_mobile.Text.Trim(), "H", "M", "", "", "0", 1,Date_97,CallHisun);
            }
            catch (Exception ex)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLog(ex);
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            //***  Office
            string sqlCSMS11_O = CallHisunCustomer.UPDATE_CSMS11(CallHisun, userInfo.Username, userInfo.LocalClient, hid_CSN.Value, "O",
                                                     CallHisunCustomer.getTel(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()),
                                                     txt_off_tel_ext.Text.Trim(), "", Date_97, m13upg);
            transaction = CallHisunCustomer._dataCenter.Sqltr?.Connection == null ? true : false;
            var resOff11 = CallHisunCustomer._dataCenter.Execute(sqlCSMS11_O,CommandType.Text, transaction).Result;
            if (resOff11.afrows == -1)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLogString(resOff11.message.ToString(), sqlCSMS11_O);
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            cmd.Parameters.Clear();
            Err_tel = "";
            try
            {
                //bool call_CSSR16_O = true;
                CSSR16 cSSR16 = new CSSR16(userInfo);
                cSSR16.checkTelType(CallHisunCustomer._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "O", "P", CallHisun.getTel(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()), txt_off_tel_ext.Text.Trim(),
                                                        ref Err_tel);
                //bool call_CSSR16_O = CallHisun.CALL_CSSR16("C", "M", hid_CSN.Value.Trim(), "0", "O", "P", CallHisun.getTel(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()), txt_off_tel_ext.Text.Trim(),
                //                                          ref Err_tel, userInfo.BizInit, userInfo.BranchNo);
                //InsertTel(CallHisun.getTel(txt_off_phone.Text.Trim(), txt_off_tel_to.Text.Trim()), "O", "P", txt_off_tel_ext.Text.Trim(), "", "0", 1, Date_97, CallHisun);
                if (Err_tel.Trim() != "")
                //if (!call_CSSR16_O && Err_tel.Trim() != "")
                {
                    CallHisunCustomer._dataCenter.RollbackMssql();
                    CallHisunCustomer._dataCenter.CloseConnectSQL();

                    lblMsgEN.Text = "Save not complete";
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                CallHisunCustomer._dataCenter.RollbackMssql();
                CallHisunCustomer._dataCenter.CloseConnectSQL();

                Utility.WriteLog(ex);
                lblMsgEN.Text = "Save not complete ";
                PopupMsg_judgment.ShowOnPageLoad = true;
                return;
            }
            //******************************//
            string TelType = "";
            string Zip = "";
            string O_TLDS = "";
            dd_homeTelType.Items.Clear();
            dd_homeTelType.Text = "";
            dd_homeTelType.SelectedIndex = -1;

            //if (txt_h_tel.Text.Trim() != "")
            //{
            string tel_h = txt_h_tel.Text.Trim();
            if (txt_h_tel_to.Text.Trim() != "")
            {
                tel_h = CallHisunCustomer.getTel(tel_h, txt_h_tel_to.Text);//tel_h + "-" + txt_h_tel_to.Text;
            }
            checkTelType(!string.IsNullOrEmpty(tel_h), !string.IsNullOrEmpty(txt_h_ext.Text.Trim()));
            //bool resTelHome = iLDataSubroutine.Call_GNSR16(hid_CSN.Value, tel_h, txt_h_ext.Text.Trim(), ref TelType, ref Zip, ref O_TLDS, ref Error, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

            //bool resTelHome = CallHisun.Call_GNSR16(hid_CSN.Value, tel_h, txt_h_ext.Text.Trim(), ref TelType, ref Zip, ref O_TLDS, ref Error, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

            //if (resTelHome)
            //{
            //    dd_homeTelType.Items.Clear();
            //    dd_homeTelType.Items.Add(TelType + ":" + O_TLDS, TelType);
            //    dd_homeTelType.SelectedIndex = 1;
            //}
            //else
            //{
            //    dd_homeTelType.Items.Clear();
            //}
            //}
            //else
            //{
            //    dd_homeTelType.Items.Clear();
            //}


            CallHisunCustomer._dataCenter.CommitMssql();
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            
            
            lblMsgEN.Text = "Save completed";
            txt_mobile_j.Text = txt_mobile.Text.Trim();

            PopupMsg_judgment.ShowOnPageLoad = true;
            //  Success //
            return;

        }
        catch (Exception ex)
        {
            CallHisunCustomer._dataCenter.RollbackMssql();
            CallHisunCustomer._dataCenter.CloseConnectSQL();

            Utility.WriteLog(ex);
            lblMsgEN.Text = "Error!! Save not complete ";
            PopupMsg_judgment.ShowOnPageLoad = true;
            return;

        }
    }
    protected void cb_OpenPanel_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_OpenPanel.Checked == true)
        {
            panel_ver_home_office.Enabled = true;

        }
        else
        {
            panel_ver_home_office.Enabled = false;
        }
    }
    protected void txt_birthDate_j_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CallHisun = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisun.UserInfomation = userInfoService.GetUserInfo();

            if (txt_birthDate_j.Text.Length == 10)
            {
                string Age = "";
                string Error = "";
                string birthdate = txt_birthDate_j.Text.Replace("/", "");
                bool resGNP0371 = iLDataSubroutine.CALL_GNP0371(birthdate, "", "DMY", "B", "", "IL", "", userInfo.BizInit, userInfo.BranchNo, ref Age, ref Error);
                //bool resGNP0371 = CallHisun.CALL_GNP0371(birthdate, "", "DMY", "B", "", "IL", "", userInfo.BizInit, userInfo.BranchNo, ref Age, ref Error);
                if (resGNP0371 == false || Error.Trim() == "Y")
                {
                    lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่";
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    txt_birthDate_j.Text = "";
                    txt_birthDate_j.Focus();
                    return;
                }
                //CALL_GNP014
                string oerrCHK = "";
                string oNamed = "";
                bool resGNP014 = iLDataSubroutine.CALL_GNP014(birthdate, "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                //bool resGNP014 = CallHisun.CALL_GNP014(birthdate, "DMY", "B", ref oerrCHK, ref oNamed, userInfo.BizInit, userInfo.BranchNo);
                if (resGNP014 == false || oerrCHK.Trim() == "Y")
                {
                    lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่";
                    CallHisunCustomer._dataCenter.CloseConnectSQL();
                    
                    PopupMsg_judgment.ShowOnPageLoad = true;
                    txt_birthDate_j.Text = "";
                    txt_birthDate_j.Focus();
                    return;
                }

                txt_age.Text = int.Parse(Age.Trim()).ToString();
                lb_day.Text = oNamed.Trim();
                dd_marital_j.Focus();
                CallHisunCustomer._dataCenter.CloseConnectSQL();
                
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            lblMsgEN.Text = "ระบุวันเกิดไม่ถูกต้อง กรุณาระบุใหม่ " + "\r\n";
            CallHisunCustomer._dataCenter.CloseConnectSQL();
            
            PopupMsg_judgment.ShowOnPageLoad = true;
            txt_birthDate_j.Text = "";
            txt_birthDate_j.Focus();
        }
    }
    protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    {
        try
        {
            Cache.Remove("ds_J_ApplyType");
            Cache.Remove("ds_J_ApplyVia");
            Cache.Remove("ds_J_ApplyChannel");
            Cache.Remove("ds_J_Marital");
            Cache.Remove("ds_J_ResidentType");
            Cache.Remove("ds_J_BusinessType");
            Cache.Remove("ds_J_Occupation");
            Cache.Remove("ds_J_Position");
            Cache.Remove("ds_J_EmployeeType");
            Cache.Remove("ds_J_SalaryType");
            Cache.Remove("ds_J_IncomeStatement");
            Cache.Remove("ds_J_TypeOfEmployee");
            Cache.Remove("ds_J_OfficeTitle");
            Cache.Remove("ds_J_Date_of_Income");
            Cache.Remove("ds_J_contract");

            bindData();
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    protected void dd_off_name_TextChanged(object sender, EventArgs e)
    {
        Label_off_name.Text = "";
        //string officename = "";
        //if (dd_off_name.Value != "")
        //{
        //    officename = dd_off_name.Value;
        //}
        string title = "";
        dd_off_name.Items.Clear();

        if (dd_off_name.Text.Trim() == "")
        {
            return;
        }
        try
        {
            if (dd_off_name.Text.Trim().Length >= 3)
            {
                //dd_off_name.Items.Clear();
                if (dd_off_title.Value != null)
                {
                    title = dd_off_title.Value.ToString();
                }
                var b = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlacklistDropDownList", new string[] { dd_off_name.Text, title });
                CompanyBlacklist.responseDDCompanyBlacklist dd_result = JsonConvert.DeserializeObject<CompanyBlacklist.responseDDCompanyBlacklist>(b);

                dd_off_name.Items.Clear();

                if (dd_result != null)
                {
                    //dd_off_name.Items.Add("--- Select ---", "");
                    foreach (var x in dd_result.Content)
                    {
                        dd_off_name.Items.Add(new ListEditItem
                        {
                            Text = x.Text,
                            Value = x.Value
                        });

                        //if (officename != "")
                        //{
                        //    dd_off_name.Value = officename;
                        //}
                    };
                }
            }
            else
            {
                //Label_off_name.Text = "กรุณาใส่ชื่อบริษัทมากกว่า 3 พยัญชนะ";
                Label_off_name.Text = "(ระบุ 3 ตัวอักษรขึ้นไป)";
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
       
    }
    private void bind_DDl_OfficeName()
    {
        try
        {
            var offname = "";
            var offtitle = "";

            if (dd_off_title.Value != null)
            {
                offtitle = dd_off_title.Value.ToString();
            }

            var b = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlacklistDropDownList", new string[] { offname, offtitle });
            CompanyBlacklist.responseDDCompanyBlacklist dd_result = JsonConvert.DeserializeObject<CompanyBlacklist.responseDDCompanyBlacklist>(b);

            dd_off_name.Items.Clear();

            if (dd_result != null)
            {
                //dd_off_name.Items.Add("--- Select ---", "");
                foreach (var x in dd_result.Content)
                {
                    dd_off_name.Items.Add(new ListEditItem
                    {
                        Text = x.Text,
                        Value = x.Value
                    });

                    //if (offname != "")
                    //{
                    //    dd_off_name.Value = offname;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    protected void dd_off_Title_TextChanged(object sender, EventArgs e)
    {
        var offname = "";
        var offtitle = "";

        //if(dd_off_name.Value != null)
        //{
        //    offname = dd_off_name.Value.ToString();
        //}
        if (dd_off_title.Value != null)
        {
            offtitle = dd_off_title.Value.ToString();
        }

        try
        {
            var b = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlacklistDropDownList", new string[] { offname, offtitle });
            CompanyBlacklist.responseDDCompanyBlacklist dd_result = JsonConvert.DeserializeObject<CompanyBlacklist.responseDDCompanyBlacklist>(b);

            dd_off_name.Items.Clear();

            if (dd_result != null)
            {
                //dd_off_name.Items.Add("--- Select ---", "");
                foreach (var x in dd_result.Content)
                {
                    dd_off_name.Items.Add(new ListEditItem
                    {
                        Text = x.Text,
                        Value = x.Value
                    });

                    if (offname != "")
                    {
                        dd_off_name.Value = offname;
                    }
                };
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void checkcharactor(string offname)
    {
        if (offname != null)
        {
            if (offname.Length >= 50)
            {
                string err = "พบข้อมูลชื่อบริษัทมีความยาว 50 ตัวอักษร กรุณากดแก้ไขเพื่อตรวจสอบชื่อบริษัทและเลือกข้อมูลที่ถูกต้องอีกคร้ัง";
                lblMsgEN.Text = err;
                //lblMsgTH.Text = err;
                PopupMsg_judgment.ShowOnPageLoad = true;
            }
        }
    }

    //BOT Report
    #region "BOT Report"
    private void bind_J_SubCompanyBusiness(string code, string csn)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            string accessKey = userInfo.AccessKey;
            var connect = new Connect();
            string url = WebConfigurationManager.AppSettings["CustomerServiceAPIURL"].ToString().Trim();

            DataSet ds = new DataSet();
            //if (Cache["ds_subbus_type"] != null)
            //{
            //    ds = (DataSet)Cache["ds_subbus_type"];
            //}
            //else
            //{
            //    string action = "/CustomerSubWorked/GetSubCompanyBusiness";
            //    JObject jsonData = new JObject();
            //    Hashtable header = new Hashtable();
            //    header.Add("AccessKey", accessKey);
            //    var response = connect.ConnectAPI(url, action, "", "GET", header);

            //    var json = JsonConvert.SerializeObject(response.data);
            //    var ReturnDt = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));
            

            //    Cache["ds_subbus_type"] = ds;
            //    Cache.Insert("ds_subbus_type", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
            //}

            string action = "/CustomerSubWorked/GetSubCompanyBusiness";
            JObject jsonData = new JObject();
            Hashtable header = new Hashtable();
            header.Add("AccessKey", accessKey);
            var response = connect.ConnectAPI(url, action, "", "GET", header);

            var json = JsonConvert.SerializeObject(response.data);
            //var resultSubCompany = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));
            //ds.Clear();
            ds.Tables.Add((DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable)));


            CallHisun = new ILDataCenter();
            dd_subbus_type.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_subbus_type.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_subbus_type.Items.Add(
                        new ListEditItem(dr["Code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["Code"].ToString().Trim()));
                }

                if (code != "" && code != "03")
                {
                    dd_subbus_type.Enabled = false;
                }
                else if ( code == "")
                {
                    dd_subbus_type.SelectedIndex = -1;
                }

                GetCustomerSubWorked(csn);
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

   // Fix bug call  CustomerServiceAPI
    private void bind_J_SubOccupation(string code, string csn)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            string accessKey = userInfo.AccessKey;

            DataSet ds = new DataSet();
            //if (Cache["ds_suboccupation"] != null)
            //{
            //    ds = (DataSet)Cache["ds_suboccupation"];
            //}
            //else
            //{
            //    var connect = new Connect();
            //    string url = WebConfigurationManager.AppSettings["CustomerServiceAPIURL"].ToString().Trim();
            //    string action = "/CustomerSubWorked/GetSubOccupation";
            //    JObject jsonData = new JObject();
            //    Hashtable header = new Hashtable();
            //    header.Add("AccessKey", accessKey);
            //    var response = connect.ConnectAPI(url, action, "", "GET", header);

            //    var json = JsonConvert.SerializeObject(response.data);
            //    var ReturnDt = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));

            //    Cache["ds_suboccupation"] = ds;
            //    Cache.Insert("ds_suboccupation", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
            //}
            var connect = new Connect();
            string url = WebConfigurationManager.AppSettings["CustomerServiceAPIURL"].ToString().Trim();
            string action = "/CustomerSubWorked/GetSubOccupation";
            JObject jsonData = new JObject();
            Hashtable header = new Hashtable();
            header.Add("AccessKey", accessKey);
            var response = connect.ConnectAPI(url, action, "", "GET", header);

            var json = JsonConvert.SerializeObject(response.data);
            //var resultSubOccupation = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));
            //ds.Clear();
            ds.Tables.Add((DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable)));
            CallHisun = new ILDataCenter();
            dd_suboccup.Items.Clear();

            if (CallHisun.check_dataset(ds))
            {
                dd_suboccup.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_suboccup.Items.Add(
                        new ListEditItem(dr["Code"].ToString().Trim() + " : " + dr["DescriptionTHAI"].ToString().Trim(), dr["ID"].ToString().Trim()));
                }

                if (code == "051" || code == "052" || code == "054" || code == "055")
                {
                    dd_suboccup.Value = "13";
                    dd_suboccup.Enabled = false;
                }
                else if (code == "")
                {
                    dd_suboccup.SelectedIndex = -1;
                }
                

                GetCustomerSubWorked(csn);
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    private void GetCustomerSubWorked(string csn)
    {
        try
        {
            if (csn != "")
            {
                userInfoService = new UserInfoService();
                userInfo = userInfoService.GetUserInfo();
                string accessKey = userInfo.AccessKey;
                var connect = new Connect();
                string url = WebConfigurationManager.AppSettings["CustomerServiceAPIURL"].ToString().Trim();

                string action_csn = "/CustomerGeneral/GetCustomerGeneralCisn/" + csn;
                Hashtable header = new Hashtable();
                header.Add("AccessKey", accessKey);
                var response_csn = connect.ConnectAPI(url, action_csn, "", "GET", header);
                var json_csn = JsonConvert.SerializeObject(response_csn.data);
                CustomerGeneral customergeneral = JsonConvert.DeserializeObject<CustomerGeneral>(json_csn);

                if (response_csn.data != null)
                {
                    DataSet ds = new DataSet();

                    string action_subwork = "/CustomerSubWorked/GetCustomerSubWorkedByCustId/" + customergeneral.id;
                    var response_subwork = connect.ConnectAPI(url, action_subwork, "", "GET", header);
                    var json_subwork = JsonConvert.SerializeObject(response_subwork.data);

                    //var resultSubWorked = (DataTable)JsonConvert.DeserializeObject(json_subwork, typeof(DataTable));
                    //ds.Clear();
                    ds.Tables.Add((DataTable)JsonConvert.DeserializeObject(json_subwork, typeof(DataTable)));
                    DataRow dr = ds.Tables[0]?.Rows.Count > 0 ? ds.Tables[0].Rows[0] : null ;

                    if (response_subwork.data != null && dr != null)
                    {
                        dd_subbus_type.Value = dr["SubCompanyBusiness"].ToString().Trim();
                        dd_suboccup.Value = dr["SubOccupation"].ToString().Trim();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }

    protected void dd_busType_TextChanged(object sender, EventArgs e)
    {
        if (dd_busType_j.Value.ToString() != "" && dd_busType_j.Value.ToString() != "03")
        {
            dd_subbus_type.Value = "";
            dd_subbus_type.Enabled = false;
        }
        else
        {
            dd_subbus_type.Enabled = true;
        }
    }

    public class CustomerGeneral
    {
        public int id { get; set; }
        public int titleID { get; set; }
        public DateTime idCardExpiredDate { get; set; }
    }

    public class CustomerSubWorked
    {
        public int ID { get; set; }
        public int CustID { get; set; }
        public string SubCompanyBusiness { get; set; }
        public string SubOccupation { get; set; }
    }

    private void SaveCustomerSubWorked()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            string accessKey = userInfo.AccessKey;

            var connect = new Connect();
            string url = WebConfigurationManager.AppSettings["CustomerServiceAPIURL"].ToString().Trim();
            string action = "/CustomerGeneral/GetCustomerGeneralCisn/" + hid_CSN.Value;

            Hashtable header = new Hashtable();
            header.Add("AccessKey", accessKey);
            var response = connect.ConnectAPI(url, action, "", "GET", header);
            var json = JsonConvert.SerializeObject(response.data);
            CustomerGeneral customergeneral = JsonConvert.DeserializeObject<CustomerGeneral>(json);

            var subbus_type = "";
            if (dd_subbus_type.Value == null)
            {
                subbus_type = "";
            }
            else
            {
                subbus_type = dd_subbus_type.Value.ToString();
            }

            var suboccup = "";
            if (dd_suboccup.Value.ToString() == "" || dd_suboccup.Value == null)
            {
                suboccup = "";
            }
            else
            {
                suboccup = dd_suboccup.Value.ToString();
            }

            JObject jsonData = new JObject();
            jsonData.Add("ID", 0);
            jsonData.Add("CustID", customergeneral.id);
            jsonData.Add("SubCompanyBusiness", subbus_type);
            jsonData.Add("SubOccupation", suboccup);
            jsonData.Add("Application", "ILSystem");
            jsonData.Add("CreateBy", userInfo.Username.ToString());
            jsonData.Add("CreateDate", DateTime.Now);
            jsonData.Add("UpdateBy", userInfo.Username.ToString());
            jsonData.Add("UpdateDate", DateTime.Now);
            jsonData.Add("IsDelete", "");

            string actionSave = "/CustomerSubWorked/InsertCustomerSubWorked";
            var responseSave = connect.ConnectAPI(url, actionSave, JsonConvert.SerializeObject(jsonData), "POST", header);

            //var jsonSave = JsonConvert.SerializeObject(responseSave);
            //var ReturnDtSave = (DataTable)JsonConvert.DeserializeObject(jsonSave, typeof(DataTable));
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            //lblMsgEN.Text = ex.Message.ToString();
            //throw new Exception(ex.Message.ToString());
        }
    }
    #endregion
}