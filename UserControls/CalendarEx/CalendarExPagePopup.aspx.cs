using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EBPortal.WebControlEx.CalendarEx
{
    public partial class CalendarExPagePopup : System.Web.UI.Page
    {
        private string m_strFormat = "dd/MM/yyyy";
        //private PickUpDate.SeparateType m_separate = PickUpDate.SeparateType.Slash;

        protected void Page_Load(object sender, EventArgs e)
        {
            cldShow.DayRender += new DayRenderEventHandler(CreateDayEffects);
            cldShow.PreRender += new EventHandler(cldShow_PreRender);
            cldShow.NextPrevFormat = System.Web.UI.WebControls.NextPrevFormat.CustomText;
            cldShow.PrevMonthText = "<< Previous";
            cldShow.NextMonthText = "Next >>";
            control.Value = Request.QueryString["ctlParent"].ToString();
            m_strFormat = Request.QueryString["FormatDate"].ToString();
            if (Request.QueryString["DefaultDate"].ToString() != "now")
            {
                string strDefDt = Request.QueryString["DefaultDate"].ToString();
                try
                {
                    cldShow.TodaysDate = DateTime.Parse(strDefDt);
                }
                catch { };
            }
            //if (m_strFormat.IndexOf("/") != -1)
            //    m_separate = PickUpDate.SeparateType.Slash;
            //else if (m_strFormat.IndexOf("-") != -1)
            //    m_separate = PickUpDate.SeparateType.Dash;
            //else
            //    m_separate = PickUpDate.SeparateType.None;
        }

        private void CreateDayEffects(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
        {
            System.Web.UI.WebControls.CalendarDay calDay = e.Day;
            if (calDay.IsToday)
                return;


            e.Cell.Attributes["onmouseover"] = "this.style.backgroundColor='pink';";

            if (cldShow.DayStyle.BackColor != Color.Empty)
                e.Cell.Attributes["onmouseout"] = "this.style.backgroundColor='" +
                    cldShow.DayStyle.BackColor.ToKnownColor() + "';";
            else
                e.Cell.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            //			}
        }
        protected void cldShow_SelectionChanged(object sender, EventArgs e)
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>javascript:ChooseReturnToTextBox2('");
            stb.Append(control.Value);
            stb.Append("','");
            stb.Append(cldShow.SelectedDate.ToString(m_strFormat));
            stb.Append("');</script>");
            RegisterClientScriptBlock("anything", stb.ToString());
        }
        protected void cldShow_PreRender(object sender, EventArgs e)
        {
            Calendar cd = (Calendar)sender;

        }
    }
}
