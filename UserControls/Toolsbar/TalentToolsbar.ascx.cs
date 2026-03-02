using EB_Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TalentToolsbar : System.Web.UI.UserControl
{
    public delegate void ChooserTalentToolsbarChangedEventHandler(object sender, ChooserTalentToolsbarChangedEventArgs e);
    public event ChooserTalentToolsbarChangedEventHandler ChooserTalentToolsbarChanged;
    private string PathImg = WebConfigurationManager.AppSettings["PathImgButton"].ToString().Trim();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnChooserToolsbar_Click(object sender, ImageClickEventArgs e)
    {
        string strCmd = "";
        int iIndex = -1;
        string strClientID = "";
        ImageButton imgbtnObj = (ImageButton)sender;
        switch (imgbtnObj.ID)
        {
            case "btnSave":
                strCmd = "Save";
                iIndex = 0;


                break;
            case "btnCancel":
                strCmd = "Cancel";
                iIndex = 1;


                break;
            case "btnDelete":
                strCmd = "Delete";
                iIndex = 2;


                break;
            case "btnNew":
                strCmd = "New";
                iIndex = 3;


                break;
            case "btnEdit":
                strCmd = "Edit";
                iIndex = 4;


                break;
            case "btnGotoList":
                strCmd = "GoTolist";
                iIndex = 5;


                break;
            case "btnSearch":
                strCmd = "Search";
                iIndex = 6;


                break;


        }

        if (ChooserTalentToolsbarChanged != null)
        {
            ChooserTalentToolsbarChanged(this, new ChooserTalentToolsbarChangedEventArgs(sender, strCmd, iIndex, imgbtnObj.UniqueID));//Save 

        }
    }

    public ImageButton GetSaveButton
    {
        get { return btnSave; }
    }
    public ImageButton GetCancelButton
    {
        get { return btnCancel; }
    }
    public ImageButton GetDeleteButton
    {
        get { return btnDelete; }
    }
    public ImageButton GetNewButton
    {
        get { return btnNew; }
    }
    public ImageButton GetEditButton
    {
        get { return btnEdit; }
    }
    public ImageButton GetGotoListButton
    {
        get { return btnGotoList; }
    }
    public ImageButton GetSearchButton
    {
        get { return btnSearch; }
    }

    #region Set Enable Button

    public bool SetEnableSaveButton
    {
        get { return btnSave.Enabled; }
        set
        {
            btnSave.Enabled = Convert.ToBoolean(value);
            btnSave.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "accept-32x32.png" : PathImg + "accept-32x32_Inactive.png");
        }
    }
    public bool SetEnableCancelButton
    {
        get { return btnCancel.Enabled; }
        set
        {
            btnCancel.Enabled = Convert.ToBoolean(value);
            btnCancel.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonRefresh.png" : PathImg + "ButtonRefresh_Inactive.png");
        }
    }
    public bool SetEnableDeleteButton
    {
        get { return btnSave.Enabled; }
        set
        {
            btnDelete.Enabled = Convert.ToBoolean(value);
            btnDelete.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "delete-32x32.png" : PathImg + "delete-32x32_Inactive.png");
        }
    }
    public bool SetEnableNewButton
    {
        get { return btnNew.Enabled; }
        set
        {
            btnNew.Enabled = Convert.ToBoolean(value);
            btnNew.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "add-32x32.png" : PathImg + "add-32x32_Inactive.png");
        }
    }
    public bool SetEnableEditButton
    {
        get { return btnEdit.Enabled; }
        set
        {
            btnEdit.Enabled = Convert.ToBoolean(value);
            btnEdit.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "edit-32x32.png" : PathImg + "edit-32x32_Inactive.png");
        }
    }
    public bool SetEnableGotoListButton
    {
        get { return btnGotoList.Enabled; }
        set
        {
            btnGotoList.Enabled = Convert.ToBoolean(value);
            btnGotoList.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonBack.png" : PathImg + "ButtonBack_Inactive.png");
        }
    }
    public bool SetEnableSearchButton
    {
        get { return btnSearch.Enabled; }
        set
        {
            btnSearch.Enabled = Convert.ToBoolean(value);
            btnSearch.ImageUrl = ((Convert.ToBoolean(value) == true) ? PathImg + "ButtonSearch.png" : PathImg + "ButtonSearch_Inactive.png");
        }
    }

    #endregion

}