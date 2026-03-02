using DevExpress.Web.ASPxEditors;
using EB_Service.BLL.Authentication;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ILSystem.App_Code.Model.NOTEAPI;
using EB_Service.Commons;
using System.Globalization;

namespace ILSystem.ManageData.WorkProcess.Note
{
    public partial class Note : System.Web.UI.Page
    {
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        UserInfo usInfo;
        public UserInfoService userInfoService;
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfoService = new UserInfoService();
            usInfo = userInfoService.GetUserInfo();

            string strFlgCopy = "Y";
            //Authen_MainSystemBLL.Get_Authorize_CopyData(usInfo.Username, usInfo.Password);
            usInfo.AuthenCopy = strFlgCopy;

            //Session["UserInfo"] = usInfo;

            if (((UserInfo)Session["UserInfo"]).AuthenCopy.ToString().Trim() == "N")
            {
                //Block Control 
                System.Text.StringBuilder sbCtlr = new System.Text.StringBuilder();
                sbCtlr.Append(@"<script language='javascript'>");
                //Ctrl+C,Ctrl+X
                sbCtlr.Append(@"document.attachEvent('onkeydown', my_onkeydown_handler);");
                sbCtlr.Append(@"function my_onkeydown_handler()");
                sbCtlr.Append(@"{if(event.ctrlKey)");
                sbCtlr.Append(@"{if(event.keyCode==67)");
                sbCtlr.Append(@"{event.returnValue = false;");
                sbCtlr.Append(@"window.status = 'Disabled Ctrl+C';");
                sbCtlr.Append(@"alert('Not have authorize to copy data.');}");
                sbCtlr.Append(@"else if(event.keyCode==88)");
                sbCtlr.Append(@"{event.returnValue = false;");
                sbCtlr.Append(@"window.status = 'Disabled Ctrl+X';");
                sbCtlr.Append(@"alert('Not have authorize to copy data.');}}}");
                //Right Click
                sbCtlr.Append(@"var isNS = (navigator.appName == 'Netscape') ? 1 : 0;");
                sbCtlr.Append(@"if(navigator.appName == 'Netscape') document.captureEvents(Event.MOUSEDOWN||Event.MOUSEUP);");
                sbCtlr.Append(@"function mischandler()");
                sbCtlr.Append(@"{ return false; }");
                sbCtlr.Append(@"function mousehandler(e)");
                sbCtlr.Append(@"{var myevent = (isNS) ? e : event;");
                sbCtlr.Append(@"var eventbutton = (isNS) ? myevent.which : myevent.button;");
                sbCtlr.Append(@"if((eventbutton==2)||(eventbutton==3))");
                sbCtlr.Append(@"{alert('Not have authorize to copy data.');");
                sbCtlr.Append(@"window.status = 'Disabled Rigth Click.';");
                sbCtlr.Append(@"return false;}}");
                sbCtlr.Append(@"document.oncontextmenu = mischandler;");
                sbCtlr.Append(@"document.onmousedown = mousehandler;");
                sbCtlr.Append(@"document.onmouseup = mousehandler;");
                sbCtlr.Append(@"</script>");

                if (!Page.ClientScript.IsStartupScriptRegistered("JSScript"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "JSScript", sbCtlr.ToString());
                }

            }


            if (IsPostBack)
            {

                return;
            }





            hid_csn.Value = Request.QueryString["param1"];
            //hid_idn.Value = Request.QueryString["param2"];
            ILDataCenter ilObj = new ILDataCenter();
            ILDataCenterMssqlInterview iLDataCenterMssql = new ILDataCenterMssqlInterview(usInfo);

            DataSet ds = iLDataCenterMssql.getCSMS00(hid_csn.Value).Result;
            if (ilObj.check_dataset(ds))
            {
                lb_Fname.Text = ds.Tables[0].Rows[0]["M00TNM"].ToString().Trim() + " " + ds.Tables[0].Rows[0]["M00TSN"].ToString().Trim();
                lb_id.Text = ds.Tables[0].Rows[0]["M00IDN"].ToString().Trim();
                bindResCode();
                bindAddNote();
            }
            else
            {
                lblMsg.Text = "Cannot find customer for save note";
            }

        }



        private void bindResCode()
        {
            try
            {
                ILDataCenter ilObj = new ILDataCenter();
                ILDataCenterMssqlInterview iLDataCenterMssql = new ILDataCenterMssqlInterview(usInfo);
                DataSet ds_resCode = new DataSet(); //ilObj.getResultCode();
                DataSet ds_ActCode = new DataSet();// ilObj.getActionCode();

                if (Cache["dsResCodeNote"] != null)
                {
                    ds_resCode = (DataSet)Cache["dsResCodeNote"];
                }
                else
                {
                    ds_resCode = iLDataCenterMssql.getResultCode().Result;
                    Cache["dsResCodeNote"] = ds_resCode;
                    Cache.Insert("dsResCodeNote", ds_resCode, null, DateTime.Now.AddHours(12), TimeSpan.Zero);

                }

                if (Cache["dsActionCodeNote"] != null)
                {
                    ds_ActCode = (DataSet)Cache["dsActionCodeNote"];
                }
                else
                {
                    ds_ActCode = iLDataCenterMssql.getActionCode().Result;
                    Cache["dsActionCodeNote"] = ds_ActCode;
                    Cache.Insert("dsActionCodeNote", ds_ActCode, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
                }


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
        private void bindAddNote()
        {
            try
            {
                userInfoService = new UserInfoService();
                usInfo = userInfoService.GetUserInfo();
                //lblMsg.Text = "";
                txt_memoReason.Text = "";
                ILDataCenter ilObj = new ILDataCenter();
                ILDataCenterMssql iLDataCenterMssql = new ILDataCenterMssql(usInfo);

                //TODO:call API GETNOTE
                Connect_NoteAPI noteAPI = new Connect_NoteAPI();
                var resultGetnote =  noteAPI.GetNote(hid_csn.Value.Trim()).Result;
                List<ResponseGetnote> responseGetnotes = new List<ResponseGetnote>();
                DataSet ds_note = new DataSet();
                if (!resultGetnote.success)
                {
                    //JsonConvert.DeserializeObject<DataSet>(data)
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<GetTimeline>(resultGetnote.data?.ToString());
                    if(response.dataAPI.Count > 0)
                    {
                        foreach(var prop in response.result.data)
                        {
                            responseGetnotes.Add(new ResponseGetnote()
                            {
                                M38ACD = prop.actionCode,
                                M38USR = prop.noteByName,
                                M38DES = prop.noteDescription,
                                M38DAT_ = prop.noteDate?.ToString("dd/MM/yyyy"),
                                M38TIM = prop.noteDate?.ToString("HH:mm:ss")


                            });
                        }
                       var res =  responseGetnotes.ToDataTable();
                        ds_note.Tables.Add(res);
                    }

                }

                Session["ds_Note"] = null;
                //DataSet ds_note = iLDataCenterMssql.getNote(hid_csn.Value.Trim());
                Session["ds_Note"] = ds_note;
                gvNote.DataSource = ds_note;
                gvNote.DataBind();
                gvNote.PageIndex = 0;

                return;

            }
            catch (Exception ex)
            {

            }
        }
        protected void gvNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNote.PageIndex = e.NewPageIndex;
            gvNote.DataSource = (DataSet)Session["ds_Note"];
            gvNote.DataBind();
        }
        protected void btn_saveNote_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                userInfoService = new UserInfoService();
                usInfo = userInfoService.GetUserInfo();
                ILDataCenter ilObj = new ILDataCenter();
                ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(usInfo);
                ilObj.UserInfomation = usInfo;

                if (hid_csn.Value.Trim() == "")
                {
                    lblMsg.Text = "Cannot add note before search customer ";
                    lblMsg.Visible = true;
                    return;
                }
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
                    txt_memoReason.Text.Trim().IndexOf("\\n") >= 0
                    // || txt_memoReason.Text.Trim().IndexOf("?") >= 0 || txt_memoReason.Text.Trim().IndexOf("+") >= 0
                    // || txt_memoReason.Text.Trim().IndexOf("%") >= 0 || txt_memoReason.Text.Trim().IndexOf("!") >= 0
                    // || txt_memoReason.Text.Trim().IndexOf("=") >= 0
                    // || txt_memoReason.Text.Trim().IndexOf(":") >= 0 || txt_memoReason.Text.Trim().IndexOf(";") >= 0
                    // || txt_memoReason.Text.Trim().IndexOf("<") >= 0 || txt_memoReason.Text.Trim().IndexOf(">") >= 0
                    )
                {
                    lblMsg.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ \r \n \t  ";
                    //lblMsg.Text = "Err Input Data Main Topic Name had Spceial Character ' \\ ? + % ! = : ; < > ";
                    lblMsg.Visible = true;
                    txt_memoReason.Focus();
                    return;
                }
                if (dd_ActionCode.Value.ToString() == "" || dd_ResCode.Value?.ToString() == "")
                {
                    lblMsg.Text = "Please input Action code or result code.";
                    lblMsg.Visible = true;
                    return;

                }
                if (txt_memoReason.Text.Trim() == "")
                {
                    lblMsg.Text = "Please input note detail.";
                    lblMsg.Visible = true;
                    return;
                }

                if (txt_memoReason.Text.Trim().Length > 200)
                {
                    lblMsg.Text = "Lengh of note detail > 200 Characters.";
                    lblMsg.Visible = true;
                    return;
                }
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

                string ErrMsgNote = "";

                //bool AddNote = iLDataSubroutine.CALL_CSSRW11("IL", lb_id.Text.Trim(), dd_ActionCode.Value.ToString(), dd_ResCode.Value.ToString(),
                //                          desc1, desc2, ref ErrMsgNote, m_userInfo.BizInit, m_userInfo.BranchNo);
                var resNote = noteAPI.AddNote(hid_csn.Value.ToString().Trim(), "0", dd_ActionCode.Value.ToString(), dd_ResCode.Value.ToString(), desc1+desc2, m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

                if (!resNote.success || ErrMsgNote.Trim() != "")
                {
                    lblMsg.Text = "Cannot Save note  : " + ErrMsgNote.ToString().Trim();
                    //ilObj.RollbackDAL();
                    //ilObj.CloseConnectioDAL();
                    return;
                }
                //ilObj.CommitDAL();
                //ilObj.CloseConnectioDAL();
                lblMsg.Text = "Save note success ";
                lblMsg.Visible = true;
                bindAddNote();


                return;

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Save note not success ";
                lblMsg.Visible = true;
                //ilObj.RollbackDAL();
                //ilObj.CloseConnectioDAL();
                return;
            }
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
    }
}