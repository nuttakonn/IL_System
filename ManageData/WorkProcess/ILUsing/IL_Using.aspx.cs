using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Helper;
using ILSystem.App_Code.Model.CompanyBlacklist;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ILSystem.App_Code.Model.AS400DB01.AS400DB01Model;
using static ILSystem.App_Code.BLL.DataCenter.ILDataSubroutine;
using EB_Service.Commons;
using System.Security.Cryptography;
using ESB.WebAppl.ILSystem.commons;
using System.Web.Services.Description;
using EB_Service.DAL;
using System.Windows.Interop;
using Org.BouncyCastle.Ocsp;
using Newtonsoft.Json.Linq;
using ILSystem.App_Code.BLL.Integrate;
using ILSystem.App_Code.Model.NOTEAPI;
using System.Threading.Tasks;

public partial class ManageData_WorkProcess_ChangeData_Received_Case_Using : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    private string G_sql = "";
    private string G_location = "300";
    private ILDataCenter ilObj;
    ILDataCenterMssql CallMasterEnt;
    ILDataCenter CallHisun;
    // Connect_GeneralAPI conn_general ;
    private readonly connectAPI _connectAPI;
    ILDataSubroutine iLDataSubroutine;
    ILDataCenterMssqlKeyinStep1 ILDataCenterMssqlKeyin;
    ILDataCenterMssqlUsingCard CallEntUsingCard;
    DataCenter dataCenter;
    UserInfo userInfo;
    public UserInfoService userInfoService;
    public ManageData_WorkProcess_ChangeData_Received_Case_Using()
    {
        _connectAPI = new connectAPI();
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();

        dataCenter = new DataCenter(userInfoService.GetUserInfo());
        if (!Page.User.Identity.IsAuthenticated || userInfoService.GetUserInfo() == null)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            catch { }
            Response.Redirect(Request.ApplicationPath + "/ManageData/Home/Home.aspx");

        }

        if (IsCallback)
        {

            Thread.Sleep(500);
        }
        if (IsPostBack)
        {
            if (!(String.IsNullOrEmpty(txt_birthDate_P.Text.Trim())))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "script1", "txt_birthDate_P.SetValue('" + txt_birthDate_P.Text + "')", true);


            }
            if (!(String.IsNullOrEmpty(txt_telM.Text.Trim())))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "script2", "txt_telM.SetValue('" + txt_telM.Text + "')", true);
            }
        }
        else
        {
            txt_card_no.Focus();
            pl_cust.Enabled = false;
            tabDetail.TabPages.FindByName("TabTCL").ClientEnabled = false;
        }


    }


    #region ****--- function ---****

    private void bind_ProductType()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            ds = CallMasterEnt.getProductType("");
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_prodType.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_prodType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_prodType.Items.Add(
                        new ListEditItem(dr["T40TYP"].ToString().Trim() + " : " + dr["T40DES"].ToString().Trim(), dr["T40TYP"].ToString().Trim()));
                }
            }
            dd_prodType.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }
    private void bindProductBrand(string desc)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            string brand = dd_prodBrand.Text.Trim();
            ds = CallMasterEnt.getProductBrand(brand);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_prodBrand.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_prodBrand.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_prodBrand.Items.Add(
                        new ListEditItem(dr["T42BRD"].ToString().Trim() + " : " + dr["T42DES"].ToString().Trim(), dr["T42BRD"].ToString().Trim()));
                }
            }
            dd_prodBrand.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindProductCode(string desc, string Type)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            ds = CallMasterEnt.getProductCode(desc, Type);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_prodcode.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_prodcode.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_prodcode.Items.Add(
                        new ListEditItem(dr["T41COD"].ToString().Trim() + " : " + dr["T41DES"].ToString().Trim(), dr["T41COD"].ToString().Trim()));
                }
            }
            dd_prodcode.SelectedIndex = -1;


        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindReason()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            ds = CallMasterEnt.getResultCode();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_reason.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_reason.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_reason.Items.Add(
                        new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
                }

                dd_reason.Value = "A";
            }


        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindMaritalStatus(string maritalSts)
    {

        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            // conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            ds = CallEntUsingCard.getGenMaritalStatus(maritalSts).Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_marital.Items.Clear();
            lb_marital.Text = "";
            if (CallHisun.check_dataset(ds))
            {
                dd_marital.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_marital.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                if (ds.Tables[0].Rows.Count == 1)
                {
                    dd_marital.SelectedIndex = 1;
                    dd_marital.Enabled = false;
                }
                else
                {
                    dd_marital.Enabled = true;
                }
            }
            else
            {
                if (Cache["ds_MaritalStatusN"] != null)
                {
                    ds = (DataSet)Cache["ds_MaritalStatusN"];
                }
                else
                {
                    ds = CallMasterEnt.getGeneralCenter("MaritalStatusID");
                    //   ds.Tables.Add(conn_general.GetGeneralMaritalStatus());
                    Cache["ds_MaritalStatusN"] = ds;
                    Cache.Insert("ds_MaritalStatusN", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
                }
                dd_marital.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_marital.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                lb_marital.Text = "[" + maritalSts + "]";
                dd_marital.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindBusinessType(string bizType)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = new DataSet();
            ds = CallMasterEnt.getTypeBusiness(bizType);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_busType.Items.Clear();
            lb_busType.Text = "";
            if (CallHisun.check_dataset(ds))
            {
                dd_busType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_busType.Items.Add(
                        new ListEditItem(dr["Code"].ToString().Trim() + " : " + dr["DescriptionTHAI"].ToString().Trim(), dr["Code"].ToString().Trim()));
                }

                if (ds.Tables[0].Rows.Count == 1)
                {


                    dd_busType.SelectedIndex = 1;
                    dd_busType.Enabled = false;
                }
                else
                {
                    dd_busType.Enabled = true;
                }
            }
            else
            {
                DataSet ds_ = CallMasterEnt.getTypeBusiness("");
                CallMasterEnt._dataCenter.CloseConnectSQL();
                dd_busType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds_.Tables[0]?.Rows)
                {
                    dd_busType.Items.Add(
                        new ListEditItem(dr["Code"].ToString().Trim() + " : " + dr["DescriptionTHAI"].ToString().Trim(), dr["Code"].ToString().Trim()));
                }
                lb_busType.Text = "[" + bizType + "]";
                dd_busType.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void bindResidentType(string residentType)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            //  conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            ds = CallEntUsingCard.getGenResidentType(residentType).Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (CallHisun.check_dataset(ds))
            {
                dd_resident.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_resident.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                if (ds.Tables[0].Rows.Count == 1)
                {
                    dd_resident.SelectedIndex = 1;
                    dd_resident.Enabled = false;
                }
                else
                {
                    dd_resident.Enabled = true;
                }
            }
            else
            {


                if (Cache["ds_ResidentTypeN"] != null)
                {
                    ds = (DataSet)Cache["ds_ResidentTypeN"];
                }
                else
                {
                    ds = CallMasterEnt.getGeneralCenter("ResidentalStatusID");
                    // ds.Tables.Add(conn_general.getResidentType());
                    Cache["ds_ResidentTypeN"] = ds;
                    Cache.Insert("ds_ResidentTypeN", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_resident.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                lb_resident.Text = "[" + residentType + "]";
                dd_resident.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }

    }

    private void bindOccupation(string occupation)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            //  conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            ds = CallEntUsingCard.getGenOccupation(occupation).Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();

            lb_occup.Text = "";
            dd_occup.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_occup.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_occup.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                if (ds.Tables[0].Rows.Count == 1)
                {
                    dd_occup.SelectedIndex = 1;
                    dd_occup.Enabled = false;
                }
                else
                {
                    dd_occup.Enabled = true;
                }
            }
            else
            {
                if (Cache["ds_Occupation"] != null)
                {
                    ds = (DataSet)Cache["ds_Occupation"];
                }
                else
                {
                    ds = CallMasterEnt.getGeneralCenter("OccupationID");
                    // ds.Tables.Add(conn_general.GetGeneralOccupation());
                    Cache["ds_Occupation"] = ds;
                    Cache.Insert("ds_Occupation", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_occup.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                lb_occup.Text = "[" + occupation + "]";
                dd_occup.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindPosition(string position)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            //  conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            ds = CallEntUsingCard.getGenPosition(position).Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_position.Items.Clear();
            lb_position.Text = "";
            if (CallHisun.check_dataset(ds))
            {
                dd_position.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_position.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }

                if (ds.Tables[0].Rows.Count == 1)
                {
                    dd_position.SelectedIndex = 1;
                    dd_position.Enabled = false;
                }
                else
                {
                    dd_position.Enabled = true;
                }
            }
            else
            {
                if (Cache["ds_Position"] != null)
                {
                    ds = (DataSet)Cache["ds_Position"];
                }
                else
                {
                    ds = CallMasterEnt.getGeneralCenter("PositionID");
                    // ds.Tables.Add(conn_general.GetGeneralPosition());
                    Cache["ds_Position"] = ds;
                    Cache.Insert("ds_Position", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }
                lb_position.Text = "[" + position + "]";
                dd_position.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindEmployeeType(string employeeType)
    {

        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            //   conn_general = new Connect_GeneralAPI();
            DataSet ds = new DataSet();
            ds = CallEntUsingCard.getEmployeeType(employeeType).Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_empType.Items.Clear();
            lb_empType.Text = "";
            if (CallHisun.check_dataset(ds))
            {
                dd_empType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_empType.Items.Add(

                    new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["descriptionTHAI"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                if (ds.Tables[0].Rows.Count == 1)
                {
                    dd_empType.SelectedIndex = 1;
                    dd_empType.Enabled = false;
                }
                else
                {
                    dd_empType.Enabled = true;
                }
            }
            else
            {
                if (Cache["ds_EmployeeType"] != null)
                {
                    ds = (DataSet)Cache["ds_EmployeeType"];
                }
                else
                {
                    ds = CallMasterEnt.getGeneralCenter("EmploymentTypeID");
                    //  ds.Tables.Add(conn_general.GetGeneralEmployment());
                    Cache["ds_EmployeeType"] = ds;
                    Cache.Insert("ds_EmployeeType", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }
                lb_empType.Text = "[" + employeeType + "]";
                dd_empType.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindCommercial(string occupation)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = CallMasterEnt.getCommercialRegister(occupation);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_comerc.Items.Clear();
            dd_comerc.Items.Add("", "");
            if (CallHisun.check_dataset(ds))
            {
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_comerc.Items.Add(
                        new ListEditItem(dr["code"].ToString().Trim() + " : " + dr["desc"].ToString().Trim(), dr["code"].ToString().Trim()));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dd_comerc.Enabled = false;
                    dd_comerc.Enabled = true;

                }
                else
                {
                    dd_comerc.Enabled = false;
                }
            }
            else
            {
                dd_comerc.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    private void bindPaymentType()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            CallHisun = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getPaymentType();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_paymentType.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_paymentType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_paymentType.Items.Add(
                        new ListEditItem(dr["gt48tc"].ToString().Trim() + " : " + dr["gt48td"].ToString().Trim(), dr["gt48tc"].ToString().Trim()));
                }
                dd_paymentType.SelectedIndex = 2;
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }
    //*** get customer account **//
    private void bindDebitAccount(string csn)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            dd_bankCode.Items.Clear();
            dd_accountType.Items.Clear();
            CallHisun = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            //***  bankcode ***//
            DataSet ds_bankCode = new DataSet();
            DataSet ds = iLDataSubroutine.getBankCode();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (CallHisun.check_dataset(ds_bankCode))
            {
                dd_bankCode.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds_bankCode.Tables[0]?.Rows)
                {
                    dd_bankCode.Items.Add(
                        new ListEditItem(dr["g32bnk"].ToString().Trim() + " : " + dr["gnb30c"].ToString().Trim(), dr["g32bnk"].ToString().Trim()));
                }
                dd_bankCode.SelectedIndex = -1;
            }

            DataSet ds_accountType = iLDataSubroutine.getAccountType();
            if (CallHisun.check_dataset(ds_accountType))
            {
                dd_accountType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds_accountType.Tables[0]?.Rows)
                {
                    dd_accountType.Items.Add(
                        new ListEditItem(dr["gn13cd"].ToString().Trim() + " : " + dr["gn13td"].ToString().Trim(), dr["gn13cd"].ToString().Trim()));
                }
                dd_accountType.SelectedIndex = -1;
            }


            DataSet ds_debit = iLDataSubroutine.getDebitAccountByCSN(csn);
            if (CallHisun.check_dataset(ds_debit))
            {
                DataRow dr_debit = ds_debit.Tables[0]?.Rows[0];
                bindBankBranch(dr_debit?["gnb31a"].ToString().Trim());
                dd_bankCode.Value = dr_debit?["gnb31a"].ToString().Trim();
                dd_bankBranch.Value = dr_debit?["gnb31c"].ToString().Trim();
                dd_accountType.Value = dr_debit?["gn13cd"].ToString().Trim();
                txt_AccountNo.Text = dr_debit?["p00bac"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    // *** bind bank branch ***//
    private void bindBankBranch(string bankCode)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallHisun = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getBankBranch(bankCode);
            dataCenter.CloseConnectSQL();
            dd_bankBranch.Items.Clear();
            if (CallHisun.check_dataset(ds))
            {
                dd_bankBranch.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_bankBranch.Items.Add(
                        new ListEditItem(dr["gnb31c"].ToString().Trim() + " : " + dr["gnb31d"].ToString().Trim(), dr["gnb31c"].ToString().Trim()));
                }
                dd_bankBranch.SelectedIndex = -1;
            }

        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    //***  bind vendor ***//
    private void bindVendor()
    {
        try
        {
            ilObj = new ILDataCenter();
            userInfo = userInfoService.GetUserInfo();
            ilObj.UserInfomation = userInfo;
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getVendor("", userInfo.BranchApp);
            dataCenter.CloseConnectSQL();
            dd_vendor.Items.Clear();
            string LogVen = JsonConvert.SerializeObject(ds);
            if (ilObj.check_dataset(ds))
            {
                dd_vendor.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_vendor.Items.Add(
                        new ListEditItem(dr["p10ven"].ToString().Trim() + ":" + dr["p10nam"].ToString().Trim() + "[" + dr["p10fi1"].ToString().Trim() + "]", dr["p10ven"].ToString().Trim() + "|" + dr["P16RNK"].ToString().Trim()));
                }
            }
        }
        catch (Exception ex)
        {
            Task.Run(() => { Utility.WriteLog(ex); });
        }
    }

    //***  bind Campaign ***//
    private void bindCampaign()
    {
        lb_prodcount.Text = "";
        try
        {
            string[] appDate_s = txt_appDate.Text.Trim().Split('/');
            string appDate = appDate_s[2] + appDate_s[1] + appDate_s[0];

            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            DataSet ds = CallMasterEnt.getCampaign(vendor?[0], txt_totalterm.Text.Trim(), "", "", "", appDate);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            dd_campaign.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_campaign.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0]?.Rows)
                {
                    dd_campaign.Items.Add(
                        new ListEditItem(dr["c01cmp"].ToString().Trim() + ":" + dr["c01cnm"].ToString().Trim(), dr["c01cmp"].ToString().Trim() + "|" + dr["c02csq"].ToString().Trim() + "|" + dr["c02rsq"].ToString().Trim()));
                }
            }
            dd_product.Focus();
        }
        catch (Exception ex)
        {
            lb_prodcount.Text = " ไม่พบข้อมูลที่ค้นหา , กรุณาลองใหม่อีกครั้ง ";
        }
    }

    //***  bind Product ***//
    protected void bindProduct(string vendorCode, string campCode, string campSeq, string product = "")
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            ILDataCenterMssqlKeyinStep1 iLDataCenterMssql = new ILDataCenterMssqlKeyinStep1(userInfoService.GetUserInfo());
            ilObj = new ILDataCenter();
            DataSet ds = iLDataCenterMssql.getProductUS_<DataSet>(vendorCode, campCode, campSeq, product).Result;
            iLDataCenterMssql._dataCenter.CloseConnectSQL();
            dd_product.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                int countProd = ds.Tables[0].Rows.Count;
                if (countProd <= 4000)
                {
                    dd_product.Items.Add("--- Select ---", "");
                    foreach (DataRow dr in ds.Tables[0]?.Rows)
                    {
                        dd_product.Items.Add(
                            new ListEditItem(dr["t44itm"].ToString().Trim() + " : " + dr["t44des"].ToString().Trim(),
                                dr["t44itm"].ToString().Trim() + "|" +
                                dr["c07min"].ToString().Trim() + "|" +
                                dr["c07max"].ToString().Trim() + "|" +
                                dr["T44PGP"].ToString().Trim()
                                ));
                    }
                    dd_product.SelectedIndex = -1;
                    lb_prodcount.Text = "Macthing " + countProd.ToString() + " items";
                }
                else
                {
                    lb_prodcount.Text = "ระบุ Product Code หรือ Product Name เสร็จแล้วกดปุ่ม [Enter] ";
                }
            }
            else
            {
                lb_prodcount.Text = "Macthing " + "0" + " item";
            }
        }
        catch (Exception ex)
        {
            lb_prodcount.Text = "Macthing " + "0" + " item";
        }
    }

    // ***  Business Process ***//
    public bool verifyCustomerN(string CardNo, string IDNo, string csn, ref string errMsg, ref string typeErr)
    {

        try
        {
            userInfo = userInfoService.GetUserInfo();
            typeErr = "";     //  for check reject type
            errMsg = "";
            ilObj = new ILDataCenter();
            DataCenter dataCenter = new DataCenter(userInfoService.GetUserInfo());
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            try
            {

                string Error_CSSRRFAM = "", ErrorMsg_CSSRRFAM = "";
                bool CALL_CSSRRFAM = iLDataSubroutine.CALL_CSSRRFAM(csn.Trim(), ref Error_CSSRRFAM, ref ErrorMsg_CSSRRFAM, userInfo.BizInit, userInfo.BranchNo);
                //ilObj.CloseConnectioDAL();
                if (Error_CSSRRFAM.ToString().Trim() == "Y")
                {
                    if (ErrorMsg_CSSRRFAM.ToString().Trim() != "")
                    {
                        //G_RFCM.Text = "Y";
                        if (ErrorMsg_CSSRRFAM.ToString().Trim().Substring(0, 1) == "R")
                        {
                            errMsg = "Not pass: Refinance ";
                            typeErr = "R";

                        }
                        if (ErrorMsg_CSSRRFAM.ToString().Trim().Substring(0, 1) == "C")
                        {
                            errMsg = "Not pass: Compromise ";
                            typeErr = "C";
                        }
                        if (ErrorMsg_CSSRRFAM.ToString().Trim() == "WR")
                        {
                            errMsg = "Not pass: Wait Refinance ";
                            typeErr = "WR";
                        }
                        if (ErrorMsg_CSSRRFAM.ToString().Trim() == "WC")
                        {
                            errMsg = "Not pass: Wait Compromise ";
                            typeErr = "WC";
                        }
                    }
                    //  ilObj.CloseConnectioDAL();
                    return false;
                }

            }
            catch
            {
                errMsg = "Erorr on Call CSSRRFAM";
                typeErr = "";
                //  ilObj.CloseConnectioDAL();
                return false;
            }






            //  check gncsc07 //
            DataSet dsGNCSC07 = new DataSet();
            Boolean GNCSC07 = iLDataSubroutine.Call_GNCSC07(IDNo, "CS", userInfo.BranchNo.ToString(), ref dsGNCSC07);
            // ilObj.CloseConnectioDAL();

            if (ilObj.check_dataset(dsGNCSC07))
            {
                DataRow dr_GNCSC07 = dsGNCSC07.Tables[1]?.Rows[0];
                string errCode = dr_GNCSC07?["WCTYP1"].ToString().Trim();
                /*
                if ((dr_GNCSC07["WCTYP1"].ToString().Trim() == "07") & (dr_GNCSC07["WMAX"].ToString().Trim() == "1"))
                {
                    errCode = "00";
                }
                */
                if (errCode == "00")
                {

                    #region check ILSR75 ใหม่ check ILSR75 ก่อน แล้ว check blacklist company ต่อ

                    // ***  check ILSR75  ***//
                    string err = "";
                    string msg = "";
                    bool boolILR75 = iLDataSubroutine.Call_ILSR75(CardNo.Replace("-", ""), userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref err, ref msg);
                    // ilObj.CloseConnectioDAL();
                    if (!boolILR75 || err.Trim() == "Y" || msg.Trim() != "")
                    {
                        errMsg = msg;
                        typeErr = "";
                        return false;
                    }
                    else
                    {
                        ILDataCenter busobj = new ILDataCenter();
                        DataSet DS = new DataSet();
                        busobj.UserInfomation = userInfo;
                        var off_name = "";
                        var off_title = "";
                        DS = CallEntUsingCard.getOfficeTitleUS(IDNo).Result;
                        CallEntUsingCard._dataCenter.CloseConnectSQL();
                        if (DS != null)
                        {
                            foreach (DataRow dr in DS.Tables[0].Rows)
                            {
                                off_name = dr["off_name"].ToString().Trim();
                                off_title = dr["off_title"].ToString().Trim();
                            }
                        }
                        //  busobj.CloseConnectioDAL();

                        try
                        {
                            var res = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlackList", new string[] { off_name, off_title });
                            CompanyBlacklist.responseCompanyBlacklist companyBlacklist = JsonConvert.DeserializeObject<CompanyBlacklist.responseCompanyBlacklist>(res);
                            if (companyBlacklist.Success == false)
                            {
                                msg += "ไม่สามารถตรวจสอบข้อมูลบริษัทได้" + "\r\n" + companyBlacklist.Message;
                                errMsg = msg;
                                typeErr = "";
                                return false;
                            }
                            if (companyBlacklist.Success == true)
                            {
                                if (companyBlacklist.data != null)
                                {
                                    string WPERR = "";
                                    string WPHSTS = "";
                                    string WPMSG = "";
                                    bool res_cs = iLDataSubroutine.Call_GNSRCS(IDNo, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);

                                    if (!res_cs || WPERR == "Y")
                                    {
                                        msg += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้" + "\r\n";
                                        errMsg = msg;
                                        typeErr = "";
                                        return true;
                                    }
                                    if (WPHSTS == "N") // ลูกค้าใหม่  
                                    {
                                        if (companyBlacklist.data.CompanyFlag == "P")
                                        {
                                            errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Blacklist(P).";
                                            typeErr = "";
                                            return false;
                                        }
                                        else if (companyBlacklist.data.CompanyFlag == "T")
                                        {
                                            errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Temporary Blacklist(T).";
                                            typeErr = "";
                                            return false;
                                        }
                                    }
                                    else if (WPHSTS == "T")
                                    {
                                        if (companyBlacklist.data.CompanyFlag == "P")
                                        {
                                            errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Blacklist(P). \n Do you continue to Save Data?";
                                            typeErr = "";
                                            return false;
                                        }
                                        else if (companyBlacklist.data.CompanyFlag == "T")
                                        {
                                            errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Temporary Blacklist(T). \n Do you continue to Save Data?";
                                            typeErr = "";
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        lblMsgTH.Text = "";
                                        if (companyBlacklist.data.CompanyFlag == "T")
                                        {
                                            lblMsgEN.Text = "Office Name : " + off_name + " is temporary Company Blacklist(T).";
                                        }
                                        else if (companyBlacklist.data.CompanyFlag == "P")
                                        {
                                            lblMsgEN.Text = "Office Name : " + off_name + " is Company Blacklist(P).";
                                        }
                                        else if (companyBlacklist.data.CompanyFlag == "S")
                                        {
                                            lblMsgEN.Text = "Office Name : " + off_name + " is pending Company Blacklist(S).";
                                        }
                                        //lblMsgEN.Text = "Save Credit Model : not complete.";
                                        PopupMsg.ShowOnPageLoad = true;
                                        return true;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsg = "Error , Ex : " + ex.Message;
                            typeErr = "";
                            return false;
                        }
                        return true;
                    }
                    #endregion

                    //#region ไม่เช็ค ILSR75 19/09/2024
                    //string msg = "";
                    //ILDataCenter busobj = new ILDataCenter();
                    //DataSet DS = new DataSet();
                    //busobj.UserInfomation = userInfo;
                    //var off_name = "";
                    //var off_title = "";
                    //DS = CallEntUsingCard.getOfficeTitleUS(IDNo).Result;
                    //CallEntUsingCard._dataCenter.CloseConnectSQL();
                    //if (DS != null)
                    //{
                    //    foreach (DataRow dr in DS.Tables[0]?.Rows)
                    //    {
                    //        off_name = dr["off_name"].ToString().Trim();
                    //        off_title = dr["off_title"].ToString().Trim();
                    //    }
                    //}
                    ////  busobj.CloseConnectioDAL();

                    //try
                    //{
                    //    var res = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlackList", new string[] { off_name, off_title });
                    //    CompanyBlacklist.responseCompanyBlacklist companyBlacklist = JsonConvert.DeserializeObject<CompanyBlacklist.responseCompanyBlacklist>(res);
                    //    if (companyBlacklist.Success == false)
                    //    {
                    //        msg += "ไม่สามารถตรวจสอบข้อมูลบริษัทได้" + "\r\n" + companyBlacklist.Message;
                    //        errMsg = msg;
                    //        typeErr = "";
                    //        return false;
                    //    }
                    //    if (companyBlacklist.Success == true)
                    //    {
                    //        if (companyBlacklist.data != null)
                    //        {
                    //            string WPERR = "";
                    //            string WPHSTS = "";
                    //            string WPMSG = "";
                    //            bool res_cs = iLDataSubroutine.Call_GNSRCS(IDNo, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);

                    //            if (!res_cs || WPERR == "Y")
                    //            {
                    //                msg += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้" + "\r\n";
                    //                errMsg = msg;
                    //                typeErr = "";
                    //                return true;
                    //            }
                    //            if (WPHSTS == "N") // ลูกค้าใหม่  
                    //            {
                    //                if (companyBlacklist.data.CompanyFlag == "P")
                    //                {
                    //                    errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Blacklist(P).";
                    //                    typeErr = "";
                    //                    return false;
                    //                }
                    //                else if (companyBlacklist.data.CompanyFlag == "T")
                    //                {
                    //                    errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Temporary Blacklist(T).";
                    //                    typeErr = "";
                    //                    return false;
                    //                }
                    //            }
                    //            else if (WPHSTS == "T")
                    //            {
                    //                if (companyBlacklist.data.CompanyFlag == "P")
                    //                {
                    //                    errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Blacklist(P). \n Do you continue to Save Data?";
                    //                    typeErr = "";
                    //                    return false;
                    //                }
                    //                else if (companyBlacklist.data.CompanyFlag == "T")
                    //                {
                    //                    errMsg += "Cannot Approve, Office Name : " + off_name + " is Company Temporary Blacklist(T). \n Do you continue to Save Data?";
                    //                    typeErr = "";
                    //                    return false;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                lblMsgTH.Text = "";
                    //                if (companyBlacklist.data.CompanyFlag == "T")
                    //                {
                    //                    lblMsgEN.Text = "Office Name : " + off_name + " is temporary Company Blacklist(T).";
                    //                }
                    //                else if (companyBlacklist.data.CompanyFlag == "P")
                    //                {
                    //                    lblMsgEN.Text = "Office Name : " + off_name + " is Company Blacklist(P).";
                    //                }
                    //                else if (companyBlacklist.data.CompanyFlag == "S")
                    //                {
                    //                    lblMsgEN.Text = "Office Name : " + off_name + " is pending Company Blacklist(S).";
                    //                }
                    //                //lblMsgEN.Text = "Save Credit Model : not complete.";
                    //                PopupMsg.ShowOnPageLoad = true;
                    //                return true;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return true;
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    errMsg = "Error , Ex : " + ex.Message;
                    //    typeErr = "";
                    //    return false;
                    //}
                    //return true;
                    //#endregion
                }
                else
                {
                    //***  show error ***//

                    switch (errCode)
                    {
                        case "01":
                            errMsg = "Not pass: Age more than 65 years or birthday less than 01/11/2506";
                            typeErr = "01";
                            break;
                        case "02":
                            //errMsg = "Not pass: AMLO / Black list / Last Contract is Branch 911 / Working outside service area.";
                            errMsg = "Not pass: AMLO / Black list / Last Contract is Branch 911.";
                            typeErr = "02";
                            break;
                        case "03":
                            errMsg = "Not pass: GB / Write Off";
                            typeErr = "03";
                            break;
                        case "04":
                            errMsg = "Not pass: Refinance / IL Flood / Waive Nego";
                            typeErr = "04";
                            break;
                        case "05":

                            errMsg = "Not pass: OD 3 Up";
                            typeErr = "05";
                            break;

                        case "06":

                            errMsg = "Not pass: OD 1 within 6 month / Salary Less than 7,000 Baht";
                            typeErr = "06";
                            break;

                        case "07":

                            errMsg = "Not pass: request receive";
                            typeErr = "07";
                            break;

                        case "08":

                            errMsg = "Not pass: TCL = 0";
                            typeErr = "08";
                            break;

                        case "09":

                            errMsg = "Not pass: Card Hold";
                            typeErr = "09";
                            break;
                        case "10":

                            errMsg = "Not pass: Card Cancel";
                            typeErr = "10";
                            break;
                        case "11":

                            errMsg = "Not pass: Apply with out original salary slip";
                            typeErr = "11";
                            break;
                        case "12":

                            errMsg = "Not pass: Fraud/Legal";
                            typeErr = "12";
                            break;
                        case "13":

                            errMsg = "Not pass: Flag Umay+";
                            typeErr = "13";
                            break;

                        default:
                            errMsg = "Not pass";
                            typeErr = "";
                            break;

                    }

                    return false;
                }

            }
            else
            {
                errMsg = "Not pass.. ";
                typeErr = "";
                return false;
            }


        }
        catch (Exception ex)
        {
            errMsg = "Error , Please try again. ";
            typeErr = "";
            return false;
        }

    }


    //***  Calculate TCL & ACL ***//
    private bool calTCL(ref string err)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            DataSet ds = new DataSet();
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfo);
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfo);
            DataCenter dataCenter = new DataCenter(userInfo);
            ds = CallEntUsingCard.getTCLUS(lb_csn.Text.Trim()).Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds))
            {
                err = "ไม่พบข้อมูล TCL กรุณาตรวจสอบ";
                return false;
            }
            DataRow dr = ds.Tables[0]?.Rows[0];
            string[] Arr_appdate = txt_appDate.Text.Split('/');
            string appDate = Arr_appdate[2] + Arr_appdate[1] + Arr_appdate[0];
            string[] Arr_birthDate = txt_birthDate_C1.Text.Split('/');
            string birthDate = Arr_birthDate[2] + Arr_birthDate[1] + Arr_birthDate[0];
            string salary = float.Parse(txt_salary.Text.Trim(), NumberStyles.Currency).ToString();
            // ***  ref string GNSR87 ***//
            string POBOTL = "";
            string PONOAP = "";
            string POCSBL = "";
            string POCRAV = "";
            string POAPAM = "";
            string POEBCL = "";
            string POAVAP = "";
            string POSTS = "";
            string POOLDC = "";
            string POVENR = "";
            string POPERV = "";
            string POEBCS = "";
            string POERR = "";
            string PERMSG = "";

            ilObj.UserInfomation = userInfo;

            string mode = "";
            mode = "O";
            //try
            //{
            //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
            //}
            //catch
            //{
            //    mode = "O";
            //}

            bool res_GNSR87 = iLDataSubroutine.Call_GNSR87("IL", lb_csn.Text.Trim(), userInfo.BranchApp.ToString(), "0", appDate, birthDate,
                                                           "1", "N", salary, dr["m3crlm"].ToString(), "-9999999999", "000000000000", mode, "", ref POBOTL, ref PONOAP, ref POCSBL,
                                                           ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, ref POVENR, ref POPERV, ref POEBCS,
                                                           ref POERR, ref PERMSG, userInfo.BizInit, userInfo.BranchNo);


            if (!res_GNSR87 || POERR.Trim() == "Y")
            {
                err = PERMSG.Trim();

                return false;
            }
            else
            {
                txt_total_crt.Text = convCurrency(dr["m3crlm"].ToString());
                txt_ebcLimit.Text = convCurrency(POEBCL);
                txt_cust_bln.Text = convCurrency(POCSBL);
                txt_tcl.Text = convCurrency(POCRAV);
                txt_app_lm.Text = convCurrency(POAPAM);
                txt_bot_loan.Text = convCurrency(POBOTL);
                txt_bot_crA.Text = convCurrency((float.Parse(POBOTL) - float.Parse(POCSBL)).ToString());
                lb_pIncome.Text = convCurrency(salary);
                lb_pApproveL.Text = convCurrency(POAPAM);
                lb_pApproveA.Text = convCurrency(POAVAP);

                string Error_ = "";
                string payment_ability = "";
                bool res093 = iLDataSubroutine.Call_GNSR093(txt_idNo.Text.Trim(), hid_m00sal.Value, ref Error_, ref payment_ability, userInfo.BizInit, userInfo.BranchNo);
                //   ilObj.CloseConnectioDAL();
                if (res093 && Error_.Trim() != "Y")
                {
                    txt_pay_abl.Text = convCurrency(payment_ability);
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
            err = "Error";
            return false;
        }
    }

    // ***  check condition before judgment ***//
    private bool checkBeforeJudgment(ref string err)
    {
        try
        {
            //  check mobile number
            if (rb_mobile.SelectedItem.Value.ToString() == "Y")
            {
                if (txt_telM.Text.Trim() == "" || txt_telM_C.Text.Trim() == "")
                {

                    txt_telM.ValidationSettings.RequiredField.IsRequired = true;
                    txt_telM_C.ValidationSettings.RequiredField.IsRequired = true;
                    txt_telM.Focus();
                    err = "กรุณาระบุ เบอร์โทรศัพท์มือถือ";
                    return false;
                }
            }
            string birthDateC = txt_birthDate_C1.Text.Replace("/", "");
            string birthDate1 = txt_birthDate_P.Text.Trim().Substring(4, 4) + txt_birthDate_P.Text.Trim().Substring(2, 2) + txt_birthDate_P.Text.Trim().Substring(0, 2);
            string birthDate2 = birthDateC.Trim().Substring(4, 4) + birthDateC.Trim().Substring(2, 2) + birthDateC.Trim().Substring(0, 2);
            string realBirth = hid_birthDate.Value.Trim();
            //int child = int.Parse(txt_child.Text.Trim());
            ilObj = new ILDataCenter();
            //1.  check birth date ***//
            if (!ilObj.checkBirthDate(birthDate1, birthDate2, realBirth, ref err))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "script5", "txt_birthDate_P.SetValue('')", true);
                txt_birthDate_C1.Text = "";
                txt_birthDate_P.Focus();
                return false;
            }
            //2. check mobile ***//
            string mobile1 = txt_telM.Text.Trim();
            string mobile2 = txt_telM_C.Text.Trim();
            string realMobile = hid_mobile.Value.Trim();
            if (!ilObj.checkMobile(mobile1, mobile2, realMobile, ref err))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "script6", "txt_telM.SetValue('')", true);
                txt_telM_C.Text = "";
                txt_telM.Focus();
                return false;
            }

            if (dd_marital.Value == null)
            {
                err = "กรุณาระบุ 3. Marital Status";
                return false;
            }
            if (dd_resident.Value == null)
            {
                err = "กรุณาระบุ 4. Type of resident";
                return false;
            }
            if (txt_fPerson.Text.Trim() == "" || txt_fPerson.Text.Trim() == "0")
            {
                err = "กรุณาระบุ 5. Total of family";
                return false;
            }
            if (txt_yearResident.Text.Trim() == "")
            {
                err = "กรุณาระบุ 6. Period of time resident (Year) หากไม่ถึง 1 ปี กรุณาระบุช่อง ปีเป็น 0 ";
                return false;
            }
            if (txt_monthResident.Text.Trim() == "")
            {
                err = "กรุณาระบุ 6. Period of time resident (Month) หากไม่มีกรุณาระบุเป็น 0 ";
                return false;
            }
            if (txt_yearResident.Text.Trim() == "0" && txt_monthResident.Text.Trim() == "0")
            {
                err = "กรุณาระบุ 6. Period of time resident ";
                return false;
            }


            if (dd_busType.Value == null)
            {
                err = "กรุณาระบุ 7. Business Type  ";
                return false;
            }
            if (dd_occup.Value.ToString() == "null")
            {
                err = "กรุณาระบุ 8. Occupation ";
                return false;
            }
            if (dd_occup.Value.ToString() == "011" || dd_occup.Value.ToString() == "012")
            {
                if (dd_comerc.Value == null)
                {
                    err = "กรณีลูกค้าเป็นเจ้าของธุรกิจ จะต้องระบุว่านำทะเบียนการค้ามาหรือไม่";
                    return false;
                }
            }
            if (dd_position.Value == null)
            {
                err = "กรุณาระบุ 9. Position";
                return false;
            }
            if (txt_empNo.Text == "")
            {
                err = "กรุณาระบุ 10. Total of Employee";
                return false;
            }
            if (dd_empType.Value == null)
            {
                err = "กรุณาระบุ 11. Employee Type";
                return false;
            }
            if (txt_service_Y.Text.Trim() == "")
            {
                err = "กรุณาระบุ 12. Length of service (Year) หากไม่ถึง 1 ปี กรุณาระบุช่อง ปีเป็น 0 ";
                return false;
            }
            if (txt_service_M.Text.Trim() == "")
            {
                err = "กรุณาระบุ 12. Length of service (Month) หากไม่มี กรุณาระบุช่อง ปีเป็น 0 ";
                return false;
            }
            if (txt_service_Y.Text.Trim() == "0" && txt_service_M.Text.Trim() == "0")
            {
                err = "กรุณาระบุ 12. Length of service ";
                return false;
            }
            if (txt_salary.Text.Trim() == "" || txt_salary.Text.Trim() == "0.00")
            {
                err = "กรุณาระบุ 13. salary ";
                return false;
            }


        }
        catch (Exception ex)
        {
            err = "Err " + ex.Message.ToString();
            return false;
        }
        return true;
    }

    //***  check before save credit ***//
    private bool checkBeforeSave(ref string err)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            //bool resVer = verifyCustomer(txt_card_no.Text.Trim(),txt_idNo.Text.Trim(),ref err);
            //if(!resVer)
            //{
            //    return false;
            //}

            bool resJudg = checkBeforeJudgment(ref err);
            if (!resJudg)
            {
                return false;
            }
            bool resProd = checkBeforeCaculateProduct(ref err);
            if (!resProd)
            {
                return false;
            }

            if (dd_paymentType.SelectedItem.Value.ToString() == "1")
            {
                if (dd_bankCode.SelectedItem.Value.ToString() == "")
                {
                    err = "กรุณาระบุ bank code";
                    return false;
                }
                if (dd_bankBranch.SelectedItem.Value.ToString() == "")
                {
                    err = "กรุณาระบุ bank branch";
                    return false;
                }
                if (dd_accountType.SelectedItem.Value.ToString() == "")
                {
                    err = "กรุณาระบุ Account Type";
                    return false;
                }
                if (txt_AccountNo.Text.Trim() == "")
                {
                    err = " กรุณาระบุ เลขที่บัญชี";
                    return false;
                }
            }


            string AppDate = hid_date_97.Value.ToString();
            //ilObj.Call_ILSR97("01", "DMY", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            //ilObj.CloseConnectioDAL();

            //**  1. check Appdate and current date ***//
            string[] appDateS = txt_appDate.Text.Trim().Split('/');
            int curDate = int.Parse(AppDate.PadLeft(8, '0').Substring(4, 4) + AppDate.PadLeft(8, '0').Substring(2, 2) + AppDate.PadLeft(8, '0').Substring(0, 2));
            int appDateC = int.Parse(appDateS[2] + appDateS[1] + appDateS[0]);
            if (appDateC > curDate)
            {
                err = "Application date ต้องน้อยกว่าหรือเท่ากับวันปัจจุบัน";
                return false;
            }
            //** 2.  check reason ** //
            if (dd_reason.SelectedItem.Value.ToString().Trim() == "")
            {
                err = "Please input reason";
                return false;
            }
            //** 3. check item > 1  **//

            //** 4. check special criteria **//

            string[] product = dd_product.SelectedItem.Value.ToString().Split('|');

            ilObj = new ILDataCenter();
            //ilObj.UserInfomation = userInfo;
            //ilObj.checkApproveCriteria(product[0], vendor[0], userInfo.BranchApp,, appDate, appType, csn, idno, curDate.ToString(), "ILUSINGWEB", nothave_th);
            //ilObj.CloseConnectioDAL();

            //** 5. จำนวนเงินที่ต้องผ่อนต่อเดือนต้องไม่มากกว่า Payment Ability **//
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallMasterEnt.getCampaign(vendor[0], txt_totalterm.Text.Trim(), campaign[0], campaign[1], campaign[2], appDateS[2] + appDateS[1] + appDateS[0]);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_camp))
            {
                err = " cannot find campaign ";
                return false;
            }
            else
            {
                DataRow dr_campaign = ds_camp.Tables[0]?.Rows[0];
                int freeInt = int.Parse(dr_campaign["c01fin"].ToString().Trim()) + 1;
                try
                {
                    Label lb_install = (Label)gv_install.Rows[freeInt].Cells[3].FindControl("lb_Installment");
                    float installment = float.Parse(lb_install.Text.Trim(), NumberStyles.Currency);
                    float paymentABL = float.Parse(txt_pay_abl.Text.Trim(), NumberStyles.Currency);
                    if (installment > paymentABL)
                    {
                        err = "จำนวนเงินที่ต้องผ่อนต่อเดือนมากกว่า Payment Ability ";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    err = "cannot find free installment";
                    return false;
                }


                string AppDateBOT = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);
                //ilObj.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDateBOT);
                //ilObj.CloseConnectioDAL();
                DataSet ds_gnmx01 = CallMasterEnt.getGNMX01(AppDateBOT);
                CallMasterEnt._dataCenter.CloseConnectSQL();
                if (ilObj.check_dataset(ds_gnmx01))
                {
                    DataRow dr_gnmx = ds_gnmx01.Tables[0]?.Rows[0];
                    float max_int = float.Parse(dr_gnmx["mx1int"].ToString().Trim());
                    float max_cru = float.Parse(dr_gnmx["mx1max"].ToString().Trim());
                    float max_ifr = float.Parse(dr_gnmx["mx1inf"].ToString().Trim());

                    //** 6. ตรวจสอบ Interest  ว่าเกินกำหนดที่ BOT กำหนดหรือไม่   **//
                    //** 7. ตรวจสอบว่า Max rate เกินกว่าที่ BOT กำหนดหรือไม่      **//
                    //** 8.	ตรวจสอบว่า Initial fee เกินกว่าที่ BOT กำหนดไว้หรือไม่ **//
                    float max_intC = float.Parse(dr_campaign["c02inr"].ToString().Trim());
                    float max_cruC = float.Parse(dr_campaign["c02crr"].ToString().Trim()) + float.Parse(dr_campaign["c02inr"].ToString().Trim()) + float.Parse(dr_campaign["c02ifr"].ToString().Trim());
                    float max_ifrC = float.Parse(dr_campaign["c02ifr"].ToString().Trim());

                    if (max_intC > max_int)
                    {
                        err = "Interest เกินกว่าที่ BOT กำหนดไว้ ";
                        return false;
                    }
                    if (max_cruC > max_cru)
                    {
                        err = "Max rate เกินกว่าที่ BOT กำหนดไว้ ";
                        return false;
                    }
                    if (max_ifrC > max_ifr)
                    {
                        err = "Initial fee เกินกว่าที่ BOT กำหนดไว้ ";
                        return false;
                    }

                }

            }
            // check ค่างวด ต้องมากกว่า 400 บาท
            try
            {
                Label txt_install = (Label)gv_install.Rows[1].Cells[3].FindControl("lb_Installment");
                float inst = float.Parse(txt_install.Text.Trim(), NumberStyles.Currency);
                int lastperiod = gv_install.Rows.Count - 1;
                for (int i = 2; i < gv_install.Rows.Count; i++)
                {
                    Label txt_install_ = (Label)gv_install.Rows[i].Cells[3].FindControl("lb_Installment");
                    if (inst > float.Parse(txt_install_.Text.Trim(), NumberStyles.Currency))
                    {
                        if (i != lastperiod)
                        {
                            inst = float.Parse(txt_install_.Text.Trim(), NumberStyles.Currency);
                        }
                    }
                }

                if (inst < 400)
                {
                    err = "ไม่อนุญาติให้ทำการอนุมัติได้เนื่องจากค่างวดต่ำกว่า 400 บาท ";
                    return false;
                }
            }
            catch (Exception ex)
            {
                err = "ไม่สามารถค้นหาค่างวดได้ ";
                return false;
            }

            // *** 	ตรวจสอบข้อมูล Business  check cssr035 ***//
            string POERRC = "";
            string POERRM = "";

            ilObj.UserInfomation = userInfoService.GetUserInfo();
            bool resCSSR035 = iLDataSubroutine.Call_CSSR035(lb_csn.Text.Trim(), dd_occup.SelectedItem.Value.ToString(), "N", ref POERRC, ref POERRM, userInfo.BizInit, userInfo.BranchNo);
            // ilObj.CloseConnectioDAL();
            if (!resCSSR035 || POERRC != "000")
            {
                err = POERRM;
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            err = "error";
            return false;
        }
    }
    private bool checkBeforeCaculateProduct(ref string err)
    {
        try
        {
            if (txt_appDate.Text.Trim().Replace("/", "").Trim() == "")
            {
                err = "Please input Application date ";
                return false;
            }
            if (dd_vendor.SelectedItem.Value.ToString().Trim() == "")
            {
                err = "Please input Vendor ";
                return false;
            }
            if (txt_totalterm.Text.Trim() == "")
            {
                err = "Please input total Term ";
                return false;
            }
            if (dd_campaign.SelectedItem.Value.ToString().Trim() == "")
            {
                err = "Campaign type must have value ";
                return false;
            }
            if (txt_campgType.Text.Trim() == "")
            {
                err = "campaign type must have value ";
                return false;
            }
            if (txt_campSeq.Text.Trim() == "")
            {
                err = "Campaign seq must have value ";
                return false;
            }
            if (dd_product.SelectedItem.Value.ToString().Trim() == "")
            {
                err = "Please input product ";
                return false;
            }
            if (txt_pay_abl.Text.Trim() == "")
            {
                err = "Payment ability must have value ";
                return false;
            }
            if (txt_total_range.Text.Trim() == "")
            {
                err = "total range must have value ";
                return false;
            }
            if (txt_non.Text.Trim() == "")
            {
                err = "Non due must have value ";
                return false;
            }
            if (txt_price.Text.Trim() == "" || txt_price.Text.Trim() == "0.00")
            {
                err = "Please input price ";
                return false;
            }
            if (txt_qty.Text.Trim() == "" || txt_qty.Text.Trim() == "0")
            {
                err = "Please input quatity ";
                return false;
            }
            if (txt_down.Text.Trim() == "")
            {
                err = "If Down not have value ,Please input 0.00 ";
                return false;
            }


            return true;

        }
        catch (Exception ex)
        {
            err = "Please verify Vendor/Campaign/Product/Price ";
            return false;
        }
    }
    //***  calculate Installment ***//
    private bool calculateInstallment(ref string err, ref DataSet ds)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            //***  clear value ***//
            txt_purch.Text = "";
            txt_loanReq.Text = "";
            txt_fDue_AMT.Text = "";
            txt_fDue_date.Text = "";
            txt_duty.Text = "";
            txt_bureau.Text = "";
            txt_contractAmt.Text = "";
            gvTerm.DataSource = null;
            gvTerm.DataBind();
            gv_install.DataSource = null;
            gv_install.DataBind();


            //******************************//

            ilObj = new ILDataCenter();
            ilObj.UserInfomation = userInfoService.GetUserInfo();

            float loanRequest = 0;
            float approveAVL = float.Parse(lb_pApproveA.Text, NumberStyles.Currency);
            //*** check condition ***//
            if (!checkBeforeCaculateProduct(ref err))
            {
                return false;
            }
            float price = float.Parse(txt_price.Text.Trim(), NumberStyles.Currency);
            int qty = int.Parse(txt_qty.Text.Trim());
            float down = float.Parse(txt_down.Text.Trim(), NumberStyles.Currency);

            loanRequest = (price * qty) - down;
            txt_loanReq.Text = loanRequest.ToString("0.00");

            float minProc = txt_minPrice.Text.Trim() == "" ? 0 : float.Parse(txt_minPrice.Text.Trim());
            float maxProc = txt_maxPrice.Text.Trim() == "" ? 0 : float.Parse(txt_maxPrice.Text.Trim());
            float Total_pur = price * qty;
            float crAVL = lb_pApproveA.Text.Trim() == "" ? 0 : float.Parse(lb_pApproveA.Text.Trim(), NumberStyles.Currency);


            // ***  ref string GNSR87 ***//
            string POBOTL = "";
            string PONOAP = "";
            string POCSBL = "";
            string POCRAV = "";
            string POAPAM = "";
            string POEBCL = "";
            string POAVAP = "";
            string POSTS = "";
            string POOLDC = "";
            string POVENR = "";
            string POPERV = "";
            string POEBCS = "";
            string POERR = "";
            string PERMSG = "";

            string mode = "";
            //try
            //{
            //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
            //}
            //catch
            //{
            //    mode = "P";
            //}
            mode = "P";
            string salary = float.Parse(lb_pIncome.Text.Trim(), NumberStyles.Currency).ToString();
            string[] appDateUser = txt_appDate.Text.Trim().Split('/');
            string[] Arr_birthDate = txt_birthDate_C1.Text.Split('/');
            string birthDate = Arr_birthDate[2] + Arr_birthDate[1] + Arr_birthDate[0];
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            //txt_total_crt
            //txt_bot_crA.Text
            bool res_GNSR87 = iLDataSubroutine.Call_GNSR87("IL", lb_csn.Text.Trim(), userInfo.BranchApp.ToString(), "0", appDateUser[2] + appDateUser[1] + appDateUser[0], birthDate,
                                                  "1", "N", salary, float.Parse(txt_total_crt.Text.Trim(), NumberStyles.Currency).ToString(), loanRequest.ToString(), vendor[0], mode, "", ref POBOTL, ref PONOAP, ref POCSBL,
                                                  ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, ref POVENR, ref POPERV, ref POEBCS,
                                                  ref POERR, ref PERMSG, userInfo.BizInit, userInfo.BranchNo);


            if (!res_GNSR87 || POERR.Trim() == "Y")
            {
                err = PERMSG.Trim();

                return false;
            }

            DataSet ds_vendor = iLDataSubroutine.getVendor(vendor[0], userInfo.BranchApp);
            dataCenter.CloseConnectSQL();


            if (!ilObj.check_dataset(ds_vendor))
            {
                err = "Cannot find vendor ";
                return false;
            }
            DataRow dr_vendor = ds_vendor.Tables[0]?.Rows[0];
            if (!(dr_vendor["p10spc"].ToString().Trim() == "F" || dr_vendor["p10spc"].ToString().Trim() == "Y"))
            {
                txt_bot_crA.Text = convCurrency(POAVAP);
            }

            //**********************************************************//

            if (!((loanRequest > 0) && (loanRequest <= Total_pur)))
            {
                err = " Loan Request ต้องไม่มากกว่า Total Purchase ";
                return false;
            }

            if (loanRequest > crAVL)
            {
                err = " Loan request ต้องไม่มากกว่า Credit Available ";
                return false;
            }

            if (((loanRequest / qty) < minProc) && (minProc > 0))
            {
                string err_loan = "";


                if (txt_minPrice.Text.Trim() == "" || txt_minPrice.Text.Trim() == "0.00")
                {
                    err_loan = txt_minPrice.Text;
                }
                else
                {
                    err_loan = (float.Parse(txt_minPrice.Text) * float.Parse(txt_qty.Text)).ToString();
                }

                err = " loan request จะต้องไม่น้อยกว่า " + err_loan;
                //err = " loan request จะต้องไม่น้อยกว่า " + txt_minPrice.Text;
                return false;
            }
            if (((loanRequest / qty) > maxProc) && (maxProc > 0))
            {
                string err_loan = "";


                if (txt_maxPrice.Text.Trim() == "" || txt_maxPrice.Text.Trim() == "0.00")
                {
                    err_loan = txt_maxPrice.Text;
                }
                else
                {
                    err_loan = (float.Parse(txt_maxPrice.Text) * float.Parse(txt_qty.Text)).ToString();
                }
                err = " loan request จะต้องไม่มากกว่า " + err_loan;
                //err = " loan request จะต้องไม่มากกว่า " + txt_maxPrice.Text;
                return false;
            }

            string AppDate = hid_date_97.Value.ToString();
            //ilObj.Call_ILSR97("01", "DMY", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            //ilObj.CloseConnectioDAL();
            if (AppDate.Trim() == "")
            {
                err = " cannot find current date ";
                return false;
            }
            //**  check Appdate and current date ***//
            string[] appDateS = txt_appDate.Text.Trim().Split('/');
            int curDate = int.Parse(AppDate.PadLeft(8, '0').Substring(4, 4) + AppDate.PadLeft(8, '0').Substring(2, 2) + AppDate.PadLeft(8, '0').Substring(0, 2));
            int appDateC = int.Parse(appDateS[2] + appDateS[1] + appDateS[0]);
            if (appDateC > curDate)
            {
                err = "Application date ต้องน้อยกว่าหรือเท่ากับวันปัจจุบัน";
                return false;
            }

            DataSet ds_ilTB06 = iLDataSubroutine.getILTB06();
            if (!ilObj.check_dataset(ds_ilTB06))
            {
                err = " cannot find data in ILTB06 ";
                return false;
            }
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallMasterEnt.getCampaign(vendor[0], txt_totalterm.Text.Trim(), campaign[0], campaign[1], "", appDateS[2] + appDateS[1] + appDateS[0]);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_camp))
            {
                err = " cannot find campaign ";
                return false;
            }




            //***** compound string *****//


            DataRow dr_iltb06 = ds_ilTB06.Tables[0]?.Rows[0];
            DataRow dr_campaign = ds_camp.Tables[0]?.Rows[0];

            string compound = txt_loanReq.Text.Trim().Replace(".", "").PadLeft(13, '0') + " " +
                              appDateUser[0].PadLeft(2, '0') + appDateUser[1].PadLeft(2, '0') + appDateUser[2].PadLeft(4, '0') + " " +
                              txt_totalterm.Text.Trim().PadLeft(3, '0') + " " +
                              txt_total_range.Text.Trim().PadLeft(2, '0') + " " +
                              txt_non.Text.Trim().PadLeft(2, '0') + " " +
                              (float.Parse(dr_campaign["c01fin"].ToString().Trim()) + 1).ToString().Replace(".", "").PadLeft(2, '0') + " " +
                              dr_campaign["c02ifr"].ToString().Trim().Replace(".", "").PadLeft(5, '0') + " " +
                              dr_iltb06["t06lon"].ToString().Trim().Replace(".", "").PadLeft(4, '0') + " " +
                              dr_iltb06["t06Dut"].ToString().Trim().Replace(".", "").PadLeft(2, '0') + " " +
                              AppDate.PadLeft(8, '0') + " ";

            string PITEXT = compound;
            string PITERM = "";
            string PIINTR = "";
            string PICRUR = "";



            foreach (DataRow dr in ds_camp.Tables[0]?.Rows)
            {

                //float intrate = float.Parse(dr["c02inr"].ToString().Trim());
                //float crurate = float.Parse(dr["c02crr"].ToString().Trim());
                string intrate = dr["c02inr"].ToString().Trim();
                string crurate = dr["c02crr"].ToString().Trim();
                int c02tot = int.Parse(dr["c02tot"].ToString().Trim());
                int c02fmt = int.Parse(dr["c02fmt"].ToString().Trim());


                PITERM = PITERM + (1000 + (c02tot - c02fmt) + 1).ToString().Substring(1, 3);

                //if ((intrate > 0) && (dr["c02inr"].ToString().Trim().Length > 1))  
                //05/10/2558// if ((intrate > 0) && (float.Parse(dr["c02inr"].ToString()).ToString().Length > 1))
                if ((Convert.ToDouble(intrate) > 0) && (dr["c02inr"].ToString().Length > 1))
                {
                    PIINTR = PIINTR + (intrate.ToString().Replace(".", "")).PadLeft(5, '0');  //(1000 + intrate).ToString().Substring(1, 3) + (100.00 + intrate).ToString().Substring(4, 2);
                }
                //else if ((intrate < 0) && (dr["c02inr"].ToString().Trim().Length > 1))
                //05/10/2558//else if ((intrate < 0) && (float.Parse(dr["c02inr"].ToString()).ToString().Length > 1))
                else if ((Convert.ToDecimal(intrate) < 0) && (dr["c02inr"].ToString().Length > 1))
                {
                    PIINTR = "-" + (PIINTR + (1000 - Convert.ToDouble(intrate)).ToString().Substring(1, 3) + (100.00 - Convert.ToDouble(intrate)).ToString("##0.00").Substring(4, 2)).Substring(1, 4);
                    //05/10/2558//PIINTR = "-" + (PIINTR + (1000 - intrate).ToString().Substring(1, 3) + (100.00 - intrate).ToString("##0.00").Substring(4, 2)).Substring(1, 4);
                    //PIINTR = PIINTR + (intrate.ToString().Replace(".", "").PadLeft(4, '0'));  อันเดิม   //(PITERM + ((1000 - intrate).ToString().Substring(1, 3)) + ((100.00 - intrate).ToString().Substring(4, 2))).Substring(1,4); 

                }

                //else if ((intrate != 0) && (dr["c02inr"].ToString().Trim().Length == 1))
                //05/10/2558//else if ((intrate != 0) && (float.Parse(dr["c02inr"].ToString()).ToString().Length == 1))
                else if ((Convert.ToDouble(intrate) != 0) && (dr["c02inr"].ToString().Length == 1))
                {
                    PIINTR = PIINTR + (1000 + intrate).ToString().Substring(1, 3) + "00";
                }
                else
                {
                    PIINTR = PIINTR + "00000";
                }


                if ((Convert.ToDouble(crurate) != 0) && (dr["c02crr"].ToString().Trim().Length > 1))
                {
                    PICRUR = PICRUR + (1000 + Convert.ToDouble(crurate)).ToString("0.00").Substring(1, 3) + (100.00 + Convert.ToDouble(crurate)).ToString("0.00").Substring(4, 2);

                }
                else if ((Convert.ToDouble(crurate) != 0) && (Convert.ToDouble(crurate) == 1))
                {
                    PICRUR = PICRUR + (1000 + Convert.ToDouble(crurate)).ToString("0.00").Substring(2, 3);
                }
                else
                {
                    PICRUR = PICRUR + "00000";
                }
            }
            // out parameter 
            string POPCAM = "";
            string POINTR = "";
            string POCRUR = "";
            string POINFR = "";
            string PODIFR = ""; string POINST = ""; string POTOAM = ""; string PODUTY = "";
            string POINTB = ""; string POCRUB = ""; string POINFB = ""; string POCONA = "";
            string POFDAT = ""; string POAINR = ""; string POACRU = ""; string PODDAT = "";
            string POPPRN = ""; string POINSD = ""; string POINTD = ""; string POCRUD = "";
            string POINFD = ""; string POCDAT = ""; string POINCM = ""; string POREBT = "";
            string POCLSA = ""; string POFLAG = "";



            //bool res24 = ilObj.Call_ILSYD24D(PITEXT, PITERM, PIINTR, PICRUR,
            //                  ref  POPCAM, ref  POINTR, ref  POCRUR, ref  POINFR,
            //                  ref  PODIFR, ref  POINST, ref  POTOAM, ref  PODUTY,
            //                  ref  POINTB, ref  POCRUB, ref  POINFB, ref  POCONA,
            //                  ref  POFDAT, ref  POAINR, ref  POACRU, ref  PODDAT,
            //                  ref  POPPRN, ref  POINSD, ref  POINTD, ref  POCRUD,
            //                  ref  POINFD, ref  POCDAT, ref  POINCM, ref  POREBT,
            //                  ref  POCLSA, ref  POFLAG, userInfo.BizInit, userInfo.BranchNo);
            //ilObj.CloseConnectioDAL();
            bool res24 = false;
            bool oldcal = false;
            double _intrate = 0; double _crurate = 0;
            _intrate = double.TryParse(PIINTR, out _intrate) ? _intrate : 0;
            _crurate = double.TryParse(PICRUR, out _crurate) ? _crurate : 0;
            oldcal = (_intrate == 0 && _crurate == 0);
            //if (oldcal)
            //{
            //    res24 = iLDataSubroutine.Call_ILSYD24D(PITEXT, PITERM, PIINTR, PICRUR,
            //                       ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
            //                       ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
            //                       ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
            //                       ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
            //                       ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
            //                       ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
            //                       ref POCLSA, ref POFLAG, userInfo.BizInit, userInfo.BranchNo);
            //    //  ilObj.CloseConnectioDAL();
            //}
            //else
            //{
            res24 = iLDataSubroutine.Call_ILSREIR(PITEXT, PITERM, PIINTR, PICRUR,
                   ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                   ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                   ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                   ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                   ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                   ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                   ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
            //  ilObj.CloseConnectioDAL();
            //}


            if (res24)
            {

                //*** compute data into grid ***//
                int row = 1;
                DataTable dt = new DataTable("Term");
                DataTable dt_installment = new DataTable("Installment");
                DataTable dt_data = new DataTable("Data_use");


                string Bureau = "0.00";
                string BureauFirst = "0.00";// Modify By Azode: 20170601

                //*** check  ncb ***//

                if (AppDate.Trim() != "")
                {
                    //** NCBPJ REQ 70667 **   
                    //DataSet ds_ncb = ilObj.getNCB(AppDate.PadLeft(8, '0').Substring(4, 4) + AppDate.PadLeft(8, '0').Substring(2, 2) + AppDate.PadLeft(8, '0').Substring(0, 2));
                    //if (ilObj.check_dataset(ds_ncb))
                    //{
                    //    //DataRow dr_ncb = ds_ncb.Tables[0].Rows[ds_ncb.Tables[0].Rows.Count - 1];
                    //    // DataRow dr_ncb = ds_ncb.Tables[0].Rows[0];
                    //    //Bureau = dr_ncb["G00AMT"].ToString();

                    //    if (ds_ncb.Tables[0].Rows.Count > 1)
                    //    {
                    //        BureauFirst = ds_ncb.Tables[0].Rows[0]["G00AMT"].ToString();// Modify By Azode: 20170601
                    //        string cust_ncb = hd_ncb.Value.Trim().ToUpper();
                    //        foreach (DataRow dr in ds_ncb.Tables[0].Rows)
                    //        {
                    //            if (dr["G00FIL"].ToString().Substring(2, 1).Trim() != "")
                    //            {
                    //                if (cust_ncb == dr["G00FIL"].ToString().Substring(2, 1))
                    //                {
                    //                    Bureau = dr["G00AMT"].ToString();
                    //                    break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                Bureau = dr["G00AMT"].ToString();
                    //            }
                    //        }
                    //    }
                    //    else if (ds_ncb.Tables[0].Rows.Count == 1)
                    //    {
                    //        DataRow dr_ncb = ds_ncb.Tables[0].Rows[ds_ncb.Tables[0].Rows.Count - 1];
                    //        Bureau = dr_ncb["G00AMT"].ToString();
                    //        BureauFirst = dr_ncb["G00AMT"].ToString();// Modify By Azode: 20170601
                    //    }


                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }


                //************** table  data term *********************************//

                int total_range = int.Parse(txt_total_range.Text.Trim());
                dt.Columns.Add("from_T", typeof(string));
                dt.Columns.Add("to_T", typeof(string));
                dt.Columns.Add("int_p", typeof(string));
                dt.Columns.Add("int_amt", typeof(string));
                dt.Columns.Add("cru_p", typeof(string));
                dt.Columns.Add("cru_amt", typeof(string));
                dt.Columns.Add("int_free", typeof(string));
                dt.Columns.Add("total_amt", typeof(string));
                dt.Columns.Add("oth", typeof(string));


                dt.Columns.Add("PRINCIPAL", typeof(string));
                dt.Columns.Add("INSTALL", typeof(string));

                dt.Columns.Add("inteir", typeof(string));
                dt.Columns.Add("crueir", typeof(string));

                dt.Columns.Add("DUTY_STAMP", typeof(string));
                dt.Columns.Add("INTEREST_BASE", typeof(string));
                dt.Columns.Add("CR_USAGE_BASE", typeof(string));
                dt.Columns.Add("INIT_FEE_BASE", typeof(string));
                dt.Columns.Add("CONTRACT_AMOUNT", typeof(string));
                dt.Columns.Add("FIRST_DATE", typeof(string));
                dt.Columns.Add("AVG_INTEREST", typeof(string));
                dt.Columns.Add("AVG_CR_USAGE", typeof(string));
                dt.Columns.Add("CREDIT_BUREAU", typeof(string));

                //dt.Columns.Add("PODUTY", typeof(string));
                //dt.Columns.Add("POINTB", typeof(string));
                //dt.Columns.Add("POCRUB", typeof(string));
                //dt.Columns.Add("POINFB", typeof(string));
                //dt.Columns.Add("POCONA", typeof(string));
                //dt.Columns.Add("POFDAT", typeof(string));
                //dt.Columns.Add("POAINR", typeof(string));
                //dt.Columns.Add("POACRU", typeof(string));
                //dt.Columns.Add("PODDAT", typeof(string));

                int len_1 = 0;
                int len_3 = 0;
                int len_4 = 0;
                int len_5 = 0;
                int len_6 = 0;
                int len_7 = 0;
                int len_8 = 0;
                int len_9 = 0;
                int len_10 = 0;
                int len_11 = 0;
                int len_12 = 0;
                int nextTerm = 0;

                decimal installFirst = 0;
                decimal int_p_sum = 0;
                decimal int_amt_sum = 0; // cell 3
                decimal cru_p_sum = 0;  // cell 4
                decimal cru_amt_sum = 0; // cell 5
                decimal int_free_sum = 0; // cell 6
                decimal total_amt_sum = 0; // cell 7
                decimal oth_sum = 0; // cell 8
                decimal inteir_sum = 0;
                decimal crueir_sum = 0;
                int _term = int.Parse(txt_totalterm.Text.Trim());
                while (row <= total_range)
                {
                    string from_t = "0";  // cell 0
                    string to_t = "0";   // cell 1
                    string int_p = "0";  // cell 2
                    string int_amt = "0"; // cell 3
                    string cru_p = "0";  // cell 4
                    string cru_amt = "0"; // cell 5
                    string int_free = "0"; // cell 6
                    string total_amt = "0"; // cell 7
                    string oth = "0"; // cell 8
                    string principal = "0";
                    string install = "0";
                    string inteir = "0";
                    string crueir = "0";

                    from_t = (1 + nextTerm).ToString(); // 0
                    if (row == 1)
                    {
                        to_t = int.Parse(PITERM.Substring(0, 3)).ToString(); // 1
                        int_p = PIINTR.Substring(0, 1) == "-" ? "-" + PIINTR.Substring(1, 2) + "." + PIINTR.Substring(3, 2) : PIINTR.Substring(1, 2) + "." + PIINTR.Substring(3, 2);//2 // **  substring เพราะว่า แต่ละ Row มีค่าไม่เท่ากัน  
                        cru_p = PICRUR.Substring(0, 1) == "-" ? "-" + PICRUR.Substring(1, 2) + "." + PICRUR.Substring(3, 2) : PICRUR.Substring(1, 2) + "." + PICRUR.Substring(3, 2); //4

                    }
                    else if (row == 2)
                    {
                        to_t = (nextTerm + int.Parse(PITERM.Substring(3, 3))).ToString();
                        int_p = PIINTR.Substring(5, 1) == "-" ? "-" + PIINTR.Substring(6, 2) + "." + PIINTR.Substring(8, 2) : PIINTR.Substring(6, 2) + "." + PIINTR.Substring(8, 2); // **  substring เพราะว่า แต่ละ Row มีค่าไม่เท่ากัน  
                        cru_p = PICRUR.Substring(5, 1) == "-" ? "-" + PICRUR.Substring(6, 2) + "." + PICRUR.Substring(8, 2) : PICRUR.Substring(6, 2) + "." + PICRUR.Substring(8, 2);
                    }
                    else if (row == 3)
                    {
                        to_t = (nextTerm + int.Parse(PITERM.Substring(6, 3))).ToString();
                        int_p = PIINTR.Substring(10, 1) == "-" ? "-" + PIINTR.Substring(11, 2) + "." + PIINTR.Substring(13, 2) : PIINTR.Substring(11, 2) + "." + PIINTR.Substring(13, 2); // **  substring เพราะว่า แต่ละ Row มีค่าไม่เท่ากัน  
                        cru_p = PICRUR.Substring(10, 1) == "-" ? "-" + PICRUR.Substring(11, 2) + "." + PICRUR.Substring(13, 2) : PICRUR.Substring(11, 2) + "." + PICRUR.Substring(13, 2);
                    }
                    else if (row == 4)
                    {
                        to_t = (nextTerm + int.Parse(PITERM.Substring(9, 3))).ToString();
                        int_p = PIINTR.Substring(15, 1) == "-" ? "-" + PIINTR.Substring(16, 2) + "." + PIINTR.Substring(18, 2) : PIINTR.Substring(16, 2) + "." + PIINTR.Substring(18, 2); // **  substring เพราะว่า แต่ละ Row มีค่าไม่เท่ากัน  
                        cru_p = PICRUR.Substring(15, 1) == "-" ? "-" + PICRUR.Substring(16, 2) + "." + PICRUR.Substring(18, 2) : PICRUR.Substring(16, 2) + "." + PICRUR.Substring(18, 2);
                    }
                    else if (row == 5)
                    {
                        to_t = (nextTerm + int.Parse(PITERM.Substring(13, 3))).ToString();
                        int_p = PIINTR.Substring(20, 1) == "-" ? "-" + PIINTR.Substring(21, 2) + "." + PIINTR.Substring(23, 2) : PIINTR.Substring(21, 2) + "." + PIINTR.Substring(23, 2); // **  substring เพราะว่า แต่ละ Row มีค่าไม่เท่ากัน  
                        cru_p = PICRUR.Substring(20, 1) == "-" ? "-" + PICRUR.Substring(21, 2) + "." + PICRUR.Substring(23, 2) : PICRUR.Substring(21, 2) + "." + PICRUR.Substring(23, 2);
                    }

                    // v_5  //** cell 3 **//
                    int_amt = POINTR.Substring(len_3, 1) == "-" ? "-" + convCurrency(POINTR.Substring(len_3 + 1, 9) + "." + POINTR.Substring(len_3 + 10, 2))
                                                                : convCurrency(POINTR.Substring(len_3, 10) + "." + POINTR.Substring(len_3 + 10, 2));

                    // V_6  //** cell5 **//
                    cru_amt = POCRUR.Substring(len_5, 1) == "-" ? "-" + convCurrency(POCRUR.Substring(len_5 + 1, 9) + "." + POCRUR.Substring(len_5 + 10, 2))
                                                                : convCurrency(POCRUR.Substring(len_5, 10) + "." + POCRUR.Substring(len_5 + 10, 2));

                    //V_7  //** cell 6**//
                    int_free = POINFR.Substring(len_6, 1) == "-" ? "-" + convCurrency(POINFR.Substring(len_6 + 1, 9) + "." + POINFR.Substring(len_6 + 10, 2))
                                                                : convCurrency(POINFR.Substring(len_6, 10) + "." + POINFR.Substring(len_6 + 10, 2));

                    //V_8  //** cell 8 **//
                    oth = PODIFR.Substring(len_8, 1) == "-" ? "-" + convCurrency(PODIFR.Substring(len_8 + 1, 2) + "." + PODIFR.Substring(len_8 + 3, 2))
                                                                : convCurrency(PODIFR.Substring(len_8, 3) + "." + PODIFR.Substring(len_8 + 3, 2));

                    // V10  //** cell 7  **//
                    total_amt = convCurrency((decimal.Parse(POTOAM.Substring(len_7, 9) + "." + POTOAM.Substring(len_7 + 9, 2)) - decimal.Parse(oth)).ToString());

                    // Principal //**cell 9 **//
                    principal = convCurrency((decimal.Parse(POPCAM.Substring(len_9, 11) + "." + POPCAM.Substring(len_9 + 11, 2))).ToString());

                    //  Install // ** cell 10 **// 
                    install = convCurrency((decimal.Parse(POINST.Substring(len_10, 7)))+ "." + "00").ToString();

                    if (row == 1)
                    {
                        //installFirst = decimal.Parse(POINST.Substring(len_10, 7)) + decimal.Parse(PODUTY) + decimal.Parse(Bureau);
                        if (oldcal)
                        {
                            installFirst = ((decimal.Parse(POINSD.Substring(len_3, 7)) / 100) - decimal.Parse(BureauFirst)) + decimal.Parse(Bureau);
                        }
                        else
                        {
                            installFirst = ((decimal.Parse(POINSD.Substring(len_3, 7)) / 100) - decimal.Parse(BureauFirst)) + decimal.Parse(Bureau); // Modify By Azode: 20170601                       
                        }
                    }

                    //double tot_rate = (double.Parse(int_p) + double.Parse(cru_p)) / 100;
                    //double nint_rate = 0;
                    //double ncru_rate = 0;
                    //double ntot_rate = ilObj.EIR_ConvertRate(tot_rate, double.Parse(loanRequest.ToString()), _term);
                    //nint_rate = ilObj.EIR_ConvertRate((double.Parse(int_p) / 100), double.Parse(loanRequest.ToString()), _term);
                    //ncru_rate = ntot_rate - nint_rate;
                    inteir = int_amt.ToString();
                    crueir = cru_amt.ToString();
                    cru_amt = "0";
                    int_amt = "0";

                    dt.Rows.Add(from_t, to_t, int_p, int_amt, cru_p, cru_amt, int_free, total_amt, oth, principal, install, inteir, crueir);






                    //***  sum data ***//
                    int_p_sum += decimal.Parse(int_p);
                    int_amt_sum += decimal.Parse(int_amt);
                    cru_p_sum += decimal.Parse(cru_p);
                    cru_amt_sum += decimal.Parse(cru_amt);
                    int_free_sum += decimal.Parse(int_free);
                    total_amt_sum += decimal.Parse(total_amt);
                    oth_sum += decimal.Parse(oth);

                    inteir_sum += decimal.Parse(inteir);
                    crueir_sum += decimal.Parse(crueir);

                    nextTerm = int.Parse(to_t);
                    row += 1;
                    len_3 += 12;
                    len_5 += 12;
                    len_6 += 12;
                    len_7 += 11;
                    len_8 += 5;
                    len_9 += 13;
                    len_10 += 7;
                }
                dt.Rows.Add("", "", convCurrency(int_p_sum.ToString()), convCurrency(int_amt_sum.ToString()), convCurrency(cru_p_sum.ToString()),
                             convCurrency(cru_amt_sum.ToString()), convCurrency(int_free_sum.ToString()), convCurrency(total_amt_sum.ToString()),
                             convCurrency(oth_sum.ToString()), "", "", inteir_sum, crueir_sum,
                             PODUTY, POINTB, POCRUB, POINFB, POCONA, POFDAT.Substring(6, 2) + "/" + POFDAT.Substring(4, 2) + "/" + POFDAT.Substring(0, 4),
                             POAINR, POACRU, "0.00");
                hid_inteirsum.Value = inteir_sum.ToString();
                hid_crueirsum.Value = crueir_sum.ToString();
                //dt.Rows.Add("", "", convCurrency(int_p_sum.ToString()), convCurrency(int_amt_sum.ToString()), convCurrency(cru_p_sum.ToString()),
                //                convCurrency(cru_amt_sum.ToString()), convCurrency(int_free_sum.ToString()), convCurrency(total_amt_sum.ToString()),
                //                convCurrency(oth_sum.ToString()), PODUTY, POINTB, POCRUB, POINFB, POCONA, POFDAT, POAINR, POACRU, PODDAT);
                //if (row == 1)
                //{
                //    dt.Rows.Add("", "", convCurrency(int_p_sum.ToString()), convCurrency(int_amt_sum.ToString()), convCurrency(cru_p_sum.ToString()),
                //                convCurrency(cru_amt_sum.ToString()), convCurrency(int_free_sum.ToString()), convCurrency(total_amt_sum.ToString()),
                //                convCurrency(oth_sum.ToString()), PODUTY, POINTB, POCRUB, POINFB, POCONA, POFDAT, POAINR, POACRU, PODDAT);
                //}
                //else 
                //{
                //    dt.Rows.Add("", "", convCurrency(int_p_sum.ToString()), convCurrency(int_amt_sum.ToString()), convCurrency(cru_p_sum.ToString()),
                //                convCurrency(cru_amt_sum.ToString()), convCurrency(int_free_sum.ToString()), convCurrency(total_amt_sum.ToString()),
                //                convCurrency(oth_sum.ToString())
                //                , "","", "", "", "", "", "", "", ""
                //                );
                //}

                //******************** Installment *********************//
                //clear variable and declare field
                row = 1;
                len_1 = 0;
                len_3 = 0;
                len_4 = 0;
                len_5 = 0;
                len_6 = 0;
                len_7 = 0;
                len_8 = 0;
                len_9 = 0;
                len_10 = 0;
                len_11 = 0;
                len_12 = 0;
                nextTerm = 0;
                float term = PODDAT.Length / 8;

                dt_installment.Columns.Add("term", typeof(string));
                dt_installment.Columns.Add("begin", typeof(string));
                dt_installment.Columns.Add("princ", typeof(string));
                dt_installment.Columns.Add("intstallment", typeof(string));
                dt_installment.Columns.Add("interest", typeof(string));
                dt_installment.Columns.Add("cr_use", typeof(string));
                dt_installment.Columns.Add("income", typeof(string));
                dt_installment.Columns.Add("cur_princ", typeof(string));
                dt_installment.Columns.Add("amt", typeof(string));
                //**   **//
                decimal amt_next = 0;
                while (row <= term)
                {
                    string term_ = "";
                    string begin = "";
                    string princ = "";
                    string installment = "";
                    string interest = "";
                    string cr_use = "";
                    string income = "";
                    string cur_princ = "";
                    string amt = "";

                    term_ = row.ToString();
                    begin = (PODDAT.Substring(len_1, 8)).Substring(6, 2) + "/" + (PODDAT.Substring(len_1, 8)).Substring(4, 2) + "/" + (PODDAT.Substring(len_1, 8)).Substring(0, 4);
                    princ = row == 1 ? loanRequest.ToString() : amt_next.ToString();

                    if (row == 1)
                    {
                        installment = convCurrency(installFirst.ToString("0.0"));
                    }
                    else
                    {
                        
                        if (oldcal)
                        {
                            installment = convCurrency(((decimal.Parse(POINSD.Substring(len_3, 7))) / 100).ToString());
                            if (row == term)
                            {
                                //hid_lastinstallment.Value = ((decimal.Parse(POINSD.Substring(len_3, 7))) / 100).ToString();
                                hid_lastinstallment.Value = POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2);

                            }
                            else
                            {
                                //hid_installment.Value = ((decimal.Parse(POINSD.Substring(len_3, 7))) / 100).ToString());
                                hid_installment.Value = POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2);

                            }
                        }
                        else
                        {
                            installment = convCurrency(POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2));
                            if (row == term)
                            {
                                hid_lastinstallment.Value = POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2);
                            }
                            else
                            {
                                hid_installment.Value = POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2);
                            }
                        }

                    }


                    //princ_next = float.Parse(princ);
                    //installment = convCurrency(POINSD.Substring(len_3, 7));
                    interest = convCurrency(POINTD.Substring(len_4, 10) + "." + POINTD.Substring(len_4 + 10, 2));
                    cr_use = convCurrency(POCRUD.Substring(len_5, 10) + "." + POCRUD.Substring(len_5 + 10, 2));
                    income = convCurrency(POINFD.Substring(len_6, 10) + "." + POINFD.Substring(len_6 + 10, 2));
                    cur_princ = convCurrency(POPPRN.Substring(len_7, 9) + "." + POPPRN.Substring(len_7 + 9, 2));
                    amt = (decimal.Parse(princ) - decimal.Parse(cur_princ)).ToString();
                    amt_next = decimal.Parse(amt);

                    len_1 += 8;
                    len_3 += 7;
                    len_4 += 12;
                    len_5 += 12;
                    len_6 += 12;
                    len_7 += 11;
                    len_9 += 8;
                    len_10 += 12;
                    len_11 += 12;
                    len_12 += 11;
                    row += 1;
                    dt_installment.Rows.Add(term_, begin, convCurrency(princ), convCurrency(installment), convCurrency(interest), convCurrency(cr_use), convCurrency(income), convCurrency(cur_princ), convCurrency(amt));

                }
                //******************** Data for use *********************//
                dt_data.Columns.Add("Duty", typeof(string));
                dt_data.Columns.Add("Inst", typeof(string));
                dt_data.Columns.Add("FdueDate", typeof(string));
                dt_data.Columns.Add("ContAmt", typeof(string));
                dt_data.Columns.Add("FdueAmt", typeof(string));
                dt_data.Columns.Add("loanRequest", typeof(string));
                dt_data.Columns.Add("Bureau", typeof(string));
                dt_data.Columns.Add("Total_pur", typeof(string));

                string duty = convCurrency(PODUTY);
                string inst = convCurrency(POINST.Substring(0, 7));
                string fdueDate = dt_installment.Rows[0]["begin"].ToString();
                string contAmt = convCurrency(POCONA);
                string fdueAmt = dt_installment.Rows[0]["intstallment"].ToString();
                string loanRequest_ = loanRequest.ToString("0.00");  //  อย่าลืม  เอา total purche - down

                dt_data.Rows.Add(duty, inst, fdueDate, contAmt, fdueAmt, loanRequest_, Bureau, convCurrency(Total_pur.ToString()));
                ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt_installment);
                ds.Tables.Add(dt_data);
                return true;

            }
            else
            {
                err = "ไม่สามารถ คำนวณค่าได้.....";
                return false;
            }



        }
        catch (Exception ex)
        {
            err = "ไม่สามารถ คำนวณค่าได้";
            return false;
        }


    }
    private void getContNo()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblCreditBureau.Text = "";
            lblCreditReview.Text = "";

            string[] appDate = txt_appDate.Text.Trim().Split('/');
            string appDateF = appDate[2] + appDate[1] + appDate[0];
            string appNo = hid_App.Value.Trim();
            ilObj = new ILDataCenter();
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            //***  call ilsr02 ***//

            string err = "";
            string werr_02 = "";
            string woemsg = "";
            string cont = "";
            bool res_ilsr02 = iLDataSubroutine.Call_ILSR02(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref cont, ref werr_02, ref woemsg, userInfo.BizInit, userInfo.BranchNo);

            if (!res_ilsr02 || werr_02.Trim() == "Y")
            {
                err = woemsg;
                //  ilObj.RollbackDAL();
                //  ilObj.CloseConnectioDAL();
                lblMsgEN.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                return;
            }

            saveCredit(appNo, cont);
            hid_Confirm.Value = "";

            //string OAPP = "";
            //string OMSGA = "";
            //string OTEL = "";
            //string OMSGT = "";
            //string ONER = "";
            //string OMSG = "";

            //bool res_45 = iLDataSubroutine.Call_GNSR45(hid_m00ofc.Value, hid_m00oft.Value, hid_m11Tel.Value, "IL", userInfo.BranchApp, appNo, appDateF, ref OAPP, ref OMSGA, ref OTEL, ref OMSGT, ref ONER, ref OMSG, userInfo.BizInit, userInfo.BranchNo);
            ////  ilObj.CloseConnectioDAL();
            //// For test
            //res_45 = true;
            //if (!res_45)
            //{
            //    lblMsgEN.Text = err;
            //    hid_Confirm.Value = "";
            //    hid_cont.Value = "";
            //    hid_App.Value = "";
            //    PopupMsg.ShowOnPageLoad = true;

            //    return;
            //}
            //if (ONER == "N")
            //{
            //    lblConfirmMsgEN.Text = OMSG + "\r\n" + " Do you want to save?";
            //    hid_cont.Value = cont;
            //    hid_App.Value = appNo;
            //    PopupConfirmSave.ShowOnPageLoad = true;
            //    return;
            //}

            //if (ONER == "Y")
            //{
            //    lblConfirmMsgEN.Text = OMSGA + "\r\n" + OMSGT + "Do you want to save?";
            //    hid_cont.Value = cont;
            //    hid_App.Value = appNo;
            //    PopupConfirmSave.ShowOnPageLoad = true;
            //    return;

            //}
            //if (ONER == "")
            //{
            //    saveCredit(appNo, cont);
            //    hid_Confirm.Value = "";
            //    //hid_cont.Value = "";
            //    //hid_App.Value = "";
            //}
        }
        catch (Exception ex)
        {
            lblMsgEN.Text = ex.Message.ToString();
            PopupMsg.ShowOnPageLoad = true;
            hid_Confirm.Value = "";
            hid_cont.Value = "";
            hid_App.Value = "";
            return;
        }
    }
    private void getAppNo()
    {
        try
        {
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblCreditBureau.Text = "";
            lblCreditReview.Text = "";

            string appNo = "";
            string err = "";
            string[] appDate = txt_appDate.Text.Trim().Split('/');
            string appDateF = appDate[2] + appDate[1] + appDate[0];
            string fErr = "";
            ilObj = new ILDataCenter();
            userInfo = userInfoService.GetUserInfo();
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            bool res_ilsr01 = iLDataSubroutine.Call_ILSR01(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref appNo, ref fErr, ref err, userInfo.BizInit, userInfo.BranchNo);


            if (!res_ilsr01 || fErr == "Y")
            {
                //  ilObj.RollbackDAL();
                //  ilObj.CloseConnectioDAL();
                lblMsgEN.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                return;
            }

            //***  call ilsr02 ***//

            string werr_02 = "";
            string woemsg = "";
            string cont = "";
            bool res_ilsr02 = iLDataSubroutine.Call_ILSR02(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref cont, ref werr_02, ref woemsg, userInfo.BizInit, userInfo.BranchNo);

            if (!res_ilsr02 || werr_02.Trim() == "Y")
            {
                err = woemsg;
                //  ilObj.RollbackDAL();
                //  ilObj.CloseConnectioDAL();
                lblMsgEN.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                return;
            }

            string OAPP = "";
            string OMSGA = "";
            string OTEL = "";
            string OMSGT = "";
            string ONER = "";
            string OMSG = "";

            bool res_45 = iLDataSubroutine.Call_GNSR45(hid_m00ofc.Value, hid_m00oft.Value, hid_m11Tel.Value, "IL", userInfo.BranchApp, appNo, appDateF, ref OAPP, ref OMSGA, ref OTEL, ref OMSGT, ref ONER, ref OMSG, userInfo.BizInit, userInfo.BranchNo);
            // ilObj.CloseConnectioDAL();

            if (!res_45)
            {
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;

                return;
            }
            if (ONER == "N")
            {
                lblConfirmMsgEN.Text = OMSG + "\r\n" + " Do you want to save?";
                hid_cont.Value = cont;
                hid_App.Value = appNo;
                PopupConfirmSave.ShowOnPageLoad = true;
                return;
            }

            if (ONER == "Y")
            {
                lblConfirmMsgEN.Text = OMSGA + "\r\n" + OMSGT + "Do you want to save?";
                hid_cont.Value = cont;
                hid_App.Value = appNo;
                PopupConfirmSave.ShowOnPageLoad = true;
                return;

            }
            if (ONER == "")
            {
                saveCredit(appNo, cont);
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
            }





        }
        catch (Exception ex)
        {
            lblMsgEN.Text = ex.Message.ToString();
            PopupMsg.ShowOnPageLoad = true;
            hid_Confirm.Value = "";
            hid_cont.Value = "";
            hid_App.Value = "";
            return;
        }
    }

    private void saveCredit(string appNo, string cont)
    {
        bool success = true;
        string AutoSign = "";
        string sms_msg = "";
        string err = "";
        lblConfirmMsgEN.Text = "";
        lblConfirmMsgTH.Text = "";
        lblMsgEN.Text = "";
        lblMsgTH.Text = "";
        lblCreditBureau.Text = "";
        lblCreditReview.Text = "";
        try
        {
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            string[] appDate = txt_appDate.Text.Trim().Split('/');
            string appDateF = appDate[2] + appDate[1] + appDate[0];


            string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            string h00sst = "";
            string h00saj = "0";
            string h00cal = "";

            //DataSet ds_cus = CallEntUsingCard.getCustInfoByCard(txt_card_no.Text.Trim().Replace("-", "")).Result;
            //CallMasterEnt._dataCenter.CloseConnectSQL();
            //if (!ilObj.check_dataset(ds_cus))
            //{
            //    err = "cannot find customer";
            //    lblMsgEN.Text = err;
            //    hid_Confirm.Value = "";
            //    //hid_cont.Value = "";
            //    //hid_App.Value = "";
            //    PopupMsg.ShowOnPageLoad = true;

            //    return;
            //}
            string idcard = txt_idNo.Text;
            string birthDate = hid_birthDate.Value?.ToString().Trim();
            string name = hidName.Value?.ToString().Trim();
            string surName = hidSurname.Value?.ToString().Trim();
            string CSN = lb_csn.Text.ToString();
            string gender = hidGender.Value?.ToString().Trim();
            string mobile = hidMobile.Value?.ToString().Trim();
            string salaryType = hidSalary.Value?.ToString().Trim();
            string zipCode = hidZipcode.Value?.ToString().Trim();
            string Tambol = hidTambol.Value?.ToString().Trim();
            string amphur = hidAmphur.Value?.ToString().Trim();
            string province = hidProvince.Value?.ToString().Trim();
            string telext = hidExt.Value?.ToString().Trim();

            string typeErr = "";
            bool resVer = verifyCustomerN(txt_card_no.Text.Trim(), txt_idNo.Text.Trim(), lb_csn.Text.Trim(), ref err, ref typeErr);
            if (!resVer)
            {
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }

            ilObj.UserInfomation = userInfoService.GetUserInfo();
            DataSet ds_vendor = iLDataSubroutine.getVendor(vendor[0], userInfo.BranchApp);
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_vendor))
            {
                err = " cannot find vendor";
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }


            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallMasterEnt.getCampaign(vendor[0], txt_totalterm.Text.Trim(), campaign[0], campaign[1], "", appDateF);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_camp))
            {
                err = " cannot find campaign ";
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            DataSet ds_091l1 = CallEntUsingCard.getILTB09().Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_091l1))
            {
                err = " cannot find vat ";
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            //TO Dooo
            DataSet ds_CSMH00 = CallEntUsingCard.getcsmh00(lb_csn.Text).Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds_CSMH00))
            {
                DataRow dr_csmh = ds_CSMH00.Tables[0]?.Rows[0];
                h00sst = dr_csmh["h00sst"].ToString().Trim();
                h00saj = dr_csmh["h00saj"].ToString().Trim();
                h00cal = dr_csmh["h00cal"].ToString().Trim();

                //err = "cannot find customer data";
                //lblMsgEN.Text = err;
                //hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                //PopupMsg.ShowOnPageLoad = true;
                //return;
            }

            DataSet ds_ilTB06 = CallMasterEnt.getILTB06();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_ilTB06))
            {
                err = " cannot find data in ILTB06 ";
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }



            //DataRow dr_cust = ds_cus.Tables[0]?.Rows[0];
            DataRow dr_vendor = ds_vendor.Tables[0]?.Rows[0];
            DataRow dr_camp = ds_camp.Tables[0]?.Rows[0];
            DataRow dr_vat = ds_091l1.Tables[0]?.Rows[0];

            DataRow dr_iltb06 = ds_ilTB06.Tables[0]?.Rows[0];

            string AppDate97 = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);

            if (AppDate97 == "")
            {
                err = "cannot get date time";
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                //hid_cont.Value = "";
                //hid_App.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;

            }

            string TelType = "";
            string Zip = "";
            string O_TLDS = "";
            string Error = "";
            bool resTelHome = iLDataSubroutine.Call_GNSR16(lb_csn.Text.Trim(), mobile, telext, ref TelType, ref Zip, ref O_TLDS, ref Error, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

            float vatAmt = (float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency) * 7) / float.Parse(dr_vat["t09bas"].ToString().Trim());
            string[] firstDue = txt_fDue_date.Text.Trim().Split('/');

            string p1brn = userInfo.BranchApp.PadLeft(3, '0');
            string p1apno = appNo;
            string p1ltyp = "02";
            string p1appt = "02";
            string p1apvs = "1";
            string p1apdt = appDateF;
            string p1vdid = vendor[0];
            string p1mkid = dr_camp["c01mkc"].ToString().Trim();
            string p1camp = campaign[0];
            string p1cmsq = campaign[1];
            string p1item = product[0];
            string p1pdgp = product[3];//dr_cust["p1pdgp"].ToString().Trim();
            string p1pric = float.Parse(txt_price.Text.Trim(), NumberStyles.Currency).ToString();
            string p1qty = int.Parse(txt_qty.Text.Trim()).ToString();
            string p1purc = float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency).ToString();
            string p1vatr = dr_vat["t09rte"].ToString().Trim();
            string p1vata = (Math.Round((float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency) * 7) / float.Parse(dr_vat["t09bas"].ToString().Trim()), 2)).ToString();
            string p1down = float.Parse(txt_down.Text.Trim(), NumberStyles.Currency).ToString();
            string p1term = txt_totalterm.Text.Trim();
            string p1rang = txt_total_range.Text.Trim();
            string p1ndue = txt_non.Text.Trim();
            string p1lndr = dr_iltb06["T06LON"].ToString().Trim();//float.Parse(txt_loanReq.Text.Trim(), NumberStyles.Currency).ToString();
            string p1dutr = dr_iltb06["t06Dut"].ToString().Trim();//float.Parse(txt_duty.Text.Trim(), NumberStyles.Currency).ToString();
            string p1infr = dr_camp["c01fin"].ToString().Trim();//(int.Parse(dr_camp["c01fin"].ToString().Trim()) + 1).ToString();
            string p1intr = txt_Int.Text.Trim();
            string p1crur = txt_cru.Text.Trim();
            string p1pram = float.Parse(txt_loanReq.Text.Trim(), NumberStyles.Currency).ToString();

            // **  get grid 1  last index **//
            Label lb_install_total = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[3].FindControl("lb_intAMT");     // 3
            Label lb_cru_amt_total = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[5].FindControl("lb_cruAmonth");  // 5
            Label lb_InitFree_total = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[6].FindControl("lb_InitFree");  // 6
            Label lb_diff = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[5].FindControl("lb_OTH");  //8

            // **  get grid 2 last index **//
            int freeInt = int.Parse(dr_camp["c01fin"].ToString()) + 1;
            Label lb_intall_f = (Label)gv_install.Rows[freeInt - 1].Cells[1].FindControl("lb_begin");
            Label lb_p1fram = (Label)gv_install.Rows[freeInt - 1].Cells[3].FindControl("lb_Installment");

            string p1inta = float.Parse(lb_install_total.Text.Trim(), NumberStyles.Currency).ToString(); // 3
            string p1crua = float.Parse(lb_cru_amt_total.Text.Trim(), NumberStyles.Currency).ToString();  // 5
            string p1infa = float.Parse(lb_InitFree_total.Text.Trim(), NumberStyles.Currency).ToString();  // 6
            string p1duty = txt_duty.Text.Trim();
            string p1diff = float.Parse(lb_diff.Text.Trim(), NumberStyles.Currency).ToString();  //8
            string p1coam = float.Parse(txt_contractAmt.Text.Trim(), NumberStyles.Currency).ToString();
            string p1fdam = float.Parse(txt_fDue_AMT.Text.Trim(), NumberStyles.Currency).ToString();
            string p1frtm = freeInt.ToString();
            string p1frdt = lb_intall_f.Text.Substring(6, 4) + lb_intall_f.Text.Substring(3, 2) + lb_intall_f.Text.Substring(0, 2);      //"GrdIns2.cells[1,strtoint(FreeIns)],7,4"+"GrdIns2.cells[1,strtoint(FreeIns)],4,2"+"GrdIns2.cells[1,strtoint(FreeIns)],1,2)+";
            string p1fram = float.Parse(lb_p1fram.Text, NumberStyles.Currency).ToString(); //"StrAlgGrdIns2.cells[3,strtoint(FreeIns)]";
            string p1aprj = "AP";
            string p1stdt = AppDate97;//m_UpdDate.ToString();
            string p1sttm = m_UpdTime.ToString();
            string p1fdue = firstDue[2] + firstDue[1] + firstDue[0];
            string p1csno = lb_csn.Text.Trim();
            string p1loca = "250";
            string p1crcd = userInfo.Username.ToString();  //  user login 
            string P1RESN = dd_reason.SelectedItem.Value.ToString();
            string p1kusr = userInfo.Username.ToString();
            string p1kdte = AppDate97;
            string p1ktim = m_UpdTime.ToString();
            string p1avdt = AppDate97;//m_UpdDate.ToString();
            string p1avtm = m_UpdTime.ToString();
            string p1fill = "                  " + txt_rank_v.Text.Trim().PadRight(2, ' ') + hd_ncb.Value.Trim().PadRight(2,' ').ToUpper();
            string p1updt = m_UpdDate.ToString();
            string p1uptm = m_UpdTime.ToString();
            string p1upus = userInfo.Username.ToString();
            string p1prog = "WEBILUSING";
            string p1wsid = userInfo.LocalClient.ToString();
            string p1rsts = "";
            string P1AUTH = userInfo.Username.ToString();
            string p1cont = cont;
            string p1cndt = appDateF;
            string p1payt = dd_paymentType.SelectedItem.Value.ToString();
            string p1pano = txt_AccountNo.Text.Trim().Replace("-", "");
            string p1paty = dd_paymentType.SelectedItem.Value.ToString();
            string p1pbrn = "";
            string p1pbcd = "";

            if (dd_paymentType.SelectedItem.Value.ToString() == "1")
            {
                p1pbrn = dd_bankBranch.SelectedItem.Value.ToString();
                p1pbcd = dd_bankCode.SelectedItem.Value.ToString();
            }


            // *****************************************************//



            // ***  call CSSR06 **//
            string mode = "";
            //try
            //{
            //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
            //}
            //catch
            //{
            //    mode = "P";
            //}
            mode = "P";
            iDB2Command cmd = new iDB2Command();
            //if (dataCenter.SqlCon.State != ConnectionState.Open)
            //{
            //    dataCenter.SqlCon.Open();
            //    dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
            //}

            cmd.CommandText = CallEntUsingCard.INSERT_ILMS01(
                                        p1brn, p1apno, p1ltyp, p1appt, p1apvs,
                                        p1apdt, p1vdid, p1mkid, p1camp, p1cmsq,
                                        p1item, p1pdgp, p1pric, p1qty, p1purc,
                                        p1vatr, p1vata, p1down, p1term, p1rang,
                                        p1ndue, p1lndr, p1dutr, p1infr, p1intr,
                                        p1crur, p1pram, p1inta, p1crua, p1infa,
                                        p1duty, p1diff, p1coam, p1fdam, p1frtm,
                                        p1frdt, p1fram, p1aprj, p1stdt, p1sttm,
                                        p1fdue, p1csno, p1loca, p1crcd, P1RESN,
                                        p1kusr, p1kdte, p1ktim, p1avdt, p1avtm,
                                        p1fill, p1updt, p1uptm, p1upus, p1prog,
                                        p1wsid, p1rsts, P1AUTH, p1cont, p1cndt,
                                        p1payt, p1pbcd, p1pbrn, p1paty, p1pano);
            bool transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
            var res01 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (res01.afrows == -1)
            {
                success = false;
                Task.Run(() => { 
                    Utility.WriteLogString(res01.message.ToString(), cmd.CommandText.ToString()); 
                });
                //Utility.WriteLogString(res01.message.ToString(), cmd.CommandText.ToString());
                lblMsgEN.Text = "Save not complete ";
                hid_Confirm.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                goto commit_rollback;
            }


            //ย้ายมาจากด้านล่าง
            cmd.Parameters.Clear();
            string M13FIL = "";
            string commer = string.IsNullOrEmpty(dd_comerc.SelectedItem.Value.ToString().Trim()) ? "  " : dd_comerc.SelectedItem.Value.ToString().Trim();
            if (string.IsNullOrEmpty(hid_CRRW.Value.ToString().Trim()))
            {
                //string commer = string.IsNullOrEmpty(dd_comerc.SelectedItem.Value.ToString().Trim()) ? "  " : dd_comerc.SelectedItem.Value.ToString().Trim();
                M13FIL = ("E").PadLeft(19, ' ') + "  " + commer + "     " + hid_CRMD.Value.ToString();
            }
            else
            {
                //string commer = string.IsNullOrEmpty(dd_comerc.SelectedItem.Value.ToString().Trim()) ? "  " : dd_comerc.SelectedItem.Value.ToString().Trim();
                M13FIL = ("E").PadLeft(19, ' ') + "  " + commer + "    " + hid_CRRW.Value.ToString() + hid_CRMD.Value.ToString();
            }
            cmd.CommandText = CallEntUsingCard.INSERT_CSMS13UC(
                                            "IL",         //m13app,  
                                            lb_csn.Text,//m13csn,  
                                            p1brn,      //m13brn,  
                                            p1apno,     //m13apn,  
                                            p1appt,     //m13apt,  
                                            p1apvs,     //m13apv,  
                                            gender,         //m13sex,  
                                            dd_marital.Value.ToString().Trim(),         //m13mrt,  
                                            "",         //m13smt,  
                                            dd_busType.SelectedItem.Value.ToString().Trim(),   //m13but,  
                                            dd_occup.SelectedItem.Value.ToString().Trim(),     //m13occ, 
                                            dd_position.SelectedItem.Value.ToString().Trim(),  //m13pos,  
                                            (txt_empNo.Text.Trim()).PadLeft(5, '0'),                      //m13off, 
                                            p1lndr == "" ? "0" : p1lndr,                                     //m13lna,  
                                            dd_resident.SelectedItem.Value.ToString(),  //m13res,  
                                            txt_fPerson.Text.Trim() == "" ? "0" : txt_fPerson.Text.Trim(),                    //m13con,  
                                            TelType,                                         //m13ttl,  
                                            hid_m11Tel.Value.ToString().Trim(),                                         //m13htl,  
                                            telext,                                         //m13hex,   dr_cust["M11EXT"].ToString().Trim()
                                            txt_telM.Text,                              //m13mtl,  
                                            txt_service_Y.Text == "" ? "0" : txt_service_Y.Text.Trim(),//m13wky,  
                                            txt_service_M.Text == "" ? "0" : txt_service_M.Text.Trim(),//m13wkm,  
                                           salaryType,                                         //m13slt,  
                                            txt_salary.Text == "" ? "0" : txt_salary.Text.Trim(), //m13net,  
                                            p1camp,                                     //m13cmp,  
                                            p1cmsq == "" ? "0" : p1cmsq,                                     //m13csq,  
                                            p1term == "" ? "0" : p1term,                                      //m13trm,  
                                            float.Parse(txt_total_crt.Text, NumberStyles.Currency).ToString(),//m13tcl,  
                                            float.Parse(txt_tcl.Text.Trim(), NumberStyles.Currency).ToString(),//m13tca,  
                                            "0",//m13gol ,  
                                            hid_birthDate.Value.ToString(),//m13bdt,  
                                            "0",//m13chl ลูก,  
                                            zipCode == "" ? "0" : zipCode,//m13hzp,  
                                            Tambol == "" ? "0" : Tambol,//m13htm,  
                                            amphur == "" ? "0" : amphur,//m13ham
                                            province == "" ? "0" : province,//m13hpv,  
                                            txt_yearResident.Text.Trim() == "" ? "0" : txt_yearResident.Text.Trim(),   //m13lyr,  
                                            txt_monthResident.Text.Trim() == "" ? "0" : txt_monthResident.Text.Trim(), //m13lmt,  
                                            p1apdt,                        //m13fdt,  
                                            rb_mobile.SelectedItem.Value.ToString() == "Y" ? "1" : "",  //m13mob,  
                                            txt_rank.Text.Trim() == "" ? "0" : txt_rank.Text.Trim(),                     //m131id,  
                                            txt_group.Text.Trim() == "" ? "0" : txt_group.Text.Trim(),                    //m13gno,  
                                            "0",                                      //m13acl,  
                                            float.Parse(txt_bot_loan.Text.Trim(), NumberStyles.Currency).ToString(), //m13bot,  
                                            float.Parse(txt_cust_bln.Text.Trim(), NumberStyles.Currency).ToString(),  //m13cbl,  
                                            txt_rank.Text.Trim() == "" ? "0" : txt_rank.Text.Trim(),                     //M13_RK,  
                                            txt_group.Text.Trim() == "" ? "0" : txt_group.Text.Trim(),                 //M13_GN,  
                                            "0",                                                //m13_ac, 
                                            userInfo.Username.ToString(),                     //m13cru,  
                                            m_UpdDate.ToString(),                               //m13cud
                                            "0",                                                //M13SAD,  
                                            "0",                                                //M13SAT,  
                                            hid_date_sal_old.Value.ToString(),                  //M13OSL,  
                                            hid_date_sal_old_d.Value.ToString(),                //M13OSD,  
                                            hid_date_sal_old_t.Value.ToString(),                //M13OST,  
                                            dd_empType.SelectedItem.Value.ToString(),           //M13EMP,  
                                            M13FIL,                                     //M13FIL
                                            //("E").PadLeft(19, ' ') + "  " + dd_comerc.SelectedItem.Value.ToString().Trim(),              //M13FIL, 
                                            h00sst,                //m13sst,  
                                            h00saj,                //m13saj,  
                                            h00cal,                //m13cal,  
                                            "WEBILUSING",                                      //m13upg,  
                                            m_UpdDate.ToString(),                               //m13udt,  
                                            m_UpdTime.ToString(),                               //m13utm,  
                                            userInfo.Username.ToString(),                     //m13usr,  
                                            userInfo.LocalClient.ToString(),                  //m13wks,  
                                            userInfo.Username.ToString(),                     //m13aus,  
                                            m_UpdDate.ToString()                                //m13aud
                                            );
            // cmd.Parameters.Add("@m13hex", dr_cust["M11EXT"].ToString().Trim());
            transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
            var res_13 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (res_13.afrows == -1)
            {
                Task.Run(() => {
                    Utility.WriteLogString(res_13.message.ToString(), cmd.CommandText.ToString());
                });
                lblMsgEN.Text = "Save not complete ";
                hid_Confirm.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                success = false;
                goto commit_rollback;
            }
            CallEntUsingCard._dataCenter.CommitMssql();
            CallEntUsingCard._dataCenter.CloseConnectSQL();

            ////Update Credit Review & Credit Model 
            //cmd.Parameters.Clear();
            //cmd.CommandText = CallEntUsingCard.UPDATE_MS13CRRW_MD(
            //                            p1brn,      //m13brn,  
            //                            p1apno,     //m13apno,
            //                            hid_CRRW.Value.ToString(),
            //                            hid_CRMD.Value.ToString());
            ////BOW update csms13
            //transaction = CallEntUsingCard._dataCenter.Sqltr? == null ? true : false;
            //var res13RW = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            //if (res13RW.afrows == -1)
            //{
            //    Utility.WriteLogString(res13RW.message.ToString(), cmd.CommandText.ToString());
            //    lblMsgEN.Text = "Save not complete ";
            //    hid_Confirm.Value = "";
            //    PopupMsg.ShowOnPageLoad = true;
            //    success = false;
            //    goto commit_rollback;
            //}

            string WOERR = "";
            string WOERRM = "";
            bool res_call06 = iLDataSubroutine.Call_CSSR06C(CallHisun, "IL", txt_idNo.Text.Trim(), userInfo.BranchApp.ToString(),
                              appNo, cont, (float.Parse(txt_tcl.Text, NumberStyles.Currency) - float.Parse(txt_loanReq.Text, NumberStyles.Currency)).ToString(),
                              "", p1apdt, hid_birthDate.Value, "1", "N", (float.Parse(lb_pIncome.Text, NumberStyles.Currency)).ToString(),
                              (float.Parse(txt_total_crt.Text, NumberStyles.Currency)).ToString(),
                              (float.Parse(txt_loanReq.Text, NumberStyles.Currency)).ToString(),
                              p1vdid, mode, ref WOERR, ref WOERRM, userInfo.BizInit, userInfo.BranchNo);
            if (!res_call06 || WOERR == "Y")
            {
                err = "Save not complete " + WOERRM;
                DeleteILMS01andCSMS13(appNo, cont, lb_csn.Text);
                //CallEntUsingCard._dataCenter.RollbackMssql();
                //CallEntUsingCard._dataCenter.CloseConnectSQL();
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                success = false;
                PopupMsg.ShowOnPageLoad = true;
                return;
                //goto commit_rollback;
            }


            //***  insert ilms02  ***//
            // **  get grid 1  last index **//
            Label lb_p2ubas = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[12].FindControl("lb_INTEREST_BASE");
            Label lb_p2ucrb = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[12].FindControl("lb_CR_USAGE");
            Label lb_p2ufeb = (Label)gvTerm.Rows[gvTerm.Rows.Count - 1].Cells[14].FindControl("lb_INIT_FEE_BASE");


            cmd.Parameters.Clear();

            cmd.CommandText = CallEntUsingCard.INSERT_ILMS02(
                                p1brn,/*p2brn*/
                                cont,/*p2cont*/
                                p1ltyp,/*p2lnty*/
                                p1csno,/*p2csno*/
                                p1apno,/*p2apno*/
                                p1appt,/*p2appt*/
                                p1crcd,/*p2crcd*/
                                p1crcd,/*p2atcd*/
                                "250",    /*p2loca*/
                                p1vdid,   /*p2vdid*/
                                p1mkid,   /*p2mkid*/
                                p1camp,   /*p2camp*/
                                p1cmsq,   /*p2cmsq*/
                                txt_campgType.Text.Trim(),    /*p2cmct*/
                                p1item, /*p2item*/
                                p1pric, /*p2pric*/
                                p1qty,  /*p2qty*/
                                p1purc, /*p2purc*/
                                float.Parse(p1vatr).ToString("0"), /*p2vatr*/
                                Math.Round(float.Parse(p1vata), 1).ToString(), /*p2vata*/
                                p1down, /*p2down*/
                                p1term, /*p2term*/
                                p1rang, /*p2rang*/
                                p1ndue, /*p2ndue*/
                                "2", /*p2dte1*/
                                AppDate97, /*p2cndt*/
                                AppDate97 /*p2bkdt*/,
                                p1lndr /*p2lndr*/,
                                p1dutr /*p2dutr*/,
                                p1infr /*p2infr*/,
                                Math.Round(float.Parse(p1intr), 2).ToString() /*p2intr*/,
                                p1crur,//float.Parse(p1crur, NumberStyles.Currency).ToString("0") /*p2crur*/,
                                p1coam /*p2toam*/,
                                p1coam /*p2osam*/,
                                float.Parse(txt_loanReq.Text, NumberStyles.Currency).ToString() /*p2pcam*/,
                                float.Parse(txt_loanReq.Text, NumberStyles.Currency).ToString() /*p2pcbl*/,
                                p1fdue /*p21due*/,
                                p1fdam /*p2fdam*/,
                                Math.Round(float.Parse(p1diff), 2).ToString() /*p2diff*/,
                                Math.Round(float.Parse(p1diff), 2).ToString() /*p2difb*/,
                                p1frtm /*p2frtm*/,
                                p1frdt /*p2frdt*/,
                                Math.Round(float.Parse(p1fram), 2).ToString()/*p2fram*/,
                                float.Parse(p1duty).ToString("0") /*p2duty*/,
                                float.Parse(p1duty).ToString("0") /*p2dutb*/,
                                Math.Round(float.Parse(p1infa), 2).ToString() /*p2fee*/ ,
                                Math.Round(float.Parse(p1infa), 2).ToString() /*p2feeb*/,
                                float.Parse(lb_p2ufeb.Text).ToString("0") /*p2ufeb  cell 14 ,1   */,
                                p1infa /*p2feib*/,
                                p1crua /*p2crua*/,
                                p1crua /*p2crub*/,
                                lb_p2ucrb.Text /*p2ucrb  StrAlgGrdIns1.cells[13,1]  */,
                                p1crua /*p2ucib*/,
                                p1inta /*p2uida*/,
                                p1inta /*p2intb*/,
                                lb_p2ubas.Text /*p2ubas* StrAlgGrdIns1.cells[12,1]  */,
                                p1inta /*p2uidb*/,
                                P1RESN /*p2resn*/,
                                m_UpdDate.ToString() /*p2updt*/,
                                m_UpdTime.ToString() /*p2uptm*/,
                                "WEBILUSING" /*p2prog*/,
                                userInfo.Username.ToString() /*p2user*/,
                                userInfo.LocalClient.ToString() /*p2ddsp*/);
            transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
            var res_02 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["MockILMS02"]?.ToString()))
            {
                res_02.afrows = -1;
            }
            if (res_02.afrows == -1)
            {
                Task.Run(() =>
                {
                    Utility.WriteLogString(res_02.message.ToString(), cmd.CommandText.ToString());
                });
                lblMsgEN.Text = "Save not complete ";
                hid_Confirm.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                success = false;
                goto commit_rollback;
            }


            //Label txt_install = (Label)gvTerm.Rows[1].Cells[3].FindControl("lb_Installment");
            //float inst = float.Parse(txt_install.Text.Trim(), NumberStyles.Currency);
            for (int i = 0; i < gvTerm.Rows.Count - 1; i++)
            {
                int seq_d012sq = i + 1;
                Label lb_F = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_Fterm");
                Label lb_Tterm = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_Tterm");
                Label lb_intP = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_intP");
                Label lb_cru_p = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_cru_p");
                Label lb_PRINCIPAL = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_PRINCIPAL");
                Label lb_PODUTY = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_DUTY_STAMP");
                Label lb_intAMT = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_intAMT");
                Label lb_cruAmonth = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_cruAmonth");
                Label lb_InitFree = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_InitFree");
                Label lb_INSTALL = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_INSTALL");
                Label lb_OTH = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_OTH");

                cmd.Parameters.Clear();
                cmd.CommandText = CallEntUsingCard.INSERT_ILMD012(
                                    p1brn, //d012br,   
                                    p1apno,//d012ap,   
                                           //lb_F.Text == "" ? "0" : lb_F.Text,//d012sq,
                                    seq_d012sq.ToString(),//d012sq,
                                    "02",       //d012lt,
                                    cont, //  d012cnt 
                                    ((float.Parse(lb_Tterm.Text) - float.Parse(lb_F.Text)) + 1).ToString(),//floattostr((arr_term[i,2]-arr_term[i,1])+1       //d012tt  อย่าลืม,
                                    float.Parse(lb_F.Text).ToString(),//floattostr(arr_term[i,1])       //d012fm,                  อย่าลืม
                                    float.Parse(lb_Tterm.Text).ToString(),//floattostr(arr_term[i,2])       //d012to,                  อย่าลืม
                                    lb_intP.Text == "" ? "0" : lb_intP.Text,//StrAlgGrdIns1.cells[2,i]       //d012ir,
                                    lb_cru_p.Text == "" ? "0" : lb_cru_p.Text,//StrAlgGrdIns1.cells[4,i]       //d012cr,
                                    lb_InitFree.Text == "" ? "0" : lb_InitFree.Text,//RzDBGrdIns.Fields[5].asstring       //d012fr,              อย่าลืม  มันคืออะไร
                                    lb_PRINCIPAL.Text == "" ? "0" : float.Parse(lb_PRINCIPAL.Text, NumberStyles.Currency).ToString(),//StrAlgGrdIns1.cells[9,i]       //d012pa,        ? งง
                                    lb_intAMT.Text == "" ? "0" : float.Parse(lb_intAMT.Text, NumberStyles.Currency).ToString(),//StrAlgGrdIns1.cells[3,i]       //d012ia,
                                    lb_cruAmonth.Text == "" ? "0" : float.Parse(lb_cruAmonth.Text, NumberStyles.Currency).ToString(),//StrAlgGrdIns1.cells[5,i]       //d012ca,     
                                    lb_InitFree.Text == "" ? "0" : float.Parse(lb_InitFree.Text, NumberStyles.Currency).ToString(),//StrAlgGrdIns1.cells[6,i]       //d012fa,
                                    lb_INSTALL.Text == "" ? "0" : float.Parse(lb_INSTALL.Text, NumberStyles.Currency).ToString(),//StrAlgGrdIns1.cells[10,i]       //d012in,
                                    lb_OTH.Text == "" ? "0" : lb_OTH.Text,//StrAlgGrdIns1.cells[8,i]       //d012df,
                                    m_UpdDate.ToString(),       //d012ud,
                                    m_UpdTime.ToString(),       //d012ut,
                                    userInfo.Username.ToString(),       //d012us,
                                    "WEBILUSING",       //d012pg,
                                    userInfo.LocalClient.ToString()       //d012ws
                                    );
                transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
                var res_012 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (res_012.afrows == -1)
                {
                    Task.Run(() =>
                    {
                        Utility.WriteLogString(res_012.message.ToString(), cmd.CommandText.ToString());
                    });
                    lblMsgEN.Text = "Save not complete ";
                    hid_Confirm.Value = "";
                    PopupMsg.ShowOnPageLoad = true;
                    success = false;
                    goto commit_rollback;
                }


            }

            double nint_rate = double.TryParse(hid_inteirsum.Value.ToString(), out nint_rate) ? Math.Round(nint_rate / 12, 4) : 0;
            double ncru_rate = double.TryParse(hid_crueirsum.Value.ToString(), out ncru_rate) ? Math.Round(ncru_rate / 12, 4) : 0;
            cmd.Parameters.Clear();
            cmd.CommandText = CallEntUsingCard.INSERT_IlMS23(p1csno, cont.ToString(), nint_rate.ToString(), ncru_rate.ToString(),
                                                   hid_inteirsum.Value.ToString(), hid_crueirsum.Value.ToString(), "0", "0", "0", "0",
                                                    m_UpdDate.ToString(), m_UpdTime.ToString(), userInfo.Username.ToString(), userInfo.LocalClient.ToString(),
                                                    "KESSAI", "A", p1term, p1fdam, "2", (int.Parse(p1term) - 1).ToString(), hid_installment.Value, hid_lastinstallment.Value);
            transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
            var res_023 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (res_023.afrows == -1)
            {
                Task.Run(() =>
                {
                    Utility.WriteLogString(res_023.message.ToString(), cmd.CommandText.ToString());
                });
                lblMsgEN.Text = "Save not complete ";
                hid_Confirm.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                success = false;
                goto commit_rollback;
            }
        commit_rollback:
            if (success)
            {
                CallEntUsingCard._dataCenter.CommitMssql();
                CallEntUsingCard._dataCenter.CloseConnectSQL();

            }
            else
            {
                CallEntUsingCard._dataCenter.RollbackMssql();
                CallEntUsingCard._dataCenter.CloseConnectSQL();
                UpdateFlagILMS01(appNo, cont, lb_csn.Text);
                return;
            }


            //*** check special criteria ***//
            string t22cod = "";
            string t22seq = "";
            string sqlRet = "";
            if (!CallEntUsingCard.checkApproveCriteria(ilObj, product[0].ToString(), vendor[0].ToString(), userInfo.BranchApp, appNo, appDateF, "02", lb_csn.Text, idcard,
                                           AppDate97, "ILUSINGWEB", false, userInfo.BizInit, userInfo.BranchNo, userInfo.Username, userInfo.LocalClient, ref t22cod, ref t22seq, ref sqlRet))
            {




                //if (!ilObj.checkApproveCriteria(ilObj, product[0], vendor[0], userInfo.BranchApp, appNo, appDateF, "02", lb_csn.Text, dr_cust["IDCard"].ToString(),
                //                              AppDate97, "ILUSINGWEB", false, userInfo.BizInit, userInfo.BranchNo, userInfo.Username, userInfo.LocalClient, ref t22cod, ref t22seq, ref sqlRet))
                //{

                //    ilObj.RollbackDAL();

                // ilObj.RollbackDAL();
                if (sqlRet != "")
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sqlRet;
                    transaction = CallEntUsingCard._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res_iltb23 = CallEntUsingCard._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;

                    if (res_iltb23.afrows == 1)
                    {
                        success = true;
                        if (success)
                        {
                            CallEntUsingCard._dataCenter.CommitMssql();
                            CallEntUsingCard._dataCenter.CloseConnectSQL();

                        }
                        else
                        {
                            CallEntUsingCard._dataCenter.RollbackMssql();
                            CallEntUsingCard._dataCenter.CloseConnectSQL();
                            return;
                        }
                    }
                }
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                lblMsgEN.Text = " Can not Approve this customer. Check by condition code : " + t22cod + " Seq: " + t22seq;
                PopupMsg.ShowOnPageLoad = true;
                fn_Clear();
                txt_card_no.Text = "";
                return;
            }



            // **  Clear NMP #70972
            string Err_CSSR67 = "";
            bool call_CSSR67 = iLDataSubroutine.Call_CSSR67(lb_csn.Text.Trim(), "ILAPPLY", ref Err_CSSR67, userInfo.BizInit, userInfo.BranchNo);
            if (call_CSSR67 == false || Err_CSSR67.Trim() != "")
            {
                err += "Error Call CSSR67 : " + Err_CSSR67.ToString();

                //  Utility.WriteLogString(res_023.message.ToString(), cmd.CommandText.ToString());
                lblMsgEN.Text = err;
                hid_Confirm.Value = "";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }

            //ilObj.CommitDAL();
            //ilObj.CloseConnectioDAL();

            string dateForILMS86 = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);
            DataSet ds_autoLayBill = CallEntUsingCard.getILMS10(p1vdid).Result;
            if (!ilObj.check_dataset(ds_autoLayBill))
            {
                //ilObj.RollbackDAL();
                CallEntUsingCard._dataCenter.CloseConnectSQL();

            }

            if (ilObj.check_dataset(ds_autoLayBill))
            {
                //***  CALL ILSR73 ***//
                string FLGERR1 = "";
                string FLGMSG1 = "";
                bool res_ilsr73 = false;
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IS250"].ToString()))
                {
                    res_ilsr73 = true;
                }
                else
                {
                    res_ilsr73 = iLDataSubroutine.Call_ILSR73(cont, dateForILMS86, ref FLGERR1, FLGMSG1, userInfo.BizInit, userInfo.BranchNo);
                }
                if (!res_ilsr73 || FLGERR1 == "Y")
                {
                    AutoSign = "ไม่สามารถ Auto Sign Lay-Bill ";
                }
                else
                {
                    AutoSign = "Auto Sign Lay-Bill completed. \r\n  ";
                }
            }

            //ilObj.CommitDAL();

            // ilObj.CloseConnectioDAL();

            if (txt_telM.Text.Trim() != "")
            {
                ILSRSMS iLSRSMS = new ILSRSMS();
                string poerrc = "";
                string poerrm = "";
                bool sms = iLSRSMS.Call_ILSRSMS(CallEntUsingCard._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                              appNo.PadLeft(11, '0'), txt_telM.Text.Trim(), ref poerrc, ref poerrm);
                //ilObj.CloseConnectioDAL();
                if (!sms || poerrc == "Y")
                {
                    sms_msg = " Cannot send SMS. " + poerrm;

                }
                else if (sms && poerrc.Trim() == "")
                {
                    sms_msg = poerrm;
                }
            }
            else
            {
                sms_msg = " Cannot send SMS. ";
            }

            CallEntUsingCard._dataCenter.CommitMssql();
            CallEntUsingCard._dataCenter.CloseConnectSQL();

            AutoSign += " Save success  " + "\r\n" +
                        // +  "[contract No.: " + cont + "]" + "\r\n" +
                        //    "[App No.:" + appNo + "] " + "\r\n" +
                        sms_msg;

            lblMsgEN.Text = AutoSign;
            lblContract.Text = "[contract No.: " + cont + "]";
            lblAppNo.Text = "[App No.:" + appNo + "] ";
            fn_Clear();
            txt_card_no.Text = "";
            PopupMsg.ShowOnPageLoad = true;
            return;

        }
        catch (Exception ex)
        {
            UpdateFlagILMS01(appNo, cont, lb_csn.Text);
            CallEntUsingCard._dataCenter.RollbackMssql();
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            err = "save not complete " + ex;
            return;
        }
    }

    private void DeleteILMS01andCSMS13(string appNo, string cont, string csn)
    {
        string sql = $@"DELETE FROM [AS400DB01].[ILOD0001].[ILMS01]
                        WHERE P1APNO = {appNo} AND P1CONT = {cont} AND P1CSNO = {csn} 
                        DELETE FROM [AS400DB01].[CSOD0001].[CSMS13]
                        WHERE M13APN = {appNo} AND M13CSN = {csn}";
        bool transaction = CallMasterEnt._dataCenter.Sqltr?.Connection == null ? true : false;
        var res_del = CallMasterEnt._dataCenter.Execute(sql, CommandType.Text, transaction).Result;
        if (res_del.success)
        {
            CallMasterEnt._dataCenter.CommitMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        else
        {
            CallMasterEnt._dataCenter.RollbackMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        return;
    }

    private void UpdateFlagILMS01(string appNo, string cont, string csn)
    {
        string sql = $@"UPDATE AS400DB01.ILOD0001.ILMS01
			            SET P1APRJ = 'CN',
				        P1RESN = 'SL4',
				        P1LOCA = 150,
				        P1RSTS = 'x'
                        WHERE P1APNO = {appNo} AND P1CONT = {cont} AND P1CSNO = {csn}
                        AND NOT EXISTS (
                              SELECT 1
                              FROM AS400DB01.ILOD0001.ILMS02
                              WHERE P2APNO = {appNo}
                                AND P2CONT = {cont}
                                AND P2CSNO = {csn} )";
        bool transaction = CallMasterEnt._dataCenter.Sqltr?.Connection == null ? true : false;
        var res_del = CallMasterEnt._dataCenter.Execute(sql, CommandType.Text, transaction).Result;
        if (res_del.success)
        {
            CallMasterEnt._dataCenter.CommitMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        else
        {
            CallMasterEnt._dataCenter.RollbackMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
        }
        return;
    }

    // function key-in //
    private void fn_key_in()
    {
        txt_totalterm.Enabled = true;
        dd_vendor.Enabled = true;
        dd_campaign.Enabled = true;
        dd_product.Enabled = true;
        txt_price.Enabled = true;
        txt_qty.Enabled = true;
        txt_down.Enabled = true;
        btn_saveCr.Enabled = false;
        btn_cal_TCL.Enabled = true;
        btn_keyin.Enabled = false;
        btn_vendorScr.Enabled = true;
    }
    private void disable_key_in()
    {
        txt_totalterm.Enabled = false;
        dd_vendor.Enabled = false;
        dd_campaign.Enabled = false;
        dd_product.Enabled = false;
        txt_price.Enabled = false;
        txt_qty.Enabled = false;
        txt_down.Enabled = false;
        btn_keyin.Enabled = true;
        btn_vendorScr.Enabled = false;

    }
    // function clear //

    private void fn_Clear()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            //***  Clear all item in judgment page.
            pl_cust.Enabled = false;
            tabDetail.TabPages.FindByName("TabTCL").ClientEnabled = false;
            txt_card_no.Focus();

            //tabDetail.ActiveTabPage.Index = 0;
            //tabDetail.TabPages[1].Enabled = false;

            ilObj = new ILDataCenter();

            ScriptManager.RegisterStartupScript(this, typeof(string), "script3", "txt_birthDate_P.SetValue('')", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "script4", "txt_telM.SetValue('')", true);

            btn_saveCr.Enabled = false;
            btn_keyin.Enabled = false;

            hid_Confirm.Value = "";
            hid_password.Value = "";

            txt_birthDate_P.Focus();
            lb_rescard.Text = "";
            lb_csn.Text = "";
            lb_nameCust.Text = "";

            txt_idNo.Text = "";
            txt_idNo.Enabled = false;

            txt_appDate.Text = ""; // ILR97

            txt_expireDate.Text = "";
            txt_expireDate.Enabled = false;

            //lb_blacklist.Text = "";
            txt_birthDate_P.Text = "";
            txt_birthDate_C1.Text = "";

            dd_marital.Value = "";
            dd_marital.Enabled = false;

            //dd_subMarital.Value = "";
            //dd_subMarital.Enabled = false;
            //txt_child.Text = "";

            //rb_sex.Value = "";
            //rb_sex.Enabled = false;

            dd_resident.Value = "";
            dd_resident.Enabled = false;

            txt_fPerson.Text = "";
            txt_fPerson.Enabled = false;

            txt_yearResident.Text = "";
            txt_yearResident.Enabled = false;

            txt_monthResident.Text = "";
            txt_monthResident.Enabled = false;

            dd_busType.Value = "";
            dd_busType.Enabled = false;

            dd_occup.Value = "";
            dd_occup.Enabled = false;

            dd_comerc.Items.Clear();
            dd_comerc.Value = "";
            dd_comerc.Enabled = false;

            dd_position.Value = "";
            dd_position.Enabled = false;

            txt_empNo.Text = "";
            txt_empNo.Enabled = false;

            dd_empType.Value = "";
            dd_empType.Enabled = false;

            txt_service_Y.Text = "";
            txt_service_Y.Enabled = false;

            txt_service_M.Text = "";
            txt_service_M.Enabled = false;

            //dd_incomeType.Value = "";
            //dd_incomeType.Enabled = false;

            txt_salary.Text = "";
            txt_salary.Enabled = false;

            //txt_sal_date.Text = "";
            //txt_sal_date.Enabled = false;

            //txt_sal_time.Text = "";
            //txt_sal_time.Enabled = false;

            //dd_telH.Value = "";
            //dd_telH.Enabled = false;

            //txt_telH.Text = "";
            //txt_telH.Enabled = false;

            //txt_tel2.Text = "";
            //txt_tel2.Enabled = false;

            //txt_telExt.Text = "";
            //txt_telExt.Enabled = false;

            rb_mobile.Value = "";
            txt_telM.Text = "";
            txt_telM_C.Text = "";
            txt_telM.Enabled = true;
            txt_telM_C.Enabled = true;
            txt_telM.ValidationSettings.RequiredField.IsRequired = false;
            txt_telM_C.ValidationSettings.RequiredField.IsRequired = false;
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfo);
            string AppDate = "";
            ilObj.UserInfomation = userInfo;
            iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            //ilObj.CloseConnectioDAL();
            if (AppDate.Trim() != "")
            {
                AppDate = AppDate.PadLeft(8, '0');
                AppDate = AppDate.Substring(6, 2) + AppDate.Substring(4, 2) + AppDate.Substring(0, 4);
                txt_appDate.Text = AppDate;
                hid_date_97.Value = AppDate;
            }

            hid_birthDate.Value = "";
            hid_mobile.Value = "";
            lb_busType.Text = "";
            lb_empType.Text = "";
            lb_marital.Text = "";
            lb_occup.Text = "";
            lb_position.Text = "";
            lb_resident.Text = "";



            //***  Clear all item in product page.
            txt_ebcLimit.Text = "";
            txt_total_crt.Text = "";
            txt_cust_bln.Text = "";
            txt_tcl.Text = "";
            txt_app_lm.Text = "";
            txt_bot_loan.Text = "";
            txt_bot_crA.Text = "";
            txt_group.Text = "";
            txt_rank.Text = "";

            dd_paymentType.SelectedIndex = -1;
            dd_bankCode.SelectedIndex = -1;
            dd_accountType.SelectedIndex = -1;
            dd_bankBranch.SelectedIndex = -1;
            txt_AccountNo.Text = "";
            lb_pApproveA.Text = "";
            lb_pApproveL.Text = "";
            lb_pIncome.Text = "";
            lb_resNCB.Text = "";
            txt_Int.Text = "";
            txt_totalterm.Text = "";
            dd_vendor.SelectedIndex = -1;
            dd_campaign.SelectedIndex = -1;
            dd_product.SelectedIndex = -1;
            fn_key_in();
            txt_campSeq.Text = "";
            txt_pay_abl.Text = "";
            txt_total_range.Text = "";
            txt_non.Text = "";
            txt_price.Text = "0.00";
            txt_qty.Text = "0.00";
            txt_minPrice.Text = "";
            txt_maxPrice.Text = "";
            txt_down.Text = "0.00";
            txt_fDue_AMT.Text = "";
            txt_fDue_date.Text = "";
            txt_duty.Text = "";
            txt_purch.Text = "";
            txt_loanReq.Text = "";
            txt_cru.Text = "";
            txt_bureau.Text = "";
            txt_contractAmt.Text = "";
            txt_rank_v.Text = "";
            gv_install.DataSource = null;
            gv_install.DataBind();
            gvTerm.DataSource = null;
            gvTerm.DataBind();

            pl_payment.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    private string convCurrency(string currency)
    {
        try
        {
            string cur = string.Format("{0:n}", decimal.Parse(currency));
            return cur;
        }
        catch (Exception ex)
        {

            return "0.00";
        }
    }
    #endregion



    protected void btn_saveCr_Click(object sender, EventArgs e)
    {
        try
        {
            hid_Confirm.Value = "";
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblConfirmMsgEN.Text = "";
            lblConfirmMsgTH.Text = "";
            lblCreditBureau.Text = "";
            lblCreditReview.Text = "";
            string err = "";
            if (hid_Confirm.Value == "")
            {
                if (!checkBeforeSave(ref err))
                {
                    lblMsgEN.Text = err;
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                if (int.Parse(txt_qty.Text.Trim()) > 1)
                {
                    hid_Confirm.Value = "QTY";
                    lblConfirmMsgEN.Text = "กรุณายืนยันความถูกต้อง สินค้ามีจำนวนมากกว่า 1 ชิ้น";
                    PopupConfirmSave.ShowOnPageLoad = true;
                    return;
                }


                hid_Confirm.Value = "S_1";
                lblConfirmMsgEN.Text = " Do you want to save ? ";
                PopupConfirmSave.ShowOnPageLoad = true;
                return;


            }


            //saveCredit(ref err);
            //if (!checkBeforeSave(ref err))
            //{

            //}
            //else
            //{
            //    lblConfirmMsgEN.Text = "Do you want to save ? ";
            //    PopupConfirmSave.ShowOnPageLoad = true;
            //}
        }
        catch (Exception ex)
        {
            lblMsgEN.Text = ex.Message.ToString();
            PopupMsg.ShowOnPageLoad = true;
            return;
        }
    }
    protected void btn_ReasonS_Click(object sender, EventArgs e)
    {

    }
    protected void btn_ProductS_Click(object sender, EventArgs e)
    {

    }
    protected void btn_vendorS_Click(object sender, EventArgs e)
    {

    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        string card_no = txt_card_no.Text.Trim().Replace("-", "");
        lblAppNo.Text = "";
        lblContract.Text = "";
        hd_ncb.Value = "";
        hid_App.Value = "";
        hid_cont.Value = "";


        fn_Clear();
        if (card_no != "")
        {
            try
            {
                
                userInfo = userInfoService.GetUserInfo();
                ILDataCenterMssqlUsingCard CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
                iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
                DataSet ds = CallEntUsingCard.getCustInfoByCard(card_no.Replace("-", "")).Result;
                CallEntUsingCard._dataCenter.CloseConnectSQL();

                if (ilObj.check_dataset(ds))
                {
                    DataRow dr = ds.Tables[0]?.Rows[0];
                    string csn = dr["CSN"].ToString().Trim();
                    hid_birthDate.Value = dr["BirthDate"].ToString().Trim();
                    hidName.Value = dr["Name"].ToString();
                    hidSurname.Value = dr["Surname"].ToString();
                    hidMobile.Value = dr["m11Mob"].ToString();
                    hidGender.Value = dr["Gender"].ToString();
                    hidExt.Value = dr["M11EXT"].ToString();
                    hidSalary.Value = dr["SalaryType"].ToString();
                    hidTambol.Value = dr["Tambol_H"].ToString();
                    hidAmphur.Value = dr["Amphur_H"].ToString();
                    hidProvince.Value = dr["Province_H"].ToString();
                    hidZipcode.Value = dr["Zipcode_H"].ToString();

                    lb_nameCust.Text = dr["TitleName"].ToString().Trim() + "  " + dr["Name"].ToString() + "   " + dr["Surname"].ToString();
                    lb_csn.Text = csn;
                    //  ***  validate customer  &&  Auto Cancel SL 36 ***//
                    string errMsg = "";
                    string typeErr = "";
                    if (!verifyCustomerN(card_no, dr["IDCard"].ToString().Trim(), lb_csn.Text.Trim(), ref errMsg, ref typeErr))
                    {
                        string ErrRj = "";
                        string appno = "";
                        if (typeErr.Trim() != "")
                        {
                            //***** get Note ******//
                            DataSet ds_note = CallEntUsingCard.getResultCodeWithCode("SL36").Result;
                            CallEntUsingCard._dataCenter.CloseConnectSQL();

                            string NoteDesc = "" + "";
                            if (ds_note != null)
                            {
                                if (ds_note.Tables[0]?.Rows.Count > 0)
                                {
                                    NoteDesc = ds_note.Tables[0].Rows[0]["G25DES"].ToString().Trim() + " (" + errMsg + " )";
                                }
                            }
                            else
                            {
                                lblMsgEN.Text = "";
                                lblMsgTH.Text = "";
                                lblMsgTH.Text = "Get note for Auto Cancel not success ";
                                pl_cust.Enabled = false;
                                PopupMsg.ShowOnPageLoad = true;
                                return;
                            }

                            // *** Auto Cancel Summary Screen ***// 
                            bool resCancel = saveCancel("ADD", "SL36", " ", NoteDesc, ref ErrRj, ref appno);
                            if (!resCancel)
                            {
                                lblMsgEN.Text = "";
                                lblMsgTH.Text = "";
                                lblMsgTH.Text = ErrRj;
                                pl_cust.Enabled = false;
                                PopupMsg.ShowOnPageLoad = true;
                                return;
                            }
                            else
                            {
                                errMsg = NoteDesc;
                                lblMsgEN.Text = "Application No. " + appno;
                                lblMsgTH.Text = "";
                                lblMsgTH.Text = "Auto Cancel complete. " + errMsg;
                                pl_cust.Enabled = false;
                                PopupMsg.ShowOnPageLoad = true;
                                fn_Clear();
                                txt_card_no.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            // **  Show error message popup **//
                            lblMsgEN.Text = "";
                            lblMsgTH.Text = "";
                            lblMsgTH.Text = errMsg;
                            pl_cust.Enabled = false;
                            PopupMsg.ShowOnPageLoad = true;
                            return;
                        }
                    }
                    pl_cust.Enabled = true;
                    hid_m00sal.Value = dr["m00sal"].ToString().Trim();

                    bindMaritalStatus(dr["MaritalStatus"].ToString().Trim());


                    txt_idNo.Text = dr["IDCard"].ToString().Trim();

                    if (dr["ExpireDate"].ToString().Trim() != "")
                    {
                        txt_expireDate.Text = dr["ExpireDate"].ToString().Trim().Substring(6, 2) + "/" + dr["ExpireDate"].ToString().Trim().Substring(4, 2) + "/" + dr["ExpireDate"].ToString().Trim().Substring(0, 4);
                    }
                    bindResidentType(dr["TypeofResident"].ToString().Trim());

                    if (dr["TotalFamily"].ToString().Trim() != "" && dr["TotalFamily"].ToString().Trim() != "0")
                    {
                        txt_fPerson.Text = dr["TotalFamily"].ToString().Trim();
                        txt_fPerson.Enabled = false;
                    }
                    else
                    {
                        txt_fPerson.Text = "";
                        txt_fPerson.Enabled = true;
                    }

                    if ((dr["ResidentYear"].ToString().Trim() == "" || dr["ResidentYear"].ToString().Trim() == "0") && (dr["ResidentMonth"].ToString().Trim() == "" || dr["ResidentMonth"].ToString().Trim() == "0"))
                    {
                        txt_yearResident.Text = "";
                        txt_yearResident.Enabled = true;

                        txt_monthResident.Text = "";
                        txt_monthResident.Enabled = true;
                    }
                    else
                    {
                        txt_yearResident.Text = dr["ResidentYear"].ToString().Trim();
                        txt_yearResident.Enabled = false;

                        txt_monthResident.Text = dr["ResidentMonth"].ToString().Trim();
                        txt_monthResident.Enabled = false;
                    }


                    bindBusinessType(dr["Business"].ToString().Trim());


                    bindOccupation(dr["Occupation"].ToString().Trim());
                    bindCommercial(dr["Occupation"].ToString().Trim());
                    if (dr["Occupation"].ToString().Trim() != "")
                    {
                        if (dr["Occupation"].ToString().Trim() == "011" || dr["Occupation"].ToString().Trim() == "012")
                        {
                            dd_comerc.Value = "N";
                        }
                        else
                        {
                            dd_comerc.Value = "";
                        }
                        dd_comerc.Enabled = false;
                    }
                    else
                    {
                        dd_comerc.Value = "";
                        dd_comerc.Enabled = true;
                    }


                    bindPosition(dr["Position"].ToString().Trim());




                    if (dr["TotalEmployee"].ToString().Trim() != "" && dr["TotalEmployee"].ToString().Trim() != "0")
                    {
                        txt_empNo.Text = dr["TotalEmployee"].ToString().Trim();
                        txt_empNo.Enabled = false;
                    }
                    else
                    {
                        txt_empNo.Text = "";
                        txt_empNo.Enabled = true;
                    }

                    bindEmployeeType(dr["EmployeeType"].ToString().Trim());

                    if ((dr["TotalWorkYear"].ToString().Trim() == "" || dr["TotalWorkYear"].ToString().Trim() == "0") && (dr["TotalWorkMonth"].ToString().Trim() == "" || dr["TotalWorkMonth"].ToString().Trim() == "0"))
                    {
                        txt_service_Y.Text = "";
                        txt_service_Y.Enabled = true;
                        txt_service_M.Text = "";
                        txt_service_M.Enabled = true;
                    }
                    else
                    {
                        txt_service_Y.Text = dr["TotalWorkYear"].ToString().Trim();
                        txt_service_Y.Enabled = false;
                        txt_service_M.Text = dr["TotalWorkMonth"].ToString().Trim();
                        txt_service_M.Enabled = false;
                    }




                    //****  Salary  ****//

                    string salary = "";
                    string date_sal = "";
                    string time_sal = "";

                    userInfo = userInfoService.GetUserInfo();
                    ilObj.UserInfomation = userInfoService.GetUserInfo();
                    bool resSal = iLDataSubroutine.Call_CSSR07(txt_idNo.Text.Trim(), "99999999", ref salary, ref date_sal, ref time_sal, userInfo.BizInit, userInfo.BranchNo);
                    //ilObj.CloseConnectioDAL();
                    if (resSal)
                    {
                        txt_salary.Text = salary;
                        hid_date_sal_old.Value = salary;
                        hid_date_sal_old_d.Value = date_sal;
                        hid_date_sal_old_t.Value = time_sal;


                    }
                    else
                    {

                    }
                    // ***  Tel home ***//


                    string TelType = "";
                    string Zip = "";
                    string O_TLDS = "";
                    string Error = "";




                    if (dr["m11mob"].ToString().Trim().Length == 10 && !dr["m11mob"].ToString().Trim().Contains('*'))//if (dr["m11mob"].ToString().Trim() != "")
                    {
                        hid_mobile.Value = dr["m11mob"].ToString().Trim();

                        rb_mobile.Value = "Y";
                        rb_mobile.Enabled = false;
                    }
                    else
                    {
                        hid_mobile.Value = "";
                        rb_mobile.Value = "N";
                        rb_mobile.Enabled = false;
                        txt_telM.Enabled = false;
                        txt_telM_C.Enabled = false;
                    }





                    hid_m11Tel.Value = dr["m11tel"].ToString().Trim();
                    hid_m00ofc.Value = dr["m00ofc"].ToString().Trim();
                    hid_m00oft.Value = dr["m00oft"].ToString().Trim();


                    txt_birthDate_P.Focus();
                }
                else
                {
                    lb_rescard.Text = "Not found customer";
                }

            }
            catch (Exception ex)
            {
                lb_rescard.Text = "Error" + ex.Message;
            }
        }
    }
    protected void btn_calculate_Click(object sender, EventArgs e)
    {

        // check judgment form before save data.
        string err = "";
        if (!checkBeforeJudgment(ref err))
        {
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblMsgTH.Text = err;
            PopupMsg.ShowOnPageLoad = true;
            return;
        }
        else
        { //*** calculate TCL  ***//
            if (!calTCL(ref err))
            {
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }


            bindPaymentType();
            // bindTerm();
            bindVendor();
            bind_ProductType();
            //bindReason();
            btn_cal_TCL.Enabled = false;
            btn_check_ncb.Enabled = true;
            btn_cancel_case.Enabled = false;
            tabDetail.TabPages.FindByName("TabTCL").ClientEnabled = true;
            tabDetail.TabPages.FindByName("TabTCL").Visible = true;
            tabDetail.ActiveTabPage = tabDetail.TabPages.FindByName("TabTCL");
            ASPxSplitter1.GetPaneByName("P_TCL").Collapsed = true;
            //ASPxSplitter1.FindControl("SplitterContentControl3").Co;
            //ASPxSplitter1.GetPaneByName('').Collapsed 
            txt_qty.Text = "1";
            lblContract.Text = "";
            lblAppNo.Text = "";



            //tabDetail.ActiveTabPage.FindControl("TabTCL");
            //tabDetail.TabPages.FindByName("TabTCL")
            //tabDetail.TabPages.FindByName("TabTCL")
            //tabDetail.TabPages[1].Enabled = true;
        }

    }
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        txt_card_no.Text = "";
        fn_Clear();
    }
    protected void btn_scr_Click(object sender, EventArgs e)
    {

    }
    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        string step = hid_Confirm.Value.Trim();
        if (step == "CRMD")
        {
            hid_Confirm.Value = "";
            hid_CRMD.Value = "N";
            btnConfirmSave.Text = "OK";
            btnConfirmCancel.Text = "Cancel";
            checkCR_MODEL();
            return;
        }
        if (step == "S_1")
        {
            hid_Confirm.Value = "";
            return;
        }
        hid_App.Value = "";
        hid_cont.Value = "";
        hid_Confirm.Value = "";
    }
    protected void btnConfirmSave_Click(object sender, EventArgs e)
    {
        try
        {
            string step = hid_Confirm.Value.Trim();
            if (step == "QTY")
            {
                hid_Confirm.Value = "";
                getAppNo();
            }
            else if (step == "S_1" && hid_App.Value == "" && hid_cont.Value == "")   //
            {
                getAppNo();

            }
            else if (step == "S_1" && hid_App.Value != "" && hid_cont.Value != "")
            {
                hid_Confirm.Value = "S_2";
                saveCredit(hid_App.Value, hid_cont.Value);
            }
            else if (step == "S_1" && hid_App.Value != "" && hid_cont.Value == "")
            {
                getContNo();
            }
            else if (step == "S_2")
            {

            }
            else if (step == "NCB")
            {
                hid_Confirm.Value = "";
                checkID_NCB();
                btnConfirmSave.Text = "YES";
                btnConfirmCancel.Text = "NO";
            }
            else if (step == "CRMD")
            {
                hid_Confirm.Value = "";
                hid_CRMD.Value = "Y";
                checkCR_MODEL();
                btnConfirmSave.Text = "OK";
                btnConfirmCancel.Text = "Cancel";
            }

        }
        catch (Exception ex)
        {
        }
    }
    private void checkCR_MODEL()
    {
        string checkreject = hid_RejectCK.Value;
        string checkbureau = hid_bureau.Value;
        string checknopopup = hid_RejectEN.Value;
        bool success = true;
        if (checkreject == "RJ")
        {
            //Start Pact 20170717 Update CSMS13
            userInfo = userInfoService.GetUserInfo();
            iDB2Command cmd = new iDB2Command();
            ILDataCenter busobj = new ILDataCenter();
            busobj.UserInfomation = userInfoService.GetUserInfo();
            //DataSet ds_cus = CallEntUsingCard.getCustInfoByCard(txt_card_no.Text.Trim().Replace("-", "")).Result;
            //CallEntUsingCard._dataCenter.CloseConnectSQL();
            //if (!busobj.check_dataset(ds_cus))
            //{
            //    lblMsgEN.Text = "cannot find customer";
            //    hid_Confirm.Value = "";
            //    hid_cont.Value = "";
            //    //hid_App.Value = "";
            //    PopupMsg.ShowOnPageLoad = true;

            //    return;
            //}
            string idcard = txt_idNo.Text;
            string birthDate = hid_birthDate.Value?.ToString().Trim();
            string name = hidName.Value?.ToString().Trim();
            string surName = hidSurname.Value?.ToString().Trim();
            string CSN = lb_csn.Text.ToString();
            string gender = hidGender.Value?.ToString().Trim();
            string mobile = hidMobile.Value?.ToString().Trim();

            int affectedRows;
            //if(dataCenter.SqlCon.State != ConnectionState.Open)
            //{
            //    dataCenter.OpenConnectSQL();

            //}
            //dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
            //DataRow dr_cust = ds_cus.Tables[0]?.Rows[0];
            string cmdCSMS13 = "";
            if (hid_CRRW.Value == "")
            {    // Up Credit Model Only
                cmdCSMS13 = "Update AS400DB01.CSOD0001.CSMS13 set " +
                                "M13FIL = SUBSTRING(M13FIL,1,28) + '" + hid_CRMD.Value.Trim() + "' + SUBSTRING(M13FIL, 30, LEN(M13FIL)) " +
                                "where m13app = 'IL'" + " and m13csn = '" + CSN + "' and m13brn = '" + userInfo.BranchApp.ToString() + "' and m13apn = '" + hid_App.Value + "' ";
            }
            else
            {   // Up Credit Reveiew and Credit Model
                cmdCSMS13 = "Update AS400DB01.CSOD0001.CSMS13 set " +
                                "M13FIL = SUBSTRING(M13FIL,1,27) + '" + hid_CRRW.Value.Trim() + hid_CRMD.Value.Trim() + "' + SUBSTRING(M13FIL, 30, LEN(M13FIL)) " +
                                "where m13app = 'IL'" + " and m13csn = '" + CSN + "' and m13brn = '" + userInfo.BranchApp.ToString() + "' and m13apn = '" + hid_App.Value + "' ";
            }

            affectedRows = -1;
            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                var res = dataCenter.Execute(cmdCSMS13, CommandType.Text, transaction).Result;
                affectedRows = res.afrows;
                if (affectedRows < 0)
                {
                    success = false;
                    Task.Run(() =>
                    {
                        Utility.WriteLogString(res.message.ToString(), cmdCSMS13);
                    });
                    lblMsgTH.Text = "";
                    lblMsgEN.Text = "Save Credit Model : not complete.";
                    PopupMsg.ShowOnPageLoad = true;
                    hid_Confirm.Value = "";
                    hid_cont.Value = "";
                    hd_ncb.Value = "";
                    goto commit_rollback;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Task.Run(() => { Utility.WriteLog(ex); });
                lblMsgTH.Text = "";
                lblMsgEN.Text = "Save Credit Model : not complete.";
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hd_ncb.Value = "";
                goto commit_rollback;
            }


        commit_rollback:
            if (success)
            {
                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();


            }
            else
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();


                return;
            }

            //End Pact 20170717 Update CSMS13
            lblMsgEN.Text = hid_RejectEN.Value;
            lblMsgTH.Text = hid_RejectTH.Value;
            PopupMsg.ShowOnPageLoad = true;
            fn_Clear();
            hid_Confirm.Value = "";
            hid_cont.Value = "";
            hid_App.Value = "";
            hd_ncb.Value = "";
            txt_card_no.Text = "";
            return;
        }
        else if (checkbureau == "C")
        {
            lblMsgEN.Text = hid_RejectEN.Value;
            lblMsgTH.Text = hid_RejectTH.Value;
            pl_cust.Enabled = false;
            PopupMsg.ShowOnPageLoad = true;
            fn_Clear();
            hid_App.Value = "";
            txt_card_no.Text = "";
            return;
        }
        else
        {
            btn_check_ncb.Enabled = false;
            btn_cal_TCL.Enabled = true;
            btn_cancel_case.Enabled = true;
            lblMsgEN.Text = hid_RejectEN.Value;
            lblMsgTH.Text = hid_RejectTH.Value;
            if (checknopopup == "N")
            {
                PopupMsg.ShowOnPageLoad = true;
                lblMsgEN.Text = "";
            }
            return;
        }
    }
    protected void gvTerm_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void dd_bankCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBankBranch(dd_bankCode.SelectedItem.Value.ToString().Trim());
            txt_AccountNo.Text = "";

        }
        catch (Exception ex)
        {
        }
    }
    protected void dd_paymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dd_paymentType.SelectedItem.Value.ToString().Trim() == "1")
        {
            bindDebitAccount(lb_csn.Text.Trim());
            dd_bankCode.Enabled = true;
            dd_bankBranch.Enabled = true;
            dd_accountType.Enabled = true;
            pl_payment.Visible = true;
        }
        else
        {
            pl_payment.Visible = false;
            dd_bankCode.Text = "";
            dd_bankBranch.Text = "";
            dd_accountType.Text = "";

            dd_bankCode.Enabled = false;
            dd_bankBranch.Enabled = false;
            dd_accountType.Enabled = false;

            dd_bankCode.Items.Clear();
            dd_bankBranch.Items.Clear();
            dd_accountType.Items.Clear();
            txt_AccountNo.Text = "";

        }

    }


    protected void dd_vendor_TextChanged(object sender, EventArgs e)
    {

        bindCampaign();
        dd_campaign.SelectedIndex = -1;
        dd_campaign.Focus();

    }
    protected void dd_campaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            ilObj = new ILDataCenter();
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');

            string[] appDate_s = txt_appDate.Text.Trim().Split('/');
            string appDate = appDate_s[2] + appDate_s[1] + appDate_s[0];

            DataSet ds = CallMasterEnt.getCampaign(vendor[0], txt_totalterm.Text.Trim(), campaign[0], campaign[1], campaign[2], appDate);
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds))
            {

                txt_campSeq.Text = campaign[1];
                DataRow dr = ds.Tables[0]?.Rows[0];
                txt_campgType.Text = dr["C01CTY"].ToString().Trim();
                txt_total_range.Text = dr["c02rsq"].ToString().Trim();
                txt_non.Text = dr["c01nxd"].ToString().Trim();

                txt_Int.Text = float.Parse(dr["C02AIR"].ToString().Trim()).ToString("0.00");
                txt_cru.Text = float.Parse(dr["C02ACR"].ToString().Trim()).ToString("0.00");

                dd_product.Items.Clear();
                dd_product.Value = "";
                bindProduct(vendor[0], campaign[0], campaign[1]);

            }


        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_cal_TCL_Click(object sender, EventArgs e)
    {
        try
        {
            string err = "";
            DataSet ds = new DataSet();
            if (calculateInstallment(ref err, ref ds))
            {
                if (ilObj.check_dataset(ds))
                {
                    gvTerm.DataSource = ds.Tables?[0];
                    gvTerm.DataBind();

                    gv_install.DataSource = ds.Tables?[1];
                    gv_install.DataBind();

                    if (ds.Tables?[2] != null)
                    {
                        DataRow dr_detail = ds.Tables?[2].Rows?[0];
                        txt_purch.Text = dr_detail["Total_pur"].ToString();
                        txt_loanReq.Text = dr_detail["loanRequest"].ToString();
                        txt_fDue_AMT.Text = dr_detail["FdueAmt"].ToString();
                        txt_fDue_date.Text = dr_detail["FdueDate"].ToString();
                        txt_duty.Text = dr_detail["Duty"].ToString();
                        txt_bureau.Text = dr_detail["Bureau"].ToString();
                        txt_contractAmt.Text = dr_detail["ContAmt"].ToString();
                    }

                    disable_key_in();
                    btn_cal_TCL.Enabled = false;
                    btn_keyin.Enabled = true;
                    btn_saveCr.Enabled = true;
                    ASPxSplitter1.GetPaneByName("P_TCL").Collapsed = false;


                }


            }
            else
            {
                //lblMsgEN.Text = "";
                //lblMsgTH.Text = "";
                lblMsgEN.Text = err;

                PopupMsg.ShowOnPageLoad = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblMsgEN.Text = ex.Message.ToString();

            PopupMsg.ShowOnPageLoad = true;
            return;
        }
    }

    private void bindAddNote()
    {
        Connect_NoteAPI connect_Note = new Connect_NoteAPI();
        try
        {
            //lblMsg.Text = "";
            userInfo = userInfoService.GetUserInfo();
            txt_memoReason.Text = "";
            ILDataCenterMssqlInterview iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
            DataSet ds_resCode = iLDataCenterMssql.getResultCode().Result;
            DataSet ds_ActCode = iLDataCenterMssql.getActionCode().Result;
            iLDataCenterMssql._dataCenter.CloseConnectSQL();
            if (!ilObj.check_dataset(ds_resCode) || !ilObj.check_dataset(ds_ActCode))
            {
                return;
            }

            dd_ActionCode.Items.Add("--- Select ---", "");
            foreach (DataRow dr in ds_ActCode.Tables?[0].Rows)
            {

                dd_ActionCode.Items.Add(
                    new ListEditItem(dr["G24ACD"].ToString().Trim() + " : " + dr["G24DES"].ToString().Trim(), dr["G24ACD"].ToString().Trim()));
            }

            dd_ActionCode.Value = "ADD";

            dd_ResCode.Items.Add("--- Select ---", "");
            foreach (DataRow dr in ds_resCode.Tables?[0].Rows)
            {

                dd_ResCode.Items.Add(
                    new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
            }
            dd_ResCode.Value = "";
            Session["ds_Note"] = null;
            //ยังไม่เขียน GetNote
            List<ResponseGetnote> responseGetnotes = new List<ResponseGetnote>();
            DataSet ds_note = new DataSet();
            var res_note = connect_Note.GetNote(lb_csn.Text.Trim()).Result;
            if (!res_note.success)
            {
                //JsonConvert.DeserializeObject<DataSet>(data)
            }
            else
            {
                var response = JsonConvert.DeserializeObject<GetTimeline>(res_note.data?.ToString());
                if (response.dataAPI.Count > 0)
                {
                    foreach (var prop in response.result.data)
                    {
                        responseGetnotes.Add(new ResponseGetnote()
                        {
                            M38ACD = prop.actionCode,
                            M38USR = prop.noteByName,
                            M38DES = prop.noteDescription,
                            M38DAT_ = prop.noteDate?.ToString("dd/MM/yyyy"),
                            M38TIM = prop.noteDate?.ToString("HH:mm:ss")


                        });
                    }
                    var res = responseGetnotes.ToDataTable();
                    ds_note.Tables.Add(res);
                }

            }
            Session["ds_Note"] = ds_note;
            gvNote.DataSource = ds_note;
            gvNote.DataBind();
            gvNote.PageIndex = 0;

            Popup_AddNote.ShowOnPageLoad = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }


    protected void gvNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNote.PageIndex = e.NewPageIndex;
        gvNote.DataSource = (DataSet)Session["ds_Note"];
        gvNote.DataBind();
    }
    protected void btn_saveNote_Click(object sender, EventArgs e)
    {
        ILDataCenter ilObj = new ILDataCenter();
        Connect_NoteAPI noteAPI = new Connect_NoteAPI();
        try
        {
            lblMsg.Text = "";
            userInfo = userInfoService.GetUserInfo();
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            string[] AppDateS = txt_appDate.Text.Trim().Split('/');
            string appDate = (AppDateS[2] + AppDateS[1] + AppDateS[0]).PadLeft(8, '0');

            if (lb_csn.Text.Trim() == "")
            {
                lblMsg.Text = "Cannot add note before search customer ";
                lblMsg.Visible = true;
                return;
            }
            //Check Input Error Format 
            if (
                txt_memoReason.Text.Trim().IndexOf("'") >= 0 ||
                txt_memoReason.Text.Trim().IndexOf("\"") >= 0 ||
                txt_memoReason.Text.Trim().IndexOf("\\r") >= 0 ||
                txt_memoReason.Text.Trim().IndexOf("\\t") >= 0 ||
                txt_memoReason.Text.Trim().IndexOf("\\n") >= 0
                //txt_memoReason.Text.Trim().IndexOf("'") >= 0 || txt_memoReason.Text.Trim().IndexOf("\"") >= 0
                //|| txt_memoReason.Text.Trim().IndexOf("?") >= 0 || txt_memoReason.Text.Trim().IndexOf("+") >= 0
                //|| txt_memoReason.Text.Trim().IndexOf("%") >= 0 || txt_memoReason.Text.Trim().IndexOf("!") >= 0
                //|| txt_memoReason.Text.Trim().IndexOf("=") >= 0
                //|| txt_memoReason.Text.Trim().IndexOf(":") >= 0 || txt_memoReason.Text.Trim().IndexOf(";") >= 0
                //|| txt_memoReason.Text.Trim().IndexOf("<") >= 0 || txt_memoReason.Text.Trim().IndexOf(">") >= 0
                )
            {
                lblMsg.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ ? + % ! = : ; < > ";
                lblMsg.Visible = true;
                txt_memoReason.Focus();
                return;
            }
            if (dd_ActionCode.Value.ToString() == "" || dd_ResCode.Value.ToString() == "")
            {
                lblMsg.Text = "Please input Action code or result code.";
                lblMsg.Visible = true;
                return;

            }
            if (txt_memoReason.Text.Trim() == "")
            {
                lblMsg.Text = "Please input note detail.";
                lblMsg.Visible = true;
                return;
            }

            if (txt_memoReason.Text.Trim().Length > 200)
            {
                lblMsg.Text = "Lengh of note detail > 200 Characters.";
                lblMsg.Visible = true;
                return;
            }
            string desc1 = "";
            string desc2 = "";
            if (txt_memoReason.Text.Trim().Length <= 100)
            {
                desc1 = txt_memoReason.Text;
            }
            else if (txt_memoReason.Text.Trim().Length > 100)
            {
                desc1 = txt_memoReason.Text.Substring(0, 100);
                desc2 = txt_memoReason.Text.Substring(100);
            }

            string ErrMsgNote = "";
            int affectedRows = -1;
            var resNote = noteAPI.AddNote(lb_csn.Text.Trim(), "0", "ADD", desc1, desc1.ToString().Trim(), appDate, m_UpdTime.ToString().PadLeft(6, '0').Trim()).Result;
            affectedRows = Convert.ToInt32(resNote.success);
            // bool AddNote = iLDataSubroutine.CALL_CSSRW11("IL", txt_idNo.Text.Trim(), dd_ActionCode.Value.ToString(), dd_ResCode.Value.ToString(),
            //                         desc1, desc2, ref ErrMsgNote, userInfo.BizInit, userInfo.BranchNo);
            if (affectedRows < 0)
            {
                lblMsg.Text = "Cannot Save note  : " + resNote.ToString().Trim();
                // ilObj.RollbackDAL();
                // ilObj.CloseConnectioDAL();
                return;
            }
            // ilObj.CommitDAL();
            // ilObj.CloseConnectioDAL();
            lblMsg.Text = "Save note success ";
            lblMsg.Visible = true;
            bindAddNote();

            //Popup_AddNote.ShowOnPageLoad = false;

            return;

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Save note not success ";
            lblMsg.Visible = true;
            // ilObj.RollbackDAL();
            // ilObj.CloseConnectioDAL();
            return;
        }
    }
    protected void btn_cancel_Save_Click(object sender, EventArgs e)
    {
        Popup_AddNote.ShowOnPageLoad = false;
    }

    protected void btn_keyin_Click(object sender, EventArgs e)
    {
        fn_key_in();
    }
    protected void txt_loanReq_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txt_price_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txt_qty_TextChanged(object sender, EventArgs e)
    {

    }
    protected void dd_product_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string[] product = dd_product.SelectedItem.Value.ToString().Trim().Split('|');
            txt_minPrice.Text = product[1];
            txt_maxPrice.Text = product[2];


        }
        catch (Exception ex)
        {
            string product = dd_product.Text.Trim();
            if (product.Length > 3 && product.Length < 15)
            {
                string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
                string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');
                bindProduct(vendor[0], campaign[0], campaign[1], product);

            }
            else
            {

                return;
            }

        }

    }
    protected void dd_product_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string product = dd_product.Text.Trim();
            if (product.Length > 3 && product.Length < 15)
            {
                string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
                string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');
                bindProduct(vendor[0], campaign[0], campaign[1], product);

            }
            else if (product.Length > 15)
            {
                string[] productSel = dd_product.Value.ToString().Split('|');
                txt_minPrice.Text = productSel[1];
                txt_maxPrice.Text = productSel[2];

                return;
            }
            else
            {

                return;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void dd_vendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            txt_rank_v.Text = vendor[1];
            bindCampaign();
            dd_campaign.SelectedIndex = -1;
            dd_campaign.Focus();

        }
        catch (Exception ex)
        {

        }
    }
    protected void dd_totalterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_vendor.Text == "")
            {

                bindVendor();
                dd_vendor.Value = "";


            }
            else
            {

                bindCampaign();
                dd_campaign.Value = "";
                dd_product.Items.Clear();
                dd_product.Value = "";
                lb_prodcount.Text = "";
                dd_vendor.Focus();


            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnNote_Click(object sender, EventArgs e)
    {
        string url = @"window.open('" + "../Note/Note.aspx?param1=" + lb_csn.Text.Trim() + "&param2=" + txt_idNo.Text.Trim() + "', 'home', 'width=900,height=550,left=45,top=150,scrollbars=yes,resizable=yes');";
        ScriptManager.RegisterStartupScript(this, typeof(string), "script_note", url, true);
        //bindAddNote();
        //Popup_AddNote.ShowOnPageLoad = true;
        //return;
    }
    protected void txt_birthDate_P_PreRender(object sender, EventArgs e)
    {
        //txt_birthDate_P.Attributes.Add("value", "123");
        //txt_birthDate_P.Attributes["value"] = txt_birthDate_P.Text;
        if (IsPostBack)
        {
            if (!(String.IsNullOrEmpty(txt_birthDate_P.Text.Trim())))
            {
                txt_birthDate_P.Attributes["value"] = txt_birthDate_P.Text;
            }
        }

    }
    protected void dd_occup_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCommercial(dd_occup.Value.ToString().Trim());
    }
    protected void dd_comerc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_comerc.Value.ToString() == "Y")
            {
                lbl_c_msg_en.Text = " ยืนยันการได้รับเอกสาร Commercial Registration ? ";
                Popup_Commerce.ShowOnPageLoad = true;
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_c_ok_Click(object sender, EventArgs e)
    {

    }
    protected void btn_c_cancel_Click(object sender, EventArgs e)
    {
        dd_comerc.Value = "N";
    }
    protected void btn_src_pdItem_Click(object sender, EventArgs e)
    {
        try
        {
            lb_res_search.Text = "";
            if (dd_prodType.Value == null)
            {
                lb_res_search.Text = "กรุณาระบุ Product Type";
                return;
            }
            if ((!dd_prodType.Value.Equals("")) && (dd_prodBrand.Value == null && dd_prodcode.Value == null))
            {
                lb_res_search.Text = "กรุณาระบุ Produc type, Brand หรือ Product code อย่างน้อย 2 ช่อง";
                return;
            }

            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            ILDataCenterMssqlKeyin = new ILDataCenterMssqlKeyinStep1(userInfoService.GetUserInfo());
            string prodType = dd_prodType.SelectedItem.Value.ToString();
            string prodCode = "";
            if (dd_prodcode.Value != null && !dd_prodcode.Value.Equals(""))
            {
                prodCode = dd_prodcode.Value.ToString();
            }

            //dd_prodcode.Value.Equals("") ? dd_prodcode.Text : dd_prodcode.Value.ToString();
            string brand_name = "";//dd_prodBrand.Value.Equals("")?dd_prodBrand.Text:dd_prodBrand.Value.ToString();
            if (dd_prodBrand.Value != null && !dd_prodBrand.Value.Equals(""))
            {
                brand_name = dd_prodBrand.Value.ToString();
            }


            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');

            DataSet ds = new DataSet();



            if (prodType != "" && brand_name != "" && prodCode != "")
            {
                ds = ILDataCenterMssqlKeyin.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, brand_name, prodCode).Result;

            }
            else if (prodType != "" && brand_name != "" && prodCode == "")
            {
                ds = ILDataCenterMssqlKeyin.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, brand_name).Result;
            }
            else if (prodType != "" && brand_name == "" && prodCode != "")
            {
                ds = ILDataCenterMssqlKeyin.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, "", prodCode).Result;
            }
            gv_item.DataSource = ds;
            gv_item.DataBind();


        }
        catch (Exception ex)
        {
            lb_res_search.Text = "Error กรุณาทำการค้นหาใหม่อีกครั้ง";
        }
    }
    protected void dd_prodBrand_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string brand = dd_prodBrand.Text.Trim();
            if (brand.Length < 10)
            {
                bindProductBrand(brand);
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void dd_prodBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }

    }
    protected void dd_prodcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }
    protected void dd_prodcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string desc = dd_prodcode.Text.Trim();
            if (desc.Length < 7)
            {
                string type = dd_prodType.Value.ToString();
                bindProductCode(desc, type);
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void gv_item_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int dataItem = int.Parse(e.CommandArgument.ToString());
            ilObj = new ILDataCenter();
            if (e.CommandName.Trim() == "SEL")
            {
                Label itm = (Label)gv_item.Rows[dataItem].Cells[5].FindControl("lb_item_code");
                Label itmDes = (Label)gv_item.Rows[dataItem].Cells[6].FindControl("lb_item_Desc");
                Label itmMin = (Label)gv_item.Rows[dataItem].Cells[12].FindControl("lb_item_MIN");
                Label itmMax = (Label)gv_item.Rows[dataItem].Cells[13].FindControl("lb_item_MAX");
                Label itmPGP = (Label)gv_item.Rows[dataItem].Cells[11].FindControl("lb_item_Group");
                string t44itm = itm.Text.Trim();
                string t44des = itmDes.Text.Trim();
                string c07min = itmMin.Text.Trim();
                string c07max = itmMax.Text.Trim();
                string T44PGP = itmPGP.Text.Trim();

                dd_product.Items.Clear();

                dd_product.Items.Add(
                            new ListEditItem(t44itm + " : " + t44des,
                                t44itm + "|" +
                                c07min + "|" +
                                c07max + "|" +
                                T44PGP
                                ));
                dd_product.SelectedIndex = 1;
                txt_minPrice.Text = c07min;
                txt_maxPrice.Text = c07max;
                Popup_ScrProd.ShowOnPageLoad = false;


            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_vendorScr_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_totalterm.Text.Trim() == "" || dd_vendor.Value == null || dd_campaign.Value == null)
            {
                lblMsgEN.Text = "กรุณาระบุ Total term , Vendor , Campaign ก่อนทำการค้นหา Product ให้ครบถ้วน";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }

            dd_prodType.SelectedIndex = -1;

            dd_prodcode.Value = "";
            dd_prodcode.Items.Clear();
            dd_prodcode.SelectedIndex = -1;

            dd_prodBrand.Value = "";
            dd_prodBrand.Items.Clear();
            dd_prodBrand.SelectedIndex = -1;


            gv_item.DataSource = null;
            gv_item.DataBind();

            Popup_ScrProd.ShowOnPageLoad = true;


        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_cancel_prod_Click(object sender, EventArgs e)
    {
        dd_prodType.SelectedIndex = -1;

        dd_prodcode.Value = "";
        dd_prodcode.Items.Clear();
        dd_prodcode.SelectedIndex = -1;

        dd_prodBrand.Value = "";
        dd_prodBrand.Items.Clear();
        dd_prodBrand.SelectedIndex = -1;


        gv_item.DataSource = null;
        gv_item.DataBind();
    }
    protected void btn_check_ncb_Click(object sender, EventArgs e)
    {
        //*** input validate term product campaign vendor ***//

        try
        {
            if (txt_totalterm.Text.Trim() == "" || dd_vendor.SelectedItem == null || dd_campaign.SelectedItem == null || dd_product.SelectedItem == null)
            {
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = "กรุณาระบุ Term , vendor , campaign , Product ก่อนทำการ Check NCB";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            //else if (txt_totalterm.Text.Trim() == "" || dd_vendor.Value.ToString() == "" || dd_campaign.Value.ToString() == "" || dd_product.Value.ToString() == "")
            //{
            //    lblMsgEN.Text = "";
            //    lblMsgTH.Text = "";
            //    lblMsgTH.Text = "กรุณาระบุ Term , vendor , campaign , Product ก่อนทำการ Check NCB";
            //    PopupMsg.ShowOnPageLoad = true;
            //    return;
            //}



        }
        catch (Exception ex)
        {
            lblMsgEN.Text = "";
            lblMsgTH.Text = "";
            lblMsgTH.Text = "กรุณาระบุ Term , vendor , campaign , Product ก่อนทำการ Check NCB";
            PopupMsg.ShowOnPageLoad = true;
            return;
        }

        //***************************************************//

        hid_Confirm.Value = "NCB";
        lblConfirmMsgEN.Text = " Do you want to Check NCB ? ";
        lblConfirmMsgTH.Text = "";
        lblCreditBureau.Text = "";
        lblCreditReview.Text = "";
        //btnConfirmCancel.Text = "Cancel";
        PopupConfirmSave.ShowOnPageLoad = true;
        return;
    }
    protected void btn_cancel_case_Click(object sender, EventArgs e)
    {

        bindNoteCancel();
        return;
    }
    protected void B_savenote_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            L_msg_note_Cancel.Text = "";

            userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            string[] AppDateS = txt_appDate.Text.Trim().Split('/');
            string appDate = (AppDateS[2] + AppDateS[1] + AppDateS[0]).PadLeft(8, '0');

            if (lb_csn.Text.Trim() == "")
            {
                L_msg_note_Cancel.Text = "Cannot add note before search customer ";
                L_msg_note_Cancel.Visible = true;
                return;
            }
            //Check Input Error Format 
            if (
                E_note_Cancel.Text.Trim().IndexOf("'") >= 0 ||
                E_note_Cancel.Text.Trim().IndexOf("\"") >= 0 ||
                E_note_Cancel.Text.Trim().IndexOf("\\r") >= 0 ||
                E_note_Cancel.Text.Trim().IndexOf("\\t") >= 0 ||
                E_note_Cancel.Text.Trim().IndexOf("\\n") >= 0
                )
            {
                L_msg_note_Cancel.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ ? + % ! = : ; < > ";
                L_msg_note_Cancel.Visible = true;
                E_note_Cancel.Focus();
                return;
            }
            if (D_action.Value.ToString() == "" || D_reason.Value.ToString() == "")
            {
                L_msg_note_Cancel.Text = "Please input Action code or result code.";
                L_msg_note_Cancel.Visible = true;
                return;

            }
            if (E_note_Cancel.Text.Trim() == "")
            {
                L_msg_note_Cancel.Text = "Please input note detail.";
                L_msg_note_Cancel.Visible = true;
                return;
            }

            if (E_note_Cancel.Text.Trim().Length > 200)
            {
                L_msg_note_Cancel.Text = "Lengh of note detail > 200 Characters.";
                L_msg_note_Cancel.Visible = true;
                return;
            }


            // *** Auto Cancel Summary Screen ***//
            string ErrCancel = "";
            string appno = "";
            bool resCC = saveCancel(D_action.Value.ToString(), D_reason.Value.ToString(), hd_ncb.Value.Trim().ToUpper(), E_note_Cancel.Text.Trim(), ref ErrCancel, ref appno, "PRODUCT");
            if (!resCC)
            {
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = ErrCancel;
                pl_cust.Enabled = false;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            else
            {
                P_note_cancel.ShowOnPageLoad = false;
                lblMsgEN.Text = "Application No. " + appno;
                lblMsgTH.Text = "";
                lblMsgTH.Text = "Cancel complete." + ErrCancel;
                pl_cust.Enabled = false;
                PopupMsg.ShowOnPageLoad = true;
                fn_Clear();
                txt_card_no.Text = "";
                return;
            }


        }
        catch (Exception ex)
        {
            lblMsg.Text = "Save note not success ";
            lblMsg.Visible = true;
            CallMasterEnt._dataCenter.RollbackMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            return;
        }

    }
    protected void B_cancelnote_Click(object sender, EventArgs e)
    {
        E_note_Cancel.Text = "";
        D_action.SelectedIndex = 0;
        D_reason.SelectedIndex = 0;
        P_note_cancel.ShowOnPageLoad = false;
    }




    #region ---   new function ---
    private bool saveCancel(string ActionCode, string ResultCode, string ncb, string errDesc, ref string ErrMsg, ref string appNo, string step = "")
    {
        bool success = true;
        try
        {
            string cmdUpdateCSMS13 = "";

            ErrMsg = "";
            ilObj = new ILDataCenter();
            userInfo = userInfoService.GetUserInfo();
            ILDataCenterMssqlUsingCard CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            ILDataCenterMssql CallMasterEnt = new ILDataCenterMssql(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();

            DataSet ds_cus = CallEntUsingCard.getCustInfoByCard(txt_card_no.Text.Trim().Replace("-", "")).Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds_cus))
            {
                DataRow dr_cust = ds_cus.Tables?[0].Rows?[0];
                iDB2Command cmd = new iDB2Command();

                // get Application No.
                int affectedRows;
                string appno_prm = "";
                string ErrorMsg = "";
                string Error = "";
                string AppDate97 = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);

                if (hid_App.Value.ToString().Trim() == "")
                {
                    bool call_ILSR01 = iLDataSubroutine.Call_ILSR01(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref appno_prm, ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
                    //  ilObj.CloseConnectioDAL();


                    if (Error.ToString().Trim() == "Y")
                    {
                        //L_message.Text = ErrorMsg.ToString();
                        ErrMsg = ErrorMsg.Trim();
                        //   ilObj.CloseConnectioDAL();
                        return false;
                    }

                    //   ilObj.CommitDAL();
                    //   ilObj.CloseConnectioDAL();
                }
                else
                {
                    appno_prm = hid_App.Value.ToString().Trim();

                }

                string[] appDate_S = txt_appDate.Text.Trim().Split('/');

                string appDate_ = "";
                if (appDate_S.Count() == 3)
                {
                    appDate_ = appDate_S[2] + appDate_S[1] + appDate_S[0];
                }
                else
                {
                    appDate_ = appDate_S[0].Substring(4, 4) + appDate_S[0].Substring(2, 2) + appDate_S[0].Substring(0, 2);
                }


                //cmd.Parameters.Clear();
                cmdUpdateCSMS13 = @"insert into AS400DB01.CSOD0001.CSMS13 " +
                                      "(m13app, m13csn, m13brn, m13apn, m13apt, m13apv, m13sex, m13mtl, m13bdt, m13upg, m13udt, m13utm, m13usr, m13wks) " +
                                      "Values(" +
                                      "'IL', " +
                                      dr_cust["CSN"].ToString().Trim() + ", " +
                                      userInfo.BranchApp.ToString() + ", " +
                                      appno_prm + "," +
                                      "'02', " +
                                      "'1' , " +
                                      "'" + dr_cust["Gender"].ToString().Trim() + "'," +
                                      "'" + dr_cust["m11mob"].ToString().Trim() + "'," +
                                      dr_cust["BirthDate"].ToString().Trim() + ", " +
                                      "'WEBILUSING', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'" + userInfo.Username + "'," +
                                      "'" + userInfo.LocalClient + "') ";

                affectedRows = -1;

                try
                {
                    //if (dataCenter.SqlCon.State != ConnectionState.Open)
                    //{
                    //    dataCenter.SqlCon.Open();
                    //    dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
                    //}
                    bool transaction = CallMasterEnt._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res = CallMasterEnt._dataCenter.Execute(cmdUpdateCSMS13, CommandType.Text, transaction).Result;
                    affectedRows = res.afrows;
                    if (affectedRows < 0)
                    {
                        Task.Run(() =>
                        {
                            Utility.WriteLogString(res.message.ToString(), cmdUpdateCSMS13);
                        });
                        ErrMsg = " Cancel not complete.";
                        success = false;
                        goto commit_rollback;
                    }
                }
                catch (Exception ex)
                {
                    Task.Run(() => { Utility.WriteLog(ex); });
                    ErrMsg = " Cancel not complete. ";
                    success = false;
                    goto commit_rollback;
                }

                //Insert ilms01
                cmd.Parameters.Clear();
                string cmdIL01 = "";
                if (step == "")
                {
                    cmdIL01 = "Insert into AS400DB01.ILOD0001.ilms01 " +
                                      "(p1brn, p1apno, p1ltyp, p1appt, p1apvs, p1apdt, p1aprj, p1csno, " +
                                      "p1loca, p1resn, p1kusr, p1kdte, p1ktim, p1stdt, p1sttm, p1crcd, p1avdt, p1avtm,P1FILL, " +
                                      "p1updt, p1uptm, p1upus, p1prog, p1wsid) " +
                                      "Values(" +
                                      userInfo.BranchApp.ToString() + ", " +
                                      appno_prm.Trim() + "," +
                                      " '02'," +
                                      " '02'," +
                                      " '1' ," +
                                      appDate_ + "," +
                                      " 'CN', " +  //  Reject Result
                                       dr_cust["CSN"].ToString().Trim() + ", " +
                                      "'150'" + "," +
                                      "'" + ResultCode + "', " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'                    " + ncb + "   " + "'," +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", '" +
                                      userInfo.Username + "'," +
                                      " 'WEBILUSING', " +
                                      "'" + userInfo.LocalClient + "') ";

                }
                else if (step == "PRODUCT")
                {
                    string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
                    string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
                    string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
                    cmdIL01 = "Insert into AS400DB01.ILOD0001.ILMS01 " +
                                      "(p1brn, p1apno, p1ltyp, p1appt, p1apvs, p1apdt, p1aprj, p1csno, " +
                                      " p1loca, p1resn, p1kusr, p1kdte, p1ktim, p1stdt, p1sttm, p1crcd, p1avdt, p1avtm,P1FILL, " +
                                      " p1term, p1vdid, p1camp, p1item, " +
                                      "p1updt, p1uptm, p1upus, p1prog, p1wsid) " +
                                      "Values(" +
                                      userInfo.BranchApp.ToString() + ", " +
                                      appno_prm.Trim() + "," +
                                      " '02'," +
                                      " '02'," +
                                      " '1' ," +
                                      appDate_ + "," +
                                      " 'CN', " +  //  Reject Result
                                       dr_cust["CSN"].ToString().Trim() + ", " +
                                      "'150'" + "," +
                                      "'" + ResultCode + "', " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'                    " + ncb + "   " + "'," +
                                      txt_totalterm.Text.Trim() + " , " +
                                      vendor[0] + " , " +
                                      campaign[0] + " , " +
                                      product[0] + " , " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", '" +
                                      userInfo.Username + "'," +
                                      " 'WEBILUSING', " +
                                      "'" + userInfo.LocalClient + "') ";
                }
                affectedRows = -1;
                try
                {
                    bool transaction = CallMasterEnt._dataCenter.Sqltr?.Connection == null ? true : false;
                    var res = CallMasterEnt._dataCenter.Execute(cmdIL01, CommandType.Text, transaction).Result;
                    affectedRows = res.afrows;
                    if (affectedRows < 0)
                    {

                        success = false;
                        goto commit_rollback;
                    }
                    // affectedRows = ilObj.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {

                    ErrMsg = " Cancel not complete..";
                    success = false;
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {

                    ErrMsg = " Cancel not complete..";
                    success = false;
                    goto commit_rollback;
                }



                if (ActionCode.Trim() == "")
                {
                    ActionCode = "Add";
                }
                Connect_NoteAPI noteAPI = new Connect_NoteAPI();

                cmd.CommandText = "";
                cmd.Parameters.Clear();


                String SNote = "";
                if (errDesc.Length <= 100)
                {
                    SNote = errDesc;
                }
                else if (errDesc.Length > 100)
                {
                    SNote = errDesc.Substring(0, 100);

                }
                try
                {
                    var resNote = noteAPI.AddNote(dr_cust["CSN"].ToString().Trim(), "0", ActionCode, ResultCode, errDesc, AppDate97, m_UpdTime.ToString().Trim()).Result;
                    affectedRows = Convert.ToInt32(resNote.success);
                    if (affectedRows < 0)
                    {
                        success = false;
                        goto commit_rollback;
                    }
                }
                catch (Exception ex)
                {

                    ErrMsg = " Cancel not complete..(Note)";
                    success = false;
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {

                    ErrMsg = " Cancel not complete..(Note)";
                    success = false;
                    goto commit_rollback;
                }

                appNo = appno_prm;
                success = true;
                goto commit_rollback;

            }
            else
            {
                ErrMsg = " Cancel not complete : Not found customer. ";
                success = false;
                goto commit_rollback;
            }
        commit_rollback:
            if (success)
            {
                CallMasterEnt._dataCenter.CommitMssql();
                CallMasterEnt._dataCenter.CloseConnectSQL();
                return true;
            }
            else
            {
                CallMasterEnt._dataCenter.RollbackMssql();
                CallMasterEnt._dataCenter.CloseConnectSQL();
                return false;
            }

        }
        catch (Exception ex)
        {
            CallMasterEnt._dataCenter.RollbackMssql();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            ErrMsg = " Cancel not complete  : " + ex.Message.Trim();
            return false;
        }
    }
    private bool saveReject(string rejCode, string ncb, string errDesc, ref string ErrMsg, ref string appno_prm, string step = "")
    {
        try
        {

            ErrMsg = "";
            ilObj = new ILDataCenter();
            userInfo = userInfoService.GetUserInfo();
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            int affectedRows = 0;
            DataSet ds_cus = CallEntUsingCard.getCustInfoByCard(txt_card_no.Text.Trim().Replace("-", "")).Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds_cus))
            {
                DataRow dr_cust = ds_cus.Tables?[0].Rows?[0];
                iDB2Command cmd = new iDB2Command();

                // get Application No.

                appno_prm = "";
                string ErrorMsg = "";
                string Error = "";
                string AppDate97 = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);

                if (hid_App.Value.ToString().Trim() == "")
                {
                    bool call_ILSR01 = iLDataSubroutine.Call_ILSR01(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref appno_prm, ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
                    //  ilObj.CloseConnectioDAL();


                    if (Error.ToString().Trim() == "Y")
                    {
                        //L_message.Text = ErrorMsg.ToString();
                        ErrMsg = ErrorMsg.Trim();
                        //   ilObj.CloseConnectioDAL();
                        return false;
                    }

                    //   ilObj.CommitDAL();
                    //   ilObj.CloseConnectioDAL();
                }
                else
                {
                    appno_prm = hid_App.Value.ToString().Trim();
                }



                string[] appDate_S = txt_appDate.Text.Trim().Split('/');

                string appDate_ = "";
                if (appDate_S.Count() == 3)
                {
                    appDate_ = appDate_S[2] + appDate_S[1] + appDate_S[0];
                }
                else
                {
                    appDate_ = appDate_S[0].Substring(4, 4) + appDate_S[0].Substring(2, 2) + appDate_S[0].Substring(0, 2);
                }
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into AS400DB01.CSOD0001.CSMS13 " +
                                  "(m13app, m13csn, m13brn, m13apn, m13apt, m13apv, m13sex, m13mtl, m13bdt, m13upg, m13udt, m13utm, m13usr, m13wks) " +
                                  "Values(" +
                                  "'IL', " +
                                  dr_cust["CSN"].ToString().Trim() + ", " +
                                  userInfo.BranchApp.ToString() + ", " +
                                  appno_prm + "," +
                                  "'02', " +
                                  "'1' , " +
                                  "'" + dr_cust["Gender"].ToString().Trim() + "'," +
                                  "'" + dr_cust["m11mob"].ToString().Trim() + "'," +
                                  dr_cust["BirthDate"].ToString().Trim() + ", " +
                                  "'WEBILUSING', " +
                                  AppDate97 + ", " +
                                  m_UpdTime + ", " +
                                  "'" + userInfo.Username + "'," +
                                  "'" + userInfo.LocalClient + "') ";


                try
                {
                    //if (dataCenter.SqlCon.State != ConnectionState.Open)
                    //{
                    //    dataCenter.SqlCon.Open();
                    //    dataCenter.Sqltr = dataCenter.SqlCon.BeginTransaction();
                    //}
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    var resCS13 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                    if (resCS13.afrows == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Task.Run(() =>
                    {
                        Utility.WriteLogString(ex.ToString(), cmd.CommandText.ToString());
                    });
                    ErrMsg = " Reject not complete.CSMS13";
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return false;
                }



                //Insert ilms01
                cmd.Parameters.Clear();

                if (step == "")
                {
                    cmd.CommandText = @"Insert into AS400DB01.ILOD0001.ILMS01 " +
                                      "(p1brn, p1apno, p1ltyp, p1appt, p1apvs, p1apdt, p1aprj, p1csno, " +
                                      "p1loca, p1resn, p1kusr, p1kdte, p1ktim, p1stdt, p1sttm, p1crcd, p1avdt, p1avtm,P1FILL, " +
                                      "p1updt, p1uptm, p1upus, p1prog, p1wsid) " +
                                      "Values(" +
                                      userInfo.BranchApp.ToString() + ", " +
                                      appno_prm.Trim() + "," +
                                      " '02'," +
                                      " '02'," +
                                      " '1' ," +
                                      appDate_ + "," +
                                      " 'RJ', " +  //  Reject Result
                                       dr_cust["CSN"].ToString().Trim() + ", " +
                                      "'210'" + "," +
                                      "'" + rejCode + "', " + // reason  wait code
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'                    " + ncb + "   " + "'," +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", '" +
                                      userInfo.Username + "'," +
                                      " 'WEBILUSING', " +
                                      "'" + userInfo.LocalClient + "') ";

                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    var resILMS01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                    if (resILMS01.afrows == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        return false;
                    }

                }
                else if (step == "PRODUCT")
                {
                    string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
                    string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
                    string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
                    cmd.CommandText = "Insert into AS400DB01.ILOD0001.ILMS01 " +
                                      "(p1brn, p1apno, p1ltyp, p1appt, p1apvs, p1apdt, p1aprj, p1csno, " +
                                      " p1loca, p1resn, p1kusr, p1kdte, p1ktim, p1stdt, p1sttm, p1crcd, p1avdt, p1avtm,P1FILL, " +
                                      " p1term, p1vdid, p1camp, p1item, " +
                                      " p1updt, p1uptm, p1upus, p1prog, p1wsid) " +
                                      " Values(" +
                                      userInfo.BranchApp.ToString() + ", " +
                                      appno_prm.Trim() + "," +
                                      " '02'," +
                                      " '02'," +
                                      " '1' ," +
                                      appDate_ + "," +
                                      " 'RJ', " +  //  Reject Result
                                       dr_cust["CSN"].ToString().Trim() + ", " +
                                      "'210'" + "," +
                                      "'" + rejCode + "', " + // reason  wait code
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'" + userInfo.Username + "', " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", " +
                                      "'                    " + ncb + "   " + "'," +
                                      txt_totalterm.Text.Trim() + " , " +
                                      vendor[0] + " , " +
                                      campaign[0] + " , " +
                                      product[0] + " , " +
                                      AppDate97 + ", " +
                                      m_UpdTime + ", '" +
                                      userInfo.Username + "'," +
                                      " 'WEBILUSING', " +
                                      "'" + userInfo.LocalClient + "') ";

                }

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    var resIL01 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                    if (resIL01.afrows == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                        return false;
                    }
                    // affectedRows = ilObj.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {

                    ErrMsg = " Reject not complete..";
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return false;
                }


                cmd.Parameters.Clear();
                Connect_NoteAPI noteAPI = new Connect_NoteAPI();


                //cmd.CommandText = "";
                //cmd.Parameters.Clear();
                //cmd.CommandText = "Insert into ilms38 (P38CSN,P38CNT,P38ACD,P38RCD,P38DE1,P38DAT,P38TIM,P38USR) " +
                //          "Values (" +
                //          dr_cust["CSN"].ToString().Trim() + "," +
                //          "0 ," +
                //          "'ADD'," +
                //          "'" + rejCode + "', " +
                //          "@P38DE1," +
                //          AppDate97 + ", " +
                //          m_UpdTime + "," +
                //          "'" + userInfo.Username + "') ";
                //cmd.Parameters.Add("@P38DE1", errDesc);
                affectedRows = -1;
                try
                {
                    //affectedRows = ilObj.ExecuteNonQuery(cmd);
                    var resNote = noteAPI.AddNote(dr_cust["CSN"].ToString().Trim(), "0", "ADD", rejCode, errDesc, AppDate97, m_UpdTime.ToString().Trim()).Result;
                    affectedRows = Convert.ToInt32(resNote.success);

                }
                catch (Exception ex)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    ErrMsg = " Reject not complete..(Note)";
                    return false;
                }
                if (affectedRows < 0)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    ErrMsg = " Reject not complete..(Note)";
                    return false;
                }


                dataCenter.CommitMssql();
                dataCenter.CloseConnectSQL();
                return true;

            }
            else
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                ErrMsg = " Reject not complete : Not found customer. ";

                return false;
            }


        }
        catch (Exception ex)
        {
            dataCenter.RollbackMssql();
            dataCenter.CloseConnectSQL();
            ErrMsg = " Reject not complete  : " + ex.Message.Trim();
            return false;
        }
    }

    //****     Req : NCB ****//
    private void checkID_NCB()
    {
        userInfo = userInfoService.GetUserInfo();
        CallEntUsingCard = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        Connect_NcbAPI ncbAPI = new Connect_NcbAPI(userInfoService.GetUserInfo());
        ilObj = new ILDataCenter();
        ilObj.UserInfomation = userInfoService.GetUserInfo();
        //******* Check NCB **********//

        string ErrorCode = "";
        //string bureau  = "";
        string reason = "";
        string aprj = "";
        string Error = "";
        string rsts = "";
        string err = "";


        //DataSet ds_cus = CallEntUsingCard.getCustInfoByCard(txt_card_no.Text.Trim().Replace("-", "")).Result;
        //CallEntUsingCard._dataCenter.CloseConnectSQL();
        //if (!ilObj.check_dataset(ds_cus))
        //{
        //    err = "cannot find customer";
        //    lblMsgEN.Text = err;
        //    hid_Confirm.Value = "";
        //    hid_cont.Value = "";
        //    //hid_App.Value = "";
        //    PopupMsg.ShowOnPageLoad = true;

        //    return;
        //}

        //DataRow dr_cust = ds_cus.Tables?[0].Rows?[0];
        string appNo = "";
        string ilsr01_Err = "";

        if (hid_App.Value.Trim() == "")
        {
            bool res_ilsr01 = iLDataSubroutine.Call_ILSR01(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref appNo, ref ilsr01_Err, ref err, userInfo.BizInit, userInfo.BranchNo);


            if (!res_ilsr01 || ilsr01_Err == "Y")
            {
                //  ilObj.RollbackDAL();
                //  ilObj.CloseConnectioDAL();
                lblMsgEN.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                hid_App.Value = "";
                return;
            }
            hid_App.Value = appNo;

        }


        string idcard = txt_idNo.Text;
        string birthDate = hid_birthDate.Value?.ToString().Trim();
        string name = hidName.Value?.ToString().Trim();
        string surName = hidSurname.Value?.ToString().Trim();
        string CSN = lb_csn.Text.ToString();
        //****  Check ID Card &&  Auto Cancel ****//
        string typeErr = "";
        string errMsg = "";
        bool res_VerID = verifyIDNumber(idcard, birthDate, name, surName, ref typeErr);
        if (!res_VerID)
        {
            string ErrRj = "";
            string appno = "";
            if (typeErr.Trim() != "")
            {
                //***** get Note ******//
                DataSet ds_note = CallEntUsingCard.getResultCodeWithCode(typeErr).Result;
                CallEntUsingCard._dataCenter.CloseConnectSQL();
                string NoteDesc = "" + "";
                if (ds_note != null)
                {
                    if (ds_note.Tables?[0].Rows.Count > 0)
                    {
                        NoteDesc = ds_note.Tables?[0].Rows?[0]["G25DES"].ToString();
                    }
                }
                else
                {
                    lblMsgEN.Text = "";
                    lblMsgTH.Text = "";
                    lblMsgTH.Text = "Get note for Auto Reject not success ";
                    pl_cust.Enabled = false;
                    PopupMsg.ShowOnPageLoad = true;

                    return;
                }


                // *** Auto Reject ID ***// 
                bool resRej = saveReject(typeErr, " ", NoteDesc, ref ErrRj, ref appno, "PRODUCT");

                if (!resRej)
                {
                    lblMsgEN.Text = "";
                    lblMsgTH.Text = "";
                    lblMsgTH.Text = ErrRj;
                    pl_cust.Enabled = false;
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                else
                {
                    errMsg = typeErr + " : " + NoteDesc;
                    lblMsgEN.Text = "Application No. " + appno;
                    lblMsgTH.Text = "";
                    lblMsgTH.Text = "Auto Reject complete." + errMsg;
                    pl_cust.Enabled = false;
                    PopupMsg.ShowOnPageLoad = true;
                    fn_Clear();
                    txt_card_no.Text = "";
                    return;
                }
            }
            else
            {
                // **  Show error message popup **//
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = errMsg;
                pl_cust.Enabled = false;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
        }
        //*************** Check NCB ***************//


        string bureau = "";
        string crreview = "";
        var resBureau = ncbAPI.CheckNCBGateway("IL", userInfo.BranchApp.ToString().Trim(), hid_App.Value, idcard, CSN,
                                                              name + " " + surName,
                                                              birthDate, "", userInfo.Username.Trim()).Result;
        if (resBureau.success)
        {
            var jsonData = JsonConvert.SerializeObject(resBureau.data);
            var resData = (JObject)JsonConvert.DeserializeObject(jsonData, typeof(JObject));
            bureau = resData["resultNCB"].ToString().Trim();
            crreview = resData["resultNCBCreditReview"].ToString().Trim();
        }


        //bool call_GNSR69 = ilObj.Call_GNSR69("IL", userInfo.BranchApp.ToString().Trim(), hid_App.Value, dr_cust["IDCard"].ToString().Trim(), dr_cust["CSN"].ToString().Trim(), dr_cust["Name"].ToString().Trim() + " " + dr_cust["Surname"].ToString().Trim(),
        //                                     dr_cust["BirthDate"].ToString().Trim(), "", ref bureau, ref crreview, ref ErrorCode, ref Error, userInfo.BizInit, userInfo.BranchNo);


        //ilObj.CloseConnectioDAL();

        //Pact 20170717 message credit model
        hid_CRMD.Value = "";
        hid_CRRW.Value = "";
        //bureau = "N";
        if (bureau.Trim() == "")
        {
            //*** return Err ***//
            lblMsgEN.Text = "Check NBC Error : " + Error;
            PopupMsg.ShowOnPageLoad = true;
            hid_Confirm.Value = "";
            hid_cont.Value = "";
            //hid_App.Value = "";
            return;
        }


        if ((bureau == "N") || (bureau == "P") || (bureau == "C") || (bureau == "D") || (bureau.Trim() == "T"))
        {
            aprj = "";
        }

        if ((bureau.Trim() == "B") || (bureau.Trim() == "R") ||
            (bureau.Trim() == "O") || (bureau.Trim() == "S"))
        {
            aprj = "RJ";
            reason = "IL3";
            if (bureau == "B")
            {
                rsts = "B";
                reason = "BL3";
            }
            if (bureau == "S")
            {
                reason = "SL24";
            }
        }
        if (bureau == "L")
        {
            aprj = "RJ";
            reason = "IL9";
        }

        // *** get note description ***//
        string Msg = "";
        DataSet DS_note = new DataSet(); ;
        string Res_note = @"select G101CD, G101ED from AS400DB01.GNOD0000.gntb101 WITH (NOLOCK) where g101cd = '" + bureau + "'";
        var resFound_Note = dataCenter.GetDataset<DataTable>(Res_note, CommandType.Text).Result;
        dataCenter.CloseConnectSQL();
        if (resFound_Note.success)
        {
            DS_note = resFound_Note.data;
        }

        if (DS_note != null && DS_note.Tables?.Count > 0)
        {
            foreach (DataRow dr in DS_note.Tables?[0].Rows)
            {
                Msg = "Result from Credit Bureau = " + reason + "-" + dr["G101ED"].ToString().Trim();
                lblCreditBureau.Text = Msg;
            }
        }
        // Retrive History
        // Pact 20170714 check credit review
        // if Credit Bureau >>> Pass >> Check Credit Review
        if (aprj == "")
        {

            //crreview = "Y";
            hid_CRRW.Value = crreview;

            string outSTS = "", outMSG = "", outFLG = "";
            bool CALL_CSSR36 = iLDataSubroutine.CALL_CSSR36(lb_csn.Text.Trim(), ref outSTS, ref outMSG, ref outFLG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
            // ilObj.CloseConnectioDAL();

            if (outSTS != "Y" && outFLG == "Y")
            {
                crreview = "";
                hid_CRRW.Value = " ";
                lblCreditReview.Text = "";
            }

            if (crreview == "N")
            {
                aprj = "RJ";
                reason = "CR01";
            }

            if (crreview == "E")
            {
                lblCreditReview.Text = "Credit Review : Not Found";
            }
            else if (crreview == "Y")
            {
                lblCreditReview.Text = "Credit Review : Pass";
            }
            else if (crreview == "N")
            {
                lblCreditReview.Text = "Credit Review : Not Pass";
            }
            else
            {
                hid_CRRW.Value = " ";
                lblCreditReview.Text = "";
            }
        }

        string NoteDesc_ILMS38 = "";
        DataSet DS = CallEntUsingCard.getResultCodeWithCode(reason).Result;
        CallEntUsingCard._dataCenter.CloseConnectSQL();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables?[0].Rows)
            {
                NoteDesc_ILMS38 = dr["g25des"].ToString().Trim();
            }
        }

        //  ilObj.CloseConnectioDAL();
        // Pact 20170717 message reject  
        hid_RejectEN.Value = "";
        hid_RejectTH.Value = "";
        hid_RejectCK.Value = aprj;
        hid_bureau.Value = "";
        if (aprj == "RJ")
        {
            hid_bureau.Value = bureau;
            if (bureau == "B")
            {

                string Error_GNSRBLC = "";
                string ErrorMsg_GNSRBLC = "";

                //ilObj.Connect400();
                bool call_GNSRBLC = iLDataSubroutine.Call_GNSRBLC(idcard, txt_card_no.Text.Trim().Replace("-", ""), "IL", "005", "15",
                                                        ref Error_GNSRBLC, ref ErrorMsg_GNSRBLC, userInfo.BizInit, userInfo.BranchNo);
                if (!call_GNSRBLC)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    lblMsgTH.Text = "";
                    lblMsgEN.Text = "Reject NBC Error: (BLACKLIST-FALSE) " + ErrorMsg_GNSRBLC;
                    PopupMsg.ShowOnPageLoad = true;
                    hid_Confirm.Value = "";
                    hid_cont.Value = "";
                    hd_ncb.Value = "";
                    return;
                }
                else if (Error_GNSRBLC.Trim() == "Y")
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    lblMsgTH.Text = "";
                    lblMsgEN.Text = "Reject NBC Error: (BLACKLIST-Y) ";
                    PopupMsg.ShowOnPageLoad = true;
                    hid_Confirm.Value = "";
                    hid_cont.Value = "";
                    hd_ncb.Value = "";
                    return;

                }

                else if (ErrorMsg_GNSRBLC.ToString().Trim() != "")
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    lblMsgTH.Text = "";
                    lblMsgEN.Text = "Reject NBC Error: (BLACKLIST-ERROR-MSG) " + ErrorMsg_GNSRBLC;
                    PopupMsg.ShowOnPageLoad = true;
                    hid_Confirm.Value = "";
                    hid_cont.Value = "";
                    hd_ncb.Value = "";
                    return;

                }
            }

            // Insert  13 


            // *** Save Data  ***//
            string AppDate97 = hid_date_97.Value.Substring(4, 4) + hid_date_97.Value.Substring(2, 2) + hid_date_97.Value.Substring(0, 2);
            int affectedRows;
            string ErrMsg = "";
            iDB2Command cmd = new iDB2Command();
            cmd.Parameters.Clear();
            string resCS13 = "";
            resCS13 = @"insert into AS400DB01.CSOD0001.CSMS13 " +
                              "(m13app, m13csn, m13brn, m13apn, m13apt, m13apv, m13sex, m13mtl, m13bdt, m13upg, m13udt, m13utm, m13usr, m13wks) " +
                              "Values(" +
                              "'IL', " +
                              CSN + ", " +
                              userInfo.BranchApp.ToString() + ", " +
                              hid_App.Value + "," +
                              "'02', " +
                              "'1' , " +
                              "'" + hidGender.Value + "'," +
                              "'" + hidMobile.Value + "'," +
                              birthDate + ", " +
                              "'WEBILUSING', " +
                              AppDate97 + ", " +
                              m_UpdTime + ", " +
                              "'" + userInfo.Username + "'," +
                              "'" + userInfo.LocalClient + "') ";
            affectedRows = -1;
            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                var res = dataCenter.Execute(resCS13, CommandType.Text, transaction).Result;
                affectedRows = res.afrows;
                if (affectedRows < 0)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                }
                //  affectedRows = ilObj.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                lblMsgTH.Text = "";
                ErrMsg = "  Auto Reject (NCB) : not complete.";
                lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                //hid_App.Value = "";
                hd_ncb.Value = "";
                //L_message.Text = "Error on Insert CSMS13";
                return;
            }
            if (affectedRows < 0)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                lblMsgTH.Text = "";
                ErrMsg = " Auto Reject (NCB) : not complete.";
                lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                //hid_App.Value = "";
                hd_ncb.Value = "";
                return;

            }


            //Insert ilms01

            string[] appDate_S = txt_appDate.Text.Trim().Split('/');

            string appDate_ = "";
            if (appDate_S.Count() == 3)
            {
                appDate_ = appDate_S[2] + appDate_S[1] + appDate_S[0];
            }
            else
            {
                appDate_ = appDate_S[0].Substring(4, 4) + appDate_S[0].Substring(2, 2) + appDate_S[0].Substring(0, 2);
            }


            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
            cmd.Parameters.Clear();

            string resILMS01 = "";
            resILMS01 = "Insert into AS400DB01.ILOD0001.ilms01 " +
                              "(p1brn, p1apno, p1ltyp, p1appt, p1apvs, p1apdt, p1aprj, p1csno, " +
                              " p1loca, p1resn, p1kusr, p1kdte, p1ktim, p1stdt, p1sttm, p1crcd, p1avdt, p1avtm,P1FILL, " +
                              " p1term, p1vdid, p1camp, p1item, " +
                              " p1updt, p1uptm, p1upus, p1prog, p1wsid ) " +
                              " Values(" +
                              userInfo.BranchApp.ToString() + ", " +
                              hid_App.Value.Trim() + "," +
                              " '02'," +
                              " '02'," +
                              " '1' ," +
                              appDate_ + ", " +
                              " 'RJ', " +  //  Reject Result
                               CSN + ", " +
                              "'210'" + "," +
                              "'" + reason + "', " + // reason  wait code
                              "'" + userInfo.Username + "', " +
                              AppDate97 + ", " +
                              m_UpdTime + ", " +
                              AppDate97 + ", " +
                              m_UpdTime + ", " +
                              "'" + userInfo.Username + "', " +
                              AppDate97 + ", " +
                              m_UpdTime + ", " +
                              "'                    " + bureau.ToUpper() + "   " + "'," +
                              txt_totalterm.Text.Trim() + " , " +
                              vendor[0] + " , " +
                              campaign[0] + " , " +
                              product[0] + " , " +
                              AppDate97 + ", " +
                              m_UpdTime + ", '" +
                              userInfo.Username + "'," +
                              " 'WEBILUSING', " +
                              "'" + userInfo.LocalClient + "') ";




            affectedRows = -1;
            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                var res01 = dataCenter.Execute(resILMS01, CommandType.Text, transaction).Result;
                affectedRows = res01.afrows;
                if (affectedRows < 0)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                }
                // affectedRows = ilObj.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

                ErrMsg = " Auto Reject (NCB) not complete.";
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }
            if (affectedRows < 0)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                lblMsgTH.Text = "";
                ErrMsg = " Auto Reject (NCB)  not complete..";
                lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                //hid_App.Value = "";
                hd_ncb.Value = "";

                return;
            }

            cmd.Parameters.Clear();

            Connect_NoteAPI noteAPI = new Connect_NoteAPI();
            affectedRows = -1;
            string SNote = "";
            try
            {
                if (reason == "CR01")
                {
                    SNote = NoteDesc_ILMS38;
                }
                else
                {
                    SNote = Msg;
                }
                var resNote = noteAPI.AddNote(CSN, "0", "ADD", reason, SNote, AppDate97, m_UpdTime.ToString().Trim()).Result;
                affectedRows = Convert.ToInt32(resNote.success);
                if (affectedRows < 0)
                {
                    lblMsgTH.Text = "";
                    ErrMsg = " Auto Reject (NCB)  not complete..(Note)";
                    lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                    PopupMsg.ShowOnPageLoad = true;
                    hid_Confirm.Value = "";
                    hid_cont.Value = "";
                    //hid_App.Value = "";
                    hd_ncb.Value = "";

                    return;
                }
            }
            catch (Exception ex)
            {
                lblMsgTH.Text = "";
                ErrMsg = " Auto Reject (NCB)  not complete.(Note)";
                lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                //hid_App.Value = "";
                hd_ncb.Value = "";
                return;
            }

            try
            {
                if (reason == "CR01")
                {

                    if (txt_telM.Text.Trim() != "")
                    {
                        Connect_SmsAPI smsAPI = new Connect_SmsAPI();
                        ILSRSMS iLSRSMS = new ILSRSMS();
                        string TextSms = string.Concat(txt_telM.Text.Trim(), "#", WebConfigurationManager.AppSettings["RJCR01"].ToString().Trim(), "");
                        var resSMS = smsAPI.SendSMS(TextSms, txt_telM.Text.Trim());
                        string successFlag = resSMS.success ? "Y" : "N";
                        string sql_Update = $@"UPDATE AS400DB01.ILOD0001.ILMS01
                                            SET P1FILL = STUFF(STUFF(P1FILL + ' ', 22, 2, '{iLSRSMS.CheckSmsType("83")}') + ' ', 24, 1, '{successFlag}')
                                            WHERE P1BRN = {userInfo.BranchApp.ToString().Trim()} AND P1APNO = {hid_App.Value.Trim()} ";
                        bool transaction = dataCenter.Sqltr == null ? true : false;
                        affectedRows = dataCenter.Execute(sql_Update, CommandType.Text, transaction).Result.afrows;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsgTH.Text = "";
                ErrMsg = " Auto Reject (NCB)  not complete.(SMS)";
                lblMsgEN.Text = "Reject NBC Error: " + ErrMsg;
                PopupMsg.ShowOnPageLoad = true;
                hid_Confirm.Value = "";
                hid_cont.Value = "";
                //hid_App.Value = "";
                hd_ncb.Value = "";
                return;
            }

            dataCenter.CommitMssql();
            dataCenter.CloseConnectSQL();


            lblMsgEN.Text = "Application No. " + hid_App.Value;
            lblMsgTH.Text = "Auto reject complete.(NCB)" + Msg;
            //Pact 20170717 
            if (reason == "CR01")
            {
                lblMsgTH.Text = "Auto reject complete.(NCB) Credit Review : CR01 - Not Pass";
            }

            hid_RejectEN.Value = lblMsgEN.Text;
            hid_RejectTH.Value = lblMsgTH.Text;

            hid_Confirm.Value = "CRMD";
            lblConfirmMsgEN.Text = " Credit Model Complete ? ";
            lblCreditBureau.Text = Msg;
            PopupConfirmSave.ShowOnPageLoad = true;

            //PopupMsg.ShowOnPageLoad = true;
            //fn_Clear();
            //hid_Confirm.Value = "";
            //hid_cont.Value = "";
            //hid_App.Value = "";
            //hd_ncb.Value = "";
            //txt_card_no.Text = "";
            return;


        }
        else if (bureau == "C")
        {    // ******   Auto Cancel when  check NCB and cannot check ***********//
            hd_ncb.Value = bureau;
            string ErrCancel = "";
            string appno = "";
            string actionCode = "Add";
            string resultCode = "SL37";


            DataSet ds_note = CallEntUsingCard.getResultCodeWithCode(resultCode).Result;
            CallEntUsingCard._dataCenter.CloseConnectSQL();
            string NoteDesc = "";
            if (ds_note != null)
            {
                if (ds_note.Tables[0].Rows.Count > 0)
                {
                    NoteDesc = ds_note.Tables?[0].Rows?[0]["G25DES"].ToString().Trim();
                }
            }
            else
            {
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = "Get note for Auto Cancel not success ";
                pl_cust.Enabled = false;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }



            bool resCC = saveCancel(actionCode, resultCode, hd_ncb.Value.Trim().ToUpper(), NoteDesc, ref ErrCancel, ref appno, "PRODUCT");
            if (!resCC)
            {
                lblMsgEN.Text = "";
                lblMsgTH.Text = "";
                lblMsgTH.Text = ErrCancel;
                pl_cust.Enabled = false;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            else
            {

                lblMsgEN.Text = "Application No. " + appno;
                lblMsgTH.Text = "";
                lblMsgTH.Text = "Auto Cancel complete." + NoteDesc;

                //Pact 20170717 
                hid_RejectEN.Value = lblMsgEN.Text;
                hid_RejectTH.Value = lblMsgTH.Text;

                hid_Confirm.Value = "CRMD";
                lblConfirmMsgEN.Text = " Credit Model Complete ? ";
                lblCreditBureau.Text = lblMsgEN.Text;
                PopupConfirmSave.ShowOnPageLoad = true;
                //pl_cust.Enabled = false;
                //PopupMsg.ShowOnPageLoad = true;
                //fn_Clear();
                //hid_App.Value = "";
                //txt_card_no.Text = "";
                return;
            }

        }
        else  //   Can check NCB & not Auto Reject  
        {

            hd_ncb.Value = bureau;
            //lb_resNCB.Text = bureau + " : " + Msg;
            //btn_check_ncb.Enabled = false;
            //btn_cal_TCL.Enabled = true;
            //btn_cancel_case.Enabled = true;
            //lblMsgTH.Text = "";            
            //PopupMsg.ShowOnPageLoad = true;
            //Pact 20170717     
            lblMsgTH.Text = "";
            lblMsgEN.Text = bureau + " : " + Msg;

            hid_Confirm.Value = "CRMD";
            lblConfirmMsgEN.Text = " Credit Model Complete ? ";
            lblCreditBureau.Text = Msg;
            PopupConfirmSave.ShowOnPageLoad = true;
            return;

        }

    }

    private bool verifyIDNumber(string IDNo, string birthDay, string thaiName, string thaiSurname, ref string ErrCode)
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            ILDataCenter busobj = new ILDataCenter();
            busobj.UserInfomation = userInfoService.GetUserInfo();

            //Check Auto Reject Call RLSRAUR            
            string WPRJAC = "", WPRJCD = "", WPRJDS = "";
            string Error = "";
            bool call_RLSRAUR = iLDataSubroutine.Call_RLSRAUR(IDNo.Trim(), birthDay.Trim(), thaiName.Trim(), thaiSurname.Trim(),
                                                    "", "", "", "", "", "", "", ref WPRJAC, ref WPRJCD, ref WPRJDS, ref Error, "IL",
                                                    userInfo.BizInit, userInfo.BranchNo);
            // busobj.CloseConnectioDAL();
            //**  Check ID **//
            if ((WPRJAC.ToString().Trim() == "I1") & (WPRJCD.ToString().Trim() == "LL5"))
            {
                ErrCode = WPRJCD.ToString().Trim();
                return false;
                //G_aprj.Text = "RJ";
                //G_loca.Text = "210";
                //G_reason.Text = WPRJCD.ToString().Trim();
            }
            if ((WPRJAC.ToString().Trim() == "I2") & (WPRJCD.ToString().Trim() == "SL35"))
            {
                ErrCode = WPRJCD.ToString().Trim();
                return false;
                //G_aprj.Text = "RJ";
                //G_loca.Text = "210";
                //G_reason.Text = WPRJCD.ToString().Trim();
            }
            //*****************************//
            return true;

        }
        catch (Exception ex)
        {
            ErrCode = "";
            return false;
        }
    }

    private void bindNoteCancel()
    {
        try
        {
            userInfo = userInfoService.GetUserInfo();
            E_note_Cancel.Text = "";
            ILDataCenter ilObj = new ILDataCenter();
            ILDataCenterMssqlInterview iLDataCenterMssql = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
            DataSet ds_resCode = iLDataCenterMssql.getResultCode().Result;
            DataSet ds_ActCode = iLDataCenterMssql.getActionCode().Result;
            if (!ilObj.check_dataset(ds_resCode) || !ilObj.check_dataset(ds_ActCode))
            {
                return;
            }

            D_action.Items.Add("--- Select ---", "");
            foreach (DataRow dr in ds_ActCode.Tables?[0].Rows)
            {

                D_action.Items.Add(
                    new ListEditItem(dr["G24ACD"].ToString().Trim() + " : " + dr["G24DES"].ToString().Trim(), dr["G24ACD"].ToString().Trim()));
            }

            D_action.Value = "ADD";

            D_reason.Items.Add("--- Select ---", "");
            foreach (DataRow dr in ds_resCode.Tables?[0].Rows)
            {

                D_reason.Items.Add(
                    new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
            }
            D_reason.Value = "";

            P_note_cancel.ShowOnPageLoad = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }


    #endregion



}