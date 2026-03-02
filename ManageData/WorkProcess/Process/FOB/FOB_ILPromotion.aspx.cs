using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web.ASPxEditors;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem.ManageData.WorkProcess.Process.FOB
{
    public partial class FOB_ILPromotion : System.Web.UI.Page
    {
        public UserInfo m_userInfo;
        public UserInfoService userInfoService;
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        public CookiesStorage cookiesStorage;
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

            cookiesStorage = new CookiesStorage();
            if (!IsPostBack)
            {
                lblError.Text = "";
                rd_LetterType.SelectedValue = "0";
                dd_lettertype.SelectedIndex = -1;
                dd_lettertype.Enabled = true;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                dd_lettertype.Items.Add("Type1", "1");
                dd_lettertype.Items.Add("Type2", "2");
                dd_lettertype.Items.Add("Type4", "4");
                dd_campaign.SelectedIndex = -1;
                dd_campaign.Enabled = false;
                dd_campaign.Items.Clear();

                txtVoicher.Text = "";
                string DateNow = DateTime.Now.ToString("dd/MM/yyyy", m_DThai);
                txt_date.Text = DateNow;

            }
        }
        private void bind_campaign(string strlettertype)
        {
            try
            {
                lblError.Text = "";
                UserInfo userInfo =  userInfoService.GetUserInfo();
                ILDataCenter busobj = new ILDataCenter();
                ILDataCenterMssqlReport mssqlReport = new ILDataCenterMssqlReport(userInfo);
                DataSet ds = new DataSet();
                string errmsg = "";
                string Type = strlettertype;

                ds = mssqlReport.getFAXCampaign(Type, ref errmsg);


                dd_campaign.Enabled = true;
                dd_campaign.Items.Clear();

                dd_vendor.Enabled = false;
                dd_vendor.Items.Clear();
                dd_vendor.SelectedIndex = -1;

                grdProduct.PageIndex = 0;
                grdProduct.DataSource = null;
                //Session["ds_gridProduct"] = null;
                ds_gridProduct.Value = null;
                grdProduct.DataBind();

                if (busobj.check_dataset(ds))
                {
                    dd_campaign.Items.Add("--- Select ---", "");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        dd_campaign.Items.Add(
                            new ListEditItem(dr["c01cmp"].ToString().Trim() + " : " + dr["c01cnm"].ToString().Trim(), dr["c01cmp"].ToString().Trim()));
                    }

                    dd_campaign.SelectedIndex = -1;
                }
                else
                {
                    dd_campaign.Enabled = false;
                    dd_campaign.Items.Clear();
                    dd_campaign.SelectedIndex = -1;
                    lblError.Text = "Error : Not Found Campaign " + errmsg;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error :" + ex.Message;
            }
        }
        private void bind_product(string strCampaign, string strVendor)
        {
            try
            {
                UserInfo userInfo =  userInfoService.GetUserInfo();
                ILDataCenter busobj = new ILDataCenter();
                ILDataCenterMssqlReport mssqlReport = new ILDataCenterMssqlReport(userInfo);
                string errorMsg = "";
                DataSet ds = mssqlReport.getFAXProduct(strCampaign, strVendor, ref errorMsg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grdProduct.DataSource = ds;
                    //Session["ds_gridProduct"] = ds;
                    ds_gridProduct.Value = cookiesStorage.JsonSerializeObjectHiddenDataSet(ds);
                    grdProduct.DataBind();
                    HttpCookie CookiesCampaign = new HttpCookie("strCampaign", strCampaign);
                    CookiesCampaign.Expires = DateTime.Now.AddMinutes(720);
                    Response.Cookies.Add(CookiesCampaign);
                    HttpCookie CookiesVendor = new HttpCookie("strVendor", strVendor);
                    CookiesVendor.Expires = DateTime.Now.AddMinutes(720);
                    Response.Cookies.Add(CookiesVendor);
                    if (grdProduct.Rows.Count == 0)
                    {
                        lblError.Text = "Data not found";
                        mssqlReport._dataCenter.CloseConnectSQL();
                        return;
                    }
                }
                else
                {
                    lblError.Text = "Data not found";
                    mssqlReport._dataCenter.CloseConnectSQL();
                    return;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Error : " + ex.Message;
            }
        }
        private void bind_vendor(string strletterType, string strCampaign)
        {
            try
            {
                grdProduct.PageIndex = 0;
                grdProduct.DataSource = null;
                Session["ds_gridProduct"] = null;
                ds_gridProduct.Value = null;
                grdProduct.DataBind();
                lblError.Text = "";
                UserInfo userInfo =  userInfoService.GetUserInfo();
                ILDataCenter busobj = new ILDataCenter();
                ILDataCenterMssqlReport mssqlReport = new ILDataCenterMssqlReport(userInfo);

                DataSet ds = new DataSet();
                string errmsg = "";
                ds = mssqlReport.getFAXVendor(strletterType, strCampaign, ref errmsg);

                mssqlReport._dataCenter.CloseConnectSQL();
                ILDataCenter ilObj = new ILDataCenter();

                dd_vendor.Items.Clear();
                if (ilObj.check_dataset(ds))
                {
                    dd_vendor.Items.Add("--- Select ---", "");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        dd_vendor.Items.Add(
                            new ListEditItem(dr["c08ven"].ToString().Trim() + " : " + dr["p10tnm"].ToString().Trim(), dr["c08ven"].ToString().Trim()));
                    }

                    dd_vendor.SelectedIndex = -1;
                    dd_vendor.Enabled = true;
                }
                else
                {
                    dd_vendor.SelectedIndex = -1;
                    dd_vendor.Enabled = false;

                    lblError.Text = "Error : Not Found Vendor " + errmsg;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error : " + ex.Message;
            }
        }
        protected void btn_preview_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (rd_LetterType.SelectedValue != "3")
            {
                if (dd_lettertype.SelectedItem.Value == "")
                {
                    lblError.Text = "Please select Letter Type";
                    return;
                }
            }

            if (dd_campaign.SelectedItem.Value == "")
            {
                lblError.Text = "Please select Campaign";
                return;
            }

            if (dd_vendor.SelectedItem.Value == "")
            {
                lblError.Text = "Please select Vendor";
                return;
            }

            if (txt_date.Text == "")
            {
                lblError.Text = "Please select Promotion start date";
                return;
            }
            if (txtVoicher.Text == "")
            {
                txtVoicher.Text = "-";
            }

            Session["NOBAC"] = dd_vendor.SelectedIndex.ToString().PadLeft(4, '0') + "/" + DateTime.Now.ToString("yyyy", new CultureInfo("th-TH"));
            Session["GIFTVOUCHER"] = txtVoicher.Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script language='javascript'>");

            sb.Append(@"window.open('PreviewFOB_ILPromotion.aspx', 'home', 'width=600,height=500,toolbar=1,location=1,directories=1,status=1,menuBar=1,scrollBars=1,resizable=1,left=50,top=20');");
            sb.Append(@"</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", sb.ToString(), false);


        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            rd_LetterType.SelectedValue = "0";

            dd_lettertype.SelectedIndex = -1;
            dd_lettertype.Enabled = true;
            dd_lettertype.Items.Clear();
            dd_lettertype.Items.Add("--- Select ---", "");
            dd_lettertype.Items.Add("Type1", "1");
            dd_lettertype.Items.Add("Type2", "2");
            dd_lettertype.Items.Add("Type4", "4");

            dd_campaign.SelectedIndex = -1;
            dd_campaign.Enabled = false;
            dd_campaign.Items.Clear();

            dd_vendor.SelectedIndex = -1;
            dd_vendor.Enabled = false;
            dd_vendor.Items.Clear();

            grdProduct.PageIndex = 0;
            grdProduct.DataSource = null;
            Session["ds_gridProduct"] = null;
            ds_gridProduct.Value = null;
            grdProduct.DataBind();

            txtVoicher.Text = "";

            string DateNow = DateTime.Now.ToString("dd/MM/yyyy", m_DThai);
            txt_date.Text = DateNow;

            lblError.Text = "";

        }
        protected void rd_LetterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            dd_lettertype.SelectedIndex = -1;
            dd_campaign.SelectedIndex = -1;
            dd_campaign.Enabled = false;
            dd_campaign.Items.Clear();

            dd_vendor.SelectedIndex = -1;
            dd_vendor.Enabled = false;
            dd_vendor.Items.Clear();

            grdProduct.PageIndex = 0;
            grdProduct.DataSource = null;
            //Session["ds_gridProduct"] = null;
            ds_gridProduct.Value = null;
            grdProduct.DataBind();

            txtVoicher.Text = "";
            string DateNow = DateTime.Now.ToString("dd/MM/yyyy", m_DThai);
            txt_date.Text = DateNow;

            if (rd_LetterType.SelectedValue == "0")
            {
                dd_lettertype.Enabled = true;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                dd_lettertype.Items.Add("Type1", "1");
                dd_lettertype.Items.Add("Type2", "2");
                dd_lettertype.Items.Add("Type4", "4");
            }
            if (rd_LetterType.SelectedValue == "1")
            {
                dd_lettertype.Enabled = true;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                dd_lettertype.Items.Add("Type2", "2");
                dd_lettertype.Items.Add("Type3", "3");
            }
            if (rd_LetterType.SelectedValue == "2")
            {
                dd_lettertype.Enabled = true;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                dd_lettertype.Items.Add("Type2", "2");
                dd_lettertype.Items.Add("Type3", "3");
            }
            if (rd_LetterType.SelectedValue == "3")
            {
                dd_lettertype.Enabled = false;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                bind_campaign("3");


            }
            if (rd_LetterType.SelectedValue == "4")
            {
                dd_lettertype.Enabled = false;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                dd_lettertype.Items.Add("Type1", "1");
                dd_lettertype.Items.Add("Type2", "2");
                dd_lettertype.Items.Add("Type4", "4");
            }
            if (rd_LetterType.SelectedValue == "5")
            {
                dd_lettertype.Enabled = false;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                bind_campaign("3");
            }
            if (rd_LetterType.SelectedValue == "6")
            {
                dd_lettertype.Enabled = false;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                bind_campaign("3");
            }
            if (rd_LetterType.SelectedValue == "7")
            {
                dd_lettertype.Enabled = false;
                dd_lettertype.Items.Clear();
                dd_lettertype.Items.Add("--- Select ---", "");
                bind_campaign("3");
            }
        }
        protected void dd_campaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Type = "";

            if ((rd_LetterType.SelectedValue == "0") || (rd_LetterType.SelectedValue == "1") || (rd_LetterType.SelectedValue == "2") || (rd_LetterType.SelectedValue == "4"))
            {
                Type = dd_lettertype.SelectedItem.Value.ToString();
            }
            else
            {
                Type = "3";
            }

            string campaign = dd_campaign.SelectedItem.Value.ToString();

            bind_vendor(Type, campaign);
        }
        protected void dd_lettertype_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((rd_LetterType.SelectedValue == "0") || (rd_LetterType.SelectedValue == "1") || (rd_LetterType.SelectedValue == "2") || (rd_LetterType.SelectedValue == "4"))
            {
                bind_campaign(dd_lettertype.SelectedItem.Value.ToString());
            }

        }
        //protected void grdVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdVendor.PageIndex = e.NewPageIndex;
        //    grdVendor.DataSource = (DataSet)Session["ds_gridVendor"];
        //    grdVendor.DataBind();
        //}
        //protected void grdVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        TableCell cell = new TableCell();
        //        for (int i = 0; i < e.Row.Cells.Count; i++)
        //        {
        //            cell = e.Row.Cells[i];

        //            cell.Wrap = false;
        //        }
        //    }
        //}
        protected void grdProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProduct.PageIndex = e.NewPageIndex;
            DataSet DS = cookiesStorage.JsonDeserializeObjectHiddenDataSet(ds_gridProduct.Value);
            if (cookiesStorage.check_dataset(DS))
            {
               // cookiesStorage.SetCookiesDataSetByName("ds_gridProduct", DS);
                grdProduct.DataSource = DS;
                grdProduct.DataBind();
            }
        }
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell cell = new TableCell();
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    cell = e.Row.Cells[i];

                    cell.Wrap = false;
                }
            }
        }
        protected void dd_vendor_SelectedIndexChanged(object sender, EventArgs e)
        {

            bind_product(dd_campaign.SelectedItem.Value.ToString(), dd_vendor.SelectedItem.Value.ToString());

        }
    }
}