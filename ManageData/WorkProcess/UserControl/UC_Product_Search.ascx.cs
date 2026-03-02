using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.Commons;
using ESB.WebAppl.ILSystem.commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ILSystem.App_Code.BLL.DataCenter.ILDataCenterMssql;
using EB_Service.Commons;

public partial class ManageData_WorkProcess_UserControl_UC_Product_Search : System.Web.UI.UserControl
{
    private ILDataCenter ilObj;
   // Connect_GeneralAPI conn_general;
    ILDataSubroutine iLDataSubroutine;
    ILDataCenterMssql CallHisunMaster;
    ILDataCenterMssqlProduct iLDataCenterMssqlProduct;
    ILDataCenterMssqlInterview CallHisunCustomer;
    //public delegate void ManageData_WorkProcess_WebUserControlDelegate(DataSet ds_term , DataSet ds_installment);
    public delegate void ManageData_WorkProcess_UserControl_UC_Product_Search_CallDelegate(string data);
    public event ManageData_WorkProcess_UserControl_UC_Product_Search_CallDelegate changeTabToTCL;

    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    private UserInfo userInfo;
    public UserInfoService userInfoService;
    public ManageData_WorkProcess_UserControl_UC_Product_Search()
    {
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

    public string IDNO
    {
        get
        {
            if (ViewState["IDNO"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["IDNO"]);
            }
        }
        set
        {
            ViewState["IDNO"] = value;
            if (ViewState["IDNO"].ToString() != "")
            {
                hid_idno.Value = (string)(ViewState["IDNO"]);
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
    public string AppDate
    {
        get
        {
            if (ViewState["Appdate"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["Appdate"]);
            }
        }
        set
        {
            ViewState["Appdate"] = value;
            if (ViewState["Appdate"].ToString() != "")
            {
                hid_appdate.Value = (string)(ViewState["Appdate"]);
            }
        }

    }

    public string birthdate
    {
        get
        {
            if (ViewState["birthdate"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["birthdate"]);
            }
        }
        set
        {
            ViewState["birthdate"] = value;
            if (ViewState["birthdate"].ToString() != "")
            {
                hid_birthdate.Value = (string)(ViewState["birthdate"]);
            }
        }
    }

    public string date97
    {
        get
        {
            if (ViewState["date97"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["date97"]);
            }
        }
        set
        {
            ViewState["date97"] = value;
            if (ViewState["date97"].ToString() != "")
            {
                hid_date97.Value = (string)(ViewState["date97"]);
            }
        }

    }

    public string loantyp
    {
        get
        {
            if (ViewState["loantyp"] == null)
                return string.Empty;
            else
            {
                return (string)(ViewState["loantyp"]);
            }
        }
        set
        {
            ViewState["loantyp"] = value;
            if (ViewState["loantyp"].ToString() != "")
            {
                hid_loantyp.Value = (string)(ViewState["loantyp"]);
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

        if (IsPostBack)
        {
            return;
        }
    }

    public void bindData()
    {
        try
        {
            if ((hid_AppNo.Value == null) || (hid_AppNo.Value.ToString() == ""))
            {
                return;
            }

            bindPaymentType();
            /*
            bindTerm();
            bindVendor();
            */

            ILDataCenter busobj = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            busobj.UserInfomation = userInfoService.GetUserInfo(); ;
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(userInfoService.GetUserInfo());

            DataSet DS = new DataSet();

            DS = MSSQL.GetDataset<DataSet>($@" select distinct CAST(p1vdid as float) as p1vdid, p1term, p10ven, p10nam, p10fi1, p16rnk, p1camp, p1pric, p1qty, p1down, 
                                         c01cmp, c01cnm, c02csq, c02rsq, t44itm, t44des, t44pgp, t40des, c07min, c07max,
                                         p1payt, p1pbcd, p1pbrn, p1paty, p1pano,
                                         C01CTY, c02rsq, c01nxd, C02AIR, C02ACR, p1aprj, P1LTYP
                                         from AS400DB01.ILOD0001.ilms01
                                         left join AS400DB01.ILOD0001.ilms10 on (p1vdid = p10ven)
                                         left join AS400DB01.ILOD0001.ilms16 on (p10VEN = P16VEN and Cast(p16std as nvarchar) <= '{hid_appdate.Value}'  and Cast(p16end as nvarchar) >= '{hid_appdate.Value}')
                                         left join AS400DB01.ILOD0001.ilcp01 on(p1camp = c01cmp)
                                         left join AS400DB01.ILOD0001.ilcp02 on(c01cmp = c02cmp and c02tot = p1term)
                                         left join AS400DB01.ILOD0001.ilcp07 on(c02cmp = c07cmp and c02csq = c07csq and (c07pit = p1item or c07pit = 0)) 
                                         left join AS400DB01.ILOD0001.iltb44 on(p1item = t44itm)
                                         left join AS400DB01.ILOD0001.iltb40 on(t44typ = t40typ)
                                         where p1brn= '{hid_brn.Value}'  and p1apno = '{hid_AppNo.Value}'  ", CommandType.Text).Result.data;

            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                dd_totalterm.Text = dr["p1term"].ToString().Trim();
                bindVendor();
                dd_vendor.Value = dr["p1vdid"].ToString().Trim() + "|" + dr["P16RNK"].ToString().Trim();
                txt_rank_v.Text = dr["P16RNK"].ToString().Trim();
                try
                {
                    hid_p1aprj.Value = dr["p1aprj"].ToString().Trim();
                    dd_paymentType.Value = dr["p1payt"].ToString().Trim();
                    load_paymenttype();
                    if (dr["p1payt"].ToString().Trim() == "")
                    {
                        dd_paymentType.SelectedIndex = 2;
                    }

                    dd_bankCode.Value = dr["p1pbcd"].ToString().Trim();
                    bindBankBranch(dr["p1pbcd"].ToString().Trim());
                    dd_bankBranch.Value = dr["p1pbrn"].ToString().Trim();
                    dd_accountType.Value = dr["p1paty"].ToString().Trim();
                    txt_AccountNo.Text = dr["p1pano"].ToString().Trim();
                    if (dr["p1pbcd"].ToString().Trim() != "")
                    {
                        P_payment.Visible = true;
                    }
                    else
                    {
                        P_payment.Visible = false;
                    }
                    //load_paymenttype();
                }
                catch { }


                dd_campaign.Items.Add(new ListEditItem(dr["c01cmp"].ToString().Trim() + ":" + dr["c01cnm"].ToString().Trim(),
                                                       dr["c01cmp"].ToString().Trim() + "|" + dr["c02csq"].ToString().Trim() + "|" + dr["c02rsq"].ToString().Trim()));
                dd_campaign.SelectedIndex = 1;
                txt_campSeq.Text = dr["c02csq"].ToString().Trim();
                txt_campgType.Text = dr["c01cty"].ToString().Trim();
                txt_total_range.Text = dr["c02rsq"].ToString().Trim();
                txt_non.Text = dr["c01nxd"].ToString().Trim();
                txt_Int.Text = float.Parse(dr["c02air"].ToString().Trim()).ToString("0.00");
                txt_cru.Text = float.Parse(dr["c02acr"].ToString().Trim()).ToString("0.00");

                dd_product.Items.Add(new ListEditItem(dr["t44itm"].ToString().Trim() + " : " + dr["t44des"].ToString().Trim(),
                                                      dr["t44itm"].ToString().Trim() + "|" + dr["c07min"].ToString().Trim() + "|" +
                                                      dr["c07max"].ToString().Trim() + "|" + dr["T44PGP"].ToString().Trim()));
                dd_product.SelectedIndex = 1;

                string[] productSel = dd_product.Value.ToString().Split('|');
                txt_minPrice.Text = productSel[1];
                txt_maxPrice.Text = productSel[2];

                txt_price.Text = dr["p1pric"].ToString().Trim();
                txt_qty.Text = dr["p1qty"].ToString().Trim();
                txt_down.Text = dr["p1down"].ToString().Trim();

                if (Convert.ToDecimal(txt_price.Text) > 0)
                {
                    btn_cal_TCL_Click(null, null);
                }
            }

            MSSQL.CloseConnectSQL();

            disable_key_in();
            btn_cal_TCL.Enabled = false;
            btn_keyin.Enabled = true;
            btn_cal_TCL.Enabled = false;

        }
        catch { }
    }

    private void bindPaymentType()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getPaymentType();
            dd_paymentType.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_paymentType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_paymentType.Items.Add(
                        new ListEditItem(dr["gt48tc"].ToString().Trim() + " : " + dr["gt48td"].ToString().Trim(), dr["gt48tc"].ToString().Trim()));
                }
                dd_paymentType.SelectedIndex = 2;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void dd_paymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_paymenttype();
    }

    void load_paymenttype()
    {
        if (dd_paymentType.SelectedItem.Value.ToString().Trim() == "1")
        {
            P_payment.Visible = true;
            bindDebitAccount(hid_CSN.Value);
            dd_bankCode.Enabled = true;
            dd_bankBranch.Enabled = true;
            dd_accountType.Enabled = true;
        }
        else
        {
            P_payment.Visible = false;
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

    //*** get customer account **//
    private void bindDebitAccount(string csn)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            dd_bankCode.Items.Clear();
            dd_accountType.Items.Clear();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            //***  bankcode ***//
            DataSet ds_bankCode = iLDataSubroutine.getBankCode();
            if (ilObj.check_dataset(ds_bankCode))
            {
                dd_bankCode.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds_bankCode.Tables[0].Rows)
                {
                    dd_bankCode.Items.Add(
                        new ListEditItem(dr["g32bnk"].ToString().Trim() + " : " + dr["gnb30c"].ToString().Trim(), dr["g32bnk"].ToString().Trim()));
                }
                dd_bankCode.SelectedIndex = -1;
            }

            DataSet ds_accountType = iLDataSubroutine.getAccountType();
            if (ilObj.check_dataset(ds_accountType))
            {
                dd_accountType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds_accountType.Tables[0].Rows)
                {
                    dd_accountType.Items.Add(
                        new ListEditItem(dr["gn13cd"].ToString().Trim() + " : " + dr["gn13td"].ToString().Trim(), dr["gn13cd"].ToString().Trim()));
                }
                dd_accountType.SelectedIndex = -1;
            }


            DataSet ds_debit = iLDataSubroutine.getDebitAccountByCSN(csn);
            if (ilObj.check_dataset(ds_debit))
            {
                DataRow dr_debit = ds_debit.Tables[0].Rows[0];
                bindBankBranch(dr_debit["gnb31a"].ToString().Trim());
                dd_bankCode.SelectedItem.Value = dr_debit["gnb31a"].ToString().Trim();
                dd_bankBranch.SelectedItem.Value = dr_debit["gnb31c"].ToString().Trim();
                dd_accountType.SelectedItem.Value = dr_debit["gn13cd"].ToString().Trim();
                txt_AccountNo.Text = dr_debit["p00bac"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void bindBankBranch(string bankCode)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getBankBranch(bankCode);
            dd_bankBranch.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_bankBranch.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_bankBranch.Items.Add(
                        new ListEditItem(dr["gnb31c"].ToString().Trim() + " : " + dr["gnb31d"].ToString().Trim(), dr["gnb31c"].ToString().Trim()));
                }
                dd_bankBranch.SelectedIndex = -1;
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void dd_bankCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBankBranch(dd_bankCode.SelectedItem.Value.ToString().Trim());
        }
        catch (Exception ex)
        {
        }
    }

    private void bindTerm()
    {
        /*
        int i = 0;
        dd_totalterm.Items.Clear();
        dd_totalterm.Items.Add("");
        for (i = 1; i <= 60; i++)
        {
            dd_totalterm.Items.Add(new ListEditItem(i.ToString(), i.ToString()));
        }
        dd_totalterm.Value = "12";
        */
    }

    private void bindVendor()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            ilObj = new ILDataCenter();
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            DataSet ds = iLDataSubroutine.getVendor("", userInfo.BranchApp);

            dd_vendor.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_vendor.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_vendor.Items.Add(
                        new ListEditItem(dr["p10ven"].ToString().Trim() + ":" + dr["p10nam"].ToString().Trim() + "[" + dr["p10fi1"].ToString().Trim() + "]", dr["p10ven"].ToString().Trim() + "|" + dr["P16RNK"].ToString().Trim()));
                }
            }
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
        }
        dd_vendor.Focus();
    }

    //***  bind Campaign ***//
    private void bindCampaign()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            string[] vendor = dd_vendor.Value.ToString().Split('|');
            DataSet ds = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.Text.Trim(), "", "", "", hid_appdate.Value.Trim());
            dd_campaign.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_campaign.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_campaign.Items.Add(
                        new ListEditItem(dr["c01cmp"].ToString().Trim() + ":" + dr["c01cnm"].ToString().Trim(), dr["c01cmp"].ToString().Trim() + "|" + dr["c02csq"].ToString().Trim() + "|" + dr["c02rsq"].ToString().Trim()));
                }
            }
            else
            {
                dd_vendor.Text = "";
            }
            dd_product.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    //***  bind Product ***//
    private void bindProduct(string vendorCode, string campCode, string campSeq, string product = "")
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            ILDataCenterMssqlKeyinStep1 ilProduct = new ILDataCenterMssqlKeyinStep1(userInfoService.GetUserInfo());
            DataSet ds = ilProduct.getProduct_<DataSet>(vendorCode, campCode, campSeq, product).Result;
            dd_product.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                int countProd = ds.Tables[0].Rows.Count;
                if (countProd <= 3000)
                {
                    dd_product.Items.Add("--- Select ---", "");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dd_product.Items.Add(
                            new ListEditItem(dr["t44itm"].ToString().Trim() + " : " + dr["t44des"].ToString().Trim(),
                                dr["t44itm"].ToString().Trim() + "|" +
                                dr["c07min"].ToString().Trim() + "|" +
                                dr["c07max"].ToString().Trim() + "|" +
                                dr["T44PGP"].ToString().Trim()
                                ));
                    }
                    lb_prodcount.Text = "Macthing " + countProd.ToString() + " items";
                }
                else
                {
                    lb_prodcount.Text = "ระบุ Product Code หรือ Product Name เสร็จแล้วกดปุ่ม [Enter] ";
                }
            }
            else
            {
                lb_prodcount.Text = "Macthing 0 " + " item";
            }
        }
        catch (Exception ex)
        {
            lb_prodcount.Text = "Macthing 0 " + " item";
        }
    }

    protected void dd_totalterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_vendor.Text == "")
            {
                dd_vendor.Text = "";
                bindVendor();
            }
            else
            {
                dd_campaign.Text = "";
                bindCampaign();
                dd_product.SelectedItem.Text = "";
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
            dd_campaign.Text = "";
            dd_product.Text = "";
            dd_campaign.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    protected void dd_campaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');
            string[] vendor = dd_vendor.SelectedItem.Value.ToString().Split('|');
            DataSet ds = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], campaign[2], hid_appdate.Value);
            if (ilObj.check_dataset(ds))
            {

                txt_campSeq.Text = campaign[1];
                DataRow dr = ds.Tables[0].Rows[0];
                txt_campgType.Text = dr["C01CTY"].ToString().Trim();
                txt_total_range.Text = dr["c02rsq"].ToString().Trim();
                txt_non.Text = dr["c01nxd"].ToString().Trim();

                txt_Int.Text = float.Parse(dr["C02AIR"].ToString().Trim()).ToString("0.00");
                txt_cru.Text = float.Parse(dr["C02ACR"].ToString().Trim()).ToString("0.00");

                dd_product.Items.Clear();
                dd_product.Text = "";
                bindProduct(vendor[0], campaign[0], campaign[1]);
                dd_product.Focus();
            }

        }
        catch (Exception ex)
        {
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
                //lb_prodcount.Text = "ระบุ Product Code หรือ Product Name ตั้งแต่ 3-15 ตัวอักษร เสร็จแล้วกดปุ่ม [Enter] ";
                return;
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
            L_error_product.Text = "";
            string err = "";
            calTCL1();

            DataSet ds = new DataSet();
            if (calculateInstallment(ref err, ref ds))
            {
                if (ilObj.check_dataset(ds))
                {
                    gvTerm.DataSource = ds.Tables[0];
                    gvTerm.DataBind();

                    gv_install.DataSource = ds.Tables[1];
                    gv_install.DataBind();

                    if (ds.Tables[2] != null)
                    {
                        DataRow dr_detail = ds.Tables[2].Rows[0];
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
                }
            }
            else
            {
                L_message.Text = "Error Cal ..!!";
                P_message_product.ShowOnPageLoad = true;

                return;
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);

            L_message.Text = "Error Cal ..!!" + ex.ToString();
            P_message_product.ShowOnPageLoad = true;
            return;
        }
    }

    //***  calculate Installment ***//
    private bool calculateInstallment(ref string err, ref DataSet ds)
    {
        try
        {   //***  clear value ***//
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
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            float loanRequest = 0;
            try
            {
                float approveAVL = float.Parse(E_Apv_avi.Text, NumberStyles.Currency);
            }
            catch { }

            //*** check condition ***//
            if (!checkBeforeCaculateProduct("", ref err))
            {
                L_error_product.Text = err.ToString();
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
            float crAVL = E_Apv_avi.Text.Trim() == "" ? 0 : float.Parse(E_Apv_avi.Text.Trim(), NumberStyles.Currency);

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
            string salary = float.Parse(EdtNetIncome.Text.Trim(), NumberStyles.Currency).ToString();
            string appDateUser = hid_appdate.Value.ToString();
            string birthDate = hid_birthdate.Value.ToString();
            string[] vendor = dd_vendor.Value.ToString().Split('|');

            //string POBOTL = "";
            //string PONOAP = "";
            //string POCSBL = "";
            //string POCRAV = "";
            //string POAPAM = "";
            //string POEBCL = "";
            //string POAVAP = "";
            //string POSTS = "";
            //string POOLDC = "";
            //string POVENR = "";
            //string POPERV = "";
            //string POEBCS = "";
            //string POERR = "";
            //string PERMSG = "";
            //ilObj.Call_GNSR87("IL", hid_CSN.Value.ToString(), userInfo.BranchApp.ToString(), "0", appDateUser.ToString(), birthDate,
            //                  "1", "N", hid_salary.Value.ToString(), EdtBOTLoan.Text.Trim(), loanRequest.ToString(), 
            //                  vendor[0], mode, "",
            //                  ref POBOTL, ref PONOAP, ref POCSBL, ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, //14,15,16,17,18,19,20,21,22
            //                  ref POVENR, ref POPERV, ref POEBCS, ref POERR, ref PERMSG, //23,24,25,26,27
            //                  userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

            //ilObj.CloseConnectioDAL();
            //if (POERR.Trim() == "Y")
            //{
            //    err = PERMSG.Trim();
            //    return false;
            //}
            //else
            //{
            //    //txt_total_crt.Text = convCurrency(dr["m3crlm"].ToString());
            //    //txt_ebcLimit.Text = convCurrency(POEBCL);
            //    //txt_cust_bln.Text = convCurrency(POCSBL);
            //    //txt_tcl.Text = convCurrency(POAVAP);
            //    //txt_app_lm.Text = convCurrency(POAPAM);
            //    //txt_bot_loan.Text = convCurrency(POBOTL);
            //    //EdtBOTLoan.Text = convCurrency((float.Parse(POBOTL) - float.Parse(POCSBL)).ToString());
            //    //EdtNetIncome.Text = convCurrency(salary);
            //    //lb_pApproveL.Text = convCurrency(POAPAM);
            //    //E_Apv_avi.Text = convCurrency(POAVAP);
            //}

            //DataSet ds_vendor = ilObj.getVendor(vendor[0]);
            //if (!ilObj.check_dataset(ds_vendor))
            //{
            //    err = "Cannot find vendor ";
            //    return false;
            //}
            //DataRow dr_vendor = ds_vendor.Tables[0].Rows[0];
            //if (!(dr_vendor["p10spc"].ToString().Trim() == "F" || dr_vendor["p10spc"].ToString().Trim() == "Y"))
            //{
            //    EdtBOTLoan.Text = convCurrency(POAVAP);
            //}

            //**********************************************************//

            if (!((loanRequest > 0) && (loanRequest <= Total_pur)))
            {
                L_message.Text = "Loan Request ต้องไม่มากกว่า Total Purchase";
                P_message_product.ShowOnPageLoad = true;
                return false;
                //L_error_product.Text = " Loan Request ต้องไม่มากกว่า Total Purchase ";
                //return false;
            }

            /*
            if (hid_status.Value == "KESSAI")
            {
                if (loanRequest > crAVL)
                {
                    L_message.Text = "Loan request ต้องไม่มากกว่า Credit Available";
                    P_message_product.ShowOnPageLoad = true;
                    return false;
                }
            }
            */
            if (((loanRequest / qty) < minProc) && (minProc > 0))
            {
                string err_loan = "";


                if (txt_minPrice.Text.Trim() == "" || txt_minPrice.Text.Trim() == "0.00")
                {
                    err_loan = txt_minPrice.Text;
                }
                else
                {
                    err_loan = (decimal.Parse(txt_minPrice.Text) * decimal.Parse(txt_qty.Text)).ToString();
                }


                L_message.Text = " loan request จะต้องไม่น้อยกว่า  " + err_loan;
                P_message_product.ShowOnPageLoad = true;
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
                L_message.Text = " loan request จะต้องไม่มากกว่า " + err_loan;
                P_message_product.ShowOnPageLoad = true;
                //err = " loan request จะต้องไม่มากกว่า " + txt_maxPrice.Text;
                return false;
            }
            //if (((loanRequest / qty) < minProc) && (minProc > 0))
            //{

            //    L_message.Text = "loan request จะต้องไม่น้อยกว่า " + txt_minPrice.Text;
            //    P_message_product.ShowOnPageLoad = true;
            //    return false;
            //}
            //if (((loanRequest / qty) > maxProc) && (maxProc > 0))
            //{
            //    L_message.Text = "loan request จะต้องไม่มากกว่า " + txt_maxPrice.Text;
            //    P_message_product.ShowOnPageLoad = true;
            //    return false;
            //}

            string AppDate = hid_appdate.Value.ToString();
            if (AppDate.Trim() == "")
            {
                L_message.Text = "cannot find current date";
                P_message_product.ShowOnPageLoad = true;
                return false;
            }
            
            DataSet ds_ilTB06 = CallHisunMaster.getILTB06();
            if (!ilObj.check_dataset(ds_ilTB06))
            {
                L_message.Text = "cannot find data in ILTB06";
                P_message_product.ShowOnPageLoad = true;
                return false;
            }
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], "", hid_appdate.Value);
            if (!ilObj.check_dataset(ds_camp))
            {
                L_message.Text = "cannot find campaign";
                P_message_product.ShowOnPageLoad = true;
                return false;
            }

            //***** compound string *****//
            DataRow dr_iltb06 = ds_ilTB06.Tables[0].Rows[0];
            DataRow dr_campaign = ds_camp.Tables[0].Rows[0];

            string compound = txt_loanReq.Text.Trim().Replace(".", "").PadLeft(13, '0') + " " +
                              appDateUser.Substring(6, 2).ToString() + appDateUser.Substring(4, 2).ToString() + appDateUser.Substring(0, 4).ToString() + " " +
                              dd_totalterm.Text.Trim().PadLeft(3, '0') + " " +
                              txt_total_range.Text.Trim().PadLeft(2, '0') + " " +
                              txt_non.Text.Trim().PadLeft(2, '0') + " " +
                              (float.Parse(dr_campaign["c01fin"].ToString().Trim()) + 1).ToString().Replace(".", "").PadLeft(2, '0') + " " +
                              dr_campaign["c02ifr"].ToString().Trim().Replace(".", "").PadLeft(5, '0') + " " +
                              dr_iltb06["t06lon"].ToString().Trim().Replace(".", "").PadLeft(4, '0') + " " +
                              dr_iltb06["t06Dut"].ToString().Trim().Replace(".", "").PadLeft(2, '0') + " " +
                              appDateUser.Substring(6, 2).ToString() + appDateUser.Substring(4, 2).ToString() + appDateUser.Substring(0, 4).ToString() + " ";

            string PITEXT = compound;
            string PITERM = "";
            string PIINTR = "";
            string PICRUR = "";

            foreach (DataRow dr in ds_camp.Tables[0].Rows)
            {

                //float intrate = float.Parse(dr["c02inr"].ToString().Trim());
                //float crurate = float.Parse(dr["c02crr"].ToString().Trim());
                string intrate = dr["c02inr"].ToString().Trim();
                string crurate = dr["c02crr"].ToString().Trim();
                int c02tot = int.Parse(dr["c02tot"].ToString().Trim());
                int c02fmt = int.Parse(dr["c02fmt"].ToString().Trim());


                PITERM = PITERM + (1000 + (c02tot - c02fmt) + 1).ToString().Substring(1, 3);

                //if ((intrate > 0) && (dr["c02inr"].ToString().Trim().Length > 1))  
                //05/10/2558//if ((intrate > 0) && (float.Parse(dr["c02inr"].ToString()).ToString().Length > 1))
                if ((Convert.ToDouble(intrate) > 0) && (dr["c02inr"].ToString().Length > 1))
                {
                    PIINTR = PIINTR + (intrate.ToString().Replace(".", "")).PadLeft(5, '0');  //(1000 + intrate).ToString().Substring(1, 3) + (100.00 + intrate).ToString().Substring(4, 2);
                }
                //else if ((intrate < 0) && (dr["c02inr"].ToString().Trim().Length > 1))
                //05102558//else if ((intrate < 0) && (float.Parse(dr["c02inr"].ToString()).ToString().Length > 1))
                else if ((Convert.ToDouble(intrate) < 0) && (dr["c02inr"].ToString().Length > 1))
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


                //05/10/2558//if ((crurate != 0) && (dr["c02crr"].ToString().Trim().Length > 1))
                if ((Convert.ToDouble(crurate) != 0) && (dr["c02crr"].ToString().Trim().Length > 1))
                {
                    //05/10/2558//PICRUR = PICRUR + (1000 + crurate).ToString("0.00").Substring(1, 3) + (100.00 + crurate).ToString("0.00").Substring(4, 2);
                    PICRUR = PICRUR + (1000 + Convert.ToDouble(crurate)).ToString("0.00").Substring(1, 3) + (100.00 + Convert.ToDouble(crurate)).ToString("0.00").Substring(4, 2);

                }
                //05/10/2558// else if ((crurate != 0) && (crurate == 1))
                else if ((Convert.ToDouble(crurate) != 0) && (Convert.ToDouble(crurate) == 1))
                {
                    //05/10/2558//PICRUR = PICRUR + (1000 + crurate).ToString("0.00").Substring(2, 3);
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

            bool res24 = false;
            bool oldcal = false;
            double _intrate = 0; double _crurate = 0;
            _intrate = double.TryParse(PIINTR, out _intrate) ? _intrate : 0;
            _crurate = double.TryParse(PICRUR, out _crurate) ? _crurate : 0;
            oldcal = (hid_loantyp.Value == "01" || (_intrate == 0 && _crurate == 0));
            if (oldcal)
            {
                res24 = iLDataSubroutine.Call_ILSYD24D(PITEXT, PITERM, PIINTR, PICRUR,
                            ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                            ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                            ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                            ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                            ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                            ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                            ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //res24 = ilObj.Call_ILSYD24D(PITEXT, PITERM, PIINTR, PICRUR,
                //            ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                //            ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                //            ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                //            ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                //            ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                //            ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                //            ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //ilObj.CloseConnectioDAL();
            }
            else
            {
                //res24 = ilObj.Call_ILSREIR(PITEXT, PITERM, PIINTR, PICRUR,
                //               ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                //               ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                //               ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                //               ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                //               ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                //               ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                //               ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //ilObj.CloseConnectioDAL();
                res24 = iLDataSubroutine.Call_ILSREIR(PITEXT, PITERM, PIINTR, PICRUR,
                               ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                               ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                               ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                               ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                               ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                               ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                               ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());

            }


            if (res24)
            {

                string Bureau = "0.00";
                string BureauFirst = "0.00";// Modify By Azode: 20170601


                //***  ncb ***//

                if (hid_appdate.Value.Trim() != "")
                {
                    if (hid_date97.Value.Trim() == "")
                    {
                        return false;
                    }
                    //** NCBPJ REQ 70667 **  
                    //DataSet ds_ncb = ilObj.getNCB(AppDate.PadLeft(8, '0').Substring(4, 4) + AppDate.PadLeft(8, '0').Substring(2, 2) + AppDate.PadLeft(8, '0').Substring(0, 2));
                    //DataSet ds_ncb = ilObj.getNCB(hid_date97.Value);
                    //if (ds_ncb.Tables[0].Rows.Count > 1)
                    //{
                    //    DataSet ds_ilms01 = ilObj.get_ilms01_setTimeReceive(hid_AppNo.Value, hid_brn.Value);
                    //    string cust_ncb = ds_ilms01.Tables[0].Rows[0]["p1fill"].ToString().Substring(20, 1);
                    //    //DataRow dr_ncb = ds_ncb.Tables[0].Select(" substring(G00FIL,0,1) =  '" + cust_ncb+"'");


                    //    foreach (DataRow dr in ds_ncb.Tables[0].Rows)
                    //    {
                    //        BureauFirst = ds_ncb.Tables[0].Rows[0]["G00AMT"].ToString();// Modify By Azode: 20170601
                    //        if (dr["G00FIL"].ToString().Substring(2, 1).Trim() != "")
                    //        {
                    //            if (cust_ncb == dr["G00FIL"].ToString().Substring(2, 1))
                    //            {
                    //                Bureau = dr["G00AMT"].ToString();
                    //                break;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Bureau = dr["G00AMT"].ToString();
                    //            Bureau = "0.00";
                    //        }
                    //    }
                    //}
                    //else if (ds_ncb.Tables[0].Rows.Count == 1)
                    //{
                    //    DataRow dr_ncb = ds_ncb.Tables[0].Rows[ds_ncb.Tables[0].Rows.Count - 1];
                    //    Bureau = dr_ncb["G00AMT"].ToString();
                    //    BureauFirst = dr_ncb["G00AMT"].ToString();// Modify By Azode: 20170601
                    //}

                }
                else
                {
                    return false;
                }


                //*** compute data into grid ***//
                int row = 1;
                DataTable dt = new DataTable("Term");
                DataTable dt_installment = new DataTable("Installment");
                DataTable dt_data = new DataTable("Data_use");

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
                int _term = int.Parse(dd_totalterm.Text.Trim());
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
                    install = convCurrency(decimal.Parse(POINST.Substring(len_10, 7)) + "." + "00").ToString();

                    if (row == 1)
                    {
                        //dt.Rows.Add(from_t, to_t, int_p, int_amt, cru_p, cru_amt, int_free, total_amt, oth, principal, installFirst);
                        // old code installFirst = decimal.Parse(POINST.Substring(len_10, 7)) + decimal.Parse(PODUTY) + decimal.Parse(Bureau);
                        if (oldcal)
                        {
                            installFirst = (decimal.Parse(POINSD.Substring(len_3, 7)) - decimal.Parse(BureauFirst)) + decimal.Parse(Bureau); // Modify By Azode: 20170601                       
                        }
                        else
                        {
                            installFirst = ((decimal.Parse(POINSD.Substring(len_3, 7)) / 100) - decimal.Parse(BureauFirst)) + decimal.Parse(Bureau); // Modify By Azode: 20170601                       
                        }

                    }
                    //else
                    //{
                    //    dt.Rows.Add(from_t, to_t, int_p, int_amt, cru_p, cru_amt, int_free, total_amt, oth, principal, install);
                    //}
                    //double tot_rate = (double.Parse(int_p) + double.Parse(cru_p)) / 100;
                    //double nint_rate = 0;
                    //double ncru_rate = 0;
                    //double ntot_rate = ilObj.EIR_ConvertRate(tot_rate, double.Parse(loanRequest.ToString()), _term);
                    //nint_rate = ilObj.EIR_ConvertRate((double.Parse(int_p) / 100), double.Parse(loanRequest.ToString()), _term);
                    //ncru_rate = ntot_rate - nint_rate;
                    //inteir = nint_rate.ToString();
                    //crueir = ncru_rate.ToString();
                    inteir = int_amt.ToString();
                    crueir = cru_amt.ToString();
                    if (hid_loantyp.Value == "02")
                    {
                        cru_amt = "0";
                        int_amt = "0";
                    }
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
                             PODUTY, POINTB, POCRUB, POINFB, POCONA, POFDAT.Substring(6, 2) + "/" + POFDAT.Substring(4, 2) + "/" + POFDAT.Substring(1, 4),
                             POAINR, POACRU, "0.00");

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
                    //princ_next = float.Parse(princ);
                    if (row == 1)
                    {
                        installment = convCurrency(installFirst.ToString("0.0"));
                    }
                    else
                    {
                        //installment = convCurrency((decimal.Parse(POINSD.Substring(len_3, 7))/100).ToString());
                        if (oldcal)
                        {
                            installment = convCurrency(POINSD.Substring(len_3, 7));
                        }
                        else
                        {
                            installment = convCurrency(POINSD.Substring(len_3, 5) + "." + POINSD.Substring(len_3 + 5, 2));
                        }
                    }
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

                //dt_data.Rows.Add(duty, inst, fdueDate, contAmt, fdue_NCBS, loanRequest_, Bureau, convCurrency(Total_pur.ToString()));
                ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt_installment);
                ds.Tables.Add(dt_data);
                return true;

            }
            else
            {
                //L_error_product.Text = "ไม่สามารถ คำนวณค่าได้.....";
                L_message.Text = "ไม่สามารถ คำนวณค่าได้.....";
                P_message_product.ShowOnPageLoad = true;
                return false;
            }



        }
        catch (Exception ex)
        {
            L_message.Text = "ไม่สามารถ คำนวณค่าได้.....";
            P_message_product.ShowOnPageLoad = true;
            //ilObj.CloseConnectioDAL();
            return false;
        }
    }



    private void disable_key_in()
    {
        dd_totalterm.Enabled = false;
        dd_vendor.Enabled = false;
        dd_campaign.Enabled = false;
        dd_product.Enabled = false;
        txt_price.Enabled = false;
        txt_qty.Enabled = false;
        txt_down.Enabled = false;
        btn_vendorScr.Enabled = false;
        //btn_keyin.Enabled = true;

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

    private bool checkBeforeCaculateProduct(string status_, ref string err)
    {
        try
        {
            if (dd_vendor.Value.ToString().Trim() == "")
            {
                err = "Please input Vendor ";
                //L_error_product.Text = "Please input Vendor ";
                //L_message.Text = "Please input Vendor";
                //P_message_product.ShowOnPageLoad = true;

                return false;
            }
            if (dd_totalterm.Text.Trim() == "")
            {
                err = "Please input total Term";
                //L_error_product.Text = "Please input total Term ";

                //L_message.Text = "Please input total Term";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (dd_campaign.SelectedItem.Value.ToString().Trim() == "")
            {
                //L_error_product.Text = "Campaign type must have value ";

                err = "Campaign type must have value ";
                //L_message.Text = "Campaign type must have value";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_campgType.Text.Trim() == "")
            {
                //L_error_product.Text = "campaign type must have value ";

                err = "Campaign type must have value";
                //L_message.Text = "Campaign type must have value";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_campSeq.Text.Trim() == "")
            {
                //L_error_product.Text = "Campaign seq must have value ";
                //L_message.Text = "Campaign seq must have value";
                //P_message_product.ShowOnPageLoad = true;

                err = "Campaign seq must have value";
                return false;
            }
            if (dd_product.SelectedItem.Value.ToString().Trim() == "")
            {
                //L_error_product.Text = "Please input product ";
                err = "Please input product ";
                //L_message.Text = "Please input product";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }

            if (Convert.ToDecimal(txt_down.Text) > Convert.ToDecimal(txt_price.Text))
            {
                err = "Please input down < price ";
                return false;
            }

            if (status_ == "SAVE")
            {
                if (txt_pay_abl.Text.Trim() == "")
                {
                    //L_error_product.Text = "Payment ability must have value ";

                    err = "Payment ability must have value ";
                    //L_message.Text = "Payment ability must have value";
                    //P_message_product.ShowOnPageLoad = true;
                    return false;
                }
            }
            if (txt_total_range.Text.Trim() == "")
            {
                //L_error_product.Text = "total range must have value ";

                err = "total range must have value ";
                //L_message.Text = "total range must have value";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_non.Text.Trim() == "")
            {
                //L_error_product.Text = "Non due must have value ";

                err = "Non due must have value";
                //L_message.Text = "Non due must have value";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_price.Text.Trim() == "" || txt_price.Text.Trim() == "0.00")
            {
                //L_error_product.Text = "Please input price ";

                err = "Please input price";
                //L_message.Text = "Please input price";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_qty.Text.Trim() == "" || txt_qty.Text.Trim() == "0")
            {
                //L_error_product.Text = "Please input quality ";

                err = "Please input quality";
                //L_message.Text = "Please input quality";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            if (txt_down.Text.Trim() == "")
            {
                //L_error_product.Text = "If Down not have value ,Please input 0.00 ";

                err = "If Down not have value ,Please input 0.00";
                //L_message.Text = "If Down not have value ,Please input 0.00";
                //P_message_product.ShowOnPageLoad = true;
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            //L_error_product.Text = "Please verify data";
            err = "Please verify data";
            //L_message.Text = "Please verify Vendor/Campaign/Product/Price ";
            //P_message_product.ShowOnPageLoad = true;
            return false;
        }
    }


    void calTCL()
    {
        ILDataCenter busobj = new ILDataCenter();
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();

        try
        {
            string SQL = "select m13csn, m13sex, 'AGE_GNP0371', m13mrt, CAST(m13ttl AS INT) as m13ttl, case when m13mtl <> '' then 1 else 0 end as m13mtl, " +
                                             "m13res, (m13lyr*12)+m13lmt as resident, (m13wky*12)+m13wkm as workyear, m13occ, m13but, m13slt, m13off, CAST(m13net as INT) as m13net, " +
                                             "m13con, m13chl, m13pos, m13emp, " + hid_brn.Value.ToString() + " as three, " + hid_date97.Value.ToString() + " as date97, '000' as one, m13apv, " +
                                             "m13hzp, m13sst, CAST(m13saj AS INT) as m13saj, m13bdt, M13CHA,M13OZP " +
                                             "from AS400DB01.CSOD0001.CSMS13 WITH (NOLOCK) " +
                                             "where m13app = 'IL' and m13csn = " + hid_CSN.Value.ToString() + " and m13brn = " + hid_brn.Value.ToString() + " " +
                                             "and m13apn = " + hid_AppNo.Value.ToString() + " ";
            L_error_product.Text = "";

            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m3csno, m3crlm, m3acam from AS400DB01.CSOD0001.CSMS03 WITH (NOLOCK) where m3csno = " + hid_CSN.Value.ToString() + " and m3del='' ", CommandType.Text).Result.data;
            G_Have_CSMS03.Text = "false";
            if (busobj.check_dataset(DS))
            {
                G_Have_CSMS03.Text = "true";
            }

            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(SQL.ToString(), CommandType.Text).Result.data;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                hid_salary.Value = dr["m13net"].ToString().Trim();
            }

            if (G_Have_CSMS03.Text == "false")
            {
                string outCSNO = "", outOPD = "", outOGNO = "", outORANK = "", outOINC = "", outOACL = "";
                string outO2GNO = "", outO2RANK = "", outO2ACL = "", outO2RNK = "", outO21ACL = "", outOTYPE = "";

                if (busobj.check_dataset(DS))
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        string in_AGE = "", error = "";

                        //string old_salary = "", date_sal = "", time_sal = "";
                        //busobj.Call_CSSR07(hid_idno.Value.ToString(), "99999999", ref old_salary, ref date_sal, ref time_sal, userInfo.BizInit, userInfo.BranchNo);
                        //busobj.CloseConnectioDAL();

                        iLDataSubroutine.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                                            ref in_AGE, ref error);
                        //busobj.CloseConnectioDAL();

                        string Array100 = dr["m13csn"].ToString().Trim().PadRight(8, '0') +
                                          dr["m13sex"].ToString().Trim().PadLeft(1, '0') +
                                          Convert.ToInt32(in_AGE.ToString()).ToString().PadLeft(3, '0') +
                                          dr["m13mrt"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13ttl"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13mtl"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13res"].ToString().Trim().PadLeft(2, '0') +
                                          dr["resident"].ToString().Trim().PadLeft(4, '0') +
                                          dr["workyear"].ToString().Trim().PadLeft(4, '0') +
                                          dr["m13occ"].ToString().Trim().PadLeft(3, '0') +
                                          dr["m13but"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13slt"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13off"].ToString().Trim().PadLeft(6, '0') +
                                          dr["m13net"].ToString().Trim().PadLeft(8, '0') +
                                          dr["m13con"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13chl"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13pos"].ToString().Trim().PadLeft(3, '0') +
                                          dr["m13emp"].ToString().Trim().PadLeft(2, '0') +
                                          dr["three"].ToString().Trim().PadLeft(3, '0') +
                                          dr["date97"].ToString().Trim().PadLeft(8, '0') +
                                          dr["one"].ToString().Trim().PadRight(3, '0') +
                                          dr["m13apv"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13hzp"].ToString().Trim().PadLeft(5, '0') +
                                          dr["m13sst"].ToString().Trim().PadLeft(2, ' ') +
                                          dr["m13saj"].ToString().Trim().PadLeft(8, '0') +
                                          dr["M13CHA"].ToString().Trim().PadLeft(3, '0') +
                                          dr["M13OZP"].ToString().Trim().PadLeft(5, '0') +
                                         "OFF";

                        iLDataSubroutine.CALL_GNSRGNOC("IL", hid_brn.Value.ToString(), hid_AppNo.Value.ToString(), Array100.ToString(),
                                              ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                              ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                              userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                        CallHisunMaster._dataCenter.CloseConnectSQL();
                    }
                    DS.Clear();
                }
                else
                {
                    //L_error_product.Text = "Pleaes verify judgment before save product";
                    L_message.Text = "Pleaes verify judgment before save product";
                    P_message_product.ShowOnPageLoad = true;
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;
                }
              
                EdtTCL.Text = "0";
                //LblStsTCL.Text = "TCL";
                LTCL.Text = "TCL(New Model)";
                EdtTCL.ReadOnly = false;
                EdtACL.Text = outOACL.ToString();
                EdtTCL.Text = outOACL.ToString();
                G_TCL_13.Text = EdtTCL.Text;
                if (G_TCL_13.Text == "")
                {
                    G_TCL_13.Text = "0";
                }
                G_ACL_13.Text = outOACL.ToString();
                if (G_ACL_13.Text == "")
                {
                    G_ACL_13.Text = "0";
                }
                if (LTCL.Text == "TCL(New Model)")
                {
                    LTCL.Text = LTCL.Text + ' ' + outOTYPE.ToString();
                }

                G_PD.Text = "";
                if (outOPD.ToString().Trim() != "")
                {
                    G_PD.Text = outOPD.ToString().Replace(".", "").Trim();
                }
                E_group.Text = outOGNO.ToString().Trim();
                E_rank.Text = outORANK.ToString().Trim();
                G_Rank_for_GNSR031.Text = outO2RANK.ToString().Trim();

                G_GROUP_ONGOING_Y.Text = outO2GNO.ToString().Trim();
                G_RANK_ONGOING_Y.Text = outO2RANK.ToString().Trim();
                G_ACL_ONGOING_Y.Text = outO2ACL.ToString().Trim();
                P4TYPE.Text = outOTYPE.ToString().Trim();
            }
            else
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    EdtTCL.Text = dr["m3crlm"].ToString().Trim();
                    //LblStsTCL.Text = "TCL(Existing)";
                    LTCL.Text = "TCL(Existing)";
                    LACL.Text = "ACL(Existing)";
                    EdtTCL.ReadOnly = true;
                    EdtACL.Text = "0";

                    if (Convert.ToDecimal(EdtTCL.Text) > (Convert.ToDecimal(hid_salary.Value.ToString()) * 5))
                    {
                        EdtTCL.Text = (((Convert.ToDecimal(hid_salary.Value.ToString()) * 5) / 100) * 100).ToString();
                    }
                    G_TCL_13.Text = EdtTCL.Text;
                    if (G_TCL_13.Text.Trim() == "")
                    {
                        G_TCL_13.Text = "0";
                    }
                    //ACL
                    // LACL.Visible := False;
                    // EdtACL.Visible := False;
                }
                CallHisunMaster._dataCenter.CloseConnectSQL();
            }
            EdtBOTLoan.Text = (Convert.ToDecimal(hid_salary.Value.ToString()) * 5).ToString();

            string outSTS = "", outMSG = "", outFLG = "";
            iLDataSubroutine.CALL_CSSR36(hid_CSN.Value.ToString(), ref outSTS, ref outMSG, ref outFLG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
            //busobj.CloseConnectioDAL();
            if (outFLG.ToString().Trim() == "Y")
            {
                G_NewModel_ZR.Text = "Y";
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(SQL.ToString(), CommandType.Text).Result.data;

                string outCSNO = "", outOPD = "", outOGNO = "", outORANK = "", outOINC = "", outOACL = "";
                string outO2GNO = "", outO2RANK = "", outO2ACL = "", outO2RNK = "", outO21ACL = "", outOTYPE = "";

                if (busobj.check_dataset(DS))
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        string in_AGE = "", error = "";
                        iLDataSubroutine.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                                            ref in_AGE, ref error);
                        CallHisunMaster._dataCenter.CloseConnectSQL();

                        string Array100 = dr["m13bdt"].ToString().Trim().PadRight(8, '0') +
                                          dr["m13sex"].ToString().Trim().PadLeft(1, '0') +
                                          in_AGE.ToString().PadRight(3, '0') +
                                          dr["m13mrt"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13ttl"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13mtl"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13res"].ToString().Trim().PadLeft(2, '0') +
                                          dr["resident"].ToString().Trim().PadRight(4, '0') +
                                          dr["workyear"].ToString().Trim().PadRight(4, '0') +
                                          dr["m13occ"].ToString().Trim().PadLeft(3, '0') +
                                          dr["m13but"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13slt"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13off"].ToString().Trim().PadRight(6, '0') +
                                          dr["m13osl"].ToString().Trim().PadRight(8, '0') +
                                          dr["m13con"].ToString().Trim().PadRight(2, '0') +
                                          dr["m13chl"].ToString().Trim().PadRight(2, '0') +
                                          dr["m13pos"].ToString().Trim().PadLeft(3, '0') +
                                          dr["m13emp"].ToString().Trim().PadLeft(2, '0') +
                                          dr["three"].ToString().Trim().PadLeft(3, '0') +
                                          dr["date97"].ToString().Trim().PadRight(8, '0') +
                                          dr["one"].ToString().Trim().PadRight(3, '0') +
                                          dr["m13apv"].ToString().Trim().PadLeft(1, '0') +
                                          dr["m13hzp"].ToString().Trim().PadLeft(5, '0') +
                                          dr["m13sst"].ToString().Trim().PadLeft(2, '0') +
                                          dr["m13saj"].ToString().Trim().PadRight(8, '0') +
                                          dr["M13CHA"].ToString().Trim().PadLeft(3, '0') +
                                          dr["M13OZP"].ToString().Trim().PadLeft(5, '0') +
                                          "OFF";
                        iLDataSubroutine.CALL_GNSRGNOC("IL", hid_brn.Value.ToString(), hid_AppNo.Value.ToString(), Array100.ToString(),
                                              ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                              ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                              userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                        //busobj.CloseConnectioDAL();
                    }
                    DS.Clear();
                }
                else
                {
                    //L_error_product.Text = "Pleaes verify judgment before save product";
                    L_message.Text = "Pleaes verify judgment before save product";
                    P_message_product.ShowOnPageLoad = true;
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;
                }

                EdtTCL.Text = "0";
                //LblStsTCL.Text = "TCL";
                LTCL.Text = "TCL(New Model ZR)";
                EdtTCL.ReadOnly = false;
                EdtACL.Text = outOACL.ToString();
                EdtTCL.Text = outOACL.ToString();
                G_TCL_13.Text = EdtTCL.Text;
                if (G_TCL_13.Text == "")
                {
                    G_TCL_13.Text = "0";
                }
                G_ACL_13.Text = outOACL.ToString();
                if (G_ACL_13.Text == "")
                {
                    G_ACL_13.Text = "0";
                }
                if (LTCL.Text == "TCL(New Model ZR)")
                {
                    LTCL.Text = LTCL.Text + ' ' + outOTYPE.ToString();
                }

                G_PD.Text = "";
                if (outOPD.ToString().Trim() != "")
                {
                    G_PD.Text = outOPD.ToString().Replace(".", "").Trim();
                }
                E_group.Text = outOGNO.ToString().Trim();
                E_rank.Text = outORANK.ToString().Trim();
                G_Rank_for_GNSR031.Text = outO2RANK.ToString().Trim();

                G_GROUP_ONGOING_Y.Text = outO2GNO.ToString().Trim();
                G_RANK_ONGOING_Y.Text = outO2RANK.ToString().Trim();
                G_ACL_ONGOING_Y.Text = outO2ACL.ToString().Trim();
                P4TYPE.Text = "Z";
            }
            else
            {
                if (outSTS.ToString() == "Y")
                {
                    DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(SQL.ToString(), CommandType.Text).Result.data;
                    if (busobj.check_dataset(DS))
                    {
                        string outPOSALN = "", outPOORNK = "", outPOOTMS = "", outPOOACL = "", outPOARNK = "", outPOATMS = "", outPOAACL = "";
                        string outPORRNK = "", outPORTMS = "", outPORACL = "", outPOFTCL = "", outPOSTRP = "", outPOTOTP = "", outPOAFLG = "";
                        string outPOHAVE = "", outPOODGC = "", outPOMDL = "", outPOPD = "", outPOGNO = "";
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            iLDataSubroutine.CALL_CSSR31(hid_CSN.Value.ToString(), hid_salary.Value.ToString(), "I", G_Rank_for_GNSR031.Text.Trim(),
                                               dr["m13occ"].ToString().Trim(), userInfo.BranchNo.ToString().Trim(), dr["m13sst"].ToString().Trim(), dr["m13saj"].ToString().Trim(),
                                              ref outPOSALN, ref outPOORNK, ref outPOOTMS, ref outPOOACL, ref outPOARNK, ref outPOATMS, ref outPOAACL, //8,9,10,11,12,13,14
                                              ref outPORRNK, ref outPORTMS, ref outPORACL, ref outPOFTCL, ref outPOSTRP, ref outPOTOTP, ref outPOAFLG, //15,16,17,18,19,20,21
                                              ref outPOHAVE, ref outPOODGC, ref outPOMDL, ref outPOPD, ref outPOGNO, //22,23,24,25,26
                                              userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                            CallHisunMaster._dataCenter.CloseConnectSQL();
                        }
                        G_Mem_ACL_Ongoing.Text = G_ACL.Text;
                        G_Mem_TCL_Ongoing.Text = G_Final_TCL.Text;
                        G_Net_Income.Text = outPOSALN.ToString().Trim();
                        G_Orank.Text = outPOORNK.ToString().Trim();
                        G_Otimes.Text = outPOOTMS.ToString().Trim();
                        G_ACL.Text = outPOOACL.ToString().Trim();
                        G_Arank.Text = outPOARNK.ToString().Trim();
                        G_Atimes.Text = outPOATMS.ToString().Trim();
                        G_AACL.Text = outPOAACL.ToString().Trim();

                        G_Rrank.Text = outPORRNK.ToString().Trim();
                        G_Rtimes.Text = outPORTMS.ToString().Trim();
                        G_TCL.Text = outPORACL.ToString().Trim();
                        G_Final_TCL.Text = outPOFTCL.ToString().Trim();
                        G_CSP.Text = outPOSTRP.ToString().Trim();
                        G_Total_CSP.Text = outPOTOTP.ToString().Trim();
                        G_Up_Down_Flag.Text = outPOAFLG.ToString().Trim();

                        G_Have_TCL.Text = outPOHAVE.ToString().Trim();
                        G_GRACE_Period.Text = outPOODGC.ToString().Trim();
                        G_Model.Text = outPOMDL.ToString().Trim();
                        G_PD1.Text = outPOPD.ToString().Trim();
                        G_GNO.Text = outPOGNO.ToString().Trim();

                        G_PD1.Text = G_PD1.Text.PadLeft(11, '0');
                        G_GNO.Text = G_GNO.Text.PadRight(3, '0');

                        E_model.Text = G_Model.Text.Trim();
                        if (E_model.Text == "3")
                        {
                            E_model.Text = "T";
                        }
                    }
                }
            }

            if (G_NewModel_ZR.Text == "Y")
            {
                G_Model.Text = P4TYPE.Text.ToString();
            }
            else
            {
                if (G_Have_TCL.Text.Trim() != "Y")
                {
                    if (G_Have_CSMS03.Text == "true")
                    {
                        G_Model.Text = "E";
                    }
                    else
                    {
                        G_Model.Text = P4TYPE.Text.Trim();
                    }
                }
            }

            string POBOTL = "", PONOAP = "", POCSBL = "", POCRAV = "", POAPAM = "", POEBCL = "", POAVAP = "", POSTS = "", POOLDC = "", POVENR = "",
                   POPERV = "", POEBCS = "", POERR = "", PERMSG = "", mode = "";
            //try
            //{
            //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
            //}
            //catch
            //{
            //    mode = "O";
            //}
            mode = "O";
            if ((G_Have_CSMS03.Text == "true") & (G_NewModel_ZR.Text != "Y"))
            {
                G_PD.Text = "            ";
                if (G_Model.Text.Trim() == "")
                {
                    G_Model.Text = " ";
                }
                if (G_Rank_for_GNSR031.Text.Trim() == "")
                {
                    G_Rank_for_GNSR031.Text = "  ";
                }
            }

            if (G_Rank_for_GNSR031.Text.Trim().Length == 0)
            {
                G_PD.Text = G_PD.Text + "  " + G_Model.Text;
            }
            if (G_Rank_for_GNSR031.Text.Trim().Length == 1)
            {
                G_PD.Text = G_PD.Text + "0" + G_Rank_for_GNSR031.Text + G_Model.Text;
            }
            if (G_Rank_for_GNSR031.Text.Trim().Length == 2)
            {
                G_PD.Text = G_PD.Text + G_Rank_for_GNSR031.Text + G_Model.Text;
            }

            string GNSR87_9 = "", GNSR87_13 = "";
            if (G_Have_TCL.Text == "Y")
            {
                LACL.Text = "ACL(TOGO)";
                LTCL.Text = "TCL(TOGO)";
                EdtACL.Text = G_ACL.Text;
                E_rank.Text = G_Orank.Text;
                Find_Old_TCL_Ongoing();
                GNSR87_9 = G_Final_TCL.Text.Trim();
                if (G_Final_TCL.Text.Trim() != EdtTCL.Text.Trim())
                {
                    EdtTCL.Text = G_Final_TCL.Text.Trim();
                }
                GNSR87_13 = "Y";
            }

            if (G_NewModel_ZR.Text == "Y")
            {
                GNSR87_13 = "Y";
            }

            if (GNSR87_9.ToString() == "")
            {
                GNSR87_9 = EdtTCL.Text.Trim();
            }

            if ((hid_salary.Value.ToString() != "0") & (G_Have_TCL.Text != "Y") & (G_Have_CSMS03.Text == "false"))
            {
                string outPPOTCL = "", outPOERR = "", outPERMSG = "";
                iLDataSubroutine.CALL_GNSR86("IL", "01", userInfo.BranchApp.ToString(), hid_appdate.Value.ToString(),
                                   hid_date97.Value.ToString(), E_rank.Text.Trim(), EdtACL.Text.Trim(),
                                   ref outPPOTCL, ref outPOERR, ref outPERMSG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //busobj.CloseConnectioDAL();

                if (outPOERR.ToString() == "Y")
                {
                    //busobj.CloseConnectioDAL();
                    return;
                }

                if (G_TCL.Text == "")
                {
                    G_TCL_13.Text = "0";
                }

                if ((G_TCL_13.Text.Trim() == EdtTCL.Text.Trim()) || (EdtTCL.Text.Trim() == "0"))
                {
                    EdtTCL.Text = outPPOTCL.ToString();
                    G_TCL_13.Text = EdtTCL.Text.Trim();
                }
                //E_Apv_avi. = EdtTCL.Text - StrToFloat(callGNSR87.value[16]);
                GNSR87_9 = EdtTCL.Text.Trim();
            }
            string[] vendor = dd_vendor.Value.ToString().Split('|');
            iLDataSubroutine.Call_GNSR87("IL", hid_CSN.Value.ToString(), userInfo.BranchApp.ToString(), hid_AppNo.Value.ToString().Trim(), hid_appdate.Value.ToString(),
                               hid_birthdate.Value.ToString(), "1", "Y", hid_salary.Value.ToString(), GNSR87_9.ToString(), txt_loanReq.Text.Trim(),
                               vendor[0].ToString(), "O", GNSR87_13.ToString(),
                               ref POBOTL, ref PONOAP, ref POCSBL, ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, //14,15,16,17,18,19,20,21,22
                               ref POVENR, ref POPERV, ref POEBCS, ref POERR, ref PERMSG, //23,24,25,26,27
                               userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
            //busobj.CloseConnectioDAL();

            EdtNetIncome.Text = hid_salary.Value.ToString();
            EdtBOTLoan.Text = POBOTL.ToString();
            EdtCrBal.Text = POCSBL.ToString();
            EAAA.Text = POCRAV.ToString();

            if ((G_Have_TCL.Text.Trim() == "Y") || (G_NewModel_ZR.Text.Trim() == "Y"))
            {
                EAAA.Text = (Convert.ToDecimal(EdtTCL.Text) - Convert.ToDecimal(EdtCrBal.Text)).ToString();
            }

            EdtCrLmt.Text = EdtTCL.Text;
            EdtESBLoan.Text = POEBCL.ToString();
            EdtCrAvi.Text = POAVAP.ToString();
            EdtCrAvi.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(EdtCrAvi.Text));
            E_Apv_avi.Text = EdtCrAvi.Text;
            lb_pApproveL.Text = EdtCrLmt.Text;

            string Error_GNSR093 = "";
            string payment_ability = "";

            iLDataSubroutine.Call_GNSR093(hid_idno.Value.ToString(), hid_salary.Value.ToString(), ref Error_GNSR093, ref payment_ability, userInfo.BizInit, userInfo.BranchNo);
            //busobj.CloseConnectioDAL();
            txt_pay_abl.Text = convCurrency(payment_ability);
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }//try
        catch (Exception ex)
        {
            //L_error_product.Text = "Error on Calculate TCL";
            L_message.Text = "Error on Calculate TCL";
            P_message_product.ShowOnPageLoad = true;
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
    }

    void calTCL1()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(userInfoService.GetUserInfo());
        CallHisunCustomer = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        try
        {
            //string SQL = "select m13csn, m13sex, 'AGE_GNP0371', m13mrt, int(m13ttl) as m13ttl, case when m13mtl <> '' then 1 else 0 end as m13mtl, " +
            //                                 "m13res, (m13lyr*12)+m13lmt as resident, (m13wky*12)+m13wkm as workyear, m13occ, m13but, m13slt, m13off, int(m13net) as m13net, " +
            //                                 "m13con, m13chl, m13pos, m13emp, " + hid_brn.Value.ToString() + " as three, " + hid_date97.Value.ToString() + " as date97, '000' as one, m13apv, " +
            //                                 "m13hzp, m13sst, int(m13saj) as m13saj, m13bdt, m13gol, m13cha,m13ozp " +
            //                                 "from csms13 " +
            //                                 "where m13app = 'IL' and m13csn = " + hid_CSN.Value.ToString() + " and m13brn = " + hid_brn.Value.ToString() + " " +
            //                                 "and m13apn = " + hid_AppNo.Value.ToString() + " ";

            //DS = busobj.RetriveAsDataSet(SQL.ToString());

            var result = MSSQL.GetDataset<DataSet>($@"select m13csn, m13sex, 'AGE_GNP0371', m13mrt, CAST(m13ttl as nvarchar) as m13ttl, case when m13mtl <> '' then 1 else 0 end as m13mtl, 
                                                 m13res, (m13lyr*12)+m13lmt as resident, (m13wky*12)+m13wkm as workyear, m13occ, m13but, m13slt, m13off, CAST(m13net as nvarchar) as m13net, 
                                                 m13con, m13chl, m13pos, m13emp,  '{ hid_brn.Value.ToString() }'  as three,  '{ hid_date97.Value.ToString() }'  as date97, '000' as one, m13apv, 
                                                 m13hzp, m13sst, CAST(m13saj as nvarchar) as m13saj, m13bdt, m13gol, m13cha,m13ozp 
                                                  from AS400DB01.[CSOD0001].[CSMS13]
                                                where m13app = 'IL' and Cast(m13csn as nvarchar) = '{hid_CSN.Value.ToString()}'  and Cast(m13brn as nvarchar) =  '{hid_brn.Value.ToString()}'
                                                and Cast(m13apn as nvarchar) =  '{hid_AppNo.Value.ToString()}' ", CommandType.Text).Result;

            DS = result.data.Tables.Count > 0 ? result.data : new DataSet();


            string in_AGE = "", error = "";
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                hid_salary.Value = dr["m13net"].ToString().Trim();

                //busobj.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                //                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                //                            ref in_AGE, ref error);
                //busobj.CloseConnectioDAL();

                iLDataSubroutine.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                                            ref in_AGE, ref error);

                DataSet ds_res = new DataSet();
                string Error = "";
                string[] vendor = dd_vendor.Value.ToString().Split('|');
                //busobj.calTCL(hid_idno.Value, hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, hid_appdate.Value, hid_date97.Value, dr["m13bdt"].ToString().Trim(),
                //          dr["m13sex"].ToString().Trim(), in_AGE.ToString().Trim(), dr["m13mrt"].ToString().Trim(), dr["m13ttl"].ToString().Trim(),
                //          dr["m13mtl"].ToString().Trim(), dr["m13res"].ToString().Trim(), dr["resident"].ToString().Trim(), dr["workyear"].ToString().Trim(),
                //          dr["m13occ"].ToString().Trim(), dr["m13but"].ToString().Trim(), dr["m13slt"].ToString().Trim(), dr["m13off"].ToString().Trim(),
                //          dr["m13net"].ToString().Trim(), dr["m13con"].ToString().Trim(), dr["m13chl"].ToString().Trim(), dr["m13pos"].ToString().Trim(),
                //          dr["m13emp"].ToString().Trim(), dr["three"].ToString().Trim(), "0", dr["m13apv"].ToString().Trim(), dr["m13hzp"].ToString().Trim(),
                //          //"", "", "0", dd_vendor.Value.ToString(),
                //          dr["m13sst"].ToString().Trim(), dr["m13saj"].ToString().Trim(), "0", dd_vendor.Value.ToString(),
                //          dr["m13gol"].ToString().Trim(), dr["m13cha"].ToString().Trim(), ref ds_res, ref Error, dr["m13ozp"].ToString());
                CallHisunCustomer.calTCL(hid_idno.Value, hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, hid_appdate.Value, hid_date97.Value, dr["m13bdt"].ToString().Trim(),
                          dr["m13sex"].ToString().Trim(), in_AGE.ToString().Trim(), dr["m13mrt"].ToString().Trim(), dr["m13ttl"].ToString().Trim(),
                          dr["m13mtl"].ToString().Trim(), dr["m13res"].ToString().Trim(), dr["resident"].ToString().Trim(), dr["workyear"].ToString().Trim(),
                          dr["m13occ"].ToString().Trim(), dr["m13but"].ToString().Trim(), dr["m13slt"].ToString().Trim(), dr["m13off"].ToString().Trim(),
                          dr["m13net"].ToString().Trim(), dr["m13con"].ToString().Trim(), dr["m13chl"].ToString().Trim(), dr["m13pos"].ToString().Trim(),
                          dr["m13emp"].ToString().Trim(), dr["three"].ToString().Trim(), "0", dr["m13apv"].ToString().Trim(), dr["m13hzp"].ToString().Trim(),
                          //"", "", "0", dd_vendor.Value.ToString(),
                          dr["m13sst"].ToString().Trim(), dr["m13saj"].ToString().Trim(), "0", vendor[0].ToString(),
                          dr["m13gol"].ToString().Trim(), dr["m13cha"].ToString().Trim(), ref ds_res, ref Error, dr["m13ozp"].ToString());
                DataRow dr_res = ds_res.Tables[0].Rows[0];
                if (busobj.check_dataset(ds_res))
                {
                    EdtTCL.Text = dr_res["EdtTCL"].ToString().Trim();
                    //LblStsTCL.Text = dr_res["LblStsTCL"].ToString().Trim();
                    LTCL.Text = dr_res["LTCL"].ToString().Trim();
                    EdtACL.Text = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim();
                    EdtTCL.Text = dr_res["EdtTCL"].ToString().Trim();
                    G_TCL_13.Text = dr_res["EdtTCL"].ToString().Trim();
                    G_ACL_13.Text = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim();
                    G_PD.Text = dr_res["G_PD"].ToString().Trim(); ;
                    E_group.Text = dr_res["E_group"].ToString().Trim() == "" ? "0" : dr_res["E_group"].ToString().Trim();
                    E_rank.Text = dr_res["E_rank"].ToString().Trim() == "" ? "0" : dr_res["E_rank"].ToString().Trim();
                    G_Rank_for_GNSR031.Text = dr_res["G_Rank_for_GNSR031"].ToString().Trim();
                    G_GROUP_ONGOING_Y.Text = dr_res["G_GROUP_ONGOING_Y"].ToString().Trim();
                    G_RANK_ONGOING_Y.Text = dr_res["G_RANK_ONGOING_Y"].ToString().Trim();
                    G_ACL_ONGOING_Y.Text = dr_res["G_ACL_ONGOING_Y"].ToString().Trim();
                    P4TYPE.Text = dr_res["P4TYPE"].ToString().Trim();

                    EdtNetIncome.Text = dr_res["EdtNetIncome"].ToString().Trim();
                    EdtBOTLoan.Text = dr_res["EdtBOTLoan"].ToString().Trim();
                    EdtCrBal.Text = dr_res["EdtCrBal"].ToString().Trim();
                    EAAA.Text = dr_res["EAAA"].ToString().Trim();
                    EdtCrLmt.Text = dr_res["EdtCrLmt"].ToString().Trim();
                    EdtESBLoan.Text = dr_res["EdtESBLoan"].ToString().Trim();
                    EdtCrAvi.Text = dr_res["EdtCrAvi"].ToString().Trim();
                    E_Apv_avi.Text = dr_res["E_Apv_avi"].ToString().Trim();
                    EdtCrAvi.Text = E_Apv_avi.Text == "" ? "0.00" : String.Format("{0:#,###,##0.00}", Convert.ToDecimal(E_Apv_avi.Text));
                  //  EdtCrAvi.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(E_Apv_avi.Text));
                    lb_pApproveL.Text = dr_res["EdtCrLmt"].ToString().Trim();
                    lb_pApproveL.Text = dr_res["EdtCrLmt"].ToString().Trim() == "" ? "0.00" : String.Format("{0:#,###,##0.00}", Convert.ToDecimal(lb_pApproveL.Text));
                   
                    txt_pay_abl.Text = dr_res["txt_pay_abl"].ToString().Trim();

                    EdtTCL.ReadOnly = false;
                    if (LTCL.Text == "TCL(Existing)")
                    {
                        EdtTCL.ReadOnly = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
        }
    }

    void Find_Old_TCL_Ongoing()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(userInfoService.GetUserInfo());

        string M13GOL = "";
        if ((G_Mem_TCL_Ongoing.Text.Trim() != G_Final_TCL.Text.Trim()) || (G_Mem_ACL_Ongoing.Text != G_ACL.Text))
        {
            G_Ongoing_TCL.Text = G_Final_TCL.Text.Trim();
            EdtACL.Text = G_Final_TCL.Text.Trim();
            EdtTCL.Text = G_Final_TCL.Text.Trim();
        }

        if ((EdtTCL.Text.Trim() == "0") || (EdtTCL.Text.Trim() == ""))
        {
            G_Ongoing_TCL.Text = G_Final_TCL.Text.Trim();
            EdtACL.Text = G_Final_TCL.Text.Trim();
            EdtTCL.Text = G_Final_TCL.Text.Trim();
        }
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();

        //DS = busobj.RetriveAsDataSet("select m13gol from AS400DB01.[CSOD0001].csms13 " +
        //                             "where m13app='IL' and m13csn = " + hid_CSN.Value.ToString() + " and " +
        //                             "m13brn=" + hid_brn.Value.ToString() + " and m13apn=" + hid_AppNo.Value.ToString() + " ");

        var result = MSSQL.GetDataset<DataSet>("select m13gol from AS400DB01.[CSOD0001].csms13 " +
                                     "where m13app='IL' and m13csn = " + hid_CSN.Value.ToString() + " and " +
                                     "m13brn=" + hid_brn.Value.ToString() + " and m13apn=" + hid_AppNo.Value.ToString() + " ", CommandType.Text).Result;

        DS = result.data.Tables.Count > 0 ? result.data : new DataSet();


        if (busobj.check_dataset(DS))
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (Convert.ToDecimal(dr["M13GOL"].ToString().Trim()) <= 0)
                {
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;
                }
                M13GOL = dr["M13GOL"].ToString().Trim();
            }
        }

        //DS = busobj.RetriveAsDataSet("select * from csms07 " +
        //                             "where c07csn=" + hid_CSN.Value.ToString() + " and " +
        //                             "c07app='IL' and c07apn=" + hid_AppNo.Value.ToString() + " and c07brn=" + hid_brn.Value.ToString() + " and " +
        //                             "c07acl=" + G_ACL.Text.Trim() + " and c07ftc=" + G_Final_TCL.Text.Trim() + " ");
        var resultCs07 = MSSQL.GetDataset<DataSet>("select C07CNT from  AS400DB01.[CSOD0001].csms07 " +
                                     "where c07csn=" + hid_CSN.Value.ToString() + " and " +
                                     "c07app='IL' and c07apn=" + hid_AppNo.Value.ToString() + " and c07brn=" + hid_brn.Value.ToString() + " and " +
                                     "c07acl=" + G_ACL.Text.Trim() + " and c07ftc=" + G_Final_TCL.Text.Trim() + " ", CommandType.Text).Result;

        DS = resultCs07.data.Tables.Count > 0 ? resultCs07.data : new DataSet();

        if (busobj.check_dataset(DS))
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if ((G_Mem_TCL_Ongoing.Text.Trim() != G_Final_TCL.Text.Trim()) || (G_Mem_ACL_Ongoing.Text.Trim() != G_ACL.Text.Trim()))
                {
                    if (M13GOL.ToString() == "")
                    {
                        M13GOL = "0";
                    }

                    G_Ongoing_TCL.Text = M13GOL.ToString().Trim();
                    EdtACL.Text = G_Final_TCL.Text.Trim();
                    EdtTCL.Text = M13GOL.ToString().Trim();
                }
            }
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();
    }

    protected void btn_keyin_Click(object sender, EventArgs e)
    {
        fn_key_in();
    }

    private void fn_key_in()
    {
        dd_totalterm.Enabled = true;
        dd_vendor.Enabled = true;
        dd_campaign.Enabled = true;
        dd_product.Enabled = true;
        txt_price.Enabled = true;
        txt_qty.Enabled = true;
        txt_down.Enabled = true;
        btn_saveCr.Enabled = false;

        btn_cal_TCL.Enabled = true;
        //btn_cal_TCL.Enabled = false;
        btn_keyin.Enabled = false;
        btn_vendorScr.Enabled = true;
    }

    protected void btn_saveCr_Click(object sender, EventArgs e)
    {
        string err = "";
        string pass_sts = "N";
        lb_err.Text = "";

        if (btn_cal_TCL.Enabled == true)
        {
            L_message.Text = "Please Calculate before Save ";
            P_message_product.ShowOnPageLoad = true;
            return;
        }

        if (!checkBeforeSave(ref err, ref pass_sts))
        {
            L_message.Text = err.ToString();
            P_message_product.ShowOnPageLoad = true;
            return;
        }

        if (hid_status.Value == "INTERVIEW")
        {
            if (err == "" || pass_sts == "Y")
            {
                lb_err.Text = err;
                L_confirm.Text = " Do you want to Save Product?";
                P_confirm_product.ShowOnPageLoad = true;
            }
        }
        else
        {
            if (pass_sts == "Y")
            {
                L_message.Text = err.ToString();
                P_message_product.ShowOnPageLoad = true;
                return;
            }
            lb_err.Text = err;
            L_confirm.Text = " Do you want to Save Product?";
            P_confirm_product.ShowOnPageLoad = true;


        }
    }

    //***  check before save credit ***//
    private bool checkBeforeSave(ref string err, ref string pass_sts)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            string err_ = "";
            bool resProd = checkBeforeCaculateProduct("SAVE", ref err_);
            if (!resProd)
            {
                //err = "กรุณาตรวจสอบ Vendor/Price/จำนวนสินค้า";
                err = err_;
                return false;
            }

            if (dd_paymentType.SelectedItem.Value.ToString().Trim() == "")
            {
                err = "กรุณาระบุ Payment Type";
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

            string AppDate = hid_date97.Value.ToString();

            //ilObj.Call_ILSR97("01", "DMY", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
            //ilObj.CloseConnectioDAL();

            string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            //ilObj.UserInfomation = userInfo;
            //ilObj.checkApproveCriteria(product[0], vendor[0], userInfo.BranchApp,, appDate, appType, csn, idno, curDate.ToString(), "ILUSINGWEB", nothave_th);
            //ilObj.CloseConnectioDAL();

            //** 5. จำนวนเงินที่ต้องผ่อนต่อเดือนต้องไม่มากกว่า Payment Ability **//
            string[] vendor = dd_vendor.Value.ToString().Split('|');
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], campaign[2], hid_appdate.Value);
            //DataSet ds_camp = ilObj.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], campaign[2], hid_appdate.Value);
            if (!ilObj.check_dataset(ds_camp))
            {
                err = " cannot find campaign ";
                return false;
            }
            else
            {
                DataRow dr_campaign = ds_camp.Tables[0].Rows[0];
                int freeInt = int.Parse(dr_campaign["c01fin"].ToString().Trim()) + 1;
                try
                {
                    Label lb_install = (Label)gv_install.Rows[freeInt].Cells[3].FindControl("lb_Installment");
                    float installment = float.Parse(lb_install.Text.Trim(), NumberStyles.Currency);
                    float paymentABL = float.Parse(txt_pay_abl.Text.Trim(), NumberStyles.Currency);
                    if (installment > paymentABL)
                    {
                        err = "จำนวนเงินที่ต้องผ่อนต่อเดือนมากกว่า Payment Ability ";
                        pass_sts = "Y";
                        //return false;
                    }
                }
                catch (Exception ex)
                {
                    err = "cannot find free installment";
                    return false;
                }


                string AppDateBOT = hid_date97.Value.Substring(4, 4) + hid_date97.Value.Substring(3, 2) + hid_date97.Value.Substring(0, 2);
                //ilObj.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDateBOT);
                //ilObj.CloseConnectioDAL();
                //DataSet ds_gnmx01 = ilObj.getGNMX01(AppDateBOT);
                DataSet ds_gnmx01 = CallHisunMaster.getGNMX01(AppDateBOT);
                if (ilObj.check_dataset(ds_gnmx01))
                {
                    DataRow dr_gnmx = ds_gnmx01.Tables[0].Rows[0];
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

            //// *** 	ตรวจสอบข้อมูล Business  check cssr035 ***//
            //string POERRC = "";
            //string POERRM = "";

            //ilObj.UserInfomation = userInfo;
            //bool resCSSR035 = ilObj.Call_CSSR035(lb_csn.Text.Trim(), dd_occup.SelectedItem.Value.ToString(), "N", ref POERRC, ref POERRM, userInfo.BizInit, userInfo.BranchNo);
            //ilObj.CloseConnectioDAL();
            //if (!resCSSR035 || POERRC != "000")
            //{
            //    err = POERRM;
            //    return false;
            //}

            return true;
        }
        catch (Exception ex)
        {
            err = "error";
            return false;
        }
    }

    protected void B_confirmcancel_Click(object sender, EventArgs e)
    {
        P_confirm_product.ShowOnPageLoad = false;
    }

    protected void B_confirmok_Click(object sender, EventArgs e)
    {
        P_message_product.ShowOnPageLoad = false;
        if (L_message.Text == "Save Product Completed")
        {
            changeTabToTCL("PASS");
        }
    }

    protected void B_confirmsave_Click(object sender, EventArgs e)
    {
        //Save ILMS01
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(userInfoService.GetUserInfo());


        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        iDB2Command cmd = new iDB2Command();

        string m13apt = "", m13apv = "";
        //DS = busobj.RetriveAsDataSet("select m13apt, m13apv from csms13 " +
        //                             "where m13app = 'IL' and m13csn = " + hid_CSN.Value.ToString() + " and m13brn = " + hid_brn.Value.ToString() + " " +
        //                             "and m13apn = " + hid_AppNo.Value.ToString() + " ");
        var resultCs07 = MSSQL.GetDataset<DataSet>("select m13apt, m13apv from [AS400DB01].[CSOD0001].csms13 " +
                                    "where m13app = 'IL' and m13csn = " + hid_CSN.Value.ToString() + " and m13brn = " + hid_brn.Value.ToString() + " " +
                                     "and m13apn = " + hid_AppNo.Value.ToString() + " ", CommandType.Text).Result;

        DS = resultCs07.data.Tables.Count > 0 ? resultCs07.data : new DataSet();
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                m13apt = dr["m13apt"].ToString().Trim();
                m13apv = dr["m13apv"].ToString().Trim();
            }
            DS.Clear();
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        string found_ilmd012 = "false";
        //DS = busobj.RetriveAsDataSet("select * from [AS400DB01].ILOD0001.ilmd012 " +
        //                             "where d012br = " + hid_brn.Value.ToString() + " and d012ap = " + hid_AppNo.Value.ToString() + " ");
        var resultILMD012 = MSSQL.GetDataset<DataSet>("select D012CT from [AS400DB01].ILOD0001.ilmd012 " +
                                     "where d012br = " + hid_brn.Value.ToString() + " and d012ap = " + hid_AppNo.Value.ToString() + " ", CommandType.Text).Result;

        DS = resultILMD012.data.Tables.Count > 0 ? resultILMD012.data : new DataSet();

        if (DS != null && DS.Tables.Count > 0)
        {

            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                found_ilmd012 = "true";
            }
            DS.Clear();
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        /******************************************* GET VALUE ****************************************************/
        string[] vendor = dd_vendor.Value.ToString().Split('|');
        string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
        string[] product = dd_product.SelectedItem.Value.ToString().Split('|');
        DataSet ds_camp = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], "", hid_appdate.Value);
        //DataSet ds_camp = busobj.getCampaign(vendor[0], dd_totalterm.Text.Trim(), campaign[0], campaign[1], "", hid_appdate.Value);
        if (!busobj.check_dataset(ds_camp))
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        iLDataCenterMssqlProduct = new ILDataCenterMssqlProduct(userInfoService.GetUserInfo());
        DataSet ds_091l1 = iLDataSubroutine.getILTB09l1();
        if (!busobj.check_dataset(ds_091l1))
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }

        DataSet ds_ilTB06 = iLDataSubroutine.getILTB06();
        if (!busobj.check_dataset(ds_ilTB06))
        {
            L_error_product.Text = " cannot find data in ILTB06 ";
        }

        DataRow dr_camp = ds_camp.Tables[0].Rows[0];
        DataRow dr_vat = ds_091l1.Tables[0].Rows[0];
        DataRow dr_iltb06 = ds_ilTB06.Tables[0].Rows[0];

        string p1vatr = dr_vat["t09rte"].ToString().Trim();
        string p1vata = (Math.Round((float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency) * 7) / float.Parse(dr_vat["t09bas"].ToString().Trim()), 2)).ToString();
        CallHisunMaster._dataCenter.CloseConnectSQL();

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
        string p1diff = float.Parse(lb_diff.Text.Trim(), NumberStyles.Currency).ToString();  //8
        string p1frtm = freeInt.ToString();
        string p1frdt = lb_intall_f.Text.Substring(6, 4) + lb_intall_f.Text.Substring(3, 2) + lb_intall_f.Text.Substring(0, 2);
        string p1fram = float.Parse(lb_p1fram.Text, NumberStyles.Currency).ToString();

        /******************************************* GET VALUE ****************************************************/

        bool success = true;
        int affectedRows = 0;
        //cmd.Parameters.Clear();
        //cmd.CommandText = "insert into ilms01hs (select " + hid_date97.Value.ToString() + ", " + m_UpdTime.ToString() + ", " +
        //                  "'" + userInfo.Username.ToString() + "', '" + userInfo.LocalClient.ToString() + "', ilms01.* " +
        //                  "from ilms01 where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + ") ";
        //affectedRows = -1;
        //try
        //{
        //    affectedRows = busobj.ExecuteNonQuery(cmd);
        //}
        //catch (Exception ex)
        //{
        //    success = false;
        //    //L_error_product.Text = "Error on Insert ILMS01HS";
        //    L_message.Text = "Error on Insert ILMS01HS";
        //    P_message_product.ShowOnPageLoad = true;
        //    goto commit_rollback;
        //}
        //if (affectedRows < 0)
        //{
        //    success = false;
        //    //L_error_product.Text = "Error on Insert ILMS01HS";
        //    L_message.Text = "Error on Insert ILMS01HS";
        //    P_message_product.ShowOnPageLoad = true;
        //    goto commit_rollback;
        //}

        string p1fill = "", UpdateUser = "";
        if (hid_status.Value.ToString() == "INTERVIEW")
        {
            p1fill = "CONCAT(SUBSTRING(p1fill,1,24),'2  PI',SUBSTRING(p1fill,30,13)) ";
            UpdateUser = "p1crcd = '" + userInfo.Username.ToString() + "', ";
        }
        if (hid_status.Value.ToString() == "KESSAI")
        {
            p1fill = "CONCAT(SUBSTRING(p1fill,1,27),'PK',SUBSTRING(p1fill,30,13)) ";
        }

        string AppType = "p1appt = '" + m13apt.ToString() + "', p1apvs = '" + m13apv.ToString() + "', ";

        string bankcode = "", branchcode = "", acctype = "", accno = "", paymenttype = "";
        if (dd_bankCode.Value != null)
        {
            bankcode = dd_bankCode.Value.ToString();
        }
        if (dd_bankBranch.Value != null)
        {
            branchcode = dd_bankBranch.Value.ToString();
        }
        if (dd_accountType.Value != null)
        {
            acctype = dd_accountType.Value.ToString();
        }
        accno = txt_AccountNo.Text;
        if (dd_paymentType.Value != null)
        {
            paymenttype = dd_paymentType.Value.ToString();
        }

        cmd.Parameters.Clear();
        string p1aprj = "";
        if (hid_p1aprj.Value == "PD")
        {
            p1aprj = "PD";
        }
        else
        {
            p1aprj = "MI";
        }

        string cmdupdate = "Update [AS400DB01].[ILOD0001].ILMS01 set " +
                       "" + AppType.ToString() + " " +
                       "p1apdt = " + hid_appdate.Value.ToString() + ", " +
                       "p1pbcd = '" + bankcode.ToString() + "', " +
                       "p1pbrn = '" + branchcode.ToString() + "', " +
                       "p1paty = '" + acctype.ToString() + "', " +
                       "p1pano = '" + accno.ToString() + "', " +
                       "p1payt = '" + paymenttype.ToString() + "', " +
                       "p1vdid = " + vendor[0] + ", " +
                       "p1mkid = " + dr_camp["c01mkc"].ToString().Trim() + ", " +
                       "p1camp = " + campaign[0] + ", " +
                       "p1cmsq = " + campaign[1] + ", " +
                       "p1item = " + product[0] + ", " +
                       "p1pdgp = '" + product[3] + "', " +
                       "p1pric = " + float.Parse(txt_price.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1qty  = " + int.Parse(txt_qty.Text.Trim()).ToString() + ", " +
                       "p1purc = " + float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1vatr = " + dr_vat["t09rte"].ToString().Trim() + ", " +
                       "p1vata = " + (Math.Round((float.Parse(txt_purch.Text.Trim(), NumberStyles.Currency) * 7) / float.Parse(dr_vat["t09bas"].ToString().Trim()), 2)).ToString() + ", " +
                       "p1down = " + float.Parse(txt_down.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1term = " + dd_totalterm.Text.Trim() + ", " +
                       "p1rang = " + txt_total_range.Text.Trim() + ", " +
                       "p1ndue = " + txt_non.Text.Trim() + ", " +
                       "p1lndr = " + dr_iltb06["T06LON"].ToString().Trim() + ", " +
                       "p1dutr = " + dr_iltb06["t06Dut"].ToString().Trim() + ", " +
                       "p1infr = " + dr_camp["c01fin"].ToString().Trim() + ", " +
                       "p1intr = " + txt_Int.Text.Trim() + ", " +
                       "p1crur = " + txt_cru.Text.Trim() + ", " +
                       "p1pram = " + float.Parse(txt_loanReq.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1inta = " + float.Parse(lb_install_total.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1crua = " + float.Parse(lb_cru_amt_total.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1infa = " + float.Parse(lb_InitFree_total.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1duty = " + txt_duty.Text.Trim() + ", " +
                       "p1diff = " + float.Parse(lb_diff.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1coam = " + float.Parse(txt_contractAmt.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1fdam = " + float.Parse(txt_fDue_AMT.Text.Trim(), NumberStyles.Currency).ToString() + ", " +
                       "p1frtm = " + p1frtm.ToString() + ", " +
                       "p1frdt = " + lb_intall_f.Text.Substring(6, 4) + lb_intall_f.Text.Substring(3, 2) + lb_intall_f.Text.Substring(0, 2) + ", " +
                       "p1fram = " + p1fram.ToString() + ", " +
                       "p1aprj = '" + p1aprj + "', " +
                       "p1fdue = " + txt_fDue_date.Text.Trim().Substring(6, 4) +
                                   txt_fDue_date.Text.Trim().Substring(3, 2) +
                                   txt_fDue_date.Text.Trim().Substring(0, 2) + ", " +
                       "p1loca = '150', " +
                       "" + UpdateUser + " " +
                       "p1fill = " + p1fill + ", " +
                       "p1updt = " + m_UpdDate.ToString() + ", " +
                       "p1uptm = " + m_UpdTime.ToString() + ", " +
                       "p1upus = '" + userInfo.Username.ToString() + "', " +
                       "p1prog = '" + hid_status.Value.Trim() + "', " +
                       "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                       "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
        affectedRows = -1;
        //if (MSSQL.SqlCon.State != ConnectionState.Open)
        //    MSSQL.SqlCon.Open();
        //MSSQL.Sqltr = MSSQL.SqlCon.BeginTransaction();
        try
        {
            var res = MSSQL.Execute(cmdupdate, CommandType.Text, MSSQL.Sqltr?.Connection == null ? true : false).Result;
            affectedRows = res.afrows;
        }
        catch (Exception ex)
        {
            success = false;
            //L_error_product.Text = "Error on Update Product ILMS01";
            L_message.Text = "Error on Update Product ILMS01";
            P_message_product.ShowOnPageLoad = true;
            goto commit_rollback;
        }
        if (affectedRows < 0)
        {
            success = false;
            //L_error_product.Text = "Error on Update Product ILMS01";
            L_message.Text = "Error on Update Product ILMS01";
            P_message_product.ShowOnPageLoad = true;
            goto commit_rollback;
        }

        if (found_ilmd012.ToString().Trim() == "true")
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "delete from AS400DB01.ILOD0001.ilmd012 where d012br = " + hid_brn.Value.ToString() + " and d012ap = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                affectedRows = MSSQL.Execute(cmd.CommandText, CommandType.Text).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                //L_error_product.Text = "Error on Delete ILMD012";
                L_message.Text = "Error on Delete ILMD012";
                P_message_product.ShowOnPageLoad = true;
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                //L_error_product.Text = "Error on Delete ILMD012";
                L_message.Text = "Error on Delete ILMD012";
                P_message_product.ShowOnPageLoad = true;
                goto commit_rollback;
            }
        }

        string cont = "";
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
            cmd.CommandText = iLDataCenterMssqlProduct.InsertILMD012(
                                hid_brn.Value.ToString(),
                                hid_AppNo.Value.ToString(),
                                //lb_F.Text == "" ? "0" : lb_F.Text,
                                seq_d012sq.ToString(),
                                "02",
                                cont.ToString() == "" ? "0" : float.Parse(cont.ToString(), NumberStyles.Currency).ToString(),
                                ((float.Parse(lb_Tterm.Text) - float.Parse(lb_F.Text)) + 1).ToString(),
                                float.Parse(lb_F.Text).ToString(),
                                float.Parse(lb_Tterm.Text).ToString(),
                                lb_intP.Text == "" ? "0" : lb_intP.Text,
                                lb_cru_p.Text == "" ? "0" : lb_cru_p.Text,
                                lb_InitFree.Text == "" ? "0" : lb_InitFree.Text,
                                lb_PRINCIPAL.Text == "" ? "0" : float.Parse(lb_PRINCIPAL.Text, NumberStyles.Currency).ToString(),
                                lb_intAMT.Text == "" ? "0" : float.Parse(lb_intAMT.Text, NumberStyles.Currency).ToString(),
                                lb_cruAmonth.Text == "" ? "0" : float.Parse(lb_cruAmonth.Text, NumberStyles.Currency).ToString(),
                                lb_InitFree.Text == "" ? "0" : float.Parse(lb_InitFree.Text, NumberStyles.Currency).ToString(),
                                lb_INSTALL.Text == "" ? "0" : float.Parse(lb_INSTALL.Text, NumberStyles.Currency).ToString(),
                                lb_OTH.Text == "" ? "0" : lb_OTH.Text,
                                m_UpdDate.ToString(),
                                m_UpdTime.ToString(),
                                userInfo.Username.ToString(),
                                "INTERVIEW",
                                userInfo.LocalClient.ToString());
            affectedRows = -1;
            try
            {
                affectedRows = MSSQL.Execute(cmd.CommandText, CommandType.Text, MSSQL.Sqltr?.Connection == null ? true : false).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                //L_error_product.Text = "Error on Insert ILMD012";
                L_message.Text = "Error on Insert ILMD012";
                P_message_product.ShowOnPageLoad = true;
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                //L_error_product.Text = "Error on Insert ILMD012";
                L_message.Text = "Error on Insert ILMD012";
                P_message_product.ShowOnPageLoad = true;
                goto commit_rollback;
            }
        }

    commit_rollback:
        if (success)
        {
            MSSQL.CommitMssql();
            MSSQL.CloseConnectSQL();
            P_confirm_product.ShowOnPageLoad = false;
            L_message.Text = "Save Product Completed";
            P_message_product.ShowOnPageLoad = true;
        }
        else
        {
            MSSQL.RollbackMssql();
            MSSQL.CloseConnectSQL();
            P_confirm_product.ShowOnPageLoad = false;
            return;
        }
    }

    protected void dd_totalterm_TextChanged(object sender, EventArgs e)
    {
        dd_vendor.Items.Clear();
        dd_vendor.Text = "";
        dd_campaign.Items.Clear();
        dd_campaign.Text = "";
        dd_product.Items.Clear();
        dd_product.Text = "";
        bindVendor();
    }


    //****************  Popup Product ********************//
    protected void btn_vendorScr_Click(object sender, EventArgs e)
    {
        try
        {
            bind_ProductType();
            if (dd_totalterm.Text.Trim() == "" || dd_vendor.Value == null || dd_campaign.Value == null)
            {
                lb_res_search.Text = "กรุณาระบุ Total term , Vendor , Campaign ก่อนทำการค้นหา Product ให้ครบถ้วน";

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

    private void bind_ProductType()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

            //DataSet ds = ilObj.getILTB40("");
            DataSet ds = CallHisunMaster.getILTB40("");
            dd_prodType.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_prodType.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_prodType.Items.Add(
                        new ListEditItem(dr["T40TYP"].ToString().Trim() + " : " + dr["T40DES"].ToString().Trim(), dr["T40TYP"].ToString().Trim()));
                }
            }
            dd_prodType.SelectedIndex = -1;


        }
        catch (Exception ex)
        {

        }
    }
    private void bindProductBrand(string desc)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            string brand = dd_prodBrand.Text.Trim();
            ILDataCenter ilObj = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

            DataSet ds = new DataSet();
            ds = CallHisunMaster.getILTB42(""); //ilObj.getILTB42(brand);

            dd_prodBrand.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_prodBrand.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_prodBrand.Items.Add(
                        new ListEditItem(dr["T42BRD"].ToString().Trim() + " : " + dr["T42DES"].ToString().Trim(), dr["T42BRD"].ToString().Trim()));
                }
            }
            dd_prodBrand.SelectedIndex = -1;


        }
        catch (Exception ex)
        {

        }
    }

    private void bindProductCode(string desc, string Type)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

            DataSet ds = new DataSet();
            ds = CallHisunMaster.getILTB41(desc, Type);
            dd_prodcode.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_prodcode.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_prodcode.Items.Add(
                        new ListEditItem(dr["T41COD"].ToString().Trim() + " : " + dr["T41DES"].ToString().Trim(), dr["T41COD"].ToString().Trim()));
                }
            }
            dd_prodcode.SelectedIndex = -1;


        }
        catch (Exception ex)
        {

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

    protected void btn_src_pdItem_Click(object sender, EventArgs e)
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
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


            ilObj = new ILDataCenter();
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


            string[] vendor = dd_vendor.Value.ToString().Split('|');
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Trim().Split('|');

            DataSet ds = new DataSet();

            ILDataCenterMssqlKeyinStep1 ilProduct = new ILDataCenterMssqlKeyinStep1(userInfoService.GetUserInfo());

            if (prodType != "" && brand_name != "" && prodCode != "")
            {
                ds = ilProduct.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, brand_name, prodCode).Result;

                //ds = ilObj.getProduct_(vendor[0], campaign[0], campaign[1], prodType, brand_name, prodCode);

            }
            else if (prodType != "" && brand_name != "" && prodCode == "")
            {
                //ds = ilObj.getProduct_(vendor[0], campaign[0], campaign[1], prodType, brand_name);
                ds = ilProduct.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, brand_name).Result;

            }
            else if (prodType != "" && brand_name == "" && prodCode != "")
            {
                //ds = ilObj.getProduct_(vendor[0], campaign[0], campaign[1], prodType, "", prodCode);
                ds = ilProduct.getProduct_<DataSet>(vendor[0], campaign[0], campaign[1], prodType, "", prodCode).Result;

            }
            gv_item.DataSource = ds;
            gv_item.DataBind();


        }
        catch (Exception ex)
        {
            lb_res_search.Text = "Error กรุณาทำการค้นหาใหม่อีกครั้ง";
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

    protected void gvTerm_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (hid_loantyp.Value == "01")
            {
                ((DataControlField)gvTerm.Columns
                 .Cast<DataControlField>()
                 .Where(fld => fld.HeaderText == "INT%(EIR/YEAR)")
                 .SingleOrDefault()).Visible = false;
                ((DataControlField)gvTerm.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "CRU%(EIR/YEAR)")
                .SingleOrDefault()).Visible = false;
                ((DataControlField)gvTerm.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Int Amount")
            .SingleOrDefault()).Visible = true;
                ((DataControlField)gvTerm.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Cru Amount")
                .SingleOrDefault()).Visible = true;
            }
            else
            {
                ((DataControlField)gvTerm.Columns
          .Cast<DataControlField>()
          .Where(fld => fld.HeaderText == "INT%(EIR/YEAR)")
          .SingleOrDefault()).Visible = true;
                ((DataControlField)gvTerm.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "CRU%(EIR/YEAR)")
                .SingleOrDefault()).Visible = true;
                ((DataControlField)gvTerm.Columns
                 .Cast<DataControlField>()
                 .Where(fld => fld.HeaderText == "Int Amount")
                 .SingleOrDefault()).Visible = false;
                ((DataControlField)gvTerm.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Cru Amount")
                .SingleOrDefault()).Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
}