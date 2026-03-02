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

namespace ILSystem.ManageData.WorkProcess.Campaign
{
    public partial class CampaignProposal : System.Web.UI.Page
    {
        protected DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        protected ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        protected ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        protected ILDataCenter ilObj = new ILDataCenter();
        protected iDB2Command cmd = new iDB2Command();
        protected DataSet dsCampaignList = new DataSet();
        protected DataSet dsAllProductMoreRate = new DataSet();
        protected DataRow drAllProductMoreRate;
        protected DataCenter dataCenter;
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        protected string SqlAll = "";
        protected string Msg = "";
        protected string MsgHText = "";
        protected string checkMode = "";
        public CookiesStorage campaignStorage;
        protected string sqlBetween = "";
        protected string sqlCondition = "";
        protected string sqlChooseCampaign = "";
        protected string sqlRank = "";
        protected string rankVendor = "";
        protected string Subsidizes = "";

        protected int startDateNews = 0;
        protected int endDateNews = 0;
        protected int closingDateNews = 0;
        protected int C07PIT_CHECK = 0;

        protected string strWhere = "";
        protected string brands, producCodes, models, minPrice, maxPrice, intRate, crrUsg, infRate, termBegin, termEnd, installs, mem_Brands, mem_ProducCodes, mem_models, G_Range, Min_Seq;
        protected string gRange, gLoanType, gOrderBy;
        protected string productItem, exceptMoreOne, minSeq, maxSeq, maxRange;
        protected int allProductMoreRate;
        protected void Page_Load(object sender, EventArgs e)
        {
            campaignStorage = new CookiesStorage();
            userInfoService = new UserInfoService();
            m_userInfo = userInfoService.GetUserInfo();
            dataCenter = new DataCenter(m_userInfo);
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
                ENABLE_DEFAULT();
                CLEAR_LIST_DATA();
                gvListDataVendor.DataBind();
                gvListData.DataBind();
                POPUP_POSITTON();
                btnReport.Enabled = false;
            }
            
        }
        protected void REPORT_PRINT(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Javascript", "javascript:TRANFER_REPORT_PRINT();", true);
        }

        protected void btAddRank_OnClick(object sender, EventArgs e)
        {
            //Popup_AddRank.ShowOnPageLoad = true;
        }
        static string FORMAT_DATE(string dateCall)
        {
            DateTime dtime;
            DateTime.TryParseExact(dateCall, "yyyyMMdd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);
            string formattedDateNew = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return formattedDateNew;
        }

        #region  Function show popup alert
        private void POPUP_POSITTON()
        {
            POPUP_MSG_ERROR.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            POPUP_MSG_ERROR.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_Campaign.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_Campaign.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            POPUP_ALERT_CHECK.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            POPUP_ALERT_CHECK.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
        }
        private void SET_MSG()
        {
            lblMsg.Text = Msg;
            POPUP_MSG_ERROR.HeaderText = MsgHText;
            POPUP_MSG_ERROR.ShowOnPageLoad = true;
        }
        #endregion

        #region function about selection campaign

        protected void SELECT_CAMPAIGN_ONCHANGE(object sender, EventArgs e)
        {
            if (int.Parse(campaignCode.Text.Length.ToString()) >= 5)
            {

                SqlAll =    " Select cast(c01cmp as varchar) as c01cmp,c01cnm,c01sty,c01vdc,c01mkc,cast(c01sdt as varchar) as c01sdt ,cast(c01edt as varchar) as c01edt," +
                            " cast(c01cad as varchar) as c01cad,cast(c01cld as varchar) as c01cld ,trim(CAST(c01srt AS nvarchar)) as c01srt,c01nxd,c01lty,p10nam,t46enm,user as user_login,'" + m_UpdDate + "' as Cur_Date,c01rng " +
                            " From AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK) " +
                            " left join AS400DB01.ILOD0001.ilcp05 as a WITH (NOLOCK) on(c01cmp = a.c05cmp and a.c05csq = 1 and a.c05rsq = 1 and a.c05par = 'M') " +
                            " left join AS400DB01.ILOD0001.ilcp05 as b WITH (NOLOCK) on(c01cmp = b.c05cmp and b.c05csq = 1 and b.c05rsq = 1 and b.c05par = 'V') " +
                            " left join AS400DB01.ILOD0001.iltb46 WITH (NOLOCK) on(a.c05pcd= t46mak) " +
                            " left join AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) on(b.c05pcd= p10ven) " +
                            " where CAST(c01cmp AS nvarchar) = '" + campaignCode.Text.Trim() + "'";
                DataSet dsData = new DataSet();
                m_userInfo = userInfoService.GetUserInfo();
                
                ilObj.UserInfomation = m_userInfo;
                dataCenter = new DataCenter(m_userInfo);
                cmd.CommandText = SqlAll;

                try
                {
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    var searchTextbox = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                    if (searchTextbox == -1)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();

                        Msg = "Please check command query !\r\n 'Select data textbox' ";
                        MsgHText = "Error Query";
                        SET_MSG();

                        return;
                    }

                    dataCenter.CommitMssql();
                    cmd.Parameters.Clear();

                }
                catch (Exception)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return;
                }
                dsData = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                if (dsData.Tables[0]?.Rows.Count <= 0)
                {
                    lblMsgAlert.Text = "Invalid campaign code!";
                    POPUP_ALERT_CHECK.ShowOnPageLoad = true;
                    ENABLE_DEFAULT();
                    CLEAR_LIST_DATA();
                    dataCenter.CloseConnectSQL();
                    return;
                }
                DataRow drCampaign = dsData.Tables[0]?.Rows[0];

                string startDateNew = FORMAT_DATE(drCampaign["C01SDT"].ToString().Trim());
                string endDateNew = FORMAT_DATE(drCampaign["C01EDT"].ToString().Trim());
                string closeAppDateNew = FORMAT_DATE(drCampaign["C01CAD"].ToString().Trim());
                string closeLayDateNew = FORMAT_DATE(drCampaign["C01CLD"].ToString().Trim());

                if (drCampaign["C01STY"].ToString().Trim() == "1")
                {
                    Subsidizes = "Maker";
                }
                else if (drCampaign["C01STY"].ToString().Trim() == "2")
                {
                    Subsidizes = "Vendor";
                }
                else if (drCampaign["C01STY"].ToString().Trim() == "3")
                {
                    Subsidizes = "Non Sub Campaign";
                }
                else if (drCampaign["C01STY"].ToString().Trim() == "4")
                {
                    Subsidizes = "Share Sub";
                }

                campaignCode.Text = drCampaign["C01CMP"].ToString().Trim();
                campaignDes.Text = drCampaign["C01CNM"].ToString().Trim();
                txtType.Text = Subsidizes;
                double txtRates = double.Parse(drCampaign["C01SRT"].ToString().Trim());
                txtRate.Text = txtRates.ToString("#,##0.00").Trim();
                ////txtRate.Text = drCampaign["C01SRT"].ToString().Trim();
                txtSubsidize.Text = drCampaign["T46ENM"].ToString().Trim();
                startDate.Text = startDateNew;
                endDate.Text = endDateNew;
                closeAppDate.Text = closeAppDateNew;
                closeLayDate.Text = closeLayDateNew;

                gRange = drCampaign["C01RNG"].ToString().Trim();
                gLoanType = drCampaign["C01LTY"].ToString().Trim();

                campaignStorage.SetCookiesDataSetByName("ds_gvListData", dsData);
                DataSet DS = new DataSet();
                SqlAll = "select isnull(count(*),0) as Count1, isnull(max(c02csq),0) as Max1 from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) where CAST(c02cmp AS nvarchar)='" + campaignCode.Text.Trim() + "'";
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                DataRow drCheckOrderBy = DS.Tables[0]?.Rows[0];
                if (drCheckOrderBy["COUNT1"].ToString() != drCheckOrderBy["MAX1"].ToString() && int.Parse(drCheckOrderBy["COUNT1"].ToString()) > 2)
                {
                    gOrderBy = " ,C02CSQ, C02RSQ ";
                }
                else
                {
                    gOrderBy = " ,C02FMT,C02TOT ";
                }
                dataCenter.CloseConnectSQL();

                DATA_LIST_NOTE();
                DATA_LIST_VENDOR();
                DATA_LIST_CAMPAIGN();
                btnReport.Enabled = true;
                return;
            }
        }
        protected void CLICK_SEARCH_CAMPAIGN_POPUP(object sender, EventArgs e)
        {
            POPUP_CAMPAIGN();
        }
        protected void CLICK_SEARCH_CAMPAIGN(object sender, EventArgs e)
        {
            POPUP_CAMPAIGN();
            Popup_Campaign.ShowOnPageLoad = true;
        }
        protected void CLEAR_POPUP_CAMPAIGN(object sender, EventArgs e)
        {
            ddlSearchCampaign.SelectedIndex = 0;
            txtSearchCampaign.Text = "";
            POPUP_CAMPAIGN();
        }
        protected void campaign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Campaign.PageIndex = e.NewPageIndex;
            gv_Campaign.DataSource = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value);;
            gv_Campaign.DataBind();
        }
        protected void campaign_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_campaign = campaignStorage.JsonDeserializeObjectHiddenDataSet(ds_gvCampaign.Value);;
            DataRow drCampaign = ds_campaign.Tables[0]?.Rows[(gv_Campaign.PageIndex * Convert.ToInt16(gv_Campaign.PageSize)) + e.NewSelectedIndex];

            string startDateNew = FORMAT_DATE(drCampaign["C01SDT"].ToString().Trim());
            string endDateNew = FORMAT_DATE(drCampaign["C01EDT"].ToString().Trim());
            string closeAppDateNew = FORMAT_DATE(drCampaign["C01CAD"].ToString().Trim());
            string closeLayDateNew = FORMAT_DATE(drCampaign["C01CLD"].ToString().Trim());

            if (drCampaign["C01STY"].ToString().Trim() == "1")
            {
                Subsidizes = "Maker";
            }
            else if (drCampaign["C01STY"].ToString().Trim() == "2")
            {
                Subsidizes = "Vendor";
            }
            else if (drCampaign["C01STY"].ToString().Trim() == "3")
            {
                Subsidizes = "Non Sub Campaign";
            }
            else if (drCampaign["C01STY"].ToString().Trim() == "4")
            {
                Subsidizes = "Share Sub";
            }

            campaignCode.Text = drCampaign["C01CMP"].ToString().Trim();
            campaignDes.Text = drCampaign["C01CNM"].ToString().Trim();
            txtType.Text = Subsidizes;
            double txtRates = double.Parse(drCampaign["C01SRT"].ToString().Trim());
            txtRate.Text = txtRates.ToString("#,##0.00").Trim();
            txtSubsidize.Text = drCampaign["T46ENM"].ToString().Trim();
            startDate.Text = startDateNew;
            endDate.Text = endDateNew;
            closeAppDate.Text = closeAppDateNew;
            closeLayDate.Text = closeLayDateNew;

            gRange = drCampaign["C01RNG"].ToString().Trim();
            gLoanType = drCampaign["C01LTY"].ToString().Trim();
            Popup_Campaign.ShowOnPageLoad = false;
            ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(ds_campaign);
            DATA_LIST_NOTE();
            DATA_LIST_VENDOR();
            DATA_LIST_CAMPAIGN();
            btnReport.Enabled = true;
            dataCenter.CloseConnectSQL();

            return;
        }

        #region list data note table
        private void DATA_LIST_NOTE()
        {
            SqlAll ="Select c11cmp, cast(c11udt as varchar(8)) as c11udt,cast(c11utm as varchar(6)) as c11utm,c11not,cast(c11nsq as varchar(3)) as c11nsq,c11lsq " +
                    "From AS400DB01.ILOD0001.ilcp11 WITH (NOLOCK) " +
                    "where CAST(c11cmp AS nvarchar) = '" + campaignCode.Text.Trim() + "' " +
                    "order by c11nsq,c11lsq";

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            cmd.CommandText = SqlAll;

            try
            {
                bool transaction = dataCenter.Sqltr == null ? true : false;
                var noteList = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (noteList == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = "Please check command query !\r\n 'List Note Table' ";
                    MsgHText = "Error Query";
                    SET_MSG();

                    return;
                }

                dataCenter.CommitMssql();
                cmd.Parameters.Clear();

            }
            catch (Exception)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return;
            }

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_gvListNote", DS);
            gvListData.DataSource = DS;
            gvListData.DataBind();
            dataCenter.CloseConnectSQL();

            //if (gvListData.Rows.Count == 0)
            //{
            //    dataCenter.CloseConnectSQL();
            //    return;
            //}
        }
        #endregion

        #region list data vendor table
        private void DATA_LIST_VENDOR()
        {
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsDateNew = new DataSet();
            SqlAll = "select p97cdt from  AS400DB01.ILOD0001.ilms97 WITH (NOLOCK) where p97rec=01";
            dsDateNew = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            DataRow drDateNew = dsDateNew.Tables[0]?.Rows[0];
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            var p97Date = drDateNew["P97CDT"].ToString();
            if (decimal.Parse(p97Date) > decimal.Parse(dateTimeNow))
            {
                p97Date = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH"));
            }
            string drDateNewCheck = p97Date;
            SqlAll = "Select isnull((c08ven),'') as c08ven1, isnull(p10nam,'') as p10nam, isnull(p16rnk,'') as p16rnk, isnull(p12wor,0) as p12wor, isnull(p12odr,0) as p12odr, p10edt, c81end, " +
                " case when(CAST(p10edt AS nvarchar) >= '" + drDateNewCheck + "') or(p10edt = 0) or(p10edt is null) then '' else SUBSTRING(CAST(isnull(p10edt, 0) AS nvarchar), 7, 2) + '/' + SUBSTRING(CAST(isnull(p10edt, 0) AS nvarchar), 5, 2) + '/' + SUBSTRING(CAST(isnull(p10edt, 0) AS nvarchar), 1, 4) end as p10edt1," +
                " case when(CAST(c81end AS nvarchar) > '" + drDateNewCheck + "') or(c81end = 0) or(c81end is null) then '' else SUBSTRING(CAST(isnull(c81end, 0) AS nvarchar), 7, 2) + '/' + SUBSTRING(CAST(isnull(c81end, 0) AS nvarchar), 5, 2) + '/' + SUBSTRING(CAST(isnull(c81end, 0) AS nvarchar), 1, 4) end as c81end1" +
                " From AS400DB01.ILOD0001.ilcp08 WITH (NOLOCK)" +
                " left join AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) on(c08ven= p10ven) " +
                " left join AS400DB01.ILOD0001.ilms16 WITH (NOLOCK) on(c08ven= p16ven and p16sts = '' and p16std<= " + drDateNewCheck + " and p16end>= " + drDateNewCheck + ")  " +
                " left join AS400DB01.ILOD0001.ilms12 WITH (NOLOCK) on(c08ven= p12ven) " +
                " left join AS400DB01.ILOD0001.ilcp081 WITH (NOLOCK) on(c81cmp=" + campaignCode.Text.Trim() + " and c08ven = c81ven) " +
                " where c08cmp=" + campaignCode.Text.Trim() + " order by c08ven ";

            
            cmd.CommandText = SqlAll;

            //try
            //{
            //    bool transaction = dataCenter.Sqltr == null ? true : false;
            //    var vendorList = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
            //    if (vendorList == -1)
            //    {
            //        dataCenter.RollbackMssql();
            //        dataCenter.CloseConnectSQL();

            //        Msg = "Please check command query !\r\n 'List Vendor Table' ";
            //        MsgHText = "Error Query";
            //        SET_MSG();

            //        return;
            //    }

            //    dataCenter.CommitMssql();
            //    cmd.Parameters.Clear();
            //}
            //catch (Exception)
            //{
            //    dataCenter.RollbackMssql();
            //    dataCenter.CloseConnectSQL();
            //    return;
            //}

            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            campaignStorage.SetCookiesDataSetByName("ds_gvListDataVendor", DS);
            gvListDataVendor.DataSource = DS;
            gvListDataVendor.DataBind();
            dataCenter.CloseConnectSQL();

        }
        protected void GV_LIST_VENDOR_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvListDataVendor = e.Row;
                if (gvListDataVendor.Cells[5].Text.Trim() == "99999999")
                {
                    gvListDataVendor.Cells[5].Text = "";
                }

            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region list data campaign table
        private void DATA_LIST_CAMPAIGN()
        {
            m_userInfo = userInfoService.GetUserInfo();
            dataCenter = new DataCenter(m_userInfo);
            DataSet dsDateCheck = new DataSet();
            SqlAll = "select c07pit,c07csq from ilcp07 WITH (NOLOCK) where c07cmp=" + campaignCode.Text.Trim() + " ";
            dsDateCheck = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (dsDateCheck.Tables[0]?.Rows.Count > 0)
            {
                DataRow drdsDateCheck = dsDateCheck.Tables[0]?.Rows[0];
                C07PIT_CHECK = int.Parse(drdsDateCheck["C07PIT"].ToString());


                if (C07PIT_CHECK > 0)
                {
                    SqlAll = "select distinct c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max," +
                        " t44lty,t44itm,t44itm,t44typ,t44brd,t44cod,t44mdl,t42des,t41des,t43des,t44des,4 as row, '" + gRange + "' as Range, 'B' as Min_Seq" +
                        " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                        " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "') " +
                        " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on(t44lty= c07lnt and t44itm = c07pit) " +
                        " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on(t44brd= t42brd) " +
                        " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on(t44lty= t41lty and t44typ = t41typ and t44cod = t41cod) " +
                        " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on(t44lty= t43lty and t44typ = t43typ and t44brd = t43brd and t44cod = t43cod and t44mdl = t43mdl) " +
                        " where c02cmp= " + campaignCode.Text.Trim() + "" +
                        " order by c02csq,t44brd,t44cod,t44mdl" + gOrderBy + " ";
                    
                    ilObj.UserInfomation = m_userInfo;
                    dsCampaignList = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    DataTable dtListDataCampaign = new DataTable();
                    dtListDataCampaign.Columns.AddRange(new DataColumn[11] { new DataColumn("TA"),
                new DataColumn("TB"),
                new DataColumn("TC"),
                new DataColumn("C07A"),
                new DataColumn("C07B"),
                new DataColumn("C02A"),
                new DataColumn("C02B"),
                new DataColumn("C02C"),
                new DataColumn("C02D"),
                new DataColumn("C02E"),
                new DataColumn("C02F") });

                    foreach (DataRow drsCampaign in dsCampaignList.Tables[0]?.Rows)
                    {
                        string ddddd = drsCampaign["T43DES"].ToString();
                        string ooodfd = drsCampaign["T44DES"].ToString();

                        brands = drsCampaign["T42DES"].ToString();
                        producCodes = drsCampaign["T41DES"].ToString();
                        if (C07PIT_CHECK == 0)
                        {
                            models = "";
                        }
                        else
                        {
                            if (drsCampaign["T43DES"].ToString() != "" || drsCampaign["T44DES"].ToString() != "")
                            {
                                models = drsCampaign["T43DES"].ToString() + " : " + drsCampaign["T44DES"].ToString();
                            }
                            else
                            {
                                models = "";
                            }
                        }

                        minPrice = drsCampaign["C07MIN"].ToString();
                        maxPrice = drsCampaign["C07MAX"].ToString();
                        intRate = drsCampaign["C02INR"].ToString();
                        crrUsg = drsCampaign["C02CRR"].ToString();
                        infRate = drsCampaign["C02IFR"].ToString();

                        if (drsCampaign["RANGE"].ToString() == "N")
                        {
                            if (drsCampaign["MIN_SEQ"].ToString() == "B")
                            {
                                termBegin = drsCampaign["C02TOT"].ToString();

                            }
                            else
                            {
                                termBegin = drsCampaign["MIN_SEQ"].ToString();
                            }
                        }
                        else
                        {
                            termBegin = drsCampaign["C02FMT"].ToString();
                        }
                        termEnd = drsCampaign["C02TOT"].ToString();
                        installs = drsCampaign["C02INS"].ToString();
                        mem_Brands = drsCampaign["T42DES"].ToString();
                        mem_ProducCodes = drsCampaign["T41DES"].ToString();
                        if (drsCampaign["T43DES"].ToString() != "" || drsCampaign["T44DES"].ToString() != "")
                        {
                            mem_models = drsCampaign["T43DES"].ToString() + " : " + drsCampaign["T44DES"].ToString();
                        }

                        dtListDataCampaign.Rows.Add(brands, producCodes, models, minPrice, maxPrice, intRate, crrUsg, infRate, termBegin, termEnd, installs);

                    }

                    for (int i = 1; i < dsCampaignList.Tables[0]?.Rows.Count; i++)
                    {
                        DataRow dr = dsCampaignList.Tables[0]?.Rows[i];
                        dr.Delete();
                    }
                    dsCampaignList.Tables[0]?.AcceptChanges();
                    campaignStorage.SetCookiesDataSetByName("ds_gvListDataCampaign", dsCampaignList);
                    gvListDataCampaign.DataSource = dtListDataCampaign;
                    gvListDataCampaign.DataBind();
                    dataCenter.CloseConnectSQL();
                }
                else if (C07PIT_CHECK == 0)
                {
                    //Check EXCEPT
                    DataSet dsExceptCheck = new DataSet();
                    SqlAll = "select c04pit from AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) where c04cmp =" + campaignCode.Text.Trim() + " ";
                    dsExceptCheck = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                    if (dsExceptCheck.Tables[0]?.Rows.Count > 0)
                    {
                        DataRow drExceptCheck = dsExceptCheck.Tables[0]?.Rows[0];
                        if (drExceptCheck["C04PIT"].ToString() == "")
                        {
                            productItem = "";
                        }
                        else
                        {
                            productItem = drExceptCheck["C04PIT"].ToString();
                        }

                        //Check Except More Product
                        DataSet dsExceptMoreCheck = new DataSet();
                        SqlAll = "select count(*) as NUMCOUNT from AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) where c04cmp=" + campaignCode.Text.Trim() + " ";
                        dsExceptMoreCheck = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                        DataRow drExceptMoreCheck = dsExceptMoreCheck.Tables[0]?.Rows[0];

                        if (int.Parse(drExceptMoreCheck["NUMCOUNT"].ToString()) < 2)
                        {
                            exceptMoreOne = "False";
                        }
                        else
                        {
                            exceptMoreOne = "True";
                        }
                        if (productItem != "")
                        {
                            DataSet dsProductItemCheck = new DataSet();
                            SqlAll = "select COUNT(c02cmp) as c02cmps from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) where c02cmp=" + campaignCode.Text.Trim() + " ";
                            dsProductItemCheck = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                            DataRow dProductItemCheck = dsProductItemCheck.Tables[0]?.Rows[0];

                            if (int.Parse(dProductItemCheck["c02cmps"].ToString()) > 1)
                            {
                                DataSet dsMinMax = new DataSet();
                                SqlAll = "select min(c02tot) AS minPrice,max(c02tot) AS maxPrice ,max(c02rsq) AS maxRange from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) where c02cmp=" + campaignCode.Text.Trim() + " ";
                                dsMinMax = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                                DataRow drsMinMax = dsMinMax.Tables[0]?.Rows[0];
                                minSeq = drsMinMax["MINPRICE"].ToString();
                                maxSeq = drsMinMax["MAXPRICE"].ToString();
                                maxRange = drsMinMax["MAXRANGE"].ToString();

                                //Check all product More Rate
                                DataSet dsProductMoreRateCheck = new DataSet();
                                SqlAll = "select distinct c02rsq,c02inr,c02crr,c02ifr from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) where c02cmp =" + campaignCode.Text.Trim() + " ";
                                dsProductMoreRateCheck = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                                DataRow drProductMoreRateCheck = dsProductMoreRateCheck.Tables[0]?.Rows[0];
                                allProductMoreRate = dsProductMoreRateCheck.Tables[0].Rows.Count;

                                if (allProductMoreRate == 1)
                                {
                                    strWhere = " where c02cmp=" + campaignCode.Text.Trim() + " and c02tot=" + maxSeq + " and c02rsq=" + maxRange + "";
                                }
                                else
                                {
                                    strWhere = " where c02cmp=" + campaignCode.Text.Trim() + "";
                                }

                                // Qery All Product More Rate
                                if (allProductMoreRate == 1)
                                {
                                    SqlAll = "c02cmp, c02csq, c02inr, c02crr, c02ifr, c02fmt, c02tot, c02ins, c07cmp, c07csq, c07pit, c07min, c07max, t44lty, t44itm, t44itm,t44typ,t44brd,t44cod,t44mdl," +
                                        " t42des,t41des,t43des,t44des,2 as row,'" + gRange + "' as Range,'" + minSeq + "' as Min_Seq " +
                                        " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                        " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "')" +
                                        " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on (1 = 2)" +
                                        " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on (c04pty= t41typ and c04pcd = t41cod)" +
                                        " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on (1 = 2)" +
                                        " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on (1 = 2)" +
                                        " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on (1 = 2) " + strWhere + "" +
                                        " union" +
                                        " select c02cmp,c02csq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins, c07cmp,c07csq,c07pit,c07min,c07max,t44lty,t44itm,t44itm,t44typ,t44brd,t44cod,t44mdl," +
                                        " t42des,case when c04pcd = 0 then t40des else t41des end as t41des, t43des,t44des,3 as row,'" + gRange + "' as Range,'B' as Min_Seq" +
                                        " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                        " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = " + gLoanType + ")" +
                                        " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on (c04cmp= c07cmp)" +
                                        " left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK) on (c04pty= t40typ)" +
                                        " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on (c04pty= t41typ and c04pcd = t41cod)" +
                                        " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on (1 = 2)" +
                                        " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on (1 = 2)" +
                                        " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on (1 = 2)" + strWhere + "  ";

                                    dsAllProductMoreRate = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
                                }
                                else
                                {
                                    if (allProductMoreRate > 1 && exceptMoreOne == "true")
                                    {
                                        minSeq = "B";
                                        SqlAll = "select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,CAST(t44itm AS nvarchar) as t44itm,t44itm as t44itm1, " +
                                               " t44typ,t44brd,t44cod,t44mdl,t42des,t41des,t43des,t44des,0 as row,'" + gRange + "' as Range, 'B' as Min_Seq " +
                                               " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                               " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "') " +
                                               " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on(1 = 2) " +
                                               " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on(c04pty= t41typ and c04pcd = t41cod) " +
                                               " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on(1 = 2) " +
                                               " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on(1 = 2) " +
                                               " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on(1 = 2) " +
                                               " where CAST(c02cmp AS nvarchar) = '" + campaignCode.Text.Trim() + "' " +
                                               " union " +
                                               " select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,CAST(t44itm AS nvarchar) as t44itm,(t44itm) as t44itm1, " +
                                               " t44typ,t44brd,t44cod,t44mdl,t42des,case when c04pcd = 0 then t40des else t41des end as t41des,t43des,t44des,3 as row, '" + gRange + "' as Range, 'B' as Min_Seq " +
                                               " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                               " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq AND c07lnt = '" + gLoanType + "') " +
                                               " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on(c04cmp= c07cmp) " +
                                               " left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK) on(c04pty= t40typ) " +
                                               " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on(c04pty= t41typ and c04pcd = t41cod) " +
                                               " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on(1 = 2) " +
                                               " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on(1 = 2) " +
                                               " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on(1 = 2) " +
                                               " where CAST(c02cmp AS nvarchar) = '" + campaignCode.Text.Trim() + "' order by c07pit " + gOrderBy + "  ";
                                    }
                                    else
                                    {
                                        minSeq = "B";
                                        SqlAll = "select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,CAST(t44itm AS nvarchar) as t44itm,t44itm as t44itm1,t44typ,t44brd,t44cod,t44mdl,t42des,case when c04pcd = 0 then t40des else t41des end as t41des,t43des,t44des,0 as row,'" + gRange + "' as Range,'B' as Min_Seq" +
                                            " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                            " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) ON (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "') " +
                                            " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) ON (c04cmp = c07cmp)" +
                                            " left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK) ON (c04pty = t40typ) " +
                                            " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) ON (c04pty = t41typ and c04pcd = t41cod) " +
                                            " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) ON (1 = 2)" +
                                            " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) ON (1 = 2)" +
                                            " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) ON (1 = 2)" +
                                            " where CAST(c02cmp AS nvarchar) =" + campaignCode.Text.Trim() + "" +
                                            " union" +
                                            " select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins, c07cmp,c07csq,c07pit,c07min,c07max, t44lty,CAST(t44itm AS nvarchar) as t44itm,t44itm as t44itm1,t44typ,t44brd,t44cod,t44mdl,t42des,case when c04pcd = 0 then t40des else t41des end as t41des, t43des,t44des,0 as row,'" + gRange + "' as Range, 'B' as Min_Seq" +
                                            " from AS400DB01.ILOD0001.ilcp02  WITH (NOLOCK)" +
                                            " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) ON (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "')" +
                                            " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) ON (c04cmp = c07cmp)" +
                                            " left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK) ON (c04pty = t40typ)" +
                                            " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) ON (c04pty = t41typ and c04pcd = t41cod)" +
                                            " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) ON (1 = 2)" +
                                            " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) ON (1 = 2)" +
                                            " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) ON (1 = 2)" +
                                            " where CAST(c02cmp AS nvarchar) =" + campaignCode.Text.Trim() + " order by c07pit " + gOrderBy + "  ";
                                    }
                                }
                            }
                            else
                            {
                                SqlAll = "select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,CAST(t44itm AS nvarchar) as t44itm,t44itm as t44itm1,t44typ,t44brd,t44cod,t44mdl,t42des,t41des,t43des,t44des,1 as row,'" + gRange + "' as Range, 'B' as Min_Seq" +
                                    " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                    " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "') " +
                                    " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on (c04cmp = c07cmp)" +
                                    " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on (1 = 2)" +
                                    " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on (1 = 2)" +
                                    " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on (1 = 2)" +
                                    " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on (1 = 2)" +
                                    " where CAST(c02cmp AS nvarchar) = " + campaignCode.Text.Trim() + "" +
                                    " union" +
                                    " select c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,CAST(t44itm AS nvarchar) as t44itm,t44itm as t44itm1,t44typ,t44brd,t44cod,t44mdl,t42des,case when c04pcd = 0 then t40des else t41des end as t41des, t43des, t44des,3 as row,'" + gRange + "' as Range, 'B' as Min_Seq " +
                                    " from AS400DB01.ILOD0001.ilcp02 WITH (NOLOCK) " +
                                    " left join AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "')" +
                                    " left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) on (c04cmp= c07cmp)" +
                                    " left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK) on (c04pty= t40typ)" +
                                    " left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK) on (c04pty= t41typ and c04pcd = t41cod)" +
                                    " left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on (1 = 2)" +
                                    " left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK) on (1 = 2)" +
                                    " left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK) on (1 = 2)" +
                                    " where CAST(c02cmp AS nvarchar) = " + campaignCode.Text.Trim() + " order by c07pit " + gOrderBy + "  ";
                            }
                            cmd.CommandText = SqlAll;
                            try
                            {
                                bool transaction = dataCenter.Sqltr == null ? true : false;
                                int vendorListCampaign = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                                if (vendorListCampaign == -1)
                                {
                                    dataCenter.RollbackMssql();
                                    dataCenter.CloseConnectSQL();

                                    Msg = "Please check command query !\r\n 'List Campaign Table' ";
                                    MsgHText = "Error Query";
                                    SET_MSG();

                                    return;
                                }
                                dataCenter.CommitMssql();
                                cmd.Parameters.Clear();
                            }
                            catch (Exception)
                            {
                                dataCenter.RollbackMssql();
                                dataCenter.CloseConnectSQL();
                                return;
                            }
                            
                            ilObj.UserInfomation = m_userInfo;
                            dsCampaignList = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                            drAllProductMoreRate = dsAllProductMoreRate.Tables[0]?.Rows[0];
                            DataTable dtListDataCampaign = new DataTable();
                            dtListDataCampaign.Columns.AddRange(new DataColumn[11] { new DataColumn("TA"),
                        new DataColumn("TB"),
                        new DataColumn("TC"),
                        new DataColumn("C07A"),
                        new DataColumn("C07B"),
                        new DataColumn("C02A"),
                        new DataColumn("C02B"),
                        new DataColumn("C02C"),
                        new DataColumn("C02D"),
                        new DataColumn("C02E"),
                        new DataColumn("C02F") });

                            brands = "All PRODUCT";
                            producCodes = "";
                            models = "";
                            minPrice = drAllProductMoreRate["C07MIN"].ToString();
                            maxPrice = drAllProductMoreRate["C07MAX"].ToString();
                            intRate = drAllProductMoreRate["C02INR"].ToString();
                            crrUsg = drAllProductMoreRate["C02CRR"].ToString();
                            infRate = drAllProductMoreRate["C02IFR"].ToString();
                            if (drAllProductMoreRate["RANGE"].ToString() == "N")
                            {

                                if (drAllProductMoreRate["MIN_SEQ"].ToString() == "B")
                                {
                                    termBegin = drAllProductMoreRate["C02TOT"].ToString();

                                }
                                else
                                {
                                    termBegin = drAllProductMoreRate["MIN_SEQ"].ToString();
                                }
                            }
                            else
                            {
                                termBegin = drAllProductMoreRate["C02FMT"].ToString();
                            }
                            termBegin = drAllProductMoreRate["MIN_SEQ"].ToString();
                            termEnd = drAllProductMoreRate["C02TOT"].ToString();
                            installs = drAllProductMoreRate["C02INS"].ToString();
                            dtListDataCampaign.Rows.Add(brands, producCodes, models, minPrice, maxPrice, intRate, crrUsg, infRate, termBegin, termEnd, installs);

                            foreach (DataRow drsCampaign in dsAllProductMoreRate.Tables[0]?.Rows)
                            {
                                brands = "Excep Product";
                                producCodes = "";
                                models = drsCampaign["T41DES"].ToString();
                                minPrice = "";
                                maxPrice = "";
                                intRate = "";
                                crrUsg = "";
                                infRate = "";
                                termBegin = "";
                                termEnd = "";
                                installs = "";
                                dtListDataCampaign.Rows.Add(brands, producCodes, models, minPrice, maxPrice, intRate, crrUsg, infRate, termBegin, termEnd, installs);
                            }
                            for (int i = 1; i < dsCampaignList.Tables[0]?.Rows.Count; i++)
                            {
                                DataRow dr = dsCampaignList.Tables[0]?.Rows[i];
                                dr.Delete();
                            }
                            dsCampaignList.Tables[0]?.AcceptChanges();
                            campaignStorage.SetCookiesDataSetByName("ds_gvListDataCampaign", dsCampaignList);
                            gvListDataCampaign.DataSource = dtListDataCampaign;
                            gvListDataCampaign.DataBind();
                            dataCenter.CloseConnectSQL();
                        }
                    }
                }
                else
                {
                    //minSeq = "B";
                    //SqlAll = "select distinct c02cmp,c02csq,c02rsq,c02inr,c02crr,c02ifr,c02fmt,c02tot,c02ins,c07cmp,c07csq,c07pit,c07min,c07max,t44lty,t44itm,digits(t44itm) as t44itm1,t44typ,t44brd,t44cod,t44mdl,t42des,t41des,t43des,t44des,4 as row,'" + gRange + "' as Range,'B' as Min_Seq" +
                    //    " from ilcp02" +
                    //    " left join ilcp07 on (c07cmp = c02cmp and c07csq = c02csq and c07lnt = '" + gLoanType + "') " +
                    //    " left join iltb44 on(t44lty= c07lnt and t44itm = c07pit)" +
                    //    " left join iltb42 on(t44brd= t42brd) " +
                    //    " left join iltb41 on(t44lty= t41lty and t44typ = t41typ and t44cod = t41cod)" +
                    //    " left join iltb43 on(t44lty= t43lty and t44typ = t43typ and t44brd = t43brd and t44cod = t43cod and t44mdl = t43mdl)" +
                    //    " where c02cmp= " + campaignCode.Text.Trim() +""+
                    //    " order by c02csq,t44brd,t44cod,t44mdl " + gOrderBy + "  ";

                }
            }
            else
            {
                campaignStorage.ClearCookies("ds_gvListDataCampaign");
                DataTable dtListDataCampaign = new DataTable();
                dtListDataCampaign.Columns.AddRange(new DataColumn[11] { new DataColumn("TA"),
                new DataColumn("TB"),
                new DataColumn("TC"),
                new DataColumn("C07A"),
                new DataColumn("C07B"),
                new DataColumn("C02A"),
                new DataColumn("C02B"),
                new DataColumn("C02C"),
                new DataColumn("C02D"),
                new DataColumn("C02E"),
                new DataColumn("C02F") });

                campaignStorage.SetCookiesDataTableByName("ds_gvListDataCampaign", dtListDataCampaign);
                gvListDataCampaign.DataSource = dtListDataCampaign;
                gvListDataCampaign.DataBind();

            }
        }
        protected void GV_LIST_CAMPAIGN_ROWDATABOUND(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvListDataCampaign = e.Row;
                double minPrices = double.Parse(gvListDataCampaign.Cells[3].Text.ToString().Trim());
                double maxPrices = double.Parse(gvListDataCampaign.Cells[4].Text.ToString().Trim());
                double intRates = double.Parse(gvListDataCampaign.Cells[5].Text.ToString().Trim());
                double crrUsgs = double.Parse(gvListDataCampaign.Cells[6].Text.ToString().Trim());
                double infRates = double.Parse(gvListDataCampaign.Cells[7].Text.ToString().Trim());
                double installss = double.Parse(gvListDataCampaign.Cells[10].Text.ToString().Trim());
                gvListDataCampaign.Cells[3].Text = minPrices.ToString("#,##0.00").Trim();
                gvListDataCampaign.Cells[4].Text = maxPrices.ToString("#,##0.00").Trim();
                gvListDataCampaign.Cells[5].Text = intRates.ToString("#0.00").Trim();
                gvListDataCampaign.Cells[6].Text = crrUsgs.ToString("#0.00").Trim();
                gvListDataCampaign.Cells[7].Text = infRates.ToString("#0.00").Trim();
                gvListDataCampaign.Cells[10].Text = installss.ToString("#,##0.00").Trim();
            }
            catch (Exception)
            { }
        }
        #endregion

        private void POPUP_CAMPAIGN()
        {
            string sqlwhere = "";
            if (campaignCode.Text != "")
            {
                SqlAll = "Select cast(c01cmp as varchar(14)) as c01cmp,c01cnm,c01sty,c01vdc,c01mkc,cast(c01sdt as varchar(8)) as c01sdt ,cast(c01edt as varchar(8)) as c01edt,cast(c01cad as varchar(8)) as c01cad,cast(c01cld as varchar(8)) as c01cld ,trim(CAST(c01srt AS nvarchar)) as c01srt,c01nxd,c01lty,p10nam,t46enm,user as user_login," + m_UpdDate + " as Cur_Date,c01rng " +
                         " From AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK) " +
                         " left join AS400DB01.ILOD0001.ilcp05 WITH (NOLOCK) as a on(c01cmp = a.c05cmp and a.c05csq = 1 and a.c05rsq = 1 and a.c05par = 'M') " +
                         " left join AS400DB01.ILOD0001.ilcp05 WITH (NOLOCK) as b on(c01cmp = b.c05cmp and b.c05csq = 1 and b.c05rsq = 1 and b.c05par = 'V') " +
                         " left join AS400DB01.ILOD0001.iltb46 WITH (NOLOCK) on(a.c05pcd= t46mak) " +
                         " left join AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) on(b.c05pcd= p10ven) " +
                         " where c01cmp like '" + campaignCode.Text.Trim() + "%'";
            }
            else
            {
                SqlAll = "Select cast(c01cmp as varchar(14)) as c01cmp,c01cnm,c01sty,c01vdc,c01mkc,cast(c01sdt as varchar(8)) as c01sdt ,cast(c01edt as varchar(8)) as c01edt,cast(c01cad as varchar(8)) as c01cad,cast(c01cld as varchar(8)) as c01cld ,trim(CAST(c01srt AS nvarchar)) as c01srt,c01nxd,c01lty,p10nam,t46enm,user as user_login," + m_UpdDate + " as Cur_Date,c01rng " +
                        " From AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK) " +
                         " left join AS400DB01.ILOD0001.ilcp05 WITH (NOLOCK) as a on(c01cmp = a.c05cmp and a.c05csq = 1 and a.c05rsq = 1 and a.c05par = 'M') " +
                         " left join AS400DB01.ILOD0001.ilcp05 WITH (NOLOCK) as b on(c01cmp = b.c05cmp and b.c05csq = 1 and b.c05rsq = 1 and b.c05par = 'V') " +
                         " left join AS400DB01.ILOD0001.iltb46 WITH (NOLOCK) on(a.c05pcd= t46mak) " +
                         " left join AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) on(b.c05pcd= p10ven) " +
                         " where c01cmp like '" + campaignCode.Text.Trim() + "%'";

            }
            if (ddlSearchCampaign.SelectedValue == "CCP" && txtSearchCampaign.Text.Trim() != "")
            {
                sqlwhere += " AND C01CMP = " + txtSearchCampaign.Text.Trim() + " ORDER BY C01CMP ";
            }
            else if (ddlSearchCampaign.SelectedValue == "DCP" && txtSearchCampaign.Text.Trim() != "")
            {
                sqlwhere += " AND C01CNM like '" + txtSearchCampaign.Text.ToUpper().Trim() + "%' ORDER BY C01CMP ";
            }
            else
            {
                sqlwhere += " ORDER BY C01CMP ";
            }
            SqlAll = SqlAll + sqlwhere;

            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (campaignStorage.check_dataset(DS))
            {
                ds_gvCampaign.Value = campaignStorage.JsonSerializeObjectHiddenDataSet(DS);
                gv_Campaign.DataSource = DS;
                gv_Campaign.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
        }
        #endregion

        #region enable false button
        private void ENABLE_DEFAULT()
        {
            campaignDes.Enabled = false;
            txtType.Enabled = false;
            txtRate.Enabled = false;
            txtSubsidize.Enabled = false;
            startDate.Enabled = false;
            endDate.Enabled = false;
            closeAppDate.Enabled = false;
            closeLayDate.Enabled = false;

            //campaignCode.Text = "";
            campaignDes.Text = "";
            txtType.Text = "";
            txtRate.Text = "";
            startDate.Text = "";
            closeAppDate.Text = "";
            txtSubsidize.Text = "";
            endDate.Text = "";
            closeLayDate.Text = "";
        }
        #endregion

        #region clear data list campaign
        private void CLEAR_LIST_DATA()
        {
            ///table Note
            campaignStorage.ClearCookies("ds_gvListNote");
            DataTable dtListData = new DataTable();
            dtListData.Columns.AddRange(new DataColumn[4] { new DataColumn("C11UDT"),
                new DataColumn("C11UTM"),
                new DataColumn("C11NSQ"),
                new DataColumn("C11NOT") });

            campaignStorage.SetCookiesDataTableByName("ds_gvListNote", dtListData);
            gvListData.DataSource = dtListData;
            gvListData.DataBind();

            ///table Vendor
            campaignStorage.ClearCookies("ds_gvListDataVendor");
            DataTable dtListDataVendor = new DataTable();
            dtListDataVendor.Columns.AddRange(new DataColumn[7] { new DataColumn("C08VEN1"),
                new DataColumn("P10NAM"),
                new DataColumn("P16RNK"),
                new DataColumn("P12WOR"),
                new DataColumn("P12ODR"),
                new DataColumn("P10EDT"),
                new DataColumn("C81END") });

            campaignStorage.SetCookiesDataTableByName("ds_gvListDataVendor", dtListDataVendor);
            gvListDataVendor.DataSource = dtListDataVendor;
            gvListDataVendor.DataBind();

            ///table Campaign
            campaignStorage.ClearCookies("ds_gvListDataCampaign");
            DataTable dtListDataCampaign = new DataTable();
            dtListDataCampaign.Columns.AddRange(new DataColumn[11] { new DataColumn("TA"),
                new DataColumn("TB"),
                new DataColumn("TC"),
                new DataColumn("C07A"),
                new DataColumn("C07B"),
                new DataColumn("C02A"),
                new DataColumn("C02B"),
                new DataColumn("C02C"),
                new DataColumn("C02D"),
                new DataColumn("C02E"),
                new DataColumn("C02F") });

            campaignStorage.SetCookiesDataTableByName("ds_gvListDataCampaign", dtListDataCampaign);
            gvListDataCampaign.DataSource = dtListDataCampaign;
            gvListDataCampaign.DataBind();
        }
        #endregion

        
    }
}