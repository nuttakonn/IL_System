using DevExpress.Web.ASPxEditors;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using ILSystem.App_Code.BLL.DataCenter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ESB.WebAppl.ILSystem.commons;
using ILSystem.App_Code.Helper;
using ILSystem.App_Code.Model;
using ILSystem.App_Code.Model.CompanyBlacklist;
//using static ILSystem.App_Code.Commons.Connect_AmloAPI;
using EB_Service.Commons;
using ILSystem.App_Code.BLL.Integrate;

public partial class ManageData_WorkProcess_UserControl_UC_TCL : System.Web.UI.UserControl
{
    private ILDataCenter ilObj;
    ILDataSubroutine iLDataSubroutine;
    ILDataCenterMssql CallHisunMaster;
    ILDataCenterMssqlTCL iLDataCenterMssqlTCL;
    ILDataCenterMssqlUsingCard iLDataCenterMssqlUsing;
    public UserInfoService userInfoService;
    //Connect_GeneralAPI generalApi;
    UserInfo userInfo;
    public delegate void ManageData_WorkProcess_WebUserControlDelegate(DataSet ds_term, DataSet ds_installment);
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

    private readonly connectAPI _connectAPI;

    public ManageData_WorkProcess_UserControl_UC_TCL()
    {
        _connectAPI = new connectAPI();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }

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

        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();

        DataSet DS = new DataSet();
        EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(userInfoService.GetUserInfo());

        D_shipto1.Items.Clear();
        //DS = busobj.RetriveAsDataSet("Select t11adt, t11tds from cstb11 where t11adt in ('H','O') order by t11adt ");

        var resultShipping = MSSQL.GetDataset<DataSet>($@"Select t11adt, t11tds from AS400DB01.CSOD0001.cstb11 where t11adt in ('H','O') order by t11adt ", CommandType.Text).Result;

        DS = resultShipping.data;

        if (DS != null && DS.Tables.Count > 0)
        {
            D_shipto1.Items.Add(new ListEditItem("Select Ship To", ""));
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                D_shipto1.Items.Add(
                       new ListEditItem(dr["t11adt"].ToString().Trim() + " : " + dr["t11tds"].ToString().Trim(), dr["t11adt"].ToString().Trim()));
            }
            DS.Clear();
        }

        D_building1.Items.Clear();
        //DS = busobj.RetriveAsDataSet("select gt08tc, gt08td from gntb08 order by GT08TC ");
        //ติดไว้ก่อน
        DS = MSSQL.GetDataset<DataSet>($@"SELECT code gt08tc,DescriptionTHAI gt08td FROM GeneralDB01.GeneralInfo.GeneralCenter WHERE Type = 'BuildingTitleID' order by Code ", CommandType.Text).Result.data;

        if (DS != null && DS.Tables.Count > 0)
        {
            D_building1.Items.Add(new ListEditItem("Select Building", ""));
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                D_building1.Items.Add(
                       new ListEditItem(dr["gt08td"].ToString().Trim(), dr["gt08tc"].ToString().Trim()));
            }
            DS.Clear();
        }

        /*
        DS = busobj.RetriveAsDataSet("select p1pram, p1vdid from ilms01 where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ");
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                Loan_Amt.Text = dr["p1pram"].ToString().Trim();
                G_vendor.Text = dr["p1vdid"].ToString().Trim();
            }
            DS.Clear();
        }
        */

        MSSQL.CloseConnectSQL();
    }

    public void bindData()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        DataCenter MSSQL = new DataCenter(userInfoService.GetUserInfo());
        iLDataCenterMssqlUsing = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
        try
        {
            if (hid_status.Value == "INTERVIEW")
            {
                B_return.ClientVisible = false;
                B_return.Visible = false;
                P_tcl_kessaiview.Visible = false;
            }
            else
            {
                P_tcl_kessaiview.Visible = true;
            }

            if ((hid_AppNo.Value == null) || (hid_AppNo.Value.ToString() == ""))
            {
                return;
            }

            bindTerm();
            bindVendor();

            ILDataCenter busobj = new ILDataCenter();
            //DataCenter MSSQL = new DataCenter();
            busobj.UserInfomation = userInfoService.GetUserInfo();
            DataSet DS = new DataSet();
            DataSet DS_ = new DataSet();
            try
            {

                //DS = busobj.RetriveAsDataSet("select distinct digits(p1vdid) as p1vdid, p1term, p10ven, p10nam, p10fi1, p16rnk, " +
                //                             "p1camp, p1pric, p1qty, p1down, p1pram, p1item, " +
                //                             "c01cmp, c01cnm, c02csq, c02rsq, t44itm, t44des, t44pgp, c07min, c07max, " +
                //                             "C01CTY, c02rsq, c01nxd, C02AIR, C02ACR, " +
                //                             "case when substr(p1fill,26,2) = 'AP' then 'Approve' " +
                //                             "when substr(p1fill,26,2) = 'CN' then 'Cancel' " +
                //                             "when substr(p1fill,26,2) = 'RJ' then 'Reject' else '' end as Int_Status,p1aprj " +
                //                             "from ilms01 " +
                //                             "left join ilms10 on(p1vdid = p10ven) " +
                //                             "left join ilms16 on(p10VEN = P16VEN and p16std <= " + hid_date97.Value + " and p16end >= " + hid_date97.Value + ") " +
                //                             "left join ilcp01 on(p1camp = c01cmp) " +
                //                             "left join ilcp02 on(c01cmp = c02cmp and c02tot = p1term) " +
                //                             "left join ilcp07 on(c02cmp = c07cmp and c02csq = c07csq and (c07pit = p1item or c07pit = 0)) " +
                //                             "left join iltb44 on(p1item = t44itm) " +
                //                             "where p1brn= " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ");

                DS = MSSQL.GetDataset<DataSet>($@"select distinct Cast(p1vdid as float) as p1vdid, p1term, p10ven, p10nam, p10fi1, p16rnk, 
                                             p1camp, p1pric, p1qty, p1down, p1pram, p1item, 
                                             c01cmp, c01cnm, c02csq, c02rsq, t44itm, t44des, t44pgp, c07min, c07max, 
                                             C01CTY, c02rsq, c01nxd, C02AIR, C02ACR, 
                                             case when substring(p1fill,26,2) = 'AP' then 'Approve' 
                                             when substring(p1fill,26,2) = 'CN' then 'Cancel' 
                                             when substring(p1fill,26,2) = 'RJ' then 'Reject' else '' end as Int_Status,p1aprj 
                                             from AS400DB01.ILOD0001.ilms01 
                                             left join AS400DB01.ILOD0001.ilms10 on(p1vdid = p10ven) 
                                             left join AS400DB01.ILOD0001.ilms16 on(p10VEN = P16VEN and CAST(p16std as nvarchar) <=  '{hid_date97.Value}'   and CAST(p16end as nvarchar) >=  '{hid_date97.Value}'  ) 
                                             left join AS400DB01.ILOD0001.ilcp01 on(p1camp = c01cmp) 
                                             left join AS400DB01.ILOD0001.ilcp02 on(c01cmp = c02cmp and c02tot = p1term) 
                                             left join AS400DB01.ILOD0001.ilcp07 on(c02cmp = c07cmp and c02csq = c07csq and (c07pit = p1item or c07pit = 0)) 
                                             left join AS400DB01.ILOD0001.iltb44 on(p1item = t44itm) 
                                             where CAST(p1brn as nvarchar)= '{hid_brn.Value}'   and CAST(p1apno as nvarchar) = '{hid_AppNo.Value}' ", CommandType.Text).Result.data;

                //DS_ = busobj.RetriveAsDataSet("Select w5cusr as I_CUST, w5ousr as I_TO, w5husr as I_TH, w5musr as I_TM, w5eusr as I_TE, " +
                //        "w5caus as K_CUST, w5oaus as K_TO, w5haus as K_TH, w5maus as K_TM, w5eaus as K_TE, " +
                //        "trim(w5csty) as CustType, W5SBCD as SubCustType, w5emst as EmployeeType, w5vrth as not_have_th, w5vrtm as not_have_tm, " +
                //        "w5salt as ver01, w5inet as ver02, w5sso as ver03, w5bol as ver04, w5voot as ver07, w5vonm as ver08, " +
                //        "w5voad as ver09, w5emst as ver10, w5rvto as ver11, w5ipto as ver12, w5tito as ver12_1, w5tyem as ver13, " +
                //        "w5htl as ver15, w5vhht as ver16, w5vhnm as ver17, w5vhad as ver18, w5tytl as ver19, w5rvth as ver20, w5ipth as ver21, w5tith as ver21_1, " +
                //        "w5mbtl as ver22, w5vmtl as ver23, w5fvto, w5fvth, w5fvtm, w5fvte " +
                //        "from ilwk05 " +
                //        "where w5brn = " + hid_brn.Value + " and w5apno = " + hid_AppNo.Value + " ");

                DS_ = MSSQL.GetDataset<DataSet>($@"Select w5cusr as I_CUST, w5ousr as I_TO, w5husr as I_TH, w5musr as I_TM, w5eusr as I_TE, 
                        w5caus as K_CUST, w5oaus as K_TO, w5haus as K_TH, w5maus as K_TM, w5eaus as K_TE, 
                        trim(w5csty) as CustType, W5SBCD as SubCustType, w5emst as EmployeeType, w5vrth as not_have_th, w5vrtm as not_have_tm, 
                        w5salt as ver01, w5inet as ver02, w5sso as ver03, w5bol as ver04, w5voot as ver07, w5vonm as ver08, 
                        w5voad as ver09, w5emst as ver10, w5rvto as ver11, w5ipto as ver12, w5tito as ver12_1, w5tyem as ver13, 
                        w5htl as ver15, w5vhht as ver16, w5vhnm as ver17, w5vhad as ver18, w5tytl as ver19, w5rvth as ver20, w5ipth as ver21, w5tith as ver21_1, 
                        w5mbtl as ver22, w5vmtl as ver23, w5fvto, w5fvth, w5fvtm, w5fvte 
                        from AS400DB01.ILOD0001.ilwk05 
                        where cast(w5brn as nvarchar) = '{hid_brn.Value}'  and cast(w5apno as nvarchar) = '{hid_AppNo.Value}' ", CommandType.Text).Result.data;

            }
            catch (Exception)
            {
                MSSQL.CloseConnectSQL();
            }

            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["Int_Status"].ToString().Trim() != "")
                {
                    L_Interview_status.Text = "Interview : " + dr["Int_Status"].ToString().Trim();
                }
                // add  p1aprj //
                hid_pending.Value = dr["p1aprj"].ToString().Trim();
                Loan_Amt.Text = dr["p1pram"].ToString().Trim();
                dd_totalterm.Value = dr["p1term"].ToString().Trim();
                dd_vendor.Value = dr["p1vdid"].ToString().Trim() + "|" + dr["P16RNK"].ToString().Trim();

                dd_campaign.Items.Add(new ListEditItem(dr["c01cmp"].ToString().Trim() + ":" + dr["c01cnm"].ToString().Trim(),
                                                       dr["c01cmp"].ToString().Trim() + "|" + dr["c02csq"].ToString().Trim() + "|" + dr["c02rsq"].ToString().Trim()));
                dd_campaign.SelectedIndex = 1;
                txt_campSeq.Text = dr["c02csq"].ToString().Trim();
                txt_campgType.Text = dr["c01cty"].ToString().Trim();
                txt_total_range.Text = dr["c02rsq"].ToString().Trim();
                txt_non.Text = dr["c01nxd"].ToString().Trim();
                txt_Int.Text = float.Parse(dr["c02air"].ToString().Trim()).ToString("0.00");
                txt_cru.Text = float.Parse(dr["c02acr"].ToString().Trim()).ToString("0.00");
                E_vendor.Text = dr["p1vdid"].ToString().Trim();
                E_product.Text = dr["p1item"].ToString().Trim();
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


            if (hid_status.Value == "INTERVIEW")
            {
                B_return.ClientVisible = false;
                B_return.Visible = false;
            }
            ClearAddr();
            Load_CustomerData_ShipToAddress();

            if (hid_status.Value == "KESSAI")
            {

                if (busobj.check_dataset(DS_))
                {
                    DataRow dr = DS_.Tables[0].Rows[0];
                    string t22cod = "", t22seq = "", sqlret = "", ver_contact = dr["ver11"].ToString().Trim();
                    bool notHaveTH = dr["not_have_th"].ToString().Trim() == "Y" ? true : false;
                    //ติดไว้ก่อน
                    if (!iLDataCenterMssqlUsing.checkApproveCriteria(busobj, E_product.Text, E_vendor.Text, userInfo.BranchApp, hid_AppNo.Value, hid_appdate.Value, "01", hid_CSN.Value,
                                                    hid_idno.Value, hid_date97.Value, "ILNORMWEB", notHaveTH/*false*/, userInfo.BizInit, userInfo.BranchNo, userInfo.Username, userInfo.LocalClient,
                                                    ref t22cod, ref t22seq, ref sqlret, ver_contact))
                    {
                        //err_ += "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา" + "\r\n";
                        //res_err = false;
                        L_message.ForeColor = System.Drawing.Color.Red;
                        L_message.Text = " Can not Approve this customer. Check by condition code : " + t22cod + " Seq: " + t22seq;
                        P_message_TCL.ShowOnPageLoad = true;
                        MSSQL.CloseConnectSQL();
                        return;
                    }
                }
                MSSQL.CloseConnectSQL();
            }
            MSSQL.CloseConnectSQL();
        }
        catch
        {
            MSSQL.CloseConnectSQL();
        }
        /*
        calTCL();

        string err = "";
        DataSet ds = new DataSet();
        if (calculateInstallment(ref err, ref ds))
        {
            if (ilObj.check_dataset(ds))
            {
                gvTerm.DataSource = ds.Tables[0];
                gvTerm.DataBind();

                gv_install.DataSource = ds.Tables[1];
                gv_install.DataBind();
            }
        }
        else
        {
            return;
        }
        */

    }

    private void bindTerm()
    {
        int i = 0;
        dd_totalterm.Items.Clear();
        dd_totalterm.Items.Add("");
        for (i = 1; i <= 60; i++)
        {
            dd_totalterm.Items.Add(new ListEditItem(i.ToString(), i.ToString()));
        }
        dd_totalterm.Value = "12";
    }

    private void bindVendor()
    {
        try
        {
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ilObj = new ILDataCenter();
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();
            DataSet ds = iLDataSubroutine.getVendor("", userInfo.BranchApp);
            CallHisunMaster._dataCenter.CloseConnectSQL();

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
        }
        catch (Exception ex)
        {
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
            ilObj = new ILDataCenter();
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
            iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
            ilObj.UserInfomation = userInfoService.GetUserInfo();

            float loanRequest = 0;
            try
            {
                float approveAVL = float.Parse(E_Apv_avi.Text, NumberStyles.Currency);
            }
            catch { }

            //*** check condition ***//
            /*
            if (!checkBeforeCaculateProduct(ref err))
            {
                //L_error_product.Text = err.ToString();
                return false;
            }
            */

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

            if (!((loanRequest > 0) && (loanRequest <= Total_pur)))
            {
                return false;
            }

            if (loanRequest > crAVL)
            {
                return false;
            }

            if (((loanRequest / qty) < minProc) && (minProc > 0))
            {
                err = " loan request จะต้องไม่น้อยกว่า " + txt_minPrice.Text;
                return false;
            }
            if (((loanRequest / qty) > maxProc) && (maxProc > 0))
            {
                err = " loan request จะต้องไม่มากกว่า " + txt_maxPrice.Text;
                return false;
            }

            string AppDate = hid_appdate.Value.ToString();
            if (AppDate.Trim() == "")
            {
                return false;
            }

            DataSet ds_ilTB06 = iLDataSubroutine.getILTB06();
            if (!ilObj.check_dataset(ds_ilTB06))
            {
                return false;
            }
            string[] campaign = dd_campaign.SelectedItem.Value.ToString().Split('|');
            DataSet ds_camp = CallHisunMaster.getCampaign(vendor[0], dd_totalterm.SelectedItem.Value.ToString(), campaign[0], campaign[1], "", AppDate);
            if (!ilObj.check_dataset(ds_camp))
            {
                return false;
            }

            //***** compound string *****//
            DataRow dr_iltb06 = ds_ilTB06.Tables[0].Rows[0];
            DataRow dr_campaign = ds_camp.Tables[0].Rows[0];

            string compound = txt_loanReq.Text.Trim().Replace(".", "").PadLeft(13, '0') + " " +
                              appDateUser.Substring(6, 2).ToString() + appDateUser.Substring(4, 2).ToString() + appDateUser.Substring(0, 4).ToString() + " " +
                              dd_totalterm.SelectedItem.Value.ToString().PadLeft(3, '0') + " " +
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

                /*
                LongTerm:=LongTerm+copy(inttostr(1000+
              (QuerySrhRateTerm.fieldbyname('c02tot').asinteger-
              QuerySrhRateTerm.fieldbyname('c02fmt').asinteger)+1),2,3);
                */
                PITERM = PITERM + (1000 + (c02tot - c02fmt) + 1).ToString().Substring(1, 3);

                //if ((intrate > 0) && (dr["c02inr"].ToString().Trim().Length > 1))  
                if ((Convert.ToDouble(intrate) > 0) && (dr["c02inr"].ToString().Length > 1))
                {
                    PIINTR = PIINTR + (intrate.ToString().Replace(".", "")).PadLeft(5, '0');  //(1000 + intrate).ToString().Substring(1, 3) + (100.00 + intrate).ToString().Substring(4, 2);
                }
                //else if ((intrate < 0) && (dr["c02inr"].ToString().Trim().Length > 1))
                else if ((Convert.ToDouble(intrate) < 0) && (dr["c02inr"].ToString().Length > 1))
                {
                    PIINTR = "-" + (PIINTR + (1000 - Convert.ToDouble(intrate)).ToString().Substring(1, 3) + (100.00 - Convert.ToDouble(intrate)).ToString("##0.00").Substring(4, 2)).Substring(1, 4);
                    //PIINTR = PIINTR + (intrate.ToString().Replace(".", "").PadLeft(4, '0'));  อันเดิม   //(PITERM + ((1000 - intrate).ToString().Substring(1, 3)) + ((100.00 - intrate).ToString().Substring(4, 2))).Substring(1,4); 

                }

                //else if ((intrate != 0) && (dr["c02inr"].ToString().Trim().Length == 1))
                else if ((Convert.ToDouble(intrate) != 0) && (dr["c02inr"].ToString()).ToString().Length == 1)
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
                //ilObj.CloseConnectioDAL();
            }
            else
            {
                res24 = iLDataSubroutine.Call_ILSREIR(PITEXT, PITERM, PIINTR, PICRUR,
                               ref POPCAM, ref POINTR, ref POCRUR, ref POINFR,
                               ref PODIFR, ref POINST, ref POTOAM, ref PODUTY,
                               ref POINTB, ref POCRUB, ref POINFB, ref POCONA,
                               ref POFDAT, ref POAINR, ref POACRU, ref PODDAT,
                               ref POPPRN, ref POINSD, ref POINTD, ref POCRUD,
                               ref POINFD, ref POCDAT, ref POINCM, ref POREBT,
                               ref POCLSA, ref POFLAG, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                //ilObj.CloseConnectioDAL();
            }


            if (res24)
            {
                string Bureau = "0.00";

                //***  ncb ***//

                if (hid_appdate.Value.Trim() != "")
                {
                    if (hid_date97.Value.Trim() == "")
                    {
                        return false;
                    }
                    //DataSet ds_ncb = ilObj.getNCB(AppDate.PadLeft(8, '0').Substring(4, 4) + AppDate.PadLeft(8, '0').Substring(2, 2) + AppDate.PadLeft(8, '0').Substring(0, 2));
                    //** NCBPJ REQ 70667 **    
                    //DataSet ds_ncb = ilObj.getNCB(hid_date97.Value);
                    //if (ilObj.check_dataset(ds_ncb))
                    //{

                    //    //DataRow dr_ncb = ds_ncb.Tables[0].Rows[ds_ncb.Tables[0].Rows.Count - 1];
                    //    //DataRow[] dr_ncb = ds_ncb.Tables[0].Rows;
                    //    if (ds_ncb.Tables[0].Rows.Count > 1)
                    //    {
                    //        DataSet ds_ilms01 = ilObj.get_ilms01_setTimeReceive(hid_AppNo.Value, hid_brn.Value);
                    //        string cust_ncb = ds_ilms01.Tables[0].Rows[0]["p1fill"].ToString().Substring(20, 1);
                    //        //DataRow dr_ncb = ds_ncb.Tables[0].Select(" substring(G00FIL,0,1) =  '" + cust_ncb+"'");


                    //        foreach (DataRow dr in ds_ncb.Tables[0].Rows)
                    //        {
                    //            if (dr["G00FIL"].ToString().Substring(2, 1).Trim() != "")
                    //            {
                    //                if (cust_ncb == dr["G00FIL"].ToString().Substring(2, 1))
                    //                {
                    //                    Bureau = dr["G00AMT"].ToString();
                    //                    Bureau = "0.00";
                    //                    break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                Bureau = dr["G00AMT"].ToString();
                    //                Bureau = "0.00";
                    //            }
                    //        }
                    //    }
                    //    else if (ds_ncb.Tables[0].Rows.Count == 1)
                    //    {
                    //        DataRow dr_ncb = ds_ncb.Tables[0].Rows[ds_ncb.Tables[0].Rows.Count - 1];
                    //        Bureau = dr_ncb["G00AMT"].ToString();
                    //        Bureau = "0.00";
                    //    }

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
                float int_p_sum = 0;
                float int_amt_sum = 0; // cell 3
                float cru_p_sum = 0;  // cell 4
                float cru_amt_sum = 0; // cell 5
                float int_free_sum = 0; // cell 6
                float total_amt_sum = 0; // cell 7
                float oth_sum = 0; // cell 8
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
                    total_amt = convCurrency((float.Parse(POTOAM.Substring(len_7, 9) + "." + POTOAM.Substring(len_7 + 9, 2)) - float.Parse(oth)).ToString());

                    // Principal //**cell 9 **//
                    principal = convCurrency((float.Parse(POPCAM.Substring(len_9, 11) + "." + POPCAM.Substring(len_9 + 11, 2))).ToString());

                    //  Install // ** cell 10 **// 
                    install = convCurrency(float.Parse(POINST.Substring(len_10, 7)) + "." + "00").ToString();

                    if (row == 1)
                    {
                        if (oldcal)
                        {
                            installFirst = decimal.Parse(POINST.Substring(len_10, 7)) + decimal.Parse(PODUTY) + decimal.Parse(Bureau);
                        }
                        else
                        {
                            installFirst = (decimal.Parse(POINST.Substring(len_10, 7)) / 100) + decimal.Parse(PODUTY) + decimal.Parse(Bureau);
                        }

                    }

                    inteir = int_amt.ToString();
                    crueir = cru_amt.ToString();
                    if (hid_loantyp.Value == "02")
                    {
                        cru_amt = "0";
                        int_amt = "0";

                    }
                    dt.Rows.Add(from_t, to_t, int_p, int_amt, cru_p, cru_amt, int_free, total_amt, oth, principal, install);

                    //***  sum data ***//
                    int_p_sum += float.Parse(int_p);
                    int_amt_sum += float.Parse(int_amt);
                    cru_p_sum += float.Parse(cru_p);
                    cru_amt_sum += float.Parse(cru_amt);
                    int_free_sum += float.Parse(int_free);
                    total_amt_sum += float.Parse(total_amt);
                    oth_sum += float.Parse(oth);
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
                hid_inteirsum.Value = inteir_sum.ToString();
                hid_crueirsum.Value = crueir_sum.ToString();
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
                float amt_next = 0;
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
                        if (oldcal)
                        {
                            installment = convCurrency(POINSD.Substring(len_3, 7));
                            if (row == term)
                            {
                                hid_lastinstallment.Value = POINSD.Substring(len_3, 7);
                            }
                            else
                            {
                                hid_installment.Value = POINSD.Substring(len_3, 7);
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


                    //installment = convCurrency(POINSD.Substring(len_3, 7));
                    interest = convCurrency(POINTD.Substring(len_4, 10) + "." + POINTD.Substring(len_4 + 10, 2));
                    cr_use = convCurrency(POCRUD.Substring(len_5, 10) + "." + POCRUD.Substring(len_5 + 10, 2));
                    income = convCurrency(POINFD.Substring(len_6, 10) + "." + POINFD.Substring(len_6 + 10, 2));
                    cur_princ = convCurrency(POPPRN.Substring(len_7, 9) + "." + POPPRN.Substring(len_7 + 9, 2));
                    amt = (float.Parse(princ) - float.Parse(cur_princ)).ToString();
                    amt_next = float.Parse(amt);

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
                return false;
            }



        }
        catch (Exception ex)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return false;
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

    void Load_CustomerData_ShipToAddress()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();

        DataCenter MSSQL = new DataCenter(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();

        DataSet DS = new DataSet();

        G_thainame.Text = "";
        G_thaisurname.Text = "";
        //D_shipto1.Items.Clear();


        var resultTCL = MSSQL.GetDataset<DataSet>($@"select CISNumber m00csn,CustID m11csn, ga.Code m00dsn,NameInENG m00enm, SurnameInENG m00esn,NickName m00nnm,NameInTHAI m00tnm,SurnameInTHAI m00tsn, 
                                     FORMAT(BirthDate,'dd/MM/yyyy','th-TH') m00bdt,Village m11vil,gc.Code m11bil,BuildingName m11bln,AddressNumber m11adr,Room m11rom,[Floor] m11flo,Moo m11moo,Soi m11soi,
                                     Road m11rod,adt.Code m11tam,ada.Code m11amp,adp.Code m11prv,PostalAreaCode m11zip,adt.DescriptionTHAI gn18dt,ada.DescriptionTHAI gn19dt,adp.DescriptionTHAI gn20dt 
                                     from CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                                     left join CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK) on(cg.ID = ca.CustID and ca.CustRefID = '' and ca.IsShipTo = 'Y')
                                     left join GeneralDB01.GeneralInfo.AddrTambol adt WITH (NOLOCK) on(ca.TambolID = adt.ID) 
                                     left join GeneralDB01.GeneralInfo.AddrAumphur ada WITH (NOLOCK) on(ca.AmphurID = ada.ID) 
                                     left join GeneralDB01.GeneralInfo.AddrProvince adp WITH (NOLOCK) on(ca.ProvinceID = adp.ID) 
                                     left join GeneralDB01.GeneralInfo.GeneralCenter gc on (gc.ID = ca.BuildingTitleID AND gc.Type = 'BuildingTitleID') 
                                     left join GeneralDB01.GeneralInfo.GeneralCenter ga on (ga.ID = ca.AddressCodeID AND ga.Type = 'AddressCodeID') 
                                     where CISNumber = {hid_CSN.Value} ", CommandType.Text).Result;

        DS = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();


        //DS = busobj.RetriveAsDataSet("select m00csn, m11csn, m00dsn, m00enm, m00esn, m00nnm, m00tnm, m00tsn, m00bdt, " +
        //                             "m11vil, m11bil, m11bln, m11adr, m11rom, m11flo, m11moo, m11soi, " +
        //                             "m11rod, m11tam, m11amp, m11prv, m11zip, gn18dt, gn19dt, gn20dt " +
        //                             "from csms00 " +
        //                             "left join csms11 on(m00csn=m11csn and m00dsn=m11cde and m11ref = '' and m11rsq = 0) " +
        //                             "left join gntb18 on(m11tam=gn18cd) " +
        //                             "left join gntb19 on(m11amp=gn19cd) " +
        //                             "left join gntb20 on(m11prv=gn20cd) " +
        //                             "left join gntb08 on(gt08tc=m11bil) " +
        //                             "where m00csn = " + hid_CSN.Value + " and m00sts = '' ");
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["m00csn"].ToString().Trim() != "")
                {
                    D_shipto1.Value = dr["m00dsn"].ToString().Trim();
                    E_nameeng1.Text = dr["m00enm"].ToString().Trim();
                    E_surnameeng1.Text = dr["m00esn"].ToString().Trim();
                    E_nickname.Text = dr["m00nnm"].ToString().Trim();
                    G_thainame.Text = dr["m00tnm"].ToString().Trim();
                    G_thaisurname.Text = dr["m00tsn"].ToString().Trim();
                    G_birthdate.Text = dr["m00bdt"].ToString().Trim();
                }

                if (dr["m11csn"].ToString().Trim() != "")
                {
                    E_village1.Text = dr["m11vil"].ToString().Trim();
                    E_buildingname1.Text = dr["m11bln"].ToString().Trim();
                    E_addressno1.Text = dr["m11adr"].ToString().Trim();
                    E_roomno1.Text = dr["m11rom"].ToString().Trim();
                    E_floor1.Text = dr["m11flo"].ToString().Trim();
                    E_moo1.Text = dr["m11moo"].ToString().Trim();
                    E_soi1.Text = dr["m11soi"].ToString().Trim();
                    E_road1.Text = dr["m11rod"].ToString().Trim();
                    D_building1.Value = dr["m11bil"].ToString().Trim();

                    try
                    {
                        D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["m11tam"].ToString().Trim());
                        D_tambol1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["m11amp"].ToString().Trim());
                        D_amphur1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["m11prv"].ToString().Trim());
                        D_province1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_zipcode1.Items.Add(dr["m11zip"].ToString().Trim(), dr["m11zip"].ToString().Trim());
                        D_zipcode1.SelectedIndex = 0;
                    }
                    catch { }
                }
            }
            DS.Clear();
        }
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
    bool Have_CSMS03 = false;  //SL42 New Model and New Model ZR  
    string s_M13FIL = "";//SL42 New Model and New Model ZR 
    string s_M13BUT = "";
    string sl42_m13net = "";
    protected void D_tambol1_TextChanged(object sender, EventArgs e)
    {
        if (D_tambol1.Text.Trim() == "")
        {
            return;
        }
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        iDB2Command cmd = new iDB2Command();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());

       // generalApi = new Connect_GeneralAPI();
        //DataSet ds = new DataSet();
        if (Cache["ds_TambolN"] != null)
        {
            DS = (DataSet)Cache["ds_TambolN"];
        }
        else
        {
            //  var resultdt = generalApi.GetGeneralTambol();
            //DS.Tables.Add(resultdt);
            cmd.Parameters.Clear();
            cmd.CommandText = $@"SELECT ID,Code,DescriptionTHAI,DescriptionENG,ShortName,Sorting,RecordStatus,Application,CreateBy,CreateDate,UpdateBy,UpdateDate,IsDelete
                            FROM GeneralDB01.GeneralInfo.AddrTambol  WITH(NOLOCK)";
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;
           
            Cache["ds_TambolN"] = DS;
            Cache.Insert("ds_TambolN", DS, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

        }
        //cmd.Parameters.Clear();
        //cmd.CommandText = "select gn18cd, gn18dt from gntb18 " +
        //                  "where gn18dt = @tambol ";
        //cmd.Parameters.Add("@tambol", D_tambol1.Text.Trim());

        //DS = busobj.RetriveAsDataSet(cmd);

        string Mem_tambol = D_tambol1.Text.Trim();

        int count_tambol = 0;
        D_tambol1.Items.Clear();
        if (DS != null && DS.Tables.Count > 0)
        {
            DataRow[] dataRows = DS.Tables[0].Select("DescriptionTHAI = '" + D_tambol1.Text.Trim() + "'");
            foreach (DataRow dr in dataRows)
            {
                D_tambol1.Items.Add(dr["DescriptionTHAI"].ToString().Trim(), dr["Code"].ToString().Trim());
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
            //busobj.CloseConnectioDAL();
            return;
        }

        /***************************************  Find Amphur  ********************************************/
      
        cmd.Parameters.Clear();
        cmd.CommandText = $@"select ada.Code gn19cd, ada.DescriptionTHAI gn19dt
                            FROM GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)
                            JOIN GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK) on(adr.AumphurID = ada.ID)
                            JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID)
                            WHERE adt.DescriptionTHAI = '{D_tambol1.Text.Trim()}'  order by ada.DescriptionTHAI";
        //cmd.Parameters.Add("@tambol", D_tambol1.Text.Trim());

        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText,CommandType.Text).Result.data;

        int count_amphur = 0;
        D_amphur1.Items.Clear();
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
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
            //busobj.CloseConnectioDAL();
            return;
        }
        if (count_amphur > 1)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
            busobj.CloseConnectioDAL();
            D_province1.Items.Clear();
            D_province1.Text = "";
            D_zipcode1.Items.Clear();
            D_zipcode1.Text = "";
            D_amphur1.Text = "";
            D_amphur1.Focus();
            return;
        }

        /***************************************  Find Province  ********************************************/
        D_province1.Items.Clear();
        D_zipcode1.Items.Clear();
        if ((count_tambol == 1) & (count_amphur == 1))
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $@"select adp.Code gn20cd, adp.DescriptionTHAI gn20dt , ZipCode gn21zp
                                FROM GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK)
                                JOIN GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK)  on(adr.ProvinceID = adp.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)  on(adr.AumphurID = ada.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID)
                                WHERE adt.DescriptionTHAI = '{D_tambol1.Text.Trim()}' AND ada.DescriptionTHAI = '{D_amphur1.Text.Trim()}' ";

            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

            int count_province = 0;
            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
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
        CallHisunMaster._dataCenter.CloseConnectSQL();
    }

    protected void D_amphur1_TextChanged(object sender, EventArgs e)
    {
        if (D_amphur1.Text.Trim() == "")
        {
            return;
        }
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        iDB2Command cmd = new iDB2Command();

        cmd.Parameters.Clear();
        cmd.CommandText = $@"SELECT Code gn19cd, DescriptionTHAI gn19dt  
                            FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK)
                            WHERE DescriptionTHAI = '{D_amphur1.Text.Trim()}'";
        //cmd.Parameters.Add("@amphur", D_amphur1.Text.Trim());

        //DS = busobj.RetriveAsDataSet(cmd);
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText, CommandType.Text).Result.data;

        string Mem_amphur = D_amphur1.Text.Trim();

        int count_amphur = 0;
        D_amphur1.Items.Clear();
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
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
            //m_da400.CloseConnect();
            return;
        }

        if (D_tambol1.Value.ToString().Trim() == "")
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $@"select adp.Code gn20cd, adp.DescriptionTHAI gn20dt , ZipCode gn21zp, adt.Code gn18cd, adt.DescriptionTHAI
                                FROM GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK)
                                JOIN GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK)  on(adr.ProvinceID = adp.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)  on(adr.AumphurID = ada.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID)
                                WHERE ada.DescriptionTHAI = '{D_amphur1.Text.Trim()}' ";
        }   
        else
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $@"select adp.Code gn20cd, adp.DescriptionTHAI gn20dt , ZipCode gn21zp, adt.Code gn18cd, adt.DescriptionTHAI
                                FROM GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK)
                                JOIN GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK)  on(adr.ProvinceID = adp.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)  on(adr.AumphurID = ada.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID)
                                WHERE adt.DescriptionTHAI = '{D_tambol1.Text.Trim()}' AND ada.DescriptionTHAI = '{D_amphur1.Text.Trim()}' ";
        }

        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText,CommandType.Text).Result.data;

        string Mem_tambol = D_tambol1.Text.Trim();
        int count_tambol = 0;
        if (DS != null && DS.Tables.Count > 0)
        {
            D_tambol1.Items.Clear();
            D_province1.Items.Clear();
            D_zipcode1.Items.Clear();
            D_tambol1.Text = "";
            D_province1.Text = "";
            D_zipcode1.Text = "";
            foreach (DataRow dr in DS.Tables[0].Rows)
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
            //List tambol
            cmd.Parameters.Clear();
            cmd.CommandText = $@"select adp.Code gn20cd, adp.DescriptionTHAI gn20dt , ZipCode gn21zp, adt.Code gn18cd, adt.DescriptionTHAI
                                FROM GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK)
                                JOIN GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK)  on(adr.ProvinceID = adp.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)  on(adr.AumphurID = ada.ID)
                                JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID)
                                WHERE ada.DescriptionTHAI = '{D_amphur1.Text.Trim()}'  ";
            
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText,CommandType.Text).Result.data;

            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
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
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
        if (count_tambol > 1)
        {
            D_tambol1.Focus();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
    }

    protected void C_address_I_TextChanged(object sender, EventArgs e)
    {
        L_count.Text = "";

        if (C_address_I.Text.Trim() == "")
        {
            return;
        }
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();

        DataSet DS = new DataSet();

        if (C_address_I.Text.Trim().Length > 20)
        {
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT Code gn18cd, DescriptionTHAI gn18dt
                                            FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK)
                                            wHERE Code = {C_address_I.Value.ToString().Substring(0, 5)}",CommandType.Text).Result.data;
            if (DS != null && DS.Tables.Count > 0)
            {
                D_tambol1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["gn18cd"].ToString().Trim());

                }
                DS.Clear();
            }

            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT Code gn19cd, DescriptionTHAI gn19dt
                                            FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK)
                                            wHERE Code = {C_address_I.Value.ToString().Substring(6, 4)} ", CommandType.Text).Result.data;
            if (DS != null && DS.Tables.Count > 0)
            {
                D_amphur1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["gn19cd"].ToString().Trim());
                }
                DS.Clear();
            }

            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT Code gn20cd, DescriptionTHAI gn20dt
                                            FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK)
                                            wHERE Code = {C_address_I.Value.ToString().Substring(11, 3)} " , CommandType.Text).Result.data;
            if (DS != null && DS.Tables.Count > 0)
            {
                D_province1.Items.Clear();
                foreach (DataRow dr in DS.Tables[0].Rows)
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

        iDB2Command cmd = new iDB2Command();
        string StrWhere = "";

        if (StrWhere == "")
        {
            StrWhere += " gn18dt like @tambol or gn19dt like @amphur or gn20dt like @province or gn21zp like @zipcode ";
        }

        cmd.Parameters.Clear();
        cmd.CommandText = $@"select CONCAT(TRIM(ISNULL(adt.DescriptionTHAI,'')), ' - ', TRIM(ISNULL(ada.DescriptionTHAI,'')), ' - ', TRIM(ISNULL(adp.DescriptionTHAI,'')), ' - ',TRIM(adr.ZipCode)) as Address ,
                        CONCAT(REPLACE(STR(adt.Code, 5), SPACE(1), '0'), '-', REPLACE(STR(ada.Code, 4), SPACE(1), '0'), '-', REPLACE(STR(adp.Code, 3), SPACE(1), '0'), '-', adr.ZipCode) as Code 
                        FROM GeneralDB01.GeneralInfo.AddrRelation adr WITH(NOLOCK) 
                        JOIN GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK)  on(adr.ProvinceID = adp.ID) 
                        JOIN GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK)  on(adr.AumphurID = ada.ID) 
                        JOIN GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(adr.TambolID = adt.ID) 
                        WHERE adt.DescriptionTHAI LIKE '%{C_address_I.Text.Trim()}%' OR ada.DescriptionTHAI LIKE '%{C_address_I.Text.Trim()}%' 
                        OR adp.DescriptionTHAI LIKE '%{C_address_I.Text.Trim()}%' OR adr.ZipCode LIKE '%{C_address_I.Text.Trim()}%' 
                        ORDER BY adp.Code, ada.Code, adt.Code";

        //cmd.Parameters.Add("@tambol", '%' + C_address_I.Text.Trim() + '%');
        //cmd.Parameters.Add("@amphur", '%' + C_address_I.Text.Trim() + '%');
        //cmd.Parameters.Add("@province", '%' + C_address_I.Text.Trim() + '%');
        //cmd.Parameters.Add("@zipcode", '%' + C_address_I.Text.Trim() + '%');

        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>(cmd.CommandText,CommandType.Text).Result.data;

        int count = 0;
        if (DS != null && DS.Tables.Count > 0)
        {
            C_address_I.Items.Clear();
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                C_address_I.Items.Add(dr["Address"].ToString().Trim(), dr["Code"].ToString().Trim());
                count += 1;
            }
            DS.Clear();
        }
        L_count.Text = count.ToString() + " Matching";
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

    void InsertTel(string TelText, string TelAdr, string TelType, string TelExt, string refType, string refSEQ, int i_addr, ILDataCenter busobj1)
    {
        int i = 0;
        if (TelText != "0")
        {
            CalCTel(TelText);
            for (i = 1; i <= 99; i++)
            {
                if ((i == 1) || (telno[i] != ""))
                {
                    InsertCSMS12(telno[i], TelAdr, TelType, TelExt, refType, refSEQ, i, i_addr, busobj1);
                }
            }
        }
    }
    
    void InsertCSMS12(string Teleno, string AddCode, string TelType, string TeleExt, string refType, string refSEQ, int iSeq, int cnt_addr, ILDataCenter busobj2)
    {
        if (E_sub_succuss.Text == "")
        {
            //waiting reduce user connection
            userInfoService = new UserInfoService();
            userInfo = userInfoService.GetUserInfo();
            ILDataCenter busobj_csms12 = new ILDataCenter();
            busobj_csms12.UserInfomation = userInfoService.GetUserInfo();
            DataSet DS = new DataSet();
            iDB2Command cmd1 = new iDB2Command();

            int w_icnt = 0, affectedRows = 0;
            E_sub_succuss.Text = "";

            if ((cnt_addr == 1) & (iSeq == 1))
            {
                DS = busobj_csms12.RetriveAsDataSet("select * from csms12 " +
                                             "where m12csn = " + hid_CSN.Value + " and " +
                                             "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                             "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ");
                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        cmd1.Parameters.Clear();
                        cmd1.CommandText = "delete from CSMS12 " +
                                            "where m12csn = " + hid_CSN.Value + " and " +
                                            "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                            "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ";
                        affectedRows = -1;
                        try
                        {
                            affectedRows = busobj2.ExecuteNonQuery(cmd1);
                        }
                        catch (Exception ex)
                        {
                            E_sub_succuss.Text = "Error";
                            L_message.Text = "Error on delete CSMS12";
                            //Control Rollback จาก E_sub_succuss.Text                        
                        }
                        if (affectedRows < 0)
                        {
                            E_sub_succuss.Text = "Error";
                            L_message.Text = "Error on delete CSMS12";
                            //Control Rollback จาก E_sub_succuss.Text
                        }
                    }
                }
            }
            w_icnt = 0;
            DS = busobj_csms12.RetriveAsDataSet("select Max(m12seq) as max_seq from csms12 " +
                                         "where m12csn = " + hid_CSN.Value + " and " +
                                         "m12ref = '" + refType + "' and m12rsq = " + refSEQ + " and " +
                                         "m12acd = '" + AddCode + "' and m12tty = '" + TelType + "' ");
            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
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
                                  "Values(" + hid_CSN.Value + ", " +
                                  "'" + refType + "', " +
                                  "" + refSEQ + ", " +
                                  "'" + AddCode + "', " +
                                  "'" + TelType + "', " +
                                  "" + w_icnt.ToString() + ", " +
                                  "'" + Teleno + "', " +
                                  "'" + TeleExt + "', " +
                                  "" + hid_date97.Value + ", " +
                                  "" + m_UpdTime + ", " +
                                  "'" + userInfo.Username + "', " +
                                  "'ILNORMAL', " +
                                  "'" + userInfo.LocalClient + "', " +
                                  "'') ";
                affectedRows = -1;
                try
                {
                    affectedRows = busobj2.ExecuteNonQuery(cmd1);
                }
                catch (Exception ex)
                {
                    E_sub_succuss.Text = "Error";
                    L_message.Text = "Error on Insert CSMS12";
                    //Control Rollback จาก E_sub_succuss.Text
                }
                if (affectedRows < 0)
                {
                    E_sub_succuss.Text = "Error";
                    L_message.Text = "Error on Insert CSMS12";
                    //Control Rollback จาก E_sub_succuss.Text
                }
            }
            busobj_csms12.CloseConnectioDAL();
        }
    }

    void calTCL()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        ILDataCenterMssqlInterview ilDataInterview = new ILDataCenterMssqlInterview(userInfoService.GetUserInfo());
        DataSet DS = new DataSet();
        DataCenter MSSQL = new DataCenter(userInfoService.GetUserInfo());

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

        P_tcl_product.Visible = false;
        if (mode != "O")
        {
            P_tcl_product.Visible = true;
        }

        try
        {
            //string SQL = "select m13csn, m13sex, 'AGE_GNP0371', m13mrt, int(m13ttl) as m13ttl, case when m13mtl <> '' then 1 else 0 end as m13mtl, " +
            //                                 "m13res, (m13lyr*12)+m13lmt as resident, (m13wky*12)+m13wkm as workyear, m13occ, m13but, m13slt, m13off, int(m13net) as m13net, " +
            //                                 "m13con, m13chl, m13pos, m13emp, " + hid_brn.Value.ToString() + " as three, " + hid_date97.Value.ToString() + " as date97, '000' as one, m13apv, " +
            //                                 "m13hzp, m13sst, int(m13saj) as m13saj, m13bdt, m13gol, m13cha , M13FIL,M13OZP  " +
            //                                 "from csms13 " +
            //                                 "where m13app = 'IL' and m13csn = " + hid_CSN.Value.ToString() + " and m13brn = " + hid_brn.Value.ToString() + " " +
            //                                 "and m13apn = " + hid_AppNo.Value.ToString() + " ";
            //DS = busobj.RetriveAsDataSet(SQL.ToString());


            var resultTCL = MSSQL.GetDataset<DataSet>($@"select m13csn, m13sex, 'AGE_GNP0371', m13mrt, CAST(m13ttl as bigint) as m13ttl, case when m13mtl <> '' then 1 else 0 end as m13mtl, 
                                             m13res, (m13lyr*12)+m13lmt as resident, (m13wky*12)+m13wkm as workyear, m13occ, m13but, m13slt, m13off, Cast(m13net as bigint) as m13net, 
                                             m13con, m13chl, m13pos, m13emp,  '{ hid_brn.Value.ToString()}' as three,  '{hid_date97.Value.ToString()}' as date97, '000' as one, m13apv, 
                                             m13hzp, m13sst, Cast(m13saj as bigint) as m13saj, m13bdt, m13gol, m13cha , M13FIL,M13OZP  
                                             from AS400DB01.CSOD0001.csms13 WITH (NOLOCK)
                                             where m13app = 'IL' and m13csn =  '{hid_CSN.Value.ToString()}'  and m13brn =  '{hid_brn.Value.ToString()}' 
                                             and m13apn =  '{hid_AppNo.Value.ToString()}' ", CommandType.Text).Result;

            DS = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();

            string in_AGE = "", error = "";
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                hid_salary.Value = dr["m13net"].ToString().Trim();
                iLDataSubroutine.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                                            ref in_AGE, ref error);
                //busobj.CALL_GNP0371(dr["m13bdt"].ToString().Trim(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                //                            userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                //                            ref in_AGE, ref error);
                //busobj.CloseConnectioDAL();

                DataSet ds_res = new DataSet();
                string Error = "";
                //busobj.calTCL(hid_idno.Value, hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, hid_appdate.Value, hid_date97.Value, dr["m13bdt"].ToString().Trim(),
                //          dr["m13sex"].ToString().Trim(), in_AGE.ToString().Trim(), dr["m13mrt"].ToString().Trim(), dr["m13ttl"].ToString().Trim(),
                //          dr["m13mtl"].ToString().Trim(), dr["m13res"].ToString().Trim(), dr["resident"].ToString().Trim(), dr["workyear"].ToString().Trim(),
                //          dr["m13occ"].ToString().Trim(), dr["m13but"].ToString().Trim(), dr["m13slt"].ToString().Trim(), dr["m13off"].ToString().Trim(),
                //          dr["m13net"].ToString().Trim(), dr["m13con"].ToString().Trim(), dr["m13chl"].ToString().Trim(), dr["m13pos"].ToString().Trim(),
                //          dr["m13emp"].ToString().Trim(), dr["three"].ToString().Trim(), "0", dr["m13apv"].ToString().Trim(), dr["m13hzp"].ToString().Trim(),
                //          dr["m13sst"].ToString().Trim(), dr["m13saj"].ToString().Trim(), Loan_Amt.Text, dd_vendor.Value.ToString(),
                //          dr["m13gol"].ToString().Trim(), dr["m13cha"].ToString().Trim(), ref ds_res, ref Error, dr["M13OZP"].ToString().Trim());
                //busobj.CloseConnectioDAL();
                string[] vendor = dd_vendor.Value.ToString().Split('|');
                ilDataInterview.calTCL(hid_idno.Value, hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, hid_appdate.Value, hid_date97.Value, dr["m13bdt"].ToString().Trim(),
                          dr["m13sex"].ToString().Trim(), in_AGE.ToString().Trim(), dr["m13mrt"].ToString().Trim(), dr["m13ttl"].ToString().Trim(),
                          dr["m13mtl"].ToString().Trim(), dr["m13res"].ToString().Trim(), dr["resident"].ToString().Trim(), dr["workyear"].ToString().Trim(),
                          dr["m13occ"].ToString().Trim(), dr["m13but"].ToString().Trim(), dr["m13slt"].ToString().Trim(), dr["m13off"].ToString().Trim(),
                          dr["m13net"].ToString().Trim(), dr["m13con"].ToString().Trim(), dr["m13chl"].ToString().Trim(), dr["m13pos"].ToString().Trim(),
                          dr["m13emp"].ToString().Trim(), dr["three"].ToString().Trim(), "0", dr["m13apv"].ToString().Trim(), dr["m13hzp"].ToString().Trim(),
                          dr["m13sst"].ToString().Trim(), dr["m13saj"].ToString().Trim(), Loan_Amt.Text, vendor[0],
                          dr["m13gol"].ToString().Trim(), dr["m13cha"].ToString().Trim(), ref ds_res, ref Error, dr["M13OZP"].ToString().Trim());

                DataRow dr_res = ds_res.Tables[0].Rows[0];
                if (busobj.check_dataset(ds_res))
                {
                    Have_CSMS03 = (bool)dr_res["G_Have_CSMS03"];  //SL42 New Model and New Model ZR  
                    EdtTCL.Text = dr_res["EdtTCL"].ToString().Trim();
                    //LblStsTCL.Text = dr_res["LblStsTCL"].ToString().Trim();
                    LTCL.Text = dr_res["LTCL"].ToString().Trim();
                    EdtACL.Text = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim();
                    EdtTCL.Text = dr_res["EdtTCL"].ToString().Trim() == "" ? "0" : dr_res["EdtTCL"].ToString().Trim();
                    G_TCL_13.Text = dr_res["EdtTCL"].ToString().Trim();
                    G_ACL_13.Text = dr_res["G_ACL_13"].ToString().Trim() == "" ? "0" : dr_res["G_ACL_13"].ToString().Trim();
                    G_PD.Text = dr_res["G_PD"].ToString().Trim();
                    E_group.Text = dr_res["E_group"].ToString().Trim() == "" ? "0" : dr_res["E_group"].ToString().Trim();
                    E_rank.Text = dr_res["E_rank"].ToString().Trim() == "" ? "0" : dr_res["E_rank"].ToString().Trim();
                    E_model.Text = dr_res["E_model"].ToString().Trim() == "" ? "0" : dr_res["E_model"].ToString().Trim();
                    G_Rank_for_GNSR031.Text = dr_res["G_Rank_for_GNSR031"].ToString().Trim();
                    G_GROUP_ONGOING_Y.Text = dr_res["G_GROUP_ONGOING_Y"].ToString().Trim();
                    G_RANK_ONGOING_Y.Text = dr_res["G_RANK_ONGOING_Y"].ToString().Trim();
                    G_ACL_ONGOING_Y.Text = dr_res["G_ACL_ONGOING_Y"].ToString().Trim();
                    P4TYPE.Text = dr_res["P4TYPE"].ToString().Trim();

                    //EdtNetIncome.Text = dr_res["EdtNetIncome"].ToString().Trim();                    
                    //EdtBOTLoan.Text = dr_res["EdtBOTLoan"].ToString().Trim();
                    //EdtCrBal.Text = dr_res["EdtCrBal"].ToString().Trim();
                    //EAAA.Text = dr_res["EAAA"].ToString().Trim();
                    //EdtCrLmt.Text = dr_res["EdtCrLmt"].ToString().Trim();
                    //EdtESBLoan.Text = dr_res["EdtESBLoan"].ToString().Trim();
                    //EdtCrAvi.Text = dr_res["EdtCrAvi"].ToString().Trim();
                    //E_Apv_avi.Text = dr_res["E_Apv_avi"].ToString().Trim();
                    EdtACL.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(EdtACL.Text));
                    EdtTCL.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(EdtTCL.Text));
                    EdtNetIncome.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtNetIncome"].ToString().Trim()));
                    EdtBOTLoan.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtBOTLoan"].ToString().Trim() == "" ? "0" : dr_res["EdtBOTLoan"].ToString().Trim()));
                    EdtCrBal.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtCrBal"].ToString().Trim() == "" ? "0" : dr_res["EdtCrBal"].ToString().Trim()));
                    EAAA.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EAAA"].ToString().Trim() == "" ? "0" : dr_res["EAAA"].ToString().Trim()));
                    EdtCrLmt.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtCrLmt"].ToString().Trim() == "" ? "0" : dr_res["EdtCrLmt"].ToString().Trim()));
                    EdtESBLoan.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtESBLoan"].ToString().Trim() == "" ? "0" : dr_res["EdtESBLoan"].ToString().Trim()));
                    EdtCrAvi.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["EdtCrAvi"].ToString().Trim() == "" ? "0" : dr_res["EdtCrAvi"].ToString().Trim()));
                    E_Apv_avi.Text = String.Format("{0:#,###,##0.00}", Convert.ToDecimal(dr_res["E_Apv_avi"].ToString().Trim() == "" ? "0" : dr_res["E_Apv_avi"].ToString().Trim()));
                    /*
                    EdtTCL.ReadOnly = false;
                    if (LTCL.Text == "TCL(Existing)")
                    {
                        EdtTCL.ReadOnly = true;
                    }
                    */

                    string s_M13FIL = "";//SL42 New Model and New Model ZR  
                   
                    if (ilObj.check_dataset(DS))
                    {
                        DataRow dr_csms13 = DS.Tables[0].Rows[0];
                        string s_M13FIL_Length = dr_csms13["M13FIL"].ToString();
                        if (s_M13FIL_Length.Trim().Length > 18)
                        {
                            s_M13FIL = s_M13FIL_Length.ToString().Substring(18, 1);
                            hfM13FIL.Value = s_M13FIL_Length.ToString().Substring(18, 1);
                        }
                        s_M13BUT = dr_csms13["M13BUT"].ToString();
                        hfM13BUT.Value = dr_csms13["M13BUT"].ToString();
                    }
                    string Error_GNSR093 = "";
                    string payment_ability = "";
                    iLDataSubroutine.Call_GNSR093(hid_idno.Value.ToString(), hid_salary.Value.ToString(), ref Error_GNSR093, ref payment_ability, userInfo.BizInit, userInfo.BranchNo);
                    //busobj.CloseConnectioDAL();
                    txt_pay_abl.Text = convCurrency(payment_ability);
                    //busobj.CloseConnectioDAL();
                }
            }
        }
        catch { }
    }

    protected void B_return_Click(object sender, EventArgs e)
    {
        L_NCB.Text = "";
        L_confirm.Text = "Do you want to Return to Interview?";
        B_confirmsave.Text = "Yes";
        B_confirmcancel.Text = "No";
        P_confirm_TCL.ShowOnPageLoad = true;
    }

    protected void B_cancel_Click(object sender, EventArgs e)
    {
        //  check Reject status  14/07/2558 
        //Check Product &  Resend Bureau
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();
        string check_NCB = "";
        L_NCB.Text = "";
        try
        {

            DataSet DS = new DataSet();

            //DS = busobj.RetriveAsDataSet("Select p1csno, substr(p1fill,28,2) as SaveProduct , substr(p1fill,21,1) as chk_ncb from ILMS01 " +
            //                             "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ");
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select p1csno, SUBSTRING(p1fill,28,2) as SaveProduct , SUBSTRING(p1fill,21,1) as chk_ncb from AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) " +
                                         "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
            CallHisunMaster._dataCenter.CloseConnectSQL();
            if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                DataRow dr = DS.Tables[0].Rows[0];
                if (dr["chk_ncb"].ToString().Trim() == "B" || dr["chk_ncb"].ToString().Trim() == "R" || dr["chk_ncb"].ToString().Trim() == "O"
                        || dr["chk_ncb"].ToString().Trim() == "S" || dr["chk_ncb"].ToString().Trim() == "L"
                        )
                {
                    L_message.Text = "Result Bureau is reject status , Please Check data " + "\r\n";
                    E_redirect.Text = "N";
                    P_message_TCL.ShowOnPageLoad = true;
                    return;

                }

                if (dr["chk_ncb"].ToString().Trim() == "C")
                {
                    check_NCB = " Application No. ยังไม่ได้ทำการ Resend Bureau " + "\r\n";

                }

            }
            DS.Clear();
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        //*************************************************************//


        L_msg_note_TCL.Text = "";
        if (hid_status.Value == "INTERVIEW")
        {
            L_NCB.Text = check_NCB;
            L_confirm.Text = "Do you want to Cancel by Interview?";
        }
        else
        {

            G_reject_cancel.Text = "where g25fil in ('C') ";
            Load_Action();
            Load_Reason();
            E_note_TCL.Text = "";
            P_note_TCL.HeaderText = "KESSAI CANCEL";
            P_note_TCL.ShowOnPageLoad = true;
            return;
            //L_confirm.Text = "Do you want to Cancel by KESSAI?";
        }

        P_confirm_TCL.ShowOnPageLoad = true;
    }

    protected void B_reject_Click(object sender, EventArgs e)
    {
        //  check Reject status  14/07/2558 
        //Check Product &  Resend Bureau
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();
        string check_NCB = "";
        L_NCB.Text = "";

        try
        {

            DataSet ds_NCB = new DataSet();

            //ds_NCB = busobj.RetriveAsDataSet("Select p1csno, substr(p1fill,28,2) as SaveProduct , substr(p1fill,21,1) as chk_ncb from ILMS01 " +
            //                             "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ");
            ds_NCB = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select p1csno, SUBSTRING(p1fill,28,2) as SaveProduct , SUBSTRING(p1fill,21,1) as chk_ncb from AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) " +
                                         "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ",CommandType.Text).Result.data;
            //busobj.CloseConnectioDAL();
            if (ds_NCB.Tables.Count > 0 && ds_NCB.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds_NCB.Tables[0].Rows[0];
                if (dr["chk_ncb"].ToString().Trim() == "B" || dr["chk_ncb"].ToString().Trim() == "R" || dr["chk_ncb"].ToString().Trim() == "O"
                        || dr["chk_ncb"].ToString().Trim() == "S" || dr["chk_ncb"].ToString().Trim() == "L"
                        )
                {
                    L_message.Text = "Result Bureau is reject status , Please Check data " + "\r\n";
                    E_redirect.Text = "N";
                    P_message_TCL.ShowOnPageLoad = true;
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;

                }

                if (dr["chk_ncb"].ToString().Trim() == "C")
                {
                    check_NCB = " Application No. ยังไม่ได้ทำการ Resend Bureau " + "\r\n";
                    //L_message.Text = "Result Bureau is reject status , Please Check data " + "\r\n";
                    //E_redirect.Text = "N";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //return;


                }

            }
            //DS.Clear();
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        //*************************************************************//




        L_msg_note_TCL.Text = "";
        if (hid_status.Value == "INTERVIEW")
        {
            L_NCB.Text = check_NCB;
            L_confirm.Text = "Do you want to Reject by Interview?";
        }
        else
        {
            G_reject_cancel.Text = "where g25fil in ('R','B') ";
            Load_Action();
            Load_Reason();
            E_note_TCL.Text = "";
            P_note_TCL.HeaderText = "KESSAI REJECT";
            P_note_TCL.ShowOnPageLoad = true;
            return;
            //L_confirm.Text = "Do you want to Reject by KESSAI?";
        }

        P_confirm_TCL.ShowOnPageLoad = true;
    }

    protected void B_approve_Click(object sender, EventArgs e)
    {
        //Check Verify call
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        iLDataCenterMssqlUsing = new ILDataCenterMssqlUsingCard(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();

        L_NCB.Text = "";
        string err_ = "";
        bool res_err = true;
        L_othmsg.Text = "";
        lb_backlist.Text = "";
        string prmWKDTE = "";
        bool call_ilsr97 = iLDataSubroutine.Call_ILSR97("88", "", userInfo.BizInit, userInfo.BranchNo, ref prmWKDTE);
        //busobj.CloseConnectioDAL();

        //ulong eir_date = 0,appdate = 0;
        //appdate = ulong.TryParse(hid_appdate.Value.ToString(), out appdate) ? appdate : 0;
        //eir_date = ulong.TryParse(prmWKDTE, out eir_date) ? eir_date : 0;
        //if (call_ilsr97 && appdate < eir_date)
        //{
        //    err_ += "ไม่สามารถ ออกผล อนุมัติได้, ระบบไม่รองรับการคำนวณดอกเบี้ยแบบ Flat Rate." + "\r\n";
        //    L_message.ForeColor = System.Drawing.Color.Red;
        //    L_message.Text = err_;
        //    P_message_TCL.ShowOnPageLoad = true;
        //    return;
        //}

        if (hid_status.Value == "INTERVIEW" || hid_status.Value == "KESSAI")
        {
            s_M13BUT = hfM13BUT.Value;
            s_M13FIL = hfM13FIL.Value;
            if (Convert.ToDouble(hid_salary.Value) < 10000 && Convert.ToInt32(E_rank.Text) == 1 && (s_M13FIL == "Z" || Have_CSMS03 == false) && (s_M13BUT.Trim() == "04" || s_M13BUT.Trim() == "05"))
            {
                err_ += "ไม่สามารถ Approve ได้เนื่องจากไม่ผ่านเงื่อนไขSL42." + "\r\n";
                L_message.ForeColor = System.Drawing.Color.Red;
                L_message.Text = err_;
                P_message_TCL.ShowOnPageLoad = true;
                return;
            }
        }

        if (hid_loantyp.Value == "01")
        {
            err_ += "ไม่สามารถ ออกผล อนุมัติได้, ระบบไม่รองรับการคำนวณดอกเบี้ยแบบ Flat Rate." + "\r\n";
            L_message.ForeColor = System.Drawing.Color.Red;
            L_message.Text = err_;
            P_message_TCL.ShowOnPageLoad = true;
            return;
        }
        //Start Pact check risk authorize
        if (hid_status.Value == "KESSAI")
        {
            //DS = busobj.RetriveAsDataSet("select m13rkf as risk from csms13 " +
            //                            "where m13app = 'IL' and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value);
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m13rkf as risk from AS400DB01.CSOD0001.CSMS13 WITH (NOLOCK) " +
                                        "where m13app = 'IL' and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value,CommandType.Text).Result.data;

            if (DS != null)
            {
                var checkPEP = CheckPEPsStatus(G_thainame.Text, G_thaisurname.Text);

                //20231219 Add  หากเป็นลูกค้าที่มีสถานภาพทางการเมือง Risk group 2 (RISK-02:PEP), ให้ระบบแสดง Flag: PEP และเป็นเคสที่ต้องได้รับการอนุมัติจากผู้ที่มี Authorize เท่านั้น
                if (DS.Tables[0].Rows[0]["risk"].ToString() == "3PV" || checkPEP == "PEP")
                {
                    DataSet SDS = new DataSet();
                    //SDS = busobj.RetriveAsDataSet("SELECT SUBSTR(AT05AT,1,1)as AT05AT FROM GNAT05 WHERE AT05JD = (" +
                    //                     "SELECT AT04JD FROM GNAT04 WHERE AT04EM in ( SELECT USEMID " +
                    //                     "FROM SYFUSDES " +
                    //                     "WHERE USCODE ='" + userInfo.Username + "' ))  ");
                    
                    SDS = CallHisunMaster._dataCenter.GetDataset<DataTable>("SELECT SUBSTRING(AT05AT,1,1)as AT05AT FROM AS400DB01.GNOD0000.GNAT05 WITH (NOLOCK) WHERE AT05JD = (" +
                                         "SELECT AT04JD FROM AS400DB01.GNOD0000.GNAT04 WITH (NOLOCK) WHERE AT04EM in ( SELECT USEMID " +
                                         "FROM AS400DB01.SYOD0000.SYFUSDES WITH (NOLOCK) " +
                                         "WHERE USCODE ='" + userInfo.Username + "' ))  ",CommandType.Text).Result.data;
                    if (SDS.Tables[0].Rows.Count > 0)
                    {
                        if (SDS.Tables[0].Rows[0]["AT05AT"].ToString() != "Y")
                        {
                            err_ += "เป็นเคสที่ต้องได้รับการ Approve จากผู้ที่มี Authorize เท่านั้น. " + "\r\n";
                            L_message.ForeColor = System.Drawing.Color.Red;
                            L_message.Text = err_;
                            P_message_TCL.ShowOnPageLoad = true;
                            CallHisunMaster._dataCenter.CloseConnectSQL();
                            return;
                        }
                    }
                    else
                    {
                        err_ += "เป็นเคสที่ต้องได้รับการ Approve จากผู้ที่มี Authorize เท่านั้น. " + "\r\n";
                        L_message.ForeColor = System.Drawing.Color.Red;
                        L_message.Text = err_;
                        P_message_TCL.ShowOnPageLoad = true;
                        CallHisunMaster._dataCenter.CloseConnectSQL();
                        return;
                    }
                    SDS.Clear();
                }
            }
            DS.Clear();
        }
        //End Pact check risk authorize

        //Check Auto Reject Call RLSRAUR            
        string WPRJAC = "", WPRJCD = "", WPRJDS = "", Error = "";
        string Birthday = G_birthdate.Text.Trim().Substring(6, 4) + G_birthdate.Text.Trim().Substring(3, 2) + G_birthdate.Text.Trim().Substring(0, 2);
        bool call_RLSRAUR = iLDataSubroutine.Call_RLSRAUR(hid_idno.Value, Birthday, G_thainame.Text.Trim(), G_thaisurname.Text.Trim(),
                                                "", "", "", "", "", "", "", ref WPRJAC, ref WPRJCD, ref WPRJDS, ref Error, "IL",
                                                userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        if ((WPRJAC.ToString().Trim() == "ID") & (WPRJCD.ToString().Trim() == "ID01"))
        {
            err_ += "ID Format incorrect." + "\r\n";
            res_err = false;
            //L_message.Text = "ID Format incorrect.";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if ((WPRJAC.ToString().Trim() == "AN") & (WPRJCD.ToString().Trim() == "ID01"))
        {
            err_ += "Customer Age Over. " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer Age Over";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if ((WPRJAC.ToString().Trim() == "B4") & (WPRJCD.ToString().Trim() == "BL4"))
        {
            err_ += "Customer Case AMLO. " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer Case AMLO";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if ((WPRJAC.ToString().Trim() == "B1") & (WPRJCD.ToString().Trim() == "BL2"))
        {
            err_ += "Customer Case Blacklist. " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer Case Blacklist";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (((WPRJAC.ToString().Trim() == "WO") || (WPRJAC.ToString().Trim() == "WL") ||
             (WPRJAC.ToString().Trim() == "WS") || (WPRJAC.ToString().Trim() == "LC"))
            & (WPRJCD.ToString().Trim() == "BL2"))
        {
            err_ += "Customer Status Not Pass. " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer Status Not Pass";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (((WPRJAC.ToString().Trim() == "PP") || (WPRJAC.ToString().Trim() == "FD") || (WPRJAC.ToString().Trim() == "LG"))
            & (WPRJCD.ToString().Trim() == "IL9"))
        {
            err_ += "Customer is Fraud/Legal. " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer is Fraud/Legal ";
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (((WPRJAC.ToString().Trim() == "03") || (WPRJAC.ToString().Trim() == "PH"))
            & (WPRJCD.ToString().Trim() == "IL8"))
        {
            err_ += "Customer OD3 Up " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer OD3 Up ";
            //busobj.CloseConnectioDAL();
            //return;
        }

        Check_AMLO();
        //busobj.CloseConnectioDAL();
        if (G_AMLO.Text.Trim() == "Y")
        {
            err_ += "Customer is AMLO " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer is AMLO";
            //busobj.CloseConnectioDAL();
            //return;
        }

        Check_RF_CM();
        //busobj.CloseConnectioDAL();
        if (G_RFCM.Text.Trim() == "Y")
        {
            err_ += "Customer is Refinance " + "\r\n";
            res_err = false;
            //L_message.Text = "Customer is Refinance";
            //busobj.CloseConnectioDAL();
            //return;
        }


        string Ver_Cust_Type = "", Ver_EmployeeType = "", not_have_th = "", not_have_tm = "",
               ver01 = "", ver02 = "", ver03 = "", ver04 = "", ver07 = "", ver08 = "",
               ver09 = "", ver10 = "", ver11 = "", ver12 = "", ver12_1 = "", ver13 = "", ver15 = "", ver16 = "", ver17 = "",
               ver18 = "", ver19 = "", ver20 = "", ver21 = "", ver21_1 = "", ver22 = "", ver23 = "",
               Ver_TO = "", Ver_TH = "", Ver_TM = "", Ver_TE = "";

        //DS = busobj.RetriveAsDataSet($@"Select w5cusr as I_CUST, w5ousr as I_TO, w5husr as I_TH, w5musr as I_TM, w5eusr as I_TE, 
        //            w5caus as K_CUST, w5oaus as K_TO, w5haus as K_TH, w5maus as K_TM, w5eaus as K_TE, 
        //            trim(w5csty) as CustType, W5SBCD as SubCustType, w5emst as EmployeeType, w5vrth as not_have_th, w5vrtm as not_have_tm, 
        //            w5salt as ver01, w5inet as ver02, w5sso as ver03, w5bol as ver04, w5voot as ver07, w5vonm as ver08, 
        //            w5voad as ver09, w5emst as ver10, w5rvto as ver11, w5ipto as ver12, w5tito as ver12_1, w5tyem as ver13, 
        //            w5htl as ver15, w5vhht as ver16, w5vhnm as ver17, w5vhad as ver18, w5tytl as ver19, w5rvth as ver20, w5ipth as ver21, w5tith as ver21_1, 
        //            w5mbtl as ver22, w5vmtl as ver23, w5fvto, w5fvth, w5fvtm, w5fvte 
        //            from ilwk05 
        //            where w5brn = {hid_brn.Value} and w5apno = {hid_AppNo.Value} ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"Select w5cusr as I_CUST, w5ousr as I_TO, w5husr as I_TH, w5musr as I_TM, w5eusr as I_TE, 
                    w5caus as K_CUST, w5oaus as K_TO, w5haus as K_TH, w5maus as K_TM, w5eaus as K_TE, 
                    trim(w5csty) as CustType, W5SBCD as SubCustType, w5emst as EmployeeType, w5vrth as not_have_th, w5vrtm as not_have_tm, 
                    w5salt as ver01, w5inet as ver02, w5sso as ver03, w5bol as ver04, w5voot as ver07, w5vonm as ver08, 
                    w5voad as ver09, w5emst as ver10, w5rvto as ver11, w5ipto as ver12, w5tito as ver12_1, w5tyem as ver13, 
                    w5htl as ver15, w5vhht as ver16, w5vhnm as ver17, w5vhad as ver18, w5tytl as ver19, w5rvth as ver20, w5ipth as ver21, w5tith as ver21_1, 
                    w5mbtl as ver22, w5vmtl as ver23, w5fvto, w5fvth, w5fvtm, w5fvte 
                    from AS400DB01.ILOD0001.ilwk05 WITH (NOLOCK)
                    where w5brn = {hid_brn.Value} and w5apno = {hid_AppNo.Value} ", CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {

            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                Ver_Cust_Type = dr["CustType"].ToString().Trim();
                Ver_EmployeeType = dr["EmployeeType"].ToString().Trim();
                not_have_th = dr["not_have_th"].ToString().Trim();
                not_have_tm = dr["not_have_tm"].ToString().Trim();
                ver01 = dr["ver01"].ToString().Trim();
                ver02 = dr["ver02"].ToString().Trim();
                ver03 = dr["ver03"].ToString().Trim();
                ver04 = dr["ver04"].ToString().Trim();
                ver07 = dr["ver07"].ToString().Trim();
                ver08 = dr["ver08"].ToString().Trim();
                ver09 = dr["ver09"].ToString().Trim();
                ver10 = dr["ver10"].ToString().Trim();
                ver11 = dr["ver11"].ToString().Trim();
                ver12 = dr["ver12"].ToString().Trim();
                ver12_1 = dr["ver12_1"].ToString().Trim();
                ver13 = dr["ver13"].ToString().Trim();
                ver15 = dr["ver15"].ToString().Trim();
                ver16 = dr["ver16"].ToString().Trim();
                ver17 = dr["ver17"].ToString().Trim();
                ver18 = dr["ver18"].ToString().Trim();
                ver19 = dr["ver19"].ToString().Trim();
                ver20 = dr["ver20"].ToString().Trim();
                ver21 = dr["ver21"].ToString().Trim();
                ver21_1 = dr["ver21_1"].ToString().Trim();
                ver22 = dr["ver22"].ToString().Trim();
                ver23 = dr["ver23"].ToString().Trim();

                Ver_TO = dr["w5fvto"].ToString().Trim();
                Ver_TH = dr["w5fvth"].ToString().Trim();
                Ver_TM = dr["w5fvtm"].ToString().Trim();
                Ver_TE = dr["w5fvte"].ToString().Trim();

                if (dr["I_CUST"].ToString().Trim() == "")
                {
                    err_ += "Please verify Customer Type" + "\r\n";
                    res_err = false;
                    //L_message.Text = "Please verify Customer Type";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //busobj.CloseConnectioDAL();
                    //return;
                }

                if (dr["CustType"].ToString().Trim() == "N" ||
                    dr["SubCustType"].ToString().Trim() == "01" || dr["SubCustType"].ToString().Trim() == "02" || dr["SubCustType"].ToString().Trim() == "06")
                {
                    if (dr["I_TO"].ToString().Trim() == "")
                    {
                        err_ += "Please verify TO" + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please verify TO";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                    if (dr["I_TH"].ToString().Trim() == "")
                    {
                        err_ += "Please verify TH" + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please verify TH";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                    if (dr["I_TM"].ToString().Trim() == "")
                    {
                        err_ += "Please verify TM" + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please verify TM";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                    if (dr["I_TE"].ToString().Trim() == "")
                    {
                        err_ += "Please verify TE" + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please verify TE";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                }
                else if (dr["CustType"].ToString().Trim() == "O")
                {
                    if (dr["SubCustType"].ToString().Trim() == "03")
                    {
                        if (dr["I_TM"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TM" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TM";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["I_TE"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TE" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TE";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                    }
                    else if (dr["SubCustType"].ToString().Trim() == "04")
                    {
                        if (dr["I_TO"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TO" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TO";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }

                        if (dr["I_TM"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TM" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TM";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["I_TE"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TE" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TE";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                    }
                    else if (dr["SubCustType"].ToString().Trim() == "05")
                    {
                        if (dr["I_TH"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TH" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TH";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["I_TM"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TM" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TM";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["I_TE"].ToString().Trim() == "")
                        {
                            err_ += "Please verify TE" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please verify TE";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                    }
                }

                if (hid_status.Value != "INTERVIEW")
                {
                    if (dr["K_CUST"].ToString().Trim() == "")
                    {
                        err_ += "Please Confirm Customer Type" + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please Confirm Customer Type";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }

                    if (dr["CustType"].ToString().Trim() == "N" ||
                        dr["SubCustType"].ToString().Trim() == "01" || dr["SubCustType"].ToString().Trim() == "02" || dr["SubCustType"].ToString().Trim() == "06")
                    {
                        if (dr["K_TO"].ToString().Trim() == "")
                        {
                            err_ += "Please Confirm TO" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please Confirm TO";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["K_TH"].ToString().Trim() == "")
                        {
                            err_ += "Please Confirm TH" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please Confirm TH";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["K_TM"].ToString().Trim() == "")
                        {
                            err_ += "Please Confirm TM" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please Confirm TM";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                        if (dr["K_TE"].ToString().Trim() == "")
                        {
                            err_ += "Please Confirm TE" + "\r\n";
                            res_err = false;
                            //L_message.Text = "Please Confirm TE";
                            //P_message_TCL.ShowOnPageLoad = true;
                            //busobj.CloseConnectioDAL();
                            //return;
                        }
                    }
                    else if (dr["CustType"].ToString().Trim() == "O")
                    {
                        if (dr["SubCustType"].ToString().Trim() == "03")
                        {

                            if (dr["K_TM"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TM" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TM";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                            if (dr["K_TE"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TE" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TE";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                        }
                        else if (dr["SubCustType"].ToString().Trim() == "04")
                        {
                            if (dr["K_TO"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TO" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TO";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }

                            if (dr["K_TM"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TM" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TM";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                            if (dr["K_TE"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TE" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TE";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                        }
                        else if (dr["SubCustType"].ToString().Trim() == "05")
                        {
                            if (dr["K_TH"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TH" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TH";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                            if (dr["K_TM"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TM" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TM";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                            if (dr["K_TE"].ToString().Trim() == "")
                            {
                                err_ += "Please Confirm TE" + "\r\n";
                                res_err = false;
                                //L_message.Text = "Please Confirm TE";
                                //P_message_TCL.ShowOnPageLoad = true;
                                //busobj.CloseConnectioDAL();
                                //return;
                            }
                        }
                    }
                }//if Status <> Interview
            }




            DS.Clear();
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        //Check Judgment
        string birthdate_csms13 = "", occupation_csms13 = "", commercialregis = "", salary_csms13 = "", Province_O = "";
        //DS = busobj.RetriveAsDataSet("Select m13cru as I_JUD, m13aus as K_JUD, m13bdt, m13occ, substr(m13fil,22,1) as commercialregis, m13net,M13OPV from csms13 " +
        //                             "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " " +
        //                             "and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select m13cru as I_JUD, m13aus as K_JUD, m13bdt, m13occ, SUBSTRING(m13fil,22,1) as commercialregis, m13net,M13OPV from AS400DB01.CSOD0001.CSMS13 WITH (NOLOCK) " +
                                     "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " " +
                                     "and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ",CommandType.Text).Result.data;



        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                birthdate_csms13 = dr["m13bdt"].ToString().Trim();
                occupation_csms13 = dr["m13occ"].ToString().Trim();
                commercialregis = dr["commercialregis"].ToString().Trim();
                salary_csms13 = dr["m13net"].ToString().Trim();
                Province_O = dr["M13OPV"].ToString().Trim();
                if (dr["I_JUD"].ToString().Trim() == "")
                {
                    err_ += "Please key Judgment " + "\r\n";
                    res_err = false;
                    //L_message.Text = "Please key Judgment ";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //busobj.CloseConnectioDAL();
                    //return;
                }
                if (hid_status.Value != "INTERVIEW")
                {
                    if (dr["K_JUD"].ToString().Trim() == "")
                    {
                        err_ += "Please Confirm Judgment " + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please Confirm Judgment";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                }
            }
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();
        //checkAutoReject(postCode_off, office_name, salary, ref G_aprj, ref G_loca, ref G_reason, ref G_Msg, ref G_err);


        //Check Product &  Resend Bureau
        //DS = busobj.RetriveAsDataSet("Select p1csno, substr(p1fill,28,2) as SaveProduct , substr(p1fill,21,1) as chk_ncb from ilms01 " +
        //                             "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select p1csno, SUBSTRING(p1fill,28,2) as SaveProduct , SUBSTRING(p1fill,21,1) as chk_ncb from AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)  " +
                                    "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ",CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                //   edit  check NCB Result 
                if (dr["chk_ncb"].ToString().Trim() == "T")
                {
                    //Start Pact 20170822 check net salary
                    bool checksal = true;
                    CheckNetSalary("APPROVE", busobj, ref err_, ref checksal);
                    if (!checksal)
                    {
                        return;
                    }
                    //End Pact 20170822 
                }

                if (dr["chk_ncb"].ToString().Trim() == "" || dr["chk_ncb"].ToString().Trim() == "C")
                {
                    err_ += "Please Resend Bureau " + "\r\n";
                    res_err = false;
                }

                if (dr["chk_ncb"].ToString().Trim() == "B" || dr["chk_ncb"].ToString().Trim() == "R" || dr["chk_ncb"].ToString().Trim() == "O"
                    || dr["chk_ncb"].ToString().Trim() == "S" || dr["chk_ncb"].ToString().Trim() == "L"
                    )
                {
                    err_ += "Result Bureau is reject status , Please Check data " + "\r\n";
                    res_err = false;
                }


                if (hid_status.Value == "INTERVIEW")
                {
                    if ((dr["SaveProduct"].ToString().Trim() != "PI") & (dr["SaveProduct"].ToString().Trim() != "PK"))
                    {
                        err_ += "Please Save Product Data " + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please Save Product Data";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }
                }
                if (hid_status.Value == "KESSAI")
                {
                    if (dr["SaveProduct"].ToString().Trim() != "PK")
                    {
                        err_ += "Please Confirm Product Data " + "\r\n";
                        res_err = false;
                        //L_message.Text = "Please Confirm Product Data";
                        //P_message_TCL.ShowOnPageLoad = true;
                        //busobj.CloseConnectioDAL();
                        //return;
                    }


                }
            }
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();
        ////Check Name Eng
        //E_nameeng1.Text = E_nameeng1.Text.ToUpper();
        //E_surnameeng1.Text = E_surnameeng1.Text.ToUpper();
        //string ErrorMsg = "";
        //bool call_GNSRNM = busobj.Call_GNSRNM(E_nameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        //if (Error.ToString().Trim() == "Y")
        //{
        //    err_ += "กรุณาใส่ชื่อลูกค้าเป็นภาษาอังกฤษ " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาใส่ชื่อลูกค้าเป็นภาษาอังกฤษ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        ////Check SurName Thai
        //call_GNSRNM = busobj.Call_GNSRNM(E_surnameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        //if (Error.ToString().Trim() == "Y")
        //{
        //    err_ += "กรุณาใส่นามสกุลลูกค้าเป็นภาษาอังกฤษ " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาใส่นามสกุลลูกค้าเป็นภาษาอังกฤษ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}

        //if (D_shipto1.SelectedIndex == 0)
        //{
        //    err_ += "กรุณาระบุ 'Ship to' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Ship to' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (E_nameeng1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Eng Name' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Eng Name' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (E_surnameeng1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Eng Surname'" + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Eng Surname' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}

        //if ((D_building1.Value == null) & (E_buildingname1.Text != ""))
        //{
        //    err_ += "กรุณาระบุ 'Building Title' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Building Title' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if ((D_building1.Value != null) & (E_buildingname1.Text == ""))
        //{
        //    err_ += "กรุณาระบุ 'Building Name' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Building Name' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}

        //if (E_addressno1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Address No' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Address No' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (E_moo1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Moo' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Moo' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (E_road1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Road' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Road' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (D_tambol1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Tambol' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Tambol' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (D_amphur1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Amphur'  " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Amphur' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}
        //if (D_province1.Text.Trim() == "")
        //{
        //    err_ += "กรุณาระบุ 'Province' " + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "กรุณาระบุ 'Province' ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}

        if (Ver_Cust_Type.ToString().Trim() == "Z")
        {
            err_ += "Customer Type = 'Z' Can not Approve'" + "\r\n";
            res_err = false;
            //L_message.Text = "Customer Type = 'Z' Can not Approve' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        if (Ver_Cust_Type.ToString().Trim() != "S")
        {
            if (Ver_EmployeeType.ToString().Trim() == "02")
            {
                err_ += "ลูกค้าไม่ได้เป็นพนักงานไม่สามารถ Approve ได้" + "\r\n";
                res_err = false;
                //L_message.Text = "ลูกค้าไม่ได้เป็นพนักงานไม่สามารถ Approve ได้";
                //P_message_TCL.ShowOnPageLoad = true;
                //busobj.CloseConnectioDAL();
                //return;
            }
        }

        if (Ver_TO == "Y")
        {
            if ((ver01.ToString().Trim() == "") || (ver02.ToString().Trim() == "0") || (ver03.ToString().Trim() == "") || (ver04.ToString().Trim() == "") ||
               //(ver07.ToString().Trim() == "") || (ver08.ToString().Trim() == "") || (ver09.ToString().Trim() == "") || (ver10.ToString().Trim() == "") ||
               (ver08.ToString().Trim() == "") || (ver09.ToString().Trim() == "") || (ver10.ToString().Trim() == "") ||
               (ver11.ToString().Trim() == "") || (ver12.ToString().Trim() == "") || (ver12_1.ToString().Trim() == "") || (ver13.ToString().Trim() == ""))
            {
                err_ += "กรณี Approve จะต้องใส่ข้อมูล Verify TO ให้ครบถ้วน" + "\r\n";
                res_err = false;
                //L_message.Text = "กรณี Approve จะต้องใส่ข้อมูล Verify TO ให้ครบถ้วน";
                //P_message_TCL.ShowOnPageLoad = true;
                //busobj.CloseConnectioDAL();
                //return;
            }
        }

        if (Ver_TH == "Y")
        {
            if (not_have_th != "Y")
            {
                if ((ver15.ToString().Trim() == "") || (ver17.ToString().Trim() == "") || (ver18.ToString().Trim() == "") ||
                    //((ver15.ToString().Trim() == "") || (ver16.ToString().Trim() == "") || (ver17.ToString().Trim() == "") || (ver18.ToString().Trim() == "") ||
                    (ver19.ToString().Trim() == "") || (ver20.ToString().Trim() == "") || (ver21.ToString().Trim() == ""))
                {
                    err_ += "กรณี Approve จะต้องใส่ข้อมูล Verify TH ให้ครบถ้วน" + "\r\n";
                    res_err = false;
                    //L_message.Text = "กรณี Approve จะต้องใส่ข้อมูล Verify TH ให้ครบถ้วน";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //busobj.CloseConnectioDAL();
                    //return;
                }
            }
        }

        if (Ver_TM == "Y")
        {
            if (not_have_tm != "Y")
            {
                if ((ver22.ToString().Trim() == "") || (ver23.ToString().Trim() == ""))
                {
                    err_ += "กรณี Approve จะต้องใส่ข้อมูล Verify TM ให้ครบถ้วน" + "\r\n";
                    res_err = false;
                    //L_message.Text = "กรณี Approve จะต้องใส่ข้อมูล Verify TM ให้ครบถ้วน";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //busobj.CloseConnectioDAL();
                    //return;
                }
            }
        }

        //Check Reference = "Y"
        if (Ver_TE == "Y")
        {
            int count_ref_y = 0;
            //DS = busobj.RetriveAsDataSet("select w12vte from ilwk12 " +
            //                             "where w12brn = " + hid_brn.Value + " and w12apn = " + hid_AppNo.Value + " ");
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select w12vte from AS400DB01.ILOD0001.ilwk12 WITH (NOLOCK)  " +
                                         "where w12brn = " + hid_brn.Value + " and w12apn = " + hid_AppNo.Value + " ",CommandType.Text).Result.data;
            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    if (dr["w12vte"].ToString().Trim() == "Y")
                    {
                        count_ref_y = 1;
                    }
                }
            }
            if (count_ref_y == 0)
            {
                err_ += "ไม่มีข้อมูล Verify flag TE ไม่สามารถ Approve ได้" + "\r\n";
                res_err = false;
                //L_message.Text = "ไม่มีข้อมูล Verify flag TE ไม่สามารถ Approve ได้";
                //P_message_TCL.ShowOnPageLoad = true;
                //busobj.CloseConnectioDAL();
                //return;
            }
        }

        //if (hid_status.Value == "INTERVIEW")
        //{
        //    L_confirm.Text = "Do you want to Approve by Interview?";
        //}
        //else
        //{

        string IDExpire = "", off_title = "", off_name = "", tel_office = "";
        if (hid_status.Value == "KESSAI")
        {
            if ((Convert.ToDecimal(E_Apv_avi.Text) < Convert.ToDecimal(Loan_Amt.Text)) || (Convert.ToDecimal(EdtCrAvi.Text) < Convert.ToDecimal(Loan_Amt.Text)))
            {
                err_ += "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา" + "\r\n";
                res_err = false;

                //L_message.Text = "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา";
                //P_message_TCL.ShowOnPageLoad = true;
                //busobj.CloseConnectioDAL();
                //return;
            }
        }


        string WPPER = "";
        string WPERR = "";
        string WPOLR = "";

        //DS = busobj.RetriveAsDataSet("select m00oft as off_title, m00ofc as off_name, m00eid from csms00 where m00csn = " + hid_CSN.Value + " and m00sts = '' ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT gc.Code as off_title, cw.OfficeName as off_name, FORMAT(cg.IDCardExpiredDate,'yyyyMMdd','th-TH') as m00eid
                                        FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                                        JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) ON (cg.ID = cw.CustID)
                                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (cw.OfficeTitleID = gc.ID AND gc.Type = 'OfficeTitleID') 
                                        WHERE cg.CISNumber = {hid_CSN.Value} ", CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                IDExpire = dr["m00eid"].ToString().Trim();
                off_title = dr["off_title"].ToString().Trim();
                off_name = dr["off_name"].ToString().Trim();
            }
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        #region newCheck blacklist company 07/2565

        try
        {
            var a = _connectAPI.getWebApi("apiCompanyBlacklist", "CompanyBlacklist", "getCompanyBlackList", new string[] { off_name, off_title });
            CompanyBlacklist.responseCompanyBlacklist companyBlacklist = JsonConvert.DeserializeObject<CompanyBlacklist.responseCompanyBlacklist>(a);

            if (companyBlacklist.Success == false)
            {
                err_ += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง" + "\r\n";
                res_err = false;
            }
            if (companyBlacklist.data != null)
            {
                if (companyBlacklist.data.CompanyFlag == "T")
                {
                    #region before 20220901
                    //err_ += off_name + " ติด Temporary Blacklist ไม่สามารถทำการ Approve ได้" + "\r\n";
                    //res_err = false;
                    #endregion
                    WPERR = "";
                    string WPHSTS = "";
                    string WPMSG = "";
                    bool res_cs = iLDataSubroutine.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                    //busobj.CloseConnectioDAL();
                    if (!res_cs || WPERR == "Y")
                    {
                        err_ += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง" + "\r\n";
                        res_err = false;
                    }
                    if (WPHSTS == "N") // ลูกค้าใหม่  
                    {
                        err_ += "[ลูกค้าใหม่] บริษัท " + off_name + " ติด Temporary Blacklist (T) ไม่สามารถทำการ Approve ได้" + "\r\n";
                        res_err = false;
                    }
                    else
                    {
                        lb_backlist.Text += "[ลูกค้าเก่า] บริษัท " + off_name + " is a Temporary blacklist company(T)." + "\r\n";
                    }
                }
                else if (companyBlacklist.data.CompanyFlag == "P")
                {
                    WPERR = "";
                    string WPHSTS = "";
                    string WPMSG = "";
                    bool res_cs = iLDataSubroutine.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                    //busobj.CloseConnectioDAL();
                    if (!res_cs || WPERR == "Y")
                    {
                        err_ += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง" + "\r\n";
                        res_err = false;
                    }
                    if (WPHSTS == "N") // ลูกค้าใหม่  
                    {
                        err_ += "[ลูกค้าใหม่] บริษัท " + off_name + " ติด Blacklist (P) ไม่สามารถทำการ Approve ได้" + "\r\n";
                        res_err = false;
                    }
                    else
                    {
                        lb_backlist.Text += "[ลูกค้าเก่า] บริษัท " + off_name + " is a blacklist company(P)." + "\r\n";
                    }
                }
                else if (companyBlacklist.data.CompanyFlag == "S")
                {
                    lb_backlist.Text += " Office name : " + off_name + " is a pending company.(S)";//confirmsave
                }
            }
        }
        catch (Exception ex)
        {
            err_ += " /:" + ex.Message;
            res_err = false;
        }
        #endregion

        //Edited by Supawadeep  เพิ่มเรื่อง check blacklist company 17/03/2558
        #region
        //bool res_BL = busobj.Call_GNSRCBST(off_name, ref WPPER, ref WPERR, ref WPOLR, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        //if (!res_BL || WPERR == "Y")
        //{
        //    err_ += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง" + "\r\n";
        //    res_err = false;

        //}
        //if (WPPER == "T")
        //{
        //    lb_backlist.Text += "Office name : " + off_name + " is a pending blacklist company ";
        //}
        //else if (WPPER == "P")
        //{
        //    WPERR = "";
        //    string WPHSTS = "";
        //    string WPMSG = "";
        //    bool res_cs = busobj.Call_GNSRCS(hid_idno.Value, "IL", userInfo.BranchApp.ToString(), "1", userInfo.BizInit.ToString(), userInfo.BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
        //    busobj.CloseConnectioDAL();
        //    if (!res_cs || WPERR == "Y")
        //    {
        //        err_ += "ไม่สามารถตรวจสอบข้อมูลลูกค้าได้ กรุณาทำรายการใหม่อีกครั้ง" + "\r\n";
        //        res_err = false;

        //    }
        //    if (WPHSTS == "N") // ลูกค้าใหม่  
        //    {
        //        err_ += "[ลูกค้าใหม่] บริษัท " + off_name + " ติด Blacklist ไม่สามารถทำการ Approve ได้" + "\r\n";
        //        res_err = false;
        //    }
        //    else
        //    {
        //        lb_backlist.Text += "[ลูกค้าเก่า] Office name : " + off_name + " is a blacklist company ";

        //    }
        //}
        #endregion




        // edit ต้องดึงข้อมูล Province  มาจาก CSMS13

        //DS = busobj.RetriveAsDataSet("select m11prv, m11tel from csms11 where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11cde = 'O' ");
        //if (DS != null)
        //{
        //    foreach (DataRow dr in DS.Tables[0].Rows)
        //    {
        //        Province_O = dr["m11prv"].ToString().Trim();
        //        tel_office = dr["m11tel"].ToString().Trim();
        //    }
        //}
        // busobj.CloseConnectioDAL();

        //REQ70694 Cancel Check Office Province
        //DS = busobj.RetriveAsDataSet("select g06cde from gntb106 where g06cde = " + Province_O.ToString() + " and g06del='' ");
        //if (DS != null)
        //{
        //    foreach (DataRow dr in DS.Tables[0].Rows)
        //    {
        //        if (dr["g06cde"].ToString().Trim() != "")
        //        {
        //            err_ += "จังหวัดที่ทำงานของลูกค้าไม่สามารถ Approve ได้" + "\r\n";
        //            res_err = false;
        //            //L_message.Text = "จังหวัดที่ทำงานของลูกค้าไม่สามารถ Approve ได้";
        //            //P_message_TCL.ShowOnPageLoad = true;
        //            //busobj.CloseConnectioDAL();
        //            //return;
        //        }
        //    }
        //}
        //busobj.CloseConnectioDAL();


        //bool call_GNP023 = busobj.Call_GNP023(IDExpire.ToString(), "YMD", "B", "30", "D", "-",
        //                                        ref CalcDate, ref Error, userInfo.BizInit, userInfo.BranchNo);

        string AppDate = "";
        iLDataSubroutine.Call_ILSR97("01", "YMD", userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(), ref AppDate);
        //busobj.CloseConnectioDAL();
        
        string CalcDate = "";
        bool call_GNP023 = iLDataSubroutine.Call_GNP023(AppDate, "YMD", "B", "30", "D", "-",
                                                ref CalcDate, ref Error, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        if (Convert.ToInt32(IDExpire.ToString()) < Convert.ToInt32(hid_date97.Value))
        {
            if ((Convert.ToInt32(IDExpire.ToString()) >= Convert.ToInt32(CalcDate)) & (Convert.ToInt32(IDExpire.ToString()) < Convert.ToInt32(hid_date97.Value)))
            {
                L_othmsg.Text = "บัตรหมดอายุแล้ว โปรดตรวจสอบ";
            }
            if (Convert.ToInt32(IDExpire.ToString()) < Convert.ToInt32(CalcDate))
            {
                err_ += "บัตรหมดอายุ > 30 วัน ไม่สามารถอนุมัติได้" + "\r\n";
                res_err = false;
                //L_message.Text = "บัตรหมดอายุ > 30 วัน ไม่่สามารถอนุมัติได้";
                //P_message_TCL.ShowOnPageLoad = true;
                //busobj.CloseConnectioDAL();
                //return;
            }
        }

        string in_AGE = "", error = "";
        iLDataSubroutine.CALL_GNP0371(birthdate_csms13.ToString(), hid_date97.Value, "YMD", "B", "01", "IL", "1",
                                        userInfo.BizInit.ToString(), userInfo.BranchNo.ToString(),
                                        ref in_AGE, ref error);
        //busobj.CloseConnectioDAL();
        if (error.ToString() != "")
        {
            err_ += "อายุของลูกค้าไม่อยู่ในช่วง 20 - 55 ปี " + "\r\n";
            res_err = false;
            //L_message.Text = "อายุของลูกค้าไม่อยู่ในช่วง 20 - 55 ปี ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            return;
        }

        // *** 	ตรวจสอบข้อมูล Business ***//
        string POERRC = "";
        string POERRM = "";
        if (commercialregis.ToString() == "")
        {
            commercialregis = "N";
        }
        iLDataSubroutine.Call_CSSR035(hid_CSN.Value, occupation_csms13.ToString(), commercialregis.ToString(), ref POERRC, ref POERRM, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        if (POERRC != "000")
        {
            L_message.Text = "Error from CSSR035 : " + POERRC.ToString() + "-" + POERRM.ToString();
            P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            return;
        }


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

        //DS = busobj.RetriveAsDataSet("select t05inc from cstb05 " +
        //                                "where t05brn = " + hid_brn.Value + " and t05app='IL' and " +
        //                                "t05ven=0 and t05opr = '" + mode.ToString() + "' ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("SELECT t05inc from AS400DB01.CSOD0001.CSTB05 WITH (NOLOCK) " +
                                        "where t05brn = " + hid_brn.Value + " and t05app='IL' and " +
                                        "t05ven=0 and t05opr = '" + mode.ToString() + "' ", CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if ((Convert.ToDecimal(salary_csms13)) < Convert.ToDecimal(dr["t05inc"].ToString().Trim()))
                {
                    err_ += "เงินเดือนของลูกค้าไม่ผ่านเกณฑ์ขั้นต่ำ ไม่สามารถ Approve ได้ " + "\r\n";
                    res_err = false;
                    //L_message.Text = "เงินเดือนของลูกค้าไม่ผ่านเกณฑ์ขั้นต่ำ กรุณาตรวจสอบ";
                    //P_message_TCL.ShowOnPageLoad = true;
                    //busobj.CloseConnectioDAL();
                    //return;
                }
            }
        }

        /*
        string Kessai_Avai = "";
        DS = busobj.RetriveAsDataSet("select gt02am from gntb02 where gt02ap='IL' and gt02cd = '" + userInfo.Username.ToString() + "' ");
        if (DS != null)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                try
                {
                    Kessai_Avai = dr["gt02am"].ToString().Trim();
                }
                catch { }
            }
        }
        if ((Kessai_Avai.ToString() == "") || (Kessai_Avai.ToString() == null))
        {
            L_message.Text = "User : " + userInfo.Username.ToString() + " ยังไม่ได้ทำการกำหนดวงเงินกรุณาติดต่อแผนก OC";
            P_message_TCL.ShowOnPageLoad = true;
            busobj.CloseConnectioDAL();
            return;
        }
        if (Convert.ToDecimal(Kessai_Avai.ToString()) < Convert.ToDecimal(Loan_Amt.Text))
        {
            L_message.Text = "User : " + userInfo.Username.ToString() + " วงเงินที่ Approve ได้ น้อยกว่ายอด Loan Request ";
            P_message_TCL.ShowOnPageLoad = true;
            busobj.CloseConnectioDAL();
            return;
        }
        if (Convert.ToDecimal(Kessai_Avai.ToString()) < Convert.ToDecimal(G_TCL_13.Text))
        {
            L_message.Text = "User : " + userInfo.Username.ToString() + " วงเงินที่ Approve ได้ น้อยกว่ายอด TCL ";
            P_message_TCL.ShowOnPageLoad = true;
            busobj.CloseConnectioDAL();
            return;
        }
        */

        if (hid_birthdate.Value.ToString().Trim() != birthdate_csms13.ToString().Trim())
        {
            err_ += "Birth date ไม่ตรงกับในระบบไม่สามารถ Approve ได้ โปรดแก้ไขและทำการ Save Judgment Form อีกครั้ง " + "\r\n";
            res_err = false;
            //L_message.Text = "Birth date ไม่ตรงกับในระบบไม่สามารถ Approve ได้ โปรดแก้ไขและทำการ Save Judgment Form อีกครั้ง ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        //if (Convert.ToDecimal(EdtCrAvi.Text) < Convert.ToDecimal(Loan_Amt.Text))
        //{
        //    err_ += "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา" + "\r\n";
        //    res_err = false;
        //    //L_message.Text = "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา ";
        //    //P_message_TCL.ShowOnPageLoad = true;
        //    //busobj.CloseConnectioDAL();
        //    //return;
        //}

        L_message.ForeColor = System.Drawing.Color.Blue;
        //if (!res_err)
        //{
        //    L_message.ForeColor = System.Drawing.Color.Red;
        //    L_message.Text = err_;
        //    P_message_TCL.ShowOnPageLoad = true;
        //    busobj.CloseConnectioDAL();
        //    return;
        //}

        string OAPP = "", OMSGA = "", OTEL = "", OMSGT = "", ONER = "", OMSG = "", str_confirm = "";
        bool res_45 = iLDataSubroutine.Call_GNSR45(off_name.ToString(), off_title.ToString(), tel_office.ToString(), "IL", userInfo.BranchApp, hid_AppNo.Value, hid_appdate.Value,
                                            ref OAPP, ref OMSGA, ref OTEL, ref OMSGT, ref ONER, ref OMSG, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        //res_45 = true;
        if (!res_45)
        {
            L_message.ForeColor = System.Drawing.Color.Red;
            L_message.Text = "Error GNSR45 ";
            P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            return;
        }
        if (ONER == "N")
        {
            str_confirm += OMSG;
        }

        if (ONER == "Y")
        {
            str_confirm += OMSGA + " " + OMSGT;
        }



        string t22cod = "", t22seq = "", sqlret = "", ver_contact = ver11;

        bool check_notHaveTH = not_have_th.Trim() == "Y" ? true : false;

        // กรณีเป็นลูกค้าเก่าจะไม่ติด OL 7
        
        if (!iLDataCenterMssqlUsing.checkApproveCriteria(busobj, E_product.Text, E_vendor.Text, userInfo.BranchApp, hid_AppNo.Value, hid_appdate.Value, "01", hid_CSN.Value,
                                            hid_idno.Value, hid_date97.Value, "ILNORMWEB", check_notHaveTH /*false*/, userInfo.BizInit, userInfo.BranchNo, userInfo.Username, userInfo.LocalClient,
                                            ref t22cod, ref t22seq, ref sqlret, ver_contact))
        {
            //err_ += "วงเงินคงเหลือน้อยกว่าวงเงินที่ขอทำสัญญา" + "\r\n";
            //res_err = false;
            L_message.ForeColor = System.Drawing.Color.Red;
            L_message.Text = " Can not Approve this customer. Check by condition code : " + t22cod + " Seq: " + t22seq;
            P_message_TCL.ShowOnPageLoad = true;
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }

        //Check Name Eng
        E_nameeng1.Text = E_nameeng1.Text.ToUpper();
        E_surnameeng1.Text = E_surnameeng1.Text.ToUpper();
        GNSRNM gNSRNM = new GNSRNM();
        string ErrorMsg = "";
        //bool call_GNSRNM = iLDataSubroutine.Call_GNSRNM(E_nameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        bool call_GNSRNM = gNSRNM.Call_GNSRNM(E_nameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        if (Error.ToString().Trim() == "Y")
        {
            err_ += "กรุณาใส่ชื่อลูกค้าเป็นภาษาอังกฤษ " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาใส่ชื่อลูกค้าเป็นภาษาอังกฤษ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        //Check SurName Thai
        //call_GNSRNM = iLDataSubroutine.Call_GNSRNM(E_surnameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        call_GNSRNM = gNSRNM.Call_GNSRNM(E_surnameeng1.Text.Trim(), "A", "E", ref Error, ref ErrorMsg, userInfo.BizInit, userInfo.BranchNo);
        //busobj.CloseConnectioDAL();
        if (Error.ToString().Trim() == "Y")
        {
            err_ += "กรุณาใส่นามสกุลลูกค้าเป็นภาษาอังกฤษ " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาใส่นามสกุลลูกค้าเป็นภาษาอังกฤษ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        if (D_shipto1.SelectedIndex == 0)
        {
            err_ += "กรุณาระบุ 'Ship to' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Ship to' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (E_nameeng1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Eng Name' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Eng Name' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (E_surnameeng1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Eng Surname'" + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Eng Surname' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        if ((D_building1.Value == null) & (E_buildingname1.Text != ""))
        {
            err_ += "กรุณาระบุ 'Building Title' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Building Title' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if ((D_building1.Value != null) & (E_buildingname1.Text == ""))
        {
            err_ += "กรุณาระบุ 'Building Name' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Building Name' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        if (E_addressno1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Address No' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Address No' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (E_moo1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Moo' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Moo' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (E_road1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Road' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Road' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (D_tambol1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Tambol' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Tambol' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (D_amphur1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Amphur'  " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Amphur' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }
        if (D_province1.Text.Trim() == "")
        {
            err_ += "กรุณาระบุ 'Province' " + "\r\n";
            res_err = false;
            //L_message.Text = "กรุณาระบุ 'Province' ";
            //P_message_TCL.ShowOnPageLoad = true;
            //busobj.CloseConnectioDAL();
            //return;
        }

        if (!res_err)
        {
            L_message.ForeColor = System.Drawing.Color.Red;
            L_message.Text = err_;
            P_message_TCL.ShowOnPageLoad = true;
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        if (hid_status.Value == "INTERVIEW")
        {
            L_confirm_info.Text = str_confirm.ToString().Trim();
            L_confirm.Text = "Do you want to Approve by Interview?";
        }
        else
        {
            L_confirm_info.Text = str_confirm.ToString().Trim();
            L_confirm.Text = "Do you want to Approve Contract?";
        }

        P_confirm_TCL.ShowOnPageLoad = true;
    }

    protected void B_confirmcancel_Click(object sender, EventArgs e)
    {
        L_NCB.Text = "";
        B_confirmsave.Text = "Save";
        B_confirmcancel.Text = "Cancel";
        P_confirm_TCL.ShowOnPageLoad = false;
    }

    protected void B_confirmok_Click(object sender, EventArgs e)
    {
        P_message_TCL.ShowOnPageLoad = false;

        if (E_redirect.Text == "Y")
        {
            if ((L_confirm.Text == "Do you want to Cancel by Interview?") ||
               (L_confirm.Text == "Do you want to Reject by Interview?") ||
               (L_confirm.Text == "Do you want to Approve by Interview?"))
            {
                Response.Redirect(string.Format("IL_List_Interview.aspx?"));
                return;
            }
            if ((L_confirm.Text == "Do you want to Return to Interview?") ||
               (L_confirm.Text == "Do you want to Cancel by KESSAI?") ||
               (L_confirm.Text == "Do you want to Reject by KESSAI?") ||
               (L_confirm.Text == "Do you want to Approve by Interview?"))
            {
                Response.Redirect(string.Format("IL_List_Kessai.aspx?"));
                return;
            }
            Response.Redirect(string.Format("IL_List_Kessai.aspx?"));
            return;
        }
    }

    protected void B_confirmsave_Click(object sender, EventArgs e)
    {
        L_NCB.Text = "";
        L_contract.Text = "";
        P_confirm_TCL.ShowOnPageLoad = false;
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();
        iDB2Command cmd = new iDB2Command();
        DataSet DS = new DataSet();
        DataSet DSVendor = new DataSet();

        bool success = true;
        int affectedRows = 0;
        string Error = "";

        string find_csms11_shipto = "";
        try
        {
            //DS = busobj.RetriveAsDataSet("select m11csn from csms11 " +
            //                             "where m11csn = " + hid_CSN.Value + " and m11cde = '" + D_shipto1.Value.ToString() + "' and m11ref='' and m11rsq=0 ");
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT CISNumber as m11csn
                                        FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
                                        JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID AND ca.CustRefID = 0)
                                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID' )
                                        WHERE CISNumber = {hid_CSN.Value} AND gc.Code = '{D_shipto1.Value.ToString()}' ", CommandType.Text).Result.data;
            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    if (dr["m11csn"].ToString().Trim() != "")
                    {
                        find_csms11_shipto = "Y";
                    }
                }
                DS.Clear();
            }
        }
        catch (Exception ex)
        {
            //success = false;
            //Error = "Error , Please try again ";
            //goto commit_rollback;
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        string find_csms11_notshipto = "", notshipto = "";
        if ((L_confirm.Text == "Do you want to Approve by Interview?") ||
           (L_confirm.Text == "Do you want to Approve Contract?"))
        {
            if (D_shipto1.Value.ToString() == "H")
            {
                notshipto = "O";
            }
            if (D_shipto1.Value.ToString() == "O")
            {
                notshipto = "H";
            }
            try
            {
                //DS = busobj.RetriveAsDataSet("select m11csn from csms11 " +
                //                             "where m11csn = " + hid_CSN.Value + " and m11cde = '" + notshipto.ToString() + "' and m11ref='' and m11rsq=0 ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT CISNumber as m11csn
                                        FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
                                        JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID AND ca.CustRefID = 0)
                                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID' )
                                        WHERE CISNumber = {hid_CSN.Value} AND gc.Code = '{notshipto.ToString()}' ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        if (dr["m11csn"].ToString().Trim() != "")
                        {
                            find_csms11_notshipto = "Y";
                        }
                    }
                    DS.Clear();
                }
            }
            catch
            {
                //Error = "Error , Please try again ";
            }
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }

        string O_tambol = "", O_amphur = "", O_province = "", O_zipcode = "", H_tambol = "", H_amphur = "", H_province = "", H_zipcode = "";
        //DS = busobj.RetriveAsDataSet("select m13csn, m13otm, m13oam, m13opv, m13ozp, m13htm, m13ham, m13hpv, m13hzp from csms13 " +
        //                                "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m13csn, m13otm, m13oam, m13opv, m13ozp, m13htm, m13ham, m13hpv, m13hzp from AS400DB01.CSOD0001.CSMS13 WITH (NOLOCK) " +
                                        "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["m13csn"].ToString().Trim() != "")
                {
                    O_tambol = dr["m13otm"].ToString().Trim();
                    O_amphur = dr["m13oam"].ToString().Trim();
                    O_province = dr["m13opv"].ToString().Trim();
                    O_zipcode = dr["m13ozp"].ToString().Trim();
                    H_tambol = dr["m13htm"].ToString().Trim();
                    H_amphur = dr["m13ham"].ToString().Trim();
                    H_province = dr["m13hpv"].ToString().Trim();
                    H_zipcode = dr["m13hzp"].ToString().Trim();
                }
            }
            DS.Clear();
        }
        CallHisunMaster._dataCenter.CloseConnectSQL();

        string Mobile_SMS = "";
        //DS = busobj.RetriveAsDataSet("select m11mob from csms11 " +
        //                             "where m11csn = " + hid_CSN.Value + " and m11cde = 'H' and m11ref='' and m11rsq=0 ");
        DS = CallHisunMaster._dataCenter.GetDataset<DataTable>($@"SELECT Mobile as m11mob
                        FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
                        JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID AND ca.CustRefID = 0)
                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID' AND gc.Code = 'H' )
                        WHERE CISNumber = {hid_CSN.Value} ", CommandType.Text).Result.data;
        if (DS != null && DS.Tables.Count > 0)
        {
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["m11mob"].ToString().Trim() != "")
                {
                    Mobile_SMS = dr["m11mob"].ToString().Trim();
                }
            }
            DS.Clear();
        }
        CallHisunMaster._dataCenter.CloseConnectSQL(); 
        string Interview_User = "", Interview_Date = "", Kessai_User = "", Kessai_Date = "", Update_User = "", TimeSNO = "";

        if (m_UpdTime.ToString().Trim().Length == 4)
        {
            TimeSNO = "00" + m_UpdTime.ToString().Trim();
        }
        if (m_UpdTime.ToString().Trim().Length == 5)
        {
            TimeSNO = "0" + m_UpdTime.ToString().Trim();
        }
        if (m_UpdTime.ToString().Trim().Length == 6)
        {
            TimeSNO = m_UpdTime.ToString().Trim();
        }
        //if (CallHisunMaster._dataCenter.SqlCon.State != ConnectionState.Open)
        //    CallHisunMaster._dataCenter.OpenConnectSQL();
        //CallHisunMaster._dataCenter.Sqltr?.Connection = CallHisunMaster._dataCenter.SqlCon.BeginTransaction();

        string Payment_Abl = txt_pay_abl.Text.Trim().Replace(",", "").Trim().Substring(0, txt_pay_abl.Text.Trim().Replace(",", "").Trim().Length - 3);
        if (hid_status.Value == "INTERVIEW")
        {
            //Interview_User = userInfo.Username.ToString();
            //Interview_Date = hid_date97.Value;
            //Kessai_User = "";
            //Kessai_Date = "00";
            Update_User = "m13cru='" + userInfo.Username.ToString() + "', m13cud=" + hid_date97.Value + ", m13aus='', m13aud=0, ";
            Update_User = Update_User + "m13sno = CONCAT('" + TimeSNO.ToString() + "','000000','" + Payment_Abl + "'), ";
        }

        if (hid_status.Value == "KESSAI")
        {
            //Interview_User = "";
            //Interview_Date = "0";
            //Kessai_User = userInfo.Username.ToString();
            //Kessai_Date = hid_date97.Value;
            Update_User = "m13aus='" + userInfo.Username.ToString() + "', m13aud=" + hid_date97.Value + ", ";
            Update_User = Update_User + "m13sno = CONCAT(SUBSTRING(m13sno,1,6),'" + TimeSNO.ToString() + "','" + Payment_Abl + "'), ";
        }
        string SQL_Update_CSMS13 = "Update AS400DB01.CSOD0001.CSMS13 set " +
                                   "" + Update_User + " " +
                                   "m13udt = " + hid_date97.Value + ", " +
                                   "m13utm = " + m_UpdTime.ToString() + ", " +
                                   "m13usr = '" + userInfo.Username.ToString() + "', " +
                                   "m13upg = '" + hid_status.Value + "', " +
                                   "m13wks = '" + userInfo.LocalClient.ToString() + "' " +
                                   "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + " ";

        if (L_confirm.Text == "Do you want to Return to Interview?")
        {
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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                              "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'4',SUBSTRING(p1fill,26,13)), " +
                              "p1updt = " + hid_date97.Value + ", " +
                              "p1uptm = " + m_UpdTime.ToString() + ", " +
                              "p1upus = '" + userInfo.Username.ToString() + "', " +
                              "p1prog = 'KESSAIRET', " +
                              "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                              "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Return to Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Return to Interview";
                goto commit_rollback;
            }
            B_confirmsave.Text = "Save";
            B_confirmcancel.Text = "Cancel";
        } //Return to Interview

        //CANCEL BY INTERVIEW
        if (L_confirm.Text == "Do you want to Cancel by Interview?")
        {
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
            //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = SQL_Update_CSMS13.ToString();
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS13 Cancel by Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS13 Cancel by Interview";
                goto commit_rollback;
            }

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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            string p1aprj = "";
            if (hid_pending.Value == "PD")
            {
                p1aprj = "PD";

            }
            else
            {
                p1aprj = "MI";

            }
            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                          "p1aprj = '" + p1aprj + "', " +
                          "p1crcd = '" + userInfo.Username.ToString() + "', " +
                          "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3CN',SUBSTRING(p1fill,28,11)), " +
                          "p1updt = " + hid_date97.Value + ", " +
                          "p1uptm = " + m_UpdTime.ToString() + ", " +
                          "p1upus = '" + userInfo.Username.ToString() + "', " +
                          "p1prog = 'INTERVIEW', " +
                          "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                          "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Cancel by Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Cancel by Interview";
                goto commit_rollback;
            }
        } //Cancel by Interview

        //CANCEL BY KESSAI
        if (L_confirm.Text == "Do you want to Cancel by KESSAI?")
        {
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
            //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = SQL_Update_CSMS13.ToString();
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS13 Cancel by KESSAI";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS13 Cancel by KESSAI";
                goto commit_rollback;
            }

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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                              "p1aprj = 'CN', " +
                              "p1loca = '150', " +
                              "p1stdt = " + hid_date97.Value + ", " +
                              "p1sttm = " + m_UpdTime.ToString() + ", " +
                              "p1auth = '" + userInfo.Username.ToString() + "', " +
                              "p1avdt = " + hid_date97.Value + ", " +
                              "p1avtm = " + m_UpdTime.ToString() + ", " +
                              //"p1resn = 'FROM NOTE', " +
                              "p1resn = '" + D_reason.Value.ToString() + "', " +
                              "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3',SUBSTRING(p1fill,26,13)), " +
                              "p1updt = " + hid_date97.Value + ", " +
                              "p1uptm = " + m_UpdTime.ToString() + ", " +
                              "p1upus = '" + userInfo.Username.ToString() + "', " +
                              "p1prog = 'KESSAI', " +
                              "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                              "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Cancel to Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Cancel to Interview";
                goto commit_rollback;
            }
        } //Cancel by KESSAI

        //REJECT BY INTERVIEW
        if (L_confirm.Text == "Do you want to Reject by Interview?")
        {
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
            //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = SQL_Update_CSMS13.ToString();
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS13 Reject by Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS13 Reject by Interview";
                goto commit_rollback;
            }

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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            string p1aprj = "";
            if (hid_pending.Value == "PD")
            {
                p1aprj = "PD";
            }
            else
            {
                p1aprj = "MI";
            }
            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                                  "p1aprj = '" + p1aprj + "', " +
                                  //"p1auth = '" + userInfo.Username.ToString() + "', " +
                                  //"p1avdt = " + hid_date97.Value + ", " +
                                  //"p1avtm = " + m_UpdTime.ToString() + ", " +
                                  "p1crcd = '" + userInfo.Username.ToString() + "', " +
                                  "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3RJ',SUBSTRING(p1fill,28,11)), " +
                                  "p1updt = " + hid_date97.Value + ", " +
                                  "p1uptm = " + m_UpdTime.ToString() + ", " +
                                  "p1upus = '" + userInfo.Username.ToString() + "', " +
                                  "p1prog = 'INTERVIEW', " +
                                  "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                                  "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Reject by Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Reject by Interview";
                goto commit_rollback;
            }
        } //Reject by Interview

        //REJECT BY KESSAI
        if (L_confirm.Text == "Do you want to Reject by KESSAI?")
        {
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
            //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = SQL_Update_CSMS13.ToString();
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS13 Reject by KESSAI";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS13 Reject by KESSAI";
                goto commit_rollback;
            }

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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                              "p1aprj = 'RJ', " +
                              "p1loca = '210', " +
                              "p1stdt = " + hid_date97.Value + ", " +
                              "p1sttm = " + m_UpdTime.ToString() + ", " +
                              "p1auth = '" + userInfo.Username.ToString() + "', " +
                              "p1avdt = " + hid_date97.Value + ", " +
                              "p1avtm = " + m_UpdTime.ToString() + ", " +
                              //"p1resn = 'FROM NOTE', " +
                              "p1resn = '" + D_reason.Value.ToString() + "', " +
                              "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3',SUBSTRING(p1fill,26,13)), " +
                              "p1updt = " + hid_date97.Value + ", " +
                              "p1uptm = " + m_UpdTime.ToString() + ", " +
                              "p1upus = '" + userInfo.Username.ToString() + "', " +
                              "p1prog = 'KESSAI', " +
                              "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                              "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Reject by KESSAI";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Reject by KESSAI";
                goto commit_rollback;
            }
        } //Reject by KESSAI

        //APPROVE BY INTERVIEW
        if (L_confirm.Text == "Do you want to Approve by Interview?")
        {
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
            //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = SQL_Update_CSMS13.ToString();
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS13 Approve by Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS13 Approve by Interview";
                goto commit_rollback;
            }

            //Update CSMS00 Engname, Nickname, Shipto
            //cmd.Parameters.Clear();
            //cmd.CommandText = "insert into csms00hs (select * from csms00 where m00csn = " + hid_CSN.Value + " and m00sts = '') ";
            //affectedRows = -1;
            //try
            //{
            //    affectedRows = busobj.ExecuteNonQuery(cmd);
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //    Error = "Error on Update CSMS00HS Interview Approve ";
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    Error = "Error on Update CSMS00HS Interview Approve";
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            cmd.CommandText = "Update CustomerDB01.CustomerInfo.CustomerGeneral set " +
                              "NameInENG = '" + E_nameeng1.Text.ToUpper().Trim() + "', " +
                              "SurnameInENG = '" + E_surnameeng1.Text.ToUpper().Trim() + "', " +
                              "NickName = '" + E_nickname.Text.ToUpper().Trim() + "', " +
                              //"m00dsn = '" + D_shipto1.Value.ToString() + "', " +
                              $@"UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}', " +
                              "UpdateBy = '" + userInfo.Username + "', " +
                              "Application = 'INTERVIEW' " +
                              "where CISNumber = " + hid_CSN.Value + " ";
            //cmd.Parameters.Add("@m00nnm", E_nickname.Text.ToUpper().Trim());
            affectedRows = -1;

            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText,CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update CSMS00 Interview Approve";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update CSMS00 Interview Approve";
                goto commit_rollback;
            }

            //Update Address Ship to
            string building = "";
            if ((D_building1.Value == null) || (D_building1.Value == ""))
            {
                building = "";
            }
            else
            {
                building = D_building1.Value.ToString();
            }
            if (find_csms11_shipto.ToString() == "Y")
            {
                //cmd.Parameters.Clear();
                //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                //                  "where m11csn = " + hid_CSN.Value + "  and m11cde = '" + D_shipto1.Value.ToString() + "' and m11ref = '' and m11rsq = 0) ";
                //affectedRows = -1;
                //try
                //{
                //    affectedRows = busobj.ExecuteNonQuery(cmd);
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    Error = "Error on Update CSMS11HS Address Shipto Interview Approve ";
                //    goto commit_rollback;
                //}
                //if (affectedRows < 0)
                //{
                //    success = false;
                //    Error = "Error on Update CSMS11HS Address Shipto Interview Approve";
                //    goto commit_rollback;
                //}

                cmd.Parameters.Clear();
                cmd.CommandText = $@"Update CustomerDB01.CustomerInfo.CustomerAddress set 
                                  Village = '{E_village1.Text.Trim()}', 
                                  BuildingTitleID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'BuildingTitleID' AND Code = {building.ToString()}), 
                                  BuildingName = '{E_buildingname1.Text.Trim()}', 
                                  Floor = '{E_floor1.Text.Trim()}', 
                                  AddressNumber = '{E_addressno1.Text.Trim()}', 
                                  Moo = '{E_moo1.Text.Trim()}', 
                                  Room = '{E_roomno1.Text.Trim()}', 
                                  Soi = '{E_soi1.Text.Trim()}', 
                                  Road = '{E_road1.Text.Trim()}', 
                                  IsShipTo = 'Y',
                                  TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {D_tambol1.Value.ToString()}),
                                  AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {D_amphur1.Value.ToString()}),
                                  ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {D_province1.Value.ToString()}), 
                                  PostalAreaCode = {D_zipcode1.Value.ToString()},
                                  UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                  UpdateBy = '{userInfo.Username}', 
                                  Application = 'INTERVIEW'
                                  where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}) and CustRefID = '' and 
                                  AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{D_shipto1.Value.ToString()}') ";

                //cmd.Parameters.Add("@m11vil", E_village1.Text.Trim());
                //cmd.Parameters.Add("@m11bil", building.ToString());
                //cmd.Parameters.Add("@m11bln", E_buildingname1.Text.Trim());
                //cmd.Parameters.Add("@m11flo", E_floor1.Text.Trim());
                //cmd.Parameters.Add("@m11adr", E_addressno1.Text.Trim());
                //cmd.Parameters.Add("@m11moo", E_moo1.Text.Trim());
                //cmd.Parameters.Add("@m11rom", E_roomno1.Text.Trim());
                //cmd.Parameters.Add("@m11soi", E_soi1.Text.Trim());
                //cmd.Parameters.Add("@m11rod", E_road1.Text.Trim());

                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update CSMS11 Address Shipto Interview Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update CSMS11 Address Shipto Interview Approve";
                    goto commit_rollback;
                }
            }
            else
            {
                cmd.Parameters.Clear();
                cmd.CommandText = $@"insert into CustomerDB01.CustomerInfo.CustomerAddress (CustID, CustRefID, AddressCodeID, Village, BuildingTitleID, BuildingName, Floor, AddressNumber, Moo, Room,
                                  Soi, Road, TambolID, AmphurID, ProvinceID, PostalAreaCode, UpdateDate, UpdateBy, Application)
                                  values ((SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value.ToString()}), 
                                0, (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = {D_shipto1.Value.ToString()}),
                                '{E_village1.Text.Trim()}', (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'BuildingTitleID' AND Code = {building.ToString()}), 
                                '{E_buildingname1.Text.Trim()}', '{E_floor1.Text.Trim()}', '{E_addressno1.Text.Trim()}', '{E_moo1.Text.Trim()}', '{E_roomno1.Text.Trim()}', 
                                '{E_soi1.Text.Trim()}', '{E_road1.Text.Trim()}', 
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {D_tambol1.Value.ToString()}),
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {D_amphur1.Value.ToString()}),
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {D_province1.Value.ToString()}),
                                {D_zipcode1.Value.ToString()},
                                '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                '{userInfo.Username}', 'INTERVIEW')";

                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Insert CSMS11 Address Shipto Interview Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Insert CSMS11 Address Shipto Interview Approve";
                    goto commit_rollback;
                }
            }

            //Update Tambol/Amphur/Province/Zipcode not ship to        
            string notshipto_tambol = "", notshipto_amphur = "", notshipto_province = "", notshipto_zipcode = "";
            if (notshipto.ToString() == "O")
            {
                notshipto_tambol = O_tambol.ToString();
                notshipto_amphur = O_amphur.ToString();
                notshipto_province = O_province.ToString();
                notshipto_zipcode = O_zipcode.ToString();
            }
            if (notshipto.ToString() == "H")
            {
                notshipto_tambol = H_tambol.ToString();
                notshipto_amphur = H_amphur.ToString();
                notshipto_province = H_province.ToString();
                notshipto_zipcode = H_zipcode.ToString();
            }
            if (find_csms11_notshipto.ToString() == "Y")
            {
                //cmd.Parameters.Clear();
                //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                //                  "where m11csn = " + hid_CSN.Value + "  and m11cde = '" + notshipto.ToString() + "' and m11ref = '' and m11rsq = 0) ";
                //affectedRows = -1;
                //try
                //{
                //    affectedRows = busobj.ExecuteNonQuery(cmd);
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    Error = "Error on Update CSMS11HS Address Not Shipto Interview Approve ";
                //    goto commit_rollback;
                //}
                //if (affectedRows < 0)
                //{
                //    success = false;
                //    Error = "Error on Update CSMS11HS Address Not Shipto Interview Approve";
                //    goto commit_rollback;
                //}

                cmd.Parameters.Clear();
                cmd.CommandText = $@"Update  CustomerDB01.CustomerInfo.CustomerAddress set 
                                  TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {notshipto_tambol.ToString()}),
                                  AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {notshipto_amphur.ToString()}),
                                  ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {notshipto_province.ToString()}), 
                                  PostalAreaCode = {notshipto_zipcode.ToString()},
                                  UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                  UpdateBy = '{userInfo.Username}', 
                                  Application = 'INTERVIEW'
                                  where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}) and CustRefID = '' and 
                                  AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{notshipto.ToString()}') ";
                                  //"m11tam = " + notshipto_tambol.ToString() + ", " +
                                  //"m11amp = " + notshipto_amphur.ToString() + ", " +
                                  //"m11prv = " + notshipto_province.ToString() + ", " +
                                  //"m11zip = " + notshipto_zipcode.ToString() + ", " +
                                  //"m11udt = " + hid_date97.Value + ", " +
                                  //"m11utm = " + m_UpdTime.ToString() + ", " +
                                  //"m11uus = '" + userInfo.Username + "', " +
                                  //"m11upg = 'INTERVIEW', " +
                                  //"m11uws = '" + userInfo.LocalClient + "' " +
                                  //"where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = '" + notshipto.ToString() + "' ";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update CSMS11 Address Not Shipto Interview Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update CSMS11 Address Not Shipto Interview Approve";
                    goto commit_rollback;
                }
            }
            else
            {
                cmd.Parameters.Clear();
                //cmd.CommandText = "insert into csms11 (m11csn, m11ref, m11rsq, m11cde, m11tam, m11amp, m11prv, m11zip, m11udt, m11utm, m11uus, m11upg, m11uws) " +
                //                  "values (" + hid_CSN.Value.ToString() + ", '', 0, '" + notshipto.ToString() + "', " +
                //                  "" + notshipto_tambol.ToString() + ", " + notshipto_amphur.ToString() + ", " +
                //                  "" + notshipto_province.ToString() + ", " + notshipto_zipcode.ToString() + ", " + hid_date97.Value + "," + m_UpdTime.ToString() + ", " +
                //                  "'" + userInfo.Username + "', 'INTERVIEW', '" + userInfo.LocalClient + "') ";
                cmd.CommandText = $@"insert into CustomerDB01.CustomerInfo.CustomerAddress (CustID, CustRefID, AddressCodeID, BuildingTitleID,TambolID, AmphurID, ProvinceID, PostalAreaCode, UpdateDate, UpdateBy, Application)
                                    Values( (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}),
                                    0, (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = {notshipto.ToString()}),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'OtherID'), 
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {notshipto_tambol.ToString()}),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {notshipto_amphur.ToString()}),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {notshipto_province.ToString()}), 
                                    {notshipto_zipcode.ToString()},
                                    '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                    '{ userInfo.Username}', 'INTERVIEW')";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Insert CSMS11 Address Not Shipto Interview Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Insert CSMS11 Address Not Shipto Interview Approve";
                    goto commit_rollback;
                }
            }


            //Update ILMS01
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
            //    goto commit_rollback;
            //}
            //if (affectedRows < 0)
            //{
            //    success = false;
            //    goto commit_rollback;
            //}

            cmd.Parameters.Clear();
            string p1aprj = "";
            if (hid_pending.Value == "PD")
            {
                p1aprj = "PD";
            }
            else
            {
                p1aprj = "MI";
            }

            cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                              "p1aprj = '" + p1aprj + "', " +
                              //"p1auth = '" + userInfo.Username.ToString() + "', " +
                              //"p1avdt = " + hid_date97.Value + ", " +
                              //"p1avtm = " + m_UpdTime.ToString() + ", " +
                              "p1crcd = '" + userInfo.Username.ToString() + "', " +
                              "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3AP',SUBSTRING(p1fill,28,11)), " +
                              "p1updt = " + hid_date97.Value + ", " +
                              "p1uptm = " + m_UpdTime.ToString() + ", " +
                              "p1upus = '" + userInfo.Username.ToString() + "', " +
                              "p1prog = 'INTERVIEW', " +
                              "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                              "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
            affectedRows = -1;
            try
            {
                bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            }
            catch (Exception ex)
            {
                success = false;
                Error = "Error on Update ILMS01 Approve to Interview";
                goto commit_rollback;
            }
            if (affectedRows < 0)
            {
                success = false;
                Error = "Error on Update ILMS01 Approve to Interview";
                goto commit_rollback;
            }
        } //Approve by Interview

        //APPROVE BY KESSAI    
        G_contract_txt.Text = "";
        L_message.Text = "";
        L_message1.Text = "";
        if (L_confirm.Text == "Do you want to Approve Contract?")
        {
            try
            {
                Check_AMLO();
                if (G_AMLO.Text == "Y")
                {
                    //L_message.Text = "Can not Approve AMLO "; ให้ค่าใน Function แล้ว
                    P_message_TCL.ShowOnPageLoad = true;
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;
                }

                Check_RF_CM();
                if (G_RFCM.Text == "Y")
                {
                    //L_message.Text = "Can not Approve AMLO "; ให้ค่าใน Function แล้ว
                    P_message_TCL.ShowOnPageLoad = true;
                    CallHisunMaster._dataCenter.CloseConnectSQL();
                    return;
                }
                
                //Find Verify Call TH
                string not_have_TH = "", ver_tel_home = "", ver_tel_to_home = "", ver_tel_ex_home = "",
                       not_have_TM = "", ver_mobile = "",
                       ver_to = "", ver_th = "", ver_tm = "",
                       ver_tel_off1 = "", ver_tel_to_off1 = "", ver_tel_ex_off1 = "",
                       ver_tel_off2 = "", ver_tel_to_off2 = "", ver_tel_ex_off2 = "",
                       ver_tel_off_mobile = "";

                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select w5vrth as not_have_TH, w5fvto as ver_to, w5fvth as ver_th, w5fvtm as ver_tm, " +
                                             "SUBSTRING(w5htl,1,9) as ver_tel_home, SUBSTRING(w5htl,11,4) as ver_tel_to_home, w5htex as ver_tel_ex_home, " +
                                             "w5vrtm as not_have_TM, w5mbtl as ver_mobile, " +
                                             "SUBSTRING(w5hotl,1,9) as ver_tel_off1, SUBSTRING(w5hotl,11,4) as ver_tel_to_off1, w5hoex as ver_tel_ex_off1, " +
                                             "SUBSTRING(w5wotl,1,9) as ver_tel_off2, SUBSTRING(w5wotl,11,4) as ver_tel_to_off2, w5woex as ver_tel_ex_off2, " +
                                             "w5motl as ver_tel_off_mobile " +
                                             "from AS400DB01.ILOD0001.ilwk05 WITH (NOLOCK)" +
                                             "where w5csno = " + hid_CSN.Value + " and w5brn = " + hid_brn.Value + " and w5apno = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        ver_to = dr["ver_to"].ToString().Trim();
                        ver_th = dr["ver_th"].ToString().Trim();
                        ver_tm = dr["ver_tm"].ToString().Trim();
                        not_have_TH = dr["not_have_TH"].ToString().Trim();
                        ver_tel_home = dr["ver_tel_home"].ToString().Trim();
                        ver_tel_to_home = dr["ver_tel_to_home"].ToString().Trim();
                        ver_tel_ex_home = dr["ver_tel_ex_home"].ToString().Trim();
                        not_have_TM = dr["not_have_TM"].ToString().Trim();
                        ver_mobile = dr["ver_mobile"].ToString().Trim();
                        ver_tel_off1 = dr["ver_tel_off1"].ToString().Trim();
                        ver_tel_to_off1 = dr["ver_tel_to_off1"].ToString().Trim();
                        ver_tel_ex_off1 = dr["ver_tel_ex_off1"].ToString().Trim();
                        ver_tel_off2 = dr["ver_tel_off2"].ToString().Trim();
                        ver_tel_to_off2 = dr["ver_tel_to_off2"].ToString().Trim();
                        ver_tel_ex_off2 = dr["ver_tel_ex_off2"].ToString().Trim();
                        ver_tel_off_mobile = dr["ver_tel_off_mobile"].ToString().Trim();
                    }
                    DS.Clear();
                }

                //Find Tel Home
                string tel_home = "", tel_to_home = "", tel_ext_home = "", tel_mobile = "";
                //DS = busobj.RetriveAsDataSet("select substr(m11tel,1,9) as tel_home, substr(m11tel,11,4) as tel_to_home, m11ext as tel_ext_home, " +
                //                             "m11mob as tel_mobile from csms11 " +
                //                             "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'H' ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select SUBSTRING(TelephoneNumber1,1,9) as tel_home, SUBSTRING(TelephoneNumber1,11,4) as tel_to_home, ExtensionNumber1 as tel_ext_home, " +
                                             "Mobile as tel_mobile from CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK) " +
                                             "JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID)" +
                                             "JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID' )" +
                                             "where cg.CISNumber = " + hid_CSN.Value + " and CustRefID = 0 and gc.Code = 'H' ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        tel_home = dr["tel_home"].ToString().Trim();
                        tel_to_home = dr["tel_to_home"].ToString().Trim();
                        tel_ext_home = dr["tel_ext_home"].ToString().Trim();
                        tel_mobile = dr["tel_mobile"].ToString().Trim();
                    }
                    DS.Clear();
                }

                //Find Tel Office
                string tel_off = "", tel_to_off = "", tel_ext_off = "", tel_off2 = "", tel_to_off2 = "", tel_ext_off2 = "", tel_mobile_off = "";
                //DS = busobj.RetriveAsDataSet("select substr(m11tel,1,9) as tel_off, substr(m11tel,11,4) as tel_to_off, m11ext as tel_ext_off, " +
                //                             "substr(m11tl2,1,9) as tel_off2, substr(m11tl2,11,4) as tel_to_off2, m11ex2 as tel_ext_off2,m11mob as tel_mobile_off  " +
                //                             "from csms11 " +
                //                             "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'O' ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select SUBSTRING(TelephoneNumber1,1,9) as tel_off, SUBSTRING(TelephoneNumber1,11,4) as tel_to_off, ExtensionNumber1 as tel_ext_off, " +
                                             "SUBSTRING(TelephoneNumber2,1,9) as tel_off2, SUBSTRING(TelephoneNumber2,11,4) as tel_to_off2, ExtensionNumber2 as tel_ext_off2, " +
                                             "Mobile as tel_mobile_off from CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK) " +
                                             "JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID)" +
                                             "JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID' )" +
                                             "where cg.CISNumber = " + hid_CSN.Value + " and CustRefID = 0 and gc.Code = 'O' ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        tel_off = dr["tel_off"].ToString().Trim();
                        tel_to_off = dr["tel_to_off"].ToString().Trim();
                        tel_ext_off = dr["tel_ext_off"].ToString().Trim();
                        tel_off2 = dr["tel_off2"].ToString().Trim();
                        tel_to_off2 = dr["tel_to_off2"].ToString().Trim();
                        tel_ext_off2 = dr["tel_ext_off2"].ToString().Trim();
                        tel_mobile_off = dr["tel_mobile_off"].ToString().Trim();

                    }
                    DS.Clear();
                }

                string Loan_Amt_ILTB06 = "", Duty_Amt_ILTB06 = "";
                //DS = busobj.RetriveAsDataSet("select t06lon,t06Dut from iltb06 ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select t06lon,t06Dut from AS400DB01.ILOD0001.iltb06 WITH (NOLOCK) ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        Loan_Amt_ILTB06 = dr["t06lon"].ToString().Trim();
                        Duty_Amt_ILTB06 = dr["t06Dut"].ToString().Trim();
                    }
                }
                //Find Data ILMS01            
                string Vendor_01 = "", Maker_01 = "", Campaign_01 = "", CampaignSeq_01 = "", CampaignType_01 = "R", Product_01 = "", Price_01 = "",
                       Qty_01 = "", Purchase_01 = "", Vattr_01 = "", Vatta_01 = "", Down_01 = "", Term_01 = "", Range_01 = "", NonDue_01 = "",
                       LoanAmt_01 = "", Duty_01 = "", Inf_01 = "", Int_01 = "", Cru_01 = "", ContAmt_01 = "", LoanReq_01 = "", FirstDut_01 = "", FirstDueAmt_01 = "",
                       Diff_01 = "", FreeIns_01 = "", TotInt_01 = "", TotCru_01 = "", TotInf_01 = "", FirstDutDate_01 = "", FirstAmt_01 = "", Credit_01 = "";
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select p1csno, p1vdid, p1mkid, p1camp, p1cmsq, p1item, p1pric, p1pric, p1qty, p1lndr, p1vatr, p1vata, p1down, p1term, " +
                                             "p1rang, p1ndue, p1pram, p1duty, p1infr, p1intr, p1crur, p1coam, p1purc, p1fdue, p1fdam, p1diff, p1frtm, " +
                                             "p1inta, p1crua, p1infa, p1frdt, p1fram, p1crcd " +
                                             "from AS400DB01.ILOD0001.ilms01 WITH (NOLOCK) " +
                                             "where p1csno=" + hid_CSN.Value + " and p1brn=" + hid_brn.Value + " and p1apno=" + hid_AppNo.Value + " ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        if (dr["p1csno"].ToString().Trim() != "")
                        {
                            Loan_Amt.Text = dr["p1pram"].ToString().Trim();
                            Vendor_01 = dr["p1vdid"].ToString().Trim();
                            Maker_01 = dr["p1mkid"].ToString().Trim();
                            Campaign_01 = dr["p1camp"].ToString().Trim();
                            CampaignSeq_01 = dr["p1cmsq"].ToString().Trim();
                            Product_01 = dr["p1item"].ToString().Trim();
                            Price_01 = dr["p1pric"].ToString().Trim();
                            Qty_01 = dr["p1qty"].ToString().Trim();
                            Purchase_01 = dr["p1purc"].ToString().Trim();
                            Vattr_01 = dr["p1vatr"].ToString().Trim();
                            Vatta_01 = dr["p1vata"].ToString().Trim();
                            Down_01 = dr["p1down"].ToString().Trim();
                            Term_01 = dr["p1term"].ToString().Trim();
                            Range_01 = dr["p1rang"].ToString().Trim();
                            NonDue_01 = dr["p1ndue"].ToString().Trim();
                            LoanAmt_01 = dr["p1lndr"].ToString().Trim();
                            Duty_01 = dr["p1duty"].ToString().Trim();
                            Inf_01 = dr["p1infr"].ToString().Trim();
                            Int_01 = dr["p1intr"].ToString().Trim();
                            Cru_01 = dr["p1crur"].ToString().Trim();
                            ContAmt_01 = dr["p1coam"].ToString().Trim();
                            LoanReq_01 = dr["p1pram"].ToString().Trim();
                            FirstDut_01 = dr["p1fdue"].ToString().Trim();
                            FirstDueAmt_01 = dr["p1fdam"].ToString().Trim();
                            Diff_01 = dr["p1diff"].ToString().Trim();
                            FreeIns_01 = dr["p1frtm"].ToString().Trim();
                            TotInt_01 = dr["p1inta"].ToString().Trim();
                            TotCru_01 = dr["p1crua"].ToString().Trim();
                            TotInf_01 = dr["p1infa"].ToString().Trim();
                            FirstDutDate_01 = dr["p1frdt"].ToString().Trim();
                            FirstAmt_01 = dr["p1fram"].ToString().Trim();
                            Credit_01 = dr["p1crcd"].ToString().Trim();
                        }
                    }
                    DS.Clear();
                }

                //Find ILMS02 for make sure check existing
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select p2csno from AS400DB01.ILOD0001.ilms02 WITH (NOLOCK) " +
                                             "where p2csno=" + hid_CSN.Value + " and p2brn=" + hid_brn.Value + " and p2apno=" + hid_AppNo.Value + " ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        if (dr["p2csno"].ToString().Trim() != "")
                        {
                            L_message.Text = "Error : Found Application No " + hid_AppNo.Value + " on ILMS02 ";
                            P_message_TCL.ShowOnPageLoad = true;
                            CallHisunMaster._dataCenter.CloseConnectSQL();
                            return;
                        }
                    }
                    DS.Clear();
                }

                //Check Auto Sign Laybill
                string[] vendor = dd_vendor.Value.ToString().Split('|');
                //DS = busobj.RetriveAsDataSet("Select p10ats from ilms10 where p10ven = " + vendor[0] + " and p10del = '' ");
                DSVendor = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select p10ats from AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) where p10ven = " + vendor[0] + " and p10del = '' ", CommandType.Text).Result.data;

                //Find OCCUPATION CSMS13
                string occupation_csms13 = "";
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m13occ from AS400DB01.CSOD0001.csms13 WITH (NOLOCK) " +
                                             "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and " +
                                             "m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        occupation_csms13 = dr["m13occ"].ToString().Trim();
                    }
                    DS.Clear();
                }
                //Check Occupation 011, 012
                string ErrCode = "", ErrMsg = "";
                if ((occupation_csms13.ToString() == "011") || (occupation_csms13.ToString() == "012"))
                {
                    bool call_commercial = iLDataSubroutine.Call_CSSR034("C", "F", occupation_csms13.ToString().Trim(),
                                                               "Y", hid_CSN.Value, "", "0", "0", "0", "0",
                                                               ref ErrCode, ref ErrMsg, userInfo.BizInit, userInfo.BranchNo);
                    if (ErrCode != "000")
                    {
                       // success = false;
                        L_message.Text = "Error Check CSSR034 ";
                        return;
                       // goto commit_rollback;
                    }
                }
                //Get Contract No.
                string G_contract = "", ErrorMsg = "";
                bool call_ILSR02 = iLDataSubroutine.Call_ILSR02(userInfo.BranchApp.ToString().PadLeft(3, '0'), "01", ref G_contract, ref Error, ref ErrorMsg,
                                                      userInfo.BizInit, userInfo.BranchNo);
                if (Error.ToString().Trim() == "Y")
                {
                    Error = ErrorMsg.ToString();
                    return;
                   // success = false;
                   // goto commit_rollback;
                }
                G_contract_txt.Text = G_contract.ToString();

                string find_ilms86 = "";
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select p86con from  AS400DB01.ILOD0001.ilms86 WITH (NOLOCK) " +
                                             "p86brn = " + hid_brn.Value + " and p86con = " + G_contract.ToString().Trim() + " ", CommandType.Text).Result.data;
                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        if (dr["p86con"].ToString().Trim() != "")
                        {
                            find_ilms86 = "Y";
                        }
                    }
                    DS.Clear();
                }

                //Update Reference CSMS01, CSMS11, CSMS12 by ILWK12
                string Error_ILSR16 = "";
                ILSR16 iLSR16 = new ILSR16();
                bool callILSR16 = iLSR16.CALL_ILSR16(CallHisunMaster._dataCenter, "", hid_CSN.Value, hid_brn.Value, hid_AppNo.Value, ref Error_ILSR16, userInfo.BizInit.ToString(), userInfo.BranchNo.ToString());
                if (Error_ILSR16.ToString() == "Y")
                {
                    success = false;
                    L_message.Text = "Error on ILSR16 (Insert Reference Data) ";
                    E_redirect.Text = "N";
                    P_message_TCL.ShowOnPageLoad = true;
                    goto commit_rollback;
                }

                //Call CSSR06C
                string WOERR = "";
                string WOERRM = "";
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
                string AppAvi_LoanReq = (Convert.ToDecimal(E_Apv_avi.Text) - Convert.ToDecimal(Loan_Amt.Text)).ToString();
                bool res_call06 = iLDataSubroutine.Call_CSSR06C(busobj, "IL", hid_idno.Value, userInfo.BranchApp.ToString(), hid_AppNo.Value, G_contract.ToString(),
                                  (float.Parse(AppAvi_LoanReq, NumberStyles.Currency)).ToString(), "", hid_appdate.Value, hid_birthdate.Value, "1", "Y",
                                  (float.Parse(EdtNetIncome.Text, NumberStyles.Currency)).ToString(),
                                  (float.Parse(EdtTCL.Text, NumberStyles.Currency)).ToString(),
                                  (float.Parse(Loan_Amt.Text, NumberStyles.Currency)).ToString(),
                                  Vendor_01.ToString(), mode, ref WOERR, ref WOERRM, userInfo.BizInit, userInfo.BranchNo);
                if (!res_call06 || WOERR == "Y")
                {
                    Error = WOERRM.ToString();
                    L_message.Text = "Error Check CSSR06C : " + WOERRM;
                    E_redirect.Text = "N";
                    P_message_TCL.ShowOnPageLoad = true;
                    return;

                }
                //cmd.Parameters.Clear();
                //cmd.CommandText = "insert into csms13hs (select * from csms13 " +
                //                  "where m13app = 'IL' and m13brn = " + hid_brn.Value.ToString() + " and m13apn = " + hid_AppNo.Value.ToString() + ") ";
                //affectedRows = -1;
                //try
                //{
                //    affectedRows = busobj.ExecuteNonQuery(cmd);
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    goto commit_rollback;
                //}
                //if (affectedRows < 0)
                //{
                //    success = false;
                //    goto commit_rollback;
                //}

                cmd.Parameters.Clear();
                cmd.CommandText = SQL_Update_CSMS13.ToString();
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update CSMS13 Approve by KESSAI";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update CSMS13 Approve by KESSAI";
                    goto commit_rollback;
                }

                

                cmd.Parameters.Clear();
               
                cmd.CommandText = "Update CustomerDB01.CustomerInfo.CustomerGeneral set " +
                                  "NameInENG = '" + E_nameeng1.Text.ToUpper().Trim() + "', " +
                                  "SurnameInENG = '" + E_surnameeng1.Text.ToUpper().Trim() + "', " +
                                  "NickName = '" + E_nickname.Text.ToUpper().Trim() + "', " +
                                  //"m00dsn = '" + D_shipto1.Value.ToString() + "', " +
                                  $@"UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}', " +
                                  "UpdateBy = '" + userInfo.Username + "', " +
                                  "Application = 'KESSAI' " +
                                  "where CISNumber = " + hid_CSN.Value + " ";
                //cmd.Parameters.Add("@m00nnm", E_nickname.Text.ToUpper().Trim());
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update CSMS00 KESSAI Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update CSMS00 KESSAI Approve";
                    goto commit_rollback;
                }

                //Update Address Ship to
                string building = "";
                if ((D_building1.Value == null) || (D_building1.Value == ""))
                {
                    building = "";
                }
                else
                {
                    building = D_building1.Value.ToString();
                }
                if (find_csms11_shipto.ToString() == "Y")
                {
                    //cmd.Parameters.Clear();
                    //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                    //                  "where m11csn = " + hid_CSN.Value + "  and m11cde = '" + D_shipto1.Value.ToString() + "' and m11ref = '' and m11rsq = 0) ";
                    //affectedRows = -1;
                    //try
                    //{
                    //    affectedRows = busobj.ExecuteNonQuery(cmd);
                    //}
                    //catch (Exception ex)
                    //{
                    //    success = false;
                    //    Error = "Error on Update CSMS11HS Address Shipto KESSAI Approve ";
                    //    goto commit_rollback;
                    //}
                    //if (affectedRows < 0)
                    //{
                    //    success = false;
                    //    Error = "Error on Update CSMS11HS Address Shipto KESSAI Approve";
                    //    goto commit_rollback;
                    //}

                    cmd.Parameters.Clear();
                   
                    cmd.CommandText = $@"Update CustomerDB01.CustomerInfo.CustomerAddress set 
                                  Village = '{E_village1.Text.Trim()}', 
                                  BuildingTitleID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'BuildingTitleID' AND Code = {building.ToString()}), 
                                  BuildingName = '{E_buildingname1.Text.Trim()}', 
                                  Floor = '{E_floor1.Text.Trim()}', 
                                  AddressNumber = '{E_addressno1.Text.Trim()}', 
                                  Moo = '{E_moo1.Text.Trim()}', 
                                  Room = '{E_roomno1.Text.Trim()}', 
                                  Soi = '{E_soi1.Text.Trim()}', 
                                  Road = '{E_road1.Text.Trim()}', 
                                  IsShipTo = 'Y',
                                  TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {D_tambol1.Value.ToString()}),
                                  AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {D_amphur1.Value.ToString()}),
                                  ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {D_province1.Value.ToString()}), 
                                  PostalAreaCode = {D_zipcode1.Value.ToString()},
                                  UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                  UpdateBy = '{userInfo.Username}', 
                                  Application = 'KESSAI'
                                  where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}) and CustRefID = '' and 
                                  AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{D_shipto1.Value.ToString()}') ";

                    affectedRows = -1;
                    try
                    {
                        bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                        affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Error = "Error on Update CSMS11 Address Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                    if (affectedRows < 0)
                    {
                        success = false;
                        Error = "Error on Update CSMS11 Address Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $@"insert into CustomerDB01.CustomerInfo.CustomerAddress (CustID, CustRefID, AddressCodeID, Village, BuildingTitleID, BuildingName, Floor, AddressNumber, Moo, Room,
                                  Soi, Road, TambolID, AmphurID, ProvinceID, PostalAreaCode, UpdateDate, UpdateBy, Application)
                                  values ((SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value.ToString()}), 
                                0, (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{D_shipto1.Value.ToString()}'),
                                '{E_village1.Text.Trim()}', (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'BuildingTitleID' AND Code = {building.ToString()}), 
                                '{E_buildingname1.Text.Trim()}', '{E_floor1.Text.Trim()}', '{E_addressno1.Text.Trim()}', '{E_moo1.Text.Trim()}', '{E_roomno1.Text.Trim()}', 
                                '{E_soi1.Text.Trim()}', '{E_road1.Text.Trim()}', 
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {D_tambol1.Value.ToString()}),
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {D_amphur1.Value.ToString()}),
                                (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {D_province1.Value.ToString()}),
                                {D_zipcode1.Value.ToString()},
                                '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                '{userInfo.Username}', 'KESSAI')";
                    affectedRows = -1;
                    try
                    {
                        bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                        affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Error = "Error on Insert CSMS11 Address Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                    if (affectedRows < 0)
                    {
                        success = false;
                        Error = "Error on Insert CSMS11 Address Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                }

                //Update Tambol/Amphur/Province/Zipcode not ship to        
                string notshipto_tambol = "", notshipto_amphur = "", notshipto_province = "", notshipto_zipcode = "";
                if (notshipto.ToString() == "O")
                {
                    notshipto_tambol = O_tambol.ToString();
                    notshipto_amphur = O_amphur.ToString();
                    notshipto_province = O_province.ToString();
                    notshipto_zipcode = O_zipcode.ToString();
                }
                if (notshipto.ToString() == "H")
                {
                    notshipto_tambol = H_tambol.ToString();
                    notshipto_amphur = H_amphur.ToString();
                    notshipto_province = H_province.ToString();
                    notshipto_zipcode = H_zipcode.ToString();
                }
                if (find_csms11_notshipto.ToString() == "Y")
                {
                    //cmd.Parameters.Clear();
                    //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                    //                  "where m11csn = " + hid_CSN.Value + "  and m11cde = '" + notshipto.ToString() + "' and m11ref = '' and m11rsq = 0) ";
                    //affectedRows = -1;
                    //try
                    //{
                    //    affectedRows = busobj.ExecuteNonQuery(cmd);
                    //}
                    //catch (Exception ex)
                    //{
                    //    success = false;
                    //    Error = "Error on Update CSMS11HS Address Not Shipto KESSAI Approve ";
                    //    goto commit_rollback;
                    //}
                    //if (affectedRows < 0)
                    //{
                    //    success = false;
                    //    Error = "Error on Update CSMS11HS Address Not Shipto KESSAI Approve";
                    //    goto commit_rollback;
                    //}

                    cmd.Parameters.Clear();
                    cmd.CommandText = $@"Update  CustomerDB01.CustomerInfo.CustomerAddress set 
                                  TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {notshipto_tambol.ToString()}),
                                  AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {notshipto_amphur.ToString()}),
                                  ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {notshipto_province.ToString()}), 
                                  PostalAreaCode = {notshipto_zipcode.ToString()},
                                  UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                  UpdateBy = '{userInfo.Username}', 
                                  Application = 'KESSAI'
                                  where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}) and CustRefID = '' and 
                                  AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{notshipto.ToString()}') ";
                    affectedRows = -1;
                    try
                    {
                        bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                        affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Error = "Error on Update CSMS11 Address Not Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                    if (affectedRows < 0)
                    {
                        success = false;
                        Error = "Error on Update CSMS11 Address Not Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $@"insert into CustomerDB01.CustomerInfo.CustomerAddress (CustID, CustRefID, AddressCodeID, BuildingTitleID,TambolID, AmphurID, ProvinceID, PostalAreaCode, UpdateDate, UpdateBy, Application)
                                    Values( (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = {hid_CSN.Value}),
                                    0, (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{notshipto.ToString()}'),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'OtherID'), 
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = {notshipto_tambol.ToString()}),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = {notshipto_amphur.ToString()}),
                                    (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = {notshipto_province.ToString()}), 
                                    {notshipto_zipcode.ToString()},
                                    '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}',
                                    '{ userInfo.Username}', 'KESSAI')";
                    affectedRows = -1;
                    try
                    {
                        bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                        affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Error = "Error on Insert CSMS11 Address Not Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                    if (affectedRows < 0)
                    {
                        success = false;
                        Error = "Error on Insert CSMS11 Address Not Shipto KESSAI Approve";
                        goto commit_rollback;
                    }
                }

                //Update CSMS11 by Verify Call Tel Home
                if ((not_have_TH != "Y") & (ver_th.ToString().Trim() != ""))
                {
                    if ((tel_home.ToString().Trim() != ver_tel_home.ToString().Trim()) ||
                       (tel_ext_home.ToString().Trim() != ver_tel_ex_home.ToString().Trim()))
                    {
                        string Tel_H1 = ver_tel_home.ToString();
                        if (ver_tel_to_home.ToString().Trim() != "")
                        {
                            Tel_H1 = ver_tel_home.ToString() + '-' + ver_tel_to_home.ToString().Trim();
                        }
                       

                        cmd.Parameters.Clear();
                        cmd.CommandText = "Update CustomerDB01.CustomerInfo.CustomerAddress set " +
                                          "TelephoneNumber1 = '" + Tel_H1 + "', " +
                                          "ExtensionNumber1 = '" + ver_tel_ex_home.ToString().Trim() + "', " +
                                          $"UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'," +
                                          "UpdateBy = '" + userInfo.Username + "', " +
                                          "Application = 'KESSAI' " +
                                          $@"where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH(NOLOCK) WHERE CISNumber = { hid_CSN.Value }) and CustRefID = '' and
                                      AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH(NOLOCK) WHERE Type = 'AddressCodeID' AND Code = 'H') ";
                        //"where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'H' ";
                        //cmd.Parameters.Add("@m11ext", ver_tel_ex_home.ToString().Trim());
                        affectedRows = -1;
                        try
                        {
                            bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                            affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL HOME KESSAI Approve";
                            goto commit_rollback;
                        }
                        if (affectedRows < 0)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL HOME KESSAI Approve";
                            goto commit_rollback;
                        }

                        try
                        {
                            CSSR16 cSSR16 = new CSSR16(userInfo);
                            string Err_tel = "";
                            cSSR16.checkTelType(CallHisunMaster._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "H", "P", Tel_H1.ToString(), ver_tel_ex_home.ToString().Trim(),
                                                 ref Err_tel);
                            //InsertTel(Tel_H1, "H", "P", ver_tel_ex_home.ToString().Trim(), "", "0", 1, busobj);
                        }
                        catch
                        {
                            success = false;
                            L_message.Text = "Error on Update CSMS12 TEL HOME KESSAI Approve CSMS12 ";
                            goto commit_rollback;
                        }
                    }
                }

                //Update CSMS11 by Verify Call Mobile Home
                if ((not_have_TM != "Y") & (ver_tm.ToString().Trim() != ""))
                {
                    if (tel_mobile.ToString().Trim() != ver_mobile.ToString().Trim())
                    {
                        //cmd.Parameters.Clear();
                        //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                        //                                        "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'H') ";
                        //affectedRows = -1;
                        //try
                        //{
                        //    affectedRows = busobj.ExecuteNonQuery(cmd);
                        //}
                        //catch (Exception ex)
                        //{
                        //    success = false;
                        //    Error = "Error on Update CSMS11HS TEL HOME MOBILE KESSAI Approve ";
                        //    goto commit_rollback;
                        //}
                        //if (affectedRows < 0)
                        //{
                        //    success = false;
                        //    Error = "Error on Update CSMS11HS TEL HOME MOBILE KESSAI Approve";
                        //    goto commit_rollback;
                        //}

                        cmd.Parameters.Clear();
                        cmd.CommandText = "Update CustomerDB01.CustomerInfo.CustomerAddress set " +
                                          "Mobile = '" + ver_mobile.ToString().Trim() + "', " +
                                          $"UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'," +
                                          "UpdateBy = '" + userInfo.Username + "', " +
                                          "Application = 'KESSAI' " +
                                          $@"where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH(NOLOCK) WHERE CISNumber = { hid_CSN.Value }) and CustRefID = '' and
                                      AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH(NOLOCK) WHERE Type = 'AddressCodeID' AND Code = 'H') ";
                        affectedRows = -1;
                        try
                        {
                            bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                            affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL HOME MOBILE KESSAI Approve";
                            goto commit_rollback;
                        }
                        if (affectedRows < 0)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL HOME MOBILE KESSAI Approve";
                            goto commit_rollback;
                        }

                        try
                        {
                            CSSR16 cSSR16 = new CSSR16(userInfo);
                            string Err_tel = "";
                            cSSR16.checkTelType(CallHisunMaster._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "H", "M", ver_mobile.ToString().Trim(), "",
                                               ref Err_tel);
                            //InsertTel(ver_mobile.ToString().Trim(), "H", "M", "", "", "0", 1, busobj);
                        }
                        catch
                        {
                            success = false;
                            L_message.Text = "Error on Update CSMS12 TEL HOME MOBILE KESSAI Approve CSMS12 ";
                            goto commit_rollback;
                        }
                    }
                }

                if (ver_to.ToString().Trim() != "")
                {
                    //Update CSMS11 by Verify Call Office            
                    if ((tel_off.ToString().Trim() != ver_tel_off1.ToString().Trim()) ||
                        (tel_to_off.ToString().Trim() != ver_tel_to_off1.ToString().Trim()) ||
                        (tel_ext_off.ToString().Trim() != ver_tel_ex_off1.ToString().Trim()) ||
                        (tel_off2.ToString().Trim() != ver_tel_off2.ToString().Trim()) ||
                        (tel_to_off2.ToString().Trim() != ver_tel_to_off2.ToString().Trim()) ||
                        (tel_ext_off2.ToString().Trim() != ver_tel_ex_off2.ToString().Trim()) ||
                        (tel_mobile_off.ToString().Trim() != ver_tel_off_mobile.ToString().Trim())) // edit mobile verify call 10/02/2558
                    {
                        string Tel_O1 = ver_tel_off1.ToString();
                        if (ver_tel_to_off1.ToString().Trim() != "")
                        {
                            Tel_O1 = ver_tel_off1.ToString() + '-' + ver_tel_to_off1.ToString().Trim();
                        }
                        string Tel_O2 = ver_tel_off2.ToString();
                        if (ver_tel_to_off2.ToString().Trim() != "")
                        {
                            Tel_O2 = ver_tel_off2.ToString() + '-' + ver_tel_to_off2.ToString().Trim();
                        }
                        //cmd.Parameters.Clear();
                        //cmd.CommandText = "insert into csms11hs (select * from csms11 " +
                        //                                        "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'O') ";
                        //affectedRows = -1;
                        //try
                        //{
                        //    affectedRows = busobj.ExecuteNonQuery(cmd);
                        //}
                        //catch (Exception ex)
                        //{
                        //    success = false;
                        //    Error = "Error on Update CSMS11HS TEL OFFICE KESSAI Approve ";
                        //    goto commit_rollback;
                        //}
                        //if (affectedRows < 0)
                        //{
                        //    success = false;
                        //    Error = "Error on Update CSMS11HS TEL OFFICE KESSAI Approve";
                        //    goto commit_rollback;
                        //}

                        cmd.Parameters.Clear();
                        //cmd.CommandText = "Update csms11 set " +
                        //                  "m11tel = '" + Tel_O1.ToString().Trim() + "', " +
                        //                  "m11ext = @m11ext, " +
                        //                  "m11tl2 = '" + Tel_O2.ToString().Trim() + "', " +
                        //                  "m11ex2 = @m11ex2, " +
                        //                  "m11mob = '" + ver_tel_off_mobile.ToString().Trim() + "', " +
                        //                  "m11udt = " + hid_date97.Value + ", " +
                        //                  "m11utm = " + m_UpdTime.ToString() + ", " +
                        //                  "m11uus = '" + userInfo.Username + "', " +
                        //                  "m11upg = 'KESSAI', " +
                        //                  "m11uws = '" + userInfo.LocalClient + "' " +
                        //                  "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = 'O' ";
                        //cmd.Parameters.Add("@m11ext", ver_tel_ex_off1.ToString().Trim());
                        //cmd.Parameters.Add("@m11ex2", ver_tel_ex_off2.ToString().Trim());
                        cmd.CommandText = "Update CustomerDB01.CustomerInfo.CustomerAddress set " +
                                          "TelephoneNumber1 = '" + Tel_O1.ToString().Trim() + "', " +
                                          "ExtensionNumber1 = '" + ver_tel_ex_off1.ToString().Trim() + "', " +
                                          "TelephoneNumber2 = '" + Tel_O2.ToString().Trim() + "', " +
                                          "ExtensionNumber2 = '" + ver_tel_ex_off2.ToString().Trim() + "', " +
                                          "Mobile = '" + ver_tel_off_mobile.ToString().Trim() + "', " +
                                         $"UpdateDate = '{(int.Parse(hid_date97.Value.Trim().Substring(0, 4)) - 543).ToString()}-{hid_date97.Value.Trim().Substring(4, 2)}-{hid_date97.Value.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'," +
                                          "UpdateBy = '" + userInfo.Username + "', " +
                                          "Application = 'KESSAI' " +
                                          $@"where CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH(NOLOCK) WHERE CISNumber = { hid_CSN.Value }) and CustRefID = '' and
                                      AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH(NOLOCK) WHERE Type = 'AddressCodeID' AND Code = 'O') ";
                        affectedRows = -1;
                        try
                        {
                            bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                            affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL OFFICE KESSAI Approve";
                            goto commit_rollback;
                        }
                        if (affectedRows < 0)
                        {
                            success = false;
                            Error = "Error on Update CSMS11 TEL OFFICE KESSAI Approve";
                            goto commit_rollback;
                        }


                        try
                        {
                            CSSR16 cSSR16 = new CSSR16(userInfo);
                            string Err_tel = "";
                            cSSR16.checkTelType(CallHisunMaster._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "O", "P", Tel_O1.ToString().Trim(), ver_tel_ex_off1.ToString().Trim(),
                                               ref Err_tel);
                            //busobj.CALL_CSSR16("C", "M", hid_CSN.Value.Trim(), "0", "O", "P", Tel_O2.ToString().Trim(), ver_tel_ex_off2.ToString().Trim(),
                            //                   ref Err_tel, userInfo.BizInit, userInfo.BranchNo);
                            cSSR16.checkTelType(CallHisunMaster._dataCenter, "C", "M", hid_CSN.Value.Trim(), "0", "O", "M", ver_tel_off_mobile.ToString().Trim(), "",
                                               ref Err_tel);
                            //InsertTel(Tel_O1.ToString().Trim(), "O", "P", ver_tel_ex_off1.ToString().Trim(), "", "0", 1, busobj);
                            //InsertTel(Tel_O2.ToString().Trim(), "O", "P", ver_tel_ex_off2.ToString().Trim(), "", "0", 2, busobj);
                            //InsertTel(ver_tel_off_mobile.ToString().Trim(), "O", "M", "", "", "0", 1, busobj);
                        }
                        catch
                        {
                            success = false;
                            L_message.Text = "Error on Update CSMS11 TEL OFFICE KESSAI Approve CSMS12 ";
                            goto commit_rollback;
                        }
                    }
                }

                

                //UPDATE ILMS01
                //cmd.Parameters.Clear();
                //cmd.CommandText = "insert into ilms01hs (select " + hid_date97.Value.ToString() + ", " + m_UpdTime.ToString() + ", " +
                //              "'" + userInfo.Username.ToString() + "', '" + userInfo.LocalClient.ToString() + "', ilms01.* " +
                //              "from ilms01 where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + ") ";
                //affectedRows = -1;
                //try
                //{
                //    affectedRows = busobj.ExecuteNonQuery(cmd);
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    Error = "Error on Insert ILMS01HS KESSAI Approve ";
                //    goto commit_rollback;
                //}
                //if (affectedRows < 0)
                //{
                //    success = false;
                //    Error = "Error on Insert ILMS01HS KESSAI Approve ";
                //    goto commit_rollback;
                //}

                cmd.Parameters.Clear();
                cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                                  "p1aprj = 'AP', " +
                                  "p1loca = '250', " +
                                  "p1cont = " + G_contract.ToString().Trim() + ", " +
                                  "p1cndt = " + hid_date97.Value + ", " +
                                  "p1stdt = " + hid_date97.Value + ", " +
                                  "p1sttm = " + m_UpdTime.ToString() + ", " +
                                  //"p1crcd = '" + userInfo.Username.ToString() + "', " +
                                  "p1auth = '" + userInfo.Username.ToString() + "', " +
                                  "p1avdt = " + hid_date97.Value + ", " +
                                  "p1avtm = " + m_UpdTime.ToString() + ", " +
                                  "p1resn = 'A', " +
                                  "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3    ',SUBSTRING(p1fill,30,9)), " +
                                  "p1updt = " + hid_date97.Value + ", " +
                                  "p1uptm = " + m_UpdTime.ToString() + ", " +
                                  "p1upus = '" + userInfo.Username.ToString() + "', " +
                                  "p1prog = 'KESSAI', " +
                                  "p1wsid = '" + userInfo.LocalClient.ToString() + "' " +
                                  "where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + " ";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update ILMS01 KESSAI Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update ILMS01 KESSAI Approve";
                    goto commit_rollback;
                }

                string Intbase_24 = "", Crubase_24 = "", Infbase_24 = "";
                for (int i = 0; i < gvTerm.Rows.Count - 1; i++)
                {
                    Label Intbase = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_intAMT");
                    Label Crubase = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_cruAmonth");
                    Label Infbase = (Label)gvTerm.Rows[i].Cells[0].FindControl("lb_InitFree");
                    Intbase_24 = float.Parse(Intbase.Text.Trim(), NumberStyles.Currency).ToString();
                    Crubase_24 = float.Parse(Crubase.Text.Trim(), NumberStyles.Currency).ToString();
                    Infbase_24 = float.Parse(Infbase.Text.Trim(), NumberStyles.Currency).ToString();
                }

                //UPDATE ILMS02
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into AS400DB01.ILOD0001.ilms02 (p2brn,p2cont,p2lnty,p2csno,p2apno,p2appt,p2crcd,p2atcd,p2loca, " +
                                "p2vdid,p2mkid,p2camp,p2cmsq,p2cmct,p2item,p2pric,p2qty,p2purc,p2vatr,p2vata,p2down, " +
                                "p2term,p2rang,p2ndue,p2dte1,p2cndt,p2bkdt,p2lndr,p2dutr,p2infr,p2intr,p2crur,p2toam, " +
                                "p2osam,p2pcam,p2pcbl,p21due,p2fdam,p2diff,p2difb,p2frtm,p2frdt,p2fram,p2duty,p2dutb, " +
                                "p2fee,p2feeb,p2ufeb,p2feib,p2crua,p2crub,p2ucrb,p2ucib,p2uida,p2intb,p2ubas,p2uidb,p2resn) " +
                                "values (" + hid_brn.Value + "," + G_contract.ToString() + ",'02'," + hid_CSN.Value + "," + hid_AppNo.Value + ",'01', " +
                                "'" + Credit_01.ToString() + "','" + userInfo.Username.ToString() + "','250', " +
                                "" + Vendor_01.ToString() + "," + Maker_01.ToString() + "," + Campaign_01.ToString() + ", " + CampaignSeq_01.ToString() + ", " +
                                "'" + CampaignType_01.ToString() + "'," + Product_01.ToString() + "," + Price_01.ToString() + ", " + Qty_01.ToString() + ", " +
                                "" + Purchase_01.ToString() + ", " + Vattr_01.ToString() + "," + Vatta_01.ToString() + "," + Down_01.ToString() + ", " +
                                "" + Term_01.ToString() + "," + Range_01.ToString() + "," + NonDue_01.ToString() + ", '2', " +
                                "" + hid_date97.Value + "," + hid_date97.Value + "," + Loan_Amt_ILTB06.ToString() + ", " + Duty_Amt_ILTB06.ToString() + "," + Inf_01.ToString() + ", " +
                                "" + Int_01.ToString() + "," + Cru_01.ToString() + ", " + ContAmt_01.ToString() + ", " + ContAmt_01.ToString() + ", " +
                                "" + LoanReq_01.ToString() + ", " + LoanReq_01.ToString() + ", " + FirstDut_01.ToString() + ", " + FirstDueAmt_01.ToString() + ", " +
                                "" + Diff_01.ToString() + "," + Diff_01.ToString() + "," + FreeIns_01.ToString() + ", " +
                                "" + FirstDutDate_01.ToString() + ", " + FirstAmt_01.ToString() + ", " + Duty_01.ToString() + ", " + Duty_01.ToString() + ", " +
                                "" + TotInf_01.ToString() + "," + TotInf_01.ToString() + ", " + Infbase_24.ToString() + ", " +
                                "" + TotInf_01.ToString() + "," + TotCru_01.ToString() + "," + TotCru_01.ToString() + ", " +
                                "" + p2ucrb.Text.Trim() + "," + TotCru_01.ToString() + "," + TotInt_01.ToString() + ", " + TotInt_01.ToString() + ", " +
                                "" + p2ubas.Text.Trim() + ", " + TotInt_01.ToString() + ", 'A') ";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Insert ILMS02 KESSAI Approve ";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Insert ILMS02 KESSAI Approve ";
                    goto commit_rollback;
                }

                //ที่ต้องมา Update ตรงนี้เพิ่มเพราะ Command Insert ILMS02 ความยาวไม่พอ
                cmd.CommandText = "update AS400DB01.ILOD0001.ilms02 " +
                                  "set p2updt = " + hid_date97.Value + ", " +
                                  "p2uptm = " + m_UpdTime.ToString() + ", " +
                                  "p2prog = 'KESSAI', " +
                                  "p2user = '" + userInfo.Username + "', " +
                                  "p2ddsp = '" + userInfo.LocalClient + "' " +
                                  "where p2brn = " + hid_brn.Value + " and p2cont = " + G_contract.ToString() + " and p2csno = " + hid_CSN.Value + " ";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update ILMS02 KESSAI Approve ";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update ILMS02 KESSAI Approve ";
                    goto commit_rollback;
                }

                //UPDATE ILMD012
                //cmd.Parameters.Clear();
                //cmd.CommandText = "insert into ilmd012hs (select " + hid_date97.Value.ToString() + ", " + m_UpdTime.ToString() + ", " +
                //                  "'" + userInfo.Username.ToString() + "', '" + userInfo.LocalClient.ToString() + "', ilmd012.* " +
                //                  "from ilmd012 where d012br = " + hid_brn.Value.ToString() + " and d012ap = " + hid_AppNo.Value.ToString() + ") ";
                //affectedRows = -1;
                //try
                //{
                //    affectedRows = busobj.ExecuteNonQuery(cmd);
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    Error = "Error on Insert ILMS0D012HS KESSAI Approve ";
                //    goto commit_rollback;
                //}
                //if (affectedRows < 0)
                //{
                //    success = false;
                //    Error = "Error on Insert ILMS0D012HS KESSAI Approve ";
                //    goto commit_rollback;
                //}

                cmd.Parameters.Clear();
                cmd.CommandText = "Update AS400DB01.ILOD0001.ilmd012 set " +
                                  "d012ct = " + G_contract.ToString() + ", " +
                                  "d012ud = " + hid_date97.Value + ", " +
                                  "d012ut = " + m_UpdTime.ToString() + ", " +
                                  "d012us = '" + userInfo.Username.ToString() + "', " +
                                  "d012pg = 'KESSAI', " +
                                  "d012ws = '" + userInfo.LocalClient.ToString() + "' " +
                                  "where d012br = " + hid_brn.Value + " and d012ap = " + hid_AppNo.Value + " ";
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update ILMD012 KESSAI Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update ILMD012 KESSAI Approve";
                    goto commit_rollback;
                }
                #region " 76361 IL (Effective Interest Rate) Modify By Manop"

                double nint_rate = double.TryParse(hid_inteirsum.Value.ToString(), out nint_rate) ? Math.Round(nint_rate / 12, 4) : 0;
                double ncru_rate = double.TryParse(hid_crueirsum.Value.ToString(), out ncru_rate) ? Math.Round(ncru_rate / 12, 4) : 0;
                cmd.Parameters.Clear();
                iLDataCenterMssqlTCL = new ILDataCenterMssqlTCL(userInfoService.GetUserInfo());
                cmd.CommandText = iLDataCenterMssqlTCL.InsertIlMS23(hid_CSN.Value, G_contract.ToString(), nint_rate.ToString(), ncru_rate.ToString(),
                                                       hid_inteirsum.Value.ToString(), hid_crueirsum.Value.ToString(), "0", "0", "0", "0",
                                                        hid_date97.Value, m_UpdTime.ToString(), userInfo.Username.ToString(), userInfo.LocalClient.ToString(),
                                                        "KESSAI", "A", Term_01, FirstAmt_01, "2", (int.Parse(Term_01) - 1).ToString(), hid_installment.Value, hid_lastinstallment.Value);
                affectedRows = -1;
                try
                {
                    bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                    affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                }
                catch (Exception ex)
                {
                    success = false;
                    Error = "Error on Update ILMS23 KESSAI Approve";
                    goto commit_rollback;
                }
                if (affectedRows < 0)
                {
                    success = false;
                    Error = "Error on Update ILMS23 KESSAI Approve";
                    goto commit_rollback;
                }
                #endregion
                //INSERT ILMS86
                if (find_ilms86.ToString().Trim() != "Y")
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert into AS400DB01.ILOD0001.ilms86 (P86RST,P86BRN,P86CON,P86PRI,P86ACT,P86RUN,P86AMT, " +
                                      "P86BAL,P86GDT,P86DUE,P86RDT,P86PGM,P86USR,P86UDT,P86UTM) " +
                                      "Values(''," + hid_brn.Value + "," + G_contract.ToString() + ",3,'NCB',1, " +
                                      "" + txt_bureau.Text + ", " + txt_bureau.Text + "," + hid_appdate.Value + "," + hid_appdate.Value + ",0, " +
                                      "'KESSAI','" + userInfo.Username.ToString() + "'," + hid_date97.Value + "," + m_UpdTime.ToString() + ") ";
                    affectedRows = -1;
                    try
                    {
                        bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
                        affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Error = "Error on Insert ilms86 KESSAI Approve ";
                        goto commit_rollback;
                    }
                    if (affectedRows < 0)
                    {
                        success = false;
                        Error = "Error on Insert ilms86 KESSAI Approve ";
                        goto commit_rollback;
                    }
                }

                if (Mobile_SMS.ToString().Trim() != "")
                {
                    string poerrc = "";
                    string poerrm = "";
                    ILSRSMS iLSRSMS = new ILSRSMS();
                    bool sms = iLSRSMS.Call_ILSRSMS(CallHisunMaster._dataCenter, "C", "IL", userInfo.BranchApp.ToString().PadLeft(3, '0'),
                                                  hid_AppNo.Value.ToString().PadLeft(11, '0'), Mobile_SMS.ToString().Trim(), ref poerrc, ref poerrm);
                    if (!sms || poerrc == "Y")
                    {
                        success = false;
                        Error = "Error on Send SMS ";
                        goto commit_rollback;
                    }
                }

                //// **  Clear NMP 
                //string Err_CSSR67 = "";
                //bool call_CSSR67 = iLDataSubroutine.Call_CSSR67(hid_CSN.Value.Trim(), "ILAPPLY", ref Err_CSSR67, userInfo.BizInit, userInfo.BranchNo);
                //if (call_CSSR67 == false || Err_CSSR67.Trim() != "")
                //{
                //    success = false;
                //    Error = "Error Call CSSR67 : " + Err_CSSR67.ToString();
                //    goto commit_rollback;
                //}

                ////**  Block Contact
                //string Err_CSSR68 = " ";
                //bool call_CSSR68 = iLDataSubroutine.Call_CSSR68(hid_CSN.Value.Trim(), "A", ref Err_CSSR68, userInfo.BizInit, userInfo.BranchNo);
                //if (Err_CSSR68.ToString().Trim() != "")
                //{
                //    success = false;
                //    Error = "Error Call CSSR68 : " + Err_CSSR68.ToString();
                //    goto commit_rollback;

                //}


                //// Clear Flag  Req: 60215[Resign Flag] 14/05/2558

                //string Err_CSGC216 = "";
                //string Msg_CSGC216 = "";
                //bool call_CSGC216 = iLDataSubroutine.CALL_CSGC216(hid_CSN.Value.Trim(), "", userInfo.BizInit, userInfo.BranchNo, ref Err_CSGC216, ref Msg_CSGC216);
                //if (Err_CSGC216.ToString().Trim() != "" || Msg_CSGC216.ToString().Trim() != "")
                //{
                //    success = false;
                //    Error = "Error Call CSGC216 : " + Msg_CSGC216.ToString();
                //    goto commit_rollback;

                //}
            }
            catch (Exception ex)
            {
                Utility.WriteLogString(ex.Message.ToString());
                CallHisunMaster._dataCenter.RollbackMssql();
                CallHisunMaster._dataCenter.CloseConnectSQL();
            }

            


        } //Approve by KESSAI

    commit_rollback:
        if (success)
        {
            CallHisunMaster._dataCenter.CommitMssql();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            if (L_confirm.Text == "Do you want to Return to Interview?")
            {
                L_message.Text = "Return to Interview Completed";
            }
            if (L_confirm.Text == "Do you want to Cancel by Interview?")
            {
                L_message.Text = "Cancel by Interview Completed";
            }
            if (L_confirm.Text == "Do you want to Cancel by KESSAI?")
            {
                L_message.Text = "Cancel by KESSAI Completed";
            }
            if (L_confirm.Text == "Do you want to Reject by Interview?")
            {
                L_message.Text = "Reject by Interview Completed";
            }
            if (L_confirm.Text == "Do you want to Reject by KESSAI?")
            {
                L_message.Text = "Reject by KESSAI Completed";
            }
            if (L_confirm.Text == "Do you want to Approve by Interview?")
            {
                L_message.Text = "Approve by Interview Completed";
            }
            if (L_confirm.Text == "Do you want to Approve Contract?")
            {
                //L_message.ForeColor = System.Drawing.Color.Green;
                L_message.Text = "KESSAI Approve,"; // +"\r\n" +" Contract No : " + G_contract_txt.Text;
                L_contract.Text = " Contract No : " + G_contract_txt.Text;
                // **  Clear NMP 
                string Err_CSSR67 = "";
                bool call_CSSR67 = iLDataSubroutine.Call_CSSR67(hid_CSN.Value.Trim(), "ILAPPLY", ref Err_CSSR67, userInfo.BizInit, userInfo.BranchNo);
                if (call_CSSR67 == false || Err_CSSR67.Trim() != "")
                {
                    success = false;
                    Error = "Error Call CSSR67 : " + Err_CSSR67.ToString();
                    L_message1.Text = Error; 
                    L_message1.ForeColor = System.Drawing.Color.Red;
                    Utility.WriteLogString(Error);
                    // goto commit_rollback;
                }

                //**  Block Contact
                string Err_CSSR68 = " ";
                bool call_CSSR68 = iLDataSubroutine.Call_CSSR68(hid_CSN.Value.Trim(), "A", ref Err_CSSR68, userInfo.BizInit, userInfo.BranchNo);
                if (Err_CSSR68.ToString().Trim() != "")
                {
                    success = false;
                    Error = "Error Call CSSR68 : " + Err_CSSR68.ToString();
                    L_message1.Text = Error;
                    L_message1.ForeColor = System.Drawing.Color.Red;
                    Utility.WriteLogString(Error);
                    //  goto commit_rollback;

                }


                // Clear Flag  Req: 60215[Resign Flag] 14/05/2558

                string Err_CSGC216 = "";
                string Msg_CSGC216 = "";
                bool call_CSGC216 = iLDataSubroutine.CALL_CSGC216(hid_CSN.Value.Trim(), "", userInfo.BizInit, userInfo.BranchNo, ref Err_CSGC216, ref Msg_CSGC216);
                if (Err_CSGC216.ToString().Trim() != "" || Msg_CSGC216.ToString().Trim() != "")
                {
                    success = false;
                    Error = "Error Call CSGC216 : " + Msg_CSGC216.ToString();
                    L_message1.Text = Error;
                    L_message1.ForeColor = System.Drawing.Color.Red;
                    Utility.WriteLogString(Error);
                    //   goto commit_rollback;

                }

                if (DSVendor != null && DSVendor.Tables.Count > 0)
                {
                    foreach (DataRow dr in DSVendor.Tables[0].Rows)
                    {
                        if (dr["p10ats"].ToString().Trim() == "Y")
                        {
                            string FLGERR1 = "", FLGMSG1 = "";
                            bool res_ilsr73 = iLDataSubroutine.Call_ILSR73(G_contract_txt.Text, hid_date97.Value,
                                                                 FLGERR1, FLGMSG1, userInfo.BizInit, userInfo.BranchNo);
                            //busobj.CloseConnectioDAL();
                            if (!res_ilsr73 || FLGERR1 == "Y")
                            {
                                L_message1.Text = "ไม่สามารถ Auto Sign Lay-Bill ";
                                L_message1.ForeColor = System.Drawing.Color.Red;
                                Utility.WriteLogString(FLGMSG1);
                                E_redirect.Text = "Y";
                                P_confirm_TCL.ShowOnPageLoad = false;
                                P_message_TCL.ShowOnPageLoad = true;
                                return;
                            }
                            else
                            {
                                L_message1.Text = "Auto Sign Lay-Bill completed  ";
                                L_message1.ForeColor = System.Drawing.Color.Red;
                                Utility.WriteLogString("Auto Sign Lay-Bill completed ");
                                E_redirect.Text = "Y";
                                P_confirm_TCL.ShowOnPageLoad = false;
                                P_message_TCL.ShowOnPageLoad = true;
                                return;
                            }
                        }
                    }
                    DS.Clear();
                }
            }
            E_redirect.Text = "Y";
            P_confirm_TCL.ShowOnPageLoad = false;
            P_message_TCL.ShowOnPageLoad = true;
            return;
        }
        else
        {
            CallHisunMaster._dataCenter.RollbackMssql();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            L_message.Text = Error.ToString();
            E_redirect.Text = "N";
            P_message_TCL.ShowOnPageLoad = true;
            return;
        }
    }

    protected void btn_cal_TCL_Click(object sender, EventArgs e)
    {
        try
        {
            string err = "";
            calTCL();

            DataSet ds = new DataSet();
            if (calculateInstallment(ref err, ref ds))
            {
                if (ilObj.check_dataset(ds))
                {
                    gvTerm.DataSource = ds.Tables[0];
                    gvTerm.DataBind();

                    gv_install.DataSource = ds.Tables[1];
                    gv_install.DataBind();

                    if (ds.Tables[0] != null)
                    {
                        DataRow dr_tablezero = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];
                        p2ucrb.Text = dr_tablezero["CR_USAGE_BASE"].ToString();
                        p2ubas.Text = dr_tablezero["INTEREST_BASE"].ToString();
                    }

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
                    btn_cal_TCL.Enabled = false;
                }
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

    protected void B_cancelnote_TCL_Click(object sender, EventArgs e)
    {
        P_note_TCL.ShowOnPageLoad = false;
    }

    void Load_Action()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        try
        {

            DataSet DS = new DataSet();
            busobj.UserInfomation = userInfoService.GetUserInfo();
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select g24acd, g24des from AS400DB01.GNOD0000.gntb24 WITH (NOLOCK) where g24acd = 'ADD' and g24del = '' ", CommandType.Text).Result.data;


            D_action.Items.Clear();
            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    D_action.Items.Add(
                            new ListEditItem(dr["g24acd"].ToString().Trim() + " : " + dr["g24des"].ToString().Trim(), dr["g24acd"].ToString().Trim()));
                }
                //DS.Clear();
            }


            D_action.SelectedIndex = 1;
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
    }

    void Load_Reason()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        try
        {

            DataSet DS = new DataSet();
            busobj.UserInfomation = userInfoService.GetUserInfo();
            DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select g25rcd, g25des from AS400DB01.GNOD0000.gntb25 WITH (NOLOCK) " + G_reject_cancel.Text.Trim() + " ", CommandType.Text).Result.data;


            D_reason.Items.Clear();

            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    D_reason.Items.Add(
                            new ListEditItem(dr["g25rcd"].ToString().Trim() + " : " + dr["g25des"].ToString().Trim(), dr["g25rcd"].ToString().Trim()));
                }


                D_reason.Items.Add(
                          new ListEditItem("SL42" + " : " + " ลูกค้าไม่ผ่านเงื่อนไข RANK < 2 และ เงินเดือน < 10,000", "SL42"));
                //DS.Clear();
                D_reason.SelectedIndex = -1;
            }
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
        catch (Exception ex)
        {
            CallHisunMaster._dataCenter.CloseConnectSQL();
        }
    }

    protected void B_savenote_TCL_Click(object sender, EventArgs e)
    {
        L_NCB.Text = "";
        L_msg_note_TCL.Text = "";
        if (D_action.Text.Trim() == "")
        {
            L_msg_note_TCL.Text = "Please select Action code";
            return;
        }
        if (D_reason.Text.Trim() == "")
        {
            L_msg_note_TCL.Text = "Please select Reason code";
            return;
        }
        if (E_note_TCL.Text.Trim() == "")
        {
            L_msg_note_TCL.Text = "Please Key Note";
            return;
        }
        if (E_note_TCL.Text.Trim().IndexOf("'") >= 0 ||
                E_note_TCL.Text.Trim().IndexOf("\"") >= 0 ||
                E_note_TCL.Text.Trim().IndexOf("\\r") >= 0 ||
                E_note_TCL.Text.Trim().IndexOf("\\t") >= 0 ||
                E_note_TCL.Text.Trim().IndexOf("\\n") >= 0)
        {
            L_msg_note_TCL.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ \r \n \t  ";
            return;
        }
        if (E_note_TCL.Text.Trim().Length > 200)
        {

            L_msg_note_TCL.Text = "Lengh of note detail > 200 Characters.";
            return;

        }
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        ILDataCenter busobj = new ILDataCenter();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();
        iDB2Command cmd = new iDB2Command();
        bool success = true;
        int affectedRows = 0;

        bool AddNote = false;
        string note1 = "", note2 = "", ErrMsgNote = "", PendingCode = "";

        //cmd.Parameters.Clear();
        //cmd.CommandText = "insert into ilms01hs (select " + hid_date97.Value + ", " + m_UpdTime.ToString() + ", " +
        //                  "'" + userInfo.Username.ToString() + "', '" + userInfo.LocalClient.ToString() + "', ilms01.* " +
        //                  "from ilms01 where p1brn = " + hid_brn.Value + " and p1apno = " + hid_AppNo.Value + ") ";
        //affectedRows = -1;
        //try
        //{
        //    affectedRows = busobj.ExecuteNonQuery(cmd);
        //}
        //catch (Exception ex)
        //{
        //    success = false;
        //    L_msg_note_TCL.Text = "Error on Insert ILMS01HS";
        //    goto commit_rollback;
        //}
        //if (affectedRows < 0)
        //{
        //    success = false;
        //    L_msg_note_TCL.Text = "Error on Insert ILMS01HS";
        //    goto commit_rollback;
        //}

        string p1aprj = "", p1loca = "";
        if (P_note_TCL.HeaderText == "KESSAI CANCEL")
        {
            p1aprj = "CN";
            p1loca = "150";
        }
        if (P_note_TCL.HeaderText == "KESSAI REJECT")
        {
            p1aprj = "RJ";
            p1loca = "210";
        }
        cmd.Parameters.Clear();
        cmd.CommandText = "Update AS400DB01.ILOD0001.ILMS01 set " +
                              "p1aprj = '" + p1aprj.ToString() + "', " +
                              "p1loca = '" + p1loca.ToString() + "', " +
                              "p1stdt = " + hid_date97.Value + ", " +
                              "p1sttm = " + m_UpdTime.ToString() + ", " +
                              "p1auth = '" + userInfo.Username.ToString() + "', " +
                              "p1avdt = " + hid_date97.Value + ", " +
                              "p1avtm = " + m_UpdTime.ToString() + ", " +
                              "p1resn = '" + D_reason.Value.ToString() + "', " +
                              "p1fill = CONCAT(SUBSTRING(p1fill,1,24),'3',SUBSTRING(p1fill,26,13)), " +
                              "p1updt = " + hid_date97.Value + ", " +
                              "p1uptm = " + m_UpdTime.ToString() + ", " +
                              "p1upus = '" + userInfo.Username.ToString() + "', " +
                              "p1prog = 'KESSAI', " +
                              "p1wsid = '" + userInfo.LocalClient.ToString() + "'  " +
                              "where p1brn = " + hid_brn.Value.ToString() + " and p1apno = " + hid_AppNo.Value.ToString() + " ";
        affectedRows = -1;
        try
        {
            bool transaction = CallHisunMaster._dataCenter.Sqltr?.Connection == null ? true : false;
            affectedRows = CallHisunMaster._dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
        }
        catch (Exception ex)
        {
            success = false;
            L_msg_note_TCL.Text = "Error on Update KESSAI Reject/Cancel ILMS01";
            goto commit_rollback;
        }
        if (affectedRows < 0)
        {
            success = false;
            L_msg_note_TCL.Text = "Error on Update KESSAI Reject/Cancel ILMS01";
            goto commit_rollback;
        }

        if (E_note_TCL.Text.Trim().Length <= 100)
        {
            note1 = E_note_TCL.Text.Trim().Substring(0, E_note_TCL.Text.Trim().Length);
        }
        else
        {
            note1 = E_note_TCL.Text.Trim().Substring(0, 100);
        }
        if (E_note_TCL.Text.Trim().Length > 100)
        {
            note2 = E_note_TCL.Text.Trim().Substring(100, E_note_TCL.Text.Trim().Length - 100);
        }
        //AddNote = iLDataSubroutine.CALL_CSSRW11("IL", hid_idno.Value, D_action.Value.ToString(), D_reason.Value.ToString(),
        //                              E_note_TCL.Text.Trim(), note2, ref ErrMsgNote, userInfo.BizInit, userInfo.BranchNo);

        Connect_NoteAPI noteAPI = new Connect_NoteAPI();
        var resNote = noteAPI.AddNote(hid_idno.Value, "0", D_action.Value.ToString(), D_reason.Value.ToString(), E_note_TCL.Text.Trim(), m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

        if (!resNote.success || ErrMsgNote.Trim() != "")
        {
            L_msg_note_TCL.Text = "Cannot Save note Reject/Cancel : " + ErrMsgNote.ToString().Trim();
            CallHisunMaster._dataCenter.RollbackMssql();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }

    commit_rollback:
        if (success)
        {
            CallHisunMaster._dataCenter.CommitMssql();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            P_note_TCL.ShowOnPageLoad = false;
            if (P_note_TCL.HeaderText == "KESSAI CANCEL")
            {
                L_message.Text = "Cancel Completed";
            }
            if (P_note_TCL.HeaderText == "KESSAI REJECT")
            {
                L_message.Text = "Reject Completed";
            }
            P_message_TCL.ShowOnPageLoad = true;
            E_redirect.Text = "Y";
        }
        else
        {
            CallHisunMaster._dataCenter.RollbackMssql();
            CallHisunMaster._dataCenter.CloseConnectSQL();
            return;
        }
    }

    void Check_AMLO()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        ILDataSubroutine dataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        //Connect_AmloAPI _AmloAPI = new Connect_AmloAPI();
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();

        G_AMLO.Text = "";
        try
        {
            DataSet dsAMLO = dataSubroutine.Get_GNSRAM(hid_idno.Value, G_thainame.Text.Trim(), G_thaisurname.Text.Trim(),
                                               hid_birthdate.Value, "REQ", userInfo.BizInit, userInfo.BranchNo);
            if (dsAMLO != null && dsAMLO.Tables?.Count > 0)
            {
                if ((dsAMLO.Tables?[0].Rows?.Count > 0) && (dsAMLO.Tables?[0].Rows?[0]["POERR"].ToString().Trim() == "Y"))
                {
                    G_AMLO.Text = "Y";
                    L_message.Text = "CAN NOT PROCESS AMLO";
                    return;
                }
            }
            //var res = _AmloAPI.CheckAmloAPI(hid_idno.Value, G_thainame.Text.Trim(), G_thaisurname.Text.Trim(), hid_birthdate.Value).Result;
            //if (res.success)
            //{
            //    var jsonData = JsonConvert.SerializeObject(res.data);
            //    var resData = (AmloRespone)JsonConvert.DeserializeObject(jsonData, typeof(AmloRespone));
            //    if (resData?.errorFlag.ToString().ToUpper().Trim() == "Y")
            //    {
            //        G_AMLO.Text = "Y";
            //        L_message.Text = L_message.Text = "CAN NOT PROCESS AMLO";
            //        return;
            //    }
            //}
        }
        catch (Exception ex)
        {
            Utility.WriteLog(ex);
            L_message.Text = "Erorr on Call GNSRAM";
            return;
        }
    }

    void Check_RF_CM()
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        ILDataCenter busobj = new ILDataCenter();
        iLDataSubroutine = new ILDataSubroutine(userInfoService.GetUserInfo());
        busobj.UserInfomation = userInfoService.GetUserInfo();
        DataSet DS = new DataSet();

        try
        {
            G_RFCM.Text = "";
            string Error_CSSRRFAM = "", ErrorMsg_CSSRRFAM = "";
            bool CALL_CSSRRFAM = iLDataSubroutine.CALL_CSSRRFAM(hid_CSN.Value, ref Error_CSSRRFAM, ref ErrorMsg_CSSRRFAM, userInfo.BizInit, userInfo.BranchNo);
            if (Error_CSSRRFAM.ToString().Trim() == "Y")
            {
                if (ErrorMsg_CSSRRFAM.ToString().Trim() != "")
                {
                    G_RFCM.Text = "Y";
                    if (ErrorMsg_CSSRRFAM.ToString().Trim().Substring(0, 1) == "R")
                    {
                        L_message.Text = "Refinance";
                    }
                    if (ErrorMsg_CSSRRFAM.ToString().Trim().Substring(0, 1) == "C")
                    {
                        L_message.Text = "Compromise";
                    }
                    if (ErrorMsg_CSSRRFAM.ToString().Trim() == "WR")
                    {
                        L_message.Text = "Wait Refinance";
                    }
                    if (ErrorMsg_CSSRRFAM.ToString().Trim() == "WC")
                    {
                        L_message.Text = "Wait Compromise";
                    }
                }
            }
        }
        catch
        {
            L_message.Text = "Erorr on Call CSSRRFAM";
            return;
        }
    }
    private void ClearAddr()
    {
        D_tambol1.SelectedIndex = -1;
        D_amphur1.SelectedIndex = -1;
        D_province1.SelectedIndex = -1;
        D_zipcode1.SelectedIndex = -1;

    }
    protected void D_shipto1_SelectedIndexChanged(object sender, EventArgs e)
    {
        userInfoService = new UserInfoService();
        userInfo = userInfoService.GetUserInfo();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        E_village1.Text = "";
        D_building1.Value = "";
        E_buildingname1.Text = "";
        E_addressno1.Text = "";
        E_roomno1.Text = "";
        E_floor1.Text = "";
        E_moo1.Text = "";
        E_soi1.Text = "";
        E_road1.Text = "";
        D_tambol1.Items.Clear();
        D_amphur1.Items.Clear();
        D_province1.Items.Clear();
        D_zipcode1.Items.Clear();
        if (D_shipto1.Value == null)
        {
            ClearAddr();
        }
        else if ((D_shipto1.Value.ToString().Trim() == "H") || (D_shipto1.Value.ToString().Trim() == "O"))
        {
            ILDataCenter busobj = new ILDataCenter();
            busobj.UserInfomation = userInfoService.GetUserInfo();

            DataSet DS = new DataSet();

            /*
            DS = busobj.RetriveAsDataSet("select m11vil, m11bil, m11bln, m11adr, m11rom, m11flo, m11moo, m11soi, m11rod, m11tam, m11amp, m11prv, m11zip, " +
                                         "gn18dt, gn19dt, gn20dt " +
                                         "from csms11 " +
                                         "left join gntb18 on(m11tam=gn18cd) " +
                                         "left join gntb19 on(m11amp=gn19cd) " +
                                         "left join gntb20 on(m11prv=gn20cd) " +
                                         "left join gntb08 on(gt08tc=m11bil) " +
                                         "where m11csn = " + hid_CSN.Value + " and m11ref = '' and m11rsq = 0 and m11cde = '" + D_shipto1.Value.ToString().Trim() + "' ");
            int found_csms11 = 0;
            if (DS != null)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    found_csms11 = 1;
                    E_village1.Text = dr["m11vil"].ToString().Trim();
                    D_building1.Value = dr["m11bil"].ToString().Trim();
                    E_buildingname1.Text = dr["m11bln"].ToString().Trim();
                    E_addressno1.Text = dr["m11adr"].ToString().Trim();
                    E_roomno1.Text = dr["m11rom"].ToString().Trim();
                    E_floor1.Text = dr["m11flo"].ToString().Trim();
                    E_moo1.Text = dr["m11moo"].ToString().Trim();
                    E_soi1.Text = dr["m11soi"].ToString().Trim();
                    E_road1.Text = dr["m11rod"].ToString().Trim();
                    try
                    {
                        D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["m11tam"].ToString().Trim());
                        D_tambol1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["m11amp"].ToString().Trim());
                        D_amphur1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["m11prv"].ToString().Trim());
                        D_province1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_zipcode1.Items.Add(dr["m11zip"].ToString().Trim(), dr["m11zip"].ToString().Trim());
                        D_zipcode1.SelectedIndex = 0;
                    }
                    catch { }
                }
                DS.Clear();
            }
            */

            //if (found_csms11 == 0)
            //{
            if (D_shipto1.Value.ToString().Trim() == "H")
            {
                //DS = busobj.RetriveAsDataSet("select m13htm as tambol, m13ham as amphur, m13hpv as province, m13hzp as zipcode, gn18dt, gn19dt, gn20dt " +
                //                         "from csms13 " +
                //                         "left join gntb18 on(m13htm=gn18cd) " +
                //                         "left join gntb19 on(m13ham=gn19cd) " +
                //                         "left join gntb20 on(m13hpv=gn20cd) " +
                //                         "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m13htm as tambol, m13ham as amphur, m13hpv as province, m13hzp as zipcode, adt.DescriptionTHAI gn18dt, ada.DescriptionTHAI gn19dt, adp.DescriptionTHAI gn20dt " +
                                        "from AS400DB01.CSOD0001.CSMS13 cs WITH(NOLOCK) " +
                                        "left join GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(m13htm = adt.Code) " +
                                        "left join GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK) on(m13ham = ada.Code) " +
                                        "left join GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK) on(m13hpv = adp.Code) " + 
                                         "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
            }

            if (D_shipto1.Value.ToString().Trim() == "O")
            {
                //DS = busobj.RetriveAsDataSet("select m13otm as tambol, m13oam as amphur, m13opv as province, m13ozp as zipcode, gn18dt, gn19dt, gn20dt " +
                //                         "from csms13 " +
                //                         "left join gntb18 on(m13otm=gn18cd) " +
                //                         "left join gntb19 on(m13oam=gn19cd) " +
                //                         "left join gntb20 on(m13opv=gn20cd) " +
                //                         "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ");
                DS = CallHisunMaster._dataCenter.GetDataset<DataTable>("select m13otm as tambol, m13oam as amphur, m13opv as province, m13ozp as zipcode, adt.DescriptionTHAI gn18dt, ada.DescriptionTHAI gn19dt, adp.DescriptionTHAI gn20dt " +
                                         "from AS400DB01.CSOD0001.CSMS13 cs WITH(NOLOCK) " +
                                         "left join GeneralDB01.GeneralInfo.AddrTambol adt WITH(NOLOCK) on(m13otm = adt.Code) " +
                                         "left join GeneralDB01.GeneralInfo.AddrAumphur ada WITH(NOLOCK) on(m13oam = ada.Code) " +
                                         "left join GeneralDB01.GeneralInfo.AddrProvince adp WITH(NOLOCK) on(m13opv = adp.Code) " +
                                         "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ", CommandType.Text).Result.data;
            }

            if (DS != null && DS.Tables.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    try
                    {
                        D_tambol1.Items.Add(dr["gn18dt"].ToString().Trim(), dr["tambol"].ToString().Trim());
                        D_tambol1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_amphur1.Items.Add(dr["gn19dt"].ToString().Trim(), dr["amphur"].ToString().Trim());
                        D_amphur1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_province1.Items.Add(dr["gn20dt"].ToString().Trim(), dr["province"].ToString().Trim());
                        D_province1.SelectedIndex = 0;
                    }
                    catch { }

                    try
                    {
                        D_zipcode1.Items.Add(dr["zipcode"].ToString().Trim(), dr["zipcode"].ToString().Trim());
                        D_zipcode1.SelectedIndex = 0;
                    }
                    catch { }
                }
            }
            //}
        }
    }

    //Start Pact 20170822 check net salary
    private void CheckNetSalary(string step, ILDataCenter busobj, ref string err_, ref bool chk)
    {
        chk = true;
        DataSet DS = new DataSet();
        CallHisunMaster = new ILDataCenterMssql(userInfoService.GetUserInfo());
        if (hid_status.Value == "KESSAI" || hid_status.Value == "INTERVIEW")
        {
            DS = iLDataSubroutine.get_RLTB10();
            if (DS != null)
            {
                DataSet DSnetsal = new DataSet();
                DSnetsal = CallHisunMaster._dataCenter.GetDataset<DataTable>("Select M13NET from AS400DB01.CSOD0001.csms13 " +
                                 "where m13app = 'IL' and m13csn = " + hid_CSN.Value + " " +
                                 "and m13brn = " + hid_brn.Value + " and m13apn = " + hid_AppNo.Value + " ",CommandType.Text).Result.data;

                if (DS.Tables[0].Rows.Count > 0 && DSnetsal.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDecimal(DS.Tables[0].Rows[0]["T10CD2"]) > Convert.ToDecimal(DSnetsal.Tables[0].Rows[0]["M13NET"]))
                    {
                        err_ += "ไม่สามารถ " + step + " เนื่องจากลูกค้าไม่ผ่าน BOT Regulation " + "\r\n";
                        L_message.ForeColor = System.Drawing.Color.Red;
                        L_message.Text = err_;
                        P_message_TCL.ShowOnPageLoad = true;
                        CallHisunMaster._dataCenter.CloseConnectSQL();
                        chk = false;
                        return;
                    }
                }
                DSnetsal.Clear();
            }
            DS.Clear();
        }
    }

    //protected void btnLinkImageAndText_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        Cache.Remove("dsResCode_TCL");
    //        Cache.Remove("dsActionCode_TCL");
    //        Load_Action();
    //        Load_Reason();

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    ///Ning 20231212 CheckPeps Status 
    protected string CheckPEPsStatus(string firstName, string surName)
    {
        string result = "";
        TokenService _tokenService = new TokenService();

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(surName))
        {
            string apiHost = WebConfigurationManager.AppSettings["PepServiceAPI"].ToString().Trim();
            string apiPath = "/api/Pep/CheckPEPsByName";
            string apiUrl = new Uri(new Uri(apiHost), apiPath).ToString();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenService.BuildToken());

                var requestModel = new PEPsModel.RequestPEPsModel
                {
                    firstNameTH = firstName,
                    lastNameTH = surName,
                    firstNameEN = firstName,
                    lastNameEN = surName
                };

                string jsonRequest = JsonConvert.SerializeObject(requestModel);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    Task<HttpResponseMessage> responseTask = httpClient.PostAsync(apiUrl, content);
                    HttpResponseMessage response = responseTask.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> jsonResponseTask = response.Content.ReadAsStringAsync();
                        jsonResponseTask.Wait(); //Blocking call

                        string jsonResponse = jsonResponseTask.Result;
                        var responseObject = JsonConvert.DeserializeObject<PEPsModel.ResponseModel>(jsonResponse);

                        Console.WriteLine(" responseObject " + responseObject.data.isPEPs);

                        if (responseObject.data.isPEPs)
                        {
                            return "PEP";
                        }
                    }
                    else
                    {
                        Console.WriteLine("API request failed. Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("API request failed. Status code:  " + ex.Message.ToString());
                }
            }
        }
        else
        {
            Console.WriteLine("Name or SurName is Null");
        }
        return result;
    }
}