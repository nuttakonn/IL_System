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
using EB_Service.Commons;

public partial class ManageData_WorkProcess_Cancel_Unsigned : System.Web.UI.Page
{
    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
    private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    ILDataCenterMssql CallMasterEnt;
    ILDataCenterMssqlInterview iLDataCenterMssqlInterview;
    ILDataCenterCancelUnsign CallEntCancelUnsign;
    ILDataSubroutine iLDataSubroutine;
    public UserInfoService userInfoService;
    UserInfo userInfo;
    public ManageData_WorkProcess_Cancel_Unsigned()
    {
        iLDataCenterMssqlInterview = new ILDataCenterMssqlInterview(userInfo);
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

        //userInfo = userInfoService.GetUserInfo();

        if (IsPostBack)
        {
            return;
        }
        PN_AddNote.Enabled = false;
    }


    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (!validateBeforeSave())
            {
                return;
            }
            checkUpdateSalary();


        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (txt_search.Text.Trim().Replace('-', ' ').Trim() == "")
        {
            lb_err.Text = "Please input data before search ";
            return;
        }
        search_data();
    }

    private void bindResCode()
    {
        try
        {
            UserInfo userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(userInfo);
            DataSet ds_resCode = new DataSet();
            DataSet ds_ActCode = new DataSet();
            ds_resCode = CallMasterEnt.getResultCode();
            CallMasterEnt._dataCenter.CloseConnectSQL();
            ds_ActCode = iLDataCenterMssqlInterview.getActionCode().Result;
            CallMasterEnt._dataCenter.CloseConnectSQL();



            if (!ilObj.check_dataset(ds_resCode) || !ilObj.check_dataset(ds_ActCode))
            {
                return;
            }


            dd_ActionCode.Items.Add("--- Select ---", "");
            foreach (DataRow dr in ds_ActCode.Tables[0].Rows)
            {

                dd_ActionCode.Items.Add(
                    new ListEditItem(dr["G24ACD"].ToString().Trim() + " : " + dr["G24DES"].ToString().Trim(), dr["G24ACD"].ToString().Trim()));
            }

            dd_ActionCode.Value = "ADD";

            dd_ResCode.Items.Add("--- Select ---", "");


            foreach (DataRow dr in ds_resCode.Tables[0].Rows)
            {
                dd_ResCode.Items.Add(
                    new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
            }
            dd_ResCode.Value = "";

        }
        catch (Exception ex)
        {
            lblMsg.Text = "กรุณาทำรายการใหม่อีกครั้ง";
        }
    }
    //private void bindResCode()
    //{
    //    try
    //    {

    //        ILDataCenter ilObj = new ILDataCenter();
    //        DataSet ds_resCode = ilObj.getResultCode();
    //        DataSet ds_ActCode = ilObj.getActionCode();
    //        if (!ilObj.check_dataset(ds_resCode) || !ilObj.check_dataset(ds_ActCode))
    //        {
    //            return;
    //        }

    //        dd_ActionCode.Items.Add("--- Select ---", "");
    //        foreach (DataRow dr in ds_ActCode.Tables[0].Rows)
    //        {

    //            dd_ActionCode.Items.Add(
    //                new ListEditItem(dr["G24ACD"].ToString().Trim() + " : " + dr["G24DES"].ToString().Trim(), dr["G24ACD"].ToString().Trim()));
    //        }

    //        dd_ActionCode.Value = "ADD";

    //        dd_ResCode.Items.Add("--- Select ---", "");


    //        foreach (DataRow dr in ds_resCode.Tables[0].Rows)
    //        {


    //            dd_ResCode.Items.Add(
    //                new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
    //        }
    //        dd_ResCode.Value = "";

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    private void search_data()
    {
        try
        {
            lb_err.Text = "";
            UserInfo m_userInfo = userInfoService.GetUserInfo();
            ILDataCenter ilObj = new ILDataCenter();
            CallMasterEnt = new ILDataCenterMssql(m_userInfo);
            CallEntCancelUnsign = new ILDataCenterCancelUnsign(m_userInfo);
            ilObj.UserInfomation = m_userInfo;
            DataSet ds = CallEntCancelUnsign.get_cust_cancel_unsign(txt_search.Text.Trim().Replace("-", ""), m_userInfo.BranchApp, dd_search.SelectedValue.ToString());
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lb_name.Text = dr["m00tnm"].ToString().Trim() + " " + dr["m00tsn"].ToString().Trim();
                lb_appNo.Text = dr["p2apno"].ToString().Trim();
                lb_sex.Text = dr["m00sex"].ToString().Trim() == "M" ? "Male" : "Female";
                lb_ebc.Text = dr["m00ebc"].ToString().Trim() == "0000000000000000" ? "-" : dr["m00ebc"].ToString().Trim();
                lb_ID.Text = dr["m00idn"].ToString().Trim();
                lb_csn.Text = dr["p2csno"].ToString().Trim();
                lb_cont.Text = dr["p2cont"].ToString().Trim();
                lb_loan_amt.Text = dr["p2pcam"].ToString().Trim() != "" ? String.Format("{0:n}", dr["p2pcam"]) : "0.00";
                lb_contract_AM.Text = dr["p2toam"].ToString().Trim() != "" ? String.Format("{0:n}", dr["p2toam"]) : "0.00";
                lb_prod_code.Text = dr["p2item"].ToString().Trim();
                lb_prod.Text = dr["t44des"].ToString().Trim();
                lb_qty.Text = dr["p2qty"].ToString().Trim();
                lb_qty.Text = dr["p2qty"].ToString().Trim();
                lb_credit.Text = dr["p3crlm"].ToString().Trim() != "" ? String.Format("{0:n}", dr["p3crlm"]) : "0.00";
                lb_lmA.Text = dr["p3pcam"].ToString().Trim() != "" ? String.Format("{0:n}", dr["p3pcam"]) : "0.00";
                bindResCode();
                //btn_save.Enabled = true;
                PN_AddNote.Enabled = true;

            }
            else
            {
                fn_clear();
                lb_err.Text = "Not Found";
                PN_AddNote.Enabled = false;
                //btn_save.Enabled = false;



            }
        }
        catch (Exception ex)
        {
            lb_err.Text = "Error";
        }
    }

    private bool validateBeforeSave()
    {
        try
        {
            //Check Input Error Format 
            //if (txt_memoReason.Text.Trim().IndexOf("'") >= 0 || txt_memoReason.Text.Trim().IndexOf("\"") >= 0
            //    || txt_memoReason.Text.Trim().IndexOf("?") >= 0 || txt_memoReason.Text.Trim().IndexOf("+") >= 0
            //    || txt_memoReason.Text.Trim().IndexOf("%") >= 0 || txt_memoReason.Text.Trim().IndexOf("!") >= 0
            //    || txt_memoReason.Text.Trim().IndexOf("=") >= 0
            //    || txt_memoReason.Text.Trim().IndexOf(":") >= 0 || txt_memoReason.Text.Trim().IndexOf(";") >= 0
            //    || txt_memoReason.Text.Trim().IndexOf("<") >= 0 || txt_memoReason.Text.Trim().IndexOf(">") >= 0
            //    )
            if (txt_memoReason.Text.Trim().IndexOf("'") >= 0 ||
               txt_memoReason.Text.Trim().IndexOf("\"") >= 0 ||
               txt_memoReason.Text.Trim().IndexOf("\\r") >= 0 ||
               txt_memoReason.Text.Trim().IndexOf("\\t") >= 0 ||
               txt_memoReason.Text.Trim().IndexOf("\\n") >= 0)
            {
                lblMsg.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ \r \n \t  ";
                txt_memoReason.Focus();
                return false;
            }
            if (dd_ActionCode.Value == null || dd_ResCode.Value == null)
            {
                lblMsg.Text = "Please input Action code or result code.";
                lblMsg.Visible = true;
                return false;

            }
            if (txt_memoReason.Text.Trim() == "")
            {
                lblMsg.Text = "Please input note detail.";
                lblMsg.Visible = true;
                return false;
            }

            if (txt_memoReason.Text.Trim().Length > 200)
            {
                lblMsg.Text = "Lengh of note detail > 200 Characters.";
                lblMsg.Visible = true;
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error ,Please check data ";
            lblMsg.Visible = true;
            return false;
        }

    }
    private void checkUpdateSalary()
    {
        ILDataCenter ilObj = new ILDataCenter();
        UserInfo m_userInfo = userInfoService.GetUserInfo();
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        CallEntCancelUnsign = new ILDataCenterCancelUnsign(m_userInfo);
        try
        {
            lblConfirmMsgEn.Text = " Do you want to Update Cancel Unsign ? ";
            lblConfirmSal.Text = "";
            ilObj.UserInfomation = m_userInfo;
            DataSet ds = CallEntCancelUnsign.get_csms032(lb_csn.Text.Trim(), m_userInfo.BranchApp, lb_appNo.Text.Trim());
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds))
            {

                lblConfirmSal.Text = " This case will change salary to " + ds.Tables[0].Rows[0]["d3osal"].ToString();
            }

            PopupConfirmSave.ShowOnPageLoad = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }

    private void save_data()
    {
        UserInfo m_userInfo = userInfoService.GetUserInfo();

        ILDataCenter ilObj = new ILDataCenter();
        iLDataSubroutine = new ILDataSubroutine(userInfo);
        CallEntCancelUnsign = new ILDataCenterCancelUnsign(userInfo);
        CallMasterEnt = new ILDataCenterMssql(m_userInfo);
        try
        {
            lblMsg.Text = "";

            ilObj.UserInfomation = m_userInfo;

            string brn = m_userInfo.BranchApp;
            string M20AVI = "";
            string P3PCAM = "";

            string ErrMsgNote = "";

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
            Connect_NoteAPI noteAPI = new Connect_NoteAPI();
            //bool AddNote = iLDataSubroutine.CALL_CSSRW11("IL", lb_ID.Text.Trim(), dd_ActionCode.SelectedItem.Value.ToString(),
            //                                  dd_ResCode.SelectedItem.Value.ToString(), desc1, desc2, ref ErrMsgNote, m_userInfo.BizInit, m_userInfo.BranchNo);
            var resNote = noteAPI.AddNote(lb_ID.Text.Trim().Trim(), "0", dd_ActionCode.SelectedItem.Value.ToString(), dd_ResCode.SelectedItem.Value.ToString(), desc1 + desc2, m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

            if (!resNote.success || ErrMsgNote.Trim() != "")
                //if (!AddNote || ErrMsgNote.Trim() != "")
            {
                lblMsg.Text = "Save not complete  ";
                return;
            }

            iDB2Command cmd = new iDB2Command();
            bool transaction;
            // insert into csms20 //
            DataSet ds_csms20 = CallEntCancelUnsign.get_csms20(lb_csn.Text.Trim());
            CallMasterEnt._dataCenter.CloseConnectSQL();
            DataSet ds_ilms03 = CallEntCancelUnsign.get_ilms03(lb_csn.Text.Trim());
            CallMasterEnt._dataCenter.CloseConnectSQL();
            if (ilObj.check_dataset(ds_csms20))
            {
                DataRow dr_20 = ds_csms20.Tables[0].Rows[0];
                M20AVI = dr_20["M20AVI"].ToString().Trim();
                //cmd.Parameters.Clear();
                //cmd.CommandText = ilObj.insertCSMS20HS(lb_csn.Text);
                //int resCSMS20HS = ilObj.ExecuteNonQuery(cmd);
                //if (resCSMS20HS == -1)
                //{
                //    ilObj.RollbackDAL();
                //    ilObj.CloseConnectioDAL();
                //    lblMsg.Text = "Save not complete ";
                //    return;
                //}
                cmd.Parameters.Clear();
                cmd.CommandText = CallEntCancelUnsign.updateCSMS20(M20AVI, lb_csn.Text.Trim(), "IL_UNSIGN", m_userInfo.Username.Trim());
                transaction = CallEntCancelUnsign._dataCenter.Sqltr == null ? true : false;
                var resCSMS20 = CallEntCancelUnsign._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (resCSMS20.afrows == -1)
                {
                    Utility.WriteLogString(resCSMS20.message.ToString(), cmd.CommandText.ToString());
                    CallEntCancelUnsign._dataCenter.RollbackMssql();
                    CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Save not complete ";
                    return;
                }

            }

            // *** insert ilms03 ***//
            if (ilObj.check_dataset(ds_ilms03))
            {
                DataRow dr_03 = ds_ilms03.Tables[0].Rows[0];
                P3PCAM = "0";//dr_03["P3PCAM"].ToString();
                //cmd.Parameters.Clear();
                //cmd.CommandText = ilObj.insertILMS03HS(lb_csn.Text.Trim(), m_userInfo.Username.Trim(), m_userInfo.LocalClient.Trim());
                //int resILMS03HS = ilObj.ExecuteNonQuery(cmd);
                //if (resILMS03HS == -1)
                //{
                //    ilObj.RollbackDAL();
                //    ilObj.CloseConnectioDAL();
                //    lblMsg.Text = "Save not complete ";
                //    return;
                //}
                cmd.Parameters.Clear();
                cmd.CommandText = CallEntCancelUnsign.UpdateILMS03(P3PCAM, lb_csn.Text.Trim(), "IL_UNSIGN", m_userInfo.Username.Trim(), m_userInfo.LocalClient.Trim());
                transaction = CallEntCancelUnsign._dataCenter.Sqltr == null ? true : false;
                var resILMS03 = CallEntCancelUnsign._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
                if (resILMS03.afrows == -1)
                {
                    Utility.WriteLogString(resILMS03.message.ToString(), cmd.CommandText.ToString());
                    CallEntCancelUnsign._dataCenter.RollbackMssql();
                    CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                    lblMsg.Text = "Save not complete ";
                    return;
                }
               
            }

            //***  insert ilms01hs ***//
            //cmd.Parameters.Clear();
            //cmd.CommandText = ilObj.InsertILMS01HS_autoPay(m_UpdDate.ToString(), m_UpdTime.ToString(), m_userInfo.Username,
            //                                               m_userInfo.LocalClient, brn, lb_appNo.Text.Trim());
            //int resILMS01HS = ilObj.ExecuteNonQuery(cmd);
            //if (resILMS01HS == -1)
            //{
            //    ilObj.RollbackDAL();
            //    ilObj.CloseConnectioDAL();
            //    lblMsg.Text = "Save not complete ";
            //    return;
            //}

            string date_User = m_UpdDate.ToString() + m_userInfo.Username;
            cmd.Parameters.Clear();
            cmd.CommandText = CallEntCancelUnsign.UpdateILMS01_cancelUnsign(date_User, brn, lb_appNo.Text.Trim(), "IL_UNSIGN", m_userInfo.Username, m_userInfo.LocalClient);
            transaction = CallEntCancelUnsign._dataCenter.Sqltr == null ? true : false;
            var resILMS01 = CallEntCancelUnsign._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (resILMS01.afrows == -1)
            {
                Utility.WriteLogString(resILMS01.message.ToString(), cmd.CommandText.ToString());
                CallEntCancelUnsign._dataCenter.RollbackMssql();
                CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                lblMsg.Text = "Save not complete ";
                return;
            }
            

            //*** insert ilms02 ***//
            //cmd.Parameters.Clear();
            //cmd.CommandText = ilObj.insertILMS02HS(brn, lb_cont.Text.Trim());
            //int resILMS02HS = ilObj.ExecuteNonQuery(cmd);
            //if (resILMS02HS == -1)
            //{
            //    ilObj.RollbackDAL();
            //    ilObj.CloseConnectioDAL();
            //    lblMsg.Text = "Save not complete ";
            //    return;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = CallEntCancelUnsign.updateILMS02(brn, lb_cont.Text.Trim(), lb_appNo.Text.Trim(), "IL_UNSIGN");
            transaction = CallEntCancelUnsign._dataCenter.Sqltr == null ? true : false;
            var resUpdateILMS02 = CallEntCancelUnsign._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result;
            if (resUpdateILMS02.afrows == -1)
            {
                Utility.WriteLogString(resUpdateILMS02.message.ToString(), cmd.CommandText.ToString());
                CallEntCancelUnsign._dataCenter.RollbackMssql();
                CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                lblMsg.Text = "Save not complete ";
                return;
            }
            
            string PRTNCD = "";
            string PERMSG1 = "";

            bool res_cssr032 = iLDataSubroutine.CALL_CSSR032("C", "T", "N", lb_csn.Text.Trim(), "IL", m_userInfo.BranchApp.ToString(), lb_appNo.Text, "0", "0", ref PRTNCD, ref PERMSG1, m_userInfo.BizInit, m_userInfo.BranchNo);
            if (!res_cssr032 || PRTNCD.Trim() != "000")
            {
                Utility.WriteLogString(res_cssr032.ToString(), cmd.CommandText.ToString());
                CallEntCancelUnsign._dataCenter.RollbackMssql();
                CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                lblMsg.Text = "Save not complete (032) " + PERMSG1;
                return;
            }

            string ErrCode = "";
            string ErrMsg = "";

            bool res_cssr034 = iLDataSubroutine.Call_CSSR034("C", "C", "", "", lb_csn.Text.Trim(), "IL", m_userInfo.BranchApp, "0", "0", "0",
                                                  ref ErrCode, ref ErrMsg, m_userInfo.BizInit, m_userInfo.BranchNo);
            if (!res_cssr034 || ErrCode.Trim() != "000")
            {
                Utility.WriteLogString(res_cssr034.ToString(), cmd.CommandText.ToString());
                CallEntCancelUnsign._dataCenter.RollbackMssql();
                CallEntCancelUnsign._dataCenter.CloseConnectSQL();
                lblMsg.Text = "Save not complete (034) " + ErrMsg;
                return;
            }

            CallEntCancelUnsign._dataCenter.CommitMssql();
            CallEntCancelUnsign._dataCenter.CloseConnectSQL();
            fn_clear();
            lblMsg.Text = "Save  complete ";
            return;

        }
        catch (Exception ex)
        {
            CallEntCancelUnsign._dataCenter.RollbackMssql();
            CallEntCancelUnsign._dataCenter.CloseConnectSQL();
            Utility.WriteLog(ex);
            lblMsg.Text = "Save not complete (Exception) ";
            return;
        }
    }

    protected void btnConfirmSave_Click(object sender, EventArgs e)
    {
        save_data();
    }
    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {

    }

    private void fn_clear()
    {
        txt_search.Text = "";
        lb_err.Text = "";
        lb_name.Text = "";
        lb_appNo.Text = "";
        lb_ebc.Text = "";
        lb_cont.Text = "";
        lb_prod.Text = "";
        lb_prod_code.Text = "";
        lb_contract_AM.Text = "";
        lb_credit.Text = "";
        lb_sex.Text = "";
        lb_ID.Text = "";
        lb_csn.Text = "";
        lb_loan_amt.Text = "";
        lb_qty.Text = "";
        lb_lmA.Text = "";
        dd_ActionCode.Value = "ADD";
        dd_ResCode.SelectedIndex = -1;
        txt_memoReason.Text = "";
        lblMsg.Text = "";
        PN_AddNote.Enabled = false;
        //btn_save.Enabled = false;
        //btn_cancel.Enabled = true;

    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        fn_clear();
    }
    protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    {
        try
        {

            Cache.Remove("dsResCodeNote");
            Cache.Remove("dsActionCodeNote");
            bindResCode();


        }
        catch (Exception ex)
        {

        }

    }
    protected void dd_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_search.Text = "";
            if (dd_search.SelectedValue == "1")
            {
                txt_search.MaskSettings.Mask = "90-000000000";
            }
            else if (dd_search.SelectedValue == "2")
            {
                txt_search.MaskSettings.Mask = "0000-000-000000000";
            }
        }
        catch (Exception ex)
        {
        }
    }

}
