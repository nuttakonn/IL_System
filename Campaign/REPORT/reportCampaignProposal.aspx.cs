using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem.ManageData.WorkProcess.Campaign.REPORT
{
    public partial class reportCampaignProposal : System.Web.UI.Page
    {
        public CookiesStorage campaignStorage;
        ReportDocument rpt = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            campaignStorage = new CookiesStorage();
            if (Page.IsPostBack)
            {
                return;
            }

            crpCampaignProposal.ToolPanelView = ToolPanelViewType.None;


            DataSet dsListData = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvCampaign");
            dsListData.Tables[0].TableName = "dtDataList";

            foreach (DataRow drListData in dsListData.Tables[0].Rows)
            {
                //dr["C01CMP"] = FORMAT_DATE(dr["C01CMP"].ToString().Trim());
                drListData["C01SDT"] = FORMAT_DATE(drListData["C01SDT"].ToString().Trim());
                drListData["C01EDT"] = FORMAT_DATE(drListData["C01EDT"].ToString().Trim());
                drListData["C01CAD"] = FORMAT_DATE(drListData["C01CAD"].ToString().Trim());
                drListData["C01CLD"] = FORMAT_DATE(drListData["C01CLD"].ToString().Trim());
                double txtRates = double.Parse(drListData["C01SRT"].ToString().Trim());
                drListData["C01SRT"] = txtRates.ToString("#,##0.00").Trim();
                //drListData["C01SRT"] = drListData["C01SRT"].ToString().Trim();
                if (drListData["C01STY"].ToString().Trim() == "1")
                {
                    drListData["C01STY"] = "Maker";
                }
                else if (drListData["C01STY"].ToString().Trim() == "2")
                {
                    drListData["C01STY"] = "Vendor";
                }
                else if (drListData["C01STY"].ToString().Trim() == "3")
                {
                    drListData["C01STY"] = "Non Sub Campaign";
                }
                else if (drListData["C01STY"].ToString().Trim() == "4")
                {
                    drListData["C01STY"] = "Share Sub";
                }
            }

            DataSet dsListNote = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvListNote");
            dsListNote.Tables[0].TableName = "dtNoteList";
            foreach (DataRow drListNote in dsListNote.Tables[0].Rows)
            {
                drListNote["C11UDT"] = FORMAT_DATE(drListNote["C11UDT"].ToString().Trim());
                drListNote["C11UTM"] = FORMAT_TIME(drListNote["C11UTM"].ToString().Trim());
            }

            DataSet dsListVendor = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvListDataVendor");
            dsListVendor.Tables[0].TableName = "dtVendorList";

            DataSet dsListCampaign = (DataSet)campaignStorage.GetCookiesDataSetByKey("ds_gvListDataCampaign");
            dsListCampaign.Tables[0].TableName = "dtCampaignList";

            rpt.Load(Server.MapPath("rpCampaignProposals.rpt"));
            rpt.SetDataSource(dsListData);
            rpt.SetDataSource(dsListNote);
            rpt.SetDataSource(dsListVendor);
            rpt.SetDataSource(dsListCampaign);
            crpCampaignProposal.ReportSource = rpt;
        }
        static string FORMAT_DATE(string dateCall)
        {
            string[] formats = { "dd/MM/yyyy", "yyyyMMdd" };
            DateTime dtime;
            DateTime.TryParseExact(dateCall, formats, CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dtime);
            string formattedDateNew = dtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return formattedDateNew;
        }
        static string FORMAT_TIME(string timeCall)
        {
            string[] formattedTimeNew = timeCall.ToString().Trim().Split(':');
            string timeTranfer = "";
            if (formattedTimeNew.Length > 1)
            {
                timeTranfer = formattedTimeNew[0] + ":" + formattedTimeNew[1] + ":" + formattedTimeNew[2];
            }
            else
            {
                timeTranfer = timeCall.ToString().Substring(0, 2) + ":" + timeCall.ToString().Substring(2, 2) + ":" + timeCall.ToString().Substring(4, 2);
            }
            return timeTranfer;
        }
    }
}