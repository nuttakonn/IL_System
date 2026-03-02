using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ESB.WebAppl.ILSystem.commons;

public partial class ManageData_WorkProcess_Update_AutoPay : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    public UserInfo m_userInfo;
    public UserInfoService userInfoService;
    protected void Page_Load(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        m_userInfo = userInfoService.GetUserInfo();
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
            string DateNow = DateTime.Now.ToString("dd/MM/yyyy", m_DThai);
            txt_from.Text = DateNow;
            txt_to.Text = DateNow;
            bind_bankCode();
            bind_branch();
            btn_save.Enabled = false;
        }
    }



    protected void btn_search_Click(object sender, EventArgs e)
    {

        searchData();
    }
    protected void rb_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rb_type.SelectedValue == "1")
        {
            fn_clear_cond();
        }
        else if (rb_type.SelectedValue == "2")
        {
            fn_clear_cond();
        }
        else if (rb_type.SelectedValue == "3")
        {
            fn_clear_cond();
            txt_id.Enabled = false;

            txt_ebc.Enabled = false;

            txt_csn.Enabled = false;

            txt_from.Enabled = true;
            txt_to.Enabled = true;
            ImageButton1.Enabled = false;
            ImageButton2.Enabled = false;

        }
        fn_clear();
        gv_cust.DataSource = null;
        gv_cust.DataBind();
    }
    private void fn_clear_cond()
    {
        txt_id.Text = "";
        txt_csn.Text = "";
        //.Text = "";
        txt_app.Text = "";
        txt_cont.Text = "";
        txt_ebc.Text = "";
        //dd_bankCode.SelectedItem.Value = "";
        //txt_from.Text = "";
        //txt_to.Text = "";

        txt_id.Enabled = true;
        txt_csn.Enabled = true;
        dd_brn.Enabled = true;
        txt_app.Enabled = true;
        txt_cont.Enabled = true;
        txt_ebc.Enabled = true;
        dd_bankCode.Enabled = true;
        txt_from.Enabled = true;
        txt_to.Enabled = true;
        ImageButton1.Enabled = true;
        ImageButton2.Enabled = true;


    }

    private void bind_branch()
    {
        try
        {
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            ILDataCenterMssql iLDataCenterMssql = new ILDataCenterMssql(userInfo);

            DataSet ds = new DataSet();
            if (Cache["dsBranch"] != null)
            {
                ds = (DataSet)Cache["dsBranch"];
            }
            else
            {
                ds = iLDataCenterMssql.getILTB01();
                Cache["dsBranch"] = ds;
                Cache.Insert("dsBranch", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }

            //DataSet ds = ilObj.getILTB01("");
            dd_brn.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_brn.Items.Add("--- Select ---", "");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["T1BRN"].ToString().Trim() != "301")
                    {
                        dd_brn.Items.Add(
                            new ListEditItem(dr["T1BRN"].ToString().Trim() + " : " + dr["T1BNME"].ToString().Trim(), dr["T1BRN"].ToString().Trim()));

                    }
                }

                dd_brn.SelectedIndex = -1;

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void bind_bankCode()
    {
        try
        {
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfo);
            DataSet ds = new DataSet();

            if (Cache["dsBankCode"] != null)
            {
                ds = (DataSet)Cache["dsBankCode"];
            }
            else
            {
                ds = iLDataSubroutine.getBankCode();
                Cache["dsBankCode"] = ds;
                Cache.Insert("dsBankCode", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

            }


            dd_bankCode.Items.Clear();
            dd_bank_code_cust.Items.Clear();

            if (ilObj.check_dataset(ds))
            {
                dd_bankCode.Items.Add("--- Select ---", "");
                dd_bank_code_cust.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_bankCode.Items.Add(
                        new ListEditItem(dr["g32bnk"].ToString().Trim() + " : " + dr["gnb30c"].ToString().Trim(), dr["g32bnk"].ToString().Trim()));

                    dd_bank_code_cust.Items.Add(
                        new ListEditItem(dr["g32bnk"].ToString().Trim() + " : " + dr["gnb30c"].ToString().Trim(), dr["g32bnk"].ToString().Trim()));
                }
                dd_bankCode.SelectedIndex = -1;
                dd_bank_code_cust.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {

        }

    }


    private void bind_bank_branch_Code(string bankcode)
    {
        try
        {
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfo);
            DataSet ds = iLDataSubroutine.getBankBranch(bankcode);
            dd_branch_code_cust.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_branch_code_cust.Items.Add("", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    dd_branch_code_cust.Items.Add(
                        new ListEditItem(dr["gnb31c"].ToString().Trim() + " : " + dr["gnb31d"].ToString().Trim(), dr["gnb31c"].ToString().Trim()));


                }
                dd_branch_code_cust.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {

        }

    }
    private void bind_acc_type()
    {
        try
        {
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(userInfo);
            DataSet ds = iLDataSubroutine.getAccountType();
            dd_accType_cust.Items.Clear();
            if (ilObj.check_dataset(ds))
            {
                dd_accType_cust.Items.Add("--- Select ---", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dd_accType_cust.Items.Add(
                       new ListEditItem(dr["gn13cd"].ToString().Trim() + " : " + dr["gn13td"].ToString().Trim(), dr["gn13cd"].ToString().Trim()));


                }
                dd_accType_cust.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {

        }

    }




    protected void gv_cust_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int dataItem = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToString() == "Sel")
            {

                Label lb_Name = (Label)gv_cust.Rows[dataItem].FindControl("lb_Name");
                Label lb_ID = (Label)gv_cust.Rows[dataItem].FindControl("lb_ID");
                Label lb_AppNo = (Label)gv_cust.Rows[dataItem].FindControl("lb_AppNo");
                Label lb_Loan = (Label)gv_cust.Rows[dataItem].FindControl("lb_Loan");
                Label lb_p1pram = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1pram");
                Label lb_p1csno = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1csno");
                Label lb_p1vdid = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1vdid");
                Label lb_p10nam = (Label)gv_cust.Rows[dataItem].FindControl("lb_p10nam");
                Label lb_p1payt = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1payt");
                Label lb_p1pbcd = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1pbcd");
                Label lb_p1pbrn = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1pbrn");
                Label lb_p1paty = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1paty");
                Label lb_p1pano = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1pano");
                Label lb_p1cont = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1cont");
                Label lb_p1brn = (Label)gv_cust.Rows[dataItem].FindControl("lb_p1brn");
                Label lb_p00bst = (Label)gv_cust.Rows[dataItem].FindControl("lb_p00bst");
                Label lb_p00doc = (Label)gv_cust.Rows[dataItem].FindControl("lb_p00doc");

                if (rb_type.SelectedValue == "1")
                {
                    lb_oper.Text = "Send to bank";
                    btn_save.Text = "Send to bank";

                    dd_payment_cust.Enabled = false;
                    dd_bank_code_cust.Enabled = true;
                    dd_branch_code_cust.Enabled = true;
                    dd_accType_cust.Enabled = true;
                    txt_doc_cust.Enabled = true;
                    txt_accNo_cust.Enabled = true;
                    txt_bank_sts_cust.Enabled = false;
                    rb_approve_sts_cust.Enabled = false;
                }
                else if (rb_type.SelectedValue == "2")
                {
                    lb_oper.Text = "Ready to auto pay";
                    btn_save.Text = "Ready to auto pay";

                    dd_payment_cust.Enabled = false;
                    dd_bank_code_cust.Enabled = false;
                    dd_branch_code_cust.Enabled = false;
                    dd_accType_cust.Enabled = false;
                    txt_doc_cust.Enabled = false;
                    txt_accNo_cust.Enabled = false;
                    txt_bank_sts_cust.Enabled = true;
                    rb_approve_sts_cust.Enabled = true;

                }
                else if (rb_type.SelectedValue == "3")
                {
                    lb_oper.Text = "Change payment type";
                    btn_save.Text = "Change payment type";
                    dd_payment_cust.Enabled = true;

                    if ((lb_p1payt.Text.Trim() == "1"))
                    {
                        dd_bank_code_cust.Enabled = true;
                        dd_branch_code_cust.Enabled = true;
                        dd_accType_cust.Enabled = true;
                        txt_accNo_cust.Enabled = true;
                    }
                    else
                    {
                        dd_bank_code_cust.Enabled = false;
                        dd_branch_code_cust.Enabled = false;
                        dd_accType_cust.Enabled = false;
                        txt_accNo_cust.Enabled = false;
                    }

                    txt_doc_cust.Enabled = false;
                    txt_bank_sts_cust.Enabled = false;
                    rb_approve_sts_cust.Enabled = false;
                }

                bind_acc_type();
                bind_bank_branch_Code(lb_p1pbcd.Text.Trim());



                lb_name_sh.Text = lb_Name.Text.Trim();
                lb_venCode_cust.Text = lb_p1vdid.Text.Trim();
                lb_venName.Text = lb_p10nam.Text.Trim();
                lb_csn_cust.Text = lb_p1csno.Text.Trim();
                lb_appNo_cust.Text = lb_AppNo.Text.Trim();
                lb_cont_cust.Text = lb_p1cont.Text.Trim();
                lb_id_no_cust.Text = lb_ID.Text.Trim();

                dd_payment_cust.Value = lb_p1payt.Text.Trim();
                dd_bank_code_cust.Value = lb_p1pbcd.Text.Trim();
                dd_branch_code_cust.Value = lb_p1pbrn.Text.Trim();
                dd_accType_cust.Value = lb_p1paty.Text.Trim().PadLeft(2, '0');

                txt_doc_cust.Text = lb_p00doc.Text.Trim();
                txt_accNo_cust.Text = lb_p1pano.Text.Trim();
                txt_bank_sts_cust.Text = lb_p00bst.Text.Trim();

                //****  hidden field ***//
                Hid_acc.Value = lb_p1pano.Text.Trim();
                Hid_AccType.Value = lb_p1paty.Text.Trim().PadLeft(2, '0');
                Hid_bank_sts.Value = lb_p00bst.Text.Trim();
                Hid_BankBranchCode.Value = lb_p1pbrn.Text.Trim();
                Hid_DebitBankCode.Value = lb_p1pbcd.Text.Trim();
                Hid_paymentType.Value = lb_p1payt.Text.Trim();
                Hid_Doc.Value = lb_p00doc.Text.Trim();
                Hid_branch_cust.Value = lb_p1brn.Text.Trim();

                hd_csn.Value = lb_csn_cust.Text;
                hd_idNo.Value = lb_id_no_cust.Text;


                if (lb_p1payt.Text.Trim() == "R")
                {
                    rb_approve_sts_cust.SelectedValue = "R";
                }
                else
                {
                    rb_approve_sts_cust.SelectedValue = "A";
                }

                btn_save.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void dd_bank_code_cust_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_bank_code_cust.Value != "")
            {
                bind_bank_branch_Code(dd_bank_code_cust.SelectedItem.Value.ToString());
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string err = "";
            string oper = "";
            if (rb_type.SelectedValue.ToString() == "1")
            {
                oper = "SEND";
            }
            else if (rb_type.SelectedValue.ToString() == "2")
            {
                oper = "READY";
            }
            else if (rb_type.SelectedValue.ToString() == "3")
            {
                oper = "CHANGE";
            }
            if (!ValidateAutoPay(oper, ref err))
            {
                lblMsgEN.Text = err;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            else
            {
                lblConfirmMsgEN.Text = " Do you want to save data ?";
                PopupConfirmSave.ShowOnPageLoad = true;
                return;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void searchData()
    {
        try
        {
            hd_idNo.Value = "";
            hd_csn.Value = "";
            UserInfo m_userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            ILDataCenterUpdateAutoPay iLDataCenterUpdateAutoPay = new ILDataCenterUpdateAutoPay(m_userInfo);
            ilObj.UserInfomation = m_userInfo;

            string id = txt_id.Text.Trim();
            string app = txt_app.Text.Trim();
            string ebc = txt_ebc.Text.Trim();
            string csn = txt_csn.Text.Trim();
            string cont = txt_cont.Text.Trim();
            string bank = dd_bankCode.SelectedItem.Value.ToString();

            string[] DFS = txt_from.Text.Trim().Split('/');
            string[] DTS = txt_to.Text.Trim().Split('/');
            string dateFrom = DFS[2] + DFS[1] + DFS[0];
            string dateTo = DTS[2] + DTS[1] + DTS[0];

            DataSet ds = new DataSet();

            string err = "";
            if (rb_type.SelectedValue == "1")
            {
                if (!validateBeforeSearch("SEND", ref err))
                {
                    lblMsgEN.Text = err;
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                ds = iLDataCenterUpdateAutoPay.get_cust_autoPay("SEND", id, app, ebc, csn, cont, bank, dd_brn.SelectedItem.Value.ToString(), dateFrom, dateTo);
            }
            else if (rb_type.SelectedValue == "2")
            {
                if (!validateBeforeSearch("READY", ref err))
                {
                    lblMsgEN.Text = err;
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                ds = iLDataCenterUpdateAutoPay.get_cust_autoPay("READY", id, app, ebc, csn, cont, bank, dd_brn.SelectedItem.Value.ToString(), dateFrom, dateTo);
            }
            else if (rb_type.SelectedValue == "3")
            {
                ds = iLDataCenterUpdateAutoPay.get_cust_autoPay("CHANGE", id, app, ebc, csn, cont, bank, dd_brn.SelectedItem.Value.ToString(), dateFrom, dateTo);
            }

            if (ilObj.check_dataset(ds))
            {
                gv_cust.DataSource = ds.Tables[0];
                gv_cust.DataBind();



            }
            else
            {
                gv_cust.DataSource = null;
                gv_cust.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblMsgEN.Text = "กรุณาตรวจสอบข้อมูลให้ถูกต้องก่อนทำการค้นหา ";
            PopupMsg.ShowOnPageLoad = true;
            return;
        }
    }
    private bool validateBeforeSearch(string oper, ref string err)
    {
        err = "";
        if (oper == "SEND" || oper == "READY")
        {
            if (txt_from.Text.Trim() == "" || txt_to.Text.Trim() == "")
            {
                err = " Please input  date";
                return false;
            }
            // check date 
            if (!compareDate(txt_from.Text, txt_to.Text))
            {
                err = " Start date must less than end date  ";
                return false;
            }

            if (!validateDate(txt_from.Text.Trim()) || !validateDate(txt_to.Text.Trim()))
            {
                err = "format date invalid ";
                return false;
            }
        }
        return true;
    }

    private bool ValidateAutoPay(string oper, ref string err)
    {
        try
        {
            ILDataCenter ilObj = new ILDataCenter();
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenterUpdateAutoPay iLDataCenterUpdateAutoPay = new ILDataCenterUpdateAutoPay(userInfo);
            Connect_GeneralAPI conn_general = new Connect_GeneralAPI();
            if (oper == "SEND")
            {
                if (dd_branch_code_cust.SelectedItem.Value.ToString() != "")
                {
                    err = " กรุณาลบข้อมูล Bank Branch ออกให้เป็นค่าว่าง เนื่องจากปัจจุบันไม่จำเป็นต้องระบุ Bank Branch ";
                    return false;

                }

                if (txt_doc_cust.Text.Trim() == "")
                {
                    err = " กรุณาระบุ Doc No. ";
                    return false;
                }


                if (txt_accNo_cust.Text.Trim() != "")
                {
                    DataSet ds = iLDataCenterUpdateAutoPay.get_gnmb30_update_auto_pay(dd_bank_code_cust.SelectedItem.Value.ToString().Trim());
                    if (!ilObj.check_dataset(ds))
                    {
                        err = " ไม่พบข้อมูลใน " + dd_bank_code_cust.SelectedItem.Text.ToString() + " กรุณาตรวจสอบ ";
                        return false;
                    }
                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["LenAccNo"].ToString() == "")
                        {
                            err = " ยังไม่ได้กำหนดความยาว Account No " + dd_bank_code_cust.SelectedItem.Text.ToString() + " กรุณาตรวจสอบ ";
                            return false;
                        }
                        else if (int.Parse(dr["LenAccNo"].ToString()) != txt_accNo_cust.Text.Trim().Length)
                        {
                            err = " กรุณาตรวจสอบความยาว Account No. ที่ระบุุ ";
                            return false;
                        }
                    }
                }
            }
            else if (oper == "READY")
            {

                if (txt_bank_sts_cust.Text.Trim() == "")
                {
                    err = " Bank Status ต้องไม่เป็นค่าว่าง ";
                    return false;
                }

                if (rb_approve_sts_cust.SelectedValue.ToString() != "R" && rb_approve_sts_cust.SelectedValue.ToString() != "A")
                {
                    err = " กรุณาเลือก Status Approve หรือ Reject ";
                    return false;
                }

                if (rb_approve_sts_cust.SelectedValue.ToString() == "A" && txt_bank_sts_cust.Text.Trim() != "90")
                {
                    err = " Status Approve , จะต้องระบุ bank status เป็น 90 ";
                    return false;
                }
                if ((rb_approve_sts_cust.SelectedValue == "R") && (txt_bank_sts_cust.Text.Trim() != "93") && (txt_bank_sts_cust.Text.Trim() != "96"))
                {
                    err = " Status is reject , Bank status จะต้องเป็น '93' หรือ '96'  ";
                    return false;
                }

            }
            else if (oper == "CHANGE")
            {
                if (dd_payment_cust.Value.ToString() == "")
                {
                    err = " กรูณาระบุ Payment type  ";
                    return false;
                }
                if (dd_payment_cust.Value.ToString() == "4")
                {
                    DataSet ds_00 = iLDataCenterUpdateAutoPay.get_ilms00_autopay(lb_csn_cust.Text.Trim());
                    if (ilObj.check_dataset(ds_00))
                    {
                        DataRow dr_00 = ds_00.Tables[0].Rows[0];
                        if (dr_00["p00cis"].ToString().Trim() == "")
                        {
                            err = " This customer have no Bank for Auto Pay, Do not change status to 'Ready Auto Pay' ";
                            return false;
                        }
                    }
                    else
                    {
                        err = " This customer have no Bank for Auto Pay, Do not change status to 'Ready Auto Pay' ";
                        return false;
                    }

                }
                DataSet ds_01 = iLDataCenterUpdateAutoPay.get_ilms01_autopay(lb_appNo_cust.Text.Trim(), lb_cont_cust.Text.Trim());
                if (ilObj.check_dataset(ds_01))
                {
                    DataRow dr_01 = ds_01.Tables[0].Rows[0];
                    if (dr_01["p1rsts"].ToString().Trim() == "" && Hid_paymentType.Value == "W")
                    {
                        err = " Waiting for Bank Approve and Status is Active Contract do not Change Payment Type  ";
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            err = " กรุณาทำรายการใหม่อีกครั้ง ";
            return false;
        }
    }
    private void SaveUpdateAutoPay()
    {
        ILDataCenter ilObj = new ILDataCenter();
        UserInfo m_userInfo = userInfoService.GetUserInfo();
        ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
        ILDataCenterUpdateAutoPay iLDataCenterUpdateAutoPay = new ILDataCenterUpdateAutoPay(m_userInfo);
        try
        {

            ilObj.UserInfomation = m_userInfo;

            string date97 = "";
            iLDataSubroutine.Call_ILSR97("01", "YMD", m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(), ref date97);

            //**  Parameter **//
            string p00cis = lb_csn_cust.Text.Trim();
            string p00sts = "";
            string p00std = date97;
            string p00efd = date97;
            string p00bnk = dd_bank_code_cust.SelectedItem.Value.ToString();
            string p00bbr = dd_branch_code_cust.SelectedItem.Value.ToString();
            string p00bac = txt_accNo_cust.Text.Trim();
            string p00aty = dd_accType_cust.SelectedItem.Value.ToString();
            string p00doc = txt_doc_cust.Text.Trim();
            string p00bst = "";
            string p00upd = m_UpdDate.ToString();
            string p00upt = m_UpdTime.ToString();
            string p00usr = m_userInfo.Username;
            string p00upg = "IL_AUTOPAY";
            string p00uws = m_userInfo.LocalClient;
            string oper = "";
            string sqlSTS = "";
            string p00cnt = lb_cont_cust.Text.Trim();
            iDB2Command cmd = new iDB2Command();
            bool transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
            if (rb_type.SelectedValue == "1") // send to bank
            {
                p00sts = "W";
                oper = "SEND";
                p00bst = "00";
                DataSet ds = iLDataCenterUpdateAutoPay.get_lims00_sendToBank(lb_csn_cust.Text.Trim(), dd_bank_code_cust.SelectedItem.Value.ToString(), dd_branch_code_cust.SelectedItem.Value.ToString(), txt_accNo_cust.Text.Trim());
                if (!ilObj.check_dataset(ds)) // ถ้ายังไม่มี record ใน ILMS00 ให้ insert มีแล้วให้ Update
                {
                    sqlSTS = "INSERT";
                    string sql_00 = iLDataCenterUpdateAutoPay.UpdateILMS00_autoPay(p00cis, p00sts, p00std, p00efd, p00bnk, p00bbr, p00bac, p00aty, p00doc, p00bst, p00upd, p00upt, p00usr, p00upg, p00uws, oper, sqlSTS, p00cnt);
                    cmd.CommandText = sql_00;
                    transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                    int res_00 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (res_00 == -1)
                    {
                        iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                        iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                        lblMsgEN.Text = "Save not complete (I:00)";
                        PopupMsg.ShowOnPageLoad = true;
                        return;
                    }
                    //cmd.Parameters.Clear();
                    //string sql_01HS = ilObj.InsertILMS01HS_autoPay(p00std, p00upt, p00usr, p00uws, Hid_branch_cust.Value, lb_appNo_cust.Text.Trim());
                    //cmd.CommandText = sql_01HS;
                    //int res_HS = ilObj.ExecuteNonQuery(cmd);
                    //if (res_HS == -1)
                    //{
                    //    ilObj.RollbackDAL();
                    //    ilObj.CloseConnectioDAL();
                    //    lblMsgEN.Text = "Save not complete (I:HS01)";
                    //    PopupMsg.ShowOnPageLoad = true;
                    //    return;
                    //}
                    cmd.Parameters.Clear();

                    string sql_01 = iLDataCenterUpdateAutoPay.UpdateILMS01_autoPay(p00sts, p00bnk, p00bbr, p00aty, p00bac, p00upd, p00upt,
                                        p00usr, p00upg, p00uws, lb_appNo_cust.Text.Trim(), lb_cont_cust.Text.Trim(), oper);
                    cmd.CommandText = sql_01;
                    transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                    int res_01 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (res_01 == -1)
                    {
                        iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                        iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                        lblMsgEN.Text = "Save not complete (U:01)";
                        PopupMsg.ShowOnPageLoad = true;
                        return;
                    }
                    iLDataCenterUpdateAutoPay._dataCenter.CommitMssql();
                    iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                    lblMsgEN.Text = "Send to Bank Completed";
                    PopupMsg.ShowOnPageLoad = true;
                    return;

                }
                else
                {
                    sqlSTS = "UPDATE";
                    //cmd.Parameters.Clear();
                    //string sql_00HS = ilObj.InsertILMS00HS_autoPay(p00std, p00upt, p00usr, p00uws, p00cis, p00bnk, p00bbr, p00bac);
                    //cmd.CommandText = sql_00HS;
                    //int res_00HS = ilObj.ExecuteNonQuery(cmd);
                    //if (res_00HS == -1)
                    //{
                    //    ilObj.RollbackDAL();
                    //    ilObj.CloseConnectioDAL();
                    //    lblMsgEN.Text = "Save not complete (I:HS01)";
                    //    PopupMsg.ShowOnPageLoad = true;
                    //    return;
                    //}
                    cmd.Parameters.Clear();
                    transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                    string sql_00 = iLDataCenterUpdateAutoPay.UpdateILMS00_autoPay(p00cis, p00sts, p00std, p00efd, p00bnk, p00bbr, p00bac, p00aty, p00doc, p00bst, p00upd, p00upt, p00usr, p00upg, p00uws, oper, sqlSTS, p00cnt);
                    cmd.CommandText = sql_00;
                    int res_00 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (res_00 == -1)
                    {
                        iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                        iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                        lblMsgEN.Text = "Save not complete (I:00)";
                        PopupMsg.ShowOnPageLoad = true;
                        return;
                    }
                    //cmd.Parameters.Clear();
                    //string sql_01HS = ilObj.InsertILMS01HS_autoPay(p00std, p00upt, p00usr, p00uws, Hid_branch_cust.Value, lb_appNo_cust.Text.Trim());
                    //cmd.CommandText = sql_01HS;
                    //int res_HS = ilObj.ExecuteNonQuery(cmd);
                    //if (res_HS == -1)
                    //{
                    //    ilObj.RollbackDAL();
                    //    ilObj.CloseConnectioDAL();
                    //    lblMsgEN.Text = "Save not complete (I:HS01)";
                    //    PopupMsg.ShowOnPageLoad = true;
                    //    return;
                    //}
                    cmd.Parameters.Clear();
                    string sql_01 = iLDataCenterUpdateAutoPay.UpdateILMS01_autoPay(p00sts, p00bnk, p00bbr, p00aty, p00bac, p00upd, p00upt,
                                        p00usr, p00upg, p00uws, lb_appNo_cust.Text.Trim(), lb_cont_cust.Text.Trim(), oper);
                    cmd.CommandText = sql_01;
                    transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                    int res_01 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (res_01 == -1)
                    {
                        iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                        iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                        lblMsgEN.Text = "Save not complete (U:01)";
                        PopupMsg.ShowOnPageLoad = true;
                        return;
                    }
                    iLDataCenterUpdateAutoPay._dataCenter.CommitMssql();
                    iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                    lblMsgEN.Text = "Send to Bank Completed";
                    fn_clear();
                    searchData();
                    PopupMsg.ShowOnPageLoad = true;

                    return;
                }
            }
            else if (rb_type.SelectedValue == "2")  // Ready to auto pay
            {
                sqlSTS = "UPDATE";
                oper = "READY";
                p00sts = rb_approve_sts_cust.SelectedValue.ToString();
                p00bst = txt_bank_sts_cust.Text.Trim();

                //cmd.Parameters.Clear();
                //string sql_00HS = ilObj.InsertILMS00HS_autoPay(p00std, p00upt, p00usr, p00uws, p00cis, p00bnk, p00bbr, p00bac);
                //cmd.CommandText = sql_00HS;
                //int res_00HS = ilObj.ExecuteNonQuery(cmd);
                //if (res_00HS == -1)
                //{
                //    ilObj.RollbackDAL();
                //    ilObj.CloseConnectioDAL();
                //    lblMsgEN.Text = "Save not complete (I:HS01)";
                //    PopupMsg.ShowOnPageLoad = true;
                //    return;
                //}

                transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                string sql_00 = iLDataCenterUpdateAutoPay.UpdateILMS00_autoPay(p00cis, p00sts, p00std, p00efd, p00bnk, p00bbr, p00bac, p00aty, p00doc, p00bst, p00upd, p00upt, p00usr, p00upg, p00uws, oper, sqlSTS, p00cnt);
                cmd.CommandText = sql_00;
                int res_00 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (res_00 == -1)
                {
                    iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                    iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                    lblMsgEN.Text = "Save not complete (I:00)";
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                //cmd.Parameters.Clear();
                //string sql_01HS = ilObj.InsertILMS01HS_autoPay(p00std, p00upt, p00usr, p00uws, Hid_branch_cust.Value, lb_appNo_cust.Text.Trim());
                //cmd.CommandText = sql_01HS;
                //int res_HS = ilObj.ExecuteNonQuery(cmd);
                //if (res_HS == -1)
                //{
                //    ilObj.RollbackDAL();
                //    ilObj.CloseConnectioDAL();
                //    lblMsgEN.Text = "Save not complete (I:HS01)";
                //    PopupMsg.ShowOnPageLoad = true;
                //    return;
                //}
                cmd.Parameters.Clear();
                string p1payt = p00sts == "A" ? "4" : "R";
                string sql_01 = iLDataCenterUpdateAutoPay.UpdateILMS01_autoPay(p1payt, p00bnk, p00bbr, p00aty, p00bac, p00upd, p00upt,
                                    p00usr, p00upg, p00uws, lb_appNo_cust.Text.Trim(), lb_cont_cust.Text.Trim(), oper);
                cmd.CommandText = sql_01;
                transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                int res_01 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (res_01 == -1)
                {
                    iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                    iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                    lblMsgEN.Text = "Save not complete (U:01)";
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                iLDataCenterUpdateAutoPay._dataCenter.CommitMssql();
                iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                fn_clear();
                searchData();
                lblMsgEN.Text = "Ready to auto pay Completed";
                PopupMsg.ShowOnPageLoad = true;
                return;

            }
            else if (rb_type.SelectedValue == "3")
            {
                string p1payt = dd_payment_cust.SelectedItem.Value.ToString();
                oper = "CHANGE";
                //string sql_01HS = ilObj.InsertILMS01HS_autoPay(p00std, p00upt, p00usr, p00uws, Hid_branch_cust.Value, lb_appNo_cust.Text.Trim());
                //cmd.CommandText = sql_01HS;
                //int res_HS = ilObj.ExecuteNonQuery(cmd);
                //if (res_HS == -1)
                //{
                //    ilObj.RollbackDAL();
                //    ilObj.CloseConnectioDAL();
                //    lblMsgEN.Text = "Save not complete (I:HS01)";
                //    PopupMsg.ShowOnPageLoad = true;
                //    return;
                //}

                //Send Sms Cancel Ready to Auto pay
                if ((Hid_paymentType.Value == "4") && (p1payt == "2" || p1payt == "3" || p1payt == "5"))
                {
                    cmd.Parameters.Clear();
                    string sql_00 = iLDataCenterUpdateAutoPay.UpdateILMS00_autoPay(p00cis, "C", p00std, p00efd, p00bnk, p00bbr, p00bac, p00aty, p00doc, p00bst, p00upd, p00upt, p00usr, p00upg, p00uws, oper, "UPDATE", p00cnt);
                    cmd.CommandText = sql_00;
                    transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                    int res_00 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (res_00 == -1)
                    {
                        iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                        iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                        lblMsgEN.Text = "Save not complete (I:00)";
                        PopupMsg.ShowOnPageLoad = true;
                        return;
                    }
                }



                cmd.Parameters.Clear();
                string sql_01 = iLDataCenterUpdateAutoPay.UpdateILMS01_autoPay(p1payt, p00bnk, p00bbr, p00aty, p00bac, p00upd, p00upt,
                                    p00usr, p00upg, p00uws, lb_appNo_cust.Text.Trim(), lb_cont_cust.Text.Trim(), oper);
                cmd.CommandText = sql_01;
                transaction = iLDataCenterUpdateAutoPay._dataCenter.Sqltr == null ? true : false;
                int res_01 = iLDataCenterUpdateAutoPay._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (res_01 == -1)
                {
                    iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
                    iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                    lblMsgEN.Text = "Save not complete (U:01)";
                    PopupMsg.ShowOnPageLoad = true;
                    return;
                }
                iLDataCenterUpdateAutoPay._dataCenter.CommitMssql();
                iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
                fn_clear();
                searchData();
                lblMsgEN.Text = "Change payment Completed";
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
        }
        catch (Exception ex)
        {
            iLDataCenterUpdateAutoPay._dataCenter.RollbackMssql();
            iLDataCenterUpdateAutoPay._dataCenter.CloseConnectSQL();
            lblMsgEN.Text = "Save not complete ";
            PopupMsg.ShowOnPageLoad = true;
            return;
        }
    }

    private void fn_clear()
    {
        try
        {
            lb_oper.Text = "";
            lb_name_sh.Text = "";
            lb_venCode_cust.Text = "";
            lb_venName.Text = "";
            lb_csn_cust.Text = "";
            lb_appNo_cust.Text = "";
            lb_cont_cust.Text = "";
            lb_id_no_cust.Text = "";
            dd_payment_cust.SelectedIndex = -1;
            dd_bank_code_cust.SelectedIndex = -1;
            dd_branch_code_cust.SelectedIndex = -1;
            dd_accType_cust.SelectedIndex = -1;
            txt_doc_cust.Text = "";
            txt_accNo_cust.Text = "";
            txt_bank_sts_cust.Text = "";
            rb_approve_sts_cust.ClearSelection();
            btn_save.Text = "";
            btn_save.Enabled = false;

        }
        catch (Exception ex)
        {

        }
    }
    protected void dd_payment_cust_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_payment_cust.SelectedItem.Value.ToString() == "1")
            {
                dd_bank_code_cust.Enabled = true;
                dd_branch_code_cust.Enabled = true;
                dd_accType_cust.Enabled = true;
                txt_accNo_cust.Enabled = true;
            }
            else
            {
                dd_bank_code_cust.Enabled = false;
                dd_branch_code_cust.Enabled = false;
                dd_accType_cust.Enabled = false;
                txt_accNo_cust.Enabled = false;
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
            SaveUpdateAutoPay();
        }
        catch (Exception ex)
        {
        }
    }

    private bool validateDate(string date)
    {
        string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy" };
        DateTime expectedDate;

        if (!DateTime.TryParseExact(date, formats, new CultureInfo("th-TH"),
                                        DateTimeStyles.None, out expectedDate))
        {
            return false;
        }
        return true;
    }

    private bool compareDate(string dateStart, string dateEnd)
    {
        string[] ArrStart = dateStart.Split('/');
        string[] ArrEnd = dateEnd.Split('/');

        DateTime dt_Start = Convert.ToDateTime(ArrStart[2] + "/" + ArrStart[1] + "/" + ArrStart[0], m_DThai);
        DateTime dt_End = Convert.ToDateTime(ArrEnd[2] + "/" + ArrEnd[1] + "/" + ArrEnd[0], m_DThai);
        if (dt_Start > dt_End)
        {
            return false;
        }
        return true;
    }
    protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    {
        try
        {
            Cache.Remove("dsBranch");
            Cache.Remove("dsBankCode");
            bind_branch();
            bind_bankCode();
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_addnote_Click(object sender, EventArgs e)
    {
        if (hd_idNo.Value.Trim() == "" || hd_csn.Value.Trim() == "")
        {
            lblMsgEN.Text = "Please select customer before add note";
            PopupMsg.ShowOnPageLoad = true;
            return;
        }
        string url = @"window.open('" + "../../Note/Note.aspx?param1=" + hd_csn.Value.Trim() + "&param2=" + hd_idNo.Value.Trim() + "', 'home', 'width=900,height=550,left=45,top=150,scrollbars=yes,resizable=yes');";
        ScriptManager.RegisterStartupScript(this, typeof(string), "script_note", url, true);
    }
}