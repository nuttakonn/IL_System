<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Verify_Call.ascx.cs" Inherits="ManageData_WorkProcess_UserControl_UC_Verify_Call" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>
<script type="text/javascript" src='<%=ResolveUrl("~/Js/shortcut.js")%>' ></script>
<script  type="text/javascript"  src='<%=ResolveUrl("~/Js/ProtectJs.js")%>' ></script>

<style type="text/css">
    .style3 {
        width: 233px;
    }

    .style8 {
        height: 24px;
        width: 187px;
    }

    .style10 {
        height: 24px;
        width: 180px;
    }

    .style11 {
        width: 504px;
    }

    .style13 {
        width: 504px;
        height: 32px;
    }

    .style14 {
        width: 500px;
        height: 32px;
    }

    .style15 {
        height: 24px;
        width: 292px;
    }
</style>
<table style="width: 100%">
      <tr>
        <td style="width: 200px; text-align: left; height: 24px;">
            <asp:Label ID="Label6" runat="server"
                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="Company type" Width="150px">
            </asp:Label>
        </td>
        <td style="width: 150px; text-align: left; height: 24px;">
            <dx:ASPxComboBox ID="ddlCompanyType" runat="server" Width="250px" 
                IncrementalFilteringMode="StartsWith">
                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxComboBox>
        </td>
        <td colspan="2" style="width: 450px; text-align: left; height: 24px;">
            <div style="float: left">
            </div>
            <div style="float: left">&nbsp;&nbsp;</div>
            <div style="float: left">
            </div>
        </td>

        <td style="width: 150px; text-align: left; height: 24px;">
            &nbsp;</td>
        <td style="width: 200px; text-align: left; height: 24px;">
            <div style="float: right">
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 200px; text-align: left; height: 24px;">
            <asp:Label ID="Label146" runat="server"
                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="Customer type" Width="150px">
            </asp:Label>
        </td>
        <td style="width: 150px; text-align: left; height: 24px;">
            <dx:ASPxComboBox ID="dd_cust_type" runat="server" Width="250px" AutoPostBack="true"
                OnSelectedIndexChanged="dd_cust_type_SelectedIndexChanged"
                IncrementalFilteringMode="StartsWith">
                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                </DisabledStyle>
            </dx:ASPxComboBox>
        </td>
        <td colspan="2" style="width: 450px; text-align: left; height: 24px;">
            <div style="float: left">
                <asp:Label ID="Label156" runat="server"
                    Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                    Text="Sub Type" Width="60px">
                </asp:Label>
            </div>
            <div style="float: left">&nbsp;&nbsp;</div>
            <div style="float: left">
                <dx:ASPxComboBox ID="dd_subType" runat="server" Width="250px"
                    IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="Silver" ForeColor="Blue">
                    </DisabledStyle>
                </dx:ASPxComboBox>
            </div>
        </td>

        <td style="width: 150px; text-align: left; height: 24px;">
            <dx:ASPxButton ID="btn_saveCust" runat="server" Text="Save Customer"
                Width="120px" Height="18px"
                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                OnClick="btn_saveCust_Click">
            </dx:ASPxButton>
        </td>
        <td style="width: 200px; text-align: left; height: 24px;">
            <div style="float: right">
                <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" Text="Refresh code"
                    Width="120px" Height="15px" AutoPostBack="true" ImagePosition="Right"
                    Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15"
                    OnClick="btnLinkImageAndText_Click">

                    <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                    </Image>

                </dx:ASPxButton>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="3"
            style="border-right: thin dotted #C0C0C0; text-align: left; background-color: #82CAFA;"
            class="style11">
            <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="----- TO -----" Width="500px" ForeColor="Blue">
            </asp:Label>
        </td>
        <td colspan="3" style="width: 500px; text-align: left; background-color: #82CAFA;">
            <asp:Label ID="Label125" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="----- TH -----" Width="500px" ForeColor="Blue">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="3"
            style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label2" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="1. ลักษณะการรับเงินเดือน" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_recieveMoney" runat="server" Width="250px"
                            TabIndex="1" IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>

                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 101%">
                <tr>
                    <td style="width: 220px; height: 24px;" align="left">
                        <dx:ASPxCheckBox runat="server" ID="cb_notHave_TH" Text="Not have TH"
                            Font-Bold="true" AutoPostBack="True"
                            OnCheckedChanged="cb_notHave_TH_CheckedChanged" TabIndex="25">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxCheckBox>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label3" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="2. รายได้ต่อเดือน(บาท)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_salary_TO" runat="server" Width="150px"
                            HorizontalAlign="Left" TabIndex="2">
                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..9999999999999g&gt;" />
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; height: 24px;" align="left">
                        <asp:Label ID="Label149_" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="15. เบอร์ติดต่อที่อยู่ปัจจุบัน" Width="220px"></asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_TH" runat="server" Width="70px" HorizontalAlign="Left"
                                TabIndex="26">
                                <MaskSettings AllowMouseWheel="False" Mask="999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label131" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="-" Width="5px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_2_TH" runat="server" Width="35px"
                                HorizontalAlign="Left" MaxLength="4" TabIndex="27">
                                <MaskSettings Mask="9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label132_" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Ext." Width="20px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_ext_TH" runat="server" Width="50px"
                                HorizontalAlign="Left" TabIndex="28" MaxLength="15">

                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style13">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label12" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="3. ผลการเช็คประกันสังคม(SSO)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_sso" runat="server" Width="250px" TabIndex="3"
                            IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; height: 24px;" align="left">
                        <asp:Label ID="Label42_" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="17. ผลการตรวจสอบ(ชื่อ)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left;">
                            <dx:ASPxComboBox ID="dd_chkName_TH" runat="server" Width="120px" TabIndex="30"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_res_name_TH" runat="server" Width="130px"
                                HorizontalAlign="Left" TabIndex="31" MaxLength="50">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label7" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="4. ผลการเช็ค BOL" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_BOL" runat="server" Width="250px" TabIndex="4"
                            IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label42" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="18. ผลการตรวจสอบ(ที่อยู่)" Width="165px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left;">
                            <dx:ASPxComboBox ID="dd_chk_addr_TH" runat="server" Width="120px"
                                TabIndex="32" IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left;">
                        </div>
                        <dx:ASPxTextBox ID="txt_res_addr_TH" runat="server" Width="130px"
                            HorizontalAlign="Left" TabIndex="33">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label124" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="5. เบอร์ติดต่อที่ทำงาน(สนญ.)" Width="180px"></asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_off_TO" runat="server" Width="80px" HorizontalAlign="Left"
                                TabIndex="5">
                                <MaskSettings AllowMouseWheel="False" Mask="999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" ValidationExpression="^[0-9]{9}$" />
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label9" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="-" Width="5px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_off2_TO" runat="server" Width="35px"
                                HorizontalAlign="Left" TabIndex="6" MaxLength="4">
                                <MaskSettings AllowMouseWheel="False" Mask="9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" />
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label132" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Ext." Width="20px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_off_ext_TO" runat="server" Width="50px"
                                HorizontalAlign="Left" TabIndex="7" MaxLength="15">
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" />
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label151" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="19.ลักษณะเบอร์โทร" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_type_tel" runat="server" Width="250px" TabIndex="34"
                            IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label127" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="6.เบอร์ติดต่อสาขาที่ประจำ" Width="180px"></asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_brn_tel" runat="server" Width="80px"
                                HorizontalAlign="Left" TabIndex="8">
                                <MaskSettings AllowMouseWheel="False" Mask="999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label128" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="-" Width="5px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel2_brn" runat="server" Width="35px"
                                HorizontalAlign="Left" TabIndex="9" MaxLength="4">
                                <MaskSettings AllowMouseWheel="False" Mask="9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label133" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Ext." Width="20px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel_ext_brn" runat="server" Width="50px"
                                HorizontalAlign="Left" TabIndex="10" MaxLength="15">
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RegularExpression ErrorText="" />
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label152" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="20. ผลการติดต่อ TH" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_resContact_TH" runat="server" Width="250px"
                            TabIndex="35" IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue"></DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label104" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="35. เบอร์ที่ทำงาน(Mobile)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_tel_off_mobil" runat="server" Width="80px"
                            HorizontalAlign="Left" MaxLength="10" TabIndex="11">
                            <MaskSettings AllowMouseWheel="False" Mask="9999999999" />
                            <ValidationSettings>
                                <RegularExpression ErrorText="" ValidationExpression="^[0-9]*$" />
                            </ValidationSettings>
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label161" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="21. ผู้ให้ข้อมูล TH" Width="148px"></asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxComboBox ID="dd_person_TH" runat="server" Width="150px" TabIndex="36"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_dd_person_TH" runat="server" Width="100px"
                                HorizontalAlign="Left" TabIndex="37" MaxLength="50">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label137" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="8.ผลการตรวจสอบ(ชื่อ)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left;">
                            <dx:ASPxComboBox ID="dd_chkName" runat="server" Width="120px" TabIndex="13"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_res_name_TO" runat="server" Width="130px"
                                HorizontalAlign="Left" TabIndex="14" MaxLength="50">
                                <ReadOnlyStyle BackColor="Silver">
                                </ReadOnlyStyle>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: left;" class="style3"></td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: right">
                            <dx:ASPxButton ID="btn_saveTH" runat="server" Text="Save TH" Width="100px" Height="18px"
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                OnClick="btn_saveTH_Click">
                            </dx:ASPxButton>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label135" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="9. ผลการตรวจสอบ(ที่อยู่)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left;">
                            <dx:ASPxComboBox ID="dd_chkAddr" runat="server" Width="120px" TabIndex="15"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_res_addr_TO" runat="server" Width="130px"
                                HorizontalAlign="Left" TabIndex="16" MaxLength="50">
                                <ReadOnlyStyle BackColor="Silver">
                                </ReadOnlyStyle>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left; background-color: #82CAFA;">
            <asp:Label ID="Label4" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="----- TM -----" Width="500px" ForeColor="Blue">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label136" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="10. สถานะภาพการเป็นพนักงาน" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_statusEmp" runat="server" Width="250px" TabIndex="17"
                            IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">

            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; height: 24px;" align="left">
                        <dx:ASPxCheckBox runat="server" ID="cb_check_TM" Text="Not have TM"
                            Font-Bold="true" AutoPostBack="True"
                            OnCheckedChanged="cb_check_TM_CheckedChanged" TabIndex="38">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxCheckBox>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;"></td>
                </tr>
            </table>

        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label138" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="11.ผลการติดต่อ TO" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_res_contact_TO" runat="server" Width="250px"
                            TabIndex="18" IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label157" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="22. เบอร์มือถือ" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_mobile_TM_P" runat="server" Width="80px"
                                HorizontalAlign="Left" TabIndex="39" ClientInstanceName="txt_mobile_TM_P" Password="true" MaxLength="10">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left;">&nbsp;&nbsp;</div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="txt_mobile_TM" runat="server" Width="80px"
                                HorizontalAlign="Left" TabIndex="40">
                                <MaskSettings Mask="9999999999" />
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label129" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="12. ผู้ให้ข้อมูล (TO)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <div style="float: left">
                            <dx:ASPxComboBox ID="dd_person_to" runat="server" Width="140px" TabIndex="19"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_person_to" runat="server" Width="110px"
                                HorizontalAlign="Left" TabIndex="20" MaxLength="50">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>

                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label160" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="23. ผลการติดต่อ TM" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_resContact_TM" runat="server" Width="250px"
                            TabIndex="41" IncrementalFilteringMode="StartsWith">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label139" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="13.ประเภทการจ้างงาน" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxComboBox ID="dd_empType" runat="server" Width="250px" TabIndex="21"
                            IncrementalFilteringMode="StartsWith">
                            <ReadOnlyStyle ForeColor="Blue">
                            </ReadOnlyStyle>
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;"></td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: right">
                            <dx:ASPxButton ID="btn_saveTM" runat="server" Text="Save TM" Width="100px" Height="18px"
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                OnClick="btn_saveTM_Click">
                            </dx:ASPxButton>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label112" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="14. ผู้มีอำนาจลงนามหนังสือรับรอง (ชื่อไทย)" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_name_support" runat="server" Width="150px"
                            HorizontalAlign="Left" TabIndex="22" MaxLength="50">
                            <ReadOnlyStyle BackColor="Silver" ForeColor="Blue">
                            </ReadOnlyStyle>
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left; background-color: #82CAFA;">
            <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                Text="----- TE Emergency person -----" Width="500px" ForeColor="Blue">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label126" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="ตำแหน่ง" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_pos_support" runat="server" Width="150px"
                            HorizontalAlign="Left" TabIndex="23" MaxLength="50">
                            <ReadOnlyStyle BackColor="Silver" ForeColor="Blue">
                            </ReadOnlyStyle>
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">

            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label163" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Seq" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_seq" runat="server" Width="100px"
                            HorizontalAlign="Left" Enabled="False" TabIndex="42">
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>

        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label115" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="แผนก" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; height: 24px;" align="left">
                        <dx:ASPxTextBox ID="txt_dep_support" runat="server" Width="150px"
                            HorizontalAlign="Left" TabIndex="24" MaxLength="50">
                            <ReadOnlyStyle BackColor="Silver" ForeColor="Blue">
                            </ReadOnlyStyle>
                            <DisabledStyle BackColor="Silver" ForeColor="Blue">
                            </DisabledStyle>

                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label159" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Relation" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxComboBox ID="dd_relation" runat="server" Width="250px" TabIndex="43"
                                IncrementalFilteringMode="StartsWith">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;"></td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: right">
                            <dx:ASPxButton ID="btn_saveTO" runat="server" Text="Save TO" Width="100px" Height="18px"
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                OnClick="btn_saveTO_Click">
                            </dx:ASPxButton>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label162" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Name" Width="220px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_Fname" runat="server" Width="100px"
                                HorizontalAlign="Left" TabIndex="44" MaxLength="50">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_Lname" runat="server" Width="100px"
                                HorizontalAlign="Left" TabIndex="45" MaxLength="50">
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">&nbsp;</td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 114%">
                <tr>
                    <td style="text-align: left;" class="style10">
                        <asp:Label ID="Label165" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Tel.1" Width="103px"></asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel1_TE" runat="server" Width="70px"
                                HorizontalAlign="Left" TabIndex="46">
                                <MaskSettings AllowMouseWheel="False" Mask="999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RequiredField ErrorText="" />
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label167" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="-" Width="5px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel1_to_TE" runat="server" Width="35px"
                                HorizontalAlign="Left" TabIndex="47">
                                <MaskSettings AllowMouseWheel="False" Mask="9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label168" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Ext." Width="20px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel1_ext_TE" runat="server" Width="50px"
                                HorizontalAlign="Left" TabIndex="48" MaxLength="15">

                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">&nbsp;
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 114%">
                <tr>
                    <td style="text-align: left;" class="style10">
                        <asp:Label ID="Label166" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Tel.2" Width="100px"></asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel2_TE" runat="server" Width="70px"
                                HorizontalAlign="Left" TabIndex="49">
                                <MaskSettings AllowMouseWheel="False" Mask="999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label169" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="-" Width="5px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel2_to_TE" runat="server" Width="35px"
                                HorizontalAlign="Left" TabIndex="50">
                                <MaskSettings AllowMouseWheel="False" Mask="9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <asp:Label ID="Label170" runat="server"
                                Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Ext." Width="20px">
                            </asp:Label>
                        </div>
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_tel2_ext" runat="server" Width="50px"
                                HorizontalAlign="Left" TabIndex="51" MaxLength="15">

                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">&nbsp;
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label142" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Mobile" Width="200px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="txt_mobile_TE" runat="server" Width="80px"
                                HorizontalAlign="Left" TabIndex="52">
                                <MaskSettings AllowMouseWheel="False" Mask="9999999999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                </ValidationSettings>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">&nbsp;
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">&nbsp; 
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">
                        <asp:Label ID="Label114" runat="server"
                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                            Text="Verify TE Flag" Width="200px">
                        </asp:Label>
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <div style="float: left">
                            <dx:ASPxComboBox ID="dd_verTE" runat="server" Width="150px" TabIndex="53"
                                IncrementalFilteringMode="StartsWith">
                                <Items>
                                    <dx:ListEditItem Text="-- SELECT --" Value="" />
                                    <dx:ListEditItem Text="Y" Value="Y" />
                                    <dx:ListEditItem Text="N" Value="N" />
                                </Items>
                                <DisabledStyle BackColor="Silver" ForeColor="Blue">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-right: thin dotted #C0C0C0; text-align: left;"
            class="style11">
            <table style="width: 100%">
                <tr>
                    <td style="width: 220px; text-align: left; height: 24px;">&nbsp;
                    </td>
                    <td style="width: 280px; text-align: left; height: 24px;">
                        <%-- <dx:ASPxButton ID="ASPxButton9" runat="server" Text="Save TO" Width="100px" Height="18px" 
                                                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                                                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css">
                                                     </dx:ASPxButton>--%>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="3" style="width: 500px; text-align: left;">
            <div style="float: right">
                <dx:ASPxButton ID="btn_saveTE" runat="server" Text="Save TE" Width="100px" Height="18px"
                    CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                    SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                    OnClick="btn_saveTE_Click">
                </dx:ASPxButton>
            </div>
            <div style="float: right">&nbsp;</div>
            <div style="float: right">
                <dx:ASPxButton ID="btn_insertPerson" runat="server" Text="Insert" Width="100px" Height="18px"
                    CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue"
                    SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                    OnClick="btn_insertPerson_Click">
                    <ClientSideEvents Click="function(s, e) {

}" />

                </dx:ASPxButton>
            </div>
        </td>
    </tr>

</table>
<asp:GridView ID="gvTE" runat="server" AutoGenerateColumns="False"
    BackColor="White"
    BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px"
    CellPadding="4"
    EnableModelValidation="True" ForeColor="Black"
    GridLines="Vertical" Width="99%"
    AllowPaging="True"
    EmptyDataText="No records found" OnRowCommand="gvTE_RowCommand">
    <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
    <AlternatingRowStyle BackColor="White" />
    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <Columns>
        <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton_Sel" runat="server" CausesValidation="False"
                    CommandName="Sel" Text="Sel" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Delete">
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton_Del" runat="server" CausesValidation="False"
                    CommandName="Del" Text="Del" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Seq">
            <ItemTemplate>
                <asp:Label ID="lb_Seq" runat="server" Text='<%#Eval("Seq")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Relation">
            <ItemTemplate>
                <asp:Label ID="lb_relation" runat="server" Text='<%#Eval("Relation")%>' Width="15px"></asp:Label>
                <asp:Label ID="lb_rel_desc" runat="server" Text='<%#Eval("Rel_DES")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <asp:Label ID="lb_name" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Surname">
            <ItemTemplate>
                <asp:Label ID="lb_surname" runat="server" Text='<%#Eval("SurName")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tel.1">
            <ItemTemplate>
                <asp:Label ID="lb_tel_1" runat="server" Text='<%#Eval("Tel_1")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="To">
            <ItemTemplate>
                <asp:Label ID="lb_To" runat="server" Text='<%#Eval("To_1")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ext.1">
            <ItemTemplate>
                <asp:Label ID="lb_ext_1" runat="server" Text='<%#Eval("Ext_1")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tel.2">
            <ItemTemplate>
                <asp:Label ID="lb_tel_2" runat="server" Text='<%#Eval("Tel_2")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="To">
            <ItemTemplate>
                <asp:Label ID="lb_To2" runat="server" Text='<%#Eval("To_2")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ext.2">
            <ItemTemplate>
                <asp:Label ID="lb_ext_2" runat="server" Text='<%#Eval("Ext_2")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Mobile">
            <ItemTemplate>
                <asp:Label ID="lb_Mobile" runat="server" Text='<%#Eval("Mobile")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Verify">
            <ItemTemplate>
                <asp:Label ID="lb_Verify" runat="server" Text='<%#Eval("Ver")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<dx:ASPxPopupControl ID="PopupMsg_ver" ClientInstanceName="PopupMsg_ver" ShowPageScrollbarWhenModal="true"
    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Above"
    HeaderText="Popup Msg" CloseAction="CloseButton" AllowDragging="True"
    AutoUpdatePosition="True" Modal="True" Width="450px"
    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
    SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
    RenderMode="Lightweight">
    <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
    </LoadingPanelImage>
    <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
    </ContentStyle>
    <HeaderStyle>
        <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px"
            PaddingTop="3px" />
        <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
    </HeaderStyle>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
            <table width="100%" align="center">
                <tr>
                    <td align="left">
                        <dx:ASPxLabel ID="lblMsgTH" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <dx:ASPxLabel ID="lblMsgEN" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <dx:ASPxButton ID="btnClosePopupMsg" runat="server" Text="OK" Width="100px">
                            <ClientSideEvents Click="function(s, e) { PopupMsg_ver.Hide(); }" />
                            <ClientSideEvents Click="function(s, e) { PopupMsg_ver.Hide(); }"></ClientSideEvents>
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



<dx:ASPxPopupControl ID="PopupConfirmSave_ver"
    ClientInstanceName="PopupConfirmSave_ver" ShowPageScrollbarWhenModal="True"
    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
    HeaderText="Confirm Save" CloseAction="CloseButton" AllowDragging="True"
    AutoUpdatePosition="True" Modal="True" Width="450px"
    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
    SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
    RenderMode="Lightweight">
    <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
    </LoadingPanelImage>
    <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
    </ContentStyle>
    <HeaderStyle>
        <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px"
            PaddingTop="3px" />
        <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
    </HeaderStyle>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server"
            SupportsDisabledAttribute="True">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <dx:ASPxLabel ID="lblConfirmMsgTH" runat="server" Font-Names="Tahoma"
                            Font-Size="Small" Style="color: #CC0000">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <dx:ASPxLabel ID="lblConfirmMsgEN" runat="server" Font-Names="Tahoma"
                            Font-Size="Small" Style="color: #800000">
                        </dx:ASPxLabel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:HiddenField ID="hid_rowNum" runat="server" />
            <asp:HiddenField ID="hid_oper" runat="server" />
            <asp:HiddenField ID="hid_rowNumSel" runat="server" />
            <asp:HiddenField ID="hid_Confirm" runat="server" />

            <br />

            <asp:HiddenField ID="hid_App" runat="server" />
            <asp:HiddenField ID="hid_cont" runat="server" />

            <br />
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="right" width="50%">
                        <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px"
                            OnClick="btnConfirmSave_Click">
                            <ClientSideEvents Click="function(s, e) 
                                    { 
                                        LoadingPanel_ver.Show();
                                        PopupConfirmSave_ver.Hide(); 
                                    }" />

                        </dx:ASPxButton>
                    </td>
                    <td align="left" width="50%">
                        <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px"
                            OnClick="btnConfirmCancel_Click">
                            <ClientSideEvents Click="function(s, e) { PopupConfirmSave_ver.Hide(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxLoadingPanel ID="ASPxLoadingPanel1_ver" runat="server"
    ClientInstanceName="LoadingPanel_ver"
    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="150px"
    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
</dx:ASPxLoadingPanel>

<asp:HiddenField ID="hid_CSN" runat="server" />
<asp:HiddenField ID="hid_AppNo" runat="server" />
<asp:HiddenField ID="hid_brn" runat="server" />
<asp:HiddenField ID="hid_status" runat="server" />
