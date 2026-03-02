<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="VendorApplicationType.aspx.cs" Inherits="ManageData_WorkProcess_VendorApplicationType" %>
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
                    <ContentPaddings Padding="5px"></ContentPaddings>
                    <HeaderTemplate>
                        Vendor Application Type
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <table width="100%" cellspacing="5" border="0" style="border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                     <asp:HiddenField ID="ds_Hiddengrid" runat="server" />
                                     <asp:HiddenField ID="ds_popup" runat="server" />
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="lbl_SearchBy" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Search By" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                        <asp:DropDownList ID="ddlSearchBy" runat="server" Width="200px">
                                            <asp:ListItem Value="VI">Vendor ID</asp:ListItem>
                                            <asp:ListItem Value="VN">Vendor Name Eng</asp:ListItem>
                                            <asp:ListItem Value="VA">Vendor Appl. Type</asp:ListItem>
                                            <asp:ListItem Value="D">Description (Thai)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                        <asp:Label ID="lbl_SelectVendor" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Select Vendor Appl. Type" Width="180px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                        <asp:TextBox ID="txtVendorApplicationType" runat="server" Width="280px"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btnSearch" runat="server" Text="Search" Width="70px" OnClick="btnSearchClick">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" Width="70px" OnClick="btnClearClick">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px; height: 24px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <asp:Timer ID="Timer1" runat="server" OnTick="TimerTick" Interval="3000"></asp:Timer>
                                        <asp:UpdateProgress ID="upProgGV1" runat="server">
                                            <ProgressTemplate>
                                                <div id="imgdivLoading" align="center" valign="middle" runat="server" style=" width: 95%;
                                                    padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;">
                                                    <asp:Image ID="LoadImg" runat="server" ImageUrl="~/Images/loading2.gif" BackColor="Transparent" ImageAlign="AbsMiddle"/>                                               
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <asp:UpdatePanel ID="upGV1" runat="server">
                                            <ContentTemplate>
                                                <div style="width: 100%; height: 413px; text-align: right;">
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                    OnRowDeleting="GridView1_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataField="d10ven" HeaderText="Vendor ID" ReadOnly="True" SortExpression="d10ven">
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="p10nam" HeaderText="Vendor Name Eng" ReadOnly="True" SortExpression="p10nam">
                                                            <ItemStyle Width="500px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="d10apt" HeaderText="Vendor Appl. Type" ReadOnly="True"
                                                            SortExpression="d10apt">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gn61dt" HeaderText="Description (Thai)" SortExpression="gn61dt" />
                                                        <asp:BoundField DataField="D10EXP" HeaderText="Expire Date" SortExpression="D10EXP" />
                                                        <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:CommandField>
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
                                                <asp:Label ID ="lblTitle" runat="server"></asp:Label>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellspacing="5" border="0" style="border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px;
                                        height: 24px;" colspan="4">
                                        <asp:Label ID="lbl_AddEdit" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Add" Width="300px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label72" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Vendor" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 450px; height: 24px;">
                                        <asp:TextBox ID="txtVendorID" runat="server" Width="100px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtVendorName" runat="server" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                        <asp:ImageButton  Height="18" id="btnVendor" runat="server" AlternateText="ImageButton" ToolTip="Find Vendor"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnVendorClick"/>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px;">
                                        <asp:Label ID="Label73" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Loan Type " Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtLoanType" runat="server" Width="100px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtLoanTypeDesc" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label4" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Branch" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtBranch" runat="server" Width="100px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtBranchName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px;">
                                        <asp:Label ID="Label70" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Select Application Type" Width="160px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px;
                                        height: 24px;">
                                        <asp:DropDownList ID="ddlApplicationType" runat="server" Style="text-align: left; font-family: Tahoma;
                                            font-size: small; width: 340px; height: 24px;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Expire Date" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                        height: 24px;">
                                         <table>
                                            <tr>
                                                <%--<td>
                                                    <dx:ASPxTextBox ID="txtExpireDate" runat="server" Width="100px" AutoPostBack="true">
                                                        <MaskSettings Mask="99/99/9999" />
                                                    </dx:ASPxTextBox>
                                                </td>--%>
                                                <td width="95" valign="middle">
                                                   <%-- <asp:TextBox ID="txtExpireDate" runat="server" Width="100px" AutoPostBack="true"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarStartDate" runat="server"
                                                        Format="dd/MM/eeee" PopupButtonID="setExpDate" TargetControlID="txtExpireDate">
                                                    </cc1:CalendarExtender>--%>
                                                    <asp:TextBox ID="txtExpireDate" runat="server" Height="20" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                            Format="dd/MM/eeee" PopupButtonID="setExpDate" TargetControlID="txtExpireDate">
                                        </cc1:CalendarExtender>
                                                </td>
                                                <td width="30">
                                               <%--     <dx:ASPxButton ID="setExpDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                        <Border BorderStyle="None" />
                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                    </dx:ASPxButton>--%>
                                                   <%--   <asp:TextBox ID="setExpDate" runat="server" Height="20" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                            Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate">
                                        </cc1:CalendarExtender>--%>
                                                    <asp:ImageButton ID="setExpDate" runat="server" ImageAlign="Bottom"
                                            ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnAdd" runat="server" Text="Add" Width="70px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        OnClick="btnAddClick">
                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnClearData" runat="server" Text="Clear" Width="70px" OnClick="btnClearDataClick">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSqlAll" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtVendor_D" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                                Height="150px" Width="300px">
                            </dx1:ASPxLoadingPanel>
                            <dx:ASPxPopupControl ID="PopupMsg" ClientInstanceName="PopupMsg" ShowPageScrollbarWhenModal="true"
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
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsg" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                            <dx:ASPxPopupControl ID="PopupMsgSuccess" ClientInstanceName="PopupMsgSuccess" ShowPageScrollbarWhenModal="true"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/success.png" />
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
                                                    <dx:ASPxButton ID="btnOKSuccess" runat="server" Text="OK" Width="100px" OnClick="btnOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirmAdd" ClientInstanceName="PopupConfirmAdd" ShowPageScrollbarWhenModal="true"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/alert.png" />
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
                                                    <dx:ASPxButton ID="btnConfirmAddOK" runat="server" Text="OK" Width="100px" OnClick="btnConfirmAddOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmAddCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnConfirmAddCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirmDelete" ClientInstanceName="PopupConfirmDelete"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Confirm Delete" CloseAction="CloseButton"
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
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblConfimMsgDelete" runat="server" Font-Names="Tahoma" Font-Size="Small"
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
                                                    <dx:ASPxButton ID="btnConfirmDeleteOK" runat="server" Text="OK" Width="100px" OnClick="btnConfirmDeleteOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmDeleteCancel" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnConfirmDeleteCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAddVendor" ClientInstanceName="PopupAddVendor" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Vendor" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;height: 24px;">
                                                    <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddVendorSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="V" Selected="True">Vendor</asp:ListItem>
                                                        <asp:ListItem Value="D">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="lblPopupAddVendorSelectVendor" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Vendor" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label10" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtVendor" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupAddVendorSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAddVendorClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%; text-align: right;">
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView2_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView2_SelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="t00lty" HeaderText="Loan Type" SortExpression="t00lty"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="t00tnm" HeaderText="LOan Desc." SortExpression="t00tnm"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="p11ven" HeaderText="Vendor" ReadOnly="True" SortExpression="p11ven">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="p10nam" HeaderText="Description" SortExpression="p10nam" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
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
                                            <asp:Label ID ="lblTitle2" runat="server"></asp:Label>
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
