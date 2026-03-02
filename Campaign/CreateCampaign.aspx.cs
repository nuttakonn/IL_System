using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ILSystem.ManageData.WorkProcess.Campaign
{
    public partial class CreateCampaign : System.Web.UI.Page
    {
        public CookiesStorage campaignStorage;
        protected void Page_Load(object sender, EventArgs e)
        {
            campaignStorage = new CookiesStorage();
            if (!IsPostBack)
            {
                BindDatatoSession();
            }
            Response.Redirect("~/ManageData/WorkProcess/Campaign/ManageCampaign.aspx");
        }

        protected void BindDatatoSession()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STATUS_CAMPAIGN"); //EDIT UPDATE DELETE
            dt.Columns.Add("ID_CAMPAIGN"); //CampaignCode

            DataRow dr = dt.NewRow();
            dr["STATUS_CAMPAIGN"] = "CREATE_CAMPAIGN";
            dr["ID_CAMPAIGN"] = null;
            dt.Rows.Add(dr);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            campaignStorage.SetCookiesDataSetByName("DS_DATA_CAMPAIGN",ds);
        }
    }
}