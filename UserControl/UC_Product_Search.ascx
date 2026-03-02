<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Product_Search.ascx.cs" Inherits="ManageData_WorkProcess_UserControl_UC_Product_Search" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx1" %>
<style type="text/css">
    .style2
    {
        height: 24px;
        width: 29%;
    }
    .style3
    {
        height: 24px;
        width: 27%;
    }
    .style5
    {
        height: 24px;
        width: 31%;
    }
    .style6
    {
        height: 24px;
        width: 33%;
    }
    .style7
    {
        height: 24px;
        width: 58%;
    }
    .style8
    {
        height: 24px;
        width: 24%;
    }
    .style9
    {
        height: 24px;
        width: 13%;
    }
    .style10
    {
        height: 24px;
        width: 44%;
    }
    .style11
    {
        height: 24px;
        width: 15%;
    }
</style>
<asp:Panel ID="P_tcl_product" runat="server" Visible="False">
    <table style="width: 100%;">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px;
                height: 24px;">
                <asp:Label ID="Label55" runat="server" Style="font-family: Tahoma; font-size: small;
                    font-weight: 700;" Text="EBC Limit " Width="130px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <dx:ASPxTextBox ID="EdtESBLoan" runat="server" Enabled="False" Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label ID="LTCL" runat="server" Style="font-family: Tahoma; font-size: small;
                    font-weight: 700;" Text="Total credit Line" Width="180px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <dx:ASPxTextBox ID="EdtTCL" runat="server" Enabled="False" Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label ID="Label57" runat="server" Style="font-family: Tahoma; font-size: small;
                    font-weight: 700;" Text="Customer balance" Width="130px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <dx:ASPxTextBox ID="EdtCrBal" runat="server" Enabled="False" Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px;
                height: 24px;">
                <asp:Label runat="server" Text="TCL Available" Width="130px" ID="Label58" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <div style="float: left;">
                </div>
                <div style="float: left;">
                    &nbsp;&nbsp;
                </div>
                <div style="float: left;">
                    <dx:ASPxTextBox runat="server" Width="90px" Height="24px" Enabled="False" ID="txt_app_lm">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label runat="server" Text="BOT Loan" Width="130px" ID="Label60" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="EdtBOTLoan">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label ID="LACL" runat="server" Style="font-family: Tahoma; font-size: small;
                    font-weight: 700;" Text="ACL" Width="112px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <div style="float: left;">
                </div>
                <dx:ASPxTextBox ID="EdtACL" runat="server" Enabled="False" Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px;
                height: 24px;">
                <asp:Label runat="server" Text="Income" Width="100px" ID="Label65" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <asp:Label runat="server" ForeColor="Blue" Width="130px" ID="EdtNetIncome" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label runat="server" Text="Approve available" Width="130px" ID="Label100" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <asp:Label runat="server" ForeColor="Blue" Width="130px" ID="E_Apv_avi" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                &nbsp;
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px;
                height: 24px;">
                <div style="float: left;">
                    <asp:Label ID="Label62" runat="server" Style="font-family: Tahoma; font-size: small;
                        font-weight: 700;" Text="Group no." Width="130px"></asp:Label>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxTextBox ID="E_group" runat="server" Enabled="False" Height="24px" Width="130px">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <div style="float: left;">
                    <asp:Label ID="Label63" runat="server" Style="font-family: Tahoma; font-size: small;
                        font-weight: 700;" Text="Rank ." Width="70px"></asp:Label>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxTextBox ID="E_rank" runat="server" Enabled="False" Height="24px" Width="130px">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label ID="Label101" runat="server" Style="font-family: Tahoma; font-size: small;
                    font-weight: 700;" Text="Model" Width="70px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                height: 24px;">
                <dx:ASPxTextBox ID="E_model" runat="server" Enabled="False" Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                <asp:TextBox ID="G_Have_CSMS03" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_TCL_13" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_ACL_13" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_PD" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Rank_for_GNSR031" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_GROUP_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_RANK_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_ACL_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="P4TYPE" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_NewModel_ZR" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Model" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Have_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Ongoing_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="EAAA" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="EdtCrLmt" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <br />
                <asp:TextBox ID="G_Net_Income" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Orank" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Otimes" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_ACL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Arank" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Atimes" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_AACL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Rrank" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Rtimes" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Final_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_CSP" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Total_CSP" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Up_Down_Flag" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_GRACE_Period" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_PD1" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_GNO" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Mem_ACL_Ongoing" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="G_Mem_TCL_Ongoing" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="Maker" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="Prod_grou" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="Loan_Amt" runat="server" Visible="False" Width="20px"></asp:TextBox>
                <asp:TextBox ID="Duty_Amt" runat="server" Visible="False" Width="20px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<table style="width: 100%;">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
            height: 24px;">
            <asp:Label runat="server" Text="Payment type " Width="100px" ID="Label95" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px;
            height: 24px;">
            <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" AutoPostBack="True"
                ID="dd_paymentType" OnSelectedIndexChanged="dd_paymentType_SelectedIndexChanged">
            </dx:ASPxComboBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: medium; height: 24px;">
            <asp:Label runat="server" ForeColor="Red" ID="L_error_product" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;" Font-Size="Large"></asp:Label>
            <asp:HiddenField ID="hid_p1aprj" runat="server" />
        </td>
    </tr>
</table>
<asp:Panel ID="P_payment" runat="server" Visible="False">
    <table style="width: 100%;">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                height: 24px;">
                <asp:Label runat="server" Text="Bank code " Width="100px" ID="Label96" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" AutoPostBack="True"
                        ID="dd_bankCode" OnSelectedIndexChanged="dd_bankCode_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </div>
                <div style="float: left;">
                    &nbsp;&nbsp;</div>
                <div style="float: left;">
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label runat="server" Text="Branch code " Width="100px" ID="Label97" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" ID="dd_bankBranch">
                    </dx:ASPxComboBox>
                </div>
                <div style="float: left;">
                    &nbsp;&nbsp;</div>
                <div style="float: left;">
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                height: 24px;">
                <asp:Label runat="server" Text="Account Type " Width="100px" ID="Label98" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" ID="dd_accountType">
                    </dx:ASPxComboBox>
                </div>
                <div style="float: left;">
                    &nbsp;&nbsp;</div>
                <div style="float: left;">
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                height: 24px;">
                <asp:Label runat="server" Text="Account No. " Width="100px" ID="Label99" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px;
                height: 24px;">
                <div style="float: left;">
                    <dx:ASPxTextBox runat="server" Width="120px" Height="24px" ID="txt_AccountNo" MaxLength="15">
                    </dx:ASPxTextBox>
                </div>
                <div style="float: left;">
                    &nbsp;&nbsp;</div>
                <div style="float: left;">
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>
<table width="100%" style="background-color: Yellow;">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            <asp:Label runat="server" Text="Product" Font-Underline="True" ForeColor="Blue" Width="130px"
                ID="Label64" Style="font-family: Tahoma; font-size: medium; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
            &nbsp;
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Total Term" Width="120px" ID="Label68" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 700px;">
            <dx:ASPxTextBox runat="server" Width="30px" Height="24px" ID="dd_totalterm" Text="0"
                MaxLength="2" OnTextChanged="dd_totalterm_TextChanged" AutoPostBack="True">
                <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                    <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                </ValidationSettings>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px;">
            <asp:Label runat="server" Text="Approve Limit" Width="130px" ID="Label66" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;">
            <asp:Label runat="server" ForeColor="Red" Width="130px" ID="lb_pApproveL" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            <dx:ASPxButton runat="server" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                Text="Save Credit" CssPostfix="Office2003Blue" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                Width="120px" Height="18px" ID="btn_saveCr" OnClick="btn_saveCr_Click">
                <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_Product.Show();
}" />
            </dx:ASPxButton>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Vendor" Width="120px" ID="Label70" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 700px;">
            <div style="float: left">
                <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                    DropDownRows="10" ValueType="System.String" Width="500px" AutoPostBack="True"
                    TabIndex="18" ID="dd_vendor" OnSelectedIndexChanged="dd_vendor_SelectedIndexChanged">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RegularExpression ErrorText=""></RegularExpression>
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="40px" Enabled="False" ID="txt_rank_v">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px;">
            <asp:Label runat="server" Text="Campaign Type" Width="130px" ID="Label71" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;">
            <dx:ASPxTextBox runat="server" Width="130px" Text="R" Height="24px" Enabled="False"
                ID="txt_campgType">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Campaign" Width="120px" ID="Label72" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 700px;">
            <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                DropDownRows="10" ValueType="System.String" Width="550px" AutoPostBack="True"
                TabIndex="19" ID="dd_campaign" OnSelectedIndexChanged="dd_campaign_SelectedIndexChanged">
                <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                    <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                </ValidationSettings>
            </dx:ASPxComboBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px;">
            <asp:Label runat="server" Text="Campaign Seq." Width="130px" ID="Label73" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ClientInstanceName="txt_campSeq"
                ID="txt_campSeq">
                <ValidationSettings ValidationGroup="v_cal_prod">
                    <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                </ValidationSettings>
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Product" Width="120px" ID="Label74" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 700px;">
            <div style="float: left;">
                <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                    DropDownRows="10" ValueType="System.String" Width="550px" AutoPostBack="True"
                    TabIndex="20" ID="dd_product" OnTextChanged="dd_product_TextChanged">
                    <ClientSideEvents TextChanged="function(s, e) {
                                                                            Callback.PerformCallback();LoadingPanel_Product.Show(); }">
                    </ClientSideEvents>
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </div>
            <div style="float: left;">
                <dx:ASPxButton ID="btn_vendorScr" runat="server" Text="..." Width="25px" AutoPostBack="true"
                    OnClick="btn_vendorScr_Click">
                    <ClientSideEvents Click="function(s, e) {LoadingPanel_Product.Show(); }"></ClientSideEvents>
                </dx:ASPxButton>
            </div>
            <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small;"
                ForeColor="red" Text="(ระบุ 4 ตัวอักษรขึ้นไป)" Width="140px"></asp:Label>
            <asp:Label runat="server" ForeColor="Blue" Height="16px" Width="530px" ID="lb_prodcount"
                Style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px;">
            <asp:Label runat="server" Text="Payment ability" Width="130px" ID="Label75" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_pay_abl">
                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Price/item" Width="120px" ID="Label77" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 700px;">
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" ID="txt_price">
                    <MaskSettings Mask="&lt;0..9999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="False">
                    </MaskSettings>
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <asp:Label runat="server" Text="Total Quatity" Width="94px" ID="Label80" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="70px" Height="24px" ID="txt_qty">
                    <MaskSettings Mask="&lt;1..100&gt;" AllowMouseWheel="False"></MaskSettings>
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </div>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <asp:Label runat="server" Text="Down" Width="94px" ID="Label1" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="70px" Height="24px" ID="txt_down" Text="0">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px;">
            <dx:ASPxButton runat="server" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                Text="Calculate" ValidationGroup="v_cal_prod" CssPostfix="Office2003Blue" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                Width="90px" Height="24px" ID="btn_cal_TCL" OnClick="btn_cal_TCL_Click">
                <ClientSideEvents Click="function(s, e) {
    LoadingPanel_Product.Show();
}" />
            </dx:ASPxButton>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;">
            <dx:ASPxButton runat="server" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                Text="Edit" CssPostfix="Office2003Blue" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                Width="90px" Height="24px" ID="btn_keyin" OnClick="btn_keyin_Click">
                <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_Product.Show();
}" />
            </dx:ASPxButton>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
            height: 24px;">
            &nbsp;
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Total range" Width="120px" ID="Label76" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style10">
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ID="txt_total_range">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <asp:Label runat="server" Text="Non" Width="30px" ID="Label78" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="50px" Height="24px" Enabled="False" ID="txt_non">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                    </ValidationSettings>
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <asp:Label runat="server" Text="Due" Width="50px" ID="Label79" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style11">
            <asp:Label runat="server" Text="Total Purchase" Width="130px" ID="Label81" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style3">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_purch">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <asp:Label runat="server" Text="First due amount" Width="120px" ID="Label85" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ID="txt_fDue_AMT">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Min price" Width="120px" ID="Label82" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style10">
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ClientInstanceName="txt_minPrice"
                    ID="txt_minPrice">
                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                    <ValidationSettings ErrorText="">
                    </ValidationSettings>
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ClientInstanceName="txt_maxPrice"
                    ID="txt_maxPrice">
                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style11">
            <asp:Label runat="server" Text="Loan request" Width="130px" ID="Label84" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style3">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_loanReq">
                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                <DisabledStyle BackColor="LightGray" ForeColor="Red">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <asp:Label runat="server" Text="First due date" Width="130px" ID="Label86" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_fDue_date">
                <MaskSettings Mask="00/00/0000"></MaskSettings>
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Duty stamp" Width="120px" ID="Label87" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style10">
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ID="txt_duty">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
            <div style="float: left">
                &nbsp;&nbsp;</div>
            <div style="float: left">
                <asp:Label runat="server" Text="Int.Avg %" Width="99px" ID="Label88" Style="font-family: Tahoma;
                    font-size: small; font-weight: 700;"></asp:Label>
            </div>
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="90px" Height="24px" Enabled="False" ID="txt_Int">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style11">
            <asp:Label runat="server" Text="Cr.Usg Avg%" Width="130px" ID="Label89" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style3">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_cru">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <asp:Label runat="server" Text="Contract Amount" Width="120px" ID="Label92" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="txt_contractAmt">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 10%; height: 24px;">
            <asp:Label runat="server" Text="Credit bureau" Width="120px" ID="Label90" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style10">
            <div style="float: left">
                <dx:ASPxTextBox runat="server" Width="100px" Height="24px" Enabled="False" ID="txt_bureau">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </div>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style11">
            <asp:Label runat="server" Text="Approve Avi" Width="130px" ID="Label116" Style="font-family: Tahoma;
                font-size: small; font-weight: 700;"></asp:Label>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style3">
            <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" ID="EdtCrAvi">
                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxTextBox>
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            &nbsp;
        </td>
        <td style="text-align: left; font-family: Tahoma; font-size: small;" class="style5">
            &nbsp;
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
            height: 24px;">
            <asp:Label runat="server" Text="Description term and rate" Font-Underline="True"
                ForeColor="Blue" Width="250px" ID="Label91" Style="font-family: Tahoma; font-size: medium;
                font-weight: 700;"></asp:Label>
        </td>
    </tr>
</table>
<asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="No records found"
    GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderWidth="1px"
    BorderStyle="Solid" ForeColor="Black" Width="99%" ID="gvTerm" 
    onrowcreated="gvTerm_RowCreated">
    <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
    <Columns>
        <asp:TemplateField HeaderText="From Term">
            <ItemTemplate>
                <asp:Label ID="lb_Fterm" runat="server" Text='<%#Eval("from_T")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="To Term">
            <ItemTemplate>
                <asp:Label ID="lb_Tterm" runat="server" Text='<%#Eval("to_T")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Int%">
            <ItemTemplate>
                <asp:Label ID="lb_intP" runat="server" Text='<%#Eval("int_p")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Int Amount">
            <ItemTemplate>
                <asp:Label ID="lb_intAMT" runat="server" Text='<%#Eval("int_amt")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="INT%(EIR/YEAR)">
            <ItemTemplate>
                <asp:Label ID="lblIntEir" runat="server" Text='<%#Eval("inteir")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cru%">
            <ItemTemplate>
                <asp:Label ID="lb_cru_p" runat="server" Text='<%#Eval("cru_p")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cru Amount">
            <ItemTemplate>
                <asp:Label ID="lb_cruAmonth" runat="server" Text='<%#Eval("cru_amt")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CRU%(EIR/YEAR)">
            <ItemTemplate>
                <asp:Label ID="lblCruEir" runat="server" Text='<%#Eval("crueir")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Init. Free">
            <ItemTemplate>
                <asp:Label ID="lb_InitFree" runat="server" Text='<%#Eval("int_free")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Total Amount">
            <ItemTemplate>
                <asp:Label ID="lb_totalAmt" runat="server" Text='<%#Eval("total_amt")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Oth">
            <ItemTemplate>
                <asp:Label ID="lb_OTH" runat="server" Text='<%#Eval("oth")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PRINCIPAL" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_PRINCIPAL" runat="server" Text='<%#Eval("PRINCIPAL")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="INSTALL" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_INSTALL" runat="server" Text='<%#Eval("INSTALL")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DUTY STAMP" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_DUTY_STAMP" runat="server" Text='<%#Eval("DUTY_STAMP")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="INTEREST_BASE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_INTEREST_BASE" runat="server" Text='<%#Eval("INTEREST_BASE")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CR.USAGE BASE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_CR_USAGE" runat="server" Text='<%#Eval("CR_USAGE_BASE")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="INIT.FEE BASE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_INIT_FEE_BASE" runat="server" Text='<%#Eval("INIT_FEE_BASE")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CONTRACT" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_CONTRACT" runat="server" Text='<%#Eval("CONTRACT_AMOUNT")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="FIRSTDUE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_FIRSTDUE" runat="server" Text='<%#Eval("FIRST_DATE")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="AVG_INTERATE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_AVG_INTERATE" runat="server" Text='<%#Eval("AVG_INTEREST")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="AVG_CREDIT_USAGE" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_AVG_CREDIT_USAGE" runat="server" Text='<%#Eval("AVG_CR_USAGE")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CREDIT BUREAU" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lb_CREDIT_BUREAU" runat="server" Text='<%#Eval("CREDIT_BUREAU")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
    </Columns>
    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#D4668D" Font-Bold="True"
        ForeColor="White"></EmptyDataRowStyle>
    <FooterStyle BackColor="#CCCC99"></FooterStyle>
    <HeaderStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="Black"></HeaderStyle>
    <PagerStyle HorizontalAlign="Right" BackColor="#F7F7DE" ForeColor="Black"></PagerStyle>
    <RowStyle BackColor="#F7F7DE"></RowStyle>
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
</asp:GridView>
<table width="100%">
    <tr>
        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
            height: 24px;">
            <asp:Label runat="server" Text="Description Installment" Font-Underline="True" ForeColor="Blue"
                Width="250px" ID="Label93" Style="font-family: Tahoma; font-size: medium; font-weight: 700;"></asp:Label>
        </td>
    </tr>
</table>
<asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="No records found"
    GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderWidth="1px"
    BorderStyle="Solid" ForeColor="Black" Width="99%" ID="gv_install">
    <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
    <Columns>
        <asp:TemplateField HeaderText="Term">
            <ItemTemplate>
                <asp:Label ID="lb_term" runat="server" Text='<%#Eval("term")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Begin">
            <ItemTemplate>
                <asp:Label ID="lb_begin" runat="server" Text='<%#Eval("begin")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Principal">
            <ItemTemplate>
                <asp:Label ID="lb_principal0" runat="server" Text='<%#Eval("princ")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Installment">
            <ItemTemplate>
                <asp:Label ID="lb_Installment" runat="server" Text='<%#Eval("intstallment")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Interest">
            <ItemTemplate>
                <asp:Label ID="lb_interest" runat="server" Text='<%#Eval("interest")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cr. Usage">
            <ItemTemplate>
                <asp:Label ID="lb_CrUsg" runat="server" Text='<%#Eval("cr_use")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Income">
            <ItemTemplate>
                <asp:Label ID="lb_income" runat="server" Text='<%#Eval("income")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cur. Principal">
            <ItemTemplate>
                <asp:Label ID="lb_CPrincipal" runat="server" Text='<%#Eval("cur_princ")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lb_amonth" runat="server" Text='<%#Eval("amt")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
        </asp:TemplateField>
    </Columns>
    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#D4668D" Font-Bold="True"
        ForeColor="White"></EmptyDataRowStyle>
    <FooterStyle BackColor="#CCCC99"></FooterStyle>
    <HeaderStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="Black"></HeaderStyle>
    <PagerStyle HorizontalAlign="Right" BackColor="#F7F7DE" ForeColor="Black"></PagerStyle>
    <RowStyle BackColor="#F7F7DE"></RowStyle>
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
</asp:GridView>
<p align="right">
    &nbsp;</p>
<dx:ASPxPopupControl ID="P_message_product" runat="server" Height="146px" Width="374px"
    AllowDragging="True" AutoUpdatePosition="True" ClientInstanceName="P_message_product"
    ShowPageScrollbarWhenModal="true" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" Font-Bold="False" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
    CssPostfix="BlackGlass">
    <HeaderStyle Font-Bold="True" Font-Size="Medium" />
    <HeaderTemplate>
        Message
    </HeaderTemplate>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <table align="center" width="100%">
                <tr>
                    <td align="center" style="text-align: left" class="style15">
                        <asp:Label ID="L_message" runat="server" Font-Size="Small" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <table align="center" width="100%">
                <tr>
                    <td align="right" style="text-align: center; width: 100%;" width="50%">
                        <dx:ASPxButton ID="B_confirmok" runat="server" Text="OK" OnClick="B_confirmok_Click"
                            Width="100px">
                            <ClientSideEvents Click="function(s, e) {    
    P_message_product.Hide();
}" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="P_confirm_product" runat="server" Height="146px" Width="374px"
    AllowDragging="True" AutoUpdatePosition="True" ClientInstanceName="P_confirm_product"
    ShowPageScrollbarWhenModal="true" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" Font-Bold="False" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
    CssPostfix="BlackGlass">
    <HeaderStyle Font-Bold="True" Font-Size="Medium" />
    <HeaderTemplate>
        Confirm Save
    </HeaderTemplate>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <table align="center" width="100%">
                <tr>
                    <td align="center" style="text-align: left">
                        <asp:Label ID="lb_err" runat="server" Font-Names="Tahoma" Font-Size="Medium" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="text-align: left">
                        <asp:Label ID="L_confirm" runat="server" Font-Names="Tahoma" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <table align="center" width="100%">
                <tr>
                    <td align="right" style="text-align: center" width="50%">
                        <dx:ASPxButton ID="B_confirmsave" runat="server" Text="Save" OnClick="B_confirmsave_Click"
                            Width="100px">
                            <ClientSideEvents Click="function(s, e) {
    P_confirm_product.Hide();
    LoadingPanel_Product.Show();
}" />
                        </dx:ASPxButton>
                    </td>
                    <td align="left" style="text-align: center" width="50%">
                        <dx:ASPxButton ID="B_confirmcancel" runat="server" Text="Cancel" OnClick="B_confirmcancel_Click"
                            Width="100px">
                            <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_Product.Show();
}" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="Popup_ScrProd" ClientInstanceName="Popup_ScrProd" ShowPageScrollbarWhenModal="true"
    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
    HeaderText="Search Product" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="false"
    Modal="True" Width="1200px" Height="100%" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
    CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
    <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
    </LoadingPanelImage>
    <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
    </ContentStyle>
    <HeaderStyle>
        <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
        <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
        </Paddings>
    </HeaderStyle>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
            <table width="100%" align="left">
                <tr>
                    <td align="left" class="style2" style="text-align: right; width: 120px;">
                        <asp:Label ID="Label114" runat="server" Style="font-family: Tahoma; font-size: small;
                            font-weight: 700;" Text="Product Type" Width="100px"></asp:Label>
                    </td>
                    <td class="style6" style="text-align: left;">
                        <div style="float: left">
                            <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodType" DropDownRows="30">
                            </dx:ASPxComboBox>
                            <%--<dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodType"></dx:ASPxTextBox>  --%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style2" style="text-align: right; width: 120px;">
                        <asp:Label ID="Label115" runat="server" Style="font-family: Tahoma; font-size: small;
                            font-weight: 700;" Text="Brand" Width="110px"></asp:Label>
                    </td>
                    <td class="style6" style="text-align: left;">
                        <div style="float: left">
                            <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodBrand" OnSelectedIndexChanged="dd_prodBrand_SelectedIndexChanged"
                                OnTextChanged="dd_prodBrand_TextChanged" AutoPostBack="True" DropDownRows="30"
                                DropDownStyle="DropDown" IncrementalFilteringMode="Contains" BackColor="#FFFFC4"
                                ValueType="System.String">
                            </dx:ASPxComboBox>
                            <%-- <dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodBrand"></dx:ASPxTextBox> --%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style2" style="text-align: right; width: 120px;">
                        <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                            font-weight: 700;" Text="Product code" Width="110px"></asp:Label>
                    </td>
                    <td class="style6" style="text-align: left;">
                        <div style="float: left">
                            <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodcode" OnSelectedIndexChanged="dd_prodcode_SelectedIndexChanged"
                                OnTextChanged="dd_prodcode_TextChanged" AutoPostBack="True" DropDownRows="30"
                                DropDownStyle="DropDown" IncrementalFilteringMode="Contains" BackColor="#FFFFC4"
                                ValueType="System.String">
                            </dx:ASPxComboBox>
                            <%--<dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodcode"></dx:ASPxTextBox>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style2" style="text-align: right; width: 120px;">
                        &nbsp;
                    </td>
                    <td class="style6" style="text-align: left;">
                        <div style="float: left;">
                        </div>
                        <div style="float: left">
                            &nbsp;&nbsp;</div>
                        <div style="float: left;">
                            <dx:ASPxButton ID="btn_src_pdItem" runat="server" Text="Search" Width="100px" OnClick="btn_src_pdItem_Click"
                                CausesValidation="true">
                                <ClientSideEvents Click="function(s, e) {LoadingPanel_Product.Show(); }"></ClientSideEvents>
                            </dx:ASPxButton>
                        </div>
                        <div style="float: left">
                            &nbsp;&nbsp;</div>
                        <div style="float: left">
                            <dx:ASPxButton ID="btn_cancel_prod" runat="server" Text="Cancel" Width="100px" OnClick="btn_cancel_prod_Click">
                            </dx:ASPxButton>
                        </div>
                        <asp:Label ID="lb_res_search" runat="server" Style="font-family: Tahoma; font-size: small;
                            font-weight: 700; color: red;" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style2" style="text-align: left; width: 120px;" colspan="2">
                        &nbsp;
                    </td>
                    <td class="style6" style="text-align: left;">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <div style="overflow-y: scroll; height: 300px">
                            <asp:GridView ID="gv_item" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                                ForeColor="Black" GridLines="Vertical" Width="100%" EmptyDataText="No records found"
                                OnRowCommand="gv_item_RowCommand">
                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                <AlternatingRowStyle BackColor="White" />
                                <FooterStyle BackColor="#CCCC99" />
                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <RowStyle BackColor="#F7F7DE" />
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="SEL"
                                                Text="SEL" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Campaign">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Campaign" runat="server" Text='<%#Eval("C07CMP")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Camp Seq">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Campaign_sql" runat="server" Text='<%#Eval("C07CSQ")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Price" runat="server" Text='<%#Eval("C07PRC")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Down">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Down" runat="server" Text='<%#Eval("C07DOW")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="item code">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_code" runat="server" Text='<%#Eval("T44ITM")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Desc" runat="server" Text='<%#Eval("T44DES")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Type" runat="server" Text='<%#Eval("T44TYP")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Brand">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Brand" runat="server" Text='<%#Eval("T44BRD")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Code44" runat="server" Text='<%#Eval("T44COD")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Model" runat="server" Text='<%#Eval("T44MDL")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Group">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_Group" runat="server" Text='<%#Eval("T44PGP")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MIN Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_MIN" runat="server" Text='<%#Eval("C07MIN")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MAX Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_item_MAX" runat="server" Text='<%#Eval("C07MAX")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:HiddenField ID="hid_CSN" runat="server" />
<asp:HiddenField ID="hid_AppNo" runat="server" />
<asp:HiddenField ID="hid_brn" runat="server" />
<asp:HiddenField ID="hid_status" runat="server" />
<asp:HiddenField ID="hid_appdate" runat="server" />
<asp:HiddenField ID="hid_birthdate" runat="server" />
<asp:HiddenField ID="hid_salary" runat="server" />
<asp:HiddenField ID="hid_idno" runat="server" />
<asp:HiddenField ID="hid_date97" runat="server" />
<asp:HiddenField ID="hid_loantyp" runat="server" />
<dx:ASPxLoadingPanel ID="ASPxLoadingPanel_Product" runat="server" ClientInstanceName="LoadingPanel_Product"
    Height="150px" Width="250px">
</dx:ASPxLoadingPanel>
