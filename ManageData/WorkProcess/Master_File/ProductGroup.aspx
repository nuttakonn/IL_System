<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="ProductGroup.aspx.cs" Inherits="ManageData_WorkProcess_ProductGroup" %>
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
    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Width="100%" Height="136px">
                    <ContentPaddings Padding="5px" />
                    <HeaderTemplate>
                        Product Group
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <table width="100%" cellpadding="5" style="border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <asp:Label ID="lblSearchBy" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Search By"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                        <asp:DropDownList ID="ddlSearchBy" runat="server" height="25px"  Width="200px">
                                            <asp:ListItem Value="LTC" Selected="True">Loan Type Code</asp:ListItem>
                                            <asp:ListItem Value="LTD">Loan Type Description</asp:ListItem>
                                            <asp:ListItem Value="PTC">Product Type Code</asp:ListItem>
                                            <asp:ListItem Value="PTD">Product Type Description</asp:ListItem>
                                            <asp:ListItem Value="PDC">Product Code</asp:ListItem>
                                            <asp:ListItem Value="PDD">Product Description</asp:ListItem>
                                            <asp:ListItem Value="PBC">Brand Code</asp:ListItem>
                                            <asp:ListItem Value="PBD">Brand Description</asp:ListItem>
                                            <asp:ListItem Value="PMC">Model Code</asp:ListItem>
                                            <asp:ListItem Value="PMD">Model Description</asp:ListItem>
                                            <asp:ListItem Value="PIC">Group Code</asp:ListItem>
                                            <asp:ListItem Value="PID">Group Description</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <asp:Label ID="Label16" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Select Product Group" Width="150px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label17" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 280px; height: 24px;">
                                        <asp:TextBox ID="txtSearchText" runat="server" Height="20px" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btnSearch" runat="server" Text="Search" Width="70px" OnClick="btnSearchClick">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" Width="70px" OnClick="btnClearClick">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btnAdvanceSearch" runat="server" Text="Advance Search" Width="130px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" OnClick="btnAdvanceSearchClick">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px;
                                        height: 24px;">
                                        <asp:HiddenField ID="ds_product_group" runat="server" />
                                        <asp:HiddenField ID="ds_product_group_item" runat="server" />
                                        <asp:HiddenField ID="ds_product_item" runat="server" />
                                        <asp:HiddenField ID="ds_loan_type" runat="server" />
                                        <asp:HiddenField ID="ds_product_type" runat="server" />
                                        <asp:HiddenField ID="ds_product_brand" runat="server" />
                                        <asp:HiddenField ID="ds_product_code" runat="server" />
                                        <asp:HiddenField ID="ds_product_model" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10" >
                                        <asp:GridView ID="gvProductGroup" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="15"
                                            BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No records found"
                                            OnPageIndexChanging="gvProductGroupPageIndexChanging"
                                            OnSelectedIndexChanging="gvProductGroupSelectedIndexChanging"
                                            OnRowDeleting="gvProductGroupRowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="T44LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="T44LTY">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Description" ReadOnly="True" SortExpression="T00TNM"></asp:BoundField>
                                                <asp:BoundField DataField="T44TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="T44TYP">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" ReadOnly="True" SortExpression="T40DES"></asp:BoundField>
                                                <asp:BoundField DataField="T44BRD" HeaderText="Brand Code" ReadOnly="True" SortExpression="T44BRD">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T42DES" HeaderText="Brand Description" ReadOnly="True" SortExpression="T42DES"></asp:BoundField>
                                                <asp:BoundField DataField="T44COD" HeaderText="Product Code" ReadOnly="True" SortExpression="T44COD">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T41DES" HeaderText="Product Description" ReadOnly="True" SortExpression="T41DES"></asp:BoundField>
                                                <asp:BoundField DataField="T44MDL" HeaderText="Model Code" ReadOnly="True" SortExpression="T44MDL">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T43DES" HeaderText="Model Description" ReadOnly="True" SortExpression="T43DES"></asp:BoundField>
                                                <asp:BoundField DataField="T44ITM" HeaderText="Group Code" ReadOnly="True" SortExpression="T44ITM">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T44DES" HeaderText="Group Description" ReadOnly="True" SortExpression="T44DES"></asp:BoundField>
                                                <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
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
                                        <asp:HiddenField ID="hdfLoanTypeCode" runat="server" />
                                        <asp:HiddenField ID="hdfProductGroupCode" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td colspan="2" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border:2px solid #D2E2F7; background-color: #D2E2F7;">
                                        <asp:Label ID="lblAddEdit" runat="server" Text="Add"></asp:Label>&nbsp;Product Group
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="150px" height="24px">
                                        <asp:Label ID="Label2" runat="server" Text="Loan Type" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtLoanTypeCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtLoanTypeDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px">
                                        <asp:Label ID="Label4" runat="server" Text="Product Type" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtProductTypeCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductTypeDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px">
                                        <asp:Label ID="Label6" runat="server" Text="Product Brand" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtProductBrandCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductBrandDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px">
                                        <asp:Label ID="Label8" runat="server" Text="Product Code" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtProductCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductCodeDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px">
                                        <asp:Label ID="Label10" runat="server" Text="Product Model" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtProductModelCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductModelDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                        <asp:ImageButton Height="20" id="btnAddProductModel" runat="server" AlternateText="ImageButton" ToolTip="Find Product Model"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnAddProductModelClick"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px">
                                        <asp:Label ID="Label12" runat="server" Text="Product Group" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td height="24px">
                                        <asp:TextBox ID="txtProductGroupCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="24px" valign="top"></td>
                                    <td height="24px" align="left">
                                        <asp:TextBox ID="txtProductItemAll" runat="server" Height="20px" Width="390px" Enabled="false"></asp:TextBox>
                                        <br />
                                         <div style="margin-top:10px;">
                                        <asp:TextBox ID="txtProductItemDescription" runat="server" Rows="2" TextMode="MultiLine" Width="390px"></asp:TextBox>
                                             </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btnAdd" runat="server" Text="Add" Width="70px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        OnClick="btnAddClick">
                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left">&nbsp;&nbsp;</td>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btnClearData" runat="server" Text="Clear" Width="70px" OnClick="btnClearDataClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table ID="tbAddItemToGroupProduct" cellspacing="0" cellpadding="5" runat="server" width="100%" style="border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">                              
                                <tr>
                                    <td width="100%" align="left" height="24px" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 8px 8px 0px 0px; border:2px solid #D2E2F7; background-color: #D2E2F7;">
                                        <asp:Label ID="Label14" runat="server" Text="Product Group Item" Height="16px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                        <dx:ASPxButton ID="btnAddItemToGroupProduct" runat="server" Text="Add Item to Group Product" Height="24px" Width="200px" 
                                            OnClick="btnAddItemToGroupProductClick"></dx:ASPxButton>
                                        <asp:HiddenField ID="hdfProductItemCode" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvProductGroupItem" runat="server" AutoGenerateColumns="False" BackColor="White" PageSize="15"
                                            BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                            AllowPaging="True" ShowHeaderWhenEmpty="true"
                                            OnPageIndexChanging="gvProductGroupItemPageIndexChanging"
                                            OnRowDeleting="gvProductGroupItemRowDeleting">
                                            <Columns>
                                                <asp:ButtonField CommandName="Delete" HeaderText="Delete" Text="Delete">
                                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="T45ITG" HeaderText="Group" ReadOnly="True" SortExpression="T45ITG">
                                                    <HeaderStyle Width="120px" />
                                                    <ItemStyle Width="120px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T45ITM" HeaderText="Product Group Item Code" ReadOnly="True" SortExpression="T45ITM">
                                                    <HeaderStyle Width="180px" />
                                                    <ItemStyle Width="180px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T44DES" HeaderText="Product Group Item Description" ReadOnly="True" SortExpression="T44DES"></asp:BoundField>
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
                            <br />
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Height="150px" Width="250px">
                            </dx1:ASPxLoadingPanel>

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
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/alertRed.png" />

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
                                                    <dx:ASPxButton ID="btnPopupMessageOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupMessageOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupMessage.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupMessage.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            
                            <%-- Popup Message Success --%>
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMessageSuccess" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="btnOKSuccess" runat="server" Text="OK" Width="100px" OnClick="btnPopupMessageOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Advance Search --%>
                            <dx:ASPxPopupControl ID="PopupAdvanceSearch" ClientInstanceName="PopupAdvanceSearch"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Model" CloseAction="CloseButton" AllowDragging="True"
                                AutoUpdatePosition="True" Modal="True" Width="700px" Height="250px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" cellpadding="5">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;">
                                                    <asp:Label ID="Label13" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Loan Type" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px;">
                                                    <asp:Label ID="Label15" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchLoanTypeCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchLoanTypeDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="20" id="btnPopupAdvanceSearchLoanType" runat="server" AlternateText="ImageButton" ToolTip="Find LoanType"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnPopupAdvanceSearchLoanTypeClick"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;">
                                                    <asp:Label ID="lblPopupAdvanceSearchProductType" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Product Type" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px;">
                                                    <asp:Label ID="Label35" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductTypeCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductTypeDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="20" id="btnPopupAdvanceSearchProductType" runat="server" AlternateText="ImageButton" ToolTip="Find ProductType"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnPopupAdvanceSearchProductTypeClick"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;">
                                                    <asp:Label ID="Label38" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Product Brand" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px;">
                                                    <asp:Label ID="Label39" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductBrandCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductBrandDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="20" id="btnPopupAdvanceSearchProductBrand" runat="server" AlternateText="ImageButton" ToolTip="Find ProductBrand"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnPopupAdvanceSearchProductBrandClick"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;">
                                                    <asp:Label ID="Label36" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Product Code" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px;">
                                                    <asp:Label ID="Label37" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="20" id="btnPopupAdvanceSearchProductCode" runat="server" AlternateText="ImageButton" ToolTip="Find Product Code"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnPopupAdvanceSearchProductCodeClick"/>
                                                </td>
                                            </tr>

                                            <tr id="SearchProductModel" runat="server" visible="false">
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;">
                                                    <asp:Label ID="Label40" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Product Model" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px;">
                                                    <asp:Label ID="Label41" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductModelCode" runat="server" Height="20px" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductModelDescription" runat="server" Height="20px" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="20" id="btnPopupAdvanceSearchProductModel" runat="server" AlternateText="ImageButton" ToolTip="Find ProductModel"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnPopupAdvanceSearchProductModelClick"/>
                                                </td>
                                            </tr>

                                            <tr id="tr_ProductItem" runat="server" visible="false">
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; vertical-align: top;">
                                                    <asp:Label ID="Label42" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Product Item" Width="120px" Height="16px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; vertical-align: top;">
                                                    <asp:Label ID="Label43" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;">
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductItemCode" runat="server" Height="20px" Width="70px"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtPopupAdvanceSearchProductItemDesc" runat="server" Height="20px" Width="300px"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="2"></td>
                                                <td colspan="2" style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;">
                                                    <%--<asp:Button ID="btnPopupAdvanceSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAdvanceSearchClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAdvanceSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAdvanceSearchClick">
                                                    </dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAdvanceSearchClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAdvanceSearchClearClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAdvanceSearchClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAdvanceSearchClearClick">
                                                    </dx:ASPxButton>
                                                    <asp:HiddenField ID="hdfFormSearch" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Loan Type --%>
                            <dx:ASPxPopupControl ID="PopupAddLoanType" ClientInstanceName="PopupAddLoanType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Loan Type" CloseAction="CloseButton"
                                AllowDragging="true" AutoUpdatePosition="true" Modal="true" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { PopupAdvanceSearch.Show();  }" />  
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddLoanTypeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="LC" Selected="True">Loan Type Code</asp:ListItem>
                                                        <asp:ListItem Value="LN">Loan Type Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Select Product Type" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddLoanTypeSearchText" runat="server" Width="185px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddLoanTypeSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddLoanTypeSearch_Click" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddLoanTypeSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddLoanTypeSearch_Click"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddLoanTypeClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddLoanTypeClear_Click" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddLoanTypeClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddLoanTypeClear_Click"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvLoanType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvLoanTypePageIndexChanging"
                                                OnSelectedIndexChanging="gvLoanTypeSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:BoundField DataField="T00LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="T00LTY">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Name" ReadOnly="True" SortExpression="T00TNM">
                                                    </asp:BoundField>
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
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Product Type --%>
                            <dx:ASPxPopupControl ID="PopupAddProductType" ClientInstanceName="PopupsAddProductType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Type" CloseAction="CloseButton"
                                AllowDragging="true" AutoUpdatePosition="true" Modal="true" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { PopupAdvanceSearch.Show();  }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label18" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductTypeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PTC" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PTD">Product Type Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px; height: 24px;">
                                                    <asp:Label ID="Label20" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Select Product Type" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label19" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductTypeSearchText" runat="server" Width="180px"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupAddProductTypeSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductTypeSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAddProductTypeClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductTypeClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvProductType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvProductTypePageIndexChanging"
                                                OnSelectedIndexChanging="gvProductTypeSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T40LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="T40LTY" Visible="false">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Name" ReadOnly="True" Visible="false" SortExpression="T00TNM">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="T40TYP">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" SortExpression="T40DES" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            
                            <%-- Popup Add Product Brand --%>
                            <dx:ASPxPopupControl ID="PopupAddProductBrand" ClientInstanceName="PopupAddProductBrand" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Product Brand" CloseAction="CloseButton" AllowDragging="true" AutoUpdatePosition="true"
                                Modal="true" Width="850px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { PopupAdvanceSearch.Show();  }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label21" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label22" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 160px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductBrandSearchBy" runat="server" Width="140px">
                                                        <asp:ListItem Value="PBC" Selected="True">Product Brand Code</asp:ListItem>
                                                        <asp:ListItem Value="PBN">Product Brand Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px;">
                                                    <asp:Label ID="Label23" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Brand" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label24" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductBrandSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupAddProductBrandSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductBrandSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAddProductBrandClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductBrandClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%; text-align: right;">
                                            <asp:GridView ID="gvProductBrand" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gvProductBrandPageIndexChanging"
                                                OnSelectedIndexChanging="gvProductBrandSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T42BRD" HeaderText="Product Brand Code" ReadOnly="True" SortExpression="T42BRD">
                                                        <ItemStyle Width="140px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T42DES" HeaderText="Product Brand Name" SortExpression="T42DES" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                            <asp:Label ID ="lblgvProductBrand" runat="server"></asp:Label>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            
                            <%-- Popup Add Product Code --%>
                            <dx:ASPxPopupControl ID="PopupAddProductCode" ClientInstanceName="PopupAddProductCode"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Code" CloseAction="CloseButton"
                                AllowDragging="true" AutoUpdatePosition="true" Modal="true" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { PopupAdvanceSearch.Show();  }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label25" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label26" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductCodeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                                        <asp:ListItem Value="PD">Product Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label27" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Product Type" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label28" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductCodeSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupAddProductCodeSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductCodeSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAddProductCodeClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductCodeClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvProductCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvProductCodePageIndexChanging"
                                                OnSelectedIndexChanging="gvProductCodeSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T41LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="T41LTY" Visible="false">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Description" ReadOnly="True" SortExpression="T00TNM" Visible="false">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="T41TYP">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" ReadOnly="True" SortExpression="T40DES" Visible="false">
                                                        <ItemStyle Width="300px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41COD" HeaderText="Product Code" ReadOnly="True" SortExpression="T41COD">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41DES" HeaderText="Product Description" SortExpression="T41DES" />
                                                    <asp:BoundField DataField="T00TNM " HeaderText="Loan Type Description" SortExpression="T00TNM" Visible="false" />
                                                    <asp:BoundField DataField="T40LTY" HeaderText="Product Type Description" SortExpression="T40LTY" Visible="false" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            
                            <%-- Popup Add Product Model --%>
                            <%-- Show When Clik Search on Popup Advance Search --%>
                            <dx:ASPxPopupControl ID="PopupAddProductModel" ClientInstanceName="PopupAddProductModel"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Model" CloseAction="CloseButton"
                                AllowDragging="true" AutoUpdatePosition="true" Modal="true" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { PopupAdvanceSearch.Show();  }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label29" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label30" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductModelSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                                        <asp:ListItem Value="BC">Brand Code</asp:ListItem>
                                                        <asp:ListItem Value="MC">Model Code</asp:ListItem>
                                                        <asp:ListItem Value="MD">Model Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label31" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Product Model" Width="150px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label32" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductModelSearchText" runat="server" Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupAddProductModelSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductModelSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAddProductModelClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductModelClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvProductModel" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" 
                                                OnPageIndexChanging="gvProductModelPageIndexChanging"
                                                OnSelectedIndexChanging="gvProductModelSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T43LTY" HeaderText="Loan Type" ReadOnly="True" SortExpression="T43LTY" Visible="false">
                                                        <ItemStyle Width="80px" HorizontalAlign="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Name" ReadOnly="True" Visible="false" SortExpression="T00TNM">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T43TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="T43TYP">
                                                        <ItemStyle Width="120px" HorizontalAlign="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" ReadOnly="True" Visible="false" SortExpression="T40DES">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T43COD" HeaderText="Product Code" ReadOnly="True" SortExpression="T43COD">
                                                        <ItemStyle Width="100px" HorizontalAlign="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41DES" HeaderText="Product Description" ReadOnly="True" SortExpression="T41DES" Visible="false">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T43BRD" HeaderText="Brand Code" ReadOnly="True" SortExpression="T43BRD">
                                                        <ItemStyle Width="80px" HorizontalAlign="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T42DES" HeaderText="Brand Name" ReadOnly="True" SortExpression="T42DES" Visible="false">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T43MDL" HeaderText="Model Code" ReadOnly="True" SortExpression="T43MDL">
                                                        <ItemStyle Width="80px" HorizontalAlign="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T43DES" HeaderText="Model Description" SortExpression="T43DES" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                        <br />
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Product Item --%>
                            <dx:ASPxPopupControl ID="PopupAddProductItem" ClientInstanceName="PopupAddProductItem" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Product Item" CloseAction="CloseButton" AllowDragging="true" AutoUpdatePosition="true"
                                Modal="true" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                                    </Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label33" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label34" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductItemSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PIC" Selected="True">Product Item Code</asp:ListItem>
                                                        <asp:ListItem Value="PID">Product Item Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label44" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Product Item" Width="150px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label45" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductItemSearchText" runat="server" Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btnPopupAddProductItemSearch" runat="server" Text="Search" Width="80px"
                                                        Height="24px" BackColor="#66CCFF" OnClick="PopupAddProductItemSearchClick" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btnPopupAddProductItemSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="PopupAddProductItemSearchClick"></dx:ASPxButton>
                                                    <%--<asp:Button ID="btnPopupAddProductItemClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductItemClearClick" />--%>
                                                    <dx:ASPxButton ID="btnPopupAddProductItemClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddProductItemClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvAddProductItem" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" 
                                                OnPageIndexChanging="gvAddProductItemPageIndexChanging"
                                                OnSelectedIndexChanging="gvAddProductItemSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:BoundField DataField="T44ITM" HeaderText="Product Item Code" ReadOnly="True" SortExpression="T44ITM">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T44DES" HeaderText="Product Item Description" ReadOnly="True" SortExpression="T44DES">
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                        <br />
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Add --%>
                            <dx:ASPxPopupControl ID="PopupConfirmAdd" ClientInstanceName="PopupConfirmAdd" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Confirm" CloseAction="CloseButton" AllowDragging="true" AutoUpdatePosition="true"
                                Modal="true" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblConfirmAddMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmAddOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmAddOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmAddCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnPopupConfirmAddCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAdd.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupMsgProcedures" ClientInstanceName="PopupMsgProcedures" ShowPageScrollbarWhenModal="true"
                                HeaderText="Error" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="msgProcedures" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                       
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnPopupMessageOK2" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgProcedures.Hide(); }" />
                                                     
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
                                Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/deletenew.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblConfirmDeleteMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmDeleteOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnPopupConfirmDeleteCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Confirm Add Item to Group --%>
                            <dx:ASPxPopupControl ID="PopupConfirmAddItemToGroup" ClientInstanceName="PopupConfirmAddItemToGroup" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Confirm" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmAddItemToGroupMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmAddItemToGroupOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmAddItemToGroupOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddItemToGroup.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddItemToGroup.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmAddItemToGroupCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnPopupConfirmAddItemToGroupCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddItemToGroup.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmAddItemToGroup.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            
                            <%-- Popup Confirm Delete Item from Group --%>
                            <dx:ASPxPopupControl ID="PopupConfirmDeleteItemFromGroup" ClientInstanceName="PopupConfirmDeleteItemFromGroup" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Confirm" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/icon/deletenew.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblPopupConfirmDeleteItemFromGroupMessage" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
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
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteItemFromGroupOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupConfirmDeleteItemFromGroupOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteItemFromGroup.Hide(); }" />
                                                      
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnPopupConfirmDeleteItemFromGroupCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnPopupConfirmDeleteItemFromGroupCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteItemFromGroup.Hide(); }" />
                                                       
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
