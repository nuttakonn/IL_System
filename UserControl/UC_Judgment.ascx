<%--<%@   Async="true" %>--%>
<%@ Control Language="C#"  AutoEventWireup="true"  CodeBehind="UC_Judgment.ascx.cs" Inherits="ManageData_WorkProcess_UserControl_UC_Judgment" %>
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

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>


<script type="text/javascript" src='<%=ResolveUrl("~/Js/shortcut.js")%>' ></script>
<script  type="text/javascript"  src='<%=ResolveUrl("~/Js/ProtectJs.js")%>' ></script>
<style type="text/css">
    .style6
    {
        height: 30px;
        width: 198px;
    }
    .style7
    {
        height: 30px;
        width: 234px;
    }
    .style8
    {
        height: 30px;
        width: 454px;
    }
    .style11
    {
        height: 30px;
        width: 33px;
    }
    .style15
    {
        height: 30px;
        width: 99px;
    }
    .style16
    {
        height: 30px;
        width: 74px;
    }
    .style17
    {
        height: 30px;
        width: 168px;
    }
    .style18
    {
        height: 30px;
        width: 250px;
    }
    .style19
    {
        width: 250px;
    }
    .style20
    {
        width: 100%;
    }
    .style21
    {
        width: 200px;
        height: 30px;
    }
    .style23
    {
        width: 130px;
        height: 30px;
    }
    .style24
    {
        width: 350px;
        height: 30px;
    }
</style>


    <table width="100%">
        <tr>
            <td style="width:500px;text-align: left;background-color:#82CAFA;">
                <div style="float:left">
                <asp:CheckBox ID="cb_OpenPanel" runat="server" Font-Bold="True" 
                    Text="Update Data" Width="120px" 
                    oncheckedchanged="cb_OpenPanel_CheckedChanged"  AutoPostBack="true"/>
                </div>
                <div style="float:left">
                <asp:Label runat="server" Text="----- Verify Office &amp; Telephone -----" 
                    ForeColor="Blue" Width="300px" ID="Label102" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                    
                </div>
                <div style="float:right">
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
    </table>
    
<asp:Panel runat="server" ID="panel_ver_home_office">
    <table  style="width: 100%; " >
        <tr>
            <td class="style15" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label105" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Office Title" Width="120px"></asp:Label>
            </td>
            <td class="style18" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                    <dx:ASPxComboBox ID="dd_off_title" runat="server" TabIndex="1" 
                        ValueType="System.String" Width="180px" AutoPostBack="True"
                        IncrementalFilteringMode="StartsWith"
                        ontextchanged="dd_off_Title_TextChanged">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </div>
            </td>
            <td class="style11" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label106" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Office name" Width="94px"></asp:Label>
            </td>
            <td class="style17" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <div style="float:left">
                    <%--<dx:ASPxTextBox ID="txt_off_name" runat="server" Height="24px" MaxLength="50" 
                        TabIndex="2" Width="150px">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                    <dx:ASPxComboBox ID="dd_off_name" runat="server" TabIndex="14"  
                        ValueType="System.String" Width="180px" AutoPostBack="True" dropdownrows="15" FilterMinLength="3"
                                dropdownstyle="DropDown" ForceDataBinding ="true"
                        IncrementalFilteringMode="Contains" MaxLength="100">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                    <asp:Label ID="Label_off_name" runat="server" Font-Size="Small" 
                                    style="text-align: left;color: red"></asp:Label>
                </div>
            </td>
            <td class="style16" 
                style="text-align: left; font-family: Tahoma; font-size: small; ">
                <asp:Label ID="Label107_" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Office Telephone" Width="110px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:300px;height:30px;">
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_off_phone" runat="server" Height="24px" TabIndex="3" 
                        Width="75px">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="999999999" />
                        <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    -</div>
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_off_tel_to" runat="server" Height="24px" TabIndex="4" 
                        Width="35px">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="9999" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                            <RequiredField ErrorText="" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    Ext.</div>
                <div style="float:left">
                    <dx:ASPxTextBox ID="txt_off_tel_ext" runat="server" Height="24px" TabIndex="5" 
                        Width="35px" MaxLength="15">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                            <RequiredField ErrorText="" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style15">
                <asp:Label runat="server" Text="Home Telephone" Width="120px" ID="Label53_" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style19" >
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="75px" Height="24px" ID="txt_h_tel" 
                        TabIndex="6" >
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="999999999" />
                        <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText=""  />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    -</div>
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="35px" Height="24px"  ID="txt_h_tel_to" 
                        TabIndex="7" >
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="9999" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                            <RequiredField ErrorText="" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    Ext.</div>
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="35px" Height="24px" ID="txt_h_ext" 
                        TabIndex="8" MaxLength="15">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                            <RequiredField ErrorText="" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style11">
                <asp:Label runat="server" Text="Mobile" Width="92px" ID="Label104_" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style17">
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="90px" Height="24px" ID="txt_mobile" 
                        TabIndex="9" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="9999999999" />
                        <ValidationSettings ErrorText="" ErrorDisplayMode="ImageWithTooltip"  
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style16">
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:250px;height:30px;">
                <dx:ASPxButton runat="server" 
                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                        Text="Save Office &amp; Tel" CssPostfix="Office2003Blue" 
                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" Width="150px" 
                        Height="18px" ID="btn_save_Info"  
                        onclick="btn_save_Info_Click" TabIndex="10">

                    </dx:ASPxButton>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel runat="server" ID="panel_judgment">
    <table width="100%">
        <tr>
            <td style="width:100px;text-align: left;background-color:#82CAFA;">
                <asp:Label runat="server" Text="----- Judgment form -----" ForeColor="Blue" 
                    Width="300px" ID="Label103" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
        </tr>
    </table>
    <table  style="width: 100%; " >
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="1. Apply Type " Width="200px" ID="Label172" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="11" ID="dd_applyType_j" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="16. จำนวนพนักงาน " Width="200px" 
                    ID="Label27" style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" 
                                             align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="28" ID="txt_empNo">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..99999g&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>

            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="2. Apply via " Width="200px" ID="Label173" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                             align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="12" ID="dd_apply_via_j" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="17. อายุงาน" Width="200px" 
                    ID="Label11" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:30px;">
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="30px" TabIndex="29" ID="txt_s_year">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="09" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">
                    <asp:Label runat="server" Text="Year" Width="40px" ID="Label28" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="30px" TabIndex="30" ID="txt_s_month" 
                        Text="0">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="09" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">
                    <asp:Label runat="server" Text="Month" Width="40px" ID="Label54" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="3. Apply Channel " Width="200px" ID="Label174" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="13" ID="dd_apply_channel_j" AutoPostBack="true" 
                    onselectedindexchanged="dd_apply_channel_j_SelectedIndexChanged" IncrementalFilteringMode="StartsWith"  
                    >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText=""  
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="18. ประเภทการจ้างงาน" Width="200px" 
                    ID="Label178" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="31" ID="dd_empType_j" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="4. Sub Apply Channel " Width="200px" 
                    ID="Label175" style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="14" ID="dd_subChannel_j" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                   
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label ID="Label35" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="เอกสารแสดงรายได้" Width="200px"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxComboBox ID="dd_statement_j" runat="server" TabIndex="32" 
                    ValueType="System.String" Width="200px" 
                    IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        SetFocusOnError="True" ValidationGroup="V_Cal_Judg">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
                <asp:HiddenField ID="hid_incomeDoc" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="5. อายุ :" Width="200px" ID="Label14" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="100px" Height="24px" TabIndex="13" 
                        ID="txt_age" Enabled="False">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">
                    <asp:HiddenField ID="hid_birthDate" runat="server" />
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label ID="Label30" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="19. ลักษณะการรับเงินเดือน" Width="200px"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxComboBox ID="dd_incomeType_j" runat="server" TabIndex="33" 
                    ValueType="System.String" Width="200px" 
                    OnSelectedIndexChanged="dd_incomeType_j_SelectedIndexChanged" 
                    AutoPostBack="true" IncrementalFilteringMode="StartsWith" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        SetFocusOnError="True" ValidationGroup="V_Cal_Judg">
                        <RegularExpression ErrorText="" />
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="6. วันเกิด" Width="200px" ID="Label13" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="100px" Height="24px" TabIndex="16" 
                        ID="txt_birthDate_j">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                        </DisabledStyle>
                        <MaskSettings Mask="00/00/0000" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">&nbsp;</div>
                <div style="float:left;">
                    <asp:Label ID="lb_day" runat="server" Font-Bold="True" Font-Size="Small" 
                        ForeColor="Red" Height="24px"></asp:Label>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label ID="Label34" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="ประเภทลูกค้า" Width="200px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <dx:ASPxComboBox ID="dd_typeCust_j" runat="server" TabIndex="34"  
                    ValueType="System.String" Width="200px" 
                    OnSelectedIndexChanged="dd_typeCust_j_SelectedIndexChanged" 
                    AutoPostBack="true" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg">
                        <RequiredField ErrorText="" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="7. Marital status" Width="200px" ID="Label10" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                     TabIndex="17" ID="dd_marital_j" IncrementalFilteringMode="StartsWith" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="20. รายได้ต่อเดือน" Width="200px" ID="Label108" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" 
                                            align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="35" ID="txt_salary" 
                    OnTextChanged="txt_salary_TextChanged" AutoPostBack="true" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..99999999g&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
                <asp:HiddenField ID="hid_salary" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="Submarital status " Width="200px" ID="Label15" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <div style="float:left;">
                    <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                        TabIndex="18" ID="dd_subMarital_j" IncrementalFilteringMode="StartsWith">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </div>
                <div style="float:left;">
                    &nbsp;
                </div>
                <div style="float:left;">
                    <asp:Label runat="server" Text="8.บุตร" Width="60px" ID="Label16" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="35px" MaxLength="2" Height="24px" 
                        TabIndex="19" ID="txt_child" Text="0">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="09" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </td>
            

            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="Auto Salary Adjust" Width="200px" ID="Label109" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="36" ID="txt_salary_adj" 
                    Enabled="false">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..99999999g&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
                <asp:HiddenField ID="hid_sal_old" runat="server" />
                <asp:HiddenField ID="hid_sal_date" runat="server" />
                <asp:HiddenField ID="hid_sal_time" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="9. เพศ" Width="200px" ID="Label17" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxRadioButtonList runat="server" RepeatDirection="Horizontal" 
                    Width="150px" TabIndex="20" ID="rb_sex"><Items><dx:ListEditItem Text="ชาย" Value="M"></dx:ListEditItem><dx:ListEditItem Text="หญิง" Value="F"></dx:ListEditItem></Items>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxRadioButtonList>
                <asp:HiddenField ID="hid_sex" runat="server" />
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="21. ลักษณะเบอร์บ้าน" Width="200px" 
                    ID="Label110" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="37" ID="dd_homeTelType" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="10. ระยะเวลาที่อยู่อาศัย" Width="200px" 
                    ID="Label20" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="30px" TabIndex="20" ID="txt_yearResident">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="09" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">
                    &nbsp;&nbsp;</div>
                <div style="float:left;">
                    <asp:Label runat="server" Text="ปี" Width="50px" ID="Label22" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
                <div style="float:left;">
                    <dx:ASPxTextBox runat="server" Width="30px" TabIndex="21" 
                        ID="txt_monthResident" Text="0">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <MaskSettings Mask="09" />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left;">
                    &nbsp;&nbsp;</div>
                <div style="float:left;">
                    <asp:Label runat="server" Text="เดือน" Width="50px" ID="Label21" 
                        style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
                </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="22. เบอร์มือถือ" Width="200px" ID="Label111" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="38" ID="txt_mobile_j">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="9999999999" />
                    
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="11. จำนวนผู้ที่อยู่อาศัยด้วย" Width="200px" ID="Label19" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxTextBox runat="server" Width="100px" TabIndex="22" ID="txt_fPerson">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..100&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="23. Postcode (Home)" Width="200px" 
                    ID="Label176" style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" 
                                            align="left">
                <div style="float:left">
                    <dx:ASPxTextBox runat="server" Width="150px"  ID="txt_postcode_j" 
                        Enabled="False">
                       <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                       
                    </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    <dx:ASPxButton ID="btn_post_H" runat="server" Text=".." Width="30px" 
                        AutoPostBack="true" CausesValidation="false" 
                        onclick="btn_post_H_Click"  TabIndex="39" />
                </div>
                <asp:HiddenField ID="hid_homeP" runat="server" />
                <asp:HiddenField ID="hid_tambolH" runat="server" />
                <asp:HiddenField ID="hid_ampH" runat="server" />
                <asp:HiddenField ID="hid_provH" runat="server" />
            </td>

        </tr>
        <tr>
            
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="12. ลักษณะที่อยู่อาศัย" Width="200px" 
                    ID="Label18" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="23" ID="dd_resident_j" IncrementalFilteringMode="StartsWith">
                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                   <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">

                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>

            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="24. Postcode (Office)" Width="200px" 
                    ID="Label177" style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;"  align="left">
                <div style="float:left;">
                <dx:ASPxTextBox runat="server" Width="150px" 
                    ID="txt_postcode_off_j" Enabled="False">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    <dx:ASPxButton ID="btn_post_O" runat="server" Text=".." Width="30px"  
                        AutoPostBack="true" CausesValidation="false"  
                        onclick="btn_post_O_Click"  TabIndex="40"  />
                </div>
                <asp:HiddenField ID="hid_offP" runat="server" />
                <asp:HiddenField ID="hid_tamO" runat="server" />
                <asp:HiddenField ID="hid_ampO" runat="server" />
                <asp:HiddenField ID="hid_provO" runat="server" />
            </td>                                   
            <%--<td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="24. ประเภทการจ้างงาน" Width="200px" 
                    ID="Label178" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="39" ID="dd_empType_j">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>--%>
        </tr>        
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:30px;">
                <asp:Label runat="server" Text="13. ประเภทธุรกิจ" Width="200px" 
                    ID="Label23" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="24" ID="dd_busType_j" IncrementalFilteringMode="StartsWith" AutoPostBack="true" OnTextChanged ="dd_busType_TextChanged"> 
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="25. วงเงินที่ต้องการ" Width="200px" ID="Label179" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="41" ID="txt_amount_j" 
                    MaxLength="7">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..9999999g&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorText="" ValidationGroup="V_Cal_Judg" 
                        SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>

        <%--BOT Report--%>
                <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:30px;">
                <asp:Label runat="server" Text="13.1. ประเภทธุรกิจย่อย" Width="200px" 
                    ID="Label1" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="24" ID="dd_subbus_type" IncrementalFilteringMode="StartsWith" > 
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" align="left">
            </td>
        </tr>
         <%--BOT Report--%>

        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="14. อาชีพ" Width="200px" ID="Label24" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="25" ID="dd_occup_j" 
                    onselectedindexchanged="dd_occup_j_SelectedIndexChanged" 
                    AutoPostBack="true" IncrementalFilteringMode="StartsWith" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="26.ความสามารถในการผ่อน" Width="200px" ID="Label180" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" 
                                            align="left">
                <dx:ASPxTextBox runat="server" Width="150px" TabIndex="42" ID="txt_solvency" 
                    MaxLength="5" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <MaskSettings Mask="&lt;0..99999g&gt;" AllowMouseWheel="False" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>

        <%--BOT Report--%>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="14.1. อาชีพย่อย" Width="200px" ID="Label3" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small;" align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="25" ID="dd_suboccup" 
                    IncrementalFilteringMode="StartsWith" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" align="left">
            </td>
        </tr>
        <%--BOT Report--%>

        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="Commercial registration" Width="200px" 
                    ID="Label25" style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="26" ID="dd_comerc" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        SetFocusOnError="True">
                        <RequiredField ErrorText="" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="27. วันที่เงินเดือนออก" Width="200px" ID="Label181" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;" 
                                            align="left">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="43" ID="dd_dateOfIncome" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="15.ตำแหน่ง" Width="200px" ID="Label26" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; " 
                                            align="left" class="style8">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="27" ID="dd_position_j" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="28.ความสามารถในการติดต่อ" Width="200px" 
                    ID="Label182" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" 
                    TabIndex="44" ID="dd_contact_judg" IncrementalFilteringMode="StartsWith">
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                        ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                        <RequiredField ErrorText="" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <%-- pact--%>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">                
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="Customer Self Declare " Width="200px" ID="Label2" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:200px;height:30px;" 
                                            align="left">
                <table cellpadding="0" cellspacing="0" class="style20" style="width:100%">
                    <tr>
                        <td style="width:50%"> 
                            <dx:ASPxTextBox ID="txt_seftDec" runat="server" AutoPostBack="true" 
                                MaxLength="2" TabIndex="35" Width="70px">
                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                </DisabledStyle>
                                <ClientSideEvents KeyUp="function ValidateIdValue(s,e){       
       var txt2 = s.inputElement;
       var str = txt2.value;
       var myArray = str.match(/^\d+$/); 
       if(str != '-'){if(myArray == null){txt2.value = '';}}       
}" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                                    SetFocusOnError="True" ValidationGroup="V_Cal_Judg">
                                    <RequiredField ErrorText="กรุณาตรวจสอบข้อมูล Self declare..!!" 
                                        IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="width:50%">
                            <asp:Label ID="Label207" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="issuer(s)"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
           <tr>
            
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                
            </td>
            <td style="font-family: Tahoma; font-size: small; " align="left" class="style8">
                
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:Label runat="server" Text="Postcode (ID)" Width="200px" 
                    ID="postcodeid"  style="font-family: Tahoma; font-size: small; font-weight: 700;"></asp:Label>
            </td>
            <td style="font-family: Tahoma; font-size: small; width:350px;height:30px;"  align="left">
                <div style="float:left;">
                <dx:ASPxTextBox runat="server" Width="150px" 
                    ID="txt_postcode_idcard_j" Enabled="False" >
                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" 
                            ValidationGroup="V_Cal_Judg" SetFocusOnError="True">
                            <RequiredField ErrorText="" IsRequired="True" />
                        </ValidationSettings>
                </dx:ASPxTextBox>
                </div>
                <div style="float:left">
                    <dx:ASPxButton ID="btn_post_I"  runat="server" Text=".." Width="30px"  
                        AutoPostBack="true" CausesValidation="false"  
                        onclick="btn_post_I_Click"  TabIndex="40"  />
                </div>
                <asp:HiddenField ID="hid_idcardP" runat="server" />
                <asp:HiddenField ID="hid_tamI" runat="server" />
                <asp:HiddenField ID="hid_ampI" runat="server" />
                <asp:HiddenField ID="hid_provI" runat="server" />
            </td>
          </tr>          
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:30px;">
                <asp:HiddenField ID="hid_CSN" runat="server" />
                <asp:HiddenField ID="hid_AppNo" runat="server" />
                <asp:HiddenField ID="hid_brn" runat="server" />
                <asp:HiddenField ID="hid_AppDate" runat="server" />
                <asp:HiddenField ID="hid_status" runat="server" />
                <asp:HiddenField ID="hid_idno" runat="server" />
                <asp:HiddenField ID="hid_G_aprj" runat="server" />
                <asp:HiddenField ID="hid_G_loca" runat="server" />
                <asp:HiddenField ID="hid_G_reason" runat="server" />
                <asp:TextBox ID="E_sub_succuss" runat="server" Visible="False" Width="50px"></asp:TextBox>
                <asp:HiddenField ID="hid_resCal" runat="server" />
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:30px;">
                &nbsp;
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:30px;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style21">
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style8">
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style23">
                &nbsp;
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; " 
                class="style24">
                <div style="float:right;">
                    <dx:ASPxButton runat="server" 
                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                        Text="Save judgment" CssPostfix="Office2003Blue" 
                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" Width="150px" 
                        Height="18px" ID="btn_judgment" onclick="btn_judgment_Click" 
                        ValidationGroup="V_Cal_Judg" TabIndex="44">
                      
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="PopupMsg_judgment" 
        ClientInstanceName="PopupMsg_judgment" ShowPageScrollbarWhenModal="true"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
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
                    <table width="100%" align="center" >
                        <tr>
                            <td align="left">
                                <dx:ASPxLabel ID="lblMsgTH" runat="server" Font-Names="Tahoma" Font-Size="Small"  Style="color: #CC0000;"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <dx:ASPxLabel ID="lblMsgEN" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:HiddenField ID="hid_validate" runat="server" />
                    <br />
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="center" >
                                <dx:ASPxButton ID="btnClosePopupMsg" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnClosePopupMsg_Click" >
                                    <ClientSideEvents Click="function(s, e) { PopupMsg_judgment.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupMsg_judgment.Hide(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

       

   <dx:ASPxPopupControl ID="PopupConfirmSave_judg" 
        ClientInstanceName="PopupConfirmSave_judg" ShowPageScrollbarWhenModal="true"
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
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgTH" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgEN" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:HiddenField ID="hid_oper_judg" runat="server" />
                  
                    <br />
                    
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnConfirmSave_Click" >
                                     <ClientSideEvents Click="function(s, e) { LoadingPanel_judg.Show();
                                        PopupConfirmSave_judg.Hide(); }" />

                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" 
                                    OnClick="btnConfirmCancel_Click">
                                    <ClientSideEvents Click="function(s, e) { PopupConfirmSave_judg.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupConfirmSave_judg.Hide();}"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        
        <dx:ASPxPopupControl ID="Popup_confirm_2" 
        ClientInstanceName="Popup_confirm_2" ShowPageScrollbarWhenModal="true"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Confirm " CloseAction="CloseButton" AllowDragging="True" 
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
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" 
                    SupportsDisabledAttribute="True">
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lb_msg_2" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hid_quest" runat="server" />
                    <br />
                    <asp:HiddenField ID="hid_val_1" runat="server" />
                    <asp:HiddenField ID="hid_val_2" runat="server" />
                    <asp:HiddenField ID="hid_val_3" runat="server" />
                    <asp:HiddenField ID="hid_val_4" runat="server" />
                    <asp:HiddenField ID="hid_val_5" runat="server" />
                    <asp:HiddenField ID="hid_val_6" runat="server" />
                   
                    <br />
                    
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btn_ok_2" runat="server" Text="YES" Width="100px" OnClick="btn_ok_2_Click" 
                                     >
                                    <ClientSideEvents Click="function(s, e) { 

LoadingPanel_judg.Show();
Popup_confirm_2.Hide(); }" />

                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btn_cancel_2" runat="server" Text="NO" Width="100px" 
                                    OnClick="btn_cancel_2_Click">
                                   <ClientSideEvents Click="function(s, e) { LoadingPanel_judg.Show(); Popup_confirm_2.Hide(); }" />
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


        <dx:ASPxPopupControl ID="Popup_Province" 
        ClientInstanceName="Popup_Province" ShowPageScrollbarWhenModal="true"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Select Province" CloseAction="CloseButton" AllowDragging="True" 
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
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                   
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style2" style="text-align: right;">
                                <asp:Label ID="L_count" runat="server" Font-Size="Small" 
                                    style="text-align: right"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <dx:ASPxComboBox ID="C_address_I" runat="server" autopostback="True" dropdownrows="15" 
                                dropdownstyle="DropDown" incrementalfilteringmode="Contains" ontextchanged="C_address_I_TextChanged" 
                                tabindex="14" backcolor="#FFFFC4" valuetype="System.String" width="300px">
                                </dx:ASPxComboBox>
                            </td>
                            <td class="style7" style="text-align: left;">
                                &nbsp;</td>
                            <td style="width:250px;text-align: left;">
                                &nbsp;</td>
                            
                        </tr>
                        
                        <tr>
                            <td class="style2" style="text-align: left;">
                                <asp:Label ID="Label202" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Tambol :" Width="80px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <dx:ASPxComboBox ID="D_tambol1" runat="server" AutoPostBack="True" 
                                DropDownStyle="DropDown" ontextchanged="D_tambol1_TextChanged" TabIndex="15" >
                                </dx:ASPxComboBox>
                            </td>
                            <td class="style7" style="text-align: left;">
                                <asp:Label ID="Label203" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Amphur :" Width="100px"></asp:Label>
                            </td>
                            <td style="width:250px;text-align: left;">
                                <div style="float:left; width: 2px;"> 
                                    <dx:ASPxComboBox ID="D_amphur1" runat="server" AutoPostBack="True" 
                                    DropDownStyle="DropDown" ontextchanged="D_amphur1_TextChanged" TabIndex="16" >
                                    </dx:ASPxComboBox>
                              
                                </div>
                               
                            </td>
                           
                        </tr>
                        <tr>
                            <td class="style2" style="text-align: left;">
                                <asp:Label ID="Label204" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Province:" Width="80px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <dx:ASPxComboBox ID="D_province1" runat="server" TabIndex="17" 
                                    ValueType="System.String">
                                </dx:ASPxComboBox>

                            </td>
                            <td class="style7" style="text-align: left;">
                                <asp:Label ID="Label205" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Postcode:" Width="65px"></asp:Label>
                            </td>
                            <td style="width:250px;text-align: left;">
                                <dx:ASPxComboBox ID="D_zipcode1" runat="server" TabIndex="18" 
                                    ValueType="System.String">
                                </dx:ASPxComboBox>
                            </td>
                            
                        </tr>
                        <tr>                            
                            <td align="center" colspan="4">
                                <div style="float:right">
                                    <dx:ASPxButton ID="btn_clear_prov" runat="server" Text="Clear" Width="100px" 
                                         CausesValidation="false" OnClick="btn_clear_prov_Click" >
                                         
                                    </dx:ASPxButton>
                                </div>
                                <div style="float:right">&nbsp;&nbsp;</div>
                                <div style="float:right">
                                <dx:ASPxButton ID="btn_sel_prov" runat="server" Text="OK" Width="100px" 
                                        OnClick="btn_sel_prov_Click" CausesValidation="false" >
                                         <ClientSideEvents Click="function(s, e) { Popup_Province.Hide(); }" />
                                    </dx:ASPxButton>
                                    
                                </div>
                                <asp:HiddenField ID="hid_Type" runat="server" />
                            </td>
                                                 
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1_judg" ClientInstanceName="LoadingPanel_judg" runat="server" Width="250px" Height="150px"   
    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" >
    </dx:ASPxLoadingPanel>
        
    <asp:HiddenField ID="hidDate97" runat="server" />

</asp:Panel>
