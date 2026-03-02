using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem.ManageData.WorkProcess.Process.FOB
{
    public partial class FOB_ILCredit : System.Web.UI.Page
    {
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));

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
                txt_date_from.Text = DateNow;
                txt_date_to.Text = DateNow;
                txt_time_from.Text = "000000";
                txt_time_to.Text = "230000";
                bind_branch();
                txt_vendor.Enabled = false;
                txt_application.Enabled = false;
                dd_brn.Enabled = false;
            }
        }

        protected void btnConfirmSave_Click(object sender, EventArgs e)
        {

            /* Call ILG002CL Generate Data Fax Outbound */
            UserInfo m_userInfo = userInfoService.GetUserInfo();
            ILDataCenter busobj = new ILDataCenter();
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_userInfo);
            EB_Service.DAL.DataCenter dataCenter = new EB_Service.DAL.DataCenter(m_userInfo);
            busobj.UserInfomation = m_userInfo;
            // 22/05/2561
            string start_date = txt_date_from.Text.Substring(6, 4) + txt_date_from.Text.Substring(3, 2) + txt_date_from.Text.Substring(0, 2);
            string end_date = txt_date_to.Text.Substring(6, 4) + txt_date_to.Text.Substring(3, 2) + txt_date_to.Text.Substring(0, 2);

            // Branch
            string Branch = "";
            if (dd_brn.SelectedItem.Value.ToString() == "")
            {
                Branch = "ALL";
            }
            else
            {
                Branch = dd_brn.SelectedItem.Value.ToString().PadLeft(3, '0');
            }

            // VENDOR
            string Vendor = "";
            if (txt_vendor.Text == "")
            {
                Vendor = "ALL";
            }
            else
            {
                Vendor = txt_vendor.Text.PadLeft(12, '0');
            }

            string appid = "";
            if (txt_application.Text.Trim() == "")
            {
                appid = "";
            }
            else
            {
                appid = txt_application.Text.Trim() + ",";
            }
            string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetFaxILCreditReport] 
                            N'{Branch}',
                            {start_date},
                            {end_date},
                            {txt_time_from.Text},
                            {txt_time_to.Text},
                            N'{m_userInfo.Username}',
                            N'{Vendor}',
                            N'{dd_typefax.SelectedValue.ToString()}',
                            N'{appid}',
                            N'{dd_business.SelectedValue.ToString()}',
                            N'{m_userInfo.LocalClient.ToString()}',
                            N'{m_userInfo.Username.ToString()}'";
            int afrows = dataCenter.Execute(sql, CommandType.Text).Result.afrows;
            //bool success = iLDataSubroutine.Call_ILG002CL(Branch, start_date, end_date, txt_time_from.Text, txt_time_to.Text, m_userInfo.Username,
            //                                    Vendor, dd_typefax.SelectedValue.ToString(), appid, dd_business.SelectedValue.ToString(),
            //                                    m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
            
            if (afrows >= 0)
            {
                /**/
                Session["start_date"] = txt_date_from.Text;
                Session["end_date"] = txt_date_to.Text;
                //Gen Crystal Report
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script language='javascript'>");

                sb.Append(@"window.open('PreviewFOB.aspx', 'home', 'width=600,height=500,toolbar=1,location=1,directories=1,status=1,menuBar=1,scrollBars=1,resizable=1,left=50,top=20');");
                sb.Append(@"</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", sb.ToString(), false);

            }


        }

        private bool ValidateData(ref string ErrMsg)
        {

            /* Check Date */
            if (txt_date_from.Text == "" || txt_date_to.Text == "")
            {
                ErrMsg = " Please input Date From and Date To before Print FAX ";
                return false;
            }

            if (!compareDate(txt_date_from.Text, txt_date_to.Text))
            {
                ErrMsg = " Date From must less than Date To  ";
                return false;
            }

            if (!validateDate(txt_date_from.Text.Trim()) || !validateDate(txt_date_to.Text.Trim()))
            {
                ErrMsg = " Format date invalid ";
                return false;
            }

            /* Check Time */
            if (txt_time_from.Text == "" || txt_time_to.Text == "")
            {
                ErrMsg = " Please input Time From and Time To before Print FAX ";
                return false;
            }

            if (Convert.ToInt32(txt_time_from.Text) > Convert.ToInt32(txt_time_to.Text))
            {
                ErrMsg = " Time From must less than Time To  ";
                return false;
            }

            //if (!validateTime(txt_time_from.Text.Trim()) || !validateTime(txt_time_to.Text.Trim()))
            //{
            //    ErrMsg = " Format Time invalid ";
            //    return false;
            //}

            /* check special */
            if (chkSpecial.Checked == true)
            {
                if (txt_vendor.Text == "")
                {
                    ErrMsg = " Please input vendor for send FAX ";
                    return false;
                }

                if (dd_brn.SelectedItem.Value == "")
                {
                    ErrMsg = " Please Select Branch for send FAX";
                    return false;
                }
            }

            return true;
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

        //private bool validateTime(string Time)
        //{
        //    string[] formats = { "HHmmss"};
        //    DateTime expectedDate;

        //    if (!DateTime.TryParseExact(Time, formats, new CultureInfo("th-TH"),
        //                                    DateTimeStyles.None, out expectedDate))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        private void bind_branch()
        {
            try
            {
                UserInfo userInfo = userInfoService.GetUserInfo();
                ILDataCenter ilObj = new ILDataCenter();
                ILDataCenterMssql dataCenterMssql = new ILDataCenterMssql(userInfo);
                DataSet ds = new DataSet();
                if (Cache["dsBranch"] != null)
                {
                    ds = (DataSet)Cache["dsBranch"];
                }
                else
                {
                    ds = dataCenterMssql.getILTB01();
                    Cache["dsBranch"] = ds;
                    Cache.Insert("dsBranch", ds, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }

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

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            string DateNow = DateTime.Now.ToString("dd/MM/yyyy", m_DThai);
            txt_date_from.Text = DateNow;
            txt_date_to.Text = DateNow;
            txt_time_from.Text = "000000";
            txt_time_to.Text = "230000";
            bind_branch();
            txt_vendor.Enabled = false;
            txt_application.Enabled = false;
            dd_brn.Enabled = false;
            chkSpecial.Checked = false;
            dd_typefax.SelectedIndex = -1;
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string error = "";

            if (!ValidateData(ref error))
            {
                lblMsgEN.Text = error;
                PopupMsg.ShowOnPageLoad = true;
                return;
            }
            else
            {
                lblConfirmMsgEN.Text = " Do you want to Print Report ?";
                PopupConfirmSave.ShowOnPageLoad = true;
                return;
            }
        }
        protected void chkSpecial_CheckedChanged(object sender, EventArgs e)
        {
            txt_vendor.Enabled = chkSpecial.Checked;
            txt_application.Enabled = chkSpecial.Checked;
            dd_brn.Enabled = chkSpecial.Checked;
            if (!chkSpecial.Checked)
            {
                txt_vendor.Text = "";
                txt_application.Text = "";
                dd_brn.SelectedIndex = 0;
            }
        }

    }
}