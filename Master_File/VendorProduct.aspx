<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="VendorProduct.aspx.cs" Inherits="ManageData_WorkProcess_VendorProduct" %>
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
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
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
                        Vendor Product
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <table width="100%" cellspacing="5" style="border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9;">
                                <tr>
                                    <asp:HiddenField ID="ds_product_type_select" runat="server" />
                                    <asp:HiddenField ID="ds_product_type" runat="server" />
                                    <asp:HiddenField ID="ds_popup" runat="server" />
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="Label72" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Vendor" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <asp:TextBox ID="txtVendorID" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                        &nbsp;
                                        <asp:TextBox ID="txtVendorName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        &nbsp;
                                        <asp:ImageButton Height="18" ID="btnVendor" runat="server" AlternateText="ImageButton" ToolTip="Find Vendor"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnSelectVendorClick" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="Label73" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Loan Type " Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <asp:TextBox ID="txtLoanType" runat="server" Width="100px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtLoanTypeDesc" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="left">
                                        <table cellpadding="10" width="380px">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton runat="server" ID="btnSave" Text="Save" CssClass="AlignButtonCenter" OnClick="btnSave_Click"></dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton runat="server" ID="btnClear" Text="Clear" CssClass="AlignButtonCenter" OnClick="btnClear_Click"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="5" style="border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <asp:Label ID="Label6" runat="server" Height="16px"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Product Type for Select" Width="631px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 20px; height: 24px;">&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px; height: 24px;">
                                        <asp:Label ID="Label8" runat="server" Height="16px"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Selected Product Type"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <dx:ASPxButton runat="server" ID="btnLoadHeader" Text="Load data from Vender Head" CssClass="AlignButtonInline" Width="200px" Height="24px" OnClick="btnLoadHeader_Click"></dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 20px; height: 24px;">&nbsp;</td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px; height: 24px;"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 45%; height: 400px;">
                                        <div style="width: 100%; height: 400px; border: 0.05em solid #808080; overflow: auto;">
                                            <asp:GridView ID="gvProductType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                ShowHeaderWhenEmpty="true" DataKeyNames="T40TYP">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="cbSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxSelectAllProductTypeChanged" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="T40TYP" HeaderText="Product Type Code" SortExpression="T40TYP">
                                                        <ItemStyle Width="125px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description"></asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                <RowStyle BackColor="#F7F7DE" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td align="center" style="font-family: Tahoma; font-size: small; width: 10px; height: 100px;">
                                        <table style="width: 172px">
                                            <tr style="width: 150px" align="center">
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" BackColor="#66CCFF" Height="24px"
                                                        Text=">>" Width="100px" OnClick="btnAddClick" ToolTip="Add" align="center" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="center">
                                                    <asp:Button ID="btnDelete" runat="server" BackColor="#66CCFF" Height="24px"
                                                        Text="<<" Width="100px" OnClick="btnDeleteClick" ToolTip="Delete" align="center" />
                                                </td>
                                            </tr>
                                        </table>                                       
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 45%; height: 400px;">
                                        <div style="width: 100%; height: 400px; border: 0.05em solid #808080; overflow: auto;">
                                            <asp:GridView ID="gvProductTypeSelected" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                ShowHeaderWhenEmpty="true" DataKeyNames="P13TYP">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="cbSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxSelectAllProductTypeSelectChanged" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="P13TYP" HeaderText="Product Type Code" SortExpression="P13TYP">
                                                        <ItemStyle Width="125px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description"></asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                <RowStyle BackColor="#F7F7DE" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Height="150px" Width="300px">
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/success.png" />
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

                            <dx:ASPxPopupControl ID="PopupConfirmSave" ClientInstanceName="PopupConfirmSave"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Confirm" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="450px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmSaveMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmSaveOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmSaveOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmSaveCencal" runat="server" Text="Cancel" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();}"></ClientSideEvents>
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
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="V" Selected="True">Vendor</asp:ListItem>
                                                        <asp:ListItem Value="D">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="lbl_pSelectVendor" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select Vendor" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label10" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txt_Vendor" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddVendorSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddVendorSearchClick" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddVendorSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorSearchClick">
                                                    </dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddVendorClear" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="btnPopupAddVendorClearClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddVendorClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorClearClick">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:BoundField DataField="t00lty" HeaderText="Loan Type" SortExpression="t00lty"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="t00tnm" HeaderText="LOan Desc." SortExpression="t00tnm"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="p11ven" HeaderText="Vendor" ReadOnly="True" SortExpression="p11ven">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="p10nam" HeaderText="Description" SortExpression="p10nam" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
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
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>
