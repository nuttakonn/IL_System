<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="Maker.aspx.cs" Inherits="ManageData_WorkProcess_Maker" %>

<%@ Register Src="~/ManageData/WorkProcess/UserControl/UC_Judgment.ascx" TagName="UC_Judgment"
    TagPrefix="uc1" %>
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
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phContents" runat="Server">
    <script src="../../../Js/shortcut.js"></script>
    <script src="../../../Js/ProtectJs.js"></script>
    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Width="100%" Height="136px">
                    <ContentPaddings Padding="5px" />
                    <HeaderTemplate>
                        Maker
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <%--<asp:Button ID="btn_search" runat="server" Text="Search" Width="80px" Height="24px"
                                            BackColor="#66CCFF" OnClick="btn_search_Click" />--%>
                                        <dx:ASPxButton ID="btn_search" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px" OnClick="btn_search_Click">
                                        </dx:ASPxButton>
                                        <asp:HiddenField ID="hidden_makergrid" runat="server" />
                                        <%--<asp:Button ID="btnClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                            BackColor="#66CCFF" OnClick="btnClear_Click" />&nbsp;--%>
                                        <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px" OnClick="btnClear_Click">
                                        </dx:ASPxButton>
                                        <%--<asp:Button ID="btnSave" runat="server" Text="Save" Width="80px" Height="24px" BackColor="#66CCFF"
                                            OnClick="btnSave_Click" />&nbsp;--%>
                                        <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="AlignButtonInline" Width="80px" Height="24px" OnClick="btnSave_Click">
                                        </dx:ASPxButton>
                                        <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" Height="24px"
                                            Enabled="false" BackColor="#66CCFF" OnClick="btnDelete_Click" />--%>
                                        <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" CssClass="AlignButtonInline" Width="80px" Height="24px" OnClick="btnDelete_Click" Enabled="false">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                            <br />
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label2" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Maker Code " Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label71" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                        height: 24px;">
                                        <asp:TextBox ID="txt_MakerCode" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="MakerCode" runat="server" Width="200px" Visible="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Title Code " Width="80px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label73" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                        height: 24px;">
                                        <dx:ASPxComboBox ID="dd_off_title" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith"
                                            Width="200px">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label4" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Thai Name " Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label72" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;
                                        height: 24px;">
                                        <asp:TextBox ID="txt_ThaiName" runat="server" Width="450px"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="English Name " Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label74" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td colspan="2" style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;
                                        height: 24px;">
                                        <asp:TextBox ID="txt_EngName" runat="server" Width="450px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <dx:ASPxPageControl Width="100%" Height="100%" ID="tabDetail" runat="server" ActiveTabIndex="3"
                                EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                ActivateTabPageAction="Click" LoadingPanelImagePosition="Top">
                                <TabPages>
                                    <dx:TabPage Text="Address" Name="Addr">
                                        <ActiveTabStyle BackColor="#3399FF">
                                        </ActiveTabStyle>
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                <dx:ASPxPageControl Width="100%" Height="100%" ID="ASPxPageControl1" runat="server"
                                                    ActiveTabIndex="1" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                    CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                    ActivateTabPageAction="Click" LoadingPanelImagePosition="Top">
                                                    <TabPages>
                                                        <dx:TabPage Text="Registration Address" Name="Address_Regis">
                                                            <ActiveTabStyle BackColor="#99FFFF">
                                                            </ActiveTabStyle>
                                                            <TabStyle Font-Bold="True">
                                                            </TabStyle>
                                                            <ContentCollection>
                                                                <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                                                height: 24px; background-color: #66CCFF" colspan="9">
                                                                                <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Registration Address " Width="150px"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Address " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reAddress" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Moo" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label68" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reMoo" runat="server" Width="50px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label12" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Village" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label69" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reVilage" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label13" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Building Type " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label70" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="dd_BuildingType1" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                    Text="Building" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label78" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reBuilding" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label15" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Room" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label79" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reRoom" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label16" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Floor " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label80" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reFloor" runat="server" Width="50px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label17" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Soi" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label81" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reSoi" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label18" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Road" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label82" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_reRoad" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label19" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Search Province" Width="110px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label83" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                                                                height: 24px;" colspan="3">
                                                                                <div style="float: left;">
                                                                                    <dx:ASPxComboBox ID="C_address_I" runat="server" AutoPostBack="True" DropDownRows="15"
                                                                                        DropDownStyle="DropDown" IncrementalFilteringMode="Contains" OnTextChanged="C_address_I_TextChanged"
                                                                                        TabIndex="14" BackColor="#FFFFC4" ValueType="System.String" Width="300px">
                                                                                    </dx:ASPxComboBox>
                                                                                </div>
                                                                                <asp:Label ID="L_count" runat="server" Font-Size="Small" Style="text-align: right"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label20" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Tambol" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label84" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_tambol1" runat="server" AutoPostBack="True" DropDownStyle="DropDown"
                                                                                    OnTextChanged="D_tambol1_TextChanged" TabIndex="15">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label21" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Amphur" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label85" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_amphur1" runat="server" AutoPostBack="True" DropDownStyle="DropDown"
                                                                                    OnTextChanged="D_amphur1_TextChanged" TabIndex="16">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label204" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Province" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label86" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_province1" runat="server" TabIndex="17" ValueType="System.String">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label22" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Postcode" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label87" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_zipcode1" runat="server" TabIndex="18" ValueType="System.String">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </dx:ContentControl>
                                                            </ContentCollection>
                                                        </dx:TabPage>
                                                        <dx:TabPage Text="Location Address" Name="Address_local">
                                                            <ActiveTabStyle BackColor="#99FFFF">
                                                            </ActiveTabStyle>
                                                            <TabStyle Font-Bold="True">
                                                            </TabStyle>
                                                            <ContentCollection>
                                                                <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                                                height: 24px; background-color: #66CCFF" colspan="9">
                                                                                <asp:Label ID="Label23" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Location Address " Width="150px"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label24" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Address " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label88" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loAddress" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label25" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Moo" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label89" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loMoo" runat="server" Width="50px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label26" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Village" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label90" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loVilage" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label27" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Building Type " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label91" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="dd_BuildingType2" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label28" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Building" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label92" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loBuilding" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label29" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Room" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label93" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loRoom" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label30" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Floor " Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label94" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loFloor" runat="server" Width="50px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label31" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Soi" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label95" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loSoi" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label32" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Road" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label96" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <asp:TextBox ID="txt_loRoad" runat="server" Width="150px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label33" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Search Province" Width="110px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label97" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                                                                height: 24px;" colspan="3">
                                                                                <div style="float: left;">
                                                                                    <dx:ASPxComboBox ID="C_address_I_2" runat="server" AutoPostBack="True" DropDownRows="15"
                                                                                        DropDownStyle="DropDown" IncrementalFilteringMode="Contains" OnTextChanged="C_address_I_2_TextChanged"
                                                                                        TabIndex="14" BackColor="#FFFFC4" ValueType="System.String" Width="300px">
                                                                                    </dx:ASPxComboBox>
                                                                                </div>
                                                                                <asp:Label ID="L_count2" runat="server" Font-Size="Small" Style="text-align: right"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label35" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Tambol" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label103" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_tambol2" runat="server" AutoPostBack="True" DropDownStyle="DropDown"
                                                                                    OnTextChanged="D_tambol2_TextChanged" TabIndex="15">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label36" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Amphur" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label98" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox runat="server" ID="D_amphur2" AutoPostBack="True" DropDownStyle="DropDown"
                                                                                    OnTextChanged="D_amphur2_TextChanged" TabIndex="16">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label37" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Province" Width="60px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label99" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_province2" runat="server" TabIndex="17" ValueType="System.String">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                                                height: 24px;">
                                                                                <asp:Label ID="Label38" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700;" Text="Postcode" Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                                                <asp:Label ID="Label104" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                                    font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                                                height: 24px;">
                                                                                <dx:ASPxComboBox ID="D_zipcode2" runat="server" TabIndex="18" ValueType="System.String">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </dx:ContentControl>
                                                            </ContentCollection>
                                                        </dx:TabPage>
                                                    </TabPages>
                                                </dx:ASPxPageControl>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Other" Name="Other">
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label39" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Register No./Tax ID." Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_RegisterNo_TaxID" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label40" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Tax ID." Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_TaxID" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label41" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Telephone No" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_TelNo" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label42" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Contact Tel. Range" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_ContactTel" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label43" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.1 Type" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:DropDownList runat="server" ID="dd_FaxNo1Type" Width="150px">
                                                                <asp:ListItem Value=""></asp:ListItem>
                                                                <asp:ListItem Value="1">1:AUTO FAX</asp:ListItem>
                                                                <asp:ListItem Value="2">2:MANUAL FAX</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label44" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="t_FaxNo1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label45" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.2 Type" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:DropDownList runat="server" ID="dd_FaxNo2Type" Width="150px">
                                                                <asp:ListItem Value=""></asp:ListItem>
                                                                <asp:ListItem Value="1">1:AUTO FAX</asp:ListItem>
                                                                <asp:ListItem Value="2">2:MANUAL FAX</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label46" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.2" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="t_FaxNo2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label47" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Person 1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Person1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label48" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Position 1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Position1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label49" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Dept 1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Dept1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label50" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Person 2" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Person2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label51" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Position 2" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Position2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label52" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Reference Dept 2" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Ref_Dept2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label53" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Issue Invoice Days" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_IssueInvoiceDays" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Whom to contact" Name="Contact">
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label54" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Seq.2" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Seq" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label55" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Thai Name" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_thaiName1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label56" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Eng Name" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_EngName1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label57" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Department" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Department" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label58" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Mobile Phone" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_MobilePhone1" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label59" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Telephone 1." Width="90px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 160px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label100" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Tel 1/Contact tel Range" Width="160px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Tel1" runat="server" Width="70"></asp:TextBox>&nbsp;/&nbsp;
                                                            <asp:TextBox ID="txt_ContactTelRange1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label101" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Extension" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Extension1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label102" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_FaxNo1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label60" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Telephone 2." Width="90px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 160px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label61" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Tel 2/Contact tel Range" Width="160px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Tel2" runat="server" Width="70"></asp:TextBox>&nbsp;/&nbsp;
                                                            <asp:TextBox ID="txt_ContactTelRange2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label62" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Extension" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Extension2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label63" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_FaxNo2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label64" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Telephone 3." Width="90px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 160px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label65" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Tel 3/Contact tel Range" Width="160px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Tel3" runat="server" Width="70"></asp:TextBox>&nbsp;/&nbsp;
                                                            <asp:TextBox ID="txt_ContactTelRange3" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label66" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Extension" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_Extension3" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                                            height: 24px;">
                                                            <asp:Label ID="Label67" runat="server" Style="font-family: Tahoma; font-size: small;
                                                                font-weight: 700;" Text="Fax No.1" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                                            height: 24px;">
                                                            <asp:TextBox ID="txt_FaxNo3" runat="server" Width="150px"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                            <%--<asp:Button runat="server" Text="Insert" BackColor="#66CCFF" Height="24px" Width="100px"
                                                                ID="Button1"></asp:Button>--%>
                                                            <dx:ASPxButton ID="Button1" runat="server" Text="Insert" CssClass="AlignButtonInline" Width="100px" Height="24px">
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:GridView ID="gWhom_Contact" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gWhom_Contact_PageIndexChanging"
                                                    OnSelectedIndexChanging="gWhom_Contact_SelectedIndexChanging" OnRowDeleting="gWhom_Contact_RowDeleting">
                                                    <Columns>
                                                        <%--<asp:ButtonField CommandName="Edit" HeaderText="Edit" Text="Edit">
                                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                        </asp:ButtonField>--%>
                                                        <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                        </asp:ButtonField>
                                                        <%--<asp:CommandField ShowDeleteButton="True" HeaderText="Del" DeleteText="Del" EditText="Del">
                                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                        </asp:CommandField>--%>
                                                        <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                        </asp:CommandField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Seq" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Name" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Surname" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Tel.1" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Ext.1" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Fax No.1" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Tel.2" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Ext.2" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Fax No.2" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Tel.3" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T41LTY" HeaderText="Fax No.3" ReadOnly="True" SortExpression="T41LTY">
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                        PageButtonCount="4" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F7F7DE" />
                                                </asp:GridView>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Note" Name="Note">
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%" align="center">
                                                    <tr>
                                                        <td align="right" class="style2" style="text-align: right; width: 100px;">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style6" style="text-align: left;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style2" style="text-align: right; width: 100px;">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style6" style="text-align: left;">
                                                            <dx:ASPxMemo ID="txt_memoReason" runat="server" Width="500px" Height="80px">
                                                            </dx:ASPxMemo>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <div style="float: left">
                                                                <dx:ASPxButton ID="btn_saveNote" runat="server" Text="OK" Width="100px" AutoPostBack="true">
                                                                    <ClientSideEvents Click="function(s, e) { 
                                                                       LoadingPanel.Show();
                                                                     }" />
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="float: left">
                                                                &nbsp;&nbsp;</div>
                                                            <div style="float: left">
                                                                <dx:ASPxButton ID="btn_cancel_Save" runat="server" Text="Cancel" Width="100px" AutoPostBack="true"
                                                                    CausesValidation="false">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="float: left">
                                                                &nbsp;&nbsp;
                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="red" Font-Bold="true" Width="300px"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:GridView ID="gvNote" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gvNote_PageIndexChanging"
                                                                OnSelectedIndexChanging="gvNote_SelectedIndexChanging" OnRowDeleting="gvNote_RowDeleting">
                                                                <Columns>
                                                                    <asp:BoundField DataField="T42BRD" HeaderText="Note date" ReadOnly="True" SortExpression="T42BRD">
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="T42BRD" HeaderText="Note time" ReadOnly="True" SortExpression="T42BRD">
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="T42BRD" HeaderText="Note By" ReadOnly="True" SortExpression="T42BRD">
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="T42BRD" HeaderText="Description" ReadOnly="True" SortExpression="T42BRD">
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                                <FooterStyle BackColor="#CCCC99" />
                                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                    PageButtonCount="4" />
                                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                <RowStyle BackColor="#F7F7DE" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>
                            <br />
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                                Height="150px" Width="250px">
                            </dx1:ASPxLoadingPanel>
                            <dx:ASPxPopupControl ID="PopupMsg" ClientInstanceName="PopupMsg" ShowPageScrollbarWhenModal="true"
                                HeaderText="" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
                                CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True" Modal="True"
                                Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/alert.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsg1" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnOK" runat="server" Text="OK" Width="100px" OnClick="btnOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirm" ClientInstanceName="PopupConfirm" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
                                HeaderText="Confirm" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="450px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
                                RenderMode="Lightweight">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/alert.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblConfirmMsgEN" runat="server" Font-Names="Tahoma" Font-Size="Small"
                                                        Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <%--<asp:HiddenField ID="hid_oper" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmOK" runat="server" Text="OK" Width="100px" OnClick="btnConfirmOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnConfirmCancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirmDelete" ClientInstanceName="PopupConfirmDelete"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Confirm Delete" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="450px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
                                RenderMode="Lightweight">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/alert.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblConfimMsg_Delete" runat="server" Font-Names="Tahoma" Font-Size="Small"
                                                        Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <%--<asp:HiddenField ID="HiddenField1" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnConfirm_OK" runat="server" Text="OK" Width="100px" OnClick="btnConfirm_OK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirm_Cancel" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnConfirm_Cancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="Popup_AddMaker" ClientInstanceName="Popup_AddMaker" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
                                HeaderText="Search" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px;
                                                    width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="C" Selected="True">Code</asp:ListItem>
                                                        <asp:ListItem Value="T">Title</asp:ListItem>
                                                        <asp:ListItem Value="MN">Maker Name</asp:ListItem>
                                                        <asp:ListItem Value="MEn">Maker Name Eng.</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                    height: 24px;">
                                                    <asp:Label ID="lbl_SelectMaker" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Maker" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label10" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txt_SearchMaker" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btn_popup_search" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btn_popup_search_Click" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btn_popup_search" runat="server" Text="Search" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_search_Click">
                                                    </dx:ASPxButton>
                                                    <%--<asp:Button ID="Button2" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF"
                                                        OnClick="btn_popup_clear_Click" />--%>
                                                    <dx:ASPxButton ID="Button2" runat="server" Text="Clear" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_clear_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="E_popup_error" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        ForeColor="Red" Height="18px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:HiddenField ID="ds_hiddenWhom" runat="server" />
                                            <asp:HiddenField ID="ds_Note" runat="server" />
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Edit" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="STRT46MAK" HeaderText="Code" ReadOnly="True" SortExpression="STRT46MAK">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="GNB2TD" HeaderText="Title" ReadOnly="True" SortExpression="GNB2TD">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T46TNM" HeaderText="Maker Name" ReadOnly="True" SortExpression="T46TNM">
                                                        <ItemStyle Width="220px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T46ENM" HeaderText="Maker Name Eng" SortExpression="T46ENM" />
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    PageButtonCount="4" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                <RowStyle BackColor="#F7F7DE" />
                                            </asp:GridView>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>