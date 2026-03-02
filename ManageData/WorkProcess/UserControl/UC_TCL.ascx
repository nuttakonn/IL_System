<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_TCL.ascx.cs" Inherits="ManageData_WorkProcess_UserControl_UC_TCL" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx1" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx2" %>
    
<style type="text/css">
    .style1
    {
        width: 432px;
    }
    .style2
    {
        width: 415px;
    }
    .style3
    {
        width: 335px;
    }
    .style4
    {
        width: 351px;
    }
    .style5
    {
        width: 413px;
    }
    .style6
    {
        width: 387px;
    }
    .style7
    {
        width: 190px;
    }
    .style15
    {
        width: 379px;
    }
    .style18
    {
        width: 525px;
    }
.dxlpLoadingPanel_Aqua 
{
	font: 9pt Tahoma;
	color: #2C4D79;
	background-color: White;
	border: solid 1px #67A2C6;
}
    .style19
    {
    }
    .style22
    {
        width: 309px;
    }
    .style25
    {
        width: 1089px;
    }
    .style33
    {
        width: 389px;
    }
    .style54
    {
        width: 1961px;
    }
    .style55
    {
        width: 2111px;
    }
</style>
                                                    <asp:Panel ID="P_TCL" 
    runat="server">
                                                        <asp:Panel ID="P_tcl_kessaiview" runat="server">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td class="style4" 
                                                                        style="text-align: left; font-family: Tahoma; font-size: small;">
                                                                        <asp:Label ID="Label33" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Income " 
                                                                            Width="170px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px; height: 24px;">
                                                                        <asp:Label ID="EdtNetIncome" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label41" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                                        <asp:Label ID="LTCL" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="TCL" 
                                                                            Width="170px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                                                        <asp:Label ID="EdtTCL" runat="server" ForeColor="Blue" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label43" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="style4" 
                                                                        style="text-align: left; font-family: Tahoma; font-size: small;">
                                                                        <asp:Label ID="Label37" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                            Text="Easy Buy Card " Width="170px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px; height: 24px;">
                                                                        <asp:Label ID="EdtESBLoan" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label39" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                                        <asp:Label ID="Label40" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                            Text="Approve able Amt" Width="170px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                                                        <asp:Label ID="EAAA" runat="server" ForeColor="Blue" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label45" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="style4" 
                                                                        style="text-align: left; font-family: Tahoma; font-size: small;">
                                                                        <asp:Label ID="LACL" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="ACL " 
                                                                            Width="170px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px; height: 24px;">
                                                                        <asp:Label ID="EdtACL" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label46" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                                        <asp:Label ID="Label211" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                            Text="Approve avaliable " Width="160px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                                                        <asp:Label ID="E_Apv_avi" runat="server" ForeColor="Red" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                            Width="120px"></asp:Label>
                                                                        &nbsp; &nbsp;
                                                                        <asp:Label ID="Label102" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                            Width="120px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td class="style4" 
                                                                    style="text-align: left; font-family: Tahoma; font-size: small;">
                                                                    <asp:Label ID="Label212" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="BOT Loan " Width="170px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px; height: 24px;">
                                                                    <asp:Label ID="EdtBOTLoan" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                        Width="120px"></asp:Label>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="Label213" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                        Width="120px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                                    <asp:Label ID="Label214" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Customer balance" Width="170px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                                                    <asp:Label ID="EdtCrBal" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                                        Width="120px"></asp:Label>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="Label215" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="บาท" 
                                                                        Width="120px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="P_tcl_product" runat="server" Visible="False">
                                                            <table style="width: 100%; ">
                                                                <tr>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:170px;height:24px;">
                                                                        <div style="float:left;">
                                                                        </div>
                                                                        <asp:Label ID="Label62" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                            Text="Group no." Width="130px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                        <div style="float:left;">
                                                                        </div>
                                                                        <dx:ASPxTextBox ID="E_group" runat="server" Enabled="False" Height="24px" 
                                                                            Width="130px">
                                                                            <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                                                                            </DisabledStyle>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                        <div style="float:left;">
                                                                            <asp:Label ID="Label63" runat="server" 
                                                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Rank ." 
                                                                                Width="70px"></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                        <div style="float:left;">
                                                                            <dx:ASPxTextBox ID="E_rank" runat="server" Enabled="False" Height="24px" 
                                                                                Width="130px">
                                                                                <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                                                                                </DisabledStyle>
                                                                            </dx:ASPxTextBox>
                                                                        </div>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                        <asp:Label ID="Label101" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Model" 
                                                                            Width="70px"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                        <dx:ASPxTextBox ID="E_model" runat="server" Enabled="False" Height="24px" 
                                                                            Width="130px">
                                                                            <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                                                                            </DisabledStyle>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:TextBox ID="G_Have_CSMS03" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_TCL_13" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_ACL_13" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_PD" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_Rank_for_GNSR031" runat="server" Visible="False" 
                                                            Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_GROUP_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_RANK_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_ACL_ONGOING_Y" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="P4TYPE" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_NewModel_ZR" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_Model" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_Have_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_Ongoing_TCL" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="EdtCrLmt" runat="server" Visible="False" Width="20px"></asp:TextBox>
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
                                                        <asp:TextBox ID="G_vendor" runat="server" Width="20px" Visible="False"></asp:TextBox>
                                                        <asp:TextBox ID="E_sub_succuss" runat="server" Width="20px" Visible="False"></asp:TextBox>
                                                        <asp:TextBox ID="E_redirect" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_contract_txt" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_reject_cancel" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_RFCM" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_AMLO" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_thainame" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_thaisurname" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="G_birthdate" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="E_product" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="E_vendor" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="p2ucrb" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="p2ubas" runat="server" Visible="False" Width="20px"></asp:TextBox>
                                                        <asp:Label ID="EdtCrAvi" runat="server" ForeColor="Blue" 
                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="0.00" 
                                                            Visible="False" Width="120px"></asp:Label>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width:100px;text-align: left;background-color:#82CAFA;">
                                                                    <asp:Label ID="Label193" runat="server" ForeColor="Blue" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="------ Delivery Document / Eng Name / Nick Name / Ship to Address ------" 
                                                                        Width="481px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="text-align: left;Width:100px;" >
                                                                    <asp:Label ID="Label210" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Ship to :" Width="80px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style55">
                                                                    <dx:ASPxComboBox ID="D_shipto1" runat="server" AutoPostBack="True" 
                                                                        onselectedindexchanged="D_shipto1_SelectedIndexChanged" TabIndex="1" 
                                                                        Width="185px">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td style="width:120px;text-align: left;">
                                                                    <asp:Label ID="Label208" runat="server" Height="20px" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Name-Surname(EN)" Width="137px"></asp:Label>
                                                                </td>
                                                                <td class="style33" style="text-align: left;">
                                                                    <div style="float:left;">
                                                                        <dx:ASPxTextBox ID="E_nameeng1" runat="server" MaxLength="50" TabIndex="2" 
                                                                            Width="130px">
                                                                        </dx:ASPxTextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="style25" style="text-align: left;">
                                                                    <dx:ASPxTextBox ID="E_surnameeng1" runat="server" MaxLength="50" TabIndex="3" 
                                                                        Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td style="text-align: left;" class="style22">
                                                                    <asp:Label ID="Label209" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Nick name" Width="72px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style18">
                                                                    <dx:ASPxTextBox ID="E_nickname" runat="server" MaxLength="20" TabIndex="4" 
                                                                        Width="70px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td class="style18" style="text-align: left;">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td  style="text-align: left;Width:100px;">
                                                                    <asp:Label ID="Label194" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="village :" Width="80px"></asp:Label>
                                                                </td>
                                                                <td class="style55" style="text-align: left;">
                                                                    <dx:ASPxTextBox ID="E_village1" runat="server" MaxLength="50" TabIndex="5" 
                                                                        Width="185px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td class="style19" style="text-align: left;">
                                                                    <asp:Label ID="Label195" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Building :" Width="100px"></asp:Label>
                                                                </td>
                                                                <td class="style33" style="text-align: left;">
                                                                    <div style="float:left;">
                                                                        <dx:ASPxComboBox ID="D_building1" runat="server" TabIndex="6" Width="130px">
                                                                        </dx:ASPxComboBox>
                                                                    </div>
                                                                </td>
                                                                <td class="style25" style="text-align: left;">
                                                                    <dx:ASPxTextBox ID="E_buildingname1" runat="server" TabIndex="7" Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td class="style22" style="text-align: left;">
                                                                    <asp:Label ID="Label196" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Address No:" Width="80px"></asp:Label>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    <div style="float:left;">
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        <dx:ASPxTextBox ID="E_addressno1" runat="server" MaxLength="10" TabIndex="8" 
                                                                            Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        <asp:Label ID="Label197" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Room:" 
                                                                            Width="40px"></asp:Label>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                    </div>
                                                                    <dx:ASPxTextBox ID="E_roomno1" runat="server" MaxLength="10" TabIndex="9" 
                                                                        Width="30px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left;Width:100px;">
                                                                    <asp:Label ID="Label198" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Floor :" 
                                                                        Width="100px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style55">
                                                                    <div style="float:left;">
                                                                        <dx:ASPxTextBox ID="E_floor1" runat="server" TabIndex="10" Width="70px" 
                                                                            MaxLength="3">
                                                                        </dx:ASPxTextBox>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        <asp:Label ID="Label199" runat="server" 
                                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Moo:" 
                                                                            Width="40px"></asp:Label>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                    </div>
                                                                    <dx:ASPxTextBox ID="E_moo1" runat="server" TabIndex="11" Width="70px" 
                                                                        MaxLength="2">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td style="text-align: left;" class="style19">
                                                                    <asp:Label ID="Label200" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Soi :" 
                                                                        Width="100px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style33">
                                                                    <dx:ASPxTextBox ID="E_soi1" runat="server" TabIndex="12" Width="130px" 
                                                                        MaxLength="30">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td class="style25" style="text-align: left;">
                                                                    &nbsp;</td>
                                                                <td style="text-align: left;" class="style22">
                                                                    <asp:Label ID="Label201" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Road :" 
                                                                        Width="100px"></asp:Label>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    <dx:ASPxTextBox ID="E_road1" runat="server" TabIndex="13" Width="130px" 
                                                                        MaxLength="30">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td  style="text-align: left;Width:100px;">
                                                                    <asp:Label ID="Label202" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Tambol :" Width="80px"></asp:Label>
                                                                </td>
                                                                <td class="style55" style="text-align: left;">
                                                                    <dx:ASPxComboBox ID="D_tambol1" runat="server" AutoPostBack="True" 
                                                                        DropDownStyle="DropDown" ontextchanged="D_tambol1_TextChanged" 
                                                                        TabIndex="15" Enabled="False">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td class="style19" style="text-align: left;">
                                                                    <asp:Label ID="Label203" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Amphur :" Width="100px"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style33">
                                                                    <div style="float:left; width: 2px;">
                                                                    </div>
                                                                    <dx:ASPxComboBox ID="D_amphur1" runat="server" AutoPostBack="True" 
                                                                        DropDownStyle="DropDown" ontextchanged="D_amphur1_TextChanged" 
                                                                        TabIndex="16" Width="130px" Enabled="False">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td class="style25" style="text-align: left;">
                                                                    &nbsp;</td>
                                                                <td style="text-align: left;" class="style22">
                                                                    <asp:Label ID="Label204" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Province:" Width="80px"></asp:Label>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    <dx:ASPxComboBox ID="D_province1" runat="server" TabIndex="17" Enabled="False">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td style="width:200px;text-align: left;">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left;Width:100px;" >
                                                                    <asp:Label ID="Label205" runat="server" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                        Text="Postcode:" Width="65px"></asp:Label>
                                                                </td>
                                                                <td class="style55" style="text-align: left;">
                                                                    <dx:ASPxComboBox ID="D_zipcode1" runat="server" TabIndex="18" Enabled="False">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td class="style19" style="text-align: left;">
                                                                    <asp:Label ID="L_count" runat="server" Font-Size="Small" 
                                                                        style="text-align: right" Visible="False"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left;" class="style33">
                                                                    <dx:ASPxComboBox ID="C_address_I" runat="server" autopostback="True" 
                                                                        backcolor="#FFFFC4" dropdownrows="15" dropdownstyle="DropDown" 
                                                                        incrementalfilteringmode="Contains" ontextchanged="C_address_I_TextChanged" 
                                                                        tabindex="14" valuetype="System.String" visible="False" width="100px" 
                                                                        xmlns:dx="devexpress.web.aspxloadingpanel">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td class="style25" style="text-align: left;">
                                                                    &nbsp;</td>
                                                                <td style="text-align: left;" class="style22">
                                                                    &nbsp;</td>
                                                                <td style="width:200px;text-align: left;">
                                                                    &nbsp;</td>
                                                                <td style="width:200px;text-align: left;">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" 
                                                                    style="text-align: left; font-family: Tahoma; font-size: small;height:24px; ">
                                                                    <div style="float:left;">
                                                                        <dx:ASPxButton ID="B_approve" runat="server" Text="APPROVE" 
                                                                            onclick="B_approve_Click">
                                                                            <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_TCL.Show();
}" />
                                                                        </dx:ASPxButton>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        &nbsp;&nbsp;&nbsp;</div>
                                                                    <div style="float:left;">
                                                                        <dx:ASPxButton ID="B_reject" runat="server" Text="REJECT" 
                                                                            onclick="B_reject_Click">
                                                                            <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_TCL.Show();
}" />
                                                                        </dx:ASPxButton>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        &nbsp;&nbsp;&nbsp;</div>
                                                                    <div style="float:left;">
                                                                        <dx:ASPxButton ID="B_cancel" runat="server" Text="CANCEL" 
                                                                            onclick="B_cancel_Click">
                                                                            <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_TCL.Show();
}" />
                                                                        </dx:ASPxButton>
                                                                    </div>
                                                                    <div style="float:left;">
                                                                        &nbsp;&nbsp;&nbsp;</div>
                                                                    <div style="float:left;">
                                                                        <dx:ASPxButton ID="B_return" runat="server" Text="RETURN TO INTERVIEW" 
                                                                            onclick="B_return_Click">
                                                                            <ClientSideEvents Click="function(s, e) {
	
    LoadingPanel_TCL.Show();
}" />
                                                                        </dx:ASPxButton>
                                                                        <asp:HiddenField ID="hid_pending" runat="server" />
                                                                    </div>
                                                                </td>
                                                                <td class="style19" colspan="2">
                                                                    <asp:Label ID="L_Interview_status" runat="server" ForeColor="Blue" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                                                                </td>
                                                                
                                                                <td class="style22">
                                                                    <asp:Label ID="L_othmsg" runat="server" ForeColor="Red" 
                                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="text-align: left; font-family: Tahoma; font-size: small;height:24px; ">
                                                                    &nbsp;</td>
                                                                <td class="style19" colspan="2">
                                                                   
                                                                </td>
                                                                
                                                                <td class="style22">
                                                                    
                                                                </td>
                                                            </tr>

                                                        </table>
</asp:Panel>




                                                 





                                          <p>




                                                 





                                          <dx:ASPxPopupControl ID="P_note_TCL" 
    runat="server" Height="146px" Width="600px" AllowDragging="True" 
    AutoUpdatePosition="True" ClientInstanceName="P_note_TCL" ShowPageScrollbarWhenModal="true"
    CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Font-Bold="False" 
    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" HeaderText = "Confirm Save">
                                              <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <table align="center" width="100%">
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="Label206" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Action Code" Width="80px"></asp:Label>
            </td>
            <td align="left">
                <div style="float:left">
                    <dx:ASPxComboBox ID="D_action" runat="server" BackColor="#FFFFC4" 
                        ValueType="System.String" Width="300px" incrementalfilteringmode="StartsWith"  >
                    </dx:ASPxComboBox>
                </div>
                <%--<div style="float:right">
                    <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" Text="Refresh code"
                    Width="120px" Height="15px" AutoPostBack="true" ImagePosition="Right" 
                        Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15" 
                        OnClick="btnLinkImageAndText_Click">
                        <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                        </Image>
                    </dx:ASPxButton>
                </div>--%>
                
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="Label207" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Reason Code" Width="100px"></asp:Label>
            </td>
            <td align="left">
                <dx:ASPxComboBox ID="D_reason" runat="server" AutoPostBack="True" 
                    BackColor="#FFFFC4" DropDownRows="12" DropDownStyle="DropDown" 
                    IncrementalFilteringMode="StartsWith" ValueType="System.String" Width="300px">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="text-align: left">
                <dx:ASPxMemo ID="E_note_TCL" runat="server" Height="51px" Width="100%" Rows="5" ></dx:ASPxMemo>
                <%--<asp:TextBox ID="E_note_TCL" runat="server" Height="51px" Width="100%"></asp:TextBox>--%>
            </td>
        </tr>
    </table>
    <asp:Label ID="L_msg_note_TCL" runat="server" Font-Names="Tahoma" 
        Font-Size="Small" ForeColor="Red"></asp:Label>
       
    <br />
    <table align="center" width="100%">
        <tr>
            <td align="right" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_savenote_TCL" runat="server" OnClick="B_savenote_TCL_Click" 
                    Text="Save" Width="100px">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel_TCL.Show();
}" />
                </dx:ASPxButton>
            </td>
            <td align="left" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_cancelnote_TCL" runat="server" 
                    OnClick="B_cancelnote_TCL_Click" Text="Cancel" Width="100px">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel_TCL.Show();
}" />
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td align="right" style="text-align: center" width="50%">
               &nbsp;
            </td>
            <td align="left" style="text-align: center" width="50%">
                 
            </td>
        </tr>

    </table>
                                                  </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>




                                                 





                                          </p>
<p>




                                                 





                                          <dx:ASPxPopupControl ID="P_message_TCL" 
    runat="server"  Width="450px" AllowDragging="True" 
    AutoUpdatePosition="True" ClientInstanceName="P_message_TCL" ShowPageScrollbarWhenModal="true"
    CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Font-Bold="False" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
                                                  CssPostfix="BlackGlass" RenderMode="Lightweight">
                                                   <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
                                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                                              <HeaderTemplate>
                                                  Message
                                              </HeaderTemplate>
                                              <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <table align="center" width="100%">
        <tr>
            <td align="left">
                <%--<asp:Label ID="L_message" runat="server" Font-Size="Small" Font-Names="Tahoma"  ForeColor="Blue" ></asp:Label>--%>
                <dx:ASPxLabel ID="L_message" runat="server" Font-Size="Small" Font-Names="Tahoma"  ForeColor="Blue" ></dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="L_message1" runat="server" Font-Names="Tahoma" Font-Size="Small" 
                    ForeColor="Blue" style="font-weight: 700"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
            <dx:ASPxLabel ID="L_contract" runat="server" Font-Size="Medium" Font-Bold="true" Font-Names="Tahoma"  ForeColor="#006633"></dx:ASPxLabel>
                <%--<aspx:Label ID="L_contract" runat="server" Font-Names="Tahoma" Font-Size="Small" 
                    ForeColor="Blue" style="font-weight: 700"></asp:Label>--%>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td align="right" style="text-align: center; width: 100%;" width="50%">
                <dx:ASPxButton ID="B_confirmok" runat="server" OnClick="B_confirmok_Click" 
                    Text="OK" Width="100px">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel_TCL.Show();
}" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                                                  </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>




                                                 





                                          </p>




                                                 





                                          <dx:ASPxPopupControl ID="P_confirm_TCL" 
    runat="server" Height="146px" Width="374px" AllowDragging="True" 
    AutoUpdatePosition="True" ClientInstanceName="P_confirm_TCL" ShowPageScrollbarWhenModal="true"
    CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Font-Bold="False" 
    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
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
                <asp:Label ID="lb_backlist" runat="server" ForeColor="Red" 
                    style="font-family: Tahoma; font-size: medium; font-weight: 700;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="L_confirm_info" runat="server" Font-Names="Tahoma" 
                    Font-Size="Medium" ForeColor="#00008B" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: left">
                    <dx:ASPxLabel ID="L_NCB" runat="server" Font-Size="Small" Font-Bold="true" Font-Names="Tahoma"  ForeColor="Red"></dx:ASPxLabel>
                   <dx:ASPxLabel ID="L_confirm" runat="server" Font-Size="Small" Font-Bold="true" Font-Names="Tahoma"  ForeColor="Red"></dx:ASPxLabel>
                <%--<asp:Label ID="L_confirm" runat="server" Font-Names="Tahoma" Font-Size="Small" 
                    ForeColor="Red"></asp:Label>--%>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td align="right" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_confirmsave" runat="server" OnClick="B_confirmsave_Click" 
                    Text="Save" Width="100px">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel_TCL.Show();
}" />
                </dx:ASPxButton>
            </td>
            <td align="left" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_confirmcancel" runat="server" 
                    OnClick="B_confirmcancel_Click" Text="Cancel" Width="100px">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel_TCL.Show();
}" />
                </dx:ASPxButton>
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
                                          <asp:HiddenField ID="hid_appdate" 
    runat="server" />
                                          <asp:HiddenField ID="hid_birthdate" 
    runat="server" />
                                      





                                                 





                                          <asp:HiddenField ID="hid_salary" 
    runat="server" />
                                      





                                                 





                                          <asp:HiddenField ID="hid_idno" 
    runat="server" />
                                          
                                               <asp:HiddenField ID="hid_loantyp" 
    runat="server" />                                 
                                               <asp:HiddenField ID="hid_inteirsum" 
    runat="server" /> 
                                                   <asp:HiddenField ID="hid_crueirsum" 
    runat="server" /> 
                                                   <asp:HiddenField ID="hid_installment" 
    runat="server" /> 
                                                   <asp:HiddenField ID="hid_lastinstallment" 
    runat="server" /> 




                                                 





                                          <asp:HiddenField ID="hid_date97" 
    runat="server" />
<asp:HiddenField ID="hfM13FIL"  runat="server" />
<asp:HiddenField ID="hfM13BUT"  runat="server" />
                                          
                                      





                                                 





<asp:Panel ID="P_product" runat="server" Visible="False">
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Total Term" Width="120px" ID="Label68" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                                                                class="style2">
                <dx:ASPxComboBox runat="server" DropDownRows="10" ValueType="System.String" 
                                                                    Width="100px" 
                    AutoPostBack="True" ID="dd_totalterm">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText="">
                        </RequiredField>
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Approve Limit" Width="130px" ID="Label66" 
                                                                    Visible="False" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                <asp:Label runat="server" ForeColor="Red" Width="130px" ID="lb_pApproveL" 
                                                                    Visible="False" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Vendor" Width="120px" ID="Label70" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                                                                class="style2">
                <div style="float:left">
                    <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" 
                                                                        
                        IncrementalFilteringMode="Contains" DropDownRows="10" ValueType="System.String" 
                                                                        Width="450px" 
                        AutoPostBack="True" TabIndex="18" ID="dd_vendor">
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RegularExpression ErrorText="">
                            </RegularExpression>
                            <RequiredField IsRequired="True" ErrorText="">
                            </RequiredField>
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </div>
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="40px" Enabled="False" ID="txt_rank_v">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Campaign Type" Width="130px" ID="Label71" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                <dx:ASPxTextBox runat="server" Width="130px" Text="R" Height="24px" 
                                                                    Enabled="False" 
                    ID="txt_campgType">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Campaign" Width="120px" ID="Label72" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                                                                class="style2">
                <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" 
                                                                    
                    IncrementalFilteringMode="Contains" DropDownRows="10" ValueType="System.String" 
                                                                    Width="450px" 
                    AutoPostBack="True" TabIndex="19" ID="dd_campaign">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText="">
                        </RequiredField>
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Campaign Seq." Width="130px" ID="Label73" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" 
                                                                    
                    ClientInstanceName="txt_campSeq" ID="txt_campSeq">
                    <ValidationSettings ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText="">
                        </RequiredField>
                    </ValidationSettings>
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Product" Width="120px" ID="Label74" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                                                                class="style2">
                <dx:ASPxComboBox runat="server" DropDownStyle="DropDown" 
                                                                    
                    IncrementalFilteringMode="Contains" DropDownRows="10" ValueType="System.String" 
                                                                    Width="450px" 
                    AutoPostBack="True" TabIndex="20" ID="dd_product">
                    <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                        <RequiredField IsRequired="True" ErrorText="">
                        </RequiredField>
                    </ValidationSettings>
                </dx:ASPxComboBox>
                <asp:Label runat="server" ForeColor="Blue" Height="16px" Width="530px" 
                                                                    ID="lb_prodcount" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Payment ability" Width="130px" ID="Label75" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                <dx:ASPxTextBox runat="server" Width="130px" Height="24px" Enabled="False" 
                                                                    ID="txt_pay_abl">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label runat="server" Text="Price/item" Width="120px" ID="Label77" 
                                                                    
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                                                                class="style2">
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="100px" Height="24px" ID="txt_price">
                        <MaskSettings Mask="&lt;0..99999g&gt;.&lt;00..99&gt;" AllowMouseWheel="False">
                        </MaskSettings>
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RequiredField IsRequired="True" ErrorText="">
                            </RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <asp:Label runat="server" Text="Total Quatity" Width="94px" ID="Label80" 
                                                                        
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="70px" Height="24px" ID="txt_qty">
                        <MaskSettings Mask="&lt;1..100&gt;" AllowMouseWheel="False">
                        </MaskSettings>
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RequiredField IsRequired="True" ErrorText="">
                            </RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <asp:Label runat="server" Text="Down" Width="94px" ID="Label1" 
                                                                        
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="70px" Height="24px" ID="txt_down" 
                                                                        Text="0">
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RequiredField IsRequired="True" ErrorText="">
                            </RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <dx:ASPxButton runat="server" 
                                                                         
                    SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Calculate" 
                                                                         
                    ValidationGroup="v_cal_prod" CssPostfix="Office2003Blue" 
                                                                         
                    CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" Width="90px" 
                                                                         Height="24px" 
                    ID="btn_cal_TCL" OnClick="btn_cal_TCL_Click">
                </dx:ASPxButton>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                &nbsp;</td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label ID="Label76" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Total range" Width="120px"></asp:Label>
            </td>
            <td class="style1" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_total_range" runat="server" Enabled="False" 
                        Height="24px" Width="100px">
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <asp:Label ID="Label78" runat="server" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Non" 
                        Width="30px"></asp:Label>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_non" runat="server" Enabled="False" Height="24px" 
                        Width="50px">
                        <ValidationSettings ErrorText="" ValidationGroup="v_cal_prod">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <asp:Label ID="Label79" runat="server" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Due" 
                        Width="50px"></asp:Label>
                </div>
            </td>
            <td class="style6" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label81" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Total Purchase" Width="130px"></asp:Label>
            </td>
            <td class="style3" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_purch" runat="server" Enabled="False" Height="24px" 
                    Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <asp:Label ID="Label85" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="First due amount" Width="120px"></asp:Label>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_fDue_AMT" runat="server" Enabled="False" Height="24px" 
                    Width="100px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label ID="Label82" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Min price" Width="120px"></asp:Label>
            </td>
            <td class="style1" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_minPrice" runat="server" 
                        ClientInstanceName="txt_minPrice" Enabled="False" Height="24px" Width="100px">
                        <ValidationSettings ErrorText="">
                        </ValidationSettings>
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_maxPrice" runat="server" 
                        ClientInstanceName="txt_maxPrice" Enabled="False" Height="24px" Width="100px">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td class="style6" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label84" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Loan request" Width="130px"></asp:Label>
            </td>
            <td class="style3" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_loanReq" runat="server" Enabled="False" Height="24px" 
                    Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Red">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <asp:Label ID="Label86" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="First due date" Width="130px"></asp:Label>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_fDue_date" runat="server" Enabled="False" Height="24px" 
                    Width="130px">
                    <MaskSettings Mask="00/00/0000" />
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label ID="Label87" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Duty stamp" Width="120px"></asp:Label>
            </td>
            <td class="style1" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_duty" runat="server" Enabled="False" Height="24px" 
                        Width="100px">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    &nbsp;&nbsp;</div>
                <div style="float:left">
                    <asp:Label ID="Label88" runat="server" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                        Text="Int.Avg %" Width="99px"></asp:Label>
                </div>
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_Int" runat="server" Enabled="False" Height="24px" 
                        Width="90px">
                        <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                        </DisabledStyle>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td class="style6" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label89" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Cr.Usg Avg%" Width="130px"></asp:Label>
            </td>
            <td class="style3" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_cru" runat="server" Enabled="False" Height="24px" 
                    Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <asp:Label ID="Label90" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Credit bureau" Width="120px"></asp:Label>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <dx:ASPxTextBox ID="txt_bureau" runat="server" Enabled="False" Height="24px" 
                    Width="100px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                <asp:Label ID="Label92" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Contract Amount" Width="120px"></asp:Label>
            </td>
            <td class="style1" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                </div>
                <dx:ASPxTextBox ID="txt_contractAmt" runat="server" Enabled="False" 
                    Height="24px" Width="130px">
                    <DisabledStyle BackColor="LightGray" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxTextBox>
            </td>
            <td class="style6" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                &nbsp;</td>
            <td class="style3" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                <div style="float:left;">
                </div>
            </td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                &nbsp;</td>
            <td class="style5" 
                style="text-align: left; font-family: Tahoma; font-size: small;">
                &nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="gvTerm" runat="server" AutoGenerateColumns="False" 
        BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
        CellPadding="4" EmptyDataText="No records found" ForeColor="Black" 
        GridLines="Vertical" Width="99%">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="From Term">
                <ItemTemplate>
                    <asp:Label ID="lb_Fterm" runat="server" Text='<%#Eval("from_T")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="To Term">
                <ItemTemplate>
                    <asp:Label ID="lb_Tterm" runat="server" Text='<%#Eval("to_T")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Int%">
                <ItemTemplate>
                    <asp:Label ID="lb_intP" runat="server" Text='<%#Eval("int_p")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Int Amount">
                <ItemTemplate>
                    <asp:Label ID="lb_intAMT" runat="server" Text='<%#Eval("int_amt")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cru Amonth">
                <ItemTemplate>
                    <asp:Label ID="lb_cruAmonth" runat="server" Text='<%#Eval("cru_amt")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Amonth">
                <ItemTemplate>
                    <asp:Label ID="lb_totalAmt" runat="server" Text='<%#Eval("total_amt")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Oth">
                <ItemTemplate>
                    <asp:Label ID="lb_OTH" runat="server" Text='<%#Eval("oth")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PRINCIPAL">
                <ItemTemplate>
                    <asp:Label ID="lb_PRINCIPAL" runat="server" Text='<%#Eval("PRINCIPAL")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="INSTALL">
                <ItemTemplate>
                    <asp:Label ID="lb_INSTALL" runat="server" Text='<%#Eval("INSTALL")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DUTY STAMP">
                <ItemTemplate>
                    <asp:Label ID="lb_DUTY_STAMP" runat="server" Text='<%#Eval("DUTY_STAMP")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="INTEREST_BASE">
                <ItemTemplate>
                    <asp:Label ID="lb_INTEREST_BASE" runat="server" 
                        Text='<%#Eval("INTEREST_BASE")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CR.USAGE BASE">
                <ItemTemplate>
                    <asp:Label ID="lb_CR_USAGE" runat="server" Text='<%#Eval("CR_USAGE_BASE")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="INIT.FEE BASE">
                <ItemTemplate>
                    <asp:Label ID="lb_INIT_FEE_BASE" runat="server" 
                        Text='<%#Eval("INIT_FEE_BASE")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CONTRACT">
                <ItemTemplate>
                    <asp:Label ID="lb_CONTRACT" runat="server" Text='<%#Eval("CONTRACT_AMOUNT")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FIRSTDUE">
                <ItemTemplate>
                    <asp:Label ID="lb_FIRSTDUE" runat="server" Text='<%#Eval("FIRST_DATE")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="AVG_INTERATE">
                <ItemTemplate>
                    <asp:Label ID="lb_AVG_INTERATE" runat="server" Text='<%#Eval("AVG_INTEREST")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="AVG_CREDIT_USAGE">
                <ItemTemplate>
                    <asp:Label ID="lb_AVG_CREDIT_USAGE" runat="server" 
                        Text='<%#Eval("AVG_CR_USAGE")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CREDIT BUREAU">
                <ItemTemplate>
                    <asp:Label ID="lb_CREDIT_BUREAU" runat="server" 
                        Text='<%#Eval("CREDIT_BUREAU")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataRowStyle BackColor="#D4668D" Font-Bold="True" ForeColor="White" 
            HorizontalAlign="Center" />
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="Black" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:GridView ID="gv_install" runat="server" AutoGenerateColumns="False" 
        BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
        CellPadding="4" EmptyDataText="No records found" ForeColor="Black" 
        GridLines="Vertical" Width="99%">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Term">
                <ItemTemplate>
                    <asp:Label ID="lb_term" runat="server" Text='<%#Eval("term")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Begin">
                <ItemTemplate>
                    <asp:Label ID="lb_begin" runat="server" Text='<%#Eval("begin")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Principal">
                <ItemTemplate>
                    <asp:Label ID="lb_principal0" runat="server" Text='<%#Eval("princ")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Installment">
                <ItemTemplate>
                    <asp:Label ID="lb_Installment" runat="server" Text='<%#Eval("intstallment")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Interest">
                <ItemTemplate>
                    <asp:Label ID="lb_interest" runat="server" Text='<%#Eval("interest")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cr. Usage">
                <ItemTemplate>
                    <asp:Label ID="lb_CrUsg" runat="server" Text='<%#Eval("cr_use")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Income">
                <ItemTemplate>
                    <asp:Label ID="lb_income" runat="server" Text='<%#Eval("income")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cur. Principal">
                <ItemTemplate>
                    <asp:Label ID="lb_CPrincipal" runat="server" Text='<%#Eval("cur_princ")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Amonth">
                <ItemTemplate>
                    <asp:Label ID="lb_amonth" runat="server" Text='<%#Eval("amt")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataRowStyle BackColor="#D4668D" Font-Bold="True" ForeColor="White" 
            HorizontalAlign="Center" />
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="LightSkyBlue" Font-Bold="True" ForeColor="Black" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
</asp:Panel>
                                                    




                                                 





                                          
                                          
                                      





                                                 





<dx:ASPxLoadingPanel ID="ASPxLoadingPanel_TCL" runat="server" 
    ClientInstanceName="LoadingPanel_TCL" Height="150px" Width="250px">
</dx:ASPxLoadingPanel>