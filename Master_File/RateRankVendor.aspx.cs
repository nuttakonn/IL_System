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

namespace ILSystem.ManageData.WorkProcess.Master_File
{
    public partial class RateRankVendor : System.Web.UI.Page
    {
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        protected ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        protected ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        protected ILDataCenter ilObj = new ILDataCenter();
        protected DataCenter dataCenter;
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        protected string SqlAll = "";
        protected string SqlAll2 = "";
        protected string RankCode = "";
        protected string TCL = "";
        protected string tmpStartDate = "";
        protected string tmpEndDate = "";
        protected string Regulations = "";
        protected string UpRank = "";
        protected string CMsg = "";
        protected string CHText = "";
        protected string Msg = "";
        protected string MsgHText = "";
        protected string newFormatDate = "";
        protected string Pk1 = "";
        protected string Pk2 = "";
        protected string Pk3 = "";
        public CookiesStorage cookiesStorage;

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
            setEndDate.Text = "99/99/9999";
            if (Page.IsPostBack)
            {
                return;
            }
            if (!IsPostBack)
            {
                Search_Data();
            }
            //SetDefault();
            PopupMsgCenter();
            txtSqlAll.Text = "";
            txtSqlAll2.Text = "";
        }

        #region Default 
        private void set_Enabled(bool type)
        {
            //btnAdd.Enabled = type;
            //btnClearData.Enabled = type;
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
            PopupConfirmSave.HeaderText = CHText;
            PopupConfirmSave.ShowOnPageLoad = true;
        }
        private void set_clear_search()
        {
            E_error.Text = "";
            ddl_SearchBy.SelectedIndex = 0;
            txt_Brand.Text = "";
        }
        private void set_default_edit()
        {


            txtRank.Text = RankCode;
            txtTCL.Text = TCL;
            //setStartDate.Text = tmpStartDate;
            startDate.Text = tmpStartDate;
            setEndDate.Text = tmpEndDate;
            setStartDate.Enabled = true;
            startDate.Enabled = true;
            txtRank.Enabled = false;
            DateTime dts = Convert.ToDateTime(tmpStartDate);
            CalendarStartDate.SelectedDate = dts;

            pk1s.Text = Pk1;
            pk2s.Text = Pk2;
            pk3s.Text = Pk3;

            botRegulation.SelectedValue = botRegulation.Items.FindByValue(Regulations).Value;
            ddlupRank.SelectedValue = ddlupRank.Items.FindByValue(UpRank).Value;
            btnAdd.Text = "  Edit  ";
            lbl_addRank.Text = "Edit Ranking";
            lbl_addRank.Focus();
        }
        private void set_clear_add_edit()
        {


            if (Msg == "PleaseCheck" && MsgHText == "ValidateSave")
            {

                PopupMsg.HeaderText = "Validate";
                lblMsg.Text = "Can not record data, please check date !";

            }
            else if (btnAdd.Text.Trim() == "Add")
            {
                PopupMsg.HeaderText = "Add";
                lblMsg.Text = "Add rank complete";

            }
            else if (btnAdd.Text.Trim() == "Edit")
            {
                PopupMsg.HeaderText = "Edit";
                lblMsg.Text = "Edit rank complete";
            }

            txtTCL.Text = "";
            txtRank.Text = "";
            //setStartDate.Text = "";

            btnAdd.Text = " Add ";
            lbl_addRank.Text = "Add Ranking";
            lbl_addRank.Focus();
        }
        #endregion

        #region clear data
        //search
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            set_clear_search();
            Search_Data();
            //GridView2.DataSource = null;
            //GridView2.DataBind();
        }

        //add/edit
        protected void btnClearData_Click(object sender, EventArgs e)
        {
            //set_clear_add_edit();
            btnAdd.Enabled = true;
            txtRank.Enabled = true;
            txtTCL.Enabled = true;

            botRegulation.Enabled = true;
            ddlupRank.Enabled = true;
            setStartDate.Enabled = true;
            startDate.Enabled = true;
            CalendarStartDate.Enabled = true;

            btnAdd.Text = "  Add  ";
            lbl_addRank.Text = "Add Ranking";

            botRegulation.SelectedValue = botRegulation.Items.FindByValue("N").Value;
            ddlupRank.SelectedValue = ddlupRank.Items.FindByValue("N").Value;
            txtTCL.Text = "";
            txtRank.Text = "";
            //setStartDate.Text = "";
            startDate.Text = "";
            DateTime dts = DateTime.Now;
            CalendarStartDate.SelectedDate = dts;
            Search_Data();
            lbl_addRank.Focus();

        }
        #endregion

        #region Bind Data and search
        protected void btn_search_Click(object sender, EventArgs e)
        {
            Search_Data();
        }
        void Search_Data()
        {
            E_error.Text = "";
            string sqlwhere = "";
            if (ddl_SearchBy.SelectedValue == "RK" && txt_Brand.Text.Trim() != "")
            {
                sqlwhere += "AND CAST(T12RNK as nvarchar) = '" + txt_Brand.Text.Trim() + "' ";
            }
            if (ddl_SearchBy.SelectedValue == "TCL" && txt_Brand.Text.Trim() != "")
            {
                sqlwhere += " AND CAST(T12RTE as nvarchar) = '" + txt_Brand.Text.ToUpper().Trim() + "' ";
            }

            SqlAll = $@"SELECT T12RNK, T12RTE, 
                       SUBSTRING(CAST(T12STD AS NVARCHAR), 7, 2) + '/' + SUBSTRING(CAST(T12STD AS NVARCHAR), 5, 2) + '/' + SUBSTRING(CAST(T12STD AS NVARCHAR), 1, 4) AS T12STD, 
                       SUBSTRING(CAST(T12END AS NVARCHAR), 7, 2) + '/' + SUBSTRING(CAST(T12END AS NVARCHAR), 5, 2) + '/' + SUBSTRING(CAST(T12END AS NVARCHAR), 1, 4) AS T12END, 
                        T12BOT, T12OUT, T12UPD, T12UPT FROM AS400DB01.ILOD0001.ILTB12 WITH (NOLOCK) 
                        WHERE T12STS = '' ";
            SqlAll = SqlAll + sqlwhere + "  ORDER BY T12RNK ASC";
            DataSet DS = new DataSet();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;
            if (cookiesStorage.check_dataset(DS))
            {
                ds_Hiddengrid.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(DS);
                GridView2.SelectedIndex = -1;
                GridView2.DataSource = DS;
                GridView2.DataBind();
            }
            else
            {
                dataCenter.CloseConnectSQL();
                return;
            }
            dataCenter.CloseConnectSQL();
            ResetGrid(GridView2, ds_Hiddengrid.Value);

        }
        #endregion

        #region select page
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.SelectedIndex = -1;
            GridView2.PageIndex = e.NewPageIndex;
            GridView2.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
            GridView2.DataBind();
        }
        #endregion

        #region select data
        protected void GridView2_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
            DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.NewSelectedIndex];

            RankCode = dr[0].ToString().Trim();
            TCL = dr[1].ToString().Trim();
            tmpStartDate = dr[2].ToString().Trim();
            tmpEndDate = dr[3].ToString().Trim();
            Regulations = dr[4].ToString().Trim();
            UpRank = dr[5].ToString().Trim();
            Pk1 = dr[2].ToString().Trim();
            Pk2 = dr[6].ToString().Trim();
            Pk3 = dr[7].ToString().Trim();

            //DateTime dtime;
            //if (DateTime.TryParseExact(tmpStartDate.ToString(), "yyyyMMdd",
            //                          CultureInfo.InvariantCulture,
            //                          DateTimeStyles.None, out dtime))
            //{
            //    Console.WriteLine(dtime);
            //}
            //string formattedDateTime = dtime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

            //newFormatDate = DateTime.ParseExact(formattedDateTime, "yyyy/MM/dd", CultureInfo.InvariantCulture)
            //.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (tmpEndDate != "99/99/9999")
            {
                txtRank.Text = RankCode;
                txtTCL.Text = TCL;
                //setStartDate.Text = tmpStartDate;
                startDate.Text = tmpStartDate;
                setEndDate.Text = tmpEndDate;
                btnAdd.Text = "Edit";
                btnAdd.Enabled = false;
                txtRank.Enabled = false;
                txtTCL.Enabled = false;
                setStartDate.Enabled = false;
                setEndDate.Enabled = false;
                botRegulation.Enabled = false;
                ddlupRank.Enabled = false;
                //DateTime dt = Convert.ToDateTime(tmpStartDate);
                //CalendaStartDate.SelectedDate = dt;
                CalendarStartDate.Enabled = false;
                startDate.Enabled = false;
            }
            else
            {

                txtTCL.Enabled = true;
                botRegulation.Enabled = true;
                ddlupRank.Enabled = true;
                btnAdd.Enabled = true;
                CalendarStartDate.Enabled = true;
                set_default_edit();
            }

            ResetGrid(GridView2, ds_Hiddengrid.Value);
        }

        private void ResetGrid(GridView GridView, string ds)
        {
            GridView.PageIndex = 0;
            GridView.DataSource = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds);
            GridView.DataBind();
        }

        #endregion

        #region insert and update data
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string txtTCLs = txtTCL.Text.ToUpper().Trim();
            string txtRanks = txtRank.Text.Trim();
            string StartDate = startDate.Text.Trim();
            string btnAdds = btnAdd.Text.Trim();


            if (txtRanks == "")
            {

                Msg = "Please input rank name  ";
                MsgHText = "Validate Save";
                set_Msg();


            }
            else if (txtRanks.Length >= 3)
            {
                Msg = "Please input rank name no more than 2 characters";
                MsgHText = "Validate Save";
                set_Msg();

            }
            else if (txtTCLs == "")
            {
                Msg = "Please input % of TCL";
                MsgHText = "Validate Save";
                set_Msg();

            }
            else if (StartDate == "")
            {
                Msg = "Please input start date  ";
                MsgHText = "Validate Save";
                set_Msg();


            }

            if (btnAdd.Text.Trim() == "Add" && txtRanks != "" && txtRanks.Length < 3 && txtTCLs != "" && StartDate != "")
            {
                CHText = "Confirm Save";
                CMsg = "Confirm Save Rank Data";
                set_confirmMsg();
            }
            else if (btnAdd.Text.Trim() == "Edit" && txtRanks != "" && txtRanks.Length < 3 && txtTCLs != "" && StartDate != "")
            {
                CHText = "Confirm Edit";
                CMsg = "Confirm Edit Rank Data";
                set_confirmMsg();

            }

        }

        #endregion

        #region Confirm Add/Edit
        protected void btnConfirmOK_Click(object sender, EventArgs e)
        {
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            string txtTCLs = txtTCL.Text.ToUpper().Trim();
            string txtRanks = txtRank.Text.Trim().ToUpper();
            string StartDate = startDate.Text.Trim();
            string[] SplitDate = StartDate.Split('/');
            string StartDateNew = SplitDate[2] + SplitDate[1] + SplitDate[0];
            string EndDateNew = "99999999";
            string botRegulations = botRegulation.Text.ToUpper().Trim();
            string upRanks = ddlupRank.Text.Trim();
            string btnAdds = btnAdd.Text.Trim();
            DataRow drs = null;
            bool transaction = dataCenter.Sqltr == null ? true : false;

            if (btnAdd.Text.Trim() == "Add")
            {
                SqlAll = "SELECT T12RNK,T12STD, T12END FROM AS400DB01.ILOD0001.ILTB12 WITH (NOLOCK) WHERE T12STS ='' AND " +
                    "T12RNK = '" + txtRanks + "' AND T12END = '" + EndDateNew + "' AND T12STS = ''";
                DataSet DS = new DataSet();

                
                ilObj.UserInfomation = m_userInfo;
                DS = dataCenter.GetDataset<DataTable>(SqlAll, CommandType.Text).Result.data;

                if (cookiesStorage.check_dataset(DS))
                {
                    drs = DS.Tables[0]?.Rows[0];
                    string checkDate = drs[1].ToString();

                    int intStartDateNew = Int32.Parse(StartDateNew);
                    int checkDates = Int32.Parse(checkDate);

                    if (intStartDateNew < checkDates)
                    {
                        Msg = "PleaseCheck";
                        MsgHText = "ValidateSave";
                        set_Msg();
                    }
                    else if (drs[0].ToString().Trim() != "")
                    {
                        DateTime a = DateTime.Now.AddDays(-5);


                        DateTime dtime;
                        if (DateTime.TryParseExact(StartDateNew.ToString(), "yyyyMMdd",
                                                  CultureInfo.InvariantCulture,
                                                  DateTimeStyles.None, out dtime))
                        {
                            Console.WriteLine(dtime);
                        }
                        string formattedDateTime = dtime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);


                        DateTime newFormatDates = Convert.ToDateTime(formattedDateTime).AddDays(-1);
                        string StartDateUpdate = newFormatDates.ToString("yyyyMMdd");


                        SqlAll = " UPDATE AS400DB01.ILOD0001.ILTB12 SET " +
                            "T12END = '" + StartDateUpdate + "'," +
                            "T12UPD = " + m_UpdDate + ", " +
                            "T12UPT = " + m_UpdTime + ", " +
                            "T12UPU = '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "' " +
                            "WHERE T12RNK = '" + txtRanks + "' AND T12STD = '" + drs[1].ToString() + "' AND T12END = '" + EndDateNew + "' AND T12STS = ''";



                        cmd.CommandText = SqlAll;
                        transaction = dataCenter.Sqltr == null ? true : false;
                        int updateILTB12 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                        SqlAll = " INSERT INTO AS400DB01.ILOD0001.ILTB12 (T12RNK, T12RTE, T12STD, T12END, T12BOT, T12OUT, T12FIL, T12UPD, T12UPT, T12UPU, T12STS) " +
                        "VALUES('" + txtRanks + "'," + txtTCLs + "," + StartDateNew + "," + EndDateNew + ",'" + botRegulations + "','" + upRanks + "', ''," + m_UpdDate + "," + m_UpdTime + ", '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "', ' ')";
                    }

                }
                else
                {
                    DateTime dtime;
                    if (DateTime.TryParseExact(StartDateNew.ToString(), "yyyyMMdd",
                                              CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out dtime))
                    {
                        Console.WriteLine(dtime);
                    }


                    cmd.CommandText = SqlAll;
                    transaction = dataCenter.Sqltr == null ? true : false;
                    int updateILTB12 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;

                    SqlAll = " INSERT INTO AS400DB01.ILOD0001.ILTB12 (T12RNK, T12RTE, T12STD, T12END, T12BOT, T12OUT, T12FIL, T12UPD, T12UPT, T12UPU, T12STS) " +
                    "VALUES('" + txtRanks + "'," + txtTCLs + "," + StartDateNew + "," + EndDateNew + ",'" + botRegulations + "','" + upRanks + "', ''," + m_UpdDate + "," + m_UpdTime + ", '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "', ' ')";
                }

            }

            else if (btnAdd.Text.Trim() == "Edit")
            {
                string[] Key1 = pk1s.Text.Trim().Split('/');
                string primaryKey1 = Key1[2] + Key1[1] + Key1[0];
                string primaryKey2 = pk2s.Text.Trim();
                string primaryKey3 = pk3s.Text.Trim();

                SqlAll = "UPDATE AS400DB01.ILOD0001.ILTB12 SET T12RNK = '" + txtRanks + "'"
                      + ",T12RTE = " + txtTCLs
                      + ",T12BOT = '" + botRegulations + "'"
                      + ",T12OUT = '" + upRanks + "'"
                      + ",T12UPD =  " + m_UpdDate
                      + ",T12UPT =  " + m_UpdTime
                      + ",T12UPU =  '" + m_userInfo.LocalClient.ToString().PadRight(10).Substring(0, 10).Trim() + "'"

                      + "  WHERE   T12STD = '" + primaryKey1 + "' AND T12UPD = '" + primaryKey2 + "' AND T12UPT = '" + primaryKey3 + "'";



            }


            cmd.CommandText = SqlAll;

            try
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resHome11 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();

                    Msg = btnAdd.Text + " Brand not complete.";
                    set_Msg();

                    return;
                }

                dataCenter.CommitMssql();
                cmd.Parameters.Clear();
                dataCenter.CloseConnectSQL();

                Msg = btnAdd.Text + " Brand  complete.";
                set_MsgSuccess();
                Search_Data();
                set_clear_add_edit();
            }
            catch (Exception ex)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();

                Msg = btnAdd.Text + " Brand  not complete";
                set_Msg();
                return;
            }
        }

        protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
            //txttxtTCLs.Focus();
        }
        #endregion

        #region delete data
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds_grid = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_Hiddengrid.Value);
            DataRow dr = ds_grid.Tables[0]?.Rows[(GridView2.PageIndex * Convert.ToInt16(GridView2.PageSize)) + e.RowIndex];
            RankCode = dr[0].ToString().Trim();
            TCL = dr[1].ToString().Trim();

            string[] sDate = dr[2].ToString().Trim().Split('/');
            string tmpStartDate = sDate[2] + sDate[1] + sDate[0];
            string[] eDate = dr[3].ToString().Trim().Split('/');
            string tmpEndDate = eDate[2] + eDate[1] + eDate[0];
            if (tmpEndDate != "99999999")
            {
                lblMsg.Text = "This record cannot be deleted";
                PopupMsg.ShowOnPageLoad = true;
            }
            else
            {
                if (validateDelete())
                {
                    m_userInfo = userInfoService.GetUserInfo();
                    dataCenter = new DataCenter(m_userInfo);


                    SqlAll = "UPDATE AS400DB01.ILOD0001.ILTB12 SET T12STS = 'X'"
                        + ", T12UPD = " + m_UpdDate
                        + ", T12UPT = " + m_UpdTime
                        + ", T12UPU =  '" + m_userInfo.Username.ToString() + "'"
                        + "  WHERE   T12RTE = " + TCL
                        + "  AND T12STS = ''"
                        + "  AND  T12RNK = '" + RankCode + "'"
                        + "  AND  T12STD = " + tmpStartDate
                        + "  AND  T12END = " + tmpEndDate;


                    txtSqlAll.Text = SqlAll;

                    SqlAll2 = "UPDATE AS400DB01.ILOD0001.ILTB12 SET T12END = '99999999'"
                     + ", T12UPD = " + m_UpdDate
                     + ", T12UPT = " + m_UpdTime
                     + ", T12UPU =  '" + m_userInfo.Username.ToString() + "'"
                     + "  WHERE T12STS = ''"
                     + "  AND  T12RNK = '" + RankCode + "'"
                     + "  AND  T12END = (SELECT MAX(T12END) AS ILTB12 FROM AS400DB01.ILOD0001.ILTB12 WITH (NOLOCK) WHERE T12STS = '' AND T12RNK = '" + RankCode + "')";
                    txtSqlAll2.Text = SqlAll2;

                    lblConfimMsg_Delete.Text = "Confirm Delete Brand Name : " + RankCode;
                    PopupConfirmDelete.ShowOnPageLoad = true;
                }
            }
            return;
        }

        private bool validateDelete()
        {

            return true;
        }
        #endregion

        #region Confirm Delete
        protected void btnConfirm_OK_Click(object sender, EventArgs e)
        {
            iDB2Command cmd = new iDB2Command();
            m_userInfo = userInfoService.GetUserInfo();
            
            ilObj.UserInfomation = m_userInfo;
            dataCenter = new DataCenter(m_userInfo);
            SqlAll = txtSqlAll.Text;
            cmd.CommandText = SqlAll;
            bool transaction = dataCenter.Sqltr == null ? true : false;

            try
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                int resHome11 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resHome11 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    Msg = "Delete Rank not complete.";
                    set_Msg();
                    return;
                }

                //set_clear_add_edit();
            }
            catch (Exception ex)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                Msg = "Delete Rank not complete.";
                set_Msg();
                return;
            }

            dataCenter.CommitMssql();
            cmd.Parameters.Clear();

            SqlAll2 = txtSqlAll2.Text;
            cmd.CommandText = SqlAll2;

            try
            {
                transaction = dataCenter.Sqltr == null ? true : false;
                int resUpdate99 = dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                if (resUpdate99 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    Msg = "Delete Rank not complete.";
                    set_Msg();
                    return;
                }

            }
            catch (Exception ex)
            {
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                Msg = "Delete Rank not complete.";
                set_Msg();
                return;
            }

            dataCenter.CommitMssql();
            cmd.Parameters.Clear();
            dataCenter.CloseConnectSQL();
            Msg = " Delete Rank complete.";
            set_MsgSuccess();
            Search_Data();
        }


        protected void btnConfirm_Cancel_Click(object sender, EventArgs e)
        {
            botRegulation.SelectedValue = botRegulation.Items.FindByValue("N").Value;
            ddlupRank.SelectedValue = ddlupRank.Items.FindByValue("N").Value;
            txtTCL.Text = "";
            txtRank.Text = "";
            startDate.Text = "";
            Search_Data();

        }
        #endregion

        #region Message all
        protected void btnOK_Click(object sender, EventArgs e)
        {
            botRegulation.SelectedValue = botRegulation.Items.FindByValue("N").Value;
            ddlupRank.SelectedValue = ddlupRank.Items.FindByValue("N").Value;
            txtTCL.Text = "";
            txtRank.Text = "";
            startDate.Text = "";
            Search_Data();

        }
        #endregion

        protected void btAddRank_OnClick(object sender, EventArgs e)
        {
            Popup_AddRank.ShowOnPageLoad = true;
        }
        

        private void setSessionRankAdd(string vendorCode)
        {
            DataTable dtold = cookiesStorage.JsonDeserializeObjectHiddenDataTable(ds_grid_rv.Value) as DataTable;
            foreach (DataRow dr in dtold.Rows)
            {
                if (vendorCode == dr["venDorC"].ToString())
                {
                    dr.Delete();
                    break;
                }

            }
            dtold.AcceptChanges();
            if (dtold != null && dtold.Rows.Count > 0)
                ds_grid_rv.Value = cookiesStorage.JsonSerializeObjectHiddenDataDataTable(dtold);

            dtold.Dispose();
        }
        private void setSessionRankUnAdd(string vendorCode)
        {
            DataTable dtold = cookiesStorage.JsonDeserializeObjectHiddenDataTable(ds_grid_rv_New.Value) as DataTable;
            foreach (DataRow dr in dtold.Rows)
            {
                if (vendorCode == dr["venDorC"].ToString())
                {
                    dr.Delete();
                    break;
                }

            }
            dtold.AcceptChanges();
            if (dtold != null && dtold.Rows.Count > 0)
                ds_grid_rv_New.Value = cookiesStorage.JsonSerializeObjectHiddenDataDataTable(dtold);
            dtold.Dispose();
        }
        protected void gvRank_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            txtRank.Text = gvRank.Rows[e.NewSelectedIndex].Cells[0].Text.ToString();
            Popup_AddRank.ShowOnPageLoad = false;
        }


        //protected void btEditVendor_Click(object sender, EventArgs e)
        //{
        //    popupEdit.ShowOnPageLoad = true;
        //}
        //protected void btnConfirm_Edit_OK_Click(object sender, EventArgs e)
        //{
        //    popupEdit.ShowOnPageLoad = false;
        //}
        //protected void btnConfirm_Edit_Cancel_Click(object sender, EventArgs e)
        //{
        //    popupEdit.ShowOnPageLoad = false;
        //}


        //protected void btDeleteVendor_Click(object sender, EventArgs e)
        //{
        //    int rowCheck = gvVendor.Rows.Count;
        //    int checkCount = 0;
        //    for (int i = 0; i < rowCheck; i++)
        //    {
        //        var checkBox = gvVendor.Rows[i].FindControl("CheckBox1") as CheckBox;
        //        if (checkBox.Checked)
        //        {
        //            checkCount++;
        //        }
        //    }
        //    if (checkCount == 1)
        //    {
        //        lblConfimMsg_Delete.Text = "Confirm Delete Vendor";
        //        PopupConfirmDelete.ShowOnPageLoad = true;
        //    }
        //}




        protected void btSave_Click(object sender, EventArgs e)
        {
            DataTable dt = cookiesStorage.JsonDeserializeObjectHiddenDataTable(ds_grid_rv_New.Value);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblConfimMsg_Delete.Text = "Confirm Save Ranking";
                PopupConfirmSave.ShowOnPageLoad = true;
            }
            //Insert data
        }
        protected void btnConfirm_Add_OK_Click(object sender, EventArgs e)
        {
            PopupConfirmSave.ShowOnPageLoad = false;
        }
        protected void btnConfirm_Add_Cancel_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            txtRank.Enabled = true;
            txtTCL.Enabled = true;
            setStartDate.Enabled = true;
            setEndDate.Enabled = true;
            botRegulation.Enabled = true;
            ddlupRank.Enabled = true;

            botRegulation.SelectedValue = botRegulation.Items.FindByValue("N").Value;
            ddlupRank.SelectedValue = ddlupRank.Items.FindByValue("N").Value;
            txtTCL.Text = "";
            txtRank.Text = "";
            startDate.Text = "";
            Search_Data();
            lbl_addRank.Focus();
        }

        private void PopupMsgCenter()
        {

            PopupMsg.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsg.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupMsgSuccess.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupMsgSuccess.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            Popup_AddRank.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            Popup_AddRank.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupConfirmDelete.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupConfirmDelete.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;
            PopupConfirmSave.PopupHorizontalAlign = DevExpress.Web.ASPxClasses.PopupHorizontalAlign.WindowCenter;
            PopupConfirmSave.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.WindowCenter;

        }
    }
}