<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="ManageData_WorkProcess_Vendor" %>
<%@ Register Src="~/ManageData/WorkProcess/UserControl/UC_Judgment.ascx" TagName="UC_Judgment" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContents" runat="Server">
    <script type="text/javascript" src="../../../Js/shortcut.js"></script>
    <script type="text/javascript" src="../../../Js/ProtectJs.js"></script>
    <script type="text/javascript">

        document.documentElement.onclick = function () {
            $find("<%= CalendarMouDate.ClientID %>").hide();
            $find("<%= CalendarJoinDate.ClientID %>").hide();
            $find("<%= CalendarExpireDate.ClientID %>").hide();
            $find("<%= CalendarFirstOpenDate.ClientID %>").hide();
            $find("<%= CalendarReference_JoinDate.ClientID %>").hide();
            $find("<%= CalendarReference_ExpireDate.ClientID %>").hide();
        }

    </script>
    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Width="100%" Height="136px">
                    <ContentPaddings Padding="5px" />
                    <HeaderTemplate>
                        Vendor
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <asp:HiddenField ID="ds_vendor" runat="server"/>
                            <asp:HiddenField ID="ds_reference" runat="server"/>
                            <asp:HiddenField ID="ds_person_to_contact" runat="server"/>
                            <asp:HiddenField ID="ds_note" runat="server"/>
                            <asp:HiddenField ID="ds_vendor_head" runat="server"/>
                            <asp:HiddenField ID="ds_marketing" runat="server"/>

                            <asp:HiddenField ID="HdnVendorProvince" runat="server"/>
                            <asp:HiddenField ID="HdnVendorAmphurs" runat="server"/>
                            <asp:HiddenField ID="HdnVendorDistrict" runat="server"/>
                            <asp:HiddenField ID="HdnVendorPostCode" runat="server"/>

                            <table width="100%" border="0" cellpadding="5" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td align="left" width="" height="24px">
                                        <table >
                                            <tr>
                                                <td align="right">
                                                  <asp:Label ID="Label1" runat="server" Text="Search By" Width="107px"></asp:Label>
                                                </td>
                                                 <td>
                                                   <asp:DropDownList ID="ddlSearchVendorBy" runat="server" Width="150px">
                                            <asp:ListItem Value="VC" Selected="True">Vendor Code</asp:ListItem>
                                            <asp:ListItem Value="VH">Vendor (Head Office)</asp:ListItem>
                                            <asp:ListItem Value="VN">Vendor Name</asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                 <td align="right">
                                                 <asp:Label ID="Label109" runat="server" Text="Search Vendor" Width="140px"></asp:Label>
                                                </td>
                                                 <td>
                                                  <asp:TextBox ID="txtSearchVendorText" runat="server" Width="185px"></asp:TextBox>
                                                </td>
                                                 <td>
                                                     <%--<asp:Button ID="btnSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                            BackColor="#66CCFF" OnClick="btnSearchClick" />&nbsp;&nbsp;--%>
                                        <dx:ASPxButton ID="btnSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                            OnClick="btnSearchClick"></dx:ASPxButton>
                                        <%--<asp:Button ID="btnClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                            BackColor="#66CCFF" OnClick="btnClearClick" />--%>
                                        
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                            OnClick="btnClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left" width="170px" height="24px">
                                       
                                    </td>
                                    <td align="left" width="150px" height="24px">
                                        
                                    </td>
                                <td></td>
                                    <td align="left" width="200px" height="24px" colspan="2">
                                    
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="font-weight: 100; text-align: center;">
                                        <asp:Timer ID="Timer1" runat="server" OnTick="TimerTick" Interval="3000"></asp:Timer>
                                        <%--<asp:UpdateProgress ID="upProgGVVendor" runat="server">
                                            <ProgressTemplate>--%>
                                                <div id="imgdivLoading" align="center" valign="middle" runat="server" style=" width: 95%;
                                                    padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;">
                                                    <asp:Image ID="LoadImg" runat="server" ImageUrl="~/Images/loading2.gif" BackColor="Transparent" ImageAlign="AbsMiddle" />                                               
                                                </div>
                                            <%--</ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <asp:UpdatePanel ID="upGVVendor" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>--%>
                                                <div style="width: 100%; min-height:400px; height: auto;text-align: right;">
                                                    <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                        AllowPaging="True" EmptyDataText="No records found" ShowHeaderWhenEmpty="true"
                                                        OnPageIndexChanging="gvVendorPageIndexChanging"
                                                        OnSelectedIndexChanging="gvVendorSelectedIndexChanging"
                                                        OnRowDeleting="gvVendorRowDeleting">
                                                        <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                        <RowStyle BackColor="#F7F7DE" />
                                                        <Columns>
                                                            <asp:BoundField DataField="P10VEN" HeaderText="Vendor Code" ReadOnly="True" SortExpression="P10VEN">
                                                                <HeaderStyle Width="160px" HorizontalAlign="Center" />
                                                                <ItemStyle Width="160px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="G11VHO" HeaderText="Vendor (Head Office)" ReadOnly="True" SortExpression="G11VHO">
                                                                      <HeaderStyle Width="160px" HorizontalAlign="Center" />
                                                                <ItemStyle Width="160px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        <%--    <asp:BoundField DataField="P10TNM" HeaderText="Branch Code" ReadOnly="True" SortExpression="T1BRN" Visible="false">
                                                                <HeaderStyle Width="100px" />
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>--%>
                                                            <asp:BoundField DataField="P10TNM" HeaderText="Vendor Name" ReadOnly="True" SortExpression="P10TNM">
                                                        <%--        <HeaderStyle Width="250px" />
                                                                <ItemStyle Width="250px" />--%>
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField CommandName="Select" HeaderText="Edit" Text="Select">
                                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                            </asp:ButtonField>--%>
                                                            <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                            </asp:ButtonField>
                                                            <%--<asp:ButtonField CommandName="Delete" HeaderText="Delete" Text="Delete">
                                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                            </asp:ButtonField>--%>
                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:CommandField>
                                                        </Columns>
                                                    </asp:GridView>
                                                      <asp:Label ID ="lblTitle" runat="server"></asp:Label>
                                                </div>
                                            <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </table>
                            <br />

                            <table width="100%" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td colspan="6" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border:2px solid #D2E2F7; background-color: #D2E2F7;">
                                        <asp:Label ID="lblAddEdit" runat="server" Text="Add"></asp:Label>
                                    </td>
                                </tr>                                
                                <tr id="AddVendor" runat="server">
                                    <td align="left" width="180px" height="24px">
                                        <asp:Label ID="Label107" runat="server" Text="Vendor Type" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" width="300px" height="24px">
                                        <asp:RadioButtonList ID="rdoVendorType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoVendorType_SelectedIndexChanged">
                                            <asp:ListItem Value="NV" Text="New Vendor"></asp:ListItem>
                                            <asp:ListItem Value="NBV" Text="New branch of vendor"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="left" height="24px" colspan="4"></td>
                                </tr>
                                <tr>
                                    <td align="left" width="180px" height="24px">
                                        <asp:Label ID="Label2" runat="server" Text="Vendor Code " Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" width="300px" height="24px">
                                        <asp:TextBox ID="txtVendorCode" runat="server" Width="220px" Enabled="false"></asp:TextBox>                                        
                                    </td>
                                    <td align="left" height="24px" colspan="4"></td>
                                </tr>
                                <tr>
                                    <td align="left" width="180px" height="24px">
                                        <asp:Label ID="Label72" runat="server" Text="Vendor (Head)" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" width="300px" height="24px">
                                        <asp:TextBox ID="txtVendorHeadCode" runat="server" Width="220px" Enabled="false"></asp:TextBox>            
                                        &nbsp;
                                        <asp:ImageButton Height="18" id="btnAddVendorHead" runat="server" 
                                            AlternateText="ImageButton" ToolTip="Find Vendor Head" ImageAlign="AbsMiddle"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnAddVendorHeadClick"/>
                                    </td>
                                    <td align="left" width="100px" height="24px">
                                        <asp:Label ID="Label73" runat="server" Text="Title Code " Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" height="24px" colspan="3">
                                        <asp:TextBox ID="txtTitleCode" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                        &nbsp;
                                        <asp:TextBox ID="txtTitleDescription" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                        &nbsp;
                                        <asp:ImageButton Height="18" id="btnAddTitle" runat="server" 
                                            AlternateText="ImageButton" ToolTip="Find Title" ImageAlign="AbsMiddle"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnAddTitleClick"/>
                                        <asp:HiddenField id ="hiddenTitle" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="180px" height="24px">
                                        <asp:Label ID="Label4" runat="server" Text="Thai Name" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" width="300px" height="24px">
                                        <asp:TextBox ID="txtThaiName" runat="server" Width="220px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td align="left" width="100px" height="24px">
                                        <asp:Label ID="Label5" runat="server" Text="English Name" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" height="24px" colspan="3">
                                        <asp:TextBox ID="txtEnglishName" runat="server" Width="220px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="180px" height="24px">
                                        <asp:Label ID="Label110" runat="server" Text="Branch Name (English)" Width="160px"></asp:Label>
                                    </td>
                                    <td align="left" width="300px" height="24px">
                                        <asp:TextBox ID="txtBranchNameEnglish" runat="server" Width="220px" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td align="left" width="100px" height="24px">
                                        <asp:Label ID="Label70" runat="server" Text="Vendor Rank" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" width="80px" height="24px">
                                        <asp:TextBox ID="txtVendorRank" runat="server" Width="80px"  MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td align="left" width="100px" height="24px">
                                        <asp:Label ID="Label71" runat="server"  Text="Vendor Grade" Width="100px"></asp:Label>
                                    </td>
                                    <td align="left" height="24px">
                                        <asp:TextBox ID="txtVendorGrade" runat="server" Width="80px" MaxLength="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="180px" height="24px"></td>
                                    <td align="left" width="300px" height="24px">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btnAdd" runat="server" Text="Add" Width="70px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        OnClick="btnAddClick">
                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left">&nbsp;&nbsp;</td>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btnClearData" runat="server" Text="Clear" Width="70px" OnClick="btnClearDataClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="4"></td>
                                </tr>
                            </table>
                            <br />

                            <%-- Tab --%>
                            <div style="padding: 5px; border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <dx:ASPxPageControl Width="100%" Height="100%" ID="tabDetail" runat="server" ActiveTabIndex="1"
                                    EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                    CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                    ActivateTabPageAction="Click" LoadingPanelImagePosition="Top">
                                    <TabPages>
                                        <dx:TabPage Text="Address" Name="TabAddress">
                                            <ActiveTabStyle BackColor="#3399FF"></ActiveTabStyle>
                                            <TabStyle Font-Bold="True">
                                            </TabStyle>
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxPageControl Width="100%" Height="100%" ID="tabAddress" runat="server"
                                                        ActiveTabIndex="0" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        ActivateTabPageAction="Click" LoadingPanelImagePosition="Top">
                                                        <TabPages>
                                                            <dx:TabPage Text="Registration Address" Name="TabAddress_RegistrationAddress">
                                                                <ActiveTabStyle BackColor="#99FFFF">
                                                                </ActiveTabStyle>
                                                                <TabStyle Font-Bold="True">
                                                                </TabStyle>
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label6" runat="server" Text="Address"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_RegistrationAddress_Address" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Address" runat="server" Width="156px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label8" runat="server" Text="Moo" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Moo" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label9" runat="server" Text="Village" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Village" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label10" runat="server" Text="Building Type " Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_RegistrationAddress_BuildingType" runat="server" Width="160px" >
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label11" runat="server" Text="Building" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Building" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label12" runat="server" Text="Room" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Room" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label13" runat="server" Text="Floor " Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Floor" runat="server" Width="50px" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label14" runat="server" Text="Soi" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Soi" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label15" runat="server" Text="Road" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_Road" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label16" runat="server" Text="Province"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_RegistrationAddress_Province" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_RegistrationAddress_Province" AutoPostBack="true"
                                                                                        runat="server" Width="160px" OnSelectedIndexChanged="ddlTabAddress_RegistrationAddress_Province_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label17" runat="server" Text="Amphur" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_RegistrationAddress_Amphur" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_RegistrationAddress_Amphur" AutoPostBack="true"
                                                                                        runat="server" Width="155px" OnSelectedIndexChanged="ddlTabAddress_RegistrationAddress_Amphur_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label18" runat="server" Text="District"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_RegistrationAddress_District" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_RegistrationAddress_District" AutoPostBack="true"
                                                                                        runat="server" Width="160px" OnSelectedIndexChanged="ddlTabAddress_RegistrationAddress_District_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label20" runat="server" Text="Post Code" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_RegistrationAddress_PostCode" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_RegistrationAddress_PostCode" runat="server" Width="150px" MaxLength="5"></asp:TextBox>                                                                                                                                                          
                                                                                </td>
                                                                                <td colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px"></td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" height="24px" colspan="5">

                                                                                    <dx:ASPxButton ID="btnTabAddress_RegistrationAddress_LoadRegistrationAddressFromVendorHead" 
                                                                                        runat="server" Text="Load Registration Address from Vendor Head" 
                                                                                        CssClass="AlignButtonInline" Width="373px" Height="24px"
                                                                                        OnClick="btnTabAddress_RegistrationAddress_LoadRegistrationAddressFromVendorHead_Click"></dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage>
                                                            <dx:TabPage Text="Location Address" Name="TabAddress_LocationAddress">
                                                                <ActiveTabStyle BackColor="#99FFFF">
                                                                </ActiveTabStyle>
                                                                <TabStyle Font-Bold="True">
                                                                </TabStyle>
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label115" runat="server" Text="Address"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_LocationAddress_Address" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Address" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label116" runat="server" Text="Moo" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Moo" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label117" runat="server" Text="Village" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Village" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label118" runat="server" Text="Building Type " Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_LocationAddress_BuildingType" runat="server" Width="160px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label119" runat="server" Text="Building" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Building" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label120" runat="server" Text="Room" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Room" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label121" runat="server" Text="Floor " Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Floor" runat="server" Width="50px" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label122" runat="server" Text="Soi" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Soi" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label123" runat="server" Text="Road" Width="80px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_Road" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label124" runat="server" Text="Province"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_LocationAddress_Province" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_LocationAddress_Province" AutoPostBack="true" 
                                                                                        runat="server" Width="160px" OnSelectedIndexChanged="ddlTabAddress_LocationAddress_Province_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label125" runat="server" Text="Amphur" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_LocationAddress_Amphur" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_LocationAddress_Amphur" AutoPostBack="true" 
                                                                                        runat="server" Width="160px" OnSelectedIndexChanged="ddlTabAddress_LocationAddress_Amphur_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px">
                                                                                    <asp:Label ID="Label126" runat="server" Text="District"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_LocationAddress_District" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabAddress_LocationAddress_District" AutoPostBack="true"
                                                                                        runat="server" Width="160px" OnSelectedIndexChanged="ddlTabAddress_LocationAddress_District_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label127" runat="server" Text="Post Code" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabAddress_LocationAddress_PostCode" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_LocationAddress_PostCode" runat="server" Width="150px" MaxLength="5"></asp:TextBox>
                                                                                </td>
                                                                                <td colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="120px" height="24px"></td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" height="24px" colspan="5">
                                                                                    <%--<asp:Button runat="server" Text="Load New Data from Registration Address" BackColor="#66CCFF"
                                                                                        Height="24px" Width="300px" ID="btnTabAddress_LoacationAddress_LoadNewDataFromRegistrationAddress" OnClick="btnTabAddress_LoacationAddress_LoadNewDataFromRegistrationAddress_Click"></asp:Button>--%>
                                                                                    <dx:ASPxButton ID="btnTabAddress_LoacationAddress_LoadNewDataFromRegistrationAddress" runat="server" Text="Load New Data from Registration Address" CssClass="AlignButtonInline" Width="352px" Height="24px"
                                                                                        OnClick="btnTabAddress_LoacationAddress_LoadNewDataFromRegistrationAddress_Click"></dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage>
                                                            <dx:TabPage Text="Address For Tax Invoice" Name="TabAddress_AddressForTaxInvoice">
                                                                <ActiveTabStyle BackColor="#99FFFF">
                                                                </ActiveTabStyle>
                                                                <TabStyle Font-Bold="True">
                                                                </TabStyle>
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="ContentControl7" runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                                            <tr>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:Label ID="Label74" runat="server" Text="Address1" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_AddressForTaxInvoice_Address1" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:Label ID="Label75" runat="server" Text="Address2" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_AddressForTaxInvoice_Address2" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:Label ID="Label76" runat="server" Text="Address3" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px">
                                                                                    <asp:TextBox ID="txtTabAddress_AddressForTaxInvoice_Address3" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="24px"></td>
                                                                                <td align="left" height="24px">
                                                                                    <%--<asp:Button runat="server" Text="Load New Data From Registration Address" BackColor="#66CCFF"
                                                                                        Height="24px" Width="300px" ID="btnTabAddress_AddressForTaxInvoice_LoadNewDataFromRegistrationAddress" OnClick="btnTabAddress_AddressForTaxInvoice_LoadNewDataFromRegistrationAddress_Click"></asp:Button>--%>
                                                                                    <dx:ASPxButton ID="btnTabAddress_AddressForTaxInvoice_LoadNewDataFromRegistrationAddress" runat="server" Text="Load New Data from Registration Address" CssClass="AlignButtonInline" Width="328px" Height="24px"
                                                                                        OnClick="btnTabAddress_AddressForTaxInvoice_LoadNewDataFromRegistrationAddress_Click"></dx:ASPxButton>
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
                                        <dx:TabPage Text="Other" Name="TabOther">
                                            <TabStyle Font-Bold="True">
                                            </TabStyle>
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxPageControl Width="100%" Height="100%" ID="tabOther" runat="server"
                                                        ActiveTabIndex="0" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        ActivateTabPageAction="Click" LoadingPanelImagePosition="Top">
                                                        <TabPages>
                                                            <dx:TabPage Text="Information">
                                                                <ActiveTabStyle BackColor="#99FFFF">
                                                                </ActiveTabStyle>
                                                                <ContentCollection>
                                                                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #D2E2F7; background-color:#EFF4F9;">
                                                                            <tr>
                                                                                <td align="left" colspan="6" height="">
                                                                                    <%--<asp:Button ID="btnTabOther_Payment_LoadAllDataFromPaymentVendor" runat="server" Width="250px" Height="24px" BackColor="#66CCFF"
                                                                                        Text="Load All Data from Payment Vendor" OnClick="btnTabOther_Payment_LoadAllDataFromPaymentVendor_Click" />--%>
                                                                                    <dx:ASPxButton ID="btnTabOther_Information_LoadAllDataFromInformationVendor" runat="server" Text="Load data from Vendor Head" CssClass="AlignButtonInline" Width="302px" Height="24px"
                                                                                        OnClick="btnTabOther_Information_LoadAllDataFromInformationVendor_Click"></dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label37" runat="server" Text="MOU Date"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_MOUDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtTabOther_Information_MOUDate" runat="server" Width="150px"></asp:TextBox>
                                                                                                            <cc1:CalendarExtender ID="CalendarMouDate" runat="server"
                                                                                                                Format="dd/MM/eeee" PopupButtonID="setMouDate" TargetControlID="txtTabOther_Information_MOUDate">
                                                                                                            </cc1:CalendarExtender>
                                                                                                        </td>
                                                                                                 
                                                                                                        <td>
<%--                                                                                                            <dx:ASPxButton CssClass="AlignButtonInline" ID="setMouDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                            </dx:ASPxButton>--%>
                                                                                                             <asp:ImageButton ID="setMouDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>                                                                                                                                                                                                                                                     
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px"">
                                                                                    <asp:Label ID="Label38" runat="server" Text="Register No." Width="132px" ></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_RegisterNoTaxID" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label22" runat="server" Text="Tax ID"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_TaxID" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px"">
                                                                                    <asp:Label ID="Label42" runat="server" Text="Join Date"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_JoinDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtTabOther_Information_JoinDate" runat="server" Width="150px"></asp:TextBox>
                                                                                                            <cc1:CalendarExtender ID="CalendarJoinDate" runat="server"
                                                                                                                Format="dd/MM/eeee" PopupButtonID="setJoinDate" TargetControlID="txtTabOther_Information_JoinDate">
                                                                                                            </cc1:CalendarExtender>
                                                                                                        </td>

                                                                                                        <td>
                                                                                                      <%--      <dx:ASPxButton CssClass="AlignButtonInline" ID="setJoinDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                            </dx:ASPxButton>--%>
                                                                                                             <asp:ImageButton ID="setJoinDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label36" runat="server" Text="Expire Date"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_ExpireDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_ExpireDate" runat="server" Width="150px"></asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarExpireDate" runat="server"
                                                                                                    Format="dd/MM/eeee" PopupButtonID="setExpDate" TargetControlID="txtTabOther_Information_ExpireDate">
                                                                                                </cc1:CalendarExtender>
                                                                                            </td>
                                                                                            <td>
                                                                             <%--                   <dx:ASPxButton CssClass="AlignButtonInline" ID="setExpDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                    <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                    <Border BorderStyle="None" />
                                                                                                    <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                </dx:ASPxButton>--%>
                                                                                                 <asp:ImageButton ID="setExpDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>

                                                                                  
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label41" runat="server" Text="First Open Date" Width="180px"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_FirstOpenDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_FirstOpenDate" runat="server" Width="150px"></asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarFirstOpenDate" runat="server"
                                                                                                    Format="dd/MM/eeee" PopupButtonID="setFirstOpenDate" TargetControlID="txtTabOther_Information_FirstOpenDate">
                                                                                                </cc1:CalendarExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%--<dx:ASPxButton CssClass="AlignButtonInline" ID="setFirstOpenDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                    <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                    <Border BorderStyle="None" />
                                                                                                    <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                </dx:ASPxButton>--%>
                                                                                                 <asp:ImageButton ID="setFirstOpenDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />

                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #D2E2F7; background-color:#eff4f9;">
                                                                            <tr>
                                                                                <td colspan="6" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border:2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                    <asp:Label ID="Label23" runat="server" Text="Telephone-Fax"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label39" runat="server" Text="Telephone No.1"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_TelephoneNo1" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label40" runat="server" Text="Contact Tel. Range1/Ext1"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">                                                                                    
                                                                                    <asp:TextBox ID="txtTabOther_Information_ContactTelRange1" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                                                                    &nbsp;
                                                                                    <asp:TextBox ID="txtTabOther_Information_Extension1" runat="server" Width="90px" MaxLength="15"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label45" runat="server" Text="Fax No.1"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="220px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_FaxType1" runat="server" Width="120px">
                                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Value="1" Text="AUTO FAX"></asp:ListItem>
                                                                                        <asp:ListItem Value="2" Text="MANUAL FAX"></asp:ListItem>
                                                                                        <asp:ListItem Value="3" Text="MESSENGER FAX"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:TextBox ID="txtTabOther_Information_FaxNo1" runat="server" Width="80px" MaxLength="9"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label43" runat="server" Text="Telephone No.2"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_TelephoneNo2" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label44" runat="server" Text="Contact Tel. Range2/Ext2"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_ContactTelRange2" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                                                                    &nbsp;
                                                                                    <asp:TextBox ID="txtTabOther_Information_Extension2" runat="server" Width="90px" MaxLength="15"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="80px" height="24px">
                                                                                    <asp:Label ID="Label78" runat="server" Text="Fax No.2"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="220px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_FaxType2" runat="server" Width="120px">
                                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Value="1" Text="AUTO FAX"></asp:ListItem>
                                                                                        <asp:ListItem Value="2" Text="MANUAL FAX"></asp:ListItem>
                                                                                        <asp:ListItem Value="3" Text="MESSENGER FAX"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:TextBox ID="txtTabOther_Information_FaxNo2" runat="server" Width="80px" MaxLength="9"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" height="24px" style="background-color: #D2E2F7;" colspan="6">
                                                                                    <asp:Label ID="Label88" runat="server" Text="For Fax Outbound" Width="250px"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label79" runat="server" Text="Vendor Head"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_VendorHead" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                                                    <%--&nbsp;
                                                                                    <asp:ImageButton Height="18" id="btnTabOther_Information_AddVendor" runat="server" 
                                                                                        AlternateText="ImageButton" ToolTip="Find Vendor Head" ImageAlign="AbsMiddle"
                                                                                        ImageUrl="~\Images\icon\search.png" />--%>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label80" runat="server" Text="Status Fax for"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px" colspan="3">
                                                                                    <asp:Label ID="Label82" runat="server" Text="Oper" Width="30px"></asp:Label>
                                                                                    &nbsp;
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_StatusFaxForOper" runat="server" Width="40px">
                                                                                        <asp:ListItem Text="Y" Value="Y" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="Label83" runat="server" Text="ISB" Width="20px"></asp:Label>
                                                                                    &nbsp;
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_StatusFaxForISB" runat="server" Width="40px">
                                                                                        <asp:ListItem Text="Y" Value="Y" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="Label24" runat="server" Text="(Y : Send by Fax, M : Send by Messenger)" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td></td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label81" runat="server" Text="Fax For HO for" ></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px" colspan="3">
                                                                                    <asp:Label ID="Label84" runat="server" Text="Oper" Width="30px"></asp:Label>
                                                                                    &nbsp;
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_FaxForHOForOper" runat="server" Width="40px">
                                                                                        <asp:ListItem Text="Y" Value="Y" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="Label85" runat="server" Text="ISB" Width="20px"></asp:Label>
                                                                                    &nbsp;
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_FaxForHOForISB" runat="server" Width="40px">
                                                                                        <asp:ListItem Text="Y" Value="Y" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="Label25" runat="server" Text="(Y/N)" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td></td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label86" runat="server" Text="Auto Fax"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px" colspan="3">
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_AutoFax" runat="server" Width="60px">
                                                                                        <asp:ListItem Value="A" Text="A" Selected="False"></asp:ListItem>
                                                                                        <asp:ListItem Value="M" Text="M" Selected="True"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="Label26" runat="server" Text="(A : Auto Fax, M : Manual)" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td></td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label87" runat="server" Text="Auto Sign Lay-Bill"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Information_AutoSignLayBill" runat="server" Width="150px">
                                                                                        <asp:ListItem Value="N" Text="Manual Sign Lay - Bill" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Value="Y" Text="Auto Sign Lay - Bill"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td></td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />

                                                                        <%-- Tab Other Information Reference --%>
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9;">
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;" colspan="6">
                                                                                    <asp:Label ID="Label46" runat="server" Text="Reference"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label47" runat="server" Text="ID Card" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                     <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                               <asp:Label ID="Label35" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_Reference_IDCard" runat="server" Width="150px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                                                                                               
                                                                                </td>
                                                                                <td>
                                                                                    <asp:HiddenField ID="hdfReferenceRow" runat="server" />
                                                                                </td>
                                                                                <td></td>
                                                                                <td></td>
                                                                                <td></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label48" runat="server" Text="Title" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label68" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddlTabOther_Information_Reference_Title" runat="server" Width="157px" Height="20px">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label49" runat="server" Text="Name" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                     <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label69" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_Reference_Name" runat="server" Width="150px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>                                                                                                                                                           
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label27" runat="server" Text="Surname" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                     <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                 <asp:Label ID="Label91" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                 <asp:TextBox ID="txtTabOther_Information_Reference_Surname" runat="server" Width="150px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table> 
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label50" runat="server" Text="Position" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                   
                                                                                    <asp:TextBox ID="txtTabOther_Information_Reference_Position" runat="server" Width="150px" Style="margin-left:11px;"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label77" runat="server" Text="Department" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Information_Reference_Department" runat="server" Width="150px" Style="margin-left:11px;"></asp:TextBox>
                                                                                </td>
                                                                                <td></td>
                                                                                <td></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label89" runat="server" Text="Join Date" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_Reference_JoinDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label92" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_Reference_JoinDate" Width="150px" runat="server"></asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarReference_JoinDate" runat="server"
                                                                                                    Format="dd/MM/eeee" PopupButtonID="setReference_JoinDate" TargetControlID="txtTabOther_Information_Reference_JoinDate">
                                                                                                </cc1:CalendarExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                         <%--       <dx:ASPxButton CssClass="AlignButtonInline" ID="setReference_JoinDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                    <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                    <Border BorderStyle="None" />
                                                                                                    <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                </dx:ASPxButton>--%>
                                                                                                  <asp:ImageButton ID="setReference_JoinDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label90" runat="server" Text="Expire Date" Width="100px"></asp:Label>
                                                                                </td>
                                                                                <%--<td align="left" width="150px" height="24px">
                                                                                    <dx:ASPxTextBox ID="txtTabOther_Information_Reference_ExpireDate" runat="server" Width="100px">
                                                                                        <MaskSettings Mask="99/99/9999" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>--%>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label95" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Information_Reference_ExpireDate" runat="server" Width="150px"></asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarReference_ExpireDate" runat="server"
                                                                                                    Format="dd/MM/eeee" PopupButtonID="setReference_ExpireDate" TargetControlID="txtTabOther_Information_Reference_ExpireDate">
                                                                                                </cc1:CalendarExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                            <%--    <dx:ASPxButton CssClass="AlignButtonInline" ID="setReference_ExpireDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                    <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                                                    <Border BorderStyle="None" />
                                                                                                    <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                </dx:ASPxButton>--%>
                                                                                                <asp:ImageButton ID="setReference_ExpireDate" runat="server" ImageAlign="Bottom" ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="left" width="200px" height="24px" colspan="2">
                                                                                    <%--<asp:Button ID="btnTabOther_Information_Reference_Add" runat="server" Text="Add" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="btnTabOther_Information_Reference_Add_Click" />
                                                                                    &nbsp;&nbsp;--%>
                                                                                    <dx:ASPxButton ID="btnTabOther_Information_Reference_Add" runat="server" Text="Add" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                                                        OnClick="btnTabOther_Information_Reference_Add_Click"></dx:ASPxButton>
                                                                                    <%--<asp:Button ID="btnTabOther_Information_Reference_Clear" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="btnTabOther_Information_Reference_Clear_Click" />--%>
                                                                                    <dx:ASPxButton ID="btnTabOther_Information_Reference_Clear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                                                        OnClick="btnTabOther_Information_Reference_Clear_Click"></dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" colspan="6">
                                                                                    <asp:GridView ID="gvTabOther_Information_Reference" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="15"
                                                                                        BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True"
                                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records found"
                                                                                        OnPageIndexChanging="gvTabOther_Information_ReferencePageIndexChanging"
                                                                                        OnSelectedIndexChanging="gvTabOther_Information_ReferenceSelectedIndexChanging"
                                                                                        OnRowDeleting="gvTabOther_Information_ReferenceRowDeleting">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="Seq" HeaderText="Seq" ReadOnly="True" SortExpression="Seq" Visible="false">
                                                                                                <HeaderStyle Width="50px" />
                                                                                                <ItemStyle Width="50px" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="IDCard" HeaderText="ID Card" ReadOnly="True" SortExpression="IDCard"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Title" HeaderText="Title" ReadOnly="True" SortExpression="Title"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Surname" HeaderText="Surname" ReadOnly="True" SortExpression="Surname"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Position" HeaderText="Position" ReadOnly="True" SortExpression="Position"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Department" HeaderText="Department" ReadOnly="True" SortExpression="Department"></asp:BoundField>
                                                                                            <asp:BoundField DataField="JoinDate" HeaderText="Join Date" ReadOnly="True" SortExpression="JoinDate"></asp:BoundField>
                                                                                            <asp:BoundField DataField="ExpireDate" HeaderText="Expire Date" ReadOnly="True" SortExpression="ExpireDate"></asp:BoundField>
                                                                                            <%--<asp:ButtonField CommandName="Select" HeaderText="Edit" Text="Select">
                                                                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                            </asp:ButtonField>--%>
                                                                                            <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                            </asp:ButtonField>
                                                                                            <%--<asp:ButtonField CommandName="Delete" HeaderText="Delete" Text="Delete">
                                                                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                            </asp:ButtonField>--%>
                                                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                            </asp:CommandField>
                                                                                        </Columns>
                                                                                        <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                        <AlternatingRowStyle BackColor="White" />
                                                                                        <FooterStyle BackColor="#CCCC99" />
                                                                                        <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                                        <RowStyle BackColor="#F7F7DE" />
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>                                                                       
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage>
                                                            <dx:TabPage Text="Payment">
                                                                <ActiveTabStyle BackColor="#99FFFF">
                                                                </ActiveTabStyle>
                                                                <ContentCollection>
                                                                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">                                                                        
                                                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                                            <tr>
                                                                                <td align="left" colspan="6" height="">
                                                                                    <%--<asp:Button ID="btnTabOther_Payment_LoadAllDataFromPaymentVendor" runat="server" Width="250px" Height="24px" BackColor="#66CCFF"
                                                                                        Text="Load All Data from Payment Vendor" OnClick="btnTabOther_Payment_LoadAllDataFromPaymentVendor_Click" />--%>
                                                                                    <dx:ASPxButton ID="btnTabOther_Payment_LoadAllDataFromPaymentVendor" runat="server" Text="Load All Data from Payment Vendor" CssClass="AlignButtonInline" Width="302px" Height="24px"
                                                                                        OnClick="btnTabOther_Payment_LoadAllDataFromPaymentVendor_Click"></dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" height="24px" width="100px">
                                                                                    <asp:Label ID="Label7" runat="server" Text="Pay to Vendor"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_PayToVendor" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px" width="150px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Payment_PayToVendor" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton Height="18" ID="btnTabOther_Payment_AddPayToVendor" runat="server"
                                                                                                    AlternateText="ImageButton" ToolTip="Find Vendor Head" ImageAlign="AbsMiddle"
                                                                                                    ImageUrl="~\Images\icon\search.png" OnClick="btnTabOther_Payment_AddPayToVendor_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:HiddenField ID="hdfTempBranchCode" runat="server" />
                                                                                    <asp:HiddenField ID="hdfTempBranchPayment" runat="server" />
                                                                                </td>
                                                                                <td align="left" height="24px" width="95px">
                                                                                    <asp:Label ID="Label19" runat="server" Text="Branch create Payment(ESB Branch)" Width="350px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_BranchCode" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" height="24px" width="150px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_BranchCode" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                                    &nbsp;
                                                                                    <asp:TextBox ID="txtTabOther_Payment_BranchPayment" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label93" runat="server" Text="Payment Type"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_PaymentType" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_PaymentType" runat="server" Width="150px"
                                                                                        OnSelectedIndexChanged="ddlTabOther_Payment_PaymentType_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label94" runat="server" Text="Bank Acc No" Width="350px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_BankAccNo" runat="server" Width="150px" Enabled="false" MaxLength="15"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label3" runat="server" Text="Bank Code"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_BankCode" runat="server" Text="*" ForeColor="Red" Font-Bold="false" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_BankCode" runat="server" Width="150px" Enabled="false"></asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label96" runat="server" Text="Bank Region"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_BankRegion" runat="server" Text="*" ForeColor="Red" Font-Bold="false" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_BankRegion" runat="server" Width="150px">
                                                                                        <asp:ListItem Text="Select"  Value="" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Value="1" Text="Bangkok & ปริมลฑล"></asp:ListItem>
                                                                                        <asp:ListItem Value="2" Text="Upcountry"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label97" runat="server" Text="Vendor Delivery CTB CHQ" Width="350px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_DeliveryCTBCHQ" runat="server" Text="*" ForeColor="Red" Font-Bold="false" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_DeliveryCTBCHQ" AutoPostBack="false" runat="server" Width="150px" Enabled="false">
                                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Text="รับที่สยามเอซี" Value="RET"></asp:ListItem>
                                                                                        <asp:ListItem Text="รับ Check ที่ Citybank" Value="C/R"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label98" runat="server" Text="Special Loan"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_SpecialLoan" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_SpecialLoan" runat="server" Width="150px">
                                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Value="N" Text="Payment to Vendor"></asp:ListItem>
                                                                                        <asp:ListItem Value="Y" Text="Payment to Customer"></asp:ListItem>
                                                                                        <asp:ListItem Value="F" Text="Flood Loan"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label99" runat="server" Text="Sign before rec. doc"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_SignBeforeRecDoc" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:RadioButtonList ID="rdoTabOther_Payment_SignBeforeRecDoc" runat="server" RepeatDirection="Horizontal">
                                                                                        <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                                                                        <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label100" runat="server" Text="Calendary or Business Day" Width="350px" ></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_CarlendaryOrBusinessDay" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:DropDownList ID="ddlTabOther_Payment_CarlendaryOrBusinessDay" runat="server" Width="150px">
                                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Value="Y" Text="Calendary Day"></asp:ListItem>
                                                                                        <asp:ListItem Value="N" Text="Business Day"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label101" runat="server" Text="Credit Days"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_CreditDays" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_CreditDays" MaxLength="2" runat="server" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label102" runat="server" Text="Marketing"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_Marketing" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Payment_Marketing" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton Height="18" ID="btnTabOther_Payment_AddMarketing" runat="server"
                                                                                                    AlternateText="ImageButton" ToolTip="Find Vendor Head" ImageAlign="AbsMiddle"
                                                                                                    ImageUrl="~\Images\icon\search.png" OnClick="btnTabOther_Payment_AddMarketing_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>                                                                 
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label103" runat="server" Text="Time Available" Width="350px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_TimeAvailable" runat="server" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label104" runat="server" Text="Lead Time"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_LeadTime" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label105" runat="server" Text="CL Branch"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                    <asp:Label ID="lblReqTabOther_Payment_CLBranch" runat="server" Text="*" ForeColor="Red" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTabOther_Payment_CLBranch" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton Height="18" ID="btnTabOther_Payment_AddCLBranch" runat="server"
                                                                                                    AlternateText="ImageButton" ToolTip="Find Vendor Head" ImageAlign="AbsMiddle"
                                                                                                    ImageUrl="~\Images\icon\search.png" OnClick="btnTabOther_Payment_AddCLBranch_Click" />
                                                                                            </td>
                                                                                            <asp:HiddenField ID="ds_cl_branch" runat="server" />
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px">
                                                                                    <asp:Label ID="Label106" runat="server" Text="Payee Name" Width="350px"></asp:Label>
                                                                                </td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px">
                                                                                    <asp:TextBox ID="txtTabOther_Payment_PayeeName" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="100px" height="24px"></td>
                                                                                <td align="right" height="24px" width="2px">
                                                                                </td>
                                                                                <td align="left" width="150px" height="24px"></td>
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
                                        <dx:TabPage Text="Person to Contact" Name="TabPersonToContact">
                                            <TabStyle Font-Bold="True">
                                            </TabStyle>
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                                    <table width="100%" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                        <tr>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label51" runat="server" Text="Seq."></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_Seq" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:HiddenField ID="hdfPersonToContactRow" runat="server" />
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;"></td>
                                                            <td style="text-align: left; width: 100px; height: 24px;"></td>
                                                            <td style="text-align: left; width: 150px; height: 24px;"></td>
                                                            <td style="text-align: left; width: 100px; height: 24px;"></td>
                                                            <td style="text-align: left; width: 150px; height: 24px;"></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label52" runat="server" Text="Thai Name"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_ThaiName" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label53" runat="server" Text="English Name"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_EngName" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label54" runat="server" Text="Department"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_Department" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label55" runat="server" Text="Mobile Phone"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_MobilePhone" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />

                                                    <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #D2E2F7; background-color:#eff4f9;">
                                                        <tr>
                                                            <td colspan="8" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                <asp:Label ID="Label21" runat="server" Text="Telephone 1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label56" runat="server" Text="Telephone No.1"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_TelephoneNo1" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label57" runat="server" Text="Contact Tel. Range1"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_ContactTelRange1" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label58" runat="server" Text="Extension1"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_Extension1" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label59" runat="server" Text="Fax No.1"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_FaxNo1" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />

                                                    <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #D2E2F7; background-color:#eff4f9;">
                                                        <tr>
                                                            <td colspan="8" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                <asp:Label ID="Label28" runat="server" Text="Telephone 2"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label60" runat="server" Text="Telephone No.2"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_TelephoneNo2" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>            
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label61" runat="server" Text="Contact Tel. Range2"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_ContactTelRange2" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label62" runat="server" Text="Extension2"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_Extension2" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label63" runat="server" Text="Fax No.2"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_FaxNo2" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />

                                                    <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #D2E2F7; background-color:#eff4f9;">
                                                        <tr>
                                                            <td colspan="8" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                <asp:Label ID="Label29" runat="server" Text="Telephone 3"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label64" runat="server" Text="Telephone No.3" ></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_TelephoneNo3" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label65" runat="server" Text="Contact Tel. Range3"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_ContactTelRange3" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label66" runat="server" Text="Extension3"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_Extension3" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label67" runat="server" Text="Fax No.3"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; width: 150px; height: 24px;">
                                                                <asp:TextBox ID="txtTabPersonToContact_FaxNo3" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                        </tr>                                                        
                                                    </table>
                                                    <br />

                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <dx:ASPxButton ID="btnTabPersonToContact_Add" runat="server" Text="Add" CssClass="AlignButtonInline" Width="100px" Height="24px"
                                                                    OnClick="btnTabPersonToContact_Add_Click"></dx:ASPxButton>&nbsp;
                                                                <dx:ASPxButton ID="btnTabPersonToContact_Clear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="100px" Height="24px"
                                                                    OnClick="btnTabPersonToContact_Clear_Click"></dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />

                                                    <div style="width: 100%; overflow-y: auto;">
                                                        <asp:GridView ID="gvTabPersonToContact" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="15"
                                                            BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No records found"
                                                            OnPageIndexChanging="gvTabPersonToContactPageIndexChanging"
                                                            OnSelectedIndexChanging="gvTabPersonToContactSelectedIndexChanging"
                                                            OnRowDeleting="gvTabPersonToContactRowDeleting">
                                                            <Columns>
                                                                <%--<asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="ckbSelectAll" runat="server" />
                                                                </HeaderTemplate>
                                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                                <%--<asp:ButtonField CommandName="Select" HeaderText="Edit" Text="Select">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                </asp:ButtonField>--%>
                                                                <asp:BoundField DataField="T49COD" HeaderText="Code" ReadOnly="True" SortExpression="T49COD" Visible="false">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center"  />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center"  />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49RTY" HeaderText="Type" ReadOnly="True" SortExpression="T49RTY" Visible="false">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center"  />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center"  />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49PAR" HeaderText="Part" ReadOnly="True" SortExpression="T49PAR" Visible="false">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center"  />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center"  />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49SEQ" HeaderText="Seq" ReadOnly="True" SortExpression="T49SEQ" Visible="false">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center"  />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center"  />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TNM" HeaderText="Thai Name" ReadOnly="True" SortExpression="T49TNM">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49ENM" HeaderText="English Name" ReadOnly="True" SortExpression="T49ENM">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49DEP" HeaderText="Department" ReadOnly="True" SortExpression="T49DEP">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49HMB" HeaderText="Mobile Phone" ReadOnly="True" SortExpression="T49HMB">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TE1" HeaderText="Telephone No.1" ReadOnly="True" SortExpression="T49TE1">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TR1" HeaderText="Contact Tel. Range1" ReadOnly="True" SortExpression="T49TR1">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49EX1" HeaderText="Extension1" ReadOnly="True" SortExpression="T49EX1">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49FX1" HeaderText="Fax No.1" ReadOnly="True" SortExpression="T49FX1">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TE2" HeaderText="Telephone No.2" ReadOnly="True" SortExpression="T49TE2">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TR2" HeaderText="Contact Tel. Range2" ReadOnly="True" SortExpression="T49TR2">
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49EX2" HeaderText="Extension2" ReadOnly="True" SortExpression="T49EX2">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49FX2" HeaderText="Fax No.2" ReadOnly="True" SortExpression="T49FX2">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TE3" HeaderText="Telephone No.3" ReadOnly="True" SortExpression="T49TE3">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49TR3" HeaderText="Contact Tel. Range3" ReadOnly="True" SortExpression="T49TR3">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49EX3" HeaderText="Extension3" ReadOnly="True" SortExpression="T49EX3">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49FX3" HeaderText="Fax No.3" ReadOnly="True" SortExpression="T49FX3">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="T49RNO" HeaderText="Rno" ReadOnly="True" SortExpression="T49RNO" Visible="false">
                                                                    <ControlStyle Width="250px" />
                                                                </asp:BoundField>
                                                                <%--<asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                </asp:ButtonField>--%>
                                                                <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                </asp:ButtonField>
                                                                <%--<asp:ButtonField CommandName="Delete" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                </asp:ButtonField>--%>
                                                                <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:CommandField>
                                                            </Columns>
                                                            <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <FooterStyle BackColor="#CCCC99" />
                                                            <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                            <RowStyle BackColor="#F7F7DE" />
                                                        </asp:GridView>
                                                    </div>

                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="Note" Name="TabNote">
                                            <TabStyle Font-Bold="True">
                                            </TabStyle>
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                                    <table width="100%" cellpadding="5" cellspacing="5" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                                        <%--<tr>
                                                            <td style="text-align: right; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label68" runat="server" Text="Action Code"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; height: 24px;">
                                                                <dx:ASPxComboBox ID="dd_ActionCode" runat="server" DropDownRows="15" DropDownStyle="DropDown"
                                                                    IncrementalFilteringMode="Contains" BackColor="#FFFFC4" ValueType="System.String"
                                                                    Width="300px">
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 100px; height: 24px;">
                                                                <asp:Label ID="Label69" runat="server" Text="Result Code"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; height: 24px;">
                                                                <dx:ASPxComboBox ID="dd_ResCode" runat="server" DropDownRows="15" DropDownStyle="DropDown"
                                                                    IncrementalFilteringMode="Contains" BackColor="#FFFFC4" ValueType="System.String"
                                                                    Width="300px">
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td align="right" valign="top" width="100px" height="24px">
                                                                <asp:Label ID="Label30" runat="server" Text="Note"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <dx:ASPxMemo ID="txtTabNote_Memo" runat="server" Width="500px" Height="80px">
                                                                </dx:ASPxMemo>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right"></td>
                                                            <td align="left">
                                                                <%--<asp:Button runat="server" Text="Add" BackColor="#66CCFF" Height="24px" Width="100px" ID="btnTabNoteAdd" OnClientClick="function(s, e) { LoadingPanel.Show(); }" OnClick="btnTabNoteAdd_Click"></asp:Button>
                                                                &nbsp;&nbsp;--%>
                                                                <dx:ASPxButton ID="btnTabNoteAdd" runat="server" Text="Add" CssClass="AlignButtonInline" Width="100px" Height="24px"
                                                                    OnClick="btnTabNoteAdd_Click"></dx:ASPxButton>
                                                                <%--<asp:Button runat="server" Text="Clear" BackColor="#66CCFF" Height="24px" Width="100px" ID="btnTabNoteClear" OnClick="btnTabNoteClear_Click"></asp:Button>--%>
                                                                <dx:ASPxButton ID="btnTabNoteClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="100px" Height="24px"
                                                                    OnClick="btnTabNoteClear_Click"></dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <asp:GridView ID="gvNote" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="15"
                                                        BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records found"
                                                        OnPageIndexChanging="gvNotePageIndexChanging">
                                                        <Columns>
                                                            <asp:BoundField DataField="NOTE_DATE" HeaderText="Note Date" ReadOnly="True" SortExpression="NOTE_DATE">
                                                                <ItemStyle Width="100px" />
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NOTE_TIME" HeaderText="Note Time" ReadOnly="True" SortExpression="NOTE_TIME">
                                                                <ItemStyle Width="100px" />
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="P14UUS" HeaderText="Note By" ReadOnly="True" SortExpression="P14UUS">
                                                                <ItemStyle Width="120px" />
                                                                <HeaderStyle Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="P14NOT" HeaderText="Description" ReadOnly="True" SortExpression="P14NOT"></asp:BoundField>
                                                        </Columns>
                                                        <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        <RowStyle BackColor="#F7F7DE" />
                                                    </asp:GridView>

                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                    </TabPages>
                                </dx:ASPxPageControl>
                            </div>
                            <br />
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Height="150px" Width="250px">
                            </dx1:ASPxLoadingPanel>

                            <%-- Popup Add Vendor Head --%>
                            <dx:ASPxPopupControl ID="PopupAddVendorHead" ClientInstanceName="PopupAddVendorHead"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Vendor Head Office" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <asp:HiddenField ID="hdfVendorSection" runat="server" />
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label108" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="124px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPoupAddVendorHeadSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="VC" Selected="True">Vendor Code</asp:ListItem>
                                                        <asp:ListItem Value="VN">Vendor Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label111" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Searh Vendor Head" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddVendorHeadSearchText" runat="server" Width="185px"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 240px;
                                                    height: 24px;">
                                                    <%--<asp:Button ID="btnPopupAddVendorHeadSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddVendorHeadSearchClick" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddVendorHeadSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorHeadSearchClick"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddVendorHeadClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddVendorHeadClearClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddVendorHeadClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorHeadClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvPopupAddVendorHead" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvPopupAddVendorHeadPageIndexChanging"
                                                OnSelectedIndexChanging="gvPopupAddVendorHeadSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                    <asp:BoundField DataField="G11VHO" HeaderText="Vendor Code" ReadOnly="True" SortExpression="P10VEN1">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P10TNM" HeaderText="Vendor Name" ReadOnly="True" SortExpression="P10TNM">
                                                    </asp:BoundField>
                                                   <%-- <asp:BoundField DataField="T1BNME" HeaderText="Branch Name" ReadOnly="True" SortExpression="T1BNME">
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>--%>
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
                            
                            <%-- Popup Message --%>
                            <dx:ASPxPopupControl ID="PopupMessage" ClientInstanceName="PopupMessage" ShowPageScrollbarWhenModal="true"
                                HeaderText="" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnPopupMessageOK" runat="server" Text="OK" Width="100px" AutoPostBack="false">
                                                        <ClientSideEvents Click="function(s, e) { PopupMessage.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupMessage.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%--- Popup Message Success --%>
                            <dx:ASPxPopupControl ID="PopupMsgSuccess" ClientInstanceName="PopupMsgSuccess" ShowPageScrollbarWhenModal="true"
                                HeaderText="" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True" Modal="True"
                                Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsgSuccess" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="btnOKSuccess" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            
                            <%-- Popup Add Title Code --%>
                            <dx:ASPxPopupControl ID="PopupAddTitle" ClientInstanceName="PopupAddTitle"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Title" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label112" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddTitleSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="TC" Selected="True">Title Code</asp:ListItem>
                                                        <asp:ListItem Value="TD">Title Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label113" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Searh Title" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddTitleSearchText" runat="server" Width="185px"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddTitleSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddTitleSearchClick" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddTitleSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddTitleSearchClick"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddTitleClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddTitleClearClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddTitleClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddTitleClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvPopupAddTitle" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvPopupAddTitlePageIndexChanging"
                                                OnSelectedIndexChanging="gvPopupAddTitleSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="GNB2TC" HeaderText="Title Code" ReadOnly="True" SortExpression="GNB2TC">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="GNB2TD" HeaderText="Title Description" ReadOnly="True" SortExpression="GNB2TD">
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                <RowStyle BackColor="#F7F7DE" />
                                            </asp:GridView>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Poup Confirm Add Reference : Tab Other Information --%>
                            <dx:ASPxPopupControl ID="PopupConfirmAddReference" ClientInstanceName="PopupConfirmAddReference" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmAddReferenceMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmAddReferenceOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmAddReferenceOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddReference.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupConfirmAddReference.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                    <asp:HiddenField id="hiddenPopupConfirmAddReference" runat="server" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmAddReferenceCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddReference.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupConfirmAddReference.Hide();}"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Poup Confirm Delete Reference : Tab Other Information --%>
                            <dx:ASPxPopupControl ID="PopupConfirmDeleteReference" ClientInstanceName="PopupConfirmDeleteReference" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmDeleteReferenceMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteReferenceOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmDeleteReferenceOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteReference.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteReference.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteReferenceCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteReference.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteReference.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Add : Tab Person to Contact --%>
                            <dx:ASPxPopupControl ID="PopupTabPersonToContactConfirmAdd" ClientInstanceName="PopupTabPersonToContactConfirmAdd" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupTabPersonToContactConfirmAddMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <%--<asp:HiddenField ID="HiddenField2" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabPersonToContactConfirmAddOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupTabPersonToContactConfirmAddOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmAdd.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmAdd.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabPersonToContactConfirmAddCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmAdd.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmAdd.Hide();}"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Delete : Tab Person to Contact --%>
                            <dx:ASPxPopupControl ID="PopupTabPersonToContactConfirmDelete" ClientInstanceName="PopupTabPersonToContactConfirmDelete" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupTabPersonToContactConfirmDeleteMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <%--<asp:HiddenField ID="HiddenField3" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabPersonToContactConfirmDeleteOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupTabPersonToContactConfirmDeleteOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabPersonToContactConfirmDeleteCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupTabPersonToContactConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Add : Tab Note --%>
                            <dx:ASPxPopupControl ID="PopupTabNoteConfirmAdd" ClientInstanceName="PopupTabNoteConfirmAdd" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupTabNoteConfirmAdd" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                       <%-- <asp:HiddenField ID="HiddenField4" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabNoteConfirmAddOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupTabNoteConfirmAddOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabNoteConfirmAdd.Hide(); }" />
                                                   <%--     <ClientSideEvents Click="function(s, e) { PopupTabNoteConfirmAdd.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupTabNoteConfirmAddCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupTabNoteConfirmAdd.Hide(); }" />
                                                      <%--  <ClientSideEvents Click="function(s, e) { PopupTabNoteConfirmAdd.Hide();}"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Pay to Vendor --%>
                            <dx:ASPxPopupControl ID="PopupPayToVendor" ClientInstanceName="PopupPayToVendor" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Pay to Vendor" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="450px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
                                RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupPayToVendorMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                       <%-- <asp:HiddenField ID="HiddenField5" runat="server" />
                                        <br />
                                        <br />--%>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupPayToVendorYes" runat="server" Text="Yes" Width="100px" OnClick="btnPopupPayToVendorYes_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupPayToVendor.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupPayToVendor.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupPayToVendorNo" runat="server" Text="No" Width="100px" OnClick="btnPopupPayToVendorNo_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupPayToVendor.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupPayToVendor.Hide();}"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Marketing --%>
                            <dx:ASPxPopupControl ID="PopupAddMarketing" ClientInstanceName="PopupAddMarketing"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Marketing" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label31" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddMarketingSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="MC" Selected="True">Marketing Code</asp:ListItem>
                                                        <asp:ListItem Value="MN">Marketing Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label32" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Searh Marketing" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddMarketingSearchText" runat="server" Width="185px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddMarketingSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddMarketingSearch_Click" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddMarketingSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddMarketingSearch_Click"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddMarketingClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddMarketingClear_Click" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddMarketingClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddMarketingClear_Click"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvMarketing" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvMarketingPageIndexChanging"
                                                OnSelectedIndexChanging="gvMarketingSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                    <asp:BoundField DataField="T74CDE" HeaderText="Marketing Code" ReadOnly="True" SortExpression="T74CDE">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T74NME" HeaderText="Marketing Name" ReadOnly="True" SortExpression="T74NME">
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
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add CL Branch --%>
                            <dx:ASPxPopupControl ID="PopupAddCLBranch" ClientInstanceName="PopupAddCLBranch"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="CL Branch" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label33" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddCLBranchSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="BC" Selected="True">Branch Code</asp:ListItem>
                                                        <asp:ListItem Value="BN">Branch Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label34" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Searh Branch" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddCLBranchSearchText" runat="server" Width="185px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddCLBranchSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddCLBranchSearch_Click" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddCLBranchSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddCLBranchSearch_Click"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddCLBranchClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddCLBranchClear_Click" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddCLBranchClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddCLBranchClear_Click"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvBranch" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvBranchPageIndexChanging"
                                                OnSelectedIndexChanging="gvBranchSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                    <asp:BoundField DataField="T1BRN" HeaderText="Branch Code" ReadOnly="True" SortExpression="T1BRN">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T1BNMT" HeaderText="Branch Name" ReadOnly="True" SortExpression="T1BNMT">
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
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Add/Edit --%>
                            <dx:ASPxPopupControl ID="PopupConfirmAddEdit" ClientInstanceName="PopupConfirmAddEdit" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmAddEditMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmAddEditOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmAddEditOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddEdit.Hide(); }" />
                                                       <%-- <ClientSideEvents Click="function(s, e) { PopupConfirmAddEdit.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmAddEditCencel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddEdit.Hide(); }" />
                                                        <%--<ClientSideEvents Click="function(s, e) { PopupConfirmAddEdit.Hide();}"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Delete --%>
                            <dx:ASPxPopupControl ID="PopupConfirmDelete" ClientInstanceName="PopupConfirmDelete" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image10" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmDeleteMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:HiddenField ID="hdfVendorCode" runat="server" />
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmDeleteOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfiPopupConfirmDeletermAddEdit.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteCancel" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
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
